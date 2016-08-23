using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Styles.Interfaces;
using IM.Styles.Classes;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmProductDetail.xaml
  /// </summary>
  public partial class frmProductDetail : Window, IRichTextBoxToolBar
  {
    #region Variables
    public Product product = new Product();//Objeto a guardar
    public Product oldProduct = new Product();//Objeto con los datos iniciales
    private List<Gift> _oldlstGifts = new List<Gift>();//Lista inicial de regalos
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    private bool isCellCancel = false;//Para la edicion del grid
    private ProductLegend _productLegend = new ProductLegend();//Product legend visualizando
    private bool blnClosing = false;//Para saber si se va a verificar los cambios pendientes
    #endregion
    public frmProductDetail()
    {
      InitializeComponent();
      #region Manejadores de Eventos
      ucRichTextBoxToolBar1.eColorPick += new EventHandler(ColorPick);
      ucRichTextBoxToolBar1.eExportRTF += new EventHandler(ExportRTF);
      ucRichTextBoxToolBar1.eLoadRTF += new EventHandler(LoadRTF);
      ucRichTextBoxToolBar1.eTextBold += new EventHandler(TextBold);
      ucRichTextBoxToolBar1.eTextCenter += new EventHandler(TextCenter);
      ucRichTextBoxToolBar1.eTextItalic += new EventHandler(TextItalic);
      ucRichTextBoxToolBar1.eTextLeft += new EventHandler(TextLeft);
      ucRichTextBoxToolBar1.eTextRight += new EventHandler(TextRight);
      ucRichTextBoxToolBar1.eTextStrikeOut += new EventHandler(TextStrikeOut);
      ucRichTextBoxToolBar1.eTextUnderLine += new EventHandler(TextUnderLine);
      ucRichTextBoxToolBar2.eChangeFontFamily += new EventHandler(ChangeFontFamily);
      ucRichTextBoxToolBar2.eChangeFontSize += new EventHandler(ChangeFontSize);
      #endregion
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      StaStart("Loading...");
      ObjectHelper.CopyProperties(product, oldProduct);
      UIHelper.SetUpControls(product, this);
      DataContext = product;
      LoadLanguages();
      LoadGifts();          
      if (enumMode != EnumMode.ReadOnly)
      {
        txtprID.IsEnabled = (enumMode == EnumMode.Add);
        txtprN.IsEnabled = true;
        chkprA.IsEnabled = true;
        cmbLanguages.IsEnabled = true;
        dgrGift.IsReadOnly = false;
        dcpText.IsEnabled = true;
        richTextBox.IsReadOnly = false;
        btnAccept.Visibility = Visibility.Visible;
        dgrGift.BeginningEdit += GridHelper.dgr_BeginningEdit;
      }      
    }
    #endregion

    #region Closing
    /// <summary>
    /// Se ejecuta cuando se está cerrando la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 221/05/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!blnClosing && enumMode != EnumMode.ReadOnly)
      {
        List<Gift> lstGifts = (List<Gift>)dgrGift.ItemsSource;
        string richText = UIRichTextBoxHelper.getRTFFromRichTextBox(ref richTextBox);
        if (!ObjectHelper.IsEquals(product, oldProduct) || richText != _productLegend.pxText || !ObjectHelper.IsListEquals(lstGifts, _oldlstGifts))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            dgrGift.CancelEdit();
          }
        }
      }
    }
    #endregion

    #region RowEditEnding
    /// <summary>
    /// Actualiza la fila al mometo de hacerle commit
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    private void dgrGift_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        dgrGift.RowEditEnding -= dgrGift_RowEditEnding;
        if (isCellCancel)
        {
          dgrGift.CancelEdit();
        }
        else
        {
          dgrGift.CommitEdit();
          dgrGift.Items.Refresh();
          GridHelper.SelectRow(dgrGift, dgrGift.SelectedIndex);
        }
        dgrGift.RowEditEnding += dgrGift_RowEditEnding;
      };
    }
    #endregion

    #region dgrGift_CellEditEnding
    /// <summary>
    /// Verifica que no se repita un registro ya seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    private void dgrGift_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if(e.EditAction==DataGridEditAction.Commit)
      {
        isCellCancel = false;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrGift,true);        
        e.Cancel = isRepeat;        
      }      
      else
      {
        isCellCancel = true;
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Agrega|Actualiza products
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
        {
          string richText = UIRichTextBoxHelper.getRTFFromRichTextBox(ref richTextBox);
          List<Gift> lstGift = (List<Gift>)dgrGift.ItemsSource;
          if (enumMode != EnumMode.Add && _productLegend.pxText == richText.Trim() && ObjectHelper.IsEquals(product, oldProduct) && ObjectHelper.IsListEquals(lstGift, _oldlstGifts))
          {
            blnClosing = true;
            Close();
          }
          else
          {
            StaStart("Saving Data...");
            string strMsj = ValidateHelper.ValidateForm(this, "Product", blnDatagrids: true);
            if (strMsj == "")
            {
              _productLegend.pxText = richText;
              List<Gift> lstAdd = lstGift.Where(gi => !_oldlstGifts.Any(gii => gii.giID == gi.giID)).ToList();
              List<Gift> lstDel = _oldlstGifts.Where(gi => !lstGift.Any(gii => gii.giID == gi.giID)).ToList();
              int nRes = await BRProducts.SaveProduct(product, (enumMode == EnumMode.Edit), _productLegend, lstAdd, lstDel);
              UIHelper.ShowMessageResult("Product", nRes);

              if (nRes > 0)
              {
                blnClosing = true;
                DialogResult = true;
                Close();
              }
            }
            else
            {
              UIHelper.ShowMessage(strMsj);
            }          
          }
        }
        catch(Exception ex)
        {
          UIHelper.ShowMessage(ex);
        }
        finally
        {
          StaEnd();
        }
      }
    #endregion

    #region IrichTextBox
    public void ChangeFontFamily(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnChangeFontFamily(ref richTextBox, ref ucRichTextBoxToolBar2.cbxfontFamilies);
    }

    public void ChangeFontSize(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnChangeFontSize(ref richTextBox, ref ucRichTextBoxToolBar2.cbxfontSize);
    }

    #region ColorPick
    /// <summary>
    /// Cambiar el color del texto
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    public void ColorPick(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnColorPick(ref richTextBox, ref ucRichTextBoxToolBar1.imgColorPick);
    }
    #endregion

    #region ExportRTF
    /// <summary>
    /// Exporta un rtf
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    public void ExportRTF(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnExportRTF(ref richTextBox);
    }
    #endregion

    #region LoadRtf
    /// <summary>
    /// Carga un rtf
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    public void LoadRTF(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnLoadRTF(ref richTextBox);
    }
    #endregion

    #region TextBold
    /// <summary>
    /// Cambia el texto a bold
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    public void TextBold(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextBold(ref richTextBox);
    }
    #endregion

    #region TextCenter
    /// <summary>
    /// Centra el texto
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel
    /// </history>
    public void TextCenter(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextCenter(ref richTextBox);
    }
    #endregion

    #region TextItalic
    /// <summary>
    /// Cambia el texto a italic
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    public void TextItalic(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextItalic(ref richTextBox);
    }
    #endregion

    #region TextLeft
    /// <summary>
    /// Alinea el texto a la izquierda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    public void TextLeft(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextLeft(ref richTextBox);
    }
    #endregion

    #region textRigth
    /// <summary>
    /// Alinea el texto a la derecha
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    public void TextRight(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextRight(ref richTextBox);
    }
    #endregion

    #region TextStrikeOut
    /// <summary>
    /// Tachado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    public void TextStrikeOut(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextStrikeOut(ref richTextBox);
    }
    #endregion

    #region TextUnderLine
    /// <summary>
    /// Subraya el texto
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/05/2016
    /// </history>
    public void TextUnderLine(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextUnderLine(ref richTextBox);
    }
    #endregion
    #endregion

    #region CmbSelecctionchanged
    /// <summary>
    /// Muestra la leyenda de un producto e idioma
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/05/2016
    /// </history>
    private void cmbLanguages_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      LoadLegends();
    }
    #endregion
    #endregion

    #region Methods
    #region LoadGifts
    /// <summary>
    /// Llena el grid de Gifts, Los combos de gifts
    /// </summary>
    /// <history>
    /// [emoguel] created 23/05/2016
    /// </history>
    private async void LoadGifts()
    {
      try
      {
        List<Gift> lstAllGifts = await BRGifts.GetGifts(-1, null);
        cmbGiftID.ItemsSource = lstAllGifts;
        List<Gift> lstGifts = lstAllGifts.Where(gi => gi.gipr == product.prID && gi.gipr != null).ToList();
        dgrGift.ItemsSource = lstGifts.ToList();
        _oldlstGifts = lstGifts.ToList();
        StaEnd();
      }
      catch(Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Product Detail");
      }
    }
    #endregion

    #region LoadLanguages
    /// <summary>
    /// Carga el combobox de Languages
    /// </summary>
    /// <history>
    /// [emoguel] created 23/05/2016
    /// </history>
    private async void LoadLanguages()
    {
      try
      {
        List<Language> lstLanguages = await BRLanguages.GetLanguages(null);
        cmbLanguages.ItemsSource = lstLanguages;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Product Detail");
      }
    }
    #endregion

    #region LoadLegends
    /// <summary>
    /// Carga el legend del product dependiendo del lenguage seleccionado
    /// </summary>
    /// <history>
    /// [emoguel] created 24/05/2016
    /// </history>
    private async void LoadLegends()
    {
      try
      {
        if (cmbLanguages.SelectedIndex > -1)
        {
          _productLegend.pxla = cmbLanguages.SelectedValue.ToString();
          _productLegend.pxpr = product.prID;
          ProductLegend prodLeg = await BRProductsLegends.GetProductLegend(_productLegend);
          if (prodLeg != null)
          {
            UIRichTextBoxHelper.LoadRTF(ref richTextBox, prodLeg.pxText);           
          }
          _productLegend.pxText = UIRichTextBoxHelper.getRTFFromRichTextBox(ref richTextBox);
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Product Detail");
      }
    }
    #endregion

    #region StaStart
    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <history>
    /// [emoguel] 24/05/2016 Created
    /// </history>
    /// <param name="message">mensaje</param>
    private void StaStart(string message)
    {      
      skpStatus.Visibility = Visibility.Visible;
      txtStatus.Text = message;
      Cursor = Cursors.Wait;
    }
    #endregion

    #region StaEnd
    /// <summary>
    /// Indica en la barra de estado que se termina un proceso
    /// </summary>
    /// <history>
    /// [emoguel] 24/05/2016 Created
    /// </history>
    private void StaEnd()
    {      
      skpStatus.Visibility = Visibility.Hidden;
      txtStatus.Text = "";
      Cursor = null;
    }
    #endregion

    #endregion

    #region richTextBox_SelectionChanged
    /// <summary>
    /// Modifica los controles segun el las propiedades del texto seleccionado
    /// </summary>
    /// <history>
    /// [jorcanche] created 20/08/2016
    ///  </history>
    private void richTextBox_SelectionChanged(object sender, RoutedEventArgs e)
    {
      RichTextBoxToolBar.OnSalectionChanded(ref richTextBox, ref ucRichTextBoxToolBar2, ref ucRichTextBoxToolBar1);
    } 
    #endregion
  }
}

using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Styles.Classes;
using IM.Styles.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IM.InvitConfig.Forms
{
  /// <summary>
  ///  Formulario para la configuracion de la informacion general de la invitacion
  /// </summary>
  /// <history>
  /// [jorcanche]  created 04032016
  /// </history>
  public partial class frmInvitConfig : Window, IRichTextBoxToolBar
  {
    #region Variables Globales
    private InvitationText _rtfInvitation = new InvitationText();
    private RichTextBox _rtb;
    #endregion

    #region Contructores y destructores
    public frmInvitConfig()
    {
      InitializeComponent();
      #region Manejadores de Eventos
      tbrStyle.eColorPick += ColorPick;
      tbrStyle.eExportRTF += ExportRTF;
      tbrStyle.eLoadRTF += LoadRTF;
      tbrStyle.eTextBold += TextBold;
      tbrStyle.eTextCenter += TextCenter;
      tbrStyle.eTextItalic += TextItalic;
      tbrStyle.eTextLeft += TextLeft;
      tbrStyle.eTextRight += TextRight;
      tbrStyle.eTextStrikeOut += TextStrikeOut;
      tbrStyle.eTextUnderLine += TextUnderLine;
      tbrFontStyle.eChangeFontFamily += ChangeFontFamily;
      tbrFontStyle.eChangeFontSize += ChangeFontSize;
      #endregion
    }
    #endregion

    #region Eventos
    public void ChangeFontFamily(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnChangeFontFamily(ref _rtb, ref tbrFontStyle.cbxfontFamilies);     
    }

    public void ChangeFontSize(object sender, EventArgs e)
    {     
      RichTextBoxToolBar.OnChangeFontSize(ref _rtb, ref tbrFontStyle.cbxfontSize);
    }

    public void ColorPick(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnColorPick(ref _rtb, ref tbrStyle.imgColorPick);
    }

    public void ExportRTF(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnExportRTF(ref _rtb);
    }

    public void LoadRTF(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnLoadRTF(ref _rtb);
    }

    public void TextBold(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextBold(ref _rtb);     
    }

    public void TextCenter(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextCenter(ref _rtb);     
    }

    public void TextItalic(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextItalic(ref _rtb);     
    }

    public void TextLeft(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextLeft(ref _rtb);      
    }

    public void TextRight(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextRight(ref _rtb);      
    }

    public void TextStrikeOut(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextStrikeOut(ref _rtb);
    }

    public void TextUnderLine(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextUnderLine(ref _rtb);
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga el formulario e inicializa variables
    /// </summary>
    ///<history>
    ///[jorcanche] created 12/05/2016
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// [edgrodriguez] 21/May/2016 Modified El método GetLeadSourcesByUser se volvió asincrónico.
    ///</history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      EnableControls(true, false, true, false);
      StaStart("Load LeadSource...");
      cmbLeadSource.ItemsSource = await BRLeadSources.GetLeadSourcesByUser(Context.User.User.peID);
      StaStart("Load Languages...");
      cmbLanguage.ItemsSource =await BRLanguages.GetLanguages(1);
      cmbLanguage.SelectedIndex = cmbLeadSource.SelectedIndex = 0;
      StaEnd();
    } 
    #endregion

    #region LoadRTF
    /// <summary>
    /// Carga los RTF´s de Header y Fotter segun el Lead Source o el lenguage que se seleccione
    /// </summary>
    ///<history>
    ///[jorcanche] created 12/05/2016
    ///</history>
    private async void LoadRTF()
    {
      if (cmbLanguage.SelectedIndex == -1 || cmbLeadSource.SelectedIndex == -1) return;
      rtbHeader.Document.Blocks.Clear();
      rtbFooter.Document.Blocks.Clear();
      _rtfInvitation = await  BRInvitation.GetInvitationFooterHeader(cmbLeadSource.SelectedValue.ToString(), cmbLanguage.SelectedValue.ToString());
      if (_rtfInvitation == null) return;
      UIRichTextBoxHelper.LoadRTF(ref rtbHeader, _rtfInvitation.itRTFHeader);
      UIRichTextBoxHelper.LoadRTF(ref rtbFooter, _rtfInvitation.itRTFFooter);
    } 
    #endregion

    #region cmbLeadSource_SelectionChanged
    /// <summary>
    /// Carga los rft´s cada vez que se selecciona un Lead Source
    /// </summary>
    ///<history>
    ///[jorcanche] created 12/05/2016
    ///</history>
    private void cmbLeadSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StaStart("Load LeadSource...");
      LoadRTF();
      StaEnd();

    } 
    #endregion

    #region cmbLanguage_SelectionChanged
    /// <summary>
    /// Carga los rtf´s cada vez que se selecciona un Language
    /// </summary>
    ///<history>
    ///[jorcanche] created 12/05/2016
    ///</history>
    private void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StaStart("Load Language");
      LoadRTF();
      StaEnd();
    } 
    #endregion

    #region EnableControls
    /// <summary>
    /// Habilita o deshabilita los controles
    /// </summary>
    /// <param name="cmb"> Combobox</param>
    /// <param name="save">Boton de Salvar</param>
    /// <param name="exit">Boton de Exit</param>
    /// <param name="style">Boton de Cancel, el toolbar de FontStyle y fornstyle</param>
    /// <param name="visibility">Visibilidad de FontStyle</param>
    ///<history>
    ///[jorcanche] created 12/05/2016
    ///</history>
    private void EnableControls(bool cmb, bool save, bool exit, bool style, Visibility visibility = Visibility.Collapsed)
    {
      cmbLanguage.IsEnabled = cmbLeadSource.IsEnabled = btnEdit.IsEnabled = btnPreview.IsEnabled = cmb;
      btnSave.IsEnabled = save;
      btnExit.IsEnabled = exit;
      tbrStyle.IsEnabled = tbrFontStyle.IsEnabled = btnCancel.IsEnabled = style;
      tbrFontStyle.Visibility = tbrStyle.Visibility = visibility;
    } 
    #endregion

    #region btnEdit_Click
    /// <summary>
    /// Edita el Header y el Footer  
    /// </summary>
    ///<history>
    ///[jorcanche] created 12/05/2016
    ///</history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      StaStart("Edit Invit...");
      EnableControls(false, true, false, true, Visibility.Visible);
      rtbFooter.IsReadOnly = rtbHeader.IsReadOnly = false;
      rtbHeader.Focus();
      StaEnd();
    } 
    #endregion

    #region btnSave_Click

    /// <summary>
    /// Guarda losa cambios de la edición 
    /// </summary>
    ///<history>
    ///[jorcanche] created 12/05/2016
    ///</history>
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        StaStart("Save Invit...");
        EnableControls(true, false, true, false);
        rtbFooter.IsReadOnly = rtbHeader.IsReadOnly = true;

        #region Carga de Header y Footer

        //Se almacena en una variabele los RTF´s
        var header = UIRichTextBoxHelper.getRTFFromRichTextBox(ref rtbHeader);
        var footer = UIRichTextBoxHelper.getRTFFromRichTextBox(ref rtbFooter);
        StaEnd();

        //Si es nula la entidad quiere decir que no existe en la base de datos así que hay que agrgarlo
        //de lo contrario se modifica
        if (_rtfInvitation == null)
        {
          _rtfInvitation = new InvitationText
          {
            itls = cmbLeadSource.SelectedValue.ToString(),
            itla = cmbLanguage.SelectedValue.ToString(),
            itRTFFooter = footer,
            itRTFHeader = header
          };
          await BREntities.OperationEntity(_rtfInvitation, EnumMode.Add);
        }
        else
        {
          //Si almenos un RichTexBox se modifico entonces se hace la actualización
          if (_rtfInvitation.itRTFFooter == footer && _rtfInvitation.itRTFHeader == header) return;
          _rtfInvitation.itRTFFooter = footer;
          _rtfInvitation.itRTFHeader = header;
          await BREntities.OperationEntity(_rtfInvitation, EnumMode.Edit);
        }      
        StaEnd();

        #endregion
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }

    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Cancela la edición 
    /// </summary>
    ///<history>
    ///[jorcanche] created 12/05/2016
    ///</history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      StaStart("Cancel Invit...");
      EnableControls(true, false, true, false, Visibility.Collapsed);
      rtbFooter.IsReadOnly = rtbHeader.IsReadOnly = true;
      LoadRTF();
      StaEnd();
    } 
    #endregion

    #region btnPreview_Click

    /// <summary>
    /// Visualiza como queda en el reporte de inviatacion
    /// </summary>
    ///<history>
    ///[jorcanche] created 12/05/2016
    ///</history>
    private async void btnPreview_Click(object sender, RoutedEventArgs e)
    {
      if (!UIRichTextBoxHelper.HasInfo(ref rtbHeader) || !UIRichTextBoxHelper.HasInfo(ref rtbFooter))
      {
        UIHelper.ShowMessage("The header and footer must contain information. \n They can not be empty");
        return;
      }
      StaStart("Preview Invit...");
      if (cmbLanguage.SelectedValue == null || cmbLeadSource.SelectedValue == null) return;
      EnableControls(true, false, true, false);
      var guest = await BRGuests.GetGuest(0);
      guest.guls = cmbLeadSource.SelectedValue.ToString();
      guest.gula = cmbLanguage.SelectedValue.ToString();
      await BREntities.OperationEntity(guest, EnumMode.Edit);
      RptInvitationHelper.RptInvitation(window: this);
      StaEnd();
    }

    #endregion

    #region btnExit_Click
    /// <summary>
    /// Cierra el formulario
    /// </summary>
    ///<history>
    ///[jorcanche] created 12/05/2016
    ///</history>
    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
      StaStart("Exit Invit...");
      Close();
      StaEnd();
    } 
    #endregion

    #region Window_Closing
    /// <summary>
    /// Pregunta antes de cerrar el formulario
    /// </summary>
    ///<history>
    ///[jorcanche] created 12/05/2016
    ///</history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (btnEdit.IsEnabled) return;
      var result = UIHelper.ShowMessage("This form is currently in edit mode. Please save or undo your changes before closing it. You still want to close?", MessageBoxImage.Question, "Mail Outs Configuration");
      if (result == MessageBoxResult.No)
      {
        e.Cancel = true;
      }
    }
    #endregion

    #region rtb_GotFocus
    /// <summary>
    /// Le asigna el control a la variable global
    /// </summary>
    ///<history>
    ///[jorcanche] created 12/05/2016
    ///</history>
    private void rtb_GotFocus(object sender, RoutedEventArgs e)
    {
      _rtb = sender as RichTextBox;
    } 
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    ///  [jorcanche] 12/05/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
          break;
        case Key.Insert:
          KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
          break;
        case Key.NumLock:
          KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
          break;
      }
    }

    #endregion   

    #region StaStart
    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <history>
    /// [jorcanche] 12/05/2016 Created
    /// </history>
    /// <param name="message">mensaje</param>
    private void StaStart(string message)
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      Cursor = Cursors.Wait;
    } 
    #endregion

    #region StaEnd
    /// <summary>
    /// Indica en la barra de estado que se termina un proceso
    /// </summary>
    /// <history>
    ///  [jorcanche] 12/05/2016 Created
    /// </history>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      Cursor = null;
    }
    #endregion

    #region OnIsKeyboardFocusedChanged
    /// <summary>
    /// comprobará si existen cambios en el teclado (Si fueron oprimidas las teclas MAYUS,INSERT y BLOQ NUM) y refresca nuestro StatusBar
    /// </summary>
    /// <history>
    /// [jorcanche]  created
    /// </history>
    private void OnIsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region SelectionChanged
    /// <summary>
    /// Modifica los controles segun el las propiedades del texto seleccionado
    /// </summary>
    /// <history>
    /// [jorcanche] created 20/08/2016
    ///  </history>
    private void SelectionChanged(object sender, RoutedEventArgs e)
    {
      RichTextBoxToolBar.OnSalectionChanded(ref _rtb, ref tbrFontStyle, ref tbrStyle);
    }

    #endregion
  }
}

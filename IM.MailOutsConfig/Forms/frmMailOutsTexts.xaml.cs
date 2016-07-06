using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.MailOutsConfig.Classes;
using IM.MailOutsConfig.Reports;
using IM.Model;
using IM.Model.Enums;
using Cursors = System.Windows.Input.Cursors;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MenuItem = System.Windows.Controls.MenuItem;
using IM.Styles.Interfaces;
using IM.Styles.Classes;
using System;

namespace IM.MailOutsConfig.Forms
{
  /// <summary>
  /// Interaction logic for frmMailOutsTexts.xaml
  /// </summary>
  public partial class frmMailOutsTexts : Window, IRichTextBoxToolBar
  {
    #region Propiedades, Atributos
    private rptMailOuts _rptMailOuts = new rptMailOuts(); //Reporte
    private MailOutText SelectedMailOutsText; //MailOutsText Selected
    public ExecuteCommandHelper RefreshDataSource { get; set; }
    #endregion

    public frmMailOutsTexts()
    {
      InitializeComponent();
      RefreshDataSource = new ExecuteCommandHelper(x => LoadDataSource());

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

    #region Eventos de la ventana
    /// <summary>
    /// Evento que se lanza al iniciar la aplicacion
    /// </summary>
    /// <history>
    /// [erosado] 06/Mar/2016 Created
    /// </history>   
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadDataSource();
      //Agregamos la informacion del usuario en la interfaz
      txtbUserName.Text = App.User.User.peN;
    }
    /// <summary>
    /// Actualiza el RTF de algun MailOutsText
    /// </summary>
    /// <history>
    /// [erosado] 09/04/2016  Created
    /// </history>
    private void imgSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (UIRichTextBoxHelper.HasInfo(ref richTextBox))
      {

        if (SelectedMailOutsText != null)
        {
          StaStart("Saving Data...");
          SelectedMailOutsText.mtRTF = UIRichTextBoxHelper.getRTFFromRichTextBox(ref richTextBox);
          DoUpdateMailOutText(SelectedMailOutsText);
        }
        else
        {
          UIHelper.ShowMessage("Select Mail Outs Text first", MessageBoxImage.Information, "Mail Outs Configuration");
        }
      }
      else
      {
        UIHelper.ShowMessage("There is no info to Save.", MessageBoxImage.Information, "Mail Outs Configuration");
      }
    }
    /// <summary>
    /// Activa el RichTextBox para que pueda ser editado
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    private void imgEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      EditModeOn();
    }
    /// <summary>
    /// Cancela el modo edicion del RTF que esta cargado.
    /// </summary>
    /// <history>
    /// [erosadp] 07/04/2016  Created
    /// </history>
    private void imgCancel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      EditModeOff();
      if (SelectedMailOutsText != null)
      {
        UIRichTextBoxHelper.LoadRTF(ref richTextBox, SelectedMailOutsText.mtRTF);
      }
    }
    /// <summary>
    /// Muestra la vista previa del reporte que esta cargado en el richTextBox
    /// </summary>
    ///<history>
    ///[erosado]  08/04/2016  Created
    /// </history>
    private void imgPreview_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (UIRichTextBoxHelper.HasInfo(ref richTextBox))
      {
        StaStart("Loading preview...");
        fillMailReport();
        var _frmViewer = new frmViewer(_rptMailOuts);
        _frmViewer.ShowDialog();
        StaEnd();
      }
      else
      {
        UIHelper.ShowMessage("There is no info to show a preview view.", MessageBoxImage.Information, "Mail Outs Configuration");
      }

    }

    private void imgAdminMailOuts_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      frmMailOutsConfig mailOutsConfig = new frmMailOutsConfig();
      mailOutsConfig.txtbUserName.Text = App.User.User.peN;
      mailOutsConfig.ShowDialog();
    }
    /// <summary>
    /// Cierra la aplicación
    /// </summary>
    ///<history>
    ///[erosado]  08/04/2016  Created
    /// </history>
    private void imgExit_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
    {
      Close();
    }
    /// <summary>
    /// Cierra la aplicación
    /// </summary>
    ///<history>
    ///[erosado]  08/04/2016  Created
    /// </history>
    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (!imgEdit.IsEnabled)
      {
        MessageBoxResult result = UIHelper.ShowMessage("This form is currently in edit mode. Please save or undo your changes before closing it. You still want to close?", MessageBoxImage.Question, "Mail Outs Configuration");
        if (result == MessageBoxResult.No)
        {
          e.Cancel = true;
        }
      }
    }
    /// <summary>
    /// Obtiene los MailOutText
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    private void cbxLeadSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      LeadSourceByUser lsbyUser = cbxLeadSource.SelectedItem as LeadSourceByUser;
      LanguageShort laShort = cbxLanguage.SelectedItem as LanguageShort;
      DoGetMailOutText(lsbyUser?.lsID, laShort?.laID);
      richTextBox.SelectAll();
      richTextBox.Selection.Text = string.Empty;
    }
    /// <summary>
    /// Carga en el richTextBox el RTF seleccionado
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    private void lsbxMailOuts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      SelectedMailOutsText = lsbxMailOuts.SelectedItem as MailOutText;

      if (SelectedMailOutsText != null)
      {
        
        UIRichTextBoxHelper.LoadRTF(ref richTextBox, SelectedMailOutsText.mtRTF);
      }
    }
    #endregion

    #region Async Methods

    /// <summary>
    /// Obtiene los LeadSources por User, Programs All  y Region All
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// [edgrodriguez] 21/05/2016 Modified. El metodo GetLeadSourcesByUser se volvió asincrónico.
    /// </history>
    /// <param name="user">loginUserID</param>
    public async void DoGetLeadSources(string user)
    {
      try
      {
        var data = await BRLeadSources.GetLeadSourcesByUser(user, EnumProgram.Inhouse);
        if (data.Count > 0)
        {
          cbxLeadSource.ItemsSource = data;
          cbxLeadSource.SelectedIndex = 0;
        }
        else
        {
          cbxLeadSource.Text = "No data found - Press Ctrl+F5 to load Data";
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex.InnerException.Message, MessageBoxImage.Error, "Mail Outs Configuration");
      }
    }

    /// <summary>
    /// Obtiene los Lenguajes Activos
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async void DoGetLanguages()
    {
      try
      {
        var data = await BRLanguages.GetLanguages(1);
        if (data.Count > 0)
        {
          cbxLanguage.ItemsSource = data;
          cbxLanguage.SelectedIndex = 0;
        }
        else
        {
          cbxLanguage.Text = "No data found - Press Ctrl+F5 to load Data";
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex.InnerException.Message, MessageBoxImage.Error, "Mail Outs Configuration");
      }
    }
    /// <summary>
    /// Obtiene la lista de MailOutText por LeadSourceID y LanguageID
    /// </summary>
    /// <param name="ls">LeadSourceID</param>
    /// <param name="la">LanguageID</param>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    public async void DoGetMailOutText(string ls, string la)
    {
      try
      {
        if (!string.IsNullOrEmpty(ls) && !string.IsNullOrEmpty(la))
        {
          var data = await BRMailOutTexts.GetMailOutTexts(ls, la, 1);
          if (data.Count > 0)
          {
            lsbxMailOuts.ItemsSource = data;
            txtbMailOutsNumber.Text = data.Count.ToString();
            lsbxMailOuts.SelectedIndex = 0;
          }
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex, MessageBoxImage.Error, "Intelligence Marketing");
      }
    }
    /// <summary>
    /// Actualiza la información del RTF de un MailOutsText
    /// </summary>
    /// <param name="mot">MailOutsText</param>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    public async void DoUpdateMailOutText(MailOutText mot)
    {
      try
      {
        await BRMailOutTexts.UpdateRTFMailOutTexts(mot);
        UIRichTextBoxHelper.LoadRTF(ref richTextBox, mot.mtRTF);
        UIHelper.ShowMessage("Data saved successfully", MessageBoxImage.Information, "Mail Outs Configuration");
        EditModeOff();
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex, MessageBoxImage.Error, "Intelligence Marketing");
      }
    }
    #endregion

    #region StatusBar
    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }

    /// <summary>
    /// Verifica que la tecla se encuentre activa/inactiva, para cambiar el estilo de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void CkeckKeysPress(StatusBarItem statusBar, Key key)
    {
      var keyPess = Keyboard.GetKeyStates(key).ToString();

      if (keyPess.Contains("Toggled")) //si está activo el Bloq Mayús
      {
        statusBar.FontWeight = FontWeights.Bold;
        statusBar.Foreground = Brushes.Black;
      }
      else
      {
        KeyDefault(statusBar);
      }
    }

    /// <summary>
    /// Configuracion inicial de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void KeyDefault(StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }

    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    /// <param name="message">mensaje</param>
    private void StaStart(string message)
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      Cursor = Cursors.Wait;

    }

    /// <summary>
    /// Indica en la barra de estado que se termina un proceso
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      Cursor = null;
    }

    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Add Labels
    /// <summary>
    /// Se encarga de agregar etiquetas al rtf desde MenuItem
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    private void button_Click(object sender, RoutedEventArgs e)
    {
      string header = ((MenuItem)sender).Header.ToString();
      if (richTextBox.IsReadOnly == false)
      {
        if (!string.IsNullOrEmpty(header))
        {
          switch (header)
          {
            case "<Last Name>":
              richTextBox.Selection.Text = "<LastName1>";
              break;
            case "<First Name>":
              richTextBox.Selection.Text = "<FirstName1>";
              break;
            case "<Book Time>":
              richTextBox.Selection.Text = "<BookTime>";
              break;
            case "<Pick Up>":
              richTextBox.Selection.Text = "<PickUp>";
              break;
            case "<Agency>":
              richTextBox.Selection.Text = "<Agency>";
              break;
            case "<Room Num>":
              richTextBox.Selection.Text = "<RoomNum>";
              break;
            case "<PR Code>":
              richTextBox.Selection.Text = "<PRCode>";
              break;
            case "<PR Name>":
              richTextBox.Selection.Text = "<PRName>";
              break;
            case "<Printed By>":
              richTextBox.Selection.Text = "<PrintedBy>";
              break;
            default:
              break;
          }
        }
      }
      else
      {
        UIHelper.ShowMessage("You must press first edit button.", MessageBoxImage.Information, "Mail Outs Configuration");
      }
    }
    /// <summary>
    /// Se encarga de agregar etiquetas al rtf desde listbox 
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    private void lstAddLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      int option = lstAddLabel.SelectedIndex;
      if (richTextBox.IsReadOnly == false)
      {
        if (option != -1)
        {
          switch (option)
          {
            case 0:
              richTextBox.Selection.Text = "<LastName1>";
              break;
            case 1:
              richTextBox.Selection.Text = "<FirstName1>";
              break;
            case 2:
              richTextBox.Selection.Text = "<BookTime>";
              break;
            case 3:
              richTextBox.Selection.Text = "<PickUp>";
              break;
            case 4:
              richTextBox.Selection.Text = "<Agency>";
              break;
            case 5:
              richTextBox.Selection.Text = "<RoomNum>";
              break;
            case 6:
              richTextBox.Selection.Text = "<PRCode>";
              break;
            case 7:
              richTextBox.Selection.Text = "<PRName>";
              break;
            case 8:
              richTextBox.Selection.Text = "<PrintedBy>";
              break;
            default:
              break;
          }
        }
      }
      else
      {
        UIHelper.ShowMessage("You must press first edit button.", MessageBoxImage.Information, "Mail Outs Configuration");
      }

    }
    #endregion

    #region Metodos 
    /// <summary>
    /// Ingresa la información del RichTextBox en un CrystalReport rptMailOuts
    /// </summary>
    /// <history>
    /// [erosado] 08/04/2016  Created
    /// </history>
    private void fillMailReport()
    {
      List<GuestMailOutsText> _guestMailOutsText = new List<GuestMailOutsText>();
      _guestMailOutsText.Add(new GuestMailOutsText { mtRTF = UIRichTextBoxHelper.getRTFFromRichTextBox(ref richTextBox), MrMrs = "<MrMrs>", LastName = "<LastName1>", Room = "<Room>", RoomNum = "<RoomNum>" });
      _rptMailOuts.Load();
      _rptMailOuts.SetDataSource(_guestMailOutsText);
    }
    /// <summary>
    /// Activa el modo Edicion del richTextBox
    /// </summary>
    /// <history>
    /// [erosado] 08/04/2016  Created
    /// </history>
    private void EditModeOn()
    {
      cbxLeadSource.IsEnabled = false;
      cbxLanguage.IsEnabled = false;
      lsbxMailOuts.IsEnabled = false;
      imgEdit.IsEnabled = false;
      imgSave.IsEnabled = true;
      imgCancel.IsEnabled = true;
      richTextBox.IsReadOnly = false;
      txtEditMode.Visibility = Visibility.Visible;
      ucRichTextBoxToolBar1.Visibility = Visibility.Visible;
      ucRichTextBoxToolBar2.Visibility = Visibility.Visible;
    }
    /// <summary>
    /// Desactiva el modo Edicion del richTextBox
    /// </summary>
    /// <history>
    /// [erosado] 08/04/2016  Created
    /// </history>
    private void EditModeOff()
    {
      cbxLeadSource.IsEnabled = true;
      cbxLanguage.IsEnabled = true;
      lsbxMailOuts.IsEnabled = true;
      imgEdit.IsEnabled = true;
      imgCancel.IsEnabled = false;
      imgSave.IsEnabled = false;
      richTextBox.IsReadOnly = true;
      txtEditMode.Visibility = Visibility.Collapsed;
      ucRichTextBoxToolBar1.Visibility = Visibility.Hidden;
      ucRichTextBoxToolBar2.Visibility = Visibility.Collapsed;
    }
    /// <summary>
    /// Carga LeadSource & Lenguages
    /// </summary>
    /// <history>
    /// [erosado] 19/04/2016  Created
    /// </history>
    private void LoadDataSource()
    {
      StaStart("Loading LeadSources & Languages...");
      //Cargamos LeadSource
      DoGetLeadSources(App.User.User.peID);
      //Cargamos Lenguage
      DoGetLanguages();
    }
    #endregion

    #region IRichTextBoxToolBar
    public void ChangeFontFamily(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnChangeFontFamily(ref richTextBox, ref ucRichTextBoxToolBar2.cbxfontFamilies);
    }

    public void ChangeFontSize(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnChangeFontSize(ref richTextBox, ref ucRichTextBoxToolBar2.cbxfontSize);
    }

    public void ColorPick(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnColorPick(ref richTextBox);
    }

    public void ExportRTF(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnExportRTF(ref richTextBox);
    }

    public void LoadRTF(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnLoadRTF(ref richTextBox);
    }

    public void TextBold(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextBold(ref richTextBox);
    }

    public void TextCenter(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextCenter(ref richTextBox);
    }

    public void TextItalic(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextItalic(ref richTextBox);
    }

    public void TextLeft(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextLeft(ref richTextBox);
    }

    public void TextRight(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextRight(ref richTextBox);
    }

    public void TextStrikeOut(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextStrikeOut(ref richTextBox);
    }

    public void TextUnderLine(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextUnderLine(ref richTextBox);
    }
    #endregion

  }
}

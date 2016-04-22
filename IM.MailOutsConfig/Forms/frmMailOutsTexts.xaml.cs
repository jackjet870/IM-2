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

namespace IM.MailOutsConfig.Forms
{
  /// <summary>
  /// Interaction logic for frmMailOutsTexts.xaml
  /// </summary>
  public partial class frmMailOutsTexts : Window
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
      // Cargamos el tipo de fuentes
      loadFontSizeAndFontFamilies();
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
    /// </history>
    /// <param name="user">loginUserID</param>
    public void DoGetLeadSources(string user)
    {
      Task.Factory.StartNew(() => BRLeadSources.GetLeadSourcesByUser(user,EnumProgram.Inhouse))
      .ContinueWith(
      task1 =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error,"Mail Outs Configuration");
          StaEnd();
          return false;
        }
        if (task1.IsCompleted)
        {
          List<LeadSourceByUser> data = task1.Result;
          if (data.Count > 0)
          {
            cbxLeadSource.ItemsSource = data;
            cbxLeadSource.SelectedIndex = 0;
          }
          else
          {
            cbxLeadSource.Text = "No data found - Press Ctrl+F5 to load Data";
          }
        }
        StaEnd();
        return false;
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }

    /// <summary>
    /// Obtiene los Lenguajes Activos
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    public void DoGetLanguages()
    {
      Task.Factory.StartNew(() => BRLanguages.GetLanguages(1))
      .ContinueWith(
      task1 =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error, "Mail Outs Configuration");
          StaEnd();
          return false;
        }
        if (task1.IsCompleted)
        {
          List<LanguageShort> data = task1.Result;
          if (data.Count > 0)
          {
            cbxLanguage.ItemsSource = data;
            cbxLanguage.SelectedIndex = 0;
          }
          else
          {
            cbxLanguage.Text = "No data found - Press Ctrl+F5 to load Data";
          }
        }
        StaEnd();
        return false;
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }
    /// <summary>
    /// Obtiene la lista de MailOutText por LeadSourceID y LanguageID
    /// </summary>
    /// <param name="ls">LeadSourceID</param>
    /// <param name="la">LanguageID</param>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    public void DoGetMailOutText(string ls, string la)
    {
      Task.Factory.StartNew(() => BRMailOutTexts.GetMailOutTexts(ls,la,1))
      .ContinueWith(
      task1 =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error,"Mail Outs Configuration");
          StaEnd();
          return false;
        }
        if (task1.IsCompleted)
        {
          List<MailOutText> data = task1.Result;
          if (data.Count > 0)
          {
            lsbxMailOuts.ItemsSource = data;
            txtbMailOutsNumber.Text = data.Count.ToString();
            lsbxMailOuts.SelectedIndex = 0;
          }
        }
        StaEnd();
        return false;
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }
    /// <summary>
    /// Actualiza la información del RTF de un MailOutsText
    /// </summary>
    /// <param name="mot">MailOutsText</param>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    public void DoUpdateMailOutText(MailOutText mot)
    {
      Task.Factory.StartNew(() => BRMailOutTexts.UpdateRTFMailOutTexts(mot))
      .ContinueWith(
      task1 =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error, "Mail Outs Configuration");
          StaEnd();
          return false;
        }
        if (task1.IsCompleted)
        {
          UIRichTextBoxHelper.LoadRTF(ref richTextBox, mot.mtRTF);
          UIHelper.ShowMessage("Data saved successfully",MessageBoxImage.Information,"Mail Outs Configuration");
          EditModeOff();
        }
        StaEnd();
        return false;
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
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

    #region ToolBar RichTextBox
    /// <summary>
    /// Carga un archivo RTF al RIchTextBox
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    private void imgLoadRTF_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      UIRichTextBoxHelper.LoadRTF(ref richTextBox);
    }
    /// <summary>
    /// Exporta el contenido del RichTextBox a un archivo RTF
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    private void imgExportRTF_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      UIRichTextBoxHelper.ExportRTF(ref richTextBox);
    }
    /// <summary>
    /// Cambia la letra del texto seleccionado en el richtextbox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    private void cbxfontFamilies_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (cbxfontFamilies.SelectedItem != null)
        richTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, cbxfontFamilies.SelectedItem);
    }
    /// <summary>
    /// Cambia a negritas el texto seleccionado en el richtextbox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    private void imgTextBold_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      object temp = richTextBox.Selection.GetPropertyValue(TextElement.FontWeightProperty);
      if (temp != null)
      {
        if (!temp.Equals(FontWeights.Bold))
        {
          richTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }
        else
        {
          richTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
        }
      }
    }
    /// <summary>
    /// Cambia a italica el texto seleccionado en el richtextbox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    private void imgTextItalic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      object temp = richTextBox.Selection.GetPropertyValue(TextElement.FontStyleProperty);
      if (temp != null)
      {
        if (!temp.Equals(FontStyles.Italic))
        {
          richTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
        }
        else
        {
          richTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
        }
      }
    }
    /// <summary>
    /// Cambia a Subrayado el texto seleccionado en el richtextbox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    private void imgTextUnderLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      object temp = richTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
      if (temp != null)
      {
        if (!temp.Equals(TextDecorations.Underline))
        {
          richTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
        }
        else
        {
          richTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
        }
      }
    }
    /// <summary>
    /// Cambia a Tachado el texto seleccionado en el richtextbox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    private void imgTextStrikeOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      object temp = richTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
      if (temp != null)
      {
        if (!temp.Equals(TextDecorations.Strikethrough))
        {
          richTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Strikethrough);
        }
        else
        {
          richTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
        }
      }
    }
    /// <summary>
    /// Cambia el tamaño del texto seleccionado en el richtextbox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    private void cbxfontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      richTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, cbxfontSize.SelectedItem.ToString());
    }

    private void imgTextLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (!richTextBox.Selection.GetPropertyValue(FlowDocument.TextAlignmentProperty).Equals(TextAlignment.Left))
      {
        richTextBox.Selection.ApplyPropertyValue(FlowDocument.TextAlignmentProperty, TextAlignment.Left);
      }
      else
      {
        richTextBox.Selection.ApplyPropertyValue(FlowDocument.TextAlignmentProperty, TextAlignment.Left);
      }
    }
    /// <summary>
    /// Centra  el texto seleccionado en el richtextbox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    private void imgTextCenter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (!richTextBox.Selection.GetPropertyValue(FlowDocument.TextAlignmentProperty).Equals(TextAlignment.Center))
      {
        richTextBox.Selection.ApplyPropertyValue(FlowDocument.TextAlignmentProperty, TextAlignment.Center);
      }
      else
      {
        richTextBox.Selection.ApplyPropertyValue(FlowDocument.TextAlignmentProperty, TextAlignment.Left);
      }
    }
    /// <summary>
    /// Alinea a la derecha  el texto seleccionado en el richtextbox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    private void imgTextRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (!richTextBox.Selection.GetPropertyValue(FlowDocument.TextAlignmentProperty).Equals(TextAlignment.Right))
      {
        richTextBox.Selection.ApplyPropertyValue(FlowDocument.TextAlignmentProperty, TextAlignment.Right);
      }
      else
      {
        richTextBox.Selection.ApplyPropertyValue(FlowDocument.TextAlignmentProperty, TextAlignment.Left);
      }
    }
    /// <summary>
    /// Cambia el color de el texto seleccionado en el richtextbox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    private void imgColorPick_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      ColorDialog colorDialog = new ColorDialog();
      if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
        TextRange range = new TextRange(richTextBox.Selection.Start, richTextBox.Selection.End);
        range.ApplyPropertyValue(TextElement.ForegroundProperty, scb);
      }

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
        UIHelper.ShowMessage("You must press first edit button.",MessageBoxImage.Information, "Mail Outs Configuration");
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
    /// Carga la información de Font size y font Familie en los combos
    /// </summary>
    /// <history>
    /// [erosado] 08/04/2016  Created
    /// </history>
    public void loadFontSizeAndFontFamilies()
    {
      //Load Font Families
      cbxfontFamilies.ItemsSource = System.Drawing.FontFamily.Families.Select(s => s.Name).ToArray();
      cbxfontFamilies.SelectedIndex = 0;

      //Load FontSize
      List<int> fontSize = new List<int>();
      for (int i = 4; i <= 72; i++)
      {
        fontSize.Add(i);
      }
      cbxfontSize.ItemsSource = fontSize;
      cbxfontSize.SelectedIndex = 0;
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
      stkTextTools.Visibility = Visibility.Visible;
      brdFontsStyle.Visibility = Visibility.Visible;
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
      stkTextTools.Visibility = Visibility.Hidden;
      brdFontsStyle.Visibility = Visibility.Collapsed;
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
      DoGetLeadSources("EROSADO");
      //Cargamos Lenguage
      DoGetLanguages();
    }

    #endregion
    
  }
}

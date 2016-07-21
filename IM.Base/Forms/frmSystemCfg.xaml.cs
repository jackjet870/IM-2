using Microsoft.Win32;
using System;
using System.Printing;
using System.Windows;
using IM.Base.Helpers;
using IM.Model.Enums;
using System.IO;
using System.Windows.Controls;

namespace IM.Base.Forms
{
  /// <summary>
  /// Módulo para configurar opciones de sistema
  /// </summary>
  /// <history>
  /// [lchairez] 05/Feb/2016 Created
  /// [vku] 06/Jun/2016 Modified. Renombre el formulario a SystemCfg
  /// </history>
  public partial class frmSystemCfg : Window
  {

    #region Atributo
    readonly EnumConfiguration _enumConfiguration;
    string defaultSelectedPath = "P:\\";
    public string FileName
    {
      get { return txtPath.Text; }
      set { txtPath.Text = value; }
    }
    #endregion

    #region Constructor
    public frmSystemCfg(
      EnumConfiguration enumConfiguration = EnumConfiguration.None
      )
    {
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
      InitializeComponent();
      _enumConfiguration = enumConfiguration;
    }
    #endregion

    #region Métodos de la forma

    #region Window_Loaded
    /// <summary>
    /// Configura el formulario
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      FillPrinters();
      SelectItemTab();
      SelectedPrinterLoaded();
      LoadReportsPath();
    }
    #endregion

    #region btnOk_Click
    /// <summary>
    /// Guarda en los registros de windows las impresoras seleccionadas
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      if (cmbPrinterInvitation.SelectedIndex == -1)
      {
        MessageBox.Show("Select one printer invitation please", "Select Printer", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }
      else if (cmbPrinterMeal.SelectedIndex == -1)
      {
        MessageBox.Show("Select one printer meal ticket please", "Select Printer", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      SaveRegistrySettings();
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      SystemCommands.CloseWindow(this);
    }
    #endregion

    #endregion

    #region Métodos privados

    #region SelectItemTab
    /// <summary>
    ///   Selecciona el tab requerido
    /// </summary>
    /// <history>
    ///   [vku] 10/Jun/2016 Created
    /// </history>
    private void SelectItemTab()
    {
      switch (_enumConfiguration)
      {
        case EnumConfiguration.Printer:
          tabControl.SelectedIndex = 0;
          Printer.Visibility = Visibility.Visible;
          break;
        case EnumConfiguration.ReportsPath:
          tabControl.SelectedIndex = 1;
          Reports.Visibility = Visibility.Visible;
          break;
        default:
         foreach (TabItem item in tabControl.Items)
          {
            item.Visibility = Visibility.Visible;
          }
          break;
      }
    }
    #endregion

    #region FillPrinters
    /// <summary>
    /// Llena los combos con las impresoras instaladas en la computadora
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void FillPrinters()
    {
      PrintServer server = new PrintServer();

      foreach (PrintQueue queue in server.GetPrintQueues())
      {
        cmbPrinterInvitation.Items.Add(queue.FullName);
        cmbPrinterMeal.Items.Add(queue.FullName);
      }

    }
    #endregion

    #region SaveRegistrySettings
    /// <summary>
    /// Guarda en el registro de windows el nombre de la impresora para invitaciones y tickets de comida
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void SaveRegistrySettings()
    {
      try
      {
        RegistryKey configurationKey = CreateUrlConfiguration();

        if (configurationKey != null)
        {
          configurationKey.SetValue("PrintInvit", cmbPrinterInvitation.SelectedValue);
          configurationKey.SetValue("PrintMealTicket", cmbPrinterMeal.SelectedValue);
          UIHelper.ShowMessage("Printer's was successfully saved", MessageBoxImage.Information, "System Configuration");
          SystemCommands.CloseWindow(this);
        }

      }
      catch (Exception ex)
      {
        string message = String.Format("Can not save the configuration printer\n {0}", ex.Message);
        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }
    }
    #endregion

    #region  CreateUrlConfiguration
    /// <summary>
    ///   Crea la ruta donde se guardaran los valores de configuración del sistema
    /// </summary>
    /// <returns>Objeto que contiene los valores de configuración</returns>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private RegistryKey CreateUrlConfiguration()
    {
      RegistryKey softwareKey = Registry.CurrentUser.OpenSubKey("Software", true); //obtenemos la ruta de la carpeta Software en el registro de windows
      RegistryKey imKey;
      RegistryKey configurationKey;
      try
      {
        //Si no existe la carpeta VB and VBA Program Settings dentro de Software la creamos, sino obtenemos su ruta
        if (softwareKey.OpenSubKey("Intelligence Marketing") == null)
        {
          imKey = softwareKey.CreateSubKey("Intelligence Marketing", RegistryKeyPermissionCheck.ReadWriteSubTree);
        }
        else
        {
          imKey = softwareKey.OpenSubKey("Intelligence Marketing", true);
        }

        if (imKey.OpenSubKey("Configuration") == null)
        {
          configurationKey = imKey.CreateSubKey("Configuration", RegistryKeyPermissionCheck.ReadWriteSubTree);
        }
        else
        {
          configurationKey = imKey.OpenSubKey("Configuration", true);
        }

        return configurationKey;
      }
      catch (Exception)
      {
        return null;
      }
    }
    #endregion

    #region SelectedPrinterLoaded
    /// <summary>
    /// Selecciona el valor almacenado en el registro de windows de cada impresora
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// [edgrodriguez] 16/Jul/2016 Modified. Se optimizo el codigo.
    /// </history>
    private void SelectedPrinterLoaded()
    {
      RegistryKey configuration = ConfigRegistryHelper.GetUrlConfigRegistry();
      cmbPrinterInvitation.SelectedValue = ConfigRegistryHelper.GetConfiguredPrinter("PrintInvit");
      cmbPrinterMeal.SelectedValue = ConfigRegistryHelper.GetConfiguredPrinter("PrintMealTicket");
    }
    #endregion

    #region LoadReportsPath
    /// <summary>
    ///  Carga la ruta para los reportes desde el registro de windows
    /// </summary>
    /// <history>
    ///   [vku] 06/Jun/2016 Created
    /// </history>
    private void LoadReportsPath()
    {
      FileName = ConfigRegistryHelper.GetReportsPath();
     }
    #endregion

    #region SavePath
    /// <summary>
    ///   Guarda la ruta para los reportes en el registro de windows
    /// </summary>
    /// <history>
    ///   [vku] 07/Jun/2016 Created
    /// </history>
    private void SavePath()
    {
      try
      {
        RegistryKey configurationKey = CreateUrlConfiguration();
        if (configurationKey != null)
        {
          configurationKey.SetValue("ReportsPath", FileName);
          UIHelper.ShowMessage("path successfully saved", MessageBoxImage.Information, "System Configuration");
          DialogResult = true;
          Close();
        }
      }
      catch (System.Security.SecurityException)
      {
        UIHelper.ShowMessage("The user doesn't have the required permissions", MessageBoxImage.Error, "System Configuration");
      }
    }
    #endregion

    #endregion

    #region Eventos

    #region btnBrowser_Click
    /// <summary>
    ///   Abre el browser Folder
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 06/Jun/2016 Created
    ///   [vku] 17/Jun/2016 Modified. Ahora verifica que exista el directorio por default (P:\\)
    /// </history>
    private void btnBrowser_Click(object sender, RoutedEventArgs e)
    {
      System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
      if (!Directory.Exists(defaultSelectedPath)){ defaultSelectedPath = "M:\\"; } 
      dialog.SelectedPath = defaultSelectedPath;
      System.Windows.Forms.DialogResult result = dialog.ShowDialog();
      if (result == System.Windows.Forms.DialogResult.OK)
        FileName = dialog.SelectedPath;
    }
    #endregion

    public event EventHandler FileNameChanged;

    #region txtPath_TextChanged
    /// <summary>
    ///   Ruta para los reportes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 06/Jun/2016 Created
    /// </history>
    private void txtPath_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
      e.Handled = true;
      if (FileNameChanged != null)
      {
        FileNameChanged(this, EventArgs.Empty);
      }
    }
    #endregion

    #region btnSaveChanges_Click
    /// <summary>
    ///  Guarda la ruta seleccionada en el registro de windows
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 06/Jun/2016 Created
    /// </history>
    private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
    {
      if (txtPath.Text != string.Empty)
      {
        SavePath();
      }
      else
      {
        UIHelper.ShowMessage("Select Path", MessageBoxImage.Exclamation, "System Configuration");
      }
    }
    #endregion

    #region btnCancelPath_Click
    /// <summary>
    ///   Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 06/Jun/2016 Created
    /// </history>
    private void btnCancelPath_Click(object sender, RoutedEventArgs e)
    {
      SystemCommands.CloseWindow(this);
    }
    #endregion

    #endregion
  }
}

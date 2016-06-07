using Microsoft.Win32;
using System;
using System.Printing;
using System.Windows;
using System.IO;
using IM.Base.Helpers;

namespace IM.Launcher.Forms
{
  /// <summary>
  /// Módulo para configurar opciones de sistema
  /// </summary>
  /// <history>
  /// [lchairez] 05/Feb/2016 Created
  /// [vku] 06/Jun/2016 Modified. Renombre el formulario a SystemCfg
  /// </history>

  public partial class SystemCfg : Window
  {

    #region Atributos
    IniFileHelper _iniFileHelper;
    public string FileName
    {
      get { return txtPath.Text; }
      set { txtPath.Text = value; }
    }
    #endregion

    #region Constructor
    public SystemCfg()
    {
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
      InitializeComponent();
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

    #region GetUrlPrinterRegistry
    /// <summary>
    /// Regresa la ruta donde se guardan los valores de las impresoras
    /// </summary>
    /// <returns>Objeto que contiene los valores de las impresoras</returns>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private RegistryKey GetUrlPrinterRegistry()
    {
      RegistryKey softwareKey = Registry.CurrentUser.OpenSubKey("Software", true); //obtenemos la ruta de la carpeta Software en el registro de windows
      RegistryKey imKey;
      RegistryKey configurationKey;
      try
      {
        //Si no existe la carpeta VB and VBA Program Settings dentro de Software la creamos, sino obtenemos su ruta
        if (softwareKey.OpenSubKey("Intelligence Marketing") == null)
        {
          return null;
        }
        else
        {
          imKey = softwareKey.OpenSubKey("Intelligence Marketing", true);
        }

        if (imKey.OpenSubKey("Configuration") == null)
        {
          return null;
        }
        else
        {
          configurationKey = imKey.OpenSubKey("Configuration", true);
        }

        return configurationKey;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return null;
      }
    }
    #endregion

    #region  CreateUrlConfiguration
    /// <summary>
    /// Crea la ruta donde se guardaran los valores de las impresoras
    /// </summary>
    /// <returns>Objeto que contiene los valores de las impresoras</returns>
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
    /// </history>
    private void SelectedPrinterLoaded()
    {
      RegistryKey configuration = GetUrlPrinterRegistry();

      if (configuration == null)
        return;

      string printInvit = configuration.GetValue("PrintInvit").ToString();
      string printMealTicket = configuration.GetValue("PrintMealTicket").ToString();

      cmbPrinterInvitation.SelectedValue = printInvit;
      cmbPrinterMeal.SelectedValue = printMealTicket;
    }
    #endregion

    #region LoadReportsPath
    /// <summary>
    ///  Carga la ruta de guardado para los reportes desde el archivo de configuración
    /// </summary>
    /// <history>
    ///   [vku] 06/Jun/2016 Created
    /// </history>
    private void LoadReportsPath()
    {
      var strArchivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (!File.Exists(strArchivo)) return;
      _iniFileHelper = new IniFileHelper(strArchivo);
      FileName = _iniFileHelper.readText("SavePath", "ReportsPath", "");
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
    /// </history>
    private void btnBrowser_Click(object sender, RoutedEventArgs e)
    {
      System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
      dialog.SelectedPath = "M:\\";
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

    #region btnOkPath_Click
    /// <summary>
    ///  Guarda la ruta para los reportes en el archivo de configuración
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 06/Jun/2016 Created
    /// </history>
    private void btnOkPath_Click(object sender, RoutedEventArgs e)
    {
      var strArchivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (!File.Exists(strArchivo)) return;
      _iniFileHelper = new IniFileHelper(strArchivo);
      _iniFileHelper.writeText("SavePath", "ReportsPath", FileName); 
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

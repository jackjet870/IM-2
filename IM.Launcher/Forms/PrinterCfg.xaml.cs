using Microsoft.Win32;
using System;
using System.Printing;
using System.Windows;

namespace IM.Launcher.Forms
{
  /// <summary>
  /// Módulo para configurar las impresoras
  /// </summary>
  /// <history>
  /// [lchairez] 05/Feb/2016 Created
  /// </history>

  public partial class PrinterCfg : Window
  {
    public PrinterCfg()
    {
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
      InitializeComponent();
    }

    #region Métodos de la forma

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
    }

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

    #region Métodos privados

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
  }
}

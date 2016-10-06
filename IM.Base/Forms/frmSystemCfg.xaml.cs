using Microsoft.Win32;
using System;
using System.Printing;
using System.Windows;
using IM.Base.Helpers;

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

    #endregion

    #region Constructor
    public frmSystemCfg()
    {
      WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
        UIHelper.ShowMessage("Select one printer invitation please");
        return;
      }
      if (cmbPrinterMeal.SelectedIndex == -1)
      {
        UIHelper.ShowMessage("Select one printer meal ticket please");
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
          UIHelper.ShowMessage("Printer's was successfully saved", MessageBoxImage.Information, "System Configuration");
          SystemCommands.CloseWindow(this);
        }

      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
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
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
      return null;
    }
    #endregion

    #region SelectedPrinterLoaded
    /// <summary>
    /// Selecciona el valor almacenado en el registro de windows de cada impresora
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// [edgrodriguez] 16/Jul/2016 Modified. Se optimizo el codigo.
    /// [edgrodriguez] 26/Jul/2016 Modified. Se corrigio un error de validacion.
    /// </history>
    private void SelectedPrinterLoaded()
    {
      cmbPrinterInvitation.SelectedValue = ConfigRegistryHelper.GetConfiguredPrinter("PrintInvit");
      cmbPrinterMeal.SelectedValue = ConfigRegistryHelper.GetConfiguredPrinter("PrintMealTicket");
    }
    #endregion
    
    #endregion

   
  }
}

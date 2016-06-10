using System;
using Microsoft.Win32;
using System.Windows;

namespace IM.Base.Helpers
{
  public class ConfigRegistry
  {

    #region GetUrlConfigRegistry
    /// <summary>
    ///   Regresa la ruta donde se guardan los valores de configuración
    /// </summary>
    /// <returns>Objeto que contiene los valores de configuración</returns>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// [vku] 07/Jun/2016 Modified. Renombre el metodo a GetUrlConfigRegistry (antes GetUrlPrinterRegistry)
    /// </history>
    public static RegistryKey GetUrlConfigRegistry()
    {
      RegistryKey softwareKey = Registry.CurrentUser.OpenSubKey("Software", true); //obtenemos la ruta de la carpeta Software en el registro de windows
      RegistryKey imKey;
      RegistryKey configurationKey;
      try
      {
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

    #region ExistReportsPath
    /// <summary>
    ///  Verifica si esta configurado la ruta de reportes
    /// </summary>
    /// <history>
    ///   [vku] 10/Jun/2016 Created
    /// </history>
    public static bool ExistReportsPath()
    {
      if (ConfigRegistry.GetUrlConfigRegistry() == null)
      {
        return false;
      }
      else
      {
        if (ConfigRegistry.GetUrlConfigRegistry().GetValue("ReportsPath") == null)
        {
          return false;
        }
        else
        {
          return true;
        }
      }
    }
    #endregion
  }
}

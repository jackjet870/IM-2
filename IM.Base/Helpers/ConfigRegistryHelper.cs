using System;
using System.IO;
using Microsoft.Win32;
using System.Windows;

namespace IM.Base.Helpers
{
  public static class ConfigRegistryHelper
  {
    private const string ReportsPath = "ReportsPath";

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
    ///   [vku]       10/Jun/2016 Created
    ///   [aalcocer]  16/06/2016 Modified. Se valida si la el directorio de la ruta de reportes existe 
    /// </history>
    public static bool ExistReportsPath()
    {
      bool exist = false;
      RegistryKey configuration = GetUrlConfigRegistry();
      string reportsPath = (string) configuration?.GetValue(ReportsPath);
      if (reportsPath != null)
        exist = Directory.Exists(reportsPath);
      return exist;
    }

    #endregion
            
    #region GetReportsPath
    /// <summary>
    /// Obtiene la ruta de reportes
    /// </summary>
    /// <returns>string || NULL si la ruta no existe</returns>
    /// <history>
    ///   [aalcocer] 11/Jun/2016 Created
    /// </history>
    public static string GetReportsPath()
    {
      string _reportsPath = null;
      if (ExistReportsPath())
      {
        _reportsPath = (string)GetUrlConfigRegistry().GetValue(ReportsPath);
      }
      return _reportsPath;
    }
    #endregion

    #region GetConfiguredPrinter
    /// <summary>
    /// Obtiene la impresora configurada.
    /// </summary>
    /// <returns>string || NULL si la ruta no existe</returns>
    /// <history>
    ///   [edgrodriguez] 16/Jul/2016 Created
    ///   [edgrodriguez] 26/Jul/2016 Modified. Se corrigió la validacion del método, ya que marcaba error cuando era nulo.
    /// </history>
    public static string GetConfiguredPrinter(string Printer)
    {
      return GetUrlConfigRegistry()?.GetValue(Printer)?.ToString();
    }
    #endregion
  }
}
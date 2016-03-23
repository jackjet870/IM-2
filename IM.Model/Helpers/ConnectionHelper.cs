using System.Data.Entity.Core.EntityClient;
using PalaceResorts.Common.PalaceTools;
using PalaceResorts.Common.PalaceTools.AppConfig;

namespace IM.Model.Helpers
{
  /// <summary>
  /// Clase para el manejo de la cadena de conexion
  /// </summary>
  public class ConnectionHelper
  {
    #region Atributos

    private static string _ConnectionString;

    #endregion

    #region Propiedades

    #region ConnectionKey

    /// <summary>
    /// Clave de la cadena de conexion en el servicio web de App Config Manager
    /// </summary>
    /// <history>
    /// [wtorres]  22/Mar/2016 Created
    /// </history>
    private static string ConnectionKey
    {
      get
      {
        return ConfigHelper.GetString("IntelligenceMarketing.ConnectionKey");
      }
    }

    #endregion

    #region ConnectionString

    /// <summary>
    /// Cadena de conexion
    /// </summary>
    /// <history>
    /// [wtorres]  22/Mar/2016 Created
    /// </history>
    public static string ConnectionString
    {
      get
      {
        if (string.IsNullOrEmpty(_ConnectionString))
        {
          var builder = new EntityConnectionStringBuilder();
          builder.Provider = "System.Data.SqlClient";
          builder.ProviderConnectionString = AppConfigHelper.GetSettingByKey(ConnectionKey);
          builder.Metadata = "res://*/IMModel.csdl|res://*/IMModel.ssdl|res://*/IMModel.msl";
          _ConnectionString = builder.ToString();
        }
        return _ConnectionString;
      }
    } 
    #endregion

    #endregion
  }
}

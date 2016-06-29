using System.Data.Entity.Core.EntityClient;
using PalaceResorts.Common.PalaceTools;
using PalaceResorts.Common.PalaceTools.AppConfig;
using System.Data.SqlClient;
using IM.Model.Enums;

namespace IM.Model.Helpers
{
  /// <summary>
  /// Clase para el manejo de la cadena de conexion
  /// </summary>
  /// <history>
  /// [wtorres]  23/Mar/2016 Created
  /// </history>
  public class ConnectionHelper
  {
    #region Atributos

    private static string _connectionString;
    private static string _sqlConnectionString;

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
    /// Cadena de conexion de Sql Server
    /// </summary>
    /// <history>
    /// [wtorres]  14/Abr/2016 Created
    /// </history>
    private static string SqlConnectionString
    {
      get
      {
        if (string.IsNullOrEmpty(_sqlConnectionString))
        {
          _sqlConnectionString = AppConfigHelper.GetSettingByKey(ConnectionKey);
        }
        return _sqlConnectionString;
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
        if (string.IsNullOrEmpty(_connectionString))
        {
          EntityConnectionStringBuilder builder = new EntityConnectionStringBuilder();
          builder.Provider = "System.Data.SqlClient";
          builder.ProviderConnectionString = SqlConnectionString;
          builder.Metadata = "res://*/IMModel.csdl|res://*/IMModel.ssdl|res://*/IMModel.msl";
          _connectionString = builder.ToString();
        }
        return _connectionString;
      }
    }
    #endregion

    #region ServerName

    /// <summary>
    /// Nombre del servidor
    /// </summary>
    /// <history>
    /// [wtorres]  12/Abr/2016 Created
    /// </history>
    public static string ServerName
    {
      get
      {
        SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
        sqlBuilder.ConnectionString = SqlConnectionString;

        return sqlBuilder.DataSource;
      }
    }
    #endregion

    #region DatabaseName

    /// <summary>
    /// Nombre de la base de datos
    /// </summary>
    /// <history>
    /// [wtorres]  12/Abr/2016 Created
    /// </history>
    public static string DatabaseName
    {
      get
      {
        SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
        sqlBuilder.ConnectionString = SqlConnectionString;

        return sqlBuilder.InitialCatalog;
      }
    }
    #endregion

    #endregion
  }
}

using System.Data.Entity.Core.EntityClient;
using PalaceResorts.Common.PalaceTools;
using PalaceResorts.Common.PalaceTools.AppConfig;
using System.Data.SqlClient;
using IM.Model.Enums;
using System;

namespace IM.Model.Helpers
{
  /// <summary>
  /// Clase para el manejo de la cadena de conexion
  /// </summary>
  /// <history>
  /// [wtorres]  23/Mar/2016 Created
  /// [wtorres]  30/Jun/2016 Modified. Ahora soporta cadenas de conexion a las bases de datos de Asistencia y Clubes
  /// </history>
  public class ConnectionHelper
  {
    #region Atributos

    private static int _databasesCount = Enum.GetNames(typeof(EnumDatabase)).Length;
    private static string[] _connectionString = new string[_databasesCount];
    private static string[] _sqlConnectionString = new string[_databasesCount];

    #endregion

    #region Propiedades

    #region ConnectionKey

    /// <summary>
    /// Clave de la cadena de conexion en el servicio web de App Config Manager
    /// </summary>
    /// <param name="database">Base de datos</param>
    /// <history>
    /// [wtorres]  22/Mar/2016 Created
    /// [wtorres]  30/Jun/2016 Modified. Agregue el parametro database
    /// </history>
    private static string ConnectionKey(EnumDatabase database)
    {
      switch (database)
      {
        case EnumDatabase.Asistencia:
          return ConfigHelper.GetString("Asistencia.ConnectionKey");
        case EnumDatabase.IntelligenceContracts:
          return ConfigHelper.GetString("IntelligenceContracts.ConnectionKey");
        default:
          return ConfigHelper.GetString("IntelligenceMarketing.ConnectionKey");
      }
    }

    #endregion

    #region SqlConnectionString

    /// <summary>
    /// Cadena de conexion de Sql Server
    /// </summary>
    /// <param name="database">Base de datos</param>
    /// <history>
    /// [wtorres]  14/Abr/2016 Created
    /// [wtorres]  30/Jun/2016 Modified. Agregue el parametro database
    /// </history>
    private static string SqlConnectionString(EnumDatabase database)
    {
      if (string.IsNullOrEmpty(_sqlConnectionString[(int)database]))
      {
        _sqlConnectionString[(int)database] = AppConfigHelper.GetSettingByKey(ConnectionKey(database));
      }
      return _sqlConnectionString[(int)database];
    }
    #endregion

    #region ConnectionString
    /// <summary>
    /// Cadena de conexion
    /// </summary>
    /// <param name="database">Base de datos</param>
    /// <history>
    /// [wtorres]  22/Mar/2016 Created
    /// [wtorres]  30/Jun/2016 Modified. Agregue el parametro database
    /// </history>
    public static string ConnectionString(EnumDatabase database = EnumDatabase.IntelligentMarketing)
    {
      if (string.IsNullOrEmpty(_connectionString[(int)database]))
      {
        EntityConnectionStringBuilder builder = new EntityConnectionStringBuilder();
        builder.Provider = "System.Data.SqlClient";
        builder.ProviderConnectionString = SqlConnectionString(database);
        string databaseModel = EnumToListHelper.GetEnumDescription(database);
        builder.Metadata = $"res://*/{databaseModel}.csdl|res://*/{databaseModel}.ssdl|res://*/{databaseModel}.msl";
        _connectionString[(int)database] = builder.ToString();
      }
      return _connectionString[(int)database];
    }
    #endregion

    #region ServerName

    /// <summary>
    /// Nombre del servidor
    /// </summary>
    /// <param name="database">Base de datos</param>
    /// <history>
    /// [wtorres]  12/Abr/2016 Created
    /// [wtorres]  30/Jun/2016 Modified. Agregue el parametro database
    /// </history>
    public static string ServerName(EnumDatabase database)
    {
      SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
      sqlBuilder.ConnectionString = SqlConnectionString(database);

      return sqlBuilder.DataSource;
    }
    #endregion

    #region DatabaseName

    /// <summary>
    /// Nombre de la base de datos
    /// </summary>
    /// <param name="database">Base de datos</param>
    /// <history>
    /// [wtorres]  12/Abr/2016 Created
    /// [wtorres]  30/Jun/2016 Modified. Agregue el parametro database
    /// </history>
    public static string DatabaseName(EnumDatabase database)
    {
      SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
      sqlBuilder.ConnectionString = SqlConnectionString(database);

      return sqlBuilder.InitialCatalog;
    }
    #endregion

    #endregion
  }
}

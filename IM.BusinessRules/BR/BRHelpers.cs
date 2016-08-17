using System;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Text.RegularExpressions;
using IM.Model.Enums;
using System.Data.Entity;
using IM.Model.Classes;

namespace IM.BusinessRules.BR
{
  public class BRHelpers
  {
    #region GetServerDate

    /// <summary>
    /// Obtiene la fecha del servidor.
    /// </summary>
    /// <history>
    /// [wtorres]   06/Jul/2016 Created
    /// </history>
    public static DateTime GetServerDate()
    {
      return GetServerDateTime().Date;
    }

    #endregion

    #region GetServerDateTime

    /// <summary>
    /// Obtiene la fecha y hora del servidor.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 29/Feb/2016 Created
    /// [wtorres]      06/Jul/2016 Modified. Renombrado. Antes se llamaba GetServerDate
    /// </history>
    public static DateTime GetServerDateTime()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        var dQuery = dbContext.Database.SqlQuery<DateTime>("SELECT GETDATE()");
        return dQuery.AsEnumerable().First();
      }
    }

    #endregion

    #region GetServerInformation
    /// <summary>
    /// Obtiene la informacion del servidor 
    /// </summary>
    /// <returns>List<string>| string[0]= Nombre del servidor| string[1]= Nombre de la Base de datos</returns>
    /// <history>
    /// [erosado] 06/06/2016  Created
    /// </history>
    public async static Task<List<string>> GetServerInformation()
    {
      List<string> result = new List<string>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          string ServerName = dbContext.Database.Connection.DataSource;
          string dbName = dbContext.Database.Connection.Database;
          result.Add(ServerName);
          result.Add(dbName);
        }
      });
      return result;
    }

    #endregion

    #region ValidateChangedByExist

    /// <summary>
    /// 
    /// </summary>
    /// <param name="changedBy"></param>
    /// <param name="password"></param>
    /// <param name="placeType"></param>
    /// <param name="placeID"></param>
    /// <param name="userType"></param>
    /// <param name="pR"></param>
    /// <returns></returns>
    /// <history>
    /// [jorcanche]  29/Mar/2016 Created
    /// </history>

    public static List<ValidationData> ValidateChangedByExist(string ptxtChangedBy, string ptxtPwd, string pstrLeadSource, string pstrUserType = "Changed By", string ptxtPR = "")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_ValidateChangedBy(ptxtChangedBy, ptxtPwd, "LS", pstrLeadSource, pstrUserType, ptxtPR).ToList();
      }
    }
    #endregion

    #region GetFieldsByTable
    /// <summary>
    /// Obtiene las propiedades de una tabla a partir de una entidad
    /// </summary>    
    /// <param name="entity">Entidad para buscar su tabla</param>
    /// <param name="database">enumerado para saber a que base de datos se va a conectar</param>
    /// <returns>Lista de tipo ColumnDefinition con todos los datos</returns>
    /// <history>
    /// [emoguel] created 05/08/2016
    /// </history>
    public static List<ColumnDefinition> GetFieldsByTable<T>(T entity, EnumDatabase database) where T : class
    {
      DbContext dbContext = null;
      #region dbContext
      switch (database)
      {
        case EnumDatabase.IntelligentMarketing:
          {
            dbContext = new IMEntities(ConnectionHelper.ConnectionString());
            break;
          }
        case EnumDatabase.Asistencia:
          {
            dbContext = new AsistenciaEntities(ConnectionHelper.ConnectionString(database));
            break;
          }

        case EnumDatabase.IntelligenceContracts:
          {
            dbContext = new ICEntities(ConnectionHelper.ConnectionString(database));
            break;
          }
      }
      #endregion

      using (dbContext)
      {
        string strTableName = GetTableName(entity, dbContext);
        string strDatabaseName = dbContext.Database.Connection.Database;
        #region Query  
        string strQuery = "Use " + strDatabaseName + @"  SELECT
                 C.name as [Column],
                 T.name as [Type],
                 C.Precision,
                 C.Scale,
                 CASE WHEN CE.NUMERIC_PRECISION IS NOT NULL THEN CE.NUMERIC_PRECISION + (CASE WHEN CE.NUMERIC_SCALE > 0 THEN 1 ELSE 0 END)
                       ELSE IsNull(CE.CHARACTER_MAXIMUM_LENGTH, C.precision) END as MaxLength,
                 CE.COLUMN_DEFAULT as [Default Value],
                 CE.IS_NULLABLE as Nullable,
                 IsNull(P.value, '') as Description
          FROM sys.tables as TA
                 LEFT JOIN sysusers S on TA.schema_id = S.uid
                 INNER JOIN sys.columns as C on C.object_id = TA.object_id
                 INNER JOIN sys.types as T on T.system_type_id = C.system_type_id and T.name <> 'sysname'
                 LEFT JOIN INFORMATION_SCHEMA.COLUMNS CE ON CE.TABLE_SCHEMA = S.name AND CE.TABLE_NAME = TA.name AND CE.COLUMN_NAME = C.name
                 LEFT JOIN sys.extended_properties as P on P.major_id = C.object_id and P.minor_id = C.column_id and P.class = 1
          WHERE TA.name = '" + strTableName + @"'
          ORDER BY S.name, TA.name, C.column_id
          ";
        #endregion
        var dQuery = dbContext.Database.SqlQuery<ColumnDefinition>(strQuery);
        return dQuery.ToList();
      }
    }
    #endregion

    #region GetTableName
    /// <summary>
    /// Obtiene el nombre de la tabla de una entidad
    /// </summary>
    /// <param name="entity">entidad a buscar el nombre</param>
    /// <param name="dbContext">dbcontext de la entidad</param>
    /// <returns>String con el nombre de la tabla</returns>
    /// <history>
    /// [emoguel] created 05/08/2016
    /// </history>
    public static string GetTableName<T>(T entity, DbContext dbContext) where T : class
    {
      var objContext = ((IObjectContextAdapter)dbContext).ObjectContext;
      var sql = objContext.CreateObjectSet<T>().ToTraceString();
      Regex regex = new Regex("FROM (?<table>.*) AS");
      Match match = regex.Match(sql);

      string table = match.Groups["table"].Value;
      var aTable = table.Split('.');
      string tableName = aTable[1].ToString().TrimEnd(']').TrimStart('[');
      return tableName;
    } 
    #endregion

  }
}
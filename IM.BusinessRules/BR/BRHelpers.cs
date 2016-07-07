﻿using System;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

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
  }
}

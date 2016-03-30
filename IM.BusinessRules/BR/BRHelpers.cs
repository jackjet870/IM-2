using System;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Collections.Generic;


namespace IM.BusinessRules.BR
{
  public class BRHelpers
  {
    #region GetServerDate

    /// <summary>
    /// Obtiene la fecha y hora del servidor.
    /// </summary>
    /// <returns>DateTime</returns>
    /// <history>
    /// [edgrodriguez] 29/Feb/2016 Created
    /// </history>
    public static DateTime GetServerDate()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var dQuery = dbContext.Database.SqlQuery<DateTime>("SELECT GETDATE()");
        return dQuery.AsEnumerable().First();
      }
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

    public static List<ValidationData>  ValidateChangedByExist(string ptxtChangedBy, string ptxtPwd, string pstrLeadSource, string pstrUserType = "Changed By", string ptxtPR = "")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_ValidateChangedBy(ptxtChangedBy, ptxtPwd, "LS", pstrLeadSource, pstrUserType, ptxtPR).ToList();              
      }
    }
    #endregion

  }
}

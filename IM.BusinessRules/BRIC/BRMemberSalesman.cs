using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;

namespace IM.BusinessRules.BRIC
{
  /// <summary>
  /// Clase para el manejor de los Salesman de la base Hotel(Intelligence Contracts)
  /// </summary>
  /// <history>
  /// [jorcanche]  created 08/07/2016
  /// </history>
  public class BRMemberSalesman
  {
    #region GetMemberSalesmen

    /// <summary>
    /// Devuelve la lista MemberSalesmen
    /// </summary>    
    /// <returns>Lista de tipo MemberSalesmen</returns>
    /// <history>
    /// [jorcanche] created 08/07/2016
    /// </history>
    public static async Task<List<MemberSalesmen>> GetMemberSalesmen(string application, string job, string code , string roles)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new ICEntities(ConnectionHelper.ConnectionString(EnumDatabase.IntelligenceContracts)))
        {
          return dbContext.USP_CL_GetMemberSalesmen(application, job, code, roles).ToList();
        }
      });
    }

    #endregion

    #region SaveMemberSalesman

    /// <summary>
    /// Obtiene el resultado en tipo de  Salesman cuando se guarda est
    /// </summary>
    /// <param name="rEcnum"></param>
    /// <param name="memberSalesmen"></param>
    /// <param name="user"></param>
    /// <history>
    /// [jorcanche]  created 08/07/2016
    /// </history>        
    public static async Task<MemberSalesmen> SaveMemberSalesman(decimal? rEcnum, MemberSalesmen memberSalesmen, string user)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new ICEntities(ConnectionHelper.ConnectionString(EnumDatabase.IntelligenceContracts)))
        {
          return dbContext.USP_CL_SaveMemberSalesman(
            rEcnum, memberSalesmen.CLMEMOPC_ID, memberSalesmen.APPLICATION, memberSalesmen.CLAOPC_ID, memberSalesmen.OPC1,
            memberSalesmen.OPC_PCT,memberSalesmen.OPC_PCT2, memberSalesmen.OPC_CPT3, memberSalesmen.OPC_PCT4, memberSalesmen.STATUS,
            memberSalesmen.ZONA, memberSalesmen.OPC, user)
            .FirstOrDefault();
        }
      });
    }
    #endregion

    #region DeleteMemberSalesman
    /// <summary>
    /// Elimina MemberSalesmen
    /// </summary>        
    /// <history>
    /// [jorcanche] created 08/07/2016
    /// </history>
    public static async Task<int> DeleteMemberSalesman(decimal? cLmemopcId)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new ICEntities(ConnectionHelper.ConnectionString(EnumDatabase.IntelligenceContracts)))
        {
          return dbContext.USP_CL_DeleteMemberSalesman(cLmemopcId);
        }
      });
    }
    #endregion

    #region ExistsSalesman
    /// <summary>
    /// Indica si existe un Salesman
    /// </summary>
    /// <param name="zOna"></param>
    /// <param name="cOde"></param>
    /// <history>
    /// [jorcanche] created 08/07/2016
    /// </history>
    public static async Task<bool> ExistsSalesman(string zOna, string cOde)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new ICEntities(ConnectionHelper.ConnectionString(EnumDatabase.IntelligenceContracts)))
        {
         return  dbContext.USP_CL_ExistsSalesman(zOna, cOde).FirstOrDefault() == 1;         
        }
      });
    } 
    #endregion
  }
}

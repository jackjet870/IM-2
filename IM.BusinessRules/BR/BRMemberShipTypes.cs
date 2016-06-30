using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRMemberShipTypes
  {
    #region GetMemberShipTypes
    /// <summary>
    /// Obtiene registros del catalogo MembershipTypes
    /// </summary>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros Inactivos | 1. Registros Activos</param>
    /// <param name="memberShipType">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo MembershipType</returns>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    public async static Task<List<MembershipType>> GetMemberShipTypes(int nStatus = -1, MembershipType memberShipType = null)
    {
      List<MembershipType> lstMemberShipType = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from mt in dbContext.MembershipTypes
                      select mt;
          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(mt => mt.mtA == blnStatus);
          }

          #region Filtros adicionales
          if (memberShipType != null)//validamos que se tenga un objeto
          {
            if (!string.IsNullOrWhiteSpace(memberShipType.mtID))//filtro por id
            {
              query = query.Where(mt => mt.mtID == memberShipType.mtID);
            }

            if (!string.IsNullOrWhiteSpace(memberShipType.mtN))//Filtro por 
            {
              query = query.Where(mt => mt.mtN.Contains(memberShipType.mtN));
            }

            if (!string.IsNullOrWhiteSpace(memberShipType.mtGroup))//Filtro por grupo
            {
              query = query.Where(mt => mt.mtGroup == memberShipType.mtGroup);
            }
          }
          #endregion
          lstMemberShipType= query.OrderBy(mt => mt.mtN).ToList();      
        }
      });
      return lstMemberShipType;
    }
    #endregion

    #region GetMemberShipTypes

    /// <summary>
    /// Obtiene el mtLevel por id de MembershipTypes
    /// </summary>
    /// <param name="mtId">Id de MembershipTypes</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros Inactivos | 1. Registros Activos</param>
    /// <returns>Lista de tipo MembershipType</returns>
    /// <history>
    /// [jorcanche] created 04/04/2016
    /// </history>
    public static async Task<byte?> GetLevelOfMemberShipTypes(string mtId ,int nStatus = -1 )
    {
      byte?  level = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var status = Convert.ToBoolean(nStatus);
          level = (from mt in dbContext.MembershipTypes
                      where mt.mtID == mtId && (nStatus == -1 || mt.mtA == status)
                      select mt.mtLevel).FirstOrDefault();       
        }
      });
      return level;
    }//select Count(*) from Sales where sagu = 7752186

    #endregion
  }
}

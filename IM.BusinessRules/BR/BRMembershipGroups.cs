using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRMembershipGroups
  {
    #region GetMembershipGroup
    /// <summary>
    /// Obtiene registros del catalogo MembershipGroups
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="membershipGroup">Objeto con filtros especiales</param>
    /// <returns> lista de tipo MembershipGroup</returns>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    public async static Task<List<MembershipGroup>> GetMembershipGroups(int nStatus = -1, MembershipGroup membershipGroup = null)
    {
      List<MembershipGroup> lstMembershipGroup = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from mg in dbContext.MembershipsGroups
                      select mg;

          if (nStatus != -1)//filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(mg => mg.mgA == blnStatus);
          }

          #region Filtros adicionales
          if (membershipGroup != null)
          {
            if (!string.IsNullOrWhiteSpace(membershipGroup.mgID))//Filtro por ID
            {
              query = query.Where(mg => mg.mgID == membershipGroup.mgID);
            }

            if (!string.IsNullOrWhiteSpace(membershipGroup.mgN))//Filtro por descripción
            {
              query = query.Where(mg => mg.mgN.Contains(membershipGroup.mgN));
            }
          }
          #endregion

          lstMembershipGroup= query.OrderBy(mg => mg.mgN).ToList();
        }
      });
      return lstMembershipGroup;
    }
    #endregion

    #region SaveMembershipGroup
    /// <summary>
    /// Agrega|Actualiza un membershipGroup
    /// Asigna|Desasina una lista de membershiptype
    /// </summary>
    /// <param name="membershipGroup">Objeto a guardar</param>
    /// <param name="lstAdd">lista a asignar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>0. No se pudo guardar | >1. Se guardó correctamente | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 19/05/2016
    /// </history>
    public static int SaveMembershipGroup(MembershipGroup membershipGroup, List<MembershipType> lstAdd, bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        using (var transaccion = dbContext.Database.BeginTransaction(IsolationLevel.Serializable))
        {
          try
          {
            #region Update
            if (blnUpdate)
            {
              dbContext.Entry(membershipGroup).State = System.Data.Entity.EntityState.Modified;
            }
            #endregion
            #region Add
            else
            {
              if (dbContext.MembershipsGroups.Where(mg => mg.mgID == membershipGroup.mgID).FirstOrDefault() != null)
              {
                return -1;
              }
              else
              {
                dbContext.MembershipsGroups.Add(membershipGroup);
              }
            }
            #endregion

            #region membership a asignar
            dbContext.MembershipTypes.AsEnumerable().Where(mt => lstAdd.Any(mtt => mtt.mtID == mt.mtID)).ToList().ForEach(mt=>{
              mt.mtGroup = membershipGroup.mgID;
            });
            #endregion

            int nRes = 0;
            nRes = dbContext.SaveChanges();
            transaccion.Commit();
            return nRes;
          }
          catch
          {
            transaccion.Rollback();
            return 0;
          }
        }
      }
    } 
    #endregion
  }
}

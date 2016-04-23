using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

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
    public static List<MembershipGroup> GetMembershipGroup(int nStatus = -1, MembershipGroup membershipGroup = null)
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

        return query.OrderBy(mg => mg.mgN).ToList();
      }
    }
    #endregion

    #region SaveMembershipGroup
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo membershipGroup
    /// </summary>
    /// <param name="membershipGroup">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Insertar</param>
    /// <returns>0. No se pudo guardar | 1. se guardó | -1. existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    public static int SaveMembershipGroup(MembershipGroup membershipGroup,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)//Si es actualizar
        {
          dbContext.Entry(membershipGroup).State = System.Data.Entity.EntityState.Modified;
        } 
        #endregion
        #region Insert
        else//insertar
        {
          MembershipGroup membershipGroupVal = dbContext.MembershipsGroups.Where(mg => mg.mgID == membershipGroup.mgID).FirstOrDefault();
          if(membershipGroupVal!=null)//validamos si existe un objeto con el mismo ID
          {
            return -1;
          }
          else
          {
            dbContext.MembershipsGroups.Add(membershipGroup);
          }
        } 
        #endregion

        return dbContext.SaveChanges();
      }
    }

    #endregion
  }
}

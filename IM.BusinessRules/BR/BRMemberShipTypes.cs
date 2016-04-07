using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

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
    public static List<MembershipType> GetMemberShipTypes(int nStatus = -1, MembershipType memberShipType = null)
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
        return query.OrderBy(mt => mt.mtN).ToList();
      }
    }
    #endregion

    #region SaveMeberShipType
    /// <summary>
    /// Guarda|Actualiza un registro en el catalogo MembershipTypes
    /// </summary>
    /// <param name="memberShipType">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>0. No se guardó | 1. se guardó correctamente | 2. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    public static int SaveMemberShipType(MembershipType memberShipType,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)//si es actualizar
        {
          dbContext.Entry(memberShipType).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Insert
        else
        {
          MembershipType memberShipTypeVal = dbContext.MembershipTypes.Where(mt => mt.mtID == memberShipType.mtID).FirstOrDefault();
          if(memberShipTypeVal!=null)//Verificar que se tenga un objeto
          {
            return 2;
          }
          else//insertar
          {
            dbContext.MembershipTypes.Add(memberShipType);
          }
        }
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRRefundTypes
  {
    #region GetRefundTypes
    /// <summary>
    /// Obtiene registros del catalogo refundTypes
    /// </summary>
    /// <param name="nStatus">-1 Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="refundType">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo refund Type</returns>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    public static List<RefundType> GetRefundTypes(int nStatus = -1, RefundType refundType = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from rf in dbContext.RefundTypes
                    select rf;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(rf => rf.rfA == blnStatus);
        }

        if (refundType != null)//Validamos si tiene un objeto
        {
          if (!string.IsNullOrWhiteSpace(refundType.rfID))//Filtro por ID
          {
            query = query.Where(rf => rf.rfID == refundType.rfID);
          }

          if (!string.IsNullOrWhiteSpace(refundType.rfN))//Filtro por estatus
          {
            query=query.Where(rf => rf.rfN.Contains(refundType.rfN));
          }
        }

        return query.OrderBy(rf => rf.rfN).ToList();
      }
    }
    #endregion

    #region SaveRefundType
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo RefunType
    /// </summary>
    /// <param name="refundType">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza  | False. Inserta</param>
    /// <returns>0. No se guardó | 1. Se guardó | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    public static int SaveRefundType(RefundType refundType, bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)
        {
          dbContext.Entry(refundType).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Insert
        else
        {
          RefundType refundTypeVal = dbContext.RefundTypes.Where(rf => rf.rfID == refundType.rfID).FirstOrDefault();
          if (refundTypeVal != null)//Validamos que no exista un registro con el mismo ID
          {
            return -1;
          }
          else//insertar
          {
            dbContext.RefundTypes.Add(refundType);
          }
        }
        #endregion
        return dbContext.SaveChanges();
      }
    } 
    #endregion
  }
}

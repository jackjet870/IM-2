using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRPaymentTypes
  {
    #region GetPaymentTypes

    /// <summary>
    /// devuelve datos del catalogo paymentTypes
    /// </summary>
    /// <param name="nStatus"> -1. Todos | 0. registros inactivos | 1. registros activos</param>
    /// <returns>lista de tipo payment type</returns>
    /// <history>
    /// [lchairez] created 10/03/2016
    /// [emoguel] modified 06/04/2016 Se agregaron mas filtros
    /// </history>
    public static List<PaymentType> GetPaymentTypes(int status=-1,PaymentType paymentType=null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from pt in dbContext.PaymentTypes
                    select pt;
        if(status!=-1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(status);
          query = query.Where(pt => pt.ptA == blnStatus);
        }

        #region Filtros adicionales
        if (paymentType != null)
        {
          if(!string.IsNullOrWhiteSpace(paymentType.ptID))//filtro por ID
          {
            query = query.Where(pt => pt.ptID == paymentType.ptID);
          }

          if(!string.IsNullOrWhiteSpace(paymentType.ptN))//Filtro por descripcion
          {
            query = query.Where(pt => pt.ptN.Contains(paymentType.ptN));
          }
        } 
        #endregion

        return query.OrderBy(pt => pt.ptN).ToList();        
      }
    }
    #endregion

    #region GetPaymentTypeId
    /// <summary>
    /// Obtiene un tipo de pago en específico
    /// </summary>
    /// <param name="paymentTypeId">Identificador del pago</param>
    /// <returns>PaymentType</returns>
    /// <history>
    /// [lchairez] 10/03/2016 Created.
    /// </history>
    public static PaymentType GetPaymentTypeId(string paymentTypeId)
    {
      using(var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.PaymentTypes.Where(p => p.ptID == paymentTypeId).SingleOrDefault();
      }
    }
    #endregion

    #region SavePaymentType
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo PaymentTypes
    /// </summary>
    /// <param name="paymentType">Objeto a guardar</param>
    /// <param name="blnUpdtae">True. Actualiza | False. Agrega</param>
    /// <returns>0. No se guardó | 1. Se guardó | 2. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    public static int SavePaymentType(PaymentType paymentType,bool blnUpdtae)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdtae)
        {
          dbContext.Entry(paymentType).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Add
        else
        {
          PaymentType paymentTypeVal = dbContext.PaymentTypes.Where(pt => pt.ptID == paymentType.ptID).FirstOrDefault();
          if(paymentTypeVal!=null)//Validamos que no si existe un registro con el mismo ID
          {
            return 2;
          }
          else//agregar
          {
            dbContext.PaymentTypes.Add(paymentType);
          }
        } 
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}

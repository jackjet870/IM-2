using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRPaymentSchemas
  {
    #region GetpaymentSchemas
    /// <summary>
    /// Obtiene registros del catalogo paymentSchemas
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="paymentSchemas">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo PaymentSchemas</returns>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    public static List<PaymentSchema> GetPaymentSchemas(int nStatus = -1, PaymentSchema paymentSchemas = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from pas in dbContext.PaymentSchemas
                    select pas;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(pas => pas.pasA == blnStatus);
        }

        #region Filtros adicionales
        if (paymentSchemas != null)//Verificamos si tenemos el objeto
        {
          if (paymentSchemas.pasID > 0)//Filtro por ID
          {
            query = query.Where(pas => pas.pasID == paymentSchemas.pasID);
          }

          if (!string.IsNullOrWhiteSpace(paymentSchemas.pasN))//Filtro por descripcion
          {
            query = query.Where(pas => pas.pasN.Contains(paymentSchemas.pasN));
          }
        }
        #endregion
        return query.OrderBy(pas => pas.pasN).ToList();
      }
    }
    #endregion

    #region SavePaymentSchema
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo PaymentSchemas
    /// </summary>
    /// <param name="paymentSchema">Objeto a guardar</param>
    /// <param name="blUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>0. No se guardó | 1. se guardó</returns>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    public static int SavePaymentSchema(PaymentSchema paymentSchema,bool blUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Entry(paymentSchema).State = (blUpdate) ? EntityState.Modified : EntityState.Added;        

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}

using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public static class BRPaymentPlaces
  {
    #region GetPaymentPlaces

    /// <summary>
    /// Obtiene registros del catalogo PaymentPlaces
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="paymentPlaces">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo Payment Place</returns>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// [aalcocer] 02/08/2016 Modified. Se modifica la asincronía con Task
    /// </history>
    public static async Task<List<PaymentPlace>> GetPaymentPlaces(int nStatus = -1, PaymentPlace paymentPlaces = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from pc in dbContext.PaymentPlaces
                      select pc;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(pc => pc.pcA == blnStatus);
          }
          if (paymentPlaces != null)//verificamos que se tenga el objeto
          {
            if (!string.IsNullOrWhiteSpace(paymentPlaces.pcID))//Filtro por ID
            {
              query = query.Where(pc => pc.pcID == paymentPlaces.pcID);
            }

            if (!string.IsNullOrWhiteSpace(paymentPlaces.pcN))//Filtro por Descripción
            {
              query = query.Where(pc => pc.pcN.Contains(paymentPlaces.pcN));
            }
          }
          return query.OrderBy(pc => pc.pcN).ToList();
        }
      });
    }

    #endregion GetPaymentPlaces
  }
}
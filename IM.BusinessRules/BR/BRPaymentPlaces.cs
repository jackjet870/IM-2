using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRPaymentPlaces
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
    /// </history>
    public static List<PaymentPlace> GetPaymentPlaces(int nStatus = -1, PaymentPlace paymentPlaces = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from pc in dbContext.PaymentPlaces
                    select pc;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(pc => pc.pcA == blnStatus);
        }

        #region Filtros Iniciales
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
        #endregion

        return query.OrderBy(pc => pc.pcN).ToList();
      }
    }
    #endregion

    #region SavePaymentPlace
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo paymentPlaces
    /// </summary>
    /// <param name="paymentPlace">Objeto a guarda</param>
    /// <param name="blnUpdate">True. Actualiza | False. Agrega</param>
    /// <returns>0. No se guardó | 1. Se guardó | 2. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    public static int SavePaymentPlace(PaymentPlace paymentPlace,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Actualizar
        if (blnUpdate)//Si es actualizar
        {
          dbContext.Entry(paymentPlace).State = System.Data.Entity.EntityState.Modified;
        } 
        #endregion
        #region Insertar
        else//Insertar
        {
          PaymentPlace paymentPlaceVal = dbContext.PaymentPlaces.Where(pc => pc.pcID == paymentPlace.pcID).FirstOrDefault();

          if(paymentPlaceVal!=null)//Verificamos que no haya un registro con el mismo ID
          {
            return 2;
          }
          else//insertar
          {
            dbContext.PaymentPlaces.Add(paymentPlaceVal);

          }
        } 
        #endregion
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}

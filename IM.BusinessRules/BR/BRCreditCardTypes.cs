using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows;

namespace IM.BusinessRules.BR
{
  public class BRCreditCardTypes
  {
    #region GetCreditCardTypes
    /// <summary>
    /// Sirve para obetener una lista de Credit Crad Types
    /// </summary>
    /// <param name="creditCardType">Objeto con los filtros adicionales</param>
    /// <param name="nStatus">-1. Devuelve todos los registros | 0. devuelve registros inactivos | 1. devuelve registros activos</param>
    /// <returns>lista de credit card types</returns>
    /// <history>
    /// [Emoguel] created 07/03/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async static Task<List<CreditCardType>> GetCreditCardTypes(CreditCardType creditCardType = null, int nStatus = -1)
    {
      List<CreditCardType> lstCreditsCard = await Task.Run(() =>
        {
         
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            var query = from cct in dbContext.CreditCardTypes select cct;

            if (nStatus != -1)//Validación por estatus
          {
              bool blnEstatus = Convert.ToBoolean(nStatus);
              query = query.Where(cct => cct.ccA == blnEstatus);
            }

            if (creditCardType != null)//Valida si se tiene un obejto
          {
              if (!string.IsNullOrWhiteSpace(creditCardType.ccID))//Validación por ID
            {
                query = query.Where(cct => cct.ccID == creditCardType.ccID);
              }

              if (!string.IsNullOrWhiteSpace(creditCardType.ccN))//Validación por nombre/Descripción
            {
                query = query.Where(cct => cct.ccN.Contains(creditCardType.ccN));
              }
            }
            return query.OrderBy(cct => cct.ccN).ToList();
          }
        });
      return lstCreditsCard;

    }
    #endregion    

    #region GetCreditCardTypeId
    /// <summary>
    /// Obtiene una tarjeta de crédito por su ID
    /// </summary>
    /// <param name="creditcardId">Identificador de la tarjeta</param>
    /// <returns>CreditCardType</returns>
    /// <history>
    /// [lchairez] 23/03/2016 Created.
    /// </history>
    public static CreditCardType GetCreditCardTypeId(string creditcardId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.CreditCardTypes.Where(c => c.ccID == creditcardId).SingleOrDefault();
      }
    }
    #endregion

  }
}

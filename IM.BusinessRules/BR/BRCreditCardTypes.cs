using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

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
    /// </history>
    public static List<CreditCardType> GetCreditCardTypes(CreditCardType creditCardType=null, int nStatus = -1)
    {
      using (var dbContext = new IMEntities())
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

        return query.OrderBy(cct=>cct.ccN).ToList();
      }
    }
    #endregion
    #region SaveCreditCarType
    /// <summary>
    /// Agrega | actualiza un regsitro en el catalogo de Credit card types
    /// </summary>
    /// <param name="creditCardType">Objeto a agregar o actualiza en la BD</param>
    /// <param name="blnUpd">boleano para saber si se va a actualziar o agregar</param>
    /// <returns>0. No se guardó el registro | 1. El registro se guardó | 2. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [Emoguel] created 07/03/2016
    /// </history>
    public static int SaveCreditCardType(CreditCardType creditCardType, bool blnUpd)
    {
      int nRes = 0;
      using (var dbContext = new IMEntities())
      {
        if (blnUpd)//Si es alctualizar
        {
          dbContext.Entry(creditCardType).State = System.Data.Entity.EntityState.Modified;
          nRes = dbContext.SaveChanges();
        }
        else//Si es Agregar
        {
          CreditCardType creditCardTypeVal = dbContext.CreditCardTypes.Where(cct => cct.ccID == creditCardType.ccID).FirstOrDefault();
          if (creditCardTypeVal != null)//Existe un registro con el mismo ID
          {
            nRes = 2;
          }
          else//Agregar a la BD
          {
            dbContext.CreditCardTypes.Add(creditCardType);
            nRes = dbContext.SaveChanges();
          }
        }
      }
      return nRes;
    }
    #endregion
  }
}

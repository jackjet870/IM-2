using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRChargeCalculationTypes
  {
    #region GetChargeCalculatioTypes

    /// <summary>
    /// Devuelve la lista de ChargeCalculationType
    /// </summary>
    /// <param name="chargeCalTyp">Objeto que contiene los filtros adicionales, puede ser null</param>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <returns>Lista de ChargeCalculationType</returns>
    /// <history>
    /// [Emoguel] created 01/03/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// </history>
    public static List<ChargeCalculationType> GetChargeCalculationTypes(ChargeCalculationType chargeCalTyp=null, int nStatus=-1)
    {      
      
      List<ChargeCalculationType> lstCharge = new List<ChargeCalculationType>();

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from cct in dbContext.ChargeCalculationTypes
                    select cct;

        if(nStatus!=-1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(cct=>cct.caA==blnStatus);
        }

        if (chargeCalTyp != null)//Si se recibe objeto
        {
          if (!string.IsNullOrWhiteSpace(chargeCalTyp.caID))//Filtra por ID
          {
            query = query.Where(cct => cct.caID == chargeCalTyp.caID);
          }

          if (!string.IsNullOrWhiteSpace(chargeCalTyp.caN))//Filtra por Descripción
          {
            query = query.Where(cct => cct.caN.Contains(chargeCalTyp.caN));
          }
        }
        lstCharge = query.OrderBy(cct=>cct.caN).ToList();
      }
      return lstCharge;
      
    }
    #endregion
  }
}

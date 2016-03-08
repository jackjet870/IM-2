using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRChargeCalculationTypes
  {

    #region GetChargeCalculatioTypes

    /// <summary>
    /// Devuelve la lista de ChargeCalculationType
    /// </summary>
    /// <param name="chargeCalTyp">Objeto que contiene los filtros adicionales</param>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <returns>Lista de ChargeCalculationType</returns>
    /// <history>
    /// [Emoguel] created 01/03/2016
    /// </history>
    public static List<ChargeCalculationType> GetChargeCalculationTypes(ChargeCalculationType chargeCalTyp, int nStatus=-1)
    {      
      
      List<ChargeCalculationType> lstCharge = new List<ChargeCalculationType>();

      using (var dbContext = new IMEntities())
      {
        var query = from cct in dbContext.ChargeCalculationTypes
                    select cct;

        if(nStatus!=-1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(cct=>cct.caA==blnStatus);
        }

        if(!string.IsNullOrWhiteSpace(chargeCalTyp.caID))//Filtra por ID
        {
          query = query.Where(cct=>cct.caID==chargeCalTyp.caID);
        }

        if(!string.IsNullOrWhiteSpace(chargeCalTyp.caN))//Filtra por Descripción
        {
          query = query.Where(cct=>cct.caN==chargeCalTyp.caN);
        }

        lstCharge = query.ToList();
      }
      return lstCharge;
      
    }
    #endregion
  }
}

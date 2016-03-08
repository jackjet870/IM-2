using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRUnavailableMotives
  {
    #region getUnavailableMotives
    /// <summary>
    /// Devuelve la lista de UnavailMots
    /// </summary>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1. Registros Activos</param>
    /// <returns>Lista de UnavailMots</returns>
    /// <history>
    /// [Emoguel] created 01/03/2016
    /// </history>
    public static List<UnavailableMotive> GetUnavailableMotives(int nStatus=-1)    
    {
      List<UnavailableMotive> lstUnavailMot = new List<UnavailableMotive>();

      using (var dbContext = new IMEntities())
      {
        var query = from um in dbContext.UnavailableMotives
                    select um;

        if(nStatus!=-1)//Se filtra por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(um=>um.umA==blnStatus);
        }

        lstUnavailMot = query.ToList();
      }

      return lstUnavailMot;      
    }
    #endregion
  }
}

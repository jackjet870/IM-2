using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRUnavailableMotives
  {
    #region GetUnavailableMotives
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

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from um in dbContext.UnavailableMotives
                    select um;

        if(nStatus!=-1)//Se filtra por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(um=>um.umA==blnStatus);
        }

        lstUnavailMot = query.OrderBy(um=>um.umN).ToList();
      }

      return lstUnavailMot;      
    }
    #endregion

    #region GetUnavailableMotive

    /// <summary>
    /// Obtiene un motivo de indisponibilidad dada su clave
    /// </summary>
    /// <param name="id">Clave</param>
    /// <param name="nStatus">false: Registros inactivos | true: Registros Activos</param>
    /// <history>
    /// [jorcanche]  12/Mar/2016 Created
    /// </history>
    public static UnavailableMotive GetUnavailableMotive(int id, bool nStatus)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.UnavailableMotives.Where(u => u.umID == id && u.umA == nStatus).FirstOrDefault();
      }
    }

    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRAgencies
  {
    #region GetAgenciesShort

    /// <summary>
    /// Obtiene el catalogo de agencias
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    public static List<AgencyShort> GetAgencies(int status)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetAgencies(Convert.ToByte(status)).ToList();
      }
    }


    /// <summary>
    /// Obtiene el catalogo de agencias con todas sus columnas
    /// </summary>
    /// <param name="agency">Objeto con filtros extra</param>
    /// <param name="nStatus">-1. Todos los registros | 0.- Registros inactivos | 1. Registros Activos</param>
    /// <returns>Devuelve una lista de Agency</returns>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    public static List<Agency> GetAgencies(Agency agency,int nStatus=-1)
    {
      using (var dbContext = new IMEntities())
      {
        var query = from ag in dbContext.Agencies
                    select ag;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(ag => ag.agA == blnStatus);
        }

        if (!string.IsNullOrWhiteSpace(agency.agID))//Filtro por ID
        {
          query = query.Where(ag=>ag.agID==agency.agID);
        }

        if(!string.IsNullOrWhiteSpace(agency.agN))//Filtro por Nombre(Descripcion)
        {
          query = query.Where(ag=>ag.agN==agency.agN);
        }

        return query.ToList();

      }
    }
    #endregion
  }
}

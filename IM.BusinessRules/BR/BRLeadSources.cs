using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public class BRLeadSources
  {
    #region GetLeadSourcesByUser

    /// <summary>
    /// Obtiene los Lead Sources de un usuario
    /// </summary>
    /// <param name="user">Usuario </param>
    /// <param name="programs"> Programa o default('ALL') </param>
    /// <param name="regions">Region o default('ALL') </param>
    /// <returns>List<LeadSourceByUser></returns>
    /// <hystory>
    /// [erosado] 08/03/2016  created
    /// [aalcocer] 17/03/2016 Modified. Agregado parametros por default
    /// [erosado] 07/04/2016  Modified. Se cambio el parametro string Progam a EnumPrograms
    /// </hystory>

    public static List<LeadSourceByUser> GetLeadSourcesByUser(string user, EnumProgram program = EnumProgram.All, string regions = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetLeadSourcesByUser(user, EnumToListHelper.GetEnumDescription(program), regions).ToList();
      }
    }

    #endregion GetLeadSourcesByUser

    #region GetLeadSources

    /// <summary>
    /// Obtiene catalogo de Lead Sources .
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    /// <returns>List<LeadSource></returns>
    /// <history>
    /// [aalcocer] 09/03/2016 Created
    /// [jorcanche] 02/05/2016 Agrego el parametro EnumProgram para que se puede saleccionar
    /// </history>
    public static List<LeadSource> GetLeadSources(int status = 0, EnumProgram program = EnumProgram.All)
    {
      var pro = EnumToListHelper.GetEnumDescription(program);
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = dbContext.LeadSources.Where(ls => status.Equals(1) ? ls.lsA : status.Equals(2) ? !ls.lsA : true);

        if (program != EnumProgram.All)
        {
          query = query.Where(ls => ls.lspg == ((program == EnumProgram.All) ? ls.lspg : pro));
        }
        return query.OrderBy(ls => ls.lsN).ToList();
      }
    }

    #endregion GetLeadSources

    #region GetLeadSourcesByZoneBoss

    /// <summary>
    /// Obtiene los Lead Sources dada una zona y el patron configurado
    /// </summary>
    /// <param name="zone"> Zona </param>
    /// <returns>List<LeadSourceShort></returns>
    /// <history>
    /// [michan] 14/04/2016 Created
    /// </history>
    public static List<LeadSourceShort> GetLeadSourcesByZoneBoss(string zone)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetLeadSourcesByZoneBoss(zone).ToList();
      }
    }

    #endregion GetLeadSources
    
    #region GetOccupationLeadSources
    /// <summary>
    /// Obtiene el porcentaje de ocupacion de un Lead Source en una fecha determinada
    /// </summary>
    /// <param name="date">La fecha del cual se motrara el porcentaje de ese día</param>
    /// <param name="lS">EL hotel del cual mostrara su porcentaje</param>
    /// <returns>string</returns>
    /// <history>
    /// [jorcanche] 11/04/2016 created
    /// </history>
    public static string GetOccupationLeadSources(DateTime date, string lS)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return "Occupancy " + dbContext.USP_OR_Occupation(date, lS).Single();
      }
    }
    #endregion

    #region GetLeadSourcesById

    /// <summary>
    ///Método para obtener una lista de LeadSources por id.
    /// </summary>
    /// <param name="lsIDList">Lista de id de Agency</param>
    /// <returns>List<LeadSourceShort></returns>
    /// <history>
    /// [vku] 20/Abr/2016 Created
    /// </history>
    public static List<LeadSource> GetLeadSourceById(IEnumerable<string> lsIDList)
    {
      List<LeadSource> lstgetLeadSources;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        lstgetLeadSources = dbContext.LeadSources.Where(x => lsIDList.Contains(x.lsID)).
          Select(x => new
          {
            x.lsID,
            x.lsN
          }).AsEnumerable().
          Select(x => new LeadSource
          {
            lsID = x.lsID,
            lsN = x.lsN
          }).ToList();
      }
      return lstgetLeadSources;
    }

    #endregion GetLeadSourcesById

    #region GetLeadSourceByID
    /// <summary>
    /// Obtienen un Lead Source por un ID especificado
    /// </summary>
    /// <param name="leadSourceID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 11/Abril/2016 Created
    /// </history>
    public static LeadSource GetLeadSourceByID(string leadSourceID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.LeadSources.Where(x => x.lsID == leadSourceID).SingleOrDefault();












  }
    } 
    #endregion

  }
}
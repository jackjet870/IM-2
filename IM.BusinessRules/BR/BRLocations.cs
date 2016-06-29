using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRLocations
  {
    #region GetLocationsByUser

    /// <summary>
    /// Obtiene las locaciones de un usuario
    /// </summary>
    /// <param name="user">Usuario </param>
    /// <param name="programs"> Programa o default('ALL') </param>
    /// <history>
    /// [wtorres]  07/Mar/2016 Created
    /// </history>
    public async static Task<List<LocationByUser>> GetLocationsByUser(string user = "All", string programs = "All")
    {
      List<LocationByUser> result = null;
      await Task.Run(() =>
     {
       using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
       {
         result = dbContext.USP_OR_GetLocationsByUser(user, programs).ToList();
       }
     });
      return result;
    }

    #endregion

    #region GetLocations
    /// <summary>
    /// Obtiene registros del catalogo Locations
    /// </summary>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1.Registros Activos</param>
    /// <param name="location">Objeto con filtros adicionales</param>
    /// <param name="blnTeamsLog">para devolver la consulta que llena el combo en TeamsLog</param>
    /// <returns>Lista de tipo Location</returns>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// [emoguel] modified 27/04/2016 Se agregó el parametro blnTeamsLog para cargar el combobos de Locations en teamLog
    /// </history>
    public async static Task<List<Location>> GetLocations(int nStatus = -1, Location location = null, bool blnTeamsLog = false)
    {
      List<Location> lstLocations = await Task.Run(() =>
         {
           using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
           {
             var query = from lo in dbContext.Locations
                         select lo;
             if (blnTeamsLog)
             {
               query = from tg in dbContext.TeamsGuestServices
                       from lo in dbContext.Locations.Where(lo => lo.loID == tg.tglo).DefaultIfEmpty().Distinct()
                       select lo;
             }
             if (nStatus != -1)//Filtro por estatus
          {
               bool blnEstatus = Convert.ToBoolean(nStatus);
               query = query.Where(lo => lo.loA == blnEstatus);
             }
          #region Filtros adicionales

          if (location != null)//verificar si se tiene un objeto
          {
               if (!string.IsNullOrWhiteSpace(location.loID))//filtro por ID
            {
                 query = query.Where(lo => lo.loID == location.loID);
               }

               if (!string.IsNullOrWhiteSpace(location.loN))//Filtro por Descripcion
            {
                 query = query.Where(lo => lo.loN.Contains(location.loN));
               }

               if (!string.IsNullOrWhiteSpace(location.losr))//Filtro por sales room
            {
                 query = query.Where(lo => lo.losr == location.losr);
               }

               if (!string.IsNullOrWhiteSpace(location.lolc))//Filtro por categoria
            {
                 query = query.Where(lo => lo.lolc == location.lolc);
               }
             }

          #endregion
          return query.OrderBy(lo => lo.loN).ToList();
           }
         });
      return lstLocations;
    }

    #endregion

    #region GetLocationsByProgram
    /// <summary>
    /// Obtiene una lista de locations filtrados por Programa.
    /// </summary>
    /// <param name="program"> Programa "IH","OUT" </param>
    /// <history>
    /// [edgrodriguez] created 27/04/2016
    /// [edgrodriguez] modified 23/05/2016 Se agregó asincronia.
    /// </history>
    public async static Task<List<Location>> GetLocationsbyProgram(string program = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var Locs = (from loc in dbContext.Locations
                    join ls in dbContext.LeadSources on loc.lols equals ls.lsID
                    where loc.loA && (program == "ALL" || ls.lspg == program)
                    select loc).OrderBy(c => c.loN);

        return await Locs.ToListAsync();
      }
    }
    #endregion

    #region getLocationByTeamGuestSevice
    /// <summary>
    /// Obtiene los Locations relacionados a TeamsGuestServices
    /// </summary>
    /// <returns>Lista tipo Object</returns>
    /// <history>
    /// [emoguel] created 18/06/2016
    /// </history>
    public static async Task<List<object>> GetLocationByTeamGuestService()
    {
      List<object> lstObject = await Task.Run(() => {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = (from lo in dbContext.Locations
                       from tg in dbContext.TeamsGuestServices.Distinct()
                       where lo.loID== tg.tglo
                       select new { loID = lo.loID, loN = lo.loN }).Distinct();
          return query.ToList<object>();
        }
      });

      return lstObject;
    }
    #endregion
  }
}

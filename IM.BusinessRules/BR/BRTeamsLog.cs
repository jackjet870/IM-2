using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRTeamsLog
  {

    #region GetTeamsLog
    /// <summary>
    /// Obtiene registros del catalog TeamsLog
    /// </summary>
    /// <param name="teamLog">Objeto con filtros adicionales</param>
    /// <returns>Lista tipo Dynamic</returns>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
    public static IEnumerable<object> GetTeamsLog(TeamLog teamLog = null,bool blnDate=false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from tl in dbContext.TeamsLogs
                    from pc in dbContext.Personnels.Where(pc => pc.peID == tl.tlChangedBy).DefaultIfEmpty()
                    from pe in dbContext.Personnels.Where(pe => pe.peID == tl.tlpe).DefaultIfEmpty()
                    from tt in dbContext.TeamsTypes.Where(tt => tt.ttID == tl.tlTeamType).DefaultIfEmpty()
                    from sr in dbContext.SalesRooms.Where(sr => sr.srID == tl.tlPlaceID).DefaultIfEmpty()
                    from ts in dbContext.TeamsSalesmen.Where(ts => ts.tsID == tl.tlTeam && ts.tssr == sr.srID).DefaultIfEmpty()
                    from lo in dbContext.Locations.Where(lo => lo.loID == tl.tlPlaceID).DefaultIfEmpty()
                    from tg in dbContext.TeamsGuestServices.Where(tg => tg.tgID == tl.tlTeam && tg.tglo == lo.loID).DefaultIfEmpty()
                    select new
                    {
                      tlID = tl.tlID,
                      tlDT=tl.tlDT,
                      teamLog = tl,
                      changedBy = pc.peN,
                      personel = pe.peN,
                      teamType = tt.ttN,
                      placeType = (tl.tlTeamType == "GS") ? "Location" : ((tl.tlTeamType == "SA") ? "Sales Room" : ""),
                      place = (tl.tlTeamType == "GS") ? lo.loN : ((tl.tlTeamType == "SA") ? sr.srN : ""),
                      team = (tl.tlTeamType == "GS") ? tg.tgN : ((tl.tlTeamType == "SA") ? ts.tsN : ""),
                    };

        if (teamLog != null)
        {
          if(teamLog.tlID>0)//Filtro por ID
          {
            query = query.Where(tl => tl.tlID == teamLog.tlID);
          }
          if(blnDate)//Filtro por Fecha
          {
            query = query.Where(tl => DbFunctions.TruncateTime(tl.teamLog.tlDT).Value == DbFunctions.TruncateTime(teamLog.tlDT));
          }

          if (!string.IsNullOrWhiteSpace(teamLog.tlChangedBy))//Filtro por changedBy
          {
            query = query.Where(tl => tl.teamLog.tlChangedBy == teamLog.tlChangedBy);
          }

          if (!string.IsNullOrWhiteSpace(teamLog.tlpe))//Filtro por Perssonel
          {
            query = query.Where(tl => tl.teamLog.tlpe == teamLog.tlpe);
          }
        }


        return query.OrderByDescending(tl => tl.teamLog.tlDT).ToList();
      }
    } 
    #endregion

  }
}

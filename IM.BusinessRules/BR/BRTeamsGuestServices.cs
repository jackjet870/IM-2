using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Enums;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRTeamsGuestServices
  {
    #region GetTeamsGuestServices
    /// <summary>
    /// Obtiene registros del catalogo TeamGuestServices
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="teamGuestServices">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo teamGuestServices</returns>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// [vku] 09/Jul/2016 Modified. Se agrego asincronía
    /// </history>
    public async static Task<List<TeamGuestServices>> GetTeamsGuestServices(int nStatus = -1, TeamGuestServices teamGuestServices = null)
    {
      List<TeamGuestServices> lstTeamGuestServices = new List<TeamGuestServices>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from tg in dbContext.TeamsGuestServices
                      select tg;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(tg => tg.tgA == blnStatus);
          }

          if (teamGuestServices != null)
          {
            if (!string.IsNullOrWhiteSpace(teamGuestServices.tglo))//Filtro por location
            {
              query = query.Where(tg => tg.tglo == teamGuestServices.tglo);
            }

            if (!string.IsNullOrWhiteSpace(teamGuestServices.tgID))//Filtro por ID
            {
              query = query.Where(tg => tg.tgID == teamGuestServices.tgID);
            }

            if (!string.IsNullOrWhiteSpace(teamGuestServices.tgN))//Filtro por descripción
            {
              query = query.Where(tg => tg.tgN.Contains(teamGuestServices.tgN));
            }

            if (!string.IsNullOrWhiteSpace(teamGuestServices.tgLeader))//Filtro por leader
            {
              query = query.Where(tg => tg.tgLeader == teamGuestServices.tgLeader);
            }
          }
          lstTeamGuestServices = query.OrderBy(tg => tg.tgN).ToList();
        }
      });
      return lstTeamGuestServices;
    }
    #endregion

    #region SaveTeam
    /// <summary>
    ///   Agrega | Actualiza un equipo de PRs
    /// </summary>
    /// <param name="team">Objeto a guardar</param>
    /// <param name="blnUpdate">True - Actualiza | False - Agrega</param>
    /// <param name="lstAdd">Personnels a asignar al equipo</param>
    /// <param name="lstDel">Personeels a remover del equipo</param>
    /// <returns>0. No se guardó | 1. Se guardó | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    ///   [vku] 13/Jul/2016 Created
    /// </history>
    public async static Task<int> SaveTeam(string idUser, TeamGuestServices team, bool blnUpdate, List<Personnel> lstAdd, List<Personnel> lstDel, List<Personnel> lstChanged)
    {
      bool HadPost = false;
      int nRes = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region ChangePersonnel
              if (lstChanged.Count > 0)
              {
                foreach (Personnel pe in lstChanged)
                {
                  dbContext.Entry(pe).State = EntityState.Modified;
                }
              }
              #endregion

              #region Update
              if (blnUpdate)
              {
                dbContext.Entry(team).State = EntityState.Modified;
              }
              #endregion
              #region Add
              else
              {
                TeamGuestServices teamVal = dbContext.TeamsGuestServices.Where(te => te.tgID == team.tgID).FirstOrDefault();
                if (teamVal != null)
                {
                  return -1;
                }
                else
                {
                  dbContext.TeamsGuestServices.Add(team);
                }
              }
              #endregion

              #region addPerssonnel
              if (lstAdd.Count > 0)
              {
                dbContext.Personnels.AsEnumerable().Where(pe => lstAdd.Any(pee => pee.peID == pe.peID)).ToList().ForEach(pe =>
                {
                  pe.peTeamType = EnumToListHelper.GetEnumDescription(EnumTeamType.TeamPRs);
                  pe.pePlaceID = team.tglo;
                  pe.peTeam = team.tgID;
                 if(pe.pepo=="GS" || pe.pepo == "OPC")
                    HadPost = true;
                  if (BRPrograms.GetProgramByLocation(team.tglo) == EnumToListHelper.GetEnumDescription(EnumProgram.Inhouse))
                    pe.pepo = "GS";
                  else pe.pepo = "OPC";
            
                  DateTime dtmServerDate = BRHelpers.GetServerDateTime();
  
                  TeamLog teamLog = new TeamLog();
                  teamLog.tlDT = dtmServerDate;
                  teamLog.tlChangedBy = idUser;
                  teamLog.tlpe = pe.peID;
                  teamLog.tlTeamType = pe.peTeamType;
                  teamLog.tlPlaceID = pe.pePlaceID;
                  teamLog.tlTeam = pe.peTeam;
                  dbContext.TeamsLogs.Add(teamLog);
                  if (HadPost == false)
                  {
                    PostLog postLog = new PostLog();
                    postLog.ppChangedBy = idUser;
                    postLog.ppDT = dtmServerDate;
                    postLog.pppe = pe.peID;
                    postLog.pppo = pe.pepo;
                    dbContext.PostsLogs.Add(postLog);
                  } 
                });
              }
              #endregion

              #region del Personnel
              if (lstDel.Count > 0)
              {
                dbContext.Personnels.AsEnumerable().Where(pe => lstDel.Any(pee => pee.peID == pe.peID)).ToList().ForEach(pe => 
                {
                  pe.peTeamType = null;
                  pe.pePlaceID = null;
                  pe.peTeam = null;
                  pe.pepo = null;
                  pe.peLinerID = null;

                  DateTime dtmServerDate = BRHelpers.GetServerDateTime();

                  TeamLog teamLog = new TeamLog();
                  teamLog.tlDT = dtmServerDate;
                  teamLog.tlChangedBy = idUser;
                  teamLog.tlpe = pe.peID;
                  teamLog.tlTeamType = pe.peTeamType;
                  teamLog.tlPlaceID = pe.pePlaceID;
                  teamLog.tlTeam = pe.peTeam;
                  dbContext.TeamsLogs.Add(teamLog);

                  PostLog postLog = new PostLog();
                  postLog.ppChangedBy = idUser;
                  postLog.ppDT = dtmServerDate;
                  postLog.pppe = pe.peID;
                  postLog.pppo = pe.pepo;
                  dbContext.PostsLogs.Add(postLog);
                });
              }
              #endregion

              int nSave = dbContext.SaveChanges();
              transaction.Commit();
              return nSave;
            }
            catch
            {
              transaction.Rollback();
              return 0;
            }
          }
        }
      });
      return nRes;
    }
    #endregion
  }
}

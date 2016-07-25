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
  public class BRTeamsSalesMen
  {
    #region GetTeamsSalesMen
    /// <summary>
    /// Obtiene registros del catalogo TeamsSalesmen
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="teamSalesMen">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo TeamSalesMen</returns>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// [vku] 25/Jul/2016 Modified. El método ahora es asincrono
    /// </history>
    public async static Task<List<TeamSalesmen>> GetTeamsSalesMen(int nStatus = -1, TeamSalesmen teamSalesMen = null)
    {
      List<TeamSalesmen> lstTeamSalesmen = new List<TeamSalesmen>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from ts in dbContext.TeamsSalesmen
                      select ts;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(ts => ts.tsA == blnStatus);
          }

          if (teamSalesMen != null)
          {
            if (!string.IsNullOrWhiteSpace(teamSalesMen.tssr))//Filtro por Sales Room
            {
              query = query.Where(ts => ts.tssr == teamSalesMen.tssr);
            }
            if (!string.IsNullOrWhiteSpace(teamSalesMen.tsID))//Filtro por ID
            {
              query = query.Where(ts => ts.tsID == teamSalesMen.tsID);
            }

            if (!string.IsNullOrWhiteSpace(teamSalesMen.tsN))//Filtro por descripción
            {
              query = query.Where(ts => ts.tsN.Contains(teamSalesMen.tsN));
            }

            if (!string.IsNullOrWhiteSpace(teamSalesMen.tsLeader))//Filtro por Lide
            {
              query = query.Where(ts => ts.tsLeader == teamSalesMen.tsLeader);
            }
          }
          lstTeamSalesmen =  query.OrderBy(ts => ts.tsN).ToList();
        }
      });
      return lstTeamSalesmen;   
    }
    #endregion

    #region SaveTeam
    /// <summary>
    ///    Agrega | Actualiza un equipo de PRs
    /// </summary>
    /// <param name="idUser">Clave de usuario</param>
    /// <param name="team">Objeto a guardar</param>
    /// <param name="blnUpdate">True - Actualiza | False - Agrega</param>
    /// <param name="lstAdd">Personnels a asignar al equipo</param>
    /// <param name="lstDel">Personeels a remover del equipo</param>
    /// <param name="lstChanged">Lista de integrantes que tuvieron cambios</param>
    /// <returns>0. No se guardó | 1. Se guardó | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    public async static Task<int> SaveTeam(string idUser, TeamSalesmen team, bool blnUpdate, List<Personnel> lstAdd, List<Personnel> lstDel, List<Personnel> lstChanged)
    {
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
                foreach (Personnel p in lstChanged)
                {
                  dbContext.Entry(p).State = EntityState.Modified;

                  DateTime dtmServerDate = BRHelpers.GetServerDateTime();

                  PostLog postLog = new PostLog();
                  postLog.ppChangedBy = idUser;
                  postLog.ppDT = dtmServerDate;
                  postLog.pppe = p.peID;
                  postLog.pppo = p.pepo;
                  dbContext.PostsLogs.Add(postLog);
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
                TeamSalesmen teamVal = dbContext.TeamsSalesmen.Where(te => te.tsID == team.tsID).FirstOrDefault();
                if (teamVal != null)
                {
                  return -1;
                }
                else
                {
                  dbContext.TeamsSalesmen.Add(team);
                }
              }
              #endregion

              #region addPerssonnel
              if (lstAdd.Count > 0)
              {
                dbContext.Personnels.AsEnumerable().Where(pe => lstAdd.Any(pee => pee.peID == pe.peID)).ToList().ForEach(pe =>
                {
                  pe.peTeamType = EnumToListHelper.GetEnumDescription(EnumTeamType.TeamSalesmen);
                  pe.pePlaceID = team.tssr;
                  pe.peTeam = team.tsID;
                 
                  DateTime dtmServerDate = BRHelpers.GetServerDateTime();

                  TeamLog teamLog = new TeamLog();
                  teamLog.tlDT = dtmServerDate;
                  teamLog.tlChangedBy = idUser;
                  teamLog.tlpe = pe.peID;
                  teamLog.tlTeamType = pe.peTeamType;
                  teamLog.tlPlaceID = pe.pePlaceID;
                  teamLog.tlTeam = pe.peTeam;
                  dbContext.TeamsLogs.Add(teamLog);
                  Personnel p = lstAdd.Where(pa => pa.peID == pe.peID).FirstOrDefault();
                  if (p.pepo != pe.pepo)
                  {
                    dbContext.Entry(p).State = EntityState.Modified;

                    PostLog postLog = new PostLog();
                    postLog.ppChangedBy = idUser;
                    postLog.ppDT = dtmServerDate;
                    postLog.pppe = pe.peID;
                    postLog.pppo = p.pepo;
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
               
                  DateTime dtmServerDate = BRHelpers.GetServerDateTime();

                  TeamLog teamLog = new TeamLog();
                  teamLog.tlDT = dtmServerDate;
                  teamLog.tlChangedBy = idUser;
                  teamLog.tlpe = pe.peID;
                  teamLog.tlTeamType = pe.peTeamType;
                  teamLog.tlPlaceID = pe.pePlaceID;
                  teamLog.tlTeam = pe.peTeam;
                  dbContext.TeamsLogs.Add(teamLog);
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

﻿using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

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
    /// [edgrodriguez] 21/May/2016 Modified. El método se volvio asincrónico.
    /// </hystory>

    public async static Task<List<LeadSourceByUser>> GetLeadSourcesByUser(string user, EnumProgram program = EnumProgram.All, string regions = "ALL")
    {
      List<LeadSourceByUser> result = new List<LeadSourceByUser>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          result = dbContext.USP_OR_GetLeadSourcesByUser(user, EnumToListHelper.GetEnumDescription(program), regions).ToList();
        }
      });

      return result;
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
    /// [aalcocer] 09/06/2016 Modified.  Se agregó asincronía.
    /// </history>
    public static async Task<List<LeadSource>> GetLeadSources(int status = 0, EnumProgram program = EnumProgram.All)
    {
      var leadSources = new List<LeadSource>();
      var pro = EnumToListHelper.GetEnumDescription(program);
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          leadSources = dbContext.LeadSources
            .Where(ls => status.Equals(1) ? ls.lsA : status.Equals(2) ? !ls.lsA : true)
            .Where(ls => program != EnumProgram.All ? ls.lspg == pro : true)
            .OrderBy(ls => ls.lsN).ToList();
        }
      });
      return leadSources;
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
    public async static Task<List<LeadSourceShort>> GetLeadSourcesByZoneBoss(string zone)
    {
      List<LeadSourceShort> leadSourceShort = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          leadSourceShort = dbContext.USP_OR_GetLeadSourcesByZoneBoss(zone).ToList();
        }
      });
      return leadSourceShort;
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
    public static Task<string> GetOccupationLeadSources(DateTime date, string lS)
    {
      return Task.Run(() =>
     {
       using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
       {
         dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_Occupation;
         return "Occupancy " + dbContext.USP_OR_Occupation(date, lS).Single();
       }
     });
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.LeadSources.Where(x => x.lsID == leadSourceID).SingleOrDefault();
      }
    }
    #endregion
   
    #region GetLeadSources
    /// <summary>
    /// Obtiene registros del catalogo LeadSources
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Registros Inactivos | 1. registros Activos </param>
    /// <param name="nRegen">-1. Todos | 0. regen Active | 1. regen Inactive </param>
    /// <param name="nAnimation">-1. Todos | 0. Animation Active | 1. Animation Inactive </param>
    /// <param name="leadSource">Objeto con filtros adicionales</param>
    /// <returns>Lista tipo LeadSource</returns>
    /// <history>
    /// [emoguel] created 13/05/2016
    /// [emogue] Modified 25/05/2016 se volvio async el Metodo
    /// </history>
    public async static Task<List<LeadSource>> GetLeadSources(int nStatus = -1, int nRegen = -1, int nAnimation = -1, LeadSource leadSource = null, bool blnAgencies = false)
    {
      List<LeadSource> lstLeadSources = new List<LeadSource>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = (from ls in dbContext.LeadSources
                       select ls);

          if (blnAgencies)
          {
            query = query.Include("Agencies");
          }

          if (nStatus != -1)//Filtro por Estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(ls => ls.lsA == blnStatus);
          }

          if (nRegen != -1)//Filtro por Regen
          {
            bool blnRegen = Convert.ToBoolean(nRegen);
            query = query.Where(ls => ls.lsRegen == blnRegen);
          }

          if (nAnimation != -1)//Filtro por Animation
          {
            bool blnAnimation = Convert.ToBoolean(nAnimation);
            query = query.Where(ls => ls.lsAnimation == blnAnimation);
          }

          if (leadSource != null)
          {
            if (!string.IsNullOrWhiteSpace(leadSource.lsID))//Filtro por ID
            {
              query = query.Where(ls => ls.lsID == leadSource.lsID);
            }

            if (!string.IsNullOrWhiteSpace(leadSource.lsN))//Filtro por descripción
            {
              query = query.Where(ls => ls.lsN.Contains(leadSource.lsN));
            }

            if (!string.IsNullOrWhiteSpace(leadSource.lspg))//Filtro por Program
            {
              query = query.Where(ls => ls.lspg == leadSource.lspg);
            }

            if (!string.IsNullOrWhiteSpace(leadSource.lssr))//Filtro por Sales Room
            {
              query = query.Where(ls => ls.lssr == leadSource.lssr);
            }

            if (!string.IsNullOrWhiteSpace(leadSource.lsar))//Filtro por Area
            {
              query = query.Where(ls => ls.lsar == leadSource.lsar);
            }

            if (!string.IsNullOrWhiteSpace(leadSource.lsrg))//Filtro por region
            {
              query = query.Where(ls => ls.lsrg == leadSource.lsrg);
            }

            if (!string.IsNullOrWhiteSpace(leadSource.lsso))//Filtro por Segment
            {
              query = query.Where(ls => ls.lsso == leadSource.lsso);
            }

          }
          lstLeadSources = query.OrderBy(ls => ls.lsN).ToList();
        }
      });
      return lstLeadSources;
    }
    #endregion

    #region SaveLeadSource
    /// <summary>
    /// Agrega|Actualiza los datos de un LeadSource
    /// </summary>
    /// <param name="leadSource">Objeto a guardar</param>
    /// <param name="lstLocAdd">Locaciones a relacionar</param>
    /// <param name="lstLocDel">Locaciones a des-relacionar</param>
    /// <param name="lstAgeAdd">Agencias a relacionar</param>
    /// <param name="lstAgeDel">Agencias a desrelacionar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Elimina</param>
    /// <returns>-1. Existe un registro con el mismo ID | 0. No se guardó | >1. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 17/05/2016
    /// [emoguel] modified 27/06/2016 --> se volvió async
    /// </history>
    public async static Task<int> SaveLeadSource(LeadSource leadSource, List<Location> lstLocAdd, List<Location> lstLocDel, List<Agency> lstAgeAdd, List<Agency> lstAgeDel, bool blnUpdate)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region LeadSources                                     
              LeadSource leadSourceSave = null;
              #region Update
              if (blnUpdate)
              {
                leadSourceSave = dbContext.LeadSources.Where(ls => ls.lsID == leadSource.lsID).Include("Agencies").FirstOrDefault();
                ObjectHelper.CopyProperties(leadSourceSave, leadSource);
              }
              #endregion
              #region Insert
              else
              {
                leadSourceSave = dbContext.LeadSources.Where(ls => ls.lsID == leadSource.lsID).FirstOrDefault();
                if (leadSourceSave != null)
                {
                  return -1;
                }
                else
                {
                  DateTime dtServer = BRHelpers.GetServerDateTime();
                  leadSource.lsTransBridgeDT = dtServer;
                  leadSource.lsTransDT = dtServer;
                  dbContext.LeadSources.Add(leadSource);
                  leadSourceSave = leadSource;
                }

                List<Language> lstLanguages = dbContext.Languages.ToList();

                #region MailOuts
                MailOut mailOut = new MailOut { mols = leadSourceSave.lsID, moCode = "WELCOME" };
                #endregion

                #region Mail Outs Text y Invits Text
                lstLanguages.ForEach(la =>
                {
                  MailOutText mailOutText = new MailOutText
                  {
                    mtls = leadSourceSave.lsID,
                    mtmoCode = "WLECOME",
                    mtla = la.laID
                  };

                  InvitationText invitationText = new InvitationText
                  {
                    itls = leadSourceSave.lsID,
                    itla = la.laID,
                    itRTF = "<No text Saved>"
                  };
                });
                #endregion
              }
              #endregion
              #endregion

              #region Agencies
              lstAgeDel.ForEach(ag =>
              {
                leadSourceSave.Agencies.Remove(leadSourceSave.Agencies.Where(agg => agg.agID == ag.agID).FirstOrDefault());
              });

              lstAgeAdd.ForEach(ag =>
              {
                leadSourceSave.Agencies.Add(dbContext.Agencies.Where(agg => agg.agID == ag.agID).FirstOrDefault());
              });
              #endregion

              #region Locations
              dbContext.Locations.AsEnumerable().Where(lo => lstLocAdd.Any(loc => loc.loID == lo.loID)).ToList().ForEach(lo =>
              {
                lo.lols = leadSourceSave.lsID;
                dbContext.Entry(lo).State = EntityState.Modified;
              });

              dbContext.Locations.AsEnumerable().Where(lo => lstLocDel.Any(loc => loc.loID == lo.loID)).ToList().ForEach(lo =>
              {
                lo.lols = null;
                dbContext.Entry(lo).State = EntityState.Modified;
              });
              #endregion

              int nRes = dbContext.SaveChanges();
              if (!blnUpdate)
              {
                dbContext.USP_OR_AddAccessAdministrator("LS");
                dbContext.SaveChanges();
              }
              transaction.Commit();
              ObjectHelper.CopyProperties(leadSource, leadSourceSave, true);
              return nRes;
            }
            catch
            {
              transaction.Rollback();
              return 0;
            }
          }
        }
      });
    }
    #endregion

    #region GetLeadsourcesByNotice
    /// <summary>
    /// Obtiene los leadSources relacionados a un Notice que equivale a traer los registros de la tabla NoticesByLeadSource
    /// </summary>
    /// <param name="idNotice">id del notice</param>
    /// <returns>Lista de tipo LeadSources</returns>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    public static async Task<List<LeadSource>> GetLeadsourcesByNotice(int idNotice)
    {
      try
      {
        return await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            var notice = dbContext.Notices.Where(no => no.noID == idNotice).Include(no => no.LeadSources).FirstOrDefault();
            return notice.LeadSources.OrderBy(ls => ls.lsN).ToList();
          }
        });
      }
      catch
      {
        throw;
      }
    }
    #endregion

    #region GetLeadSourceProgram
    /// <summary>
    /// Obtiene el program de un LeadSource en especifico 
    /// </summary>
    /// <param name="lsID">lsID</param>
    /// <returns>string program</returns>
    /// <history>
    /// [erosado] 02/08/2016  Created.
    /// </history>
    public static async Task<string> GetLeadSourceProgram(string lsID)
    {
      try
      {
        return await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            var program = dbContext.LeadSources.FirstOrDefault(x => x.lsID == lsID);
            return program?.lspg;
          }
        });
      }
      catch
      {
        throw;
      }
    }
    #endregion

    #region GetLeadSourceByGuestId

    /// <summary>
    /// Obtienen el nombre de un Lead Source por ID de un Huesped
    /// </summary>
    /// <param name="guestId">Id del Huesped</param>
    /// <returns></returns>
    /// <history>
    /// [JORCANCHE] 11/JUL/2016 Created
    /// </history>
    public static async Task<string> GetLeadSourceByGuestId(int guestId)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return (from ls in dbContext.LeadSources
                  join gu in dbContext.Guests on ls.lsID equals gu.guls
                  where gu.guID == guestId
                  select ls.lsN).FirstOrDefault();
        }
      });
    }

    #endregion
  }
}
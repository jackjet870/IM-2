using IM.Model;
using IM.Model.Helpers;
using IM.Model.Classes;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  /// <summary>
  /// Clase manejador de reglas de tipo Seasons
  /// </summary>
  /// <history>
  /// [vipacheco] 07/03/2016 Created
  /// [vku] 26/Jul/2016 Modified. Agregue el metodo GetSeasons
  /// </history>
  public class BRSeasons
  {
    #region UpdateSeasonDates
    /// <summary>
    /// Función encargada de actualizar las fechas de temporada hasta el año ingresado
    /// </summary>
    /// <param name="yearServer"></param>
    /// <history>
    /// [vipacheco] 07/03/2016 Created
    /// </history>
    public async static Task UpdateSeasonDates(int yearServer)
    {
      await Task.Run(() =>
      {
        using (var model = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          model.USP_OR_ActualizarFechasTemporadas(yearServer);
        }
      });
    }
    #endregion

    #region GetSeasons
    /// <summary>
    ///   Obtiene los registros del catalogo de temporadas
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="season">Objeto con filtros adicionales</param>
    /// <returns>Lista de temporadas</returns>
    /// <history>
    ///   [vku] 26/Jul/2016 Created
    /// </history>
    public static async Task<List<Season>> GetSeasons(int nStatus = -1, Season season = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from ss in dbContext.Seasons
                      select ss;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(ss => ss.ssA == blnStatus);
          }

          if (season != null)
          {
            if (!string.IsNullOrWhiteSpace(season.ssID))//Filtro por ID
            {
              query = query.Where(ss => ss.ssID == season.ssID);
            }

            if (!string.IsNullOrWhiteSpace(season.ssN))//Filtro por descripción
            {
              query = query.Where(ss => ss.ssN.Contains(season.ssN));
            }
          }
          return query.OrderBy(ss => ss.ssN).ToList();
        }
      });
    }
    #endregion

    #region GetSeasonDates
    /// <summary>
    ///   Obtiene los registros de las fechas de temporada
    /// </summary>
    /// <param name="seasonD">Objeto con filtros adicionales</param>
    /// <returns>Lista de fechas de una temporada</returns>
    /// <history>
    ///   [vku] 27/Jul/2016 Created
    /// </history>
    public static async Task<List<SeasonDate>> GetSeasonDates(SeasonDate seasonD = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from sd in dbContext.SeasonsDates
                      select sd;

          if (seasonD != null)
          {
            if (!string.IsNullOrWhiteSpace(seasonD.sdss))//Filtro por ID de season
            {
              query = query.Where(sd => sd.sdss == seasonD.sdss);
            }
          }
          return query.OrderBy(ssd => ssd.sdStartD).ToList();
        }
      });
    }
    #endregion

    #region GetRangeDatesForValidateTraslape
    /// <summary>
    ///   Obtiene el rango de fechas para validar que no se traslape con uno de otra temporada
    /// </summary>
    /// <param name="dtmDate">Fecha</param>
    /// <returns></returns>
    /// <history>
    ///   [vku] 30/Jul/2016 Created
    /// </history>
    public static List<RangeDatesTraslape> GetRangeDatesForValidateTraslape(DateTime dtmDate)
    {
      List<RangeDatesTraslape> lstObject = new List<RangeDatesTraslape>();
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = (from ssd in dbContext.SeasonsDates
                       join s in dbContext.Seasons on ssd.sdss equals s.ssID
                       where ssd.sdStartD <= dtmDate && dtmDate <= ssd.sdEndD 
                       select new RangeDatesTraslape
                       {
                         sdss = ssd.sdss,
                         sdStartD = ssd.sdStartD,
                         sdEndD = ssd.sdEndD,
                         ssN = s.ssN
                       });
          lstObject = query.ToList();
        }
      return lstObject;
    }
    #endregion

    #region GetRangeDatesForValidateTraslapeIsEdit
    /// <summary>
    ///   Obtiene el rango de fechas para validar que no se traslape con uno de otra temporada si esta modificando una temporada
    /// </summary>
    /// <param name="dtmDate">Fecha</param>
    /// <param name="ssID">Clave de la temporada</param>
    /// <returns></returns>
    /// <history>
    ///   [vku] 20/Jul/2016 Created
    /// </history>
    public static List<RangeDatesTraslape> GetRangeDatesForValidateTraslapeIsEdit(DateTime dtmDate, string ssID)
    {
      List<RangeDatesTraslape> lstObject = new List<RangeDatesTraslape>();
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = (from ssd in dbContext.SeasonsDates
                       join s in dbContext.Seasons on ssd.sdss equals s.ssID
                       where ssd.sdStartD <= dtmDate && dtmDate <= ssd.sdEndD && ssd.sdss != ssID
                       select new RangeDatesTraslape
                       {
                         sdss = ssd.sdss,
                         sdStartD = ssd.sdStartD,
                         sdEndD = ssd.sdEndD,
                         ssN = s.ssN
                       });
          lstObject = query.ToList();
        }
   
      return lstObject;
    }
    #endregion

    #region SaveSeason
    /// <summary>
    ///   Guarda las fechas de temporadas
    /// </summary>
    /// <returns></returns>
    /// <history>
    ///   [vku] 03/Ago/2016 Created
    /// </history>
    public async static Task<int> SaveSeason(Season season, bool blnUpdate, List<SeasonDate> lstAdd, List<SeasonDate> lstDel, List<SeasonDate> lstChange)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region Update
              if (blnUpdate)
              {
                dbContext.Entry(season).State = EntityState.Modified;
              }
              #endregion

              #region Add
              else
              {
                Season seasonVal = dbContext.Seasons.Where(ss => ss.ssID == season.ssID).FirstOrDefault();
                if (seasonVal != null)
                {
                  return -1;
                }
                else
                {
                  dbContext.Seasons.Add(season);
                }
              }
              #endregion

              #region dellRangeDates
              if (lstDel.Count > 0)
              {
                lstDel.ForEach(item =>
                {
                  dbContext.Entry(item).State = EntityState.Deleted;
                });
              }
              #endregion

              #region addRangeDates
              if (lstAdd.Count > 0)
              {
                lstAdd.ForEach(item =>
                {
                  SeasonDate seasonD = dbContext.SeasonsDates.Where(dbSD => dbSD.sdss == season.ssID && dbSD.sdStartD == item.sdStartD && dbSD.sdEndD == item.sdEndD).FirstOrDefault();
                  if (seasonD == null)
                  {
                    item.sdss = season.ssID;
                    dbContext.Entry(item).State = EntityState.Added;
                  }
                });
              }
              #endregion

              #region ChangeRange 
              if(lstChange.Count > 0)
              {
                lstChange.ForEach(item =>
                {
                  SeasonDate seasonD = dbContext.SeasonsDates.Where(dbSD => dbSD.sdss == item.sdss && dbSD.sdStartD == item.sdStartD && dbSD.sdEndD == item.sdEndD).FirstOrDefault();
                  if (seasonD == null)
                  {
                    dbContext.Entry(item).State = EntityState.Added;
                  }
                });
              }
              #endregion

              int nRes = dbContext.SaveChanges();
              transaction.Commit();
              return nRes;
            }
            catch(Exception ex)
            {
              transaction.Rollback();
              return 0;
            }
         }
       }
     });
    }
    #endregion
  }
}

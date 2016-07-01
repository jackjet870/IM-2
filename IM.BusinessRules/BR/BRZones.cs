using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
    public class BRZones
  {
    #region GetZonesTransfer
    /// <summary>
    /// Obtiene las zonas de transferencia
    /// </summary>
    /// <returns></returns>
    /// <history>
    ///   [michan] 24/02/2016 Created
    /// </history>
    public async static Task<List<ZoneTransfer>> GetZonesTransfer()
    {
      List < ZoneTransfer > zoneTransfer  = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          zoneTransfer = dbContext.USP_OR_GetZonesTransfer().ToList();
        }
      });
      return zoneTransfer;
    }
    #endregion

    #region getZones
    /// <summary>
    /// OObtiene registros del catalogo Zones
    /// </summary>
    /// <param name="nStatus">-1 Todos | 0. inactivos | 1. Activos</param>
    /// <param name="zone">objeto con filtros adicionales</param>
    /// <returns>Lista de tipo ZOne</returns>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    public static async Task<List<Zone>> GetZones(int nStatus = -1, Zone zone = null)
    {
      List<Zone> lstZones = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from zn in dbContext.Zones
                      select zn;

          if (nStatus != -1)//filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(zn => zn.znA == blnStatus);
          }

          if (zone != null)
          {
            if (!string.IsNullOrWhiteSpace(zone.znID))//filtro por id
            {
              query = query.Where(zn => zn.znID == zone.znID);
            }

            if (!string.IsNullOrWhiteSpace(zone.znN))//Filtro por descripción
            {
              query = query.Where(zn => zn.znN.Contains(zone.znN));
            }
          }

          return query.OrderBy(zn => zn.znN).ToList();
        }
      });

      return lstZones;
    }
    #endregion

    #region SaveZone
    /// <summary>
    /// Guarda un registro en el catalogo Zone
    /// </summary>
    /// <param name="zone">objeto a guardar</param>
    /// <param name="lstAdd">Lista a asignar</param>
    /// <param name="lstDel">Lista a eliminar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>-1. Extiste un registro con el mismo ID | 0. No se guardó | 1. Se guardó</returns>
    /// <history>
    /// [emoguel] created 09/06/2016
    /// </history>
    public async static Task<int> SaveZone(Zone zone, List<LeadSource> lstAdd, List<LeadSource> lstDel, bool blnUpdate)
    {
      int nRes = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
              try
              {

                #region Update
                if (blnUpdate)
                {
                  dbContext.Entry(zone).State = System.Data.Entity.EntityState.Modified;
                }
                #endregion
                #region Add
                else
                {
                  if (dbContext.Zones.Where(zn => zn.znID == zone.znID).FirstOrDefault() != null)
                  {
                    return -1;
                  }
                  else
                  {
                    dbContext.Zones.Add(zone);
                  }
                }
                #endregion

                #region LeadSources
                //Add
                dbContext.LeadSources.AsEnumerable().Where(ls => lstAdd.Any(lss => lss.lsID == ls.lsID)).ToList().ForEach(ls =>
                {
                  ls.lszn = zone.znID;
                });
                //Delete
                dbContext.LeadSources.AsEnumerable().Where(ls => lstDel.Any(lss => lss.lsID == ls.lsID)).ToList().ForEach(ls =>
                {
                  ls.lszn = null;
                });
                #endregion

                int nSave = dbContext.SaveChanges();
                transacction.Commit();
                return nSave;
              }
              catch
              {
                transacction.Rollback();
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

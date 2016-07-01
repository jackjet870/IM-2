using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;
using IM.Model.Classes;

namespace IM.BusinessRules.BR
{
  public class BRSegmentsByAgency
  {
    #region GetSegmentsByAgecy
    /// <summary>
    /// Devuelve la lista de SegmentByAgcy
    /// </summary>
    /// <param name="segmentByAgency">Objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0.Registros Inactivos | 1.Registros Activos</param>
    /// <returns></returns>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// [emoguel] modified 31/05/2016--->Se volvió async
    /// </history>
    public async static Task<List<SegmentByAgency>> GetSegMentsByAgency(SegmentByAgency segmentByAgency=null, int nStatus = -1)
    {
      List<SegmentByAgency> lstSegmentsByAgency = new List<SegmentByAgency>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from sba in dbContext.SegmentsByAgencies
                      select sba;

          if (nStatus != -1)//Filtro por Estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(sba => sba.seA == blnStatus);
          }

          if (segmentByAgency != null)//Valida si se tiene un objeto
          {
            if (!string.IsNullOrWhiteSpace(segmentByAgency.seID))//Filtro por ID
            {
              query = query.Where(sba => sba.seID == segmentByAgency.seID);
            }

            if (!string.IsNullOrWhiteSpace(segmentByAgency.seN))//Fitro por nombre(Descripcion)
            {
              query = query.Where(sba => sba.seN.Contains(segmentByAgency.seN));
            }
          }

          lstSegmentsByAgency = query.OrderBy(sba => sba.seN).ToList();
        }
      });
      return lstSegmentsByAgency;
    }
    #endregion

    #region SaveSegmentgByAgency
    /// <summary>
    /// Guarda un registro en el catalogo Segments By Agency
    /// Desasigna|Asigna Agencias a un segment
    /// </summary>
    /// <param name="segmentByAgency">OBjeto a guardar</param>
    /// <param name="lstAdd">Lista a asignar</param>
    /// <param name="lstDel">Lista a desasignar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>-1. Existe un registro con el mismo ID | 0. No se guardó | 1. Se guardó</returns>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    public async static Task<int> SaveSegmentByAgency(SegmentByAgency segmentByAgency,List<Agency> lstAdd, List<Agency> lstDel,bool blnUpdate)
    {
      int nRes = await Task.Run(() => {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region Update
              if (blnUpdate)
              {
                dbContext.Entry(segmentByAgency).State = System.Data.Entity.EntityState.Modified;
              }
              #endregion
              #region Add
              else
              {
                if (dbContext.SegmentsByAgencies.Where(se => se.seID == segmentByAgency.seID).FirstOrDefault() != null)//Existe un registro
                {
                  return -1;
                }
                else//Agregar
                {
                  #region GetPosition
                  var items = (from sa in dbContext.SegmentsByAgencies
                               select new Item
                               {
                                 Id = sa.seO.ToString(),
                                 UserId = sa.seID,
                                 Name = sa.seN,
                                 By = "Agency"
                               }).Union(from sl in dbContext.SegmentsByLeadSources
                                        select new Item
                                        {
                                          Id = sl.soO.ToString(),
                                          UserId = sl.soID,
                                          Name = sl.soN,
                                          By = "Lead Source"
                                        }).ToList().OrderBy(it => int.Parse(it.Id)).ToList();
                  var item = items.LastOrDefault();
                  segmentByAgency.seO = Convert.ToInt32(item.Id) + 1; 
                  #endregion
                  dbContext.SegmentsByAgencies.Add(segmentByAgency);
                }
              }
              #endregion

              #region Agencies
              //Asignar
              dbContext.Agencies.AsEnumerable().Where(ag => lstAdd.Any(agg => agg.agID == ag.agID)).ToList().ForEach(ag =>
              {
                ag.agse = segmentByAgency.seID;
              });

              //Desasignar
              dbContext.Agencies.AsEnumerable().Where(ag => lstDel.Any(agg => agg.agID == ag.agID)).ToList().ForEach(ag =>
              {
                ag.agse = null;
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

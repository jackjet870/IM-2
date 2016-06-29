using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;
using IM.Model.Classes;

namespace IM.BusinessRules.BR
{
  public class BRSegmentsByLeadSource
  {
    #region GetSegmentsByLeadSource
    /// <summary>
    /// Obtiene registros del catalogo SegmentByLeadSource
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="segment">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo SegmentByLeadSource</returns>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    public async static Task<List<SegmentByLeadSource>> GetSegmentsByLeadSource(int nStatus = -1, SegmentByLeadSource segment = null)
    {
      List<SegmentByLeadSource> lstSegments = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from so in dbContext.SegmentsByLeadSources
                      select so;

          if (nStatus != -1)//Filtro por Estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(so => so.soA == blnStatus);
          }

          if (segment != null)
          {
            if (!string.IsNullOrWhiteSpace(segment.soID))//Filtro por ID
            {
              query = query.Where(so => so.soID == segment.soID);
            }

            if (!string.IsNullOrWhiteSpace(segment.soN))//Filtro por descripción
            {
              query = query.Where(so => so.soN.Contains(segment.soN));
            }
          }

          return query.OrderBy(so => so.soN).ToList();
        }
      });

      return lstSegments;
    }
    #endregion

    #region SaveSegmentByLeadSource
    /// <summary>
    /// Guarda un registro en el catalogo Segments By Agency
    /// Desasigna|Asigna Agencias a un segment
    /// </summary>
    /// <param name="segmentByLeadSource">OBjeto a guardar</param>
    /// <param name="lstAdd">Lista a asignar</param>
    /// <param name="lstDel">Lista a desasignar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>-1. Existe un registro con el mismo ID | 0. No se guardó | 1. Se guardó</returns>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    public async static Task<int> SaveSegmentByLeadSource(SegmentByLeadSource segmentByLeadSource, List<LeadSource> lstAdd, List<LeadSource> lstDel, bool blnUpdate)
    {
      int nRes = await Task.Run(() => {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region Update
              if (blnUpdate)
              {
                dbContext.Entry(segmentByLeadSource).State = System.Data.Entity.EntityState.Modified;
              }
              #endregion
              #region Add
              else
              {
                if (dbContext.SegmentsByLeadSources.Where(so => so.soID == segmentByLeadSource.soID).FirstOrDefault() != null)//Existe un registro
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
                  segmentByLeadSource.soO = Convert.ToInt32(item.Id) + 1;
                  #endregion
                  dbContext.SegmentsByLeadSources.Add(segmentByLeadSource);
                }
              }
              #endregion

              #region Agencies
              //Asignar
              dbContext.LeadSources.AsEnumerable().Where(ls => lstAdd.Any(lss => lss.lsID == ls.lsID)).ToList().ForEach(ls =>
              {
                ls.lsso = segmentByLeadSource.soID;
              });

              //Desasignar
              dbContext.LeadSources.AsEnumerable().Where(ls => lstDel.Any(lss => lss.lsID == ls.lsID)).ToList().ForEach(ls =>
              {
                ls.lsso = null;
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

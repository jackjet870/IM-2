using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Classes;

namespace IM.BusinessRules.BR
{
  public class BRSegmentsCategories
  {
    #region GetSegmentsCategories
    /// <summary>
    /// Obtiene registros del catalogo SegmentCategories
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="segmenCategory">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo SegmentCategory</returns>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    public async static Task<List<SegmentCategory>> GetSegmentsCategories(int nStatus = -1, SegmentCategory segmenCategory = null)
    {
      List<SegmentCategory> lstSegmentsCategories = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            var query = from sc in dbContext.SegmentsCategories
                        select sc;

            if (nStatus != -1)//Filtro por estatus
            {
              bool blnStatus = Convert.ToBoolean(nStatus);
              query = query.Where(sc => sc.scA == blnStatus);
            }

            if (segmenCategory != null)//Verificamos que tengamos un objeto
            {
              if (!string.IsNullOrWhiteSpace(segmenCategory.scID))//filtro por ID
              {
                query = query.Where(sc => sc.scID == segmenCategory.scID);
              }

              if (!string.IsNullOrWhiteSpace(segmenCategory.scN))//Filtro por Descripción
              {
                query = query.Where(sc => sc.scN.Contains(segmenCategory.scN));
              }
            }

            return query.OrderBy(sc => sc.scN).ToList();
          }

        });

      return lstSegmentsCategories;
    }
    #endregion

    #region SaveSegmentCategory
    /// <summary>
    /// Guarda un SegmentCategory
    /// </summary>
    /// <param name="segmentCategory">OBjeto a guardar</param>
    /// <param name="lstAdd">Lista a asignar</param>
    /// <param name="lstDel">Lista a desasignar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>-1. Existe un registro con el mismo ID| 0. Nno se guardó | 1. se guardó </returns>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    public static async Task<int> SaveSegmentCategory(SegmentCategory segmentCategory,List<Item>lstAdd,List<Item>lstDel,bool blnUpdate)
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
                  dbContext.Entry(segmentCategory).State = System.Data.Entity.EntityState.Modified;
                }
                #endregion
                #region Add
                else
                {
                  if(dbContext.SegmentsCategories.Where(sc=>sc.scID==segmentCategory.scID).FirstOrDefault()!=null)
                  {
                    return -1;
                  }
                  else
                  {
                    int maxvalue = dbContext.SegmentsCategories.Max(sc => sc.scO);
                    segmentCategory.scO = maxvalue + 1;
                    dbContext.SegmentsCategories.Add(segmentCategory);
                  }
                }
                #endregion

                #region Segments Add
                dbContext.SegmentsByAgencies.AsEnumerable().Where(se => lstAdd.Any(it => it.UserId == se.seID && it.By == "Agency")).ToList().ForEach(se =>
                      {
                        se.sesc = segmentCategory.scID;
                      });

                dbContext.SegmentsByLeadSources.AsEnumerable().Where(so => lstAdd.Any(it => so.soID == it.UserId && it.By != "Agency")).ToList().ForEach(so =>
                      {
                        so.sosc = segmentCategory.scID;
                      });
                #endregion

                #region Segments Del
                dbContext.SegmentsByAgencies.AsEnumerable().Where(se => lstDel.Any(it => it.UserId == se.seID && it.By == "Agency")).ToList().ForEach(se =>
                {
                  se.sesc = null;
                });

                dbContext.SegmentsByLeadSources.AsEnumerable().Where(so => lstDel.Any(it => so.soID == it.UserId && it.By != "Agency")).ToList().ForEach(so =>
                {
                  so.sosc = null;
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

    #region ChangeSegmentsCategoryOrder
    /// <summary>
    /// Actualiza la posición de los registros de segmentcategory
    /// </summary>
    /// <param name="lstSegmentsCategoriesOrder">Lista de SegmentCategory</param>
    /// <returns>0. No se guardó | >0. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 03/06/2016
    /// </history>
    public static async Task<int> ChangeSegmentsCategoryOrder(List<SegmentCategory> lstSegmentsCategoriesOrder)
    {
      int nRes = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              lstSegmentsCategoriesOrder.ForEach(sc =>
              {
                dbContext.Entry(sc).State = System.Data.Entity.EntityState.Modified;
              });
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Classes;

namespace IM.BusinessRules.BR
{
  public class BRSegmentsOrder
  {
    #region GetSegmentsOrder
    /// <summary>
    /// Obtiene la lista de segmentsOrder
    /// </summary>
    /// <returns>Lista de tipo Object</returns>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    public async static Task<List<Item>> GetSegmentsOrder()
    {
      List<Item> lsObjects = new List<Item>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = (from sa in dbContext.SegmentsByAgencies
                       select new Item
                       {
                         Id = sa.seO.ToString(),
                         UserId = sa.seID,
                         Name = sa.seN,
                         By = "Agency",
                         Category=sa.sesc
                       }).Union(from sl in dbContext.SegmentsByLeadSources
                                select new Item
                                {
                                  Id = sl.soO.ToString(),
                                  UserId = sl.soID,
                                  Name = sl.soN,
                                  By = "Lead Source",
                                  Category=sl.sosc
                                });
          lsObjects = query.ToList();
        }
      });

      return lsObjects;
    }
    #endregion

    #region SaveSegmentsOrder
    /// <summary>
    /// Actualiza el orden de los registros de segmentByAgency y SegmentByLeadSource
    /// </summary>
    /// <param name="lstAgency">Lista de segmentByAgency</param>
    /// <param name="lstLeadSource">Lista de LeadSource</param>
    /// <returns>0. No se guardó | >0. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 01/06/2016
    /// </history>
    public async static Task<int> SaveSegmentsOrder(List<Item> lstAgency,List<Item>lstLeadSource)
    {
      int nRes = await Task.Run(() => {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        { 
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              lstAgency.ForEach(it => {
                var segmentA = dbContext.SegmentsByAgencies.Where(se => se.seID == it.UserId).FirstOrDefault();
                segmentA.seO = Convert.ToInt32(it.Id);
              });              

              lstLeadSource.ForEach(it =>
              {
                var segmentLS = dbContext.SegmentsByLeadSources.Where(so => so.soID == it.UserId).FirstOrDefault();
                segmentLS.soO = Convert.ToInt32(it.Id);
              });

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

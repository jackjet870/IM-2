using IM.Model;
using IM.Model.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public static class BRShowSalesman
  {
    #region GetShowsSalesmen

    /// <summary>
    /// Carga los shows de vendedores
    /// </summary>
    /// <param name="guestId">Guest ID</param>
    /// <returns><list type="ShowSalesman"></list></returns>
    /// <history>
    /// [aalcocer] 10/08/2016  Created.
    /// </history>
    public static async Task<List<ShowSalesman>> GetShowsSalesmen(int guestId)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.ShowsSalesmen.Where(ss => ss.shgu == guestId).Include(ss => ss.Personnel).ToList();
        }
      });
    }

    #endregion GetShowsSalesmen

    #region SaveShowsSalesmen

    /// <summary>
    /// Guarda los shows de vendedores
    /// </summary>
    /// <param name="guestId">Guest ID</param>
    /// <param name="lstShowSalesman">Lista de shows de vendedores</param>
    /// <history>
    /// [aalcocer] created 10/08/2016
    /// </history>
    public static async Task<int> SaveShowsSalesmen(int guestId, List<ShowSalesman> lstShowSalesman)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              //Del
              dbContext.ShowsSalesmen.RemoveRange(dbContext.ShowsSalesmen.Where(ss => ss.shgu == guestId));

              //Add
              lstShowSalesman.ForEach(ss =>
              {
                ShowSalesman showSalesman = ObjectHelper.CopyProperties(ss);
                dbContext.Entry(showSalesman).State = EntityState.Added;
              });

              var nSave = dbContext.SaveChanges();
              transacction.Commit();
              return nSave;
            }
            catch
            {
              transacction.Rollback();
              throw;
            }
          }
        }
      });
    }

    #endregion SaveShowsSalesmen
  }
}
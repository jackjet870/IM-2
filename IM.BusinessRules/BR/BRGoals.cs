using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace IM.BusinessRules.BR
{

  public class BRGoals
  {
    #region GetGoals
    /// <summary>
    /// Obtiene registros del catalogo Goal
    /// </summary>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    public static List<Goal> GetGoals(Goal goal)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from g in dbContext.Goals
                    select g;

        if (!string.IsNullOrWhiteSpace(goal.gopy))
        {
          query = query.Where(g => g.gopy == goal.gopy);
        }
        if (!string.IsNullOrWhiteSpace(goal.gopd))
        {
          query = query.Where(g => g.gopd == goal.gopd);
        }

        List<Goal> lstGoals = query.ToList().Where(g => g.goDateFrom.ToString("yyyyMMdd") == goal.goDateFrom.ToString("yyyyMMdd") && g.goDateTo.ToString("yyyyMMdd") == goal.goDateTo.ToString("yyyyMMdd")).ToList();

        return lstGoals;
      }
    }
    #endregion

    #region SaveGoal
    /// <summary>
    /// Agrega|Actualiza|Elimina registros en el catalogo Goals
    /// </summary>
    /// <param name="goal">objeto con los datos nuevos a guardar</param>
    /// <param name="lstAdd">Registros a agregar en la BD</param>
    /// <param name="lstUpdate">Registros a actualizar en la BD</param>
    /// <param name="lstDel">Registros a eliminar de la BD</param>
    /// <returns>0. No se guardó | >0. Se guardaron correctamente </returns>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    public static int SaveGoal(Goal goal, List<Goal> lstAdd, List<Goal> lstUpdate, List<Goal> lstDel)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
          try
          {

            #region Add            
            lstAdd.ForEach(go =>
            {
              go.goDateFrom = goal.goDateFrom;
              go.goDateTo = goal.goDateTo;
              go.gopd = goal.gopd;
              go.gopy = goal.gopy;
              dbContext.Entry(go).State = EntityState.Added;
            });
            #endregion

            #region Update
            lstUpdate.ForEach(go =>
            {
              go.goDateFrom = goal.goDateFrom;
              go.goDateTo = goal.goDateTo;
              go.gopd = goal.gopd;
              go.gopy = goal.gopy;
              dbContext.Entry(go).State = EntityState.Modified;
            });
            #endregion

            #region Delete
            lstDel.ForEach(go=> {
              dbContext.Entry(go).State = EntityState.Deleted;
            });            
            #endregion

            int nRes = dbContext.SaveChanges();
            transacction.Commit();
            return nRes;
          }
          catch(DbEntityValidationException e)
          {
            transacction.Rollback();
            return 0;
          }
        }
      }
    } 
    #endregion
  }
}

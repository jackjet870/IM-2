using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRPostsLog
  {
    #region GetPostLog
    /// <summary>
    /// Obtiene registros del catalogo PostLogs
    /// </summary>
    /// <param name="postLog">Objeto con filtros adicionales</param>
    /// <param name="blnDate">True. filtra pos ppDT</param>
    /// <returns>Lista de tipo PostLog</returns>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    public static List<PostLog> GetPostsLog(PostLog postLog = null,bool blnDate=false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from pp in dbContext.PostsLogs.Include("Personnel").Include("Personnel_ChangedBy").Include("Post")
                    select pp;

        #region Filtros
        if (postLog != null)//verficamos que se tenga un objeto
        {
          if (blnDate)//Filtro por fecha
          {
            query = query.Where(pp => DbFunctions.TruncateTime(pp.ppDT).Value == DbFunctions.TruncateTime(postLog.ppDT).Value);
          }

          if (!string.IsNullOrWhiteSpace(postLog.ppChangedBy))//Filtro por changedBy
          {
            query = query.Where(pp => pp.ppChangedBy == postLog.ppChangedBy);
          }

          if (!string.IsNullOrWhiteSpace(postLog.pppe))//Filtro por Perssonel
          {
            query = query.Where(pp => pp.pppe == postLog.pppe);
          }
        }
        #endregion
        
        return query.OrderByDescending(pp => pp.ppDT).ToList();
      }
    }
    #endregion

    #region SavePostLog
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo PostsLog
    /// </summary>
    /// <param name="postLog">objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>0. No se guardó | 1. Se guardó</returns>
    /// <history>
    /// [emoguel] created 12/04/2016
    /// </history>
    public static int SavePostLog(PostLog postLog, bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Entry(postLog).State=(blnUpdate)?EntityState.Modified:EntityState.Added;                
        return dbContext.SaveChanges();
      }
    }
    #endregion

    #region DeletePostLog
    /// <summary>
    /// Elimina registros del catalogo PostLogs
    /// </summary>
    /// <param name="lstPostsLog">Lista de PostLog a Eliminar</param>
    /// <returns>0. No se elimino nada | >0. Se eliminaron registros</returns>
    /// <history>
    /// [emoguel] created 13/04/2016
    /// </history>
    public static int DeletePostsLog(List<PostLog> lstPostsLog)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        lstPostsLog.ForEach(pp => {
          dbContext.Entry(pp).State = EntityState.Deleted;
          });
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}

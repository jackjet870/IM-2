using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRNotices
  {
    #region GetNotices
    /// <summary>
    /// Trae todas las noticias del día y del Hotel
    /// </summary>
    /// <param name="leadSource">Hotel en el que se cargo el  modulo</param>
    /// <param name="date">Fecha currente</param>
    /// <history>
    /// [jorcanche] created 18/04/2016 Created
    /// [jorcanche] se agrego asincronia 08/07/2016 
    /// </history>
    public static async Task<List<NoticeShort>> GetNotices(string leadSource, DateTime? date)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_GetNotices(leadSource, date).ToList();
        }
      });
    }
    #endregion

    #region GetNotices
    /// <summary>
    /// obtiene registros del catalogo notices
    /// </summary>
    /// <param name="nStatus">-1.Todos | 0.Inactivos | 1.Activos</param>
    /// <param name="notice">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo Notice</returns>
    /// <history>
    /// [emoguel] created 23/07/2016
    /// </history>
    public static async Task<List<Notice>> GetNotices(int nStatus = -1, Notice notice = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from no in dbContext.Notices
                      select no;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(no => no.noA == blnStatus);
          }

          if (notice != null)
          {
            if (notice.noID > 0)//Filtro por ID
            {
              query = query.Where(no => no.noID == notice.noID);
            }

            if (!string.IsNullOrWhiteSpace(notice.noTitle))//Filtro por descripción
            {
              query = query.Where(no => no.noTitle.Contains(notice.noTitle));
            }
          }

          return query.OrderBy(no => no.noTitle).ToList();
        }
      });
    }
    #endregion

    #region SaveNotice
    /// <summary>
    /// Guarda un notice y sus Lead Sources
    /// </summary>
    /// <param name="notice">objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <param name="lstAddLeadSources">LeadSource a agregar</param>
    /// <param name="lstDelLeadSources">Leadsource a eliminar</param>
    /// <returns>-1. Existe un registro con el mismo ID | 0. No se guardó | >0. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 27/06/2016
    /// </history>
    public static async Task<int> SaveNotice(Notice notice, bool blnUpdate, List<LeadSource> lstAddLeadSources, List<LeadSource> lstDelLeadSources)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              int nSave = 0;
              Notice noticeSave = new Notice();
              #region Save Notice
              #region Update
              if (blnUpdate)
              {
                noticeSave = dbContext.Notices.Where(no => no.noID == notice.noID).Include(no => no.LeadSources).FirstOrDefault();
                ObjectHelper.CopyProperties(noticeSave, notice);
              }
              #endregion
              #region Add
              else
              {
                if (dbContext.Notices.Where(no => no.noID == notice.noID).FirstOrDefault() != null)
                {
                  return -1;
                }
                else
                {
                  dbContext.Notices.Add(notice);
                  nSave = dbContext.SaveChanges();
                  noticeSave = notice;
                }
              }
              #endregion
              #endregion

              #region LeadSource
              //Eliminar Lead Source
              noticeSave.LeadSources.Where(ls => lstDelLeadSources.Any(lss => ls.lsID == lss.lsID)).ToList().ForEach(ls =>
                    {
                      noticeSave.LeadSources.Remove(ls);
                    });

              //Agregar Lead Source
              var lstAdd = dbContext.LeadSources.AsEnumerable().Where(ls => lstAddLeadSources.Any(lss => ls.lsID == lss.lsID)).ToList();
              lstAdd.ForEach(ls =>
              {
                noticeSave.LeadSources.Add(ls);
              });
              #endregion

              nSave += dbContext.SaveChanges();
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
    #endregion

  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRLanguages
  {
    #region GetLanguages

    /// <summary>
    /// Obtiene el catalogo de idiomas
    /// </summary
    /// <param name="status">0- Sin filtro, 1-Activos, 2. Inactivos</param>
    /// <returns>List<Model.GetLanguages></returns>
    /// <history>
    /// [aalcocer] 24/02/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async static Task<List<LanguageShort>> GetLanguages(int status)
    {
      List<LanguageShort> result = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          result = dbContext.USP_OR_GetLanguages(Convert.ToByte(status)).ToList();
        }
      });
      return result;
    }

    #region GetLanguages
    /// <summary>
    /// Obtiene registros del catalogo Languages
    /// </summary>
    /// <param name="language">Objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1.Registros activos</param>
    /// <returns>Lista de tipo Language</returns>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// </history>
    public async static  Task<List<Language>> GetLanguages(Language language = null, int nStatus = -1)
    {
      List<Language> lstLanguages = new List<Language>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from la in dbContext.Languages
                      select la;
          if (nStatus != -1)//Filtro por estatus
          {
            bool blnEstatus = Convert.ToBoolean(nStatus);
            query = query.Where(la => la.laA == blnEstatus);
          }

          if (language != null)//Se verifica que se tenga un objeto con filtros
          {
            if (!string.IsNullOrWhiteSpace(language.laID))//Filtro por ID
            {
              query = query.Where(la => la.laID == language.laID);
            }

            if (!string.IsNullOrWhiteSpace(language.laN))//Filtro por descripcion
            {
              query = query.Where(la => la.laN.Contains(language.laN));
            }
          }

          lstLanguages= query.OrderBy(la => la.laN).ToList();
        }
      });
      return lstLanguages;
    }
    #endregion
    #endregion
  }
}
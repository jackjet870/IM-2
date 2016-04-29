using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity.Validation;

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
    ///   [aalcocer] 24/02/2016 Created
    /// </history>
    public static List<LanguageShort> GetLanguages(int status)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetLanguages(Convert.ToByte(status)).ToList();
      }
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
    public static List<Language> GetLanguages(Language language = null, int nStatus = -1)
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

        return query.OrderBy(la => la.laN).ToList();
      }
    }
    #endregion
    #endregion
  }
}
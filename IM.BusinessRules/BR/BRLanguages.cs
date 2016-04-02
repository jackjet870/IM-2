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

    #region SaveLanguages
    /// <summary>
    /// Actualiza|Guarda un registro en el catalogo Languages
    /// </summary>
    /// <param name="language">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>0. No se guardó | 1. registro guardado | 2. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// </history>
    public static int SaveLanguage(Language language,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)//Actualizar
        {
          dbContext.Entry(language).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Insert
        else//insertar
        {
          Language languageVal = dbContext.Languages.Where(la => la.laID == language.laID).FirstOrDefault();
          if(languageVal!=null)//Validamos si existe un objeto con el mismo nombre}
          {
            return 2;//Existe un registro con el mismo nombre
          }
          else
          {
            dbContext.Languages.Add(language);            
          }
        }
        #endregion        
        return dbContext.SaveChanges();        
      }
    }
    #endregion
  }
}
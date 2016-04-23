using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRGiftsCategories
  {

    #region GetGiftsCategories
    /// <summary>
    /// obtiene registros del catalogo GiftsCategs
    /// </summary>
    /// <param name="giftCategory">Objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1.Registros activos</param>
    /// <returns>Lista de tipo GiftCategory</returns>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    public static List<GiftCategory> GetGiftsCategories(GiftCategory giftCategory = null, int nStatus = -1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from gc in dbContext.GiftsCategories
                    select gc;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(gc => gc.gcA == blnEstatus);
        }

        if (giftCategory != null)//Verificamos si se tiene objeto
        {
          if (!string.IsNullOrWhiteSpace(giftCategory.gcID))//Filtro por ID
          {
            query = query.Where(gc => gc.gcID == giftCategory.gcID);
          }

          if (!string.IsNullOrWhiteSpace(giftCategory.gcN))//Filtro por descripcion
          {
            query = query.Where(gc => gc.gcN.Contains(giftCategory.gcN));
          }
        }

        return query.OrderBy(gc => gc.gcN).ToList();
      }
    }
    #endregion

    #region SaveGiftCategory
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo GiftsCategs
    /// </summary>
    /// <param name="giftCategory">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Agrega</param>
    /// <returns>0. No se guardó | 1.Se guardó correctamente | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    public static int SaveGiftCategory(GiftCategory giftCategory,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Actualizar
        if (blnUpdate)//Si es actualizar
        {
          dbContext.Entry(giftCategory).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Insertar
        else//Si es insertar
        {
          GiftCategory giftCategoryVal = dbContext.GiftsCategories.Where(gc => gc.gcID == giftCategory.gcID).FirstOrDefault();
          if(giftCategoryVal!=null)//Si existe un registro con el mismo ID
          {
            return -1;
          }
          else//Se agrega
          {
            dbContext.GiftsCategories.Add(giftCategory);
          }
        }
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}

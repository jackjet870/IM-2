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
  }
}

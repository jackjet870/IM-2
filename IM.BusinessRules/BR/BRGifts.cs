using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRGifts
  {
    #region GetGifts

    /// <summary>
    /// Método para obtener una lista de Regalos por lugar y/o estatus.
    /// </summary>
    /// <param name="location">Lugar o Almacén</param>
    /// <param name="Status">0. Sin filtro.
    /// 1. Activos.
    /// 2. Inactivos.</param>
    /// <history>
    /// [edgrodriguez] 24/Feb/2016 Created
    /// </history>
    public static List<GiftShort> GetGifts(string location = "ALL", int Status = 0)
    {
      List<GiftShort> lstgetGifs = new List<GiftShort>();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //dbContext.Database.CommandTimeout = 180;
        var dx = dbContext.USP_OR_GetGifts(location, Convert.ToByte(Status));
        lstgetGifs = dx.ToList();
      }
      return lstgetGifs;
    }

    #endregion

    #region GetGiftsCategories
    /// <summary>
    /// Método para obtener una lista de categorias de regalo.
    /// </summary>
    /// <param name="Status">0. Sin filtro.
    /// 1. Activos.
    /// 2. Inactivos.</param>
    /// <returns> List<GiftsCateg> </returns>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    public static List<GiftCategory> GetGiftsCategories(int Status = 0)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstGiftsCateg = dbContext.GiftsCategories;
        switch (Status)
        {
          case 1:
            return lstGiftsCateg.Where(c => c.gcA == true).ToList();
          case 2:
            return lstGiftsCateg.Where(c => c.gcA == false).ToList();
          default:
            return lstGiftsCateg.ToList();
        }
      } 
    }
    #endregion
  }
}

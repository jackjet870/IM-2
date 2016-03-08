using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

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
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetGifts(location, Convert.ToByte(Status)).ToList();
      }
    }

    #endregion
  }
}

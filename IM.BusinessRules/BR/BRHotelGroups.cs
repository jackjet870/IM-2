using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRHotelGroups
  {

    #region GetHotelGroups
    /// <summary>
    /// Obtiene registros del catalogo HotelGroups
    /// </summary>
    /// <param name="hotelGroup">Objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1. Registros Activos</param>
    /// <returns>Lista de tipo HotelGroup</returns>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    public static List<HotelGroup> GetHotelGroups(HotelGroup hotelGroup = null, int nStatus = -1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from hg in dbContext.HotelsGroups
                    select hg;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(hg => hg.hgA == blnEstatus);
        }

        if (hotelGroup != null)//Verificamos si tenemos un objeto
        {
          if (!string.IsNullOrWhiteSpace(hotelGroup.hgID))//Filtro por ID
          {
            query = query.Where(hg => hg.hgID == hotelGroup.hgID);
          }

          if (!string.IsNullOrWhiteSpace(hotelGroup.hgN))//Filtro por descripcion
          {
            query = query.Where(hg => hg.hgN.Contains(hotelGroup.hgN));
          }
        }

        return query.OrderBy(hg => hg.hgN).ToList();
      }
    } 
    #endregion

  }
}

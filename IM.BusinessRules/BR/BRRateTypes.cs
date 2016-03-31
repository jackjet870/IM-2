using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRRateTypes
  {
    #region GetRateType
    /// <summary>
    /// 
    /// </summary>
    /// <param name="raID">Identificador de RateType</param>
    /// <param name="raA"> Estatus de RateType</param>
    /// <param name="raIDMayorA"> Estatus mayor a un ID especificado </param>
    /// <param name="orderByraN"> Estatus para ordenar por raN </param>
    /// <returns> List<RateType> </returns>
    /// <history>
    /// [edgrodriguez] 08/03/2016 Created
    /// [vipacheco] 18/03/2016 Modified --> Se agregó parametro raIDMayorA para la busqueda mayor a ese ID especificado
    /// </history>
    public static List<RateType> GetRateType(int? raID = null, bool? raA = null, bool raIDMayorA = false, bool orderByraN = false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstRateTypes = dbContext.RateTypes.ToList();
        if (raID != null && raID >= 0)
        {
          if (raIDMayorA)
            lstRateTypes = lstRateTypes.Where(c => c.raID > Convert.ToInt32(raID)).ToList();
          else
          lstRateTypes = lstRateTypes.Where(c => c.raID == Convert.ToInt32(raID)).ToList();
        }
        if (raA != null)
          lstRateTypes = lstRateTypes.Where(c => c.raA == Convert.ToBoolean(raA)).ToList();

        if (orderByraN)
          lstRateTypes = lstRateTypes.OrderBy(c => c.raN).ToList();

        return lstRateTypes;
      }
    } 
    #endregion
  }
}

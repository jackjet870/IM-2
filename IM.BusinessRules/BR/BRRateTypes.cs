using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRRateTypes
  {
    #region getRateType
    /// <summary>
    /// 
    /// </summary>
    /// <param name="raID">Identificador de RateType</param>
    /// <param name="raA"> Estatus de RateType</param>
    /// <returns> List<RateType> </returns>
    /// <history>
    /// [edgrodriguez] 08/03/2016 Created
    /// </history>
    public static List<RateType> getRateType(int? raID = null, bool? raA = null)
    {
      using (var dbContext = new IMEntities())
      {
        var lstRateTypes = dbContext.RateTypes.ToList();
        if (raID != null && raID >= 0)
          lstRateTypes = lstRateTypes.Where(c => c.raID == Convert.ToInt32(raID)).ToList();

        if (raA != null)
          lstRateTypes = lstRateTypes.Where(c => c.raA == Convert.ToBoolean(raA)).ToList();

        return lstRateTypes;
      }
    } 
    #endregion
  }
}

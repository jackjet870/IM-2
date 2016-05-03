using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;

namespace IM.BusinessRules.BR
{
   public class BREfficiency
  {
    #region GetEffificencyBySr

    /// <summary>
    /// Obtiene una lista de Efficiency 
    /// </summary>
    /// <param name="sr">Sales Room</param>
    /// <param name="dateFrom">Fecha de inicio </param>
    /// <param name="dateTo">Fecha final</param>
    /// <history>
    /// [ecanul] 26/04/2016 Created
    /// </history>
    public static List<Efficiency> GetEffificencyBySr(string sr, DateTime dateFrom, DateTime dateTo)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from ef in dbContext.Efficiencies
          where ef.efsr == sr &&
                ((ef.efDateFrom.Year == dateFrom.Year && ef.efDateFrom.Month == dateFrom.Month) ||
                 (ef.efDateTo.Year == dateTo.Year && ef.efDateTo.Month == dateTo.Month))
                && ef.efDateTo < dateTo
          orderby ef.efDateFrom
          select (ef);
        return query.ToList();
      }
    }

    #endregion
  }
}

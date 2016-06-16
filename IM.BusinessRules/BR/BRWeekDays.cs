using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRWeekDays
  {
    #region GetWeekDays
    /// <summary>
    ///   Obtienes los dia de la semana en el lenguaje especificado
    /// </summary>
    /// <param name="lan"></param>
    /// <returns></returns>
    /// <history>
    ///   [vku] 11/Jun/2016 Created
    /// </history>
    public async static Task<List<WeekDay>> GetWeekDays(string lan)
    {
      List<WeekDay> lstWeekDays = new List<WeekDay>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from weekdays in dbContext.WeekDays select weekdays;
          if (lan != string.Empty)
            {
              if(!string.IsNullOrWhiteSpace(lan))
              query = query.Where(wd => wd.wdla == lan);
            }
          lstWeekDays = query.OrderBy(wd => wd.wdDay).ToList();
        }
      });
      return lstWeekDays;
    }
    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Base.Helpers
{
  public class DateHelper
  {

    /// <summary>
    /// Obtiene un rango de fecha formateado
    /// </summary>
    /// <param name="dateFrom">Fecha inicio</param>
    /// <param name="dateTo">Fecha Fin</param>
    /// <returns>Rango de fecha formateada (string) </returns>
    /// <history>
    /// [erosado] 15/Mar/2016 Created
    /// </history>
    public static string DateRange(DateTime dateFrom, DateTime dateTo)
    {
      string dateRange = string.Empty;
      //Comparar si es la misma fecha
      if (dateFrom.Date == dateTo.Date)
      {
        dateRange = string.Format("{0:MMMM d, yyyy}", dateFrom);
      }
      // Si son diferentes años
      else if (dateFrom.Year != dateTo.Year)
      {
        dateRange = string.Format("{0:MMMM d, yyyy} - {1:MMMM d, yyyy}", dateFrom, dateTo);
      }
      //Si son diferentes Meses
      else if (dateFrom.Month != dateTo.Month)
      {
        dateRange = string.Format("{0:MMMM d} - {1:MMMM d, yyyy}", dateFrom, dateTo);
      }
      // Mismo mes y mismo año
      else
      {
        dateRange = string.Format("{0:MMMM d} - {1:d, yyyy}", dateFrom, dateTo);
      }

      return dateRange;
    }
  }
}

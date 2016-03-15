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
      if (dateFrom.ToShortDateString() == dateTo.ToShortDateString())
      {
        dateRange = string.Concat("dd-MMMM-yyyy", dateFrom.ToShortDateString());
      }
      // Si son diferentes años
      else if (dateFrom.Year != dateTo.Year)
      {
        dateRange = string.Concat(dateFrom.ToString("dd-MMMM-yyyy"), " - ", dateTo.ToString("dd-MMMM-yyyy"));
      }
      //Si son diferentes Meses
      else if (dateFrom.Month != dateTo.Month)
      {
        dateRange = string.Concat(dateFrom.ToString("dd"), " de ", dateFrom.ToString("MMMM"), " - ", dateTo.ToString("dd"), " de ", dateTo.ToString("MMMM"), " del ", dateTo.ToString("yyyy"));
      }
      // Mismo mes y mismo año
      else
      {
        dateRange = string.Concat(dateFrom.ToString("dd"), " - ", dateTo.ToString("dd"), " de ", dateTo.ToString("MMMM"), " del ", dateTo.ToString("yyyy"));
      }

      return dateRange;
    }
  }
}

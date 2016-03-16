using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de las diferentes fechas
  /// predefinidas mensuales.
  /// </summary>
  /// <history>
  /// [edgrodriguez] 02/Mar/2016 Created
  /// </history>
  public enum EnumPredefinedDateMonthly
  {
    dmAllDates = -1,
    dmDatesSpecified,
    dmThisMonth,
    dmPreviousMonth,
    dm2MonthsAgo,
    dm3MonthsAgo
  }
}

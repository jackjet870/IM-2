using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de las diferentes fechas
  /// predefinidas.
  /// </summary>
  /// <history>
  /// [edgrodriguez] 02/Mar/2016 Created
  /// </history>
  public enum EnumPredefinedDate
  {
        
    daAllDates = -1,
    daDatesSpecified,
    daToday,
    daYesterday,
    daThisWeek,
    daPreviousWeek,
    daThisHalf,
    daPreviousHalf,
    daThisMonth,
    daPreviousMonth,
    daThisYear,
    daPreviousYear,
    
    // Estas opciones no se despliegan en la lista de fechas predefinidas "sin periodo"
    da2WeeksAgo,
    da3WeeksAgo,
    da2MonthsAgo,
    da3MonthsAgo,
  }
}

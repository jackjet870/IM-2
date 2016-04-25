using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enum que controla el tipo de datos de una columna en excel
  /// </summary>
  /// <history>
  /// [erosado] 12/Mar/2016 Created
  /// [erosado] 17/Mar/2016 Modified Se agregaron los tipos Boolean y Date
  /// </history>
  public enum EnumFormatTypeExcel
  {
    General = 0,
    Percent,
    Currency,
    Number,
    DecimalNumber,
    Boolean,
    Date,
    Time,
    Month
  }
}
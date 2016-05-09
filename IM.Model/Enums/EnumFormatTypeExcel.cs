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
  /// [ecanul] 06/05/2016 Modified Se agregaron los tipos PercentWithCero, NumberWithCero, DecimalNumberWithCero
  /// [ecanuñ] 09/05/2016 Modified Se agrego el valor Id, para enumerar los campos tipo ID
  /// </history>
  public enum EnumFormatTypeExcel
  {
    General = 0,
    Percent,
    PercentWithCero,
    Currency,
    Number,
    NumberWithCero,
    DecimalNumber,
    DecimalNumberWithCero,
    Boolean,
    Date,
    Time,
    Month,
    Id
  }
}
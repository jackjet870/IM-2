using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerador para identificar los tipos de cierres
  /// </summary>
  /// <history>
  /// [vipacheco] 01/03/2016 Created
  /// </history>
  public enum EnumEntities
  {
    [Description("Shows")]
    Shows = 0,
    [Description("Meal Tickets")]
    MealTickets,
    [Description("Sales")]
    Sales,
    [Description("Gifts Receipts")]
    GiftsReceipts,
    CxC
  }
}

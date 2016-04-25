using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de los diferentes tipos de movimientos de huespedes
  /// </summary>
  /// <history>
  /// [jorcanche]  15/Mar/2016 Created
  /// </history>
  public enum EnumGuestsMovementsType
  {
    [Description("CN")]
    Contact,
    [Description("BK")]
    Booking,
    [Description("SH")]
    Show,
    [Description("SL")]
    Sale,

  }
}
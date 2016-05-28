using System.Collections.Generic;

namespace IM.Model.Classes
{
  /// <summary>
  ///  Entidad que trae la informacion para el reporte de regalos recibidos por sala de ventas
  /// </summary>
  /// <history>
  ///   [vku] 09/May/2016 Created
  /// </history>
  public class GiftsReceivedBySRData
  {
    public List<RptGiftsReceivedBySR> GiftsReceivedBySR { get; set; }
    public List<CurrencyShort> Currencies { get; set; }
  }
}

using System.Collections.Generic;

namespace IM.Model.Classes
{
  /// <summary>
  ///  Entidad que trae la información para el reporte de producción  por agencia y sala
  /// </summary>
  /// <history>
  ///   [vku] 30/May/2016 Created
  /// </history>
  public class ProductionByAgencySalesRoomOuthouseData
  {
    public List<RptProductionByAgencySalesRoomOuthouse> ProductionByAgencySalesRoomOuthouse { get; set; }
    public List<MembershipTypeShort> MembershipTypes { get; set; }
    public List<RptProductionByAgencySalesRoomOuthouse_SalesRoomByMembershipType> ProductionByAgencySalesRoomOuthouse_SalesByMembershipType { get; set; }
  }
}

using System.Collections.Generic;

namespace IM.Model.Classes
{
  /// <summary>
  ///  Entidad que trae la informacion para el reporte de produccion por agencia (Outside)
  /// </summary>
  /// <history>
  ///   [vku] 28/Abr/2016 Created
  /// </history>
  public class ProductionByAgencyOuthouseData
  {
    public List<RptProductionByAgencyOuthouse> ProductionByAgencyOuthouse { get; set; }
    public List<MembershipTypeShort> MembershipTypes { get; set; }
    public List<RptProductionByAgencyOuthouse_SalesByMembershipType> ProductionByAgencyOuthouse_SalesByMembershipType { get; set; }
  }
}

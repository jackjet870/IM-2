using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Classes
{
  /// <summary>
  ///   Entidada que trae la informacion para el reporte Production By Agency Inhouse
  /// </summary>
  /// <history>
  ///   [aalcocer] 09/05/2016 Created
  /// </history>
  public class ProductionByAgencyInhouseData
  {
    public List<RptProductionByAgencyInhouse> ProductionByAgencyInhouses { get; set; }
    public List<MembershipTypeShort> MembershipTypeShorts { get; set; }

    public List<RptProductionByAgencyInhouse_SalesByMembershipType> ProductionByAgencyInhouse_SalesByMembershipTypes { get; set; }
  }
}
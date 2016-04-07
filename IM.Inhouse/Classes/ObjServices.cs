using IM.Services.WirePRService;
using IM.Services.ClubesService;

namespace IM.Inhouse.Classes
{
  /// <summary>
  /// Clases heredadas para utilizarlas en los reportes de Crystal Reports
  /// </summary>
  /// <history>
  /// [jorcanche] 05/04/2016 Created
  /// [ecanul] 07/06/2016 Modificated Archivo renombrado a ObjServices, Agregado de clase RptEquityMembershipIM
  /// </history>
  public class RptReservationIM : RptReservationOrigos {}

  public class RptEquityMembershipIM : RptEquityMembership {}
}

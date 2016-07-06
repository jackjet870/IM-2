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
  /// [jorcanche] 11/04/2016 Modificated  agrego la clase 
  /// [ecanul] 06/07/2016 Modified. Agregue la clase heredada de RptEquityMembershipsPrevious
  /// </history>
  /// 
  #region Reservation
  public class RptReservationIM : RptReservationOrigos { } 
  #endregion

  #region ClubesService
  public class RptEquityMembershipIM : RptEquityMembership { }

  public class RptEquityVerificationIM : RptEquityVerification { }

  public class RptEquityMembershipsPreviousIM : RptEquityMembershipsPrevious { }

  public class RptEquitySalesmanIM : RptEquitySalesman { }

  public class RptEquityCoOwnerIM : RptEquityCoOwner { }

  public class RptEquityBeneficiaryIM : RptEquityBeneficiary { }

  public class RptEquityHotelIM : RptEquityHotel { }

  public class RptEquityGolfFieldsHeaderIM : RptEquityGolfFieldsHeader { }

  public class RptEquityGolfFieldsDetailM : RptEquityGolfFieldsDetail { }

  public class RptEquityRoomTypeIM : RptEquityRoomType { }

  public class RptEquityBalanceElectronicPurseHeaderIM : RptEquityBalanceElectronicPurseHeader { }

  public class RptEquityBalanceElectronicPurseDetailIM : RptEquityBalanceElectronicPurseDetail { }

  public class RptEquityPaymentPromiseIM : RptEquityPaymentPromise { }

  public class RptEquityWeeksNightsHeaderIM : RptEquityWeeksNightsHeader { }

  public class RptEquityWeeksNightsDetailIM : RptEquityWeeksNightsDetail { }

  public class RptEquityGolfRCIIM : RptEquityWeeksNightsHeader { }

  public class infoPromotionAffiliationStatusIM : infoPromotionAffiliationStatus { }

  public class MemberExtensionIM : MemberExtension { }
  #endregion

  #region CallCenterService

  public class RptEquityMembershipCallCIM : Services.CallCenterService.RptEquityMembership { }

  public class RptEquityProvisionIM : Services.CallCenterService.RptEquityProvision { }

  public class RptEquityReservationIM : Services.CallCenterService.RptEquityReservation { }

  #endregion

}

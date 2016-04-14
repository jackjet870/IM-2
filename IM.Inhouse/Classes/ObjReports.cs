using IM.Services.WirePRService;
using IM.Services.ClubesService;
using IM.Model;

namespace IM.Inhouse.Classes
{
  /// <summary>
  /// Clases heredadas para utilizarlas en los reportes de Crystal Reports
  /// </summary>
  /// <history>
  /// [jorcanche] 05/04/2016 Created
  /// [ecanul] 07/06/2016 Modificated Archivo renombrado a ObjServices, Agregado de clase RptEquityMembershipIM
  /// [jorcanche] 11/04/2016 Modificated Arse agrego la clase  
  /// </history>
  /// 
  public class RptReservationIM : RptReservationOrigos { }
  
  #region ClubesService
  public class RptEquityMembershipIM : RptEquityMembership { }

  public class RptEquityVerificationIM : RptEquityVerification { }

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

  public class RptInvitationIM : RptInvitation
  {
    public string ChangeBy
    {
      get { return App.User.User.peID; }
    }

    public string DateTime
    {
      get { return BusinessRules.BR.BRHelpers.GetServerDate().ToString(); }
    }

  }

  public class RptInvitationDepositsIM : RptInvitation_Deposit { }

  public class RptInvitationGiftsIM : RptInvitation_Gift { }

  public class RptInvitationGuestsIM : RptInvitation_Guest { }
  
}

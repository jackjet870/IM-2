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

  public class RptEquityMembershipIM : RptEquityMembership { }

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

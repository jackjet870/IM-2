using IM.Model;

namespace IM.Base.Classes
{
  /// <summary>
  /// Clases heredadas para utilizarlas en los reportes de Crystal Reports 
  /// </summary>
  /// <history>
  /// [jorcanche] created 17062016
  /// [jorcanche] Se agraron las clases RptInvitationIM, RptInvitationDepositsIM, RptInvitationGiftsIM, RptInvitationGuestsIM 
  /// </history>
 
    #region Invitation
    public class RptInvitationIM : RptInvitation { }

    public class RptInvitationDepositsIM : RptInvitation_Deposit { }

    public class RptInvitationGiftsIM : RptInvitation_Gift { }

    public class RptInvitationGuestsIM : RptInvitation_Guest { }
    #endregion
}

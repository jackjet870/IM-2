using IM.Model;
using System;
using System.Reflection;

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
    public class RptInvitationIM : RptInvitation {
    public DateTime InvitT => guInvitT ?? new DateTime(1899, 12, 31);
    public DateTime InvitD => guInvitD ?? new DateTime(1899, 12, 31);
    public RptInvitationIM(RptInvitation parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  }

    public class RptInvitationDepositsIM : RptInvitation_Deposit {
    public RptInvitationDepositsIM(RptInvitation_Deposit parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  }

    public class RptInvitationGiftsIM : RptInvitation_Gift {
    public int guestID => guID ?? 0;
    public RptInvitationGiftsIM(RptInvitation_Gift parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  }

    public class RptInvitationGuestsIM : RptInvitation_Guest {
    public int guAge => (Age != null) ? Convert.ToInt32(Age) : 0;
    public RptInvitationGuestsIM(RptInvitation_Guest parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  }
    #endregion
}

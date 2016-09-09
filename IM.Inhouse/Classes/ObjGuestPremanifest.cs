using System.Reflection;
using IM.Model;
using System;

namespace IM.Inhouse.Classes
{
  class ObjGuestPremanifest : GuestPremanifest
  {
    public GuestPremanifest Parent { get; set; }

    int Status;
    public ObjGuestPremanifest(GuestPremanifest parent, DateTime serverDate)
    {
      Parent = parent;

      foreach (PropertyInfo prop in parent.GetType().GetProperties())
      { GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null); }

      Status = HelperToObjGuest.Status(guCheckIn, guCheckOutD, guAvail, guInvit, guBookCanc, guBookD, guShow, guInfo,serverDate);
    }

    public int StatusColumn
    {
      get { return Status; }
    }
    public string ColorColumn
    {
      get { return HelperToObjGuest.ColorStatus(Status); }
    }
    public string SourceImage
    {
      get { return (guPRNote ? "pack://application:,,,/IM.Styles;component/Images/Note.ico" : string.Empty); }
    }
    public string EquityColumn
    {
      get { return (string.IsNullOrEmpty(guMembershipNum) ? string.Empty : "pack://application:,,,/IM.Styles;component/Images/Report.ico"); }
    }
  }
}

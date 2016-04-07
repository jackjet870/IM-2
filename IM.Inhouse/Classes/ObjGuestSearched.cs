using System.Reflection;
using IM.Model;
using IM.BusinessRules.BR;

namespace IM.Inhouse.Classes
{
  class ObjGuestSearched : GuestSearched
  {
    public GuestSearched Parent { get; set; }

    int Status;

    public ObjGuestSearched(GuestSearched parent)
    {
      Parent = parent;

      foreach (PropertyInfo prop in parent.GetType().GetProperties())
      { GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null); }

      Status = HelperToObjGuest.Status(guCheckIn, guCheckOutD, guAvail, guInvit, guBookCanc, guBookD, guShow, guInfo);
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
      get { return (guPRNote ? "pack://application:,,,/IM.Base;component/Images/Note.ico" : string.Empty); }
    }
    public string EquityColumn
    {
      get { return (string.IsNullOrEmpty(guMembershipNum) ? string.Empty : "pack://application:,,,/IM.Base;component/Images/Reporte.ico"); }
    }
  }
}

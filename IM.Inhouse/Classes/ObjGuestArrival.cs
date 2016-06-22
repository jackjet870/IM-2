using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IM.Model;
using System.Drawing;

namespace IM.Inhouse.Classes
{
  class ObjGuestArrival : GuestArrival
  {
    public GuestArrival Parent { get; set; }

    int Status;
    public ObjGuestArrival(GuestArrival parent, DateTime serverDate)
    {
      Parent = parent;

      foreach (PropertyInfo prop in parent.GetType().GetProperties())
      { GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null); }

      Status  = HelperToObjGuest.Status(guCheckIn, guCheckOutD, guAvail,guInvit, guBookCanc, guBookD, guShow, guInfo, serverDate);
    }
    //Atributo que se utiliza cuando se cargan el DataGrid, me  agrega el estatus
    public int StatusColumn
    {
      get { return Status; }
    }
    //Atributo que se utiliza cuando se cargan el DataGrid, me agrega el color segun el estatus 
    public string ColorColumn
    {
      get { return HelperToObjGuest.ColorStatus(Status); }
    }
    //Atributo que se utiliza cuando se cargan el DataGrid y me  agrega la imagen si la guPRNote esta en verdadero 
    public string SourceImage
    {
      get { return (guPRNote ? "pack://application:,,,/IM.Base;component/Images/Nota.ico" : string.Empty); }
    }

    public string EquityColumn
    {
      get { return (string.IsNullOrEmpty(guMembershipNum)? string.Empty:"pack://application:,,,/IM.Base;component/Images/Reporte.ico" ); }
    }
    public string ReservacionImage
    {
      get { return (string.IsNullOrEmpty(guHReservID) ? string.Empty : "pack://application:,,,/IM.Base;component/Images/Reporte.ico"); }
    }
  }
}

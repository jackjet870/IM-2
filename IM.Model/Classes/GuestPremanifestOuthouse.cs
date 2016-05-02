using System;

namespace IM.Model.Classes
{
  public  class GuestPremanifestOuthouse
  {
    public string guStatus { get; set; }
    public int guID { get; set; }
    public bool guCheckIn { get; set; }
    public string guRoomNum { get; set; }
    public string guLastName1 { get; set; }
    public string guFirstName1 { get; set; }
    public DateTime guCheckInD { get; set; }
    public DateTime guCheckOutD { get; set; }
    public string guco { get; set; }
    public string coN { get; set; }
    public string guag { get; set; }
    public string agN { get; set; }
    public bool guAvail { get; set; }
    public bool guInfo { get; set; }
    public string guPRInfo { get; set; }
    public Nullable<System.DateTime> guInfoD { get; set; }
    public bool guInvit { get; set; }
    public Nullable<System.DateTime> guInvitD { get; set; }
    public Nullable<System.DateTime> guBookD { get; set; }
    public Nullable<System.DateTime> guBookT { get; set; }
    public string guPRInvit1 { get; set; }
    public string guMembershipNum { get; set; }
    public bool guBookCanc { get; set; }
    public bool guShow { get; set; }
    public bool guSale { get; set; }
    public string guComments { get; set; }
    public decimal guPax { get; set; }
  }
}

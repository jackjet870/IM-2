using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.GuestsPR.Objects
{
  public class GuestByPRFormat
  {
    public int guID { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Ls { get; set; }
    public string Agency { get; set; }
    public string Market { get; set; }
    public string Exernal { get; set; }
    public string Rebook { get; set; }
    public System.DateTime CheckInD { get; set; }
    public string Avail { get; set; }
    public string OriginAvail { get; set; }
    public string Info { get; set; }
    public string FU { get; set; }
    public string Invit{ get; set; }
    public string Pr { get; set; }
    public string PRName { get; set; }
    public string Quinella { get; set; }
    public int RoomsQtyBooks { get; set; }
    public string Show { get; set; }
    public string Tour { get; set; }
    public string InOut { get; set; }
    public string WalkOut { get; set; }
    public string QuinellaSplit { get; set; }
    public byte ShowsQty { get; set; }
    public string Sale { get; set; }
    public Nullable<int> Sales { get; set; }
    public Nullable<decimal> Amount { get; set; }
  }
}

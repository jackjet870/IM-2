using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.Classes
{
  public class BookingDepositInvitation
  {
    public int bdID { get; set; }

    public int bdgu { get; set; }

    public decimal bdAmount { get; set; }

    public decimal bdReceived { get; set; }

    public string bdcu { get; set; }

    public string Currency { get; set; }

    public string bdpt { get; set; }

    public string PaymentType { get; set; }

    public string bdcc { get; set; }

    public string CreditCard { get; set; }

    public string bdpc { get; set; }

    public string bdCardNum { get; set; }

    public string bdExpD { get; set; }

    public int? bdAuth { get; set; }

    public int? bdFolioCXC { get; set; }

    public string bdUserCXC { get; set; }

    public DateTime? bdEntryDCXC { get; set; }

    public bool  isNew { get; set; }

    public bool isUpdated { get; set; }

  }
}

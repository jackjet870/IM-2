using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.Classes
{
  public class GuestCreditCardInvitation
  {

    public int ccgu { get; set; }

    public string ccID { get; set; }

    public string  CreditCard { get; set; }

    public int ccQty { get; set; }

    public bool isNew { get; set; }

    public bool isUpdate { get; set; }

    public string  ccIDPrevious { get; set; }
  }
}

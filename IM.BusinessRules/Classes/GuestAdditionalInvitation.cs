using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.Classes
{
  public class GuestAdditionalInvitation : IM.Model.Guest
  {
    public bool isNew { get; set; }

    public bool isUpdate { get; set; }

    public int? guIDPrevious { get; set; }
  }
}

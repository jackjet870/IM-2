using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.Classes
{
  public class GuestStatusInvitation
  {

    public int gsgu { get; set; }

    public string gsID { get; set; }

    public string  GuestStatus { get; set; }

    public int gsQty { get; set; }

    public bool isNew { get; set; }

    public bool isUpdate { get; set; }

    public string  gsIDPrevious { get; set; }
  }
}

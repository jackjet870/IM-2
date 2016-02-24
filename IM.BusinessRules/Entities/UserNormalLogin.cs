using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.Entities
{
  public class UserNormalLogin
  {
    public string PeID { get; set; }

    public string PeN { get; set; }

    public bool PeA { get; set; }

    public string PePwd { get; set; }

    public int PePwdDays { get; set; }

    public DateTime PePwdD { get; set; }
  }
}

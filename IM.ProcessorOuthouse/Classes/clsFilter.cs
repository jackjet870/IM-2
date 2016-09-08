using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.ProcessorOuthouse.Classes
{
  public class clsFilter
  {
    public List<string> _lstLeadSources { get; set; } = new List<string>();
    public List<string> _lstLeadSourcesPaymentComm { get; set; } = new List<string>();
    public List<string> _lstPaymentTypes { get; set; } = new List<string>();
    public List<string> _lstPRs { get; set; } = new List<string>();
    public List<string> _lstChargeTo { get; set; } = new List<string>();
    public List<string> _lstGifts { get; set; } = new List<string>();
    public List<string> _lstGiftsProdGift { get; set; } = new List<string>();

    public bool AllLeadSources { get; set; }
    public bool AllLeadSourcesPaymentComm { get; set; }
    public bool AllPaymentTypes { get; set; }
    public bool AllPRs { get; set; }
    public bool AllChargeTo { get; set; }
    public bool AllGift { get; set; }
    public bool AllGiftsProdGift { get; set; }
  }
}

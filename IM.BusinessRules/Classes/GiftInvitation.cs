using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.Classes
{
  public class GiftInvitation
  {

    public int iggu { get; set; }

    public int igQty { get; set; }

    public string iggi { get; set; }

    public string Gift { get; set; }

    public int igAdults { get; set; }

    public int igMinors { get; set; }

    public int igExtraAdults { get; set; }

    public decimal igPriceA { get; set; }

    public decimal igPriceM { get; set; }

    public decimal igPriceAdult { get; set; }

    public decimal igPriceMinor { get; set; }

    public decimal igPriceExtraAdult { get; set; }

    public String igct { get; set; }

    public bool isNew { get; set; }

    public bool isUpdate { get; set; }

    public string iggiPrevious { get; set; }
  }
}

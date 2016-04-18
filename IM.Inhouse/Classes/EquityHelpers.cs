using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Inhouse.Classes
{
  public class EquityHelpers
  {
    public static decimal GetIVAByOffice (string office)
    {
      decimal iva = 0;
      switch(office)
      {
        case "7":
          iva = Convert.ToDecimal(1.15);
          break;
        case "12":
          iva = 1;
          break;
        default:
          iva = Convert.ToDecimal(1.1);
          break;
      }
      return iva;
    }
  }
}

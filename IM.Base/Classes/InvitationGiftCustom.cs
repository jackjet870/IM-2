using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using System.ComponentModel;
using IM.Base.Helpers;
namespace IM.Base.Classes
{
  public class InvitationGiftCustom : InvitationGift, IDataErrorInfo
  {
    public InvitationGiftCustom()
    {
      igQty = 1;
    }
    public string this[string columnName]
    {
      get
      {
        string errorMessage = string.Empty;

        switch (columnName)
        {
          case "igQty":
            if (igQty == 0) errorMessage = "Quantity can't be lower than 1";
            break;
          case "iggi":
            if (igQty == 0)
            {
              errorMessage = "Enter the quantity first";
            }
            else if (string.IsNullOrEmpty(iggi) || iggi == null)
            {
              errorMessage = "Please select a gift";
            }
            break;
          case "igAdults":
            if (igAdults == 0 && igMinors == 0) errorMessage = "Quantity of adult and quantity of minors can't be both zero";
            break;
        }

        return errorMessage;
      }
    }

    public string Error
    {
      get
      {
        return "Something is wrong";
      }
    }
  }
}

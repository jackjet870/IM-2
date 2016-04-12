
using IM.Model;
using System.ComponentModel;
using System;

namespace IM.Base.Classes
{
  public class objInvitGift : InvitationGift, IDataErrorInfo
  {
    public string Error
    {
      get
      {
        return String.Concat(this[igQty.ToString()], " ", this[iggi], " ", this[igAdults.ToString()]);
      }
    }

    public string this[string columnName]
    {
      get
      {
        string errorMessage = null;
        switch (columnName)
        {
          case "igQty":
            if (igQty == 0)
            {
              errorMessage = "Input a Quantity.";
            }
            break;
          case "iggi":
            if (igQty == 0)
            {
              errorMessage = "Input a Quantity.";
            }
            else if (String.IsNullOrWhiteSpace(iggi))
            {
              errorMessage = "Select a Gift.";
            }
            break;
          case "igAdults":
            if(igAdults == 0 && !String.IsNullOrEmpty(iggi))
            {
              errorMessage = "Adult quantity can not be less 1";
            }
            break;
        }
        return errorMessage;
      }
    }
  }
}

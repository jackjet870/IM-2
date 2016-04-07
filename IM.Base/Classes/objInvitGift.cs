
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
        return String.Concat(this[igQty.ToString()], " ", this[iggi], " ",
                             this[igComments]);
      }
    }

    public string this[string columnName]
    {
      get
      {
        string errorMessage = null;
        switch (columnName)
        {
          case "iggi":
            if (String.IsNullOrWhiteSpace(iggi))
            {
              errorMessage = "Select a Gift.";
            }
            break;
          case "igQty":
            if (igQty == 0)
            {
              errorMessage = "Input a Quantity.";
            }
            break;
        }
        return errorMessage;
      }
    }
  }
}

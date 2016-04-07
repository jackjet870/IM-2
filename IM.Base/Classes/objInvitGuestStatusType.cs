using IM.Model;
using System;
using System.ComponentModel;

namespace IM.Base.Classes
{
  public class objInvitGuestStatusType: GuestStatus, IDataErrorInfo
  {
    public string Error
    {
      get
      {
        return String.Concat(this[gtQuantity.ToString()], " ", this[gtgs]);
      }
    }

    public string this[string columnName]
    {
      get
      {
        string errorMessage = null;
        switch (columnName)
        {
          case "gtgs":
            if (String.IsNullOrWhiteSpace(gtgs))
            {
              errorMessage = "Select a Status.";
            }
            break;
          case "gtQuantity":
            if (gtQuantity == 0)
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

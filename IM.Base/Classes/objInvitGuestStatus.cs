using IM.Model;
using System;
using System.ComponentModel;

namespace IM.Base.Classes
{
  public class objInvitGuestStatus: GuestStatus, IDataErrorInfo
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
            else
            {
              gtQuantity = 1;
            }
            break;
        }
        return errorMessage;
      }
    }
  }
}

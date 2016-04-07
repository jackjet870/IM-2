using System;
using IM.Model;
using System.ComponentModel;

namespace IM.Base.Classes
{
  public class objInvitCreditCard: GuestCreditCard, IDataErrorInfo
  {
    public string Error
    {
      get
      {
        return String.Concat(this[gdQuantity.ToString()], " ", this[gdcc]);
      }
    }

    public string this[string columnName]
    {
      get
      {
        string errorMessage = null;
        switch (columnName)
        {
          case "gdcc":
            if (String.IsNullOrWhiteSpace(gdcc))
            {
              errorMessage = "Select a Credit Card.";
            }
            break;
          case "gdQuantity":
            if (gdQuantity == 0)
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

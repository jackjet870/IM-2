using System;
using System.ComponentModel;
using IM.Model;

namespace IM.Base.Classes
{
  public class objInvitBookingDeposit: BookingDeposit, IDataErrorInfo
  {
    public string Error
    {
      get
      {
        return String.Concat(this[bdAmount.ToString()], " ", this[bdReceived.ToString()]);
      }
    }

    public string this[string columnName]
    {
      get
      {
        string errorMessage = null;
        switch (columnName)
        {
          case "bdcu":
            if (String.IsNullOrWhiteSpace(bdcu))
            {
              errorMessage = "Select a Currency.";
            }
            break;
          case "bdcc":
            if (String.IsNullOrEmpty(bdcc))
            {
              errorMessage = "Select a Credit Card";
            }
            break;
          case "bdpt":
            if (String.IsNullOrEmpty(bdpt))
            {
              errorMessage = "Select a Payment Type";
            }
            break;
          case "bdpc":
            if (String.IsNullOrEmpty(bdpc))
            {
              errorMessage = "Input a Refund Place.";
            }
            break;
          case "bdReceived":
            if (bdReceived > bdAmount)
            {
              errorMessage = "The received amount can not be more than the deposit amount";
            }
            else if (bdReceived == 0 && bdAmount == 0)
              errorMessage = "Currency without Amount specified.";
            break;
          case "bdAmount":
            if (bdAmount == 0 && bdReceived == 0)
            {
              errorMessage = "Currency without Amount specified.";
            }
            break;
          case "bdExpD":
            if (bdpt == "CC")
            {
              var dt = new DateTime();
              if (!DateTime.TryParse(bdExpD, out dt))
              {
                errorMessage = "The expire day does not has a correct format. (MM/YY)";
              }
            }
            break;
          case "bdCardNum":
            if(!String.IsNullOrEmpty(bdCardNum) && bdCardNum.Length > 4)
            {
              errorMessage = "Type the last four numbers.";
            }
            break;
        }

        return errorMessage;
      }
    }
  }
}

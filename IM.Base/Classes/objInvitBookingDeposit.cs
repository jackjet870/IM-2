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
        return String.Concat(this[bdAmount.ToString()], " ", this[bdReceived.ToString()], "  ", this[bdcu]
                            , " ", this[bdpt], " ", this[bdcc], " ", this[bdCardNum.ToString()], " ", this[bdExpD]
                            , " ", this[!string.IsNullOrEmpty(bdAuth) ? bdAuth : String.Empty], " ", this[bdFolioCXC.HasValue ?  bdFolioCXC.Value.ToString() : String.Empty]);
      }
    }

    public string this[string columnName]
    {
      get
      {
        string errorMessage = null;
        switch (columnName)
        {
          case "bdAmount":
            if(bdAmount == 0)
            {
              errorMessage = "Currency without Amount specified.";
            }
            else if(bdAmount != bdReceived)
            {
              errorMessage = "The Deposit and Received amounts have to be equals";
            }
            break;
          case "bdReceived":
            if (bdReceived == 0)
            {
              errorMessage = "Input a Deposit amount";
            }
            else if (bdAmount != bdReceived)
            {
              errorMessage = "The Deposit and Received amounts have to be equals";
            }
            break;
          case "bdcu":
            if (String.IsNullOrWhiteSpace(bdcu))
            {
              errorMessage = "Select a Currency.";
            }
            break;
          case "bdpt":
            if (String.IsNullOrEmpty(bdpt))
            {
              errorMessage = "Select a Payment Type";
            }
            break;
          case "bdcc":
            if (bdpt == "CC" && String.IsNullOrEmpty(bdcc))
            {
              errorMessage = "Select a Credit Card";
            }
            break;
          case "bdpc":
            if (String.IsNullOrEmpty(bdpc))
            {
              errorMessage = "Input a Refund Place.";
            }
            break;
          case "bdExpD":
            if (bdpt == "CC")
            {
              var dt = new DateTime();
              var exp = "01/" + bdExpD;
              if (!DateTime.TryParse(exp, out dt))
              {
                errorMessage = "The expire day does not has a correct format. (MM/YY)";
              }
            }
            break;
          case "bdCardNum":
            if (bdpt != "CC") break;

            if (bdCardNum != null && bdCardNum > 9999)
            {
              errorMessage = "Type the last four numbers.";
            }
            break;
          case "bdAuth":
            if(bdpt == "CC" && String.IsNullOrEmpty(bdAuth.ToString()))
            {
              errorMessage = "Input a Auth ID";
            }
            break;
          case "bdFolioCXC":
            if(!bdFolioCXC.HasValue)
            {
              errorMessage = "Input a Folio CxC";
            }
            else if(bdFolioCXC.Value <=0)
            {
              errorMessage = "The Folio CxC can not be less than zero";
            }
            break;
        }

        return errorMessage;
      }
    }
  }
}

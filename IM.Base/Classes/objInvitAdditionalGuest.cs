using System;
using System.ComponentModel;

namespace IM.Base.Classes
{
  public class objInvitAdditionalGuest :  IDataErrorInfo
  {
    public int guIDParent { get; set; }

    public int guID { get; set; }

    public string  guLastName1 { get; set; }

    public string guFirstName1 { get; set; }

    public string Error
    {
      get
      {
        return String.Concat(this[guID.ToString()]);
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
            if (String.IsNullOrWhiteSpace(guID.ToString()))
            {
              errorMessage = "Select a Guest.";
            }
            break;
        }
        return errorMessage;
      }
    }
  }
}

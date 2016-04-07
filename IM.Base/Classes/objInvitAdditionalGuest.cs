using System;
using IM.Model;
using System.ComponentModel;

namespace IM.Base.Classes
{
  public class objInvitAdditionalGuest :  Guest, IDataErrorInfo
  {
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

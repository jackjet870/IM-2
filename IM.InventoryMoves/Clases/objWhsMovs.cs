using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.InventoryMovements.Clases
{
  public class objWhsMovs : Model.WhsMov, IDataErrorInfo
  {
    public string Error
    {
      get
      {
        return String.Concat(this[wmQty.ToString()], " ", this[wmgi], " ",
                             this[wmComments]);
      }
    }

    public string this[string columnName]
    {
      get
      {
        string errorMessage = null;
        switch (columnName)
        {
          case "wmgi":
            if (String.IsNullOrWhiteSpace(wmgi))
            {
              errorMessage = "Select a Gift.";
            }
            break;
          case "wmQty":
            if (wmQty == 0)
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
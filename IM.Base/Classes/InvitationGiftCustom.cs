using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using System.ComponentModel;
using IM.Base.Helpers;
using System.Runtime.CompilerServices;

namespace IM.Base.Classes
{
  public class InvitationGiftCustom : InvitationGift, IDataErrorInfo, INotifyPropertyChanged
  {
    public int IgQtyCustom
    {
      get { return igQty; }
      set { igQty = value; OnPropertyChanged(); }
    }
    public string IggiCustom
    {
      get { return iggi; }
      set
      {
        iggi = value; OnPropertyChanged();
        IgAdultsCustom = 1; OnPropertyChanged("IgAdultsCustom");
      }
    }

    public int IgAdultsCustom
    {
      get { return igAdults; }
      set { igAdults = value; OnPropertyChanged(); }
    }


    public InvitationGiftCustom()
    {

    }
    public string this[string columnName]
    {
      get
      {
        string errorMessage = string.Empty;

        switch (columnName)
        {
          case "IgQtyCustom":
            if (igQty == 0) errorMessage = "Quantity can't be lower than 1";
            break;
          case "Iggi":
            if (igQty == 0)
            {
              errorMessage = "Enter the quantity first";
              break;
            }
            else if (string.IsNullOrEmpty(iggi))
            {
              errorMessage = "Please select a gift";
              break;
            }
            break;
          case "igAdults":
            if (igAdults == 0 && igMinors == 0) errorMessage = "Quantity of adult and quantity of minors can't be both zero";
            break;
        }

        return errorMessage;
      }
    }

    public string Error
    {
      get
      {
        return string.Empty;
      }
    }

    #region Implementacion INotifyPropertyChange
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, value)) return;
      field = value;
      OnPropertyChanged(propertyName);
    }
    #endregion
  }
}

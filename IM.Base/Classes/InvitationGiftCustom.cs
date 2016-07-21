using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using System.ComponentModel;
using IM.Base.Helpers;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace IM.Base.Classes
{
  public class InvitationGiftCustom : InvitationGift, IDataErrorInfo
  {
    #region Propiedades 
    //public int IgQtyCustom
    //{
    //  get { return igQty; }
    //  set { igQty = value; OnPropertyChanged(); }
    //}
    //public string IggiCustom
    //{
    //  get { return iggi; }
    //  set
    //  {
    //    iggi = value; OnPropertyChanged();
    //  }
    //}
    //public int IgAdultsCustom
    //{
    //  get { return igAdults; }
    //  set { igAdults = value; OnPropertyChanged(); }
    //}

    //public int IgMinorsCustom
    //{
    //  get { return igMinors; }
    //  set { igMinors = value; OnPropertyChanged(); }
    //}

    private decimal _maxAuth;

    public decimal MaxAuth
    {
      get { return _maxAuth; }
      set { _maxAuth = value; }
    }

    private decimal _totalCost;

    public decimal TotalCost
    {
      get { return _totalCost; }
      set { _totalCost = value; }
    }

    private decimal _totalPrice;

    public decimal TotalPrice
    {
      get { return _totalPrice; }
      set { _totalPrice = value; }
    }
    #endregion
    public InvitationGiftCustom()
    {
      //Inicializa la propiedad
      igQty = 1;
    }
    #region Constructor

    #endregion

    #region IDataError
    public string Error
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public string this[string columnName]
    {
      get
      {
        string errorMessage = null;
        switch (columnName)
        {
          case "igQty":
            if (igQty == 0)
            {
              errorMessage = "Input a Quantity.";
            }
            break;
          case "iggi":
            if (igQty == 0)
            {
              errorMessage = "Input a Quantity.";
            }
            else if (String.IsNullOrWhiteSpace(iggi))
            {
              errorMessage = "Select a Gift.";
            }
            break;
          case "igAdults":
            if (igAdults == 0 && !String.IsNullOrEmpty(iggi))
            {
              errorMessage = "Adult quantity can not be less 1";
            }
            break;
        }
        return errorMessage;
      }
    }

    #endregion

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

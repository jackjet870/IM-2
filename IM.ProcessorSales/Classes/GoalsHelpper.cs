using IM.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IM.ProcessorSales.Classes
{
  public class GoalsHelpper : RptConcentrateDailySales, INotifyPropertyChanged
  {
    private bool _isCheck;

    public bool IsCheck
    {
      get { return _isCheck; }
      set { SetField(ref _isCheck, value); }
    }

    private SalesRoomByUser _salesRoomByUser;

    public SalesRoomByUser SalesRoomByUser
    {
      get { return _salesRoomByUser; }
      set { SetField(ref _salesRoomByUser, value); }
    }

    private decimal _goal;

    public decimal Goal
    {
      get { return _goal; }
      set { SetField(ref _goal, value); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, value)) return;
      field = value;
      OnPropertyChanged(propertyName);
    }
  }
}
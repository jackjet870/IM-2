using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IM.ProcessorSales.Classes
{
  public class MultiDateHelpper : INotifyPropertyChanged, IDataErrorInfo
  {
    private string _salesRoom;

    public string SalesRoom
    {
      get { return _salesRoom; }
      set { SetField(ref _salesRoom, value); }
    }

    private DateTime _dtStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

    public DateTime DtStart
    {
      get { return _dtStart; }
      set { SetField(ref _dtStart, value); }
    }

    private DateTime _dtEnd = DateTime.Now;

    public DateTime DtEnd
    {
      get { return _dtEnd; }
      set { SetField(ref _dtEnd, value); }
    }

    private bool _isMain;

    public bool IsMain
    {
      get { return _isMain; }
      set { SetField(ref _isMain, value); }
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

    public string Error => string.Concat(this[SalesRoom], " ", this[DtStart.ToShortDateString()], " ", this[DtEnd.ToShortDateString()]);

    public string this[string columnName]
    {
      get
      {
        string errorMessage = null;
        switch (columnName)
        {
          case "SalesRoom":
            if (string.IsNullOrEmpty(SalesRoom))
            {
              errorMessage = "Select a Sales Room.";
            }
            break;
        }
        return errorMessage;
      }
    }
  }
}
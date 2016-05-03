using System;
using System.ComponentModel;

namespace IM.ProcessorSales.Classes
{
  public class WeeksHelpper: INotifyPropertyChanged
  {
    private bool _include;

    public bool include
    {
      get { return _include; }
      set { _include = value; OnChanged("include"); }
    }

    public DateTime dtStart { get; set; }

    public DateTime dtEnd { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnChanged(string prop)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
  }
}

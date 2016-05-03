using System;
using System.ComponentModel;

namespace IM.ProcessorSales.Classes
{
  public class MultiDateHelpper : INotifyPropertyChanged
  {
    public string salesRoom { get; set; }

    public DateTime dtStart { get; set; }

    public DateTime dtEnd { get; set; }

    private bool _isMain;
    public bool isMain
    {
      get { return _isMain; }
      set { _isMain = value; OnChanged("isMain"); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnChanged(string prop)
    {
      PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(prop));
    }


  }
}

using IM.Model;
using System.ComponentModel;

namespace IM.ProcessorSales.Classes
{
  public class GoalsHelpper : INotifyPropertyChanged
  {
    private bool _isCheck;

    public bool isCheck
    {
      get { return _isCheck; } 
      set { _isCheck = value; onChanged("isCheck"); }
    }

    public SalesRoomByUser salesRoom { get; set; }

    public decimal goal { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    private void onChanged(string prop)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
  }
}

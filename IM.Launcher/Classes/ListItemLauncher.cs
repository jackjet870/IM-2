using IM.Launcher.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IM.Launcher.Classes
{
  public class ListItemLauncher
  {
    public int Orden { get; set; }

    public string Descripcion  { get; set; }

    public string UriImage { get; set; }

    public BitmapImage Image { get; set; }
  }
}

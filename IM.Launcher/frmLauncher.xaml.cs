using IM.Launcher.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IM.Launcher
{
  /// <summary>
  /// Interaction logic for frmLauncher.xaml
  /// </summary>
  public partial class frmLauncher : Window
  {
    #region Variables

    private bool pushCtrl = false;
    private bool pushCtrlE = false;

    #endregion

    #region Constructor y Destructor
    public frmLauncher()
    {
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
      InitializeComponent();
      
    }

    #endregion

    #region Métodos de la forma

    /// <summary>
    /// Al iniciar la forma crea los botones del menú
    /// </summary>
    /// <history>
    /// [lchairez] 09/Mar/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      var thisApp = Assembly.GetExecutingAssembly();
      AssemblyName name = new AssemblyName(thisApp.FullName);

      lblVersion.Content = "Launcher v" + name.Version;
      lsbLauncher.ItemsSource = BuildLauncher();
    }

    /// <summary>
    /// Ejecuta tareas con algunas teclas de configuración se realizan tareas.
    /// </summary>
    /// <history>
    /// [lchairez] 09/Mar/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
      {
        pushCtrl = true;
      }
      if (e.Key == Key.E && pushCtrl)
      {
        pushCtrlE = true;
        this.Close();
      }
      if (e.Key == Key.P && pushCtrl)
      {
        ShowInfoUser();
      }
      if (e.Key == Key.S && pushCtrl)
      {
        MessageBox.Show("Setup");
      }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = !pushCtrlE;
      this.WindowState = System.Windows.WindowState.Minimized;
    }

    /// <summary>
    /// Abre cada módulo al oprimir dos veces el botón correspodiente
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void lsbLauncher_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if(lsbLauncher.SelectedItem != null)
      {
        var item = (ListItemLauncher)lsbLauncher.SelectedItem;
        CallingExe((EnumMenu)item.Orden);
      }
    }

    /// <summary>
    /// Abre cada módulo a oprimir Enter en el botón correspodiente
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void lsbLauncher_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key == Key.Enter && lsbLauncher.SelectedItem != null)
      {
        var item = (ListItemLauncher)lsbLauncher.SelectedItem;
        CallingExe((EnumMenu)item.Orden);
      }
    }

    #endregion

    #region Métodos privados
    private IEnumerable<ListItemLauncher> BuildLauncher()
    {
      var lstNames = new ConfigButton().ListOfButtons();
      ListItemLauncher itemLauncher;
      var lstItemLauncher = new List<ListItemLauncher>();

      string pathImages =System.IO. Path.Combine(Directory.GetCurrentDirectory(), "Images\\");

      foreach (var item in lstNames)
      {
        itemLauncher = new ListItemLauncher();
        itemLauncher.Orden = item.Key;
        itemLauncher.Descripcion = item.Value;
        itemLauncher.UriImage = String.Format("{0}{1}_32.png", pathImages, ((EnumMenu) item.Key).ToString());
        itemLauncher.Image = new BitmapImage(new Uri(itemLauncher.UriImage));
        lstItemLauncher.Add(itemLauncher);
      }

      return lstItemLauncher;
    }

    /// <summary>
    /// Ejecuta el .exe de cada módulo, según corresponda
    /// </summary>
    /// <param name="button">Botón oprimido</param>
    /// <history>
    /// [lchairez] 09/Mar/2016 Created
    /// </history>
    private void CallingExe(EnumMenu option)
    {
      string path = Environment.CurrentDirectory;
      switch (option)
      {
        case EnumMenu.InHouse:
          System.Diagnostics.Process.Start(String.Format("{0}\\IM.Inhouse.exe", path));
          break;
        case EnumMenu.Assignment:
          //llamar módulo
          break;
        case EnumMenu.MailOuts:
          System.Diagnostics.Process.Start(String.Format("{0}\\IM.MailOuts.exe", path));
          break;
        case EnumMenu.Animation:
          //llamar módulo
          break;
        case EnumMenu.Regen:
          //llamar módulo
          break;
        case EnumMenu.Outhouse:
          //llamar módulo
          break;
        case EnumMenu.Host:
          System.Diagnostics.Process.Start(String.Format("{0}\\IM.Host.exe", path));
          break;
        case EnumMenu.InventoryMovs:
          System.Diagnostics.Process.Start(String.Format("{0}\\IM.InventoryMovements.exe", path));
          break;
        case EnumMenu.ProcessorINH:
          //llamar módulo
          break;
        case EnumMenu.ProcessorOUT:
          //llamar módulo
          break;
        case EnumMenu.ProcessorGRAL:
          //llamar módulo
          break;
        case EnumMenu.ProcessorSales:
          //llamar módulo
          break;
        case EnumMenu.PRStatistics:
          //llamar módulo
          break;
        case EnumMenu.Graph:
          //llamar módulo
          break;
        case EnumMenu.GuestsByPR:
          //llamar módulo
          break;
        case EnumMenu.SalesByPR:
          //llamar módulo
          break;
        case EnumMenu.SalesByLiner:
          //llamar módulo
          break;
        case EnumMenu.SalesByCloser:
          //llamar módulo
          break;
        case EnumMenu.Administrator:
          System.Diagnostics.Process.Start(String.Format("{0}\\IM.Administrator.exe", path));
          break;
        case EnumMenu.MailOutsConfig:
          //llamar módulo
          break;
        case EnumMenu.InvitationsConfig:
          //llamar módulo
          break;
        case EnumMenu.PrinterConfig:
          var printer = new PrinterCfg();
          printer.Owner = this;
          printer.ShowDialog();
          break;
      }
    }

    /// <summary>
    /// Muestra información sobre el usuario
    /// </summary>
    /// <history>
    /// [lchairez] 09/Mar/2016 Created
    /// </history>
    private void ShowInfoUser()
    {

      string userName = Environment.UserName;
      string machineName = Environment.MachineName;
      string ipAddress = "Local IP Address Not Found!"; ;

      var host = Dns.GetHostEntry(Dns.GetHostName());
      foreach (var ip in host.AddressList)
      {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
          ipAddress = ip.ToString();
          break;
        }
      }

      string message = String.Format("User Name: {0}\nComputer Name Local: {1}\nComputer IP Address Local: {2}\nComputer Name Remote: {3}\nComputer IP Address Remote: {4}", userName, machineName, ipAddress, String.Empty, String.Empty);

      MessageBox.Show(message, "Intelligence Marketing Launcher", MessageBoxButton.OK, MessageBoxImage.Information);
    }


    #endregion
        
  }
}

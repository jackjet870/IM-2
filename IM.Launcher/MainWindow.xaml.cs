using IM.Launcher.Classes;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace IM.Launcher
{
  /// <summary>
  /// Formulario que invoca los diferentes módulos del proyecto Intelligement Marketing
  /// </summary>
  /// <history>
  /// [lchairez] 05/Feb/2016 Created
  /// </history>
  public partial class MainWindow : Window
  {
    private bool pushCtrl = false;
    private bool pushCtrlE = false;
    public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

    #region Métodos de la forma

    /// <summary>
    /// Al iniciar la forma crea los botones del menú
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      var thisApp = Assembly.GetExecutingAssembly();
      AssemblyName name = new AssemblyName(thisApp.FullName);

      lblVersion.Content = "Launcher v" + name.Version;
      lblVersion.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
      CreateButtons(this);
    }

    /// <summary>
    /// Evita que el formulario se cierre al oprimir el botón de cerrar
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void Launcher_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = !pushCtrlE;
      this.WindowState = System.Windows.WindowState.Minimized;
    }

    /// <summary>
    /// Abre cada módulo a oprimir el botón correspodiente
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    void btn_Click(object sender, RoutedEventArgs e)
    {
      var button = (Button)sender;
      CallingExe(button);
    }

    /// <summary>
    /// Ejecuta tareas con algunas teclas de configuración se realizan tareas.
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void Launcher_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
      {
          pushCtrl = true;
      }
      if(e.Key == Key.E && pushCtrl)
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

    #endregion

    #region Métodos privados

    /// <summary>
    /// Crea los botones que mandaran llamar a los .exe's de los módulos
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void CreateButtons(MainWindow form)
    {
      var lstButtons = new Classes.ConfigButton().CreateButtons();
      foreach (var btn in lstButtons.OrderBy(o=> (int)o.Tag))
      {
          //Agregamos sus eventos
          btn.Click += btn_Click;

          grdPanel.Children.Add(btn);
      }
    }

    /// <summary>
    /// Ejecuta el .exe de cada módulo, según corresponda
    /// </summary>
    /// <param name="button">Botón oprimido</param>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void CallingExe(Button button)
    {
      string path = Environment.CurrentDirectory;
      switch ((EnumMenu)button.Tag)
      {
          case EnumMenu.InHouse:
            var login = new IM.Base.Forms.frmLogin();
            login.Owner = this;
            login.ShowDialog();
            break;
          case EnumMenu.Assignment:
              //llamar módulo
              break;
          case EnumMenu.MailOuts:
              //llamar módulo
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
              //llamar módulo
              break;
          case EnumMenu.InventoryMovs:
              //llamar módulo
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
              //llamar módulo
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
    /// [lchairez] 05/Feb/2016 Created
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

      MessageBox.Show(message, "Intelligence Marketing Launcher",MessageBoxButton.OK,MessageBoxImage.Information);
    }
        
    #endregion

  }
}

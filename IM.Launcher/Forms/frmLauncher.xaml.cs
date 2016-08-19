using IM.Base.Helpers;
using IM.Base.Forms;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IM.Model.Helpers;
using IM.Model.Classes;

namespace IM.Launcher.Forms
{
  /// <summary>
  /// Formulario que invoca los diferentes módulos del proyecto Intelligement Marketing
  /// </summary>
  /// <history>
  /// [lchairez] 05/Feb/2016 Created
  /// </history>
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

    #region Window_Loaded
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
      lsbLauncher.ItemsSource = CreateMenu();
    }
    #endregion

    #region Window_KeyDown
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
        UIHelper.ShowMessage("Setup");
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Evita que el formulario se cierre al oprimir el botón de cerrar
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = !pushCtrlE;
      this.WindowState = System.Windows.WindowState.Minimized;
    }
    #endregion

    #region lsbLauncher_MouseDoubleClick
    /// <summary>
    /// Abre cada módulo al oprimir dos veces el botón correspodiente
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void lsbLauncher_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (lsbLauncher.SelectedItem != null)
      {
        Item item = (Item)lsbLauncher.SelectedItem;
        CallingExe(EnumToListHelper.StringToEnum<EnumMenu>(item.Id));
      }
    }
    #endregion

    #region lsbLauncher_KeyDown
    /// <summary>
    /// Abre cada módulo a oprimir Enter en el botón correspodiente
    /// </summary>
    /// <history>
    /// [lchairez] 05/Feb/2016 Created
    /// </history>
    private void lsbLauncher_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter && lsbLauncher.SelectedItem != null)
      {
        Item item = (Item)lsbLauncher.SelectedItem;
        CallingExe(EnumToListHelper.StringToEnum<EnumMenu>(item.Id));
      }
    }
    #endregion

    #endregion

    #region Metodos privados

    #region CreateMenu
    /// <summary>
    /// Construye la lista de opciones con las que contará el menú
    /// </summary>
    /// <history>
    /// [wtorres] 11/Abr/2016 Modified. Ahora genera la lista en base al enumerado EnumMenu
    /// [vku]     06/Jun/2016 Modified. Se cambia el boton PrinteroConfig a SystemConfig
    /// [wtorres] 15/Jul/2016 Modified. Ahora genera una lista de ItemsList en lugar de una lista de ListItemLauncher
    /// </history>
    private ItemsList CreateMenu()
    {
      var lstMenu = new ItemsList();

      lstMenu.Add(EnumMenu.Assignment);
      lstMenu.Add(EnumMenu.MailOuts);
      lstMenu.Add(EnumMenu.InventoryMovs);
      lstMenu.Add(EnumMenu.ProcessorINH);
      lstMenu.Add(EnumMenu.PRStatistics);
      lstMenu.Add(EnumMenu.Graph);
      lstMenu.Add(EnumMenu.GuestsByPR);
      lstMenu.Add(EnumMenu.SalesByPR);
      lstMenu.Add(EnumMenu.SalesByLiner);
      lstMenu.Add(EnumMenu.SalesByCloser);
      lstMenu.Add(EnumMenu.MailOutsConfig);
      lstMenu.Add(EnumMenu.InvitationsConfig);
      lstMenu.Add(EnumMenu.SystemConfig);

      return lstMenu;
    }
    #endregion

    #region CallingExe
    /// <summary>
    /// Ejecuta el .exe de cada módulo, según corresponda
    /// </summary>
    /// <param name="button">Botón oprimido</param>
    /// <history>
    /// [lchairez] 09/Mar/2016 Created
    /// [vku] 06/06/20161 Modified. Se añade SystemConfig (antes PrinterConfig)
    /// </history>
    private void CallingExe(EnumMenu option)
    {
      string path = Environment.CurrentDirectory;
      switch (option)
      {
        case EnumMenu.Inhouse:
          Process.Start(String.Format("{0}\\IM.Inhouse.exe", path));
          break;
        case EnumMenu.Assignment:
          Process.Start(String.Format("{0}\\IM.Assignment.exe", path));
          break;
        case EnumMenu.MailOuts:
          Process.Start(String.Format("{0}\\IM.MailOuts.exe", path));
          break;
        case EnumMenu.Outhouse:
          Process.Start(String.Format("{0}\\IM.Outhouse.exe", path));
          break;
        case EnumMenu.Host:
          Process.Start(String.Format("{0}\\IM.Host.exe", path));
          break;
        case EnumMenu.InventoryMovs:
          Process.Start(String.Format("{0}\\IM.InventoryMovements.exe", path));
          break;
        case EnumMenu.ProcessorINH:
          Process.Start(String.Format("{0}\\IM.ProcessorInhouse.exe", path));
          break;
        case EnumMenu.ProcessorOUT:
          Process.Start(String.Format("{0}\\IM.ProcessorOuthouse.exe", path));
          break;
        case EnumMenu.ProcessorGRAL:
          Process.Start(String.Format("{0}\\IM.ProcessorGeneral.exe", path));
          break;
        case EnumMenu.ProcessorSales:
          Process.Start(String.Format("{0}\\IM.ProcessorSales.exe", path));
          break;
        case EnumMenu.PRStatistics:
          Process.Start(String.Format("{0}\\IM.PRStatistics.exe", path));
          break;
        case EnumMenu.Graph:
          Process.Start(String.Format("{0}\\IM.Graph.exe", path));
          break;
        case EnumMenu.GuestsByPR:
          Process.Start(String.Format("{0}\\IM.GuestsPR.exe", path));
          break;
        case EnumMenu.SalesByPR:
          Process.Start(String.Format("{0}\\IM.SalesPR.exe", path));
          break;
        case EnumMenu.SalesByLiner:
          Process.Start(String.Format("{0}\\IM.SalesLiner.exe", path));
          break;
        case EnumMenu.SalesByCloser:
          Process.Start(String.Format("{0}\\IM.SalesCloser.exe", path));
          break;
        case EnumMenu.Administrator:
          Process.Start(String.Format("{0}\\IM.Administrator.exe", path));
          break;
        case EnumMenu.MailOutsConfig:
          Process.Start(String.Format("{0}\\IM.MailOutsConfig.exe", path));
          break;
        case EnumMenu.InvitationsConfig:
          Process.Start(String.Format("{0}\\IM.InvitConfig.exe", path));
          break;
        case EnumMenu.SystemConfig:
          var system = new frmSystemCfg();
          system.Owner = this;
          system.ShowDialog();
          break;
      }
    }
    #endregion

    #region ShowInfoUser
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

      UIHelper.ShowMessage(message);
    } 
    #endregion

    #endregion
  }
}

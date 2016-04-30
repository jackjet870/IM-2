using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using System.Data;
using System.IO;
using System.Diagnostics;
using IM.Base.Forms;
using IM.Model.Enums;
using IM.Model.Classes;

namespace IM.SalesCloser.Forms
{
  /// <summary>
  /// Interaction logic for frmSalesCloser.xaml
  /// </summary>
  public partial class frmSalesCloser : Window
  {
    #region Propiedades, Atributos
    List<bool> filtersBool = null;
    List<Tuple<string, string>> filtersReport = null;

    public ExecuteCommandHelper LoadCombo { get; set; }
    #endregion
    public frmSalesCloser()
    {
      InitializeComponent();
      LoadCombo = new ExecuteCommandHelper(x => LoadPersonnel());
    }

    #region Eventos Ventana
    /// <summary>
    /// Evento que se lanza al iniciar la aplicacion
    /// </summary>
    /// <history>
    /// [erosado] 22/Mar/2016 Created
    /// </history>   
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadPersonnel();
      //Seleccionamos los días en el datapicker 
      dtpkFrom.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      dtpkTo.SelectedDate = DateTime.Now;
      //Agregamos login del usuario en la interfaz
      setNewUserLogin();
    }
    /// <summary>
    /// Evento que se lanza cuando realizamos la consulta boton Search
    /// </summary>
    /// <history>
    /// [erosado] 22/Mar/2016 Created
    /// </history>
    private void imgButtonOk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      StaStart("Loading data...");
      imgButtonOk.IsEnabled = false;
      filtersBool = new List<bool>();
      PersonnelShort closer = cbxPersonnel.SelectedValue as PersonnelShort;
      filtersReport = new List<Tuple<string, string>>();
      filtersReport.Add(new Tuple<string, string>("Closer", string.Concat(closer.peID, " - ", closer.peN)));

      DoGetSalesByCloser(dtpkFrom.SelectedDate.Value, dtpkTo.SelectedDate.Value, App.User.SalesRoom.srID, closer.peID);
    }
    /// <summary>
    /// Evento que se lanza cuando generamos nuestro reporte boton Print
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void imgButtonPrint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      List<SaleByCloser> listaSaleByCloser = dtgr.DataContext as List<SaleByCloser>;
      if (listaSaleByCloser != null)
      {
        //Obtenemos dateRange
        string dateRange = DateHelper.DateRange(dtpkFrom.SelectedDate.Value, dtpkTo.SelectedDate.Value);
        string dateRangeFileName = DateHelper.DateRangeFileName(dtpkFrom.SelectedDate.Value, dtpkTo.SelectedDate.Value);
        //Obtenemos el nombre del reporte y el dateRange
        string rptName = "Sales By Closer";
        //Obtenemos el dataTable con la lista formateada
        DataTable dt = TableHelper.GetDataTableFromList(listaSaleByCloser, true);
        //Creamos el reporte
        FileInfo fi = EpplusHelper.CreateGeneralRptExcel(filtersReport, dt, rptName, dateRangeFileName, Utilities.UseFulMethods.getExcelFormatTable());

        if (fi != null)
        {
          Process.Start(fi.FullName);
        }
      }
      else
      {
        UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
      }
    }
    /// <summary>
    /// Evento que se lanza cuando abrimos la ventana About boton About
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void imgButtonAbout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      frmAbout formAbout = new frmAbout();
      formAbout.ShowDialog();
    }
    /// <summary>
    /// Evento que se lanza cuando queremos salir de la aplicacion boton exit
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void imgButtonExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.Close();
    }

    /// <summary>
    /// Evento de dispara cuando el usuario cambia de sesion en el modulo.
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void imageLogOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      frmLogin frmlogin = new frmLogin(loginType: EnumLoginType.SalesRoom, changePassword: true, autoSign: true);
      if (App.User.AutoSign)
      {
        frmlogin.UserData = App.User;
      }
      frmlogin.ShowDialog();

      if (frmlogin.IsAuthenticated)
      {
        App.User = frmlogin.UserData;
        LoadPersonnel();
      }

    }
    #endregion

    #region SelectionChanged
    /// <summary>
    /// Evento que se dispara cada que el usuario preciona el mouse sobre una fila del datagrid
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void dtgr_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarReg.Content = string.Format("{0}/{1}", (dtgr.SelectedIndex + 1).ToString(), dtgr.Items.Count.ToString());
    }
    #endregion

    #region Async Methods
    /// <summary>
    /// Obtiene la lista del personal
    /// </summary>
    /// <param name="salerRooms">filtro leadsources</param>
    /// <param name="roles">rol del usuario loggeado</param>
    /// <history>
    /// [erosado] 24/Mar/2016 Created
    /// </history>

    public void DoGetPersonnel(string salesRooms, string roles)
    {
      Task.Factory.StartNew(() => BRPersonnel.GetPersonnel("ALL", salesRooms, roles, 1))
      .ContinueWith(
      (task1) =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error);
          StaEnd();
          return false;
        }
        else
        {
          if (task1.IsCompleted)
          {
            List<PersonnelShort> data = task1.Result;
            if (data.Count > 0)
            {
              data.Insert(0, new PersonnelShort() { peID = "ALL", peN = "ALL", deN = "ALL" });
              cbxPersonnel.ItemsSource = data;
            }
            setNewUserLogin();
          }
          StaEnd();
          return false;
        }
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }

    /// <summary>
    /// Obtiene los Sales by Closer
    /// </summary>
    /// <param name="dateFrom">fecha inicial</param>
    /// <param name="dateTo">fecha final</param>
    /// <param name="saleRooms">SalesRooms</param>
    /// <param name="closer">closer</param>
    /// <history>
    /// [erosado] 24/Mar/2016 Created
    /// </history>
    public void DoGetSalesByCloser(DateTime dateFrom, DateTime dateTo, string salesRoom, string closer)
    {
      Task.Factory.StartNew(() => BRSales.GetSalesByCloser(dateFrom, dateTo, salesRoom, closer))
      .ContinueWith(
      (task1) =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error);
          StaEnd();
          imgButtonOk.IsEnabled = true;
          return false;
        }
        else
        {
          if (task1.IsCompleted)
          {
            task1.Wait(1000);
            List<SaleByCloser> data = task1.Result;

            if (data.Count > 0)
            {
              dtgr.DataContext = data;
              StatusBarReg.Content = string.Format("{0}/{1}", (dtgr.SelectedIndex + 1).ToString(), dtgr.Items.Count.ToString());
            }
            else
            {
              UIHelper.ShowMessage("There is no data");
              dtgr.DataContext = null;
            }
            StaEnd();
            imgButtonOk.IsEnabled = true;

          }

          return false;
        }
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }
    #endregion

    #region StatusBar
    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }

    /// <summary>
    /// Verifica que la tecla se encuentre activa/inactiva, para cambiar el estilo de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void CkeckKeysPress(System.Windows.Controls.Primitives.StatusBarItem statusBar, Key key)
    {
      var keyPess = Keyboard.GetKeyStates(key).ToString();

      if (keyPess.Contains("Toggled")) //si está activo el Bloq Mayús
      {
        statusBar.FontWeight = FontWeights.Bold;
        statusBar.Foreground = Brushes.Black;
      }
      else
      {
        KeyDefault(statusBar);
      }
    }

    /// <summary>
    /// Configuracion inicial de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }

    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    /// <param name="message">mensaje</param>
    private void StaStart(String message)
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      this.Cursor = Cursors.Wait;

    }

    /// <summary>
    /// Indica en la barra de estado que se termina un proceso
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    /// <param name="message">mensaje</param>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      this.Cursor = null;
    }

    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Metodos
    /// <summary>
    /// Este metodo se encarga de validar y actualizar los permisos del usuario logeado sobre el sistema
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    public void setNewUserLogin()
    {
      //Agregamos la informacion del usuario en la interfaz
      txtbUserName.Text = App.User.User.peN;
      txtbLocation.Text = App.User.SalesRoom.srN;
      //Validamos permisos y restricciones para el combobox
      cbxPersonnel.IsEnabled = true;

      if (cbxPersonnel.Items.Count > 0)
      {
        List<PersonnelShort> lstPS = cbxPersonnel.ItemsSource as List<PersonnelShort>;
        int index = lstPS.FindIndex(x => x.peID.Equals(App.User.User.peID));
        if (index != -1)
        {
          cbxPersonnel.SelectedIndex = index;
        }
        else
        {
          cbxPersonnel.SelectedIndex = 0;
        }
      }
      else
      {
        cbxPersonnel.Text = "No data found - Press Ctrl+F5 to load Data";
      }

    }

    /// <summary>
    /// Carga personal en el combobox
    /// </summary>
    /// <history>
    /// [erosado] 18/04/2016  Created
    /// </history>
    public void LoadPersonnel()
    {
      StaStart("Loading Personnel...");
      DoGetPersonnel(App.User.SalesRoom.srID, Model.Helpers.EnumToListHelper.GetEnumDescription(EnumRole.Closer));
    }
    #endregion
  }
}

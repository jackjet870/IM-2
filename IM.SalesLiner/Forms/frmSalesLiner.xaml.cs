using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using System.Diagnostics;
using IM.Base.Forms;
using IM.Model.Enums;
using IM.Model.Helpers;
using Xceed.Wpf.Toolkit;

namespace IM.SalesLiner.Forms
{
  /// <summary>
  /// Interaction logic for frmSalesLiner.xaml
  /// </summary>
  public partial class frmSalesLiner : Window
  {
    #region Propiedades, Atributos
    private List<Tuple<string, string>> filtersReport;
    public ExecuteCommandHelper LoadCombo { get; set; }
    #endregion
    public frmSalesLiner()
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
      dtpkFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      dtpkTo.Value = DateTime.Now;
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
      if (DateHelper.ValidateValueDate(dtpkFrom, dtpkTo))
      {
        if (cbxPersonnel?.SelectedValue != null)
        {
          StaStart("Loading data...");
          imgButtonOk.IsEnabled = false;
          var linerPersonalShort = cbxPersonnel.SelectedValue as PersonnelShort;
          filtersReport = new List<Tuple<string, string>>();
          filtersReport.Add(new Tuple<string, string>("Liner", string.Concat(linerPersonalShort.peID, " - ", linerPersonalShort.peN)));

          DoGetSalesByLiner(dtpkFrom.Value.Value, dtpkTo.Value.Value, App.User.SalesRoom.srID, linerPersonalShort.peID);
        }
        else
        {
          UIHelper.ShowMessage("Please select a personnel", MessageBoxImage.Warning);
          cbxPersonnel.Focus();
        }
      }
    }
    /// <summary>
    /// Evento que se lanza cuando generamos nuestro reporte boton Print
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private async void imgButtonPrint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var listaSaleByLiner = dtgr.DataContext as List<SaleByLiner>;
      if (listaSaleByLiner != null)
      {
        var dateRangeFileName = DateHelper.DateRangeFileName(dtpkFrom.Value.Value, dtpkTo.Value.Value);
        //Obtenemos el nombre del reporte y el dateRange
        const string rptName = "Sales By Liner";
        //Obtenemos el dataTable con la lista formateada
        var dt = TableHelper.GetDataTableFromList(listaSaleByLiner, true);
        //Creamos el reporte
        var fi = await EpplusHelper.CreateCustomExcel(dt, filtersReport, rptName, dateRangeFileName, Utilities.UseFulMethods.getExcelFormatTable(), addEnumeration: true);

        if (fi != null)
        {
          frmDocumentViewer documentViewer = new frmDocumentViewer(fi, App.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly), false);
          documentViewer.ShowDialog();
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
      formAbout.Owner = this;
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
      Close();
    }

    /// <summary>
    /// Evento de dispara cuando el usuario cambia de sesion en el modulo.
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private async void imageLogOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var frmlogin = new frmLogin(loginType: EnumLoginType.SalesRoom, changePassword: true, autoSign: true, switchLoginUserMode: true);
      await frmlogin.getAllPlaces();
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

    /// <summary>
    /// Enviamos el Focus al siguiente DateTimePicker To o si esta en DateTimePicker From se va el focus al boton Search
    /// </summary>
    /// <history>
    /// [erosado] 01/07/2016  Created.
    /// </history>
    private void dtpkEnterKey(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        DateTimePicker dtpk = sender as DateTimePicker;
        if (dtpk.Name == "dtpkFrom")
        {
          dtpkTo.Focus();
        }
        else
        {
          cbxPersonnel.Focus();
        }
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
    /// <param name="salesRooms">srID</param>
    /// <param name="roles">rol del usuario loggeado</param>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async void DoGetPersonnel(string salesRooms, string roles)
    {
      try
      {
        var data = await BRPersonnel.GetPersonnel("ALL", salesRooms, roles);
        if (data.Count > 0)
        {
          data.Insert(0, new PersonnelShort() { peID = "ALL", peN = "ALL", deN = "ALL" });
          cbxPersonnel.ItemsSource = data;
        }
        setNewUserLogin();
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex.InnerException.Message, MessageBoxImage.Error);
      }
    }

    /// <summary>
    /// Obtiene los Sales Liner
    /// </summary>
    /// <param name="dateFrom">fecha inicial</param>
    /// <param name="dateTo">fecha final</param>
    /// <param name="salesRoom">srID</param>
    /// <param name="Liner">peID</param>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    public async void DoGetSalesByLiner(DateTime dateFrom, DateTime dateTo, string salesRoom, string Liner)
    {
      try
      {
        var data = await BRSales.GetSalesByLiner(dateFrom, dateTo, salesRoom, Liner);

        if (data.Count > 0)
        {
          dtgr.DataContext = data;
          StatusBarReg.Content = $"{(dtgr.SelectedIndex + 1).ToString()}/{dtgr.Items.Count.ToString()}";
        }
        else
        {
          UIHelper.ShowMessage("There is no data");
          dtgr.DataContext = null;
        }
        StaEnd();
        imgButtonOk.IsEnabled = true;
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }


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
      Cursor = Cursors.Wait;

    }

    /// <summary>
    /// Indica en la barra de estado que se termina un proceso
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      Cursor = null;
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
        selectPersonnelInCombobox(App.User.User.peID);
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
    /// [vipacheco] 02/Mayo/2016 Modified --> Se cambio el parametro de string a un tipo EnumRole
    /// </history>
    public void LoadPersonnel()
    {
      StaStart("Loading Personnel...");
      DoGetPersonnel(App.User.SalesRoom.srID, EnumToListHelper.GetEnumDescription(EnumRole.Liner));
    }

    /// <summary>
    /// Busca en una lista y selecciona al personal
    /// </summary>
    /// <param name="user">peID</param>
    /// <history>
    /// [erosado] 25/04/2016
    /// </history>
    private void selectPersonnelInCombobox(string user)
    {
      var lstPS = cbxPersonnel.ItemsSource as List<PersonnelShort>;
      var index = lstPS.FindIndex(x => x.peID.Equals(user));
      cbxPersonnel.SelectedIndex = index != -1 ? index : 0;
    }
    #endregion
  }
}

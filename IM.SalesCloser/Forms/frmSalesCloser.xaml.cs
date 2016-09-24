using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Data;
using System.IO;
using Xceed.Wpf.Toolkit;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;

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
          imgButtonOk.Focus();
          StaStart("Loading data...");
          imgButtonOk.IsEnabled = false;
          filtersBool = new List<bool>();
          PersonnelShort closer = cbxPersonnel.SelectedValue as PersonnelShort;
          filtersReport = new List<Tuple<string, string>>();
          filtersReport.Add(new Tuple<string, string>("Closer", string.Concat(closer.peID, " - ", closer.peN)));
          DoGetSalesByCloser(dtpkFrom.Value.Value, dtpkTo.Value.Value, Context.User.SalesRoom.srID, closer.peID);
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
    /// [emoguel] 09/09/2016 Modified. Ahora se abre el visor de reportes
    /// </history>
    private async void imgButtonPrint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      List<SaleByCloser> listaSaleByCloser = dtgr.DataContext as List<SaleByCloser>;
      if (listaSaleByCloser != null)
      {
        //Obtenemos dateRange
        string dateRange = DateHelper.DateRange(dtpkFrom.Value.Value, dtpkTo.Value.Value);
        string dateRangeFileName = DateHelper.DateRangeFileName(dtpkFrom.Value.Value, dtpkTo.Value.Value);
        //Obtenemos el nombre del reporte y el dateRange
        string rptName = "Sales By Closer";
        //Obtenemos el dataTable con la lista formateada
        DataTable dt = TableHelper.GetDataTableFromList(listaSaleByCloser, true);
        //Creamos el reporte
        FileInfo fi = await ReportBuilder.CreateCustomExcel(dt, filtersReport, rptName, dateRangeFileName, Utilities.UseFulMethods.getExcelFormatTable(), addEnumeration: true);


        if (fi != null)
        {
          frmDocumentViewer documentViewer = new frmDocumentViewer(fi, Context.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly), false);
          documentViewer.Owner = this;
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
      this.Close();
    }

    /// <summary>
    /// Evento de dispara cuando el usuario cambia de sesion en el modulo.
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private async void imageLogOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      frmLogin frmlogin = new frmLogin(loginType: EnumLoginType.SalesRoom, changePassword: true, autoSign: true, switchLoginUserMode: true);
      await frmlogin.getAllPlaces();
      if (Context.User.AutoSign)
      {
        frmlogin.UserData = Context.User;
      }
      frmlogin.ShowDialog();

      if (frmlogin.IsAuthenticated)
      {
        Context.User = frmlogin.UserData;
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
      StatusBarReg.Content = $"{(dtgr.SelectedIndex + 1).ToString()}/{dtgr.Items.Count.ToString()}";
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

    public async void DoGetPersonnel(string salesRooms, string roles)
    {
      try
      {
        List<PersonnelShort> data = await BRPersonnel.GetPersonnel("ALL", salesRooms, roles, 1);
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
        UIHelper.ShowMessage(ex);
      }
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
    public async void DoGetSalesByCloser(DateTime dateFrom, DateTime dateTo, string salesRoom, string closer)
    {
      try
      {
        var data = await BRSales.GetSalesByCloser(dateFrom, dateTo, salesRoom, closer);
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
      txtbUserName.Text = Context.User.User.peN;
      txtbLocation.Text = Context.User.SalesRoom.srN;
      //Validamos permisos y restricciones para el combobox
      cbxPersonnel.IsEnabled = true;

      if (cbxPersonnel.Items.Count > 0)
      {
        List<PersonnelShort> lstPS = cbxPersonnel.ItemsSource as List<PersonnelShort>;
        int index = lstPS.FindIndex(x => x.peID.Equals(Context.User.User.peID));
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
      DoGetPersonnel(Context.User.SalesRoom.srID, EnumToListHelper.GetEnumDescription(EnumRole.Closer));
    }
    #endregion
  }
}

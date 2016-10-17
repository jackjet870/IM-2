﻿using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.GuestsPR.Utilities;
using IM.Model;
using IM.Model.Enums;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using System.Linq;

namespace IM.GuestsPR.Forms
{
  /// <summary>
  /// Interaction logic for frmGuestsPR.xaml
  /// </summary>
  public partial class frmGuestsPR : Window
  {
    #region Propiedades, Atributos
    private List<bool> filtersBool;
    private List<Tuple<string, string>> filtersReport;
    public ExecuteCommandHelper LoadCombo { get; set; }
    #endregion

    public frmGuestsPR()
    {
      InitializeComponent();
      LoadCombo = new ExecuteCommandHelper(x => LoadPersonnel());
    }

    #region Eventos Ventana

    #region Window_Loaded
    /// <summary>
    /// Evento que se lanza al iniciar la aplicacion
    /// </summary>
    /// <history>
    /// [erosado] 17/Mar/2016 Created
    /// </history>   
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadPersonnel();

      //Seleccionamos los días en el datapicker 
      dtpkFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      dtpkTo.Value = DateTime.Now;
      //Agregamos login del usuario en la interfaz
      SetNewUserLogin();
    }
    #endregion

    #region imgButtonOk_MouseLeftButtonDown
    /// <summary>
    /// Evento que se lanza cuando realizamos la consulta boton Search
    /// </summary>
    /// <history>
    /// [erosado] 17/Mar/2016 Created
    /// </history>
    private void imgButtonOk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      //Limpiamos el Grid y buscamos la informacion
      dtgr.DataContext = null;
      GetGuestByPR();
    }
    #endregion

    #region imgButtonPrint_MouseLeftButtonDown
    /// <summary>
    /// Evento que se lanza cuando generamos nuestro reporte boton Print
    /// </summary>
    /// <history>
    /// [erosado] 17/Mar/2016 Created
    /// </history>
    private async void imgButtonPrint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var listaGuestByPR = dtgr.DataContext as List<GuestByPR>;
      if (listaGuestByPR != null)
      {
        var dateRangeFileName = DateHelper.DateRangeFileName(dtpkFrom.Value.Value, dtpkTo.Value.Value);
        //Obtenemos el nombre del reporte y el dateRange
        var rptName = "Guests By PR";
        //Obtenemos el dataTable con la lista formateada
        var dt = TableHelper.GetDataTableFromList(listaGuestByPR, true);
        //Creamos el reporte
        var fi = await ReportBuilder.CreateCustomExcelAsync(dt, filtersReport, rptName, dateRangeFileName, UseFulMethods.getExcelFormatTable(), addEnumeration: true);
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
    #endregion

    #region imgButtonAbout_MouseLeftButtonDown
    /// <summary>
    /// Evento que se lanza cuando abrimos la ventana About boton About
    /// </summary>
    /// <history>
    /// [erosado] 17/Mar/2016 Created
    /// </history>
    private void imgButtonAbout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var formAbout = new frmAbout();
      formAbout.Owner = this;
      formAbout.ShowDialog();
    }
    #endregion

    #region imgButtonExit_MouseLeftButtonDown
    /// <summary>
    /// Evento que se lanza cuando queremos salir de la aplicacion boton exit
    /// </summary>
    /// <history>
    /// [erosado] 17/Mar/2016 Created
    /// </history>
    private void imgButtonExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Close();
    }
    #endregion

    #region imageLogOut_MouseLeftButtonDown
    /// <summary>
    /// Evento de dispara cuando el usuario cambia de sesion en el modulo.
    /// </summary>
    /// <history>
    /// [erosado] 19/Mar/2016 Created
    /// </history>
    private async void imageLogOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var frmlogin = new frmLogin(loginType: EnumLoginType.Location, changePassword: true, autoSign: true, switchLoginUserMode: true);
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
    #endregion

    #region dtpEnterKey
    /// <summary>
    /// Enviamos el Focus al siguiente DatePicker To o si esta en DatePicker From se va el focus al boton Search
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
    /// [erosado] 16/Mar/2016 Created
    /// </history>
    private void dtgr_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarReg.Content = string.Format("{0}/{1}", (dtgr.SelectedIndex + 1).ToString(), dtgr.Items.Count.ToString());
    }
    #endregion

    #endregion

    #region Async Methods

    #region DoGetPersonnel
    /// <summary>
    /// Obtiene la lista del personal
    /// </summary>
    /// <param name="leadSources">filtro leadsources</param>
    /// <param name="roles">rol del usuario loggeado</param>
    /// <history>
    /// [erosado] Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async void DoGetPersonnel(string leadSources, string roles)
    {
      try
      {
        var data = await BRPersonnel.GetPersonnel(leadSources, "ALL", roles);
        if (data.Count > 0)
        {
          data.Insert(0, new PersonnelShort() { peID = "ALL", peN = "ALL", deN = "ALL" });
          cbxPersonnel.ItemsSource = data;
        }
        SetNewUserLogin();
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region DoGetGuestsByPR
    /// <summary>
    /// Obtiene los Guests By PR
    /// </summary>
    /// <param name="dateFrom">fecha inicial</param>
    /// <param name="dateTo">fecha final</param>
    /// <param name="leadSources">LeadoSource</param>
    /// <param name="PR">Pr</param>
    /// <param name="filters">Filters</param>
    /// <history>
    /// [erosado] 16/Mar/2016 Created
    /// </history>
    public async void DoGetGuestsByPR(DateTime dateFrom, DateTime dateTo, string leadSources, string PR, List<bool> filters)
    {
      try
      {
        var data = await BRGuests.GetGuestsByPR(dateFrom, dateTo, leadSources, PR, filters);
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
        imgButtonPrint.IsEnabled = true;
      }
      catch (Exception ex)
      {
        StaEnd();
        imgButtonOk.IsEnabled = true;
        UIHelper.ShowMessage(ex);
      }


    }
    #endregion

    #endregion

    #region StatusBar
    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [erosado] 15/Mar/2016 Created
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
    /// [erosado] 15/Mar/2016 Created
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
    /// [erosado] 15/Mar/2016 Created
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
    /// [erosado] 15/Mar/2016 Created
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
    /// [erosado] 15/Mar/2016 Created
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
    /// [erosado] 15/Mar/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Metodos

    #region SetNewUserLogin
    /// <summary>
    /// Este metodo se encarga de validar y actualizar los permisos del usuario logeado sobre el sistema
    /// </summary>
    /// <history>
    /// [erosado] 19/Mar/2016 Created
    /// </history>
    public void SetNewUserLogin()
    {
      //Agregamos la informacion del usuario en la interfaz
      txtbUserName.Text = Context.User.User.peN;
      txtbLocation.Text = Context.User.Location.loN;
      //Validamos permisos y restricciones para el combobox
      if (Context.User.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.Special))
      {
        cbxPersonnel.IsEnabled = true;
        if (cbxPersonnel.Items.Count > 0)
        {
          selectPersonnelInCombobox(Context.User.User.peID, true);
        }
        else
        {
          cbxPersonnel.Text = "No data found - Press Ctrl+F5 to load Data";
        }
      }
      else
      {
        cbxPersonnel.IsEnabled = false;
        if (cbxPersonnel.Items.Count > 0)
        {
          selectPersonnelInCombobox(Context.User.User.peID, false);
        }
        else
        {
          cbxPersonnel.Text = "No data found - Press Ctrl+F5 to load Data";
        }
      }
    }
    #endregion

    #region LoadPersonnel
    /// <summary>
    /// Carga personal en el combobox
    /// </summary>
    /// <history>
    /// [erosado] 18/04/2016  Created
    /// </history>
    public void LoadPersonnel()
    {
      StaStart("Loading personnel...");
      DoGetPersonnel(Context.User.LeadSource.lsID, "PR");
    }
    #endregion

    #region selectPersonnelInCombobox
    /// <summary>
    /// Busca en una lista y selecciona al personal
    /// </summary>
    /// <param name="user">peID</param>
    /// <history>
    /// [erosado] 25/04/2016
    /// </history>
    private bool selectPersonnelInCombobox(string user, bool specialLevel)
    {
      var lstPS = cbxPersonnel.ItemsSource as List<PersonnelShort>;
      var index = lstPS.FindIndex(x => x.peID.Equals(user));
      if (index != -1)
      {
        cbxPersonnel.SelectedIndex = index;
        GetGuestByPR();
        return true;
      }
      else
      {
        //Limpiamos el DataGrid
        dtgr.DataContext = null;

        //Si no tiene permisos especiales  deshabilitamos los controles
        if (!specialLevel)
        {
          imgButtonOk.IsEnabled = false;
          imgButtonPrint.IsEnabled = false;
        }
        cbxPersonnel.SelectedItem = null;

        return false;
      }
    }
    #endregion

    #region GetGuestByPR
    /// <summary>
    /// Trae los Guest del PR seleccionado.
    /// </summary>
    /// <history>
    /// [erosado] 19/07/2016  Created.
    /// </history>
    private void GetGuestByPR()
    {
      if (DateHelper.ValidateValueDate(dtpkFrom, dtpkTo))
      {
        if (cbxPersonnel?.SelectedValue != null)
        {
       
          if (chkAssign?.IsChecked == true || chkContact?.IsChecked == true || chkFollowUp?.IsChecked == true || chkInvitation?.IsChecked == true || chkShows?.IsChecked == true)
          {
            imgButtonOk.IsEnabled = false;            
            filtersBool = new List<bool>();
            var leadSource = (chkLeadSource.IsChecked == true ? "ALL" : Context.User.LeadSource.lsID);
            var personnelShort = cbxPersonnel.SelectedValue as PersonnelShort;
            #region Check Filter for Report
            filtersReport = new List<Tuple<string, string>>();

            filtersReport.Add(chkLeadSource.IsChecked == true ? new Tuple<string, string>("Lead Source", "ALL") : new Tuple<string, string>("Lead Source", Context.User.LeadSource.lsID));
            filtersReport.Add(chkContact.IsChecked == true ? new Tuple<string, string>("Contacts", "YES") : new Tuple<string, string>("Contacts", "ALL"));
            filtersReport.Add(chkFollowUp.IsChecked == true ? new Tuple<string, string>("Follow Up", "YES") : new Tuple<string, string>("Follow Up", "ALL"));
            filtersReport.Add(chkInvitation.IsChecked == true ? new Tuple<string, string>("Invitation", "YES") : new Tuple<string, string>("Invitation", "ALL"));
            filtersReport.Add(chkShows.IsChecked == true ? new Tuple<string, string>("Shows", "YES") : new Tuple<string, string>("Shows", "ALL"));
            filtersReport.Add(chkWithSale.IsChecked == true ? new Tuple<string, string>("With Sale", "YES") : new Tuple<string, string>("With Sale", "ALL"));
            filtersReport.Add(chkBasedOnArrival.IsChecked == true ? new Tuple<string, string>("Based On Arrival Date", "YES") : new Tuple<string, string>("Based On Arrival Date", "ALL"));

            filtersBool.Add(chkAssign.IsChecked ?? false);
            filtersBool.Add(chkContact.IsChecked ?? false);
            filtersBool.Add(chkFollowUp.IsChecked ?? false);
            filtersBool.Add(chkInvitation.IsChecked ?? false);
            filtersBool.Add(chkShows.IsChecked ?? false);
            filtersBool.Add(chkWithSale.IsChecked ?? false);
            filtersBool.Add(chkBasedOnArrival.IsChecked ?? false);
            #endregion

            StaStart("Loading data...");
            DoGetGuestsByPR(dtpkFrom.Value.Value, dtpkTo.Value.Value, leadSource, personnelShort.peID, filtersBool);
          }
          else
          {
            UIHelper.ShowMessage("Please specify at least one of the following 5 options: Assign, Contact, Follow Up, Invit, Show", MessageBoxImage.Warning);
          }
        }
        else
        {
          UIHelper.ShowMessage("Please select a personnel", MessageBoxImage.Warning);
          cbxPersonnel.Focus();
        }
      }
    }
    #endregion

    #endregion
  }
}

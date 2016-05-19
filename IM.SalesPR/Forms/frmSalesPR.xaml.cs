﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

namespace IM.SalesPR.Forms
{
  public partial class frmSalesPR : Window
  {
    #region Propiedades, Atributos

    private List<Tuple<string, string>> _filtersReport;
    public ExecuteCommandHelper LoadCombo { get; set; }
    #endregion

    public frmSalesPR()
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
      SetNewUserLogin();
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
      //new List<bool>();
      var leadSource = (chkLeadSource.IsChecked == true ? "ALL" : App.User.LeadSource.lsID);
      var personnelShort = cbxPersonnel.SelectedValue as PersonnelShort;
      _filtersReport = new List<Tuple<string, string>>();
      _filtersReport.Add(chkLeadSource.IsChecked == true ? new Tuple<string, string>("Lead Source", "ALL") : new Tuple<string, string>("Lead Source", App.User.LeadSource.lsID));

      if (dtpkFrom.SelectedDate != null && dtpkTo.SelectedDate != null)
      {
        DoGetSalesByPr(dtpkFrom.SelectedDate.Value, dtpkTo.SelectedDate.Value, leadSource, personnelShort?.peID, (bool)rdoSalesPr.IsChecked);
      }
    }
    /// <summary>
    /// Evento que se lanza cuando generamos nuestro reporte boton Print
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void imgButtonPrint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var listaSaleByPr = dtgr.DataContext as List<SaleByPR>;
      if (listaSaleByPr != null)
      {
        if (dtpkFrom.SelectedDate != null & dtpkTo.SelectedDate != null)
        {
          var dateRangeFileName = DateHelper.DateRangeFileName(dtpkFrom.SelectedDate.Value, dtpkTo.SelectedDate.Value);
          //Obtenemos el nombre del reporte y el dateRange
          const string rptName = "Sales By PR";
          //Obtenemos el dataTable con la lista formateada 
          var dt = TableHelper.GetDataTableFromList(listaSaleByPr, true);
          //Creamos el reporte
          var fi = EpplusHelper.CreateGeneralRptExcel(_filtersReport, dt, rptName, dateRangeFileName, Utilities.UseFulMethods.getExcelFormatTable());

          if (fi != null)
          {
            Process.Start(fi.FullName);
          }
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
    /// [erosado] 22/Mar/2016 Created
    /// </history>
    private void imgButtonExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Close();
    }

    /// <summary>
    /// Evento de dispara cuando el usuario cambia de sesion en el modulo.
    /// </summary>
    /// <history>
    /// [erosado] 22/Mar/2016 Created
    /// </history>
    private void imageLogOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var frmlogin = new frmLogin(loginType: EnumLoginType.Location, changePassword: true, autoSign: true);
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
      StatusBarReg.Content = $"{(dtgr.SelectedIndex + 1)}/{dtgr.Items.Count}";
    }
    #endregion

    #region Async Methods
    /// <summary>
    /// Obtiene la lista del personal
    /// </summary>
    /// <param name="leadSources">filtro leadsources</param>
    /// <param name="roles">rol del usuario loggeado</param>
    /// <history>
    /// [erosado] 22/Mar/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async void DoGetPersonnel(string leadSources, string roles)
    {
      try
      {
        var data =await  BRPersonnel.GetPersonnel(leadSources, "ALL", roles);
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
        UIHelper.ShowMessage(ex.InnerException.Message, MessageBoxImage.Error);
      }
    }

    /// <summary>
    /// Obtiene los Guests By PR
    /// </summary>
    /// <param name="dateFrom">fecha inicial</param>
    /// <param name="dateTo">fecha final</param>
    /// <param name="leadSources">LeadoSource</param>
    /// <param name="PR">Pr</param>
    /// <param name="searchBySalePr">True - SearchBySalePr  - False SearchByContacts</param>
    /// <history>
    /// [erosado] 22/Mar/2016 Created
    /// </history>
    public void DoGetSalesByPr(DateTime dateFrom, DateTime dateTo, string leadSources, string PR, bool searchBySalePr)
    {
      Task.Factory.StartNew(() => BRSales.GetSalesByPR(dateFrom, dateTo, leadSources, PR, searchBySalePr))
      .ContinueWith(
      (task1) =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception?.InnerException.Message, MessageBoxImage.Error);
          StaEnd();
          imgButtonOk.IsEnabled = true;
          return false;
        }
        else
        {
          if (task1.IsCompleted)
          {
            task1.Wait(1000);
            List<SaleByPR> data = task1.Result;

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
    /// [erosado] 22/Mar/2016 Created
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
    /// [erosado] 22/Mar/2016 Created
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
    /// [erosado] 22/Mar/2016 Created
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
    /// [erosado] 22/Mar/2016 Created
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
    /// [erosado] 22/Mar/2016 Created
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
    /// [erosado] 22/Mar/2016 Created
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
    /// [erosado] 22/Mar/2016 Created
    /// </history>
    public void SetNewUserLogin()
    {
      //Agregamos la informacion del usuario en la interfaz
      txtbUserName.Text = App.User.User.peN;
      txtbLocation.Text = App.User.Location.loN;
      //Validamos permisos y restricciones para el combobox

      if (App.User.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.Special))
      {
        chkLeadSource.Visibility = Visibility.Visible;
        cbxPersonnel.IsEnabled = true;
        if (cbxPersonnel.Items.Count > 0)
        {
          var lstPs = cbxPersonnel.ItemsSource as List<PersonnelShort>;
          cbxPersonnel.SelectedIndex = lstPs.FindIndex(x => x.peID == App.User.User.peID);
        }
        else
        {
          cbxPersonnel.Text = "No data found - Press Ctrl+F5 to load Data";
        }
      }
      else
      {
        chkLeadSource.Visibility = Visibility.Collapsed;
        cbxPersonnel.IsEnabled = false;
        if (cbxPersonnel.Items.Count > 0)
        {
          var lstPs = cbxPersonnel.ItemsSource as List<PersonnelShort>;
          var index = lstPs.FindIndex(x => x.peID.Equals(App.User.User.peID));
          cbxPersonnel.SelectedIndex = index;
        }
        else
        {
          cbxPersonnel.Text = "No data found";
        }
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
      DoGetPersonnel(App.User.LeadSource.lsID, Model.Helpers.EnumToListHelper.GetEnumDescription(EnumRole.PR));
    }
    #endregion
  }
}

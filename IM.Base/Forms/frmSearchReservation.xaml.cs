using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Services.Helpers;
using IM.Services.WirePRService;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Xceed.Wpf.Toolkit;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchReservation.xaml
  /// </summary>
  public partial class frmSearchReservation : Window
  {

    private UserData _user;
    private string _leadSourceID;
    public ReservationOrigos _reservationInfo;

    #region Constructor
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userdata">Informacion del Usuario Logueado</param>
    /// <param name="leadSourceID">Clave del Lead Source</param>
    /// <history>
    /// [vipacheco] 17/Agosto/2016 Created
    /// </history>
    public frmSearchReservation(UserData userdata)
    {
      _user = userdata;
      _leadSourceID = userdata.LeadSource != null ? userdata.LeadSource.lsID : userdata.SalesRoom.srID;
      InitializeComponent();
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga los valores iniciales del formulario
    /// </summary>
    /// <history>
    /// [vipacheco] 17/Agosto/2016 Created
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      // Cargamos el combo de LeadSource
      cboLeadSourse.ItemsSource = await BRLeadSources.GetLeadSourcesByUser(_user.User.peID, EnumProgram.Inhouse);
      cboLeadSourse.SelectedValue = _user.LeadSource != null ? _user.LeadSource.lsID : _user.SalesRoom.srID;
      cboLeadSourse.IsEnabled = (_leadSourceID == "");

      DateTime dateServer = BRHelpers.GetServerDate();
      // Fecha Inicial
      dtpStart.Value = dateServer.AddDays(-7);
      // Fecha Final
      dtpEnd.Value = dateServer;

      // Verificamos que Bloq Key estan activos
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Metodos de Boton

    #region btnSearch_MouseLeftButtonDown
    /// <summary>
    /// Realiza la busqueda de los Guest con los criterios ingresados
    /// </summary>
    /// <history>
    /// [vipacheco] 17/Agosto/2016 Created
    /// </history>
    private async void btnSearch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (ValidateCriteria())
      {
        _busyIndicator.IsBusy = true;
        await LoadGrid();
        _busyIndicator.IsBusy = false;
      }
    }
    #endregion

    #region btnOk_MouseLeftButtonDown
    /// <summary>
    /// Obtiene el guest seleccionado y cierra el formulario
    /// </summary>
    /// <history>
    /// [vipacheco] 17/Agosto/2016 Created
    /// </history>
    private void btnOk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      // Verificamos que al menos se tenga un guest seleccionado.
      if (grdGuests.SelectedItems.Count == 0)
      {
        UIHelper.ShowMessage("Select at least one Guest", title: "IM Search");
        return;
      }
      DialogResult = true;
      _reservationInfo = grdGuests.SelectedItem as ReservationOrigos;
      Close();
    }
    #endregion

    #region btnCancel_MouseLeftButtonDown
    /// <summary>
    /// Cancela la busqueda y cierra el formulario
    /// </summary>
    /// <history>
    /// [vipacheco] 17/Agosto/2016 Created
    /// </history>
    private void btnCancel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      DialogResult = false;
      Close();
    }
    #endregion 

    #endregion

    #region LoadGrid
    /// <summary>
    /// Carga el datagrid con los guests encontrados según los criterios de busqueda ingresados.
    /// </summary>
    /// <history>
    /// [vipacheco] 17/Agosto/2016 Created
    /// </history>
    private async Task LoadGrid()
    {
      try
      {
        ReservationOrigos[] reservation;
        reservation = await WirePRHelper.GetReservationsOrigos(cboLeadSourse.SelectedValue.ToString(), txtFolio.Text, txtRoomNum.Text, txtName.Text, dtpStart.Value.Value, dtpEnd.Value.Value);

        // Verificamos si se encontro alguna reservacion.
        if (!reservation.Any())
        {
          UIHelper.ShowMessage("No reservations found", MessageBoxImage.Information);
          return;
        }
        grdGuests.ItemsSource = reservation;
      }
      catch (Exception exception)
      {
        UIHelper.ShowMessage(exception);
      }
    }
    #endregion

    #region ValidateCriteria
    /// <summary>
    /// Valida los criterios de busqueda
    /// </summary>
    /// <returns> Tue - Todo es correcto | False - Sin criteria ingresado </returns>
    /// <history>
    /// [vipacheco] 17/Agosto/2016 Created
    /// </history>
    private bool ValidateCriteria()
    {
      // validamos el Lead Source
      if (cboLeadSourse.SelectedItem == null)
      {
        UIHelper.ShowMessage("Please specify the Lead Source.", MessageBoxImage.Information);
        cboLeadSourse.Focus();
        return false;
      }
      // validamos que haya al menos un criterio de busqueda
      else if (string.IsNullOrEmpty(txtFolio.Text) && string.IsNullOrEmpty(txtRoomNum.Text) && string.IsNullOrEmpty(txtName.Text))
      {
        UIHelper.ShowMessage("Please specify a search criteria.", MessageBoxImage.Information);
        txtFolio.Focus();
        return false;
      }
      // validamos las fechas
      else if (!DateHelper.ValidateValueDate(dtpStart, dtpEnd))
      {
        return false;
      }
      return true;
    }
    #endregion

    #region dtp_ValueChanged
    /// <summary>
    /// Cambio de fechas en el control DateTimePicker
    /// </summary>
    /// <history>
    /// [vipacheco] 17/Agosto/2016 Created
    /// </history>
    private void dtp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      DateHelper.ValidateValueDate((DateTimePicker)sender);
    }
    #endregion

    #region CkeckKeysPress
    /// <summary>
    /// Revisa si alguna de las teclas Bloq Mayús, Bloq Núm o Insert está activo
    /// </summary>
    /// <param name="statusBar">StatusBarItem al que se le asignaran valores si está activa la tecla</param>
    /// <param name="key">tecla que revisaremos si se encuentra activa</param>
    /// <history>
    /// [vipacheco] 08/Ago/2016 Created
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
    #endregion

    #region KeyDefault
    /// <summary>
    /// Asigna valores por defecto a los StatusBarItem cuando se carga el form
    /// </summary>
    /// <param name="statusBar">StatusBarItem al que se le asignaran los valores</param>
    /// <history>
    /// [vipacheco] 08/Ago/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Verifica cuando se presiona las teclas Bloq
    /// </summary>
    /// <history>
    /// [vipacheco] 17/Agosto/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }
    #endregion

    #region grdGuests_SelectionChanged
    /// <summary>
    /// Actualiza el total de resultados encontrados y la seleccion actual.
    /// </summary>
    /// <history>
    /// [vipacheco] 18/Agosto/2016 Created
    /// </history>
    private void grdGuests_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (grdGuests.Items.Count == 0)
      {
        StatusBarReg.Content = "0/0";
        return;
      }
      StatusBarReg.Content = string.Format("{0}/{1}", grdGuests.Items.IndexOf(grdGuests.CurrentItem) + 1, grdGuests.Items.Count);
    }
    #endregion

    #region Cell_DoubleClick    
    /// <summary>
    /// Selecciona la infomacion del guest pulsado
    /// </summary>
    /// <history>
    /// [vipacheco] 18/Agosto/2016 Created
    /// </history>
    private void grdGuest_DoubleClick(object sender, RoutedEventArgs e)
    {
      // Construimos el formulario a mostrar
      _reservationInfo = grdGuests.SelectedItem as ReservationOrigos;
      DialogResult = true;
      Close();

  }
    #endregion

    #region grdReceipts_KeyDown
    /// <summary>
    /// Seleciona la infomacion del guest seleccionado
    /// cambia de fila con el boton tab
    /// </summary>
    /// <history>
    /// [vipacheco] 18/Agosto/2016 Created
    /// </history>
    private void grdGuest_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            grdGuest_DoubleClick(null, null);
            blnHandled = true;
            break;
}
      }
      e.Handled = blnHandled;
    }
    #endregion

  }
}

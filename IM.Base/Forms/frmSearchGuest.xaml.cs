using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model.Classes;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using System.Windows.Media;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchGuest.xaml
  /// </summary>
  public partial class frmSearchGuest : Window
  {
    #region Atributos

    private EnumProgram _program;
    private UserData user;

    #endregion

    #region Propiedades

    public bool cancel { get; set; }
    public List<Guest> lstGuestAdd { get; set; }

    #endregion

    #region Contructores y Destructores
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userdata"> Informacion del Usuario </param>
    /// <param name="program"> Enumerado que identifica el tipo de programa origen</param>
    public frmSearchGuest(UserData userdata, EnumProgram program)
    {
      InitializeComponent();
      user = userdata;
      _program = program;
    }
    #endregion

    #region Metodos

    #region AddGuest
    /// <summary>
    /// Agrega un guest encontrado al listado 
    /// </summary>
    /// <history>
    ///   [ecanul] 19/07/2016 Created. (Creado metodo propio, antes se encontraba dentro del boton Ok)
    /// </history>
    private void AddGuest()
    {
      if (dtgGuests.SelectedItems.Count == 0)
      {
        UIHelper.ShowMessage("Select at least one Guest", title: "IM Search");
        return;
      }
      if (_program == EnumProgram.Outhouse && !user.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.Standard))
      {
        UIHelper.ShowMessage("Account has only read access.");
        return;
      }
      _busyIndicator.IsBusy = true;
      _busyIndicator.BusyContent = "Loading Selected Guests...";
      lstGuestAdd = dtgGuests.SelectedItems.OfType<Guest>().ToList();
      _busyIndicator.IsBusy = true;

      Close();
      cancel = false;
    }
    #endregion


    #endregion

    #region Eventos del Formulario

    #region dtp_ValueChanged
    /// <summary>
    /// Cambio de fechas en el control DateTimePicker
    /// </summary>
    /// <history>
    /// [ecanul] 28/07/2016 Created
    /// </history>
    private void dtp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      DateHelper.ValidateValueDate((DateTimePicker)sender);
    }
    #endregion

    #region ValidateCriteria
    private bool ValidateCriteria()
    {
      //validamos que haya al menos un criterio de busqueda
      if (cmbLeadSourse.SelectedIndex > -1 && string.IsNullOrEmpty(txtName.Name) &&
       string.IsNullOrEmpty(txtRoom.Text) && string.IsNullOrEmpty(txtReservation.Text) && string.IsNullOrEmpty(txtGUID.Text))
      {
        UIHelper.ShowMessage("Please specify a search criteria.", MessageBoxImage.Asterisk, "any criteria");
        cmbLeadSourse.Focus();
        return false;
      }
      else //validamos las fechas
        return ValidateDateRange();
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// 
    /// </summary>
    /// <history>
    /// [erosado] 24/05/2016  Modified. Se agregó asincronía
    /// [vipacheco] 05/Agosto/2016 Modified -> Se agrego switch para el manejo de los tipos de apertura del search, se elimino ambiguedad de la columna guBooKD, se corrigio 
    ///                                        la consulta de sales room de OutHouse
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      switch (_program)
      {
        case EnumProgram.Inhouse:
          // Cargamos el combo de LeadSource
          cmbLeadSourse.ItemsSource = await BRLeadSources.GetLeadSourcesByUser(user.User.peID, EnumProgram.Inhouse);
          cmbLeadSourse.SelectedValue = user.LeadSource.lsID;
          // Ocultamos los criterios de busqueda no necesarios para el caso
          stkSalesRoom.Visibility = stkPR.Visibility = Visibility.Collapsed;
          break;
        case EnumProgram.Outhouse:
          stkReservation.Visibility = btnCancel.Visibility = guHReservIDColumn.Visibility = guAccountGiftsCardColumn.Visibility = Visibility.Collapsed;
          btnOK.Content = "Transfer";
          guBookD.Visibility = stkSalesRoom.Visibility = Visibility.Visible;
          txbDateFrom.Text = "Book D. From";
          txbDateTo.Text = "Book D. To";
          txbName.Text = "Guest";
          guIDColumn.Header = "ID";
          dtgGuests.SelectionUnit = DataGridSelectionUnit.FullRow;
          dtgGuests.SelectionMode = DataGridSelectionMode.Single;
          guCheckInDColumn.Header = "Check In Date";
          guCheckOutDColumn.Header = "Check Out Date";

          // Cargamos el combo de LeadSource
          cmbLeadSourse.ItemsSource = await BRLeadSources.GetLeadSourcesByUser(user.User.peID, EnumProgram.Outhouse);
          cmbLeadSourse.SelectedValue = user.LeadSource.lsID;
          // Cargamos el combo de Sales Room
          cmbSalesRoom.ItemsSource = await BRSalesRooms.GetSalesRoomsByUser(user.User.peID);
          cmbSalesRoom.SelectedIndex = -1;
          break;
      }

      DateTime serverDate = BRHelpers.GetServerDate();
      dtpTo.Value = serverDate;
      dtpFrom.Value = serverDate.AddDays(-7);
      StatusBarReg.Content = "0 Guests";

      // Activamos los metodos encargado de verificar los bloq
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
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

    #region btnOK_Click
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      AddGuest();
    }
    #endregion

    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      cancel = true;
      Close();
    }

    #endregion

    #region Window_KeyDown
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

    #region Window_Closing
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      cancel = true;
    }
    #endregion

    #region ValidateDateRange
    /// <summary>
    /// Valida el valor de dtpStart sea mayo que dtpEnd
    /// </summary>
    /// <history>[jorcanche] 17/03/2016</history>
    public bool ValidateDateRange()
    {
      if (dtpFrom.Value.Value > dtpTo.Value.Value)
      {
        UIHelper.ShowMessage("Start Date can not be greater than End Date.", MessageBoxImage.Exclamation, "Date");
        DateTime dt = dtpTo.Value.Value.AddDays(-1);
        dtpFrom.Value = dt;
        dtpFrom.Focus();
        return false;
      }
      return true;
    }
    #endregion

    #region txtGUID_KeyDown
    /// <summary>
    /// Solo permite ingresar numeros en el combobox del Datagrid
    /// </summary>
    /// <history>
    /// [jorcanche] 04/05/2016 created
    /// </history>
    private void txtGUID_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
        e.Handled = false;
      else
        e.Handled = true;
    }

    #endregion

    #region dtgGuests_MouseDoubleClick
    private void dtgGuests_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      AddGuest();
    }
    #endregion

    #region btnSearch_MouseLeftButtonDown
    /// <summary>
    /// Metodo de busqueda de acuerdo a los criterios ingresados.
    /// </summary>
    /// <history>
    /// [vipacheco] 05/Agosto/2016 Created -> Se cambio del evento onclick porque se cambio el control
    /// </history>
    private async void btnSearch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (ValidateCriteria())
      {
        _busyIndicator.IsBusy = true;
        _busyIndicator.BusyContent = "Loading Guests...";

        dtgGuests.ItemsSource = await BRGuests.GetSearchGuestByLS(
                                cmbLeadSourse.SelectedValue != null ? cmbLeadSourse.SelectedValue.ToString() : string.Empty,
                                _program == EnumProgram.Inhouse ? string.Empty : cmbSalesRoom.SelectedIndex == -1 ? string.Empty : cmbSalesRoom.SelectedValue.ToString(),
                                txtName.Text,
                                txtRoom.Text,
                                txtReservation.Text,
                                (!txtGUID.Text.Equals(string.Empty) ? Convert.ToInt32(txtGUID.Text) : 0),
                                dtpFrom.Value.Value,
                                dtpTo.Value.Value,
                                _program,
                                txtPR.Text);

        StatusBarReg.Content = dtgGuests.Items.Count.ToString() + (dtgGuests.Items.Count == 1 ? " Guest" : " Guests");
        _busyIndicator.IsBusy = false;
      }
    }
    #endregion

    #endregion

  }
}
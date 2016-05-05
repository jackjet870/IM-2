using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Base.Classes;
using IM.Model.Classes;
using System.Windows.Controls;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchGuest.xaml
  /// </summary>
  public partial class frmSearchGuest : Window
  {
    #region Atributos

    private EnumProgram _program { get; set; }
    //Guest _guest;
    //LeadSource _leadSource;
    //private List<Guest> _lstGuests { get; set; }
    private UserData user;

    #endregion

    #region Propiedades

    public bool cancel { get; set; }
    public List<Guest> lstGuestAdd { get; set; }

    #endregion

    #region Contructores y Destructores
    public frmSearchGuest(UserData userdata, EnumProgram program = EnumProgram.Inhouse)
    {
      InitializeComponent();
      _program = program;
      user = userdata;
    }
    #endregion

    #region Metodos

    #region StaStart
    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <param name="message">mensaje a mostrar</param>
    /// <history>[ECANUL] 31-03-2016 Created </history>
    private void StaStart(String message)
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      this.Cursor = Cursors.Wait;
    }
    #endregion

    #region StaEnd
    /// <summary>
    /// Indica en la barra de estado que se finalizo un proceso
    /// </summary>
    /// <history>[ECANUL] 31-03-2016 Created </history>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      this.Cursor = null;
    }
    #endregion

    #endregion

    #region Eventos del Formulario

    #region btnSearch_Click
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateCriteria())
      {
        StaStart("Loading Guests...");

        dtgGuests.ItemsSource = BRGuests.GetSearchGuestByLS(
                                cmbLeadSourse.SelectedValue.ToString(),
                                _program == EnumProgram.Inhouse ? string.Empty : cmbSalesRoom.SelectedIndex == -1 ?string.Empty:cmbSalesRoom.SelectedValue.ToString(),                            
                                txtName.Text,
                                txtRoom.Text,
                                txtReservation.Text,
                                (!txtGUID.Text.Equals(string.Empty) ? Convert.ToInt32(txtGUID.Text) : 0),
                                dtpFrom.SelectedDate.Value,
                                dtpTo.SelectedDate.Value,
                                _program,
                                txtPR.Text);

        StatusBarReg.Content = dtgGuests.Items.Count.ToString() +  (dtgGuests.Items.Count == 1 ? " Guest" : " Guests");
        StaEnd();
      }
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
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      cmbLeadSourse.ItemsSource = BRLeadSources.GetLeadSources(1, _program);
      dtpTo.SelectedDate = BRHelpers.GetServerDate().Date;
      dtpFrom.SelectedDate = BRHelpers.GetServerDate().AddDays(-7).Date;
      cmbLeadSourse.SelectedValue = user.LeadSource.lsID;
      StatusBarReg.Content = "0 Guests";
      if (_program == EnumProgram.Outhouse)
      {
        cmbSalesRoom.ItemsSource = BRSalesRooms.GetSalesRooms(1);
        cmbSalesRoom.SelectedIndex = -1;
        spReservation.Visibility = btnCancel.Visibility = guBookD.Visibility = guHReservIDColumn.Visibility = guAccountGiftsCardColumn.Visibility = Visibility.Collapsed;
        btnOK.Content = "Transfer";
        Width = 950;
        spPR.Visibility = guBookD.Visibility = spSR.Visibility = Visibility.Visible;
        lblDateFrom.Content = "Book D. From";
        lblDateTo.Content = "Book D. To";
        guIDColumn.Header = "ID";
        dtgGuests.SelectionUnit = DataGridSelectionUnit.FullRow;
        dtgGuests.SelectionMode = DataGridSelectionMode.Single;
        guCheckInDColumn.Header = "Check In Date";
        guCheckOutDColumn.Header = "Check Out Date";
      } 
  
    }
    #endregion

    #region btnOK_Click
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      if (dtgGuests.SelectedItems.Count == 0)
      {
        MessageBox.Show("Select at least one Guest", "IM Search");
        return;
      }
      if (_program == EnumProgram.Outhouse && !user.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.Standard))
      {
        MessageBox.Show("Account has only read access.");
        return;
      }
      StaStart("Loading Selected Guests...");
      //_lstGuests = dtgGuests.SelectedItems.OfType<Guest>().ToList();
      lstGuestAdd = dtgGuests.SelectedItems.OfType<Guest>().ToList();
      StaEnd();
      //lstGuestAdd = _lstGuests;
      //dtgGuests.ItemsSource = _lstGuests;
      Close();
      cancel = false;
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

    #region dtpTo_SelectedDateChanged
    /// <history>
    /// [jorcanche] 04/05/2016 created
    /// </history>
    private void dtpTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      ValidateValueDate(sender);
    }
    #endregion

    #region dtpFrom_SelectedDateChanged
    /// <history>
    /// [jorcanche] 04/05/2016 created
    /// </history>
    private void dtpFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      ValidateValueDate(sender);
    }
    #endregion

    #region ValidateValueDate
    /// <summary>
    /// Valida que sea correcta la fecha proporcionada
    /// </summary>
    /// <param name="sender">Objeto de tipo DataPicker</param>
    /// <history>[jorcanche] 17/03/2016</history>
    public void ValidateValueDate(object sender)
    {
      //Obtener el valor actual del que tiene el dtpDate
      var picker = sender as DatePicker;
      if (!picker.SelectedDate.HasValue)
      {
        //Cuando el usuario ingresa una fecha invalida
        MessageBox.Show("Specify the Date", "date invalidates", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //Y le asignamos la fecha del servidor (la actual hora actual)
        picker.SelectedDate = BRHelpers.GetServerDate();
      }
    }
    #endregion

    #region ValidateDateRange
    /// <summary>
    /// Valida el valor de dtpStart sea mayo que dtpEnd
    /// </summary>
    /// <history>[jorcanche] 17/03/2016</history>
    public bool ValidateDateRange()
    {
      if (dtpFrom.SelectedDate.Value > dtpTo.SelectedDate.Value)
      {
        UIHelper.ShowMessage("Start Date can not be greater than End Date.", MessageBoxImage.Exclamation, "Date");
        DateTime dt = dtpTo.SelectedDate.Value.AddDays(-1);
        dtpFrom.SelectedDate = dt;
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

    #endregion


  }
}

///// <summary>
///// Llena datos para crear la clausula Where.
///// </summary>
///// <param name="where">Clausula WHERE si ya se tiene predefinida (Experimental)</param>
///// <history>[ECNUL] 31-03-2016 Created</history>
//void CreateWhere()
//{
//  _guest = new Guest();
//  _leadSource = new LeadSource();
//  _leadSource.lspg = EnumToListHelper.GetEnumDescription(_program);
//  if (txtGUID.Text != "") //Si se puso id del Huesped
//    _guest.guID = Convert.ToInt32(txtGUID.Text.Trim());
//  else
//  {
//    //Busca por nombre y apellido
//    if (txtName.Text != "")
//    {
//      _guest.guLastName1 = txtName.Text;
//      _guest.guFirstName1 = txtName.Text;
//      _guest.guLastname2 = txtName.Text;
//      _guest.guFirstName2 = txtName.Text;
//    }
//    else if (txtRoom.Text != "") //Numero de habitacion
//      _guest.guRoomNum = txtRoom.Text;
//    else if (txtReservation.Text != "") //Folio de reservacion
//      _guest.guHReservID = txtReservation.Text;
//    ///BUSQUEDAS WHERE OBLIGADAS
//    //Between de Fecha de llegada Si no se tiene GUID Siempre se busca por fecha de llegada
//    _guest.guCheckInD = Convert.ToDateTime(dtpFrom.SelectedDate.Value.ToShortDateString());
//    _guest.guCheckOutD = Convert.ToDateTime(dtpTo.SelectedDate.Value.ToShortDateString()); //Deberia ser CheckInD tambien pero se usa este para hacer el between
//    _guest.guls = cmbLeadSourse.SelectedValue.ToString(); //Toma el LeadSource del Combo Obligado siempre se busca por LS
//    _guest.gusr = (_program == EnumProgram.Outhouse) ? (cmbSalesRoom.SelectedIndex > -1)?cmbSalesRoom.SelectedValue.ToString(): _guest.gusr : _guest.gusr;
//    //Se usa si se tiene un Where Antes
//    //if (false)
//    //  guest.guID = guest.guID;
//  }
//}


///// <summary>
///// Carga El grid Con los paramntros de busqueda ya especificados
///// </summary>
//void CargaGrid()
//{
//  StaStart("Loading Guests...");
//  CreateWhere();
//  _lstGuests = BRGuests.GetSearchGuestByLS(_guest, _leadSource);
//  if (_lstGuests.Count != 0)
//    dtgGuests.ItemsSource = _lstGuests;

//  if (_lstGuests.Count == 1)
//    StatusBarReg.Content = _lstGuests.Count + " Guest";
//  else
//    StatusBarReg.Content = _lstGuests.Count + " Guests";

//  StaEnd();
//}
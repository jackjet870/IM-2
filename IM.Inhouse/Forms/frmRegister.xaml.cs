using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Inhouse.Classes;
using IM.Inhouse.Forms;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IM.Services.Helpers;
using IM.Services.WirePRService;

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmRegister.xaml//6515-8201 Eric Chihun 
  /// </summary>
  public partial class frmRegister : Window
  {
    #region Atributos

    private CollectionViewSource _guestPremanifestViewSource;
    private CollectionViewSource _guestArrivalViewSource;
    private CollectionViewSource _guestAvailableViewSource;
    private CollectionViewSource _guestSearchedViewSource;

    private DateTime _serverDate, _guestdateFrom, _guestDateTo;
    private int _available, _invited, _onGroup, _info, _guestGuid = 0;
    private string _markets = "ALL", _guestName, _guestRoom, _guestReservation;

    #endregion

    #region Constructores y destructores

    public frmRegister()
    {
      InitializeComponent();
    }

    #endregion

    #region Metodos

    #region EnabledCtrls

    /// <summary>
    /// Configura los controles para que esten habilitados o deshabilidatos cuando se presionan los TabsControl
    /// </summary>
    /// <param name="Av">gprAvailable Falso / Verdadero</param>
    /// <param name="Da">dtpDate  Falso / Verdadero></param>
    /// <param name="Inf">gprInfo Falso / Verdadero </param>
    /// <param name="Inv">gprInvited Falso/ Verdadero </param>
    /// <param name="Ong">gprOngroup Falso/ Verdadero</param>
    /// <param name="Mks">listMarkets Falso/ Verdadero</param>
    /// <history>
    /// [jorcanche] 23/02/2015 Created
    /// </history>
    /// <returns>Void</returns>
    private void EnabledCtrls(bool Av, bool Da, bool Inf, bool Inv, bool Ong = true, bool Mks = true)
    {
      gprAvailable.IsEnabled = Av;
      dtpDate.IsEnabled = Da;
      gprInfo.IsEnabled = Inf;
      gprInvited.IsEnabled = Inv;
      gprOngroup.IsEnabled = Ong;
      listMarkets.IsEnabled = Mks;
    }

    #endregion

    #region LoadGrid

    /// <summary>
    /// Metodo que sirve para carga los DataGrid's segun su estado de Visibilidad
    /// </summary>
    ///<history>[jorcanche] 15/03/2016 </history>
    private void LoadGrid()
    {
      if (_guestArrivalViewSource != null && _guestPremanifestViewSource != null && _guestAvailableViewSource != null)
      {
        //GuestArrival
        if (ccArrivals.Visibility.Equals(Visibility.Visible))
        {
          _guestArrivalViewSource.Source =
            (List<ObjGuestArrival>)BRGuests.GetGuestsArrivals(_serverDate, App.User.LeadSource.lsID, _markets, _available, _info, _invited, _onGroup)
            .Select(parent => new ObjGuestArrival(parent)).ToList();
        }
        //GuestAvailable
        if (ccAvailables.Visibility.Equals(Visibility.Visible))
        {
          //_guestAvailableViewSource.Source = BRGuests.GetGuestsAvailables(BRHelpers.GetServerDate().Date, App.User.LeadSource.lsID, _markets, _info, _invited, _onGroup);
          _guestAvailableViewSource.Source =
           (List<ObjGuestAvailable>)BRGuests.GetGuestsAvailables(BRHelpers.GetServerDate().Date, App.User.LeadSource.lsID, _markets, _info, _invited, _onGroup)
           .Select(parent => new ObjGuestAvailable(parent)).ToList();
        }
        //GuestPremanifest
        if (ccPremanifest.Visibility.Equals(Visibility.Visible))
        {
          _guestPremanifestViewSource.Source =
           (List<ObjGuestPremanifest>)BRGuests.GetGuestsPremanifest(_serverDate, App.User.LeadSource.lsID, _markets, _onGroup)
            .Select(parent => new ObjGuestPremanifest(parent)).ToList();
        }
        //GuestSearched
        if (ccGetGuest.Visibility.Equals(Visibility.Visible))
        {

          _guestSearchedViewSource.Source =
            (List<ObjGuestSearched>)BRGuests.GetGuests(_guestdateFrom, _guestDateTo, App.User.LeadSource.lsID, _guestName, _guestRoom, _guestReservation, _guestGuid)
            .Select(parent => new ObjGuestSearched(parent)).ToList();
        }
      }
    }
    #endregion 

    #region DataGridVisibility.
    /// <summary>
    /// Configur la visibilidad de los cuatro Datagrids 
    /// </summary>
    /// <param name="ccArrivals"></param>
    /// <param name="ccAvailables"></param>
    /// <param name="ccPremanifest"></param>
    /// <param name="ccGetGuest"></param>
    /// <history>[jorcanche] 17/03/2016</history>
    public void DataGridVisibility(Visibility ccArrivals, Visibility ccAvailables, Visibility ccPremanifest, Visibility ccGetGuest)
    {
      this.ccArrivals.Visibility = ccArrivals;
      this.ccPremanifest.Visibility = ccPremanifest;
      this.ccAvailables.Visibility = ccAvailables;
      this.ccGetGuest.Visibility = ccGetGuest;
    }
    #endregion

    #region ValidateCheckIn
    /// <summary>
    ///  Valida los datos para poder hacer Check In
    /// </summary>
    /// <param name="CheckIn">Si ya se hizo CheckIn</param>
    /// <param name="pguCheckInD"></param>
    /// <param name="pguCheckOutD"></param>
    /// <history>[jorcanche] 17/03/2016</history>
    private bool ValidateCheckIn(bool CheckIn, DateTime pguCheckInD, DateTime pguCheckOutD)
    {
      // impedimos modificar los datos si el sistema esta en modo de solo lectura
      //si tiene permiso estandar
      if (App.User.HasPermission(Model.Enums.EnumPermission.Register, EnumPermisionLevel.Standard))
      {
        //si no tiene 
        if (CheckIn)
        {
          //validamos que el huesped este en casa
          if (App.User.HasPermission(EnumPermission.Register, EnumPermisionLevel.Standard) && pguCheckInD > BRHelpers.GetServerDate()
             || pguCheckOutD < BRHelpers.GetServerDate())
          {
            UIHelper.ShowMessage("Guest is not in house.", MessageBoxImage.Asterisk, "Permissions");
            return false;
          }
          else
          {
            //validamos que el huesped este en casa con 2 dias de tolerancia
            if (App.User.HasPermission(EnumPermission.Register, EnumPermisionLevel.Special) && pguCheckInD > BRHelpers.GetServerDate().Date.AddDays(+2)
              || pguCheckOutD < BRHelpers.GetServerDate().Date.AddDays(-2))
            {
              UIHelper.ShowMessage("Guest is not in house.", MessageBoxImage.Asterisk, "Permissions");
              return false;
            }
            else
            {
              //Preguntamos al usuario si en verdan desea darle Check In al Huesped
              MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to mark this record as Check-in? /n This change can not be undone.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
              if (result == MessageBoxResult.No)
              {
                return false;
              }
            }
          }
        }
      }
      return true;
    }
    #endregion

    #region ValidateAvailability
    /// <summary>
    /// Valida los parametros para que pueda abrir el formulario de Availability
    /// </summary>
    /// <param name="CheckIn"></param>
    /// <history>[jorcanche] 16/03/2016</history>
    private bool ValidateAvailability(bool CheckIn)
    {
      //Validamos que el huesped haya hecho Check In
      if (!CheckIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-in.", MessageBoxImage.Asterisk);
        return false;
      }
      else
      {
        if (!App.User.HasPermission(Model.Enums.EnumPermission.Available, EnumPermisionLevel.ReadOnly))
        {
          UIHelper.ShowMessage("Access denied.", MessageBoxImage.Asterisk);
          return false;
        }
      }
      return true;
    }

    #endregion

    #region ValidateContact
    /// <summary>
    /// Valida los datos para desplegar el formulario de contactacion
    /// </summary>
    /// <param name="CheckIn"></param>
    /// <param name="Contact"></param>
    /// <param name="CheckOutD"></param>
    /// <returns></returns>
    ///<history>[jorcanche] 13/03/2016</history>
    private bool ValidateContact(bool CheckIn, bool Contact, DateTime CheckOutD)
    {
      //validamos que el huesped haya hecho Check In
      if (!CheckIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-in.", MessageBoxImage.Asterisk);
        return false;
      }
      // no se permite contactar si ya hizo Check Out o si ya esta contactado el Guest
      if (!Contact && CheckOutD < BRHelpers.GetServerDate().Date)
      {
        UIHelper.ShowMessage("Guest already made Check-out.", MessageBoxImage.Asterisk);
        return false;
      }
      //validamos que el usuario tenga permiso de lectura
      if (!App.User.HasPermission(EnumPermission.Register, EnumPermisionLevel.ReadOnly))
      {
        UIHelper.ShowMessage("Access denied.", MessageBoxImage.Asterisk);
        return false;
      }
      return true;
    }
    #endregion

    #region ValidateFollowUp
    /// <summary>
    /// Valida todas los parametros para abrir el formulario de FolloUp
    /// </summary>
    /// <param name="checkIn"></param>
    /// <param name="followUp"></param>
    /// <param name="avail"></param>
    /// <param name="contact"></param>
    /// <param name="invit"></param>
    /// <param name="checkOutD"></param>
    /// <history>[jorcanche] 12/03/2016</history>
    /// <returns></returns>
    private bool ValidateFollowUp(bool checkIn, bool followUp, bool avail, bool contact, bool invit, DateTime checkOutD)
    {
      //Validamos que el huesped haya hecho Check In
      if (!checkIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-in.", MessageBoxImage.Asterisk);
        return false;
      }
      //validamos que el huesped no haya hecho Check Out
      if (!followUp && checkOutD < BRHelpers.GetServerDate().Date)
      {
        UIHelper.ShowMessage("Guest already made Check-out.", MessageBoxImage.Asterisk);
        return false;
      }
      //Validamos que el huesped este disponible
      if (!followUp && !avail)
      {
        UIHelper.ShowMessage("Guest is not available.", MessageBoxImage.Asterisk);
        return false;
      }
      //Validamos que el huesped este contactado
      if (!followUp && !contact)
      {
        UIHelper.ShowMessage("Guest is not contacted.",  MessageBoxImage.Asterisk);
        return false;
      }
      //Validamos que el Huesped no este invitado
      if (!followUp && invit)
      {
        UIHelper.ShowMessage("Guest is invited.", MessageBoxImage.Asterisk);
        return false;
      }
      //Validamos que el usuario tenga permisos de lectura
      if (!App.User.HasPermission(EnumPermission.Register, EnumPermisionLevel.ReadOnly))
      {
        UIHelper.ShowMessage("Access denied.", MessageBoxImage.Asterisk);
        return false;
      }
      return true;
    }
    #endregion

    #region CheckIn
    /// <summary>
    /// Valida el tipo de guest y determina si el husped debe estar como disponible
    /// </summary>
    /// <history>[jorcanche] 15/03/2016 </history>
    /// <param name="guest"></param>
    /// <param name="typeGuest"></param>
    public bool CheckIn(object guest, int typeGuest)
    {
      switch (typeGuest)
      {
        case 1:
          //Determinamos el caso 
          var itemGuestArrival = guest as GuestArrival;
          //Validamos
          if (ValidateCheckIn(itemGuestArrival.guCheckIn, itemGuestArrival.guCheckInD, itemGuestArrival.guCheckOutD))
          {
            //determinamos si el huesped debe estar como disponible
            if (itemGuestArrival.guum == 0)
            {
              dgGuestArrival.SelectedItems.OfType<GuestArrival>().ToList().ForEach(item => { item.guAvail = true; });
              dgGuestArrival.Items.Refresh();
            }
            else
            {
              SaveAvailGuest(itemGuestArrival.guID);
            }
            //Si no hubo problema en las validaciones mandamos el valor que obtuvo al hacer click en el checkbox          
            return itemGuestArrival.guCheckIn;
          }
          else
          {
            //despalomeamos el checkbox porque no se pude hacer el checkin
            return false;
          }
       
        case 3:
          var itemGuestPremanifest = guest as GuestPremanifest;
          if (ValidateCheckIn(itemGuestPremanifest.guCheckIn, itemGuestPremanifest.guCheckInD, itemGuestPremanifest.guCheckOutD))
          {
            //determinamos si el huesped debe estar como disponible   
            if (BRGuests.GetGuest(itemGuestPremanifest.guID).guum.Equals(0))
            {
              dgGuestPremanifest.SelectedItems.OfType<GuestPremanifest>().ToList().ForEach(item => item.guAvail = true);
              dgGuestPremanifest.Items.Refresh();
            }
            else
            {
              SaveAvailGuest(itemGuestPremanifest.guID);
            }
            //Si no hubo problema en las validaciones mandamos el valor que obtuvo al hacer click en el checkbox          
            return itemGuestPremanifest.guCheckIn;
          }
          else
          {
            //despalomeamos el checkbox porque no se pude hacer el checkin
            return false;
          }
        default:
          var itemGuestSearched = guest as GuestSearched;
          if (ValidateCheckIn(itemGuestSearched.guCheckIn, itemGuestSearched.guCheckInD, itemGuestSearched.guCheckOutD))
          {
            //determinamos si el huesped debe estar como disponible
            if (itemGuestSearched.guum == 0)
            {
              guestSearchedDataGrid.SelectedItems.OfType<GuestSearched>().ToList().ForEach(item => item.guAvail = true);
              guestSearchedDataGrid.Items.Refresh();
            }
            else
            {
              SaveAvailGuest(itemGuestSearched.guID);
            }
            //Si no hubo problema en las validaciones mandamos el valor que obtuvo al hacer click en el checkbox          
            return itemGuestSearched.guCheckIn;
          }
          else
          {
            //despalomeamos el checkbox porque no se pude hacer el checkin
            return false;
          }
      }
    }
    #endregion

    #region SaveAvailGuest
    /// <summary>
    /// Guarda en la base el CheckIn y el Avail
    /// </summary>
    /// <history>[jorcanche] 10/03/2016</history>
    /// <param name="GUID">Id del Guest</param>
    public void SaveAvailGuest(int GUID)
    {
      Guest guest = BRGuests.GetGuest(GUID);
      guest.guCheckIn = true;
      guest.guAvail = true;
      BRGuests.SaveGuest(guest);
      LoadGrid();
    }
    #endregion

    #region ValueRevert
    /// <summary>
    /// Invierte el valor del Check, 
    /// </summary>
    /// <param name="Check"></param>
    /// <history>[jorcanche] 20/03/2016 </history>
    /// <returns>bool</returns>
    public bool ValueRevert(bool Check)
    {
      if (Check)
      {
        return false;
      }
      else
      {
        return true;
      }
    }
    #endregion

    #region ValidateCancelInvitation
    //Valida los datos para cancelar una invitacion
    public bool ValidateCancelInvitation(bool guCheckIn, DateTime guCheckOutD, bool guInvit, bool guShow)
    {
      //Validamos que el huesped haya hecho CheckIn
      if (!guCheckIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-in.", MessageBoxImage.Asterisk);
        return false;
      }
      //Validamos que el huesped no haya hehco Check Out
      if (guCheckOutD < BRHelpers.GetServerDate().Date)
      {
        UIHelper.ShowMessage("Guest already made Check-out.", MessageBoxImage.Asterisk);
        return false;
      }
      //Validamos que el huesped haya sido invitado
      if (!guInvit)
      {
        UIHelper.ShowMessage("Guest has not been invited.", MessageBoxImage.Asterisk);
        return false;
      }
      //validamos que el huesped no tenga show
      if (guShow)
      {
        UIHelper.ShowMessage("Guest already attended to the show.", MessageBoxImage.Asterisk);
        return false;
      }

      return true;
    }
    #endregion

    #region ValidateLogin
    public frmLogin ValidateLogin()
    {
      //Validamos las credenciales del usuario y sus permisos 
      frmLogin log = new frmLogin(null, false, EnumLoginType.Normal, false);
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
        if (log.userData.HasPermission(EnumPermission.Register, EnumPermisionLevel.Standard))
        {
          return log;
        }
        else
        {
          UIHelper.ShowMessage("You do not have the required permissions to perform this operation.", MessageBoxImage.Asterisk, "Permissions");
          return null;
        }
      }
      else
      {
        return null;
      }
    }

    #endregion

    #endregion

    #region Eventos del formulario

    #region Window_Loaded
    /// <summary>
    /// Carga los controles que necesitamos 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      txtUser.Text = App.User.User.peN;
      txtLocation.Text = App.User.Location.loN;
      dtpDate.SelectedDate = BRHelpers.GetServerDate().Date;

      _guestArrivalViewSource = ((CollectionViewSource)(this.FindResource("guestArrivalViewSource")));
      _guestAvailableViewSource = ((CollectionViewSource)(this.FindResource("guestAvailableViewSource")));
      _guestPremanifestViewSource = ((CollectionViewSource)(this.FindResource("guestPremanifestViewSource")));
      _guestSearchedViewSource = ((CollectionViewSource)(this.FindResource("guestSearchedViewSource")));
      LoadGrid();
      listMarkets.ItemsSource = BRMarkets.GetMarkets(1);

    }

    #endregion

    #region listMarkets_SelectionChanged

    /// <summary>
    /// ocurre el evento cuando se selecciona uno o mas mercados, los enlista en una cadena separados por comas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history> [jorcanche] 09/03/2016 </history>
    private void listMarkets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      int cont = 0; _markets = string.Empty;
      var selectedItems = listMarkets.SelectedItems;
      foreach (MarketShort selectedItem in selectedItems)
      {
        cont = cont + 1;
        _markets += selectedItem.mkID.ToString();
        if (selectedItems.Count > 1 && cont < selectedItems.Count)
        {
          _markets = _markets + ",";
        }
      }
      LoadGrid();
    }

    #endregion

    #region dtpDate_SelectedDateChanged

    private void dtpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      //Obtener el valor actual del que tiene el dtpDate
      var picker = sender as DatePicker;
      if (!picker.SelectedDate.HasValue)
      {
        //Cuando el usuario ingresa una fecha invalida
        MessageBox.Show("Specify the Date", "date invalidates", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //Y le asignamos la fecha del servidor (la actual hora actual)
        dtpDate.SelectedDate = BRHelpers.GetServerDate();
      }
      else
      {
        //le asignamos el valor del dtpDate a la variable global para que otro control tenga acceso al valor actual del dtpDate
        _serverDate = picker.SelectedDate.Value;
        //Cargamos el grid del tab que esta seleccionado
        LoadGrid();
        //gprInfo.BindingGroup.GetValue                 
      }
    }
    #endregion   

    #region rb_Checked

    /// <summary>
    /// Evento que ocurre cuando se cambia los filtros.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>[jorcanche] 25/02/2016</history>
    private void rb_Checked(object sender, RoutedEventArgs e)
    {
      var ck = sender as RadioButton;
      switch (ck.Name)
      {
        case "rbYesAvailable":
          _available = 1;
          break;
        case "rbNoAvailable":
          _available = 0;
          break;
        case "rbBothAvailable":
          _available = 2;
          break;
        case "rbYesInvited":
          _invited = 1;
          break;
        case "rbNoInvited":
          _invited = 0;
          break;
        case "rbBothInvited":
          _invited = 2;
          break;
        case "rbYesOnGroup":
          _onGroup = 1;
          break;
        case "rbNoOnGroup":
          _onGroup = 0;
          break;
        case "rbBothOnGroup":
          _onGroup = 2;
          break;
        case "rbYesInfo":
          _info = 1;
          break;
        case "rbNoInfo":
          _info = 0;
          break;
        case "rbBothInfo":
          _info = 2;
          break;
      }
      LoadGrid();
    }
    #endregion

    #region 1.- Arrival

    #region ChkguCheckInArrival_Click
    /// <summary>
    /// Evento que ocurre cuando se selecciona alguna fila en la columna CheckIn del datagrid Arrival
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>[jorcanche] 09/01/2015</history>
    private void ChkguCheckInArrival_Click(object sender, RoutedEventArgs e)
    {
      //Se debe igualar el valor del check al valor que arroje las validaciones
      var chk = sender as CheckBox;
      if (chk.IsChecked.Value)
      {
        chk.IsChecked = CheckIn((dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem))), 1);
      }
    }
    #endregion  

    #region ChkguAvailArrival_Click
    /// <summary>
    /// Evento que ocurre cuando se selecciona alguna fila en la columna Avail del datagrid Arrival
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>[jorcanche] 09/01/2015</history></historyZ>
    private void ChkguAvailArrival_Click(object sender, RoutedEventArgs e)
    {
      var Arrival = dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;
      var chkguAvail = sender as CheckBox;
      chkguAvail.IsChecked = ValueRevert(chkguAvail.IsChecked.Value);
      if (ValidateAvailability(Arrival.guCheckIn))
      {
        frmAvailability frmAvail = new frmAvailability(Arrival.guID);
        frmAvail.Owner = this;
        frmAvail.ShowInTaskbar = false;
        if (!frmAvail.ShowDialog().Value)
        {
          if (frmAvail._wasSaved)
          {
            //Descripcion: Actualiza los datos del grid despues de guardar la informacion de disponibilidad
            //PR de Disponibilidad y si se marco como no disponible 
            dgGuestArrival.SelectedItems.OfType<GuestArrival>().ToList().ForEach(item => { item.guPRAvail = frmAvail.guPRAvail; item.guum = frmAvail.guum; item.guAvail = frmAvail.Avail; });
            dgGuestArrival.Items.Refresh();
            //LoadGrid();
          }
        }
      }
    }
    #endregion   

    #region ChkguInfoArrival_Click
    /// <summary>
    /// Evento que ocurre cuando se selecciona alguna fila en la columna Info del datagrid Arrival
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>[jorcanche] 09/02/2015</history>
    private void ChkguInfoArrival_Click(object sender, RoutedEventArgs e)
    {
      var Arrival = dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;
      var chkguInfo = sender as CheckBox;
      chkguInfo.IsChecked = ValueRevert(chkguInfo.IsChecked.Value);
      if (ValidateContact(Arrival.guCheckIn, Arrival.guInfo, Arrival.guCheckOutD))
      {
        frmContact frmCont = new frmContact(Arrival.guID);
        frmCont.Owner = this;
        frmCont.ShowInTaskbar = false;
        if (!frmCont.ShowDialog().Value)
        {
          if (frmCont._wasSave)
          {
            dgGuestArrival.SelectedItems.OfType<GuestArrival>().ToList().ForEach(item => { item.guPRInfo = frmCont.PRInfo; item.guInfoD = frmCont.InfoD; item.guCheckIn = true; item.guInfo = true; });
            dgGuestArrival.Items.Refresh();
            //LoadGrid();
          }
        }
      }
    }

    #endregion

    #region ChkFollowArrival_Click
    /// <summary>
    /// Evento que ocurre cuando se selecciona alguna fila en la columna FollowUp del datagrid Arrival
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <hitory>[jorcanche] 09/03/2015</hitory>
    private void ChkFollowArrival_Click(object sender, RoutedEventArgs e)
    {
      var Arrival = dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;
      var chkkFollow = sender as CheckBox;
      chkkFollow.IsChecked = ValueRevert(chkkFollow.IsChecked.Value);
      if (ValidateFollowUp(Arrival.guCheckIn, Arrival.guFollow, Arrival.guAvail, Arrival.guInfo, Arrival.guInvit, Arrival.guCheckOutD))
      {
        frmFollowUp frmFoll = new frmFollowUp(Arrival.guID);
        frmFoll.Owner = this;
        frmFoll.ShowInTaskbar = false;
        if (!frmFoll.ShowDialog().Value)
        {
          if (frmFoll._wasSaved)
          {
            dgGuestArrival.SelectedItems.OfType<GuestArrival>().ToList().ForEach(item =>
            {
              item.guFollowD = frmFoll.FollowD;
              item.guPRFollow = frmFoll.PRFollow;
              item.guFollow = true;
              item.guAvail = true;
              item.guInfo = true;
            });
            dgGuestArrival.Items.Refresh();
            //LoadGrid();
          }
        }
      }
    }
    #endregion

    #region NotesArrival_MouseLeftButtonDown
    private void NotesArrival_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (dgGuestArrival.SelectedItem != null)
      {
        var Arrival = dgGuestArrival.Items[dgGuestArrival.Items.CurrentPosition] as GuestArrival;
        frmPRNotes prnote = new frmPRNotes(Arrival.guID);
        prnote.Owner = this;
        prnote.ShowInTaskbar = false;
        if (!prnote.ShowDialog().Value)
        {
          if (prnote._saveNote)
          {
            dgGuestArrival.SelectedItems.OfType<ObjGuestArrival>().ToList().ForEach(item => item.guPRNote = true);
            dgGuestArrival.Items.Refresh();
          }
        }
      }
    }
    #endregion

    #region ChkBookCancArrival_Click
    private void ChkBookCancArrival_Click(object sender, RoutedEventArgs e)
    {
      var Arrival = dgGuestArrival.Items[dgGuestArrival.Items.CurrentPosition] as GuestArrival;
      var chk = sender as CheckBox;
      chk.IsChecked = chk.IsChecked.Value ? false : true; 
      if (ValidateCancelInvitation(Arrival.guCheckIn, Arrival.guCheckOutD, Arrival.guInvit, Arrival.guShow))
      {
        var log = ValidateLogin();
        if (log != null)
        {
          frmBookingCancel bc = new frmBookingCancel(Arrival.guID, log.userData.User);
          bc.Owner = this;
          bc.ShowInTaskbar = false;
          if (!bc.ShowDialog().Value)
          {
            dgGuestArrival.SelectedItems.OfType<GuestArrival>().ToList().ForEach(item => item.guBookCanc = bc._cancelado.Value);
            dgGuestArrival.Items.Refresh();
          }
        }
      }
    }
    #endregion

    #region ReservationArrival_MouseLeftButtonUp
    private void ReservationArrival_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var arrival = dgGuestArrival.Items[dgGuestArrival.Items.CurrentPosition] as GuestArrival;
      if (!string.IsNullOrEmpty(arrival.gulsOriginal) && !string.IsNullOrEmpty(arrival.guHReservID))
      {
       RptReservationOrigos reservation = 
         WirePRHelper.GetRptReservationOrigos(arrival.gulsOriginal, arrival.guHReservID);
      }
    } 
    #endregion

    #endregion

    #region 2.- Availables

    #region ChkguCheckInAvailables_Click
    private void ChkguCheckInAvailables_Click(object sender, RoutedEventArgs e)
    {
      //No contiene la columna ChekIn cuando es Available
    }
    #endregion

    #region ChkguAvailAvailable_Click
    private void ChkguAvailAvailable_Click(object sender, RoutedEventArgs e)
    {
      var Avilable = dgGuestAvailable.Items.GetItemAt(dgGuestAvailable.Items.IndexOf(dgGuestAvailable.CurrentItem)) as GuestAvailable;
      var chkguAvail = sender as CheckBox;
      chkguAvail.IsChecked = ValueRevert(chkguAvail.IsChecked.Value);
      if (ValidateAvailability(Avilable.guCheckIn))
      {
        frmAvailability frmAvail = new frmAvailability(Avilable.guID);
        frmAvail.Owner = this;
        frmAvail.ShowInTaskbar = false;
        if (!frmAvail.ShowDialog().Value)
        {
          if (frmAvail._wasSaved)
          {
            //Descripcion: Actualiza los datos del grid despues de guardar la informacion de disponibilidad
            //PR de Disponibilidad y si se marco como no disponible 
            dgGuestAvailable.SelectedItems.OfType<GuestAvailable>().ToList().ForEach(item => { item.guPRAvail = frmAvail.guPRAvail; item.guum = frmAvail.guum; item.guAvail = frmAvail.Avail; });
            dgGuestAvailable.Items.Refresh();
            //LoadGrid();
          }
        }
      }
    }

    #endregion

    #region ChkguInfoAvailable_Click
    private void ChkguInfoAvailable_Click(object sender, RoutedEventArgs e)
    {
      var Available = dgGuestAvailable.Items.GetItemAt(dgGuestAvailable.Items.IndexOf(dgGuestAvailable.CurrentItem)) as GuestAvailable;
      var chkguInfo = sender as CheckBox;
      chkguInfo.IsChecked = ValueRevert(chkguInfo.IsChecked.Value);
      if (ValidateContact(Available.guCheckIn, Available.guInfo, Available.guCheckOutD))
      {
        frmContact frmCont = new frmContact(Available.guID);
        frmCont.Owner = this;
        frmCont.ShowInTaskbar = false;
        if (!frmCont.ShowDialog().Value)
        {
          if (frmCont._wasSave)
          {
            dgGuestAvailable.SelectedItems.OfType<GuestAvailable>().ToList().ForEach(item =>
            { item.guPRInfo = frmCont.PRInfo; item.guInfoD = frmCont.InfoD; item.guCheckIn = true; item.guInfo = true; });
            dgGuestAvailable.Items.Refresh();
            //LoadGrid();
          }
        }
      }
    }

    #endregion

    #region ChkFollowAvailable_Click
    private void ChkFollowAvailable_Click(object sender, RoutedEventArgs e)
    {
      var Available = dgGuestAvailable.Items.GetItemAt(dgGuestAvailable.Items.IndexOf(dgGuestAvailable.CurrentItem)) as GuestAvailable;
      var chkkFollow = sender as CheckBox;
      chkkFollow.IsChecked = ValueRevert(chkkFollow.IsChecked.Value);
      if (ValidateFollowUp(Available.guCheckIn, Available.guFollow, Available.guAvail, Available.guInfo, Available.guInvit, Available.guCheckOutD))
      {
        frmFollowUp frmFoll = new frmFollowUp(Available.guID);
        frmFoll.Owner = this;
        frmFoll.ShowInTaskbar = false;
        if (!frmFoll.ShowDialog().Value)
        {
          if (frmFoll._wasSaved)
          {
            dgGuestAvailable.SelectedItems.OfType<GuestAvailable>().ToList().ForEach(item =>
            {
              item.guFollowD = frmFoll.FollowD;
              item.guPRFollow = frmFoll.PRFollow;
              item.guFollow = true;
              item.guAvail = true;
              item.guInfo = true;
            });
            dgGuestAvailable.Items.Refresh();
            //LoadGrid();
          }
        }
      }
    }
    #endregion

    #region NotesAvailable_MouseLeftButtonUp
    private void NotesAvailable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (dgGuestAvailable.SelectedItem != null)
      {
        var Arrival = dgGuestAvailable.SelectedItem as GuestAvailable;
        frmPRNotes prnote = new frmPRNotes(Arrival.guID);
        prnote.Owner = this;
        prnote.ShowInTaskbar = false;
        if (!prnote.ShowDialog().Value)
        {
          if (prnote._saveNote)
          {
            dgGuestAvailable.SelectedItems.OfType<GuestAvailable>().ToList().ForEach(item => item.guPRNote = true);
            dgGuestAvailable.Items.Refresh();
          }
        }
      }
    }

    #endregion

    #region ChkBookCancAvailable_Click
    private void ChkBookCancAvailable_Click(object sender, RoutedEventArgs e)
    {
      var Available = dgGuestAvailable.Items[dgGuestAvailable.Items.CurrentPosition] as GuestAvailable;
      var chk = sender as CheckBox;
      chk.IsChecked = ValueRevert(chk.IsChecked.Value);
      if (ValidateCancelInvitation(Available.guCheckIn, Available.guCheckOutD, Available.guInvit, Available.guShow))
      {
        var log = ValidateLogin();
        if (log != null)
        {
          frmBookingCancel bc = new frmBookingCancel(Available.guID, log.userData.User);
          bc.Owner = this;
          bc.ShowInTaskbar = false;
          if (!bc.ShowDialog().Value)
          {
            dgGuestAvailable.SelectedItems.OfType<GuestAvailable>().ToList().ForEach(item => item.guBookCanc = bc._cancelado.Value);
            dgGuestAvailable.Items.Refresh();
          }
        }
      }
    }
    #endregion

    #endregion

    #region 3.- Premanifest

    #region ChkguCheckInPremanifest_Click
    private void ChkguCheckInPremanifest_Click(object sender, RoutedEventArgs e)
    {
      //Se debe igualar el valor del check al valor que arroje las validaciones
      var chk = sender as CheckBox;
      if (chk.IsChecked.Value)
      {
        chk.IsChecked = CheckIn(dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.IndexOf(dgGuestPremanifest.CurrentItem)), 3);
      }
    }

    #endregion
   
    #region ChkguAvailPremanifest_Click
    private void ChkguAvailPremanifest_Click(object sender, RoutedEventArgs e)
    {
      var Premanifest = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.IndexOf(dgGuestPremanifest.CurrentItem)) as GuestPremanifest;
      var chkguAvail = sender as CheckBox;
      chkguAvail.IsChecked = ValueRevert(chkguAvail.IsChecked.Value);
      if (ValidateAvailability(Premanifest.guCheckIn))
      {
        frmAvailability frmAvail = new frmAvailability(Premanifest.guID);
        frmAvail.Owner = this;
        frmAvail.ShowInTaskbar = false;
        if (!frmAvail.ShowDialog().Value)
        {
          if (frmAvail._wasSaved)
          {
            //Descripcion: Actualiza los datos del grid despues de guardar la informacion de disponibilidad
            //PR de Disponibilidad y si se marco como no disponible 
            dgGuestPremanifest.SelectedItems.OfType<GuestPremanifest>().ToList().ForEach(item => { item.guAvail = frmAvail.Avail; });
            dgGuestPremanifest.Items.Refresh();
            //LoadGrid();
          }
        }
      }
    }
    #endregion

    #region ChkguInfoPremanifest_Click
    private void ChkguInfoPremanifest_Click(object sender, RoutedEventArgs e)
    {
      var Premanifest = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.IndexOf(dgGuestPremanifest.CurrentItem)) as GuestPremanifest;
      var chkguInfo = sender as CheckBox;
      chkguInfo.IsChecked = ValueRevert(chkguInfo.IsChecked.Value);
      if (ValidateContact(Premanifest.guCheckIn, Premanifest.guInfo, Premanifest.guCheckOutD))
      {
        frmContact frmCont = new frmContact(Premanifest.guID);
        frmCont.Owner = this;
        frmCont.ShowInTaskbar = false;
        if (!frmCont.ShowDialog().Value)
        {
          if (frmCont._wasSave)
          {
            dgGuestPremanifest.SelectedItems.OfType<GuestPremanifest>().ToList().ForEach(item => { item.guPRInfo = frmCont.PRInfo; item.guCheckIn = true; item.guInfo = true; });
            dgGuestPremanifest.Items.Refresh();
            //LoadGrid();
          }
        }
      }
    }
    #endregion

    #region ChkguFollowPremanifest_Click
    private void ChkguFollowPremanifest_Click(object sender, RoutedEventArgs e)
    {
      var Premanifest = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.IndexOf(dgGuestPremanifest.CurrentItem)) as GuestPremanifest;
      var chkkFollow = sender as CheckBox;
      chkkFollow.IsChecked = ValueRevert(chkkFollow.IsChecked.Value);
      if (ValidateFollowUp(Premanifest.guCheckIn, Premanifest.guFollow, Premanifest.guAvail, Premanifest.guInfo, Premanifest.guInvit, Premanifest.guCheckOutD))
      {
        frmFollowUp frmFoll = new frmFollowUp(Premanifest.guID);
        frmFoll.Owner = this;
        frmFoll.ShowInTaskbar = false;
        if (!frmFoll.ShowDialog().Value)
        {
          if (frmFoll._wasSaved)
          {
            dgGuestPremanifest.SelectedItems.OfType<GuestPremanifest>().ToList().ForEach(item =>
            {
              item.guFollowD = frmFoll.FollowD;
              item.guPRFollow = frmFoll.PRFollow;
              item.guFollow = true;
              item.guAvail = true;
              item.guInfo = true;
            });
            dgGuestPremanifest.Items.Refresh();
            //LoadGrid();
          }
        }
      }
    }
    #endregion

    #region NotesPremanifest_MouseLeftButtonUp
   
   
    private void NotesPremanifest_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (dgGuestPremanifest.SelectedItem != null)
      {
        var Arrival = dgGuestPremanifest.SelectedItem as GuestPremanifest;
        frmPRNotes prnote = new frmPRNotes(Arrival.guID);
        prnote.Owner = this;
        prnote.ShowInTaskbar = false;
        if (!prnote.ShowDialog().Value)
        {
          if (prnote._saveNote)
          {
            dgGuestPremanifest.SelectedItems.OfType<ObjGuestPremanifest>().ToList().ForEach(item => item.guPRNote = true);
            dgGuestPremanifest.Items.Refresh();
          }
        }
      }
    }
    #endregion

    #region ChkBookCancPremanifest_Click
    private void ChkBookCancPremanifest_Click(object sender, RoutedEventArgs e)
    {
      var Premanifest = dgGuestPremanifest.Items[dgGuestPremanifest.Items.CurrentPosition] as GuestPremanifest;
      var chk = sender as CheckBox;
      chk.IsChecked = ValueRevert(chk.IsChecked.Value);
      if (ValidateCancelInvitation(Premanifest.guCheckIn, Premanifest.guCheckOutD, Premanifest.guInvit, Premanifest.guShow))
      {
        var log = ValidateLogin();
        if (log != null)
        {
          frmBookingCancel bc = new frmBookingCancel(Premanifest.guID, log.userData.User);
          bc.Owner = this;
          bc.ShowInTaskbar = false;
          if (!bc.ShowDialog().Value)
          {
            dgGuestPremanifest.SelectedItems.OfType<GuestPremanifest>().ToList().ForEach(item => item.guBookCanc = bc._cancelado.Value);
            dgGuestPremanifest.Items.Refresh();
          }
        }
      }
    }
    #endregion

    #endregion

    #region 4.- GetGuest

    #region ChkguCheckInGetGuest_Click
    private void ChkguCheckInGetGuest_Click(object sender, RoutedEventArgs e)
    {
      //Se debe igualar el valor del check al valor que arroje las validaciones
      var chk = sender as CheckBox;
      if (chk.IsChecked.Value)
      {
        chk.IsChecked = CheckIn(dgGuestAvailable.Items[dgGuestAvailable.Items.CurrentPosition], 4);
      }

    }
    #endregion

    #region ChkguAvailGetGuest_Click
    private void ChkguAvailGetGuest_Click(object sender, RoutedEventArgs e)
    {
      var Searched = guestSearchedDataGrid.Items.GetItemAt(guestSearchedDataGrid.Items.IndexOf(guestSearchedDataGrid.CurrentItem)) as GuestSearched;
      var chkguAvail = sender as CheckBox;
      chkguAvail.IsChecked = ValueRevert(chkguAvail.IsChecked.Value);
      if (ValidateAvailability(Searched.guCheckIn))
      {
        frmAvailability frmAvail = new frmAvailability(Searched.guID);
        frmAvail.Owner = this;
        frmAvail.ShowInTaskbar = false;
        if (!frmAvail.ShowDialog().Value)
        {
          if (frmAvail._wasSaved)
          {
            //Descripcion: Actualiza los datos del grid despues de guardar la informacion de disponibilidad
            //PR de Disponibilidad y si se marco como no disponible 
            guestSearchedDataGrid.SelectedItems.OfType<GuestSearched>().ToList().ForEach(item => { item.guPRAvail = frmAvail.guPRAvail; item.guum = frmAvail.guum; item.guAvail = frmAvail.Avail; });
            guestSearchedDataGrid.Items.Refresh();
            //LoadGrid();
          }
        }
      }
    }
    #endregion

    #region ChkguInfoGetGuest_Click
    private void ChkguInfoGetGuest_Click(object sender, RoutedEventArgs e)
    {
      var Searched =guestSearchedDataGrid.Items.GetItemAt(guestSearchedDataGrid.Items.IndexOf(guestSearchedDataGrid.CurrentItem)) as GuestSearched;
      var chkguInfo = sender as CheckBox;
      chkguInfo.IsChecked = ValueRevert(chkguInfo.IsChecked.Value);
      if (ValidateContact(Searched.guCheckIn, Searched.guInfo, Searched.guCheckOutD))
      {
        frmContact frmCont = new frmContact(Searched.guID);
        frmCont.Owner = this;
        frmCont.ShowInTaskbar = false;
        if (!frmCont.ShowDialog().Value)
        {
          if (frmCont._wasSave)
          {
            guestSearchedDataGrid.SelectedItems.OfType<GuestArrival>().ToList().ForEach(item => { item.guPRInfo = frmCont.PRInfo; item.guInfoD = frmCont.InfoD; item.guCheckIn = true; item.guInfo = true; });
            guestSearchedDataGrid.Items.Refresh();
            //LoadGrid();
          }
        }
      }
    }
    #endregion

    #region ChkguFollowGetGuest_Click
    private void ChkguFollowGetGuest_Click(object sender, RoutedEventArgs e)
    {
      var Searched = guestSearchedDataGrid.Items.GetItemAt(guestSearchedDataGrid.Items.IndexOf(guestSearchedDataGrid.CurrentItem)) as GuestSearched;
      var chkkFollow = sender as CheckBox;
      chkkFollow.IsChecked = ValueRevert(chkkFollow.IsChecked.Value);
      if (ValidateFollowUp(Searched.guCheckIn, Searched.guFollow, Searched.guAvail, Searched.guInfo, Searched.guInvit, Searched.guCheckOutD))
      {
        frmFollowUp frmFoll = new frmFollowUp(Searched.guID);
        frmFoll.Owner = this;
        frmFoll.ShowInTaskbar = false;
        if (!frmFoll.ShowDialog().Value)
        {
          if (frmFoll._wasSaved)
          {
            guestSearchedDataGrid.SelectedItems.OfType<GuestSearched>().ToList().ForEach(item =>
            {
              item.guFollowD = frmFoll.FollowD;
              item.guPRFollow = frmFoll.PRFollow;
              item.guFollow = true;
              item.guAvail = true;
              item.guInfo = true;
            });
            guestSearchedDataGrid.Items.Refresh();
            //LoadGrid();
          }
        }
      }
    }

  
    #endregion

    #region NotesSearched_MouseLeftButtonUp
    private void NotesSearched_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (guestSearchedDataGrid.SelectedItem != null)
      {
        var Arrival = guestSearchedDataGrid.SelectedItem as GuestSearched;
        frmPRNotes prnote = new frmPRNotes(Arrival.guID);
        prnote.Owner = this;
        prnote.ShowInTaskbar = false;
        if (!prnote.ShowDialog().Value)
        {
          if (prnote._saveNote)
          {
            guestSearchedDataGrid.SelectedItems.OfType<ObjGuestSearched>().ToList().ForEach(item => item.guPRNote = true);
            guestSearchedDataGrid.Items.Refresh();
          }
        }
      }
    }

    private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
    {

    }
    #endregion

    #region ChkBookCancSearched_Click
    private void ChkBookCancSearched_Click(object sender, RoutedEventArgs e)
    {
      var Searched = guestSearchedDataGrid.Items[guestSearchedDataGrid.Items.CurrentPosition] as GuestSearched;
      var chk = sender as CheckBox;
      chk.IsChecked = ValueRevert(chk.IsChecked.Value);
      if (ValidateCancelInvitation(Searched.guCheckIn, Searched.guCheckOutD, Searched.guInvit, Searched.guShow))
      {
        var log = ValidateLogin();
        if (log != null)
        {
          frmBookingCancel bc = new frmBookingCancel(Searched.guID, log.userData.User);
          bc.Owner = this;
          bc.ShowInTaskbar = false;
          if (!bc.ShowDialog().Value)
          {
            guestSearchedDataGrid.SelectedItems.OfType<GuestSearched>().ToList().ForEach(item => item.guBookCanc = bc._cancelado.Value);
            guestSearchedDataGrid.Items.Refresh();
          }
        }
      }
    }
    #endregion


    #endregion

    #region btnArrivals_Clicked
    /// <summary>
    ///Evento que ocurre cuando se oprime el boton arrival y ejecuta las configuaraciones 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>[jorcanche] 22/03/2015</history>
    private void btnArrivals_Clicked(object sender, RoutedEventArgs e)
    {
      EnabledCtrls(true, true, true, true);
      DataGridVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Hidden,Visibility.Hidden);
      LoadGrid();
    }
    #endregion

    #region btnAvailables_Clicked
    /// <summary>
    ///Evento que ocurre cuando se oprime el boton Availables y ejecuta las configuaraciones 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>[jorcanche] 22/03/2015</history>
    private void btnAvailables_Clicked(object sender, RoutedEventArgs e)
    {
      EnabledCtrls(false, false, true, true);
      DataGridVisibility(Visibility.Hidden, Visibility.Visible, Visibility.Hidden,Visibility.Hidden);
      LoadGrid();
    }
    #endregion

    #region btnPremanifiest_Click
    /// <summary>
    ///Evento que ocurre cuando se oprime el boton Premanifest y ejecuta las configuaraciones 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>[jorcanche] 22/03/2015</history>
    private void btnPremanifiest_Click(object sender, RoutedEventArgs e)
    {
      EnabledCtrls(false, true, false, false);
      DataGridVisibility(Visibility.Hidden, Visibility.Hidden, Visibility.Visible,Visibility.Hidden);
      LoadGrid();
    }
    #endregion

    #region btnDaysOff_Click
    private void btnDaysOff_Click(object sender, RoutedEventArgs e)
    {
      frmDaysOff frmDaysOff = new frmDaysOff(EnumTeamType.TeamPRs);
      frmDaysOff.Show();
    }
    #endregion

    #region btnSearchGuest_Click
    /// <summary>
    /// Desplaza la informacion encontrada en el DataGrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>[jorcanche] 17/03/2016</history>
    private void btnSearchGuest_Click(object sender, RoutedEventArgs e)
    {
      frmSearchGuests SearchGuests = new frmSearchGuests();
      SearchGuests.Owner = this;
      SearchGuests.ShowInTaskbar = false;
      //Validamos que se halla cerrado la ventana 
      if (!SearchGuests.ShowDialog().Value)
      {         
        //Validamos que le halla dado aceptar y no Cerrar ventana.
        if (!SearchGuests._Cancel)
        {
          //Traemos los Datos
          _guestDateTo = SearchGuests._dateTo;
          _guestdateFrom = SearchGuests._dateFrom;
          _guestGuid = SearchGuests._guestID;
          _guestName = SearchGuests._name;
          _guestRoom = SearchGuests._room;
          _guestReservation = SearchGuests._reservation;
          //Manipulamos los controlos 
          EnabledCtrls(false, false, false, false, false, false);
          //Ocultamos los demas datagrid´s
          DataGridVisibility(Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Visible);
          LoadGrid();
        }

      }
    }
    #endregion

    #region btnAssistance_Click
    private void btnAssistance_Click(object sender, RoutedEventArgs e)
    {
      Forms.frmAssistance frmAssistance = new Forms.frmAssistance(EnumPlaceType.LeadSource);
      frmAssistance.Show();
    }
    #endregion

    #region ChkInvit_Click
    private void ChkInvit_Click(object sender, RoutedEventArgs e)
    {
      GuestArrival itema = dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;
      var chk = sender as CheckBox;    //bool? con = ck.IsChecked;
      var userData = IM.BusinessRules.BR.BRPersonnel.Login(Model.Enums.EnumLoginType.Location, App.User.User.peID, App.User.Location.loID);
      var invit = new IM.Base.Forms.frmInvitationBase(IM.BusinessRules.Enums.InvitationType.InHouse, userData, itema.guID, chk.IsChecked.Value ? Model.Enums.EnumInvitationMode.modOnlyRead : Model.Enums.EnumInvitationMode.modAdd);
      invit.ShowDialog();
    }
    #endregion

    #endregion

  

  
  }

}



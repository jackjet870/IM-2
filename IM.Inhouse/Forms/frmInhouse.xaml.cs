using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Inhouse.Classes;
using IM.Inhouse.Forms;
using IM.Model;
using IM.Model.Classes;
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
using IM.Services.CallCenterService;
using IM.Inhouse.Reports;
using System.IO;
using System.Collections;
using System.Windows.Documents;
using IM.Model.Helpers;
using System.Data;

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmRegister.xaml
  /// /// </summary>
  public partial class frmInhouse : Window
  {
    #region Atributos

    private CollectionViewSource _guestPremanifestViewSource;
    private CollectionViewSource _guestArrivalViewSource;
    private CollectionViewSource _guestAvailableViewSource;
    private CollectionViewSource _guestSearchedViewSource;

    private DateTime _serverDate, _guestdateFrom, _guestDateTo;
    private int _available, _invited, _onGroup, _info, _guestGuid = 0;
    private string _markets = "ALL", _guestName, _guestRoom, _guestReservation;

    //Le damos memoria solo una vez al visualizador de noticias
    //frmNotices not = new frmNotices();

    private EnumScreen screen;
   
    #endregion

    #region Constructores y destructores

    public frmInhouse()
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
            BRGuests.GetGuestsArrivals(_serverDate, App.User.LeadSource.lsID, _markets, _available, _info, _invited, _onGroup)
            .Select(parent => new ObjGuestArrival(parent)).ToList();
          screen = EnumScreen.Arrivals;
        }
        //GuestAvailable
        if (ccAvailables.Visibility.Equals(Visibility.Visible))
        {
          //_guestAvailableViewSource.Source = BRGuests.GetGuestsAvailables(BRHelpers.GetServerDate().Date, App.User.LeadSource.lsID, _markets, _info, _invited, _onGroup);
          _guestAvailableViewSource.Source =
           BRGuests.GetGuestsAvailables(BRHelpers.GetServerDate().Date, App.User.LeadSource.lsID, _markets, _info, _invited, _onGroup)
           .Select(parent => new ObjGuestAvailable(parent)).ToList();
          screen = EnumScreen.Availables;
        }
        //GuestPremanifest
        if (ccPremanifest.Visibility.Equals(Visibility.Visible))
        {
          _guestPremanifestViewSource.Source =
          BRGuests.GetGuestsPremanifest(_serverDate, App.User.LeadSource.lsID, _markets, _onGroup)
            .Select(parent => new ObjGuestPremanifest(parent)).ToList();
          screen = EnumScreen.Premanifest;
        }
        //GuestSearched
        if (ccGetGuest.Visibility.Equals(Visibility.Visible))
        {
          _guestSearchedViewSource.Source =
           BRGuests.GetGuests(_guestdateFrom, _guestDateTo, App.User.LeadSource.lsID, _guestName, _guestRoom, _guestReservation, _guestGuid)
            .Select(parent => new ObjGuestSearched(parent)).ToList();
          screen = EnumScreen.Search;
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
              MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to mark this record as Check-in? \n This change can not be undone.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
        UIHelper.ShowMessage("Guest is not contacted.", MessageBoxImage.Asterisk);
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
            SaveAvailGuest(itemGuestArrival.guID);
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
            SaveAvailGuest(itemGuestPremanifest.guID);
            //Si no hubo problema en las validaciones mandamos el valor que obtuvo al hacer click en el checkbox          
            return itemGuestPremanifest.guCheckIn;
          }
          else
          {
            //despalomeamos el checkbox porque no se pude hacer el checkin
            return false;
          }
        default:
          GuestSearched itemGuestSearched = guest as GuestSearched;
          if (ValidateCheckIn(itemGuestSearched.guCheckIn, itemGuestSearched.guCheckInD, itemGuestSearched.guCheckOutD))
          {
            //determinamos si el huesped debe estar como disponible
            if (itemGuestSearched.guum == 0)
            {
              guestSearchedDataGrid.SelectedItems.OfType<GuestSearched>().ToList().ForEach(item => item.guAvail = true);
              guestSearchedDataGrid.Items.Refresh();
            }
            SaveAvailGuest(itemGuestSearched.guID);
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
      if (App.User.AutoSign)
      {
        //App.User.User.pePwd = EncryptHelper.Encrypt(App.User.User.pePwd);
        log.userData = App.User;
      }
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
        //log.userData.User.pePwd = EncryptHelper.Encrypt(App.User.User.pePwd);
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

    #region GetEquityReport

    /// <summary>
    /// Invoca un reporte de Equity
    /// </summary>
    /// <param name="guest">Fila del dtg seleccionada</param>
    /// <param name="type">1 - Arrivals | 2 - Aviable |3 - Premanifest | 4 GuestSearched </param>
    /// <history>
    /// [ecanul] 06/04/2016 Created
    /// </history>
    void GetEquityReport(object guest, int type)
    {
      StaStart("Loading Equity Report ...");
      switch (type)
      {
        case 1:
          var itemGuestArrival = guest as GuestArrival;
          EquityHelpers.EquityReport(itemGuestArrival.guMembershipNum, itemGuestArrival.guCompany, (int)itemGuestArrival.agcl, (int)itemGuestArrival.gucl);
          break;
        case 2:
          var itemGuestAvailable = guest as GuestAvailable;
          EquityHelpers.EquityReport(itemGuestAvailable.guMembershipNum, itemGuestAvailable.guCompany, (int)itemGuestAvailable.agcl, (int)itemGuestAvailable.gucl);
          break;
        case 3:
          var itemGuestPremanifest = guest as GuestPremanifest;
          EquityHelpers.EquityReport(itemGuestPremanifest.guMembershipNum, itemGuestPremanifest.guCompany, (int)itemGuestPremanifest.agcl, (int)itemGuestPremanifest.gucl);
          break;
        case 4:
          var itemGuestSearched = guest as GuestSearched;
          EquityHelpers.EquityReport(itemGuestSearched.guMembershipNum, itemGuestSearched.guCompany, (int)itemGuestSearched.agcl, (int)itemGuestSearched.gucl);
          break;
      }
      StaEnd();
    }
    
    #endregion

    #region Reservation
    /// <summary>
    /// Muestra el reporte de la reservacion del Guest
    /// </summary>
    /// <param name="dg">Ingresar el Datagrid</param>
    /// <history>
    /// [jorcanche] 04/04/2016
    /// </history>
    public void Reservation(DataGrid dg)
    {
      var guest = dg.Items[dg.Items.CurrentPosition] as GuestArrival;
      if (!string.IsNullOrEmpty(guest.gulsOriginal) && !string.IsNullOrEmpty(guest.guHReservID))
      {
        //obtenemos los datos del reporte del servicio de Wire PR
        RptReservationOrigos reservation =
         WirePRHelper.GetRptReservationOrigos(guest.gulsOriginal, guest.guHReservID);
        if (reservation == null)
        {
          UIHelper.ShowMessage("Reservation not found", MessageBoxImage.Error);
        }
        else
        {
          var rpt = new rptReservation();
          var reporte = new List<RptReservationOrigos>();
          reporte.Add(reservation);
          rpt.Load();
          rpt.SetDataSource(reporte);
          var _frmViewer = new frmViewer(rpt);
          _frmViewer.ShowDialog();
        }
      }
    }
    #endregion

    #region OpenInfo
    /// <summary>
    /// Despliega formulario de contactacion
    /// </summary>
    /// <param name="dg">Datagrid</param>
    /// <param name="objguest">Objeto Guest Arrival ó Guest Premanifest ó Guest Arrival ó Guest Searched</param>
    /// <param name="sender"> Instancia del CheckBox</param>
    public void OpenInfo(DataGrid dg, bool guCheckIn, bool guInfo, DateTime guCheckOutD, int guID, object sender, int tipo)
    {
      var chkguInfo = sender as CheckBox;
      chkguInfo.IsChecked = ValueRevert(chkguInfo.IsChecked.Value);

      if (ValidateContact(guCheckIn, guInfo, guCheckOutD))
      {
        StaStart("Loading Contact´s Info...");
        frmContact frmCont = new frmContact(guID);
        frmCont.Owner = this;
        frmCont.ShowInTaskbar = false;
        StaEnd();
        if (!frmCont.ShowDialog().Value)
        {
          if (frmCont._wasSave)
          {
            StaStart("Save Contact´s Info...");
            UpdateGridInfo(tipo, dg, frmCont);
            StaEnd();
          }
        }
        StaEnd();
      }
      StaEnd();
    }
    #endregion

    #region UpdateGridInfo
    /// <summary>
    /// Actualiza el datagrid segun el los parametros ingresados
    /// </summary>
    /// <param name="tipo">Tipo de Guest: Arrival, Premanifest, Avail, Searched</param>
    /// <param name="dg"> DataGrid </param>
    /// <param name="frmCont">Formulario de contactacion</param>
    public void UpdateGridInfo(int tipo, DataGrid dg, frmContact frmCont)
    {
      switch (tipo)
      {
        case 1:
          dg.SelectedItems.OfType<GuestArrival>().ToList().ForEach(item =>
          { item.guPRInfo = frmCont.PRInfo; item.guInfoD = frmCont.InfoD; item.guCheckIn = true; item.guInfo = true; });
          dg.Items.Refresh();
          //LoadGrid();
          break;
        case 2:
          dg.SelectedItems.OfType<GuestAvailable>().ToList().ForEach(item =>
          { item.guPRInfo = frmCont.PRInfo; item.guInfoD = frmCont.InfoD; item.guCheckIn = true; item.guInfo = true; });
          dg.Items.Refresh();
          //LoadGrid();
          break;
        case 3:
          dg.SelectedItems.OfType<GuestPremanifest>().ToList().ForEach(item =>
          { item.guPRInfo = frmCont.PRInfo; item.guCheckIn = true; item.guInfo = true; });
          dg.Items.Refresh();
          //LoadGrid();
          break;
        case 4:
          dg.SelectedItems.OfType<GuestSearched>().ToList().ForEach(item =>
          { item.guPRInfo = frmCont.PRInfo; item.guInfoD = frmCont.InfoD; item.guCheckIn = true; item.guInfo = true; });
          dg.Items.Refresh();
          //LoadGrid();
          break;
      }
    }

    #endregion

    #region OpenAvail
    public void OpenAvail(DataGrid dg, int guID, bool guCheckIn, object sender, int tipo)
    {
      var chkguAvail = sender as CheckBox;
      chkguAvail.IsChecked = ValueRevert(chkguAvail.IsChecked.Value);
      if (ValidateAvailability(guCheckIn))
      {
        StaStart("Loading Information´s Availability...");
        frmAvailability frmAvail = new frmAvailability(guID);
        frmAvail.Owner = this;
        frmAvail.ShowInTaskbar = false;
        StaEnd();
        if (!frmAvail.ShowDialog().Value)
        {
          if (frmAvail._wasSaved)
          {
            StaStart("Save Information´s Availability...");
            //Descripcion: Actualiza los datos del grid despues de guardar la informacion de disponibilidad
            //PR de Disponibilidad y si se marco como no disponible 
            UpdateGridAvail(tipo, dg, frmAvail);
            StaEnd();
          }
        }
      }
      StaEnd();
    }
    #endregion

    #region UpdateGridAvail
    public void UpdateGridAvail(int tipo, DataGrid dg, frmAvailability frmAvail)
    {
      switch (tipo)
      {
        case 1:
          dg.SelectedItems.OfType<GuestArrival>().ToList().ForEach(item =>
          { item.guPRAvail = frmAvail.guPRAvail; item.guum = frmAvail.guum; item.guAvail = frmAvail.Avail; });
          dg.Items.Refresh();
          //LoadGrid();
          break;
        case 2:
          dg.SelectedItems.OfType<ObjGuestAvailable>().ToList().ForEach(item =>
          { item.guPRAvail = frmAvail.guPRAvail; item.guum = frmAvail.guum; item.guAvail = frmAvail.Avail; });
          dg.Items.Refresh();
          //LoadGrid();
          break;
        case 3:
          dg.SelectedItems.OfType<GuestPremanifest>().ToList().ForEach(item =>
          { item.guAvail = frmAvail.Avail; });
          dg.Items.Refresh();
          //LoadGrid();
          break;
        case 4:
          dg.SelectedItems.OfType<GuestSearched>().ToList().ForEach(item =>
          { item.guPRAvail = frmAvail.guPRAvail; item.guum = frmAvail.guum; item.guAvail = frmAvail.Avail; });
          dg.Items.Refresh();
          //LoadGrid();
          break;
      }
    }
    #endregion

    #region OpenFollow
    public void OpenFollow(DataGrid dg, bool guCheckIn, bool guInfo, bool guFollow, bool guAvail, bool guInvit, DateTime guCheckOutD, int guID, object sender, int tipo)
    {
      var chkkFollow = sender as CheckBox;
      chkkFollow.IsChecked = ValueRevert(chkkFollow.IsChecked.Value);
      if (ValidateFollowUp(guCheckIn, guFollow, guAvail, guInfo, guInvit, guCheckOutD))
      {
        StaStart("Loading Follow Up screen...");
        frmFollowUp frmFoll = new frmFollowUp(guID);
        frmFoll.Owner = this;
        frmFoll.ShowInTaskbar = false;
        StaEnd();
        if (!frmFoll.ShowDialog().Value)
        {
          if (frmFoll._wasSaved)
          {
            StaStart("Save Follow up screen");
            UpdateGridFollow(tipo, dg, frmFoll);
            //LoadGrid();
            StaEnd();
          }
        }
      }
    }
    #endregion

    #region UpdateGridFollow
    public void UpdateGridFollow(int tipo, DataGrid dg, frmFollowUp frmFoll)
    {
      switch (tipo)
      {
        case 1:
          dg.SelectedItems.OfType<GuestArrival>().ToList().ForEach(item =>
         {
           item.guFollowD = frmFoll.FollowD;
           item.guPRFollow = frmFoll.PRFollow;
           item.guFollow = true;
           item.guAvail = true;
           item.guInfo = true;
         });
          dg.Items.Refresh();
          //LoadGrid();
          break;
        case 2:
          dg.SelectedItems.OfType<GuestAvailable>().ToList().ForEach(item =>
          {
            item.guFollowD = frmFoll.FollowD;
            item.guPRFollow = frmFoll.PRFollow;
            item.guFollow = true;
            item.guAvail = true;
            item.guInfo = true;
          });
          dg.Items.Refresh();
          //LoadGrid();
          break;
        case 3:
          dg.SelectedItems.OfType<GuestPremanifest>().ToList().ForEach(item =>
          {
            item.guFollowD = frmFoll.FollowD;
            item.guPRFollow = frmFoll.PRFollow;
            item.guFollow = true;
            item.guAvail = true;
            item.guInfo = true;
          });
          dg.Items.Refresh();
          //LoadGrid();
          break;
        case 4:
          dg.SelectedItems.OfType<GuestSearched>().ToList().ForEach(item =>
          {
            item.guFollowD = frmFoll.FollowD;
            item.guPRFollow = frmFoll.PRFollow;
            item.guFollow = true;
            item.guAvail = true;
            item.guInfo = true;
          });
          dg.Items.Refresh();
          //LoadGrid();
          break;
      }
    }
    #endregion

    #region ShowbtnWitGifts

    /// <summary>
    /// Muestra u oculta el Boton Pre/Gifts
    /// </summary>
    /// <param name="show"> true muestra | false oculta</param>
    /// <history>
    /// [ecanul] 19/04/2016 Created
    /// </history>
    void ShowbtnWitGifts(bool show)
    {
      if (show)
        colGifts.Width = new GridLength(1, GridUnitType.Star);
      else
        colGifts.Width = new GridLength(0);
    } 

    #endregion

    #region CreateExcelReport

    /// <summary>
    /// Invoca el Reporte Solicitado en formato Excel
    /// </summary>
    /// <history>
    /// <param name="WithGifts">Opcional true = PremanifestWithGifts | false reporte comun</param>
    /// [ecanul] 18/04/2016 Created
    /// [ecanul] 19/04/2016 Modificated Agregada funcionalidad para Aviables, Premanifest y Premanifest With Gifts
    /// </history>
    void CreateExcelReport(bool WithGifts = false)
    {
      bool hasData = false;
      switch (screen)
      {
        case EnumScreen.Arrivals:
          if (dgGuestArrival.Items.Count > 0)
          {
            List<RptArrivals> arrivals = BRGeneralReports.GetRptArrivals(dtpDate.SelectedDate.Value, App.User.LeadSource.lsID, _markets, _available, _info, _invited, _onGroup);
            ReportsToExcel.ArrivalsToExcel(arrivals, dtpDate.SelectedDate.Value);
            hasData = true;
          }
          break;
        case EnumScreen.Availables:
          if (dgGuestAvailable.Items.Count > 0)
          {
            List<RptAvailables> aviables = BRGeneralReports.GetRptAviables(BRHelpers.GetServerDate().Date, App.User.LeadSource.lsID, _markets, _info, _invited, _onGroup);
            ReportsToExcel.AvailablesToExcel(aviables);
            hasData = true;
          }
          break;
        case EnumScreen.Premanifest:
          if (dgGuestPremanifest.Items.Count > 0)
          {
            if (!WithGifts) //Si no se mando nada o mando falso
            {
              List<RptPremanifest> premanifest = BRGeneralReports.GetRptPremanifest(dtpDate.SelectedDate.Value, App.User.LeadSource.lsID, _markets, _onGroup);
              ReportsToExcel.PremanifestToExcel(premanifest);
              hasData = true;
            }
            else
            {
              List<RptPremanifestWithGifts> withGifts = BRGeneralReports.GetRptPremanifestWithGifts(dtpDate.SelectedDate.Value, App.User.LeadSource.lsID, _markets, _onGroup);
              ReportsToExcel.PremanifestWithGiftsToExcel(withGifts);
              hasData = true;
            }
          }
          break;
      }

      if (hasData) //Muestra mensaje para informar que el reporte ha sido generado con exito
        UIHelper.ShowMessage("Generated Report", MessageBoxImage.Information, "Inhouse");
      else //Si el Grid esta vacio
        UIHelper.ShowMessage("There is no data.");

    } 

    #endregion

    #region StaStart
    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <param name="message">mensaje a mostrar</param>
    /// <history>[jorcanche] 05/04/2016 Created </history>
    private void StaStart(string message)
    {
      lblStatusBarMessage.Text = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      this.Cursor = Cursors.Wait;
    }
    #endregion

    #region StaEnd
    /// <summary>
    /// Indica en la barra de estado que se finalizo un proceso
    /// </summary>
    /// <history>[jorcanche] 05/04/2016 Created</history>
    private void StaEnd()
    {
      lblStatusBarMessage.Text = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      this.Cursor = null;
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
      Inhouse_Loaded();
    }

    #endregion

    #region Inhouse_Loaded
    /// <summary>
    /// Inicializa Inhouse (Carga los controles y la Informacón dependiendo del LeadSource y el usuario Logueado)
    /// </summary>
    /// <history>
    /// [jorcanche] 11/04/2016
    /// </history>
    private void Inhouse_Loaded()
    {
      //Guardamos el log del login 
      BRLoginLogs.SaveGuestLog(App.User.Location.loID, App.User.User.peID, Environment.MachineName.ToString());

      //Cargamos la variable de Occupancy
      txtOccupancy.Text = BRLeadSources.GetOccupationLeadSources(BRHelpers.GetServerDate().Date, App.User.Location.loID).ToString(); ;

      //Indicamos al statusbar que me muestre cierta informacion cuando oprimimos cierto teclado
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);

      //StaStart("Load Inhouse...");
      //Cargamos las variables del usuario
      txtUser.Text = App.User.User.peN;
      txtLocation.Text = App.User.Location.loN;

      //Cargamos la fecha actual del servidor
      dtpDate.SelectedDate = BRHelpers.GetServerDate().Date;

      //Inicializamos las variables de los DataGrids
      _guestArrivalViewSource = ((CollectionViewSource)(this.FindResource("guestArrivalViewSource")));
      _guestAvailableViewSource = ((CollectionViewSource)(this.FindResource("guestAvailableViewSource")));
      _guestPremanifestViewSource = ((CollectionViewSource)(this.FindResource("guestPremanifestViewSource")));
      _guestSearchedViewSource = ((CollectionViewSource)(this.FindResource("guestSearchedViewSource")));

      //Cargamos los datagrids
      LoadGrid();

      //Cargamos el listado de markets
      listMarkets.ItemsSource = BRMarkets.GetMarkets(1);
      //StaEnd();

      //Abrimos el visualizador de  noticias    
      Window win = Application.Current.Windows.Cast<Window>().FirstOrDefault(x => x is frmNotices);
      if (win != null)
      {
        win.Activate();
        return;
      }
      win = new frmNotices();
      win.Show();
      //if (not != null)
      //{
      //  if (!not.IsLoaded)
      //  {
      //    not.Show();
      //  }
      //}
      //else
      //{
      //  not = new frmNotices();
      //  not.Show();
      //}
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
        txtOccupancy.Text = BRLeadSources.GetOccupationLeadSources(picker.SelectedDate.Value, App.User.Location.loID);
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
      OpenAvail(dgGuestArrival, Arrival.guID, Arrival.guCheckIn, sender, 1);
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
      OpenInfo(dgGuestArrival, Arrival.guCheckIn, Arrival.guInfo, Arrival.guCheckOutD, Arrival.guID, sender, 1);
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
      //StaStart("Loading Follow Up screen...");
      var Arrival = dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;
      OpenFollow(dgGuestArrival, Arrival.guCheckIn, Arrival.guInfo, Arrival.guFollow, Arrival.guAvail, Arrival.guInvit, Arrival.guCheckOutD, Arrival.guID, sender, 1);
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

    #region chkGuestsGroupsArrivals_Clic
    private void chkGuestsGroupsArrivals_Click(object sender, RoutedEventArgs e)
    {
      bool show = true;
      bool group = false;
      frmGuestsGroups frmGgGu;
      var chk = sender as CheckBox;
      int guID, guIDToAdd = 0;
      GuestsGroup gg = new GuestsGroup();
      EnumAction action;

      GuestArrival itema = dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;

      action = EnumAction.None;
      guID = itema.guID;
      group = itema.guGroup;
      if (itema.guGroup)//Si tiene Group
      {
        gg = BRGuestsGroupsIntegrants.GetGuestGroupByGuest(guID);
        guIDToAdd = 0;
      }//Si no tiene Grupo
      else
      {
        guIDToAdd = guID;
        //impedimos modificar los datos si el sistema esta en modo de solo lectura
        if (App.Current.Properties.IsReadOnly)
        {
          MessageBox.Show("The system is in read-only mode", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          show = false;
        }//Si no esta en modo ReadOnly
        else
        {
          MessageBoxCustomHelper.CustomMeesageBox("Create group", "Add to group", "Cancel");
          MessageBoxResult res = MessageBox.Show("This guest does not belong to a group \nDo you want to create a new group \nor want to add the guest to an existing group?",
            "Add Guest", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

          switch (res)
          {
            case MessageBoxResult.Yes: //Crea Nuevo
              action = EnumAction.Add;
              break;
            case MessageBoxResult.No://Agrega A
              action = EnumAction.AddTo;
              break;
            case MessageBoxResult.Cancel://Cancela
              show = false;
              break;
          }
          MessageBoxCustomHelper.ReturnToCommonMessageBox();
        }
      }
      if (show)//Si no se cancelo en nungun momento se muestra el formulario
      {
        frmGgGu = new frmGuestsGroups(gg.gxID, guID, guIDToAdd, dtpDate.SelectedDate.Value, action);
        frmGgGu.ShowDialog();
        Guest gu = BRGuests.GetGuest(guID);
        group = gu.guGroup;
        LoadGrid();
      }
    }

    #endregion

    #region ReservationArrival_MouseLeftButtonUp
    private void ReservationArrival_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      Reservation(dgGuestArrival);
    }
    #endregion

    #region ArrivalsEquity_MouseLeftButtonUp
    private void ArrivalsEquity_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var guest = dgGuestArrival.Items[dgGuestArrival.Items.CurrentPosition] as GuestArrival;
      GetEquityReport(guest, 1);
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
      OpenAvail(dgGuestAvailable, Avilable.guID, Avilable.guCheckIn, sender, 2);
    }

    #endregion

    #region ChkguInfoAvailable_Click
    private void ChkguInfoAvailable_Click(object sender, RoutedEventArgs e)
    {
      var Available = dgGuestAvailable.Items.GetItemAt(dgGuestAvailable.Items.IndexOf(dgGuestAvailable.CurrentItem)) as GuestAvailable;
      OpenInfo(dgGuestAvailable, Available.guCheckIn, Available.guInfo, Available.guCheckOutD, Available.guID, sender, 2);
    }

    #endregion

    #region ChkFollowAvailable_Click
    private void ChkFollowAvailable_Click(object sender, RoutedEventArgs e)
    {
      //StaStart("Loading Follow Up screen...");
      var Available = dgGuestAvailable.Items.GetItemAt(dgGuestAvailable.Items.IndexOf(dgGuestAvailable.CurrentItem)) as GuestAvailable;
      OpenFollow(dgGuestAvailable, Available.guCheckIn, Available.guInfo, Available.guFollow, Available.guAvail, Available.guInvit, Available.guCheckOutD, Available.guID, sender, 2);
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
          StaStart("loading Cancel invitation screen...");
          frmBookingCancel bc = new frmBookingCancel(Available.guID, log.userData.User);
          bc.Owner = this;
          bc.ShowInTaskbar = false;
          StaEnd();
          if (!bc.ShowDialog().Value)
          {
            dgGuestAvailable.SelectedItems.OfType<GuestAvailable>().ToList().ForEach(item => item.guBookCanc = bc._cancelado.Value);
            dgGuestAvailable.Items.Refresh();
          }
        }
      }
    }
    #endregion

    #region chkGuestsGroupsAviables

    private void chkGuestsGroupsAvailables_Click(object sender, RoutedEventArgs e)
    {
      bool show = true;
      bool group = false;
      frmGuestsGroups frmGgGu;
      var chk = sender as CheckBox;
      int guID, guIDToAdd = 0;
      GuestsGroup gg = new GuestsGroup();
      EnumAction action;

      GuestAvailable itema = dgGuestAvailable.Items.GetItemAt(dgGuestAvailable.Items.IndexOf(dgGuestAvailable.CurrentItem)) as GuestAvailable;

      action = EnumAction.None;
      guID = itema.guID;
      group = itema.guGroup;
      if (itema.guGroup)//Si tiene Group
      {
        gg = BRGuestsGroupsIntegrants.GetGuestGroupByGuest(guID);
        guIDToAdd = 0;
      }//Si no tiene Grupo
      else
      {
        guIDToAdd = guID;
        //impedimos modificar los datos si el sistema esta en modo de solo lectura
        if (App.Current.Properties.IsReadOnly)
        {
          MessageBox.Show("The system is in read-only mode", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          show = false;
        }//Si no esta en modo ReadOnly
        else
        {
          MessageBoxCustomHelper.CustomMeesageBox("Create group", "Add to group", "Cancel");
          MessageBoxResult res = MessageBox.Show("This guest does not belong to a group \nDo you want to create a new group \nor want to add the guest to an existing group?",
            "Add Guest", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

          switch (res)
          {
            case MessageBoxResult.Yes: //Crea Nuevo
              action = EnumAction.Add;
              break;
            case MessageBoxResult.No://Agrega A
              action = EnumAction.AddTo;
              break;
            case MessageBoxResult.Cancel://Cancela
              show = false;
              break;
          }
          MessageBoxCustomHelper.ReturnToCommonMessageBox();
        }
      }
      if (show)//Si no se cancelo en nungun momento se muestra el formulario
      {
        frmGgGu = new frmGuestsGroups(gg.gxID, guID, guIDToAdd, dtpDate.SelectedDate.Value, action);
        frmGgGu.ShowDialog();
        Guest gu = BRGuests.GetGuest(guID);
        group = gu.guGroup;
        LoadGrid();
      }
    }

    #endregion

    #region AviablesEquity_MouseLeftButtonUp
    private void AviablesEquity_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var guest = dgGuestAvailable.Items[dgGuestAvailable.Items.CurrentPosition] as GuestAvailable;
      GetEquityReport(guest, 2);
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
      //StaStart("Loading Information´s Availability...");
      var Premanifest = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.IndexOf(dgGuestPremanifest.CurrentItem)) as GuestPremanifest;
      OpenAvail(dgGuestPremanifest, Premanifest.guID, Premanifest.guCheckIn, sender, 3);
    }
    #endregion

    #region ChkguInfoPremanifest_Click
    private void ChkguInfoPremanifest_Click(object sender, RoutedEventArgs e)
    {
      GuestArrival itema = dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;
      var chk = sender as CheckBox;    //bool? con = ck.IsChecked;
      var userData = BRPersonnel.Login(Model.Enums.EnumLoginType.Location, App.User.User.peID, App.User.Location.loID);
      var invit = new frmInvitationBase(EnumInvitationType.InHouse, userData, itema.guID, !chk.IsChecked.Value ? EnumInvitationMode.modOnlyRead : EnumInvitationMode.modAdd);
      var res = invit.ShowDialog();
      chk.IsChecked = (res.HasValue && res.Value) || itema.guInvit;
      var Premanifest = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.IndexOf(dgGuestPremanifest.CurrentItem)) as GuestPremanifest;
      OpenInfo(dgGuestPremanifest, Premanifest.guCheckIn, Premanifest.guInfo, Premanifest.guCheckOutD, Premanifest.guID, sender, 3);
    }
    #endregion

    #region ChkguFollowPremanifest_Click
    private void ChkguFollowPremanifest_Click(object sender, RoutedEventArgs e)
    {
      //StaStart("Loading Follow Up screen...");
      var Premanifest = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.IndexOf(dgGuestPremanifest.CurrentItem)) as GuestPremanifest;
      OpenFollow(dgGuestPremanifest, Premanifest.guCheckIn, Premanifest.guInfo, Premanifest.guFollow, Premanifest.guAvail, Premanifest.guInvit, Premanifest.guCheckOutD, Premanifest.guID, sender, 3);
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
        StaStart("loading Cancel invitation screen...");
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

    #region chkGuestsGroupsPremanifest

    private void chkGuestsGroupsPremanifest_Click(object sender, RoutedEventArgs e)
    {
      bool show = true;
      bool group = false;
      frmGuestsGroups frmGgGu;
      var chk = sender as CheckBox;
      int guID, guIDToAdd = 0;
      GuestsGroup gg = new GuestsGroup();
      EnumAction action;

      GuestPremanifest itema = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.IndexOf(dgGuestPremanifest.CurrentItem)) as GuestPremanifest;

      action = EnumAction.None;
      guID = itema.guID;
      group = itema.guGroup;
      if (itema.guGroup)//Si tiene Group
      {
        gg = BRGuestsGroupsIntegrants.GetGuestGroupByGuest(guID);
        guIDToAdd = 0;
      }//Si no tiene Grupo
      else
      {
        guIDToAdd = guID;
        //impedimos modificar los datos si el sistema esta en modo de solo lectura
        if (App.Current.Properties.IsReadOnly)
        {
          MessageBox.Show("The system is in read-only mode", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          show = false;
        }//Si no esta en modo ReadOnly
        else
        {
          MessageBoxCustomHelper.CustomMeesageBox("Create group", "Add to group", "Cancel");
          MessageBoxResult res = MessageBox.Show("This guest does not belong to a group \nDo you want to create a new group \nor want to add the guest to an existing group?",
            "Add Guest", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

          switch (res)
          {
            case MessageBoxResult.Yes: //Crea Nuevo
              action = EnumAction.Add;
              break;
            case MessageBoxResult.No://Agrega A
              action = EnumAction.AddTo;
              break;
            case MessageBoxResult.Cancel://Cancela
              show = false;
              break;
          }
          MessageBoxCustomHelper.ReturnToCommonMessageBox();
        }
      }
      if (show)//Si no se cancelo en nungun momento se muestra el formulario
      {
        frmGgGu = new frmGuestsGroups(gg.gxID, guID, guIDToAdd, dtpDate.SelectedDate.Value, action);
        frmGgGu.ShowDialog();
        Guest gu = BRGuests.GetGuest(guID);
        group = gu.guGroup;
        LoadGrid();
      }
    }

    #endregion

    #region PremanifestEquity_MouseLeftButtonUp
    private void PremanifestEquity_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var guest = dgGuestPremanifest.Items[dgGuestPremanifest.Items.CurrentPosition] as GuestPremanifest;
      GetEquityReport(guest, 3);
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
        chk.IsChecked = CheckIn(guestSearchedDataGrid.Items[guestSearchedDataGrid.Items.CurrentPosition], 4);
      }
    }
    #endregion

    #region ChkguAvailGetGuest_Click
    private void ChkguAvailGetGuest_Click(object sender, RoutedEventArgs e)
    {
      //StaStart("Loading Information´s Availability...");
      var Searched = guestSearchedDataGrid.Items.GetItemAt(guestSearchedDataGrid.Items.IndexOf(guestSearchedDataGrid.CurrentItem)) as GuestSearched;
      OpenAvail(dgGuestPremanifest, Searched.guID, Searched.guCheckIn, sender, 4);
    }
    #endregion

    #region ChkguInfoGetGuest_Click
    private void ChkguInfoGetGuest_Click(object sender, RoutedEventArgs e)
    {
      var Searched = guestSearchedDataGrid.Items.GetItemAt(guestSearchedDataGrid.Items.IndexOf(guestSearchedDataGrid.CurrentItem)) as GuestSearched;
      OpenInfo(guestSearchedDataGrid, Searched.guCheckIn, Searched.guInfo, Searched.guCheckOutD, Searched.guID, sender, 4);
    }
    #endregion

    #region ChkguFollowGetGuest_Click
    private void ChkguFollowGetGuest_Click(object sender, RoutedEventArgs e)
    {
      var Searched = guestSearchedDataGrid.Items.GetItemAt(guestSearchedDataGrid.Items.IndexOf(guestSearchedDataGrid.CurrentItem)) as GuestSearched;
      OpenFollow(guestSearchedDataGrid, Searched.guCheckIn, Searched.guInfo, Searched.guFollow, Searched.guAvail, Searched.guInvit, Searched.guCheckOutD, Searched.guID, sender, 4);
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


    private void CheckBox_Click(object sender, RoutedEventArgs e)
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
          StaStart("loading Cancel invitation screen...");
          frmBookingCancel bc = new frmBookingCancel(Searched.guID, log.userData.User);
          bc.Owner = this;
          bc.ShowInTaskbar = false;
          StaEnd();
          if (!bc.ShowDialog().Value)
          {
            guestSearchedDataGrid.SelectedItems.OfType<GuestSearched>().ToList().ForEach(item => item.guBookCanc = bc._cancelado.Value);
            guestSearchedDataGrid.Items.Refresh();
          }
        }
      }
    }
    #endregion

    #region GetGuestEquity_MouseLeftButtonUp

    private void GetGuestEquity_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var guest = guestSearchedDataGrid.Items[guestSearchedDataGrid.Items.CurrentPosition] as GuestSearched;
      GetEquityReport(guest, 4);
    }

    #endregion

    #endregion

    #region chkGuestsGroupsSearched

    private void chkGuestsGroupsGuestSearched_Click(object sender, RoutedEventArgs e)
    {
      bool show = true;
      bool group = false;
      frmGuestsGroups frmGgGu;
      var chk = sender as CheckBox;
      int guID, guIDToAdd = 0;
      GuestsGroup gg = new GuestsGroup();
      EnumAction action;

      GuestSearched itema = guestSearchedDataGrid.Items.GetItemAt(guestSearchedDataGrid.Items.IndexOf(guestSearchedDataGrid.CurrentItem)) as GuestSearched;

      action = EnumAction.None;
      guID = itema.guID;
      group = itema.guGroup;
      if (itema.guGroup)//Si tiene Group
      {
        gg = BRGuestsGroupsIntegrants.GetGuestGroupByGuest(guID);
        guIDToAdd = 0;
      }//Si no tiene Grupo
      else
      {
        guIDToAdd = guID;
        //impedimos modificar los datos si el sistema esta en modo de solo lectura
        if (App.Current.Properties.IsReadOnly)
        {
          MessageBox.Show("The system is in read-only mode", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          show = false;
        }//Si no esta en modo ReadOnly
        else
        {
          MessageBoxCustomHelper.CustomMeesageBox("Create group", "Add to group", "Cancel");
          MessageBoxResult res = MessageBox.Show("This guest does not belong to a group \nDo you want to create a new group \nor want to add the guest to an existing group?",
            "Add Guest", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

          switch (res)
          {
            case MessageBoxResult.Yes: //Crea Nuevo
              action = EnumAction.Add;
              break;
            case MessageBoxResult.No://Agrega A
              action = EnumAction.AddTo;
              break;
            case MessageBoxResult.Cancel://Cancela
              show = false;
              break;
          }
          MessageBoxCustomHelper.ReturnToCommonMessageBox();
        }
      }
      if (show)//Si no se cancelo en nungun momento se muestra el formulario
      {
        frmGgGu = new frmGuestsGroups(gg.gxID, guID, guIDToAdd, dtpDate.SelectedDate.Value, action);
        frmGgGu.ShowDialog();
        Guest gu = BRGuests.GetGuest(guID);
        group = gu.guGroup;
        LoadGrid();
      }
    }

    #endregion

    #region dg_SelectionChanged
    private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var dg = sender as DataGrid;
      StatusBarReg.Content = string.Format("{0}/{1}", dg.Items.CurrentPosition + 1, dg.Items.Count);
    }



    #endregion

    #region Comments_LostFocus
    private void Comments_LostFocus(object sender, RoutedEventArgs e)
    {
      var txt = sender as TextBox;
      if (!string.IsNullOrEmpty(txt.Text))
      {
        Guest guest = null;
        if (txt.Name == "guCommentsColumnArrival")
        {
          var Row = dgGuestArrival.SelectedItem as GuestArrival;
          guest = BRGuests.GetGuest(Row.guID);
        }
        if (txt.Name == "guCommentsColumnAvailable")
        {
          var Row = dgGuestAvailable.SelectedItem as GuestAvailable;
          guest = BRGuests.GetGuest(Row.guID);
        }
        if (txt.Name == "guCommentsColumnPremanifest")
        {
          var Row = dgGuestPremanifest.SelectedItem as GuestPremanifest;
          guest = BRGuests.GetGuest(Row.guID);
        }
        if (txt.Name == "guCommentsColumnSearched")
        {
          var Row = guestSearchedDataGrid.SelectedItem as GuestSearched;
          guest = BRGuests.GetGuest(Row.guID);
        }
        guest.guComments = txt.Text;
        BRGuests.SaveGuest(guest);
      }
    }
    #endregion

    #region CommentsColumn_Loaded
    private void CommentsColumn_Loaded(object sender, RoutedEventArgs e)
    {
      var txt = sender as TextBox;
      txt.Focus();
    }
    #endregion

    #region btnRefresh_Click
    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
      StaStart("Loading...");
      LoadGrid();
      StaEnd();
    }
    #endregion

    #region btnLogin_Click
    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
      frmLogin log = new frmLogin(null, false, EnumLoginType.Location, true);
      if (App.User.AutoSign)
      {
        log.userData = App.User;
      }
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
        App.User = log.userData;
        Inhouse_Loaded();
      }
    }
    #endregion

    #region btnAbout_Click
    private void btnAbout_Click(object sender, RoutedEventArgs e)
    {
      frmAbout formAbout = new frmAbout();
      formAbout.ShowInTaskbar = false;
      formAbout.ShowDialog();
    }
    #endregion

    #region btnPreview_Click
    private void btnPreview_Click(object sender, RoutedEventArgs e)
    {
      //string Culturas = string.Empty;
      //foreach (string str in LanguageHelper.cultures())
      //{
      //  Culturas = Culturas + str + " \n";
      //}
      //MessageBox.Show(Culturas);

      StaStart("Printing "+ EnumToListHelper.GetEnumDescription(screen) + "...");
      CreateExcelReport();
      StaEnd();
    }

    #endregion

    #region btnPrint_Click
    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      //RPTInvitation();
    }
    #endregion

    //public void RPTInvitation()
    //{
    //  //Traemos la informacion del store y la almacenamos en un procedimiento
    //  InvitationData invitationData = BRInvitation.RptInvitationData(7751973);

    //  //Determinamos el Lenguaje
    //  LanguageHelper.IDLanguage = invitationData.Invitation.gula;

    //  //Modificamos el tamaño de las condiciones de la invitación (Pie de Informe "itRTFFooter")
    //  //invitationData.Invitation.itRTFFooter = ChangedPropertyRTF(invitationData.Invitation.itRTFFooter);
    //  //var s = invitationData.Invitation.itRTFFooter

    //  //Le damos memoria al reporte de Invitacion
    //  var rptInvi = new rptInvitation();

    //  //Le agregamos la informacion 
    //  rptInvi.SetDataSource(TableHelper.GetDataTableFromList(ObjectHelper.ObjectToList(invitationData.Invitation)));

    //  //Cargamos los subreportes
    //  rptInvi.Subreports["rptInvitationGuests.rpt"].SetDataSource(TableHelper.GetDataTableFromList(invitationData.InvitationGuest));
    //  rptInvi.Subreports["rptInvitationDeposits.rpt"].SetDataSource(invitationData.InvitationDeposit);
    //  rptInvi.Subreports["rptInvitationGifts.rpt"].SetDataSource(TableHelper.GetDataTableFromList(invitationData.InvitationGift));

    //  //Cargamos las Etiquetas 
    //  lblInvitation(rptInvi);

    //  //Cargamos el Viewer
    //  var _frmViewer = new frmViewer(rptInvi);
    //  _frmViewer.ShowDialog();
    //}

    //public void lblInvitation(rptInvitation rptInv)
    //{       
    //  //Etiquetas de Depositos
    //  rptInv.SetParameterValue("lblDeposits", LanguageHelper.GetMessage(EnumMessage.msgLblDeposits));
    //  rptInv.SetParameterValue("lblDeposit", LanguageHelper.GetMessage(EnumMessage.msgLblDeposit));
    //  rptInv.SetParameterValue("lblCurrency", LanguageHelper.GetMessage(EnumMessage.msgLblCurrency));    
    //  //Depositos quemados
    //  rptInv.SetParameterValue("lblDepositBurned", LanguageHelper.GetMessage(EnumMessage.msgLblDepositBurned));
    //  //************************************************************************************************

    //  //Etiquetas de Regalos      
    //  rptInv.SetParameterValue("lblGifts", LanguageHelper.GetMessage(EnumMessage.msgLblGifts));
    //  rptInv.SetParameterValue("lblQuantityGifts", LanguageHelper.GetMessage(EnumMessage.msgLblQuantityAbbreviation));
    //  rptInv.SetParameterValue("lblGift", LanguageHelper.GetMessage(EnumMessage.msgLblGift));
    //  //************************************************************************************************

    //  //Etiquetas Invitados
    //  rptInv.SetParameterValue("lblGuests", LanguageHelper.GetMessage(EnumMessage.msgLblGuests));
    //  rptInv.SetParameterValue("lblLastName", LanguageHelper.GetMessage(EnumMessage.msgLblLastName));
    //  rptInv.SetParameterValue("lblFirstName", LanguageHelper.GetMessage(EnumMessage.msgLblFirstName));
    //  rptInv.SetParameterValue("lblAge", LanguageHelper.GetMessage(EnumMessage.msgLblAge));
    //  rptInv.SetParameterValue("lblMaritalStatus", LanguageHelper.GetMessage(EnumMessage.msgLblMaritalStatus));
    //  rptInv.SetParameterValue("lblOccupation", LanguageHelper.GetMessage(EnumMessage.msgLblOccupation));
    //  //************************************************************************************************
      
    //  //Numero de membresia
    //  rptInv.SetParameterValue("lblMembershipNum", LanguageHelper.GetMessage(EnumMessage.msgLblMembershipNum));
    //  //Fecha
    //  rptInv.SetParameterValue("lblDate", LanguageHelper.GetMessage(EnumMessage.msgLblDate));
    //  //Hora
    //  rptInv.SetParameterValue("lblTime", LanguageHelper.GetMessage(EnumMessage.msgLblTime));
    //  //Agencia
    //  rptInv.SetParameterValue("lblAgency", LanguageHelper.GetMessage(EnumMessage.msgLblAgency));
    //  //Pais
    //  rptInv.SetParameterValue("lblCountry", LanguageHelper.GetMessage(EnumMessage.msgLblCountry));
    //  //Hotel
    //  rptInv.SetParameterValue("lblHotel", LanguageHelper.GetMessage(EnumMessage.msgLblHotel));
    //  //Numero de habitación
    //  rptInv.SetParameterValue("lblRoomNum", LanguageHelper.GetMessage(EnumMessage.msgLblRoomNum));
    //  //Pax
    //  rptInv.SetParameterValue("lblPax", LanguageHelper.GetMessage(EnumMessage.msgLblPax));
    //  //Location
    //  rptInv.SetParameterValue("lblLocation", LanguageHelper.GetMessage(EnumMessage.msgLblLocation));
    //  //Guest Service
    //  rptInv.SetParameterValue("lblGuestService", LanguageHelper.GetMessage(EnumMessage.msgLblGuestService));
    //  //Notas
    //  rptInv.SetParameterValue("lblNotes", LanguageHelper.GetMessage(EnumMessage.msgLblNotes));
    //  //Fecha y hora
    //  rptInv.SetParameterValue("lblDateTime", BRHelpers.GetServerDate());
    //  //Cambiado por
    //  rptInv.SetParameterValue("lblChangedBy", App.User.User.peID);
    //}

    public string ChangedPropertyRTF(string rtf)
    {
      //using (MemoryStream ms = new MemoryStream())
      //{
        var rtbRTF = new System.Windows.Forms.RichTextBox();

        rtbRTF.Rtf = rtf;

         rtbRTF.SelectionFont = new System.Drawing.Font("Arial", 13);
         rtbRTF.Font = rtbRTF.SelectionFont;
        //rtbRTF.Font = rtbRTF.SelectionFont;

        // rtbRTF.SelectAll();
        return rtbRTF.Rtf.ToString();
        //rtbRTF.Selection.ApplyPropertyValue(Inline.FontSizeProperty, new System.Drawing.Size(FontSize, FontSize));

        //  TextRange textRange = new TextRange(rtbRTF.Document.ContentStart, rtbRTF.Document.ContentEnd);
        //  textRange.Save(ms, DataFormats.Rtf);
        //  return System.Text.Encoding.Default.GetString(ms.ToArray());
      //}
    }

    public string SetRtf(string document)
    {
      var documentBytes = System.Text.Encoding.UTF8.GetBytes(document);
      using (var reader = new MemoryStream(documentBytes))
      {
        RichTextBox rtb = new RichTextBox();
        reader.Position = 0;
        rtb.SelectAll();
        rtb.Selection.Load(reader, DataFormats.Rtf);
        rtb.FontSize = 14;

        return rtb.SelectionBrush.ToString() ; ;
      }
    }

    #region btnWithGifts_Click

    private void btnWithGifts_Click(object sender, RoutedEventArgs e)
    {
      CreateExcelReport(true);
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
      StaStart("Loading Arrival...");
      EnabledCtrls(true, true, true, true);
      DataGridVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);
      LoadGrid();
      StaEnd();
      //oculta el boton btnWitGifts que exporta el reporte Premanifest WithGifts
      ShowbtnWitGifts(false);
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
      StaStart("Loading Available...");
      EnabledCtrls(false, false, true, true);
      DataGridVisibility(Visibility.Hidden, Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
      LoadGrid();
      StaEnd();
      //oculta el boton btnWitGifts que exporta el reporte Premanifest WithGifts
      ShowbtnWitGifts(false);
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
      StaStart("Loading Premanifest...");
      EnabledCtrls(false, true, false, false);
      DataGridVisibility(Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
      LoadGrid();
      StaEnd();
      //Muestra el boton btnWitGifts que exporta el reporte Premanifest WithGifts
      ShowbtnWitGifts(true);
    }
    #endregion

    #region btnGroups_Click
    private void btnGrouos_Click(object sender, RoutedEventArgs e)
    {
      Forms.frmGuestsGroups frmGroups = new frmGuestsGroups(0, 0, 0, dtpDate.SelectedDate.Value, EnumAction.Search);
      frmGroups.ShowDialog();
    }
    #endregion

    #region btnDaysOff_Click
    private void btnDaysOff_Click(object sender, RoutedEventArgs e)
    {
      frmDaysOff frmDaysOff = new frmDaysOff(EnumTeamType.TeamPRs);
      frmDaysOff.ShowDialog();
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
          StaStart("loading Searched...");
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
          StaEnd();
          //oculta el boton btnWitGifts que exporta el reporte Premanifest WithGifts
          ShowbtnWitGifts(false);
        }

      }
    }
    #endregion

    #region btnAssistance_Click
    private void btnAssistance_Click(object sender, RoutedEventArgs e)
    {
      Forms.frmAssistance frmAssistance = new Forms.frmAssistance(EnumPlaceType.LeadSource);
      frmAssistance.ShowDialog();
    }
    #endregion

    #region ChkInvit_Click
    private void ChkInvit_Click(object sender, RoutedEventArgs e)
    {
      GuestArrival itema = dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;
      var chk = sender as CheckBox;
      if (!itema.guCheckIn)
      {
        MessageBox.Show("Guest has not made Check In");
        chk.IsChecked = false;
        return;
      }

      var isChecked = chk.IsChecked.HasValue && chk.IsChecked.Value;
      chk.IsChecked = itema.guInvit;
      var userData = BRPersonnel.Login(EnumLoginType.Location, App.User.User.peID, App.User.Location.loID);
      var invit = new frmInvitationBase(EnumInvitationType.InHouse, userData, itema.guID, !isChecked ? EnumInvitationMode.modOnlyRead : EnumInvitationMode.modAdd);
      invit.Owner = this;
      invit.ShowInTaskbar = false;
      var res = invit.ShowDialog();
      itema.guInvit = itema.guInvit || (res.HasValue && res.Value);
      chk.IsChecked = itema.guInvit;
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

 
  }
}


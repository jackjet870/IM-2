using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Inhouse.Classes;
using IM.Inhouse.Reports;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Services.Helpers;
using IM.Services.WirePRService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Application = System.Windows.Application;
using CheckBox = System.Windows.Controls.CheckBox;
using Cursors = System.Windows.Input.Cursors;
using DataGrid = System.Windows.Controls.DataGrid;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using RadioButton = System.Windows.Controls.RadioButton;
using TextBox = System.Windows.Controls.TextBox;

namespace IM.Inhouse.Forms
{
  /// <summary>
  /// Formulario para la gestion de los Huespedes
  /// </summary>
  /// <history>
  /// [jorcanche] created 15/03/2016
  /// </history>
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
    private EnumScreen _screen = EnumScreen.Arrivals;

    #endregion Atributos

    #region Constructores y destructores

    public frmInhouse()
    {
      InitializeComponent();
    }

    #endregion Constructores y destructores

    #region Metodos

    #region EnabledCtrls

    /// <summary>
    /// Configura los controles para que esten habilitados o deshabilidatos cuando se presionan los TabsControl
    /// </summary>
    /// <param name="av">gprAvailable Falso / Verdadero</param>
    /// <param name="Da">dtpDate  Falso / Verdadero></param>
    /// <param name="Inf">gprInfo Falso / Verdadero </param>
    /// <param name="Inv">gprInvited Falso/ Verdadero </param>
    /// <param name="Ong">gprOngroup Falso/ Verdadero</param>
    /// <param name="Mks">listMarkets Falso/ Verdadero</param>
    /// <history>
    /// [jorcanche] 23/02/2015 Created
    /// </history>
    private void EnabledCtrls(bool av, bool Da, bool Inf, bool Inv, bool Ong = true, bool Mks = true)
    {
      gprAvailable.IsEnabled = av;
      dtpDate.IsEnabled = Da;
      gprInfo.IsEnabled = Inf;
      gprInvited.IsEnabled = Inv;
      gprOngroup.IsEnabled = Ong;
      listMarkets.IsEnabled = Mks;
    }

    #endregion EnabledCtrls

    #region LoadGrid

    /// <summary>
    /// Metodo que sirve para carga los DataGrid's segun su estado de Visibilidad
    /// </summary>
    ///<history>
    /// [jorcanche] 15/03/2016 created
    ///</history>
    private async void LoadGrid()
    {
      var serverDate = BRHelpers.GetServerDate();
      if (_guestArrivalViewSource == null || _guestPremanifestViewSource == null || _guestAvailableViewSource == null) return;

      switch (_screen)
      {
        case EnumScreen.Arrivals: //GuestArrival
          ccArrivals.Visibility = Visibility.Visible;
          ccAvailables.Visibility = ccPremanifest.Visibility = ccGetGuest.Visibility = Visibility.Hidden;
          var lstGuestArrivals = await BRGuests.GetGuestsArrivals(_serverDate, App.User.LeadSource.lsID, _markets, _available, _info, _invited, _onGroup);
          _guestArrivalViewSource.Source = lstGuestArrivals.Select(parent => new ObjGuestArrival(parent, serverDate)).ToList();
          break;

        case EnumScreen.Availables: //GuestAvailable
          ccAvailables.Visibility = Visibility.Visible;
          ccArrivals.Visibility = ccGetGuest.Visibility = ccPremanifest.Visibility = Visibility.Hidden;
          var lstGuestAvailables = await BRGuests.GetGuestsAvailables(BRHelpers.GetServerDate().Date, App.User.LeadSource.lsID, _markets, _info, _invited, _onGroup);
          _guestAvailableViewSource.Source = lstGuestAvailables.Select(parent => new ObjGuestAvailable(parent, serverDate));
          break;

        case EnumScreen.Premanifest: //GuestPremanifest
          ccPremanifest.Visibility = Visibility.Visible;
          ccArrivals.Visibility = ccGetGuest.Visibility = ccAvailables.Visibility = Visibility.Hidden;
          var lstGuestsPremanifest = await BRGuests.GetGuestsPremanifest(_serverDate, App.User.LeadSource.lsID, _markets, _onGroup);
          _guestPremanifestViewSource.Source = lstGuestsPremanifest.Select(parent => new ObjGuestPremanifest(parent, serverDate)).ToList();
          break;

        case EnumScreen.Search: //GuestSearch
          ccGetGuest.Visibility = Visibility.Visible;
          ccArrivals.Visibility = ccPremanifest.Visibility = ccAvailables.Visibility = Visibility.Hidden;
          var lstSearchGuest = await BRGuests.GetGuests(_guestdateFrom, _guestDateTo, App.User.LeadSource.lsID, _guestName, _guestRoom, _guestReservation, _guestGuid);
          _guestSearchedViewSource.Source = lstSearchGuest.Select(parent => new ObjGuestSearched(parent, serverDate)).ToList();
          break;

        default:
          throw new ArgumentOutOfRangeException();
      }
      StaEnd();
    }

    #endregion LoadGrid

    #region ValidateCheckIn

    /// <summary>
    ///  Valida los datos para poder hacer Check In
    /// </summary>
    /// <param name="checkIn">Si ya se hizo CheckIn</param>
    /// <param name="pguCheckInD"></param>
    /// <param name="pguCheckOutD"></param>
    /// <history>[jorcanche] 17/03/2016</history>
    private static bool ValidateCheckIn(bool checkIn, DateTime pguCheckInD, DateTime pguCheckOutD)
    {
      // impedimos modificar los datos si el sistema esta en modo de solo lectura
      //si tiene permiso estandar
      if (!App.User.HasPermission(EnumPermission.Register, EnumPermisionLevel.Standard)) return true;
      //si no tiene
      if (!checkIn) return true;
      //validamos que el huesped este en casa
      if (App.User.HasPermission(EnumPermission.Register, EnumPermisionLevel.Standard) &&
          pguCheckInD > BRHelpers.GetServerDate() || pguCheckOutD < BRHelpers.GetServerDate())
      {
        UIHelper.ShowMessage("Guest is not in house.", MessageBoxImage.Asterisk, "Permissions");
        return false;
      }
      //validamos que el huesped este en casa con 2 dias de tolerancia
      if (App.User.HasPermission(EnumPermission.Register, EnumPermisionLevel.Special) &&
          pguCheckInD > BRHelpers.GetServerDate().AddDays(+2) ||
          pguCheckOutD < BRHelpers.GetServerDate().AddDays(-2))
      {
        UIHelper.ShowMessage("Guest is not in house.", MessageBoxImage.Asterisk, "Permissions");
        return false;
      }
      //Preguntamos al usuario si en verdan desea darle Check In al Huesped
      var result =
        UIHelper.ShowMessage("Are you sure you want to mark this record as Check-in? \n This change can not be undone.",
         MessageBoxImage.Question);
      return result != MessageBoxResult.No;
    }

    #endregion ValidateCheckIn

    #region ValidateAvailability

    /// <summary>
    /// Valida los parametros para que pueda abrir el formulario de Availability
    /// </summary>
    /// <param name="checkIn"></param>
    /// <history>[jorcanche] 16/03/2016</history>
    private static bool ValidateAvailability(bool checkIn)
    {
      //Validamos que el huesped haya hecho Check In
      if (!checkIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-in.");
        return false;
      }
      if (App.User.HasPermission(EnumPermission.Available, EnumPermisionLevel.ReadOnly)) return true;
      UIHelper.ShowMessage("Access denied.");
      return false;
    }

    #endregion ValidateAvailability

    #region ValidateContact

    /// <summary>
    /// Valida los datos para desplegar el formulario de contactacion
    /// </summary>
    /// <param name="checkIn">Si ya hizo CheckIn</param>
    /// <param name="contact"> Si ya esta contactado</param>
    /// <param name="checkOutD">Fecha de contactación</param>
    /// <returns></returns>
    ///<history>[jorcanche] 13/03/2016</history>
    private static bool ValidateContact(bool checkIn, bool contact, DateTime checkOutD)
    {
      //validamos que el huesped haya hecho Check In
      if (!checkIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-in.");
        return false;
      }
      // no se permite contactar si ya hizo Check Out o si ya esta contactado el Guest
      if (!contact && checkOutD < BRHelpers.GetServerDate())
      {
        UIHelper.ShowMessage("Guest already made Check-out.");
        return false;
      }
      //validamos que el usuario tenga permiso de lectura
      if (App.User.HasPermission(EnumPermission.Register, EnumPermisionLevel.ReadOnly)) return true;
      UIHelper.ShowMessage("Access denied.");
      return false;
    }

    #endregion ValidateContact

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
    private static bool ValidateFollowUp(bool checkIn, bool followUp, bool avail, bool contact, bool invit,
      DateTime checkOutD)
    {
      //Validamos que el huesped haya hecho Check In
      if (!checkIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-in.");
        return false;
      }
      //validamos que el huesped no haya hecho Check Out
      if (!followUp && checkOutD < BRHelpers.GetServerDate())
      {
        UIHelper.ShowMessage("Guest already made Check-out.");
        return false;
      }
      //Validamos que el huesped este disponible
      if (!followUp && !avail)
      {
        UIHelper.ShowMessage("Guest is not available.");
        return false;
      }
      //Validamos que el huesped este contactado
      if (!followUp && !contact)
      {
        UIHelper.ShowMessage("Guest is not contacted.");
        return false;
      }
      //Validamos que el Huesped no este invitado
      if (!followUp && invit)
      {
        UIHelper.ShowMessage("Guest is invited.");
        return false;
      }
      //Validamos que el usuario tenga permisos de lectura
      if (App.User.HasPermission(EnumPermission.Register, EnumPermisionLevel.ReadOnly)) return true;
      UIHelper.ShowMessage("Access denied.");
      return false;
    }

    #endregion ValidateFollowUp

    #region CheckIn

    /// <summary>
    /// Valida el tipo de guest y determina si el husped debe estar como disponible
    /// </summary>
    /// <history>[jorcanche] 15/03/2016 </history>
    public async Task<bool> CheckIn(DataGrid dg)
    {
      var items = dg.SelectedItem;
      int nIndex = dg.SelectedIndex;
      int nColumn = dg.CurrentCell.Column.DisplayIndex;
      Type t = items.GetType();
      //Validamos
      if (ValidateCheckIn((bool)t.GetProperty("guCheckIn").GetValue(items),
        (DateTime)t.GetProperty("guCheckInD").GetValue(items),
        (DateTime)t.GetProperty("guCheckOutD").GetValue(items)))
      {
        #region Updated DataGrid

        var guestGuum = await BRGuests.GetGuest((int)t.GetProperty("guID").GetValue(items));
        //Determinamos si el huesped debe estar como disponible
        var guum = t.Name != "ObjGuestPremanifest" ? (byte)t.GetProperty("guum").GetValue(items) : guestGuum.guum;

        if (guum.Equals(0))
        {
          t.GetProperty("guAvail").SetValue(items, true);
          dg.Items.Refresh();
        }

        #endregion Updated DataGrid

        #region Save DataBase

        var editGuest = await BRGuests.GetGuest((int)t.GetProperty("guID").GetValue(items));
        editGuest.guCheckIn = true;
        editGuest.guAvail = guum.Equals(0);
        await BREntities.OperationEntity(editGuest,EnumMode.Edit);

        #endregion Save DataBase

        GridHelper.SelectRow(dg, nIndex, nColumn);
        //Si no hubo problema en las validaciones mandamos el valor que obtuvo al hacer click en el checkbox
        return (bool)t.GetProperty("guCheckIn").GetValue(items);
      }
      //despalomeamos el checkbox porque no se pude hacer el checkin
      return false;
    }

    #endregion CheckIn

    #endregion Metodos

    #region ValidateCancelInvitation

    //Valida los datos para cancelar una invitacion
    public bool ValidateCancelInvitation(bool guCheckIn, DateTime guCheckOutD, bool guInvit, bool guShow)
    {
      //Validamos que el huesped haya hecho CheckIn
      if (!guCheckIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-in.");
        return false;
      }
      //Validamos que el huesped no haya hehco Check Out
      if (guCheckOutD < BRHelpers.GetServerDate())
      {
        UIHelper.ShowMessage("Guest already made Check-out.");
        return false;
      }
      //Validamos que el huesped haya sido invitado
      if (!guInvit)
      {
        UIHelper.ShowMessage("Guest has not been invited.");
        return false;
      }
      //validamos que el huesped no tenga show
      if (!guShow) return true;
      UIHelper.ShowMessage("Guest already attended to the show.");
      return false;
    }

    #endregion ValidateCancelInvitation

    #region ValidateLogin

    public frmLogin ValidateLogin()
    {
      //Validamos las credenciales del usuario y sus permisos
      var log = new frmLogin();
      if (App.User.AutoSign)
      {
        log.UserData = App.User;
      }
      log.ShowDialog();
      if (!log.IsAuthenticated) return null;
      if (log.UserData.HasPermission(EnumPermission.Register, EnumPermisionLevel.Standard))
      {
        return log;
      }
      UIHelper.ShowMessage("You do not have the required permissions to perform this operation.", MessageBoxImage.Asterisk, "Permissions");
      return null;
    }

    #endregion ValidateLogin

    #region GetEquityReport

    /// <summary>
    /// Invoca un reporte de Equity
    /// </summary>
    /// <param name="guest">Fila del dtg seleccionada</param>
    /// <param name="type">1 - Arrivals | 2 - Aviable |3 - Premanifest | 4 GuestSearched </param>
    /// <history>
    /// [ecanul] 06/04/2016 Created
    /// </history>
    private void GetEquityReport(object guest, int type)
    {
      StaStart("Loading Equity Report ...");
      switch (type)
      {
        case 1:
          var itemGuestArrival = guest as GuestArrival;
          if (itemGuestArrival != null)
            EquityHelpers.EquityReport(itemGuestArrival.guMembershipNum, itemGuestArrival.guCompany,
              itemGuestArrival.agcl, itemGuestArrival.gucl);
          break;

        case 2:
          var itemGuestAvailable = guest as GuestAvailable;
          if (itemGuestAvailable != null)
            EquityHelpers.EquityReport(itemGuestAvailable.guMembershipNum, itemGuestAvailable.guCompany,
             itemGuestAvailable.agcl, itemGuestAvailable.gucl);
          break;

        case 3:
          var itemGuestPremanifest = guest as GuestPremanifest;
          if (itemGuestPremanifest != null)
            EquityHelpers.EquityReport(itemGuestPremanifest.guMembershipNum, itemGuestPremanifest.guCompany,
              itemGuestPremanifest.agcl, itemGuestPremanifest.gucl);
          break;

        case 4:
          var itemGuestSearched = guest as GuestSearched;
          if (itemGuestSearched != null)
            EquityHelpers.EquityReport(itemGuestSearched.guMembershipNum, itemGuestSearched.guCompany,
              itemGuestSearched.agcl, itemGuestSearched.gucl);
          break;
      }
      StaEnd();
    }

    #endregion GetEquityReport

    #region OpenInfo

    /// <summary>
    /// Despliega formulario de contactacion
    /// </summary>
    /// <param name="dg">Datagrid</param>
    /// <param name="sender"> Instancia del CheckBox</param>
    public void OpenInfo(DataGrid dg, bool guCheckIn, bool guInfo, DateTime guCheckOutD, int guID, object sender)
    {
      var chkguInfo = sender as CheckBox;
      chkguInfo.IsChecked = !chkguInfo.IsChecked.Value;

      if (ValidateContact(guCheckIn, !guInfo, guCheckOutD))
      {
        StaStart("Loading Contact´s Info...");
        frmContact frmCont = new frmContact(guID, App.User);
        frmCont.Owner = this;
        frmCont.ShowDialog();
        if (frmCont._wasSave)
        {
          StaStart("Save Contact´s Info...");

          #region Update

          var item = dg.SelectedItem;
          Type t = item.GetType();
          t.GetProperty("guPRInfo").SetValue(item, frmCont.PRInfo);
          t.GetProperty("guCheckIn").SetValue(item, true);
          t.GetProperty("guInfo").SetValue(item, true);
          if (t.Name != "ObjGuestPremanifest") t.GetProperty("guInfoD").SetValue(item, frmCont.InfoD);
          dg.Items.Refresh();
          GridHelper.SelectRow(dg, dg.SelectedIndex, dg.CurrentCell.Column.DisplayIndex);

          #endregion Update
        }
      }
      StaEnd();
    }

    #endregion OpenInfo

    #region OpenAvail

    public void OpenAvail(DataGrid dg, int guId, bool guCheckIn, object sender)
    {
      var chkguAvail = sender as CheckBox;
      //Validamos si es nulo
      if (chkguAvail?.IsChecked == null) return;

      chkguAvail.IsChecked = !chkguAvail.IsChecked.Value;
      if (ValidateAvailability(guCheckIn))
      {
        StaStart("Loading Information´s Availability...");
        var frmAvail = new frmAvailability(guId)
        {
          Owner = this,
          ShowInTaskbar = false
        };
        StaEnd();
        frmAvail.ShowDialog();
        if (frmAvail.WasSaved)
        {
          StaStart("Save Information´s Availability...");
          // Actualiza los datos del grid despues de guardar la informacion de disponibilidad PR de Disponibilidad y si se marco como no disponible

          #region Updated

          var item = dg.SelectedItem;
          var t = item.GetType();
          if (t.Name != "ObjGuestPremanifest")
          {
            t.GetProperty("guPRAvail").SetValue(item, frmAvail.GuPrAvail);
            t.GetProperty("guum").SetValue(item, frmAvail.Guum);
          }
          t.GetProperty("guAvail").SetValue(item, frmAvail.Avail);
          dg.Items.Refresh();
          GridHelper.SelectRow(dg, dg.SelectedIndex, dg.CurrentCell.Column.DisplayIndex);

          #endregion Updated
        }
      }
      StaEnd();
    }

    #endregion OpenAvail

    #region OpenFollow

    public void OpenFollow(DataGrid dg, bool guCheckIn, bool guInfo, bool guFollow, bool guAvail, bool guInvit, DateTime guCheckOutD, int guID, object sender)
    {
      var chkkFollow = sender as CheckBox;
      chkkFollow.IsChecked = !chkkFollow.IsChecked.Value;
      if (ValidateFollowUp(guCheckIn, !guFollow, guAvail, guInfo, guInvit, guCheckOutD))
      {
        StaStart("Loading Follow Up screen...");
        frmFollowUp frmFoll = new frmFollowUp(guID);
        frmFoll.Owner = this;
        frmFoll.ShowInTaskbar = false;
        StaEnd();
        frmFoll.ShowDialog();
        if (frmFoll._wasSaved)
        {
          StaStart("Save Follow up screen");

          #region Updated

          var item = dg.SelectedItem;
          Type t = item.GetType();
          t.GetProperty("guFollowD").SetValue(item, frmFoll.FollowD);
          t.GetProperty("guPRFollow").SetValue(item, frmFoll.PrFollow);
          t.GetProperty("guFollow").SetValue(item, true);
          t.GetProperty("guAvail").SetValue(item, true);
          t.GetProperty("guInfo").SetValue(item, true);
          dg.Items.Refresh();
          GridHelper.SelectRow(dg, dg.SelectedIndex, dg.CurrentCell.Column.DisplayIndex);

          #endregion Updated

          StaEnd();
        }
      }
    }

    #endregion OpenFollow

    #region CreateExcelReport

    /// <summary>
    /// Invoca el Reporte Solicitado en formato Excel
    /// </summary>
    /// <history>
    /// <param name="WithGifts">Opcional true = PremanifestWithGifts | false reporte comun</param>
    /// [ecanul] 18/04/2016 Created
    /// [ecanul] 19/04/2016 Modificated Agregada funcionalidad para Aviables, Premanifest y Premanifest With Gifts
    /// </history>
    private async void CreateExcelReport(bool WithGifts = false)
    {
      bool hasData = false;
      switch (_screen)
      {
        case EnumScreen.Arrivals:
          if (dgGuestArrival.Items.Count > 0)
          {
            List<RptArrivals> arrivals = BRGeneralReports.GetRptArrivals(dtpDate.Value.Value, App.User.LeadSource.lsID,
              _markets, _available, _info, _invited, _onGroup);
            ReportsToExcel.ArrivalsToExcel(arrivals, dtpDate.Value.Value);
            hasData = true;
          }
          break;

        case EnumScreen.Availables:
          if (dgGuestAvailable.Items.Count > 0)
          {
            List<RptAvailables> aviables = BRGeneralReports.GetRptAviables(BRHelpers.GetServerDate(),
              App.User.LeadSource.lsID, _markets, _info, _invited, _onGroup);
            ReportsToExcel.AvailablesToExcel(aviables);
            hasData = true;
          }
          break;

        case EnumScreen.Premanifest:
          if (dgGuestPremanifest.Items.Count > 0)
          {
            if (!WithGifts) //Si no se mando nada o mando falso
            {
              List<RptPremanifest> premanifest = await BRGeneralReports.GetRptPremanifest(dtpDate.Value.Value,
                App.User.LeadSource.lsID, _markets, _onGroup);
              ReportsToExcel.PremanifestToExcel(premanifest);
              hasData = true;
            }
            else
            {
              List<RptPremanifestWithGifts> withGifts = await BRGeneralReports.GetRptPremanifestWithGifts(
                dtpDate.Value.Value, App.User.LeadSource.lsID);
              ReportsToExcel.PremanifestWithGiftsToExcel(withGifts);
              hasData = true;
            }
          }
          break;
      }
      if (!hasData) //Muestra mensaje para informar que el reporte ha sido generado con exito
      {
        UIHelper.ShowMessage("There is no data.");
      }
    }

    #endregion CreateExcelReport

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

    #endregion StaStart

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

    #endregion StaEnd

    #region Eventos del formulario

    #region Window_Loaded

    /// <summary>
    /// Carga los controles que necesitamos
    /// </summary>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Inhouse_Loaded();
    }

    #endregion Window_Loaded

    #region Inhouse_Loaded

    /// <summary>
    /// Inicializa Inhouse (Carga los controles y la Informacón dependiendo del LeadSource y el usuario Logueado)
    /// </summary>
    /// <history>
    /// [jorcanche] 11/04/2016
    /// </history>
    private async void Inhouse_Loaded()
    {
      //Guardamos el log del login
      BRLoginLogs.SaveGuestLog(App.User.Location.loID, App.User.User.peID, Environment.MachineName);
      //Indicamos al statusbar que me muestre cierta informacion cuando oprimimos cierto teclado
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
      StaStart("Load Catalogs Inhouse...");
      //Cargamos las variables del usuario
      txtUser.Text = App.User.User.peN;
      txtLocation.Text = App.User.Location.loN;
      //Cargamos la fecha actual del servidor
      dtpDate.Value = BRHelpers.GetServerDate();
      dtpDate_ValueChanged(null, null);
      //Inicializamos las variables de los DataGrids
      _guestArrivalViewSource = (CollectionViewSource)FindResource("GuestArrivalViewSource");
      _guestAvailableViewSource = (CollectionViewSource)FindResource("GuestAvailableViewSource");
      _guestPremanifestViewSource = (CollectionViewSource)FindResource("GuestPremanifestViewSource");
      _guestSearchedViewSource = (CollectionViewSource)FindResource("GuestSearchedViewSource");

      //Cargamos los datagrids
      LoadGrid();

      //Cargamos el listado de markets
      listMarkets.ItemsSource = await BRMarkets.GetMarkets(1);

      //Abrimos el visualizador de  noticias
      var win = Application.Current.Windows.Cast<Window>().FirstOrDefault(x => x is frmNotices);
      if (win != null)
      {
        win.Activate();
        return;
      }
      win = new frmNotices { Owner = this };
      win.Show();
    }

    #endregion Inhouse_Loaded

    #region listMarkets_SelectionChanged

    /// <summary>
    /// ocurre el evento cuando se selecciona uno o mas mercados, los enlista en una cadena separados por comas
    /// </summary>
    /// <history>
    /// [jorcanche] 09/03/2016
    /// </history>
    private void listMarkets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var cont = 0;
      _markets = string.Empty;
      var selectedItems = listMarkets.SelectedItems;
      foreach (MarketShort selectedItem in selectedItems)
      {
        cont = cont + 1;
        _markets += selectedItem.mkID;
        if (selectedItems.Count > 1 && cont < selectedItems.Count)
        {
          _markets = _markets + ",";
        }
      }
      _markets = selectedItems.Count == 0 ? "ALL" : _markets;
      StaStart($"Loading {_screen} by Markets: {_markets}...");
      LoadGrid();
    }

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
      if (stbStatusBar != null) StaStart("Load Guest´s by " + ck.GroupName);
      LoadGrid();
    }

    #endregion rb_Checked

    #region 1.- Arrival

    #region ChkguCheckInArrival_Click

    /// <summary>
    /// Evento que ocurre cuando se selecciona alguna fila en la columna CheckIn del datagrid Arrival
    /// </summary>
    /// <history>
    /// [jorcanche] created 09/04/2016
    /// </history>
    private async void ChkguCheckInArrival_Click(object sender, RoutedEventArgs e)
    {
      //Debido a que el evento Click invierte el valor del Check
      //Lo solucionaremos invirtiendo el if sieguiente.
      //Es decir cuando el valor sea False quiere decie que originalmente es True
      //Y cuando el valor esta en True quiere decir que originalmente estaba en False
      var chk = sender as CheckBox;
      //Es decir cuando es true entra en el metodo CheckIn por que en la interfaz mostraba que estaba deseleccionado
      // y cuando esté seleccionado aqui en el evento estara en false y le regresaremos el valor a true para que no se pueda editar el guest 
      //ya que un Guest con Check In ya no se le puede quitar 

      chk.IsChecked = chk.IsChecked.Value ? await CheckIn(dgGuestArrival) : true;   
    }

    #endregion ChkguCheckInArrival_Click

    #region ChkguAvailArrival_Click

    /// <summary>
    /// Evento que ocurre cuando se selecciona alguna fila en la columna Avail del datagrid Arrival
    /// </summary>
    /// <history>
    /// [jorcanche] 09/01/2015
    /// </history>
    private void ChkguAvailArrival_Click(object sender, RoutedEventArgs e)
    {
      var Arrival = dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;
      OpenAvail(dgGuestArrival, Arrival.guID, Arrival.guCheckIn, sender);
    }

    #endregion ChkguAvailArrival_Click

    #region ChkguInfoArrival_Click

    /// <summary>
    /// Evento que ocurre cuando se selecciona alguna fila en la columna Info del datagrid Arrival
    /// </summary>
    /// <history>
    /// [jorcanche] 09/02/2015
    /// </history>
    private void ChkguInfoArrival_Click(object sender, RoutedEventArgs e)
    {
      var Arrival =
        dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;
      OpenInfo(dgGuestArrival, Arrival.guCheckIn, Arrival.guInfo, Arrival.guCheckOutD, Arrival.guID, sender);
    }

    #endregion ChkguInfoArrival_Click

    #region ChkFollowArrival_Click

    /// <summary>
    /// Evento que ocurre cuando se selecciona alguna fila en la columna FollowUp del datagrid Arrival
    /// </summary>
    /// <hitory>
    /// [jorcanche] 09/03/2015
    /// </hitory>
    private void ChkFollowArrival_Click(object sender, RoutedEventArgs e)
    {
      var Arrival = dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;
      OpenFollow(dgGuestArrival, Arrival.guCheckIn, Arrival.guInfo, Arrival.guFollow, Arrival.guAvail, Arrival.guInvit, Arrival.guCheckOutD, Arrival.guID, sender);
    }

    #endregion ChkFollowArrival_Click

    #region NotesArrival_MouseLeftButtonDown

    private void NotesArrival_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (dgGuestArrival.SelectedItem != null)
      {
        var Arrival = dgGuestArrival.Items[dgGuestArrival.Items.CurrentPosition] as GuestArrival;
        frmPRNotes prnote = new frmPRNotes(Arrival.guID);
        prnote.Owner = this;
        prnote.ShowInTaskbar = false;
        prnote.ShowDialog();

        if (prnote.SaveNote)
        {
          dgGuestArrival.SelectedItems.OfType<ObjGuestArrival>().ToList().ForEach(item => item.guPRNote = true);
          dgGuestArrival.Items.Refresh();
        }
      }
    }

    #endregion NotesArrival_MouseLeftButtonDown

    #region ChkBookCancArrival_Click

    /// <summary>
    /// Abre el formulario de frmBookingCancel
    /// </summary>
    /// <history>
    /// [jorcanche]  created 16/08/2016
    /// </history>
    private void ChkBookCancArrival_Click(object sender, RoutedEventArgs e)
    {
      var chk = sender as CheckBox;
      var guest = dgGuestArrival.Items[dgGuestArrival.Items.CurrentPosition] as GuestArrival;

      if (guest == null || chk?.IsChecked == null) return;

      chk.IsChecked = !chk.IsChecked.Value;
      if (!ValidateCancelInvitation(guest.guCheckIn, guest.guCheckOutD, guest.guInvit, guest.guShow)) return;
      var log = ValidateLogin();
      if (log == null) return;
      var bc = new frmBookingCancel(guest.guID, log.UserData.User)
      {
        Owner = this
      };
      bc.ShowDialog();

      dgGuestArrival.SelectedItems.OfType<GuestArrival>()
        .ToList()
        .ForEach(item =>
        {
          if (bc.Cancelado != null) item.guBookCanc = bc.Cancelado.Value;
        });
      dgGuestArrival.Items.Refresh();
    }

    #endregion ChkBookCancArrival_Click

    #region chkGuestsGroupsArrivals_Clic

    private async void chkGuestsGroupsArrivals_Click(object sender, RoutedEventArgs e)
    {
      bool show = true;
      bool group = false;
      frmGuestsGroups frmGgGu;
      var chk = sender as CheckBox;
      int guID, guIDToAdd = 0;
      GuestsGroup gg = new GuestsGroup();
      EnumAction action;

      GuestArrival itema =
        dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;

      action = EnumAction.None;
      guID = itema.guID;
      if (itema.guGroup) //Si tiene Group
      {
        gg = await BRGuestsGroupsIntegrants.GetGuestGroupByGuest(guID);
        guIDToAdd = 0;
      } //Si no tiene Grupo
      else
      {
        guIDToAdd = guID;
        //impedimos modificar los datos si el sistema esta en modo de solo lectura
        if (App.Current.Properties.IsReadOnly)
        {
          UIHelper.ShowMessage("The system is in read-only mode");
          show = false;
        } //Si no esta en modo ReadOnly
        else
        {
          MessageBoxCustomHelper.CustomMeesageBox("Create group", "Add to group", "Cancel");
          MessageBoxResult res =
            MessageBox.Show(
              "This guest does not belong to a group \nDo you want to create a new group \nor want to add the guest to an existing group?",
              "Add Guest", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

          switch (res)
          {
            case MessageBoxResult.Yes: //Crea Nuevo
              action = EnumAction.Add;
              break;

            case MessageBoxResult.No: //Agrega A
              action = EnumAction.AddTo;
              break;

            case MessageBoxResult.Cancel: //Cancela
              show = false;
              chk.IsChecked = false;
              break;
          }
          MessageBoxCustomHelper.ReturnToCommonMessageBox();
        }
      }
      if (show) //Si no se cancelo en nungun momento se muestra el formulario
      {
        frmGgGu = new frmGuestsGroups(gg.gxID, guID, guIDToAdd, dtpDate.Value.Value, action);
        frmGgGu.Owner = this;
        frmGgGu.ShowDialog();
        Guest gu = await BRGuests.GetGuest(guID);
        group = gu.guGroup;
        LoadGrid();
      }
    }

    #endregion chkGuestsGroupsArrivals_Clic

    #region ReservationArrival_MouseLeftButtonUp

    private void ReservationArrival_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      try
      {
        var guest = dgGuestArrival.Items[dgGuestArrival.Items.CurrentPosition] as GuestArrival;
        if (string.IsNullOrEmpty(guest?.gulsOriginal) || string.IsNullOrEmpty(guest.guHReservID)) return;
        //obtenemos los datos del reporte del servicio de Wire PR
        var reservation = WirePRHelper.GetRptReservationOrigos(guest.gulsOriginal, guest.guHReservID);
        if (reservation == null)
          UIHelper.ShowMessage("Reservation not found", MessageBoxImage.Error);
        else
        {
          var rpt = new rptReservation();
          var reporte = new List<RptReservationOrigos> { reservation };
          rpt.SetDataSource(reporte);
          var frmViewer = new frmViewer(rpt);
          frmViewer.ShowDialog();
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(UIHelper.GetMessageError(ex), MessageBoxImage.Error, "GetRptReservationOrigos");
      }
    }

    #endregion ReservationArrival_MouseLeftButtonUp

    #region ArrivalsEquity_MouseLeftButtonUp

    private void ArrivalsEquity_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var guest = dgGuestArrival.Items[dgGuestArrival.Items.CurrentPosition] as GuestArrival;
      GetEquityReport(guest, 1);
    }

    #endregion ArrivalsEquity_MouseLeftButtonUp

    #endregion 1.- Arrival

    #region 2.- Availables

    #region ChkguCheckInAvailables_Click

    private void ChkguCheckInAvailables_Click(object sender, RoutedEventArgs e)
    {
      //No contiene la columna ChekIn cuando es Available
    }

    #endregion ChkguCheckInAvailables_Click

    #region ChkguAvailAvailable_Click

    private void ChkguAvailAvailable_Click(object sender, RoutedEventArgs e)
    {
      var Avilable = dgGuestAvailable.Items.GetItemAt(dgGuestAvailable.Items.IndexOf(dgGuestAvailable.CurrentItem)) as GuestAvailable;
      OpenAvail(dgGuestAvailable, Avilable.guID, Avilable.guCheckIn, sender);
    }

    #endregion ChkguAvailAvailable_Click

    #region ChkguInfoAvailable_Click

    private void ChkguInfoAvailable_Click(object sender, RoutedEventArgs e)
    {
      var Available =
        dgGuestAvailable.Items.GetItemAt(dgGuestAvailable.Items.IndexOf(dgGuestAvailable.CurrentItem)) as GuestAvailable;
      OpenInfo(dgGuestAvailable, Available.guCheckIn, Available.guInfo, Available.guCheckOutD, Available.guID, sender);
    }

    #endregion ChkguInfoAvailable_Click

    #region ChkFollowAvailable_Click

    private void ChkFollowAvailable_Click(object sender, RoutedEventArgs e)
    {
      //StaStart("Loading Follow Up screen...");
      var Available =
        dgGuestAvailable.Items.GetItemAt(dgGuestAvailable.Items.IndexOf(dgGuestAvailable.CurrentItem)) as GuestAvailable;
      OpenFollow(dgGuestAvailable, Available.guCheckIn, Available.guInfo, Available.guFollow, Available.guAvail, Available.guInvit, Available.guCheckOutD, Available.guID, sender);
    }

    #endregion ChkFollowAvailable_Click

    #region NotesAvailable_MouseLeftButtonUp

    private void NotesAvailable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (dgGuestAvailable.SelectedItem != null)
      {
        var Arrival = dgGuestAvailable.SelectedItem as GuestAvailable;
        frmPRNotes prnote = new frmPRNotes(Arrival.guID);
        prnote.Owner = this;
        prnote.ShowDialog();
        if (prnote.SaveNote)
        {
          dgGuestAvailable.SelectedItems.OfType<GuestAvailable>().ToList().ForEach(item => item.guPRNote = true);
          dgGuestAvailable.Items.Refresh();
        }
      }
    }

    #endregion NotesAvailable_MouseLeftButtonUp

    #region ChkBookCancAvailable_Click

    private void ChkBookCancAvailable_Click(object sender, RoutedEventArgs e)
    {
      var Available = dgGuestAvailable.Items[dgGuestAvailable.Items.CurrentPosition] as GuestAvailable;
      var chk = sender as CheckBox;
      chk.IsChecked = !chk.IsChecked.Value;
      if (ValidateCancelInvitation(Available.guCheckIn, Available.guCheckOutD, Available.guInvit, Available.guShow))
      {
        var log = ValidateLogin();
        if (log != null)
        {
          StaStart("loading Cancel invitation screen...");
          frmBookingCancel bc = new frmBookingCancel(Available.guID, log.UserData.User);
          bc.Owner = this;
          bc.ShowInTaskbar = false;
          StaEnd();
          if (!bc.ShowDialog().Value)
          {
            dgGuestAvailable.SelectedItems.OfType<GuestAvailable>()
              .ToList()
              .ForEach(item => item.guBookCanc = bc.Cancelado.Value);
            dgGuestAvailable.Items.Refresh();
          }
        }
      }
    }

    #endregion ChkBookCancAvailable_Click

    #region chkGuestsGroupsAviables

    private async void chkGuestsGroupsAvailables_Click(object sender, RoutedEventArgs e)
    {
      bool show = true;
      bool group = false;
      frmGuestsGroups frmGgGu;
      var chk = sender as CheckBox;
      int guID, guIDToAdd = 0;
      GuestsGroup gg = new GuestsGroup();
      EnumAction action;

      GuestAvailable itema =
        dgGuestAvailable.Items.GetItemAt(dgGuestAvailable.Items.IndexOf(dgGuestAvailable.CurrentItem)) as GuestAvailable;

      action = EnumAction.None;
      guID = itema.guID;
      group = itema.guGroup;
      if (itema.guGroup) //Si tiene Group
      {
        gg = await BRGuestsGroupsIntegrants.GetGuestGroupByGuest(guID);
        guIDToAdd = 0;
      } //Si no tiene Grupo
      else
      {
        guIDToAdd = guID;
        //impedimos modificar los datos si el sistema esta en modo de solo lectura
        if (App.Current.Properties.IsReadOnly)
        {
          UIHelper.ShowMessage("The system is in read-only mode");
          show = false;
        } //Si no esta en modo ReadOnly
        else
        {
          MessageBoxCustomHelper.CustomMeesageBox("Create group", "Add to group", "Cancel");
          MessageBoxResult res =
            MessageBox.Show(
              "This guest does not belong to a group \nDo you want to create a new group \nor want to add the guest to an existing group?",
              "Add Guest", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

          switch (res)
          {
            case MessageBoxResult.Yes: //Crea Nuevo
              action = EnumAction.Add;
              break;

            case MessageBoxResult.No: //Agrega A
              action = EnumAction.AddTo;
              break;

            case MessageBoxResult.Cancel: //Cancela
              show = false;
              chk.IsChecked = false;
              break;
          }
          MessageBoxCustomHelper.ReturnToCommonMessageBox();
        }
      }
      if (show) //Si no se cancelo en nungun momento se muestra el formulario
      {
        frmGgGu = new frmGuestsGroups(gg.gxID, guID, guIDToAdd, dtpDate.Value.Value.Date, action);
        frmGgGu.Owner = this;
        frmGgGu.ShowDialog();
        Guest gu = await BRGuests.GetGuest(guID);
        group = gu.guGroup;
        LoadGrid();
      }
    }

    #endregion chkGuestsGroupsAviables

    #region AviablesEquity_MouseLeftButtonUp

    private void AviablesEquity_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var guest = dgGuestAvailable.Items[dgGuestAvailable.Items.CurrentPosition] as GuestAvailable;
      GetEquityReport(guest, 2);
    }

    #endregion AviablesEquity_MouseLeftButtonUp

    #endregion 2.- Availables

    #region 3.- Premanifest

    #region ChkguCheckInPremanifest_Click

    private async void ChkguCheckInPremanifest_Click(object sender, RoutedEventArgs e)
    {
      var chk = sender as CheckBox;
      chk.IsChecked = chk.IsChecked.Value ? await CheckIn(dgGuestPremanifest) : true;
    }

    #endregion ChkguCheckInPremanifest_Click

    #region ChkguAvailPremanifest_Click

    private void ChkguAvailPremanifest_Click(object sender, RoutedEventArgs e)
    {
      var Premanifest = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.IndexOf(dgGuestPremanifest.CurrentItem)) as GuestPremanifest;
      OpenAvail(dgGuestPremanifest, Premanifest.guID, Premanifest.guCheckIn, sender);
    }

    #endregion ChkguAvailPremanifest_Click

    #region ChkguInfoPremanifest_Click

    private void ChkguInfoPremanifest_Click(object sender, RoutedEventArgs e)
    {
      var Premanifest = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.IndexOf(dgGuestPremanifest.CurrentItem)) as GuestPremanifest;
      OpenInfo(dgGuestPremanifest, Premanifest.guCheckIn, Premanifest.guInfo, Premanifest.guCheckOutD, Premanifest.guID, sender);
    }

    #endregion ChkguInfoPremanifest_Click

    #region ChkguFollowPremanifest_Click

    private void ChkguFollowPremanifest_Click(object sender, RoutedEventArgs e)
    {
      var Premanifest = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.IndexOf(dgGuestPremanifest.CurrentItem)) as GuestPremanifest;
      OpenFollow(dgGuestPremanifest, Premanifest.guCheckIn, Premanifest.guInfo, Premanifest.guFollow, Premanifest.guAvail, Premanifest.guInvit, Premanifest.guCheckOutD, Premanifest.guID, sender);
    }

    #endregion ChkguFollowPremanifest_Click

    #region NotesPremanifest_MouseLeftButtonUp

    private void NotesPremanifest_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (dgGuestPremanifest.SelectedItem != null)
      {
        var Arrival = dgGuestPremanifest.SelectedItem as GuestPremanifest;
        frmPRNotes prnote = new frmPRNotes(Arrival.guID);
        prnote.Owner = this;
        prnote.ShowDialog();
        if (prnote.SaveNote)
        {
          dgGuestPremanifest.SelectedItems.OfType<ObjGuestPremanifest>().ToList().ForEach(item => item.guPRNote = true);
          dgGuestPremanifest.Items.Refresh();
        }
      }
    }

    #endregion NotesPremanifest_MouseLeftButtonUp

    #region ChkBookCancPremanifest_Click

    private void ChkBookCancPremanifest_Click(object sender, RoutedEventArgs e)
    {
      var Premanifest = dgGuestPremanifest.Items[dgGuestPremanifest.Items.CurrentPosition] as GuestPremanifest;
      var chk = sender as CheckBox;
      chk.IsChecked = !chk.IsChecked.Value;
      if (ValidateCancelInvitation(Premanifest.guCheckIn, Premanifest.guCheckOutD, Premanifest.guInvit, Premanifest.guShow))
      {
        StaStart("loading Cancel invitation screen...");
        var log = ValidateLogin();
        if (log != null)
        {
          frmBookingCancel bc = new frmBookingCancel(Premanifest.guID, log.UserData.User);
          bc.Owner = this;
          bc.ShowInTaskbar = false;
          bc.ShowDialog();
          dgGuestPremanifest.SelectedItems.OfType<GuestPremanifest>().ToList().ForEach(item => item.guBookCanc = bc.Cancelado.Value);
          dgGuestPremanifest.Items.Refresh();
        }
      }
    }

    #endregion ChkBookCancPremanifest_Click

    #region chkGuestsGroupsPremanifest

    private async void chkGuestsGroupsPremanifest_Click(object sender, RoutedEventArgs e)
    {
      bool show = true;
      bool group = false;
      frmGuestsGroups frmGgGu;
      var chk = sender as CheckBox;
      int guID, guIDToAdd = 0;
      GuestsGroup gg = new GuestsGroup();
      EnumAction action;

      GuestPremanifest itema =
        dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.IndexOf(dgGuestPremanifest.CurrentItem)) as
          GuestPremanifest;

      action = EnumAction.None;
      guID = itema.guID;
      group = itema.guGroup;
      if (itema.guGroup) //Si tiene Group
      {
        gg = await BRGuestsGroupsIntegrants.GetGuestGroupByGuest(guID);
        guIDToAdd = 0;
      } //Si no tiene Grupo
      else
      {
        guIDToAdd = guID;
        //impedimos modificar los datos si el sistema esta en modo de solo lectura
        if (App.Current.Properties.IsReadOnly)
        {
          UIHelper.ShowMessage("The system is in read-only mode");
          show = false;
        } //Si no esta en modo ReadOnly
        else
        {
          MessageBoxCustomHelper.CustomMeesageBox("Create group", "Add to group", "Cancel");
          MessageBoxResult res =
            MessageBox.Show(
              "This guest does not belong to a group \nDo you want to create a new group \nor want to add the guest to an existing group?",
              "Add Guest", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

          switch (res)
          {
            case MessageBoxResult.Yes: //Crea Nuevo
              action = EnumAction.Add;
              break;

            case MessageBoxResult.No: //Agrega A
              action = EnumAction.AddTo;
              break;

            case MessageBoxResult.Cancel: //Cancela
              show = false;
              chk.IsChecked = false;
              break;
          }
          MessageBoxCustomHelper.ReturnToCommonMessageBox();
        }
      }
      if (show) //Si no se cancelo en nungun momento se muestra el formulario
      {
        frmGgGu = new frmGuestsGroups(gg.gxID, guID, guIDToAdd, dtpDate.Value.Value, action);
        frmGgGu.Owner = this;
        frmGgGu.ShowDialog();
        Guest gu = await BRGuests.GetGuest(guID);
        group = gu.guGroup;
        LoadGrid();
      }
    }

    #endregion chkGuestsGroupsPremanifest

    #region PremanifestEquity_MouseLeftButtonUp

    private void PremanifestEquity_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var guest = dgGuestPremanifest.Items[dgGuestPremanifest.Items.CurrentPosition] as GuestPremanifest;
      GetEquityReport(guest, 3);
    }

    #endregion PremanifestEquity_MouseLeftButtonUp

    #endregion 3.- Premanifest

    #region 4.- GetGuest

    #region ChkguCheckInGetGuest_Click

    private async void ChkguCheckInGetGuest_Click(object sender, RoutedEventArgs e)
    {      
      var chk = sender as CheckBox;
      chk.IsChecked = chk.IsChecked.Value ? await CheckIn(guestSearchedDataGrid) : true;
    }

    #endregion ChkguCheckInGetGuest_Click

    #region ChkguAvailGetGuest_Click

    private void ChkguAvailGetGuest_Click(object sender, RoutedEventArgs e)
    {
      var Searched = guestSearchedDataGrid.Items.GetItemAt(guestSearchedDataGrid.Items.IndexOf(guestSearchedDataGrid.CurrentItem)) as GuestSearched;
      OpenAvail(guestSearchedDataGrid, Searched.guID, Searched.guCheckIn, sender);
    }

    #endregion ChkguAvailGetGuest_Click

    #region ChkguInfoGetGuest_Click

    private void ChkguInfoGetGuest_Click(object sender, RoutedEventArgs e)
    {
      var Searched = guestSearchedDataGrid.Items.GetItemAt(guestSearchedDataGrid.Items.IndexOf(guestSearchedDataGrid.CurrentItem)) as GuestSearched;
      OpenInfo(guestSearchedDataGrid, Searched.guCheckIn, Searched.guInfo, Searched.guCheckOutD, Searched.guID, sender);
    }

    #endregion ChkguInfoGetGuest_Click

    #region ChkguFollowGetGuest_Click

    private void ChkguFollowGetGuest_Click(object sender, RoutedEventArgs e)
    {
      var Searched = guestSearchedDataGrid.Items.GetItemAt(guestSearchedDataGrid.Items.IndexOf(guestSearchedDataGrid.CurrentItem)) as GuestSearched;
      OpenFollow(guestSearchedDataGrid, Searched.guCheckIn, Searched.guInfo, Searched.guFollow, Searched.guAvail, Searched.guInvit, Searched.guCheckOutD, Searched.guID, sender);
    }

    #endregion ChkguFollowGetGuest_Click

    #region NotesSearched_MouseLeftButtonUp

    private void NotesSearched_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (guestSearchedDataGrid.SelectedItem != null)
      {
        var Arrival = guestSearchedDataGrid.SelectedItem as GuestSearched;
        frmPRNotes prnote = new frmPRNotes(Arrival.guID);
        prnote.Owner = this;
        prnote.ShowDialog();
        if (prnote.SaveNote)
        {
          guestSearchedDataGrid.SelectedItems.OfType<ObjGuestSearched>().ToList().ForEach(item => item.guPRNote = true);
          guestSearchedDataGrid.Items.Refresh();
        }
      }
    }

    #endregion NotesSearched_MouseLeftButtonUp

    #region ChkBookCancSearched_Click

    private void ChkBookCancSearched_Click(object sender, RoutedEventArgs e)
    {
      var Searched = guestSearchedDataGrid.Items[guestSearchedDataGrid.Items.CurrentPosition] as GuestSearched;
      var chk = sender as CheckBox;
      chk.IsChecked = !chk.IsChecked.Value;
      if (ValidateCancelInvitation(Searched.guCheckIn, Searched.guCheckOutD, Searched.guInvit, Searched.guShow))
      {
        var log = ValidateLogin();
        if (log != null)
        {
          StaStart("loading Cancel invitation screen...");
          frmBookingCancel bc = new frmBookingCancel(Searched.guID, log.UserData.User);
          bc.Owner = this;
          bc.ShowInTaskbar = false;
          StaEnd();
          bc.ShowDialog();
          guestSearchedDataGrid.SelectedItems.OfType<GuestSearched>().ToList().ForEach(item => item.guBookCanc = bc.Cancelado.Value);
          guestSearchedDataGrid.Items.Refresh();
        }
      }
    }

    #endregion ChkBookCancSearched_Click

    #region GetGuestEquity_MouseLeftButtonUp

    private void GetGuestEquity_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var guest = guestSearchedDataGrid.Items[guestSearchedDataGrid.Items.CurrentPosition] as GuestSearched;
      GetEquityReport(guest, 4);
    }

    #endregion GetGuestEquity_MouseLeftButtonUp

    #endregion 4.- GetGuest

    #region chkGuestsGroupsSearched

    private async void chkGuestsGroupsGuestSearched_Click(object sender, RoutedEventArgs e)
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
      if (itema.guGroup) //Si tiene Group
      {
        gg = await BRGuestsGroupsIntegrants.GetGuestGroupByGuest(guID);
        guIDToAdd = 0;
      } //Si no tiene Grupo
      else
      {
        guIDToAdd = guID;
        //impedimos modificar los datos si el sistema esta en modo de solo lectura
        if (App.Current.Properties.IsReadOnly)
        {
          UIHelper.ShowMessage("The system is in read-only mode");
          show = false;
        } //Si no esta en modo ReadOnly
        else
        {
          MessageBoxCustomHelper.CustomMeesageBox("Create group", "Add to group", "Cancel");
          MessageBoxResult res =
            MessageBox.Show(
              "This guest does not belong to a group \nDo you want to create a new group \nor want to add the guest to an existing group?",
              "Add Guest", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

          switch (res)
          {
            case MessageBoxResult.Yes: //Crea Nuevo
              action = EnumAction.Add;
              break;

            case MessageBoxResult.No: //Agrega A
              action = EnumAction.AddTo;
              break;

            case MessageBoxResult.Cancel: //Cancela
              show = false;
              chk.IsChecked = false;
              break;
          }
          MessageBoxCustomHelper.ReturnToCommonMessageBox();
        }
      }
      if (show) //Si no se cancelo en nungun momento se muestra el formulario
      {
        frmGgGu = new frmGuestsGroups(gg.gxID, guID, guIDToAdd, dtpDate.Value.Value, action);
        frmGgGu.Owner = this;
        frmGgGu.ShowDialog();
        Guest gu = await BRGuests.GetGuest(guID);
        group = gu.guGroup;
        LoadGrid();
      }
    }

    #endregion chkGuestsGroupsSearched

    #region dg_SelectionChanged

    /// <summary>
    /// Actualiza el conteo de los registros del Data Grid
    /// </summary>
    /// <history>
    /// [jorcanche] created 14/07/2016
    /// </history>
    private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var dg = sender as DataGrid;
      if (dg != null) StatusBarReg.Content = $"{dg.Items.CurrentPosition + 1}/{dg.Items.Count}";
    }

    #endregion dg_SelectionChanged

    #region Comments_LostFocus

    private async void Comments_LostFocus(object sender, RoutedEventArgs e)
    {
      var txt = sender as TextBox;
      if (!string.IsNullOrEmpty(txt.Text))
      {
        Guest guest = null;
        if (txt.Name == "guCommentsColumnArrival")
        {
          var row = dgGuestArrival.SelectedItem as GuestArrival;
          guest = await BRGuests.GetGuest(row.guID);
        }
        if (txt.Name == "guCommentsColumnAvailable")
        {
          var row = dgGuestAvailable.SelectedItem as GuestAvailable;
          guest = await BRGuests.GetGuest(row.guID);
        }
        if (txt.Name == "guCommentsColumnPremanifest")
        {
          var row = dgGuestPremanifest.SelectedItem as GuestPremanifest;
          guest = await BRGuests.GetGuest(row.guID);
        }
        if (txt.Name == "guCommentsColumnSearched")
        {
          var row = guestSearchedDataGrid.SelectedItem as GuestSearched;
          guest = await BRGuests.GetGuest(row.guID);
        }
        guest.guComments = txt.Text;
        await BREntities.OperationEntity(guest,EnumMode.Edit);
      }
    }

    #endregion Comments_LostFocus

    #region CommentsColumn_Loaded

    private void CommentsColumn_Loaded(object sender, RoutedEventArgs e)
    {
      var txt = sender as TextBox;
      txt.Focus();
    }

    #endregion CommentsColumn_Loaded

    #region btnRefresh_Click

    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
      StaStart("Loading...");
      LoadGrid();
    }

    #endregion btnRefresh_Click

    #region btnLogin_Click

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [erosado] 01/06/2016  Modified. se agrego async
    /// </history>
    private async void btnLogin_Click(object sender, RoutedEventArgs e)
    {
      var log = new frmLogin(null, EnumLoginType.Location, program: EnumProgram.Inhouse, changePassword: false,
        autoSign: true, switchLoginUserMode: true);

      await log.getAllPlaces();
      if (App.User.AutoSign)
      {
        log.UserData = App.User;
      }
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
        App.User = log.UserData;
        Inhouse_Loaded();
      }
    }

    #endregion btnLogin_Click

    #region btnAbout_Click

    private void btnAbout_Click(object sender, RoutedEventArgs e)
    {
      frmAbout formAbout = new frmAbout();
      formAbout.Owner = this;
      formAbout.ShowDialog();
    }

    #endregion btnAbout_Click

    #region btnPrint_Click

    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      StaStart("Printing " + EnumToListHelper.GetEnumDescription(_screen) + "...");
      CreateExcelReport();
      StaEnd();
    }

    #endregion btnPrint_Click

    #region btnWithGifts_Click

    #region MyRegion

    /// <summary>
    /// Crea el reporte
    /// </summary>
    /// <history>
    /// [jorcanche] created 27/jul/2016
    /// </history>
    private void btnWithGifts_Click(object sender, RoutedEventArgs e)
    {
      CreateExcelReport(true);
    }

    #endregion MyRegion

    #region dtpDate_ValueChanged

    /// <summary>
    /// Actualiza la variable global _serverDate y carga los controles
    /// </summary>
    ///<history>
    /// [jorcanche] created 17/07/2016
    /// </history>
    private async void dtpDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (!IsInitialized) return;
      if (dtpDate.Text == string.Empty) dtpDate.Value = _serverDate;
      if (dtpDate.Value == null || _serverDate == dtpDate.Value.Value) return;
      StaStart($"Loading {_screen}...");
      _serverDate = dtpDate.Value.Value;
      txtOccupancy.Text = await BRLeadSources.GetOccupationLeadSources(dtpDate.Value.Value, App.User.Location.loID);
      LoadGrid();
    }

    #endregion dtpDate_ValueChanged

    private async void btnExtInvit_Click(object sender, RoutedEventArgs e)
    {
      //External Invitation
      var login = new frmLogin(loginType: EnumLoginType.Location, program: EnumProgram.Inhouse,
        validatePermission: true, permission: EnumPermission.PRInvitations, permissionLevel: EnumPermisionLevel.Standard,
        switchLoginUserMode: true, invitationMode: true, invitationPlaceId: App.User.Location.loID);

      if (App.User.AutoSign)
      {
        login.UserData = App.User;
      }
      await login.getAllPlaces();
      login.ShowDialog();

      if (login.IsAuthenticated)
      {
        var invitacion = new frmInvitation(EnumModule.InHouse, EnumInvitationType.newExternal, login.UserData)
        {
          Owner = this
        };
        invitacion.ShowDialog();
        
        //Si se guardó la información
        if (invitacion.SaveGuestInvitation)
        {
         
        }
      }
    }

    #endregion btnWithGifts_Click

    #endregion listMarkets_SelectionChanged

    #region btnArrivals_Clicked

    /// <summary>
    ///Evento que ocurre cuando se oprime el boton arrival y ejecuta las configuaraciones
    /// </summary>
    /// <history>[jorcanche] 22/03/2015</history>
    private void btnArrivals_Clicked(object sender, RoutedEventArgs e)
    {
      StaStart("Loading Arrival...");
      EnabledCtrls(true, true, true, true);
      _screen = EnumScreen.Arrivals; // DataGridVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);
      LoadGrid();
    }

    #endregion btnArrivals_Clicked

    #region btnAvailables_Clicked

    /// <summary>
    ///Evento que ocurre cuando se oprime el boton Availables y ejecuta las configuaraciones
    /// </summary>
    /// <history>[jorcanche] 22/03/2015</history>
    private void btnAvailables_Clicked(object sender, RoutedEventArgs e)
    {
      StaStart("Loading Available...");
      EnabledCtrls(false, false, true, true);
      _screen = EnumScreen.Availables;// DataGridVisibility(Visibility.Hidden, Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
      LoadGrid();
      //oculta el boton btnWitGifts que exporta el reporte Premanifest WithGifts
    }

    #endregion btnAvailables_Clicked

    #region btnPremanifiest_Click

    /// <summary>
    ///Evento que ocurre cuando se oprime el boton Premanifest y ejecuta las configuaraciones
    /// </summary>
    /// <history>[jorcanche] 22/03/2015</history>
    private void btnPremanifiest_Click(object sender, RoutedEventArgs e)
    {
      StaStart("Loading Premanifest...");
      EnabledCtrls(false, true, false, false);
      _screen = EnumScreen.Premanifest;// DataGridVisibility(Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
      LoadGrid();
      //Muestra el boton btnWitGifts que exporta el reporte Premanifest WithGifts
    }

    #endregion btnPremanifiest_Click

    #region btnGroups_Click

    private void btnGrouos_Click(object sender, RoutedEventArgs e)
    {
      frmGuestsGroups frmGroups = new frmGuestsGroups(0, 0, 0, dtpDate.Value.Value, EnumAction.Search);
      frmGroups.Owner = this;
      frmGroups.ShowDialog();
    }

    #endregion btnGroups_Click

    #region btnDaysOff_Click

    private void btnDaysOff_Click(object sender, RoutedEventArgs e)
    {
      frmDaysOff frmDaysOff = new frmDaysOff(EnumTeamType.TeamPRs, App.User);
      frmDaysOff.ShowDialog();
    }

    #endregion btnDaysOff_Click

    #region btnSearchGuest_Click

    /// <summary>
    /// Desplaza la informacion encontrada en el DataGrid
    /// </summary>
    /// <history>
    /// [jorcanche] 17/03/2016
    /// </history>
    private void btnSearchGuest_Click(object sender, RoutedEventArgs e)
    {
      StaStart("loading Searched...");
      //StaStart("loading Searched...");
      var searchGuests = new frmSearchGuests { Owner = this };
      //Validamos que se halla cerrado la ventana
      StaEnd();
      searchGuests.ShowDialog();
      //Validamos que le halla dado aceptar y no Cerrar ventana.
      if (!searchGuests._Cancel)
      {
        //Traemos los Datos
        _guestDateTo = searchGuests._dateTo;
        _guestdateFrom = searchGuests._dateFrom;
        _guestGuid = searchGuests._guestID;
        _guestName = searchGuests._name;
        _guestRoom = searchGuests._room;
        _guestReservation = searchGuests._reservation;
        //Manipulamos los controlos
        EnabledCtrls(false, false, false, false, false, false);
        _screen = EnumScreen.Search;
        StaStart("loading Searched...");
        LoadGrid();
      }
    }

    #endregion btnSearchGuest_Click

    #region btnAssistance_Click

    private void btnAssistance_Click(object sender, RoutedEventArgs e)
    {
      frmAssistance frmAssistance = new frmAssistance(EnumPlaceType.LeadSource, App.User);
      frmAssistance.Owner = this;
      frmAssistance.ShowDialog();
    }

    #endregion btnAssistance_Click

    #region ChkInvit_Click

    /// <summary>
    /// Sirve para realizar una invitacion, o abrirla en modo lectura
    /// </summary>
    /// <history>
    /// [jorcanche] 11/ago/2016 Modified. Se mejoro el Evento
    /// [erosado] 09/08/2016  Modified. Se habilito para que se inivie el nuevo formulario de invitacion.
    /// </history>
    private void ChkInvit_Click(object sender, RoutedEventArgs e)
    {
      var chk = sender as CheckBox;
      DataGrid dg;
      switch (_screen)
      {
        case EnumScreen.Arrivals:
          dg = dgGuestArrival;
          break;

        case EnumScreen.Availables:
          dg = dgGuestAvailable;
          break;

        case EnumScreen.Premanifest:
          dg = dgGuestPremanifest;
          break;

        case EnumScreen.Search:
          dg = guestSearchedDataGrid;
          break;

        default:
          throw new ArgumentOutOfRangeException();
      }
      var item = dg.SelectedItem;
      var t = item.GetType();
      var guAvail = t.GetProperty("guAvail").GetValue(item);
      var guInfo = t.GetProperty("guInfo").GetValue(item);
      var guInvit = t.GetProperty("guInvit").GetValue(item);
      var guCheckOutD = t.GetProperty("guCheckOutD").GetValue(item);
      var guId = t.GetProperty("guID").GetValue(item);
      var guCheckIn = t.GetProperty("guCheckIn").GetValue(item);
      var gagu = t.GetProperty("gagu").GetValue(item);

      //Validamos Valores nulos
      if (chk?.IsChecked == null) return;

      //Invertimos el valor del Check para que no se modifique. El formulario Invitación definira si hubo invitación o no
      chk.IsChecked = !chk.IsChecked.Value;

      if (ShowNotBookingMotive((bool)guAvail, (bool)guInfo, (bool)guInvit, (DateTime)guCheckOutD, (int)guId, dg))
      {
        //Despliega el formulario de Invitación
        ShowInvitation((bool)guCheckIn, (int?)gagu, (int)guId, chk.IsChecked.Value, dg);

      }
    }

    #endregion ChkInvit_Click

    #region ShowInvitation

    /// <summary>
    /// Despliega el formulario de invitacion
    /// </summary>
    /// <param name="guCheckIn">Check In del Huesped</param>
    /// <param name="gagu">Si es un Guest Adicional</param>
    /// <param name="guId">Id del Guest</param>
    /// <param name="isInvit">Si ya se invito el guest</param>
    /// <param name="dg">Datagrid que se actualizara</param>
    /// <history>
    /// [jorcanche] 16/ago/2016 Created
    /// </history>
    private async void ShowInvitation(bool guCheckIn, int? gagu, int guId, bool isInvit, DataGrid dg)
    {
      if (ValidateInvitation(guCheckIn, gagu))
      {
        frmLogin login = null;
        if (!isInvit)
        {
          login = new frmLogin(loginType: EnumLoginType.Location, program: EnumProgram.Inhouse, validatePermission: true, 
            permission: EnumPermission.PRInvitations, permissionLevel: EnumPermisionLevel.Standard, switchLoginUserMode: true, 
            invitationMode: true, invitationPlaceId: App.User.Location.loID);

          if (App.User.AutoSign)
          {
            login.UserData = App.User;
          }
          await login.getAllPlaces();
          login.ShowDialog();
        }
        else if (App.User.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.Standard) && !App.User.AutoSign)
        {
          login = new frmLogin(loginType: EnumLoginType.Location, program: EnumProgram.Inhouse, validatePermission: true,
            permission: EnumPermission.PRInvitations, permissionLevel: EnumPermisionLevel.Standard, switchLoginUserMode: true,
            invitationMode: true, invitationPlaceId: App.User.Location.loID);

          await login.getAllPlaces();
          login.ShowDialog();
        }        
        if (isInvit || login.IsAuthenticated)
        {
          var invitacion = new frmInvitation
            (EnumModule.InHouse, EnumInvitationType.existing, login != null ? login.UserData : App.User, guId) { Owner = this };
          invitacion.ShowDialog();
          //Si se guardó la información
          if (invitacion.SaveGuestInvitation)
          {
            //actualizamos los datos del grid
            //TODO:Revisar este metodo JORGE CANCHE
            UpdateGridInvitation(invitacion.CatObj.Guest, invitacion._module, dg);
          }
        }
       
      }
    }

    #endregion ShowInvitation

    #region UpdateGridInvitation

    /// <summary>
    /// Actualiza el datagrid de OutHouse
    /// </summary>
    /// <param name="invitacion">Formulario de Incitación</param>
    /// <param name="module">En que modulo se esta invocando</param>
    /// <param name="dg">DataGrid Current</param>
    private void UpdateGridInvitation(Guest invitacion, EnumModule module, DataGrid dg)
    {
      var item = dg.SelectedItem;
      var t = item.GetType();
      var  lstProperties = t.GetProperties().ToList();

      //***********************************************Disponible ***********************************************
      if (lstProperties.Any(c => c.Name == "guInvit"))
      {
        t.GetProperty("guInvit").SetValue(item, true);
      }

      //***********************************************Seguimiento*********************************************** 
      if (lstProperties.Any(c => c.Name == "guFollow"))
      {
        //Si estaba contactado y no como invitado 
        if ((bool) t.GetProperty("guInfo").GetValue(item) &&
            (bool) t.GetProperty("guInvit").GetValue(item) == false)
        {
          //Con seguimiento 
          t.GetProperty("guFollow").SetValue(item, true);

          //PR y Fecha de seguimiento
          if (string.IsNullOrEmpty(t.GetProperty("guPRFollow").GetValue(item)?.ToString()))
          {
            t.GetProperty("guPRFollow").SetValue(item, invitacion.guPRInvit1);
            t.GetProperty("guFollowD").SetValue(item, invitacion.guInvitD);
          }
        }
      }
      //***********************************************Contactacion ***********************************************
      if (lstProperties.Any(c => c.Name == "guInfo"))
      {        
        //Contactado
        t.GetProperty("guInfo").SetValue(item, true);

        //PR y fecha de contactacion
        if (string.IsNullOrEmpty(t.GetProperty("guPRInfo").GetValue(item)?.ToString()))
        {
          t.GetProperty("guPRInfo").SetValue(item, invitacion.guPRInvit1);
          t.GetProperty("guInfoD").SetValue(item, invitacion.guInvitD);
        }
      }

      //***********************************************Invitacion ***********************************************
      //Invitado
      t.GetProperty("guInvit").SetValue(item, true);

      //PR de Invitación
      if (lstProperties.Any(c => c.Name == "guPRInvit1"))
      {
        t.GetProperty("guPRInvit1").SetValue(item, invitacion.guPRInvit1);
      }

      //Invitacion no cancelada
      if (lstProperties.Any(c => c.Name == "guBookCanc"))
      {
        t.GetProperty("guBookCanc").SetValue(item, false);
      }

      //Fecha de no booking
      if (lstProperties.Any(c => c.Name == "guBookD"))
      {
        t.GetProperty("guBookD").SetValue(item, invitacion.guBookD);
      }

      //Hora de booking
      if (lstProperties.Any(c => c.Name == "guBookT"))
      {
        if (module != EnumModule.OutHouse)
        {
          if (invitacion.guResch)
          {
            if (invitacion.guReschD == invitacion.guReschT)
            {
              t.GetProperty("guBookT").SetValue(item, invitacion.guReschT);
            }
          }
          else
          {
            t.GetProperty("guBookT").SetValue(item, invitacion.guBookT);
          }
        }
        else
        {
          t.GetProperty("guBookT").SetValue(item, invitacion.guBookT);
        }
      }
      //Quiniela
      if (lstProperties.Any(c => c.Name == "guQuinella"))
      {
        t.GetProperty("guQuinella").SetValue(item, invitacion.guQuinella);
      }

      dg.Items.Refresh();      
    }

    #endregion

    #region ValidateInvitation

    /// <summary>
    /// Valida los datos para desplegar el formulario de invitacion
    /// </summary>
    /// <param name="guCheckIn">Si ya hizo Check In el Huesped </param>
    /// <param name="gagu">si es un Huesped adicional</param>
    /// <history>
    /// [jorcanche] 16/ago/2016 Created
    /// </history>
    private bool ValidateInvitation(bool guCheckIn, int? gagu)
    {
      //Validamos que el huesped haya hecho Check In
      if (!guCheckIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-In.");
        return false;
      }
      //Validamos que no sea un huesped adicional
      if (gagu != null)
      {
        UIHelper.ShowMessage("An additional guest can not have invitation.");
        return false;
      }

      //Validamos que tenga permiso de lectura de invitaciones
      if (App.User.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.ReadOnly)) return true;
      UIHelper.ShowMessage("Access denied.");
      return false;
    }

    #endregion ValidateInvitation

    #region ShowNotBookingMotive

    /// <summary>
    /// Despliega el formulario que permite cambiar el motivo de no booking de un huesped
    /// </summary>
    /// <param name="guAvail">Si esta disponible</param>
    /// <param name="guInfo">Si ya esta contactado</param>
    /// <param name="guInvit">Si ya esta invitado</param>
    /// <param name="guCheckOutD">Si ya se fue del hotel</param>
    /// <param name="guId">Id del Guest</param>
    /// <param name="dg">DataGrid del que se esta modificando</param>
    /// <history>
    /// [JORCANCHE] created 16/08/2016
    /// </history>
    private bool ShowNotBookingMotive(bool guAvail, bool guInfo, bool guInvit, DateTime guCheckOutD, int guId, DataGrid dg)
    {
      //Variable que servira para indicar si se abrira el formulario de Invitacion o Solo se guardare los motivos de no booking
      var showInvitation = true;

      //Si los datos son validos
      if (ValidateNotBookingMotive(guAvail, guInfo, guInvit, guCheckOutD))
      {
        showInvitation = false;

        //desplegamos el formulario
        var frmNotBookingMotive = new frmNotBookingMotive(guId) { Owner = this };
        frmNotBookingMotive.ShowDialog();

        //Si no se guardo y tampoco se Invito quiere decir que se cerro solo así el formulario
        if (frmNotBookingMotive.Save || frmNotBookingMotive.Invit)
        {
          //Si se desea invitar al huesped y no guardo al huesped
          if (frmNotBookingMotive.Invit)
          {
            showInvitation = true;
          }
          //Si no desea invitar al huesped y guardo los cambios
          else
          {
            //Si guardo los cambios, entonces actualizamos el DataGrid
            //select guInvit from Guests where guID = 7755761
            var item = dg.SelectedItem;
            var t = item.GetType();
            //Fecha y PR de no booking
            t.GetProperty("guNoBookD").SetValue(item, Convert.ToDateTime(frmNotBookingMotive.txtguNoBookD.Text));
            t.GetProperty("guPRNoBook").SetValue(item, frmNotBookingMotive.cbmguPRNoBook.SelectedValue);
            //Motivo de no booking
            t.GetProperty("gunb").SetValue(item, frmNotBookingMotive.cbmgunb.SelectedValue);

            dg.Items.Refresh();
            GridHelper.SelectRow(dg, dg.SelectedIndex, dg.CurrentCell.Column.DisplayIndex);
          }
        }
      }
      return showInvitation;
    }

    #endregion ShowNotBookingMotive

    #region ValidateNotBookingMotive

    /// <summary>
    /// Valida si se debe de abrir el formulario NotBookingMotive
    /// </summary>
    /// <param name="guAvail">Si esta disponible</param>
    /// <param name="guInfo">Si ya esta contactado</param>
    /// <param name="guInvit">Si ya esta invitado</param>
    /// <param name="guCheckOutD">Si ya se fue del hotel</param>
    /// <history>
    /// [JORCANCHE] created 16/08/2016
    /// </history>
    private bool ValidateNotBookingMotive(bool guAvail, bool guInfo, bool guInvit, DateTime guCheckOutD)
    {
      //validamos que el huesped este disponible
      if (!guAvail) return false;
      //validamos que el huesped este contactado
      if (!guInfo) return false;
      //validamos que el huesped no este invitado
      if (guInvit) return false;
      //validamos que el huesped haya hecho Check Out
      if (guCheckOutD > _serverDate) return false;
      return true;
    }

    #endregion ValidateNotBookingMotive

    #region Window_KeyDown

    /// <summary>
    /// Actualiza el status bar
    /// </summary>
    ///<hystory>
    /// [jorcanche] created 14/07/2016
    /// </hystory>
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

    #endregion Window_KeyDown

    #region FrmInhouse_OnClosing

    /// <summary>
    /// Cierra todas las ventanas
    /// </summary>
    ///<history>
    /// [jorcanche] created 14/07/2016
    /// </history>
    private void FrmInhouse_OnClosing(object sender, CancelEventArgs e)
    {
      Application.Current.Shutdown();
    }

    #endregion FrmInhouse_OnClosing

    #endregion Eventos del formulario
  }
}
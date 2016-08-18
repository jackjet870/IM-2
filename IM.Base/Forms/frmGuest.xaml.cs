using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmGuest.xaml
  /// </summary>
  /// <history>
  /// [lchairez] 09/Feb/2016 Created
  /// </history>
  public partial class frmGuest : Window
  {
    #region Atributos

    private UserData _user;
    private readonly int _guestId;
    private bool _isReadOnly;
    private EnumModule _module;

    //private bool _isInvitation;
    private bool _wasSelectedByKeyboard;

    private DataGridCellInfo _IGCurrentCell;//Celda que se esta modificando
    private bool _hasError; //Sirve para las validaciones True hubo Error | False NO
    private bool _isCellCancel;//Sirve para cuando se cancela la edicion de una Celda
    private bool _dontShowAgainGuestStatus;
    public Guest NewGuest => ((GuestInvitationRules)DataContext).Guest;

    #region Objetos

    private Invitation _invitation;

    #endregion Objetos

    #endregion Atributos

    #region Constructores y destructores

    public frmGuest(UserData user, int guestId, bool isInvitation, EnumModule module, bool isReadOnly = false)
    {
      WindowStartupLocation = WindowStartupLocation.CenterScreen;
      _user = user;
      _guestId = guestId;
      //_isInvitation = isInvitation;
      _isReadOnly = isReadOnly;
      _module = module;
      InitializeComponent();
    }

    #endregion Constructores y destructores

    #region Métodos de la forma

    /// <summary>
    /// Configura los elementos de la forma
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void frmGuest_Loaded(object sender, RoutedEventArgs e)
    {
      //Iniciamos el BusyIndicator
      _busyIndicator.IsBusy = true;
      _busyIndicator.BusyContent = "Please wait, we are preparing the Guest form...";
      var catObj = new GuestInvitationRules(_module,_user, _guestId);
      await catObj.LoadAll();
      DataContext = catObj;
      ConfigurationControls();
      LoadControls();
      UIHelper.SetUpControls(new Guest(), this);
      //Detenemos el BusyIndicator
      _busyIndicator.IsBusy = false;
    }

    /// <summary>
    /// Asigna la clave de la agencia al TextBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void cmbAgency_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!_wasSelectedByKeyboard)
      {
        //txtAgency.Text = cmbAgency.SelectedValue.ToString();
      }
      _wasSelectedByKeyboard = false;
    }

    /// <summary>
    /// Permite solo Agregar un Registro al Grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dtgGuestStatus_LoadingRow(object sender, DataGridRowEventArgs e)
    {
      //System.ComponentModel.IEditableCollectionView itemView = dtgGuestStatus.Items;
      //if (_lstObjInvitGuestStatus.Count == 1 && itemView.IsAddingNew)
      //{
      //  itemView.CommitNew();
      //  dtgGuestStatus.CanUserAddRows = false;
      //}
    }

    /// <summary>
    /// Revisa los datos ingresados la grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dtgGuestStatus_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      //switch (e.Column.SortMemberPath)
      //{
      //  case "gtgs":
      //    var gtgs = e.EditingElement as ComboBox;
      //    if (gtgs.SelectedIndex != -1)
      //    {
      //      _lstObjInvitGuestStatus[e.Row.GetIndex()].gtgs = gtgs.SelectedValue.ToString();
      //      _lstObjInvitGuestStatus[e.Row.GetIndex()].gtQuantity = Convert.ToByte(1);
      //      CalculateMaxAuthGifts();
      //    }
      //    break;
      //}
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (!Validate()) return;
      SaveGuestInformation();
      Close();
    }

    /// <summary>
    /// Evento del Combobox GuestStatus
    /// Sirve para actualizar la caja de texto txtGiftMaxAuth
    /// dependiendo del GuestStatus que elija el usuario.
    /// </summary>
    ///<history>
    ///[edgrodriguez]  11/08/2016  Created.
    /// </history>
    private void cmbGuestStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //Obtenemos el GuestStatusType del combobox cmbGuestStatus
      var guStatusType = cmbGuestStatus.SelectedItem as GuestStatusType;

      txtGiftMaxAuth.Text = $"{guStatusType?.gsMaxAuthGifts ?? 0:C2}";

      //TODO: GUESTSTATUSTYPES Revizar el caso cuando se traigan los regalos de la Base de datos
      //GuestStatus _guestsStatus = BRGuestStatus.GetGuestsStatus(_guestID);
      //GuestStatusType _guestStatusType = BRGuestStatusTypes.GetGuestStatusTypeByID(_guestsStatus.gtgs);
      //curMaxAuthGifts = _guestsStatus.gtQuantity * _guestStatusType.gsMaxAuthGifts;
    }

    #region Métodos Genéricos

    /// <summary>
    /// Revisa que los textbox númericos solo resivan números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private new void PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
      if (e.Text == ".")
        e.Handled = false;
      else if (!char.IsDigit(e.Text, e.Text.Length - 1))
        e.Handled = true;
    }

    #endregion Métodos Genéricos

    #region Eventos del GRID Invitation Gift

    #region BeginningEdit

    /// <summary>
    /// Se ejecuta antes de que entre en modo edicion alguna celda
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/08/2016  Created.
    /// </history>
    private void dtgGifts_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      //Preguntamos si desea agregar GuestStatusType para el calculo de costos
      if (cmbGuestStatus.SelectedValue == null && !_dontShowAgainGuestStatus)
      {
        MessageBoxResult result = UIHelper.ShowMessage("We recommend first select the status of the guest, that would help us calculate costs and prices, do you want to select it now?", MessageBoxImage.Question, "Intelligence Marketing");
        if (result == MessageBoxResult.Yes)
        {
          e.Cancel = true;
          _hasError = true;
          _isCellCancel = true;
          _dontShowAgainGuestStatus = false;
          cmbGuestStatus.Focus();
        }
        else
        {
          _dontShowAgainGuestStatus = true;
        }
      }
      else
      {
        _hasError = false;
        _isCellCancel = false;
      }

      //Si el grid no esta en modo edicion, permite hacer edicion.
      if (!GridHelper.IsInEditMode(dtgGifts) && !_hasError)
      {
        dtgGifts.BeginningEdit -= dtgGifts_BeginningEdit;
        //Obtenemos el objeto de la fila que se va a editar
        InvitationGift invitationGift = e.Row.Item as InvitationGift;
        //Obtenemos la celda que vamos a validar
        _IGCurrentCell = dtgGifts.CurrentCell;
        //Hacemos la primera validacion
        InvitationValidationRules.StartEdit(ref invitationGift, ref _IGCurrentCell, dtgGifts, ref _hasError);
        //Si tuvo algun error de validacion cancela la edicion de la celda.
        e.Cancel = _hasError;
        dtgGifts.BeginningEdit += dtgGifts_BeginningEdit;
      }
      //Si ya se encuenta en modo EDIT cancela la edicion, para no salirse de la celda sin hacer Commit antes
      else
      {
        e.Cancel = true;
      }
    }

    #endregion BeginningEdit

    #region PreparingCellForEdit

    /// <summary>
    /// Se ejecuta cuando la celda entra en modo edicion
    /// </summary>
    /// <history>
    /// [erosado] 08/08/2016  Created.
    /// </history>
    private void dtgGifts_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
      //Sirve para agregar el Focus a las celdas
      Control ctrl = e.EditingElement as Control;
      ctrl.Focus();
    }

    #endregion PreparingCellForEdit

    #region CellEditEnding

    /// <summary>
    /// Se ejecuta cuando la celda en edicion pierde el foco
    /// </summary>
    /// <history>
    /// [erosado] 08/08/2016  Created.
    /// </history>
    private async void dtgGifts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      //Si paso las validaciones del preparingCellForEdit
      if (!_hasError)
      {
        //Si viene en modo Commit
        if (e.EditAction == DataGridEditAction.Commit)
        {
          //esta bandera se pone en falso por que No se ha cancelado la edicion de la celda
          _isCellCancel = false;
          //Obtenemos el Objeto
          InvitationGift invitationGift = e.Row.Item as InvitationGift;

          //Bandera que checata que todo salga bien en la validacion siguiente.
          bool _hasErrorValidateEdit = false;
          //Validamos la celda
          // InvitationValidationRules.ValidateEdit(ref invitationGift, ref _hasErrorValidateEdit, ref _IGCurrentCell);

          //Si Paso las validaciones
          if (!_hasErrorValidateEdit)
          {
            //Obtenemos el program
            var program = await BRLeadSources.GetLeadSourceProgram(_user.LeadSource.lsID);

            InvitationValidationRules.AfterEdit(dtgGifts, ref invitationGift, _IGCurrentCell, ref txtGiftTotalCost, ref txtGiftTotalPrice, ref txtGiftMaxAuth, cmbGuestStatus.SelectedItem as GuestStatusType, program);
          }
          //Si fallaron las validaciones del AfterEdit se cancela la edicion de la celda.
          else
          {
            e.Cancel = true;
          }
        }
        //Si entra en modo Cancel Se enciende esta bandera ya que servira en RowEditEnding
        else
        {
          _isCellCancel = true;
        }
      }
    }

    #endregion CellEditEnding

    #region RowEditEnding

    /// <summary>
    /// Se ejecuta cuando la fila pierde el foco, o termina la edicion (Commit o Cancel)
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/08/2016  Created.
    /// </history>
    private void dtgGifts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)

    {
      DataGrid dtg = sender as DataGrid;
      InvitationGift invitationGift = e.Row.Item as InvitationGift;

      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCancel)
        {
          dtg.RowEditEnding -= dtgGifts_RowEditEnding;
          dtg.CancelEdit();
          dtg.RowEditEnding -= dtgGifts_RowEditEnding;
        }
        else
        {
          if (invitationGift.igQty != 0 && !string.IsNullOrEmpty(invitationGift.iggi)) return;
          UIHelper.ShowMessage("Please enter the required fields Qty and Gift to continue", MessageBoxImage.Exclamation, "Intelligence Marketing");
          e.Cancel = true;
        }
      }
    }

    #endregion RowEditEnding

    #endregion Eventos del GRID Invitation Gift

    #endregion Métodos de la forma

    #region Métodos privados

    #region Configuración de controles

    private async void ConfigurationControls()
    {
      btnSave.IsEnabled = !_isReadOnly;
      dtgGifts.IsReadOnly = _isReadOnly;
      grbGuestInfo.IsEnabled =
        grbOtherInformation.IsEnabled =
          grbGuest1.IsEnabled =
            grbGuest2.IsEnabled =
              grbGuestStatus.IsEnabled = !_isReadOnly;

      dtpArrival.IsEnabled =
        dtpDeparture.IsEnabled = (await BRLeadSources.GetLeadSourceProgram(txtguls.Text)) == "IH";
    }

    #endregion Configuración de controles

    #region Cargar datos del invitado

    /// <summary>
    /// Manda llamar los métodos para cargar la información del guest.
    /// </summary>
    private void LoadControls()
    {
      LoadCommonControls(); //Se cargan los controles que son comunes en todos los tipos de invitacion.
    }

    /// <summary>
    /// Carga los controles comunes de todos los tipos de invitación
    /// </summary>
    /// <history>
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private void LoadCommonControls()
    {
      #region User

      txtUser.Text = _user.User.peID;
      txtPassword.Password = _user.User.pePwd;

      #endregion User
    }

    #endregion Cargar datos del invitado

    #region Métodos para calcular los costos de los regalos

    /// <summary>
    /// Calcula los costos y precios de adultos y menores de un regalo
    /// </summary>
    /// <param name="useCxCCost">Indica si se utilizará el costo del empleado</param>
    private void CalculateCostsPrices(bool useCxCCost = false)
    {
      //decimal costAdult, costMinor, priceAdult, priceMinor, priceExtraAdult, quantity;

      //foreach (var row in _lstObjInvitGift)
      //{
      //  var gift = IM.BusinessRules.BR.BRGifts.GetGiftId(row.iggi);
      //  if (gift != null)
      //  {
      //    // Costos
      //    // si se va a usar el costo de empleado
      //    if (useCxCCost)
      //    {
      //      costAdult = gift.giPrice1;
      //      costMinor = gift.giPrice4;
      //    }
      //    else // se va a usar el cosrto al público
      //    {
      //      costAdult = gift.giPrice1;
      //      costMinor = gift.giPrice2;
      //    }

      //    // Precios
      //    priceAdult = gift.giPublicPrice;
      //    priceMinor = gift.giPriceMinor;
      //    priceExtraAdult = gift.giPriceExtraAdult;
      //    quantity = row.igQty;

      //    // Total del costo adultos
      //    row.igPriceA = quantity * (row.igAdults + row.igExtraAdults) * costAdult;
      //    // Total del costo de menores
      //    row.igPriceM = quantity * row.igMinors * costMinor;
      //    // Total del precio adultos
      //    row.igPriceAdult = quantity * row.igAdults * priceAdult;
      //    //Total del precio de menores
      //    row.igPriceMinor = quantity * row.igMinors * priceMinor;
      //    // Total del precio de adultos extra
      //    row.igPriceExtraAdult = quantity * row.igExtraAdults * priceExtraAdult;
      //  }
      //}
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="onlyCancellled"></param>
    /// <param name="cancel"></param>
    private void CalculateTotalGifts(bool onlyCancellled = false, string cancel = "")
    {
      //decimal cost, price, totalCost = 0, totalPrice = 0;

      //foreach (var row in _lstObjInvitGift)
      //{
      //  // calculamos el costo del regalo
      //  cost = row.igPriceA + row.igPriceA;

      //  //calculamos el precio del regalo
      //  price = row.igPriceAdult + row.igPriceMinor + row.igPriceExtraAdult;

      //  //si se desean todos los regalos
      //  if (!onlyCancellled)
      //  {
      //    totalCost += cost;
      //    totalPrice += price;
      //  }

      //}
      //txtTotalCost.Text = totalCost.ToString("$#,##0.00;$(#,##0.00)");
      //txtTotalPrice.Text = totalPrice.ToString("$#,##0.00;$(#,##0.00)");
    }

    /// <summary>
    /// Calcula el monto maximo de regalos
    /// </summary>
    /// <history>
    /// [emoguel] modified se volvió async
    /// </history>
    private async void CalculateMaxAuthGifts()
    {
      //decimal maxAuthGifts = 0;

      //foreach (var row in _lstObjInvitGuestStatus)
      //{
      //  if (row.gtgs == null || row.gtQuantity <= 0) continue;
      //  var guestStatusType = await BRGuestStatusTypes.GetGuestStatusTypes(new Model.GuestStatusType { gsID = row.gtgs });
      //  var guestStaType=guestStatusType.FirstOrDefault();
      //  maxAuthGifts += row.gtQuantity * guestStaType.gsMaxAuthGifts;
      //}

      //txtMaxAuth.Text = maxAuthGifts.ToString("$#,##0.00;$(#,##0.00)");
    }

    #endregion Métodos para calcular los costos de los regalos

    #region Métodos para guardar la información del invitado

    /// <summary>
    /// Guarda la informaciónde la forma en la base de datos
    /// </summary>
    private void SaveGuestInformation()
    {
      _invitation = new Invitation();
      var guest = NewGuest;//IM.BusinessRules.BR.BRGuests.GetGuestById(_guestId);

      //#region Other Information
      //guest.guRoomNum = ForStringValue(txtRoom.Text);
      //guest.guag = ForStringValue(txtAgency.Text);
      //guest.guPax = ForDecimalValue(txtPax.Text);
      //guest.guCheckInD = txtArrival.SelectedDate.HasValue ? txtArrival.SelectedDate.Value : guest.guCheckInD;
      //guest.guCheckOutD = txtDeparture.SelectedDate.HasValue ? txtDeparture.SelectedDate.Value : guest.guCheckOutD;

      //#endregion

      //#region Guest 1
      //guest.guLastName1 = ForStringValue(txtLastNameGuest1.Text);
      //guest.guFirstName1 = ForStringValue(txtFirstNameGuest1.Text);
      //guest.guAge1 = string.IsNullOrEmpty(txtAgeGuest1.Text) ? (byte?)null : Convert.ToByte(txtAgeGuest1.Text);
      //guest.gums1 = ForStringValue(cmbMaritalStatusGuest1.SelectedValue);
      //guest.guOccup1 = ForStringValue(txtOcuppationGuest1.Text);
      //guest.guEmail1 = ForStringValue(txtEmailGuest1.Text);
      //#endregion

      //#region Guest 2
      //guest.guLastname2 = ForStringValue(txtLastNameGuest2.Text);
      //guest.guFirstName2 = ForStringValue(txtFirstNameGuest2.Text);
      //guest.guAge2 = string.IsNullOrEmpty(txtAgeGuest2.Text) ? (byte?)null : Convert.ToByte(txtAgeGuest2.Text);
      //guest.gums2 = ForStringValue(cmbMaritalStatusGuest2.SelectedValue);
      //guest.guOccup2 = ForStringValue(txtOcuppationGuest2.Text);
      //guest.guEmail2 = ForStringValue(txtEmailGuest2.Text);
      //#endregion

      //#region Guest Status
      //guest.guGStatus = ForStringValue(txtGuestStatus.Text);
      //SaveGuestStatus();
      //#endregion

      #region Gifts
      SaveGifts();

      #endregion

      _invitation.Guest = guest;

      //BRGuests.SaveGuestInvitation(invitation);
    }

    /// <summary>
    /// Guarda todos los regalos asignados
    /// </summary>
    private void SaveGifts()
    {
      _invitation.NewGifts = new List<Model.InvitationGift>();
      _invitation.UpdatedGifts = new List<Model.InvitationGift>();
      _invitation.DeletedGifts = new List<Model.InvitationGift>();


      if (!((GuestInvitation)DataContext).InvitationGiftList.Any()) return;

      //Convertimos la lista a un objeto de la capa Model
      var gifts = ((GuestInvitation)DataContext).InvitationGiftList;
        //  _lstObjInvitGift.Select(c => new InvitationGift
      //{
      //  igAdults = c.igAdults,
      //  igComments = c.igComments,
      //  igct = c.igct,
      //  igExtraAdults = c.igExtraAdults,
      //  igFolios = c.igFolios,
      //  iggi = c.iggi,
      //  iggr = c.iggr,
      //  iggu = c.iggu,
      //  igMinors = c.igMinors,
      //  igPriceA = c.igPriceA,
      //  igPriceAdult = c.igPriceAdult,
      //  igPriceExtraAdult = c.igPriceExtraAdult,
      //  igPriceM = c.igPriceM,
      //  igPriceMinor = c.igPriceMinor,
      //  igQty = c.igQty
      //}).ToList();

      //Obtenemos los regalos que se modificarán
      _invitation.UpdatedGifts = gifts.Where(c => c.iggu != 0).ToList();

      //Obtenemos los regalos nuevos para asignales los precios y el invitado
      var newGifts = gifts.Where(c => c.iggu == 0).ToList();
      if (!newGifts.Any()) return;

      //Asignamos el guest a los nuevos regalos
      newGifts.ForEach(c =>
      {
        c.iggu = c.iggu != 0 ? c.iggu : _guestId;
        c.igct = !string.IsNullOrEmpty(c.igct) ? c.igct : "MARKETING";
      });
      _invitation.NewGifts.AddRange(newGifts);
    }

    /// <summary>
    /// Guarda los estados del invitado
    /// </summary>
    private void SaveGuestStatus()
    {
      _invitation.NewGuestStatus = new List<Model.GuestStatus>();
      _invitation.UpdatedGuestStatus = new List<Model.GuestStatusType>();
      _invitation.DeletedGuestStatus = new List<Model.GuestStatusType>();
      CalculateMaxAuthGifts();

      if (!((GuestInvitation)DataContext).GuestStatusTypes.Any()) return;
      ////Asignamos el guest
      //_lstObjInvitGuestStatus.ForEach(c =>
      //{
      //  c.gtgu = c.gtgu != 0 ? c.gtgu : _guestId;
      //});

      //Convertimos la lista a un objeto de la capa Model
      //var gs = _lstObjInvitGuestStatus.Select(c => new GuestStatus
      //{
      //  gtgs = c.gtgs,
      //  gtgu = c.gtgu,
      //  gtQuantity = c.gtQuantity
      //}).ToList();

      //_invitation.DeletedGuestStatus.AddRange(_lstGuestStatus); //Borramos lo que tenia la base de datos
      _invitation.NewGuestStatus.AddRange(new List<GuestStatus> { (GuestStatus)cmbGuestStatus.SelectedItem }); //Agregamos lo que tiene el grid
    }

    #endregion Métodos para guardar la información del invitado

    #region Métodos para Válidar la información

    private bool Validate()
    {
      bool res = true;

      //if (!txtArrival.SelectedDate.HasValue) //validamos la fecha de llegada
      //{
      //  res = false;
      //  Helpers.UIHelper.ShowMessage("Specify an arrival date");
      //  txtArrival.Focus();
      //}
      //else if (!txtDeparture.SelectedDate.HasValue) //validamos la fecha de salida
      //{
      //  res = false;
      //  Helpers.UIHelper.ShowMessage("Specify an departure date");
      //  txtDeparture.Focus();
      //}
      //else if (String.IsNullOrEmpty(txtAgency.Text))
      //{
      //  res = false;
      //  Helpers.UIHelper.ShowMessage("Specify an agency");
      //  cmbAgency.Focus();
      //}
      //else if (String.IsNullOrEmpty(txtLastNameGuest1.Text)) //validamos el apellido
      //{
      //  res = false;
      //  Helpers.UIHelper.ShowMessage("Input the guest last name");
      //  txtLastNameGuest1.Focus();
      //}
      //else if (String.IsNullOrEmpty(txtFirstNameGuest1.Text)) //validamos el nombre
      //{
      //  res = false;
      //  Helpers.UIHelper.ShowMessage("Input the guest first name");
      //  txtFirstNameGuest1.Focus();
      //}
      //else if (!ValidateGifts()) //validamos los regalos
      //{
      //  res = false;
      //}
      //else if (!ValidateGuestStatus()) //validamos los estatus de invitados
      //{
      //  res = false;
      //}
      //else if (!ValidateChangedBy()) //validamos quien hizo el cambio y su contraseña
      //{
      //  res = false;
      //}
      //else if (!ValidateChangedByExist())//validamos que los datos de quien hizo el cambio y su contraseña existan
      //{
      //  res = false;
      //}

      return res;
    }

    /// <summary>
    /// Validamos la información de los nuevos regalos
    /// </summary>
    /// <returns>Boolean</returns>
    private bool ValidateGifts()
    {
      bool res = true;
      //string title = "Gifts Section";
      ////Revisamos que todos los regalos tengan una cantidad
      //if (_lstObjInvitGift.Where(g => g.igQty == 0).Any())
      //{
      //  res = false;
      //  Helpers.UIHelper.ShowMessage("any of the gifts does not have a specific quantity", title: title);
      //}
      //else if (_lstObjInvitGift.Where(g => String.IsNullOrEmpty(g.iggi)).Any()) //Revisamos que todos los registros tengan un regalo
      //{
      //  res = false;
      //  Helpers.UIHelper.ShowMessage("any of the gifts does not have a specific gift", title: title);
      //}
      //else if (_lstObjInvitGift.Where(g => g.igAdults == 0).Any()) //Revisamos que todos los registros tengan almenos un Adulto asignado
      //{
      //  res = false;
      //  Helpers.UIHelper.ShowMessage("any of the gifts does not have a specific adults", title: title);
      //}

      //if (res)
      //{
      //  foreach (var row in _lstObjInvitGift)
      //  {
      //    var gift = IM.BusinessRules.BR.BRGifts.GetGiftId(row.iggi);
      //    if (row.igQty > gift.giMaxQty)
      //    {
      //      string error = String.Format("The maximu quantity authorized of the gift {0} has been exceeded.\n Max authotized = {1}", gift.giN, gift.giMaxQty);
      //      Helpers.UIHelper.ShowMessage(error, title: title);
      //      res = false;
      //      break;
      //    }
      //  }
      //}

      //if (!res)
      //{
      //  dtgGifts.Focus();
      //}
      return res;
    }

    /// <summary>
    /// Validamos la información de quien realiza los cambios
    /// </summary>
    /// <returns></returns>
    private bool ValidateChangedBy()
    {
      bool res = true;

      if (string.IsNullOrEmpty(txtUser.Text))
      {
        res = false;
        Helpers.UIHelper.ShowMessage("Specify who is making the change.");
        txtUser.Focus();
      }
      else if (string.IsNullOrEmpty(txtPassword.Password))
      {
        res = false;
        Helpers.UIHelper.ShowMessage("Specify who is making the change.");
        txtUser.Focus();
      }

      return res;
    }

    private bool ValidateChangedByExist()
    {
      bool res = true;

      var pass = EncryptHelper.Encrypt(txtPassword.Password);

      var valid = BRGuests.ChangedByExist(txtUser.Text, pass, _user.LeadSource.lsID);

      if (valid.Focus == string.Empty) return res;

      res = false;

      //desplegamos el mensaje de error
      Helpers.UIHelper.ShowMessage(valid.Message);

      //establecemos el foco en el control que tiene el error
      switch (valid.Focus)
      {
        case "ID":
        case "ChangedBy":
          txtUser.Focus();
          break;

        case "Password":
          txtPassword.Focus();
          break;
      }
      return res;
    }

    #endregion Métodos para Válidar la información

    #endregion Métodos privados
  }
}
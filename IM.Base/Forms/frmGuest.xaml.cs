using System;
using System.Windows;
using IM.BusinessRules.BR;
using IM.Model.Classes;
using IM.Model;
using System.Collections.Generic;
using IM.Model.Enums;
using System.Windows.Data;
using IM.Base.Classes;
using System.Linq;
using System.Windows.Controls;
using IM.Base.Helpers;

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
    UserData _user;
    int _guestId;
    bool _isInvitation;
    bool _includedTours;
    bool _isReadOnly;
    bool _wasSelectedByKeyboard = false;

    #region Listas
    private List<InvitationGift> _lstGifts = new List<InvitationGift>();
    private List<GuestStatus> _lstGuestStatus = new List<GuestStatus>();

    private List<objInvitGift> _lstObjInvitGift = null;
    private List<objInvitGuestStatus> _lstObjInvitGuestStatus = null;
    #endregion

    #region Objetos

    Invitation invitation;

    #region Regalos
    CollectionViewSource objInvitGiftViewSource;
    CollectionViewSource giftShortViewSource;
    #endregion

    #region Guest Status
    CollectionViewSource objInvitGuestStatusViewSource;
    CollectionViewSource guestStatusTypeViewSource;
    objInvitGuestStatus objInvitGuestStatusTemp = new objInvitGuestStatus();
    #endregion

    #endregion

    #endregion

    #region Atributo Público
    public Guest guestAdditional = null;
    #endregion

    #region Constructores y destructores
    public frmGuest(UserData user, int guestId, bool isInvitation, bool includedTours, bool isReadOnly = false)
    {
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
      this._user = user;
      this._guestId = guestId;
      this._isInvitation = isInvitation;
      this._includedTours = includedTours;
      this._isReadOnly = isReadOnly;
      InitializeComponent();
    }
    #endregion

    #region Métodos de la forma

    /// <summary>
    /// Configura los elementos de la forma
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void frmGuest_Loaded(object sender, RoutedEventArgs e)
    {
      ConfigurationControls();
      LoadControls();
    }

    /// <summary>
    /// Asigna la cable de la agencia al ComboBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void txtAgency_LostFocus(object sender, RoutedEventArgs e)
    {
      _wasSelectedByKeyboard = true;
      cmbAgency.SelectedValue = txtAgency.Text;
    }

    /// <summary>
    /// Asigna la cable de la agencia al TextBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void cmbAgency_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!_wasSelectedByKeyboard)
      {
        txtAgency.Text = cmbAgency.SelectedValue.ToString();
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
      System.ComponentModel.IEditableCollectionView itemView = dtgGuestStatus.Items;
      if (_lstObjInvitGuestStatus.Count == 1 && itemView.IsAddingNew)
      {
        itemView.CommitNew();
        dtgGuestStatus.CanUserAddRows = false;
      }
    }

    /// <summary>
    /// Revisa los datos ingresados la grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dtgGuestStatus_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      switch (e.Column.SortMemberPath)
      {
        case "gtgs":
          var gtgs = e.EditingElement as ComboBox;
          if (gtgs.SelectedIndex != -1)
          {
            _lstObjInvitGuestStatus[e.Row.GetIndex()].gtgs = gtgs.SelectedValue.ToString();
            _lstObjInvitGuestStatus[e.Row.GetIndex()].gtQuantity = Convert.ToByte(1);
            CalculateMaxAuthGifts();
          }
          break;
      }
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (Validate())
      {
        SaveGuestInformation();
        this.Close();
      }
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

    #endregion

    #endregion

    #region Métodos privados

    #region Configuración de controles
    private void ConfigurationControls()
    {
      btnIncludedTours.Visibility = _includedTours ? Visibility.Visible : Visibility.Hidden;
      txtArrival.IsEnabled = !_includedTours;
      txtDeparture.IsEnabled = !_includedTours;
      btnSave.IsEnabled = !_isReadOnly;
    }
    #endregion

    #region Cargar datos del invitado

    /// <summary>
    /// Manda llamar los métodos para cargar la información del guest.
    /// </summary>
    private void LoadControls()
    {
      LoadCommonControls(); //Se cargan los controles que son comunes en todos los tipos de invitacion.

      
      #region Datos del Invitado
      LoadGuestInformation();
      #endregion
    }

    /// <summary>
    /// Carga la información extraida de la base de datos
    /// </summary>
    private void LoadGuestInformation()
    {
      var guest = IM.BusinessRules.BR.BRGuests.GetGuestById(_guestId);

      if (guest == null) return;
      
      #region Información del invitado
      txtGuid.Text = guest.guID.ToString();
      txtReservationNumber.Text = guest.guHReservID;
      txtSalesRoom.Text = String.IsNullOrEmpty(guest.gusr) ? String.Empty : guest.gusr;
      txtLeadSource.Text = String.IsNullOrEmpty(guest.guls) ? String.Empty : guest.guls;

      #endregion

      #region Other Information
      txtRoom.Text = guest.guRoomNum;
      cmbAgency.SelectedValue = guest.guag;
      txtAgency.Text = guest.guag;
      txtPax.Text = guest.guPax.ToString("#.00");
      txtArrival.Text = guest.guCheckInD.ToString("dd/MM/yyyy");
      txtDeparture.Text = guest.guCheckOutD.ToString("dd/MM/yyyy");

      #endregion

      #region Guest 1
      txtLastNameGuest1.Text = guest.guLastName1;
      txtFirstNameGuest1.Text = guest.guFirstName1;
      txtAgeGuest1.Text = guest.guAge1.HasValue ? guest.guAge1.Value.ToString() : String.Empty;
      cmbMaritalStatusGuest1.SelectedValue = String.IsNullOrEmpty(guest.gums1) ? String.Empty : guest.gums1;
      txtOcuppationGuest1.Text = guest.guOccup1;
      txtEmailGuest1.Text = guest.guEmail1;
      #endregion

      #region Guest 2
      txtLastNameGuest2.Text = guest.guLastname2;
      txtFirstNameGuest2.Text = guest.guFirstName2;
      txtAgeGuest2.Text = guest.guAge2.HasValue ? guest.guAge2.Value.ToString() : String.Empty;
      cmbMaritalStatusGuest2.SelectedValue = String.IsNullOrEmpty(guest.gums2) ? String.Empty : guest.gums2;
      txtOcuppationGuest2.Text = guest.guOccup2;
      txtEmailGuest2.Text = guest.guEmail2;
      #endregion

      #region Guest Status
      txtGuestStatus.Text = guest.guGStatus;

      LoadGuestStatusGrid();
      //LLENAR GRID
      #endregion

      #region Gifts
      LoadGiftGrid();
      #endregion
    }

    /// <summary>
    /// Carga los controles comunes de todos los tipos de invitación
    /// </summary>
    /// <history>
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadCommonControls()
    {
      #region User
      txtUser.Text = _user.User.peID;
      txtPassword.Password = _user.User.pePwd;
      #endregion

      #region ComboBoxes

      var agencies = await IM.BusinessRules.BR.BRAgencies.GetAgencies(1);
      LoadComboBox(agencies, cmbAgency, "ag");

      var maritalStatus = await IM.BusinessRules.BR.BRMaritalStatus.GetMaritalStatus(1);
      LoadComboBox(maritalStatus, cmbMaritalStatusGuest1, "ms");
      LoadComboBox(maritalStatus, cmbMaritalStatusGuest2, "ms");

      //Combo Guest Estatus
      guestStatusTypeViewSource = ((CollectionViewSource)(this.FindResource("guestStatusTypeViewSource")));
      guestStatusTypeViewSource.Source = await BRGuests.GetGuestStatusType(1);

      //Combo Regalos
      giftShortViewSource = ((CollectionViewSource)(this.FindResource("giftShortViewSource")));
      giftShortViewSource.Source = await BRGifts.GetGifts();
      #endregion

    }

    #endregion

    #region Métodos para cargar los grids

    /// <summary>
    /// Carga la información del Grid de regalos
    /// </summary>
    private void LoadGiftGrid()
    {
      _lstObjInvitGift = new List<objInvitGift>();

      var invitGift = BRGifts.GetGiftsByGuest(_guestId);

      _lstObjInvitGift = invitGift.Select(c => new objInvitGift
      {
        igAdults = c.igAdults,
        igComments = c.igComments,
        igct = c.igct,
        igExtraAdults = c.igExtraAdults,
        igFolios = c.igFolios,
        iggi = c.iggi,
        iggr = c.iggr,
        iggu = c.iggu,
        igMinors = c.igMinors,
        igPriceA = c.igPriceA,
        igPriceAdult = c.igPriceAdult,
        igPriceExtraAdult = c.igPriceExtraAdult,
        igPriceM = c.igPriceM,
        igPriceMinor = c.igPriceMinor,
        igQty = c.igQty
      }).ToList();

      _lstGifts = invitGift;

      objInvitGiftViewSource = ((CollectionViewSource)(this.FindResource("objInvitGiftViewSource")));
      objInvitGiftViewSource.Source = _lstObjInvitGift;

      CalculateCostsPrices();
      CalculateTotalGifts();
    }

    /// <summary>
    /// Carga la información del Grid de estado de los invitados
    /// </summary>
    private void LoadGuestStatusGrid()
    {
      var guestStatus = BRGuestStatusTypes.GetGuestStatus(_guestId);

      _lstObjInvitGuestStatus = guestStatus.Select(c => new objInvitGuestStatus
      {
        gtgs = c.gtgs,
        gtgu = c.gtgu,
        gtQuantity = c.gtQuantity
      }).ToList();
      _lstGuestStatus = guestStatus; // esta lista mantiene los registros de la base de datos sin modificaciones.

      objInvitGuestStatusViewSource = ((CollectionViewSource)(this.FindResource("objInvitGuestStatusViewSource")));
      objInvitGuestStatusViewSource.Source = _lstObjInvitGuestStatus;

      txtGuestStatus.Text = guestStatus.Any() ? guestStatus.First().gtgs : String.Empty;
      CalculateMaxAuthGifts();

      if (_lstObjInvitGuestStatus.Any()) dtgGuestStatus.CanUserAddRows = false;
    }

    #endregion

    #region Métodos para cargar ComboBoxes

    /// <summary>
    /// Carga los combos de la forma
    /// </summary>
    /// <param name="items">Lista de elementos que contendrá el combo</param>
    /// <param name="combo">Combo que se llenará con los elementos</param>
    /// <param name="displayItem">Nombre del elemento</param>
    /// <param name="valueItem">Valor del elemento</param>
    /// <param name="defaultValue">Valor que será seleccionado por default</param>
    private void LoadComboBox(IEnumerable<object> items, ComboBox combo, string displayItem, string valueItem, string defaultValue = "")
    {
      combo.DisplayMemberPath = displayItem;
      combo.SelectedValuePath = valueItem;
      combo.SelectedValue = defaultValue;
      combo.ItemsSource = items;
    }
    
    /// <summary>
    /// Carga los combos de la forma
    /// </summary>
    /// <param name="items">Lista de elementos que contendrá el combo</param>
    /// <param name="combo">Combo que se llenará con los elementos</param>
    /// <param name="prefix">prefijo</param>
    /// <param name="defaultValue">Valor que será seleccionado por default</param>
    private void LoadComboBox(IEnumerable<object> items, ComboBox combo, string prefix, string defaultValue = "")
    {
      combo.DisplayMemberPath = String.Format("{0}N", prefix);
      combo.SelectedValuePath = String.Format("{0}ID", prefix);
      combo.SelectedValue = defaultValue;
      combo.ItemsSource = items;
    }

    #endregion

    #region Métodos para calcular los costos de los regalos

    /// <summary>
    /// Calcula los costos y precios de adultos y menores de un regalo
    /// </summary>
    /// <param name="useCxCCost">Indica si se utilizará el costo del empleado</param>
    private void CalculateCostsPrices(bool useCxCCost = false)
    {
      decimal costAdult, costMinor, priceAdult, priceMinor, priceExtraAdult, quantity;

      foreach (var row in _lstObjInvitGift)
      {
        var gift = IM.BusinessRules.BR.BRGifts.GetGiftId(row.iggi);
        if (gift != null)
        {
          // Costos
          // si se va a usar el costo de empleado
          if (useCxCCost)
          {
            costAdult = gift.giPrice1;
            costMinor = gift.giPrice4;
          }
          else // se va a usar el cosrto al público
          {
            costAdult = gift.giPrice1;
            costMinor = gift.giPrice2;
          }

          // Precios
          priceAdult = gift.giPublicPrice;
          priceMinor = gift.giPriceMinor;
          priceExtraAdult = gift.giPriceExtraAdult;
          quantity = row.igQty;

          // Total del costo adultos
          row.igPriceA = quantity * (row.igAdults + row.igExtraAdults) * costAdult;
          // Total del costo de menores
          row.igPriceM = quantity * row.igMinors * costMinor;
          // Total del precio adultos
          row.igPriceAdult = quantity * row.igAdults * priceAdult;
          //Total del precio de menores
          row.igPriceMinor = quantity * row.igMinors * priceMinor;
          // Total del precio de adultos extra
          row.igPriceExtraAdult = quantity * row.igExtraAdults * priceExtraAdult;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="onlyCancellled"></param>
    /// <param name="cancel"></param>
    private void CalculateTotalGifts(bool onlyCancellled = false, string cancel = "")
    {
      decimal cost, price, totalCost = 0, totalPrice = 0;

      foreach (var row in _lstObjInvitGift)
      {
        // calculamos el costo del regalo
        cost = row.igPriceA + row.igPriceA;

        //calculamos el precio del regalo
        price = row.igPriceAdult + row.igPriceMinor + row.igPriceExtraAdult;

        //si se desean todos los regalos
        if (!onlyCancellled)
        {
          totalCost += cost;
          totalPrice += price;
        }


      }
      txtTotalCost.Text = totalCost.ToString("$#,##0.00;$(#,##0.00)");
      txtTotalPrice.Text = totalPrice.ToString("$#,##0.00;$(#,##0.00)");
    }

    /// <summary>
    /// Calcula el monto maximo de regalos
    /// </summary>
    /// <history>
    /// [emoguel] modified se volvió async
    /// </history>
    private async void CalculateMaxAuthGifts()
    {
      decimal maxAuthGifts = 0;

      foreach (var row in _lstObjInvitGuestStatus)
      {
        if (row.gtgs != null && row.gtQuantity > 0)
        {
          var guestStatusType = await BRGuestStatusTypes.GetGuestStatusTypes(new Model.GuestStatusType { gsID = row.gtgs });
          var guestStaType=guestStatusType.FirstOrDefault();
          maxAuthGifts += row.gtQuantity * guestStaType.gsMaxAuthGifts;
        }
      }

      txtMaxAuth.Text = maxAuthGifts.ToString("$#,##0.00;$(#,##0.00)");
    }

    #endregion

    #region Métodos para guardar la información del invitado
    /// <summary>
    /// Guarda la informaciónde la forma en la base de datos
    /// </summary>
    private void SaveGuestInformation()
    {
      invitation = new Invitation();
      var guest = IM.BusinessRules.BR.BRGuests.GetGuestById(_guestId);

      #region Other Information
      guest.guRoomNum = ForStringValue(txtRoom.Text);
      guest.guag = ForStringValue(txtAgency.Text);
      guest.guPax = ForDecimalValue(txtPax.Text);
      guest.guCheckInD = txtArrival.SelectedDate.HasValue ? txtArrival.SelectedDate.Value : guest.guCheckInD;
      guest.guCheckOutD = txtDeparture.SelectedDate.HasValue ? txtDeparture.SelectedDate.Value : guest.guCheckOutD;

      #endregion

      #region Guest 1
      guest.guLastName1 = ForStringValue(txtLastNameGuest1.Text);
      guest.guFirstName1 = ForStringValue(txtFirstNameGuest1.Text);
      guest.guAge1 = String.IsNullOrEmpty(txtAgeGuest1.Text) ? (byte?)null : Convert.ToByte(txtAgeGuest1.Text);
      guest.gums1 = ForStringValue(cmbMaritalStatusGuest1.SelectedValue);
      guest.guOccup1 = ForStringValue(txtOcuppationGuest1.Text);
      guest.guEmail1 = ForStringValue(txtEmailGuest1.Text);
      #endregion

      #region Guest 2
      guest.guLastname2 = ForStringValue(txtLastNameGuest2.Text);
      guest.guFirstName2 = ForStringValue(txtFirstNameGuest2.Text);
      guest.guAge2 = String.IsNullOrEmpty(txtAgeGuest2.Text) ? (byte?)null : Convert.ToByte(txtAgeGuest2.Text);
      guest.gums2 = ForStringValue(cmbMaritalStatusGuest2.SelectedValue);
      guest.guOccup2 = ForStringValue(txtOcuppationGuest2.Text);
      guest.guEmail2 = ForStringValue(txtEmailGuest2.Text);
      #endregion

      #region Guest Status 
      guest.guGStatus = ForStringValue(txtGuestStatus.Text);
      SaveGuestStatus();
      #endregion

      #region Gifts
      SaveGifts();

      #endregion

      invitation.Guest = guest;


      BRGuests.SaveGuestInvitation(invitation);

    }

    /// <summary>
    /// Guarda todos los regalos asignados
    /// </summary>
    private void SaveGifts()
    {
      invitation.NewGifts = new List<Model.InvitationGift>();
      invitation.UpdatedGifts = new List<Model.InvitationGift>();
      invitation.DeletedGifts = new List<Model.InvitationGift>();

      if (!_lstObjInvitGift.Any()) return;

      //Convertimos la lista a un objeto de la capa Model
      var gifts = _lstObjInvitGift.Select(c => new InvitationGift
      {
        igAdults = c.igAdults,
        igComments = c.igComments,
        igct = c.igct,
        igExtraAdults = c.igExtraAdults,
        igFolios = c.igFolios,
        iggi = c.iggi,
        iggr = c.iggr,
        iggu = c.iggu,
        igMinors = c.igMinors,
        igPriceA = c.igPriceA,
        igPriceAdult = c.igPriceAdult,
        igPriceExtraAdult = c.igPriceExtraAdult,
        igPriceM = c.igPriceM,
        igPriceMinor = c.igPriceMinor,
        igQty = c.igQty
      }).ToList();

      //Obtenemos los regalos que se modificarán
      invitation.UpdatedGifts = gifts.Where(c => c.iggu != 0).ToList();

      //Obtenemos los regalos nuevos para asignales los precios y el invitado
      var newGifts = gifts.Where(c => c.iggu == 0).ToList();
      if (newGifts.Any())
      {
        //Asignamos el guest a los nuevos regalos
        newGifts.ForEach(c =>
        {
          c.iggu = c.iggu != 0 ? c.iggu : _guestId;
          c.igct = !String.IsNullOrEmpty(c.igct) ? c.igct : "MARKETING";
        });
        invitation.NewGifts.AddRange(newGifts);
      }
    }

    /// <summary>
    /// Guarda los estados del invitado
    /// </summary>
    private void SaveGuestStatus()
    {
      invitation.NewGuestStatus = new List<Model.GuestStatus>();
      invitation.UpdatedGuestStatus = new List<Model.GuestStatus>();
      invitation.DeletedGuestStatus = new List<Model.GuestStatus>();
      CalculateMaxAuthGifts();

      if (_lstObjInvitGuestStatus.Any())
      {

        //Asignamos el guest
        _lstObjInvitGuestStatus.ForEach(c =>
        {
          c.gtgu = c.gtgu != 0 ? c.gtgu : _guestId;
        });

        //Convertimos la lista a un objeto de la capa Model
        var gs = _lstObjInvitGuestStatus.Select(c => new GuestStatus
        {
          gtgs = c.gtgs,
          gtgu = c.gtgu,
          gtQuantity = c.gtQuantity
        }).ToList();

        invitation.DeletedGuestStatus.AddRange(_lstGuestStatus); //Borramos lo que tenia la base de datos
        invitation.NewGuestStatus.AddRange(gs); //Agregamos lo que tiene el grid
      }
    }
    
    #endregion

    #region Métodos para asignar valores al objeto Guest

    /// <summary>
    /// Devuelme una cadena
    /// </summary>
    /// <param name="value">objeto que se recibe</param>
    /// <returns>string</returns>
    private string ForStringValue(object value)
    {
      if (value == null) return String.Empty;
      return String.IsNullOrEmpty(value.ToString()) ? String.Empty : value.ToString().Trim();
    }

    /// <summary>
    /// Devuelme un decimal
    /// </summary>
    /// <param name="value">objeto que se recibe</param>
    /// <returns>string</returns>
    private decimal ForDecimalValue(object value)
    {
      return String.IsNullOrEmpty(value.ToString().Trim()) ? 0 : Convert.ToDecimal(value.ToString().Trim());
    }

    /// <summary>
    /// Devuelme un intero
    /// </summary>
    /// <param name="value">objeto que se recibe</param>
    /// <returns>string</returns>
    private int ForIntegerValue(object value)
    {
      return String.IsNullOrEmpty(value.ToString().Trim()) ? 0 : Convert.ToInt32(value.ToString().Trim());
    }

    /// <summary>
    /// Devuelve un booleano
    /// </summary>
    /// <param name="value">objeto que se recibe</param>
    /// <returns>bool</returns>
    private bool ForBooleanValue(bool? value)
    {
      return value.HasValue ? value.Value : false;
    }






    #endregion

    #region Métodos para Válidar la información
    private bool Validate()
    {
      bool res = true;

      if (!txtArrival.SelectedDate.HasValue) //validamos la fecha de llegada
      {
        res = false;
        Helpers.UIHelper.ShowMessage("Specify an arrival date");
        txtArrival.Focus();
      }
      else if (!txtDeparture.SelectedDate.HasValue) //validamos la fecha de salida
      {
        res = false;
        Helpers.UIHelper.ShowMessage("Specify an departure date");
        txtDeparture.Focus();
      }
      else if (String.IsNullOrEmpty(txtAgency.Text))
      {
        res = false;
        Helpers.UIHelper.ShowMessage("Specify an agency");
        cmbAgency.Focus();
      }
      else if (String.IsNullOrEmpty(txtLastNameGuest1.Text)) //validamos el apellido
      {
        res = false;
        Helpers.UIHelper.ShowMessage("Input the guest last name");
        txtLastNameGuest1.Focus();
      }
      else if (String.IsNullOrEmpty(txtFirstNameGuest1.Text)) //validamos el nombre
      {
        res = false;
        Helpers.UIHelper.ShowMessage("Input the guest first name");
        txtFirstNameGuest1.Focus();
      }
      else if (!ValidateGifts()) //validamos los regalos
      {
        res = false;
      }
      else if (!ValidateGuestStatus()) //validamos los estatus de invitados
      {
        res = false;
      }
      else if (!ValidateChangedBy()) //validamos quien hizo el cambio y su contraseña
      {
        res = false;
      }
      else if (!ValidateChangedByExist())//validamos que los datos de quien hizo el cambio y su contraseña existan
      {
        res = false;
      }
       
      return res;
    }

    /// <summary>
    /// Valida que almenos se haya registrado un estatus
    /// </summary>
    /// <returns></returns>
    private bool ValidateGuestStatus()
    {
      bool res = true;
      string title = "Guest Status Section";
      if (_lstObjInvitGuestStatus.Any())
      {
        if (_lstObjInvitGuestStatus.Where(g => g.gtQuantity == 0).Any())
        {
          Helpers.UIHelper.ShowMessage("Any status does not has a quantity", title: title);
          res = false;
        }
        else if (_lstObjInvitGuestStatus.Where(g => String.IsNullOrEmpty(g.gtgs)).Any())
        {
          Helpers.UIHelper.ShowMessage("Any status does not has a status", title: title);
          res = false;
        }
      }
      if (!res)
      {
        dtgGuestStatus.Focus();
      }
      return res;
    }

    /// <summary>
    /// Validamos la información de los nuevos regalos
    /// </summary>
    /// <returns>Boolean</returns>
    private bool ValidateGifts()
    {
      bool res = true;
      string title = "Gifts Section";
      //Revisamos que todos los regalos tengan una cantidad
      if (_lstObjInvitGift.Where(g => g.igQty == 0).Any())
      {
        res = false;
        Helpers.UIHelper.ShowMessage("any of the gifts does not have a specific quantity", title: title);
      }
      else if (_lstObjInvitGift.Where(g => String.IsNullOrEmpty(g.iggi)).Any()) //Revisamos que todos los registros tengan un regalo
      {
        res = false;
        Helpers.UIHelper.ShowMessage("any of the gifts does not have a specific gift", title: title);
      }
      else if (_lstObjInvitGift.Where(g => g.igAdults == 0).Any()) //Revisamos que todos los registros tengan almenos un Adulto asignado
      {
        res = false;
        Helpers.UIHelper.ShowMessage("any of the gifts does not have a specific adults", title: title);
      }

      if (res)
      {
        foreach (var row in _lstObjInvitGift)
        {
          var gift = IM.BusinessRules.BR.BRGifts.GetGiftId(row.iggi);
          if (row.igQty > gift.giMaxQty)
          {
            string error = String.Format("The maximu quantity authorized of the gift {0} has been exceeded.\n Max authotized = {1}", gift.giN, gift.giMaxQty);
            Helpers.UIHelper.ShowMessage(error, title: title);
            res = false;
            break;
          }
        }
      }

      if (!res)
      {
        dtgGifts.Focus();
      }
      return res;
    }

    /// <summary>
    /// Validamos la información de quien realiza los cambios
    /// </summary>
    /// <returns></returns>
    private bool ValidateChangedBy()
    {
      bool res = true;

      if(String.IsNullOrEmpty(txtUser.Text))
      {
        res = false;
        Helpers.UIHelper.ShowMessage("Specify who is making the change.");
        txtUser.Focus();
      }
      else if(String.IsNullOrEmpty(txtPassword.Password))
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
      
      if(valid.Focus != String.Empty)
      {
        res = false;

        //desplegamos el mensaje de error
        Helpers.UIHelper.ShowMessage(valid.Message);

        //establecemos el foco en el control que tiene el error
        switch(valid.Focus)
        {
          case "ID":
          case "ChangedBy":
            txtUser.Focus();
            break;
          case "Password":
            txtPassword.Focus();
            break;
        }
      }
      return res;
    }

    #endregion

    #endregion

   
  }
}

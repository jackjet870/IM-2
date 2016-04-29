using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Enums;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Controls.Primitives;
using System.ComponentModel;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmGiftsReceipts.xaml
  /// </summary>
  public partial class frmGiftsReceipts : Window, INotifyPropertyChanged
  {

    private static int _guestID;
    private int _chargeToChanged = 0;
    private DateTime? _dtpClose;
    public EnumModeOpen modeOpen;
    public EnumGiftsType _giftsType;

    List<GiftsReceiptsShort> _lstGiftsReceipt;
    List<BookingDeposit> _lstBookingDesposit;
    List<ChargeTo> _lstChargeTo;
    List<Gift> _lstGifts;
    
    // Variable para las tasas de cambio!
    private List<ExchangeRateShort> _lstExchangeRate;

    // Moneda de la sala de ventas
    private string _SRCurrency;

    // Variable para saber si es un intercambio de regalo
    private bool _IsExchange;

    // Variable para saber si se valida el maximo autorizado de gifts
    private bool _blnValidateMaxAuthGifts;

    // Varibale para la validacion del guest status
    private bool _blnApplyGuestStatusValidation;

    // Variable que contendra la informacion del guest status info
    private GuestStatusValidateData _GuesStatusInfo;

    // Variable para validar el row seleccionado del grid regalos
    private string _giftIDSelected = "";

    CollectionViewSource _dsGiftsReceiptsShort;
    CollectionViewSource _dsGiftsReceipt;
    CollectionViewSource _dsPersonnel_Offered;
    CollectionViewSource _dsPersonnel_Gifts;
    CollectionViewSource _dsChargeTo;
    CollectionViewSource _dsPaymentType;
    CollectionViewSource _dsCurrencyPRDeposit;
    CollectionViewSource _dsCurrency;
    CollectionViewSource _dsCurrencyTaxiOut;
    CollectionViewSource _dsGiftsReceiptDetailShort;
    CollectionViewSource _dsGifts;
    CollectionViewSource _dsGiftsReceiptPaymentShort;
    CollectionViewSource _dsBookingDeposit;
    CollectionViewSource _dsCurrencyDeposits;
    CollectionViewSource _dsGiftsReceiptPackage;
    CollectionViewSource _dsBanks;
    CollectionViewSource _dsSourcePayments;
    CollectionViewSource _dsHotel;
    CollectionViewSource _dsSalesRooms;

    public event PropertyChangedEventHandler PropertyChanged;

    public frmGiftsReceipts(int guestID = 0)
    {
      _guestID = guestID;
      _giftsType = EnumGiftsType.ReceiptGifts;

      // Obtenemos la fecha de cierre de los recibos de regalos de la sala
      _dtpClose = BRSalesRooms.GetCloseSalesRoom(EnumSalesRoomType.GiftsReceipts, App.User.SalesRoom.srID);

      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _dsGiftsReceiptsShort = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptsShort")));
      _dsGiftsReceipt = ((CollectionViewSource)(this.FindResource("dsGiftsReceipt")));
      _dsPersonnel_Offered = ((CollectionViewSource)(this.FindResource("dsPersonnel_Offered")));
      _dsPersonnel_Gifts = ((CollectionViewSource)(this.FindResource("dsPersonnel_Gifts")));
      _dsChargeTo = ((CollectionViewSource)(this.FindResource("dsChargeTo")));
      _dsPaymentType = ((CollectionViewSource)(this.FindResource("dsPaymentType")));
      _dsCurrencyPRDeposit = ((CollectionViewSource)(this.FindResource("dsCurrencyPRDeposit")));
      _dsCurrency = ((CollectionViewSource)(this.FindResource("dsCurrency")));
      _dsCurrencyTaxiOut = ((CollectionViewSource)(this.FindResource("dsCurrencyTaxiOut")));
      _dsGiftsReceiptDetailShort = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptDetailShort")));
      _dsGifts = ((CollectionViewSource)(this.FindResource("dsGifts")));
      _dsGiftsReceiptPaymentShort = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptPaymentShort")));
      _dsBookingDeposit = ((CollectionViewSource)(this.FindResource("dsBookingDeposit")));
      _dsCurrencyDeposits = ((CollectionViewSource)(this.FindResource("dsCurrencyDeposits")));
      _dsGiftsReceiptPackage = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptPackage")));
      _dsBanks = ((CollectionViewSource)(this.FindResource("dsBanks")));
      _dsSourcePayments = ((CollectionViewSource)(this.FindResource("dsSourcePayments")));
      _dsHotel = ((CollectionViewSource)(this.FindResource("dsHotels")));
      _dsSalesRooms = ((CollectionViewSource)(this.FindResource("dsSalesRooms")));


      // Obtenemos los colaboradores
      _dsPersonnel_Offered.Source = BRPersonnel.GetPersonnel("ALL", "ALL", "ALL", 1);

      _dsPersonnel_Gifts.Source = BRPersonnel.GetPersonnel("ALL", "ALL", "ALL", 1);

      // Obtenemos los Charge To
      _lstChargeTo = BRChargeTos.GetChargeTos();
      _dsChargeTo.Source = _lstChargeTo;

      //Obtenemos los Parments Types
      _dsPaymentType.Source = BRPaymentTypes.GetPaymentTypes(1);

      // Obtenemos las Monedas de la CxC de PR
      _dsCurrencyPRDeposit.Source = BRCurrencies.GetCurrencies(null, 1);

      // Obtenemos las Monedas de la CxC de Taxi Out
      _dsCurrencyTaxiOut.Source = BRCurrencies.GetCurrencies(null, 1);

      // Obtenemos las monedas
      _dsCurrency.Source = BRCurrencies.GetCurrencies(null, 1);

      // Obtenemos las monedas para los deposits
      _dsCurrencyDeposits.Source = BRCurrencies.GetCurrencies(null, 1);

      //Obtenemos los regalos
      _lstGifts = BRGifts.GetGifts(1);
      _dsGifts.Source = _lstGifts;

      // Obtenemos los bancos
      _dsBanks.Source = BRBanks.GetBanks(1);

      // Obtenemos los Source Payments
      _dsSourcePayments.Source = BRSourcePayments.GetSourcePayments(1);

      // Obtenemos las salas
      _dsSalesRooms.Source = BRSalesRooms.GetSalesRooms(1);

      // Obtenemos los hoteles
      _dsHotel.Source = BRHotels.GetHotels(null, 1);

      // Ocultamos los botones edicion de grid's
      controlBottonEditionGifts(Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);

      switch (modeOpen)
      {
        case EnumModeOpen.Edit: // Permisos de edicion y agregar nuevos items sin guestID y permisos especial - Fuente:Boton
          OptionEdit();
          break;
        case EnumModeOpen.Preview: // Entra cuando el usuario es de solo LECTURA y tiene guestID - Fuente:CheckBox
          _lstGiftsReceipt = BRGiftsReceipts.GetGiftsReceipts(_guestID);
          _dsGiftsReceiptsShort.Source = _lstGiftsReceipt;

          ControlsReadOnly(true, true, true);
          ControlVisibility(Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);
          ControlEnable(false, false, false, false, false, false);
          ControlBottonEdition(Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);
          break;
        case EnumModeOpen.Search: // Modo busqueda con permisos de Lectura - Fuente:Boton
          OptionSearch();
          break;
        case EnumModeOpen.PreviewEdit: // Mode de busqueda con edicion con guestID - Fuente:CheckBox
          _lstGiftsReceipt = BRGiftsReceipts.GetGiftsReceipts(_guestID);
          _dsGiftsReceiptsShort.Source = _lstGiftsReceipt;

          break;
      }
    }

    #region OptionSearch
    /// <summary>
    /// Metodo que evaluar las opciones en modo busqueda
    /// </summary>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private void OptionSearch()
    {
      // Habitilitamos e Inhabilitamos las opciones correspondientes a la opcion
      ControlsReadOnly(true, true, true);
      ControlEnable(false, false, false, false, false, false);
      controlBottonEditionGifts(Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);
     ControlBottonEdition(Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden,
                           Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);
      ControlVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);

      // Iniciamos los controles necesario con valores por default
      dtpgrD.Value = frmHost._dtpServerDate.Date;
    }
    #endregion

    private void controlBottonEditionGifts( Visibility _btnAdd, Visibility _btnRemove, Visibility _btnCancel, Visibility _btnSave)
    {
      btnCancelGift.Visibility = _btnCancel;
      btnAddGift.Visibility = _btnAdd;
      btnRemoveGift.Visibility = _btnRemove;
      btnSaveGift.Visibility = _btnSave;
    }

    #region OptionEdit
    /// <summary>
    /// Metodo que evaluar las opciones en modo Edit
    /// </summary>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private void OptionEdit()
    {
      // Habitilitamos e Inhabilitamos las opciones correspondientes a la opcion
      ControlsReadOnly(true, true, true);
      ControlEnable(false, false, false, false, false, false);
      ControlBottonEdition(Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden,
                           Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);
      ControlVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);

      // Iniciamos los controles necesario con valores por default
      dtpgrD.Value = frmHost._dtpServerDate.Date;

      // Si tiene permisos super special se habilita el boton New y Exchange
      bool blnPermissionSpecial = App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.SuperSpecial);

      if (blnPermissionSpecial)
      {
        btnNew.Visibility = Visibility.Visible;
        btnExchange.Visibility = Visibility.Visible;
      }
    }
    #endregion

    #region grdGiftsReceipt_DoubleClick
    /// <summary>
    /// Función encargada de Cargar la informacion de acuerdo al GIFT RECEIPT seleccionado!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    private void grdGiftsReceipt_DoubleClick(object sender, RoutedEventArgs e)
    {
      GiftsReceiptShort_Selected();
      SetMaxAuthGifts();
      CalculateTotalGifts();
      CalculateTotalPayments();
    }
    #endregion

    #region grdGiftsReceiptDetail_DoubleClick
    /// <summary>
    /// Función encargada de Cargar la informacion para edicion de acuerdo al GIFT seleccionado!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    private void grdGiftsReceiptDetail_DoubleClick(object sender, RoutedEventArgs e)
    {
      
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            grdGiftsReceipt_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }

      e.Handled = blnHandled;
    }

    #endregion

    #region ControlsReadOnly
    /// <summary>
    /// Asigna parametro de solo lectura a los controles correspondientes.
    /// </summary>
    /// <param name="_grdGiftsReceiptDetail"></param>
    /// <param name="_dgBookingDeposit"></param>
    /// <param name="_dgGiftsReceiptPaymentShort"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void ControlsReadOnly(bool _grdGiftsReceiptDetail, bool _dgBookingDeposit, bool _dgGiftsReceiptPaymentShort)
    {
      grdGiftsReceiptDetail.IsReadOnly = _grdGiftsReceiptDetail;
      dgBookingDeposit.IsReadOnly = _dgBookingDeposit;
      dgGiftsReceiptPaymentShort.IsReadOnly = _dgGiftsReceiptPaymentShort;
    } 
    #endregion

    #region ControlEnable
    /// <summary>
    /// Habilita o Deshabilita los controles correspondientes segun sea el caso.
    /// </summary>
    /// <param name="_chkgrCancel"></param>
    /// <param name="_chkgrExchange"></param>
    /// <param name="_cbogrpe"></param>
    /// <param name="_cbogrHost"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void ControlEnable(bool _chkgrCancel, bool _chkgrExchange, bool _cbogrpe, bool _cbogrHost, bool _cboHotel, bool _cboSalesRooms)
    {
      chkgrCancel.IsEnabled = _chkgrCancel;
      chkgrExchange.IsEnabled = _chkgrExchange;
      cbogrpe.IsEnabled = _cbogrpe;
      cbogrHost.IsEnabled = _cbogrHost;
      cboHotel.IsEnabled = _cboHotel;
      cboSalesRoom.IsEnabled = _cboSalesRooms;
    } 
    #endregion

    #region ControlVisibility
    /// <summary>
    /// Oculta y deshabilita los controles necesarios cuando la vista es Preview
    /// </summary>
    /// <param name="_hidden"></param>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    private void ControlVisibility(Visibility _sectionCriterio = Visibility.Visible, Visibility _brdExchange = Visibility.Visible,
                                   Visibility _lblgrMaxAuthGifts = Visibility.Visible, Visibility _txtgrMaxAuthGifts = Visibility.Visible)
    {
      grbSelectionCriteria.Visibility = _sectionCriterio;
      brdExchange.Visibility = _brdExchange;
      lblgrMaxAuthGifts.Visibility = _lblgrMaxAuthGifts;
      txtgrMaxAuthGifts.Visibility = _txtgrMaxAuthGifts;
    }
    #endregion

    #region ControlBottonEdition
    /// <summary>
    /// Oculta los boton segun el modo de apertura del formulario
    /// </summary>
    /// <param name="_btnNew"></param>
    /// <param name="_btnExchange"></param>
    /// <param name="_btnEdit"></param>
    /// <param name="_btnSave"></param>
    /// <param name="_btnUndo"></param>
    /// <param name="_btnPrint"></param>
    /// <param name="_btnCancel"></param>
    /// <param name="_btnCancelSistur"></param>
    /// <param name="_btnEPurse"></param>
    /// <param name="_btnCancelElectronicPurse"></param>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    private void ControlBottonEdition(Visibility _btnNew, Visibility _btnExchange, Visibility _btnEdit, Visibility _btnSave,
                                      Visibility _btnUndo, Visibility _btnPrint, Visibility _btnCancel, Visibility _btnCancelSistur, Visibility _btnEPurse,
                                      Visibility _btnCancelElectronicPurse)
    {
      btnNew.Visibility = _btnNew;
      btnExchange.Visibility = _btnExchange;
      btnEdit.Visibility = _btnEdit;
      btnSave.Visibility = _btnSave;
      btnUndo.Visibility = _btnUndo;
      btnPrint.Visibility = _btnPrint;
      btnCancel.Visibility = _btnCancel;
      btnCancelSisturPromotions.Visibility = _btnCancelSistur;
      btnEPurse.Visibility = _btnEPurse;
      btnCancelElectronicPurse.Visibility = _btnCancelElectronicPurse;
    }
    #endregion
    
    #region grdReceipts_Loaded
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 07/04/2016 Created
    /// </history>
    private void grdReceipts_Loaded(object sender, RoutedEventArgs e)
    {
      /*if (_lstGiftsReceiptSort != null)
      {
        if (_lstGiftsReceiptSort.Count > 0)
        {
          GiftsReceiptShort_Selected();

          // Verificamos si esta en modo busqueda para obtener el guestID!
          if (modeOpen == EnumModeOpen.Search)
          {
            _guestID = Convert.ToInt32(txtgrgu.Text);
          }

          // Cargamos los Booking Deposits
          BookinDeposits_Loaded();
          SetMaxAuthGifts();
          CalculateTotalGifts();
          CalculateTotalPayments();
          LoadExchangeRates();
          ConvertDepositToUsDlls();
          GetGuestData(_guestID);
        }
      }*/
    }
    #endregion
   
    private void Load_grdReceipts()
    {
      if (_lstGiftsReceipt != null)
      {
        if (_lstGiftsReceipt.Count > 0)
        {
          GiftsReceiptShort_Selected();

          // Verificamos si esta en modo busqueda para obtener el guestID!
          if (modeOpen == EnumModeOpen.Search || modeOpen == EnumModeOpen.Edit)
          {
            _guestID = Convert.ToInt32(txtgrgu.Text);
          }

          // Cargamos los Booking Deposits
          BookinDeposits_Loaded();
          SetMaxAuthGifts();
          CalculateTotalGifts();
          CalculateTotalPayments();
          LoadExchangeRates();
          ConvertDepositToUsDlls();
          GetGuestData(_guestID);
          txtgrcxcAdj_Validate();

          // Configuramos la informacion de GuestStatus que se validara
          if (_guestID > 0 || txtgrID.Text != "")
          LoadGuesStatusInfo(_guestID, Convert.ToInt32(txtgrID.Text));
        
        }
      }
    }

    #region LoadGuesStatusInfo
    /// <summary>
    /// Carga la informacion de GuestStatus para validaicon de nuevo schema de regalos
    /// </summary>
    /// <param name="receiptID"></param>
    /// <param name="guestID"></param>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private void LoadGuesStatusInfo(int receiptID, int guestID)
    {
      _blnApplyGuestStatusValidation = false;

      GuestStatusValidateData _guestStatus = BRGuestStatus.GetStatusValidateInfo(guestID, receiptID);

      // Solo si esta configurado se realiza la revision
      if (_guestStatus != null)
        if (_guestStatus.gsMaxQtyTours > 0)
          _blnApplyGuestStatusValidation = true;
    }
    #endregion

    #region Search_GiftsReceipt
    /// <summary>
    /// Obtiene los datos ingresados por el usuario para la busqueda de algun Gift Receipt
    /// </summary>
    /// <returns> True - Encontro el Gift Receipt | False - Ningun resultado encontrado </returns>
    /// <history>
    /// 12/Abril/2016 Created
    /// </history>
    private bool Search_GiftsReceipt()
    {
      int _guest = txtCgrgu.Text != "" ? Convert.ToInt32(txtCgrgu.Text) : 0;
      string _salesRoom = App.User.SalesRoom.srID;
      int _receipt = txtCgrID.Text != "" ? Convert.ToInt32(txtCgrID.Text) : 0;
      string _folio = txtCgrNum.Text != "" ? txtCgrNum.Text : "ALL";
      DateTime? _dateFrom = dtpCgrDFrom.Value.Value.Date;
      DateTime? _dateTo = dtpTo.Value.Value.Date;
      string _name = txtCgrGuest.Text != "" ? txtCgrGuest.Text : "ALL";
      string _reservation = txtCReservation.Text != "" ? txtCReservation.Text : "ALL";

      //Ejecutamos el procedimiento almancenado con los criterios de busqueda
      _lstGiftsReceipt = BRGiftsReceipts.GetGiftsReceipts(_guest, _salesRoom, _receipt, _folio, _dateFrom, _dateTo, _name, _reservation);

      // Si no hay recibos de regalos
      if (_lstGiftsReceipt.Count == 0)
      {
        btnCancel.IsEnabled = false;
        btnEPurse.IsEnabled = false;
        btnCancelElectronicPurse.IsEnabled = false;
        btnCancelSisturPromotions.IsEnabled = false;

        return false;
      }
      else
      {
        _dsGiftsReceiptsShort.Source = _lstGiftsReceipt;
        //        grdReceipts_Loaded(null, null);
        return true;
      }
    } 
    #endregion

    private void SetMaxAuthGifts()
    {
      decimal curMaxAuthGifts;
      bool blnWithMaxAuthGifts = false;

      ChargeTo _chargeTo = (ChargeTo)cbogrct.SelectedItem;

      curMaxAuthGifts = CalculateMaxAuthGifts(_chargeTo.ctID, txtgrls.Text, ref blnWithMaxAuthGifts);
      txtgrMaxAuthGifts.Text = string.Format("${0}", curMaxAuthGifts);
      lblgrMaxAuthGifts.Visibility = (blnWithMaxAuthGifts) ? Visibility.Visible : Visibility.Hidden;
      txtgrMaxAuthGifts.Visibility = (blnWithMaxAuthGifts) ? Visibility.Visible : Visibility.Hidden;
      _blnValidateMaxAuthGifts = blnWithMaxAuthGifts;
    }

    private decimal CalculateMaxAuthGifts(string chargeTo, string leadSource, ref bool withMaxAuthGifts)
    {
      decimal curMaxAuthGifts = 0;
      withMaxAuthGifts = true;

      ChargeTo _chargeTo = (ChargeTo)cbogrct.SelectedItem;

      switch (_chargeTo.ctCalcType)
      {
        //Monto maximo de regalos por Lead Source
        case "A":
          // Si tiene Lead Source
          if (leadSource != "")
          {
            LeadSource _leadSource = BRLeadSources.GetLeadSourceByID(leadSource);
            //si encontro el Lead Source
            if (_leadSource != null)
            {
              curMaxAuthGifts = _leadSource.lsMaxAuthGifts;
            }
          }
          break;
        // Monto maximo de regalos por Guest Status
        case "C":
          GuestStatus _guestsStatus = BRGuestStatus.GetGuestsStatus(_guestID);
          GuestStatusType _guestStatusType = BRGuestStatusTypes.GetGuestStatusTypeByID(_guestsStatus.gtgs);
          curMaxAuthGifts = _guestsStatus.gtQuantity * _guestStatusType.gsMaxAuthGifts;
          break;
        //  Sin monto maximo de regalos
        default:
          curMaxAuthGifts = 0;
          withMaxAuthGifts = false;
          break;
      }
      return curMaxAuthGifts;
    }

    #region GetGuestData
    /// <summary>
    /// Obtiene la informacion de un Guest
    /// </summary>
    /// <param name="guestID"> Identificador del Guest </param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void GetGuestData(int guestID)
    {
      // validamos que se haya enviado una clave de huesped
      if (guestID == 0)
      {
        return;
      }
      // Obtenemos los datos del huesped
      GuestShort _guestShort = BRGuests.GetGuestShort(guestID);

      if (_guestShort != null)
      {
        txtGuestName.Text = _guestShort.Name;
        txtReservationCaption.Text = _guestShort.guHReservID;
        txtAgencyCaption.Text = _guestShort.agN;
        txtProgramCaption.Text = _guestShort.pgN;
      }
      else
      {
        txtGuestName.Text = "";
        txtReservationCaption.Text = "";
        txtAgencyCaption.Text = "";
        txtProgramCaption.Text = "";
      }
    }
    #endregion

    public void CalculateTotalGifts(bool OnlyCancellled = false, bool CancelField = false)
    {
      decimal curCost = 0;
      decimal curPrice = 0;
      decimal curTotalCost = 0;
      decimal curTotalPrice = 0;
      decimal curTotalToPay = 0;

      foreach (GiftsReceiptDetailShort item in grdGiftsReceiptDetail.Items)
      {
        if (item.geQty != 0 && item.gegi != "")
        {
          // Calculamos el costo del regalo
          curCost = (item.gePriceA) + (item.gePriceM);

          // Calculamos el precio del regalo
          curPrice = (item.gePriceAdult) + (item.gePriceMinor) + (item.gePriceExtraAdult);

          // Si se desean todos los regalos
          if (!OnlyCancellled)
          {
            // Si la cantidad es positiva
            if (item.geQty > 0)
            {
              curTotalCost += curCost;
              curTotalPrice += curPrice;

              // Si es del recibo de regalo
              if (_giftsType == EnumGiftsType.ReceiptGifts)
              {
                // Si el regalo esta marcado como venta
                if (item.geSale == true)
                {
                  curTotalToPay += curPrice;
                }
              }
            }
          }
          // Si se desean solo los regalos cancelados
          else
          {
            // Si el regalo se desea cancelar
            if (CancelField != false)
            {
              curTotalCost += curCost;
              curTotalPrice += curPrice;
            }
          }
        }
      }

      // Actualizamos la etiqueta costo total
      txtTotalCostCaption.Text = string.Format("{0:C2}", curTotalCost);

      // Actualizamos la etiqueta de precio total
      txtTotalPriceCaption.Text = string.Format("{0:C2}", curTotalPrice);

      //Actualizamos la etiqueta de total a pagar
      txtTotalToPayCaption.Text = string.Format("{0:C2}", curTotalToPay);

    }

    #region CalculateTotalPayments
    /// <summary>
    /// Calcula el total de pagos
    /// </summary>
    /// <history>
    /// [vipacheco] 13/Abril/2016 Created
    /// </history>
    private void CalculateTotalPayments()
    {
      decimal curTotalPaid = 0;
      DateTime dtmReceipt;
      decimal curExchangeRate = 0;

      // Obtenemos la fecha de recibo
      dtmReceipt = dtpgrD.Value.Value.Date;

      // Cargamos los tipos de cambio de la fecha de recibo
      BRExchangeRate.GetExchangeRatesByDate(dtmReceipt);

      // Recorremos los pagos
      foreach (GiftsReceiptPaymentShort item in dgGiftsReceiptPaymentShort.Items)
      {
        // Si se ingreso la cantidad pagada
        if (item.gyAmount >= 0)
        {
          // Localizamos el tipo de cambio
          ExchangeRate _exchangeRate = BRExchangeRate.GetExchangeRateByID(item.gycu);

          if (_exchangeRate != null)
            curExchangeRate = _exchangeRate.exExchRate;
          else
            curExchangeRate = 1;

          curTotalPaid += item.gyAmount * curExchangeRate;
        }
      }
      txtTotalPaymentsCaption.Text = string.Format("{0:C2}", curTotalPaid);
    } 
    #endregion

    private void LoadExchangeRates()
    {
      // Obtenemos la fecha de recibo
      DateTime dtmReceipt = dtpgrD.Value.Value.Date;

      // Cargamos los tipos de cambio de la fecha de recibo
      _lstExchangeRate = BRExchangeRate.GetExchangeRatesByDate(dtmReceipt);

      //Cargamos la moneda
      SalesRoomShort _sr = (SalesRoomShort)cboSalesRoom.SelectedItem;
      SalesRoom _salesRoom = BRSalesRooms.GetSalesRoomByID(_sr.srID);
      _SRCurrency = _salesRoom.srcu;
    }

    private void ConvertDepositToUsDlls()
    {
      decimal curDeposit = 0;
      decimal curExchangeRate = 0;
      decimal curExchangeRateMN = 0;

      // Localizamos el tipo de cambio de ese dia de la moneda en que esta el deposito
      Currency _currency = (Currency)cbogrcu.SelectedItem;

      if (_lstExchangeRate != null)
      {
        ExchangeRateShort _exchangeRate = _lstExchangeRate.Where(x => x.excu == _currency.cuID).FirstOrDefault();

        if (_exchangeRate != null)
          curExchangeRate = _exchangeRate.exExchRate;
        else
          curExchangeRate = 1;

        // Localizamos el tipo de cambio de ese dia de la moneda de la sala de ventas
        ExchangeRateShort _exchangeRateCurrency = _lstExchangeRate.Where(x => x.excu == _SRCurrency).FirstOrDefault();

        if (_exchangeRateCurrency != null)
          curExchangeRateMN = _exchangeRateCurrency.exExchRate;
        else
          curExchangeRateMN = 1;

        // Obtenemos el monto del deposito
        //if (txtgrDepositTwisted.Text != "")
        //{
        if (Convert.ToDecimal(txtgrDepositTwisted.Text) > 0)
          curDeposit = Convert.ToDecimal(txtgrDepositTwisted.Text);
        //else if (txtgrDeposit.Text != "")
        //{
        else if (Convert.ToDecimal(txtgrDeposit.Text) > 0)
          curDeposit = Convert.ToDecimal(txtgrDeposit.Text);
        // }
        else
          curDeposit = 0;
        //}

        // Monto en dolares
        txtDepositUS.Text = string.Format("{0:0.00}", Math.Round(curDeposit, 4) * Math.Round(curExchangeRate, 4));

        // Monto en moneda nacional
        txtDepositMN.Text = string.Format("{0:0.00}", (Math.Round(curDeposit, 4) * Math.Round(curExchangeRate, 4)) / Math.Round(curExchangeRateMN, 4));

      }
    }

    #region GiftsReceiptShort_Selected
    /// <summary>
    /// Obtienen la seleccion del row en el dataGrid gifts
    /// </summary>
    /// <history>
    /// [vipacheco] 08/04/2016 Created
    /// </history>
    private void GiftsReceiptShort_Selected()
    {
      GiftsReceiptsShort _grSelected = (GiftsReceiptsShort)grdReceipts.SelectedItem;

      List<GiftsReceipt> _result = BRGiftsReceipts.GetGiftReceipt(_grSelected.grID);
      _dsGiftsReceipt.Source = _result;

      // Cargamos los regalos del recibo de regalo seleccionado
      _dsGiftsReceiptDetailShort.Source = BRGiftsReceipts.GetGiftsReceiptsDetail(_grSelected.grID);

      // Cargamos los regalos payments
      _dsGiftsReceiptPaymentShort.Source = BRGiftsReceiptsPayments.GetGiftsReceiptPayments(_grSelected.grID);
    }
    #endregion

    private void BookinDeposits_Loaded()
    {
      //Cargamos los depositos
      _lstBookingDesposit = BRBookingDeposits.GetBookingDeposits(_guestID);
      var _resultLst = _lstBookingDesposit.Select(c => new
      {
        bdAmount = c.bdAmount,
        bdReceived = c.bdReceived,
        CxC = c.bdAmount - c.bdReceived,
        bdcu = c.bdcu,
        bdpt = c.bdpt,
        bdpc = c.bdpc,
        bdcc = c.bdcc,
        bdCardNum = c.bdCardNum,
        bdExpD = c.bdExpD,
        bdAuth = c.bdAuth,
        bdFolioCXC = c.bdFolioCXC,
        bdUserCXC = c.bdUserCXC,
        bdEntryDCXC = c.bdEntryDCXC
      }).ToList();

      _dsBookingDeposit.Source = _resultLst;
    }
    
    private void cbogrct_Loaded(object sender, RoutedEventArgs e)
    {
      /*ChargeTo _ChargeTo = (ChargeTo)cbogrct.SelectedItem;

      if (_ChargeTo != null)
      {
        //txtTotalCxC.Text = string.Format("${}", tx);
      }
      */
    }

    private void cbogrct_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (_chargeToChanged >= 1) // Esta validación se realiza para evitar el bug de doble changed del control
      {
        bool _useCxCCost;

        ChargeTo _ChargeTo = (ChargeTo)cbogrct.SelectedItem;

        if (_ChargeTo != null)
        {
          switch (_ChargeTo.ctID)
          {
            case "PR":
            case "LINER":
            case "CLOSER":
              _useCxCCost = true;
              break;
            default:
              _useCxCCost = false;
              break;
          }

          CalculateAllCostsPrices(_useCxCCost);

          CalculateCharge();
        }
        CalculateCharge();
      }
      _chargeToChanged++; // Para el manejador del bug del doble changed del control
    }
    
    private void CalculateAllCostsPrices(bool CalculateAllPrices = false)
    {
      // Recorremos los regalos
      foreach (GiftsReceiptDetailShort item in grdGiftsReceiptDetail.Items)
      {
        // Se verifica si se ingreso los regalos
        if (item.gegi != "")
        {
          // Si es encuentra el regalo
          Gift _gift = BRGifts.GetGiftId(item.gegi);
          if (_gift != null)
          {
            // Costos
            // Si se va a usar costo de empleado
            if (CalculateAllPrices)
            {
              //curC
            }
          }

        }
      }
    }

    #region CalculateCharge
    /// <summary>
    /// Calcula el cargo de regalos segun el tipo de calculo
    /// </summary>
    /// <history>
    /// [vipacheco] 14/abril/2016 Created
    /// </history>
    private void CalculateCharge()
    {
      decimal curCharge = 0;
      decimal curTotalCost = 0;
      decimal curMaxAuthGifts = 0;
      bool blnTour = false;

      ChargeTo _ChargeTo = (ChargeTo)cbogrct.SelectedItem;
      
      curTotalCost = txtTotalCostCaption.Text != "" ? Convert.ToDecimal(txtTotalCostCaption.Text.Trim(new char[] {'$'})) : 0;

      //Establecemos el monto maximo de regalos
      SetMaxAuthGifts();
      curMaxAuthGifts = txtgrMaxAuthGifts.Text != "" ? Convert.ToDecimal(txtgrMaxAuthGifts.Text.Trim(new char[] { '$' })) : 0;

      // Si no es un intercambio de regalos
      if (!_IsExchange)
      {
        // Localizamos a quien se carga
        switch (_ChargeTo.ctCalcType)
        {
          // si el huesped tiene tour el cargo es por el total de regalos menos el monto maximo
          // autorizado. De lo contrario el cargo es por el total de regalos
          case "A":
            // Validamos si tiene tour
            Guest _guest = BRGuests.GetGuest(Convert.ToInt32(txtgrgu.Text));
            blnTour = _guest.guTour;
            if (blnTour)
              curCharge = CalculateChargeBasedOnMaxAuthGifts(curTotalCost, curMaxAuthGifts);
            else
              curCharge = curTotalCost;
            break;
          // El cargo es por el costo total de regalos
          case "B":
            curCharge = curTotalCost;
            break;
          // El cargo es por el costo total de regalos menos el monto maximo autorizado
          case "C":
            curCharge = CalculateChargeBasedOnMaxAuthGifts(curTotalCost, curMaxAuthGifts);
            break;
          // No generan cargo
          case "Z":
            curCharge = 0;
            break;
          default:
            break;
        }
      }
      txtgrCxCGifts.Text = string.Format("{0:0.00}", curCharge);

      // Calculamos el total del cargo
      txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text != "" ? txtgrCxCGifts.Text : "0") + Convert.ToDecimal(txtgrCxCAdj.Text != "" ? txtgrCxCAdj.Text : "0"));
    } 
    #endregion

    #region CalculateChargeBasedOnMaxAuthGifts
    /// <summary>
    /// Calcula el cargo de recibo de regalos basado en el monto maximo de regalos
    /// </summary>
    /// <param name="curTotalCost"></param>
    /// <param name="curMaxAuthGifts"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 14/Abril/2016 Created
    /// </history>
    private decimal CalculateChargeBasedOnMaxAuthGifts(decimal curTotalCost, decimal curMaxAuthGifts)
    {
      // Si el costo total de regalos es mayor al monto maximo autorizado
      if (curTotalCost > curMaxAuthGifts)
        return curTotalCost - curMaxAuthGifts;
      // Si el monto de regalos esta dentro del monto maximo autorizado
      else
        return 0; // Por la naturaleza del calculo el cargo es siempre 0 si el total de regalos no es mayor al autorizado
    } 
    #endregion

    #region btnClose_Click
    /// <summary>
    /// Cierra el formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 09/Abril/2016 Created
    /// </history>
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
    #endregion
    
    #region ValidateNumber
    /// <summary>
    /// Valida que el texto introducido sea solamente de tipo numero
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void ValidateNumber(object sender, TextCompositionEventArgs e)
    {
      string _value = e.Text;

      // Se valida que solamente sean Digitos Numericos
      if (!char.IsDigit(Convert.ToChar(_value)))
      {
        e.Handled = true;
      }
    }
    #endregion

    #region Initialize_Date
    /// <summary>
    /// Inicializa lo DatePicker con la fecha del servidor
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void Initialize_Date(object sender, RoutedEventArgs e)
    {
      dtpTo.Value = frmHost._dtpServerDate;
      dtpCgrDFrom.Value = frmHost._dtpServerDate.AddDays(-7);
    }
    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Busca los Gifts Receipt con los criterios ingresados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      bool blnResult = false;
      if (ValidateCriteria())
      {
        blnResult = Search_GiftsReceipt();

        // Si se encontro el Gift Receipt
        if (blnResult)
        {
          if (modeOpen == EnumModeOpen.Search) // Si esta en modo Search - Con permiso de solo lectura
          {
            Load_grdReceipts(); // Se carga el grid de regalos
          }
          else if (modeOpen == EnumModeOpen.Edit) // Si esta en modo Search - con permiso de edicion
          {
            // Determinamos si el Gift Receipt es editable mediante la fecha de cierre
            bool _iscloded = IsClosed(dtpgrD.Value.Value.Date, _dtpClose);

            if (_iscloded)
            {
              // Habilitamos los botones correspondientes
              ControlBottonEdition(Visibility.Visible, Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Visible,
                                   Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);
            }
            else
            {
              // Habilitamos los botones correspondientes
              ControlBottonEdition(Visibility.Visible, Visibility.Visible, Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Visible,
                                   Visibility.Visible, Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
            }
           
            Load_grdReceipts(); // Se carga el grid de regalos
          }
        }
        // Si no se encontro ninguna coincidencia
        else
        {
          UIHelper.ShowMessage("Not Found Gift Receipt with input specifications", MessageBoxImage.Information);
        }
      }
    }
    #endregion

    #region ValidateCriteria
    /// <summary>
    /// Valida los criterios de busqueda
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private bool ValidateCriteria()
    {
      bool blnValid = true;

      if (dtpCgrDFrom.Value.Value == null)
      {
        UIHelper.ShowMessage("Specify the Start Date.", MessageBoxImage.Information);
        blnValid = false;
      }
      else if (dtpTo.Value.Value == null)
      {
        UIHelper.ShowMessage("Specify the End Date.", MessageBoxImage.Information);
        blnValid = false;
      }
      else if (dtpCgrDFrom.Value.Value > dtpTo.Value.Value)
      {
        UIHelper.ShowMessage("Start Date can not be greater than End Date.", MessageBoxImage.Information);
        blnValid = false;
      }

      return blnValid;
    }
    #endregion

    private void grdGiftsReceiptDetail_RowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
    {
      if (btnEdit.IsVisible)
      {
        if (_giftIDSelected != "")
        {
          DataGrid innerDataGrid = e.DetailsElement as DataGrid;
          var _result = BRGiftsReceiptsPacks.GetGiftsReceiptPackage(Convert.ToInt32(txtgrID.Text), _giftIDSelected);

          if (_result.Count > 0)
          {
            var _r = _result.Select(x => new { gkgi = x.gkgi, gkQty = x.gkQty }).ToList();
            _dsGiftsReceiptPackage.Source = _r;
          }
          else
            _dsGiftsReceiptPackage.Source = null;
        }
      }
    }

    #region ValidateDecimal
    /// <summary>
    /// Valida que el texto introducido sea decimal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void ValidateDecimal(object sender, TextCompositionEventArgs e)
    {
      Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
      e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
    }
    #endregion

    #region ValidateEnter_PreviewKeyDown
    /// <summary>
    /// Recalcula el deposito despues de un cambio en los textbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 15/Abril/2016 Created
    /// </history>
    private void ValidateEnter_PreviewKeyDown(object sender, KeyEventArgs e)

     {
      TextBox _controlSelected = (TextBox)sender;

      if (e.Key == Key.Enter || e.Key == Key.Tab)
      {
        // convertimos el deposito a dolares americanos
        ConvertDepositToUsDlls();

        // si tiene los dos depositos, pone el deposito quemado en cero
        if (Convert.ToDecimal(txtgrDeposit.Text) > 0 && Convert.ToDecimal(txtgrDepositTwisted.Text) > 0)
        {
          if (_controlSelected.Name == "txtgrDeposit")
            txtgrDepositTwisted.Text = string.Format("{0:0.00}", 0);

          if (_controlSelected.Name == "txtgrDepositTwisted")
            txtgrDeposit.Text = string.Format("{0:0.00}", 0);
        }
      }
    } 
    #endregion

    #region cbogrcu_SelectionChanged
    /// <summary>
    /// Metodo Evento cambio de combo currency
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 15/Abril/2016 Created
    /// </history>
    private void cbogrcu_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // convertimos el deposito a dolares americanos
      ConvertDepositToUsDlls();
    }
    #endregion

    private void txtgrCxCAdj_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter || e.Key == Key.Tab)
      {
        txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text) + Convert.ToDecimal(txtgrCxCAdj.Text));
      }
    }


    private void txtgrcxcAdj_Validate()
    {
      if (txtgrCxCAdj.Text != "")
      {
        txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text) + Convert.ToDecimal(txtgrCxCAdj.Text));
      }
      
    }

    #region btnCancel_Click
    /// <summary>
    /// Permite cancelar un recibo de regalos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (UIHelper.ShowMessage("Are you sure you want to cancel this receipt? \r\n This change can not be undone.", MessageBoxImage.Question) == MessageBoxResult.Yes)
      {
        // si no tiene regalos pendientes por cancelar en los sistemas externos
        if (CancelExternalProducts())
        {

        }
      }
    }
    #endregion

    #region CancelExternalProducts
    /// <summary>
    /// Cancela los productos de sistemas externos
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private bool CancelExternalProducts()
    {
      int rowCountgeInElectronicPurse = 0, rowcountgeCancelElectronicPurse = 0, rowCountgeInPVPPromo = 0, rowCountgeCancelPVPPromo = 0;
      foreach (GiftsReceiptDetailShort item in grdGiftsReceiptDetail.Items)
      {
        rowCountgeInElectronicPurse += Convert.ToInt32(item.geInElectronicPurse);
        rowcountgeCancelElectronicPurse += Convert.ToInt32(item.geCancelElectronicPurse);
        rowCountgeInPVPPromo += Convert.ToInt32(item.geInPVPPromo);
        rowCountgeCancelPVPPromo += Convert.ToInt32(item.geCancelPVPPromo);
      }
      // si tiene regalos pendientes por cancelar en el monedero electronico
      if (rowCountgeInElectronicPurse > rowcountgeCancelElectronicPurse)
      {
        return true; 
      }
      // Si tiene regalos pendientes por cancelar en Sistur
      else if (rowCountgeInPVPPromo > rowCountgeCancelPVPPromo)
      {
        return true;
      }
      // si no tiene regalos pendientes por cancelar en los sistemas externos
      else
        return true;
    }

    #endregion

    #region btnCancelSisturPromotions_Click
    /// <summary>
    /// Despliega el formulario de cancelacion de promociones de Sistur
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnCancelSisturPromotions_Click(object sender, RoutedEventArgs e)
    {
      // si los datos son validos
      if (ValidateCancelSisturPromotions())
      {
        // desplegamos el formulario de cancelacion de productos externos
        //----------------------------------------------------------------------------------------> Falta OJO Checar (btnCancelSisturPromotions_Click())

        //---------------------------------------------------------------------------------------->
      }
    }
    #endregion

    #region ValidateCancelSisturPromotions
    /// <summary>
    /// Valida si se permite cancelar los regalos con promociones relacionadas
    /// </summary>
    /// <returns> True- Valido | False - No valido </returns>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private bool ValidateCancelSisturPromotions()
    {
      int rowCountgeInPVPPromo = 0, rowCountgeCancelPVPPromo = 0;
      foreach (GiftsReceiptDetailShort item in grdGiftsReceiptDetail.Items)
      {
        rowCountgeInPVPPromo += Convert.ToInt32(item.geInPVPPromo);
        rowCountgeCancelPVPPromo += Convert.ToInt32(item.geCancelPVPPromo);
      }

      // validamos si tiene permiso especial de recibos de regalos
      if (!App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Special))
      {
        UIHelper.ShowMessage("Access denied", MessageBoxImage.Exclamation);
        return false;
      }
      // validamos si tiene regalos de promociones de Sistur que no estan cancelados
      else if (rowCountgeInPVPPromo > rowCountgeCancelPVPPromo)
      {
        UIHelper.ShowMessage("No gifts to cancel from the Sistur promotion.", MessageBoxImage.Exclamation);
        return false;
      }
      return true;
    }
    #endregion

    /// <summary>
    /// Despliega el formulario de cancelacion de productos del monedero electronico
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnCancelElectronicPurse_Click(object sender, RoutedEventArgs e)
    {
      // si los datos son validos
      if (ValidateCancelElectronicPurse())
      {
        // desplegamos el formulario de cancelacion de productos externos
        //----------------------------------------------------------------------------------------> Falta OJO Checar (btnCancelElectronicPurse_Click())

        //---------------------------------------------------------------------------------------->
      }
    }

    #region ValidateCancelElectronicPurse
    /// <summary>
    /// Valida si se permite cancelar los regalos de un monedero electronico
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    public bool ValidateCancelElectronicPurse()
    {
      int rowCountgeInElectronicPurse = 0, rowcountgeCancelElectronicPurse = 0;
      foreach (GiftsReceiptDetailShort item in grdGiftsReceiptDetail.Items)
      {
        rowCountgeInElectronicPurse += Convert.ToInt32(item.geInElectronicPurse);
        rowcountgeCancelElectronicPurse += Convert.ToInt32(item.geCancelElectronicPurse);
      }

      // validamos si tiene permiso especial de recibos de regalos
      if (!App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Special))
      {
        UIHelper.ShowMessage("Access Denied.", MessageBoxImage.Exclamation);
        return false;
      }
      else if (rowCountgeInElectronicPurse > rowcountgeCancelElectronicPurse)
      {
        UIHelper.ShowMessage("No gifts to cancel from the electronic purse.", MessageBoxImage.Exclamation);
        return false;
      }
      return true;
    }
    #endregion

    /// <summary>
    /// Despliega el formulario del monedero electronico
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnEPurse_Click(object sender, RoutedEventArgs e)
    {
      // si los datos son validos
      if (ValidateEPurse())
      {
        //----------------------------------------------------------------------------------------> Falta OJO Checar (btnEPurse_Click())

        //---------------------------------------------------------------------------------------->
      }
    }

    #region ValidateEPurse
    /// <summary>
    /// Valida si se permite guardar los regalos en un monedero electronico
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private bool ValidateEPurse()
    {
      // Buscamos el guest con el Id correspondiente.
      Guest _guest = BRGuests.GetGuest(Convert.ToInt32(txtgrgu.Text));

      // Validamos si tiene show.
      if (!_guest.guShow)
      {
        UIHelper.ShowMessage("Invitation without show.", MessageBoxImage.Exclamation);
        return false;
      }
      return true;
    }

    #endregion

    /// <summary>
    /// Despliega el historico de un recibo de regalos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      if (txtgrID.Text != "")
      {
        //----------------------------------------------------------------------------------------> Falta OJO Checar (btnLog_Click())

        //---------------------------------------------------------------------------------------->
      }
      else
        UIHelper.ShowMessage("Select a receipt", MessageBoxImage.Information);
    }

    private void btnExchange_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    ///  Permite Guardar los cambios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (Validate())
      {

      }
    }

    /// <summary>
    /// Valida los datos
    /// </summary>
    /// <returns></returns>
    private bool Validate()
    {
      if (!ValidateChangedBy(txtChangedBy, txtPwd))
        return false;
      else if (!ValidateGeneral())
      {
        return false;
      }
      return true;
    }

    #region ValidateChangedBy
    /// <summary>
    /// Valida que se ingrese quien hizo el cambio y su contraseña
    /// </summary>
    /// <param name="changeBy"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private bool ValidateChangedBy(TextBox changeBy, PasswordBox password)
    {
      if (!ValidateHelper.ValidateChangedBy(changeBy, password, App.User.User.peN))
        return false;

      // Se consulta con la informacion ingresada.
      ValidationData _validate = BRHelpers.ValidateChangedByExist(txtChangedBy.Text, EncryptHelper.Encrypt(txtPwd.Password), App.User.SalesRoom.srID).Single();

      // Se verifica si se encontro algun resultado.
      if (_validate.Focus != "" && _validate.Message != "")
        return false;
      else
        return true;
    } 
    #endregion

    private bool ValidateGeneral()
    {
      // validamos los regalos - RG(ReceiptGifts)
      if (!ValidateGrid("RG", ref grdGiftsReceiptDetail))
        return false;

      // validamos los pagos - GRPayments(GiftsReceiptsPayments)
      else if (!ValidateGrid("GRPayments", ref dgGiftsReceiptPaymentShort))
        return false;


      // validamos que los pagos cubran el importe a pagar
      else if (Convert.ToDecimal(txtTotalToPayCaption.Text.Trim(new char[] { '$' })) > 0 &&
               Convert.ToDecimal(txtTotalToPayCaption.Text.Trim(new char[] { '$' })) > Convert.ToDecimal(txtTotalPaymentsCaption.Text.Trim(new char[] { '$' })))
      {
        UIHelper.ShowMessage("The payments do not cover the amount to pay", MessageBoxImage.Exclamation);
        return false;
      }

      // validamos que los datos del recibo de regalos existan
      else if (!ValidateExist())
        return false;

      return true;
    }

    /// <summary>
    /// Valida que los datos de un recibo de regalos existan
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private bool ValidateExist()
    {
      SalesRoomShort _sr = (SalesRoomShort)cboSalesRoom.SelectedItem;

      ValidationData _validation = BRGiftsReceipts.ValidateGiftsReceipt(txtChangedBy.Text, EncryptHelper.Encrypt(txtPwd.Password), Convert.ToInt32(txtgrgu.Text),
                                                                        txtgrlo.Text, _sr.srID, txtgrHost.Text, txtgrpe.Text);

      // si hay un error
      if (_validation.Focus != "")
      {
        // desplegamos el mensaje de error
        UIHelper.ShowMessage(_validation.Message, MessageBoxImage.Information);

        // establecemos el foco en el control que tiene el error
        switch (_validation.Focus)
        {
          case "ChangedBy":
            txtChangedBy.Focus();
            break;
          case "Password":
            txtPwd.Focus();
            break;
          case "Guest":
            txtgrgu.Focus();
            break;
          case "SalesRoom":
            cboSalesRoom.Focus();
            break;
          case "Location":
            txtgrlo.Focus();
            break;
          case "Personnel":
            txtgrpe.Focus();
            break;
          case "GiftsHost":
            txtgrHost.Focus();
            break;
        }
      }
      return true;
    }

    private bool ValidateGrid(string gridValidate, ref DataGrid datagrid)
    {
      int i = 0, j = 0;

      switch(gridValidate.ToUpper())
      {
        case "RG": // Grid ReceiptGifts

          grdGiftsReceiptDetail.IsReadOnly = true;
          foreach (GiftsReceiptDetailShort _gr in grdGiftsReceiptDetail.Items)
          {
            foreach (GiftsReceiptDetailShort _gr2 in grdGiftsReceiptDetail.Items)
            {
              if (i != j)
              {
                // si se ingreso un regalo y es el mismo regalo
                if (_gr.gegi == _gr2.gegi && _gr.gegi != "")
                {
                  // si el regalo esta cargado al mismo sector
                  if (_gr.gect != "" && _gr2.gect != "")
                  {
                    // si el regalo esta cargado al mismo sector
                    if (_gr.gect == _gr2.gect)
                    {
                      Gift _gift = BRGifts.GetGiftId(_gr.gegi);
                      UIHelper.ShowMessage("Gifts must not be repeated. \r\n Gift repetead is '" + _gift.giN + "'.", MessageBoxImage.Exclamation);
                      grdGiftsReceiptDetail.IsReadOnly = false;
                      return false;
                    }
                  }
                  // si el regalo no esta cargado a un sector
                  else
                  {
                    UIHelper.ShowMessage("Select an option from the Charge to list.", MessageBoxImage.Exclamation);
                    grdGiftsReceiptDetail.IsReadOnly = false;

                    return false;
                  }

                  // si el regalo esta cargado a una persona
                  if (_gr.geCxC != "")
                  {
                    // Localizamos a la persona
                    Personnel _personnel = BRPersonnel.GetPersonnelById(_gr.geCxC);

                    // Si no encontrol a la persona
                    if (_personnel == null)
                    {
                      UIHelper.ShowMessage("The personnel ID doesn't exist.", MessageBoxImage.Exclamation);
                      grdGiftsReceiptDetail.IsReadOnly = false;
                      return false;
                    }
                  }
                }
              }
              j++;
            }
            j = 0;
            i++;
          }

          // Si se debe validar el monto maximo de regalos
          if (_blnValidateMaxAuthGifts)
          {
            // validamos el monto maximo de regalos
            return ValidateMaxAuthGifts(txtTotalCostCaption.Text, txtgrMaxAuthGifts.Text);
          }

          // Si hay GuestStatus o se debe validar
          if (_blnApplyGuestStatusValidation)
          {
            return ValidateGiftsGuestStatus();
          }
          break;
        case "GRPAYMENTS": // Grid GiftsReceiptsPayments
          return ValidatePayments();
      }

      return true;
    }

    private void Expander_Expanded(object sender, RoutedEventArgs e)
    {
      if (btnEdit.IsVisible)
      {
        for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
          if (vis is DataGridRow)
          {
            var row = (DataGridRow)vis;
            GiftsReceiptDetailShort _gifts = (GiftsReceiptDetailShort)row.DataContext;
            _giftIDSelected = _gifts.gegi;
            row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            break;
          }
      }
    }

    private void Expander_Collapsed(object sender, RoutedEventArgs e)
    {
      if (btnEdit.IsVisible)
      {
        for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
          if (vis is DataGridRow)
          {
            var row = (DataGridRow)vis;
            _giftIDSelected = "";
            _dsGiftsReceiptPackage.Source = null;
            row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            break;
          }
      }
    }


    private bool ValidatePayments()
    {
      if (!ValidateGridPayments())
        return false;

      return true;
    }


    private bool ValidateGridPayments()
    {
      int i = 0, j = 0;
      string currency = "", Payment = "", Source = "";

      // Se valida si un elemento esta repetido
      string message = "Payments must not be repeated.\r\n Payment repetead is ";

      dgGiftsReceiptPaymentShort.IsReadOnly = true;
      // si esta repetido algun elemento
      foreach (GiftsReceiptPaymentShort _gr in dgGiftsReceiptPaymentShort.Items)
      {
        foreach (GiftsReceiptPaymentShort _gr2 in dgGiftsReceiptPaymentShort.Items)
        {
          if (i != j)
          {
            // si se ingreso el mismo currency
            if (_gr.gycu == _gr2.gycu)
            {
              currency = _gr.gycu;
            }
            if (_gr.gypt == _gr2.gypt)
            {
              Payment = _gr.gypt;
            }
            if (_gr.gysb == _gr2.gysb)
            {
              Source = _gr.gysb;
            }
          }

          if (currency != "" || Payment != "" || Source != "")
          {
            message += currency + " " + Payment + " " + Source;
            dgGiftsReceiptPaymentShort.IsReadOnly = false;

            return false;
          }
          j++;
        }
        j = 0;
        i++;
      }

      return true;
    }

    #region ValidateGiftsGuestStatus
    /// <summary>
    /// Valida la informacion del GuestStaus x los regalos || Valida los regalos y el GuestStatus
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private bool ValidateGiftsGuestStatus()
    {
      int i, iToursUsed, iDiscsUsed, iTourAllowed, iTours, iTCont = 0, iDCont = 0, iMaxTours;
      decimal iPax, iDiscAllowed, iDisc;
      bool? blnDisc;
      string strMsg = "";

      // Aisgnamos los valores del GuestStatus para validar
      iMaxTours = (int)_GuesStatusInfo.gsMaxQtyTours;
      iToursUsed = _GuesStatusInfo.TourUsed;
      blnDisc = _GuesStatusInfo.gsAllowTourDisc;
      iDiscsUsed = _GuesStatusInfo.DiscUsed;
      iPax = _GuesStatusInfo.guPax;


      // Los descuentos permitidos son los restantes de los PAX restantes
      iDiscAllowed = iPax - iMaxTours;
      iDiscAllowed = iDiscAllowed - iDiscsUsed;
      // Los Tours permitidos
      iTourAllowed = iMaxTours - iToursUsed;

      iTours = iTourAllowed;
      iDisc = iDiscAllowed;

      // Obtenemos los reglos que hay
      List<GiftsReceiptDetailShort> _lstGRD = new List<GiftsReceiptDetailShort>();
      foreach (GiftsReceiptDetailShort item in grdGiftsReceiptDetail.Items)
      {
        _lstGRD.Add(item);
      }

      List<Gift> _gifts = new List<Gift>();
      if (_lstGRD != null)
      {
        if (_lstGRD.Count > 0)
        {
          _gifts = BRGifts.GetGiftsInputList(_lstGRD);
        }
      }

      // Validamos con cada registro
      foreach (GiftsReceiptDetailShort _item in grdGiftsReceiptDetail.Items)
      {
        Gift _giftResult = BRGifts.GetGiftId(_item.gegi);

        if (_giftResult != null)
        {
          if (_giftResult.gigc == "TOURS" && (bool)_giftResult.giDiscount)
          {
            iDisc = iDisc - (_giftResult.giQty * _item.geQty);
            iDCont = iDCont + 1;
          }
          else if (_giftResult.gigc == "TOURS" && !(bool)_giftResult.giDiscount)
          {
            iTours = iTours - (_giftResult.giQty * _item.geQty);
            iTCont = iTCont + 1;
          }
        }
      }

      //Revisamos el remanente de la revision de Gifts
      if (iTours < 0)
        strMsg = "The maximum number of tours " + iTourAllowed + " has been exceeded. \r\n There are " + iTCont + " tours on this receipt";

      if (iDisc < 0 && strMsg == "")
        strMsg = "The maximum number of discount tours " + iDiscAllowed + " has been exceeded.\r\n There are " + iDCont + " discount tours on this receipt";


      //Revisamos el remanente de la revision de Gifts
      if (strMsg != "")
      {
        UIHelper.ShowMessage(strMsg, MessageBoxImage.Exclamation);
        return false;
      }
      else
        return true;
    } 
    #endregion

    #region ValidateMaxAuthGifts
    /// <summary>
    /// Valida el monto maximo de regalos
    /// </summary>
    /// <param name="totalGifts"></param>
    /// <param name="maxAuthGifts"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private bool ValidateMaxAuthGifts(string totalGifts, string maxAuthGifts)
    {
      decimal pcurTotalGifts = Convert.ToDecimal(totalGifts.Trim(new char[] { '$' }));
      decimal pcurMaxAuthGifts = Convert.ToDecimal(maxAuthGifts.Trim(new char[] { '$' }));

      // si se rebasa el monto maximo de regalos
      if (pcurTotalGifts > pcurMaxAuthGifts)
      {
        decimal curCharge = pcurTotalGifts - pcurMaxAuthGifts;
        string message = "The maximum amount authorized of gifts has been exceeded. \r\n" +
                              "Max authorized = " + String.Format("{0:C2", pcurMaxAuthGifts) + "\r\n" +
                              "Total Gifts = " + string.Format("{0:C2}", pcurTotalGifts) + "\r\n" +
                              "It will generate a charge of " + string.Format("{0:C2}", curCharge) + " to PR \r\n Save anyway?";


        return UIHelper.ShowMessage(message, MessageBoxImage.Question) == MessageBoxResult.Yes ? true : false;
      }

      return true;
    } 
    #endregion

    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      // Activamos controles de Grid Gifts
      controlBottonEditionGifts(Visibility.Visible, Visibility.Visible,Visibility.Hidden, Visibility.Hidden);

      //Ajustamos el margen del grid
      Thickness _margin = grdGiftsReceiptDetail.Margin;
      _margin.Left = 0;
      _margin.Top = 27;
      _margin.Right = 9;
      _margin.Bottom = 25;
      grdGiftsReceiptDetail.Margin = _margin;

      bool blnEnable;

      // Grid principal
      grdReceipts.IsReadOnly = true;

      // Criterios de busqueda
      ControlVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);

      // Botones
      ControlBottonEdition(Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Visible, Visibility.Hidden, Visibility.Hidden,
                           Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);
      btnClose.Visibility = Visibility.Hidden;


      // Autentificacion automatica
      if (App.User.AutoSign)
      {
        this.txtChangedBy.Text = App.User.User.peID;
        this.txtPwd.Password = App.User.User.pePwd;
      }

      //int i = 0;
      ////Inhabilitamos los controles necesarios con promosiones SISTUR
      //foreach (GiftsReceiptDetailShort _item in grdGiftsReceiptDetail.Items)
      //{
      //  DataGridRow _row = (DataGridRow)grdGiftsReceiptDetail.ItemContainerGenerator.ContainerFromIndex(i);
      //  EnableCellGrid(_item.geInPVPPromo, _item, ref _row);

      //  i++;
      //}


      // Determinamos si se puede modificar el recibo
      blnEnable = EnableEdit();

      // Habilitamos los controles que se pueden editar

      if (blnEnable)
      {
        txtgrNum.IsReadOnly = false;
        txtgrMemberNum.IsReadOnly = false;
        dtpgrD.IsReadOnly = false;
        cboSalesRoom.IsReadOnly = false;
        txtgrGuest2.IsReadOnly = false;
        txtgrRoomNum.IsReadOnly = false;
        txtgrRoomNum.IsReadOnly = false;
        txtgrPax.IsReadOnly = false;
        txtgrTaxiOut.IsReadOnly = false;
        txtgrTaxiOutDiff.IsReadOnly = false;
        ControlEnable(false, false, true, true, true, true);
        ControlVisibility(Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);
        ControlsReadOnly(false, false, false);
      }

      // Guest ID
      txtgrgu.IsEnabled = blnEnable;


      // si no tiene Guest ID, permitimos modificar la locacion
      //txtgrlo.IsEnabled = blnEnable && txtgrgu.Text == "";


      // si no tiene Guest ID o si tiene permiso especial de recibos de regalos,
      // permitimos modificar la sala de ventas
      //txtgrsr.IsEnabled = blnEnable && (txtgrgu.Text == "" || App.User.HasPermission(EnumPermission.Gifts, EnumPermisionLevel.SuperSpecial));

      // Charge To
      cbogrct.IsEnabled = blnEnable;


      // si no es un recibo de intercambio de regalos, permitimos modificar los depositos y los montos de taxi
      //EnableDepositsTaxis(blnEnable); // && chkgrExchange.IsChecked.Value == false;
    }

    private void EnableCellGrid(bool inSistur, GiftsReceiptDetailShort giftsReceipt, ref DataGridRow rowCurrent)
    {
      int i = 0;
      // Obtenemos el Gift para evaluar los permisos
      Gift _gifts = _lstGifts.Where(x => x.giID == giftsReceipt.gegi).SingleOrDefault();

      // recorremos las columnas del grid
      foreach (var _item in grdGiftsReceiptDetail.Columns)
      {
        DataGridCell _cell = GridHelper.GetCell(grdGiftsReceiptDetail, rowCurrent, i);

        if (_cell != null)
        {

          if (inSistur)
          {
            switch (_cell.Column.SortMemberPath)
            {
              case "geQty":
              case "gegi":
              case "gePriceA":
              case "gePriceM":
              case "gePriceAdult":
              case "gePriceMinor":
              case "gePriceExtraAdult":
              case "geSale":
                _cell.IsEnabled = false;
                break;
              case "geFolios":
                if (_gifts.giWFolio)
                  _cell.IsEnabled = true;
                else
                  _cell.IsEnabled = false;
                break;
              case "geAdults":
              case "geMinors":
                if (_gifts.giWPax)
                  _cell.IsEnabled = true;
                else
                  _cell.IsEnabled = false;
                break;
              default:
                break;
            }
          }
          else
          {
            switch (_cell.Column.SortMemberPath)
            {
              case "gePriceA":
              case "gePriceM":
              case "gePriceAdult":
              case "gePriceMinor":
              case "gePriceExtraAdult":
              case "geSale":
                _cell.IsEnabled = true;
                break;
              case "geFolios":
                if (_gifts.giWFolio)
                  _cell.IsEnabled = true;
                else
                  _cell.IsEnabled = false;
                break;
              case "geAdults":
              case "geMinors":
                if (_gifts.giWPax)
                  _cell.IsEnabled = true;
                else
                  _cell.IsEnabled = false;
                break;

              default:
                break;
            }
          }
        }
        i++;
      }
    }

    private void EnableDepositsTaxis(bool pblnEnable)
    {
      // Depositos
      txtgrDeposit.IsEnabled = pblnEnable;
      txtgrDepositTwisted.IsEnabled = pblnEnable;
      cbogrcu.IsEnabled = pblnEnable;
      cbogrpt.IsEnabled = pblnEnable;
    }

    #region EnableEdit
    /// <summary>
    /// Determina si se debe permitir modificar los datos de un recibo de regalos
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private bool EnableEdit()
    {
      // si el recibo es de una fecha cerrada
      if (IsClosed(dtpgrD.Value.Value.Date, _dtpClose))
        return false;

      // si el recibo no es nuevo
      else if (txtgrID.Text != "")
      {
        // si el recibo es de una fecha anterior o igual a la fecha de cierre
        if (dtpgrD.Value.Value.Date <= _dtpClose)
          return false;

        // si el recibo es de una fecha posterior a la fecha de cierre
        else
        {
          // si el recibo no es de hoy y no tiene permiso especial de recibos de regalos
          if (dtpgrD.Value.Value.Date != frmHost._dtpServerDate && !App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Special))//  .Permissions.GiftsReceipts < perSpecial Then
            return false;
        }
      }
      return true;
    } 
    #endregion

    #region IsClosed
    /// <summary>
    /// Determina si una entidad esta cerrada
    /// </summary>
    /// <param name="date"></param>
    /// <param name="dateClosed"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016
    /// </history>
    private bool IsClosed(DateTime date, DateTime? dateClosed)
    {
      // Si la fecha es menor o igual a la fecha de cierre
      if (date <= _dtpClose)
      {
        return true;
      }
      
      return false;
    } 
    #endregion

    /// <summary>
    /// Crea un nuevo Gift Receipt
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// </history>
    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
      /*
      frmGiftsReceiptsDetail _frmGiftReceiptAdd = new frmGiftsReceiptsDetail();
      _frmGiftReceiptAdd.ShowInTaskbar = false;
      _frmGiftReceiptAdd.Owner = this;
      _frmGiftReceiptAdd.modeOpen = EnumModeOpen.Add;
      _frmGiftReceiptAdd.ShowDialog();     */

      grdGiftsReceiptDetail.ItemsSource = null;
      grdGiftsReceiptDetail.IsReadOnly = false;
      //grdGiftsReceiptDetail.Items.Clear();
      //dgGiftsReceiptPaymentShort.Items.Clear();
    }

    #region btnUndo_Click
    /// <summary>
    /// Cancela alguna edicion en proceso
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private void btnUndo_Click(object sender, RoutedEventArgs e)
    {
      // Habitilitamos e Inhabilitamos las opciones correspondientes a la opcion
      ControlsReadOnly(true, true, true);
      ControlEnable(false, false, false, false, false, false);
      ControlBottonEdition(Visibility.Visible, Visibility.Visible, Visibility.Visible, Visibility.Hidden, Visibility.Hidden,
                           Visibility.Visible, Visibility.Visible, Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
      btnClose.Visibility = Visibility.Visible;
      ControlVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);

      // Activamos controles de Grid Gifts
      controlBottonEditionGifts(Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);

      //Ajustamos el margen del grid
      Thickness _margin = grdGiftsReceiptDetail.Margin;
      _margin.Left = 0;
      _margin.Top = 2;
      _margin.Right = 9;
      _margin.Bottom = 25;
      grdGiftsReceiptDetail.Margin = _margin;

      txtgrNum.IsReadOnly = true;
      txtgrMemberNum.IsReadOnly = true;
      dtpgrD.IsReadOnly = true;
      cboSalesRoom.IsReadOnly = true;
      txtgrGuest2.IsReadOnly = true;
      txtgrRoomNum.IsReadOnly = true;
      txtgrRoomNum.IsReadOnly = true;
      txtgrPax.IsReadOnly = true;
      txtgrTaxiOut.IsReadOnly = true;
      txtgrTaxiOutDiff.IsReadOnly = true;

      // Autentificacion
      txtChangedBy.Text = "";
      txtPwd.Password = "";

      Load_grdReceipts(); // Se carga el grid de regalos
    }
    #endregion

    private void grdGiftsReceiptDetail_AddingNewItem(object sender, AddingNewItemEventArgs e)
    {
      e.NewItem = new GiftsReceiptDetailShort {gect = "MARKETING" };
    }
    /*
    private void SomeSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var comboBox = sender as ComboBox;
      if (comboBox.SelectedItem != null)
      {
        DataGridRow _row = (DataGridRow)grdGiftsReceiptDetail.ItemContainerGenerator.ContainerFromItem(grdGiftsReceiptDetail.SelectedItem);

        // Cambios de Precio
        var xx = _row.DataContext;
        DataGridCell _cell = GridHelper.GetCell(grdGiftsReceiptDetail, _row, 7);
        GiftsReceiptDetailShort _gus;
        if (xx != null)
        {
          _gus = (GiftsReceiptDetailShort)xx;

          _gus.gePriceA = 500;
          _row.DataContext = _gus;
        }

        DataGridCell _cell = GridHelper.GetCell(grdGiftsReceiptDetail, _row, 7);

        if (_cell.Column is DataGridTextColumn)
        {
          DataGridColumn _cbo = (DataGridTextColumn)_cell.Column;
          
        }    

      }
    }
     */

    private void grdGiftsReceiptDetail_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      // Only act on Commit
      if (e.EditAction == DataGridEditAction.Commit)
      {
        GiftsReceiptDetailShort driver = e.Row.DataContext as GiftsReceiptDetailShort;


        //driver.Save();
      }
    }

    private void grdGiftsReceiptDetail_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {

    }

    private void DataGrid_CellGotFocus(object sender, RoutedEventArgs e)
    {
      /*
      // Lookup for the source to be DataGridCell
      if (e.OriginalSource.GetType() == typeof(DataGridCell))
      {
        // Starts the Edit on the row;
        DataGrid grd = (DataGrid)sender;
        grd.BeginEdit(e);

        Control control = GridHelper.GetVisualChild<Control>(e.OriginalSource as DataGridCell);
        if (control != null)
        {
          GiftsReceiptDetailShort _gift = (GiftsReceiptDetailShort)control.DataContext;
          control.Focus();

          if (_gift.geInPVPPromo)
          {
            control.IsEnabled = true;
          }
        }
      }      */
    }

    private void DriversDataGrid_PreviewDeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
    {
      if (e.Command == DataGrid.DeleteCommand)
      {
        if (!(MessageBox.Show("Are you sure you want to delete?", "Please confirm.", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
        {
          // Cancel Delete.
          e.Handled = true;
        }
      }
      /*
      else if (e.Command == ScrollBar.LineDownCommand || e.Command == ScrollBar.LineUpCommand || e.Command == ScrollBar.PageDownCommand || e.Command == ScrollBar.PageUpCommand || e.Command == ScrollBar.ScrollToVerticalOffsetCommand)
      {
        //Inhabilitamos los controles necesarios con promosiones SISTUR
        for (int i = 0; i < grdGiftsReceiptDetail.Items.Count-2; i++)
        {

          DataGridRow _row = (DataGridRow)grdGiftsReceiptDetail.ItemContainerGenerator.ContainerFromIndex(i);

          if (_row != null)
          {
            GiftsReceiptDetailShort _item = (GiftsReceiptDetailShort)_row.DataContext;
            EnableCellGrid(_item.geInPVPPromo, _item, ref _row);
          }
          
        }
      }  */
    }

    private void btnRemoveGift_Click(object sender, RoutedEventArgs e)
    {
      // Habilitamos y deshabilitamos las opciones de remove Gifts
      chkDelete.Visibility = Visibility.Visible;
      btnRemoveGift.Visibility = Visibility.Hidden;
      btnAddGift.Visibility = Visibility.Hidden;
      btnSaveGift.Visibility = Visibility.Visible;
      btnCancelGift.Visibility = Visibility.Visible;
    }

    private void btnCancelGift_Click(object sender, RoutedEventArgs e)
    {
      // Habilitamos y deshabilitamos las opciones de remove Gifts
      chkDelete.Visibility = Visibility.Hidden;
      btnRemoveGift.Visibility = Visibility.Visible;
      btnAddGift.Visibility = Visibility.Visible;
      btnSaveGift.Visibility = Visibility.Hidden;
      btnCancelGift.Visibility = Visibility.Hidden;
    }

  }
}

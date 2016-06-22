using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Classes;
using IM.Model;
using IM.Model.Enums;
using IM.Services.Helpers;
using PalaceResorts.Common.PalaceTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmCancelExternalProducts.xaml
  /// </summary>
  public partial class frmCancelExternalProducts : Window
  {

    #region Variables
    private EnumExternalProduct _EnumExternalProduct;
    private int _ReceiptID, _GuestID;
    public bool _ValidateMaxAuthGifts, _UseCxCCost, _Exchange, _Cancelled;
    public MessageBoxResult _Result = MessageBoxResult.Cancel; // Variable encargado de devolver la respuesta del usuario.
    public int ReceiptExchangeID = 0;
    public frmGiftsReceipts _FrmGiftsReceipt;
    public frmCancelExternalProducts _FrmCancelExternalProducts;

    GuestShort _Guest; // Variable que contendra la informacion del Guest.

    private string _CancelField; // Nombre del campo que indica si el regalo esta cancelado en el sistema externo
    private string PropertyOpera; // Propiedad de Opera asociada a la sala de ventas
    private EnumPromotionsSystem _PromotionsSystem;// Sistema de promociones

    CollectionViewSource _dsSalesRoom;
    CollectionViewSource _dsLeadSource;
    CollectionViewSource _dsPrograms;
    CollectionViewSource _dsGiftsReceiptDetailCancel;
    CollectionViewSource _dsGifts;
    CollectionViewSource _dsGiftInvitationWithoutReceipt;

    #endregion

    #region Contructor
    public frmCancelExternalProducts(EnumExternalProduct enumExternalProducts, int ReceiptID, int GuestID,
                                 string NameGuest, decimal MaxAuthGifts, decimal TotalGifts, decimal CurAdjustment,
                                 bool ValidateMaxAuthGifts, bool UseCxCCost, bool Exchange, frmGiftsReceipts FrmGiftsReceipt)
    {
      _EnumExternalProduct = enumExternalProducts;
      _ReceiptID = ReceiptID;
      _GuestID = GuestID;
      _ValidateMaxAuthGifts = ValidateMaxAuthGifts;
      _UseCxCCost = UseCxCCost;
      _Exchange = Exchange;
      _Cancelled = false;
      _FrmGiftsReceipt = FrmGiftsReceipt;
      _FrmCancelExternalProducts = this;

      InitializeComponent();

      // Si es monedero electronico
      if (_EnumExternalProduct == EnumExternalProduct.expElectronicPurse)
      {
        this.Title = "Cancel Electronic Purse";
        _CancelField = "geCancelElectronicPurse";
        geCancelPVPPromoColumn.Visibility = Visibility.Hidden;
      }
      // si son las promociones de Sistur
      else
      {
        this.Title = "Cancel Sistur Promotions";
        _CancelField = "geCancelPVPPromo";
        geCancelElectronicPurseColumn.Visibility = Visibility.Hidden;
      }

      //Obtenemos los datos del Huesped.
      _Guest = BRGuests.GetGuestShort(_GuestID);
      txtReceipt.Text = $"{_ReceiptID}";
      txtGuestID.Text = $"{_GuestID}";
      txtNameInvitation.Text = _Guest.Name;
      cboSalesRoom.SelectedValue = _Guest.gusr;

      // Si es Inhouse
      if (_Guest.lspg.Equals("IH"))
      {
        // Si es una invitacion inhouse externa
        if (string.IsNullOrEmpty(_Guest.guHReservID))
          PropertyOpera = _Guest.lsPropertyOpera;
      }
      // Si es OutHouse
      else
        PropertyOpera = _Guest.srPropertyOpera;

      cboProgram.SelectedValue = _Guest.lspg;
      cboLeadSource.SelectedValue = _Guest.gulsOriginal;
      txtReservation.Text = _Guest.guHReservID == "" ? null : _Guest.guHReservID;
      dtpArrival.Value = _Guest.guCheckInD;
      dtpDeparture.Value = _Guest.guCheckOutD;
      txtQtyEPurses.Text = $"{_Guest.guQtyGiftsCard}";
      txtAccountInvitation.Text = _Guest.guAccountGiftsCard == "" ? null : _Guest.guAccountGiftsCard;

      // Por default cancelamos las promociones en el sistemas de promociones de PVP
      _PromotionsSystem = EnumPromotionsSystem.PVP;

      // Si es Inhouse
      if (_Guest.lspg.Equals("IH"))
      {
        if ((bool)_Guest.lsUseSistur)
          _PromotionsSystem = EnumPromotionsSystem.Sistur;
      }
      // Si es OutHouse
      else
      {
        if ((bool)_Guest.srUseSistur)
          _PromotionsSystem = EnumPromotionsSystem.Sistur;
      }

      // Monto maximo de reagalos
      txtMaxAuthGifts.Text = $"${MaxAuthGifts}";

      // Totales de regalos
      txtTotalGiftsInvitation.Text = $"${BRGiftsReceipts.CalculateTotalsGiftsInvitation(_GuestID)}";

      // Si se genera un recibo Exchange
      if (_Exchange)
        txtTotalGiftsCancel.Text = $"${0.00}";
      // si se desea cancelar el recibo
      else
        txtTotalGiftsCancel.Text = $"${TotalGifts}";

      txtTotalGiftsExchange.Text = $"${0.00}";
      CalculateTotalGifts();

      // Totales del cargo
      txtgrcxcAdj.Text = $"${CurAdjustment}";
      ReceiptGifts.CalculateCharge(_GuestID, (ChargeTo)FrmGiftsReceipt.cbogrct.SelectedItem, txtTotalCost, FrmGiftsReceipt.chkgrExchange, FrmGiftsReceipt.txtgrgu, ref txtgrcxcGifts,
                                              ref txtTotalCxC, ref FrmGiftsReceipt.txtgrCxCAdj, ref FrmGiftsReceipt._blnValidateMaxAuthGifts, ref FrmGiftsReceipt.txtgrls,
                                              ref FrmGiftsReceipt.txtgrMaxAuthGifts, ref FrmGiftsReceipt.lblgrMaxAuthGifts);

      // Si se desea cancelar el recibo
      if (!Exchange)
        grbGiftsReceiptExchange.Visibility = Visibility.Hidden;

      // Impedimos modificar los datos si el sistema esta en modo de solo lectura
      if (ConfigHelper.GetString("ReadOnly").ToUpper().Equals("TRUE"))
        btnSave.Visibility = Visibility.Hidden;

    }
    #endregion


    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _dsSalesRoom = ((CollectionViewSource)(this.FindResource("dsSalesRoom")));
      _dsPrograms = ((CollectionViewSource)(this.FindResource("dsPrograms")));
      _dsLeadSource = ((CollectionViewSource)(this.FindResource("dsLeadSource")));
      _dsGifts = ((CollectionViewSource)(this.FindResource("dsGifts")));

      _dsGiftsReceiptDetailCancel = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptDetailCancel")));
      _dsGiftInvitationWithoutReceipt = ((CollectionViewSource)(this.FindResource("dsGiftInvitationWithoutReceipt")));

      // Cargamos los Sales Room 
      _dsSalesRoom.Source = frmHost._lstSalesRoom;
      //Cargamos los Programs
      _dsPrograms.Source = frmHost._lstPrograms;
      // Cargamos los LeadSources
      _dsLeadSource.Source = frmHost._lstLeadSources;
      // Cargamos los Gifts
      _dsGifts.Source = frmHost._lstGifts;

      // Obtenemos los regalos a cancelar.
      _dsGiftsReceiptDetailCancel.Source = BRGiftsReceiptDetail.GetGiftsReceiptDetailCancel(_ReceiptID, _EnumExternalProduct);

      // si se debe generar un recibo exchange
      if (_Exchange)
        _dsGiftInvitationWithoutReceipt.Source = BRInvitsGifts.GetGiftsInvitationWithoutReceipt(0);
      // Si se desea cancelar el recibo
      else
      {
        CheckAllCell(ref grdCancel, _CancelField);
        TextBox x = null;
        ReceiptGifts.CalculateTotalGifts(grdCancel, EnumGiftsType.ReceiptGifts, ref txtTotalCost, ref x, ref x);
        grdCancel.IsReadOnly = true;
      }

    }
    #endregion

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (Validate())
      {
        Save();
        DialogResult = true;
      }
    }

    #region Save
    /// <summary>
    /// Guarda los datos
    /// </summary>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    private void Save()
    {
      List<string> aGiftsCancelled = null;

      // si se cancelo al menos un regalo
      if (CancelGifts(ref aGiftsCancelled))
      {
        _Cancelled = true;

        // si se debe generar un recibo exchange
        if (_Exchange)
        {
          // guardamos el recibo de regalos exchange
          SaveReceiptExchange();

          // si tiene regalos de intercambio
          if (grdExchange.Items.Count > 0)
          {
            // guardamos los regalos de intercambio

          }
        }
      }
    }
    #endregion

    #region SaveReceiptExchange
    /// <summary>
    /// Guarda el recibo de regalos exchange
    /// </summary>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    private async void SaveReceiptExchange()
    {
      // obtenemos los datos del recibo
      GiftsReceipt _GRResult = BRGiftsReceipts.GetGiftReceipt(_ReceiptID);

      // Construimos el nuevo GiftsReceipts
      GiftsReceipt _GiftsReceipt = new GiftsReceipt()
      {
        grNum = txtgrNum.Text,
        grD = frmHost._dtpServerDate,
        grgu = _GRResult.grgu,
        grExchange = true,
        grGuest = _GRResult.grGuest,
        grPax = _GRResult.grPax,
        grHotel = _GRResult.grHotel,
        grRoomNum = _GRResult.grRoomNum,
        grpe = _GRResult.grpe,
        grlo = _GRResult.grlo,
        grls = _GRResult.grls,
        grsr = _GRResult.grsr,
        grWh = _GRResult.grWh,
        grMemberNum = _GRResult.grMemberNum,
        grHost = _GRResult.grHost,
        grComments = txtgrComments.Text,
        grDeposit = 0,
        grDepositTwisted = 0,
        grcu = "US",
        grcxcPRDeposit = 0,
        grcucxcPRDeposit = "US",
        grCxCClosed = false,
        grExchangeRate = 0,
        grct = _GRResult.grct,
        grMaxAuthGifts = _GRResult.grMaxAuthGifts,
        grcxcGifts = 0,
        grcxcComments = null,
        grTaxiIn = 0,
        grTaxiOut = 0,
        grCancel = false,
        grClosed = false,
        grCxCAppD = null,
        grTaxiOutDiff = 0,
        grGuest2 = _GRResult.grGuest2
      };

      // Guardamos el Gift Receipt
      ReceiptExchangeID = BRGiftsReceipts.SaveGiftReceipt(_GiftsReceipt);

      // Guardamos el historico del recibo
      await BRGiftsReceiptLog.SaveGiftsReceiptsLog(ReceiptExchangeID, App.User.User.peID);
    }
    #endregion

    #region CancelGifts
    /// <summary>
    /// Cancela los regalos
    /// </summary>
    /// <param name="GiftsCancelled"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    private bool CancelGifts(ref List<string> GiftsCancelled)
    {
      bool blnOk = false;
      string strGift = "";
      List<string> aGiftsToCancel = new List<string>();

      // recorremos los regalos
      foreach (var item in grdCancel.Items)
      {
        Type type = item.GetType();
        var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

        // Si el regalo se desea cancelar
        if (((bool)type.GetProperty(_CancelField).GetValue(item, null)) == true)
        {
          strGift = (string)type.GetProperty("gegi").GetValue(item, null);

          // si el regalo es manejado por un sistema externo
          // y no es un paquete (solo aplica para el monedero electronico)
          Gift _Gift = null;
          if ((Common.IsExternalProduct(_EnumExternalProduct, strGift, ref _Gift)) && (_EnumExternalProduct != EnumExternalProduct.expElectronicPurse || _EnumExternalProduct == EnumExternalProduct.expElectronicPurse && _Gift.giPack))
          {
            // agregamos el regalo a la lista de regalos por cancelar
            aGiftsToCancel.Add(strGift);

            // Si es el monedero electronico
            if (_EnumExternalProduct == EnumExternalProduct.expElectronicPurse)
            {

            }
            else
            {
              if (SisturHelper.CancelPromotionSistur(strGift, (string)type.GetProperty("gePVPPromotion").GetValue(item, null), cboProgram.SelectedValue.ToString(),
                                                               cboLeadSource.SelectedValue.ToString(), PropertyOpera, _ReceiptID, txtReservation, _PromotionsSystem, ref GiftsCancelled) && !blnOk)
              {
                // Cancelamos el regalo en origos
                BRGiftsReceipts.CancelGiftPromotionSistur(_ReceiptID, strGift);
                blnOk = true;
              }
            }
          }
        }
      }

      // si es el monedero electronico
      //****************************************************>

      

      //****************************************************>
      return blnOk; 
    }
    #endregion

    #region Validate
    /// <summary>
    /// Valida los datos
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Mayo/2016 Created
    /// </history>
    private bool Validate()
    {
      // Validamos los regalos a cancelar
      if (!GiftsCancel.Validate(grdCancel, _CancelField))
        return false;

      // Validamos el recibo exchange
      else if (!ValidateGiftsReceiptExchange())
        return false;

      else if (!ValidateMaxAuthGifts())
        return false;

      return true;
    }
    #endregion

    #region ValidateMaxAuthGifts
    /// <summary>
    /// Valida el monto maximo de regalos
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    private bool ValidateMaxAuthGifts()
    {
      // si se debe validar el monto maximo de regalos
      if (_ValidateMaxAuthGifts)
      {
        // Validamos el monto maximo de regalos
        if (!Gifts.ValidateMaxAuthGifts(txtTotalCost.Text, txtMaxAuthGifts.Text))
          return false;
      }
      return true;
    } 
    #endregion

    #region ValidateGiftsReceiptExchange
    /// <summary>
    /// Valida el recibo exchange
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Mayo/2016 Created
    /// </history>
    private bool ValidateGiftsReceiptExchange()
    {
      // Si se debe generar un recibo exchange
      if (_Exchange)
      {
        // validamos los regalos de intercambio
        if (!GiftsExchange.Validate(grdExchange))
          return false;
        // validamos que no se vayan a duplicar los regalos en el recibo exchange
        else if (!ValidateGiftsRepeated())
          return false;
      }

      return true;
    }
    #endregion

    #region Cancel_Click
    /// <summary>
    /// Recalcula los costos cuando se pulsa un checkbox Cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 25/Mayo/2016 Created
    /// </history>
    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
      GiftsCancel.CalculateTotalGifts(grdCancel, ref txtTotalGiftsCancel, true, _CancelField);
      CalculateTotalGifts();
      _FrmGiftsReceipt.CalculateCharge(ref _FrmCancelExternalProducts);
    } 
    #endregion

    #region ValidateGiftsRepeated
    /// <summary>
    /// Valida que no se vayan a duplicar los regalos en el recibo exchange
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Mayo/2016 Created
    /// </history>
    private bool ValidateGiftsRepeated()
    {
      List<string> aGiftsCancelled = GiftsCancel.GetItems(grdCancel, EnumCheckedStatus.Checked, _CancelField);
      List<string> aGiftsExchange = GiftsExchange.GetItems(grdExchange);

      // si tiene regalos de intercambio
      if (aGiftsExchange.Count > 0)
      {
        // Recorremos los regalos de intercambio
        foreach (string item in aGiftsExchange)
        {
          string result = aGiftsCancelled.Where(x => x == item).SingleOrDefault();

          // si el regalo de intercambio esta en los regalos cancelados
          if (result != null && result != "")
          {
            // localizamos el regalo
            Gift _Gift = frmHost._lstGifts.Where(x => x.giID == result).First();
            UIHelper.ShowMessage(" The Gift '" + _Gift.giN + "' from 'Gifts exchange' can not be added \r\n" +
                                 "to receipt exchange because would ve repeated. \r\n \r\n" + 
                                 "To solve this situation, delete this gift from 'Gifts exchange' \r\n" +
                                 "or not cancel it from 'Gifts to cancel'", MessageBoxImage.Exclamation);
            return false;
          }
        }
      }

      return true;
    } 
    #endregion

    #region CalculateTotalGifts
    /// <summary>
    /// Calcula el total de los regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 16/Mayo/2016 Created
    /// </history>
    private void CalculateTotalGifts()
    {
      decimal TotalGiftsInvitation = string.IsNullOrEmpty(txtTotalGiftsInvitation.Text) ? 0 : Convert.ToDecimal(txtTotalGiftsInvitation.Text.Trim(new char[] { '$' }));
      decimal TotalGiftsCancel = string.IsNullOrEmpty(txtTotalGiftsCancel.Text) ? 0 : Convert.ToDecimal(txtTotalGiftsCancel.Text.Trim(new char[] { '$' }));
      decimal TotalGiftsExchange = string.IsNullOrEmpty(txtTotalGiftsExchange.Text) ? 0 : Convert.ToDecimal(txtTotalGiftsExchange.Text.Trim(new char[] { '$' }));

      txtTotalCost.Text = $"${(TotalGiftsInvitation - TotalGiftsCancel) + TotalGiftsExchange} ";
    } 
    #endregion

    #region btnClose_Click
    /// <summary>
    /// Cierra el formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Mayo/2016 Created
    /// </history>
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close();
    }
    #endregion

    #region CheckAllCell
    /// <summary>
    /// Marca / desmarca todas las celdas de una columna
    /// </summary>
    /// <history>
    /// [vipacheco] 17/Mayo/2016 Created
    /// </history>
    private void CheckAllCell(ref DataGrid Grid, string Field, bool Action = true)
    {
      foreach (GiftsReceiptDetailCancel item in Grid.Items)
      {
        if (Field.Equals("geCancelElectronicPurse"))
        {
          item.geCancelElectronicPurse = true;
        }
        else if (Field.Equals("geCancelPVPPromo"))
          item.geCancelPVPPromo = true;
      }
      Grid.IsReadOnly = true;
      Grid.Items.Refresh();  // Refrescamos 
      Grid.IsReadOnly =false;
    }
    #endregion

  }
}

using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Classes;
using IM.Model;
using IM.Model.Enums;
using IM.Services.Helpers;
using PalaceResorts.Common.PalaceTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmCancelExternalProducts.xaml
  /// </summary>
  public partial class frmCancelExternalProducts : Window
  {

    #region Variables
    private bool bandCancel;
    private DataGridCellInfo _currentCell;

    private EnumExternalProduct _EnumExternalProduct;
    private int _ReceiptID, _GuestID;
    private GiftsReceipt _giftsDataContext;
    public bool _ValidateMaxAuthGifts, useCxCCost, _Exchange, _Cancelled, _isExchangeReceipt, _applicationAdj;
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
    CollectionViewSource _dsGiftsReceiptDetail;

    ObservableCollection<GiftsReceiptDetail> _obsGifts;
    ObservableCollection<GiftsReceiptDetail> _obsGiftsComplet;
    ObservableCollection<GiftsReceiptDetailCancel> _obsGiftsCancel;


    #endregion

    #region Contructor
    public frmCancelExternalProducts(EnumExternalProduct enumExternalProducts, int ReceiptID, int GuestID,
                                 string NameGuest, decimal MaxAuthGifts, decimal TotalGifts, decimal CurAdjustment,
                                 bool ValidateMaxAuthGifts, bool pUseCxCCost, bool Exchange, frmGiftsReceipts FrmGiftsReceipt, GiftsReceipt pGiftsDataContext, bool pIsExchangeReceipt)
    {
      _EnumExternalProduct = enumExternalProducts;
      _ReceiptID = ReceiptID;
      _GuestID = GuestID;
      _ValidateMaxAuthGifts = ValidateMaxAuthGifts;
      useCxCCost = pUseCxCCost;
      _Exchange = Exchange;
      _isExchangeReceipt = pIsExchangeReceipt;
      _Cancelled = false;
      _FrmGiftsReceipt = FrmGiftsReceipt;
      _FrmCancelExternalProducts = this;
      _giftsDataContext = pGiftsDataContext;

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
      txtGuestID.Text = $"{ _GuestID}";
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
      txtMaxAuthGifts.Text = string.Format("{0:C2}", MaxAuthGifts);

      // Totales de regalos
      txtTotalGiftsInvitation.Text = string.Format("{0:C2}", BRGiftsReceipts.CalculateTotalsGiftsInvitation(_GuestID));

      // Si se genera un recibo Exchange
      if (_Exchange)
        txtTotalGiftsCancel.Text = string.Format("{0:C2}", 0.00);
      // si se desea cancelar el recibo
      else
        txtTotalGiftsCancel.Text = string.Format("{0:C2}", TotalGifts);

      txtTotalGiftsExchange.Text = string.Format("{0:C2}", 0.00);
      CalculateTotalGifts();

      // Totales del cargo
      txtgrcxcAdj.Text = string.Format("{0:C2}", CurAdjustment);

      // Activamos la bandera para saber si se ajustaran costos CxC
      _applicationAdj = CurAdjustment > 0 ? true : false;

      ReceiptsGifts.CalculateCharge(_GuestID, (ChargeTo)FrmGiftsReceipt.cbogrct.SelectedItem, txtTotalCost, _isExchangeReceipt, ref txtgrcxcGifts,
                                              ref txtTotalCxC, ref FrmGiftsReceipt.txtgrCxCAdj, ref FrmGiftsReceipt._validateMaxAuthGifts, _Guest.gulsOriginal,
                                              ref FrmGiftsReceipt.txtgrMaxAuthGifts, ref FrmGiftsReceipt.lblgrMaxAuthGifts);

      // Si se desea cancelar el recibo
      if (!Exchange)
      {
        stkGiftsExchange.Visibility = Visibility.Collapsed;
        WindowMain.Height = 424; // Ajustamos la ventana
      }

      // Impedimos modificar los datos si el sistema esta en modo de solo lectura
      if (ConfigHelper.GetString("ReadOnly").ToUpper().Equals("TRUE"))
        btnSave.Visibility = Visibility.Hidden;

    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _dsSalesRoom = ((CollectionViewSource)(this.FindResource("dsSalesRoom")));
      _dsPrograms = ((CollectionViewSource)(this.FindResource("dsPrograms")));
      _dsLeadSource = ((CollectionViewSource)(this.FindResource("dsLeadSource")));
      _dsGifts = ((CollectionViewSource)(this.FindResource("dsGifts")));

      _dsGiftsReceiptDetailCancel = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptDetailCancel")));
      _dsGiftsReceiptDetail = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptDetail")));

      // Cargamos los Sales Room 
      _dsSalesRoom.Source = frmHost._lstSalesRoom;
      //Cargamos los Programs
      _dsPrograms.Source = frmHost._lstPrograms;
      // Cargamos los LeadSources
      _dsLeadSource.Source = frmHost._lstLeadSources;
      // Cargamos los Gifts
      _dsGifts.Source = frmHost._lstGifts;

      // Obtenemos los regalos a cancelar.
      List<GiftsReceiptDetailCancel> lstResultCancel = BRGiftsReceiptDetail.GetGiftsReceiptDetailCancel(_ReceiptID, _EnumExternalProduct);
      _obsGiftsCancel = new ObservableCollection<GiftsReceiptDetailCancel>(lstResultCancel);
      _dsGiftsReceiptDetailCancel.Source = _obsGiftsCancel;

      // si se debe generar un recibo exchange
      if (_Exchange)
      {
        List<GiftsReceiptDetail> lstResult = await BRInvitsGifts.GetGiftsInvitationWithoutReceipt(0);
        _obsGifts = new ObservableCollection<GiftsReceiptDetail>(lstResult);
        _obsGiftsComplet = new ObservableCollection<GiftsReceiptDetail>(lstResult);
        _dsGiftsReceiptDetail.Source = _obsGifts;
      }
      // Si se desea cancelar el recibo
      else
      {
        CheckAllCell(ref grdCancel, _CancelField);
        TextBox x = null;
        ReceiptsGifts.CalculateTotalGifts(grdCancel, EnumGiftsType.ReceiptGifts, ref txtTotalCost, ref x, ref x);
        grdCancel.IsReadOnly = true;
      }

    }
    #endregion

    #region btnSave_MouseLeftButtonDown
    /// <summary>
    /// Guarda la informacion
    /// </summary>
    /// <history>
    /// [vipacheco] 16/Mayo/2016 Created
    /// </history>
    private async void btnSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (Validate())
      {
        try
        {
          await Save();
        }
        catch (Exception ex)
        {
          UIHelper.ShowMessage(UIHelper.GetMessageError(ex), MessageBoxImage.Error, "Error Save promotions in sistur");
        }

        DialogResult = true;
      }
    }
    #endregion

    #region Save
    /// <summary>
    /// Guarda los datos
    /// </summary>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    private async Task Save()
    {
      List<string> aGiftsCancelled = await CancelGifts();

      // si se cancelo al menos un regalo
      if (aGiftsCancelled.Count > 0)
      {
        _Cancelled = true;

        // si se debe generar un recibo exchange
        if (_Exchange)
        {
          // guardamos el recibo de regalos exchange
          await SaveReceiptExchange();

          grdExchange.IsReadOnly = true;
          // si tiene regalos de intercambio
          if (grdExchange.Items.Count > 0)
          {
            // guardamos los regalos de intercambio
            GiftsExchange.Save(ReceiptExchangeID, grdExchange);

            // Guardamos las promociones en Sistur
            string msjSavePromotionsSistur = await SisturHelper.SavePromotionsSistur(ReceiptExchangeID, "", App.User.User.peID);
            if (!string.IsNullOrEmpty(msjSavePromotionsSistur))
              UIHelper.ShowMessage(msjSavePromotionsSistur, MessageBoxImage.Information, "Save promotions in sistur");
          }
        }

        // guardamos los regalos cancelados
        await GiftsCancel.Save(_ReceiptID, ReceiptExchangeID, aGiftsCancelled, useCxCCost, _giftsDataContext, _obsGifts, _obsGiftsCancel, _Exchange, _CancelField);

        // si se maneja el monto maximo de regalos
        if (_ValidateMaxAuthGifts)
        {
          // actualizamos el cargo del recibo de regalos
          decimal Charge = Convert.ToDecimal(txtgrcxcGifts.Text.TrimStart('$'));
          decimal Adjustment = Convert.ToDecimal(txtgrcxcAdj.Text.TrimStart('$'));
          await BRGiftsReceipts.UpdateCharge(_GuestID, Charge, Adjustment);
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
    private async Task SaveReceiptExchange()
    {
      // obtenemos los datos del recibo
      GiftsReceipt _GRResult = await BRGiftsReceipts.GetGiftReceipt(_ReceiptID);

      // Construimos el nuevo GiftsReceipts
      GiftsReceipt _GiftsReceipt = new GiftsReceipt()
      {
        grNum = txtgrNum.Text,
        grD = frmHost.dtpServerDate,
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
        grGuest2 = _GRResult.grGuest2,
        grcucxcTaxiOut = _GRResult.grcucxcTaxiOut,
        grpt = _GRResult.grpt
      };

      // Guardamos el Gift Receipt
      ReceiptExchangeID = await BRGiftsReceipts.SaveGiftReceipt(_GiftsReceipt);

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
    private async Task<List<string>> CancelGifts()
    {
      string strGift = "", GiftsCancellled = "", GiftsNotCancellled = "";
      List<string> aGiftsToCancel = new List<string>();
      List<string> GiftsCancelled = new List<string>();

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
            if (_EnumExternalProduct != EnumExternalProduct.expElectronicPurse)
            {
              if (await SisturHelper.CancelPromotionSistur(strGift, (string)type.GetProperty("gePVPPromotion").GetValue(item, null), cboProgram.SelectedValue.ToString(),
                                          cboLeadSource.SelectedValue.ToString(), PropertyOpera, _ReceiptID, txtReservation, GiftsCancelled))
              {
                // Cancelamos el regalo en origos
                await BRGiftsReceipts.CancelGiftPromotionSistur(_ReceiptID, strGift);
              }
            }
          }
        }
      }

      // si se cancelaron todos los regalos
      if (aGiftsToCancel.Count == GiftsCancelled.Count)
      {
        UIHelper.ShowMessage("Gifts were successfully cancelled", MessageBoxImage.Information, "Intelligence Marketing");
      }
      // si no se cancelaron todos los regalos
      else
      {
        // si no se pudo cancelar ningun regalo
        if (GiftsCancelled.Count <= 0)
        {

          UIHelper.ShowMessage("Gifts were not cancelled", MessageBoxImage.Information, "Intelligence Marketing");

        }
        // si se pudo cancelar al menos un regalo
        else
        {
          // recorremos los regalos que se deseaban cancelar
          foreach (string _Gift in aGiftsToCancel)
          {
            // localizamos el regalo
            Gift _giftResult = frmHost._lstGifts.Where(x => x.giID == _Gift).Single();

            // buscamos el regalo en el arreglo de regalos cancelados
            int iIndex = GiftsCancelled.IndexOf(_Gift);

            // si el regalo fue cancelado
            if (iIndex > 0)
            {
              GiftsCancellled += _giftResult.giN + "\r\n";
            }
            // si el regalo no fue cancelado
            else
            {
              GiftsNotCancellled += _giftResult.giN + "\r\n";
            }
          }
          UIHelper.ShowMessage("The following gifts were cancelled from the account: \r\n" + GiftsCancellled + "\r\n\r\n" + "But the following gifts were not cancelled from the account: \r\n" + GiftsNotCancellled, MessageBoxImage.Exclamation, "Intelligence Marketing");
        }
      }

      return GiftsCancelled;
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
    public void CalculateTotalGifts()
    {
      decimal TotalGiftsInvitation = string.IsNullOrEmpty(txtTotalGiftsInvitation.Text) ? 0 : Convert.ToDecimal(txtTotalGiftsInvitation.Text.Trim(new char[] { '$' }));
      decimal TotalGiftsCancel = string.IsNullOrEmpty(txtTotalGiftsCancel.Text) ? 0 : Convert.ToDecimal(txtTotalGiftsCancel.Text.Trim(new char[] { '$' }));
      decimal TotalGiftsExchange = string.IsNullOrEmpty(txtTotalGiftsExchange.Text) ? 0 : Convert.ToDecimal(txtTotalGiftsExchange.Text.Trim(new char[] { '$', '(', ')' }));

      txtTotalCost.Text = string.Format("{0:C2}", (TotalGiftsInvitation - TotalGiftsCancel) + TotalGiftsExchange);
    }
    #endregion

    #region btnClose_Click
    /// <summary>
    /// Cierra el formulario
    /// </summary>
    /// <history>
    /// [vipacheco] 16/Mayo/2016 Created
    /// </history>
    private void btnClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
    }
    #endregion

    #region GiftsPacks_Expanded
    /// <summary>
    /// Despliega los gifts del paquete
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// vipacheco] 25/Abril/2016 Created
    /// </history>
    private void GiftsPacks_Expanded(object sender, RoutedEventArgs e)
    {
      for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
        if (vis is DataGridRow)
        {
          var row = (DataGridRow)vis;
          GiftsReceiptDetail _GiftDetail = row.DataContext as GiftsReceiptDetail;

          if (_GiftDetail == null) return;
          // Buscamos el regalo seleccionado
          Gift _Gift = frmHost._lstGifts.Where(x => x.giID == _GiftDetail.gegi).Single();

          // Verificamos si tiene paquete de regalos
          if (_Gift.giPack)
          {
            // Buscamos los regalos del paquete
            var packs = frmHost._lstGiftsPacks.Where(x => x.gpPack == _Gift.giID).ToList();
            var giftsPacks = packs.Select(x => new GiftsReceiptPackageItem { gkQty = _GiftDetail.geQty, gkgi = frmHost._lstGifts.Where(w => w.giID == x.gpgi).Select(s => s.giN).First() }).ToList();
            List<GiftsReceiptPackageItem> lstResult = giftsPacks;

            row.DetailsVisibility = Visibility.Visible;
            // Localizamos el recurso
            CollectionViewSource dsGiftsReceiptPackage = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptPackage")));
            // Asignamos el valor a la lista
            dsGiftsReceiptPackage.Source = lstResult;
          }

          break;
        }
    }
    #endregion

    #region grdExchange_PreviewExecuted
    /// <summary>
    /// Recalcula los costos cuando se elimina un item del grid Exchange
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 18/Julio/2016 Created
    /// </history>
    private void grdExchange_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      if (e.Command == DataGrid.DeleteCommand)
      {
        GiftsReceiptDetail _deleteGift = grdExchange.SelectedItem as GiftsReceiptDetail;
        decimal TotalGiftsExchange = string.IsNullOrEmpty(txtTotalGiftsExchange.Text) ? 0 : Convert.ToDecimal(txtTotalGiftsExchange.Text.Trim(new char[] { '$' }));
        decimal result = TotalGiftsExchange - _deleteGift.gePriceA;
        txtTotalGiftsExchange.Text = string.Format("{0:C2}", result > 0 ? result : 0);
        CalculateTotalGifts();
      }
    }
    #endregion

    #region GiftsPacks_Collapsed
    /// <summary>
    /// Colapsa los gifts de regalo del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 25/Abril/2016 Created
    /// </history>
    private void GiftsPacks_Collapsed(object sender, RoutedEventArgs e)
    {
      for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
        if (vis is DataGridRow)
        {
          var row = (DataGridRow)vis;
          CollectionViewSource dsGiftsReceiptPackage = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptPackage")));
          dsGiftsReceiptPackage.Source = null;

          row.DetailsVisibility = Visibility.Collapsed; //row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
          break;
        }
    }
    #endregion

    #region grdExchange_PreparingCellForEdit
    /// <summary>
    /// Determina si se puede editar la informacion del grid
    /// </summary>
    /// <history>
    /// [vipacheco] 25/Junio/2016 Created
    /// </history>
    private void grdExchange_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
      DataGrid dataGrid = sender as DataGrid;
      GiftsReceiptDetail giftsReceiptDetail = dataGrid.Items.CurrentItem as GiftsReceiptDetail;
      _currentCell = grdExchange.CurrentCell;

      ReceiptsGifts.StartEdit(EnumMode.Edit, giftsReceiptDetail, ref _currentCell, ref grdExchange, ref bandCancel);
    }
    #endregion

    #region grdExchange_CellEditEnding
    /// <summary>
    /// Evento para validar los cambios de una celda en el grid
    /// </summary>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created 
    /// </history>
    private async void grdExchange_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!bandCancel)
      {
        grdExchange.CellEditEnding -= grdExchange_CellEditEnding;
        DataGrid dataGrid = sender as DataGrid;
        ComboBox comboBox = e.EditingElement as ComboBox;
        GiftsReceiptDetail giftsReceiptDetail = dataGrid.Items.CurrentItem as GiftsReceiptDetail;

        // Validamos la celda
        bool HasErrorValidate = await ReceiptsGifts.ValidateEdit(dataGrid, giftsReceiptDetail, true, _currentCell);

        // Si se cancela la edicion
        if (!HasErrorValidate)
        {
          ChargeTo pChargeTo = frmHost._lstChargeTo.Where(x => x.ctID.ToUpper() == "MARKETING").Single();
          LeadSource pLeadSource = cboLeadSource.SelectedItem as LeadSource;

          ReceiptsGifts.AfterEdit(ref grdExchange, _Guest.guID, row: ref giftsReceiptDetail, pCell: _currentCell, pUseCxCCost: useCxCCost, pIsExchange: _isExchangeReceipt,
                                        pChargeTo: pChargeTo, pLeadSourceID: pLeadSource.lsID, pTxtTotalCost: txtTotalGiftsExchange, pTxtgrCxCGifts: txtgrcxcGifts,
                                        pTxtTotalCxC: txtTotalCxC, pTxtgrCxCAdj: txtgrcxcAdj, pTxtgrMaxAuthGifts: txtMaxAuthGifts, pLblgrMaxAuthGifts: txbMaxAuthGiftsCaption);

          CalculateTotalGifts();
          txbMaxAuthGiftsCaption.Visibility = Visibility.Visible;
          txtMaxAuthGifts.Visibility = Visibility.Visible;

          // Verificamos si se actualiza los costs CxC
          if (_applicationAdj)
          {
            txtgrcxcGifts.Text = txtTotalCost.Text;
            decimal cxcGifts = string.IsNullOrEmpty(txtgrcxcGifts.Text) ? 0 : Convert.ToDecimal(txtgrcxcGifts.Text.TrimStart('$'));
            decimal cxcAdj = string.IsNullOrEmpty(txtgrcxcAdj.Text) ? 0 : Convert.ToDecimal(txtgrcxcAdj.Text.TrimStart('$'));
            txtTotalCxC.Text = string.Format("{0:C2}", cxcAdj + cxcGifts);
          }
        }
        else
        {
          e.Cancel = true;
        }
        grdExchange.CellEditEnding += grdExchange_CellEditEnding;
      }
      // Verificamos si se puso en modo lectura la celda
      if (_currentCell.Column.IsReadOnly)
        _currentCell.Column.IsReadOnly = false;
    }
    #endregion

    #region grdGifts_RowEditEnding
    /// <summary>
    /// Evento para finalizar la edicion de un row en el grid
    /// </summary>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created 
    /// </history>
    private void grdGifts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      DataGrid dataGrid = sender as DataGrid;

      if (e.EditAction == DataGridEditAction.Commit)
      {
        GiftsReceiptDetail giftsReceipt = dataGrid.SelectedItem as GiftsReceiptDetail;

        // validamos que los campos obligatorio ya se encuentren ingresados.
        if (giftsReceipt.geQty > 0 && giftsReceipt.gegi != null)
        {
          e.Cancel = false;
        }
        // Si aun faltan campos obligatorios se cancela el commit de la fila
        else
        {
          e.Cancel = true;
        }
      }
    }
    #endregion


  }
}

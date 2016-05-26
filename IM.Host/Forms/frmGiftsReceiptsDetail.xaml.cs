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

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmGiftsReceipts.xaml
  /// </summary>
  public partial class frmGiftsReceiptsDetail : Window
  {
    private static int _guestID;
    public EnumModeOpen modeOpen;
    public EnumGiftsType _giftsType;

    List<ChargeTo> _lstChargeTo;
    
    // Variable para las tasas de cambio!
    private List<ExchangeRateShort> _lstExchangeRate;

    // Moneda de la sala de ventas
    private string _SRCurrency;

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

    public frmGiftsReceiptsDetail(int guestID = 0)
    {
      _guestID = guestID;
      _giftsType = EnumGiftsType.ReceiptGifts;

      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
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

      // Obtenemos los colaboradores
      _dsPersonnel_Offered.Source = await BRPersonnel.GetPersonnel("ALL", "ALL", "ALL", 1);

      _dsPersonnel_Gifts.Source =await BRPersonnel.GetPersonnel("ALL", "ALL", "ALL", 1);

      // Obtenemos los Charge To
      _lstChargeTo = await BRChargeTos.GetChargeTos();
      _dsChargeTo.Source = _lstChargeTo;

      //Obtenemos los Parments Types
      _dsPaymentType.Source =await BRPaymentTypes.GetPaymentTypes(1);

      // Obtenemos las Monedas de la CxC de PR
      _dsCurrencyPRDeposit.Source =await BRCurrencies.GetCurrencies(null, 1);

      // Obtenemos las Monedas de la CxC de Taxi Out
      _dsCurrencyTaxiOut.Source =await BRCurrencies.GetCurrencies(null, 1);

      // Obtenemos las monedas
      _dsCurrency.Source =await BRCurrencies.GetCurrencies(null, 1);

      // Obtenemos las monedas para los deposits
      _dsCurrencyDeposits.Source =await BRCurrencies.GetCurrencies(null, 1);

      //Obtenemos los regalos
      _dsGifts.Source =await BRGifts.GetGifts("ALL", 1);

      // Obtenemos los bancos
      _dsBanks.Source = BRBanks.GetBanks(1);

      // Obtenemos los Source Payments
      _dsSourcePayments.Source = BRSourcePayments.GetSourcePayments(1);

      switch (modeOpen)
      {
        case EnumModeOpen.Add:
          grdDeposits.IsReadOnly = true;


          break;
        case EnumModeOpen.Edit: 
          break;
        case EnumModeOpen.Preview: 

          break;
        case EnumModeOpen.PreviewEdit:
          break;
      }
    }

    private void SetMaxAuthGifts()
    {
      decimal curMaxAuthGifts;
      bool blnWithMaxAuthGifts = false;

      ChargeTo _chargeTo = (ChargeTo)cbogrct.SelectedItem;

      curMaxAuthGifts = CalculateMaxAuthGifts(_chargeTo.ctID, txtgrls.Text, ref blnWithMaxAuthGifts);
      txtgrMaxAuthGifts.Text = string.Format("${0}", curMaxAuthGifts);
      lblgrMaxAuthGifts.Visibility = (blnWithMaxAuthGifts) ? Visibility.Visible : Visibility.Hidden;
      txtgrMaxAuthGifts.Visibility = (blnWithMaxAuthGifts) ? Visibility.Visible : Visibility.Hidden;
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

      foreach (GiftsReceiptDetailShort item in dgGiftsReceiptDetailShort.Items)
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
      txtTotalCostCaption.Text = string.Format("${0:0.##}", curTotalCost);

      // Actualizamos la etiqueta de precio total
      txtTotalPriceCaption.Text = string.Format("${0:0.00}", curTotalPrice);

      //Actualizamos la etiqueta de total a pagar
      txtTotalToPayCaption.Text = string.Format("${0:0.00}", curTotalToPay);

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
      txtTotalPaymentsCaption.Text = string.Format("${0:0.00}", curTotalPaid);
    } 
    #endregion

    private void LoadExchangeRates()
    {
      // Obtenemos la fecha de recibo
      DateTime dtmReceipt = Convert.ToDateTime(dtpgrD.Value.Value.Date).Date;

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
    /*
    #region GiftsReceiptShort_Selected
    /// <summary>
    /// Obtienen la seleccion del row en el dataGrid gifts
    /// </summary>
    /// <history>
    /// [vipacheco] 08/04/2016 Created
    /// </history>
    private void GiftsReceiptShort_Selected()
    {
      //GiftsReceiptsShort _grSelected = (GiftsReceiptsShort)grdReceipts.SelectedItem;

      List<GiftsReceipt> _result = BRGiftsReceipts.GetGiftReceipt(_grSelected.grID);
      _dsGiftsReceipt.Source = _result;

      // Cargamos los regalos del recibo de regalo seleccionado
      _dsGiftsReceiptDetailShort.Source = BRGiftsReceipts.GetGiftsReceiptsDetail(_grSelected.grID);

      // Cargamos los regalos payments
      _dsGiftsReceiptPaymentShort.Source = BRGiftsReceiptsPayments.GetGiftsReceiptPayments(_grSelected.grID);
    }
    #endregion*/
/*
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
    */
   /* private void cbogrct_Loaded(object sender, RoutedEventArgs e)
    {
      ChargeTo _ChargeTo = (ChargeTo)cbogrct.SelectedItem;

      if (_ChargeTo != null)
      {
        //txtTotalCxC.Text = string.Format("${}", tx);
      }

    }

    private void cbogrct_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //  bool _useCxCCost;

      //  ChargeTo _ChargeTo = (ChargeTo)cbogrct.SelectedItem;

      //  if (_ChargeTo != null)
      //  {
      //    switch (_ChargeTo.ctID)
      //    {
      //      case "PR":
      //      case "LINER":
      //      case "CLOSER":
      //        _useCxCCost = true;
      //        break;
      //      default:
      //        _useCxCCost = false;
      //        break;
      //    }

      //    CalculateAllCostsPrices(_useCxCCost);

      //    CalculateCharge();
      //  }
      CalculateCharge();
    }

    private void CalculateAllCostsPrices(bool CalculateAllPrices = false)
    {
      // Recorremos los regalos
      foreach (GiftsReceiptDetailShort item in dgGiftsReceiptDetailShort.Items)
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
      txtTotalCxC.Text = string.Format("{0:0.00}", Convert.ToDecimal(txtgrCxCGifts.Text != "" ? txtgrCxCGifts.Text : "0") + Convert.ToDecimal(txtgrCxCAdj.Text != "" ? txtgrCxCAdj.Text : "0"));
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
    */
    
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
    
    /*

    private void dgGiftsReceiptDetailShort_RowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
    {
        DataGrid innerDataGrid = e.DetailsElement as DataGrid;
        GiftsReceiptDetailShort _dt = (GiftsReceiptDetailShort)dgGiftsReceiptDetailShort.SelectedItem;
        var _result = BRGiftsReceiptsPacks.GetGiftsReceiptPackage(Convert.ToInt32(txtgrID.Text), _dt.gegi);
        if (_result.Count > 0)
        {
          innerDataGrid.Visibility = Visibility.Visible;
          var _r = _result.Select(x => new { gkgi = x.gkgi, gkQty = x.gkQty }).ToList();
          innerDataGrid.ItemsSource = _r;
        }
        else
          innerDataGrid.Visibility = Visibility.Collapsed;
    }*/

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

    /*
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
      foreach (GiftsReceiptDetailShort item in dgGiftsReceiptDetailShort.Items)
      {
        rowCountgeInElectronicPurse += Convert.ToInt32(item.geInElectronicPurse);
        rowcountgeCancelElectronicPurse += Convert.ToInt32(item.geCancelElectronicPurse);
        rowCountgeInPVPPromo += Convert.ToInt32(item.geInPVPPromo);
        rowCountgeCancelPVPPromo += Convert.ToInt32(item.geCancelPVPPromo);
      }
      // si tiene regalos pendientes por cancelar en el monedero electronico
      if (rowCountgeInElectronicPurse > rowcountgeCancelElectronicPurse)
      {

      }
      // Si tiene regalos pendientes por cancelar en Sistur
      else if (rowCountgeInPVPPromo > rowCountgeCancelPVPPromo)
      {

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
      foreach (GiftsReceiptDetailShort item in dgGiftsReceiptDetailShort.Items)
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
      foreach (GiftsReceiptDetailShort item in dgGiftsReceiptDetailShort.Items)
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
    */
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
      if (!ValidateChangedBy(txtChangedBy.Text, txtPwd.Text))
        return false;
      else if (true)
      {
        return true;
      }
    }

    /// <summary>
    /// Valida que se ingrese quien hizo el cambio y su contraseña
    /// </summary>
    /// <param name="changeBy"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private bool ValidateChangedBy(string changeBy, string password)
    {
      return true;
    }

    private void dgGiftsReceiptPaymentShort_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
    {
      //e.NewItem = new GiftsReceiptPaymentShort { gy };
    }

    private void dgGiftsReceiptPaymentShort_AddingNewItem(object sender, AddingNewItemEventArgs e)
    {
      e.NewItem = new GiftsReceiptPaymentShort { gype = App.User.User.peID, UserName = App.User.User.peN };
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
    
    private void dgGiftsReceiptPaymentShort_Loaded(object sender, RoutedEventArgs e)
    {
      List<GiftsReceiptPaymentShort> _initialValue = new List<GiftsReceiptPaymentShort>();
      _dsGiftsReceiptPaymentShort.Source = _initialValue;
    }

    private void dgGiftsReceiptDetailShort_Loaded(object sender, RoutedEventArgs e)
    {
      List<GiftsReceiptDetailShort> _initialValue = new List<GiftsReceiptDetailShort>();
      _dsGiftsReceiptDetailShort.Source = _initialValue;
    }

    private void SomeSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var comboBox = sender as ComboBox;
      var selectedItem = this.dgGiftsReceiptDetailShort.CurrentItem;

    }

    private void dgGiftsReceiptDetailShort_AddingNewItem(object sender, AddingNewItemEventArgs e)
    {
      e.NewItem = new GiftsReceiptDetailShort { gect = "MARKETING" };
    }
  }
}

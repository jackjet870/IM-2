using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;
using System.Collections.Generic;

namespace IM.ProcessorGeneral.Classes
{
  public static class clsFormatReport
  {
    #region Reports by Sales Room

    #region Bookings

    #region RptBookingsBySalesRoomProgramTime

    /// <summary>
    /// Formato para el reporte Bookings By Sales Room, Program & Time
    /// </summary>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static ColumnFormatList RptBookingsBySalesRoomProgramTime()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Sales Room", "SalesRoom", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Program", "Program", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Book Type", "BookType", axis: ePivotFieldAxis.Row);
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Time", "Time", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      return lst;
    }

    #endregion RptBookingsBySalesRoomProgramTime

    #region RptBookingsBySalesRoomProgramLeadSourceTime

    /// <summary>
    /// Formato para el reporte Bookings By Sales Room, Program, Lead Sources & Time
    /// </summary>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static ColumnFormatList RptBookingsBySalesRoomProgramLeadSourceTime()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Sales Room", "SalesRoom", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Program", "Program", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Lead Source", "LeadSource", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Book Type", "BookType", axis: ePivotFieldAxis.Row);
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Time", "Time", axis: ePivotFieldAxis.Column);
      return lst;
    }

    #endregion RptBookingsBySalesRoomProgramLeadSourceTime

    #endregion Bookings

    #region CxC

    #region RptCxc

    /// <summary>
    /// Formato para el reporte CxC
    /// </summary>
    /// <history>
    /// [edgrodriguez] 22/Mar/2016 Created
    /// </history>
    public static ColumnFormatList RptCxc()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Group", "grGroup", isGroup: true, isVisible: false);
      lst.Add("PR", "grpe");
      lst.Add("PR Name", "peN");
      lst.Add("Chb PP", "grNum", format: EnumFormatTypeExcel.Id);
      lst.Add("Rcpt", "grID");
      lst.Add("Date", "grD", format: EnumFormatTypeExcel.Date);
      lst.Add("Guest ID", "grgu", format: EnumFormatTypeExcel.Id);
      lst.Add("Guest Name", "grGuest");
      lst.Add("Qty", "geQty", format: EnumFormatTypeExcel.Number);
      lst.Add("Gift", "giN");
      lst.Add("Ad", "geAdults", format: EnumFormatTypeExcel.Number);
      lst.Add("Min", "geMinors", format: EnumFormatTypeExcel.Number);
      lst.Add("Folios", "geFolios");
      lst.Add("CxC Comments", "grCxCComments");
      lst.Add("Receipt Comments", "grComments");
      lst.Add("Total Gifts", "TotalGift", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("CxC Gift", "grCxCGifts", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("CxC Adj", "grCxCAdj", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("CxC Deposit", "grCxCPRDeposit", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("CxC Taxi Out", "grCxCTaxiOut", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Total CxC", "TotalCxC", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Ex. Rate", "ExchRateSalesRoom", format: EnumFormatTypeExcel.DecimalNumber);
      lst.Add("CxC Paid US", "CxCPaidUS", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("CxC Paid MN", "CxCPaidMN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);

      lst.Add("Currency Deposit", "grcuCxCPRDeposit", isVisible: false);
      lst.Add("Ex. Rate Deposit", "ExchRateDeposit", format: EnumFormatTypeExcel.DecimalNumber, isVisible: false);
      lst.Add("Deposit US", "CxCDepositUS", format: EnumFormatTypeExcel.Currency, isVisible: false);
      lst.Add("Deposit MN", "CxCDepositMN", format: EnumFormatTypeExcel.Currency, isVisible: false);
      lst.Add("Currency Taxi Out", "grcuCxCTaxiOut", isVisible: false);
      lst.Add("Ex. Rate Taxi Out", "ExchRateTaxiOut", format: EnumFormatTypeExcel.DecimalNumber, isVisible: false);
      lst.Add("Taxi Out US", "CxCTaxiOutUS", format: EnumFormatTypeExcel.Currency, isVisible: false);
      lst.Add("Taxi Out MN", "CxCTaxiOutMN", format: EnumFormatTypeExcel.Currency, isVisible: false);
      return lst;
    }

    #endregion RptCxC

    #region RptCxcByType

    /// <summary>
    /// Formato para el reporte CxC
    /// </summary>
    /// <history>
    /// [edgrodriguez] 17/May/2016 Created
    /// </history>
    public static ColumnFormatList RptCxcByType()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("grpe", "grpe", isGroup: true, isVisible: false);
      lst.Add("Receipt Date", "grD", format: EnumFormatTypeExcel.Date);
      lst.Add("Folio", "grNum");
      lst.Add("SR", "grsr");
      lst.Add("PR Name", "peN");
      lst.Add("Description", "Comments");
      lst.Add("CxC", "CxC", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Exchange Rate", "exExchRate", format: EnumFormatTypeExcel.Currency);
      lst.Add("CxC Pesos", "CxCP", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Total CxC", "TotalCxC", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Amount to Pay (USD)", "ToPayUS", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Amount To Pay (MXN)", "ToPayMN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      return lst;
    }

    #endregion

    #region RptCxcDeposits

    /// <summary>
    /// Formato para el reporte CxC Deposits
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/Mar/2016 Created
    /// </history>
    public static ColumnFormatList RptCxcDeposits()
    {
      ColumnFormatList lst = new ColumnFormatList();

      lst.Add("Ch B", "grID", format: EnumFormatTypeExcel.Id, axis: ePivotFieldAxis.Row);
      lst.Add("Chb PP", "grNum", axis: ePivotFieldAxis.Row);
      lst.Add("Date", "grD", axis: ePivotFieldAxis.Row);
      lst.Add("LS", "grls", axis: ePivotFieldAxis.Row);
      lst.Add("GUID", "grgu", format: EnumFormatTypeExcel.Id, axis: ePivotFieldAxis.Row);
      lst.Add("Guest Name", "grGuest", axis: ePivotFieldAxis.Row);
      lst.Add("Hotel", "grHotel", axis: ePivotFieldAxis.Row);
      lst.Add("PR", "grpe", axis: ePivotFieldAxis.Row);
      lst.Add("PR Name", "peN", axis: ePivotFieldAxis.Row);
      lst.Add("Host", "grHost", axis: ePivotFieldAxis.Row);
      lst.Add("Host Name", "HostN", axis: ePivotFieldAxis.Row);
      lst.Add("CxC", "grCxCPRDeposit", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Currency", "cuN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("CxC Currency", "grcuCxCPRDeposit", axis: ePivotFieldAxis.Row, isVisible: false);
      return lst;
    }

    #endregion RptCxcDeposits

    #region RptCxcGifts

    /// <summary>
    /// Formato para el reporte CxCGifts
    /// </summary>
    /// <history>
    /// [edgrodriguez] 29/Mar/2016 Created
    /// </history>
    public static ColumnFormatList RptCxcGifts()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("CHB", "grID", axis: ePivotFieldAxis.Row);
      lst.Add("Chb PP", "grNum", axis: ePivotFieldAxis.Row);
      lst.Add("PR", "grpe", axis: ePivotFieldAxis.Row);
      lst.Add("PR Name", "peN", axis: ePivotFieldAxis.Row);
      lst.Add("Location", "grlo", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row);
      lst.Add("Host", "grHost", axis: ePivotFieldAxis.Row);
      lst.Add("Host Name", "HostN", axis: ePivotFieldAxis.Row);
      lst.Add("GUID", "grgu", axis: ePivotFieldAxis.Row);
      lst.Add("Guest Name", "grGuest", axis: ePivotFieldAxis.Row);
      lst.Add("Quantity", "Quantity", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, axis: ePivotFieldAxis.Values);
      lst.Add("Cost", "Cost", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, axis: ePivotFieldAxis.Values);
      lst.Add("Folios", "Folios", axis: ePivotFieldAxis.Values);
      lst.Add("Adults", "Adults", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row);
      lst.Add("Minors", "Minors", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row);
      lst.Add("Rcpt Date", "grD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("USD", "CostUS", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, axis: ePivotFieldAxis.Row);
      lst.Add("Exch. Rate", "exExchRate", axis: ePivotFieldAxis.Row);
      lst.Add("MX", "CostMX", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, axis: ePivotFieldAxis.Row);
      lst.Add("Member #", "grMemberNum", axis: ePivotFieldAxis.Row);
      lst.Add("Comments", "grCxCComments", axis: ePivotFieldAxis.Row);
      lst.Add("Gift", "Gift", axis: ePivotFieldAxis.Column);
      lst.Add("Gift Name", "giN", axis: ePivotFieldAxis.Column);

      return lst;
    }

    #endregion RptCxcGifts

    #region RptCxcNotAuthorized

    /// <summary>
    /// Formato para el reporte CxC
    /// </summary>
    /// <history>
    /// [edgrodriguez] 01/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptCxcNotAuthorized()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Rcpt", "grID");
      lst.Add("Chb PP", "grNum");
      lst.Add("LS", "grls");
      lst.Add("GUID", "grgu");
      lst.Add("Guest Name", "grGuest");
      lst.Add("Rcpt Date", "grD", format: EnumFormatTypeExcel.Date);
      lst.Add("PR", "grpe");
      lst.Add("PR Name", "peN");
      lst.Add("CxC Gifts", "grCxCGifts", format: EnumFormatTypeExcel.Currency);
      lst.Add("CxC Dep", "grCxCPRDeposit", format: EnumFormatTypeExcel.Currency);
      lst.Add("CxC Taxi", "grCxCTaxiOut", format: EnumFormatTypeExcel.Currency);
      lst.Add("CxC", "CxC", format: EnumFormatTypeExcel.Currency);
      return lst;
    }

    #endregion RptCxcNotAuthorized

    #region RptCxcPayments

    /// <summary>
    /// Formato para el reporte CxC Payments
    /// </summary>
    /// <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptCxcPayments()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Rcpt", "grID");
      lst.Add("Rcpt D", "grD", format: EnumFormatTypeExcel.Date);
      lst.Add("LS", "grls");
      lst.Add("PR", "grpe");
      lst.Add("PR Name", "peN");
      lst.Add("Pay USD", "AmountToPayUSD", format: EnumFormatTypeExcel.Currency);
      lst.Add("Pay MXN", "AmountToPayMXN", format: EnumFormatTypeExcel.Currency);
      lst.Add("Paid USD", "AmountPaidUSD", format: EnumFormatTypeExcel.Currency);
      lst.Add("Paid MXN", "AmountPaidMXN", format: EnumFormatTypeExcel.Currency);
      lst.Add("Bal USD", "BalanceUSD", format: EnumFormatTypeExcel.Currency);
      lst.Add("Bal MXN", "BalanceMXN", format: EnumFormatTypeExcel.Currency);
      lst.Add("GUID", "grgu");
      lst.Add("Reserv", "guHReservID");
      lst.Add("Out Inv", "guOutInvitNum");
      lst.Add("Payment Date", "cxD", format: EnumFormatTypeExcel.Date);
      lst.Add("Rec By", "cxReceivedBy", format: EnumFormatTypeExcel.Number);
      lst.Add("Rec Name", "ReceivedByName");
      lst.Add("Amt USD", "AmountUSD", format: EnumFormatTypeExcel.Currency);
      lst.Add("Amt MXN", "AmountMXN", format: EnumFormatTypeExcel.Currency);
      return lst;
    }

    #endregion RptCxcPayments

    #endregion CxC

    #region Deposits

    #region RptDeposits

    /// <summary>
    /// Formato para el reporte CxC
    /// </summary>
    /// <history>
    /// [edgrodriguez] 01/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptDeposits()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Ch B", "grID", axis: ePivotFieldAxis.Row);
      lst.Add("Chb PP", "grNum", axis: ePivotFieldAxis.Row);
      lst.Add("Out. Inv.", "guOutInvitNum", axis: ePivotFieldAxis.Row);
      lst.Add("GUID", "grgu", axis: ePivotFieldAxis.Row);
      lst.Add("Guest", "grGuest", axis: ePivotFieldAxis.Row);
      lst.Add("Hotel", "grHotel", axis: ePivotFieldAxis.Row);
      lst.Add("LS", "grls", axis: ePivotFieldAxis.Row);
      lst.Add("SR", "grsr", axis: ePivotFieldAxis.Row);
      lst.Add("PR", "grpe", axis: ePivotFieldAxis.Row);
      lst.Add("PR Name", "peN", axis: ePivotFieldAxis.Row);
      lst.Add("Date", "grD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Book Date", "guBookD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Show", "guShow", axis: ePivotFieldAxis.Row);
      lst.Add("Deposited", "grDeposit", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Received", "guDepositReceived", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Burned", "grDepositTwisted", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("CxC", "grDepositCxC", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Currency", "cuN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Payment Type", "ptN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      return lst;
    }

    #endregion RptDeposits

    #region RptBurnedDeposits

    /// <summary>
    /// Formato para el reporte Burned Deposits
    /// </summary>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptBurnedDeposits()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Ch B", "grID", axis: ePivotFieldAxis.Row);
      lst.Add("Chb PP", "grNum", axis: ePivotFieldAxis.Row);
      lst.Add("Date", "grD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Guest ID", "grgu", axis: ePivotFieldAxis.Row);
      lst.Add("Last Name", "grGuest", axis: ePivotFieldAxis.Row);
      lst.Add("Hotel", "grHotel", axis: ePivotFieldAxis.Row);
      lst.Add("Loc", "grlo", axis: ePivotFieldAxis.Row);
      lst.Add("SR", "grsr", axis: ePivotFieldAxis.Row);
      lst.Add("PR", "grpe", axis: ePivotFieldAxis.Row);
      lst.Add("PR Name", "peN", axis: ePivotFieldAxis.Row);
      lst.Add("Host", "grHost", axis: ePivotFieldAxis.Row);
      lst.Add("Member #", "memberNum", axis: ePivotFieldAxis.Row);
      lst.Add("Processable", "procAmount", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Pending", "pendAmount", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Burned", "grDepositTwisted", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("Comments", "grComments", axis: ePivotFieldAxis.Row);
      lst.Add("Currency", "cuN", axis: ePivotFieldAxis.Column);
      lst.Add("Payment Type", "ptN", axis: ePivotFieldAxis.Column);
      return lst;
    }

    #endregion RptBurnedDeposits

    #region RptBurnedDepositsByResorts

    /// <summary>
    /// Formato para el reporte Burned Deposits by Resorts
    /// </summary>
    /// <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptBurnedDepositsByResorts()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Resort", "guHotelB", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Ch B", "grID", axis: ePivotFieldAxis.Row);
      lst.Add("Chb PP", "grNum", axis: ePivotFieldAxis.Row);
      lst.Add("Date", "grD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Guest ID", "grgu", axis: ePivotFieldAxis.Row);
      lst.Add("Last Name", "grGuest", axis: ePivotFieldAxis.Row);
      lst.Add("Hotel", "grHotel", axis: ePivotFieldAxis.Row);
      lst.Add("Loc", "grlo", axis: ePivotFieldAxis.Row);
      lst.Add("SR", "grsr", axis: ePivotFieldAxis.Row);
      lst.Add("PR", "grpe", axis: ePivotFieldAxis.Row);
      lst.Add("PR Name", "peN", axis: ePivotFieldAxis.Row);
      lst.Add("Host", "grHost", axis: ePivotFieldAxis.Row);
      lst.Add("Member #", "memberNum", axis: ePivotFieldAxis.Row);
      lst.Add("Processable", "procAmount", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Pending", "pendAmount", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Burned", "grDepositTwisted", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Currency", "cuN", axis: ePivotFieldAxis.Column);
      lst.Add("Payment Type", "ptN", axis: ePivotFieldAxis.Column);
      return lst;
    }

    #endregion RptBurnedDepositsByResorts

    #region RptPaidDeposits

    /// <summary>
    /// Formato para el reporte Paid Deposits
    /// </summary>
    /// <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptPaidDeposits()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Ch B", "grID", axis: ePivotFieldAxis.Row);
      lst.Add("Chb PP", "grNum", axis: ePivotFieldAxis.Row);
      lst.Add("Date", "grD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Guest ID", "grgu", axis: ePivotFieldAxis.Row);
      lst.Add("Guest Name", "grGuest", axis: ePivotFieldAxis.Row);
      lst.Add("Hotel", "grHotel", axis: ePivotFieldAxis.Row);
      lst.Add("LS", "grls", axis: ePivotFieldAxis.Row);
      lst.Add("SR", "grsr", axis: ePivotFieldAxis.Row);
      lst.Add("PR", "grpe", axis: ePivotFieldAxis.Row);
      lst.Add("Host", "grHost", axis: ePivotFieldAxis.Row);
      lst.Add("Book D", "guBookD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Deposit", "grDeposit", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Burned", "grDepositTwisted", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Currency", "cuN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Payment Type", "ptN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("PR Name", "peN", axis: ePivotFieldAxis.Row, isVisible: false);
      return lst;
    }

    #endregion RptPaidDeposits

    #endregion Deposits

    #region Gift

    #region RptCancelledGiftsManifest

    /// <summary>
    /// Formato para el reporte Daily Gift Simple
    /// </summary>
    /// <history>
    /// [edgrodriguez] 06/Jun/2016 Created
    /// </history>
    public static ColumnFormatList RptCancelledGiftsManifest()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("GUID", "grgu", axis: ePivotFieldAxis.Row);
      lst.Add("Guest Name", "grGuest", axis: ePivotFieldAxis.Row);
      lst.Add("SR", "grsr", axis: ePivotFieldAxis.Row);
      lst.Add("Loc", "grlo", axis: ePivotFieldAxis.Row);
      lst.Add("Host", "grHost", axis: ePivotFieldAxis.Row);
      lst.Add("Host Name", "peN", axis: ePivotFieldAxis.Row);
      lst.Add("Chb", "grID", axis: ePivotFieldAxis.Row);
      lst.Add("Date", "grD", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.Date);
      lst.Add("Deposit", "grDeposit", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Burned", "grDepositTwisted", format: EnumFormatTypeExcel.DecimalNumberWithCero, function: DataFieldFunctions.Sum);
      lst.Add("Taxi", "grTaxiOut", format: EnumFormatTypeExcel.DecimalNumberWithCero);
      lst.Add("Quantity", "geQty", format: EnumFormatTypeExcel.Number);
      lst.Add("Cost", "Cost", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Folios", "geFolios");
      lst.Add("Comments", "grComments", axis: ePivotFieldAxis.Row);
      lst.Add("Gift", "Gift");
      lst.Add("GiftN", "GiftN");
      lst.Add("grcu", "grcu");
      lst.Add("grpt", "grpt");
      return lst;
    }

    #endregion RptDailyGiftSimple

    #region RptDailyGiftSimple

    /// <summary>
    /// Formato para el reporte Daily Gift Simple
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptDailyGiftSimple()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Ch B", "grID", axis: ePivotFieldAxis.Row);
      lst.Add("Quantity", "geQty", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values);
      lst.Add("Location", "grlo", axis: ePivotFieldAxis.Row);
      lst.Add("Gift", "giShortN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Gift Name", "giN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Cancelled", "grCancel", isVisible: false);
      return lst;
    }

    #endregion RptDailyGiftSimple

    #region RptGiftsByCategory

    /// <summary>
    /// Formato para el reporte Gifts By Category
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptGiftsByCategory()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Gift", "Gift", axis: ePivotFieldAxis.Row);
      lst.Add("Quantity", "Quantity", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Total Qty", "TotalQty", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Unit Cost", "UnitCost", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row);
      lst.Add("Total Cost", "TotalCost", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Day", "Day", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Category", "Category", axis: ePivotFieldAxis.Row, isVisible: false);
      return lst;
    }

    #endregion RptGiftsByCategory

    #region RptGiftsByCategoryProgram

    /// <summary>
    /// Formato para el reporte Gifts By Category
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptGiftsByCategoryProgram()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Program", "Program", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Gift", "Gift", axis: ePivotFieldAxis.Row);
      lst.Add("Quantity", "Quantity", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Total Qty", "TotalQty", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Unit Cost", "UnitCost", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row);
      lst.Add("Total Cost", "TotalCost", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Day", "Day", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Category", "Category", isVisible: false);
      return lst;
    }

    #endregion RptGiftsByCategoryProgram

    #region RptGiftsCertificates

    /// <summary>
    /// Formato para el reporte Gifts Certificates
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptGiftsCertificates()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Status", "Status", isGroup: true, isVisible: false);
      lst.Add("Gift ID", "GiftID", axis: ePivotFieldAxis.Row);
      lst.Add("Gift Name", "GiftN", axis: ePivotFieldAxis.Row);
      lst.Add("Receipt", "Receipt", axis: ePivotFieldAxis.Row);
      lst.Add("Folios", "Folios", axis: ePivotFieldAxis.Row);
      lst.Add("Date", "Date", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Host", "Host", axis: ePivotFieldAxis.Row);
      lst.Add("Guest", "Guest", axis: ePivotFieldAxis.Row);
      lst.Add("Location", "Location", axis: ePivotFieldAxis.Row);
      lst.Add("Adults", "Adults", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Minors", "Minors", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Extra Adults", "ExtraAdults", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Pax", "Pax", format: EnumFormatTypeExcel.DecimalNumber, axis: ePivotFieldAxis.Row);
      lst.Add("CxC", "CxC", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Paid", "Paid", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Refund", "Refund", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Comments", "Comments", axis: ePivotFieldAxis.Row);
      lst.Add("Currency", "Currency", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Payment Type", "PaymentType", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      return lst;
    }

    #endregion RptGiftsCertificates

    #region RptGiftsManifest

    /// <summary>
    /// Formato para el reporte Gifts Manifest
    /// </summary>
    /// <history>
    /// [edgrodriguez] 09/May/2016 Created
    /// </history>
    public static ColumnFormatList RptGiftsManifest()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Guest ID", "GuestID", axis: ePivotFieldAxis.Row);
      lst.Add("Reserv ID", "ReservID", axis: ePivotFieldAxis.Row);
      lst.Add("Date", "Date", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Guest", "Guest", axis: ePivotFieldAxis.Row);
      lst.Add("Guest 2", "Guest2", axis: ePivotFieldAxis.Row);
      lst.Add("Pax", "Pax", format: EnumFormatTypeExcel.DecimalNumber, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Agency", "Agency", axis: ePivotFieldAxis.Row);
      lst.Add("Membership", "Membership", axis: ePivotFieldAxis.Row);
      lst.Add("Location", "Location", axis: ePivotFieldAxis.Row);
      lst.Add("PR", "PR", axis: ePivotFieldAxis.Row);
      lst.Add("PR Name", "PRN", axis: ePivotFieldAxis.Row);
      lst.Add("Host", "Host", axis: ePivotFieldAxis.Row);
      lst.Add("Host Name", "HostN", axis: ePivotFieldAxis.Row);
      lst.Add("Receipt ID", "ReceiptID", axis: ePivotFieldAxis.Row);
      lst.Add("Show", "Show", axis: ePivotFieldAxis.Row);
      lst.Add("Hotel Burned", "HotelBurned", axis: ePivotFieldAxis.Row);
      lst.Add("Cancel", "Cancel", axis: ePivotFieldAxis.Row);
      lst.Add("Cancel Date", "CancelledDate", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Deposited", "Deposited", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Taxi Out", "TaxiOut", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Taxi Out Diff", "TaxiOutDiff", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Quantity", "Quantity", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Cost", "Cost", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Folios", "Folios");
      lst.Add("Total", "TotalCost", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Comments", "Comments", axis: ePivotFieldAxis.Row);
      lst.Add("Burned", "Burned", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum, isVisible: false);
      lst.Add("Currency", "Currency");
      lst.Add("Payment Type", "PaymentType");
      lst.Add("Gift", "Gift");
      lst.Add("GiftN", "GiftN");
      lst.Add("Adults", "Adults", format: EnumFormatTypeExcel.DecimalNumber, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum, isVisible: false);
      lst.Add("Minors", "Minors", format: EnumFormatTypeExcel.DecimalNumber, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum, isVisible: false);
      return lst;
    }

    #endregion

    #region RptGiftsReceipts

    /// <summary>
    /// Formato para el reporte Gifts Manifest
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/May/2016 Created
    /// </history>
    public static ColumnFormatList RptGiftsReceipts()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Guest ID", "GuestID", axis: ePivotFieldAxis.Row);
      lst.Add("Reserv ID", "ReservID", axis: ePivotFieldAxis.Row);
      lst.Add("Date", "Date", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Guest", "Guest", axis: ePivotFieldAxis.Row);
      lst.Add("Guest 2", "Guest2", axis: ePivotFieldAxis.Row);
      lst.Add("Pax", "Pax", format: EnumFormatTypeExcel.DecimalNumber, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Agency", "Agency", axis: ePivotFieldAxis.Row);
      lst.Add("Membership", "Membership", axis: ePivotFieldAxis.Row);
      lst.Add("Location", "Location", axis: ePivotFieldAxis.Row);
      lst.Add("PR", "PR", axis: ePivotFieldAxis.Row);
      lst.Add("PR Name", "PRN", axis: ePivotFieldAxis.Row);
      lst.Add("Host", "Host", axis: ePivotFieldAxis.Row);
      lst.Add("Host Name", "HostN", axis: ePivotFieldAxis.Row);
      lst.Add("Receipt ID", "ReceiptID", axis: ePivotFieldAxis.Row);
      lst.Add("Show", "Show", axis: ePivotFieldAxis.Row);
      lst.Add("Hotel Burned", "HotelBurned", axis: ePivotFieldAxis.Row);
      lst.Add("Cancel", "Cancel", axis: ePivotFieldAxis.Row);
      lst.Add("Cancel Date", "CancelledDate", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Deposited", "Deposited", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Burned", "Burned", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Taxi Out", "TaxiOut", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Taxi Out Diff", "TaxiOutDiff", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Quantity", "Quantity", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Cost", "Cost", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("With Cost", "WithCost", format: EnumFormatTypeExcel.Boolean);
      lst.Add("Public Price", "PublicPrice", format: EnumFormatTypeExcel.Currency);
      lst.Add("Folios", "Folios");
      lst.Add("Total", "TotalCost", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Comments", "Comments", axis: ePivotFieldAxis.Row);

      lst.Add("Currency", "Currency");
      lst.Add("Payment Type", "PaymentType");
      lst.Add("Gift", "Gift");
      lst.Add("GiftN", "GiftN");
      lst.Add("Adults", "Adults", isVisible: false);
      lst.Add("Minors", "Minors", isVisible: false);
      return lst;
    }

    #endregion

    #region RptGiftsReceiptsPayments

    /// <summary>
    /// Formato para el reporte Gifts Receipts Payments
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptGiftsReceiptsPayments()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("GroupSource1", "GroupSource1", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("GroupSource2", "GroupSource2", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Source", "Source", axis: ePivotFieldAxis.Row);
      lst.Add("Amount", "Amount", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Amount US", "AmountUS", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("PaymentType", "PaymentType", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Currency", "Currency", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      return lst;
    }

    #endregion RptGiftsReceiptsPayments

    #region RptGiftsSale

    /// <summary>
    /// Formato para el reporte Gifts Sale
    /// </summary>
    /// <history>
    /// [edgrodriguez] 28/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptGiftsSale()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Program", "Program", axis: ePivotFieldAxis.Row);
      lst.Add("Receipt", "Receipt", axis: ePivotFieldAxis.Row);
      lst.Add("Date", "Date", format: EnumFormatTypeExcel.Date);
      lst.Add("Cancel", "Cancel", axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Count);
      lst.Add("Cancel Date", "CancelDate", axis: ePivotFieldAxis.Row);
      lst.Add("Sales Room", "SalesRoom", axis: ePivotFieldAxis.Row);
      lst.Add("Lead Source", "LeadSource", axis: ePivotFieldAxis.Row);
      lst.Add("PR ID", "PR", axis: ePivotFieldAxis.Row);
      lst.Add("PR Name", "PRN", axis: ePivotFieldAxis.Row);
      lst.Add("Out Invit", "OutInvit", axis: ePivotFieldAxis.Row);
      lst.Add("GUID", "GuestID", axis: ePivotFieldAxis.Row);
      lst.Add("Last Name", "LastName", axis: ePivotFieldAxis.Row);
      lst.Add("First Name", "FirstName", axis: ePivotFieldAxis.Row);
      lst.Add("Gift ID", "Gift", axis: ePivotFieldAxis.Row);
      lst.Add("Gift Name", "GiftN", axis: ePivotFieldAxis.Row);
      lst.Add("Gift Sale", "GiftSale", axis: ePivotFieldAxis.Row);
      lst.Add("Adults", "Adults", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Minors", "Minors", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Extra Adults", "ExtraAdults", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Price US", "PriceUS", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Price MX", "PriceMX", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Price CAN", "PriceCAN", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Amount", "Amount", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Total To Pay", "TotalToPay", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Payment Total", "PaymentTotal", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Difference", "Difference", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("User ID", "User", axis: ePivotFieldAxis.Row);
      lst.Add("User Name", "UserN", axis: ePivotFieldAxis.Row);
      lst.Add("Exchange Rate", "ExchangeRate", axis: ePivotFieldAxis.Row);
      lst.Add("Comments", "Comments", axis: ePivotFieldAxis.Row);
      lst.Add("Source", "Source", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Currency", "Currency", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("PaymentType", "PaymentType", axis: ePivotFieldAxis.Column);
      return lst;
    }

    #endregion RptGiftsReceiptsPayments

    #region RptGiftsUsedBySistur

    /// <summary>
    /// Formato para el reporte Gifts Used by Sistur
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptGiftsUsedBySistur()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Sales Room", "srN", isGroup: true);
      lst.Add("Program", "pgN", isGroup: true);
      lst.Add("Lead Source", "lsN", isGroup: true);
      lst.Add("Rcpt", "grID");
      lst.Add("Rcpt Date", "grD", format: EnumFormatTypeExcel.Date);
      lst.Add("Cancel", "grCancel", function: DataFieldFunctions.Count);
      lst.Add("Cancel Date", "grCancelD", format: EnumFormatTypeExcel.Date);
      lst.Add("Exch", "grExchange", function: DataFieldFunctions.Count);
      lst.Add("GUID", "guID");
      lst.Add("Reserv #", "guHReservID");
      lst.Add("Out Inv", "guOutInvitNum");
      lst.Add("Last Name", "guLastName1");
      lst.Add("First Name", "guFirstName1");
      lst.Add("Qty", "geQty", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Unit", "giQty", format: EnumFormatTypeExcel.Number);
      lst.Add("Gift ID", "giID");
      lst.Add("Gift Name", "giN");
      lst.Add("Sistur Promotion", "Promotion");
      lst.Add("Granted", "Granted", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sistur Cancel", "geCancelPVPPromo", function: DataFieldFunctions.Count);
      lst.Add("Used", "gspUsed", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sistur Coupon", "gspCoupon");
      lst.Add("Coupon Date", "gspD", format: EnumFormatTypeExcel.Date);
      lst.Add("Pax", "gspPax", format: EnumFormatTypeExcel.Number);
      lst.Add("Sistur Price", "gspAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Cost", "Cost", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Difference", "Difference", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("CeCos", "Cecos");
      return lst;
    }

    #endregion RptGiftsUsedBySistur

    #region RptWeeklyGiftSimple

    /// <summary>
    /// Formato para el reporte Daily Gift Simple
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptWeeklyGiftSimple()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Date", "grD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Quantity", "Qty", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values);
      lst.Add("Gift Name", "Gift", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Gift", "ShortN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      return lst;
    }

    #endregion RptWeeklyGiftSimple

    #endregion Gift

    #region Guests

    #region RptGuestCeco

    /// <summary>
    /// Formato para el reporte Guest CECO
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static List<ColumnFormat> RptGuestCeco()
    {
      return new List<ColumnFormat>() {
new ColumnFormat() { Title = "Society", PropertyName = "soccecoid", Axis = ePivotFieldAxis.Row, Order = 6 },
new ColumnFormat() { Title = "GUID", PropertyName = "guID", Axis = ePivotFieldAxis.Row, Order = 4 },
new ColumnFormat() { Title = "Book Date", PropertyName = "guBookD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 7 },
new ColumnFormat() { Title = "Sales Room", PropertyName = "srN", Axis = ePivotFieldAxis.Row, Order = 1, Outline = true },
new ColumnFormat() { Title = "Activity", PropertyName = "acn", Axis = ePivotFieldAxis.Row, Order = 2, Outline = true },
new ColumnFormat() { Title = "CECO", PropertyName = "ceco", Axis = ePivotFieldAxis.Row, Order = 5 },
new ColumnFormat() { Title = "Market Segment", PropertyName = "mksN", Axis = ePivotFieldAxis.Row, Order = 3, Outline = true }
      };
    }

    #endregion RptGuestCeco

    #region RptGuestNoBuyers

    /// <summary>
    /// Formato para el reporte Guest No Buyers
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptGuestNoBuyers()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Program", "Program", isGroup: true, isVisible: false);
      lst.Add("LeadSource", "LeadSource", isGroup: true, isVisible: false);
      lst.Add("GUID", "GuestID");
      lst.Add("Last Name", "LastName");
      lst.Add("First Name", "FirstName");
      lst.Add("Email", "Email");
      lst.Add("Email 2", "Email2");
      lst.Add("City", "City");
      lst.Add("State", "State");
      lst.Add("Country", "CountryN");
      lst.Add("Country ID", "Country", isVisible: false);
      return lst;
    }

    #endregion RptGuestNoBuyers

    #region RptInOut

    /// <summary>
    /// Formato para el reporte In & Out
    /// </summary>
    /// <history>
    /// [edgrodriguez] 09/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptInOut()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Sales Room", "SalesRoom", isGroup: true, isVisible: false);
      lst.Add("Location", "Location", isGroup: true, isVisible: false);
      lst.Add("GUID", "GUID");
      lst.Add("Hotel", "Hotel");
      lst.Add("Room", "Room");
      lst.Add("Pax", "Pax", format: EnumFormatTypeExcel.DecimalNumber);
      lst.Add("Last Name", "LastName");
      lst.Add("First Name", "FirstName");
      lst.Add("Agency ID", "Agency");
      lst.Add("Agency", "AgencyN");
      lst.Add("Country ID", "Country");
      lst.Add("Country", "CountryN");
      lst.Add("Show D", "ShowDate", format: EnumFormatTypeExcel.Date);
      lst.Add("Time In", "TimeIn", format: EnumFormatTypeExcel.Time);
      lst.Add("Time Out", "TimeOut", format: EnumFormatTypeExcel.Time);
      lst.Add("Direct", "Direct", function: DataFieldFunctions.Sum);
      lst.Add("Tour", "Tour", function: DataFieldFunctions.Count);
      lst.Add("In Out", "InOut", function: DataFieldFunctions.Count);
      lst.Add("Walk Out", "WalkOut", function: DataFieldFunctions.Count);
      lst.Add("Courtesy Tour", "CourtesyTour", function: DataFieldFunctions.Count);
      lst.Add("Save Program", "SaveProgram", function: DataFieldFunctions.Count);
      lst.Add("PR 1", "PR1");
      lst.Add("PR1 Name", "PR1N");
      lst.Add("PR 2", "PR2");
      lst.Add("PR2 Name", "PR2N");
      lst.Add("PR 3", "PR3");
      lst.Add("PR3 Name", "PR3N");
      lst.Add("Host", "Host");
      lst.Add("Comments", "Comments");
      return lst;
    }

    #endregion RptInOut

    #region RptManifestRange

    /// <summary>
    /// Formato para el reporte Guest Manifest Range
    /// </summary>
    /// <history>
    /// [edgrodriguez] 06/Jun/2016 Created
    /// </history>
    public static ColumnFormatList RptManifestRange()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Date", "DateManifest", format: EnumFormatTypeExcel.Date);
      lst.Add("Group", "SaleTypeN", isGroup: true);
      lst.Add("GUID", "guID");
      lst.Add("SR", "SalesRoom");
      lst.Add("Loc", "Location");
      lst.Add("Hotel", "Hotel");
      lst.Add("Room", "Room");
      lst.Add("Pax", "Pax");
      lst.Add("Last Name", "LastName");
      lst.Add("First Name", "FirstName");
      lst.Add("Agency ID", "Agency");
      lst.Add("Agency", "AgencyN");
      lst.Add("Country ID", "Country");
      lst.Add("Country", "CountryN");
      lst.Add("Show", "showD", format: EnumFormatTypeExcel.Date);
      lst.Add("T In", "TimeInT", format: EnumFormatTypeExcel.Time);
      lst.Add("T Out", "TimeOutT", format: EnumFormatTypeExcel.Time);
      lst.Add("Check In", "CheckIn", format: EnumFormatTypeExcel.Date);
      lst.Add("Check Out", "CheckOut", format: EnumFormatTypeExcel.Date);
      lst.Add("D", "Direct", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Tr", "Tour", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("IO", "InOut", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("WO", "WalkOut", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("CT", "CTour", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sve", "SaveTour", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("PR 1", "PR1");
      lst.Add("PR 1 Name", "PR1N");
      lst.Add("PR 2", "PR2");
      lst.Add("PR 2 Name", "PR2N");
      lst.Add("PR 3", "PR3");
      lst.Add("PR 3 Name", "PR3N");
      lst.Add("Liner 1", "Liner1");
      lst.Add("Liner 1 Name", "Liner1N");
      lst.Add("Liner 2", "Liner2");
      lst.Add("Liner 2 Name", "Liner2N");
      lst.Add("Closer 1", "Closer1");
      lst.Add("Closer 1 Name", "Closer1N");
      lst.Add("Closer 2", "Closer2");
      lst.Add("Closer 2 Name", "Closer2N");
      lst.Add("Closer 3", "Closer3");
      lst.Add("Closer 3 Name", "Closer3N");
      lst.Add("Exit 1", "Exit1");
      lst.Add("Exit 1 Name", "Exit1N");
      lst.Add("Exit 2", "Exit2");
      lst.Add("Exit 2 Name", "Exit2N");
      lst.Add("Hostess", "Hostess");
      lst.Add("Proc Sales", "ProcSales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Proc Orig", "ProcOriginal", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Proc New", "ProcNew", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Proc Gross", "ProcGross", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Pend Sales", "PendSales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Pend Original", "PendOriginal", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Pend New", "PendNew", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Pend Gross", "PendGross", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("C Cost", "ClosingCost", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Memb #", "MemberShip");
      lst.Add("Comments", "Comments");
      lst.Add("SaleType", "SaleType", isVisible: false);

      return lst;
    }

    #endregion RptManifestRange      

    #region RptGuestNoShow

    /// <summary>
    /// Formato para el reporte Guest No Show
    /// </summary>
    /// <history>
    /// [edgrodriguez] 09/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptGuestNoShow()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("GUID", "guID");
      lst.Add("Loc", "guloInvit");
      lst.Add("Room", "guRoomNum");
      lst.Add("Pax", "gupax");
      lst.Add("Last Name", "guLastName1");
      lst.Add("First Name", "guFirstName1");
      lst.Add("Agency ID", "guag");
      lst.Add("Agency", "agN");
      lst.Add("Country ID", "guco");
      lst.Add("Country", "coN");
      lst.Add("Chk-In Date", "guCheckInD", format: EnumFormatTypeExcel.Date);
      lst.Add("Book D", "guBookD", format: EnumFormatTypeExcel.Date);
      lst.Add("Book Cancel", "guBookCanc");
      lst.Add("Deposit", "guDeposit", format: EnumFormatTypeExcel.Currency);
      lst.Add("Hotel", "guHotel", isVisible: false);
      return lst;
    }

    #endregion RptGuestNoShow

    #endregion Guests

    #region Meal Tickets

    #region RptMealTickets

    /// <summary>
    /// Formato para el reporte Meal Tickets
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptMealTickets()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Rate Type", "RateTypeN", isGroup: true, isVisible: false);
      lst.Add("No", "meID");
      lst.Add("Date", "meD", format: EnumFormatTypeExcel.Date);
      lst.Add("Guest ID", "megu");
      lst.Add("Guest Name", "guLastName1");
      lst.Add("Qty", "meQty", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Type", "myN");
      lst.Add("Folios", "meFolios");
      lst.Add("Adults", "meAdults", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Minors", "meMinors", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Show", "guShow");
      lst.Add("Total", "Total", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Loc", "guloInfo");
      lst.Add("PR", "guPRInvit1");
      lst.Add("PR Name", "guPRInvit1N");
      lst.Add("Host", "guEntryHost");
      lst.Add("Host Name", "guEntryHostN");
      lst.Add("Liner", "guLiner1");
      lst.Add("Liner Name", "guLiner1N");
      lst.Add("Comments", "meComments");
      lst.Add("Personnel ID", "mepe");
      lst.Add("Personnel Name", "peN");
      lst.Add("# Collaborator", "peCollaboratorID");
      lst.Add("Agency", "agN");
      lst.Add("Representative", "merep");
      lst.Add("mera", "mera", isVisible: false);
      return lst;
    }

    #endregion RptMealTickets

    #region RptMealTicketsByHost

    /// <summary>
    /// Formato para el reporte Meal Tickets by Host
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptMealTicketsByHost()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Rate Type", "RateTypeN", isGroup: true, isVisible: false);
      lst.Add("Host", "guEntryHost", isGroup: true, isVisible: false);
      lst.Add("No", "meID");
      lst.Add("Date", "meD", format: EnumFormatTypeExcel.Date);
      lst.Add("Guest ID", "megu");
      lst.Add("Guest Name", "guLastName1");
      lst.Add("Qty", "meQty", format: EnumFormatTypeExcel.Number);
      lst.Add("Type", "myN");
      lst.Add("Folios", "meFolios");
      lst.Add("Adults", "meAdults", format: EnumFormatTypeExcel.Number);
      lst.Add("Minors", "meMinors", format: EnumFormatTypeExcel.Number);
      lst.Add("Show", "guShow");
      lst.Add("Total", "Total", format: EnumFormatTypeExcel.Currency);
      lst.Add("Loc", "guloInfo");
      lst.Add("PR", "guPRInvit1");
      lst.Add("PR Name", "guPRInvit1N");
      lst.Add("Liner", "guLiner1");
      lst.Add("Liner Name", "guLiner1N");
      lst.Add("Comments", "meComments");
      lst.Add("# Collaborator", "peCollaboratorID");
      lst.Add("Collaborator Name", "peN");
      lst.Add("Agency", "agN");
      lst.Add("Representative", "merep");
      lst.Add("mera", "mera", isVisible: false);
      lst.Add("mepe", "mepe", isVisible: false);
      lst.Add("Host Name", "guEntryHostN", isVisible: false);
      return lst;
    }

    #endregion RptMealTicketsByHost

    #region RptMealTicketsCost

    /// <summary>
    /// Formato para el reporte Meal Tickets Cost
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptMealTicketsCost()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Date", "Date", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Adults", "Adults", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("Minors", "Minors", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("$ Adults", "AdultsAmount", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("$ Minors", "MinorsAmount", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("$ Total", "TotalAmount", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("Type", "Type", axis: ePivotFieldAxis.Column);
      return lst;
    }

    #endregion RptMealTicketsCost

    #endregion Meal Tickets

    #region Memberships

    #region RptMemberships

    /// <summary>
    /// Formato para el reporte Memberships
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptMemberships()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Sale ID", "saID");
      lst.Add("Sale Date", "saD", format: EnumFormatTypeExcel.Date);
      lst.Add("Proc Date", "saProcD", format: EnumFormatTypeExcel.Date);
      lst.Add("Canc Date", "saCancelD", format: EnumFormatTypeExcel.Date);
      lst.Add("Loc", "salo");
      lst.Add("SR", "sasr");
      lst.Add("Membership", "saMembershipNum");
      lst.Add("MT", "samt");
      lst.Add("ST", "sast");
      lst.Add("Last Name 1", "saLastName1");
      lst.Add("First Name 1", "saFirstName1");
      lst.Add("Last Name 2", "saLastName2");
      lst.Add("First Name 2", "saFirstName2");
      lst.Add("Proc Vol", "ProcAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("OOP Vol", "OOPAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Canc Vol", "CancAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Total Proc", "TotalProcAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Pend Vol", "PendAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("PR1", "saPR1");
      lst.Add("PR2", "saPR2");
      lst.Add("PR3", "saPR3");
      lst.Add("Liner1", "saLiner1");
      lst.Add("Liner2", "saLiner2");
      lst.Add("Closer1", "saCloser1");
      lst.Add("Closer2", "saCloser2");
      lst.Add("Closer3", "saCloser3");
      lst.Add("Exit1", "saExit1");
      lst.Add("Exit2", "saExit2");
      lst.Add("VLO", "saVLO", format: EnumFormatTypeExcel.Number);
      lst.Add("Podium", "saPodium");
      lst.Add("Comments", "saComments");
      lst.Add("GUID", "sagu", isVisible: false);
      return lst;
    }

    #endregion RptMemberships

    #region RptMembershipsByAgencyMarket

    /// <summary>
    /// Formato para el reporte Memberships
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptMembershipsByAgencyMarket()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Market", "mkN", isGroup: true, isVisible: false);
      lst.Add("Agency", "agN", isGroup: true, isVisible: false);
      lst.Add("Sale ID", "saID");
      lst.Add("Sale Date", "saProcD", format: EnumFormatTypeExcel.Date);
      lst.Add("Lead Source", "lsN");
      lst.Add("Sales Room", "srN");
      lst.Add("Membership", "saMembershipNum");
      lst.Add("Membership Type", "mtN");
      lst.Add("ST", "sast");
      lst.Add("Last Name", "saLastName1");
      lst.Add("First Name", "saFirstName1");
      lst.Add("Proc Vol", "ProcAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("OOP Vol", "OOPAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Canc Vol", "CancAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Total Proc", "TotalProcAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Pend Vol", "PendAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("% DP", "saDownPaymentPercentage", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([ProcAmount]=0,0,[saDownPayment]/[ProcAmount])");
      lst.Add("$ DP", "saDownPayment", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("% EDP", "saDownPaymentPaidPercentage", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([ProcAmount]=0,0,[saDownPaymentPaid]/[ProcAmount])");
      lst.Add("$ EDP", "saDownPaymentPaid", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Comments", "saComments");
      return lst;
    }

    #endregion RptMembershipsByAgencyMarket

    #region RptMembershipsByHost

    /// <summary>
    /// Formato para el reporte Memberships By Host
    /// </summary>
    /// <history>
    /// [edgrodriguez] 14/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptMembershipsByHost()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Host", "guEntryHost", isGroup: true, isVisible: false);
      lst.Add("Sale Type", "SaleType", isGroup: true, isVisible: false);
      lst.Add("Sale ID", "saID");
      lst.Add("Sale Date", "saD", format: EnumFormatTypeExcel.Date);
      lst.Add("LS", "sals");
      lst.Add("SR", "sasr");
      lst.Add("Membership", "saMembershipNum");
      lst.Add("MT", "samt");
      lst.Add("ST", "sast");
      lst.Add("Last Name", "saLastName1");
      lst.Add("First Name", "saFirstName1");
      lst.Add("Proc Vol", "ProcAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("OOP Vol", "OOPAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Canc Vol", "CancAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Total Proc", "TotalProcAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Pend Vol", "PendAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Comments", "saComments");
      lst.Add("Host Name", "guEntryHostN", isVisible: false);
      return lst;
    }

    #endregion RptMembershipsByHost

    #endregion Memberships

    #region Production

    #region RptProductionBySalesRoom

    /// <summary>
    /// Formato para el reporte Production by SalesRoom
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptProductionBySalesRoom()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Sales Room", "SalesRoom", format: EnumFormatTypeExcel.Number);
      lst.Add("Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh %", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossBooks]=0,0,[GrossShows]/[GrossBooks])");
      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Amount", "SalesAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows]=0,0,[SalesAmount]/[Shows])");
      lst.Add("Ci %", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows]=0,0,[Sales]/[Shows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales]=0,0,[SalesAmount]/[Sales])");
      return lst;
    }

    #endregion RptProductionBySalesRoom

    #region RptProductionBySalesRoomMarket

    /// <summary>
    /// Formato para el reporte Production by SalesRoom & Market
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptProductionBySalesRoomMarket()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Market", "Market", isGroup: true, isVisible: false);
      lst.Add("Sales Room", "SalesRoom", format: EnumFormatTypeExcel.Number);
      lst.Add("Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh %", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossBooks]=0,0,[GrossShows]/[GrossBooks])");
      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Amount", "SalesAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows]=0,0,[SalesAmount]/[Shows])");
      lst.Add("Ci %", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows]=0,0,[Sales]/[Shows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales]=0,0,[SalesAmount]/[Sales])");
      return lst;
    }

    #endregion RptProductionBySalesRoomMarket

    #region RptProductionBySalesRoomMarketSubMarket

    /// <summary>
    /// Formato para el reporte Production by SalesRoom,Market & Submarket
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptProductionBySalesRoomMarketSubMarket()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Program", "Program", isGroup: true, isVisible: false);
      lst.Add("Market", "Market", isGroup: true, isVisible: false);
      lst.Add("Submarket", "Submarket", isGroup: true, isVisible: false);
      lst.Add("Sales Room", "SalesRoom", format: EnumFormatTypeExcel.Number);
      lst.Add("Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh %", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossBooks]=0,0,[GrossShows]/[GrossBooks])");
      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Amount", "SalesAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows]=0,0,[SalesAmount]/[Shows])");
      lst.Add("Ci %", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows]=0,0,[Sales]/[Shows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales]=0,0,[SalesAmount]/[Sales])");
      return lst;
    }

    #endregion RptProductionBySalesRoomMarketSubMarket

    #region RptProductionByShowProgram

    /// <summary>
    /// Formato para el reporte Production by Show & Program
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptProductionByShowProgram()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Show Program Category", "ShowProgramCategory", isGroup: true, isVisible: false);
      lst.Add("Show Program", "ShowProgram");
      lst.Add("Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh %", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossBooks]=0,0,[GrossShows]/[GrossBooks])");
      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Amount", "SalesAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows]=0,0,[SalesAmount]/[Shows])");
      lst.Add("Ci %", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows]=0,0,[Sales]/[Shows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales]=0,0,[SalesAmount]/[Sales])");
      return lst;
    }

    #endregion RptProductionByShowProgram

    #region RptProductionByShowProgramProgram

    /// <summary>
    /// Formato para el reporte Production by Show,Program & Program
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptProductionByShowProgramProgram()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Program", "Program", isGroup: true, isVisible: false);
      lst.Add("Show Program Category", "ShowProgramCategory", isGroup: true, isVisible: false);
      lst.Add("Show Program", "ShowProgram");
      lst.Add("Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh %", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossBooks]=0,0,[GrossShows]/[GrossBooks])");
      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Amount", "SalesAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows]=0,0,[SalesAmount]/[Shows])");
      lst.Add("Ci %", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows]=0,0,[Sales]/[Shows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales]=0,0,[SalesAmount]/[Sales])");
      return lst;
    }

    #endregion RptProductionByShowProgramProgram

    #endregion Production

    #region Salesmen

    #region RptCloserStatistic

    /// <summary>
    /// Formato para el reporte Closer Statistics
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/May/2016 Created
    /// </history>
    public static ColumnFormatList RptCloserStatistic()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("PS", "saCloserps", isGroup: true, isVisible: false);
      lst.Add("ID", "saCloser");
      lst.Add("Closer Name", "saCloserN");
      lst.Add("Ups", "Shows", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Proc Sales", "ProcSales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Processable", "ProcAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("OOP Sales", "OOPSales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("OOP", "OOPAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Cancel Sales", "CancelSales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Cancelled", "CancelAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("C %", "CancelF", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([TotalProc]=0, 0, [CancelAmount]/[TotalProc])");
      lst.Add("Subtotal", "TotalProc", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Sales", "TotalProcSales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Total", "TotalProcAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows]=0, 0, [TotalProcAmount]/[Shows])");
      lst.Add("CI %", "ClosingF", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows]=0, 0, [TotalProcSales]/[Shows])");
      lst.Add("AvgSales", "AvgSales", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([TotalProcSales]=0, 0, [TotalProcAmount]/[TotalProcSales])");
      lst.Add("Pend Sales", "PendSales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Pending", "PendAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      return lst;
    }

    #endregion

    #region RptLinerStatistic

    /// <summary>
    /// Formato para el reporte Liner Statistics
    /// </summary>
    /// <history>
    /// [edgrodriguez] 12/May/2016 Created
    /// </history>
    public static ColumnFormatList RptLinerStatistic()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("PS", "saLinerps", isGroup: true, isVisible: false);
      lst.Add("ID", "saLiner");
      lst.Add("Liner Name", "saLinerN");
      lst.Add("Ups", "Shows", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("WO", "WalkOut", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("T Ups", "TotalShows", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Proc Sales", "ProcSales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Processable", "ProcAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("OOP Sales", "OOPSales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("OOP", "OOPAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Cancel Sales", "CancSales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Cancelled", "CancAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("C %", "CancelF", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([TotalProc]=0, 0, [CancAmount]/[TotalProc])");
      lst.Add("Subtotal", "TotalProc", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Sales", "TotalProcSales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Total", "TotalProcAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Proc Sales Ln", "ProcSalesLn", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Proc Ln", "ProcAmountLn", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Proc Sales FM", "ProcSalesFM", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Proc FM", "ProcAmountFM", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Proc Sales FB", "ProcSalesFB", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Proc FB", "ProcAmountFB", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows]=0, 0, [TotalProcAmount]/[Shows])");
      lst.Add("CI %", "ClosingF", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows]=0, 0, [TotalProcSales]/[Shows])");
      lst.Add("AvgSales", "AvgSales", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([TotalProcSales]=0, 0, [TotalProcAmount]/[TotalProcSales])");
      lst.Add("Pend Sales", "PendSales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Pending", "PendAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      return lst;
    }

    #endregion

    #region RptWeeklyMonthlyHostessByPr

    /// <summary>
    /// Formato para el reporte Weekly & Monthly Hostess
    /// </summary>
    /// <history>
    /// [edgrodriguez] 09/Jun/2016 Created
    /// </history>
    public static ColumnFormatList RptWeeklyMonthlyHostessByPr()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("LS", "guls", isGroup: true, isVisible: false);
      lst.Add("PR Name", "guPRInvitN", axis: ePivotFieldAxis.Row);
      lst.Add("PR", "guPRInvit", axis: ePivotFieldAxis.Row);
      lst.Add("D", "guDirect", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("IO", "guInOut", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("WO", "guWalkOut", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Type", "Type", axis: ePivotFieldAxis.Column);
      lst.Add("guloInvit", "guloInvit", isVisible: false);
      lst.Add("gusr", "gusr", isVisible: false);
      return lst;
    }

    public static ColumnFormatList RptWeeklyMonthlyHostessByTourTime()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("LS", "guls", isGroup: true, isVisible: false);
      lst.Add("Day", "guD", format: EnumFormatTypeExcel.Day, axis: ePivotFieldAxis.Row);
      lst.Add("B", "guBook", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Sh", "guShow", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum, formula: "IF([guBook]=0,0,[guShow]/[guBook])");
      lst.Add("D", "guDirect", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Time", "Time", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      return lst;
    }

    #endregion

    #endregion

    #region Taxis

    #region RptTaxisIn

    /// <summary>
    /// Formato para el reporte Taxis In
    /// </summary>
    /// <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptTaxisIn()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Show Type", "ShowType", isGroup: true);
      lst.Add("GUID", "guID");
      lst.Add("Loc", "guloInvit");
      lst.Add("Date", "guShowD", format: EnumFormatTypeExcel.Date);
      lst.Add("Last Name", "guGuest");
      lst.Add("Pax", "guPax", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Adults", "Adults", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Minors", "Minors", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Hotel", "guHotel");
      lst.Add("PR", "guPRInvit1");
      lst.Add("Taxi", "guTaxiIn", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Host", "guEntryHost");
      lst.Add("Comments", "guWComments");
      return lst;
    }

    #endregion RptTaxisIn

    #region RptTaxisOut

    /// <summary>
    /// Formato para el reporte Taxis Out
    /// </summary>
    /// <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptTaxisOut()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Ch B", "grID");
      lst.Add("Chb PP", "grNum");
      lst.Add("Date", "grD", format: EnumFormatTypeExcel.Date);
      lst.Add("LS", "grlo");
      lst.Add("Guest Name", "grGuest");
      lst.Add("Pax", "grPax", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Hotel", "grHotel");
      lst.Add("PR", "grpe");
      lst.Add("Taxi", "grTaxiOut", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Taxi Diff", "grTaxiOutDiff", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Host", "grHost");
      lst.Add("Comments", "guWComments");
      return lst;
    }

    #endregion RptTaxisOut

    #endregion Taxis

    #endregion Reports by Sales Room

    #region Reports by Lead Source

    #region rptDepositsBurnedGuests

    /// <summary>
    /// Formato para el reporte Deposits Burned Guests
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptDepositsBurnedGuests()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("GUID", "guID", axis: ePivotFieldAxis.Row);
      lst.Add("Out Invit", "guOutInvitNum", axis: ePivotFieldAxis.Row);
      lst.Add("Guest", "Guest", axis: ePivotFieldAxis.Row);
      lst.Add("Hotel", "guHotel", axis: ePivotFieldAxis.Row);
      lst.Add("LS", "guls", axis: ePivotFieldAxis.Row);
      lst.Add("SR", "gusr", axis: ePivotFieldAxis.Row);
      lst.Add("Book Date", "guBookD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("PR", "guPRInvit1", axis: ePivotFieldAxis.Row);
      lst.Add("PR Name", "peN", axis: ePivotFieldAxis.Row);
      lst.Add("Burned", "guDepositTwisted", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Currency", "gucu", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Payment Type", "gupt", axis: ePivotFieldAxis.Column, sort: eSortType.Descending);
      lst.Add("Out Invit D", "guInvitD", format: EnumFormatTypeExcel.Date, isVisible: false);
      return lst;
    }

    #endregion rptDepositsBurnedGuests

    #region rptDepositRefunds

    /// <summary>
    /// Formato para el reporte Deposit Refunds
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptDepositRefunds()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Sales Room", "srN");
      lst.Add("Lead Source", "lsN");
      lst.Add("Refund ID", "drID");
      lst.Add("Refund Folio", "drFolio");
      lst.Add("Refund Date", "drD", format: EnumFormatTypeExcel.Date);
      lst.Add("Reservation", "guHReservID");
      lst.Add("Out Invitation", "guOutInvitNum");
      lst.Add("Name", "GuestName");
      lst.Add("Total", "Total", format: EnumFormatTypeExcel.Currency);
      lst.Add("PR ID", "peID");
      lst.Add("PR Name", "peN");
      lst.Add("Amount", "bdAmount", format: EnumFormatTypeExcel.Currency);
      lst.Add("Currency", "cuN");
      lst.Add("Card Type", "ccN");
      lst.Add("Card Number", "bdCardNum");
      lst.Add("Expiration Date", "bdExpD");
      lst.Add("Authorization", "bdAuth", format: EnumFormatTypeExcel.Number);
      return lst;
    }

    #endregion rptDepositRefunds

    #region rptDepositsNoShow

    /// <summary>
    /// Formato para el reporte Deposits No Show
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptDepositsNoShow()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("PR", "guPRInvit1", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("GUID", "guID", axis: ePivotFieldAxis.Row);
      lst.Add("Out Invit", "guOutInvitNum", axis: ePivotFieldAxis.Row);
      lst.Add("Guest", "Guest", axis: ePivotFieldAxis.Row);
      lst.Add("Hotel", "guHotel", axis: ePivotFieldAxis.Row);
      lst.Add("LS", "guls", axis: ePivotFieldAxis.Row);
      lst.Add("SR", "gusr", axis: ePivotFieldAxis.Row);
      lst.Add("Book Date", "guBookD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Invit D", "guInvitD", format: EnumFormatTypeExcel.Date, isVisible: false);
      lst.Add("PR Name", "peN", isVisible: false);
      lst.Add("Deposited", "guDeposit", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Received", "guDepositReceived", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Burned", "guDepositTwisted", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Currrency", "gucu", axis: ePivotFieldAxis.Column);
      lst.Add("Payment Type", "gupt", axis: ePivotFieldAxis.Column);
      return lst;
    }

    #endregion rptDepositsNoShow

    #region rptInOutByPR

    /// <summary>
    /// Formato para el reporte In & Out By PR
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptInOutByPr()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("PR 1", "PR1", isGroup: true, isVisible: false);
      lst.Add("GUID", "GUID");
      lst.Add("Location", "Location");
      lst.Add("Hotel", "Hotel");
      lst.Add("Room", "Room");
      lst.Add("Pax", "Pax", format: EnumFormatTypeExcel.DecimalNumber);
      lst.Add("Last Name", "LastName");
      lst.Add("First Name", "FirstName");
      lst.Add("Agency ID", "Agency");
      lst.Add("Agency", "AgencyN");
      lst.Add("Country ID", "Country");
      lst.Add("Country", "CountryN");
      lst.Add("Show D", "ShowDate", format: EnumFormatTypeExcel.Date);
      lst.Add("Time In", "TimeIn", format: EnumFormatTypeExcel.Time);
      lst.Add("Time Out", "TimeOut", format: EnumFormatTypeExcel.Time);
      lst.Add("Direct", "Direct", function: DataFieldFunctions.Sum);
      lst.Add("Tour", "Tour", function: DataFieldFunctions.Count);
      lst.Add("In Out", "InOut", function: DataFieldFunctions.Count);
      lst.Add("Walk Out", "WalkOut", function: DataFieldFunctions.Count);
      lst.Add("Courtesy Tour", "CourtesyTour", function: DataFieldFunctions.Count);
      lst.Add("Save Program", "SaveProgram", function: DataFieldFunctions.Count);
      lst.Add("PR 2", "PR2");
      lst.Add("PR2 Name", "PR2N");
      lst.Add("PR 3", "PR3");
      lst.Add("PR3 Name", "PR3N");
      lst.Add("Host", "Host");
      lst.Add("Comments", "Comments");
      lst.Add("PR 1 Name", "PR1N", isVisible: false);
      return lst;
    }

    #endregion rptInOutByPR

    #region RptPaidDepositsByPR

    /// <summary>
    /// Formato para el reporte Paid Deposits
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptPaidDepositsByPr()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("PR", "grpe", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Ch B", "grID", axis: ePivotFieldAxis.Row);
      lst.Add("Chb PP", "grNum", axis: ePivotFieldAxis.Row);
      lst.Add("Date", "grD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Guest ID", "grgu", axis: ePivotFieldAxis.Row);
      lst.Add("Book D", "guBookD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Guest Name", "grGuest", axis: ePivotFieldAxis.Row);
      lst.Add("Hotel", "grHotel", axis: ePivotFieldAxis.Row);
      lst.Add("LS", "grls", axis: ePivotFieldAxis.Row);
      lst.Add("SR", "grsr", axis: ePivotFieldAxis.Row);
      lst.Add("Host", "grHost", axis: ePivotFieldAxis.Row);
      lst.Add("Deposit", "grDeposit", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Burned", "grDepositTwisted", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Currency", "cuN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Payment Type", "ptN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("PR Name", "peN", isVisible: false);
      return lst;
    }

    #endregion RptPaidDepositsByPR

    #region RptPersonnelAccess

    /// <summary>
    /// Formato para el reporte Personnel Access
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>✓
    public static ColumnFormatList RptPersonnelAccess()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("lsN", "lsN", isGroup: true, isVisible: false);
      lst.Add("peps", "peps", isGroup: true, isVisible: false);
      lst.Add("ID", "peID");
      lst.Add("Name", "peN");
      lst.Add("Captain", "peCaptain");
      lst.Add("PR", "PR");
      lst.Add("PR Mem", "PRMembers");
      lst.Add("Liner", "Liner");
      lst.Add("Closer", "Closer");
      lst.Add("Exit", "Exit");
      lst.Add("Podium", "Podium");
      lst.Add("PR Capt", "PRCaptain");
      lst.Add("PR Sup", "PRSupervisor");
      lst.Add("Ln Capt", "LinerCaptain");
      lst.Add("Clo Capt", "CloserCaptain");
      lst.Add("Entry H", "EntryHost");
      lst.Add("Gifts H", "GiftsHost");
      lst.Add("Exit H", "ExitHost");
      lst.Add("VLO", "VLO");
      lst.Add("Manager", "Manager");
      lst.Add("Admin", "Administrator");
      return lst;
    }

    #endregion RptPersonnelAccess

    #region RptSelfGen

    /// <summary>
    /// Formato para el reporte Self Gen
    /// </summary>
    /// <history>
    /// [edgrodriguez] 25/Abr/2016 Created
    /// </history>✓
    public static ColumnFormatList RptSelfGen()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Peps", "PRps", isGroup: true, isVisible: false);
      lst.Add("ID", "PRID");
      lst.Add("PR Name", "PRN");
      lst.Add("Sh", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("IO", "IO", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("WO", "WO", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Proc", "Proc", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Processable", "ProcAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("OOP", "OOP", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("OOP Amount", "OOPAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Cancel", "Cancel", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Cancel Amount", "CancelAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("C %", "CancelF", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Subtotal]=0,IF([CancelAmount]=0,0,1),[CancelAmount]/[Subtotal])");
      lst.Add("Subtotal", "Subtotal", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "[ProcAmount]+[OOPAmount]");
      lst.Add("Total", "TotalProc", format: EnumFormatTypeExcel.Number, isCalculated: true, formula: "[Proc]+[OOP]-[Cancel]");
      lst.Add("Total Amount", "TotalProcAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Efficiency", "Eff", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows]=0,0,[TotalProcAmount]/[Shows])");
      lst.Add("CI %", "CI", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows]=0,0,[TotalProc]/[Shows])");
      lst.Add("Avg Sale", "AvgSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([TotalProc]=0,0,[TotalProcAmount]/[TotalProc])");
      lst.Add("Pending", "Pending", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Pending Amount", "PendingAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      return lst;
    }

    #endregion RptSelfGen

    #endregion Reports by Lead Source

    #region General Reports

    #region rptAgencies

    /// <summary>
    /// Formato para el reporte Agencies
    /// </summary>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptAgencies()
    {

      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Agency", "agID");
      lst.Add("Agency Name", "agN");
      lst.Add("Unavailable Motive", "umN");
      lst.Add("Market", "agmk");
      lst.Add("Show Pay", "agShowPay");
      lst.Add("Sale Pay", "agSalePay");
      lst.Add("Rep", "agrp");
      lst.Add("Active", "agA", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);

      return lst;
    }

    #endregion rptAgencies

    #region rptGifts

    /// <summary>
    /// Formato para el reporte general de Gifts
    /// </summary>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static ColumnFormatList RptGifts()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Gift ID", "giID");
      lst.Add("Gift Name", "giN");
      lst.Add("Short N.", "giShortN");
      lst.Add("Order", "giO", format: EnumFormatTypeExcel.Number);
      lst.Add("Price", "giPrice1", format: EnumFormatTypeExcel.Currency);
      lst.Add("Price Min.", "giPrice2", format: EnumFormatTypeExcel.Currency);
      lst.Add("CXC", "giPrice3", format: EnumFormatTypeExcel.Currency);
      lst.Add("CXC Min.", "giPrice4", format: EnumFormatTypeExcel.Currency);
      lst.Add("Package", "giPack", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Category", "gcN");
      lst.Add("Inv.", "giInven", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Folio", "giWFolio", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Pax", "giWPax", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Active", "giA", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      return lst;
    }

    #endregion rptGifts

    #region RptGiftsKardex

    /// <summary>
    /// Formato para el reporte general de Gifts
    /// </summary>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static ColumnFormatList RptGiftsKardex()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Date", "MovD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("In", "InQty", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Out", "OutQty", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Stock.", "InvQty", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values);
      lst.Add("MovGi", "MovGi", axis: ePivotFieldAxis.Column);
      lst.Add("MovGiN", "MovGiN", axis: ePivotFieldAxis.Column);
      return lst;
    }

    #endregion RptGiftsKardex

    #region RptLoginsLog

    /// <summary>
    /// Formato para el reporte general Logins Log
    /// </summary>
    /// <history>
    /// [edgrodriguez] 27/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptLoginsLog()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Date Time", "Date_Time", format: EnumFormatTypeExcel.DateTime);
      lst.Add("Location", "Location");
      lst.Add("Code", "Code");
      lst.Add("Name", "Name");
      lst.Add("PC", "PC");
      return lst;
    }

    #endregion RptLoginsLog

    #region rptPersonnel

    /// <summary>
    /// Formato para el reporte general de Personnel
    /// </summary>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static ColumnFormatList RptPersonnel()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Status", "peps", isGroup: true, isVisible: false);
      lst.Add("Dept", "deN", isGroup: true, isVisible: false);
      lst.Add("Post", "poN", isGroup: true, isVisible: false);
      lst.Add("Place", "Place", isGroup: true, isVisible: false);
      lst.Add("ID", "peID");
      lst.Add("Name", "peN");
      lst.Add("Active", "peA", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Collaborator", "peCollaboratorID", format: EnumFormatTypeExcel.Number);
      lst.Add("Captain", "peCaptain");
      lst.Add("PR", "PR", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("PR Mem", "PRMembers", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Liner", "Liner", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Closer", "Closer", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Exit", "Exit", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Podium", "Podium", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("PR Capt", "PRCaptain", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("PR Sup", "PRSupervisor", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Ln Capt", "LinerCaptain", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Clo Capt", "CloserCaptain", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Entry H", "EntryHost", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Gift H", "GiftsHost", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Exit H", "ExitHost", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("VLO", "VLO", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Manager", "Manager", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      lst.Add("Admin", "Administrator", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);

      return lst;
    }

    #endregion rptPersonnel

    #region rptProductionByLeadSourceMarketMonthly

    /// <summary>
    /// Formato para el reporte general Production By Lead Source & Market(Monthly)
    /// </summary>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptProductionByLeadSourceMarketMonthly()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Program", "Program", isGroup: true);
      lst.Add("Year", "Year", isGroup: true);
      lst.Add("Month", "MonthN", isGroup: true);
      lst.Add("Market", "Market", isGroup: true);
      lst.Add("Lead Source", "LeadSource");
      lst.Add("Arrivals", "Arrivals", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Contacts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Contacts %", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals]=0,0,[Contacts]/[Arrivals])");
      lst.Add("Availables", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Availables %", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts]=0,0,[Availables]/[Contacts])");
      lst.Add("Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Books %", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables]=0,0,[Books]/[Availables])");
      lst.Add("Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Shows %", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossBooks]=0,0,[GrossShows]/[GrossBooks])");
      lst.Add("Shows Arrivals %", "ShowsArrivalsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals]=0,0,[Shows]/[Arrivals])");
      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Amount", "SalesAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Closing %", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows]=0,0,[Sales]/[Shows])");
      lst.Add("Efficiency", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows]=0,0,[SalesAmount]/[Shows])");
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("IO", "InOuts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("WO", "WalkOuts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("RT", "Tours", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("CT", "CourtesyTours", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Save", "SaveTours", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Tours", "TotalTours", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("UPS", "UPS", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales]=0,0,[SalesAmount]/[Sales])");
      lst.Add("Month", "Month", isVisible: false);
      lst.Add("Market Total", "MarketTotal", isVisible: false);
      return lst;
    }

    #endregion rptProductionByLeadSourceMarketMonthly

    #region rptProductionReferral

    /// <summary>
    /// Formato para el reporte Production Referral
    /// </summary>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptProductionReferral()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Year", "Year", isGroup: true, isVisible: false);
      lst.Add("Month", "MonthN");
      lst.Add("Arrivals", "Arrivals", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Shows %", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals]=0,0,[Shows]/[Arrivals])");
      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Vol", "SalesAmount", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Closing %", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows]=0,0,[Sales]/[Shows])");
      lst.Add("Efficiency", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows]=0,0,[SalesAmount]/[Shows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales]=0,0,[SalesAmount]/[Sales])");
      lst.Add("MonthID", "Month", isVisible: false);
      return lst;
    }

    #endregion rptProductionReferral

    #region RptReps

    /// <summary>
    /// Formato para el reporte Reps
    /// </summary>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptReps()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Rep ID", "rpID");
      lst.Add("Active", "rpA", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Center);
      return lst;
    }

    #endregion RptReps

    #region RptSalesByProgramLeadSourceMarket

    /// <summary>
    /// Formato para el reporte Sales By Program, Leadsource & Market
    /// </summary>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptSalesByProgramLeadSourceMarket()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Program", "Program", isGroup: true, isVisible: false);
      lst.Add("Lead Source", "LeadSource", isGroup: true, isVisible: false);
      lst.Add("Market", "Market", isGroup: true, isVisible: false);
      lst.Add("GUID", "GuestID");
      lst.Add("Last Name", "LastName");
      lst.Add("FirstName", "FirstName");
      lst.Add("Membership", "Membership");
      lst.Add("Date", "SaleDate", format: EnumFormatTypeExcel.Date);
      lst.Add("Proc", "Procesable", function: DataFieldFunctions.Count);
      lst.Add("Proc Date", "SaleProcesableDate", format: EnumFormatTypeExcel.Date);
      lst.Add("Cancel", "Cancel", function: DataFieldFunctions.Count);
      lst.Add("Cancel Date", "CancelDate", format: EnumFormatTypeExcel.Date);
      return lst;
    }

    #endregion RptSalesByProgramLeadSourceMarket

    #region RptWarehouseMovements

    /// <summary>
    /// Formato para el reporte Warehouse Movements
    /// </summary>
    /// <history>
    /// [edgrodriguez] 26/Abr/2016 Created
    /// </history>
    public static ColumnFormatList RptWarehouseMovements()
    {
      ColumnFormatList lst = new ColumnFormatList();

      lst.Add("Date", "wmD", format: EnumFormatTypeExcel.Date);
      lst.Add("Qty", "wmQty", format: EnumFormatTypeExcel.Number);
      lst.Add("Gift", "giN");
      lst.Add("Code", "wmpe");
      lst.Add("User", "peN");
      lst.Add("Comments", "wmComments");

      return lst;
    }

    #endregion RptWarehouseMovements

    #endregion General Reports
  }
}
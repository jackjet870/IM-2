using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
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
    public static List<ExcelFormatTable> RptBookingsBySalesRoomProgramTime()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Sales Room", Axis = ePivotFieldAxis.Row, Order = 1, Sort = eSortType.Ascending},
new ExcelFormatTable() { Title = "Program", Axis = ePivotFieldAxis.Row, Order = 2, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Book Type", Axis = ePivotFieldAxis.Row, Order = 3, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title="Time", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1 }
      };
    }

    #endregion RptBookingsBySalesRoomProgramTime

    #region RptBookingsBySalesRoomProgramLeadSourceTime

    /// <summary>
    /// Formato para el reporte Bookings By Sales Room, Program, Lead Sources & Time
    /// </summary>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptBookingsBySalesRoomProgramLeadSourceTime()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "Sales Room", Axis = ePivotFieldAxis.Row, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Program", Axis = ePivotFieldAxis.Row, Order = 2, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Lead Source", Axis = ePivotFieldAxis.Row, Order = 3, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Book Type", Axis = ePivotFieldAxis.Row, Order = 4, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Time", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1 }
      };
    }

    #endregion RptBookingsBySalesRoomProgramLeadSourceTime

    #endregion Bookings

    #region CxC

    #region RptCxC

    /// <summary>
    /// Formato para el reporte CxC
    /// </summary>
    /// <history>
    /// [edgrodriguez] 17/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptCxc()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable { Title = "grpe", PropertyName = "grpe", IsGroup = true, Order = 1 },
new ExcelFormatTable { Title = "Receipt Date", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Order = 1 },
new ExcelFormatTable { Title = "Receipt ID", PropertyName = "grID", Order = 2 },
new ExcelFormatTable { Title = "Folio", PropertyName = "grNum", Order = 3 },
new ExcelFormatTable { Title = "SR", PropertyName = "grsr", Order = 4 },
new ExcelFormatTable { Title = "Lead Source", PropertyName = "grls", Order = 5 },
new ExcelFormatTable { Title = "PR", PropertyName = "peID", Order = 6 },
new ExcelFormatTable { Title = "PR Name", PropertyName = "peN", Order = 7 },
new ExcelFormatTable { Title = "Description", PropertyName = "Comments", Order = 8 },
new ExcelFormatTable { Title = "CxC", PropertyName = "CxC", Format = EnumFormatTypeExcel.Currency, Order = 9, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable { Title = "Exchange Rate", PropertyName = "exExchRate", Format = EnumFormatTypeExcel.Currency, Order = 10, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable { Title = "CxC Pesos", PropertyName = "CxCP", Format = EnumFormatTypeExcel.Currency, Order = 11, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable { Title = "Total CxC", PropertyName = "TotalCxC", Format = EnumFormatTypeExcel.Currency, Order = 12, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable { Title = "Amount to Pay (USD)", PropertyName = "ToPayUS", Format = EnumFormatTypeExcel.Currency, Order = 13, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable { Title = "Amount To Pay (MXN)", PropertyName = "ToPayMN", Format = EnumFormatTypeExcel.Currency, Order = 14, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable { Title = "Amount Paid (USD)", PropertyName = "PaidUS", Format = EnumFormatTypeExcel.Currency, Order = 15, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable { Title = "Amount Paid (MXN)", PropertyName = "PaidMN", Format = EnumFormatTypeExcel.Currency, Order = 16, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable { Title = "Balance (USD)", PropertyName = "BalanceUS", Format = EnumFormatTypeExcel.Currency, Order = 17, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable { Title = "Balance (MXN)", PropertyName = "BalanceMN", Format = EnumFormatTypeExcel.Currency, Order = 18, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable { Title = "CxC Comments", PropertyName = "GiftComments", Order = 19 },
new ExcelFormatTable { Title = "Guest ID", PropertyName = "guID", Order = 20 },
new ExcelFormatTable { Title = "Reservation", PropertyName = "guHReservID", Order = 21 },
new ExcelFormatTable { Title = "Out Invitation", PropertyName = "guOutInvitNum", Order = 22 },
new ExcelFormatTable { Title = "Authorized", PropertyName = "AuthSts", Order = 23 },
new ExcelFormatTable { Title = "Authorization Date", PropertyName = "grCxCAppD", Format = EnumFormatTypeExcel.Date, Order = 24 },
new ExcelFormatTable { Title = "Authorized By", PropertyName = "grAuthorizedBy", Order = 25 },
new ExcelFormatTable { Title = "Authorized Name", PropertyName = "AuthName", Order = 26 },
new ExcelFormatTable { Title = "CxC Authorization Comments", PropertyName = "CxCComments", Order = 27 }
      };
    }

    #endregion

    #region RptCxCByType

    /// <summary>
    /// Formato para el reporte CxC
    /// </summary>
    /// <history>
    /// [edgrodriguez] 22/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptCxcByType()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Group", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "PR", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Chb", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "PP", Format=EnumFormatTypeExcel.Id, Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Rcpt", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Date", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Guest ID", Format = EnumFormatTypeExcel.Id, Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Guest Name", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Qty", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "Gift", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Ad", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Min", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Folios", Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Total Gifts", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "CxC Gift", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 2 },
new ExcelFormatTable() { Title = "CxC Adj", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 3 },
new ExcelFormatTable() { Title = "CxC Deposit", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 4 },
new ExcelFormatTable() { Title = "Currency Deposit", Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() { Title = "Ex. Rate Deposit", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 5 },
new ExcelFormatTable() { Title = "Deposit US", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6 },
new ExcelFormatTable() { Title = "Deposit MN", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 7 },
new ExcelFormatTable() { Title = "CxC Taxi Out", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8 },
new ExcelFormatTable() { Title = "Currency Taxi Out", Axis = ePivotFieldAxis.Row, Order = 15 },
new ExcelFormatTable() { Title = "Ex. Rate Taxi Out", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 9 },
new ExcelFormatTable() { Title = "Taxi Out US", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10 },
new ExcelFormatTable() { Title = "Taxi Out MN", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 11 },
new ExcelFormatTable() { Title = "Total CxC", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12 },
new ExcelFormatTable() { Title = "CxC Paid US", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 13 },
new ExcelFormatTable() { Title = "CxC Paid MN", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14 },
new ExcelFormatTable() { Title = "Ex. Rate", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 15 },
new ExcelFormatTable() { Title = "CxC Comments", Axis = ePivotFieldAxis.Row, Order = 16 },
new ExcelFormatTable() { Title = "Receipt Comments", Axis = ePivotFieldAxis.Row, Order = 17 }
      };
    }

    #endregion RptCxC

    #region RptCxcDeposits

    /// <summary>
    /// Formato para el reporte CxC Deposits
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptCxcDeposits()
    {
      return new List<ExcelFormatTable>
      {
new ExcelFormatTable() { Title = "Ch B", Format = EnumFormatTypeExcel.Id, Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Chb PP", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Date", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "LS", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "GUID", Format = EnumFormatTypeExcel.Id, Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Guest Name", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Hotel", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "PR", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "PR Name", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "Host", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Host Name", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "CxC", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "CxC Currency", Axis = ePivotFieldAxis.Row, Order = 12},
new ExcelFormatTable() { Title = "Currency", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending }
      };
    }

    #endregion RptCxcDeposits

    #region RptCxcGifts

    /// <summary>
    /// Formato para el reporte CxCGifts
    /// </summary>
    /// <history>
    /// [edgrodriguez] 29/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptCxcGifts()
    {
      return new List<ExcelFormatTable>{
new ExcelFormatTable() {  Title="CHB", PropertyName = "grID", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() {  Title="Chb PP",PropertyName = "grNum", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() {  Title="PR",PropertyName = "grpe", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() {  Title="PR Name",PropertyName = "peN", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() {  Title="Location",PropertyName = "grlo", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() {  Title="Host",PropertyName = "grHost", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() {  Title="Host Name" ,PropertyName = "HostN", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() {  Title="GUID" ,PropertyName = "grgu", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() {  Title="Guest Name" ,PropertyName = "grGuest", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() {  Title="Gift",PropertyName = "Gift", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() {  Title="Gift Name",PropertyName = "giN", Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() {  Title="Quantity" ,PropertyName = "Quantity", Format = EnumFormatTypeExcel.Number,Function = DataFieldFunctions.Sum, Axis = ePivotFieldAxis.Values, Order = 10 },
new ExcelFormatTable() {  Title="Adults" ,PropertyName = "Adults", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() {  Title="Minors" ,PropertyName = "Minors", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() {  Title="Folios" ,PropertyName = "Folios", Axis = ePivotFieldAxis.Values, Order = 12 },
new ExcelFormatTable() {  Title="Cost" ,PropertyName = "Cost", Format = EnumFormatTypeExcel.Currency,Function= DataFieldFunctions.Sum, Axis = ePivotFieldAxis.Values, Order = 11 },
new ExcelFormatTable() {  Title="Rcpt Date" ,PropertyName = "grD", Format=EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 15},
new ExcelFormatTable() {  Title="USD" ,PropertyName = "CostUS", Format = EnumFormatTypeExcel.Currency,Function = DataFieldFunctions.Sum, Axis = ePivotFieldAxis.Row, Order = 16 },
new ExcelFormatTable() {  Title="Exch. Rate" ,PropertyName = "exExchRate", Axis = ePivotFieldAxis.Row, Order = 17 },
new ExcelFormatTable() {  Title="MX" ,PropertyName = "CostMX", Format=EnumFormatTypeExcel.Currency,Function = DataFieldFunctions.Sum, Axis = ePivotFieldAxis.Row, Order = 18 },
new ExcelFormatTable() {  Title="Member #" ,PropertyName = "grMemberNum", Axis = ePivotFieldAxis.Row, Order = 19 },
new ExcelFormatTable() {  Title="Comments"  ,PropertyName = "grCxCComments", Axis = ePivotFieldAxis.Row, Order = 20 }
      };
    }

    #endregion RptCxcGifts

    #region RptCxcNotAuthorized

    /// <summary>
    /// Formato para el reporte CxC
    /// </summary>
    /// <history>
    /// [edgrodriguez] 01/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptCxcNotAuthorized()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Rcpt", PropertyName = "grID", Order = 1 },
new ExcelFormatTable() { Title = "Chb PP", PropertyName = "grNum", Order = 2 },
new ExcelFormatTable() { Title = "LS", PropertyName = "grls", Order = 3 },
new ExcelFormatTable() { Title = "GUID", PropertyName = "grgu", Order = 4 },
new ExcelFormatTable() { Title = "Guest Name", PropertyName = "grGuest", Order = 5 },
new ExcelFormatTable() { Title = "Rcpt Date", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Order = 6 },
new ExcelFormatTable() { Title = "PR", PropertyName = "grpe", Order = 7 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN", Order = 8 },
new ExcelFormatTable() { Title = "CxC Gifts", PropertyName = "grCxCGifts", Format = EnumFormatTypeExcel.DecimalNumber, Order = 9 },
new ExcelFormatTable() { Title = "CxC Dep", PropertyName = "grCxCPRDeposit", Format = EnumFormatTypeExcel.Number, Order = 10 },
new ExcelFormatTable() { Title = "CxC Taxi", PropertyName = "grCxCTaxiOut", Format = EnumFormatTypeExcel.Number, Order = 11 },
new ExcelFormatTable() { Title = "CxC", PropertyName = "CxC", Format = EnumFormatTypeExcel.Number, Order = 12 }
      };
    }

    #endregion RptCxcNotAuthorized

    #region RptCxcPayments

    /// <summary>
    /// Formato para el reporte CxC Payments
    /// </summary>
    /// <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptCxcPayments()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "LS", PropertyName = "grls", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "Rcpt", PropertyName = "grID", Order = 1 },
new ExcelFormatTable() { Title = "Rcpt D", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Order = 2 },
new ExcelFormatTable() { Title = "PR", PropertyName = "grpe", Order = 3 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN", Order = 4 },
new ExcelFormatTable() { Title = "Pay USD", PropertyName = "AmountToPayUSD", Format = EnumFormatTypeExcel.Currency, Order = 5 },
new ExcelFormatTable() { Title = "Pay MXN", PropertyName = "AmountToPayMXN", Format = EnumFormatTypeExcel.Currency, Order = 6 },
new ExcelFormatTable() { Title = "Paid USD", PropertyName = "AmountPaidUSD", Format = EnumFormatTypeExcel.Currency, Order = 7 },
new ExcelFormatTable() { Title = "Paid MXN", PropertyName = "AmountPaidMXN", Format = EnumFormatTypeExcel.Currency, Order = 8 },
new ExcelFormatTable() { Title = "Bal USD", PropertyName = "BalanceUSD", Format = EnumFormatTypeExcel.Currency, Order = 9 },
new ExcelFormatTable() { Title = "Bal MXN", PropertyName = "BalanceMXN", Format = EnumFormatTypeExcel.Currency, Order = 10 },
new ExcelFormatTable() { Title = "GUID", PropertyName = "grgu", Order = 11 },
new ExcelFormatTable() { Title = "Reserv", PropertyName = "guHReservID", Order = 12 },
new ExcelFormatTable() { Title = "Out Inv", PropertyName = "guOutInvitNum", Order = 13 },
new ExcelFormatTable() { Title = "Payment Date", PropertyName = "cxD", Format = EnumFormatTypeExcel.Date, Order = 14},
new ExcelFormatTable() { Title = "Rec By", PropertyName = "cxReceivedBy", Format = EnumFormatTypeExcel.Number, Order = 15 },
new ExcelFormatTable() { Title = "Rec Name", PropertyName = "ReceivedByName", Order = 16 },
new ExcelFormatTable() { Title = "Amt USD", PropertyName = "AmountUSD", Format = EnumFormatTypeExcel.Currency, Order = 17 },
new ExcelFormatTable() { Title = "Amt MXN", PropertyName = "AmountMXN", Format = EnumFormatTypeExcel.Currency, Order = 18 }
      };
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
    public static List<ExcelFormatTable> RptDeposits()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Ch B", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Chb PP", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Out. Inv.", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "GUID", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Guest", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Hotel", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "LS", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "SR", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "PR" , Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "PR Name", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Date", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Book Date", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Show", Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Currency", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Payment Type", Axis = ePivotFieldAxis.Column, Order = 2, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Deposited", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Received", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Burned", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "CxC", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 4, Function = DataFieldFunctions.Sum }
      };
    }

    #endregion RptDeposits

    #region RptBurnedDeposits

    /// <summary>
    /// Formato para el reporte Burned Deposits
    /// </summary>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptBurnedDeposits()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Ch B", PropertyName = "grID", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Chb PP", PropertyName = "grNum", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Date", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Guest ID", PropertyName = "grgu", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "grGuest", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "grHotel", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Loc", PropertyName = "grlo", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "SR", PropertyName = "grsr", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "PR", PropertyName = "grpe", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Host", PropertyName = "grHost", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "grComments", Axis = ePivotFieldAxis.Row, Order = 16 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "cuN", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "ptN", Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "Member #", PropertyName = "memberNum", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Processable", PropertyName = "procAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 13, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Pending", PropertyName = "pendAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 14, Function = DataFieldFunctions.Sum  }
      };
    }

    #endregion RptBurnedDeposits

    #region RptBurnedDepositsByResorts

    /// <summary>
    /// Formato para el reporte Burned Deposits by Resorts
    /// </summary>
    /// <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptBurnedDepositsByResorts()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Ch B", PropertyName = "grID", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Chb PP", PropertyName = "grNum", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Date", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Guest ID", PropertyName = "grgu", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "grGuest", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "grHotel", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Resort", PropertyName = "guHotelB", Axis = ePivotFieldAxis.Row, Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "Loc", PropertyName = "grlo", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "SR", PropertyName = "grsr", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "PR", PropertyName = "grpe", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Host", PropertyName = "grHost", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "cuN", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "ptN", Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "Member #", PropertyName = "memberNum", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Processable", PropertyName = "procAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 13, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Pending", PropertyName = "pendAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 14, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum  }
      };
    }

    #endregion RptBurnedDepositsByResorts

    #region RptPaidDeposits

    /// <summary>
    /// Formato para el reporte Paid Deposits
    /// </summary>
    /// <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptPaidDeposits()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Ch B", PropertyName = "grID", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Chb PP", PropertyName = "grNum", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Date", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Guest ID", PropertyName = "grgu", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Book D", PropertyName = "guBookD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Guest Name", PropertyName = "grGuest", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "grHotel", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "LS", PropertyName = "grls", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "SR", PropertyName = "grsr", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "PR", PropertyName = "grpe", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Host", PropertyName = "grHost", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "cuN", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "ptN", Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() { Title = "Deposit", PropertyName = "grDeposit", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum }
      };
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
    public static List<ExcelFormatTable> RptCancelledGiftsManifest()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "GUID", PropertyName = "grgu", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Guest Name", PropertyName = "grGuest", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "SR", PropertyName = "grsr", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Loc", PropertyName = "grlo", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Host", PropertyName = "grHost", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Host Name", PropertyName = "peN", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Chb", PropertyName = "grID", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Date", PropertyName = "grD", Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.Date, Order = 8 },
new ExcelFormatTable() { Title = "Deposit", PropertyName = "grDeposit", Format = EnumFormatTypeExcel.DecimalNumberWithCero, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 9 },
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.DecimalNumberWithCero, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order =  10 },
new ExcelFormatTable() { Title = "grcu", PropertyName = "grcu"},
new ExcelFormatTable() { Title = "grpt", PropertyName = "grpt"},
new ExcelFormatTable() { Title = "Taxi", PropertyName = "grTaxiOut", Format = EnumFormatTypeExcel.DecimalNumberWithCero, Order = 11 },
new ExcelFormatTable() { Title = "Quantity", PropertyName = "geQty", Format = EnumFormatTypeExcel.Number, Order = 12 },
new ExcelFormatTable() { Title = "Folios", PropertyName = "geFolios", Order = 14},
new ExcelFormatTable() { Title = "Cost", PropertyName = "Cost", Format = EnumFormatTypeExcel.DecimalNumberWithCero, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 13 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "grComments", Axis = ePivotFieldAxis.Row, Order = 15 },
new ExcelFormatTable() { Title = "Gift", PropertyName = "Gift" },
new ExcelFormatTable() { Title = "GiftN", PropertyName = "GiftN" }
      };
    }

    #endregion RptDailyGiftSimple

    #region RptDailyGiftSimple

    /// <summary>
    /// Formato para el reporte Daily Gift Simple
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptDailyGiftSimple()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Canceled", PropertyName = "grCancel"},
new ExcelFormatTable() { Title = "Ch B", PropertyName = "grID", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Gift Name", PropertyName = "giN", Axis = ePivotFieldAxis.Column, Order = 2, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Gift", PropertyName = "giShortN", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Quantity", PropertyName = "geQty", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "Location", PropertyName = "grlo", Axis = ePivotFieldAxis.Row, Order = 1, Sort = eSortType.Ascending },
      };
    }

    #endregion RptDailyGiftSimple

    #region RptGiftsByCategory

    /// <summary>
    /// Formato para el reporte Gifts By Category
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGiftsByCategory()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Gift", PropertyName = "Gift", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Total Quantity", PropertyName = "TotalQty", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 2, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Unit Cost", PropertyName = "UnitCost", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 3, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Total Cost", PropertyName = "TotalCost",Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 4, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Quantity", PropertyName = "Quantity", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Day", PropertyName = "Day", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "Category", PropertyName = "Category",Axis = ePivotFieldAxis.Row, IsGroup = true, Order = 5 }
      };
    }

    #endregion RptGiftsByCategory

    #region RptGiftsByCategoryProgram

    /// <summary>
    /// Formato para el reporte Gifts By Category
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGiftsByCategoryProgram()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Gift", PropertyName = "Gift", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Total Quantity", PropertyName = "TotalQty", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 2, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Unit Cost", PropertyName = "UnitCost", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 3, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Total Cost", PropertyName = "TotalCost",Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 4, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Quantity", PropertyName = "Quantity", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Day", PropertyName = "Day", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending},
new ExcelFormatTable() { Title = "Program", PropertyName = "Program",Axis = ePivotFieldAxis.Row, IsGroup = true, Order = 1 },
new ExcelFormatTable() { Title = "Category", PropertyName = "Category",Axis = ePivotFieldAxis.Row, IsGroup = true, Order = 2 }
      };
    }

    #endregion RptGiftsByCategoryProgram

    #region RptGiftsCertificates

    /// <summary>
    /// Formato para el reporte Gifts Certificates
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGiftsCertificates()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Gift ID", PropertyName = "GiftID", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Gift Name", PropertyName = "GiftN", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Receipt", PropertyName = "Receipt", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Status", PropertyName = "Status", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "Folios", PropertyName = "Folios", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Date", PropertyName = "Date", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Host", PropertyName = "Host", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Guest", PropertyName = "Guest", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Location", PropertyName = "Location", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Adults", PropertyName = "Adults", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 9, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Minors", PropertyName = "Minors", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 10, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Extra Adults", PropertyName = "ExtraAdults", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 11, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Pax", PropertyName = "Pax", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "CxC", PropertyName = "CxC", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 13, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "PaymentType", Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "Currency", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "Paid", PropertyName = "Paid", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 14, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Refund", PropertyName = "Refund", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 15, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Comments", PropertyName = "Comments", Axis = ePivotFieldAxis.Row, Order = 16 }
      };
    }

    #endregion RptGiftsCertificates

    #region RptGiftsManifest

    /// <summary>
    /// Formato para el reporte Gifts Manifest
    /// </summary>
    /// <history>
    /// [edgrodriguez] 09/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGiftsManifest()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Guest ID", PropertyName = "GuestID", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Reserv ID", PropertyName = "ReservID", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Date", PropertyName = "Date", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Guest", PropertyName = "Guest", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Guest 2", PropertyName = "Guest2", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Pax", PropertyName = "Pax", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Agency", PropertyName = "Agency", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Membership", PropertyName = "Membership", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Location", PropertyName = "Location", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "PR", PropertyName = "PR", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "PRN", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Host", PropertyName = "Host", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Host Name", PropertyName = "HostN", Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Receipt ID", PropertyName = "ReceiptID", Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() { Title = "Show", PropertyName = "Show", Axis = ePivotFieldAxis.Row, Order = 15 },
new ExcelFormatTable() { Title = "Hotel Burned", PropertyName = "HotelBurned", Axis = ePivotFieldAxis.Row, Order = 16 },
new ExcelFormatTable() { Title = "Cancel", PropertyName = "Cancel", Axis = ePivotFieldAxis.Row, Order = 17 },
new ExcelFormatTable() { Title = "Cancel Date", PropertyName = "CancelledDate", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 18 },

new ExcelFormatTable() { Title = "Deposited", PropertyName = "Deposited", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum, Order = 19 },
new ExcelFormatTable() { Title = "Burned", PropertyName = "Burned", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum, IsVisible = false },
new ExcelFormatTable() { Title = "Currency", PropertyName = "Currency"  },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "PaymentType" },
new ExcelFormatTable() { Title = "Taxi Out", PropertyName = "TaxiOut", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Function = DataFieldFunctions.Sum, Order = 20 },
new ExcelFormatTable() { Title = "Taxi Out Diff", PropertyName = "TaxiOutDiff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Function = DataFieldFunctions.Sum, Order = 21 },
new ExcelFormatTable() { Title = "Gift", PropertyName = "Gift" },
new ExcelFormatTable() { Title = "GiftN", PropertyName = "GiftN" },
new ExcelFormatTable() { Title = "Quantity", PropertyName = "Quantity", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum, Order = 22 },
new ExcelFormatTable() { Title = "Adults", PropertyName = "Adults", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum, IsVisible = false },
new ExcelFormatTable() { Title = "Minors", PropertyName = "Minors", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum, IsVisible = false },
new ExcelFormatTable() { Title = "Folios", PropertyName = "Folios", Order = 24 },
new ExcelFormatTable() { Title = "Cost", PropertyName = "Cost", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, Order = 23 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "Comments", Axis = ePivotFieldAxis.Row, Order = 25 }
      };
    }

    #endregion

    #region RptGiftsReceipts

    /// <summary>
    /// Formato para el reporte Gifts Manifest
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGiftsReceipts()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Guest ID", PropertyName = "GuestID", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Reserv ID", PropertyName = "ReservID", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Date", PropertyName = "Date", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Guest", PropertyName = "Guest", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Guest 2", PropertyName = "Guest2", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Pax", PropertyName = "Pax", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Agency", PropertyName = "Agency", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Membership", PropertyName = "Membership", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Location", PropertyName = "Location", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "PR", PropertyName = "PR", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Host", PropertyName = "Host", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Host Name", PropertyName = "HostN", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Receipt ID", PropertyName = "ReceiptID", Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Show", PropertyName = "Show", Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() { Title = "Hotel Burned", PropertyName = "HotelBurned", Axis = ePivotFieldAxis.Row, Order = 15 },
new ExcelFormatTable() { Title = "Cancel", PropertyName = "Cancel", Axis = ePivotFieldAxis.Row, Order = 16 },
new ExcelFormatTable() { Title = "Cancel Date", PropertyName = "CancelledDate", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 17 },

new ExcelFormatTable() { Title = "With Cost", PropertyName = "WithCost", Format = EnumFormatTypeExcel.Boolean, Order = 24 },
new ExcelFormatTable() { Title = "Public Price", PropertyName = "PublicPrice", Format = EnumFormatTypeExcel.Currency, Order = 25 },
new ExcelFormatTable() { Title = "Deposited", PropertyName = "Deposited", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum, Order = 18 },
new ExcelFormatTable() { Title = "Burned", PropertyName = "Burned", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum, Order = 19 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "Currency"  },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "PaymentType" },
new ExcelFormatTable() { Title = "Taxi Out", PropertyName = "TaxiOut", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Function = DataFieldFunctions.Sum, Order = 20, },
new ExcelFormatTable() { Title = "Taxi Out Diff", PropertyName = "TaxiOutDiff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Function = DataFieldFunctions.Sum, Order = 21 },
new ExcelFormatTable() { Title = "Gift", PropertyName = "Gift" },
new ExcelFormatTable() { Title = "GiftN", PropertyName = "GiftN" },
new ExcelFormatTable() { Title = "Quantity", PropertyName = "Quantity", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum, Order = 22 },
new ExcelFormatTable() { Title = "Adults", PropertyName = "Adults", IsVisible = false },
new ExcelFormatTable() { Title = "Minors", PropertyName = "Minors", IsVisible = false },
new ExcelFormatTable() { Title = "Folios", PropertyName = "Folios", Order = 26 },
new ExcelFormatTable() { Title = "Cost", PropertyName = "Cost", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, Order = 23 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "Comments", Axis = ePivotFieldAxis.Row, Order = 27 }
      };
    }

    #endregion

    #region RptGiftsReceiptsPayments

    /// <summary>
    /// Formato para el reporte Gifts Receipts Payments
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGiftsReceiptsPayments()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Source", PropertyName = "Source", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Amount", PropertyName = "Amount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 1, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Amount US", PropertyName = "AmountUS", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 2, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "PaymentType", PropertyName = "PaymentType", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending},
new ExcelFormatTable() { Title = "Currency", PropertyName = "Currency", Axis = ePivotFieldAxis.Column, Order = 2, Sort = eSortType.Ascending},
new ExcelFormatTable() { Title = "GroupSource1", PropertyName = "GroupSource1", Axis = ePivotFieldAxis.Row, IsGroup = true, Order = 1 },
new ExcelFormatTable() { Title = "GroupSource2", PropertyName = "GroupSource2", Axis = ePivotFieldAxis.Row, IsGroup= true, Order = 2 }
      };
    }

    #endregion RptGiftsReceiptsPayments

    #region RptGiftsSale

    /// <summary>
    /// Formato para el reporte Gifts Sale
    /// </summary>
    /// <history>
    /// [edgrodriguez] 28/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGiftsSale()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Program", PropertyName = "Program", IsGroup = true, Order = 1 },
new ExcelFormatTable() { Title = "Receipt", PropertyName = "Receipt", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Date", PropertyName = "Date", IsGroup = true, Order = 2 },
new ExcelFormatTable() { Title = "Cancel", PropertyName = "Cancel", Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Count, Order = 2 },
new ExcelFormatTable() { Title = "Cancel Date", PropertyName = "CancelDate", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoom", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Lead Source", PropertyName = "LeadSource", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "PR ID", PropertyName = "PR", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "PRN", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Out Invit", PropertyName = "OutInvit", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "GUID", PropertyName = "GuestID", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "LastName", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "FirstName", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Gift ID", PropertyName = "Gift", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Gift Name", PropertyName = "GiftN", Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Gift Sale", PropertyName = "GiftSale", Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() { Title = "Adults", PropertyName = "Adults", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Number, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 15 },
new ExcelFormatTable() { Title = "Minors", PropertyName = "Minors", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Number, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 16 },
new ExcelFormatTable() { Title = "Extra Adults", PropertyName = "ExtraAdults", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Number, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 17 },
new ExcelFormatTable() { Title = "Price US", PropertyName = "PriceUS", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.DecimalNumber, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 18 },
new ExcelFormatTable() { Title = "Price MX", PropertyName = "PriceMX", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Number, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 19 },
new ExcelFormatTable() { Title = "Price CAN", PropertyName = "PriceCAN", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Number, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 20 },
new ExcelFormatTable() { Title = "Total To Pay", PropertyName = "TotalToPay", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.DecimalNumber, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 22 },
new ExcelFormatTable() { Title = "Payment Total", PropertyName = "PaymentTotal", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.DecimalNumber, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 23 },
new ExcelFormatTable() { Title = "Difference", PropertyName = "Difference", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.DecimalNumber, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 24 },
new ExcelFormatTable() { Title = "User ID", PropertyName = "User", Axis = ePivotFieldAxis.Row, Order = 25 },
new ExcelFormatTable() { Title = "User Name", PropertyName = "UserN", Axis = ePivotFieldAxis.Row, Order = 26 },
new ExcelFormatTable() { Title = "Amount", PropertyName = "Amount", Axis = ePivotFieldAxis.Values, Format =EnumFormatTypeExcel.DecimalNumber, AggregateFunction = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 21 },
new ExcelFormatTable() { Title = "Source", PropertyName = "Source", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "Currency", Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() { Title = "PaymentType", PropertyName = "PaymentType", Axis = ePivotFieldAxis.Column, Order = 3 }
      };
    }

    #endregion RptGiftsReceiptsPayments

    #region RptGiftsUsedBySistur

    /// <summary>
    /// Formato para el reporte Gifts Used by Sistur
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGiftsUsedBySistur()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Sales Room", PropertyName = "srN", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "Program", PropertyName = "pgN", Order = 2, IsGroup = true },
new ExcelFormatTable() { Title = "Lead Source", PropertyName = "lsN",  Order = 3, IsGroup = true },
new ExcelFormatTable() { Title = "Rcpt", PropertyName = "grID", Order = 1 },
new ExcelFormatTable() { Title = "Rcpt Date", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Order = 2 },
new ExcelFormatTable() { Title = "Cancel", PropertyName = "grCancel", Order = 3, SubTotalFunctions = eSubTotalFunctions.Count },
new ExcelFormatTable() { Title = "Cancel Date", PropertyName = "grCancelD", Format = EnumFormatTypeExcel.Date, Order = 4 },
new ExcelFormatTable() { Title = "Exch", PropertyName = "grExchange", Order = 5, SubTotalFunctions = eSubTotalFunctions.Count },
new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Order = 6 },
new ExcelFormatTable() { Title = "Reserv #", PropertyName = "guHReservID", Order = 7 },
new ExcelFormatTable() { Title = "Out Inv", PropertyName = "guOutInvitNum", Order = 8 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "guLastName1", Order = 9 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "guFirstName1", Order = 10 },
new ExcelFormatTable() { Title = "Qty", PropertyName = "geQty", Format = EnumFormatTypeExcel.Number, Order = 11, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Unit", PropertyName = "giQty", Format = EnumFormatTypeExcel.Number, Order = 12 },
new ExcelFormatTable() { Title = "Gift ID", PropertyName = "giID", Order = 13 },
new ExcelFormatTable() { Title = "Gift Name", PropertyName = "giN", Order = 14 },
new ExcelFormatTable() { Title = "Sistur Promotion", PropertyName = "Promotion", Order = 15 },
new ExcelFormatTable() { Title = "Granted", PropertyName = "Granted", Format = EnumFormatTypeExcel.Number, Order = 16, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Sistur Cancel", PropertyName = "geCancelPVPPromo", Order = 17, SubTotalFunctions = eSubTotalFunctions.Count },
new ExcelFormatTable() { Title = "Used", PropertyName = "gspUsed", Format = EnumFormatTypeExcel.Number, Order = 18, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Sistur Coupon", PropertyName = "gspCoupon", Order = 19 },
new ExcelFormatTable() { Title = "Coupon Date", PropertyName = "gspD", Format = EnumFormatTypeExcel.Date, Order = 20 },
new ExcelFormatTable() { Title = "Pax", PropertyName = "gspPax", Format = EnumFormatTypeExcel.Number, Order = 21 },
new ExcelFormatTable() { Title = "Sistur Price", PropertyName = "gspAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 22, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Cost", PropertyName = "Cost", Format = EnumFormatTypeExcel.DecimalNumber, Order = 23, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Difference", PropertyName = "Difference", Format = EnumFormatTypeExcel.DecimalNumber, Order = 24, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "CeCos", PropertyName = "Cecos", Order = 25 }
      };
    }

    #endregion RptGiftsUsedBySistur

    #region RptWeeklyGiftSimple

    /// <summary>
    /// Formato para el reporte Daily Gift Simple
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptWeeklyGiftSimple()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Date", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row },
new ExcelFormatTable() { Title = "Gift Name", PropertyName = "Gift", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Gift", PropertyName = "ShortN", Axis = ePivotFieldAxis.Column, Order = 2, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Quantity", PropertyName = "Qty", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1 },
      };
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
    public static List<ExcelFormatTable> RptGuestCeco()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Society", PropertyName = "soccecoid", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Book Date", PropertyName = "guBookD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Sales Room", PropertyName = "srN", Axis = ePivotFieldAxis.Row, Order = 1, Outline = true },
new ExcelFormatTable() { Title = "Activity", PropertyName = "acn", Axis = ePivotFieldAxis.Row, Order = 2, Outline = true },
new ExcelFormatTable() { Title = "CECO", PropertyName = "ceco", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Market Segment", PropertyName = "mksN", Axis = ePivotFieldAxis.Row, Order = 3, Outline = true }
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
    public static List<ExcelFormatTable> RptGuestNoBuyers()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Program", PropertyName = "Program", Axis = ePivotFieldAxis.Row, Order = 1, Compact = true, Outline = true  },
new ExcelFormatTable() { Title = "LeadSource", PropertyName = "LeadSource", Axis = ePivotFieldAxis.Row, Order = 2, Compact = true, Outline = true  },
new ExcelFormatTable() { Title = "GUID", PropertyName = "GuestID", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "LastName", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "FirstName", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Email", PropertyName = "Email", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Email 2", PropertyName = "Email2", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "City", PropertyName = "City", Axis = ePivotFieldAxis.Row, Order = 8},
new ExcelFormatTable() { Title = "State", PropertyName = "State", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "Country ID", PropertyName = "Country" },
new ExcelFormatTable() { Title = "Country", PropertyName = "CountryN", Axis = ePivotFieldAxis.Row, Order = 10 }
      };
    }

    #endregion RptGuestNoBuyers

    #region RptInOut

    /// <summary>
    /// Formato para el reporte In & Out
    /// </summary>
    /// <history>
    /// [edgrodriguez] 09/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptInOut()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoom", Axis = ePivotFieldAxis.Row, Order = 1, IsGroup = true  },
new ExcelFormatTable() { Title = "Location", PropertyName = "Location", Axis = ePivotFieldAxis.Row, Order = 2, IsGroup = true  },
new ExcelFormatTable() { Title = "GUID", PropertyName = "GUID", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "Hotel", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Room", PropertyName = "Room", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Pax", PropertyName = "Pax", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "LastName", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "FirstName", Axis = ePivotFieldAxis.Row, Order = 6},
new ExcelFormatTable() { Title = "Agency ID", PropertyName = "Agency", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Agency", PropertyName = "AgencyN", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Country ID", PropertyName = "Country", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "Country", PropertyName = "CountryN", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Show D", PropertyName = "ShowDate", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Time In", PropertyName = "TimeIn", Format = EnumFormatTypeExcel.Time, Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Time Out", PropertyName = "TimeOut", Format = EnumFormatTypeExcel.Time, Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Direct", PropertyName = "Direct", Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 14 },
new ExcelFormatTable() { Title = "Tour", PropertyName = "Tour", Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Count, Order = 15 },
new ExcelFormatTable() { Title = "In Out", PropertyName = "InOut", Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Count, Order = 16 },
new ExcelFormatTable() { Title = "Walk Out", PropertyName = "WalkOut", Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Count, Order = 17 },
new ExcelFormatTable() { Title = "Courtesy Tour", PropertyName = "CourtesyTour", Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Count, Order = 18 },
new ExcelFormatTable() { Title = "Save Program", PropertyName = "SaveProgram", Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Count, Order = 19 },
new ExcelFormatTable() { Title = "PR 1", PropertyName = "PR1", Axis = ePivotFieldAxis.Row, Order = 20 },
new ExcelFormatTable() { Title = "PR1 Name", PropertyName = "PR1N", Axis = ePivotFieldAxis.Row, Order = 21 },
new ExcelFormatTable() { Title = "PR 2", PropertyName = "PR2", Axis = ePivotFieldAxis.Row, Order = 22 },
new ExcelFormatTable() { Title = "PR2 Name", PropertyName = "PR2N", Axis = ePivotFieldAxis.Row, Order = 23 },
new ExcelFormatTable() { Title = "PR 3", PropertyName = "PR3", Axis = ePivotFieldAxis.Row, Order = 24 },
new ExcelFormatTable() { Title = "PR3 Name", PropertyName = "PR3N", Axis = ePivotFieldAxis.Row, Order = 25 },
new ExcelFormatTable() { Title = "Host", PropertyName = "Host", Axis = ePivotFieldAxis.Row, Order = 26 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "Comments", Axis = ePivotFieldAxis.Row, Order = 27 }
      };
    }

    #endregion RptInOut

    #region RptManifestRange

    /// <summary>
    /// Formato para el reporte Guest Manifest Range
    /// </summary>
    /// <history>
    /// [edgrodriguez] 06/Jun/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptManifestRange()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Date", PropertyName = "DateManifest", Format = EnumFormatTypeExcel.Date, Order = 1 },
new ExcelFormatTable() { Title = "SaleType", PropertyName = "SaleType", IsVisible = false },
new ExcelFormatTable() { Title = "SaleTypeN", PropertyName = "SaleTypeN", IsGroup = true, Order = 1 },
new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Order = 2 },
new ExcelFormatTable() { Title = "SR", PropertyName = "SalesRoom", Order = 3 },
new ExcelFormatTable() { Title = "Loc", PropertyName = "Location", Order = 4 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "Hotel", Order = 5 },
new ExcelFormatTable() { Title = "Room", PropertyName = "Room", Order = 6 },
new ExcelFormatTable() { Title = "Pax", PropertyName = "Pax", Order = 7 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "LastName", Order = 8 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "FirstName", Order = 9 },
new ExcelFormatTable() { Title = "Agency ID", PropertyName = "Agency", Order = 10 },
new ExcelFormatTable() { Title = "Agency", PropertyName = "AgencyN", Order = 11 },
new ExcelFormatTable() { Title = "Country ID", PropertyName = "Country", Order = 12 },
new ExcelFormatTable() { Title = "Country", PropertyName = "CountryN", Order = 13 },
new ExcelFormatTable() { Title = "Show", PropertyName = "showD", Format = EnumFormatTypeExcel.Date, Order = 14 },
new ExcelFormatTable() { Title = "T In", PropertyName = "TimeInT", Format = EnumFormatTypeExcel.Time, Order = 15 },
new ExcelFormatTable() { Title = "T Out", PropertyName = "TimeOutT", Format = EnumFormatTypeExcel.Time, Order = 16 },
new ExcelFormatTable() { Title = "Check In", PropertyName = "CheckIn", Format = EnumFormatTypeExcel.Date, Order = 17 },
new ExcelFormatTable() { Title = "Check Out", PropertyName = "CheckOut", Format = EnumFormatTypeExcel.Date, Order = 18 },
new ExcelFormatTable() { Title = "D", PropertyName = "Direct", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 19 },
new ExcelFormatTable() { Title = "Tr", PropertyName = "Tour", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 20 },
new ExcelFormatTable() { Title = "IO", PropertyName = "InOut", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 21 },
new ExcelFormatTable() { Title = "WO", PropertyName = "WalkOut", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 22 },
new ExcelFormatTable() { Title = "CT", PropertyName = "CTour", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 23 },
new ExcelFormatTable() { Title = "Sve", PropertyName = "SaveTour", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 24 },
new ExcelFormatTable() { Title = "PR 1", PropertyName = "PR1", Order = 25 },
new ExcelFormatTable() { Title = "PR 1 Name", PropertyName = "PR1N", Order = 26 },
new ExcelFormatTable() { Title = "PR 2", PropertyName = "PR2" , Order = 27 },
new ExcelFormatTable() { Title = "PR 2 Name", PropertyName = "PR2N", Order = 28 },
new ExcelFormatTable() { Title = "PR 3", PropertyName = "PR3", Order = 29 },
new ExcelFormatTable() { Title = "PR 3 Name", PropertyName = "PR3N", Order = 30 },
new ExcelFormatTable() { Title = "Liner 1", PropertyName = "Liner1", Order = 31 },
new ExcelFormatTable() { Title = "Liner 1 Name", PropertyName = "Liner1N", Order = 32 },
new ExcelFormatTable() { Title = "Liner 2", PropertyName = "Liner2", Order = 33 },
new ExcelFormatTable() { Title = "Liner 2 Name", PropertyName = "Liner2N", Order = 34 },
new ExcelFormatTable() { Title = "Closer 1", PropertyName = "Closer1", Order = 35 },
new ExcelFormatTable() { Title = "Closer 1 Name", PropertyName = "Closer1N", Order = 36 },
new ExcelFormatTable() { Title = "Closer 2", PropertyName = "Closer2", Order = 37 },
new ExcelFormatTable() { Title = "Closer 2 Name", PropertyName = "Closer2N", Order = 38 },
new ExcelFormatTable() { Title = "Closer 3", PropertyName = "Closer3", Order = 39 },
new ExcelFormatTable() { Title = "Closer 3 Name", PropertyName = "Closer3N", Order = 40 },
new ExcelFormatTable() { Title = "Exit 1", PropertyName = "Exit1", Order = 41 },
new ExcelFormatTable() { Title = "Exit 1 Name", PropertyName = "Exit1N", Order = 42 },
new ExcelFormatTable() { Title = "Exit 2", PropertyName = "Exit2", Order = 43 },
new ExcelFormatTable() { Title = "Exit 2 Name", PropertyName = "Exit2N", Order = 44 },
new ExcelFormatTable() { Title = "Hostess", PropertyName = "Hostess", Order = 45 },
new ExcelFormatTable() { Title = "Proc Sales", PropertyName = "ProcSales",Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 46 },
new ExcelFormatTable() { Title = "Proc Orig",  PropertyName = "ProcOriginal", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 47 },
new ExcelFormatTable() { Title = "Proc New", PropertyName = "ProcNew", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 48 },
new ExcelFormatTable() { Title = "Proc Gross", PropertyName = "ProcGross",  Format = EnumFormatTypeExcel.DecimalNumber,Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 49 },
new ExcelFormatTable() { Title = "Pend Sales", PropertyName = "PendSales", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 50 },
new ExcelFormatTable() { Title = "Pend Original", PropertyName = "PendOriginal", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 51 },
new ExcelFormatTable() { Title = "Pend New", PropertyName = "PendNew", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 52 },
new ExcelFormatTable() { Title = "Pend Gross", PropertyName = "PendGross", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 53 },
new ExcelFormatTable() { Title = "C Cost", PropertyName = "ClosingCost", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 54 },
new ExcelFormatTable() { Title = "Memb #", PropertyName = "MemberShip", Order = 55 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "Comments", Order = 56 }
      };
    }

    #endregion RptManifestRange

    #region RptManifestRangeByLs

    /// <summary>
    /// Formato para el reporte Guest Manifest Range
    /// </summary>
    /// <history>
    /// [edgrodriguez] 13/Jun/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptManifestRangeByLs()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "SaleType", PropertyName = "SaleType", IsVisible = false },
new ExcelFormatTable() { Title = "SaleTypeN", PropertyName = "SaleTypeN", IsGroup = true, Order = 1 },
new ExcelFormatTable() { Title = "Date", PropertyName = "DateManifest", Format = EnumFormatTypeExcel.Date, Order = 1 },
new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Order = 4 },
new ExcelFormatTable() { Title = "Location", PropertyName = "Location", IsVisible = false},
new ExcelFormatTable() { Title = "LocationN", PropertyName = "LocationN", IsGroup = true, Order = 2 },
new ExcelFormatTable() { Title = "ShowProgram", PropertyName = "ShowProgramN", IsVisible = false },
new ExcelFormatTable() { Title = "SR", PropertyName = "SalesRoom", Order = 2 },
new ExcelFormatTable() { Title = "LS", PropertyName = "LeadSource", Order  = 3 },
new ExcelFormatTable() { Title = "Sequency", PropertyName = "Sequency", IsVisible = false },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "Hotel", Order = 9 },
new ExcelFormatTable() { Title = "Room", PropertyName = "Room", Order = 10 },
new ExcelFormatTable() { Title = "Pax", PropertyName = "Pax", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 13 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "LastName", Order = 5 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "FirstName", Order = 6 },
new ExcelFormatTable() { Title = "Agency ID", PropertyName = "Agency", Order = 11 },
new ExcelFormatTable() { Title = "Agency", PropertyName = "AgencyN", Order = 12 },
new ExcelFormatTable() { Title = "Show D", PropertyName = "ShowD", IsVisible = false },
new ExcelFormatTable() { Title = "Country ID", PropertyName = "Country", Order = 7 },
new ExcelFormatTable() { Title = "Country", PropertyName = "CountryD", Order = 8 },
new ExcelFormatTable() { Title = "T In", PropertyName = "TimeInT", Format = EnumFormatTypeExcel.Time, Order = 14 },
new ExcelFormatTable() { Title = "T Out", PropertyName = "TimeOutT", Format = EnumFormatTypeExcel.Time, Order = 15 },
new ExcelFormatTable() { Title = "Sh", PropertyName = "Show", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 17 },
new ExcelFormatTable() { Title = "Tr", PropertyName = "Tour", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 18 },
new ExcelFormatTable() { Title = "IO", PropertyName = "IO", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 19 },
new ExcelFormatTable() { Title = "WO", PropertyName = "WO", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 20 },
new ExcelFormatTable() { Title = "CT", PropertyName = "CTour", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 21 },
new ExcelFormatTable() { Title = "Sve", PropertyName = "STour", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 22 },
new ExcelFormatTable() { Title = "D", PropertyName = "Direct", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 23 },
new ExcelFormatTable() { Title = "R", PropertyName = "Resch", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 24 },
new ExcelFormatTable() { Title = "PR 1", PropertyName = "PR1", Order = 25 },
new ExcelFormatTable() { Title = "PR 1 Name", PropertyName = "PR1N", Order = 26 },
new ExcelFormatTable() { Title = "PR 2", PropertyName = "PR2" , Order = 27 },
new ExcelFormatTable() { Title = "PR 2 Name", PropertyName = "PR2N", Order = 28 },
new ExcelFormatTable() { Title = "PR 3", PropertyName = "PR3", Order = 29 },
new ExcelFormatTable() { Title = "PR 3 Name", PropertyName = "PR3N", Order = 30 },
new ExcelFormatTable() { Title = "Liner 1", PropertyName = "Liner1", Order = 31 },
new ExcelFormatTable() { Title = "Liner 1 Name", PropertyName = "Liner1N", Order = 32 },
new ExcelFormatTable() { Title = "Liner 2", PropertyName = "Liner2", Order = 33 },
new ExcelFormatTable() { Title = "Liner 2 Name", PropertyName = "Liner2N", Order = 34 },
new ExcelFormatTable() { Title = "Closer 1", PropertyName = "Closer1", Order = 35 },
new ExcelFormatTable() { Title = "Closer 1 Name", PropertyName = "Closer1N", Order = 36 },
new ExcelFormatTable() { Title = "Closer 2", PropertyName = "Closer2", Order = 37 },
new ExcelFormatTable() { Title = "Closer 2 Name", PropertyName = "Closer2N", Order = 38 },
new ExcelFormatTable() { Title = "Closer 3", PropertyName = "Closer3", Order = 39 },
new ExcelFormatTable() { Title = "Closer 3 Name", PropertyName = "Closer3N", Order = 40 },
new ExcelFormatTable() { Title = "Exit 1", PropertyName = "Exit1", Order = 41 },
new ExcelFormatTable() { Title = "Exit 1 Name", PropertyName = "Exit1N", Order = 42 },
new ExcelFormatTable() { Title = "Exit 2", PropertyName = "Exit2", Order = 43 },
new ExcelFormatTable() { Title = "Exit 2 Name", PropertyName = "Exit2N", Order = 44 },
new ExcelFormatTable() { Title = "Memb #", PropertyName = "saMembershipNum", Order = 45 },
new ExcelFormatTable() { Title = "Host", PropertyName = "Hostess", Order = 16 },
new ExcelFormatTable() { Title = "Sale", PropertyName = "ProcSales",Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 46 },
new ExcelFormatTable() { Title = "Proc Orig",  PropertyName = "ProcOriginal", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 47 },
new ExcelFormatTable() { Title = "Proc New", PropertyName = "ProcNew", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 48 },
new ExcelFormatTable() { Title = "Proc Gross", PropertyName = "ProcGross",  Format = EnumFormatTypeExcel.DecimalNumber,Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 49 },
new ExcelFormatTable() { Title = "Pend Sales", PropertyName = "PendSales", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 50 },
new ExcelFormatTable() { Title = "Pend Original", PropertyName = "PendOriginal", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 51 },
new ExcelFormatTable() { Title = "Pend New", PropertyName = "PendNew", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 52 },
new ExcelFormatTable() { Title = "Pend Gross", PropertyName = "PendGross", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 53 },
new ExcelFormatTable() { Title = "Deposit Sale", PropertyName = "DepositSale", Order = 54 },
new ExcelFormatTable() { Title = "Deposit Sale Num", PropertyName = "DepositSaleNum", Order = 55 },
new ExcelFormatTable() { Title = "CC", PropertyName = "CC", Order = 56 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "Comments", Order = 57 }
      };
    }

    /// <summary>
    /// Formato para el reporte Guest Manifest Range
    /// </summary>
    /// <history>
    /// [edgrodriguez] 13/Jun/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptManifestRangeByLs_Bookings()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "guloInvit", PropertyName = "guloInvit", IsVisible = false },
new ExcelFormatTable() { Title = "LocationN", PropertyName = "LocationN", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "guBookT", PropertyName = "guBookT", Order = 1, Axis = ePivotFieldAxis.Column, Sort = eSortType.Ascending },
new ExcelFormatTable() {Title = "guBookTime",PropertyName= "guBookTime", Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "Bookings", PropertyName = "Bookings", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2 }
      };
    }

    #endregion RptManifestRangeByLs

    #region RptGuestNoShow

    /// <summary>
    /// Formato para el reporte Guest No Show
    /// </summary>
    /// <history>
    /// [edgrodriguez] 09/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGuestNoShow()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Axis = ePivotFieldAxis.Row, Order = 4  },
new ExcelFormatTable() { Title = "Room", PropertyName = "guRoomNum", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "guHotel", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "guLastName1", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "guFirstName1", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Loc", PropertyName = "guloInvit", Axis = ePivotFieldAxis.Row, Order = 1, Compact = true, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default },
new ExcelFormatTable() { Title = "Agency ID", PropertyName = "guag", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Agency", PropertyName = "agN", Axis = ePivotFieldAxis.Row, Order = 3},
new ExcelFormatTable() { Title = "Country ID", PropertyName = "guco", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Country", PropertyName = "coN", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Pax", PropertyName = "gupax", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Check-In Date", PropertyName = "guCheckInD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Deposit", PropertyName = "guDeposit", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "Book Date", PropertyName = "guBookD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Book Cancel", PropertyName = "guBookCanc", Axis = ePivotFieldAxis.Row, Order = 14 }
      };
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
    public static List<ExcelFormatTable> RptMealTickets()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "No", PropertyName = "meID", Axis = ePivotFieldAxis.Row, Order = 2  },
new ExcelFormatTable() { Title = "Date", PropertyName = "meD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Guest ID", PropertyName = "megu", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Qty", PropertyName = "meQty", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "Type", PropertyName = "myN", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Adults", PropertyName = "meAdults", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2 },
new ExcelFormatTable() { Title = "Minors", PropertyName = "meMinors", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3 },
new ExcelFormatTable() { Title = "Total", PropertyName = "Total", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 4},
new ExcelFormatTable() { Title = "Folios", PropertyName = "meFolios", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "meComments", Axis = ePivotFieldAxis.Row, Order = 16 },
new ExcelFormatTable() { Title = "Guest Name", PropertyName = "guLastName1", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Loc", PropertyName = "guloInfo", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "Host", PropertyName = "guEntryHost", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Show", PropertyName = "guShow", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Host Name", PropertyName = "guEntryHostN", Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "PR", PropertyName = "guPRInvit1", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "guPRInvit1N", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Liner", PropertyName = "guLiner1", Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() { Title = "Liner Name", PropertyName = "guLiner1N", Axis = ePivotFieldAxis.Row, Order = 15 },
new ExcelFormatTable() { Title = "mera", PropertyName = "mera" },
new ExcelFormatTable() { Title = "Rate Type", PropertyName = "RateTypeN", Axis = ePivotFieldAxis.Row, Order = 1, Compact = true, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default },
new ExcelFormatTable() { Title = "mepe", PropertyName = "mepe" },
new ExcelFormatTable() { Title = "# Collaborator", PropertyName = "peCollaboratorID", Axis = ePivotFieldAxis.Row, Order = 17 },
new ExcelFormatTable() { Title = "Collaborator", PropertyName = "peN", Axis = ePivotFieldAxis.Row, Order = 18 },
new ExcelFormatTable() { Title = "Agency", PropertyName = "agN", Axis = ePivotFieldAxis.Row, Order = 19 },
new ExcelFormatTable() { Title = "Representative", PropertyName = "merep", Axis = ePivotFieldAxis.Row, Order = 20 }
      };
    }

    #endregion RptMealTickets

    #region RptMealTicketsByHost

    /// <summary>
    /// Formato para el reporte Meal Tickets by Host
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptMealTicketsByHost()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "No", PropertyName = "meID", Axis = ePivotFieldAxis.Row, Order = 3  },
new ExcelFormatTable() { Title = "Date", PropertyName = "meD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Guest ID", PropertyName = "megu", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Qty", PropertyName = "meQty", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "Type", PropertyName = "myN", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Adults", PropertyName = "meAdults", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2 },
new ExcelFormatTable() { Title = "Minors", PropertyName = "meMinors", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3 },
new ExcelFormatTable() { Title = "Total", PropertyName = "Total", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 4},
new ExcelFormatTable() { Title = "Folios", PropertyName = "meFolios", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "meComments", Axis = ePivotFieldAxis.Row, Order = 16 },
new ExcelFormatTable() { Title = "Guest Name", PropertyName = "guLastName1", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Loc", PropertyName = "guloInfo", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Host", PropertyName = "guEntryHost", Axis = ePivotFieldAxis.Row, Order = 2, Compact = true, Outline = true  },
new ExcelFormatTable() { Title = "Show", PropertyName = "guShow", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "Host Name", PropertyName = "guEntryHostN", Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "PR", PropertyName = "guPRInvit1", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "guPRInvit1N", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Liner", PropertyName = "guLiner1", Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() { Title = "Liner Name", PropertyName = "guLiner1N", Axis = ePivotFieldAxis.Row, Order = 15 },
new ExcelFormatTable() { Title = "mera", PropertyName = "mera" },
new ExcelFormatTable() { Title = "Rate Type", PropertyName = "RateTypeN", Axis = ePivotFieldAxis.Row, Order = 1, Compact = true, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default },
new ExcelFormatTable() { Title = "mepe", PropertyName = "mepe" },
new ExcelFormatTable() { Title = "# Collaborator", PropertyName = "peCollaboratorID", Axis = ePivotFieldAxis.Row, Order = 17 },
new ExcelFormatTable() { Title = "Collaborator", PropertyName = "peN", Axis = ePivotFieldAxis.Row, Order = 18 },
new ExcelFormatTable() { Title = "Agency", PropertyName = "agN", Axis = ePivotFieldAxis.Row, Order = 19 },
new ExcelFormatTable() { Title = "Representative", PropertyName = "merep", Axis = ePivotFieldAxis.Row, Order = 20 }
      };
    }

    #endregion RptMealTicketsByHost

    #region RptMealTicketsCost

    /// <summary>
    /// Formato para el reporte Meal Tickets Cost
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptMealTicketsCost()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Date", PropertyName = "Date", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 1  },
new ExcelFormatTable() { Title = "Type", PropertyName = "Type", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "Adults", PropertyName = "Adults", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "Minors", PropertyName = "Minors", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2 },
new ExcelFormatTable() { Title = "$ Adults", PropertyName = "AdultsAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 3 },
new ExcelFormatTable() { Title = "$ Minors", PropertyName = "MinorsAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 4 },
new ExcelFormatTable() { Title = "$ Total", PropertyName = "TotalAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 5 }
      };
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
    public static List<ExcelFormatTable> RptMemberships()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Sale ID", PropertyName = "saID", Order = 1  },
new ExcelFormatTable() { Title = "Sale Date", PropertyName = "saD", Format = EnumFormatTypeExcel.Date, Order = 2 },
new ExcelFormatTable() { Title = "Proc Date", PropertyName = "saProcD", Format = EnumFormatTypeExcel.Date, Order = 3 },
new ExcelFormatTable() { Title = "Canc Date", PropertyName = "saCancelD", Format = EnumFormatTypeExcel.Date, Order = 4 },
new ExcelFormatTable() { Title = "Loc", PropertyName = "salo", Order = 5 },
new ExcelFormatTable() { Title = "SR", PropertyName = "sasr", Order = 6 },
new ExcelFormatTable() { Title = "Membership", PropertyName = "saMembershipNum", Order = 7  },
new ExcelFormatTable() { Title = "MT", PropertyName = "samt", Order = 8 },
new ExcelFormatTable() { Title = "ST", PropertyName = "sast", Order = 9 },
new ExcelFormatTable() { Title = "Last Name 1", PropertyName = "saLastName1", Order = 10 },
new ExcelFormatTable() { Title = "First Name 1", PropertyName = "saFirstName1", Order = 11 },
new ExcelFormatTable() { Title = "Last Name 2", PropertyName = "saLastName2", Order = 12 },
new ExcelFormatTable() { Title = "First Name 2", PropertyName = "saFirstName2", Order = 13  },
new ExcelFormatTable() { Title = "Proc Vol", PropertyName = "ProcAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 14, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "OOP Vol", PropertyName = "OOPAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 15, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Canc Vol", PropertyName = "CancAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 16, Function = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "Pend Vol", PropertyName = "PendAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 18, Function = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "PR1", PropertyName = "saPR1", Order = 19 },
new ExcelFormatTable() { Title = "PR2", PropertyName = "saPR2", Order = 20  },
new ExcelFormatTable() { Title = "PR3", PropertyName = "saPR3", Order = 21 },
new ExcelFormatTable() { Title = "Liner1", PropertyName = "saLiner1", Order = 22 },
new ExcelFormatTable() { Title = "Liner2", PropertyName = "saLiner2", Order = 23 },
new ExcelFormatTable() { Title = "Closer1", PropertyName = "saCloser1", Order = 24 },
new ExcelFormatTable() { Title = "Closer2", PropertyName = "saCloser2", Order = 25 },
new ExcelFormatTable() { Title = "Closer3", PropertyName = "saCloser3", Order = 26  },
new ExcelFormatTable() { Title = "Exit1", PropertyName = "saExit1", Order = 27 },
new ExcelFormatTable() { Title = "Exit2", PropertyName = "saExit2", Order = 28 },
new ExcelFormatTable() { Title = "VLO", PropertyName = "saVLO", Format = EnumFormatTypeExcel.Number, Order = 29 },
new ExcelFormatTable() { Title = "Podium", PropertyName = "saPodium", Order = 30 },
//new ExcelFormatTable() { Title = "GUID", PropertyName = "sagu" },
new ExcelFormatTable() { Title = "Comments", PropertyName = "saComments", Order = 31 },
new ExcelFormatTable() { Title = "Total Proc", PropertyName = "TotalProcAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 17, Function = DataFieldFunctions.Sum  }
      };
    }

    #endregion RptMemberships

    #region RptMembershipsByAgencyMarket

    /// <summary>
    /// Formato para el reporte Memberships
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptMembershipsByAgencyMarket()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Market", PropertyName = "mkN", Order = 1, IsGroup = true  },
new ExcelFormatTable() { Title = "Agency", PropertyName = "agN", Order = 2, IsGroup = true },
new ExcelFormatTable() { Title = "Sale ID", PropertyName = "saID", Order = 1  },
new ExcelFormatTable() { Title = "Sale Date", PropertyName = "saProcD", Format = EnumFormatTypeExcel.Date, Order = 2 },
new ExcelFormatTable() { Title = "Lead Source", PropertyName = "lsN", Order = 3 },
new ExcelFormatTable() { Title = "Sales Room", PropertyName = "srN", Order = 4 },
new ExcelFormatTable() { Title = "Membership", PropertyName = "saMembershipNum", Order = 5  },
new ExcelFormatTable() { Title = "Membership Type", PropertyName = "mtN", Order = 6 },
new ExcelFormatTable() { Title = "ST", PropertyName = "sast", Order = 7 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "saLastName1", Order = 8 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "saFirstName1", Order = 9 },
new ExcelFormatTable() { Title = "Proc Vol", PropertyName = "ProcAmount", Format = EnumFormatTypeExcel.Currency, Order = 10, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "OOP Vol", PropertyName = "OOPAmount", Format = EnumFormatTypeExcel.Currency, Order = 11, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Canc Vol", PropertyName = "CancAmount", Format = EnumFormatTypeExcel.Currency, Order = 12, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Pend Vol", PropertyName = "PendAmount", Format = EnumFormatTypeExcel.Currency, Order = 14, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "% DP", PropertyName = "saDownPaymentPercentage",IsCalculated = true, Formula = "IF([ProcAmount]=0,0,[saDownPayment]/[ProcAmount])" , Format = EnumFormatTypeExcel.Percent, Order = 15, SubTotalFunctions = eSubTotalFunctions.Avg },
new ExcelFormatTable() { Title = "% EDP", PropertyName = "saDownPaymentPaidPercentage", IsCalculated = true, Formula = "IF([ProcAmount]=0,0,[saDownPaymentPaid]/[ProcAmount])", Format = EnumFormatTypeExcel.Percent, Order = 17, SubTotalFunctions = eSubTotalFunctions.Avg },
new ExcelFormatTable() { Title = "Comments", PropertyName = "saComments", Order = 19 },
new ExcelFormatTable() { Title = "Total Proc", PropertyName = "TotalProcAmount", Format = EnumFormatTypeExcel.Currency, Order = 13, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "$ DP", PropertyName = "saDownPayment", Format = EnumFormatTypeExcel.Currency, Order = 16, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "$ EDP", PropertyName = "saDownPaymentPaid", Format = EnumFormatTypeExcel.Currency, Order = 18, SubTotalFunctions = eSubTotalFunctions.Sum  }
      };
    }

    #endregion RptMembershipsByAgencyMarket

    #region RptMembershipsByHost

    /// <summary>
    /// Formato para el reporte Memberships By Host
    /// </summary>
    /// <history>
    /// [edgrodriguez] 14/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptMembershipsByHost()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Host", PropertyName = "guEntryHost", Order = 1, IsGroup = true  },
new ExcelFormatTable() { Title = "Host Name", PropertyName = "guEntryHostN", IsVisible = false },
new ExcelFormatTable() { Title = "Sale Type", PropertyName = "SaleType", Order = 2, IsGroup = true  },
new ExcelFormatTable() { Title = "Sale ID", PropertyName = "saID", Order = 1 },
new ExcelFormatTable() { Title = "Sale Date", PropertyName = "saD", Format = EnumFormatTypeExcel.Date, Order = 2 },
new ExcelFormatTable() { Title = "LS", PropertyName = "sals", Order = 3 },
new ExcelFormatTable() { Title = "SR", PropertyName = "sasr", Order = 4  },
new ExcelFormatTable() { Title = "Membership", PropertyName = "saMembershipNum", Order = 5 },
new ExcelFormatTable() { Title = "MT", PropertyName = "samt", Order = 6 },
new ExcelFormatTable() { Title = "ST", PropertyName = "sast", Order = 7 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "saLastName1", Order = 8 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "saFirstName1", Order = 9 },
new ExcelFormatTable() { Title = "Proc Vol", PropertyName = "ProcAmount", Format = EnumFormatTypeExcel.Currency, Order = 10, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "OOP Vol", PropertyName = "OOPAmount", Format = EnumFormatTypeExcel.Currency, Order = 11, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Canc Vol", PropertyName = "CancAmount", Format = EnumFormatTypeExcel.Currency, Order = 12, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Pend Vol", PropertyName = "PendAmount", Format = EnumFormatTypeExcel.Currency, Order = 14, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Comments", PropertyName = "saComments", Order = 15 },
new ExcelFormatTable() { Title = "Total Proc", PropertyName = "TotalProcAmount", Format = EnumFormatTypeExcel.Currency, Order = 13, SubTotalFunctions = eSubTotalFunctions.Sum  }
      };
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
    public static List<ExcelFormatTable> RptProductionBySalesRoom()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoom", Format = EnumFormatTypeExcel.Number, Order = 1 },
new ExcelFormatTable() { Title = "Books", PropertyName = "GrossBooks", Format = EnumFormatTypeExcel.Number, Order = 2, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Directs", PropertyName = "Directs", Format = EnumFormatTypeExcel.Number, Order = 3, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "T Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Order = 4, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Shows", PropertyName = "GrossShows", Format = EnumFormatTypeExcel.Number, Order = 5, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "T Shows", PropertyName = "Shows", Format = EnumFormatTypeExcel.Number, Order = 6, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Sh %", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Order = 7, IsCalculated = true, Formula="IF([GrossBooks]=0,0,[GrossShows]/[GrossBooks])" },
new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales", Format = EnumFormatTypeExcel.Number, Order = 8, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount", Format = EnumFormatTypeExcel.Number, Order = 9, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Ci %", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Order = 11, IsCalculated = true, Formula="IF([Shows]=0,0,[Sales]/[Shows])" },
new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Number, Order = 10, IsCalculated = true, Formula="IF([Shows]=0,0,[SalesAmount]/[Shows])" },
new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Number, Order = 12, IsCalculated = true, Formula="IF([Sales]=0,0,[SalesAmount]/[Sales])" }
      };
    }

    #endregion RptProductionBySalesRoom

    #region RptProductionBySalesRoomMarket

    /// <summary>
    /// Formato para el reporte Production by SalesRoom & Market
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptProductionBySalesRoomMarket()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoom", Format = EnumFormatTypeExcel.Number, Order = 1 },
new ExcelFormatTable() { Title = "Market", PropertyName = "Market", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "Books", PropertyName = "GrossBooks", Format = EnumFormatTypeExcel.Number, Order = 2, SubTotalFunctions= eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Directs", PropertyName = "Directs", Format = EnumFormatTypeExcel.Number, Order = 3, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "T Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Order = 4, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Shows", PropertyName = "GrossShows", Format = EnumFormatTypeExcel.Number, Order = 5, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "T Shows", PropertyName = "Shows", Format = EnumFormatTypeExcel.Number, Order = 6, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Sh %", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Order = 7, IsCalculated = true, Formula="IF([GrossBooks]=0,0,[GrossShows]/[GrossBooks])" },
new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales", Format = EnumFormatTypeExcel.Number, Order = 8, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount", Format = EnumFormatTypeExcel.Number, Order = 9, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Ci %", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Order = 11, IsCalculated = true, Formula="IF([Shows]=0,0,[Sales]/[Shows])" },
new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Number, Order = 10, IsCalculated = true, Formula="IF([Shows]=0,0,[SalesAmount]/[Shows])" },
new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Number, Order = 12, IsCalculated = true, Formula="IF([Sales]=0,0,[SalesAmount]/[Sales])" }
      };
    }

    #endregion RptProductionBySalesRoomMarket

    #region RptProductionBySalesRoomMarketSubMarket

    /// <summary>
    /// Formato para el reporte Production by SalesRoom,Market & Submarket
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptProductionBySalesRoomMarketSubMarket()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoom", Format = EnumFormatTypeExcel.Number, Order = 1 },
new ExcelFormatTable() { Title = "Program", PropertyName = "Program", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "Market", PropertyName = "Market", Order = 2, IsGroup = true },
new ExcelFormatTable() { Title = "Submarket", PropertyName = "Submarket", Order = 3, IsGroup = true },
new ExcelFormatTable() { Title = "Books", PropertyName = "GrossBooks", Format = EnumFormatTypeExcel.Number, Order = 2, Function = DataFieldFunctions.Sum, SubTotalFunctions= eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Directs", PropertyName = "Directs", Format = EnumFormatTypeExcel.Number, Order = 3, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "T Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Order = 4, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Shows", PropertyName = "GrossShows", Format = EnumFormatTypeExcel.Number, Order = 5, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "T Shows", PropertyName = "Shows", Format = EnumFormatTypeExcel.Number, Order = 6, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Sh %", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Order = 7, IsCalculated = true, Formula="IF([GrossBooks]=0,0,[GrossShows]/[GrossBooks])" },
new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales", Format = EnumFormatTypeExcel.Number, Order = 8, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount", Format = EnumFormatTypeExcel.Number, Order = 9, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Ci %", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Order = 11, IsCalculated = true, Formula="IF([Shows]=0,0,[Sales]/[Shows])" },
new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Number, Order = 10, IsCalculated = true, Formula="IF([Shows]=0,0,[SalesAmount]/[Shows])" },
new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Number, Order = 12, IsCalculated = true, Formula="IF([Sales]=0,0,[SalesAmount]/[Sales])" }
      };
    }

    #endregion RptProductionBySalesRoomMarketSubMarket

    #region RptProductionByShowProgram

    /// <summary>
    /// Formato para el reporte Production by Show & Program
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptProductionByShowProgram()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Show Program Category", PropertyName = "ShowProgramCategory", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "Show Program", PropertyName = "ShowProgram", Order = 1 },
new ExcelFormatTable() { Title = "Books", PropertyName = "GrossBooks", Format = EnumFormatTypeExcel.Number, Order = 2, SubTotalFunctions= eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Directs", PropertyName = "Directs", Format = EnumFormatTypeExcel.Number, Order = 3, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "T Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Order = 4, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Shows", PropertyName = "GrossShows", Format = EnumFormatTypeExcel.Number, Order = 5, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "T Shows", PropertyName = "Shows", Format = EnumFormatTypeExcel.Number, Order = 6, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Sh %", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Order = 7, IsCalculated = true, Formula="IF([GrossBooks]=0,0,[GrossShows]/[GrossBooks])" },
new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales", Format = EnumFormatTypeExcel.Number, Order = 8, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount", Format = EnumFormatTypeExcel.Number, Order = 9, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Ci %", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Order = 11, IsCalculated = true, Formula="IF([Shows]=0,0,[Sales]/[Shows])" },
new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Number, Order = 10, IsCalculated = true, Formula="IF([Shows]=0,0,[SalesAmount]/[Shows])" },
new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Number, Order = 12, IsCalculated = true, Formula="IF([Sales]=0,0,[SalesAmount]/[Sales])" }
      };
    }

    #endregion RptProductionByShowProgram

    #region RptProductionByShowProgramProgram

    /// <summary>
    /// Formato para el reporte Production by Show,Program & Program
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptProductionByShowProgramProgram()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Program", PropertyName = "Program", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "Show Program Category", PropertyName = "ShowProgramCategory", Order = 2, IsGroup = true },
new ExcelFormatTable() { Title = "Show Program", PropertyName = "ShowProgram", Order = 1 },
new ExcelFormatTable() { Title = "Books", PropertyName = "GrossBooks", Format = EnumFormatTypeExcel.Number, Order = 2, SubTotalFunctions= eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Directs", PropertyName = "Directs", Format = EnumFormatTypeExcel.Number, Order = 3, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "T Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Order = 4, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Shows", PropertyName = "GrossShows", Format = EnumFormatTypeExcel.Number, Order = 5, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "T Shows", PropertyName = "Shows", Format = EnumFormatTypeExcel.Number, Order = 6, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Sh %", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Order = 7, IsCalculated = true, Formula="IF([GrossBooks]=0,0,[GrossShows]/[GrossBooks])" },
new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales", Format = EnumFormatTypeExcel.Number, Order = 8, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount", Format = EnumFormatTypeExcel.Number, Order = 9, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Ci %", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Order = 11, IsCalculated = true, Formula="IF([Shows]=0,0,[Sales]/[Shows])" },
new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Number, Order = 10, IsCalculated = true, Formula="IF([Shows]=0,0,[SalesAmount]/[Shows])" },
new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Number, Order = 12, IsCalculated = true, Formula="IF([Sales]=0,0,[SalesAmount]/[Sales])" }
      };
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
    public static List<ExcelFormatTable> RptCloserStatistic()
    {
      return new List<ExcelFormatTable> {
new ExcelFormatTable() { Title = "ID", PropertyName = "saCloser", Order = 1 },
new ExcelFormatTable() { Title = "Closer Name", PropertyName = "saCloserN", Order = 2 },
new ExcelFormatTable() { Title = "PS", PropertyName = "saCloserps", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "Ups", PropertyName = "Shows", Format = EnumFormatTypeExcel.DecimalNumber, Order = 3, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Proc Sales", PropertyName = "ProcSales", Format = EnumFormatTypeExcel.DecimalNumber, Order = 4, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Proc Amount", PropertyName = "ProcAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 5, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "OOP Sales", PropertyName = "OOPSales", Format = EnumFormatTypeExcel.DecimalNumber, Order = 6, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "OOP Amount", PropertyName = "OOPAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 7, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Cancel Sales", PropertyName = "CancelSales", Format = EnumFormatTypeExcel.DecimalNumber, Order = 8, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Cancelled", PropertyName = "CancelAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 9, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "C %", PropertyName = "CancelF", Format = EnumFormatTypeExcel.Percent, Order = 10, IsCalculated = true, Formula = "IF([TotalProc]=0, 0, [CancelAmount]/[TotalProc])" },
new ExcelFormatTable() { Title = "Subtotal", PropertyName = "TotalProc", Format = EnumFormatTypeExcel.DecimalNumber, Order = 11, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Sales", PropertyName = "TotalProcSales", Format = EnumFormatTypeExcel.DecimalNumber, Order = 12, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Total", PropertyName = "TotalProcAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 13, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.DecimalNumber, Order = 14, IsCalculated = true, Formula = "IF([Shows]=0, 0, [TotalProcAmount]/[Shows])" },
new ExcelFormatTable() { Title = "CI %", PropertyName = "ClosingF", Format = EnumFormatTypeExcel.Percent, Order = 15, IsCalculated = true, Formula = "IF([Shows]=0, 0, [TotalProcSales]/[Shows])" },
new ExcelFormatTable() { Title = "AvgSales", PropertyName = "AvgSales", Format = EnumFormatTypeExcel.DecimalNumber, Order = 16, IsCalculated = true, Formula = "IF([TotalProcSales]=0, 0, [TotalProcAmount]/[TotalProcSales])" },
new ExcelFormatTable() { Title = "Pend Sales", PropertyName = "PendSales", Format = EnumFormatTypeExcel.DecimalNumber, Order = 17, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Pending", PropertyName = "PendAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 18, SubTotalFunctions = eSubTotalFunctions.Sum }
      };
    }

    #endregion

    #region RptLinerStatistic

    /// <summary>
    /// Formato para el reporte Liner Statistics
    /// </summary>
    /// <history>
    /// [edgrodriguez] 12/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptLinerStatistic()
    {
      return new List<ExcelFormatTable> {
new ExcelFormatTable() { Title = "ID", PropertyName = "saLiner", Order = 1 },
new ExcelFormatTable() { Title = "Closer Name", PropertyName = "saLinerN", Order = 2 },
new ExcelFormatTable() { Title = "PS", PropertyName = "saLinerps", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "Ups", PropertyName = "Shows", Format = EnumFormatTypeExcel.DecimalNumber,Order = 3, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "WO", PropertyName = "WalkOut", Format = EnumFormatTypeExcel.DecimalNumber, Order = 4, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "T Ups", PropertyName = "TotalShows", Format = EnumFormatTypeExcel.DecimalNumber, Order = 5, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Proc Sales", PropertyName = "ProcSales", Format = EnumFormatTypeExcel.DecimalNumber,Order = 6, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Proc Amount", PropertyName = "ProcAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 7, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "OOP Sales", PropertyName = "OOPSales", Format = EnumFormatTypeExcel.DecimalNumber, Order = 8, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "OOP Amount", PropertyName = "OOPAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 9, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Cancel Sales", PropertyName = "CancSales", Format = EnumFormatTypeExcel.DecimalNumber, Order = 10, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Cancelled", PropertyName = "CancAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 11, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Sales", PropertyName = "TotalProcSales", Format = EnumFormatTypeExcel.DecimalNumber, Order = 14, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Total", PropertyName = "TotalProcAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 15, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Proc Sales Ln", PropertyName = "ProcSalesLn", Format = EnumFormatTypeExcel.DecimalNumber,Order = 16, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Proc Amount Ln", PropertyName = "ProcAmountLn", Format = EnumFormatTypeExcel.DecimalNumber, Order = 17, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Proc Sales FM", PropertyName = "ProcSalesFM", Format = EnumFormatTypeExcel.DecimalNumber, Order = 18, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Proc Amount FM", PropertyName = "ProcAmountFM", Format = EnumFormatTypeExcel.DecimalNumber, Order = 19, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Proc Sales FB", PropertyName = "ProcSalesFB", Format = EnumFormatTypeExcel.DecimalNumber, Order = 20, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Proc Amount FB", PropertyName = "ProcAmountFB", Format = EnumFormatTypeExcel.DecimalNumber, Order = 21, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Pend Sales", PropertyName = "PendSales", Format = EnumFormatTypeExcel.DecimalNumber, Order = 25, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Pending", PropertyName = "PendAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 26, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.DecimalNumber, Order = 22, IsCalculated = true, Formula = "IF([Shows]=0, 0, [TotalProcAmount]/[Shows])" },
new ExcelFormatTable() { Title = "CI %", PropertyName = "ClosingF", Format = EnumFormatTypeExcel.Percent, Order = 23, IsCalculated = true, Formula = "IF([Shows]=0, 0, [TotalProcSales]/[Shows])" },
new ExcelFormatTable() { Title = "AvgSales", PropertyName = "AvgSales", Format = EnumFormatTypeExcel.DecimalNumber, Order = 24, IsCalculated = true, Formula = "IF([TotalProcSales]=0, 0, [TotalProcAmount]/[TotalProcSales])" },
new ExcelFormatTable() { Title = "Subtotal", PropertyName = "TotalProc", Format = EnumFormatTypeExcel.DecimalNumber, Order = 13, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "C %", PropertyName = "CancelF", Format = EnumFormatTypeExcel.Percent, Order = 12, IsCalculated = true, Formula = "IF([TotalProc]=0, 0, [CancAmount]/[TotalProc])" }
      };
    }

    #endregion

    #region RptWeeklyMonthlyHostessByPr

    /// <summary>
    /// Formato para el reporte Weekly & Monthly Hostess
    /// </summary>
    /// <history>
    /// [edgrodriguez] 09/Jun/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptWeeklyMonthlyHostessByPr()
    {
      return new List<ExcelFormatTable> {
new ExcelFormatTable() { Title = "Type", PropertyName = "Type", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "guls", PropertyName = "guls", IsGroup = true },
new ExcelFormatTable() { Title = "guloInvit", PropertyName = "guloInvit", IsVisible = false },
new ExcelFormatTable() { Title = "D", PropertyName = "guDirect", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "IO", PropertyName = "guInOut", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 4, Function = DataFieldFunctions.Sum,SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "WO", PropertyName = "guWalkOut", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "PR", PropertyName = "guPRInvit", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "guPRInvitN", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "gusr", PropertyName = "gusr", IsVisible = false }
      };
    }

    public static List<ExcelFormatTable> RptWeeklyMonthlyHostessByTourTime()
    {
      return new List<ExcelFormatTable> {
new ExcelFormatTable() { Title = "guls", PropertyName = "guls", IsGroup = true, Order = 1 },
new ExcelFormatTable() { Title = "Day", PropertyName = "guD", Format = EnumFormatTypeExcel.Day, Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Time", PropertyName = "Time", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "B", PropertyName = "guBook", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "Sh", PropertyName = "guShow", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum,SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum, Formula = "IF([guBook]=0,0,[guShow]/[guBook])" },
new ExcelFormatTable() { Title = "D", PropertyName = "guDirect", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 4, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum }
      };
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
    public static List<ExcelFormatTable> RptTaxisIn()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Show Type", PropertyName = "ShowType", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Order = 1 },
new ExcelFormatTable() { Title = "Loc", PropertyName = "guloInvit", Order = 2 },
new ExcelFormatTable() { Title = "Date", PropertyName = "guShowD", Format = EnumFormatTypeExcel.Date, Order = 3 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "guGuest", Order = 4 },
new ExcelFormatTable() { Title = "Pax", PropertyName = "guPax", Format = EnumFormatTypeExcel.DecimalNumber, Order = 5, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Adults", PropertyName = "Adults", Format = EnumFormatTypeExcel.Number, Order = 6, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Minors", PropertyName = "Minors", Format = EnumFormatTypeExcel.Number, Order = 7, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "guHotel", Order = 8 },
new ExcelFormatTable() { Title = "PR", PropertyName = "guPRInvit1", Order = 9 },
new ExcelFormatTable() { Title = "Taxi", PropertyName = "guTaxiIn", Format = EnumFormatTypeExcel.DecimalNumber, Order = 10, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Host", PropertyName = "guEntryHost", Order = 11 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "guWComments", Order = 12 }
      };
    }

    #endregion RptTaxisIn

    #region RptTaxisOut

    /// <summary>
    /// Formato para el reporte Taxis Out
    /// </summary>
    /// <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptTaxisOut()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Ch B", PropertyName = "grID", Order =1 },
new ExcelFormatTable() { Title = "Guest Name", PropertyName = "grGuest", Order = 5 },
new ExcelFormatTable() { Title = "LS", PropertyName = "grlo", Order = 4 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "grHotel", Order = 7 },
new ExcelFormatTable() { Title = "Pax", PropertyName = "grPax", Order = 6, Format = EnumFormatTypeExcel.DecimalNumber, Function=DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Date", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Order = 3 },
new ExcelFormatTable() { Title = "PR", PropertyName = "grpe", Order = 8 },
new ExcelFormatTable() { Title = "Taxi", PropertyName = "grTaxiOut", Format = EnumFormatTypeExcel.DecimalNumber, Order = 9, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Taxi Diff", PropertyName = "grTaxiOutDiff", Format = EnumFormatTypeExcel.DecimalNumber, Order = 10, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Chb PP", PropertyName = "grNum", Order = 2 },
new ExcelFormatTable() { Title = "Host", PropertyName = "grHost", Order = 11 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "guWComments", Order = 12 }
      };
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
    public static List<ExcelFormatTable> RptDepositsBurnedGuests()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Out Invit D", PropertyName = "guInvitD" },
new ExcelFormatTable() { Title = "Book Date", PropertyName = "guBookD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Guest", PropertyName = "Guest", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "guHotel", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "LS", PropertyName = "guls", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "SR", PropertyName = "gusr", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "PR", PropertyName = "guPRInvit1", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Out Invit", PropertyName = "guOutInvitNum", Axis = ePivotFieldAxis.Row, Order = 2},
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "gucu", Axis = ePivotFieldAxis.Column, Order = 1,Sort =eSortType.Ascending },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "gupt", Axis = ePivotFieldAxis.Column, Order = 2, Sort =eSortType.Descending },
new ExcelFormatTable() { Title = "Burned", PropertyName = "guDepositTwisted", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 10, AggregateFunction = DataFieldFunctions.Sum, Function = DataFieldFunctions.Sum }
      };
    }

    #endregion rptDepositsBurnedGuests

    #region rptDepositRefunds

    /// <summary>
    /// Formato para el reporte Deposit Refunds
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptDepositRefunds()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "Sales Room", PropertyName = "srN", Order = 1 },
new ExcelFormatTable() { Title = "Lead Source", PropertyName = "lsN", Order = 2 },
new ExcelFormatTable() { Title = "Refund ID", PropertyName = "drID", Order = 3 },
new ExcelFormatTable() { Title = "Refund Folio", PropertyName = "drFolio", Order = 4 },
new ExcelFormatTable() { Title = "Refund Date", PropertyName = "drD", Format = EnumFormatTypeExcel.Date, Order = 5 },
new ExcelFormatTable() { Title = "Reservation", PropertyName = "guHReservID", Order = 6 },
new ExcelFormatTable() { Title = "Out Invitation", PropertyName = "guOutInvitNum", Order = 7 },
new ExcelFormatTable() { Title = "Name", PropertyName = "GuestName", Order = 8 },
new ExcelFormatTable() { Title = "Total", PropertyName = "Total", Format = EnumFormatTypeExcel.DecimalNumber, Order = 9},
new ExcelFormatTable() { Title = "PR ID", PropertyName = "peID", Order = 10 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN", Order = 11 },
new ExcelFormatTable() { Title = "Amount", PropertyName = "bdAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 12 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "cuN", Order = 13 },
new ExcelFormatTable() { Title = "Card Type", PropertyName = "ccN", Order = 14 },

new ExcelFormatTable() { Title = "Card Number", PropertyName = "bdCardNum", Order = 15 },
new ExcelFormatTable() { Title = "Expiration Date", PropertyName = "bdExpD", Order = 16 },
new ExcelFormatTable() { Title = "Authorization", PropertyName = "bdAuth", Format = EnumFormatTypeExcel.Number, Order = 17 }
      };
    }

    #endregion rptDepositRefunds

    #region RptDepositByPr

    /// <summary>
    /// Formato para el reporte Deposit By PR
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptDepositByPr()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "guInvitD", PropertyName = "guInvitD" },
new ExcelFormatTable() { Title = "Book Date", PropertyName = "guBookD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Guest", PropertyName = "Guest", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "guHotel", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "LS", PropertyName = "guls", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "SR", PropertyName = "gusr", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "PR", PropertyName = "guPRInvit1", Axis = ePivotFieldAxis.Row, IsGroup = true, Order = 1 },
new ExcelFormatTable() { Title = "Out Invit", PropertyName = "guOutInvitNum", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN" },
new ExcelFormatTable() { Title = "Currrency", PropertyName = "gucu", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending},
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "gupt", Axis = ePivotFieldAxis.Column, Order = 2, Sort = eSortType.Descending},
new ExcelFormatTable() { Title = "Deposited", PropertyName = "guDeposit", Format= EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 9, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Received", PropertyName = "guDepositReceived", Format= EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 10, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Burned", PropertyName = "guDepositTwisted", Format= EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 11, SubTotalFunctions = eSubTotalFunctions.Sum }
      };
    }

    #endregion RptDepositByPr

    #region rptDepositsNoShow

    /// <summary>
    /// Formato para el reporte Deposits No Show
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptDepositsNoShow()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "guInvitD", PropertyName = "guInvitD" },
new ExcelFormatTable() { Title = "Book Date", PropertyName = "guBookD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Guest", PropertyName = "Guest", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "guHotel", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "LS", PropertyName = "guls", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "SR", PropertyName = "gusr", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "PR", PropertyName = "guPRInvit1", Axis = ePivotFieldAxis.Row, IsGroup = true, Order = 1 },
new ExcelFormatTable() { Title = "Out Invit", PropertyName = "guOutInvitNum", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN" },
new ExcelFormatTable() { Title = "Currrency", PropertyName = "gucu", Axis = ePivotFieldAxis.Column, Order = 1},
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "gupt", Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() { Title = "Deposited", PropertyName = "guDeposit", Format= EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 1, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Received", PropertyName = "guDepositReceived", Format= EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 2, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Burned", PropertyName = "guDepositTwisted", Format= EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 3, SubTotalFunctions = eSubTotalFunctions.Sum }
      };
    }

    #endregion rptDepositsNoShow

    #region rptInOutByPR

    /// <summary>
    /// Formato para el reporte In & Out By PR
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptInOutByPr()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "PR 1", PropertyName = "PR1", Order = 1, IsGroup =true },
new ExcelFormatTable() { Title = "PR 1 Name", PropertyName = "PR1N", IsVisible = false },
new ExcelFormatTable() { Title = "GUID", PropertyName = "GUID", Order = 1 },
new ExcelFormatTable() { Title = "Location", PropertyName = "Location", Order = 2 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "Hotel", Order = 3 },
new ExcelFormatTable() { Title = "Room", PropertyName = "Room", Order = 4 },
new ExcelFormatTable() { Title = "Pax", PropertyName = "Pax", Format = EnumFormatTypeExcel.DecimalNumber, Order = 5 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "LastName", Order = 6 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "FirstName", Order = 7},
new ExcelFormatTable() { Title = "Agency ID", PropertyName = "Agency", Order = 8 },
new ExcelFormatTable() { Title = "Agency", PropertyName = "AgencyN", Order = 9 },
new ExcelFormatTable() { Title = "Country ID", PropertyName = "Country", Order = 10 },
new ExcelFormatTable() { Title = "Country", PropertyName = "CountryN", Order = 11 },
new ExcelFormatTable() { Title = "Show D", PropertyName = "ShowDate", Format = EnumFormatTypeExcel.Date, Order = 12 },
new ExcelFormatTable() { Title = "Time In", PropertyName = "TimeIn", Format = EnumFormatTypeExcel.Time, Order = 13 },
new ExcelFormatTable() { Title = "Time Out", PropertyName = "TimeOut", Format = EnumFormatTypeExcel.Time, Order = 14 },
new ExcelFormatTable() { Title = "Direct", PropertyName = "Direct", SubTotalFunctions = eSubTotalFunctions.Sum, Order = 15 },
new ExcelFormatTable() { Title = "Tour", PropertyName = "Tour", SubTotalFunctions = eSubTotalFunctions.Count, Order = 16 },
new ExcelFormatTable() { Title = "In Out", PropertyName = "InOut", SubTotalFunctions = eSubTotalFunctions.Count, Order = 17 },
new ExcelFormatTable() { Title = "Walk Out", PropertyName = "WalkOut", SubTotalFunctions = eSubTotalFunctions.Count, Order = 18 },
new ExcelFormatTable() { Title = "Courtesy Tour", PropertyName = "CourtesyTour", SubTotalFunctions = eSubTotalFunctions.Count, Order = 19 },
new ExcelFormatTable() { Title = "Save Program", PropertyName = "SaveProgram", SubTotalFunctions = eSubTotalFunctions.Count, Order = 20 },
new ExcelFormatTable() { Title = "PR 2", PropertyName = "PR2", Order = 21 },
new ExcelFormatTable() { Title = "PR2 Name", PropertyName = "PR2N", Order = 22 },
new ExcelFormatTable() { Title = "PR 3", PropertyName = "PR3", Order = 23 },
new ExcelFormatTable() { Title = "PR3 Name", PropertyName = "PR3N", Order = 24 },
new ExcelFormatTable() { Title = "Host", PropertyName = "Host", Order = 25 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "Comments", Order = 26 }
      };
    }

    #endregion rptInOutByPR

    #region RptPaidDepositsByPR

    /// <summary>
    /// Formato para el reporte Paid Deposits
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptPaidDepositsByPr()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Ch B", PropertyName = "grID", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Chb PP", PropertyName = "grNum", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Date", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Guest ID", PropertyName = "grgu", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Book D", PropertyName = "guBookD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Guest Name", PropertyName = "grGuest", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "grHotel", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "LS", PropertyName = "grls", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "SR", PropertyName = "grsr", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "PR", PropertyName = "grpe", Axis = ePivotFieldAxis.Row, Order = 1, IsGroup = true},
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN"},
new ExcelFormatTable() { Title = "Host", PropertyName = "grHost", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "cuN", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "ptN", Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() { Title = "Deposit", PropertyName = "grDeposit", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 12, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
        new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 13, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum }
      };
    }

    #endregion RptPaidDepositsByPR

    #region RptPersonnelAccess

    /// <summary>
    /// Formato para el reporte Personnel Access
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>✓
    public static List<ExcelFormatTable> RptPersonnelAccess()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "lsN", PropertyName = "lsN", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "peps", PropertyName = "peps", Order = 2, IsGroup = true },
new ExcelFormatTable() { Title = "ID", PropertyName = "peID", Order = 1 },
new ExcelFormatTable() { Title = "Name", PropertyName = "peN", Order = 2 },
new ExcelFormatTable() { Title = "Captain", PropertyName = "peCaptain", Order = 3 },
new ExcelFormatTable() { Title = "PR", PropertyName = "PR", Order = 4 },
new ExcelFormatTable() { Title = "PR Mem", PropertyName = "PRMembers", Order = 5 },
new ExcelFormatTable() { Title = "Liner", PropertyName = "Liner", Order = 6 },
new ExcelFormatTable() { Title = "Closer", PropertyName = "Closer", Order = 7 },
new ExcelFormatTable() { Title = "Exit", PropertyName = "Exit", Order = 8 },
new ExcelFormatTable() { Title = "Podium", PropertyName = "Podium", Order = 9 },
new ExcelFormatTable() { Title = "PR Capt", PropertyName = "PRCaptain", Order = 10 },
new ExcelFormatTable() { Title = "PR Sup", PropertyName = "PRSupervisor", Order = 11 },
new ExcelFormatTable() { Title = "Ln Capt", PropertyName = "LinerCaptain", Order = 12 },
new ExcelFormatTable() { Title = "Clo Capt", PropertyName = "CloserCaptain", Order = 13 },
new ExcelFormatTable() { Title = "Entry H", PropertyName = "EntryHost", Order = 14 },
new ExcelFormatTable() { Title = "Gifts H", PropertyName = "GiftsHost", Order = 15 },
new ExcelFormatTable() { Title = "Exit H", PropertyName = "ExitHost", Order = 16 },
new ExcelFormatTable() { Title = "VLO", PropertyName = "VLO", Order = 17 },
new ExcelFormatTable() { Title = "Manager", PropertyName = "Manager", Order = 18 },
new ExcelFormatTable() { Title = "Admin", PropertyName = "Administrator", Order = 19 }
      };
    }

    #endregion RptPersonnelAccess

    #region RptSelfGen

    /// <summary>
    /// Formato para el reporte Self Gen
    /// </summary>
    /// <history>
    /// [edgrodriguez] 25/Abr/2016 Created
    /// </history>✓
    public static List<ExcelFormatTable> RptSelfGen()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "ID", PropertyName = "PRID", Order = 1 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "PRN", Order = 2 },
new ExcelFormatTable() { Title = "Peps", PropertyName = "PRps", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "Sh", PropertyName = "Shows", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 3 },
new ExcelFormatTable() { Title = "IO", PropertyName = "IO", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 4 },
new ExcelFormatTable() { Title = "WO", PropertyName = "WO", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 5 },
new ExcelFormatTable() { Title = "Proc", PropertyName = "Proc", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 6 },
new ExcelFormatTable() { Title = "Proc Vol", PropertyName = "ProcAmount", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 7 },
new ExcelFormatTable() { Title = "OOP", PropertyName = "OOP", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 8 },
new ExcelFormatTable() { Title = "OOP Amount", PropertyName = "OOPAmount", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 9 },
new ExcelFormatTable() { Title = "Cancel", PropertyName = "Cancel", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 10 },
new ExcelFormatTable() { Title = "Cancel Amount", PropertyName = "CancelAmount", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 11 },
new ExcelFormatTable() { Title = "C %", PropertyName = "CancelF", Format = EnumFormatTypeExcel.Percent, IsCalculated = true, Formula = "IF([Subtotal]=0,IF([CancelAmount]=0,0,1),[CancelAmount]/[Subtotal])", Order = 12 },
new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal", Format = EnumFormatTypeExcel.DecimalNumber, IsCalculated = true, Formula = "[ProcAmount]+[OOPAmount]", Order = 13 },
new ExcelFormatTable() { Title = "Total", PropertyName = "TotalProc", Format = EnumFormatTypeExcel.Number, IsCalculated = true, Formula = "[Proc]+[OOP]-[Cancel]", Order = 14 },
new ExcelFormatTable() { Title = "Total Amount", PropertyName = "TotalProcAmount", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 15 },
new ExcelFormatTable() { Title = "Efficiency", PropertyName = "Eff", Format = EnumFormatTypeExcel.DecimalNumber, IsCalculated = true, Formula = "IF([Shows]=0,0,[TotalProcAmount]/[Shows])", Order = 16 },
new ExcelFormatTable() { Title = "CI %", PropertyName = "CI", Format = EnumFormatTypeExcel.Percent, IsCalculated = true, Formula = "IF([Shows]=0,0,[TotalProc]/[Shows])", Order = 17 },
new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AvgSale", Format =  EnumFormatTypeExcel.DecimalNumber, IsCalculated = true, Formula = "IF([TotalProc]=0,0,[TotalProcAmount]/[TotalProc])", Order = 18 },
new ExcelFormatTable() { Title = "Pending", PropertyName = "Pending", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 19 },
new ExcelFormatTable() { Title = "Pending Amount", PropertyName = "PendingAmount", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 20 }
      };
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
    public static List<ExcelFormatTable> RptAgencies()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Agency" },
new ExcelFormatTable() { Title = "Agency Name" },
new ExcelFormatTable() { Title = "Unavailable Motive" },
new ExcelFormatTable() { Title = "Market" },
new ExcelFormatTable() { Title = "Show Pay" },
new ExcelFormatTable() { Title = "Sale Pay" },
new ExcelFormatTable() { Title = "Rep",  },
new ExcelFormatTable() { Title = "Active", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center }
      };
    }

    #endregion rptAgencies

    #region rptGifts

    /// <summary>
    /// Formato para el reporte general de Gifts
    /// </summary>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGifts()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "Gift ID" },
new ExcelFormatTable() { Title = "Gift Name" },
new ExcelFormatTable() { Title = "Short N." },
new ExcelFormatTable() { Title = "Order", Format = EnumFormatTypeExcel.Number },
new ExcelFormatTable() { Title = "Price", Format = EnumFormatTypeExcel.Currency },
new ExcelFormatTable() { Title = "Price Min.", Format = EnumFormatTypeExcel.Currency },
new ExcelFormatTable() { Title = "CXC", Format = EnumFormatTypeExcel.Currency },
new ExcelFormatTable() { Title = "CXC Min.", Format = EnumFormatTypeExcel.Currency },
new ExcelFormatTable() { Title = "Package", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Category" },
new ExcelFormatTable() { Title = "Inv.", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Folio", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Pax", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Active", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center }
      };
    }

    #endregion rptGifts

    #region RptGiftsKardex

    /// <summary>
    /// Formato para el reporte general de Gifts
    /// </summary>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGiftsKardex()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "Date", PropertyName = "MovD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "MovGi", PropertyName = "MovGi", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "MovGiN", PropertyName = "MovGiN", Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() { Title = "In", PropertyName = "InQty", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Out", PropertyName = "OutQty", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Stock.", PropertyName = "InvQty", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3 }
      };
    }

    #endregion RptGiftsKardex

    #region RptLoginsLog

    /// <summary>
    /// Formato para el reporte general Logins Log
    /// </summary>
    /// <history>
    /// [edgrodriguez] 27/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptLoginsLog()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "Date Time", Format = EnumFormatTypeExcel.Date },
new ExcelFormatTable() { Title = "Location" },
new ExcelFormatTable() { Title = "Code" },
new ExcelFormatTable() { Title = "Name" },
new ExcelFormatTable() { Title = "PC" }
      };
    }

    #endregion RptLoginsLog

    #region rptPersonnel

    /// <summary>
    /// Formato para el reporte general de Personnel
    /// </summary>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptPersonnel()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Status", Order = 1 },
new ExcelFormatTable() { Title = "Dept", Order = 2 },
new ExcelFormatTable() { Title = "Post", Order = 3 },
new ExcelFormatTable() { Title = "Place", Order = 4 },
new ExcelFormatTable() { Title = "ID", Order = 5 },
new ExcelFormatTable() { Title = "Name", Order = 6 },
new ExcelFormatTable() { Title = "Active", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 7 },
new ExcelFormatTable() { Title = "Collaborator", Format = EnumFormatTypeExcel.Number, Order = 8 },
new ExcelFormatTable() { Title = "Captain", Order = 9 },
new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 10 },
new ExcelFormatTable() { Title = "PR Mem", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 11 },
new ExcelFormatTable() { Title = "Liner", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 12 },
new ExcelFormatTable() { Title = "Closer", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 13 },
new ExcelFormatTable() { Title = "Exit", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 14 },
new ExcelFormatTable() { Title = "Podium", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 15 },
new ExcelFormatTable() { Title = "PR Capt", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 16 },
new ExcelFormatTable() { Title = "PR Sup", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 17 },
new ExcelFormatTable() { Title = "Ln Capt", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 18 },
new ExcelFormatTable() { Title = "Clo Capt", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 19 },
new ExcelFormatTable() { Title = "Entry H", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 20 },
new ExcelFormatTable() { Title = "Gift H", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 21 },
new ExcelFormatTable() { Title = "Exit H", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 22 },
new ExcelFormatTable() { Title = "VLO", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 23 },
new ExcelFormatTable() { Title = "Manager", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 24 },
new ExcelFormatTable() { Title = "Admin", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 25 }
      };
    }

    #endregion rptPersonnel

    #region rptProductionByLeadSourceMarketMonthly

    /// <summary>
    /// Formato para el reporte general Production By Lead Source & Market(Monthly)
    /// </summary>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptProductionByLeadSourceMarketMonthly()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Year", PropertyName = "Year", Order = 2, IsGroup = true },
new ExcelFormatTable() { Title = "Month",  PropertyName = "Month", IsVisible = false },
new ExcelFormatTable() { Title = "MonthN",  PropertyName = "MonthN", Order = 3, IsGroup = true },
new ExcelFormatTable() { Title = "Market Total",  PropertyName = "MarketTotal", IsVisible = false },
new ExcelFormatTable() { Title = "Market",  PropertyName = "Market", Order = 4, IsGroup = true },
new ExcelFormatTable() { Title = "Lead Source",  PropertyName = "LeadSource", Order = 1 },
new ExcelFormatTable() { Title = "Program",  PropertyName = "Program", Order = 1, IsGroup = true },
new ExcelFormatTable() { Title = "Arrivals",  PropertyName = "Arrivals", Format = EnumFormatTypeExcel.Number, Order = 2, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Contacts",  PropertyName = "Contacts", Format = EnumFormatTypeExcel.Number, Order = 3, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Contacts %",  PropertyName = "ContactsFactor", Format = EnumFormatTypeExcel.Percent, Order = 4, IsCalculated = true, Formula = "IF([Arrivals]=0,0,[Contacts]/[Arrivals])" },
new ExcelFormatTable() { Title = "Availables",  PropertyName = "Availables", Format = EnumFormatTypeExcel.Number, Order = 5, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Availables %",  PropertyName = "AvailablesFactor", Format = EnumFormatTypeExcel.Percent, Order = 6, IsCalculated = true, Formula = "IF([Contacts]=0,0,[Availables]/[Contacts])" },
new ExcelFormatTable() { Title = "Books",  PropertyName = "GrossBooks", Format = EnumFormatTypeExcel.Number, Order = 7, SubTotalFunctions = eSubTotalFunctions.Sum},
new ExcelFormatTable() { Title = "T Books",  PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Order = 9, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Books %",  PropertyName = "BooksFactor", Format = EnumFormatTypeExcel.Percent, Order = 10, IsCalculated = true, Formula = "IF([Availables]=0,0,[Books]/[Availables])" },
new ExcelFormatTable() { Title = "Shows",  PropertyName = "GrossShows", Format = EnumFormatTypeExcel.Number, Order = 11, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "T Shows",  PropertyName = "Shows", Format = EnumFormatTypeExcel.Number, Order = 12, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Shows %",  PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Order = 13, IsCalculated = true, Formula = "IF([GrossBooks]=0,0,[GrossShows]/[GrossBooks])" },
new ExcelFormatTable() { Title = "Shows Arrivals %",  PropertyName = "ShowsArrivalsFactor", Format = EnumFormatTypeExcel.Percent, Order = 14, IsCalculated = true, Formula = "IF([Arrivals]=0,0,[Shows]/[Arrivals])" },
new ExcelFormatTable() { Title = "Sales",  PropertyName = "Sales", Format = EnumFormatTypeExcel.Number, Order = 22, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Amount",  PropertyName = "SalesAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 23, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Closing %",  PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Order = 24, IsCalculated = true, Formula = "IF([Shows]=0,0,[Sales]/[Shows])" },
new ExcelFormatTable() { Title = "Efficiency",  PropertyName = "Efficiency", Format = EnumFormatTypeExcel.DecimalNumber, Order = 25, IsCalculated = true, Formula = "IF([Shows]=0,0,[SalesAmount]/[Shows])" },
new ExcelFormatTable() { Title = "Directs",  PropertyName = "Directs", Format = EnumFormatTypeExcel.Number, Order = 8, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "IO",  PropertyName = "InOuts", Format = EnumFormatTypeExcel.Number, Order = 15, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "WO",  PropertyName = "WalkOuts", Format = EnumFormatTypeExcel.Number, Order = 16, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "RT", PropertyName = "Tours", Format = EnumFormatTypeExcel.Number, Order = 17, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "CT", PropertyName = "CourtesyTours", Format = EnumFormatTypeExcel.Number, Order = 18, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Save", PropertyName = "SaveTours", Format = EnumFormatTypeExcel.Number, Order = 19, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "T Tours", PropertyName = "TotalTours", Format = EnumFormatTypeExcel.Number , Order = 20, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "UPS", PropertyName = "UPS", Format = EnumFormatTypeExcel.Number, Order = 21, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.DecimalNumber, Order = 26, IsCalculated = true, Formula = "IF([Sales]=0,0,[SalesAmount]/[Sales])" }
      };
    }

    #endregion rptProductionByLeadSourceMarketMonthly

    #region rptProductionReferral

    /// <summary>
    /// Formato para el reporte Production Referral
    /// </summary>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptProductionReferral()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Month", PropertyName = "MonthN", Order = 1 },
new ExcelFormatTable() { Title = "MonthID",  PropertyName = "Month", IsVisible = false },
new ExcelFormatTable() { Title = "Year",  PropertyName = "Year", Order = 1 , IsGroup = true },
new ExcelFormatTable() { Title = "Arrivals",  PropertyName = "Arrivals", Format = EnumFormatTypeExcel.Number, Order = 2, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Shows",  PropertyName = "Shows", Format = EnumFormatTypeExcel.Number, Order = 3, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Shows %",  PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Order = 4, IsCalculated = true, Formula = "IF([Arrivals]=0,0,[Shows]/[Arrivals])" },
new ExcelFormatTable() { Title = "Sales",  PropertyName = "Sales", Format = EnumFormatTypeExcel.Number, Order = 5, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Vol",  PropertyName = "SalesAmount", Format = EnumFormatTypeExcel.DecimalNumber, Order = 6, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Closing %",  PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Order = 7, Formula = "IF([Shows]=0,0,[Sales]/[Shows])" },
new ExcelFormatTable() { Title = "Efficiency",  PropertyName = "Efficiency", Format = EnumFormatTypeExcel.DecimalNumber, Order = 8, Formula = "IF([Shows]=0,0,[SalesAmount]/[Shows])" },
new ExcelFormatTable() { Title = "Avg Sale",  PropertyName = "AverageSale", Format = EnumFormatTypeExcel.DecimalNumber, Order = 9, Formula = "IF([Sales]=0,0,[SalesAmount]/[Sales])" }
      };
    }

    #endregion rptProductionReferral
    
    #region RptReps

    /// <summary>
    /// Formato para el reporte Reps
    /// </summary>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptReps()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() {Title = "Rep ID", PropertyName = "rpID", Axis = ePivotFieldAxis.Row, Order = 1 },
        new ExcelFormatTable() {Title = "Active", PropertyName = "rpA", Format = EnumFormatTypeExcel.Boolean, Axis = ePivotFieldAxis.Row, Order = 2 }
      };
    }

    #endregion RptReps

    #region RptSalesByProgramLeadSourceMarket

    /// <summary>
    /// Formato para el reporte Sales By Program, Leadsource & Market
    /// </summary>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptSalesByProgramLeadSourceMarket()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() {Title = "Program", PropertyName = "Program", IsGroup = true, Order = 1 },
        new ExcelFormatTable() {Title = "Lead Source", PropertyName = "LeadSource", IsGroup  = true, Order = 2 },
        new ExcelFormatTable() {Title = "Market", PropertyName = "Market", IsGroup = true, Order = 3 },
        new ExcelFormatTable() {Title = "GUID", PropertyName = "GuestID", Order = 1 },
        new ExcelFormatTable() {Title = "Last Name", PropertyName = "LastName", Order = 2 },
        new ExcelFormatTable() {Title = "FirstName", PropertyName = "FirstName", Order = 3 },
        new ExcelFormatTable() {Title = "Membership", PropertyName = "Membership", Order = 4 },
        new ExcelFormatTable() {Title = "Date", PropertyName = "SaleDate", Format = EnumFormatTypeExcel.Date, Order = 5 },
        new ExcelFormatTable() {Title = "Proc", PropertyName = "Procesable", SubTotalFunctions = eSubTotalFunctions.Count, Order = 6 },
        new ExcelFormatTable() {Title = "Proc Date", PropertyName = "SaleProcesableDate", Format = EnumFormatTypeExcel.Date, Order = 7 },
        new ExcelFormatTable() {Title = "Cancel", PropertyName = "Cancel", SubTotalFunctions = eSubTotalFunctions.Count, Order = 8 },
        new ExcelFormatTable() {Title = "Cancel Date", PropertyName = "CancelDate", Format = EnumFormatTypeExcel.Date, Order = 9 }
      };
    }

    #endregion RptSalesByProgramLeadSourceMarket

    #region RptWarehouseMovements

    /// <summary>
    /// Formato para el reporte Warehouse Movements
    /// </summary>
    /// <history>
    /// [edgrodriguez] 26/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptWarehouseMovements()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() {Title = "Date", PropertyName = "wmD", Axis = ePivotFieldAxis.Row, Order = 1 },
        new ExcelFormatTable() {Title = "Qty", PropertyName = "wmQty", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 2 },
        new ExcelFormatTable() {Title = "Gift", PropertyName = "giN", Axis = ePivotFieldAxis.Row, Order = 3 },
        new ExcelFormatTable() {Title = "Code", PropertyName = "wmpe", Axis = ePivotFieldAxis.Row, Order = 4 },
        new ExcelFormatTable() {Title = "User", PropertyName = "peN", Axis = ePivotFieldAxis.Row, Order = 5 },
        new ExcelFormatTable() {Title = "Comments", PropertyName = "wmComments", Axis = ePivotFieldAxis.Row, Order = 6 }
      };
    }

    #endregion RptWarehouseMovements



    #endregion General Reports
  }
}
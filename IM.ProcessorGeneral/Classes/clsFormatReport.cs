﻿using IM.Model.Classes;
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
new ExcelFormatTable() { Title = "Sales Room", Axis = ePivotFieldAxis.Row, Order = 1, Sort = eSortType.Ascending, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Program", Axis = ePivotFieldAxis.Row, Order = 2, Sort = eSortType.Ascending, SubTotalFunctions = eSubTotalFunctions.Sum },
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
new ExcelFormatTable() { Title = "Sales Room", Axis = ePivotFieldAxis.Row, Order = 1, Sort = eSortType.Ascending, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Program", Axis = ePivotFieldAxis.Row, Order = 2, Sort = eSortType.Ascending, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Lead Source", Axis = ePivotFieldAxis.Row, Order = 3, Sort = eSortType.Ascending, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Book Type", Axis = ePivotFieldAxis.Row, Order = 4, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Time", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1 }
      };
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
    public static List<ExcelFormatTable> RptCxc()
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

    #region RptCxcByType

    /// <summary>
    /// Formato para el reporte CxC
    /// </summary>
    /// <history>
    /// [edgrodriguez] 17/May/2016 Created
    /// </history>
    public static ExcelFormatItemsList RptCxcByType()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("grpe", "grpe", isGroup: true, isVisible: false);
      lst.Add("Receipt Date", "grD", format: EnumFormatTypeExcel.Date);
      lst.Add("Receipt ID", "grID");
      lst.Add("Folio", "grNum");
      lst.Add("SR", "grsr");
      lst.Add("Lead Source", "grls");
      lst.Add("PR", "peID");
      lst.Add("PR Name", "peN");
      lst.Add("Description", "Comments");
      lst.Add("CxC", "CxC", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Exchange Rate", "exExchRate", format: EnumFormatTypeExcel.Currency);
      lst.Add("CxC Pesos", "CxCP", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Total CxC", "TotalCxC", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Amount to Pay (USD)", "ToPayUS", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Amount To Pay (MXN)", "ToPayMN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Amount Paid (USD)", "PaidUS", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Amount Paid (MXN)", "PaidMN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Balance (USD)", "BalanceUS", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Balance (MXN)", "BalanceMN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("CxC Comments", "GiftComments");
      lst.Add("Guest ID", "guID");
      lst.Add("Reservation", "guHReservID");
      lst.Add("Out Invitation", "guOutInvitNum");
      lst.Add("Authorized", "AuthSts");
      lst.Add("Authorization Date", "grCxCAppD", format: EnumFormatTypeExcel.Date);
      lst.Add("Authorized By", "grAuthorizedBy");
      lst.Add("Authorized Name", "AuthName");
      lst.Add("CxC Authorization Comments", "CxCComments");
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
    public static ExcelFormatItemsList RptCxcNotAuthorized()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptCxcPayments()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
new ExcelFormatTable() { Title = "Deposited", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Received", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Burned", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "CxC", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 4, Function = DataFieldFunctions.Sum }
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
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "Member #", PropertyName = "memberNum", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Processable", PropertyName = "procAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 13, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Pending", PropertyName = "pendAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 14, Function = DataFieldFunctions.Sum  }
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
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "Member #", PropertyName = "memberNum", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Processable", PropertyName = "procAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 13, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Pending", PropertyName = "pendAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 14, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum  }
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
new ExcelFormatTable() { Title = "Deposit", PropertyName = "grDeposit", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum }
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
new ExcelFormatTable() { Title = "Deposit", PropertyName = "grDeposit", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 9 },
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.DecimalNumberWithCero, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order =  10 },
new ExcelFormatTable() { Title = "grcu", PropertyName = "grcu"},
new ExcelFormatTable() { Title = "grpt", PropertyName = "grpt"},
new ExcelFormatTable() { Title = "Taxi", PropertyName = "grTaxiOut", Format = EnumFormatTypeExcel.DecimalNumberWithCero, Order = 11 },
new ExcelFormatTable() { Title = "Quantity", PropertyName = "geQty", Format = EnumFormatTypeExcel.Number, Order = 12 },
new ExcelFormatTable() { Title = "Folios", PropertyName = "geFolios", Order = 14},
new ExcelFormatTable() { Title = "Cost", PropertyName = "Cost", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 13 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "grComments", Axis = ePivotFieldAxis.Row, Order = 15 },
new ExcelFormatTable() { Title = "Gift", PropertyName = "Gift" },
new ExcelFormatTable() { Title = "GiftN", PropertyName = "GiftN" }
//TODO: Agregar Columna Total
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
new ExcelFormatTable() { Title = "Paid", PropertyName = "Paid", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Refund", PropertyName = "Refund", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
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
new ExcelFormatTable() { Title = "Pax", PropertyName = "Pax", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 6, Function = DataFieldFunctions.Sum },
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
new ExcelFormatTable() { Title = "Total", PropertyName = "TotalCost", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, Order = 25 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "Comments", Axis = ePivotFieldAxis.Row, Order = 26 }
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
new ExcelFormatTable() { Title = "Pax", PropertyName = "Pax", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 6, Function = DataFieldFunctions.Sum },
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

new ExcelFormatTable() { Title = "With Cost", PropertyName = "WithCost", Format = EnumFormatTypeExcel.Boolean, Order = 25 },
new ExcelFormatTable() { Title = "Public Price", PropertyName = "PublicPrice", Format = EnumFormatTypeExcel.Currency, Order = 26 },
new ExcelFormatTable() { Title = "Deposited", PropertyName = "Deposited", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum, Order = 19 },
new ExcelFormatTable() { Title = "Burned", PropertyName = "Burned", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum, Order = 20 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "Currency"  },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "PaymentType" },
new ExcelFormatTable() { Title = "Taxi Out", PropertyName = "TaxiOut", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Function = DataFieldFunctions.Sum, Order = 21, },
new ExcelFormatTable() { Title = "Taxi Out Diff", PropertyName = "TaxiOutDiff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Function = DataFieldFunctions.Sum, Order = 22 },
new ExcelFormatTable() { Title = "Gift", PropertyName = "Gift" },
new ExcelFormatTable() { Title = "GiftN", PropertyName = "GiftN" },
new ExcelFormatTable() { Title = "Quantity", PropertyName = "Quantity", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum, Order = 23 },
new ExcelFormatTable() { Title = "Adults", PropertyName = "Adults", IsVisible = false },
new ExcelFormatTable() { Title = "Minors", PropertyName = "Minors", IsVisible = false },
new ExcelFormatTable() { Title = "Folios", PropertyName = "Folios", Order = 27 },
new ExcelFormatTable() { Title = "Cost", PropertyName = "Cost", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, Order = 24 },
new ExcelFormatTable() { Title = "Total", PropertyName = "TotalCost", Format = EnumFormatTypeExcel.Currency, Function = DataFieldFunctions.Sum, Order = 28 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "Comments", Axis = ePivotFieldAxis.Row, Order = 29 }
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
new ExcelFormatTable() { Title = "Amount", PropertyName = "Amount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 1, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Amount US", PropertyName = "AmountUS", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 2, SubTotalFunctions = eSubTotalFunctions.Sum },
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
    public static ExcelFormatItemsList RptGiftsSale()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
      lst.Add("Amount", "Amount", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, AggregateFormat: DataFieldFunctions.Sum);
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
    public static ExcelFormatItemsList RptGiftsUsedBySistur()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptInOut()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptManifestRange()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptGuestNoShow()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
new ExcelFormatTable() { Title = "No", PropertyName = "meID", Axis = ePivotFieldAxis.Row, Order = 4  },
new ExcelFormatTable() { Title = "Date", PropertyName = "meD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Guest ID", PropertyName = "megu", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Qty", PropertyName = "meQty", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "Type", PropertyName = "myN", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Adults", PropertyName = "meAdults", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2 },
new ExcelFormatTable() { Title = "Minors", PropertyName = "meMinors", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3 },
new ExcelFormatTable() { Title = "Total", PropertyName = "Total", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 4},
new ExcelFormatTable() { Title = "Folios", PropertyName = "meFolios", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "meComments", Axis = ePivotFieldAxis.Row, Order = 16 },
new ExcelFormatTable() { Title = "Guest Name", PropertyName = "guLastName1", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Loc", PropertyName = "guloInfo", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Host", PropertyName = "guEntryHost", Axis = ePivotFieldAxis.Row, Order = 3, Compact = true, Outline = true  },
new ExcelFormatTable() { Title = "Show", PropertyName = "guShow", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Host Name", PropertyName = "guEntryHostN", Axis = ePivotFieldAxis.Row, Order = 2, Compact = true, Outline = true },
new ExcelFormatTable() { Title = "PR", PropertyName = "guPRInvit1", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "guPRInvit1N", Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Liner", PropertyName = "guLiner1", Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() { Title = "Liner Name", PropertyName = "guLiner1N", Axis = ePivotFieldAxis.Row, Order = 15 },
new ExcelFormatTable() { Title = "mera", PropertyName = "mera" },
new ExcelFormatTable() { Title = "Rate Type", PropertyName = "RateTypeN", Axis = ePivotFieldAxis.Row, Order = 1 },
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
    public static ExcelFormatItemsList RptMemberships()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptMembershipsByAgencyMarket()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptMembershipsByHost()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptProductionBySalesRoom()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptProductionBySalesRoomMarket()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptProductionBySalesRoomMarketSubMarket()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptProductionByShowProgram()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptProductionByShowProgramProgram()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptCloserStatistic()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptLinerStatistic()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptWeeklyMonthlyHostessByPr()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("LS", "guls", isGroup: true, isVisible: false);
      lst.Add("PR Name", "guPRInvitN", axis: ePivotFieldAxis.Row);
      lst.Add("PR", "guPRInvit", axis: ePivotFieldAxis.Row);
      lst.Add("D", "guDirect", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("IO", "guInOut", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("WO", "guWalkOut", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("Type", "Type", axis: ePivotFieldAxis.Column);
      lst.Add("guloInvit", "guloInvit", isVisible: false);
      lst.Add("gusr", "gusr", isVisible: false);
      return lst;
    }

    public static ExcelFormatItemsList RptWeeklyMonthlyHostessByTourTime()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("LS", "guls", isGroup: true, isVisible: false);
      lst.Add("Day", "guD", format: EnumFormatTypeExcel.Day, axis: ePivotFieldAxis.Row);
      lst.Add("B", "guBook", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("Sh", "guShow", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, formula: "IF([guBook]=0,0,[guShow]/[guBook])");
      lst.Add("D", "guDirect", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
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
    public static ExcelFormatItemsList RptTaxisIn()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptTaxisOut()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static List<ExcelFormatTable> RptDepositsBurnedGuests()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Out Invit D", PropertyName = "guInvitD", Format = EnumFormatTypeExcel.Date },
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
new ExcelFormatTable() { Title = "Burned", PropertyName = "guDepositTwisted", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, AggregateFunction = DataFieldFunctions.Sum, Function = DataFieldFunctions.Sum }
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
    public static ExcelFormatItemsList RptDepositRefunds()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static List<ExcelFormatTable> RptDepositsNoShow()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Invit D", PropertyName = "guInvitD", Format = EnumFormatTypeExcel.Date },
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
new ExcelFormatTable() { Title = "Deposited", PropertyName = "guDeposit", Format= EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 1, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Received", PropertyName = "guDepositReceived", Format= EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 2, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Burned", PropertyName = "guDepositTwisted", Format= EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 3, SubTotalFunctions = eSubTotalFunctions.Sum }
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
    public static ExcelFormatItemsList RptInOutByPr()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
new ExcelFormatTable() { Title = "Deposit", PropertyName = "grDeposit", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
        new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 13, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum }
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
    public static ExcelFormatItemsList RptPersonnelAccess()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("lsN", "lsN", isGroup: true);
      lst.Add("peps", "peps", isGroup: true);
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
    public static ExcelFormatItemsList RptSelfGen()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
    public static ExcelFormatItemsList RptProductionByLeadSourceMarketMonthly()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
      lst.Add("RT","Tours", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
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
    public static ExcelFormatItemsList RptProductionReferral()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Year", "Year", isGroup: true, isVisible:false);
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
    public static ExcelFormatItemsList RptSalesByProgramLeadSourceMarket()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
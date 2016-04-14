using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using System.Windows;
using System.Data;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;

namespace IM.ProcessorGeneral.Classes
{
  public static class clsFormatReport
  {
    #region Reports by Sales Room

    #region Bookings

    #region rptBookingsBySalesRoomProgramTime
    /// <summary>
    /// Formato para el reporte Bookings By Sales Room, Program & Time
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptBookingsBySalesRoomProgramTime()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Sales Room", Axis = ePivotFieldAxis.Row, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Program", Axis = ePivotFieldAxis.Row, Order = 2, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Book Type", Axis = ePivotFieldAxis.Row, Order = 3, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title="Time", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1 }
      };
    }
    #endregion

    #region rptBookingsBySalesRoomProgramLeadSourceTime
    /// <summary>
    /// Formato para el reporte Bookings By Sales Room, Program, Lead Sources & Time
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptBookingsBySalesRoomProgramLeadSourceTime()
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
    #endregion

    #endregion

    #region CxC

    #region rptCxC

    /// <summary>
    /// Formato para el reporte CxC
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 22/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptCxC()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Group", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "PR", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Chb", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "PP", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Rcpt", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Date", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Guest ID", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 7 },
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

    #endregion

    #region rptCxCDeposits
    /// <summary>
    /// Formato para el reporte CxC Deposits
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 23/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptCxCDeposits()
    {
      return new List<ExcelFormatTable>
      {
new ExcelFormatTable() { Title = "Ch B", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Chb PP", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Date", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "LS", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "GUID", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 5 },
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
    #endregion

    #region rptCxCGifts
    /// <summary>
    /// Formato para el reporte CxCGifts
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 29/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptCxCGifts()
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
new ExcelFormatTable() {  Title="Quantity" ,PropertyName = "Quantity", Format = EnumFormatTypeExcel.Number,Function = DataFieldFunctions.Sum, Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() {  Title="Adults" ,PropertyName = "Adults", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() {  Title="Minors" ,PropertyName = "Minors", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() {  Title="Folios" ,PropertyName = "Folios", Axis = ePivotFieldAxis.Values, Order = 3 },
new ExcelFormatTable() {  Title="Cost" ,PropertyName = "Cost", Format = EnumFormatTypeExcel.Currency,Function= DataFieldFunctions.Sum, Axis = ePivotFieldAxis.Values, Order = 2 },
new ExcelFormatTable() {  Title="Rcpt Date" ,PropertyName = "grD", Format=EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 12},
new ExcelFormatTable() {  Title="USD" ,PropertyName = "CostUS", Format = EnumFormatTypeExcel.Currency,Function = DataFieldFunctions.Sum, Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() {  Title="Exch. Rate" ,PropertyName = "exExchRate", Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() {  Title="MX" ,PropertyName = "CostMX", Format=EnumFormatTypeExcel.Currency,Function = DataFieldFunctions.Sum, Axis = ePivotFieldAxis.Row, Order = 15 },
new ExcelFormatTable() {  Title="Member #" ,PropertyName = "grMemberNum", Axis = ePivotFieldAxis.Row, Order = 16 },
new ExcelFormatTable() {  Title="Comments"  ,PropertyName = "grCxCComments", Axis = ePivotFieldAxis.Row, Order = 17 }
      };
    }
    #endregion

    #region rptCxCNotAuthorized

    /// <summary>
    /// Formato para el reporte CxC
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 01/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptCxCNotAuthorized()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Rcpt", PropertyName = "grID", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Chb PP", PropertyName = "grNum", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "LS", PropertyName = "grls", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "GUID", PropertyName = "grgu", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Guest Name", PropertyName = "grGuest", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Rcpt Date", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "PR", PropertyName = "grpe", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "CxC Gifts", PropertyName = "grCxCGifts", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "CxC Dep", PropertyName = "grCxCPRDeposit", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "CxC Taxi", PropertyName = "grCxCTaxiOut", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "CxC", PropertyName = "CxC", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 12 }
      };
    }

    #endregion

    #endregion

    #region Deposits

    #region rptDeposits

    /// <summary>
    /// Formato para el reporte CxC
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 01/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptDeposits()
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
new ExcelFormatTable() { Title = "Show", Format = EnumFormatTypeExcel.Boolean, Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Currency", Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Payment Type", Axis = ePivotFieldAxis.Column, Order = 2, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Deposited", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Received", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Burned", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "CxC", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 4, Function = DataFieldFunctions.Sum }
      };
    }

    #endregion

    #region rptBurnedDeposits

    /// <summary>
    /// Formato para el reporte Burned Deposits
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptBurnedDeposits()
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
new ExcelFormatTable() { Title = "Comments", PropertyName = "grComments", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "cuN", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "ptN", Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "Member #", PropertyName = "memberNum", Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Processable", PropertyName = "procAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 14, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Pending", PropertyName = "pendAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 15, Function = DataFieldFunctions.Sum  }
      };
    }

    #endregion

    #region RptBurnedDepositsByResorts

    /// <summary>
    /// Formato para el reporte Burned Deposits by Resorts
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
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
new ExcelFormatTable() { Title = "Resort", PropertyName = "guHotelB", Axis = ePivotFieldAxis.Row, Order = 7, IsGroup = true },
new ExcelFormatTable() { Title = "Loc", PropertyName = "grlo", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "SR", PropertyName = "grsr", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "PR", PropertyName = "grpe", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Host", PropertyName = "grHost", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "cuN", Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "ptN", Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "Member #", PropertyName = "memberNum", Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() { Title = "Processable", PropertyName = "procAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 15, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Pending", PropertyName = "pendAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 16, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum  }
      };
    }

    #endregion

    #region RptPaidDeposits

    /// <summary>
    /// Formato para el reporte Paid Deposits
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
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
new ExcelFormatTable() { Title = "Deposit", PropertyName = "grDeposit", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum, AggregateFunction = DataFieldFunctions.Sum }
      };
    }

    #endregion

    #endregion

    #region Gift

    #region RptDailyGiftSimple

    /// <summary>
    /// Formato para el reporte Daily Gift Simple
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
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

    #endregion

    #region RptWeeklyGiftSimple

    /// <summary>
    /// Formato para el reporte Daily Gift Simple
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
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

    #endregion

    #endregion

    #region Guests

    #region RptGuestCeco

    /// <summary>
    /// Formato para el reporte Guest CECO
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
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

    #endregion

    #region RptGuestNoBuyers

    /// <summary>
    /// Formato para el reporte Guest No Buyers
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGuestNoBuyers()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Program", PropertyName = "Program", Axis = ePivotFieldAxis.Row, Order = 1, Compact = true, Outline = true  },
new ExcelFormatTable() { Title = "LeadSource", PropertyName = "LeadSource", Axis = ePivotFieldAxis.Row, Order = 2, Compact = true, Outline = true  },
new ExcelFormatTable() { Title = "GUID", PropertyName = "GuestID", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 3 },
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

    #endregion

    #region RptInOut

    /// <summary>
    /// Formato para el reporte In & Out
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 09/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptInOut()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoom", Axis = ePivotFieldAxis.Row, Order = 1, Compact = true, Outline = true  },
new ExcelFormatTable() { Title = "Location", PropertyName = "Location", Axis = ePivotFieldAxis.Row, Order = 2, Compact = true, Outline = true  },
new ExcelFormatTable() { Title = "GUID", PropertyName = "GUID", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "Hotel", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Room", PropertyName = "Room", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Pax", PropertyName = "Pax", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "LastName", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "FirstName", Axis = ePivotFieldAxis.Row, Order = 8},
new ExcelFormatTable() { Title = "Agency ID", PropertyName = "Agency", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "Agency", PropertyName = "AgencyN", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Country ID", PropertyName = "Country", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Country", PropertyName = "CountryN", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Show D", PropertyName = "ShowDate", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Time In", PropertyName = "TimeIn", Format = EnumFormatTypeExcel.Time, Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() { Title = "Time Out", PropertyName = "TimeOut", Format = EnumFormatTypeExcel.Time, Axis = ePivotFieldAxis.Row, Order = 15 },
new ExcelFormatTable() { Title = "Direct", PropertyName = "Direct", Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "Tour", PropertyName = "Tour", Axis = ePivotFieldAxis.Values, Order = 2 },
new ExcelFormatTable() { Title = "In Out", PropertyName = "InOut", Axis = ePivotFieldAxis.Values, Order = 3 },
new ExcelFormatTable() { Title = "Walk Out", PropertyName = "WalkOut", Axis = ePivotFieldAxis.Values, Order = 4 },
new ExcelFormatTable() { Title = "Courtesy Tour", PropertyName = "CourtesyTour", Axis = ePivotFieldAxis.Values, Order = 5 },
new ExcelFormatTable() { Title = "Save Program", PropertyName = "SaveProgram", Axis = ePivotFieldAxis.Values, Order = 6 },
new ExcelFormatTable() { Title = "PR 1", PropertyName = "PR1", Axis = ePivotFieldAxis.Row, Order = 16 },
new ExcelFormatTable() { Title = "PR1 Name", PropertyName = "PR1N", Axis = ePivotFieldAxis.Row, Order = 17 },
new ExcelFormatTable() { Title = "PR 2", PropertyName = "PR2", Axis = ePivotFieldAxis.Row, Order = 18 },
new ExcelFormatTable() { Title = "PR2 Name", PropertyName = "PR2N", Axis = ePivotFieldAxis.Row, Order = 19 },
new ExcelFormatTable() { Title = "PR 3", PropertyName = "PR3", Axis = ePivotFieldAxis.Row, Order = 20 },
new ExcelFormatTable() { Title = "PR3 Name", PropertyName = "PR3N", Axis = ePivotFieldAxis.Row, Order = 21 },
new ExcelFormatTable() { Title = "Host", PropertyName = "Host", Axis = ePivotFieldAxis.Row, Order = 22 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "Comments", Axis = ePivotFieldAxis.Row, Order = 23 }
      };
    }

    #endregion

    #region RptGuestNoShow

    /// <summary>
    /// Formato para el reporte Guest No Show
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
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
new ExcelFormatTable() { Title = "Book Cancel", PropertyName = "guBookCanc", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 14 }
      };
    }

    #endregion

    #endregion

    #region Meal Tickets

    #region RptMealTickets

    /// <summary>
    /// Formato para el reporte Meal Tickets
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
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
new ExcelFormatTable() { Title = "Show", PropertyName = "guShow", Format = EnumFormatTypeExcel.Boolean, Axis = ePivotFieldAxis.Row, Order = 8 },
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

    #endregion

    #region RptMealTicketsByHost

    /// <summary>
    /// Formato para el reporte Meal Tickets by Host
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
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
new ExcelFormatTable() { Title = "Show", PropertyName = "guShow", Format = EnumFormatTypeExcel.Boolean, Axis = ePivotFieldAxis.Row, Order = 9 },
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

    #endregion

    #region RptMealTicketsCost

    /// <summary>
    /// Formato para el reporte Meal Tickets Cost
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
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

    #endregion

    #endregion

    #region Memberships

    #region RptMemberships

    /// <summary>
    /// Formato para el reporte Memberships
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptMemberships()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Sale ID", PropertyName = "saID", Axis = ePivotFieldAxis.Row, Order = 1  },
new ExcelFormatTable() { Title = "Sale Date", PropertyName = "saD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Proc Date", PropertyName = "saProcD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Canc Date", PropertyName = "saCancelD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Loc", PropertyName = "salo", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "SR", PropertyName = "sasr", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Membership", PropertyName = "saMembershipNum", Axis = ePivotFieldAxis.Row, Order = 7  },
new ExcelFormatTable() { Title = "MT", PropertyName = "samt", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "ST", PropertyName = "sast", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "Last Name 1", PropertyName = "saLastName1", Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "First Name 1", PropertyName = "saFirstName1", Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Last Name 2", PropertyName = "saLastName2", Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "First Name 2", PropertyName = "saFirstName2", Axis = ePivotFieldAxis.Row, Order = 13  },
new ExcelFormatTable() { Title = "Proc Vol", PropertyName = "ProcAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 14, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "OOP Vol", PropertyName = "OOPAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 15, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Canc Vol", PropertyName = "CancAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 16, Function = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "Pend Vol", PropertyName = "PendAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 18, Function = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "PR1", PropertyName = "saPR1", Axis = ePivotFieldAxis.Row, Order = 19 },
new ExcelFormatTable() { Title = "PR2", PropertyName = "saPR2", Axis = ePivotFieldAxis.Row, Order = 20  },
new ExcelFormatTable() { Title = "PR3", PropertyName = "saPR3", Axis = ePivotFieldAxis.Row, Order = 21 },
new ExcelFormatTable() { Title = "Liner1", PropertyName = "saLiner1", Axis = ePivotFieldAxis.Row, Order = 22 },
new ExcelFormatTable() { Title = "Liner2", PropertyName = "saLiner2", Axis = ePivotFieldAxis.Row, Order = 23 },
new ExcelFormatTable() { Title = "Closer1", PropertyName = "saCloser1", Axis = ePivotFieldAxis.Row, Order = 24 },
new ExcelFormatTable() { Title = "Closer2", PropertyName = "saCloser2", Axis = ePivotFieldAxis.Row, Order = 25 },
new ExcelFormatTable() { Title = "Closer3", PropertyName = "saCloser3", Axis = ePivotFieldAxis.Row, Order = 26  },
new ExcelFormatTable() { Title = "Exit1", PropertyName = "saExit1", Axis = ePivotFieldAxis.Row, Order = 27 },
new ExcelFormatTable() { Title = "Exit2", PropertyName = "saExit2", Axis = ePivotFieldAxis.Row, Order = 28 },
new ExcelFormatTable() { Title = "VLO", PropertyName = "saVLO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 29 },
new ExcelFormatTable() { Title = "Podium", PropertyName = "saPodium", Axis = ePivotFieldAxis.Row, Order = 30 },
new ExcelFormatTable() { Title = "GUID", PropertyName = "sagu" },
new ExcelFormatTable() { Title = "Comments", PropertyName = "saComments", Axis = ePivotFieldAxis.Row, Order = 31 },
new ExcelFormatTable() { Title = "Total Proc", PropertyName = "TotalProcAmount", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Row, Order = 17, Function = DataFieldFunctions.Sum  }
      };
    }

    #endregion

    #region RptMembershipsByAgencyMarket

    /// <summary>
    /// Formato para el reporte Memberships
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptMembershipsByAgencyMarket()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Market", PropertyName = "mkN", Order = 1, IsGroup = true  },
new ExcelFormatTable() { Title = "Agency", PropertyName = "agN", Order = 2, IsGroup = true },
new ExcelFormatTable() { Title = "Sale ID", PropertyName = "saID", Axis = ePivotFieldAxis.Row, Order = 1  },
new ExcelFormatTable() { Title = "Sale Date", PropertyName = "saProcD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Lead Source", PropertyName = "lsN", Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Sales Room", PropertyName = "srN", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Membership", PropertyName = "saMembershipNum", Axis = ePivotFieldAxis.Row, Order = 5  },
new ExcelFormatTable() { Title = "Membership Type", PropertyName = "mtN", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "ST", PropertyName = "sast", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "saLastName1", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "saFirstName1", Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "Proc Vol", PropertyName = "ProcAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 10, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "OOP Vol", PropertyName = "OOPAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 11, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "Canc Vol", PropertyName = "CancAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 12, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Pend Vol", PropertyName = "PendAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 14, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "% DP", PropertyName = "saDownPaymentPercentage", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Row, Order = 15, SubTotalFunctions = eSubTotalFunctions.Avg },
new ExcelFormatTable() { Title = "% EDP", PropertyName = "saDownPaymentPaidPercentage", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Row, Order = 17, SubTotalFunctions = eSubTotalFunctions.Avg },
new ExcelFormatTable() { Title = "Comments", PropertyName = "saComments", Axis = ePivotFieldAxis.Row, Order = 19 },
new ExcelFormatTable() { Title = "Total Proc", PropertyName = "TotalProcAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 13, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "$ DP", PropertyName = "saDownPayment", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 16, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "$ EDP", PropertyName = "saDownPaymentPaid", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 18, SubTotalFunctions = eSubTotalFunctions.Sum  }
      };
    }

    #endregion

    #region RptMembershipsByHost

    /// <summary>
    /// Formato para el reporte Memberships By Host
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 14/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptMembershipsByHost()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Host", PropertyName = "guEntryHost", Order = 1, IsGroup = true  },
new ExcelFormatTable() { Title = "Host Name", PropertyName = "guEntryHostN", Order = 2 },
new ExcelFormatTable() { Title = "Sale Type", PropertyName = "SaleType", Order = 3, IsGroup = true  },
new ExcelFormatTable() { Title = "Sale ID", PropertyName = "saID", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Sale Date", PropertyName = "saD", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "LS", PropertyName = "sals", Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "SR", PropertyName = "sasr", Axis = ePivotFieldAxis.Row, Order = 3  },
new ExcelFormatTable() { Title = "Membership", PropertyName = "saMembershipNum", Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "MT", PropertyName = "samt", Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "ST", PropertyName = "sast", Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "saLastName1", Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "saFirstName1", Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "Proc Vol", PropertyName = "ProcAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 9, SubTotalFunctions = eSubTotalFunctions.Sum },
new ExcelFormatTable() { Title = "OOP Vol", PropertyName = "OOPAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 10, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Canc Vol", PropertyName = "CancAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 11, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Pend Vol", PropertyName = "PendAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 13, SubTotalFunctions = eSubTotalFunctions.Sum  },
new ExcelFormatTable() { Title = "Comments", PropertyName = "saComments", Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() { Title = "Total Proc", PropertyName = "TotalProcAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 12, SubTotalFunctions = eSubTotalFunctions.Sum  }
      };
    }

    #endregion

    #endregion

    #endregion

    #region General Reports

    #region rptGifts
    /// <summary>
    /// Formato para el reporte general de Gifts
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptGifts()
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
new ExcelFormatTable() { Title = "Active", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
      };
    }
    #endregion

    #region rptPersonnel
    /// <summary>
    /// Formato para el reporte general de Personnel
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptPersonnel()
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
    #endregion 
    #endregion
  }
}
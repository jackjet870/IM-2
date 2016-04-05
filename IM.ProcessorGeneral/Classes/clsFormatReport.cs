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

    #region rptBookingsBySalesRoomProgramTime
    // <summary>
    /// Formato para el reporte Bookings By Sales Room, Program & Time
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptBookingsBySalesRoomProgramTime()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Program", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left , Order = 2, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Book Type", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 3, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title="Time", Format=EnumFormatTypeExcel.General,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 1 }
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
new ExcelFormatTable() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Program", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 2, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Lead Source", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 3, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Book Type", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 4, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Time", Format=EnumFormatTypeExcel.General,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 1 }
      };
    }
    #endregion

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
new ExcelFormatTable() { Title = "Group", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 1 },
new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 2 },
new ExcelFormatTable() { Title = "Chb", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 3 },
new ExcelFormatTable() { Title = "PP", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Order = 4 },
new ExcelFormatTable() { Title = "Rcpt", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 5 },
new ExcelFormatTable() { Title = "Date", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Order = 6 },
new ExcelFormatTable() { Title = "Guest ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Order = 7 },
new ExcelFormatTable() { Title = "Guest Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 8 },
new ExcelFormatTable() { Title = "Qty", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Order = 9 },
new ExcelFormatTable() { Title = "Gift", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 10 },
new ExcelFormatTable() { Title = "Ad", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Order = 11 },
new ExcelFormatTable() { Title = "Min", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Order = 12 },
new ExcelFormatTable() { Title = "Folios", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 13 },
new ExcelFormatTable() { Title = "Total Gifts", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "CxC Gift", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2 },
new ExcelFormatTable() { Title = "CxC Adj", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3 },
new ExcelFormatTable() { Title = "CxC Deposit", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4 },
new ExcelFormatTable() { Title = "Currency Deposit", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 14 },
new ExcelFormatTable() { Title = "Ex. Rate Deposit", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5 },
new ExcelFormatTable() { Title = "Deposit US", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6 },
new ExcelFormatTable() { Title = "Deposit MN", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7 },
new ExcelFormatTable() { Title = "CxC Taxi Out", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8 },
new ExcelFormatTable() { Title = "Currency Taxi Out", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 15 },
new ExcelFormatTable() { Title = "Ex. Rate Taxi Out", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9 },
new ExcelFormatTable() { Title = "Taxi Out US", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10 },
new ExcelFormatTable() { Title = "Taxi Out MN", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11 },
new ExcelFormatTable() { Title = "Total CxC", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12 },
new ExcelFormatTable() { Title = "CxC Paid US", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13 },
new ExcelFormatTable() { Title = "CxC Paid MN", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14 },
new ExcelFormatTable() { Title = "Ex. Rate", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15 },
new ExcelFormatTable() { Title = "CxC Comments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 16 },
new ExcelFormatTable() { Title = "Receipt Comments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 17 }
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
new ExcelFormatTable() { Title = "Ch B", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Order = 1 },
new ExcelFormatTable() { Title = "Chb PP", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 2 },
new ExcelFormatTable() { Title = "Date", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 3 },
new ExcelFormatTable() { Title = "LS", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 4 },
new ExcelFormatTable() { Title = "GUID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Order = 5 },
new ExcelFormatTable() { Title = "Guest Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 6 },
new ExcelFormatTable() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 7 },
new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 8 },
new ExcelFormatTable() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 9 },
new ExcelFormatTable() { Title = "Host", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 10 },
new ExcelFormatTable() { Title = "Host Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 11 },
new ExcelFormatTable() { Title = "CxC", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "CxC Currency", Format=EnumFormatTypeExcel.General, Alignment=ExcelHorizontalAlignment.Left, Order = 12},
new ExcelFormatTable() { Title = "Currency", Format=EnumFormatTypeExcel.General, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending }
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
new ExcelFormatTable() {  Title="CHB", PropertyName = "grID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() {  Title="Chb PP",PropertyName = "grNum", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() {  Title="PR",PropertyName = "grpe", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() {  Title="PR Name",PropertyName = "peN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() {  Title="Location",PropertyName = "grlo", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() {  Title="Host",PropertyName = "grHost", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() {  Title="Host Name" ,PropertyName = "HostN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() {  Title="GUID" ,PropertyName = "grgu", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() {  Title="Guest Name" ,PropertyName = "grGuest", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() {  Title="Gift",PropertyName = "Gift", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() {  Title="Gift Name",PropertyName = "giN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() {  Title="Quantity" ,PropertyName = "Quantity", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left,Function=OfficeOpenXml.Table.PivotTable.DataFieldFunctions.Sum, Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() {  Title="Adults" ,PropertyName = "Adults", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() {  Title="Minors" ,PropertyName = "Minors", Format=EnumFormatTypeExcel.Number, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() {  Title="Folios" ,PropertyName = "Folios", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3 },
new ExcelFormatTable() {  Title="Cost" ,PropertyName = "Cost", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left,Function=OfficeOpenXml.Table.PivotTable.DataFieldFunctions.Sum, Axis = ePivotFieldAxis.Values, Order = 2 },
new ExcelFormatTable() {  Title="Rcpt Date" ,PropertyName = "grD", Format=EnumFormatTypeExcel.Date, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 12},
new ExcelFormatTable() {  Title="USD" ,PropertyName = "CostUS", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left,Function=OfficeOpenXml.Table.PivotTable.DataFieldFunctions.Sum, Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() {  Title="Exch. Rate" ,PropertyName = "exExchRate", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 14 },
new ExcelFormatTable() {  Title="MX" ,PropertyName = "CostMX", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left,Function=OfficeOpenXml.Table.PivotTable.DataFieldFunctions.Sum, Axis = ePivotFieldAxis.Row, Order = 15 },
new ExcelFormatTable() {  Title="Member #" ,PropertyName = "grMemberNum", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 16 },
new ExcelFormatTable() {  Title="Comments"  ,PropertyName = "grCxCComments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 17 }
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
new ExcelFormatTable() { Title = "Rcpt", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Chb PP", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "LS", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "GUID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Guest Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Rcpt Date", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC Gifts", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC Dep", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC Taxi", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left }
      };
    }

    #endregion

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
new ExcelFormatTable() { Title = "Ch B", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Chb PP", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Out. Inv.", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "GUID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Guest", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "LS", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "SR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.General , Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Date", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Book Date", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Show", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Currency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Payment Type", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 2, Sort = eSortType.Ascending },
new ExcelFormatTable() { Title = "Deposited", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Received", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Burned", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "CxC", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Function = DataFieldFunctions.Sum }
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
new ExcelFormatTable() { Title = "Ch B", PropertyName = "grID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Chb PP", PropertyName = "grNum", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Date", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Guest ID", PropertyName = "grgu", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "grGuest", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "grHotel", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Loc", PropertyName = "grlo", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "SR", PropertyName = "grsr", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "PR", PropertyName = "grpe", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "Host", PropertyName = "grHost", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "grComments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "cuN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "ptN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "Member #", PropertyName = "memberNum", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Processable", PropertyName = "procAmount", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 14, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Pending", PropertyName = "pendAmount", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 15, Function = DataFieldFunctions.Sum  }
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
new ExcelFormatTable() { Title = "Ch B", PropertyName = "grID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "Chb PP", PropertyName = "grNum", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2 },
new ExcelFormatTable() { Title = "Date", PropertyName = "grD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 3 },
new ExcelFormatTable() { Title = "Guest ID", PropertyName = "grgu", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 4 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "grGuest", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 5 },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "grHotel", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 6 },
new ExcelFormatTable() { Title = "Resort", PropertyName = "guHotelB", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 7 },
new ExcelFormatTable() { Title = "Loc", PropertyName = "grlo", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 8 },
new ExcelFormatTable() { Title = "SR", PropertyName = "grsr", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 9 },
new ExcelFormatTable() { Title = "PR", PropertyName = "grpe", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 10 },
new ExcelFormatTable() { Title = "PR Name", PropertyName = "peN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 11 },
new ExcelFormatTable() { Title = "Host", PropertyName = "grHost", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 12 },
new ExcelFormatTable() { Title = "Currency", PropertyName = "cuN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 1 },
new ExcelFormatTable() { Title = "Payment Type", PropertyName = "ptN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 2 },
new ExcelFormatTable() { Title = "Burned", PropertyName = "grDepositTwisted", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum  },
new ExcelFormatTable() { Title = "Member #", PropertyName = "memberNum", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 13 },
new ExcelFormatTable() { Title = "Processable", PropertyName = "procAmount", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 14, Function = DataFieldFunctions.Sum },
new ExcelFormatTable() { Title = "Pending", PropertyName = "pendAmount", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 15, Function = DataFieldFunctions.Sum  }
      };
    }

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
new ExcelFormatTable() { Title = "Gift ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Gift Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Short N.", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Order", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Price", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Price Min.", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CXC", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CXC Min.", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Package", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Category", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
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
new ExcelFormatTable() { Title = "Status", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 1 },
new ExcelFormatTable() { Title = "Dept", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 2 },
new ExcelFormatTable() { Title = "Post", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 3 },
new ExcelFormatTable() { Title = "Place", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 4 },
new ExcelFormatTable() { Title = "ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 5 },
new ExcelFormatTable() { Title = "Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 6 },
new ExcelFormatTable() { Title = "Active", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center, Order = 7 },
new ExcelFormatTable() { Title = "Collaborator", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Order = 8 },
new ExcelFormatTable() { Title = "Captain", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 9 },
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
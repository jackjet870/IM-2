using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Host.Classes
{
  public static class clsFormatReport
  {
    #region RptUpList
    /// <summary>
    /// Formato para el reporte Up List Start o Up List End
    /// </summary>
    /// <returns> List of ExcelFormatTable</returns>
    /// <history>
    /// [edgrodriguez] 21/Jun/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptUpList()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() { Title = "Sales man", PropertyName = "Salesman", Axis = ePivotFieldAxis.Row, Order = 1 },
        new ExcelFormatTable() { Title = "Name", PropertyName = "SalesmanN", Axis = ePivotFieldAxis.Row, Order = 2 },
        new ExcelFormatTable() { Title = "Post", PropertyName = "SalesmanPost", IsVisible = false },
        new ExcelFormatTable() { Title = "Post Name", PropertyName = "SalesmanPostN", IsGroup =  true, Order = 1 },
        new ExcelFormatTable() { Title = "Day Off", PropertyName = "DayOffList", Axis = ePivotFieldAxis.Row, Order = 3 },
        new ExcelFormatTable() { Title = "Language", PropertyName = "Language", Axis= ePivotFieldAxis.Values, Order = 4 },
        new ExcelFormatTable() { Title = "Location", PropertyName = "Location", Axis = ePivotFieldAxis.Values, Order = 5},
        new ExcelFormatTable() { Title = "Time", PropertyName = "Time", Format = EnumFormatTypeExcel.Time, Axis = ePivotFieldAxis.Values, Order = 6 },
        new ExcelFormatTable() { Title = "TimeN", PropertyName = "TimeN", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Column, Sort = eSortType.Ascending, Order = 1 },
        new ExcelFormatTable() { Title = "Amount Ytd", PropertyName = "AmountYtd", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 7 },
        new ExcelFormatTable() { Title = "Amount M", PropertyName = "AmountM", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 8 }
      };
    }
    #endregion

    #region RptExchangeRatesLog
    /// <summary>
    /// Formato para el reporte Exchange Rates Log
    /// </summary>
    /// <returns> List of ExcelFormatTable</returns>
    /// <history>
    /// [edgrodriguez] 08/Jul/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptExchangeRatesLog()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() { Title = "By", PropertyName = "elChangedBy", Order = 1 },
        new ExcelFormatTable() { Title = "Name", PropertyName = "ChangedByN", Order = 2 },
        new ExcelFormatTable() { Title = "Update Date/Time", PropertyName = "elID", Format = EnumFormatTypeExcel.DateTime, Order= 3 },
        new ExcelFormatTable() { Title = "Currency", PropertyName = "elcu", Order = 4 },
        new ExcelFormatTable() { Title = "Exch. Rate", PropertyName = "elExchangeRate", Format = EnumFormatTypeExcel.DecimalNumber, Order = 5 }
      };
    }
    #endregion

    #region RptGiftReceiptsLog
    /// <summary>
    /// Formato para el reporte Gifts Receipts Log
    /// </summary>
    /// <returns> List of ExcelFormatTable</returns>
    /// <history>
    /// [edgrodriguez] 08/Jun/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptGiftReceiptsLog()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() { Title = "Sales man", PropertyName = "goChangedBy", Order = 1 },
        new ExcelFormatTable() { Title = "Name", PropertyName = "ChangedByN", Order = 2 },
        new ExcelFormatTable() { Title = "Update Date/Time", PropertyName = "goID", Format = EnumFormatTypeExcel.DateTime, Order = 3 },
        new ExcelFormatTable() { Title = "Receipt D", PropertyName = "goD", Format = EnumFormatTypeExcel.Date, Order = 4 },
        new ExcelFormatTable() { Title = "Offered By", PropertyName = "gope", Order = 5 },
        new ExcelFormatTable() { Title = "Offered By Name", PropertyName = "OfferedByN", Order = 6 },
        new ExcelFormatTable() { Title = "Host", PropertyName = "goHost", Order = 7 },
        new ExcelFormatTable() { Title = "Host Name", PropertyName = "HostN", Order = 8 },
        new ExcelFormatTable() { Title = "Deposit", PropertyName = "goDeposit", Format = EnumFormatTypeExcel.Currency, Order = 9 },
        new ExcelFormatTable() { Title = "Burned", PropertyName = "goBurned", Format = EnumFormatTypeExcel.Currency, Order = 10 },
        new ExcelFormatTable() { Title = "Currency", PropertyName = "gocu", Order = 11 },
        new ExcelFormatTable() { Title = "Payment Type", PropertyName = "ptN", Order = 12 },
        new ExcelFormatTable() { Title = "CxC PR Dep", PropertyName = "goCXCPRDeposit", Format = EnumFormatTypeExcel.Currency, Order = 13 },
        new ExcelFormatTable() { Title = "Taxit Out", PropertyName = "goTaxiOut", Format = EnumFormatTypeExcel.Currency, Order = 14 },
        new ExcelFormatTable() { Title = "Total Gifts", PropertyName = "goTotalGifts", Format = EnumFormatTypeExcel.DecimalNumber, Order = 15 },
        new ExcelFormatTable() { Title = "Charge To", PropertyName = "goct", Order = 16 },
        new ExcelFormatTable() { Title = "CxC Gifts", PropertyName = "goCXCGifts", Format = EnumFormatTypeExcel.Currency, Order = 17 },
        new ExcelFormatTable() { Title = "CxC Adj.", PropertyName = "goCXCAdj", Format = EnumFormatTypeExcel.Currency, Order = 18 },
        new ExcelFormatTable() { Title = "Re-Printed", PropertyName = "goReimpresion", Order = 19 },
        new ExcelFormatTable() { Title = "Re-Printed Motive", PropertyName = "rmN", Order = 20 },
        new ExcelFormatTable() { Title = "Authorized By", PropertyName = "goAuthorizedBy", Order = 21 },
        new ExcelFormatTable() { Title = "Authorized By Name", PropertyName = "AuthorizedByN", Order = 22 },
        new ExcelFormatTable() { Title = "Paid", PropertyName = "goAmountPaid", Format = EnumFormatTypeExcel.Currency, Order = 23 },
        new ExcelFormatTable() { Title = "Under Pay Motive", PropertyName = "upN", Order = 24 },
        new ExcelFormatTable() { Title = "Cancelled Date", PropertyName = "goCancelD", Format = EnumFormatTypeExcel.Date, Order = 25 },
        };
    }
    #endregion

    #region RptGiftReceiptsLog
    /// <summary>
    /// Formato para el reporte Gifts Receipts Log
    /// </summary>
    /// <returns> List of ExcelFormatTable</returns>
    /// <history>
    /// [edgrodriguez] 08/Jun/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptCloseSalesRoomLog()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() { Title = "By", PropertyName = "sqChangedBy", Order = 1 },
        new ExcelFormatTable() { Title = "Name", PropertyName = "ChangedByN", Order = 2 },
        new ExcelFormatTable() { Title = "Update Date/Time", PropertyName = "sqID", Format = EnumFormatTypeExcel.DateTime, Order = 3 },
        new ExcelFormatTable() { Title = "Shows Close D", PropertyName = "sqShowsCloseD", Format = EnumFormatTypeExcel.Date, Order = 4 },
        new ExcelFormatTable() { Title = "Meal T Close D", PropertyName = "sqMealTicketsCloseD", Format = EnumFormatTypeExcel.Date, Order = 5 },
        new ExcelFormatTable() { Title = "Sales Close D", PropertyName = "sqSalesCloseD", Format = EnumFormatTypeExcel.Date, Order = 6 },
        new ExcelFormatTable() { Title = "Gifts Rcpt Close D", PropertyName = "sqGiftsRcptCloseD", Format = EnumFormatTypeExcel.Date, Order = 7 },
        new ExcelFormatTable() { Title = "CxC Close D", PropertyName = "sqCxCCloseD", Format = EnumFormatTypeExcel.Date, Order = 8 }
        };
    }
    #endregion

    #region RptSaleLog
    /// <summary>
    /// Formato para el reporte de Sales Log
    /// </summary>
    /// <history>
    /// [jorcanche]  created  07072016
    /// </history>    
    public static List<ExcelFormatTable> RptSaleLog()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable {PropertyName = "slChangedBy", Title = "By", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "ChangedByN", Title = "Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slID", Title = "Update Date/Time", Alignment = ExcelHorizontalAlignment.Left,Format=EnumFormatTypeExcel.DateTime},
        new ExcelFormatTable {PropertyName = "slgu", Title = "GUID", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "GuestName", Title = "Guest Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slD", Title = "Sale D", Alignment = ExcelHorizontalAlignment.Left,Format=EnumFormatTypeExcel.Date},
        new ExcelFormatTable {PropertyName = "slProcD", Title = "Proc D", Alignment = ExcelHorizontalAlignment.Left,Format=EnumFormatTypeExcel.Date},
        new ExcelFormatTable {PropertyName = "slCancelD", Title = "Cancel D", Alignment = ExcelHorizontalAlignment.Left,Format=EnumFormatTypeExcel.Date},
        new ExcelFormatTable {PropertyName = "slMembershipNum", Title = "Member #", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "stN", Title = "Sale Type", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slReference", Title = "Reference", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "mtN", Title = "M Type', 'Membership Type", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slOriginalAmount", Title = "O Amount", Alignment = ExcelHorizontalAlignment.Left,Format=EnumFormatTypeExcel.Currency},
        new ExcelFormatTable {PropertyName = "slNewAmount", Title = "N Amount", Alignment = ExcelHorizontalAlignment.Left,Format=EnumFormatTypeExcel.Currency},
        new ExcelFormatTable {PropertyName = "slGrossAmount", Title = "G Amount", Alignment = ExcelHorizontalAlignment.Left,Format=EnumFormatTypeExcel.Currency},
        new ExcelFormatTable {PropertyName = "sllo", Title = "Loc", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slls", Title = "LS", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slsr", Title = "SR", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slSelfGen", Title = "SG", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slPR1", Title = "PR 1", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "PR1N", Title = "PR Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slPR2", Title = "PR 2", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "PR2N", Title = "PR 2 Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slPR3", Title = "PR 3", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "PR3N", Title = "PR 3 Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slLiner1Type", Title = "Liner1 T", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slLiner1", Title = "Liner 1", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "Liner1N", Title = "Liner 1 Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slLiner2", Title = "Liner 2", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "Liner2N", Title = "Liner 2 Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slCloser1", Title = "Closer 1", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "Closer1N", Title = "Closer 1 Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slCloser2", Title = "Closer 2", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "Closer2N", Title = "Closer 2 Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slCloser3", Title = "Closer 3", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "Closer3N", Title = "Closer 3 Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slExit1", Title = "Exit 1", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "Exit1N", Title = "Exit 1 Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slExit2", Title = "Exit 2", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "Exit2N", Title = "Exit 2 Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slPodium", Title = "Podium", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "PodiumN", Title = "Podium Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slVLO", Title = "VLO", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "VLON", Title = "VLO Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slPRCaptain1", Title = "PR Cptn", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "PR1CaptainN", Title = "PR Captain Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slPRCaptain2", Title = "PR Cptn 2", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "PR2CaptainN", Title = "PR Captain 2 Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slLinerCaptain1", Title = "Liner Cptn", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "LinerCaptainN", Title = "Liner Captain Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slCloserCaptain1", Title = "Closer  Cptn", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "CloserCaptainN", Title = "Closer Captain Name", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slCloser1P", Title = "CL1 %", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slCloser2P", Title = "CL2 %", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slCloser3P", Title = "CL3 %", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slExit1P", Title = "Exit1 %", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slExit2P", Title = "Exit2 %", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slClosingCost", Title = "C. Cost", Alignment = ExcelHorizontalAlignment.Left},
        new ExcelFormatTable {PropertyName = "slOverPack", Title = "O. Pack", Alignment = ExcelHorizontalAlignment.Left}
      };
    }
    #endregion
  }
}
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
  }
}
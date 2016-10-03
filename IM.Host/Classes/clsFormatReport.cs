using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;
namespace IM.Host.Classes
{
  public static class clsFormatReport
  {
    #region RptUpList
    /// <summary>
    /// Formato para el reporte Up List Start o Up List End
    /// </summary>
    /// <returns> List of ColumnFormat</returns>
    /// <history>
    /// [edgrodriguez] 21/Jun/2016 Created
    /// [edgrodriguez] 05/09/2016 Modified. Se cambio el formato.
    /// </history>
    public static ColumnFormatList RptUpList()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Post Name", "SalesmanPostN", isGroup: true, isVisible:false);
      lst.Add("ID", "Salesman", axis: ePivotFieldAxis.Row);
      lst.Add("Salesman Name", "SalesmanN", axis: ePivotFieldAxis.Row);
      lst.Add("Off", "DayOffList", axis: ePivotFieldAxis.Row);
      lst.Add("Lang", "Language", axis: ePivotFieldAxis.Values);
      lst.Add("Location", "Location", axis: ePivotFieldAxis.Values);
      lst.Add("Time", "Time", format: EnumFormatTypeExcel.Time, axis: ePivotFieldAxis.Values);
      lst.Add("TimeN", "TimeN", format: EnumFormatTypeExcel.General, axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Vol Ytd", "AmountYtd", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row);
      lst.Add("Vol Month", "AmountM", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row);
      lst.Add("Post", "SalesmanPost", isVisible: false);
      return lst;
    }
    #endregion

    #region RptExchangeRatesLog
    /// <summary>
    /// Formato para el reporte Exchange Rates Log
    /// </summary>
    /// <returns> List of ColumnFormat</returns>
    /// <history>
    /// [edgrodriguez] 08/Jul/2016 Created
    /// </history>
    public static ColumnFormatList RptExchangeRatesLog()
    {
      ColumnFormatList lst = new ColumnFormatList();
        lst.Add("By", "elChangedBy" );
        lst.Add("Name", "ChangedByN");
        lst.Add("Update Date/Time", "elID", format: EnumFormatTypeExcel.DateTime );
        lst.Add("Currency", "elcu" );
        lst.Add("Exch. Rate", "elExchangeRate", format: EnumFormatTypeExcel.DecimalNumber );
      return lst;
    }
    #endregion

    #region RptGiftReceiptsLog
    /// <summary>
    /// Formato para el reporte Gifts Receipts Log
    /// </summary>
    /// <returns> List of ColumnFormat</returns>
    /// <history>
    /// [edgrodriguez] 08/Jun/2016 Created
    /// </history>
    public static ColumnFormatList RptGiftReceiptsLog()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Sales man", "goChangedBy");
      lst.Add("Name", "ChangedByN");
      lst.Add("Update Date/Time", "goID", format: EnumFormatTypeExcel.DateTime);
      lst.Add("Receipt D", "goD", format: EnumFormatTypeExcel.Date);
      lst.Add("Offered By", "gope");
      lst.Add("Offered By Name", "OfferedByN");
      lst.Add("Host", "goHost");
      lst.Add("Host Name", "HostN");
      lst.Add("Deposit", "goDeposit", format: EnumFormatTypeExcel.Currency);
      lst.Add("Burned", "goBurned", format: EnumFormatTypeExcel.Currency);
      lst.Add("Currency", "gocu");
      lst.Add("Payment Type", "ptN");
      lst.Add("CxC PR Dep", "goCXCPRDeposit", format: EnumFormatTypeExcel.Currency);
      lst.Add("Taxit Out", "goTaxiOut", format: EnumFormatTypeExcel.Currency);
      lst.Add("Total Gifts", "goTotalGifts", format: EnumFormatTypeExcel.DecimalNumber);
      lst.Add("Charge To", "goct");
      lst.Add("CxC Gifts", "goCXCGifts", format: EnumFormatTypeExcel.Currency);
      lst.Add("CxC Adj.", "goCXCAdj", format: EnumFormatTypeExcel.Currency);
      lst.Add("Re-Printed", "goReimpresion");
      lst.Add("Re-Printed Motive", "rmN");
      lst.Add("Authorized By", "goAuthorizedBy");
      lst.Add("Authorized By Name", "AuthorizedByN");
      lst.Add("Paid", "goAmountPaid", format: EnumFormatTypeExcel.Currency);
      lst.Add("Under Pay Motive", "upN");
      lst.Add("Cancelled Date", "goCancelD", format: EnumFormatTypeExcel.Date);
      return lst;
    }
    #endregion

    #region RptGiftReceiptsLog
    /// <summary>
    /// Formato para el reporte Gifts Receipts Log
    /// </summary>
    /// <returns> List of ColumnFormat</returns>
    /// <history>
    /// [edgrodriguez] 08/Jun/2016 Created
    /// </history>
    public static ColumnFormatList RptCloseSalesRoomLog()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("By", "sqChangedBy");
      lst.Add("Name", "ChangedByN");
      lst.Add("Update Date/Time", "sqID", format: EnumFormatTypeExcel.DateTime);
      lst.Add("Shows Close D", "sqShowsCloseD", format: EnumFormatTypeExcel.Date);
      lst.Add("Meal T Close D", "sqMealTicketsCloseD", format: EnumFormatTypeExcel.Date);
      lst.Add("Sales Close D", "sqSalesCloseD", format: EnumFormatTypeExcel.Date);
      lst.Add("Gifts Rcpt Close D", "sqGiftsRcptCloseD", format: EnumFormatTypeExcel.Date);
      lst.Add("CxC Close D", "sqCxCCloseD", format: EnumFormatTypeExcel.Date);
      return lst;
    }
    #endregion

    #region RptSaleLog
    /// <summary>
    /// Formato para el reporte de Sales Log
    /// </summary>
    /// <history>
    /// [jorcanche]  created  07072016
    /// </history>    
    public static ColumnFormatList RptSaleLog()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("By", "slChangedBy", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Name", "ChangedByN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Update Date/Time", "slID", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.DateTime);
      lst.Add("GUID", "slgu", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Guest Name", "GuestName", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Sale D", "slD", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Date);
      lst.Add("Proc D", "slProcD", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Date);
      lst.Add("Cancel D", "slCancelD", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Date);
      lst.Add("Member #", "slMembershipNum", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Sale Type", "stN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Reference", "slReference", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("M Type', 'Membership Type", "mtN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("O Amount", "slOriginalAmount", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Currency);
      lst.Add("N Amount", "slNewAmount", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Currency);
      lst.Add("G Amount", "slGrossAmount", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Currency);
      lst.Add("Loc", "sllo", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("LS", "slls", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("SR", "slsr", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("SG", "slSelfGen", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR 1", "slPR1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR Name", "PR1N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR 2", "slPR2", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR 2 Name", "PR2N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR 3", "slPR3", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR 3 Name", "PR3N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Liner1 T", "slLiner1Type", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Liner 1", "slLiner1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Liner 1 Name", "Liner1N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Liner 2", "slLiner2", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Liner 2 Name", "Liner2N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer 1", "slCloser1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer 1 Name", "Closer1N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer 2", "slCloser2", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer 2 Name", "Closer2N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer 3", "slCloser3", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer 3 Name", "Closer3N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Exit 1", "slExit1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Exit 1 Name", "Exit1N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Exit 2", "slExit2", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Exit 2 Name", "Exit2N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Podium", "slPodium", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Podium Name", "PodiumN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("VLO", "slVLO", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("VLO Name", "VLON", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR Cptn", "slPRCaptain1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR Captain Name", "PR1CaptainN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR Cptn 2", "slPRCaptain2", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR Captain 2 Name", "PR2CaptainN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Liner Cptn", "slLinerCaptain1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Liner Captain Name", "LinerCaptainN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer  Cptn", "slCloserCaptain1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer Captain Name", "CloserCaptainN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("CL1 %", "slCloser1P", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("CL2 %", "slCloser2P", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("CL3 %", "slCloser3P", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Exit1 %", "slExit1P", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Exit2 %", "slExit2P", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("C. Cost", "slClosingCost", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("O. Pack", "slOverPack", aligment: ExcelHorizontalAlignment.Left);
      return lst;
    }
    #endregion

    #region RptSalesmenChanges
    /// <summary>
    /// Formato para el reporte de Sales Log
    /// </summary>
    /// <history>
    /// [jorcanche]  created  03102016
    /// </history>    
    public static ColumnFormatList RptSalesmenChanges()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Date / Time", "schDT", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Authorized By", "schAuthorizedBy", aligment: ExcelHorizontalAlignment.Left);      
      lst.Add("Authorized By Name", "AuthorizedByN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Made By", "schMadeBy", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Made By Name", "MadeByN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Role", "roN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Position", "schPosition", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Old Salesman", "schOldSalesman", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Old Salesman Name", "OldSalesmanN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("New Salesman", "schNewSalesman", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("New Salesman Name", "NewSalesmanN", aligment: ExcelHorizontalAlignment.Left);    
      return lst;
    }
    #endregion
  }
}
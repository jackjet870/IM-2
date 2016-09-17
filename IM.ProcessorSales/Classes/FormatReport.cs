using System.Collections.Generic;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Table.PivotTable;

namespace IM.ProcessorSales.Classes
{
  public static class FormatReport
  {
    #region Reports by Sales Room

    #region RptStatisticsBySalesRoomLocation
    /// <summary>
    /// Formato para el reporte Static By Sales Room Location
    /// </summary>
    /// <history>
    /// [ecanul] 06/05/2016 Created
    /// [ecanul] 09/05/2016 Modified, Ahora usa columnas para Pivot, para poder realizar el calculo correcto
    /// </history>
    public static ExcelFormatItemsList RptStatisticsBySalesRoomLocation()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Zona", "Zona", isGroup: true, isVisible: false);
      lst.Add("Program", "Program");
      lst.Add("Sales Room ID", "SalesRoomId");
      lst.Add("Sales Room", "SalesRoom", isGroup: true);
      lst.Add("Location ID", "LocationId");
      lst.Add("Location", "Location");
      lst.Add("Volume", "SalesAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("VIP", "SalesVIP", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Reg", "SalesRegular", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Exit", "SalesExit", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Total", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows] =0,0,[Sales]/[Shows])");
      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows] =0,0,[SalesAmount]/[Shows])");
      lst.Add("AV/S", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales] =0,0,[SalesAmount]/[Sales])");
      lst.Add("OOP", "SalesAmountOOP", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      return lst;
    }

    #endregion

    #region RptStatisticsByLocation
    /// <summary>
    /// Formato para el reporte Statics by Location 
    /// </summary>
    /// <history>
    /// [ecanul] 09/05/2016 Created
    /// </history>
    public static ExcelFormatItemsList RptStatisticsByLocation()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Location", "Location");
      lst.Add("Volume", "SalesAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("VIP", "SalesVIP", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Reg", "SalesRegular", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Exit", "SalesExit", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Total", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows] =0,0,[Sales]/[Shows])");
      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.DecimalNumber, isCalculated: true, formula: "IF([Shows] =0,0,[SalesAmount]/[Shows])");
      lst.Add("AV/S", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales] =0,0,[SalesAmount]/[Sales])");
      lst.Add("OOP", "SalesAmountOOP", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      return lst;
    }

    #endregion

    #region RptStatisticsByLocationMonthly
    /// <summary>
    /// Formato para el reporte Statics by Location Monthly
    /// </summary>
    /// <history>
    /// [ecanul] 10/05/2016 Created
    /// </history>
    public static ExcelFormatItemsList RptStatisticsByLocationMonthly()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Program", "Program", isGroup: true, isVisible: false);
      lst.Add("Location", "Location");
      lst.Add("Volume Previous", "SalesAmountPrevious", format: EnumFormatTypeExcel.Currency, function:DataFieldFunctions.Sum);
      lst.Add("Shows Previous", "UPSPrevious", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Goal", "Goal", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Shows", "GrossUPS", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated:true, formula: "IF([Books]=0,0,[GrossUPS]/[Books])");
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Total Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Volume", "SalesAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("EFF", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated:true, formula: "IF([Shows]=0,0,[SalesAmount]/[Shows])");
      lst.Add("C%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows]=0,0,[Sales]/[Shows])");
      lst.Add("AV/S", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales]=0,0,[SalesAmount]/[Sales])");
      return lst;
    }
    #endregion

    #region RptSalesByLocationMonthly
    /// <summary>
    /// Formato para el reporte Sales by Location Monthly
    /// </summary>
    /// <history>
    /// [ecanul] 10/05/2016 Created
    /// </history>
    public static ExcelFormatItemsList RptSalesByLocationMonthly()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Location", "Location", isGroup: true, isVisible: false);
      lst.Add("Year", "Year", isGroup: true, isVisible: false);
      lst.Add("Month", "MonthN");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Volume", "SalesAmountTotal", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Cancel", "SalesAmountCancel", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("V/N after CXL", "SalesAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("AV/S", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated:true, formula: "IF([Sales] =0,0,[SalesAmount]/[Sales])");
      lst.Add("C%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows]=0,0,[Sales]/[Shows])");
      lst.Add("EFF", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows] =0,0,[SalesAmount]/[Shows])");

      return lst;
    }
    #endregion

    #region RptConcentrateDailySales
    /// <summary>
    /// Formato para el reporte RptConcentrateDailySales
    /// </summary>
    /// <history>
    /// [ecanul] 13/05/2016 Created
    /// </history>
    public static ExcelFormatItemsList RptConcentrateDailySales()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("SalesRoom", "SalesRoom");
      lst.Add("Goal", "Goal", format: EnumFormatTypeExcel.DecimalNumberWithCero, function:DataFieldFunctions.Sum);
      lst.Add("Difference", "Difference", format: EnumFormatTypeExcel.DecimalNumberWithCero, function: DataFieldFunctions.Sum);
      lst.Add("UPS", "UPS", format: EnumFormatTypeExcel.NumberWithCero, function: DataFieldFunctions.Sum);
      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.NumberWithCero, function: DataFieldFunctions.Sum);
      lst.Add("Proc", "Proc", format: EnumFormatTypeExcel.DecimalNumberWithCero, function: DataFieldFunctions.Sum);
      lst.Add("OOP", "OOP", format: EnumFormatTypeExcel.DecimalNumberWithCero, function: DataFieldFunctions.Sum);
      lst.Add("Fall", "Fall", format: EnumFormatTypeExcel.DecimalNumberWithCero, function: DataFieldFunctions.Sum);
      lst.Add("Cxld", "Cancel", format: EnumFormatTypeExcel.DecimalNumberWithCero, function: DataFieldFunctions.Sum);
      lst.Add("Total Proc", "TotalProc", format: EnumFormatTypeExcel.DecimalNumberWithCero, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([UPS]=0,0,[Sales]/[UPS])");
      lst.Add("EFF", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([UPS]=0,0,[TotalProc]/[UPS])");
      lst.Add("AV/S", "AverageSales", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales] =0,0,[TotalProc]/[Sales])");
      lst.Add("Pact", "Pact", format: EnumFormatTypeExcel.PercentWithCero);
      lst.Add("Collect", "Collect", format: EnumFormatTypeExcel.PercentWithCero);
      return lst;
    }
    #endregion

    #region RptDailySales
    /// <summary>
    /// Formato para el reporte RptConcentrateDailySales
    /// </summary>
    /// <history>
    /// [ecanul] 16/05/2016 Created
    /// </history>
    public static ExcelFormatItemsList RptDailySales()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Date", "Date", format: EnumFormatTypeExcel.Date);
      lst.Add("UPS", "UPS", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sale", "Sale", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Exit", "Exit", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("VIP", "VIP", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Total Sale", "TotalSales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Proc", "Proc", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("OOP", "OOP", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Fall", "Fall", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Cxld", "Cxld", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Total Proc", "TotalProc", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([UPS]=0,0,[TotalSales]/[UPS])");
      lst.Add("EFF", "Efficiency", format: EnumFormatTypeExcel.DecimalNumber, isCalculated: true, formula: "IF([UPS] =0,0,[TotalProc]/[UPS])");
      lst.Add("AV/S", "AverageSales", format: EnumFormatTypeExcel.DecimalNumber, isCalculated: true, formula: "IF([TotalSales] =0,0,[TotalProc]/[TotalSales])");
      lst.Add("Pact", "Pact", format: EnumFormatTypeExcel.Percent);
      lst.Add("Collect", "Collect", format: EnumFormatTypeExcel.Percent);
      return lst;
    }
    #endregion

    #region RptManifest
    /// <summary>
    /// Formato para el reporte RptManifest
    /// </summary>
    /// <history>
    /// [ecanul] 07/06/2016 Created
    /// </history>
    public static ExcelFormatItemsList RptManifest()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Sale TypeN", "SaleTypeN", isGroup: true, isVisible: false);
      lst.Add("GUID", "guID", format: EnumFormatTypeExcel.Id);
      lst.Add("Last Name", "guLastName1");
      lst.Add("First Name", "guFirstName1");
      lst.Add("Sh", "guShow", function: DataFieldFunctions.Count);
      lst.Add("Tr", "guTour", function: DataFieldFunctions.Count);
      lst.Add("WO", "guWalkout", function: DataFieldFunctions.Count);
      lst.Add("CT", "guCTour", function: DataFieldFunctions.Count);
      lst.Add("Sve", "guSaveProgram", function: DataFieldFunctions.Count);
      lst.Add("SG", "guSelfGen", function: DataFieldFunctions.Count);
      lst.Add("Overfl", "guOverflow", function: DataFieldFunctions.Count);
      lst.Add("Date", "salesmanDate", format: EnumFormatTypeExcel.Date);
      lst.Add("Sold", "sold", function: DataFieldFunctions.Count);
      lst.Add("Sale", "sale", function: DataFieldFunctions.Count);
      lst.Add("Sale ID", "saID", format: EnumFormatTypeExcel.Id);
      lst.Add("Memb. #", "saMembershipNum", format: EnumFormatTypeExcel.Id);
      lst.Add("M. Type", "samt");
      lst.Add("Proc", "procSales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Proc. Amount", "saGrossAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      //PR1
      lst.Add("ID", "PR1", superHeader: "PR 1");
      lst.Add("Name", "PR1N", superHeader: "PR 1");
      lst.Add("Post", "PR1P", superHeader: "PR 1");
      //PR2
      lst.Add("ID ", "PR2", superHeader: "PR 2");
      lst.Add("Name ", "PR2N", superHeader: "PR 2");
      lst.Add("Post ", "PR2P", superHeader: "PR 2");
      //PR3
      lst.Add("ID  ", "PR3", superHeader: "PR 3");
      lst.Add("Name  ", "PR3N", superHeader: "PR 3");
      lst.Add("Post  ", "PR3P", superHeader: "PR 3");
      //Liner1
      lst.Add(" ID", "Liner1", format: EnumFormatTypeExcel.Id, superHeader: "Liner 1");
      lst.Add(" Name", "Liner1N", superHeader: "Liner 1");
      lst.Add(" Post", "Liner1P", superHeader: "Liner 1");
      //Liner2
      lst.Add("  ID", "Liner2", format: EnumFormatTypeExcel.Id, superHeader: "Liner 2");
      lst.Add("  Name", "Liner2N", superHeader: "Liner 2");
      lst.Add("  Post", "Liner2P", superHeader: "Liner 2");
      //Closer1
      lst.Add(" ID ", "Closer1", format: EnumFormatTypeExcel.Id, superHeader: "Closer 1");
      lst.Add(" Name ", "Closer1N", superHeader: "Closer 1");
      lst.Add(" Post ", "Closer1P", superHeader: "Closer 1");
      //Closer2
      lst.Add("  ID  ", "Closer2", format: EnumFormatTypeExcel.Id, superHeader: "Closer 2");
      lst.Add("  Name  ", "Closer2N", superHeader: "Closer 2");
      lst.Add("  Post  ", "Closer2p", superHeader: "Closer 2");
      //Closer3
      lst.Add("ID   ", "Closer3", format: EnumFormatTypeExcel.Id, superHeader: "Closer 3");
      lst.Add("Name   ", "Closer3N", superHeader: "Closer 3");
      lst.Add("Post   ", "Closer3p", superHeader: "Closer 3");
      //Exit1
      lst.Add("   ID", "Exit1", format: EnumFormatTypeExcel.Id, superHeader: "Exit 1");
      lst.Add("   Name", "Exit1N", superHeader: "Exit 1");
      lst.Add("   Post", "Exit1P", superHeader: "Exit 1");
      //Exit2
      lst.Add("   ID   ", "Exit2", format: EnumFormatTypeExcel.Id, superHeader: "Exit 2");
      lst.Add("   Name   ", "Exit2N", superHeader: "Exit 2");
      lst.Add("   Post   ", "Exit2P", superHeader: "Exit 2");
      lst.Add("Sale Type", "SaleType", format: EnumFormatTypeExcel.Number, isVisible: false);
      return lst;
    }
    #endregion

    #region RptFTMInOutHouse
    /// <summary>
    /// Formato para el reporte RptFtmIn&OutHouse
    /// </summary>
    /// <history>
    /// [ecanul] 04/07/2016 Created
    /// </history>
    public static ExcelFormatItemsList RptFTMInOutHouse()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("ID", "Liner", format: EnumFormatTypeExcel.General);
      lst.Add("Salesman Name", "peN", format: EnumFormatTypeExcel.General);
      lst.Add("OOP", "OOP", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Count);
      //OVERFLOW
      lst.Add("VOL", "OFSalesAmount", format: EnumFormatTypeExcel.Currency, superHeader: "Overflow", function: DataFieldFunctions.Sum);
      lst.Add("UPS", "OFShows", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Overflow", function: DataFieldFunctions.Sum);
      lst.Add("Sale", "OFSales", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Overflow", function: DataFieldFunctions.Sum);
      lst.Add("Exit", "OFExit", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Overflow", function: DataFieldFunctions.Sum);
      lst.Add("Total", "OFTotal", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Overflow", function: DataFieldFunctions.Sum);
      //REGEN
      lst.Add("VOL ", "RSalesAmount", format: EnumFormatTypeExcel.Currency, superHeader: "Regen", function: DataFieldFunctions.Sum);
      lst.Add("UPS ", "RShows", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Regen", function: DataFieldFunctions.Sum);
      lst.Add("Sale ", "RSales", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Regen", function: DataFieldFunctions.Sum);
      lst.Add("Exit ", "RExit", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Regen", function: DataFieldFunctions.Sum);
      lst.Add("Total ", "RTotal", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Regen", function: DataFieldFunctions.Sum);
      //Normal
      lst.Add(" VOL", "NSalesAmount", format: EnumFormatTypeExcel.Currency, superHeader: "Normal", function: DataFieldFunctions.Sum);
      lst.Add(" UPS", "NShows", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Normal", function: DataFieldFunctions.Sum);
      lst.Add(" Sale", "NSales", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Normal", function: DataFieldFunctions.Sum);
      lst.Add(" Exit", "NExit", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Normal", function: DataFieldFunctions.Sum);
      lst.Add(" Total", "NTotal", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Normal", function: DataFieldFunctions.Sum);
      //Total
      lst.Add(" VOL ", "TSalesAmount", format: EnumFormatTypeExcel.Currency, superHeader: "Total", function: DataFieldFunctions.Sum);
      lst.Add(" UPS ", "TShows", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Total", function: DataFieldFunctions.Sum);
      lst.Add(" Sale ", "TSales", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Total", function: DataFieldFunctions.Sum);
      lst.Add(" Exit ", "TExit", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Total", function: DataFieldFunctions.Sum);
      lst.Add(" Total ", "TTotal", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Total", function: DataFieldFunctions.Sum);
      lst.Add("EFF Overf", "EFFOver", format: EnumFormatTypeExcel.DecimalNumber, isCalculated: true, formula: "IF([NShows]=0,0,[TSalesAmount]/[NShows])");
      lst.Add("EFF", "EFF", isCalculated: true, formula: "IF([TShows]=0,0,[TSalesAmount]/[TShows])");
      lst.Add("C%", "CPer", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([TShows]=0,0,[TTotal]/[TShows])");
      lst.Add("AV/S", "AVS", format: EnumFormatTypeExcel.DecimalNumber, isCalculated: true, formula: "IF([TTotal]=0,0,[TSalesAmount]/[TTotal])");
      return lst;
    }
    #endregion

    #region RptStatisticsBySegments
    /// <summary>
    /// Formato para el reporte StatsBySegment
    /// </summary>
    /// <history>
    /// [aalcocer] 04/07/2016 Created
    /// </history>
    internal static ExcelFormatItemsList RptStatisticsBySegments()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Salesman Type", "SalemanType", axis: ePivotFieldAxis.Row, isGroup: true);
      lst.Add("ID", "SalemanID", axis: ePivotFieldAxis.Row);
      lst.Add("Salesman Name", "SalemanName", axis: ePivotFieldAxis.Row);

      lst.Add("UPS", "UPS", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Sales", "Sales", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Amount", "Amount", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);

      lst.Add("Efficiency", "Efficiency", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.Currency, isCalculated:true, formula: "IF([UPS] =0,0,[Amount]/[UPS])");
      lst.Add("%", "ClosingFactor", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([UPS] =0,0,[Sales]/[UPS])");
      lst.Add("Segment", "SegmentN", axis: ePivotFieldAxis.Column);
      return lst;
    }
    #endregion

    #region RptStatisticsBySegmentsGroupedByTeams
    /// <summary>
    /// Formato para el reporte StatsBySegment agrupado por Teams
    /// </summary>
    /// <history>
    /// [aalcocer] 04/07/2016 Created
    /// </history>
    internal static ExcelFormatItemsList RptStatisticsBySegmentsGroupedByTeams()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Team", "Team", axis: ePivotFieldAxis.Row, isGroup: true );
      lst.Add("Status", "Status", axis: ePivotFieldAxis.Row, isGroup:true);
      lst.Add("Salesman Type", "SalemanType", axis: ePivotFieldAxis.Row, isGroup: true);
      lst.Add("ID", "SalemanID", axis: ePivotFieldAxis.Row);
      lst.Add("Salesman Name", "SalemanName", axis: ePivotFieldAxis.Row);

      lst.Add("UPS", "UPS", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Sales", "Sales", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Amount", "Amount", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);

      lst.Add("Efficiency", "Efficiency", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([UPS] =0,0,[Amount]/[UPS])");
      lst.Add("%", "ClosingFactor", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([UPS] =0,0,[Sales]/[UPS])");
      lst.Add("Segment", "SegmentN", axis: ePivotFieldAxis.Column);
      return lst;
    }
    #endregion

    #region RptStatisticsByCloser
    /// <summary>
    /// Formato para el reporte StatisticsByCloser
    /// </summary>
    /// <history>
    /// [aalcocer]  04/07/2016 Created
    /// [ecanul]    16/08/2016 Modified, cambiado el formato para que se use con el formato CreateExcelCustom
    /// </history>
    internal static ExcelFormatItemsList RptStatisticsByCloser(bool groupByTeams = false)
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Team", "Team", isGroup: groupByTeams, isVisible: !groupByTeams);
      lst.Add("Saleman Status", "SalesmanStatus", isVisible: !groupByTeams, isGroup: groupByTeams);
      lst.Add("ID", "SalemanID");
      lst.Add("Saleman Name", "SalemanName");
      //Total 
      lst.Add("VOL", "TSalesAmount", format: EnumFormatTypeExcel.Currency,
        superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add("OOP", "TOOP", format: EnumFormatTypeExcel.Number,
        superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add("UPS", "TUPS", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add("Sale", "TSalesRegular", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add("Exit", "TSalesExit", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add("Total", "TSales", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add("EFF", "TEfficiency", format: EnumFormatTypeExcel.Currency,
        superHeader: "Total Global", isCalculated: true, formula: "IF([TUPS] = 0, 0, [TSalesAmount] / [TUPS])");
      lst.Add("C%", "TClosingFactor", format: EnumFormatTypeExcel.Percent,
        superHeader: "Total Global", isCalculated: true, formula: "IF([TUPS] = 0, 0, [TSales]/[TUPS])");
      lst.Add("AV/S", "TSaleAverage", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Total Global", isCalculated: true, formula: "IF([TSales]= 0, 0, [TSalesAmount]/[TSales])");
      //Closer
      lst.Add("VOL ", "CSalesAmount", format: EnumFormatTypeExcel.Currency,
        superHeader: "Closer", function: DataFieldFunctions.Sum);
      lst.Add("OOP ", "COOP", format: EnumFormatTypeExcel.Number,
        superHeader: "Closer", function: DataFieldFunctions.Sum);
      lst.Add("UPS ", "CUPS", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Closer", function: DataFieldFunctions.Sum);
      lst.Add("Sale ", "CSalesRegular", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Closer", function: DataFieldFunctions.Sum);
      lst.Add("Exit ", "CSalesExit", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Closer", function: DataFieldFunctions.Sum);
      lst.Add("Total ", "CSales", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Closer", function: DataFieldFunctions.Sum);
      lst.Add("EFF ", "CEfficiency", format: EnumFormatTypeExcel.Currency,
        superHeader: "Closer", isCalculated: true, formula: "IF([CUPS] = 0, 0, [CSalesAmount] / [CUPS])");
      lst.Add("C% ", "CClosingFactor", format: EnumFormatTypeExcel.Percent,
        superHeader: "Closer", isCalculated: true, formula: "IF([CUPS] = 0, 0, [CSales]/[CUPS])");
      lst.Add("AV/S ", "CSaleAverage", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Closer", isCalculated: true, formula: "IF([CSales]= 0, 0, [CSalesAmount]/[CSales])");
      // As Front To Back
      lst.Add("VOL  ", "AsSalesAmount", format: EnumFormatTypeExcel.Currency,
        superHeader: "As Front To Back", function: DataFieldFunctions.Sum);
      lst.Add("OOP  ", "AsOOP", format: EnumFormatTypeExcel.Number,
        superHeader: "As Front To Back", function: DataFieldFunctions.Sum);
      lst.Add("UPS  ", "AsUPS", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "As Front To Back", function: DataFieldFunctions.Sum);
      lst.Add("Sale  ", "AsSalesRegular", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "As Front To Back", function: DataFieldFunctions.Sum);
      lst.Add("Exit  ", "AsSalesExit", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "As Front To Back", function: DataFieldFunctions.Sum);
      lst.Add("Total  ", "AsSales", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "As Front To Back", function: DataFieldFunctions.Sum);
      lst.Add("EFF  ", "AsEfficiency", format: EnumFormatTypeExcel.Currency,
        superHeader: "As Front To Back", isCalculated: true, formula: "IF([AsUPS] = 0, 0, [AsSalesAmount] / [AsUPS])");
      lst.Add("C%  ", "AsClosingFactor", format: EnumFormatTypeExcel.Percent,
        superHeader: "As Front To Back", isCalculated: true, formula: "IF([AsUPS] = 0, 0, [AsSales]/[AsUPS])");
      lst.Add("AV/S  ", "AsSaleAverage", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "As Front To Back", isCalculated: true, formula: "IF([AsSales]= 0, 0, [AsSalesAmount]/[AsSales])");
      // With Junior
      lst.Add(" VOL", "WSalesAmount", format: EnumFormatTypeExcel.Currency,
       superHeader: "With Junior", function: DataFieldFunctions.Sum);
      lst.Add(" OOP", "WOOP", format: EnumFormatTypeExcel.Number,
        superHeader: "With Junior", function: DataFieldFunctions.Sum);
      lst.Add(" UPS", "WUPS", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "With Junior", function: DataFieldFunctions.Sum);
      lst.Add(" Sale", "WSalesRegular", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "With Junior", function: DataFieldFunctions.Sum);
      lst.Add(" Exit", "WSalesExit", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "With Junior", function: DataFieldFunctions.Sum);
      lst.Add(" Total", "WSales", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "With Junior", function: DataFieldFunctions.Sum);
      lst.Add(" EFF", "WEfficiency", format: EnumFormatTypeExcel.Currency,
        superHeader: "With Junior", isCalculated: true, formula: "IF([WUPS] = 0, 0, [WSalesAmount] / [WUPS])");
      lst.Add(" C%", "WClosingFactor", format: EnumFormatTypeExcel.Percent,
        superHeader: "With Junior", isCalculated: true, formula: "IF([WUPS] = 0, 0, [WSales]/[WUPS])");
      lst.Add(" AV/S", "WSaleAverage", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "With Junior", isCalculated: true, formula: "IF([WSales]= 0, 0, [WSalesAmount]/[WSales])");
      // Total As Front To Back And With Junior
      lst.Add("  VOL", "AWSalesAmount", format: EnumFormatTypeExcel.Currency,
        superHeader: "Total As Front To Back And With Junior", function: DataFieldFunctions.Sum);
      lst.Add("  OOP", "AWOOP", format: EnumFormatTypeExcel.Number,
        superHeader: "Total As Front To Back And With Junior", function: DataFieldFunctions.Sum);
      lst.Add("  UPS", "AWUPS", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Total As Front To Back And With Junior", function: DataFieldFunctions.Sum);
      lst.Add("  Sale", "AWSalesRegular", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Total As Front To Back And With Junior", function: DataFieldFunctions.Sum);
      lst.Add("  Exit", "AWSalesExit", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Total As Front To Back And With Junior", function: DataFieldFunctions.Sum);
      lst.Add("  Total", "AWSales", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Total As Front To Back And With Junior", function: DataFieldFunctions.Sum);
      lst.Add("  EFF", "AWEfficiency", format: EnumFormatTypeExcel.Currency,
        superHeader: "Total As Front To Back And With Junior", isCalculated: true, formula: "IF([AWUPS] = 0, 0, [AWSalesAmount] / [AWUPS])");
      lst.Add("  C%", "AWClosingFactor", format: EnumFormatTypeExcel.Percent,
        superHeader: "Total As Front To Back And With Junior", isCalculated: true, formula: "IF([AWUPS] = 0, 0, [AWSales]/[AWUPS])");
      lst.Add("  AV/S", "AWSaleAverage", format: EnumFormatTypeExcel.DecimalNumber,
        superHeader: "Total As Front To Back And With Junior", isCalculated: true, formula: "IF([AWSales]= 0, 0, [AWSalesAmount]/[AWSales])");
      return lst;
    }
    #endregion

    #region RptStatisticsByExitCloser
    /// <summary>
    /// Formato para el reporte StatisticsByExitCloser
    /// </summary>
    /// <history>
    /// [aalcocer] 18/07/2016 Created
    /// </history>
    internal static ExcelFormatItemsList RptStatisticsByExitCloser()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();


      lst.Add("SalemanType", "SalemanType", axis: ePivotFieldAxis.Row, isGroup: true);

      lst.Add("ID", "SalemanID", axis: ePivotFieldAxis.Row);
      lst.Add("Salesman Name", "SalemanName", axis: ePivotFieldAxis.Row);
      lst.Add("VOL", "SalesAmount", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("OPP", "OPP", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("UPS", "UPS", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Sales", "Sales", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Total", "SalesTotal", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);

      lst.Add("EFF", "Efficiency", format: EnumFormatTypeExcel.Currency, formula: "IF([UPS] =0,0, [SalesAmount]/[UPS])", isCalculated: true);
      lst.Add("C%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, formula: "IF([UPS] =0,0,[SalesTotal]/[UPS])", isCalculated: true);
      lst.Add("AV/S", "SaleAverage", format: EnumFormatTypeExcel.Currency, formula: "IF([SalesTotal] =0,0,[SalesAmount]/[SalesTotal])", isCalculated: true);
      lst.Add("SalesAmountRange", "SalesAmountRange", axis: ePivotFieldAxis.Column, format: EnumFormatTypeExcel.Currency);
      return lst;
    }
    #endregion RptStatisticsByExitCloser

    #region RptStatisticsByExitCloserGroupedByTeams
    /// <summary>
    /// Formato para el reporte StatisticsByExitCloser agrupado por Teams
    /// </summary>
    /// <history>
    /// [aalcocer] 18/07/2016 Created
    /// </history>
    internal static ExcelFormatItemsList RptStatisticsByExitCloserGroupedByTeams()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();

      lst.Add("Team", "Team", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Status", "SalesmanStatus", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("SalemanType", "SalemanType", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);

      lst.Add("ID", "SalemanID", axis: ePivotFieldAxis.Row);
      lst.Add("Salesman Name", "SalemanName", axis: ePivotFieldAxis.Row);
      lst.Add("VOL", "SalesAmount", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("OOP", "OPP", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("UPS", "UPS", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Sales", "Sales", axis: ePivotFieldAxis.Values, format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Total", "SalesTotal", axis: ePivotFieldAxis.Row, format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("EFF", "Efficiency", format: EnumFormatTypeExcel.Currency, formula: "IF([UPS] =0,0, [SalesAmount]/[UPS])", isCalculated: true);
      lst.Add("C%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, formula: "IF([UPS] =0,0,[SalesTotal]/[UPS])", isCalculated: true);
      lst.Add("AV/S", "SaleAverage", format: EnumFormatTypeExcel.Currency, formula: "IF([SalesTotal] =0,0,[SalesAmount]/[SalesTotal])", isCalculated: true);
      lst.Add("SalesAmountRange", "SalesAmountRange", axis: ePivotFieldAxis.Column, format: EnumFormatTypeExcel.Currency);
      return lst;
    }
    #endregion RptStatisticsByExitCloserGroupedByTeams

    #region RptSelfGenAndSelfGenTeam
    /// <summary>
    /// Formato para el reporte SelfGen & SelfGenTeam
    /// </summary>
    /// <history>
    ///   [ecanul] 28/07/2016 Created
    /// </history>
    public static ExcelFormatItemsList RptSelfGenAndSelfGenTeam()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();

      lst.Add("SelfGenType", "SelfGenType", format: EnumFormatTypeExcel.General, isGroup: true, isVisible: false);
      lst.Add("ID", "Liner", format: EnumFormatTypeExcel.General);
      lst.Add("Salesman Name", "SalesmanName", format: EnumFormatTypeExcel.General);
      lst.Add("OOP", "OOP", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Count);
      //OVERFLOW
      lst.Add("VOL", "OFVol", format: EnumFormatTypeExcel.Currency, superHeader: "Overflow", function: DataFieldFunctions.Sum);
      lst.Add("UPS", "OFUPS", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Overflow", function: DataFieldFunctions.Count);
      lst.Add("Sales", "OFSales", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Overflow", function: DataFieldFunctions.Sum);
      //REGEN
      lst.Add("VOL ", "RGVol", format: EnumFormatTypeExcel.Currency, superHeader: "Regen", function: DataFieldFunctions.Sum);
      lst.Add("UPS ", "RGUPS", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Regen", function: DataFieldFunctions.Sum);
      lst.Add("Sales ", "RGSales", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Regen", function: DataFieldFunctions.Sum);
      //Normal
      lst.Add(" VOL", "NVol", format: EnumFormatTypeExcel.Currency, superHeader: "Normal", function: DataFieldFunctions.Sum);
      lst.Add(" UPS", "NUPS", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Normal", function: DataFieldFunctions.Sum);
      lst.Add(" Sales", "NSales", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Normal", function: DataFieldFunctions.Sum);
      //Total
      lst.Add(" VOL ", "TVol", format: EnumFormatTypeExcel.Currency, superHeader: "Total", function: DataFieldFunctions.Sum);
      lst.Add(" UPS ", "TUPS", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Total", function: DataFieldFunctions.Sum);
      lst.Add(" Sales ", "TSales", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Total", function: DataFieldFunctions.Sum);
      lst.Add("EFF", "EFF", isCalculated: true, formula: "IF([TUPS]=0,0,[TVol]/[TUPS])");
      lst.Add("C%", "CPer", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([TUPS]=0,0,[TSales]/[TUPS])");
      lst.Add("AV/S", "AVS", format: EnumFormatTypeExcel.DecimalNumber, isCalculated: true, formula: "IF([TSales]=0,0,[TVol]/[TSales])");
      return lst;
    }
    #endregion

    #region RptStatisticsByFTB
    /// <summary>
    /// Formato para el reporte StatisticsByFTB
    /// </summary>
    /// <history>
    /// [michan] 21/07/2016 Created
    /// [edgrodriguez] 03/09/2016 Modified. Se agregó una bandera para indicar si el reporte sera por equipos o individual.
    ///                                     Se cambio el formato.
    /// </history>
    internal static ExcelFormatItemsList RptStatisticsByFTB(bool byTeams)
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Team", "Team", format: EnumFormatTypeExcel.General, isGroup: byTeams, isVisible: false);
      lst.Add("Status", "SalesmanStatus", format: EnumFormatTypeExcel.General, isGroup: byTeams, isVisible: false);
      lst.Add("Post", "PostName", format: EnumFormatTypeExcel.General, isGroup: true, isVisible: false);

      lst.Add("ID", "SalemanID", format: EnumFormatTypeExcel.General);
      lst.Add("Salesman Name", "SalemanName", format: EnumFormatTypeExcel.General);

      //OWN
      lst.Add("VOL", "OAmount", format: EnumFormatTypeExcel.Currency, superHeader: "FrontToBack(Own)", function: DataFieldFunctions.Sum);
      lst.Add("OOP", "OOPP", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(Own)", function: DataFieldFunctions.Sum);
      lst.Add("UPS", "OUPS", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(Own)", function: DataFieldFunctions.Sum);
      lst.Add("Sale", "OSales", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(Own)", function: DataFieldFunctions.Sum);
      lst.Add("Exit", "OExit", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(Own)", function: DataFieldFunctions.Sum);
      lst.Add("Total", "OTotal", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(Own)", function: DataFieldFunctions.Sum);
      lst.Add("EFF", "OEfficiency", format: EnumFormatTypeExcel.Currency, superHeader: "FrontToBack(Own)", function: DataFieldFunctions.Sum);
      lst.Add("C%", "OClosingFactor", format: EnumFormatTypeExcel.Percent, superHeader: "FrontToBack(Own)", function: DataFieldFunctions.Sum);
      lst.Add("AWS", "OSaleAverage", format: EnumFormatTypeExcel.Currency, superHeader: "FrontToBack(Own)", function: DataFieldFunctions.Sum);

      //With
      lst.Add("VOL ", "WAmount", format: EnumFormatTypeExcel.Currency, superHeader: "FrontToBack(With Closer)", function: DataFieldFunctions.Sum);
      lst.Add("OOP ", "WOPP", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(With Closer)", function: DataFieldFunctions.Sum);
      lst.Add("UPS ", "WUPS", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(With Closer)", function: DataFieldFunctions.Sum);
      lst.Add("Sale ", "WSales", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(With Closer)", function: DataFieldFunctions.Sum);
      lst.Add("Exit ", "WExit", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(With Closer)", function: DataFieldFunctions.Sum);
      lst.Add("Total ", "WTotal", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(With Closer)", function: DataFieldFunctions.Sum);
      lst.Add("EFF ", "WEfficiency", format: EnumFormatTypeExcel.Currency, superHeader: "FrontToBack(With Closer)", function: DataFieldFunctions.Sum);
      lst.Add("C% ", "WClosingFactor", format: EnumFormatTypeExcel.Percent, superHeader: "FrontToBack(With Closer)", function: DataFieldFunctions.Sum);
      lst.Add("AWS ", "WSaleAverage", format: EnumFormatTypeExcel.Currency, superHeader: "FrontToBack(With Closer)", function: DataFieldFunctions.Sum);
      //Total
      lst.Add(" VOL", "TAmount", format: EnumFormatTypeExcel.Currency, superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add(" OOP", "TOPP", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add(" UPS", "TUPS", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add(" Sale", "TSales", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add(" Exit", "TExit", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add(" Total", "TTotal", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add(" EFF", "TEfficiency", format: EnumFormatTypeExcel.Currency, superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add(" C%", "TClosingFactor", format: EnumFormatTypeExcel.Percent, superHeader: "Total Global", function: DataFieldFunctions.Sum);
      lst.Add(" AWS", "TSaleAverage", format: EnumFormatTypeExcel.Currency, superHeader: "Total Global", function: DataFieldFunctions.Sum);
      //As
      lst.Add(" VOL ", "AAmount", format: EnumFormatTypeExcel.Currency, superHeader: "FrontToBack(As Closer)", function: DataFieldFunctions.Sum);
      lst.Add(" OOP ", "AOOP", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(As Closer)", function: DataFieldFunctions.Sum);
      lst.Add(" UPS ", "AOUPS", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(As Closer)", function: DataFieldFunctions.Sum);
      lst.Add(" Sale ", "ASales", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(As Closer)", function: DataFieldFunctions.Sum);
      lst.Add(" Exit ", "AExit", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(As Closer)", function: DataFieldFunctions.Sum);
      lst.Add(" Total ", "ATotal", format: EnumFormatTypeExcel.DecimalNumber, superHeader: "FrontToBack(As Closer)", function: DataFieldFunctions.Sum);
      lst.Add(" EFF ", "AEfficiency", format: EnumFormatTypeExcel.Currency, superHeader: "FrontToBack(As Closer)", function: DataFieldFunctions.Sum);
      lst.Add(" C% ", "AClosingFactor", format: EnumFormatTypeExcel.Percent, superHeader: "FrontToBack(As Closer)", function: DataFieldFunctions.Sum);
      lst.Add(" AWS ", "ASaleAverage", format: EnumFormatTypeExcel.Currency, superHeader: "FrontToBack(As Closer)", function: DataFieldFunctions.Sum);
      return lst;
    }
    #endregion RptStatisticsByFTB

    #region RptStatisticsByFTBByLocations
    /// <summary>
    /// Formato para el reporte StatisticsByFTB ByLocations
    /// </summary>
    /// <history>
    /// [michan] 22/07/2016 Created
    /// [edgrodriguez] 05/09/2016 Modified. Se agregó una bandera para indicar si el reporte sera por equipos o individual.
    ///                                     Se cambio el formato.
    /// </history>
    internal static ExcelFormatItemsList RptStatisticsByFTBByLocations(bool ByTeam)
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Team", "Team", format: EnumFormatTypeExcel.General, isGroup:ByTeam, isVisible: false);
      lst.Add("Locations", "Locations", format: EnumFormatTypeExcel.General, isGroup: true, isVisible: false);
      lst.Add("ID", "SalemanID", format: EnumFormatTypeExcel.General);
      lst.Add("Salesman Name", "SalemanName", format: EnumFormatTypeExcel.General);

      lst.Add("UPS", "UPS", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("OWN", "Own", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("WithCloser", "WithCloser", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("AsCloser", "AsCloser", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Total", "Total", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);

      lst.Add("Sale", "Sales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Exit", "Exit", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Total ", "TotalSales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("EFF", "Efficiency", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, function: DataFieldFunctions.Sum);
      lst.Add("AWS", "SaleAverage", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Status", "SalesmanStatus", format: EnumFormatTypeExcel.General, isVisible: false);
      lst.Add("Post", "PostName", format: EnumFormatTypeExcel.General, isVisible: false);
      return lst;
    }
    #endregion RptStatisticsByFTB

    #region RptStatisticsByFTBByCategories
    /// <summary>
    /// Formato para el reporte StatisticsByFTB ByLocations
    /// </summary>
    /// <history>
    /// [michan] 22/07/2016 Created
    /// [edgrodriguez] 05/09/2016 Modified. Se agregó una bandera para indicar si el reporte sera por equipos o individual.
    ///                                     Se cambio el formato.
    /// </history>
    internal static ExcelFormatItemsList RptStatisticsByFTBByCategories(bool byTeam)
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Team", "Team", format: EnumFormatTypeExcel.General, isGroup: byTeam, isVisible: false);
      lst.Add("Locations", "Locations", format: EnumFormatTypeExcel.General, isGroup: true, isVisible: false);

      lst.Add("ID", "SalemanID", format: EnumFormatTypeExcel.General);
      lst.Add("Salesman Name", "SalemanName", format: EnumFormatTypeExcel.General);

      lst.Add("UPS", "UPS", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("OWN", "Own", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("WithCloser", "WithCloser", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("AsCloser", "AsCloser", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Total", "Total", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);

      lst.Add("Sale", "Sales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Exit", "Exit", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Total ", "TotalSales", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("EFF", "Efficiency", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, function: DataFieldFunctions.Sum);
      lst.Add("AWS", "SaleAverage", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Status", "SalesmanStatus", format: EnumFormatTypeExcel.General,  isVisible: false);
      lst.Add("Post", "PostName", format: EnumFormatTypeExcel.General,  isVisible: false);

      return lst;
    }
    #endregion RptStatisticsByFTB
        
    #region RptEfficiencyWeekly
    /// <summary>
    /// Retorna el formato para el reporte Rpt Efficiency Weekly
    /// </summary>
    /// <history>
    ///   [ecanul] 16/08/2016 Created
    /// </history>
    public static ExcelFormatItemsList RptEfficiencyWeekly()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Efficiency Type", "EfficiencyType", isGroup: true, isVisible: false);
      lst.Add("ID", "SalemanID");
      lst.Add("Salesman Name", "SalemanName");
      return lst;
    }
    #endregion

    #endregion
  }
}

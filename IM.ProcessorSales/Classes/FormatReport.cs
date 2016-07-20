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
    public static List<ExcelFormatTable> RptStatisticsBySalesRoomLocation()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable()
        {
          Title = "Zona",
          Order = 0,
          Axis = ePivotFieldAxis.Row,
          Compact = true,
          Outline = true,
          SubTotalFunctions = eSubTotalFunctions.Default,
          InsertBlankRow = true
        },
        new ExcelFormatTable()
        {
          Title = "Sales Room",
          Order = 1,
          Axis = ePivotFieldAxis.Row,
          Compact = true,
          Outline = true,
          SubTotalFunctions = eSubTotalFunctions.Default,
          InsertBlankRow = true
        },
        new ExcelFormatTable() {Title = "Program", Order = 2, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable() {Title = "Sales Room ID", Order = 3, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable() {Title = "Location Id", Order = 4, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable() {Title = "Location", Order = 5, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable()
        {
          Title = "Volume",
          Order = 0,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber
        },
        new ExcelFormatTable()
        {
          Title = "Shows",
          Order = 1,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Number
        },
        new ExcelFormatTable()
        {
          Title = "VIP",
          Order = 2,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Number
        },
        new ExcelFormatTable()
        {
          Title = "Reg",
          Order = 3,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Number
        },
        new ExcelFormatTable()
        {
          Title = "Exit",
          Order = 4,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Number
        },
        new ExcelFormatTable()
        {
          Title = "Total",
          Order = 5,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Number
        },
        new ExcelFormatTable()
        {
          Title = "OOP",
          Order = 9,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Number
        },
        new ExcelFormatTable
        {
          Title = "C%",
          Order = 6,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Percent,
          Formula = "IF(Shows =0,0,Total/Shows)"
        },
        new ExcelFormatTable
        {
          Title = "Eff",
          Order = 7,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Formula = "IF(Shows =0,0,Volume/Shows)"
        },
        new ExcelFormatTable
        {
          Title = "AV/S",
          Order = 8,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Formula = "IF(Total =0,0,Volume/Total)"
        }
      };
    }

    #endregion

    #region RptStatisticsByLocation
    /// <summary>
    /// Formato para el reporte Statics by Location 
    /// </summary>
    /// <history>
    /// [ecanul] 09/05/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptStatisticsByLocation()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() {Title = "Location", Axis = ePivotFieldAxis.Row, Order = 0},
        new ExcelFormatTable()
        {
          Title = "Volume",
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Order = 0,
          Function = DataFieldFunctions.Sum,
          SubtotalWithCero = true
        },
        new ExcelFormatTable()
        {
          Title = "Shows",
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Number,
          Order = 1,
          Function = DataFieldFunctions.Sum,
          SubtotalWithCero = true
        },
        new ExcelFormatTable()
        {
          Title = "VIP",
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Number,
          Order = 2,
          Function = DataFieldFunctions.Sum,
          SubtotalWithCero = true
        },
        new ExcelFormatTable()
        {
          Title = "Reg",
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Number,
          Order = 3,
          Function = DataFieldFunctions.Sum,
          SubtotalWithCero = true
        },
        new ExcelFormatTable()
        {
          Title = "Exit",
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Number,
          Order = 4,
          Function = DataFieldFunctions.Sum,
          SubtotalWithCero = true
        },
        new ExcelFormatTable()
        {
          Title = "Total",
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Number,
          Order = 5,
          Function = DataFieldFunctions.Sum,
          SubtotalWithCero = true
        },
        new ExcelFormatTable()
        {
          Title = "OOP",
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Order = 9,
          Function = DataFieldFunctions.Sum,
          SubtotalWithCero = true
        },

        new ExcelFormatTable
        {
          Title = "C%",
          Order = 6,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Percent,
          Formula = "IF(Shows =0,0,Total/Shows)"
        },
        new ExcelFormatTable
        {
          Title = "Eff",
          Order = 7,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Formula = "IF(Shows =0,0,Volume/Shows)"
        },
        new ExcelFormatTable
        {
          Title = "AV/S",
          Order = 8,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Formula = "IF(Total =0,0,Volume/Total)"
        }
      };
    }

    #endregion

    #region RptStatisticsByLocationMonthly
    /// <summary>
    /// Formato para el reporte Statics by Location Monthly
    /// </summary>
    /// <history>
    /// [ecanul] 10/05/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptStatisticsByLocationMonthly()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() {Title = "Program", Order = 0, Axis = ePivotFieldAxis.Row, Compact = true, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default, InsertBlankRow = true},
        new ExcelFormatTable() { Title = "Location", Order = 1, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable() {Title = "Volume Previous", Order = 0, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable() {Title = "Shows Previous", Order = 1, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable() {Title = "Goal", Order = 2, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable() {Title = "Books", Order = 3, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable() {Title = "Shows ", Order = 4, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number },
        new ExcelFormatTable() {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable() {Title = "Total Shows", Order = 7, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable() {Title = "Volume", Order = 8, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable() {Title = "Sales", Order = 9, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable()
        {
          Title = "Sh%",
          Order = 5,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Percent,
          Formula = "IF('Books'=0,0,'Shows '/'Books')"
        },
        new ExcelFormatTable
        {
          Title = "EFF",
          Order = 10,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Formula = "IF('Total Shows'=0,0,'Volume'/'Total Shows')"
        },
        new ExcelFormatTable
        {
          Title = "C%",
          Order = 11,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Percent,
          Formula = "IF('Total Shows'=0,0,'Sales'/'Total Shows')"
        },
        new ExcelFormatTable
        {
          Title = "AV/S",
          Order = 12,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Formula = "IF(Sales =0,0,Volume/Sales)"
        }
      };
    }
    #endregion

    #region RptSalesByLocationMonthly
    /// <summary>
    /// Formato para el reporte Sales by Location Monthly
    /// </summary>
    /// <history>
    /// [ecanul] 10/05/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptSalesByLocationMonthly()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() { Title = "Location", Order = 0, Axis = ePivotFieldAxis.Row, Compact = true, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default, InsertBlankRow = true},
        new ExcelFormatTable() { Title = "Year", Order = 1, Axis = ePivotFieldAxis.Row, Compact = true, Outline = true},
        new ExcelFormatTable() {Title = "Month", Order = 2, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable() {Title = "Shows", Order = 0, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number },
        new ExcelFormatTable() {Title = "Sales", Order = 1, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable() {Title = "Volume", Order = 2, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable() {Title = "Cancel", Order = 3, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable() {Title = "V/N after CXL", Order = 4, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable
        {
          Title = "AV/S",
          Order = 5,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Formula = "IF('Sales' =0,0,'V/N after CXL'/'Sales')"
        },
        new ExcelFormatTable
        {
          Title = "C%",
          Order = 6,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Percent,
          Formula = "IF(Shows=0,0,Sales/Shows)"
        },
        new ExcelFormatTable
        {
          Title = "EFF",
          Order = 7,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Formula = "IF('Shows' =0,0,'V/N after CXL'/'Shows')"
        }
      };
    }
    #endregion

    #region RptConcentrateDailySales
    /// <summary>
    /// Formato para el reporte RptConcentrateDailySales
    /// </summary>
    /// <history>
    /// [ecanul] 13/05/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptConcentrateDailySales()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() {Title = "SalesRoom", Order = 0, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable() {Title = "Goal", Order = 0, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumberWithCero },
        new ExcelFormatTable() {Title = "Diference", Order = 1, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumberWithCero},
        new ExcelFormatTable() {Title = "UPS", Order = 2, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.NumberWithCero },
        new ExcelFormatTable() {Title = "Sales", Order = 3, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.NumberWithCero},
        new ExcelFormatTable() {Title = "Proc", Order = 4, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumberWithCero},
        new ExcelFormatTable() {Title = "OPP", Order = 5, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumberWithCero},
        new ExcelFormatTable() {Title = "Fall", Order = 6, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumberWithCero},
        new ExcelFormatTable() {Title = "Cxld", Order = 7, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumberWithCero},
        new ExcelFormatTable() {Title = "Total Proc", Order = 8, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumberWithCero},
        new ExcelFormatTable() {Title = "Pact", Order = 12, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.PercentWithCero},
        new ExcelFormatTable() {Title = "Collect", Order = 13, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.PercentWithCero},
        new ExcelFormatTable
        {
          Title = "C%",
          Order = 9,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Percent,
          Formula = "IF(UPS=0,0,Sales/UPS)"
        },
        new ExcelFormatTable
        {
          Title = "EFF",
          Order = 10,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Formula = "IF('UPS' =0,0,'Total Proc'/'UPS')"
        },
        new ExcelFormatTable
        {
          Title = "AV/S",
          Order = 11,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Formula = "IF('Sales' =0,0,'Total Proc'/'Sales')"
        }
      };
    }
    #endregion

    #region RptDailySales
    /// <summary>
    /// Formato para el reporte RptConcentrateDailySales
    /// </summary>
    /// <history>
    /// [ecanul] 16/05/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptDailySales()
    {
      return new List<ExcelFormatTable>()
      {
        //new ExcelFormatTable() {Title = "Rasd", Order = 0, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable() {Title = "Date", Order = 0, Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Date},
        new ExcelFormatTable() {Title = "UPS", Order = 0, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number },
        new ExcelFormatTable() {Title = "Sale", Order = 1, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable() {Title = "Exit", Order = 2, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable() {Title = "VIP", Order = 3, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable() {Title = "Total Sale", Order = 4, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable() {Title = "Proc ", Order = 5, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable() {Title = "OPP", Order = 6, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable() {Title = "Fall", Order = 7, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable() {Title = "Cxld", Order = 8, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable() {Title = "Total Proc", Order = 9, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable() {Title = "Pact", Order = 13, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency,},
        new ExcelFormatTable() {Title = "Collect", Order = 14, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency},
        new ExcelFormatTable
        {
          Title = "C%",
          Order = 10,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Percent,
          Formula = "IF(UPS=0,0,Sale/UPS)"
        },
        new ExcelFormatTable
        {
          Title = "EFF",
          Order = 11,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Formula = "IF('UPS' =0,0,'Total Proc'/'UPS')"
        },
        new ExcelFormatTable
        {
          Title = "AV/S",
          Order = 12,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          Formula = "IF('Total Sale' =0,0,'Total Proc'/'Total Sale')"
        },
        new ExcelFormatTable
        {
          Title = "Pact Factor",
          Order = 15,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Percent,
          Formula = "IF('Pact'=0,0,IF('Total Proc'=0,0,'Pact'/1.1/'Total Proc'))"
        },
         new ExcelFormatTable
        {
          Title = "Collect Factor",
          Order = 15,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Percent,
          Formula = "IF('Collect'=0,0,IF('Total Proc'=0,0,'Collect'/1.1/'Total Proc'))"
        }
      };
    }
    #endregion

    #region RptManifest
    /// <summary>
    /// Formato para el reporte RptManifest
    /// </summary>
    /// <history>
    /// [ecanul] 07/06/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptManifest()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Format = EnumFormatTypeExcel.Id, Axis = ePivotFieldAxis.Row, Order = 1},
        new ExcelFormatTable() { Title = "Last Name", PropertyName = "guLastName1", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2},
        new ExcelFormatTable() { Title = "First Name", PropertyName = "guFirstName1", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 3},
        new ExcelFormatTable()
        {
          Title = "Sh", PropertyName = "guShow",
          Format = EnumFormatTypeExcel.General,
          Axis = ePivotFieldAxis.Values,
          SubTotalFunctions = eSubTotalFunctions.Count,
          Order = 4
        },
        new ExcelFormatTable()
        {
          Title = "Tr", PropertyName = "guTour",
          Format = EnumFormatTypeExcel.General,
          Axis = ePivotFieldAxis.Values,
          SubTotalFunctions = eSubTotalFunctions.Count,
          Order = 5
        },
        new ExcelFormatTable()
        {
          Title = "WO", PropertyName = "guWalkout",
          Format = EnumFormatTypeExcel.General,
          Axis = ePivotFieldAxis.Values,
          SubTotalFunctions = eSubTotalFunctions.Count,
          Order = 6
        },
        new ExcelFormatTable()
        {
          Title = "CT", PropertyName = "guCTour",
          Format = EnumFormatTypeExcel.General,
          Axis = ePivotFieldAxis.Values,
          SubTotalFunctions = eSubTotalFunctions.Count,
          Order = 7
        },
        new ExcelFormatTable()
        {
          Title = "Sve", PropertyName = "guSaveProgram",
          Format = EnumFormatTypeExcel.General,
          Axis = ePivotFieldAxis.Values,
          SubTotalFunctions = eSubTotalFunctions.Count,
          Order = 8
        },
        new ExcelFormatTable()
        {
          Title = "SG", PropertyName = "guSelfGen",
          Format = EnumFormatTypeExcel.General,
          Axis = ePivotFieldAxis.Values,
          SubTotalFunctions = eSubTotalFunctions.Count,
          Order = 9
        },
        new ExcelFormatTable()
        {
          Title = "Overfl", PropertyName = "guOverflow",
          Format = EnumFormatTypeExcel.General,
          Axis = ePivotFieldAxis.Values,
          SubTotalFunctions = eSubTotalFunctions.Count,
          Order = 10
        },
        new ExcelFormatTable() { Title = "Date", PropertyName = "salesmanDate", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 11 },
        new ExcelFormatTable()
        {
          Title = "Sold", PropertyName = "sold",
          Format = EnumFormatTypeExcel.General,
          Axis = ePivotFieldAxis.Values,
          SubTotalFunctions = eSubTotalFunctions.Count,
          Order = 12
        },
        new ExcelFormatTable()
        {
          Title = "Sale", PropertyName = "sale",
          Format = EnumFormatTypeExcel.General,
          Axis = ePivotFieldAxis.Values,
          SubTotalFunctions = eSubTotalFunctions.Count,
          Order = 13
        },
        new ExcelFormatTable()
        {
          Title = "Sale Type", PropertyName = "SaleType",
          Format = EnumFormatTypeExcel.Number,
          IsVisible = false
        },
        new ExcelFormatTable()
        {
          Title = "Sale TypeN", PropertyName = "SaleTypeN",
          Format = EnumFormatTypeExcel.General,
          Axis = ePivotFieldAxis.Values,
          Order = 1,
          IsGroup = true
        },
        new ExcelFormatTable()
        {
          Title = "Sale ID", PropertyName = "saID",
          Format = EnumFormatTypeExcel.Id,
          Axis = ePivotFieldAxis.Values,
          Order = 14
        },
        new ExcelFormatTable()
        {
          Title = "Memb. #", PropertyName = "saMembershipNum",
          Format = EnumFormatTypeExcel.Id,
          Axis = ePivotFieldAxis.Row,
          Order = 15
        },
        new ExcelFormatTable()
        {
          Title = "M. Type", PropertyName = "samt",
          Format = EnumFormatTypeExcel.General,
          Axis = ePivotFieldAxis.Row,
          Order = 16
        },
        new ExcelFormatTable()
        {
          Title = "Proc", PropertyName = "procSales",
          Format = EnumFormatTypeExcel.Number,
          Axis = ePivotFieldAxis.Values,
          SubTotalFunctions = eSubTotalFunctions.Sum,
          Order = 17
        },
        new ExcelFormatTable()
        {
          Title = "Proc. Amount", PropertyName = "saGrossAmount",
          Format = EnumFormatTypeExcel.Currency,
          Axis = ePivotFieldAxis.Values,
          SubTotalFunctions = eSubTotalFunctions.Sum,
          Order = 18
        },
        //PR1
        new ExcelFormatTable() { Title = "ID", PropertyName = "PR1", Axis = ePivotFieldAxis.Values, Order=19, SuperHeader = "PR 1"},
        new ExcelFormatTable() {Title="Name",PropertyName="PR1N", Axis= ePivotFieldAxis.Row,Order=20, SuperHeader = "PR 1"},
        new ExcelFormatTable() {Title="Post",PropertyName="PR1P", Axis= ePivotFieldAxis.Row,Order=21, SuperHeader = "PR 1"},
        //PR2
        new ExcelFormatTable() {Title="ID ",PropertyName="PR2", Axis= ePivotFieldAxis.Row,Order=22, SuperHeader = "PR 2"},
        new ExcelFormatTable() {Title="Name ",PropertyName="PR2N", Axis= ePivotFieldAxis.Row,Order=23, SuperHeader = "PR 2"},
        new ExcelFormatTable() {Title="Post ",PropertyName="PR2P", Axis= ePivotFieldAxis.Row,Order=24, SuperHeader = "PR 2"},
        //PR3
        new ExcelFormatTable() {Title="ID  ",PropertyName="PR3", Axis= ePivotFieldAxis.Row,Order=25, SuperHeader = "PR 3"},
        new ExcelFormatTable() {Title="Name  ",PropertyName="PR3N", Axis= ePivotFieldAxis.Row,Order=26, SuperHeader = "PR 3"},
        new ExcelFormatTable() {Title="Post  ",PropertyName="PR3P", Axis= ePivotFieldAxis.Row,Order=27, SuperHeader = "PR 3"},
        //Liner1
        new ExcelFormatTable() {Title=" ID",PropertyName="Liner1", Format = EnumFormatTypeExcel.Id, Axis= ePivotFieldAxis.Row,Order=28, SuperHeader = "Liner 1"},
        new ExcelFormatTable() {Title=" Name",PropertyName="Liner1N", Axis= ePivotFieldAxis.Row,Order=29, SuperHeader = "Liner 1" },
        new ExcelFormatTable() {Title=" Post",PropertyName="Liner1P", Axis= ePivotFieldAxis.Row,Order=30, SuperHeader = "Liner 1" },
        //Liner2
        new ExcelFormatTable() {Title="  ID",PropertyName="Liner2", Format = EnumFormatTypeExcel.Id, Axis= ePivotFieldAxis.Row,Order=31, SuperHeader = "Liner 2"},
        new ExcelFormatTable() {Title="  Name",PropertyName="Liner2N", Axis= ePivotFieldAxis.Row,Order=32, SuperHeader = "Liner 2"},
        new ExcelFormatTable() {Title="  Post",PropertyName="Liner2P", Axis= ePivotFieldAxis.Row,Order=33, SuperHeader = "Liner 2"},
        //Closer1
        new ExcelFormatTable() {Title=" ID ",PropertyName="Closer1", Format = EnumFormatTypeExcel.Id, Axis= ePivotFieldAxis.Row,Order=34, SuperHeader = "Closer 1"},
        new ExcelFormatTable() {Title=" Name ",PropertyName="Closer1N", Axis= ePivotFieldAxis.Row,Order=35, SuperHeader = "Closer 1"},
        new ExcelFormatTable() {Title=" Post ",PropertyName="Closer1P", Axis= ePivotFieldAxis.Row,Order=36, SuperHeader = "Closer 1"},
        //Closer2
        new ExcelFormatTable() {Title="  ID  ",PropertyName="Closer2", Format = EnumFormatTypeExcel.Id, Axis= ePivotFieldAxis.Row,Order=37, SuperHeader = "Closer 2"},
        new ExcelFormatTable() {Title="  Name  ",PropertyName="Closer2N", Axis= ePivotFieldAxis.Row,Order=38, SuperHeader = "Closer 2"},
        new ExcelFormatTable() {Title="  Post  ",PropertyName="Closer2p", Axis= ePivotFieldAxis.Row,Order=39, SuperHeader = "Closer 2"},
        //Closer3
        new ExcelFormatTable() {Title="ID   ",PropertyName="Closer3", Format = EnumFormatTypeExcel.Id, Axis= ePivotFieldAxis.Row,Order=40, SuperHeader = "Closer 3"},
        new ExcelFormatTable() {Title="Name   ",PropertyName="Closer3N", Axis= ePivotFieldAxis.Row,Order=41, SuperHeader = "Closer 3"},
        new ExcelFormatTable() {Title="Post   ",PropertyName="Closer3p", Axis= ePivotFieldAxis.Row,Order=42, SuperHeader = "Closer 3"},
        //Exit1
        new ExcelFormatTable() {Title="   ID",PropertyName="Exit1", Format = EnumFormatTypeExcel.Id, Axis= ePivotFieldAxis.Row,Order=43, SuperHeader = "Exit 1"},
        new ExcelFormatTable() {Title="   Name",PropertyName="Exit1N", Axis= ePivotFieldAxis.Row,Order=44, SuperHeader = "Exit 1"},
        new ExcelFormatTable() {Title="   Post",PropertyName="Exit1P", Axis= ePivotFieldAxis.Row,Order=45, SuperHeader = "Exit 1"},
        //Exit2
        new ExcelFormatTable() {Title="   ID   ",PropertyName="Exit2", Format = EnumFormatTypeExcel.Id, Axis= ePivotFieldAxis.Values,Order=46, SuperHeader = "Exit 2"},
        new ExcelFormatTable() {Title="   Name   ",PropertyName="Exit2N", Axis= ePivotFieldAxis.Row,Order=47, SuperHeader = "Exit 2"},
        new ExcelFormatTable() {Title="   Post   ",PropertyName="Exit2P", Axis= ePivotFieldAxis.Row,Order=48, SuperHeader = "Exit 2"}
      };
    }
    #endregion
    
    #region RptFTMInOutHouse
    /// <summary>
    /// Formato para el reporte RptFtmIn&OutHouse
    /// </summary>
    /// <history>
    /// [ecanul] 04/07/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptFTMInOutHouse()
    {
      return new List<ExcelFormatTable>()
      {
         new ExcelFormatTable()
        {
          Title = " ", PropertyName = "GR",
          Format = EnumFormatTypeExcel.Boolean,
          Axis = ePivotFieldAxis.Values,
          Order = 1,
          IsGroup = true
        },
        new ExcelFormatTable() { Title = "ID", PropertyName = "Liner", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1},
        new ExcelFormatTable() { Title = "Salesman Name", PropertyName = "peN", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2},
        new ExcelFormatTable() { Title = "OOP", PropertyName = "OOP", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 3 ,SubTotalFunctions = eSubTotalFunctions.Count },
        //OVERFLOW
        new ExcelFormatTable() { Title = "VOL", PropertyName = "OFSalesAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 4, SuperHeader = "Overflow",SubTotalFunctions = eSubTotalFunctions.Count },
        new ExcelFormatTable() { Title = "UPS", PropertyName = "OFShows", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 5, SuperHeader = "Overflow" ,SubTotalFunctions = eSubTotalFunctions.Count},
        new ExcelFormatTable() { Title = "Sale", PropertyName = "OFSales", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 6, SuperHeader = "Overflow",SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable() { Title = "Exit", PropertyName = "OFExit", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 7, SuperHeader = "Overflow",SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable() { Title = "Total", PropertyName = "OFTotal", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 8, SuperHeader = "Overflow",SubTotalFunctions = eSubTotalFunctions.Sum },
        //REGEN
        new ExcelFormatTable() { Title = "VOL ", PropertyName = "RSalesAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 9, SuperHeader = "Regen" ,SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable() { Title = "UPS ", PropertyName = "RShows", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 10, SuperHeader = "Regen",SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable() { Title = "Sale ", PropertyName = "RSales", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 11, SuperHeader = "Regen",SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable() { Title = "Exit ", PropertyName = "RExit", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 12, SuperHeader = "Regen",SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable() { Title = "Total ", PropertyName = "RTotal", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 13, SuperHeader = "Regen",SubTotalFunctions = eSubTotalFunctions.Sum },
         //Normal
        new ExcelFormatTable() { Title = " VOL", PropertyName = "NSalesAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, SuperHeader = "Normal",SubTotalFunctions = eSubTotalFunctions.Sum },
        new ExcelFormatTable() { Title = " UPS", PropertyName = "NShows", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 15, SuperHeader = "Normal",SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable() { Title = " Sale", PropertyName = "NSales", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 16, SuperHeader = "Normal",SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable() { Title = " Exit", PropertyName = "NExit", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 17, SuperHeader = "Normal",SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable() { Title = " Total", PropertyName = "NTotal", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 18, SuperHeader = "Normal",SubTotalFunctions = eSubTotalFunctions.Sum },
        //Total
        new ExcelFormatTable() { Title = " VOL ", PropertyName = "TSalesAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, SuperHeader = "Total",SubTotalFunctions = eSubTotalFunctions.Sum  },
        new ExcelFormatTable() { Title = " UPS ", PropertyName = "TShows", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 20, SuperHeader = "Total",SubTotalFunctions = eSubTotalFunctions.Sum },
        new ExcelFormatTable() { Title = " Sale ", PropertyName = "TSales", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 21, SuperHeader = "Total" ,SubTotalFunctions = eSubTotalFunctions.Sum },
        new ExcelFormatTable() { Title = " Exit ", PropertyName = "TExit", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 22, SuperHeader = "Total" ,SubTotalFunctions = eSubTotalFunctions.Sum },
        new ExcelFormatTable() { Title = " Total ", PropertyName = "TTotal", Format = EnumFormatTypeExcel.DecimalNumber, Axis = ePivotFieldAxis.Values, Order = 23, SuperHeader = "Total"  ,SubTotalFunctions = eSubTotalFunctions.Sum },

        new ExcelFormatTable
        {
          Title = "EFF Overf",
          PropertyName = "EFFOver",
          Order = 24,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          IsCalculated = true,
          Formula = "IF([NShows]=0,0,[TSalesAmount]/[NShows])"
        },

        new ExcelFormatTable
        {
          Title = "EFF",
          PropertyName = "EFF",
          Order = 25,
          Axis = ePivotFieldAxis.Values,
          IsCalculated = true,
          Formula = "IF([TShows]=0,0,[TSalesAmount]/[TShows])"
        },

        new ExcelFormatTable
        {
          Title = "C%",
          PropertyName = "CPer",
          Order = 26,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.Percent,
          IsCalculated = true,
          Formula = "IF([TShows]=0,0,[TTotal]/[TShows])"
         // Formula = "IF(' UPS '=0,0,' Total '/ ' UPS ')"
        },

        new ExcelFormatTable
        {
          Title = "AV/S",
          PropertyName = "AVS",
          Order = 27,
          Axis = ePivotFieldAxis.Values,
          Format = EnumFormatTypeExcel.DecimalNumber,
          IsCalculated = true,
          Formula = "IF([TTotal]=0,0,[TSalesAmount]/[TTotal])"
          //Formula = "IF(' Sale '=0,0,' VOL '/' Sale ')"
        }
      };
    }
    #endregion

    #region RptStatisticsBySegments
    /// <summary>
    /// Formato para el reporte StatsBySegment
    /// </summary>
    /// <history>
    /// [aalcocer] 04/07/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> RptStatisticsBySegments()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Segment",Order = 0, Axis = ePivotFieldAxis.Column, SubTotalFunctions = eSubTotalFunctions.Default},

        new ExcelFormatTable {Title = "Salesman Type", Order = 0,  Axis = ePivotFieldAxis.Row, Compact = true, Outline = true, InsertBlankRow = true, SubTotalFunctions = eSubTotalFunctions.Default, Sort = eSortType.Ascending},
        new ExcelFormatTable {Title = "ID", Order = 1, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Salesman Name", Order = 2, Axis = ePivotFieldAxis.Row},

        new ExcelFormatTable {Title = "UPS", Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable {Title = "Sales", Order = 4, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable {Title = "Amount", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency},

        new ExcelFormatTable { Title = "Efficiency", Order = 6, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('UPS' =0,0,'Amount'/'UPS')" },
        new ExcelFormatTable { Title = "%", Order = 7, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('UPS' =0,0,'Sales'/'UPS')" }
      };
    }
    #endregion

    #region RptStatisticsBySegmentsGroupedByTeams
    /// <summary>
    /// Formato para el reporte StatsBySegment agrupado por Teams
    /// </summary>
    /// <history>
    /// [aalcocer] 04/07/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> RptStatisticsBySegmentsGroupedByTeams()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Segment",Order = 0, Axis = ePivotFieldAxis.Column, SubTotalFunctions = eSubTotalFunctions.Default},

        new ExcelFormatTable {Title = "Team", Order = 0,  Axis = ePivotFieldAxis.Row, Compact = true, Outline = true, InsertBlankRow = true, SubTotalFunctions = eSubTotalFunctions.Default},
        new ExcelFormatTable {Title = "Status", Order = 1, Axis = ePivotFieldAxis.Row, Compact = true, Outline = true, InsertBlankRow = true, SubTotalFunctions = eSubTotalFunctions.Default},
        new ExcelFormatTable {Title = "Salesman Type", Order = 2,  Axis = ePivotFieldAxis.Row, Compact = true, Outline = true, InsertBlankRow = true, SubTotalFunctions = eSubTotalFunctions.Default, Sort = eSortType.Ascending},
        new ExcelFormatTable {Title = "ID", Order = 3, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Salesman Name", Order = 4, Axis = ePivotFieldAxis.Row},

        new ExcelFormatTable {Title = "UPS", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable {Title = "Sales", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable {Title = "Amount", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency},

        new ExcelFormatTable { Title = "Efficiency", Order = 8, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('UPS' =0,0,'Amount'/'UPS')" },
        new ExcelFormatTable { Title = "%", Order = 9, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('UPS' =0,0,'Sales'/'UPS')" }
      };
    }
    #endregion

    #region RptStatisticsByCloser
    /// <summary>
    /// Formato para el reporte StatisticsByCloser
    /// </summary>
    /// <history>
    /// [aalcocer] 04/07/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> RptStatisticsByCloser()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "SalemanType",Order = 0, Axis = ePivotFieldAxis.Column},

        new ExcelFormatTable {Title = "ID", Order = 0, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Salesman Name", Order = 1, Axis = ePivotFieldAxis.Row},

        new ExcelFormatTable {Title = "VOL", Order = 2, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency},
        new ExcelFormatTable {Title = "OPP", Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable {Title = "UPS", Order = 4, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable {Title = "Sale", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable {Title = "Exit", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable {Title = "Total", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        
        new ExcelFormatTable { Title = "EFF", Order = 8, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('UPS' =0,0,'VOL'/'UPS')" },
        new ExcelFormatTable { Title = "C%", Order = 9, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('UPS' =0,0,'Total'/'UPS')" },
        new ExcelFormatTable { Title = "AV/S", Order = 10, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Total' =0,0,'VOL'/'Total')" }
      };
    }
    #endregion

    #region RptStatisticsByCloserGroupedByTeams
    /// <summary>
    /// Formato para el reporte StatisticsByCloser agrupado por Teams
    /// </summary>
    /// <history>
    /// [aalcocer] 04/07/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> RptStatisticsByCloserGroupedByTeams()
    {
      return new List<ExcelFormatTable>
      {
         new ExcelFormatTable{Title = "SalemanType",Order = 0, Axis = ePivotFieldAxis.Column},

        new ExcelFormatTable {Title = "Team", Order = 0,  Axis = ePivotFieldAxis.Row, Compact = true, Outline = true, InsertBlankRow = true, SubTotalFunctions = eSubTotalFunctions.Default},
        new ExcelFormatTable {Title = "Status", Order = 1, Axis = ePivotFieldAxis.Row, Compact = true, Outline = true, InsertBlankRow = true, SubTotalFunctions = eSubTotalFunctions.Default},        
        new ExcelFormatTable {Title = "ID", Order = 2, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Salesman Name", Order = 3, Axis = ePivotFieldAxis.Row},

        new ExcelFormatTable {Title = "VOL", Order = 2, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency},
        new ExcelFormatTable {Title = "OPP", Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable {Title = "UPS", Order = 4, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable {Title = "Sale", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable {Title = "Exit", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},
        new ExcelFormatTable {Title = "Total", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber},

        new ExcelFormatTable { Title = "EFF", Order = 8, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('UPS' =0,0,'VOL'/'UPS')" },
        new ExcelFormatTable { Title = "C%", Order = 9, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('UPS' =0,0,'Total'/'UPS')" },
        new ExcelFormatTable { Title = "AV/S", Order = 10, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Total' =0,0,'VOL'/'Total')" }
      };
    }
    #endregion RptStatisticsByCloserGroupedByTeams

    #region RptStatisticsByExitCloser
    /// <summary>
    /// Formato para el reporte StatisticsByExitCloser
    /// </summary>
    /// <history>
    /// [aalcocer] 18/07/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> RptStatisticsByExitCloser()
    {
      return new List<ExcelFormatTable>
      {
        
        new ExcelFormatTable{Title = "SalesAmountRange", PropertyName = "SalesAmountRange", Order = 1, Axis = ePivotFieldAxis.Column, Format = EnumFormatTypeExcel.Currency},

        new ExcelFormatTable { Title = "SalemanType", PropertyName = "SalemanType", Axis = ePivotFieldAxis.Row, IsGroup= true, Order = 1 },

        new ExcelFormatTable {Title = "ID", PropertyName = "SalemanID", Order = 1, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Salesman Name", PropertyName = "SalemanName", Order = 2, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "VOL", PropertyName  = "SalesAmount", Order = 3, Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Currency, SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable {Title = "OPP", PropertyName  = "OPP", Order = 4, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.DecimalNumber, SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable {Title = "UPS", PropertyName  = "UPS", Order = 5, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.DecimalNumber, SubTotalFunctions = eSubTotalFunctions.Sum},        
        new ExcelFormatTable {Title = "Sales", PropertyName  = "Sales", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber, SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable {Title = "Total", PropertyName  = "SalesTotal", Order = 7, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.DecimalNumber, SubTotalFunctions = eSubTotalFunctions.Sum},

        new ExcelFormatTable { Title = "EFF", PropertyName  = "Efficiency", Order = 8, Format = EnumFormatTypeExcel.Currency, Formula = "IF([UPS] =0,0, [SalesAmount]/[UPS])", IsCalculated = true},
        new ExcelFormatTable { Title = "C%",PropertyName  = "ClosingFactor", Order = 9, Format = EnumFormatTypeExcel.Percent, Formula = "IF([UPS] =0,0,[SalesTotal]/[UPS])", IsCalculated = true},
        new ExcelFormatTable { Title = "AV/S", PropertyName  = "SaleAverage", Order = 10, Format = EnumFormatTypeExcel.Currency, Formula = "IF([SalesTotal] =0,0,[SalesAmount]/[SalesTotal])",IsCalculated = true}
      };
    }
    #endregion RptStatisticsByExitCloser

    #region RptStatisticsByExitCloserGroupedByTeams
    /// <summary>
    /// Formato para el reporte StatisticsByExitCloser agrupado por Teams
    /// </summary>
    /// <history>
    /// [aalcocer] 18/07/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> RptStatisticsByExitCloserGroupedByTeams()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "SalesAmountRange", PropertyName = "SalesAmountRange", Order = 1, Axis = ePivotFieldAxis.Column, Format = EnumFormatTypeExcel.Currency},

        new ExcelFormatTable { Title = "SalemanType", PropertyName = "SalemanType", Axis = ePivotFieldAxis.Row, IsGroup= true, Order = 1},
        new ExcelFormatTable {Title = "Team", PropertyName = "Team",  Axis = ePivotFieldAxis.Row, IsGroup= true, Order = 2},
        new ExcelFormatTable {Title = "Status", PropertyName = "SalesmanStatus",  Axis = ePivotFieldAxis.Row, IsGroup= true, Order = 3},

        new ExcelFormatTable {Title = "ID", PropertyName = "SalemanID", Order = 1, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Salesman Name", PropertyName = "SalemanName", Order = 2, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "VOL", PropertyName  = "SalesAmount", Order = 3, Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Currency, SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable {Title = "OPP", PropertyName  = "OPP", Order = 4, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.DecimalNumber, SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable {Title = "UPS", PropertyName  = "UPS", Order = 5, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.DecimalNumber, SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable {Title = "Sales", PropertyName  = "Sales", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.DecimalNumber, SubTotalFunctions = eSubTotalFunctions.Sum},
        new ExcelFormatTable {Title = "Total", PropertyName  = "SalesTotal", Order = 7, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.DecimalNumber, SubTotalFunctions = eSubTotalFunctions.Sum},

        new ExcelFormatTable { Title = "EFF", PropertyName  = "Efficiency", Order = 8, Format = EnumFormatTypeExcel.Currency, Formula = "IF([UPS] =0,0, [SalesAmount]/[UPS])", IsCalculated = true},
        new ExcelFormatTable { Title = "C%",PropertyName  = "ClosingFactor", Order = 9, Format = EnumFormatTypeExcel.Percent, Formula = "IF([UPS] =0,0,[SalesTotal]/[UPS])", IsCalculated = true},
        new ExcelFormatTable { Title = "AV/S", PropertyName  = "SaleAverage", Order = 10, Format = EnumFormatTypeExcel.Currency, Formula = "IF([SalesTotal] =0,0,[SalesAmount]/[SalesTotal])",IsCalculated = true}        
      };
    }
    #endregion RptStatisticsByExitCloserGroupedByTeams

    #endregion
  }
}

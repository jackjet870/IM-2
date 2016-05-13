using System.Collections.Generic;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
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
        new ExcelFormatTable() { Title = "SalesRoom", Order = 0, Axis = ePivotFieldAxis.Row},
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

    #endregion
  }
}

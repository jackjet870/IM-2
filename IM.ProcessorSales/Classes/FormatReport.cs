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

    public static List<ExcelFormatTable> RptStatisticsBySalesRoomLocation()
    {
      return new List<ExcelFormatTable>()
      {
        //new ExcelFormatTable() { Title = "Zona", Axis = ePivotFieldAxis.Row, Order = 1, IsGroup = true},
        //new ExcelFormatTable() { Title = "Location ID", Axis = ePivotFieldAxis.Column, Order = 4},
        //new ExcelFormatTable() {Title = "Location",Axis = ePivotFieldAxis.Column,Order = 5},
        //new ExcelFormatTable() {Title = "Sales Room ID",Axis = ePivotFieldAxis.Column,Order = 2},
        //new ExcelFormatTable() {Title = "Sales Room",Axis = ePivotFieldAxis.Column,Order = 3},
        //new ExcelFormatTable() {Title = "Program",Axis = ePivotFieldAxis.Column,Order = 1},
        //new ExcelFormatTable() {Title = "Shows",Axis = ePivotFieldAxis.Column,Order = 7, Format = EnumFormatTypeExcel.Number},
        //new ExcelFormatTable() {Title = "VIP",Axis = ePivotFieldAxis.Column,Order = 8, Format = EnumFormatTypeExcel.Number},
        //new ExcelFormatTable() {Title = "Reg",Axis = ePivotFieldAxis.Column,Order = 9, Format = EnumFormatTypeExcel.Number},
        //new ExcelFormatTable() {Title = "Exit",Axis = ePivotFieldAxis.Column,Order = 10, Format = EnumFormatTypeExcel.Number},
        //new ExcelFormatTable() {Title = "Total",Axis = ePivotFieldAxis.Column,Order = 11, Format = EnumFormatTypeExcel.Number},
        //new ExcelFormatTable() {Title = "Volume",Axis = ePivotFieldAxis.Column,Order = 6,Format = EnumFormatTypeExcel.Number},
        //new ExcelFormatTable() {Title = "C%",Axis = ePivotFieldAxis.Column,Order = 12, Format = EnumFormatTypeExcel.Percent},
        //new ExcelFormatTable() {Title = "OOP",Axis = ePivotFieldAxis.Column,Order = 15, Format = EnumFormatTypeExcel.Number},
        //new ExcelFormatTable() {Title = "EFF",Axis = ePivotFieldAxis.Column,Order = 13, Format = EnumFormatTypeExcel.Number},
        //new ExcelFormatTable() {Title = "AV/S",Axis = ePivotFieldAxis.Column,Order = 14, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable() {Title = "Zona", PropertyName  = "Zona", Order = 1,IsGroup = true},
        new ExcelFormatTable() {Title = "Location ID", PropertyName  = "LocationId", Order = 4},
        new ExcelFormatTable() {Title = "Location", PropertyName  = "Location", Order = 5},
        new ExcelFormatTable() {Title = "Sales Room ID", PropertyName  = "SalesRoomId", Order = 2},
        new ExcelFormatTable() {Title = "Sales Room", PropertyName  = "SalesRoom", Order = 2,IsGroup = true},
        new ExcelFormatTable() {Title = "Sales Room", PropertyName  = "SalesRoom", Order = 3},
        new ExcelFormatTable() {Title = "Program", PropertyName  = "Program", Order = 1},
        new ExcelFormatTable() {Title = "Shows", PropertyName  = "Shows", Order = 7, Format = EnumFormatTypeExcel.Number,SubTotalFunctions = eSubTotalFunctions.Sum, SubtotalWithCero = true},
        new ExcelFormatTable() {Title = "VIP", PropertyName  = "SalesVIP", Order = 8, Format = EnumFormatTypeExcel.Number,SubTotalFunctions = eSubTotalFunctions.Sum, SubtotalWithCero = true},
        new ExcelFormatTable() {Title = "Reg", PropertyName  = "SalesRegular", Order = 9, Format = EnumFormatTypeExcel.Number,SubTotalFunctions = eSubTotalFunctions.Sum, SubtotalWithCero = true},
        new ExcelFormatTable() {Title = "Exit", PropertyName  = "SalesExit", Order = 10, Format = EnumFormatTypeExcel.Number,SubTotalFunctions = eSubTotalFunctions.Sum, SubtotalWithCero = true},
        new ExcelFormatTable() {Title = "Total", PropertyName  = "Sales", Order = 11, Format = EnumFormatTypeExcel.Number,SubTotalFunctions = eSubTotalFunctions.Sum, SubtotalWithCero = true},
        new ExcelFormatTable() {Title = "Volume", PropertyName  = "SalesAmount", Order = 6, Format = EnumFormatTypeExcel.DecimalNumber,SubTotalFunctions = eSubTotalFunctions.Sum, SubtotalWithCero = true},
        new ExcelFormatTable() {Title = "C%", PropertyName  = "ClosingFactor", Order = 12, Format = EnumFormatTypeExcel.Percent,SubTotalFunctions = eSubTotalFunctions.Sum, SubtotalWithCero = true},
        new ExcelFormatTable() {Title = "OOP", PropertyName  = "SalesAmountOOP", Order = 15, Format = EnumFormatTypeExcel.DecimalNumber,SubTotalFunctions = eSubTotalFunctions.Sum, SubtotalWithCero = true},
        new ExcelFormatTable() {Title = "EFF", PropertyName  = "Efficiency", Order = 13, Format = EnumFormatTypeExcel.DecimalNumber,SubTotalFunctions = eSubTotalFunctions.Sum, SubtotalWithCero = true},
        new ExcelFormatTable() {Title = "AV/S", PropertyName  = "AverageSale", Order = 14, Format = EnumFormatTypeExcel.DecimalNumber,SubTotalFunctions = eSubTotalFunctions.Sum, SubtotalWithCero = true},
      };
    }

    #endregion

    #endregion
  }
}

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
        new ExcelFormatTable() { Title = "Sales man", PropertyName = "Salesman", Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1 },
        new ExcelFormatTable() { Title = "Name", PropertyName = "SalesmanN", Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2 },
        new ExcelFormatTable() { Title = "Post", PropertyName = "SalesmanPost", IsVisible = false },
        new ExcelFormatTable() { Title = "Post Name", PropertyName = "SalesmanPostN", Alignment = ExcelHorizontalAlignment.Left, IsGroup =  true, Order = 1 },
        new ExcelFormatTable() { Title = "Day Off", PropertyName = "DayOffList", Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 3 },
        new ExcelFormatTable() { Title = "Language", PropertyName = "Language", Alignment = ExcelHorizontalAlignment.Left, Axis= ePivotFieldAxis.Values, Order = 4 },
        new ExcelFormatTable() { Title = "Location", PropertyName = "Location", Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5},
        new ExcelFormatTable() { Title = "Time", PropertyName = "Time", Format = EnumFormatTypeExcel.Time, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6 },
        new ExcelFormatTable() { Title = "TimeN", PropertyName = "TimeN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Sort = eSortType.Ascending, Order = 1 },
        new ExcelFormatTable() { Title = "Amount Ytd", PropertyName = "AmountYtd", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 7 },
        new ExcelFormatTable() { Title = "Amount M", PropertyName = "AmountM", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 8 }
      };
    }
    #endregion
  }
}

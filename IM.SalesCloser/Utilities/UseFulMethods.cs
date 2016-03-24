using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace IM.SalesCloser.Utilities
{
  public class UseFulMethods
  {
    /// <summary>
    /// Genera las columnas que necesito en el reporte RPTStatistics
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    /// [erosado] 23/Mar/2016  Created
    /// </history>
    public static List<ExcelFormatTable> getExcelFormatTable()
    {
      List<ExcelFormatTable> formatColumns = new List<ExcelFormatTable>();
      formatColumns.Add(new ExcelFormatTable() { Title = "Sale ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "LS", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "SR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Member. #", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Sale Type", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Date", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });//Date
      formatColumns.Add(new ExcelFormatTable() { Title = "Proc Date", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });//Date
      formatColumns.Add(new ExcelFormatTable() { Title = "Last Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Closer", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Closer Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Closer 2", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Closer 2 Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Closer 3", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Closer 3 Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Exit", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Exit Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Exit 2", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Exit 2 Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Amount", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left });//Currency
      formatColumns.Add(new ExcelFormatTable() { Title = "Cxld", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });//Boolean
      formatColumns.Add(new ExcelFormatTable() { Title = "Cancel D", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });//Date
      return formatColumns;
    }
  }
}

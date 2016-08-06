using System.Collections.Generic;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
namespace IM.Assignment.Classes
{
  /// <summary>
  /// Clase para el formato de tablas de reportes
  /// Interaction logic for frmAssignment.xaml
  /// </summary>
  /// <history>
  ///   [vku] 31/Mar/2016 Created
  /// </history>
  public class clsFormatTable
  {
    #region Atributos
    public static List<ExcelFormatTable> format = new List<ExcelFormatTable>();
    #endregion

    #region getExcelFormatTableAssignByPR
    /// <summary>
    ///  Formato para el reporte de asignacion po PR
    /// </summary>
    /// <history>
    ///   [vku] 31/Mar/2016 Created
    ///   [vku] 20/Jul/2016 Modified. Elimine las columnas PR ID, PR N Assigned
    ///                               Se corrigio el orden de las columnas    
    /// </history>
    public static List<ExcelFormatTable> getExcelFormatTableAssignByPR()
    {
      format.Clear();
      format.Add(new ExcelFormatTable() { Title = "In", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Order = 0  });
      format.Add(new ExcelFormatTable() { Title = "Out", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Order = 1 });
      format.Add(new ExcelFormatTable() { Title = "Room", Format = EnumFormatTypeExcel.Id, Alignment = ExcelHorizontalAlignment.Left, Order = 2});
      format.Add(new ExcelFormatTable() { Title = "Last Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 3 });
      format.Add(new ExcelFormatTable() { Title = "First Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 4 });
      format.Add(new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.Id, Alignment = ExcelHorizontalAlignment.Left, Order = 5 });
      format.Add(new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 6 });
      format.Add(new ExcelFormatTable() { Title = "Member #", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 7 });
      format.Add(new ExcelFormatTable() { Title = "Pax", Format = EnumFormatTypeExcel.DecimalNumberWithCero, Alignment = ExcelHorizontalAlignment.Left, Order = 8 });
      format.Add(new ExcelFormatTable() { Title = "Comments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 9 });
      format.Add(new ExcelFormatTable() { Title = "Contract", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 10 });
      format.Add(new ExcelFormatTable() { Title = "Monday", Format = EnumFormatTypeExcel.DecimalNumberWithCero, Alignment = ExcelHorizontalAlignment.Left, Order = 11 });
      format.Add(new ExcelFormatTable() { Title = "Tuesday", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 12 });
      format.Add(new ExcelFormatTable() { Title = "Wednesday", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 13 });
      format.Add(new ExcelFormatTable() { Title = "Thursday", Format = EnumFormatTypeExcel.DecimalNumberWithCero, Alignment = ExcelHorizontalAlignment.Left, Order = 14 });
      format.Add(new ExcelFormatTable() { Title = "Friday", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 15 });
      format.Add(new ExcelFormatTable() { Title = "Saturday", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 16 });
      format.Add(new ExcelFormatTable() { Title = "Sunday", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 17 });
      return format;
    }
    #endregion

    #region getExcelFormatTableGenAsignyArvls
    /// <summary>
    /// Formato para el reporte de llegadas y asignacion
    /// </summary>
    /// <history>
    ///   [vku] 31/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> getExcelFormatTableGenAsignyArvls()
    {
      format.Clear();
      format.Add(new ExcelFormatTable() { Title = "ID", Format = EnumFormatTypeExcel.Id, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Check In D", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "In", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Room", Format = EnumFormatTypeExcel.Id, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Last Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "First Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "PR ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "PR N Assigned", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.Id, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Member #", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Amount", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Liner", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Closer", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      return format;
    }
    #endregion
  }
}

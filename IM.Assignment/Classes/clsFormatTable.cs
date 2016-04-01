using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
namespace IM.Assignment.Classes
{
  /// <summary>
  /// Clase para el formato de tablas de reportes
  /// Interaction logic for frmAssignment.xaml
  /// </summary>
  /// <history>
  ///   [vku] 31/03/2016 Created
  /// </history>
  public class clsFormatTable
  {
    #region Atributos
    public static List<ExcelFormatTable> format = new List<ExcelFormatTable>();
    #endregion

    #region getExcelFormatTableAssignByPR
    public static List<ExcelFormatTable> getExcelFormatTableAssignByPR()
    {
      format.Clear();
      format.Add(new ExcelFormatTable() { Title = "In", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Out", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Room", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Last Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "First Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Member #", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "01", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Comments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "PR ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "PR N Assigned", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Pax", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      return format;
    }
    #endregion

    #region getExcelFormatTableGenAsignyArvls
    public static List<ExcelFormatTable> getExcelFormatTableGenAsignyArvls()
    {
      format.Clear();
      format.Add(new ExcelFormatTable() { Title = "ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Check In D", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "In", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Room", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Last Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "First Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "PR ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "PR N Assigned", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Member #", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Gross", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Liner", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      format.Add(new ExcelFormatTable() { Title = "Closer", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      return format;
    }
    #endregion
  }
}

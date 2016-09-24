using System.Collections.Generic;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;

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
    public static ExcelFormatItemsList getExcelFormatTableAssignByPR()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("In", "guCheckInD", format: EnumFormatTypeExcel.Date);
      lst.Add("Out", "guCheckOutD", format: EnumFormatTypeExcel.Date);
      lst.Add("Room", "guRoomNum", format: EnumFormatTypeExcel.Id);
      lst.Add("Last Name", "guLastName1");
      lst.Add("First Name", "guFirstName1");
      lst.Add("Agency ID", "guag", format: EnumFormatTypeExcel.Id);
      lst.Add("Agency", "agN");
      lst.Add("Member #", "guMemberShipNum");
      lst.Add("Pax", "guPax", format: EnumFormatTypeExcel.DecimalNumberWithCero);
      lst.Add("Comments", "guComments");
      lst.Add("Contract", "guO1");
      lst.Add("Monday", "monday", format: EnumFormatTypeExcel.DecimalNumberWithCero);
      lst.Add("Tuesday", "tuesday");
      lst.Add("Wednesday", "wednesday");
      lst.Add("Thursday", "thursday", format: EnumFormatTypeExcel.DecimalNumberWithCero);
      lst.Add("Friday", "friday");
      lst.Add("Saturday", "saturday");
      lst.Add("Sunday", "sunday");
      return lst;
    }
    #endregion

    #region getExcelFormatTableGenAsignyArvls
    /// <summary>
    /// Formato para el reporte de llegadas y asignacion
    /// </summary>
    /// <history>
    ///   [vku] 31/Mar/2016 Created
    /// </history>
    public static ExcelFormatItemsList getExcelFormatTableGenAsignyArvls()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("ID", "guID", format: EnumFormatTypeExcel.Id);
      lst.Add("Check In D", "guCheckInD", format: EnumFormatTypeExcel.Date);
      lst.Add("In", "guCheckIn", format: EnumFormatTypeExcel.Boolean);
      lst.Add("Room", "guRoomNum", format: EnumFormatTypeExcel.Id);
      lst.Add("Last Name", "guLastName1");
      lst.Add("First Name", "guFirstName1");
      lst.Add("PR ID", "guPRAssign");
      lst.Add("PR N Assigned", "peN");
      lst.Add("Agency ID", "guag", format: EnumFormatTypeExcel.Id);
      lst.Add("Agency", "agN");
      lst.Add("Member #", "guMemberShipNum");
      lst.Add("Amount", "Gross");
      lst.Add("PR", "PR");
      lst.Add("Liner", "Liner");
      lst.Add("Closer", "Closer");
      return lst;
    }
    #endregion
  }
}

using IM.Base.Helpers;
using IM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace IM.Assignment.Classes
{
  public class clsReports
  {
    #region ExportRptAssignmentByPR
    /// <summary>
    ///   Obtiene los datos para exportar a excel el reporte AssignmentByPR
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fecha (Nombre sugerido para el reporte)</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptAssignmentByPR">Lista de huespedes asignados por PR</param>
    /// <history>
    ///   [vku] 20/Jul/2016 Created
    /// </history>
    public static FileInfo ExportRptAssignmentByPR(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptAssignmentByPR> lstRptAssignmentByPR)
    {
      var lstRptAssignmentByPRAux = lstRptAssignmentByPR.Select(c => new
      {
        c.guCheckInD,
        c.guCheckOutD,
        c.guRoomNum,
        c.guLastName1,
        c.guFirstName1,
        c.guag,
        c.agN,
        c.guMemberShipNum,
        c.guPax,
        c.guComments,
        c.guO1,
        monday= "",
        tuesday = "",
        wednesday = "",
        thursday = "",
        friday = "",
        saturday = "",
        sunday = "",
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptAssignmentByPRAux);
      return  EpplusHelper.CreateGeneralRptExcel(filters, dtData, strReport, dateRangeFileName, clsFormatTable.getExcelFormatTableAssignByPR());
    }
    #endregion

    #region ExportRptGeneralAssignment
    /// <summary>
    ///   Obtiene los datos para exportar a excel el reporte GeneralAssignment
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fecha (Nombre sugerido para el reporte)</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptAssignment">Lista de huespedes asignados</param>
    /// <history>
    ///   [vku] 20/Jul/2016 Created
    /// </history>
    public static FileInfo ExportRptGeneralAssignment(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptAssignment> lstRptAssignment)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptAssignment, true);
      return EpplusHelper.CreateGeneralRptExcel(filters, dtData, strReport, dateRangeFileName, clsFormatTable.getExcelFormatTableGenAsignyArvls());
    }
    #endregion

    #region ExportRptAssignmentArrivals
    /// <summary>
    ///    Obtiene los datos para exportar a excel el reporte AssignmentArrivals
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fecha (Nombre sugerido para el reporte)</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptAssignmentArrivals">Lista de llegadas y su asignación</param>
    /// <history>
    ///   [vku] 20/Jul/2016 Created
    /// </history>
    public static FileInfo ExportRptAssignmentArrivals(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptAssignmentArrivals> lstRptAssignmentArrivals)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptAssignmentArrivals, true);
      return EpplusHelper.CreateGeneralRptExcel(filters, dtData, strReport, dateRangeFileName, clsFormatTable.getExcelFormatTableGenAsignyArvls());
    }
    #endregion
  }
}

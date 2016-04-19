using IM.Base.Helpers;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;

namespace IM.Inhouse.Classes
{
  public class ReportsToExcel
  {
    #region Atributos

    private static List<Tuple<string, string>> filters;
    private static DataTable dt;
    private static string rptName;

    #endregion

    #region ArrivalsToExcel

    /// <summary>
    /// Obtiene Un reporte de Arrivals en formato de Excel
    /// </summary>
    /// <param name="arrivlas">Lista de Arrivals para hacer el excel</param>}
    /// <history>
    /// [ecanul] 18/04/2016 Created
    /// </history>
    public static void ArrivalsToExcel(List<RptArrivals> arrivlas, DateTime date)
    {
      filters = new List<Tuple<string, string>>();
      dt = new DataTable();
      filters.Add(Tuple.Create("Lead Source", App.User.LeadSource.lsID));


      if (arrivlas.Count > 0)
      {
        dt = TableHelper.GetDataTableFromList(arrivlas, true);
        rptName = "Arrivals " + App.User.LeadSource.lsN;
        string dateRange = DateHelper.DateRangeFileName(date, date);
        List<ExcelFormatTable> format = new List<ExcelFormatTable>();
        format.Add(new ExcelFormatTable() { Title = "GUID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Reserv.#", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Room", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "LastName", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "In", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Pax", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Check-Out", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "County ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "County", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Av", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Info", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Inv", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "PR B", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Comments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

        EpplusHelper.CreateGeneralRptExcel(filters, dt, rptName, dateRange, format);
      }
    }

    #endregion

  }
}

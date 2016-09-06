using IM.Base.Helpers;
using IM.Base.Reports;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;

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
    /// [ecanul] 19/04/2016 Created
    /// [jorcanche]  se agrego para abrir el archivo despues de guardar
    /// </history>
    public static void ArrivalsToExcel(List<RptArrivals> arrivlas, DateTime date)
    {
      filters = new List<Tuple<string, string>>();
      dt = new DataTable();
      filters.Add(Tuple.Create("Lead Source", App.User.LeadSource.lsID));


      if (arrivlas.Count > 0)
      {
        dt = TableHelper.GetDataTableFromList(arrivlas, true);
        rptName = "Arrivals ";
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

        OpenFile(EpplusHelper.CreateGeneralRptExcel(filters, dt, rptName, dateRange, format));

      }
    }

    #endregion

    #region AvailablesToExcel

    /// <summary>
    /// Obtiene un reporte de Aviables en formato Excel
    /// </summary>
    /// <param name="aviables">Lista de Aviables</param>
    /// <history>
    /// [ecanul] 19/04/2016 Created
    /// [jorcanche]  se agrego para abrir el archivo despues de guardar
    /// </history>
    public static void AvailablesToExcel(List<RptAvailables> aviables)
    {
      filters = new List<Tuple<string, string>>();
      dt = new DataTable();
      filters.Add(Tuple.Create("Lead Source", App.User.LeadSource.lsID));
      DateTime date = BRHelpers.GetServerDate();
      if (aviables.Count > 0)
      {
        dt = TableHelper.GetDataTableFromList(aviables, true);
        rptName = "Aviables ";
        string dateRange = DateHelper.DateRangeFileName(date, date);
        List<ExcelFormatTable> format = new List<ExcelFormatTable>();

        format.Add(new ExcelFormatTable() { Title = "GUID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Reserv.#", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Room", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "LastName", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Pax", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Check-In", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Check-Out", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Country ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Country", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Av", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Info", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Inv", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "PR B", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Comments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

        OpenFile(EpplusHelper.CreateGeneralRptExcel(filters, dt, rptName, dateRange, format));
      }
    }

    #endregion

    #region PremanifestToExcel

    /// <summary>
    /// Obtiene un reporte de Premanifest en Formato de Excel
    /// </summary>
    /// <param name="premanifest">Listado con Premanifest</param>
    /// <history>
    /// [ecanul] 19/04/2016 Created
    /// [jorcanche]  se agrego para abrir el archivo despues de guardar
    /// [edgrodriguez] 05/09/2016 Modified. Se cambio el método CreateExcelCustom por CreatCustomExcel
    /// </history>
    public static async void PremanifestToExcel(List<RptPremanifest> premanifest)
    {

      filters = new List<Tuple<string, string>>();
      dt = new DataTable();
      filters.Add(Tuple.Create("Lead Source", App.User.LeadSource.lsID));
      DateTime date = BRHelpers.GetServerDate();
      if (premanifest.Count > 0)
      {

        dt = TableHelper.GetDataTableFromList(premanifest, true);
        rptName = "Premanifest ";
        int j = dt.Columns.Count;
        string dateRange = DateHelper.DateRangeFileName(date, date);
        OpenFile(await EpplusHelper.CreateCustomExcel(dt, filters, rptName, dateRange, clsFormatReports.RptPremanifest()));
      }
    }

    #endregion

    #region PremanifestWithGuiftsToExcel

    /// <summary>
    /// Obtiene un reporte de Premanifest With Gifts en Formato de Excel
    /// </summary>
    /// <param name="premanifest">Listado con Premanifest With Gifts</param>
    /// <history>
    /// [ecanul] 19/04/2016 Created
    /// [jorcanche]  se agrego para abrir el archivo despues de guardar
    /// </history>
    public static async void PremanifestWithGiftsToExcel(List<RptPremanifestWithGifts> withGifts)
    {
      filters = new List<Tuple<string, string>>();
      dt = new DataTable();
      filters.Add(Tuple.Create("Lead Source", App.User.LeadSource.lsID));
      DateTime date = BRHelpers.GetServerDate();
      if (withGifts.Count > 0)
      {
        dt = TableHelper.GetDataTableFromList(withGifts, true);
        rptName = "Premanifest with gifts";
        string dateRange = DateHelper.DateRangeFileName(date, date);

        OpenFile(await EpplusHelper.CreateCustomExcel(dt, filters, rptName, dateRange, clsFormatReports.RptPremanifestWithGifts()));
      }
    }

    #endregion


    /// <summary>
    /// abre el reporte de excel despues de guardarse
    /// </summary>
    /// <history>
    /// [jorcanche] 07/05/2016 created
    /// </history>
    private static void OpenFile(FileInfo file)
    {
      if (file != null)
      {
        Process.Start(file.FullName);
      }
    }
  }
}


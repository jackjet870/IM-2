using IM.Base.Helpers;
using IM.Base.Reports;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
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
    public async static void ArrivalsToExcel(List<RptArrivals> arrivlas, DateTime date)
    {
      filters = new List<Tuple<string, string>>();
      dt = new DataTable();
      filters.Add(Tuple.Create("Lead Source", App.User.LeadSource.lsID));


      if (arrivlas.Count > 0)
      {
        dt = TableHelper.GetDataTableFromList(arrivlas, true);
        rptName = "Arrivals ";
        string dateRange = DateHelper.DateRangeFileName(date, date);
        ExcelFormatItemsList format = new ExcelFormatItemsList();
        format.Add("GUID", "guID");
        format.Add("Reserv.#", "guHReservID");
        format.Add("Room", "guRoomNum");
        format.Add("LastName", "guLastName1");
        format.Add("In", "guCheckIn", format: EnumFormatTypeExcel.Boolean);
        format.Add("Pax", "guPax", format: EnumFormatTypeExcel.DecimalNumber);
        format.Add("Check-Out", "guCheckOutD", format: EnumFormatTypeExcel.Date);
        format.Add("County ID", "guco");
        format.Add("County", "coN");
        format.Add("Agency ID", "guag");
        format.Add("Agency", "agN");
        format.Add("Av", "guAvail", format: EnumFormatTypeExcel.Boolean);
        format.Add("Info", "guInfo", format: EnumFormatTypeExcel.Boolean);
        format.Add("Inv", "guInvit", format: EnumFormatTypeExcel.Boolean);
        format.Add("PR B", "guPRInvit1");
        format.Add("Comments", "guComments");

        OpenFile(await EpplusHelper.CreateCustomExcel(dt,filters,rptName,dateRange, format, addEnumeration:true));
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
    public async static void AvailablesToExcel(List<RptAvailables> aviables)
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
        ExcelFormatItemsList format = new ExcelFormatItemsList();

        format.Add("GUID", "guID");
        format.Add("Reserv.#", "guHReservID");
        format.Add("Room", "guRoomNum");
        format.Add("LastName", "guLastName1");
        format.Add("Pax", "guPax", format: EnumFormatTypeExcel.DecimalNumber);
        format.Add("Check-In", "guCheckInD", format: EnumFormatTypeExcel.Date);
        format.Add("Check-Out", "guCheckOutD", format: EnumFormatTypeExcel.Date);
        format.Add("Country ID", "guco");
        format.Add("Country", "coN");
        format.Add("Agency ID", "guag");
        format.Add("Agency", "agN");
        format.Add("Av", "guAvail", format: EnumFormatTypeExcel.Boolean);
        format.Add("Info", "guInfo", format: EnumFormatTypeExcel.Boolean);
        format.Add("Inv", "guInvit", format: EnumFormatTypeExcel.Boolean);
        format.Add("PR B", "guPRInvit1");
        format.Add("Comments", "guComments");

        OpenFile(await EpplusHelper.CreateCustomExcel(dt, filters, rptName, dateRange, format, addEnumeration: true));
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


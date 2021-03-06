﻿using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.Base.Reports;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;

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
    public async static void ArrivalsToExcel(List<RptArrivals> arrivlas, DateTime date,Window window)
    {
      filters = new List<Tuple<string, string>>();
      dt = new DataTable();
      filters.Add(Tuple.Create("Lead Source", Context.User.LeadSource.lsID));


      if (arrivlas.Count > 0)
      {
        dt = TableHelper.GetDataTableFromList(arrivlas, true);
        rptName = "Arrivals ";
        string dateRange = DateHelper.DateRangeFileName(date, date);
        ColumnFormatList format = new ColumnFormatList();
        format.Add("GUID", "guID", width: 7);
        format.Add("Reserv.#", "guHReservID", width: 8);
        format.Add("Room", "guRoomNum", width: 7);
        format.Add("LastName", "guLastName1", width: 20);
        format.Add("In", "guCheckIn", format: EnumFormatTypeExcel.Boolean, width: 4, aligment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);
        format.Add("Pax", "guPax", format: EnumFormatTypeExcel.DecimalNumber, width: 6, aligment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);
        format.Add("Check-Out", "guCheckOutD", format: EnumFormatTypeExcel.Date, width: 10);        
        format.Add("Country", "coN", width: 13);        
        format.Add("Agency", "agN", width: 15, wordWrap: true);
        format.Add("Av", "guAvail",  format: EnumFormatTypeExcel.Boolean, width:  4, aligment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);
        format.Add("Info", "guInfo", format: EnumFormatTypeExcel.Boolean, width: 4, aligment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);
        format.Add("Inv", "guInvit", format: EnumFormatTypeExcel.Boolean, width: 4, aligment: OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);
        format.Add("PR B", "guPRInvit1", width: 15);
        format.Add("Comments", "guComments", width: 40, wordWrap: true);
        format.Add("County ID", "guco", isVisible: false);
        format.Add("Agency ID", "guag", isVisible:  false);
        OpenFile(await ReportBuilder.CreateCustomExcelAsync(dt, filters, rptName, dateRange, format, addEnumeration: true, autoFit:false, numColumnsForApplicationName: 4), window);
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
    public async static void AvailablesToExcel(List<RptAvailables> aviables,Window window)
    {
      filters = new List<Tuple<string, string>>();
      dt = new DataTable();
      filters.Add(Tuple.Create("Lead Source", Context.User.LeadSource.lsID));
      DateTime date = BRHelpers.GetServerDate();
      if (aviables.Count > 0)
      {
        dt = TableHelper.GetDataTableFromList(aviables, true);
        rptName = "Availables";
        string dateRange = DateHelper.DateRangeFileName(date, date);
        ColumnFormatList format = new ColumnFormatList();

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

        OpenFile(await ReportBuilder.CreateCustomExcelAsync(dt, filters, rptName, dateRange, format, addEnumeration: true), window);
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
    public static async void PremanifestToExcel(List<RptPremanifest> premanifest,Window window)
    {

      filters = new List<Tuple<string, string>>();
      dt = new DataTable();
      filters.Add(Tuple.Create("Lead Source", Context.User.LeadSource.lsID));
      DateTime date = BRHelpers.GetServerDate();
      if (premanifest.Count > 0)
      {

        dt = TableHelper.GetDataTableFromList(premanifest, true);
        rptName = "Premanifest ";
        int j = dt.Columns.Count;
        string dateRange = DateHelper.DateRangeFileName(date, date);
        OpenFile(await ReportBuilder.CreateCustomExcelAsync(dt, filters, rptName, dateRange, clsFormatReports.RptPremanifest()), window);
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
    public static async void PremanifestWithGiftsToExcel(List<RptPremanifestWithGifts> withGifts,Window window)
    {
      filters = new List<Tuple<string, string>>();
      dt = new DataTable();
      filters.Add(Tuple.Create("Lead Source", Context.User.LeadSource.lsID));
      DateTime date = BRHelpers.GetServerDate();
      if (withGifts.Count > 0)
      {
        dt = TableHelper.GetDataTableFromList(withGifts, true);
        rptName = "Premanifest with gifts";
        string dateRange = DateHelper.DateRangeFileName(date, date);

        OpenFile(await ReportBuilder.CreateCustomExcelAsync(dt, filters, rptName, dateRange, clsFormatReports.RptPremanifestWithGifts()), window);
      }
    }

    #endregion


    /// <summary>
    /// abre el reporte de excel despues de guardarse
    /// </summary>
    /// <history>
    /// [jorcanche] 07/05/2016 created
    /// [emoguel] 08/09/2016 Modified. Ahora abre el visor de reportes
    /// </history>
    private static void OpenFile(FileInfo file,Window window)
    {
      if (file != null)
      {
        frmDocumentViewer documentViewer = new frmDocumentViewer(file,Context.User.HasPermission(EnumPermission.RptExcel,EnumPermisionLevel.ReadOnly),false);
        documentViewer.Owner = window;       
        documentViewer.ShowDialog();
      }
    }
  }
}


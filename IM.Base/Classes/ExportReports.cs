﻿using IM.Base.Helpers;
using IM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;
using OfficeOpenXml.Style;

namespace IM.Base.Classes
{
  public static class ExportReports
  {
    #region asAssistanceType
    /// <summary>
    /// Devuele el valor de un tipo de asistencia por el nombre 
    /// </summary>
    /// <param name="atId">ID de la asistencia a Buscar</param>
    /// <history>
    /// [ecanul] 14/09/2016 Created
    /// </history>
    private static string asAssistanceType(string atId)
    {
      var assistanceType = string.Empty;
      switch (atId)
      {
        case "A":
          assistanceType = "Assistance";
          break;
        case "B":
          assistanceType = "Absence";
          break;
        case "D":
          assistanceType = "Day Off";
          break;
        case "L":
          assistanceType = "Licence";
          break;
        case "P":
          assistanceType = "Permission";
          break;
        case "S":
          assistanceType = "Suspension";
          break;
        case "V":
          assistanceType = "Vacation";
          break;
        default:
          assistanceType = string.Empty;
          break;
      }
      return assistanceType;
    }
    #endregion

    #region RptAssitance
    /// <summary>
    /// Genera el reporte de Asistencias semanales 
    /// </summary>
    /// <param name="list">Listado de asistencias</param>
    /// <param name="palaceId">ID de LS - SR</param>
    /// <param name="dtmStart">Fecha inicio</param>
    /// <param name="dtmEnd">Fecha Fin</param>
    /// <history>
    /// [ecanul] 14/09/2016 Created
    /// </history>
    public static async Task<FileInfo> RptAssitance(List<AssistanceData> list, DateTime dtmStart, DateTime dtmEnd, List<Tuple<string, string>> filters)
    {
      DataTable dt = new DataTable();
      string rptName;

      var nList = list.Select(c => new
      {
        c.asPlaceType,
        c.asPlaceID,
        c.asStartD,
        c.asEndD,
        c.aspe,
        c.peN,

        asMonday = asAssistanceType(c.asMonday),
        asTuesday = asAssistanceType(c.asTuesday),
        asWednesday = asAssistanceType(c.asWednesday),
        asThursday = asAssistanceType(c.asThursday),
        asFriday = asAssistanceType(c.asFriday),
        asSaturday = asAssistanceType(c.asSaturday),
        asSunday = asAssistanceType(c.asSunday),

        c.asNum
      }).ToList();

      dt = TableHelper.GetDataTableFromList(nList, true);
      rptName = "Assistance";

      string dateRange = DateHelper.DateRange(dtmStart, dtmEnd);
      string dateRangeFileName = DateHelper.DateRangeFileName(dtmStart, dtmEnd);

      FileInfo file = await ReportBuilder.CreateCustomExcelAsync(dt, filters, rptName, dateRangeFileName, RptAssistanceFormat());
      return file;
    } 
    #endregion

    #region RptAssistanceFormat
    /// <summary>
    /// Crea el formato para crear el reporte de Assistencias
    /// </summary>
    /// <history>
    /// [ecanul] 13/09/2016 Created
    /// </history>
    private static ColumnFormatList RptAssistanceFormat()
    {
      ColumnFormatList format = new ColumnFormatList();
      format.Add("Place Type", "asPlaceType");
      format.Add("Place ID", "asPlaceID");
      format.Add("Date Start", "asStartD", format: EnumFormatTypeExcel.Date);
      format.Add("Date End", "asEndD", format: EnumFormatTypeExcel.Date);
      format.Add("ID", "aspe");
      format.Add("Name", "peN");
      format.Add("Monday", "asMonday");
      format.Add("Tuesday", "asTuesday");
      format.Add("Wednesday", "asWednesday");
      format.Add("Thursday", "asThursday");
      format.Add("Friday", "asFriday");
      format.Add("Saturday", "asSaturday");
      format.Add("Sunday", "asSunday");
      format.Add("#Assistence", "asNum");
      return format;
    }
    #endregion

    #region RptSalesmenChanges
    /// <summary>
    /// Formato para el reporte de Sales Log
    /// </summary>
    /// <history>
    /// [jorcanche]  created  03102016
    /// </history>    
    public static ColumnFormatList RptSalesmenChanges()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Date / Time", "schDT", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Authorized By", "schAuthorizedBy", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Authorized By Name", "AuthorizedByN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Made By", "schMadeBy", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Made By Name", "MadeByN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Role", "roN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Position", "schPosition", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Old Salesman", "schOldSalesman", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Old Salesman Name", "OldSalesmanN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("New Salesman", "schNewSalesman", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("New Salesman Name", "NewSalesmanN", aligment: ExcelHorizontalAlignment.Left);
      return lst;
    }
    #endregion
  }
}

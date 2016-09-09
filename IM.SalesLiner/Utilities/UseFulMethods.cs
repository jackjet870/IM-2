using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace IM.SalesLiner.Utilities
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
    public static ExcelFormatItemsList getExcelFormatTable()
    {
      ExcelFormatItemsList formatColumns = new ExcelFormatItemsList();
      formatColumns.Add("Sale ID", "saID");
      formatColumns.Add("LS", "sals");
      formatColumns.Add("SR", "sasr");
      formatColumns.Add("Member. #", "saMembershipNum");
      formatColumns.Add("Sale Type", "stN");
      formatColumns.Add("Date", "saD", format: EnumFormatTypeExcel.Date);//Date
      formatColumns.Add("Proc Date", "saProcD", format: EnumFormatTypeExcel.Date);//Date
      formatColumns.Add("Last Name", "saLastName1");
      formatColumns.Add("Liner", "saLiner1");
      formatColumns.Add("Liner Name", "Liner1N");
      formatColumns.Add("Liner 2", "saLiner2");
      formatColumns.Add("Liner 2 Name", "Liner2N");    
      formatColumns.Add("Amount", "saGrossAmount", format: EnumFormatTypeExcel.Currency);//Currency
      formatColumns.Add("Cxld", "saCancel", format: EnumFormatTypeExcel.Boolean);//Boolean
      formatColumns.Add("Cancel D", "saCancelD", format: EnumFormatTypeExcel.Date);//Date
      return formatColumns;
    }
  }
}

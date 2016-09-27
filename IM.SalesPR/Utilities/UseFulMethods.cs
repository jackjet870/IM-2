using OfficeOpenXml.Table.PivotTable;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;

namespace IM.SalesPR.Utilities
{
  public class UseFulMethods
  {
    /// <summary>
    /// Genera las columnas que necesito en el reporte Sales by PR
    /// </summary>
    /// <returns>List<ExcelFormatItemsList></returns>
    /// <history>
    /// [erosado] 23/Mar/2016  Created
    /// </history>
    public static ExcelFormatItemsList getExcelFormatTable()
    {
      ExcelFormatItemsList  formatColumns = new ExcelFormatItemsList();
      formatColumns.Add("Sale ID", "saID");
      formatColumns.Add("LS", "sals");
      formatColumns.Add("SR", "sasr");
      formatColumns.Add("Member. #", "saMembershipNum");
      formatColumns.Add("Sale Type", "stN");
      formatColumns.Add("Date", "saD", format:  EnumFormatTypeExcel.Date);//Date
      formatColumns.Add("Proc Date", "saProcD", format:  EnumFormatTypeExcel.Date);//Date
      formatColumns.Add("Last Name", "saLastName1");
      formatColumns.Add("Chk-Out D", "guCheckOutD", format:  EnumFormatTypeExcel.Date);//Date
      formatColumns.Add("Agency ID", "guag");
      formatColumns.Add("Agency", "agN");
      formatColumns.Add("PR", "saPR1");
      formatColumns.Add("PR Name", "PR1N"); 
      formatColumns.Add("PR2", "saPR2");
      formatColumns.Add("PR2 Name", "PR2N"); 
      formatColumns.Add("PR3", "saPR3");
      formatColumns.Add("PR3 Name", "PR3N");
      formatColumns.Add("Q", "guQ", format:  EnumFormatTypeExcel.Boolean);//Boolean
      formatColumns.Add("Amount", "saGrossAmount", format:  EnumFormatTypeExcel.Currency, function:DataFieldFunctions.Sum);//Currency
      formatColumns.Add("Cxld", "saCancel", format:  EnumFormatTypeExcel.Boolean);//Boolean
      formatColumns.Add("Cancel D", "saCancelD", format:  EnumFormatTypeExcel.Date);//Date
      return formatColumns;
    }
  }
}

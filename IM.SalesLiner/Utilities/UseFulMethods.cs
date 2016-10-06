using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;

namespace IM.SalesLiner.Utilities
{
  public class UseFulMethods
  {
    /// <summary>
    /// Genera las columnas que necesito en el reporte RPTStatistics
    /// </summary>
    /// <returns>List<ColumnFormat></returns>
    /// <history>
    /// [erosado] 23/Mar/2016  Created
    /// </history>
    public static ColumnFormatList getExcelFormatTable()
    {
      ColumnFormatList formatColumns = new ColumnFormatList();
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

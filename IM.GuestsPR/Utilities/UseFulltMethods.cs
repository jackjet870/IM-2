using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace IM.GuestsPR.Utilities
{
  public class UseFulltMethods
  {
    /// <summary>
    /// Genera las columnas que necesito en el reporte RPTStatistics
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    /// [erosado] 14/Mar/2016  Created
    /// </history>
    public static List<ExcelFormatTable> getExcelFormatTable()
    {
      List<ExcelFormatTable> formatColumns = new List<ExcelFormatTable>();
      formatColumns.Add(new ExcelFormatTable() { Title = "GUID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Last Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "First Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "LS", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "gusr", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "guag", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "gumk", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "Market", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Ext", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Rbk", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Check-In D", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left }); // date

      formatColumns.Add(new ExcelFormatTable() { Title = "guPRAssign", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "Avl", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "O.Avl", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Info", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "guInfoD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left }); //date

      formatColumns.Add(new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "FU", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "guFollowD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });//date
      formatColumns.Add(new ExcelFormatTable() { Title = "guPRFollow", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PRFollowN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "Invit", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "guBookD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });//date
      formatColumns.Add(new ExcelFormatTable() { Title = "guPRInvit1", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PR1N", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "guPRInvit2", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PR2N", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "guPRInvit3", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PR3N", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      
      formatColumns.Add(new ExcelFormatTable() { Title = "Qui", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "SH", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "guShowD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });//date

      formatColumns.Add(new ExcelFormatTable() { Title = "Tour", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "WO", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "QS", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Sale", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Sales", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Amount", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left });
      return formatColumns;
    }



    public static string LeadSourceLoginListInToStringSplitByComma(List<LeadSourceLogin> lista)
    {
      string lstArraySplitByComa = string.Empty;
      if (lista.Count > 0)
      {
        foreach (LeadSourceLogin item in lista)
        {
          lstArraySplitByComa += string.Concat(item.lsID, ",");
        }
      }
      return lstArraySplitByComa;
    }
  }
}

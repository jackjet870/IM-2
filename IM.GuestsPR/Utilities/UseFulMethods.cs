using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace IM.GuestsPR.Utilities
{
  public class UseFulMethods
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

      formatColumns.Add(new ExcelFormatTable() { Title = "SR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "Market ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "Market", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Ext", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Rbk", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Check-In D", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left }); // date

      formatColumns.Add(new ExcelFormatTable() { Title = "PR A", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "Avl", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "O.Avl", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Info D", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "guInfoD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left }); //date

      formatColumns.Add(new ExcelFormatTable() { Title = "PR Info", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PR Info Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "FU", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "Follow D", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });//date
      formatColumns.Add(new ExcelFormatTable() { Title = "PR Follow", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PR Follow Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "Invit", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "Book D", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });//date
      formatColumns.Add(new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PR 2", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PR 2 Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PR 3", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PR 3 Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });

      
      formatColumns.Add(new ExcelFormatTable() { Title = "Qui", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "SH", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });

      formatColumns.Add(new ExcelFormatTable() { Title = "Show D", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });//date

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

  }
}

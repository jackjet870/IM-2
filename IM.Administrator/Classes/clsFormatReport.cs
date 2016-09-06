using IM.Model.Classes;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using IM.Model.Enums;

namespace IM.Administrator.Classes
{
  class clsFormatReport
  {
    #region RptGiftLog
    /// <summary>
    /// Formato para el excel de GiftLog
    /// </summary>
    /// <returns> List of ExcelFormatTable</returns>
    /// <history>
    /// [emoguel] modified 06/07/2016
    /// </history>
    public static List<ExcelFormatTable> RptGiftLog()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() { Title = "Name", PropertyName = "ChangedByN", Alignment = ExcelHorizontalAlignment.Left },
        new ExcelFormatTable() { Title = "By", PropertyName = "ggChangedBy", Alignment = ExcelHorizontalAlignment.Left },
        new ExcelFormatTable() { Title = "Update Date/Time", PropertyName = "ggID", Alignment = ExcelHorizontalAlignment.Left, Format=EnumFormatTypeExcel.DateTime},
        new ExcelFormatTable() { Title = "Cost Adults", PropertyName = "ggPrice1", Alignment = ExcelHorizontalAlignment.Left, Format=EnumFormatTypeExcel.DecimalNumberWithCero },
        new ExcelFormatTable() { Title = "Cost Minors", PropertyName = "ggPrice2", Alignment = ExcelHorizontalAlignment.Left, Format=EnumFormatTypeExcel.DecimalNumberWithCero},
        new ExcelFormatTable() { Title = "CxC Adults", PropertyName = "ggPrice3", Alignment = ExcelHorizontalAlignment.Left, Format=EnumFormatTypeExcel.DecimalNumberWithCero},
        new ExcelFormatTable() { Title = "CxC Minors", PropertyName = "ggPrice4", Alignment = ExcelHorizontalAlignment.Left, Format=EnumFormatTypeExcel.DecimalNumberWithCero },
        new ExcelFormatTable() { Title = "Price Adults", PropertyName = "ggPriceAdults", Alignment = ExcelHorizontalAlignment.Left, Format=EnumFormatTypeExcel.DecimalNumberWithCero },
        new ExcelFormatTable() { Title = "Price Minors", PropertyName = "ggPriceMinors", Alignment = ExcelHorizontalAlignment.Left, Format=EnumFormatTypeExcel.DecimalNumberWithCero },
        new ExcelFormatTable() { Title = "P.E. Adults", PropertyName = "ggPriceExtraAdults", Alignment = ExcelHorizontalAlignment.Left, Format=EnumFormatTypeExcel.DecimalNumberWithCero},
        new ExcelFormatTable() { Title = "Category", PropertyName = "gcN", Alignment = ExcelHorizontalAlignment.Left, Order = 11 },
        new ExcelFormatTable() { Title = "Package", PropertyName = "ggPack", Alignment = ExcelHorizontalAlignment.Center, Format=EnumFormatTypeExcel.Boolean },
        new ExcelFormatTable() { Title = "Inventory", PropertyName = "ggInven", Alignment = ExcelHorizontalAlignment.Center, Format=EnumFormatTypeExcel.Boolean },
        new ExcelFormatTable() { Title = "Active", PropertyName = "ggA", Alignment = ExcelHorizontalAlignment.Center, Format=EnumFormatTypeExcel.Boolean },
        new ExcelFormatTable() { Title = "With Folio", PropertyName = "ggWFolio", Alignment = ExcelHorizontalAlignment.Center, Format=EnumFormatTypeExcel.Boolean },
        new ExcelFormatTable() { Title = "With Pax", PropertyName = "ggWPax", Alignment = ExcelHorizontalAlignment.Center, Format=EnumFormatTypeExcel.Boolean },
        new ExcelFormatTable() { Title = "Order", PropertyName = "ggO", Alignment = ExcelHorizontalAlignment.Left },
      };
    }
    #endregion
  }
}

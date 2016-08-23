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
        new ExcelFormatTable() { Title = "Name", PropertyName = "ChangedByN", Alignment = ExcelHorizontalAlignment.Left, Order = 1 },
        new ExcelFormatTable() { Title = "By", PropertyName = "ggChangedBy", Alignment = ExcelHorizontalAlignment.Left, Order = 2 },
        new ExcelFormatTable() { Title = "Update Date/Time", PropertyName = "ggID", Alignment = ExcelHorizontalAlignment.Left, Order = 3 ,Format=EnumFormatTypeExcel.DateTime},
        new ExcelFormatTable() { Title = "Cost Adults", PropertyName = "ggPrice1", Alignment = ExcelHorizontalAlignment.Left, Order = 4,Format=EnumFormatTypeExcel.DecimalNumberWithCero },
        new ExcelFormatTable() { Title = "Cost Minors", PropertyName = "ggPrice2", Alignment = ExcelHorizontalAlignment.Left, Order = 5, Format=EnumFormatTypeExcel.DecimalNumberWithCero},
        new ExcelFormatTable() { Title = "CxC Adults", PropertyName = "ggPrice3", Alignment = ExcelHorizontalAlignment.Left, Order = 6, Format=EnumFormatTypeExcel.DecimalNumberWithCero},
        new ExcelFormatTable() { Title = "CxC Minors", PropertyName = "ggPrice4", Alignment = ExcelHorizontalAlignment.Left, Order = 7,Format=EnumFormatTypeExcel.DecimalNumberWithCero },
        new ExcelFormatTable() { Title = "Price Adults", PropertyName = "ggPriceAdults", Alignment = ExcelHorizontalAlignment.Left, Order = 8,Format=EnumFormatTypeExcel.DecimalNumberWithCero },
        new ExcelFormatTable() { Title = "Price Minors", PropertyName = "ggPriceMinors", Alignment = ExcelHorizontalAlignment.Left, Order = 9,Format=EnumFormatTypeExcel.DecimalNumberWithCero },
        new ExcelFormatTable() { Title = "P.E. Adults", PropertyName = "ggPriceExtraAdults", Alignment = ExcelHorizontalAlignment.Left, Order = 10 ,Format=EnumFormatTypeExcel.DecimalNumberWithCero},
        new ExcelFormatTable() { Title = "Category", PropertyName = "gcN", Alignment = ExcelHorizontalAlignment.Left, Order = 11 },
        new ExcelFormatTable() { Title = "Package", PropertyName = "ggPack", Alignment = ExcelHorizontalAlignment.Center, Order = 12, Format=EnumFormatTypeExcel.Boolean },
        new ExcelFormatTable() { Title = "Inventory", PropertyName = "ggInven", Alignment = ExcelHorizontalAlignment.Center, Order = 13, Format=EnumFormatTypeExcel.Boolean },
        new ExcelFormatTable() { Title = "Active", PropertyName = "ggA", Alignment = ExcelHorizontalAlignment.Center, Order = 14, Format=EnumFormatTypeExcel.Boolean },
        new ExcelFormatTable() { Title = "With Folio", PropertyName = "ggWFolio", Alignment = ExcelHorizontalAlignment.Center, Order = 15, Format=EnumFormatTypeExcel.Boolean },
        new ExcelFormatTable() { Title = "With Pax", PropertyName = "ggWPax", Alignment = ExcelHorizontalAlignment.Center, Order = 16, Format=EnumFormatTypeExcel.Boolean },
        new ExcelFormatTable() { Title = "Order", PropertyName = "ggO", Alignment = ExcelHorizontalAlignment.Left, Order = 17 },
      };
    }
    #endregion
  }
}

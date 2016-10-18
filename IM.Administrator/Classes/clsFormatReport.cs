using OfficeOpenXml.Style;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;

namespace IM.Administrator.Classes
{
  class clsFormatReport
  {
    #region RptGiftLog
    /// <summary>
    /// Formato para el excel de GiftLog
    /// </summary>
    /// <returns> List of ColumnFormat</returns>
    /// <history>
    /// [emoguel] modified 06/07/2016
    /// </history>
    public static ColumnFormatList RptGiftLog()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Name", "ChangedByN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("By", "ggChangedBy", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Update Date/Time", "ggID", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.DateTime);
      lst.Add("Cost Adults", "ggPrice1", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.DecimalNumberWithCero);
      lst.Add("Cost Minors", "ggPrice2", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.DecimalNumberWithCero);
      lst.Add("CxC Adults", "ggPrice3", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.DecimalNumberWithCero);
      lst.Add("CxC Minors", "ggPrice4", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.DecimalNumberWithCero);
      lst.Add("Price Adults", "ggPriceAdults", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.DecimalNumberWithCero);
      lst.Add("Price Minors", "ggPriceMinors", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.DecimalNumberWithCero);
      lst.Add("P.E. Adults", "ggPriceExtraAdults", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.DecimalNumberWithCero);
      lst.Add("Category", "gcN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Package", "ggPack", aligment: ExcelHorizontalAlignment.Center, format: EnumFormatTypeExcel.Boolean);
      lst.Add("Inventory", "ggInven", aligment: ExcelHorizontalAlignment.Center, format: EnumFormatTypeExcel.Boolean);
      lst.Add("Active", "ggA", aligment: ExcelHorizontalAlignment.Center, format: EnumFormatTypeExcel.Boolean);
      lst.Add("With Folio", "ggWFolio", aligment: ExcelHorizontalAlignment.Center, format: EnumFormatTypeExcel.Boolean);
      lst.Add("With Pax", "ggWPax", aligment: ExcelHorizontalAlignment.Center, format: EnumFormatTypeExcel.Boolean);
      lst.Add("Order", "ggO", aligment: ExcelHorizontalAlignment.Left);
      return lst;
    }
    #endregion

    #region RptPersonnelLog
    /// <summary>
    /// Formato para el excel de Personnel Log
    /// </summary>
    /// <returns> List of ColumnFormat</returns>
    /// <history>
    /// [emoguel] 17/10/2016 created
    /// </history>
    public static ColumnFormatList RptPersonnelLog()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Changed By", "plgChangedBy", aligment: ExcelHorizontalAlignment.Left,width:15);
      lst.Add("Changed By Name", "ChangedByN", aligment: ExcelHorizontalAlignment.Left, width: 20);
      lst.Add("Update Date/Time", "plgDT", aligment: ExcelHorizontalAlignment.Left,width: 15);
      lst.Add("Personnel", "plgpe", aligment: ExcelHorizontalAlignment.Left, width: 15);
      lst.Add("Personnel Name", "peN", aligment: ExcelHorizontalAlignment.Left, width: 20);
      lst.Add("Department", "plgde", aligment: ExcelHorizontalAlignment.Left, width: 20);
      lst.Add("Department Name", "deN", aligment: ExcelHorizontalAlignment.Left, width: 15);
      lst.Add("Post", "plgpo", aligment: ExcelHorizontalAlignment.Left, width: 15);
      lst.Add("Post Name", "poN", aligment: ExcelHorizontalAlignment.Left, width: 15);
      lst.Add("Sales Room", "plgsr", aligment: ExcelHorizontalAlignment.Left, width: 15);
      lst.Add("Sales Room Name", "srN", aligment: ExcelHorizontalAlignment.Left, width: 15);
      lst.Add("Location", "plglo", aligment: ExcelHorizontalAlignment.Left, width: 15);
      lst.Add("Location Name", "loN", aligment: ExcelHorizontalAlignment.Left, width: 15);
      lst.Add("Roles", "Roles", aligment: ExcelHorizontalAlignment.Left,wordWrap:true,width:50);
      lst.Add("Sales Rooms", "SalesRooms", aligment: ExcelHorizontalAlignment.Left, wordWrap: true,width:50);
      lst.Add("Lead Sources", "LeadSources", aligment: ExcelHorizontalAlignment.Left, wordWrap: true,width:50);
      return lst;
    }
    #endregion
  }
}

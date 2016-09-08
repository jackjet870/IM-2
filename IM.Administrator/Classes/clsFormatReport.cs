﻿using IM.Model.Classes;
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
    public static ExcelFormatItemsList RptGiftLog()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
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
  }
}

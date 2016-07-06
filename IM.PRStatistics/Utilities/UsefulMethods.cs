using System;
using System.Collections.Generic;
using System.Windows.Controls;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;

namespace IM.PRStatistics.Utilities
{
  public class UsefulMethods
  {
    /// <summary>
    /// Se encarga de agregar todos los ID de los elementos seleccionados en una cadena(string)
    /// </summary>
    /// <param name="lsbx">ListBox Control</param>
    /// <returns>String ID Elementos Seleccionados</returns>
    /// <history>
    /// [erosado] 07/Mar/2016 Created
    /// </history>
    public static string SelectedItemsIdToString(ListBox lsbx)
    {
      try
      {
        string lsSelectedItems = string.Empty;

        if (lsbx.SelectedItems.Count > 0)
        {
          switch (lsbx.Name)
          {
            case "lsbxLeadSources":
              foreach (LeadSourceByUser item in lsbx.SelectedItems)
              {
                lsSelectedItems += string.Concat(item.lsID, ",");
              }
              break;

            case "lsbxSalesRooms":
              foreach (SalesRoomByUser item in lsbx.SelectedItems)
              {
                lsSelectedItems += string.Concat(item.srID, ",");
              }
              break;

            case "lsbxCountries":
              foreach (CountryShort item in lsbx.SelectedItems)
              {
                lsSelectedItems += string.Concat(item.coID, ",");
              }
              break;

            case "lsbxAgencies":
              foreach (AgencyShort item in lsbx.SelectedItems)
              {
                lsSelectedItems += string.Concat(item.agID, ",");
              }
              break;

            case "lsbxMarkets":
              foreach (MarketShort item in lsbx.SelectedItems)
              {
                lsSelectedItems += string.Concat(item.mkID, ",");
              }
              break;

            default:
              break;
          }

        }
        return lsSelectedItems = lsSelectedItems.Remove(lsSelectedItems.Length - 1);
      }
      catch (Exception ex)
      {
        throw ex;
      }

    }
    /// <summary>
    /// Se encarga de agregar todos los Nombres de los elementos seleccionados en una cadena(string)
    /// </summary>
    /// <param name="lsbx">ListBox Control</param>
    /// <returns>String Nombres Elementos Seleccionados</returns>
    /// <history>
    /// [erosado] 07/Mar/2016 Created
    /// </history>
    public static string SelectedItemsNameToString(ListBox lsbx)
    {
      try
      {
        string lsSelectedItems = string.Empty;

        if (lsbx.SelectedItems.Count > 0)
        {
          switch (lsbx.Name)
          {
            case "lsbxLeadSources":
              foreach (LeadSourceByUser item in lsbx.SelectedItems)
              {
                lsSelectedItems += string.Concat(item.lsN, ",");
              }
              break;

            case "lsbxSalesRooms":
              foreach (SalesRoomByUser item in lsbx.SelectedItems)
              {
                lsSelectedItems += string.Concat(item.srN, ",");
              }
              break;

            case "lsbxCountries":
              foreach (CountryShort item in lsbx.SelectedItems)
              {
                lsSelectedItems += string.Concat(item.coN, ",");
              }
              break;

            case "lsbxAgencies":
              foreach (AgencyShort item in lsbx.SelectedItems)
              {
                lsSelectedItems += string.Concat(item.agN, ",");
              }
              break;

            case "lsbxMarkets":
              foreach (MarketShort item in lsbx.SelectedItems)
              {
                lsSelectedItems += string.Concat(item.mkN, ",");
              }
              break;

            default:
              break;
          }

        }
        return lsSelectedItems = lsSelectedItems.Remove(lsSelectedItems.Length - 1);
      }
      catch (Exception ex)
      {
        throw ex;
      }

    }


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
      formatColumns.Add(new ExcelFormatTable() { Title = "PR Id", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Assign", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Conts", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "C%", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Avails", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "A%", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Bk", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Bk%", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Deep", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Dir", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Sh", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "T Sh", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "SG", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Proc #", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Processable", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Out Pending #", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Out Pending", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "C #", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Cancelled", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Total #", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Total", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Proc PR #", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Proc PR", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Proc SG #", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Proc SG", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Cl %", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Ca %", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left });
      formatColumns.Add(new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left });
      

      return formatColumns;
    }


  }
}
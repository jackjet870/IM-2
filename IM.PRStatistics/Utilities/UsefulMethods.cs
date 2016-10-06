using System;
using System.Windows.Controls;
using IM.Model;
using OfficeOpenXml.Table.PivotTable;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;

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
    /// <returns>List<ColumnFormat></returns>
    /// <history>
    /// [erosado] 14/Mar/2016  Created
    /// </history>
    public static ColumnFormatList getExcelFormatTable()
    {
      ColumnFormatList formatColumns = new ColumnFormatList();
      formatColumns.Add("PR Id", "PR_ID");
      formatColumns.Add("PR Name", "PR_NAME");
      formatColumns.Add("PR Status", "P_Status",isGroup:true, isVisible:false);
      formatColumns.Add("Assign", "Assign", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      formatColumns.Add("Conts", "Conts", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("C%", "C_Factor", format: EnumFormatTypeExcel.Percent, isCalculated:true, formula: "IF([Assign]=0,0,[Conts]/[Assign])");
      formatColumns.Add("Avails", "Avails",format:EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("A%", "A_Factor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Conts]=0,0,[Avails]/[Conts])");
      formatColumns.Add("Bk", "Bk", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("Bk%", "Bk_Factor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Avails]=0,0,[T_Books]/[Avails])");
      formatColumns.Add("TBooks", "T_Books", format: EnumFormatTypeExcel.DecimalNumber, function:DataFieldFunctions.Sum);
      formatColumns.Add("Dep", "Dep", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("Dir", "Dir", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("Sh", "Sh", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("IO", "IO", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("Sh%", "Sh_Factor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Bk]=0,0,[TSh]/[Bk])");
      formatColumns.Add("T Sh", "TSh", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("SG", "SG", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("Proc #", "Proc_Number", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("Processable", "Processable", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "[Total] - [Out_Pending]");
      formatColumns.Add("Out Pending #", "OutP_Number", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("Out Pending", "Out_Pending", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      formatColumns.Add("C #", "C_Number", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("Cancelled", "Cancelled", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      formatColumns.Add("Total #", "Total_Number", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      formatColumns.Add("Total", "Total", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      formatColumns.Add("Proc PR #", "Proc_PR_Number", isCalculated: true, formula: "[Proc_Number]-[Proc_SG_Number]");
      formatColumns.Add("Proc PR", "Proc_PR", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "[Total]-[Proc_SG]");
      formatColumns.Add("Proc SG #", "Proc_SG_Number", function: DataFieldFunctions.Sum);
      formatColumns.Add("Proc SG", "Proc_SG", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      formatColumns.Add("Eff", "Eff", format: EnumFormatTypeExcel.DecimalNumber, isCalculated: true, formula: "IF([TSh]=0,0,[Total]/[TSh])");
      formatColumns.Add("Cl %", "Cl_Factor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([TSh]=0,0,[Proc_Number]/[TSh])");
      formatColumns.Add("Ca %", "Ca_Factor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Total]=0,0,[Cancelled]/[Total])");
      formatColumns.Add("Avg Sale", "Avg_Sale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Proc_Number]=0,0,[Total]/[Proc_Number])");
      return formatColumns;
    }


  }
}
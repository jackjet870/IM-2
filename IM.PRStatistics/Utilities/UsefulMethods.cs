using System;
using System.Collections.Generic;
using System.Windows.Controls;
using IM.Model;
using System.Data;
using System.ComponentModel;
using IM.Base.Helpers;

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
    /// [erosado] 07/Marz/2016 Created
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
    /// [erosado] 07/Marz/2016 Created
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
  }
}

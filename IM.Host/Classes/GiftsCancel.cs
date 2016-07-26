using IM.Base.Helpers;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using IM.Base.Classes;

namespace IM.Host.Classes
{
  /// <summary>
  /// Clase para el manejo de productos externos a cancelar
  /// </summary>
  /// <history>
  /// [vipacheco] 18/Mayo/2016 Created
  /// </history>
  public class GiftsCancel
  {

    #region Validate
    /// <summary>
    /// Valida los regalos a cancelar de tarjetas de regalos
    /// </summary>
    /// <param name="Grid"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 18/Mayo/2016 Created
    /// </history>
    public static bool Validate(DataGrid Grid, string CancelField)
    {
      int RowsCount = 0;
      // Validamos que haya al menos un regalo por cancelar
      foreach (var item in Grid.Items)
      {
        Type type = item.GetType();
        var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

        if (property.Count() > 0)
        {
          if (((bool)type.GetProperty(CancelField).GetValue(item, null)) != false)
          {
            RowsCount++;
            break;
          }
        }
      }

      if (RowsCount != 0) // Si existe al menos un regalo por cancelar.
        return true;
      else
      {
        UIHelper.ShowMessage("Specify at least one gift for cancel.", System.Windows.MessageBoxImage.Information);
        return false;
      }
    }
    #endregion

    #region CalculateTotalGifts
    /// <summary>
    /// Calcula el total de regalos
    /// </summary>
    /// <param name="OnlyCancellled"></param>
    /// <history>
    /// [vipacheco] 24/Mayo/2016 Created
    /// </history>
    public static void CalculateTotalGifts(DataGrid Grid, ref TextBox txtTotalCost, bool OnlyCancellled = false, string CancelField = "")
    {
      TextBox _null = null;
      Gifts.CalculateTotalGifts(Grid, EnumGiftsType.ReceiptGifts, ref txtTotalCost, ref _null, ref _null, OnlyCancellled, CancelField);
    }
    #endregion

    #region GetItems
    /// <summary>
    /// Obtiene los elementos del Grid
    /// </summary>
    /// <param name="Grid"></param>
    /// <param name="CheckedStatus"></param>
    /// <param name="CancelField"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    public static List<string> GetItems(DataGrid Grid, EnumCheckedStatus CheckedStatus, string CancelField)
    {
      switch (CheckedStatus)
      {
        case EnumCheckedStatus.All:
          return GridHelper.GetItems(Grid, "gegi");
        case EnumCheckedStatus.Checked:
          return GridHelper.GetItemsChecked(Grid, "gegi", CancelField);
        case EnumCheckedStatus.Unchecked:
          return GridHelper.GetItemsChecked(Grid, "gegi", CancelField, false);
        default:
          return GridHelper.GetItems(Grid, "gegi");
      }
    } 
    #endregion

  }
}

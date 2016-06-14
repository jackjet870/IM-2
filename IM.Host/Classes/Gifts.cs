using IM.Base.Helpers;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IM.Host.Classes
{
  /// <summary>
  /// Clase para el manejo de regalos regalos
  /// </summary>
  /// <history>
  /// [vipacheco] 24/Mayo/2016 Created
  /// </history>
  public class Gifts
  {

    /// <summary>
    /// Calcula el costo y precio total de regalos
    /// </summary>
    /// <param name="Grid"></param>
    /// <param name="GiftsType"></param>
    /// <param name="txtTotalCost"></param>
    /// <param name="txtTotalPrice"></param>
    /// <param name="txtTotalToPay"></param>
    /// <param name="OnlyCancellled"></param>
    /// <param name="CancelField"></param>
    /// <history>
    /// [vipacheco] 24/Mayo/2016 Created
    /// </history>
    public static void CalculateTotalGifts(DataGrid Grid, EnumGiftsType GiftsType, ref TextBox txtTotalCost, ref TextBox txtTotalPrice, ref TextBox txtTotalToPay,
                                bool OnlyCancellled = false, string CancelField = "")
    {
      decimal curCost = 0;
      decimal curPrice = 0;
      decimal curTotalCost = 0;
      decimal curTotalPrice = 0;
      decimal curTotalToPay = 0;

      foreach (var item in Grid.Items)
      {
        Type type = item.GetType(); // Obtenemos el tipo

        if (((int)type.GetProperty("geQty").GetValue(item, null)) != 0 && ((string)type.GetProperty("gegi").GetValue(item, null)) != "")
        {
          // Calculamos el costo del regalo
          curCost = ((decimal)type.GetProperty("gePriceA").GetValue(item, null)) + ((decimal)type.GetProperty("gePriceM").GetValue(item, null));

          // Calculamos el precio del regalo
          curPrice = ((decimal)type.GetProperty("gePriceAdult").GetValue(item, null)) + ((decimal)type.GetProperty("gePriceMinor").GetValue(item, null)) + ((decimal)type.GetProperty("gePriceExtraAdult").GetValue(item, null));

          // Si se desean todos los regalos
          if (!OnlyCancellled)
          {
            // Si la cantidad es positiva
            if (((int)type.GetProperty("geQty").GetValue(item, null)) > 0)
            {
              curTotalCost += curCost;
              curTotalPrice += curPrice;

              // Si es del recibo de regalo
              if (GiftsType == EnumGiftsType.ReceiptGifts)
              {
                // Si el regalo esta marcado como venta
                if (((bool)type.GetProperty("geSale").GetValue(item, null)) == true)
                {
                  curTotalToPay += curPrice;
                }
              }
            }
          }
          // Si se desean solo los regalos cancelados
          else
          {
            // Si el regalo se desea cancelar
            if (((bool)type.GetProperty(CancelField).GetValue(item, null)) != false)
            {
              curTotalCost += curCost;
              curTotalPrice += curPrice;
            }
          }
        }
      }

      // Actualizamos la etiqueta costo total
      txtTotalCost.Text = string.Format("{0:C2}", curTotalCost);

      // Actualizamos la etiqueta de precio total
      if (txtTotalPrice != null)
      {
        txtTotalPrice.Text = string.Format("{0:C2}", curTotalPrice);
      }

      //Actualizamos la etiqueta de total a pagar
      if (txtTotalToPay != null)
      {
        txtTotalToPay.Text = string.Format("{0:C2}", curTotalToPay);
      }

    }


    #region ValidateMaxAuthGifts
    /// <summary>
    /// Valida el monto maximo de regalos
    /// </summary>
    /// <param name="totalGifts"></param>
    /// <param name="maxAuthGifts"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// [vipacheco] 26/Mayo/2016 Modified --> Se migro a esta clase generica
    /// </history>
    public static bool ValidateMaxAuthGifts(string totalGifts, string maxAuthGifts)
    {
      decimal pcurTotalGifts = Convert.ToDecimal(totalGifts.Trim(new char[] { '$' }));
      decimal pcurMaxAuthGifts = Convert.ToDecimal(maxAuthGifts.Trim(new char[] { '$' }));

      // si se rebasa el monto maximo de regalos
      if (pcurTotalGifts > pcurMaxAuthGifts)
      {
        decimal curCharge = pcurTotalGifts - pcurMaxAuthGifts;
        string message = "The maximum amount authorized of gifts has been exceeded. \r\n" +
                              "Max authorized = " + String.Format("{0:C2", pcurMaxAuthGifts) + "\r\n" +
                              "Total Gifts = " + string.Format("{0:C2}", pcurTotalGifts) + "\r\n" +
                              "It will generate a charge of " + string.Format("{0:C2}", curCharge) + " to PR \r\n Save anyway?";


        return UIHelper.ShowMessage(message, MessageBoxImage.Question) == MessageBoxResult.Yes ? true : false;
      }

      return true;
    }
    #endregion
  }
}

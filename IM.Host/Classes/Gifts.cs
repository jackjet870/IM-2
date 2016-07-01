using IM.Base.Helpers;
using IM.Model;
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

    #region CalculateTotalGifts
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
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

        if (properties.Count > 0) // Verificamos que tenga propiedades
        {
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
    } 
    #endregion

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
      decimal pcurTotalGifts = Convert.ToDecimal(totalGifts.TrimStart('$'));
      decimal pcurMaxAuthGifts = Convert.ToDecimal(maxAuthGifts.TrimStart('$'));

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

    #region ValidateAdultsMinors
    /// <summary>
    /// Valida la cantidad de adultos y menores
    /// </summary>
    /// <param name="pEnum"></param>
    /// <param name="pRow"></param>
    /// <param name="pCancel"></param>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created
    /// </history>
    public static void ValidateAdultsMinors(EnumAdultsMinors pEnum, GiftsReceiptDetail pRow, ref bool pCancel)
    {
      int value = 0;

      switch (pEnum)
      {
        case EnumAdultsMinors.Adults:
          value = pRow.geAdults;
          GridHelper.ValidateEditNumber(ref value, ref pCancel, "Quantity of adults", 99, 0);
          pRow.geAdults = value;
          break;
        case EnumAdultsMinors.Minors:
          value = pRow.geMinors;
          GridHelper.ValidateEditNumber(ref value, ref pCancel, "Quantity of minors", 99, 0);
          pRow.geAdults = value;
          break;
        case EnumAdultsMinors.ExtraAdults:
          value = pRow.geMinors;
          GridHelper.ValidateEditNumber(ref value, ref pCancel, "Quantity of extra adults", 99, 0);
          pRow.geAdults = value;
          break;
      }

      // si es valido
      if (!pCancel)
      {
        if (pRow.geAdults == 0 && pRow.geMinors == 0)
        {
          pCancel = true;
          UIHelper.ShowMessage("Quantity of adults and quantity of minors they can not both be zero.", MessageBoxImage.Exclamation, "Intelligence MArketing");
        }
      }

    }
    #endregion

    #region ValidateMaxQuantityOnEntryQuantity
    /// <summary>
    /// Valida la cantidad maxima de un regalo al ingresar la cantidad
    /// </summary>
    /// <param name="pRow"></param>
    /// <param name="pIsExchange"></param>
    /// <param name="pCancel"></param>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created
    /// </history>
    public static void ValidateMaxQuantityOnEntryQuantity(GiftsReceiptDetail pRow, bool pIsExchange, int pLowerBound, ref bool pCancel)
    {
      int value = pRow.geQty;
      GridHelper.ValidateEditNumber(ref value, ref pCancel, "Quantity ", 999, pLowerBound, 1);

      // si es valido
      if (!pCancel)
      {
        Gift gift = frmHost._lstGifts.Where(x => x.giID == pRow.gegi).First();

        // Si ya se ingreso el regalo
        if (gift != null)
        {
          pCancel = !ValidateMaxQuantity(gift, pRow.geQty, pIsExchange);
        }
      }
    } 
    #endregion

    #region ValidateMaxQuantity
    /// <summary>
    /// Valida la cantidad maxima de un regalo
    /// </summary>
    /// <param name="pGift"></param>
    /// <param name="pQuantity"></param>
    /// <param name="pIsExchange"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created
    /// </history>
    public static bool ValidateMaxQuantity(Gift pGift, int pQuantity, bool pIsExchange)
    {
      // si el regalo tiene limite de cantidad
      if (pGift.giMaxQty > 0)
      {
        // si la cantidad maxima del regalo fue excedida
        if (pQuantity > pGift.giMaxQty)
        {
          UIHelper.ShowMessage("The maximum quantity authorized of the gift " + pGift.giN + "has been exceeded. \r\n" +
                                "Max authorized = " + pGift.giMaxQty, MessageBoxImage.Exclamation, "Intelligence Marketing");
          return false;
        }
      }
      // si el regalo tiene monto modificable, solo se permite un regalo
      else if (pGift.giAmountModifiable)
      {
        // si es un recibo normal
        if (!pIsExchange)
        {
          UIHelper.ShowMessage("The gift " + pGift.giN + " has amount modifiable therefore quantity must be 1 or -1.", MessageBoxImage.Exclamation, "Intelligence Marketing");
          return false;
        }
      }

      return true;
    } 
    #endregion

  }
}

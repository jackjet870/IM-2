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

    #region CalculateCostsPrices
    /// <summary>
    /// Calcula los costos y precios de adultos y menores de un regalo
    ///          Total = Cantidad de regalos * Cantidad de personas * Precio
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pGrid"></param>
    /// <param name="pRow"></param>
    /// <param name="pGiftField"></param>
    /// <param name="pQuantityField"></param>
    /// <param name="pAdultsField"></param>
    /// <param name="pMinorsField"></param>
    /// <param name="pExtraAdultsField"></param>
    /// <param name="pCostAdultsField"></param>
    /// <param name="pCostMinorsField"></param>
    /// <param name="pPriceAdultsField"></param>
    /// <param name="pPriceMinorsField"></param>
    /// <param name="pPriceExtraAdultsField"></param>
    /// <param name="pLstGifts"></param>
    /// <param name="pUseCxCCost"></param>
    /// <param name="pEnum"></param>
    /// <history>
    /// [vipacheco] 04/Julio/2016 Created
    /// </history>
    public static void CalculateCostsPrices<T,U>(ref T pCurrent, int pRow, string pGiftField, string pQuantityField, string pAdultsField, string pMinorsField,
                                               string pExtraAdultsField, string pCostAdultsField, string pCostMinorsField, string pPriceAdultsField, string pPriceMinorsField,
                                               string pPriceExtraAdultsField, List<U> pLstGifts, bool pUseCxCCost = false, EnumPriceType pEnum = EnumPriceType.All)
    {
      decimal costAdult, costMinor, priceAdult, priceMinor, priceExtraAdult, quantity;

      // Obtenemos su tipo
      Type type = pCurrent.GetType();

      // Si se encuentra el regalo
      T objGift = pCurrent;
      var gift = pLstGifts.Where(x => (string)x.GetType().GetProperty("giID").GetValue(x, null) == (string)type.GetProperty(pGiftField).GetValue(objGift, null)).FirstOrDefault();
      if (gift != null)
      {
        // costos
        // Si se va usar el costo de empleado
        if (pUseCxCCost)
        {
          costAdult = (decimal)gift.GetType().GetProperty("giPrice3").GetValue(gift, null);
          costMinor = (decimal)gift.GetType().GetProperty("giPrice4").GetValue(gift, null);
        }
        // si se va a usar el costo publico
        else
        {
          costAdult = (decimal)gift.GetType().GetProperty("giPrice1").GetValue(gift, null);
          costMinor = (decimal)gift.GetType().GetProperty("giPrice2").GetValue(gift, null);
        }
        // Precios
        priceAdult = (decimal)gift.GetType().GetProperty("giPublicPrice").GetValue(gift, null);
        priceMinor = (decimal)gift.GetType().GetProperty("giPriceMinor").GetValue(gift, null);
        priceExtraAdult = (decimal)gift.GetType().GetProperty("giPriceExtraAdult").GetValue(gift, null);
        quantity = (int)type.GetProperty(pQuantityField).GetValue(pCurrent, null);

        switch (pEnum)
        {
          case EnumPriceType.All:
            // Total del costo adultos
            pCurrent.GetType().GetProperty(pCostAdultsField).SetValue(pCurrent, (quantity * ((int)type.GetProperty(pAdultsField).GetValue(pCurrent, null) + (int)type.GetProperty(pExtraAdultsField).GetValue(pCurrent, null)) * costAdult), null);
            // Total del costo de menores
            type.GetProperty(pCostMinorsField).SetValue(pCurrent, (quantity * (int)type.GetProperty(pMinorsField).GetValue(pCurrent, null) * costMinor), null);
            // Total del precio adultos
            type.GetProperty(pPriceAdultsField).SetValue(pCurrent, (quantity * (int)type.GetProperty(pAdultsField).GetValue(pCurrent, null) * priceAdult), null);
            // Total del precio de menores
            type.GetProperty(pPriceMinorsField).SetValue(pCurrent, (quantity * (int)type.GetProperty(pMinorsField).GetValue(pCurrent, null) * priceMinor), null);
            // Total del precio de adultos extra
            type.GetProperty(pPriceExtraAdultsField).SetValue(pCurrent, (quantity * (int)type.GetProperty(pExtraAdultsField).GetValue(pCurrent, null) * priceExtraAdult), null);
            break;
          case EnumPriceType.Adults:
            // Total del costo adultos
            type.GetProperty(pCostAdultsField).SetValue(pCurrent, (quantity * ((int)type.GetProperty(pAdultsField).GetValue(pCurrent, null) + (int)type.GetProperty(pExtraAdultsField).GetValue(pCurrent, null)) * costAdult), null);
            // Total del precio adultos
            type.GetProperty(pPriceAdultsField).SetValue(pCurrent, (quantity * (int)type.GetProperty(pAdultsField).GetValue(pCurrent, null) * priceAdult), null);
            break;
          case EnumPriceType.Minors:
            // Total del costo de menores
            type.GetProperty(pCostMinorsField).SetValue(pCurrent, (quantity * (int)type.GetProperty(pMinorsField).GetValue(pCurrent, null) * costMinor), null);
            // Total del precio de menores
            type.GetProperty(pPriceMinorsField).SetValue(pCurrent, (quantity * (int)type.GetProperty(pMinorsField).GetValue(pCurrent, null) * priceMinor), null);
            break;
          case EnumPriceType.ExtraAdults:
            // Total del costo adultos
            type.GetProperty(pCostAdultsField).SetValue(pCurrent, (quantity * ((int)type.GetProperty(pAdultsField).GetValue(pCurrent, null) + (int)type.GetProperty(pExtraAdultsField).GetValue(pCurrent, null)) * costAdult), null);
            // Total del precio de adultos extra
            type.GetProperty(pPriceExtraAdultsField).SetValue(pCurrent, (quantity * (int)type.GetProperty(pExtraAdultsField).GetValue(pCurrent, null) * priceExtraAdult), null);
            break;
        }
      }
    }
    #endregion

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
      bool cancel = false;

      switch (pEnum)
      {
        case EnumAdultsMinors.Adults:
          value = pRow.geAdults;
          GridHelper.ValidateEditNumber(ref value, ref cancel, "Quantity of adults", 99, 0);
          break;
        case EnumAdultsMinors.Minors:
          value = pRow.geMinors;
          GridHelper.ValidateEditNumber(ref value, ref cancel, "Quantity of minors", 99, 0);
          break;
        case EnumAdultsMinors.ExtraAdults:
          value = pRow.geMinors;
          GridHelper.ValidateEditNumber(ref value, ref cancel, "Quantity of extra adults", 99, 0);
          break;
      }
      // si es valido
      if (!cancel)
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
    public static void ValidateMaxQuantityOnEntryQuantity<T>(ref T pRow, bool pIsExchange, int pLowerBound, ref bool pCancel, string pQuantityField, string pGiftField)
    {
      int value = (int)pRow.GetType().GetProperty(pQuantityField).GetValue(pRow, null);
      bool cancel = false;
      GridHelper.ValidateEditNumber(ref value, ref cancel, "Quantity ", 9999, pLowerBound, 1);

      // si es valido
      if (!cancel)
      {
        Gift gift = null;
        if (pRow.GetType().GetProperty(pGiftField).GetValue(pRow, null) != null)
        {
          string strGift = (string)pRow.GetType().GetProperty(pGiftField).GetValue(pRow, null);
          gift = frmHost._lstGifts.Where(x => x.giID == strGift).First();
        }

        // Si ya se ingreso el regalo
        if (gift != null)
        {
          pCancel = !ValidateMaxQuantity(gift, value, pIsExchange, ref pRow, pQuantityField);
        }
      }
      else
      {
        pCancel = true;
      }
    }
    #endregion

    #region ValidateMaxQuantityOnEntryGift
    /// <summary>
    /// Valida la cantidad maxima de un regalo al ingresar el regalo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pGift"></param>
    /// <param name="pQuantityField"></param>
    /// <param name="pRow"></param>
    /// <param name="pIsExchange"></param>
    /// <history>
    /// [vipacheco] 04/Julio/2016 Created
    /// </history>
    public static void ValidateMaxQuantityOnEntryGift<T>(Gift pGift, string pQuantityField, ref T pRow, bool pIsExchange)
    {
      Type type = pRow.GetType();

      if (!ValidateMaxQuantity(pGift, (int)type.GetProperty(pQuantityField).GetValue(pRow, null), pIsExchange, ref pRow, pQuantityField))
      {
        // establecemos como cantidad la cantidad maxima autorizada
        pRow.GetType().GetProperty(pQuantityField).SetValue(pRow, pGift.giMaxQty, null);
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
    public static bool ValidateMaxQuantity<T>(Gift pGift, int pQuantity, bool pIsExchange, ref T pRow, string pQuantityField)
    {
      // si el regalo tiene limite de cantidad
      if (pGift.giMaxQty > 0)
      {
        // si la cantidad maxima del regalo fue excedida
        if (pQuantity > pGift.giMaxQty)
        {
          UIHelper.ShowMessage("The maximum quantity authorized of the gift " + pGift.giN + "has been exceeded. \r\n" +
                                "Max authorized = " + pGift.giMaxQty, MessageBoxImage.Exclamation, "Intelligence Marketing");
          pRow.GetType().GetProperty(pQuantityField).SetValue(pRow, pGift.giMaxQty, null);
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
          pRow.GetType().GetProperty(pQuantityField).SetValue(pRow, 1, null);
          return false;
        }
      }

      return true;
    } 
    #endregion

  }
}

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

namespace IM.Base.Classes
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
    /// <param name="pCurrent">Tipo de objeto del grid</param>
    /// <param name="gift"></param>
    /// <param name="pQuantityField">Nombre de la propiedad</param>
    /// <param name="pAdultsField">Nombre de la propiedad</param>
    /// <param name="pMinorsField">Nombre de la propiedad</param>
    /// <param name="pExtraAdultsField">Nombre de la propiedad</param>
    /// <param name="pCostAdultsField">Nombre de la propiedad</param>
    /// <param name="pCostMinorsField">Nombre de la propiedad</param>
    /// <param name="pPriceAdultsField">Nombre de la propiedad</param>
    /// <param name="pPriceMinorsField">Nombre de la propiedad</param>
    /// <param name="pPriceExtraAdultsField">Nombre de la propiedad</param>
    /// <param name="pUseCxCCost">True Costo empleado | False Costo publico</param>
    /// <param name="pEnum">Identifica el campo que vamos a calcular</param>
    /// <history>
    /// [vipacheco] 04/Julio/2016 Created
    /// [erosado] 26/07/2016  Modified. Se eliminaron parametros y se agrego el parametro Gift, para eliminar el reflection.
    /// </history>
    public static void CalculateCostsPrices<T>(ref T pCurrent, Gift gift,  string pQuantityField, string pAdultsField, string pMinorsField,
                                               string pExtraAdultsField, string pCostAdultsField, string pCostMinorsField, string pPriceAdultsField, string pPriceMinorsField,
                                               string pPriceExtraAdultsField,bool pUseCxCCost = false, EnumPriceType pEnum = EnumPriceType.All)
    {
      decimal costAdult, costMinor, priceAdult, priceMinor, priceExtraAdult, quantity;

      // Obtenemos su tipo
      Type type = pCurrent.GetType();

      // Si se encuentra el regalo
      T objGift = pCurrent;
    
      if (gift != null)
      {
        // costos
        // Si se va usar el costo de empleado
        if (pUseCxCCost)
        {
          costAdult = gift.giPrice3;
          costMinor = gift.giPrice4;
        }
        // si se va a usar el costo publico
        else
        {
          costAdult = gift.giPrice1;
          costMinor = gift.giPrice2;
        }
        // Precios
        priceAdult = gift.giPublicPrice;
        priceMinor = gift.giPriceMinor;
        priceExtraAdult = gift.giPriceExtraAdult;
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
    /// <param name="Grid">Grid donde estan los gift</param>
    /// <param name="GiftsType">Tipo de regalo</param>
    /// <param name="txtTotalCost">Costo total</param>
    /// <param name="txtTotalPrice">Precio total</param>
    /// <param name="txtTotalToPay">Total a pagar</param>
    /// <param name="OnlyCancellled">True resta el costo del gift | False No hace nada</param>
    /// <param name="CancelField">Propiedad para cancelar un gift</param>
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
    /// <param name="totalGifts">Total de costo de los regalos</param>
    /// <param name="maxAuthGifts">Máximo autorizado de los regalos</param>
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
    /// <param name="Enum">Identifica el campor que vamos a calcular</param>
    /// <param name="Row">Objeto del grid</param>
    /// <param name="Cancel">True Fallo la validacion | False Paso la validacion</param>
    /// <param name="adultsField">Propiedad Adults</param>
    /// <param name="minorsField">Propiedaad Minors</param>
    /// <param name="extraAdultsField">Propiedad ExtraAdults</param>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created
    /// [erosado] 26/07/2016  Modified. Se agrego Reflection para volver generico, y se agrego el parametro ExtraAdults
    /// </history>
    public static void ValidateAdultsMinors<T>(EnumAdultsMinors Enum, T Row, ref bool Cancel, string adultsField, string minorsField, string extraAdultsField)
    {
      int value = 0;
      bool cancel = false;

      switch (Enum)
      {
        case EnumAdultsMinors.Adults:
          value =(int)Row.GetType().GetProperty(adultsField).GetValue(Row, null);
          GridHelper.ValidateEditNumber(ref value, ref cancel, "Quantity of adults", 99, 0);
          break;
        case EnumAdultsMinors.Minors:
          value = (int)Row.GetType().GetProperty(minorsField).GetValue(Row, null);
          GridHelper.ValidateEditNumber(ref value, ref cancel, "Quantity of minors", 99, 0);
          break;
        case EnumAdultsMinors.ExtraAdults:
          value = (int)Row.GetType().GetProperty(extraAdultsField).GetValue(Row, null);
          GridHelper.ValidateEditNumber(ref value, ref cancel, "Quantity of extra adults", 99, 0);
          break;
      }
      // si es valido
      if (!cancel)
      {
        if ((int)Row.GetType().GetProperty(adultsField).GetValue(Row, null) == 0 && (int)Row.GetType().GetProperty(minorsField).GetValue(Row, null) == 0)
        {
          Cancel = true;
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
    /// <param name="gift">Gift que vamos calcular</param>
    /// <param name="pLowerBound">limite inferior para ingresar</param>
    /// <param name="pQuantityField">Propiedad Cantidad del regalo</param>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created
    /// [erosado] 26/07/2016  Modified. se agregó el parametro Gift.
    /// </history>
    public static void ValidateMaxQuantityOnEntryQuantity<T>(ref T pRow, Gift gift ,bool pIsExchange, int pLowerBound, ref bool pCancel, string pQuantityField)
    {
      int value = (int)pRow.GetType().GetProperty(pQuantityField).GetValue(pRow, null);
      bool cancel = false;
      GridHelper.ValidateEditNumber(ref value, ref cancel, "Quantity ", 9999, pLowerBound, 1);

      // si es valido
      if (!cancel)
      {
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
    /// <param name="pGift">Informacion  del Gift</param>
    /// <param name="pQuantity">Cantidad ingresada</param>
    /// <param name="pIsExchange">True es de intercambio | False No </param>
    /// <param name="pQuantityField">Propiedad Qty</param>
    /// <param name="pRow">Objeto grid</param>
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

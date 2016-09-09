using IM.Base.Helpers;
using IM.BusinessRules.BR;
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
    public static void CalculateCostsPrices<T>(ref T pCurrent, Gift gift, string pQuantityField, string pAdultsField, string pMinorsField,
                                               string pExtraAdultsField, string pCostAdultsField, string pCostMinorsField, string pPriceAdultsField, string pPriceMinorsField,
                                               string pPriceExtraAdultsField, bool pUseCxCCost = false, EnumPriceType pEnum = EnumPriceType.All)
    {
      decimal costAdult, costMinor, priceAdult, priceMinor, priceExtraAdult, quantity;

      // Obtenemos su tipo
      Type type = pCurrent.GetType();

      // Si se encuentra el regalo

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
    /// [erosado] 27/07/2016  Modified. Se volvio generico
    /// </history>
    public static void CalculateTotalGifts(DataGrid Grid, EnumGiftsType GiftsType, string QuantityField, string giftField, string priceMField,
      string priceMinorField, string priceAdultField, string priceAField, string priceExtraAdultsField, TextBox txtTotalCost,
      TextBox txtTotalPrice = null, TextBox txtTotalToPay = null, bool OnlyCancellled = false, string CancelField = "", string saleField = "")
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
          if (((int)type.GetProperty(QuantityField).GetValue(item, null)) != 0 && ((string)type.GetProperty(giftField).GetValue(item, null)) != "")
          {
            // Calculamos el costo del regalo
            curCost = ((decimal)type.GetProperty(priceAField).GetValue(item, null)) + ((decimal)type.GetProperty(priceMField).GetValue(item, null));

            // Calculamos el precio del regalo
            curPrice = ((decimal)type.GetProperty(priceAdultField).GetValue(item, null)) + ((decimal)type.GetProperty(priceMinorField).GetValue(item, null)) + ((decimal)type.GetProperty(priceExtraAdultsField).GetValue(item, null));

            // Si se desean todos los regalos
            if (!OnlyCancellled)
            {
              // Si la cantidad es positiva
              if (((int)type.GetProperty(QuantityField).GetValue(item, null)) > 0)
              {
                curTotalCost += curCost;
                curTotalPrice += curPrice;

                // Si es del recibo de regalo
                if (GiftsType == EnumGiftsType.ReceiptGifts)
                {
                  // Si el regalo esta marcado como venta
                  if (((bool)type.GetProperty(saleField).GetValue(item, null)) == true)
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
        txtTotalCost.IsReadOnly = true;

        // Actualizamos la etiqueta de precio total
        if (txtTotalPrice != null)
        {
          txtTotalPrice.Text = string.Format("{0:C2}", curTotalPrice);
          txtTotalPrice.IsReadOnly = true;
        }

        //Actualizamos la etiqueta de total a pagar
        if (txtTotalToPay != null)
        {
          txtTotalToPay.Text = string.Format("{0:C2}", curTotalToPay);
          txtTotalToPay.IsReadOnly = true;
        }
      }
    }
    #endregion

    #region ValidateMaxAuthGifts
    /// <summary>
    /// Valida el monto maximo de regalos
    /// </summary>
    /// <param name="totalCostGifts">Total de costo de los regalos</param>
    /// <param name="maxAuthGifts">Máximo autorizado de los regalos</param>
    /// <returns>True totalCostGift Autorizado | False No autorizado</returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// [vipacheco] 26/Mayo/2016 Modified --> Se migro a esta clase generica
    /// </history>
    public static bool ValidateMaxAuthGifts(string totalCostGifts, string maxAuthGifts)
    {
      decimal totalCost = 0;
      decimal maxAuth = 0;

      if (decimal.TryParse(totalCostGifts.TrimStart('$'), out totalCost) && decimal.TryParse(maxAuthGifts.TrimStart('$'), out maxAuth))
      {

        // si se rebasa el monto maximo de regalos
        if (totalCost > maxAuth)
        {
          decimal curCharge = totalCost - totalCost;
          string message = "The maximum amount authorized of gifts has been exceeded. \r\n" +
                                "Max authorized = " + String.Format("{0:C2}", totalCost) + "\r\n" +
                                "Total Gifts = " + string.Format("{0:C2}", totalCost) + "\r\n" +
                                "It will generate a charge of " + string.Format("{0:C2}", curCharge) + " to PR \r\n Save anyway?";

          return UIHelper.ShowMessage(message, MessageBoxImage.Question) == MessageBoxResult.Yes ? true : false;
        }

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
    public static void ValidateAdultsMinors<T>(EnumAdultsMinors Enum, T Row, ref bool Cancel, string adultsField, string minorsField, string extraAdultsField = "")
    {
      int value = 0;
      bool cancel = false;

      switch (Enum)
      {
        case EnumAdultsMinors.Adults:
          value = (int)Row.GetType().GetProperty(adultsField).GetValue(Row, null);
          GridHelper.ValidateEditNumber(ref value, cancel, "Quantity of adults", 99, 0);
          break;
        case EnumAdultsMinors.Minors:
          value = (int)Row.GetType().GetProperty(minorsField).GetValue(Row, null);
          GridHelper.ValidateEditNumber(ref value, cancel, "Quantity of minors", 99, 0);
          break;
        case EnumAdultsMinors.ExtraAdults:
          value = (int)Row.GetType().GetProperty(extraAdultsField).GetValue(Row, null);
          GridHelper.ValidateEditNumber(ref value, cancel, "Quantity of extra adults", 99, 0);
          break;
      }
      // si es valido
      if (!cancel)
      {
        //Comparo AdultsField con AdultsMinors, Si son 0 ambos mando un mensaje
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
    /// <param name="row"></param>
    /// <param name="isExchange"></param>
    /// <param name="pCancel"></param>
    /// <param name="gift">Gift que vamos calcular</param>
    /// <param name="lowerBound">limite inferior para ingresar</param>
    /// <param name="quantityField">Propiedad Cantidad del regalo</param>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created
    /// [erosado] 26/07/2016  Modified. se agregó el parametro Gift.
    /// </history>
    public static void ValidateMaxQuantityOnEntryQuantity<T>(ref T row, Gift gift, bool isExchange, int lowerBound, ref bool pCancel, string quantityField)
    {
      int value = (int)row.GetType().GetProperty(quantityField).GetValue(row);
      bool cancel = false;
      GridHelper.ValidateEditNumber(ref value, cancel, "Quantity ", 9999, lowerBound, 1);

      // si es valido
      if (!cancel)
      {
        // Si ya se ingreso el regalo
        if (gift != null)
        {
          pCancel = !ValidateMaxQuantity(gift, value, isExchange, ref row, quantityField);
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
          UIHelper.ShowMessage("The maximum quantity authorized of the gift " + pGift.giN + " has been exceeded. \r\n" +
                                "Max authorized = " + pGift.giMaxQty, MessageBoxImage.Exclamation, "Intelligence Marketing");

          //Le asigna el valor Maximo que permite regalar ese Gift
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

    #region ValidateMaxQuantityGiftTour
    /// <summary>
    /// Valida la informacion del GuestStaus x los regalos || Valida los regalos y el GuestStatus || Tours validados por Guest
    /// </summary>
    /// <param name="dtg"> Datagrid donde se encuentran la lista de Gifts a validar </param>
    /// <param name="guestStatus">Informacion para validar el status de un Guest </param>
    /// <param name="qtyField"> Nombre de la propiedad que contiene Qty</param>
    /// <param name="giftField"> Nombre de la propiedad que contiene el giftID </param>
    /// <returns>TRUE - Tours validos | FALSE - Se ingreso una candidad maxima a los TOURS permitidos</returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    public static bool ValidateGiftsGuestStatus(DataGrid dtg, GuestStatusValidateData guestStatus, string qtyField, string giftField)
    {
      int iToursUsed, iDiscsUsed, iTourAllowed, iTours, iTCont = 0, iDCont = 0, iMaxTours;
      decimal iPax, iDiscAllowed, iDisc, iAdults = 0, iMinors = 0, TotPax = 0;
      bool? blnDisc;
      string strMsg = "";

      // Asignamos los valores del GuestStatus para validar
      iMaxTours = guestStatus.gsMaxQtyTours ?? 0;
      iToursUsed = guestStatus.TourUsed;
      blnDisc = guestStatus.gsAllowTourDisc;
      iDiscsUsed = guestStatus.DiscUsed;
      iPax = guestStatus.guPax;

      // Calculamos el total Pax
      CalculateAdultsMinorsByPax(iPax, ref iAdults, ref iMinors);
      TotPax = iAdults + iMinors;

      // Los Tours permitidos
      iTourAllowed = iMaxTours - iToursUsed;
      iTours = iTourAllowed;

      // Validamos con cada registro de tour
      foreach (var item in dtg.Items.SourceCollection)
      {
        var properties = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();

        if (properties.Any())
        {
          var giftValue = item.GetType().GetProperty(giftField).GetValue(item) as Gift;
          int qtyValue = Convert.ToInt32(item.GetType().GetProperty(qtyField).GetValue(item));

          if (giftValue != null)
          {
            // Evaluamos si son de toures y con descuento
            if (giftValue.gigc == "TOURS" && !(bool)giftValue.giDiscount)
            {
              iTours += iTours - (giftValue.giQty * qtyValue);
              iTCont += iTCont + (giftValue.giQty * qtyValue);
            }
          }
        }
      }

      // Los descuentos permitidos son los restantes de los PAX restantes
      iDiscAllowed = TotPax - iTCont;
      iDiscAllowed = iDiscAllowed - iDiscsUsed;
      iDisc = iDiscAllowed;

      // Validamos con cada registro de descuentos
      foreach (var item in dtg.Items.SourceCollection)
      {
        var properties = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();

        if (properties.Any())
        {
          var giftValue = item.GetType().GetProperty(giftField).GetValue(item) as Gift;
          int qtyValue = Convert.ToInt32(item.GetType().GetProperty(qtyField).GetValue(item));

          if (giftValue != null)
          {
            if (giftValue.gigc == nameof(EnumGiftCategory.TOURS) && (bool)giftValue.giDiscount)
            {
              iDisc = iDisc - (giftValue.giQty * qtyValue);
              iDCont = iDCont + (giftValue.giQty * qtyValue);
            }
          }
        }
      }

      //Revisamos el remanente de la revision de Gifts
      if (iTours < 0)
      {
        UIHelper.ShowMessage("The maximum number of tours " + iTourAllowed + " has been exceeded. \r\n There are " + iTCont + " tours on this receipt", MessageBoxImage.Exclamation);
        return false;
      }

      if (iDisc < 0 && strMsg == "")
      {
        UIHelper.ShowMessage("The maximum number of discount tours " + iDiscAllowed + " has been exceeded.\r\n There are " + iDCont + " discount tours on this receipt", MessageBoxImage.Exclamation);
        return false;
      }

      return true;
    }
    #endregion

    #region CalculateAdultsMinorsByPax
    /// <summary>
    /// Calcula el numero de adultos y menores en base al Pax
    /// </summary>
    /// <param name="pcurPax"></param>
    /// <param name="piAdults"></param>
    /// <param name="piMinors"></param>
    /// <history>
    /// [vipacheco] 10/Mayo/2016 Created
    /// </history>
    private static void CalculateAdultsMinorsByPax(decimal pcurPax, ref decimal piAdults, ref decimal piMinors)
    {
      piAdults = Convert.ToInt32(pcurPax);
      piMinors = (pcurPax - piAdults) * 10;
    }
    #endregion

    //#region LoadGuesStatusInfo
    ///// <summary>
    ///// Carga la informacion de GuestStatus para validaicon de nuevo schema de regalos
    ///// </summary>
    ///// <param name="guestID"> Clave del Guest</param>
    ///// <param name="applyGuestStatusValidation"></param>
    ///// <history>
    ///// [vipacheco] 19/Abril/2016 Created
    ///// [vipacheco] 08/Agosto/2016 Modified --> Migrado a esta clase  para el uso generico
    ///// </history>
    //public static void LoadGuesStatusInfo(int guestID, ref bool applyGuestStatusValidation, ref GuestStatusValidateData pGuestStatusInfo, int pReceiptID = 0)
    //{
    //  applyGuestStatusValidation = false;

    //  pGuestStatusInfo = BRGuestStatus.GetStatusValidateInfo(pGuestID, pReceiptID);

    //  // Solo si esta configurado se realiza la revision
    //  if (pGuestStatusInfo != null)
    //    if (pGuestStatusInfo.gsMaxQtyTours > 0)
    //      applyGuestStatusValidation = true;
    //}
    //#endregion

  }
}

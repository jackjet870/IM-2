using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportAppServer.Controllers;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Forms;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using IM.Base.Classes;

namespace IM.Host.Classes
{
  /// <summary>
  /// Clase encargada de procesar funciones en común con varias ventanas
  /// </summary>
  /// <history>
  /// [vipacheco] 14/Abril/2016 Created
  /// [vipacheco] 16/Mayo/2016 Modified --> Migrado a esta clase generica de Gifts Receipt
  /// </history>
  public class ReceiptsGifts
  {
    private static Guest _guest = new Guest();

    #region StartEdit
    /// <summary>
    /// Determina si se puede editar la informacion del grid
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="row"></param>
    /// <param name="cell"></param>
    /// <param name="grid"></param>
    /// <param name="pCancel"></param>
    /// <history>
    /// [vipacheco] 24/junio/2016 Created
    /// [vipacheco] 02/julio/2016 Modified --> se agrego un parametro mas para validaciones sobre el grid
    /// </history>
    public static void StartEdit(EnumMode mode, GiftsReceiptDetail row, ref DataGridCellInfo cell, ref DataGrid grid, ref bool pCancel)
    {
      // Obtenemos el index del row en edicion
      int rowIndex = grid.SelectedIndex != -1 ? grid.SelectedIndex : 0;

      switch (mode)
      {
        // Edicion total
        case EnumMode.Edit:
          switch (cell.Column.SortMemberPath)
          {
            // Cantidad
            case "geQty":
              bool blnResult = !EnableQuantity(row);
              cell.Column.IsReadOnly = blnResult;
              pCancel = blnResult;
              break;
            // Regalo
            case "gegi":
              // si no se ha ingresado la cantidad
              if (row.geQty == 0)
              {
                UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                pCancel = true;
                GridHelper.SelectRow(grid, rowIndex, 0);
              }
              // determinamos si se puede modificar el regalo
              else if (!EnableGift(row))
              {
                pCancel = false;
                cell.Column.IsReadOnly = true;
              }
              else
              {
                pCancel = false;
                cell.Column.IsReadOnly = false;
              }

              // Habilitamos las columnas
              DataGridCellInfo Adults = new DataGridCellInfo(grid.SelectedIndex, grid.Columns[3]);
              Adults.Column.IsReadOnly = false;
              DataGridCellInfo Minors = new DataGridCellInfo(grid.SelectedIndex, grid.Columns[4]);
              Minors.Column.IsReadOnly = false;
              DataGridCellInfo EAdults = new DataGridCellInfo(grid.SelectedIndex, grid.Columns[5]);
              EAdults.Column.IsReadOnly = false;

              break;
            // Numero de adultos, menores y adultos extra
            case "geAdults":
            case "geMinors":
            case "geExtraAdults":
              // si no se ha ingresado la cantidad
              if (row.geQty == 0)
              {
                UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                pCancel = true;
                GridHelper.SelectRow(grid, rowIndex, 0);
              }
              // si no se ha ingresado el regalo
              else if (row.gegi == null)
              {
                UIHelper.ShowMessage("Enter the Gift first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                pCancel = true;
                GridHelper.SelectRow(grid, rowIndex, 2);
              }
              else
              {
                // localizamos el regalo
                Gift _gift = frmHost._lstGifts.Where(x => x.giID == row.gegi).First();

                // se permite modificar si el regalo maneja Pax
                cell.Column.IsReadOnly = !_gift.giWPax;
                pCancel = !_gift.giWPax;
              }
              break;
            // Folios
            case "geFolios":
              // si no se ha ingresado la cantidad
              if (row.geQty == 0)
              {
                UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                pCancel = true;
                GridHelper.SelectRow(grid, rowIndex, 0);
              }
              // si no se ha ingresado el regalo
              else if (row.gegi == null)
              {
                UIHelper.ShowMessage("Enter the Gift first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                pCancel = true;
                GridHelper.SelectRow(grid, rowIndex, 2);
              }
              else
              {
                // localizamos el regalo
                Gift _gift = frmHost._lstGifts.Where(x => x.giID == row.gegi).First();

                // se permite modificar si el regalo maneja Pax
                cell.Column.IsReadOnly = !_gift.giWPax;
                pCancel = false;
              }
              break;
            case "gect":
            case "geCxC":
            case "geCharge":
              // si no se ha ingresado el regalo, la cantidad de adultos y la cantidad de menores
              if (row.gegi == null && row.geMinors == 0 && row.geAdults == 0)
              {
                UIHelper.ShowMessage("No gift and quantity has been selected.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                pCancel = true;
                GridHelper.SelectRow(grid, rowIndex, 0);
              }
              else
              {
                cell.Column.IsReadOnly = false;
                pCancel = false;
              }
              break;
            // Costo de adultos y menores
            case "gePriceA":
            case "gePriceM":
              // localizamos el regalo
              Gift _giftA = frmHost._lstGifts.Where(x => x.giID == row.gegi).First();

              // se permite modificar si el regalo permite modificar los montos
              cell.Column.IsReadOnly = !_giftA.giAmountModifiable;
              pCancel = false;
              break;
            // Regalos tipo venta y comentarios
            case "geSale":
            case "geComments":
              // si no se ha ingresado la cantidad
              if (row.geQty == 0)
              {
                UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                pCancel = true;
                GridHelper.SelectRow(grid, rowIndex, 0);
              }
              else
              {
                pCancel = false;
                cell.Column.IsReadOnly = false;
              }
              break;
            // las demas columnas no se permiten modificar
            default:
              pCancel = false;
              cell.Column.IsReadOnly = true;
              break;
          }
          break;

        case EnumMode.EditPartial:
          // si se ingreso la cantidad y el regalo
          if (row.geQty == 0 && row.gegi != null)
          {
            switch (cell.Column.SortMemberPath)
            {
              case "geFolios":
                // si no se ha ingresado la cantidad
                if (row.geQty == 0)
                {
                  UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                  pCancel = true;
                  GridHelper.SelectRow(grid, rowIndex, 0);
                }
                // si no se ha ingresado el regalo
                else if (row.gegi == null)
                {
                  UIHelper.ShowMessage("Enter the Gift first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                  pCancel = true;
                  GridHelper.SelectRow(grid, rowIndex, 2);
                }
                else
                {
                  // localizamos el regalo
                  Gift _gift = frmHost._lstGifts.Where(x => x.giID == row.gegi).First();

                  // se permite modificar si el regalo maneja folios
                  cell.Column.IsReadOnly = !_gift.giWFolio;
                  pCancel = false;
                }
                break;
              case "geComments":
                // si no se ha ingresado la cantidad
                if (row.geQty == 0)
                {
                  UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                  pCancel = true;
                  GridHelper.SelectRow(grid, rowIndex, 0);
                }
                // si no se ha ingresado el regalo
                else if (row.gegi == null)
                {
                  UIHelper.ShowMessage("Enter the Gift first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                  pCancel = true;
                  GridHelper.SelectRow(grid, rowIndex, 2);
                }
                else
                {
                  pCancel = false;
                  cell.Column.IsReadOnly = false;
                }
                break;
              default:
                pCancel = false;
                cell.Column.IsReadOnly = true;
                break;
            }
          }
          else
          {
            pCancel = false;
            cell.Column.IsReadOnly = true;
          }
          break;
        default:
          break;
      }
    }
    #endregion

    #region ValidateEdit
    /// <summary>
    /// Valida la informacion del grid
    /// </summary>
    /// <param name="dtg">Grid Principal</param>
    /// <param name="row">Objeto del grid seleccionada</param>
    /// <param name="isExchange">TRUE - Es de intercambio | FALSE - No es intercambio</param>
    /// <param name="cell"> Celda Actual</param>
    /// <returns> TRUE - Si se cancela la actualizacion | FALSE - Continua la actualizacion</returns>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created
    /// </history>
    public async static Task<bool> ValidateEdit(DataGrid dtg, GiftsReceiptDetail row, bool isExchange, DataGridCellInfo cell)
    {
      bool cancel = false;
      int LowerBound = 0;

      // si es un intercambio de regalos
      if (isExchange)
        LowerBound = -999;
      else
        LowerBound = 1;

      switch (cell.Column.SortMemberPath)
      {
        case nameof(row.geAdults):
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.Adults, row, ref cancel, "geAdults", "geMinors", "geExtraAdults");
          break;
        case nameof(row.geMinors):
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.Minors, row, ref cancel, "geAdults", "geMinors", "geExtraAdults");
          break;
        case nameof(row.geExtraAdults):
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.ExtraAdults, row, ref cancel, "geAdults", "geMinors", "geExtraAdults");
          break;
        case nameof(row.geQty):
          if (row.geQty == 0)
          {
            if (row.gegi != null)
            {
          //Obtenemos el Gift
              var gift = frmHost._lstGifts.Where(x => x.giID == row.gegi).First();

          // validamos la cantidad maxima del regalo
          Gifts.ValidateMaxQuantityOnEntryQuantity(ref row, gift, isExchange, LowerBound,ref  cancel, "geQty");

          //si el regalo esta guardado como promocion de Opera
              if (row.geAsPromotionOpera == true)
          {
            // ejecutamos el procedimiento almacenado
                GuestPromotion guest = await BRGuestsPromotions.GetGuestPromotion(row.gegr, row.gegi);
            int quantity = guest.gpQty;

            // validamos que no se le de una cantidad inferior a la que se le habia dado antes
                if (quantity < row.geQty)
            {
              UIHelper.ShowMessage("The quantity can not be lower than the previous (" + quantity + ").", MessageBoxImage.Information);
                  cancel = true;
            }
          }
            }
            else
            {
              int value = row.geQty;
              GridHelper.ValidateEditNumber(ref value, cancel, "Quantity ", 9999, LowerBound, 1);
              row.geQty = 1;
              GridHelper.UpdateCellsFromARow(dtg);
            }
          }
          break;
        case "geCxC":
          cancel = false;
          break;
        case "geFolios":
          if (row.geFolios != null)
          {
            // validamos los folios
            dynamic varTemp = ValidateFolios(row.geFolios);

            // si los folios no son  validos
            if (varTemp == null)
              cancel = true;
            else
            {
              // si el numero de folios coincide con la cantidad de regalos 
              if (varTemp.Count == Math.Abs(row.geQty))
              {
                cancel = false;
                row.geFolios = row.geFolios.ToUpper();
              }
              else
              {
                UIHelper.ShowMessage("The number of folios does not match the especified quantity.", MessageBoxImage.Information, "Intelligence Marketing");
                cancel = true;
              }
            }
          }
          break;
        case "geCharge":
          if (row.geCharge == -1)
          {
            row.geCharge = 0;
          }
          break;
      }
      return cancel;

    }
    #endregion

    #region AfterEdit
    /// <summary>
    /// Establece los valores default de las columnas del grid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pGrid"></param>
    /// <param name="pGuest"></param>
    /// <param name="pRowSelected"></param>
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
    /// <param name="row"></param>
    /// <param name="pCell"></param>
    /// <param name="pUseCxCCost"></param>
    /// <param name="pIsExchange"></param>
    /// <param name="pForm"></param>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created
    /// </history>
    public static void AfterEdit(ref DataGrid dtg, GuestShort pGuest, ref GiftsReceiptDetail row, DataGridCellInfo pCell, bool pUseCxCCost, bool pIsExchange, ChargeTo pChargeTo,
                                    string pLeadSourceID, TextBox pTxtTotalCost, TextBox pTxtTotalPrice = null, TextBox pTxtTotalToPay = null, TextBox pTxtgrCxCGifts = null,
                                    TextBox pTxtTotalCxC = null, TextBox pTxtgrCxCAdj = null, TextBox pTxtgrMaxAuthGifts = null, TextBlock pLblgrMaxAuthGifts = null)
    {
      bool calculateCharge = false;

      // Obtenemos los nombres de los campos a utilizar
      string quantityField = nameof(row.geQty);
      string adultsField = nameof(row.geAdults);
      string minorsField = nameof(row.geMinors);
      string extraAdultsField = nameof(row.geExtraAdults);
      string costAdultsField = nameof(row.gePriceA);
      string costMinorsField = nameof(row.gePriceM);
      string priceAdultsField = nameof(row.gePriceAdult);
      string priceMinorsField = nameof(row.gePriceMinor);
      string priceExtraAdultsField = nameof(row.gePriceExtraAdult);

      string gegi = row.gegi;
      //Obtenemos el Gift
      var gift = frmHost._lstGifts.Where(x => x.giID == gegi).FirstOrDefault();

      switch (pCell.Column.SortMemberPath)
      {
        case nameof(row.geQty):
          // si se ingreso el regalo, la cantidad de adultos y la cantidad de menores
          if (row.gegi != null && row.geAdults != 0 && row.geMinors != 0)
          {
            // calculamos los costos y precios
            Gifts.CalculateCostsPrices(ref row, gift, quantityField, adultsField, minorsField, extraAdultsField, costAdultsField,
                                       costMinorsField, priceAdultsField, priceMinorsField, priceExtraAdultsField, pUseCxCCost);
            calculateCharge = true;
          }
          break;
        case nameof(row.gePriceA):
        case nameof(row.gePriceM):
          // si se ingreso el regalo, la cantidad de adultos y la cantidad de menores
          if (row.gegi != null && row.geAdults == 0 && row.geMinors == 0)
            calculateCharge = true;
          break;
        case nameof(row.gegi):
          // si se ingreso el regalo
          if (gift != null)
          {
            // cargar a Marketing
            row.gect = "MARKETING";

            // si el regalo no maneja Pax
            if (!gift.giWPax)
            {
              row.geAdults = 1;
              row.geMinors = 0;
              row.geExtraAdults = 0;
            }
            // establecemos los valores default de adultos y menores
            row.geAdults = 1;
            row.geMinors = 0;
            row.geExtraAdults = 0;
            row.gePriceA = 0;
            row.gePriceM = 0;
            row.gePriceAdult = 0;
            row.gePriceMinor = 0;
            row.gePriceExtraAdult = 0;

            //establecemos si el regalo es tipo venta
            row.geSale = gift.giSale;

            // validamos la cantidad maxima del regalo
            Gifts.ValidateMaxQuantityOnEntryGift(gift, nameof(row.geQty), ref row, pIsExchange);

            // calculamos los costos y precios
            Gifts.CalculateCostsPrices(ref row, gift, quantityField, adultsField, minorsField, extraAdultsField, costAdultsField,
                                       costMinorsField, priceAdultsField, priceMinorsField, priceExtraAdultsField, pUseCxCCost);
            calculateCharge = true;
          }
          break;
        case nameof(row.geAdults):
          // calculamos los costos y precios
          Gifts.CalculateCostsPrices(ref row, gift, quantityField, adultsField, minorsField, extraAdultsField, costAdultsField,
                                     costMinorsField, priceAdultsField, priceMinorsField, priceExtraAdultsField, pUseCxCCost, EnumPriceType.Adults);
          calculateCharge = true;
          break;
        case nameof(row.geMinors):
          // calculamos los costos y precios
          Gifts.CalculateCostsPrices(ref row, gift, quantityField, adultsField, minorsField, extraAdultsField, costAdultsField,
                                     costMinorsField, priceAdultsField, priceMinorsField, priceExtraAdultsField, pUseCxCCost, EnumPriceType.Minors);
          calculateCharge = true;
          break;
        case nameof(row.geExtraAdults):
          // calculamos los costos y precios
          Gifts.CalculateCostsPrices(ref row, gift, quantityField, adultsField, minorsField, extraAdultsField, costAdultsField,
                                     costMinorsField, priceAdultsField, priceMinorsField, priceExtraAdultsField, pUseCxCCost, EnumPriceType.ExtraAdults);
          calculateCharge = true;
          break;
      }
      // calculamos el monto total de regalos
      Gifts.CalculateTotalGifts(dtg, EnumGiftsType.ReceiptGifts, quantityField, nameof(row.gegi), costMinorsField, priceMinorsField, priceAdultsField, costAdultsField, priceExtraAdultsField, pTxtTotalCost, pTxtTotalPrice, pTxtTotalToPay, saleField: "geSale");

      // si se debe calcular el cargo
      if (calculateCharge)
      {
        bool validate = false;
        CalculateCharge(pGuest != null ? pGuest.guID : 0, pChargeTo, pTxtTotalCost, pIsExchange, ref pTxtgrCxCGifts, ref pTxtTotalCxC, ref pTxtgrCxCAdj, ref validate, pLeadSourceID, ref pTxtgrMaxAuthGifts, ref pLblgrMaxAuthGifts);
      }

      GridHelper.UpdateCellsFromARow(dtg);
    }
    #endregion

    #region ValidateFolios
    /// <summary>
    /// Valida los folios de un regalo de recibos de regalos
    ///             La funcion devuelve nulo cuando los folios estan erroneos
    /// </summary>
    /// <param name="pFolios"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 29/Julio/2016 Created
    /// </history>
    public static dynamic ValidateFolios(string pFolios)
    {
      bool isRange = false, Error = false;
      string initFolio = "", endFolio = "", num1 = "", num2 = "", serie = "", folioSec = "";
      int intNum1, intNum2;
      EnumFormatFolio FormatFolios;

      // si la lista de folios solo contiene caracteres validos
      if (ValidateIntervals(pFolios))
      {
        var intervals = pFolios.Split(',');

        // recorremos los intervalos
        foreach (string currentInterval in intervals)
        {
          isRange = false;

          // Si es un rango
          if (currentInterval.Split('-').Count() > 1)
          {
            var Folios = currentInterval.Split('-');

            // si tiene mas de 2 folios en el rango
            if (Folios.Count() > 2)
            {
              UIHelper.ShowMessage("Error: Too many separators on interval. " + currentInterval, MessageBoxImage.Information);
              Error = true;
              break;
            }
            // si tiene hasta 2 folios en el rango
            else
            {
              // si no se definio el folio inicial o el folio final
              if (Folios[0] == "" || Folios[1] == "")
              {
                UIHelper.ShowMessage("Error: End or Start Interval not defined. " + Folios[0] + Folios[1], MessageBoxImage.Exclamation, "Intelligence Marketing");
                Error = true;
                break;
              }
              // si se definio el folio inicial y el folio final
              else
              {
                initFolio = Folios[0];
                endFolio = Folios[1];
                isRange = true;
              }
            }
          }

          // si es un rango bien formado
          if (isRange)
          {
            // obtenemos el formato de los folios
            FormatFolios = GetFormatFolios(initFolio, endFolio);

            // si los formatos de los folios son validos
            if (FormatFolios != EnumFormatFolio.Invalid)
            {
              // recorremos los caracteres del folio inicial
              for (int i = 0; i < initFolio.Length; i++)
              {
                bool bandEntryCase = false;

                switch (FormatFolios)
                {
                  case EnumFormatFolio.Numbers:
                    num1 = initFolio;
                    bandEntryCase = true;
                    break;
                  case EnumFormatFolio.LettersNumbers:
                    // si es un numero
                    if (CaracterType(Convert.ToChar(initFolio.Substring(i, 1))) == EnumCaracterType.Number)
                    {
                      serie = initFolio.Substring(0, i - 1);
                      num1 = initFolio.Substring(i, initFolio.Length);
                      bandEntryCase = true;
                    }
                    break;
                  case EnumFormatFolio.NumbersLetters:
                    // si es letra
                    if (CaracterType(Convert.ToChar(initFolio.Substring(i, 1))) == EnumCaracterType.Letter)
                    {
                      num1 = initFolio.Substring(0, i - 1);
                      serie = initFolio.Substring(i, initFolio.Length);
                      bandEntryCase = true;
                    }
                    break;
                }

                // verificamos si hay que salir del for
                if (bandEntryCase)
                  break;
              }

              // recorremos los caracteres del folio final
              for (int i = 0; i < endFolio.Length; i++)
              {
                // si es un numero
                if (CaracterType(Convert.ToChar(endFolio.Substring(i, 1))) == EnumCaracterType.Number)
                  num2 += endFolio.Substring(i, 1);
              }
              intNum1 = Convert.ToInt32(num1);
              intNum2 = ((Convert.ToInt32(num1)) / (10 * num2.Length)) + Convert.ToInt32(num2);

              // si el rango esta traslapado
              if (intNum1 > intNum2)
              {
                UIHelper.ShowMessage("Error: Start Folio is greater than End Folio on interval \r\n" + initFolio + "-" + endFolio, MessageBoxImage.Exclamation, "Intelligence Marketing");
                Error = true;
                break;
              }
              // si el rango no esta traslapado
              else
              {
                // armamos la lista de folios secuenciales
                for (int i = intNum1; i < intNum2; i++)
                {
                  switch (FormatFolios)
                  {
                    case EnumFormatFolio.Numbers:
                      folioSec += Convert.ToString(i);
                      break;
                    case EnumFormatFolio.LettersNumbers:
                      folioSec += serie.ToUpper() + i;
                      break;
                    case EnumFormatFolio.NumbersLetters:
                      folioSec += i + serie.ToUpper();
                      break;
                  }
                }
              }
            }
            // si los formatos de los folios no son validos 
            else
            {
              UIHelper.ShowMessage("Error: Invalid Format Folio. " + initFolio + " - " + endFolio, MessageBoxImage.Exclamation, "Intelligence Marketing");
              Error = true;
              break;
            }
          }
          // si no es un rango y tiene un folio
          else if (currentInterval != "")
          {
            // obtenemos el formato del folio
            FormatFolios = GetFormatFolios(currentInterval);

            // si el formato del folio es valido
            if (FormatFolios != EnumFormatFolio.Invalid)
              folioSec += currentInterval.ToUpper();
            // si el formato del folio no es valido
            else
            {
              UIHelper.ShowMessage("Error: Invalid Format Folio. " + currentInterval, MessageBoxImage.Exclamation);
              Error = true;
              break;
            }
          }
        }
      }
      // si la lista de folios contiene caracteres invalidos
      else
      {
        UIHelper.ShowMessage("Invalid Characters was detected.", MessageBoxImage.Exclamation, "Intelligence Marketing");
        Error = true;
      }

      // si no hubo error
      if (!Error)
      {
        List<string> foliosReturn = new List<string>();
        // armamos el arreglo de folios
        var folios = folioSec.Split(' ');
        for (int i = 0; i < folios.Length; i++)
        {
          foliosReturn.Add(folios[i]);
        }
        return foliosReturn;
      }
      else
        return null;
    }
    #endregion

    #region GetFormatFolio
    /// <summary>
    /// Obtiene los formatos de los folios inicial y final
    /// </summary>
    /// <param name="pInitFolio"></param>
    /// <param name="pEndFolio"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 01/Junio/2016 Created
    /// </history>
    public static EnumFormatFolio GetFormatFolios(string pStartFolio, string pEndFolio = "")
    {
      EnumFormatFolio formatStartFolio, formatEndFolio, formatFolios;

      formatFolios = EnumFormatFolio.Invalid;
      formatStartFolio = GetFormatFolio(pStartFolio);

      // si el folio inicial es valido y no solo tiene letras
      if (formatStartFolio != EnumFormatFolio.Invalid && formatStartFolio != EnumFormatFolio.Letters)
      {
        // si se envio un folio final
        if (pEndFolio != "")
        {
          formatEndFolio = GetFormatFolio(pEndFolio);

          // si el folio final es valido
          if (formatEndFolio != EnumFormatFolio.Invalid)
          {
            // si el segundo folio tiene el formato ### o tiene mismo formato que el folio inicial
            if (formatEndFolio == EnumFormatFolio.Numbers || formatEndFolio == formatStartFolio)
            {
              // se acepta
              formatFolios = formatEndFolio;
            }
          }
        }
        // si no se envio el folio final se regresa el formato del folio inicial que es valido
        else
        {
          formatFolios = formatStartFolio;
        }
      }

      return formatFolios;
    }
    #endregion

    #region GetFormatFolio
    /// <summary>
    /// Obtiene el formato de un folio
    /// </summary>
    /// <param name="pFolio"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 01/Julio/2016 Created
    /// </history>
    public static EnumFormatFolio GetFormatFolio(string pFolio)
    {
      EnumFormatFolio FormatFolio = EnumFormatFolio.None;

      // recorremos los caracteres del folio inicial
      for (int i = 0; i < pFolio.Length; i++)
      {
        Char current = Convert.ToChar(pFolio.Substring(i, 1));

        switch (CaracterType(current))
        {
          case EnumCaracterType.Letter:
            switch (FormatFolio)
            {
              case EnumFormatFolio.None:
                FormatFolio = EnumFormatFolio.Letters;
                break;
              case EnumFormatFolio.Numbers:
                FormatFolio = EnumFormatFolio.NumbersLetters;
                break;
              case EnumFormatFolio.LettersNumbers:
                FormatFolio = EnumFormatFolio.Invalid;
                return FormatFolio;
            }
            break;

          case EnumCaracterType.Number:
            switch (FormatFolio)
            {
              case EnumFormatFolio.None:
                FormatFolio = EnumFormatFolio.Numbers;
                break;
              case EnumFormatFolio.Letters:
                FormatFolio = EnumFormatFolio.LettersNumbers;
                break;
              case EnumFormatFolio.NumbersLetters:
                FormatFolio = EnumFormatFolio.Invalid;
                return FormatFolio;
            }
            break;
        }
      }

      return FormatFolio;
    }
    #endregion

    #region ValidateIntervals
    /// <summary>
    /// Valida que la lista de folios solo contenga caracteres validos
    /// </summary>
    /// <param name="pFolios"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 01/Julio/2016 Created
    /// </history>
    public static bool ValidateIntervals(string pFolios)
    {
      for (int i = 0; i < pFolios.Length; i++)
      {
        char value = Convert.ToChar(pFolios.Substring(i, 1));
        // Si el caracter no es valido
        if (CaracterType(value) == EnumCaracterType.None)
          return false;
      }

      return true;
    }
    #endregion

    #region CaracterType
    /// <summary>
    /// Determina si un caracter es una letra, numero, espacio en blanco, guion o coma; si no es ninguno de ellos devuelve None
    /// </summary>
    /// <param name="pChar"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 01/Julio/2016 Created
    /// </history>
    public static EnumCaracterType CaracterType(char pChar)
    {
      if (Char.IsLetter(pChar))
        return EnumCaracterType.Letter;
      else if (Char.IsNumber(pChar))
        return EnumCaracterType.Number;
      else if (Char.IsWhiteSpace(pChar))
        return EnumCaracterType.Blank;
      else if (pChar == '-')
        return EnumCaracterType.Indent;
      else if (pChar == ',')
        return EnumCaracterType.Colon;
      else
        return EnumCaracterType.None;
    }
    #endregion

    #region EnableQuantity
    /// <summary>
    /// Habilita | Deshabilita la columna cantidad
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 24/Junio/2016 Created
    /// </history>
    private static bool EnableQuantity(GiftsReceiptDetail row)
    {
      // no se permite modificar la cantidad de regalos guardados en el monedero electronico
      if (row.geInElectronicPurse == true)
      {
        UIHelper.ShowMessage("You can not modify the quantity of gifts have been given in the electronic purse.", MessageBoxImage.Exclamation, "Intelligence Marketing");
        return false;
      }
      // no se permite modificar la cantidad de regalos guardados en promociones de Sistur
      else if (row.geInPVPPromo == true)
      {
        UIHelper.ShowMessage("You can not modify the quantity of gifts have been given in Sistur promotions.", MessageBoxImage.Exclamation, "Intelligence Marketing");
        return false;
      }
      // no se permite modificar la cantidad de regalos guardados en Opera
      else if (row.geInOpera == true)
      {
        UIHelper.ShowMessage("You can not modify the quantity of packages containing gifts have been given in Opera.", MessageBoxImage.Exclamation, "Intelligence Marketing");
        return false;
      }

      return true;
    }
    #endregion

    #region EnableGift
    /// <summary>
    /// Habilita / deshabilita la columna de regalo
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 24/Junio/2016 Created
    /// </history>
    public static bool EnableGift(GiftsReceiptDetail row)
    {
      // no se permite modificar regalos guardados en promociones de Sistur
      if (row.geInPVPPromo == true)
      {
        UIHelper.ShowMessage("You can not modify gifts have been given in Sistur promotions.", MessageBoxImage.Exclamation, "Intelligence Marketing");
        return false;
      }
      // no se permite modificar regalos guardados en Opera
      else if (row.geInOpera == true)
      {
        UIHelper.ShowMessage("You can not modify gifts have been given in Opera.", MessageBoxImage.Exclamation, "Intelligence Marketing");
        return false;
      }
      // no se permite modificar regalos usados como promocion de Opera
      else if (IsUsedGuestPromotion(row))
      {
        UIHelper.ShowMessage("You can not modify gifts have been used as promotion of Opera.", MessageBoxImage.Exclamation, "Intelligence Marketing");
        return false;
      }
      return true;
    }
    #endregion

    #region IsUsedGuestPromotion
    /// <summary>
    /// Determina si un regalo ha sido usado como promocion de Opera
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 25/Junio/2016 Created
    /// </history>
    public static bool IsUsedGuestPromotion(GiftsReceiptDetail row)
    {
      // si el regalo esta guardado como promocion de Opera
      if (row.geAsPromotionOpera == true)
      {
        return BRGuestsPromotions.GetIsUsedGuestPromotion(row.gegr, row.gegi);
      }
      return false;
    }
    #endregion

    //#region LoadGuesStatusInfo
    ///// <summary>
    ///// Carga la informacion de GuestStatus para validaicon de nuevo schema de regalos
    ///// </summary>
    ///// <param name="receiptID"></param>
    ///// <param name="pGuestID"></param>
    ///// <history>
    ///// [vipacheco] 19/Abril/2016 Created
    ///// [vipacheco] 29/Junio/2016 Modified --> Migrado a esta clase  y agregado un parametro mas de referencia
    ///// </history>
    //public static void LoadGuesStatusInfo(int pReceiptID, int pGuestID, ref bool pApplyGuestStatusValidation, ref GuestStatusValidateData pGuestStatusInfo)
    //{
    //  pApplyGuestStatusValidation = false;

    //  pGuestStatusInfo = BRGuestStatus.GetGuestStatusInfo(pGuestID, pReceiptID);

    //  // Solo si esta configurado se realiza la revision
    //  if (pGuestStatusInfo != null)
    //    if (pGuestStatusInfo.gsMaxQtyTours > 0)
    //      pApplyGuestStatusValidation = true;
    //}
    //#endregion

    #region CalculateCharge
    /// <summary>
    /// Calcula el cargo de regalos segun el tipo de calculo
    /// </summary>
    /// <history>
    /// [vipacheco] 14/Mayo/2016 Created
    /// </history>
    public static void CalculateCharge(int GuestID, ChargeTo ChargeTo, TextBox txtTotalCost, bool pIsExchange,
                                       ref TextBox txtgrCxCGifts, ref TextBox txtTotalCxC, ref TextBox txtgrCxCAdj,
                                       ref bool ValidateMaxAuthGifts, string pLeadSourceID, ref TextBox txtgrMaxAuthGifts,
                                       ref TextBlock lblgrMaxAuthGifts)
    {
      decimal curCharge = 0;
      decimal curTotalCost = 0;
      decimal curMaxAuthGifts = 0;
      bool blnTour = false;

      curTotalCost = txtTotalCost.Text != "" ? Convert.ToDecimal(txtTotalCost.Text.Trim(new char[] { '$' })) : 0;

      //Establecemos el monto maximo de regalos
      SetMaxAuthGifts(GuestID, ChargeTo, ref ValidateMaxAuthGifts, pLeadSourceID, ref txtgrMaxAuthGifts, ref lblgrMaxAuthGifts);

      curMaxAuthGifts = txtgrMaxAuthGifts.Text != "" ? Convert.ToDecimal(txtgrMaxAuthGifts.Text.Trim(new char[] { '$' })) : 0;

      // Si no es un intercambio de regalos
      if (!pIsExchange)
      {
        // Localizamos a quien se carga
        switch (ChargeTo.ctCalcType)
        {
          // si el huesped tiene tour el cargo es por el total de regalos menos el monto maximo
          // autorizado. De lo contrario el cargo es por el total de regalos
          case "A":
            // Validamos si tiene tour
             LoadGuest($"{GuestID}"); 
            blnTour = _guest.guTour;
            if (blnTour)
              curCharge = CalculateChargeBasedOnMaxAuthGifts(curTotalCost, curMaxAuthGifts);
            else
              curCharge = curTotalCost;
            break;
          // El cargo es por el costo total de regalos
          case "B":
            curCharge = curTotalCost;
            break;
          // El cargo es por el costo total de regalos menos el monto maximo autorizado
          case "C":
            curCharge = CalculateChargeBasedOnMaxAuthGifts(curTotalCost, curMaxAuthGifts);
            break;
          // No generan cargo
          case "Z":
            curCharge = 0;
            break;
          default:
            break;
        }
      }
      txtgrCxCGifts.Text = string.Format("{0:C2}", curCharge);

      // Calculamos el total del cargo
      txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text != "" ? txtgrCxCGifts.Text.Trim(new char[] { '$' }) : "0") + Convert.ToDecimal(txtgrCxCAdj.Text != "" ? txtgrCxCAdj.Text.Trim(new char[] { '$' }) : "0"));
    }

    #region LoadGuest
    /// <summary>
    /// Carga el guest segun el Id
    /// </summary>
    /// <param name="guestId">Id del guest</param>
    /// <history>
    /// [jorcanche] created 06072016
    /// </history>
    private static async void LoadGuest(string guestId)
    {
      _guest = await BRGuests.GetGuest(Convert.ToInt32(guestId));
    } 
    #endregion

    #endregion

    #region SetMaxAuthGifts
    /// <summary>
    /// Establece el monto maximo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 14/Mayo/2016 Created
    /// [vipacheco] 16/Mayo/2016 Modified --> Migrado a esta clase general de ReceiptGifts
    /// </history>
    public static void SetMaxAuthGifts(int GuestID, ChargeTo ChargeTo, ref bool ValidateMaxAuthGifts, string pLeadSourceID, ref TextBox txtgrMaxAuthGifts,
                                 ref TextBlock lblgrMaxAuthGifts)
    {
      decimal curMaxAuthGifts;
      bool blnWithMaxAuthGifts = false;

      curMaxAuthGifts = CalculateMaxAuthGifts(GuestID, pLeadSourceID, ChargeTo, ref blnWithMaxAuthGifts);
      txtgrMaxAuthGifts.Text = string.Format("${0}", curMaxAuthGifts);
      lblgrMaxAuthGifts.Visibility = (blnWithMaxAuthGifts) ? Visibility.Visible : Visibility.Hidden;
      txtgrMaxAuthGifts.Visibility = (blnWithMaxAuthGifts) ? Visibility.Visible : Visibility.Hidden;
      ValidateMaxAuthGifts = blnWithMaxAuthGifts;
    }
    #endregion

    #region CalculateMaxAuthGifts
    /// <summary>
    /// Calcula el monto maximo de regalos
    /// </summary>
    /// <param name="chargeTo"></param>
    /// <param name="pLeadSourceID"></param>
    /// <param name="withMaxAuthGifts"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// [vipacheco] 16/Mayo/2016 Modified --> Migrado a esta clase general de ReceiptGifts
    /// </history>
    public static decimal CalculateMaxAuthGifts(int GuestID, string pLeadSourceID, ChargeTo ChargeTo, ref bool withMaxAuthGifts)
    {
      decimal curMaxAuthGifts = 0;
      withMaxAuthGifts = true;

      switch (ChargeTo.ctCalcType)
      {
        //Monto maximo de regalos por Lead Source
        case "A":
          // Si tiene Lead Source
          if (pLeadSourceID != "")
          {
            LeadSource _leadSource = BRLeadSources.GetLeadSourceByID(pLeadSourceID);
            //si encontro el Lead Source
            if (_leadSource != null)
            {
              curMaxAuthGifts = _leadSource.lsMaxAuthGifts;
            }
          }
          break;
        // Monto maximo de regalos por Guest Status
        case "C":
          GuestStatus _guestsStatus = BRGuestStatus.GetGuestsStatus(GuestID);
          GuestStatusType _guestStatusType = BRGuestStatusTypes.GetGuestStatusTypeByID(_guestsStatus.gtgs);
          curMaxAuthGifts = _guestsStatus.gtQuantity * _guestStatusType.gsMaxAuthGifts;
          break;
        //  Sin monto maximo de regalos
        default:
          curMaxAuthGifts = 0;
          withMaxAuthGifts = false;
          break;
      }
      return curMaxAuthGifts;
    }
    #endregion

    #region CalculateChargeBasedOnMaxAuthGifts
    /// <summary>
    /// Calcula el cargo de recibo de regalos basado en el monto maximo de regalos
    /// </summary>
    /// <param name="curTotalCost"></param>
    /// <param name="curMaxAuthGifts"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 14/Abril/2016 Created
    /// [vipacheco] 16/Mayo/2016 Modified --> Migrado a esta clase general
    /// </history>
    private static decimal CalculateChargeBasedOnMaxAuthGifts(decimal curTotalCost, decimal curMaxAuthGifts)
    {
      // Si el costo total de regalos es mayor al monto maximo autorizado
      if (curTotalCost > curMaxAuthGifts)
        return curTotalCost - curMaxAuthGifts;
      // Si el monto de regalos esta dentro del monto maximo autorizado
      else
        return 0; // Por la naturaleza del calculo el cargo es siempre 0 si el total de regalos no es mayor al autorizado
    }
    #endregion

    #region CalculateTotalCharge
    /// <summary>
    /// Calcula el total del cargo
    /// </summary>
    /// <param name="txtTotalCxC"></param>
    /// <param name="txtgrCxCGifts"></param>
    /// <history>
    /// [vipacheco] 16/Mayo/2016 Created
    /// </history>
    public static void CalculateTotalCharge(ref TextBox txtTotalCxC, ref TextBox txtgrCxCGifts, ref TextBox txtgrCxCAdj)
    {
      txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text != "" ? txtgrCxCGifts.Text : "0") + Convert.ToDecimal(txtgrCxCAdj.Text != "" ? txtgrCxCAdj.Text : "0"));
    }
    #endregion

    #region CalculateTotalGifts
    /// <summary>
    /// Calcula el total de los Gifts
    /// </summary>
    /// <param name="Grid"></param>
    /// <param name="GiftsType"></param>
    /// <param name="txtTotalCost"></param>
    /// <param name="txtTotalPrice"></param>
    /// <param name="txtTotalToPay"></param>
    /// <param name="OnlyCancellled"></param>
    /// <param name="CancelField"></param>
    /// <history>
    /// [vipacheco] 14/Mayo/2016 Created
    /// </history>
    public static void CalculateTotalGifts(DataGrid Grid, EnumGiftsType GiftsType, ref TextBox txtTotalCost, ref TextBox txtTotalPrice, ref TextBox txtTotalToPay,
                                    bool OnlyCancellled = false, string CancelField = "")
    {
      GiftsReceiptDetailCancel row = Grid.SelectedItem as GiftsReceiptDetailCancel;
      Gifts.CalculateTotalGifts(Grid, GiftsType, nameof(row.geQty), nameof(row.gegi), nameof(row.gePriceM), nameof(row.gePriceMinor), nameof(row.gePriceAdult), nameof(row.gePriceA), nameof(row.gePriceExtraAdult), txtTotalCost, txtTotalPrice, txtTotalToPay, OnlyCancellled, CancelField);
    }
    #endregion

    #region Save
    /// <summary>
    /// Guarda los regalos de recibos de regalos
    /// </summary>
    /// <param name="_LogGiftDetail"></param>
    /// <param name="_GuestID"></param>
    /// <param name="GiftReceiptID"></param>
    /// <param name="_blnInvitationGifts"></param>
    /// <history>
    /// [vipacheco] 30/Mayo/2016 Created
    /// [vipacheco] 21/Junio/2016 Migrado a esta clase
    /// </history>
    public async static Task Save(ObservableCollection<GiftsReceiptDetail> pGiftsDetail, int pReceiptID, int pGuestID, bool pIsExchange)
    {
      bool notFound = false;

      // cargamos los regalos del recibo de regalos
      List<GiftsReceiptDetail> lstResult = await BRGiftsReceiptDetail.GetGiftsReceiptDetail(pReceiptID);

      // agregamos o actualiza los regalos en la BD
      foreach (GiftsReceiptDetail current in pGiftsDetail)
      {
        // si se ingreso la cantidad, el regalo, a quien se carga y no es un regalo de un paquete
        if (current.geQty > 0 && current.gegi != null && current.gect != null)
        {
          // localizamos el regalo y a quien se carga
          GiftsReceiptDetail result = lstResult.Where(x => x.gegi == current.gegi && x.gect == current.gect).FirstOrDefault();

          // si no se encuentra el regalo
          if (result == null)
          {
            current.gegr = pReceiptID;

            // Si se ingreso los campos obligatorios.
            if (current.geQty > 0 && current.gegi != null)
            {
            // agregamos un regalo
            await BREntities.OperationEntity(current, EnumMode.Add);

            // Buscamos el regalo
            Gift gift = frmHost._lstGifts.Where(x => x.giID == current.gegi).Single();

              // Verificamos si tiene regalos del paquete
            if (gift.giPack)
            {
              // Buscamos los regalos del paquete
              var packs = frmHost._lstGiftsPacks.Where(x => x.gpPack == gift.giID).ToList();
              var giftsPacks = packs.Select(x => new GiftsReceiptPackageItem
              {
                gkgr = pReceiptID,
                gkPack = x.gpPack,
                gkgi = x.gpgi,
                gkQty = 1,
                gkAdults = 1,
                gkMinors = 0,
                gkPriceA = frmHost._lstGifts.Where(f => f.giID == x.gpgi).Select(s => s.giPrice1).Single(),
                gkPriceM = 0
              }).ToList();

              // Guardamos los regalos
              await BREntities.OperationEntities(giftsPacks, EnumMode.Add);
            }
          }
          }
          // Si se encuentra el regalo
          else
          {
            await BREntities.OperationEntity(result, EnumMode.Edit);
          }
        }
      }

      // eliminamos los regalos de la BD si fueron eliminados en el grid
      if (lstResult != null && lstResult.Count > 0)
      {
        foreach (var item in lstResult)
        {

          // localizamos el regalo en el grid
          notFound = false;
          foreach (var row in pGiftsDetail)
          {
            // si se ingreso la cantidad, el regalo, la cantidad de adultos y la cantidad de menores
            if (row.gegi != null)
            {
              // si coinciden los campos
              if (row.geQty == item.geQty && row.gegi == item.gegi && row.geAdults == item.geAdults &&
                  row.geMinors == item.geMinors && row.geExtraAdults == item.geExtraAdults && row.gect == item.gect)
              {
                notFound = true;
                break;
              }
            }
          }
          // si no se encontro el regalo en el grid se elimina de la BD
          if (!notFound)
          {
            // eliminamos los regalos del paquete
            List<GiftsReceiptPackageItem> lstPackages = await BRGiftsReceiptsPacks.GetGiftsReceiptPackage(pReceiptID, item.gegi);
            await BREntities.OperationEntities(lstPackages, EnumMode.Delete);

            // eliminamos la promocion del regalo
            if (item.geAsPromotionOpera)
              BRGuestsPromotions.DeleteGuestPromotion(item.gegr, item.gegi);

            // Eliminamos el regalo
            await BREntities.OperationEntity(item, EnumMode.Delete);
          }
        }
      }

      // si tiene invitacion y no es un intercambio de regalos
      if (pGuestID > 0 && !pIsExchange)
      {
        // actualizamos los regalos de la invitacion
        UpdateInvitsGifts(pReceiptID, pGuestID, pGiftsDetail);
      }

    }
    #endregion

    #region UpdateInvitsGifts
    /// <summary>
    /// Actualiza los regalos de la invitacion
    /// </summary>
    /// <param name="pReceiptID"></param>
    /// <param name="pGuestID"></param>
    /// <history>
    /// [vipacheco] 11/Julio/2016 Created
    /// </history>
    public async static void UpdateInvitsGifts(int pReceiptID, int pGuestID, ObservableCollection<GiftsReceiptDetail> pGiftsReceipts)
    {
      bool blnUpd = false;

      //  seleccionamos los regalos de la invitacion
      var lstResult = await BRInvitsGifts.GetInvitsGiftsByGuestID(pGuestID);

      // actualizamos los regalos en la BD
      foreach (var item in pGiftsReceipts)
      {
        // si se ingreso la cantidad, el regalo y a quien se carga
        if (item.geQty != 0 && item.gegi != null && item.gect != null)
        {
          // localizamos el regalo y a quien se carga
          InvitationGift result = lstResult.Where(x => x.iggi == item.gegi && x.igct == item.gect).FirstOrDefault();

          // si se encuentra el regalo
          if (result != null)
          {
            // actualizamos el regalo existente solo si cambio algun campo
            // si cambio el recibo
            if ((result.iggr == null && pReceiptID != 0) || (pReceiptID == 0 && result.iggr != null) || (result.iggr != pReceiptID))
            {
              result.iggr = pReceiptID;
              blnUpd = true;
            }
            // si cambio los folios
            if ((result.igFolios == null && item.geFolios != null) || (item.geFolios == null && result.igFolios != null) || (result.igFolios != item.geFolios))
            {
              result.igFolios = item.geFolios;
              blnUpd = true;
            }
            // si cambio los comentarios
            if ((result.igComments == null && item.geComments != null) || (item.geComments == null && result.igComments != null) || (result.igComments != item.geComments))
            {
              result.igComments = item.geComments;
              blnUpd = true;
            }
          }
          // si se debe actualizar el regalo
          if (blnUpd)
          {
            await BREntities.OperationEntity(result, EnumMode.Edit);
          }
        }
      }

    }
    #endregion

    #region Validate
    /// <summary>
    /// Valida los regalos de recibos de regalos
    /// </summary>
    /// <param name="pRows"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 12/Julio/2016 Created
    /// </history>
    public static bool Validate(ObservableCollection<GiftsReceiptDetail> pRows, bool pValidateMaxAuthGifts, GuestStatusValidateData pGuestStatus, 
                                string pTotalCost, string pMaxAuthGifts, DataGrid dtg)
    {
      bool blnvalid = true;
      int i = 0;
      foreach (var item in pRows)
      {
        int j = 0;
        foreach (var itemTemp in pRows)
        {
          if (i != j)
          {
            // si se ingreso un regalo y es el mismo regalo
            if (item.gegi == itemTemp.gegi && item.gegi != null)
            {
              string giftsRepeated = frmHost._lstGifts.Where(x => x.giID == item.gegi).Select(s => s.giN).First();
              UIHelper.ShowMessage("Gifts must not be repeated.\r\nGift repetead is '" + giftsRepeated + "'.", MessageBoxImage.Exclamation, "Intelligence Marketing");
              blnvalid = false;
              break;
            }
          }
          j++;
        }
        i++;
        // Si no es valido
        if (!blnvalid)
          break;
      }

      // Si se debe validar el monto maximo de regalos
      if (pValidateMaxAuthGifts)
      {
        // validamos el monto maximo de regalos
        blnvalid = Gifts.ValidateMaxAuthGifts(pTotalCost, pMaxAuthGifts);
      }

      // Si hay GuestStatus o se debe validar
      if (pGuestStatus != null && pGuestStatus.gsMaxQtyTours > 0)
      {
        // TODO: Eliminar cuando se hayan hecho las pruebas suficientes con el metodo generico.
        blnvalid = Gifts.ValidateGiftsGuestStatus(dtg, pGuestStatus, "geQty", "gegi");
      }

      return blnvalid;
    }
    #endregion

    #region ValidateGiftsGuestStatus
    /// <summary>
    /// Valida la informacion del GuestStaus x los regalos || Valida los regalos y el GuestStatus
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    /// TODO: Eliminar este metodo cuando se hayan realizado las pruebas necesarias con el metodo generico
    private static bool ValidateGiftsGuestStatus(ObservableCollection<GiftsReceiptDetail> pGridGifts, GuestStatusValidateData pGuestStatus)
    {
      int iToursUsed, iDiscsUsed, iTourAllowed, iTours, iTCont = 0, iDCont = 0, iMaxTours;
      decimal iPax, iDiscAllowed, iDisc, iAdults = 0, iMinors = 0, TotPax = 0;
      bool? blnDisc;
      string strMsg = "";

      // Asignamos los valores del GuestStatus para validar
      iMaxTours = (int)pGuestStatus.gsMaxQtyTours;
      iToursUsed = pGuestStatus.TourUsed;
      blnDisc = pGuestStatus.gsAllowTourDisc;
      iDiscsUsed = pGuestStatus.DiscUsed;
      iPax = pGuestStatus.guPax;

      // Calculamos el total Pax
      CalculateAdultsMinorsByPax(iPax, ref iAdults, ref iMinors);
      TotPax = iAdults + iMinors;

      // Los Tours permitidos
      iTourAllowed = iMaxTours - iToursUsed;
      iTours = iTourAllowed;

      // Validamos con cada registro de tour
      foreach (GiftsReceiptDetail _item in pGridGifts)
      {
        Gift _giftResult = frmHost._lstGifts.Where(x => x.giID == _item.gegi).SingleOrDefault();

        if (_giftResult != null)
        {
          // Evaluamos si son de toures y con descuento
          if (_giftResult.gigc == "TOURS" && !(bool)_giftResult.giDiscount)
          {
            iTours += iTours - (_giftResult.giQty * _item.geQty);
            iTCont += iTCont + (_giftResult.giQty * _item.geQty);
          }
        }
      }

      // Los descuentos permitidos son los restantes de los PAX restantes
      iDiscAllowed = TotPax - iTCont;
      iDiscAllowed = iDiscAllowed - iDiscsUsed;
      iDisc = iDiscAllowed;

      // Validamos con cada registro de descuentos
      foreach (GiftsReceiptDetail _item in pGridGifts)
      {
        Gift _giftResult = frmHost._lstGifts.Where(x => x.giID == _item.gegi).SingleOrDefault(); //  BRGifts.GetGiftId(_item.gegi);

        if (_giftResult != null)
        {
          if (_giftResult.gigc == "TOURS" && (bool)_giftResult.giDiscount)
          {
            iDisc = iDisc - (_giftResult.giQty * _item.geQty);
            iDCont = iDCont + (_giftResult.giQty * _item.geQty);
          }
        }
      }

      //Revisamos el remanente de la revision de Gifts
      if (iTours < 0)
        strMsg = "The maximum number of tours " + iTourAllowed + " has been exceeded. \r\n There are " + iTCont + " tours on this receipt";

      if (iDisc < 0 && strMsg == "")
        strMsg = "The maximum number of discount tours " + iDiscAllowed + " has been exceeded.\r\n There are " + iDCont + " discount tours on this receipt";


      //Revisamos el remanente de la revision de Gifts
      if (strMsg != "")
      {
        UIHelper.ShowMessage(strMsg, MessageBoxImage.Exclamation);
        return false;
      }
      else
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

    #region printReceiptGift
    /// <summary>
    /// Muestra en pantalla el Reporte del Recibo de regalo.
    /// </summary>
    /// <param name="receipID"></param>
    /// <param name="isCharge"></param>
    /// <history>
    /// [edgrodriguez] 12/Jul/2016 Created
    /// </history>
    public static async void printReceiptGift(int receipID, bool isCharge = false)
    {
      var receipt = await BRGiftsReceipts.GetRptGiftsReceipt(receipID, isCharge);
      var giftRcpt = (receipt[0] as List<RptGiftsReceipt>).Select(c => new objRptGiftsReceiptIM(c)).FirstOrDefault();
      var giftRcpt_Gifts = (receipt.Any() && receipt.Count > 1) ? (receipt[1] as List<RptGiftsReceipt_Gifts>).Select(c => new objRptGiftsReceipt_GiftsIM(c)).ToList() : null;
      var giftRcpt_ProdLegends = (receipt.Any() && receipt.Count > 1) ? (receipt[2] as List<RptGiftsReceipt_ProductLegends>).Select(c => new objRptGiftsReceipt_ProductLegendsIM(c)).ToList() : null;

      var rptGiftReceipt = new Reports.rptGiftsReceipt();

      rptGiftReceipt.Database.Tables[0].SetDataSource(IM.Model.Helpers.ObjectHelper.ObjectToList(giftRcpt));

      if (!isCharge)
      {
        rptGiftReceipt.Subreports[0].SetDataSource(giftRcpt_Gifts);
        rptGiftReceipt.Subreports[1].SetDataSource(giftRcpt_ProdLegends);
      }

      //Cambiamos el lenguaje de las etiquetas.
      CrystalReportHelper.SetLanguage(rptGiftReceipt, giftRcpt.gula);

      //Oculatamos la seccion de Charge
      rptGiftReceipt.SetParameterValue("isCharge", isCharge);
      //Cambiamos el titulo del reporte.Segun lenguaje o si es un cargo.
      rptGiftReceipt.SetParameterValue("msgLblCoupon", ((isCharge) ? "Chargeback Voucher" : LanguageHelper.GetMessage(EnumMessage.msgLblCoupon)));

      //Si es reimpresion reemplazamos los campos clave.
      if (giftRcpt.grReimpresion > 1)
      {
        var msgReimpresion = LanguageHelper.GetMessage(EnumMessage.msglblReimpresion);
        msgReimpresion = (string.IsNullOrEmpty(msgReimpresion)) ? "" : msgReimpresion.Replace("[grReimpresion]", giftRcpt.grReimpresion.ToString()).Replace("[rmN]", giftRcpt.rmN?.ToString() ?? "");
        (rptGiftReceipt.ReportDefinition.ReportObjects["msglblReimpresion"] as TextObject).Text = msgReimpresion;
      }

      CrystalReportHelper.ShowReport(rptGiftReceipt, $"{((isCharge) ? "Chargeback Voucher" : "Gifts Receipt")} {giftRcpt.grID}", PrintDevice: EnumPrintDevice.pdPrinter);
    }
    #endregion

  }
}

using IM.Base.Helpers;
using System.Windows.Controls;
using IM.Model.Enums;
using System;
using System.Windows;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Helpers;

namespace IM.Base.Classes
{
  public class InvitationValidationRules
  {
    #region Grid Invitation Gift

    #region StartEdit
    /// <summary>
    /// Inicia las validaciones de los campos del Grid
    /// </summary>
    /// <param name="invitationGift">Objeto enlazado al Row que se esta modificando</param>
    /// <param name="currentCellInfo">Celda que se esta editando</param>
    /// <param name="dtg">El datagrid que se esta modificando</param>
    /// <param name="_hasError">True tiene error | False No tiene</param>
    public static void StartEdit(ref InvitationGift invitationGift, ref DataGridCellInfo currentCellInfo, DataGrid dtg, ref bool _hasError)
    {
      //Index del Row en edicion
      int rowIndex = dtg.SelectedIndex != -1 ? dtg.SelectedIndex : 0;

      if (invitationGift.iggr == null || invitationGift.iggr == 0)
      {
        switch (currentCellInfo.Column.SortMemberPath)
        {
          case "iggi":
            //Si no ha ingresado una cantidad
            if (invitationGift.igQty == 0)
            {
              UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
              _hasError = true;
              //Asignamos la cantidad minima 
              invitationGift.igQty = 1;
              //Mandamos el foco a la columna igQty index[0].
              GridHelper.SelectRow(dtg, rowIndex, 0, true);
            }
            break;
          case "igAdults":
          case "igMinors":
          case "igExtraAdults":
            //Si no se ha seleccionado un regalo
            if (string.IsNullOrEmpty(invitationGift.iggi))
            {
              UIHelper.ShowMessage("Enter the gift first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
              _hasError = true;
              //Mandamos el foco a la columna iggi index[1].
              GridHelper.SelectRow(dtg, rowIndex, 1, true);
            }
            else
            {
              //Obtenemos el Gift Completo
              var gift = BRGifts.GetGiftId(invitationGift.iggi);
              // se permite modificar si el regalo maneja Pax
              currentCellInfo.Column.IsReadOnly = !gift.giWPax;
            }
            break;
          default:
            break;
        }

      }
      //Si el regalo ya fue entregado (No necesita validar las columnas)
      else
      {
        _hasError = false;
      }
    }
    #endregion

    #region ValidateEdit
    /// <summary>
    /// Valida la informacion del Grid
    /// </summary>
    /// <param name="invitationGift">Objeto enlazada a la fila que se esta editando</param>
    /// <param name="currentCellInfo">Celda que se esta editando</param>
    /// <returns>
    /// True  si tiene error| False si NO tiene
    /// </returns>
    /// <history>
    /// [erosado] 27/07/2016  Created.
    /// </history>
    internal static bool ValidateEdit(ref InvitationGift invitationGift, ref DataGridCellInfo currentCellInfo)
    {
      bool _hasError = false;
      switch (currentCellInfo.Column.SortMemberPath)
      {
        case "igQty":
          //Si tiene seleccionado un gift
          if (invitationGift.iggi != null)
          {
            //Buscamos el Gift
            var gift = BRGifts.GetGiftId(invitationGift.iggi);
            //Validacion cantidad máxima del regalo
            Gifts.ValidateMaxQuantityOnEntryQuantity(ref invitationGift, gift, false, 1, ref _hasError, nameof(invitationGift.igQty));
          }//Si no ha seleccionado el Gift
          else
          {
            //Validacion cantidad máxima del regalo
            Gifts.ValidateMaxQuantityOnEntryQuantity(ref invitationGift, null, false, 1, ref _hasError, nameof(invitationGift.igQty));
          }
          return _hasError;
        case "igAdults":
          //Validacion Numero de Adultos
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.Adults, invitationGift, ref _hasError, nameof(invitationGift.igAdults), nameof(invitationGift.igMinors));
          return _hasError;
        case "igMinors":
          //Validacion Numero de menores
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.Minors, invitationGift, ref _hasError, nameof(invitationGift.igAdults), nameof(invitationGift.igMinors));
          return _hasError;
        default:
          break;
      }
      return _hasError;
    }
    #endregion

    #region AfterEdit
    /// <summary>
    /// Valida la informacion de la celda.
    /// </summary>
    /// <param name="dtg">DataGrid que estamos validando</param>
    /// <param name="invitationGift">Objeto enlazado a la fila que estamos validando</param>
    /// <param name="currentCell">Celda que estamos validando</param>
    /// <param name="txtTotalCost">Caja de texto donde se pondrá el resultado del calculo de Costos</param>
    /// <param name="txtTotalPrice">Caja de texto donde se pondrá el resultado del calculo de Precios</param>
    /// <param name="txtgrMaxAuthGifts">Caja de texto donde se pondrá el resultado del calculo de costos</param>
    internal static void AfterEdit(DataGrid dtg, ref InvitationGift invitationGift, DataGridCellInfo currentCell,
      ref TextBox txtTotalCost, ref TextBox txtTotalPrice, ref TextBox txtgrMaxAuthGifts, GuestStatusType guestStatusType, string program = "")
    {
      bool _passValidate = false;
      //Obtenemos el Gift
      var gift = BRGifts.GetGiftId(invitationGift.iggi);

      switch (currentCell.Column.SortMemberPath)
      {
        case "igQty":
          //Si son diferentes de NULL
          if (invitationGift?.igQty >= 0 && invitationGift?.igAdults >= 0 && invitationGift?.igMinors >= 0 &&
             invitationGift?.igExtraAdults >= 0)
          {
            //Calcula costos y precios
            Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
              nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
              nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
              nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
              nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult));
          }
          break;
        case "iggi":
          //Si se selecciono el Gift
          if (!string.IsNullOrEmpty(invitationGift.iggi))
          {
            // Cargar a Marketing
            invitationGift.igct = "MARKETING";

            //Si el regalo no maneja Pax
            if (!gift.giWPax)
            {
              //Agrega valores default
              invitationGift.igAdults = 1;
              invitationGift.igMinors = 0;
              invitationGift.igExtraAdults = 0;
            }
            //Establecemos valores default de adultos y menores
            invitationGift.igAdults = 1;
            invitationGift.igMinors = 0;
            invitationGift.igExtraAdults = 0;
            invitationGift.igPriceA = 0;
            invitationGift.igPriceM = 0;
            invitationGift.igPriceAdult = 0;
            invitationGift.igPriceMinor = 0;
            invitationGift.igPriceExtraAdult = 0;

            //Validamos la cantidad maxima que podemos regalar de el regalo en específico.
            Gifts.ValidateMaxQuantity(gift, invitationGift.igQty, false, ref invitationGift, nameof(invitationGift.igQty));

            //Si es OutHouse y ya selecciono guestStatusType
            if (program == EnumToListHelper.GetEnumDescription(EnumProgram.Outhouse) && guestStatusType != null)
            {
              //Valida que no den mas de los tours permitidos && Validamos que no den mas de los descuentos de Tour 
              _passValidate = Gifts.ValidateMaxQuantityGiftTour(dtg, guestStatusType, nameof(invitationGift.igQty), nameof(invitationGift.iggi));

            }

            //Calculamos los costos y los precios 
            Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
                nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
                nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
                nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
                nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult), false);
          }
          break;
        case "igAdults":
          // calculamos los costos y precios
          Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
              nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
              nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
              nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
              nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult), false, EnumPriceType.Adults);
          break;
        case "igMinors":
          // calculamos los costos y precios
          Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
              nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
              nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
              nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
              nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult), false, EnumPriceType.Minors);
          break;
        case "igExtraAdults":
          // calculamos los costos y precios
          Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
              nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
              nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
              nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
              nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult), false, EnumPriceType.ExtraAdults);
          break;
        default:
          break;
      }
      //Calculamos el monto total de los regalos
      Gifts.CalculateTotalGifts(dtg, EnumGiftsType.InvitsGifts, nameof(invitationGift.igQty), nameof(invitationGift.iggi),
        nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceAdult),
        nameof(invitationGift.igPriceA), nameof(invitationGift.igPriceExtraAdult), txtTotalCost, txtTotalPrice);

      //Refresca los datos en las celdas del Grid
      GridHelper.UpdateCellsFromARow(dtg);
    }

    #endregion

    #endregion
  }
}

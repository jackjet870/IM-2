using IM.Base.Helpers;
using System.Windows.Controls;
using IM.Model.Enums;
using System;
using System.Windows;
using IM.Model;
using IM.BusinessRules.BR;

namespace IM.Base.Classes
{
  public class InvitationValidationRules
  {
    #region Grid Invitation Gift

    #region StartEdit
    /// <summary>
    /// Inicia las validaciones de los campos del Grid
    /// </summary>
    /// <param name="_invitMode"></param>
    /// <param name="invitationGift"></param>
    /// <param name="_IGCurrentCell"></param>
    /// <param name="dataGrid"></param>
    /// <param name="_hasError"></param>
    public static void StartEdit(EnumInvitationMode _invitMode, InvitationGift invitationGift,
      DataGridCellInfo _IGCurrentCell, DataGrid dataGrid, ref bool _hasError)
    {
      //Index del Row en edicion
      int rowIndex = dataGrid.SelectedIndex != -1 ? dataGrid.SelectedIndex : 0;

      if (invitationGift.iggr == null || invitationGift.iggr == 0)
      {
        switch (dataGrid.CurrentCell.Column.SortMemberPath)
        {
          case "iggi":
            //Si no ha ingresado una cantidad
            if (invitationGift.igQty == 0)
            {
              UIHelper.ShowMessage("Enter the quantity first.", System.Windows.MessageBoxImage.Exclamation, "Intelligence Marketing");
              _hasError = true;
              GridHelper.SelectRow(dataGrid, rowIndex, 0);
              //dataGrid.CurrentCell = new DataGridCellInfo(invitationGift, dataGrid.Columns[0]);
            }
            break;
          case "igAdults":
          case "igMinors":
          case "igExtraAdults":
            //Si no se ha seleccionado un regalo
            if (string.IsNullOrEmpty(invitationGift.iggi))
            {
              UIHelper.ShowMessage("Enter the gift first.", System.Windows.MessageBoxImage.Exclamation, "Intelligence Marketing");
              _hasError = true;
              //GridHelper.SelectRow(dataGrid, rowIndex, 1, true);
              dataGrid.CurrentCell = new DataGridCellInfo(invitationGift, dataGrid.Columns[0]);
            }
            else
            {
              //Obtenemos el Gift Completo
              var gift = BRGifts.GetGiftId(invitationGift.iggi);
              // se permite modificar si el regalo maneja Pax
              _IGCurrentCell.Column.IsReadOnly = gift.giWPax;
            }
            break;
          default:
            break;
        }

      }
      //Si el regalo ya fue entregado
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
    /// <param name="invitationGiftCustom">objeto de la fila que se esta editando</param>
    /// <param name="hasError">True Falló | False pasó la validacion</param>
    /// <param name="IGCurrentCell">Informacion de la celda que se esta validando</param>
    /// <history>
    /// [erosado] 27/07/2016  Created.
    /// </history>
    internal static void ValidateEdit(InvitationGift invitationGiftCustom, bool hasError, DataGridCellInfo IGCurrentCell)
    {
      switch (IGCurrentCell.Column.SortMemberPath)
      {
        case "igQty":
          if (invitationGiftCustom.iggi != null)//Si tiene seleccionado un gift
          {
            //Buscamos el Gift
            var gift = BRGifts.GetGiftId(invitationGiftCustom.iggi);
            //Validacion cantidad máxima del regalo
            Gifts.ValidateMaxQuantityOnEntryQuantity(invitationGiftCustom, gift, false, 1, hasError, nameof(invitationGiftCustom.igQty));
          }//Si no ha seleccionado el Gift
          else
          {
            //Validacion cantidad máxima del regalo
            Gifts.ValidateMaxQuantityOnEntryQuantity(invitationGiftCustom, null, false, 1, hasError, nameof(invitationGiftCustom.igQty));
          }
          break;
        case "igAdults":
          //Validacion Numero de Adultos
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.Adults, invitationGiftCustom, hasError, nameof(invitationGiftCustom.igAdults), nameof(invitationGiftCustom.igMinors));
          break;
        case "igMinors":
          //Validacion Numero de menores
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.Minors, invitationGiftCustom, hasError, nameof(invitationGiftCustom.igAdults), nameof(invitationGiftCustom.igMinors));
          break;
        default:
          break;
      }
    }
    #endregion

    #region AfterEdit
    internal static void AfterEdit(DataGrid dtg, InvitationGift invitationGift, DataGridCellInfo _IGCurrentCell,
      TextBox txtTotalCost, TextBox txtTotalPrice, TextBox txtgrMaxAuthGifts)
    {
      var gift = BRGifts.GetGiftId(invitationGift.iggi);
      switch (_IGCurrentCell.Column.SortMemberPath)
      {
        case "igQty":
          if (!string.IsNullOrEmpty(invitationGift.igQty.ToString()) && !string.IsNullOrEmpty(invitationGift.igAdults.ToString())
            && !string.IsNullOrEmpty(invitationGift.igMinors.ToString()) && !string.IsNullOrEmpty(invitationGift.igExtraAdults.ToString()))
          {
            Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
              nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
              nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
              nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
              nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult));
          }
          break;
        case "iggi":
          if (!string.IsNullOrEmpty(invitationGift.iggi))
          {
            // Cargar a Marketing
            invitationGift.igct = "MARKETING";

            //Si el regalo no maneja Pax
            if (!gift.giWPax)
            {
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

            //Validamos la cantidad maxima del regalo
            Gifts.ValidateMaxQuantityOnEntryGift(gift, nameof(invitationGift.igQty), ref invitationGift, false);

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

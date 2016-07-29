using IM.Base.Helpers;
using System.Windows.Controls;
using IM.Model.Enums;
using System;
using System.Windows;
using IM.Model;

namespace IM.Base.Classes
{
  public class InvitationValidationRules
  {


    //public static void StartEdit(Enums.EnumMode mode, GiftsReceiptDetail row, ref DataGridCellInfo cell, ref DataGrid grid)
    //{
    //  //bool validationResult=false;

    //  //switch (e.Column.SortMemberPath)
    //  //{
    //  //  case "IggiCustom":
    //  //    {
    //  //      if (string.IsNullOrEmpty(item.igQty.ToString()))
    //  //      {
    //  //        UIHelper.ShowMessage("Please enter the quantity first", title: "Invitation");
    //  //        item.IgQtyCustom = 1;
    //  //        return;
    //  //      }
    //  //      break;
    //  //    }
    //  //  case "IgAdultsCustom":
    //  //    {
    //  //      if (item.igMinors == 0 && item.igAdults == 0)
    //  //      {
    //  //        UIHelper.ShowMessage("Quantity of adult and quantity of minors can't be both zero", title: "Invitation");
    //  //        item.IgAdultsCustom = 1;
    //  //      }
    //  //      break;
    //  //    }
    //  //  case "IgMinorsCustom":
    //  //    {
    //  //      if (item.igAdults == 0 && item.igMinors == 0)
    //  //      {
    //  //        UIHelper.ShowMessage("Quantity of adult and quantity of minors can't be both zero", title: "Invitation");
    //  //        item.IgMinorsCustom = 1;
    //  //      }
    //  //      break;
    //  //    }
    //  //  default:
    //  //    {
    //  //      break;
    //  //    }
    //  //}


    //  //return validationResult;
    //  return false;
    //}
    internal static void StartEdit(EnumMode mode, InvitationGiftCustom row, ref DataGridCellInfo cell, ref DataGrid grid, ref bool cancell)
    {
      ////Obtenemos el Index del Row
      //int rowIndex = grid.SelectedIndex != -1 ? grid.SelectedIndex : 0;

      //switch (mode)
      //{
      //  case EnumMode.edit:

      //    switch (cell.Column.SortMemberPath)
      //    {
      //      // Cantidad
      //      case "geQty":
      //        bool blnResult = !EnableQuantity(row);
      //        cell.Column.IsReadOnly = blnResult;
      //        cancell = blnResult;
      //        break;
      //      // Regalo
      //      case "gegi":
      //        // si no se ha ingresado la cantidad
      //        if (row.igQty == 0)
      //        {
      //          UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
      //          cancell = true;

      //          GridHelper.SelectRow(grid, rowIndex, 0, true);
      //        }
      //        // determinamos si se puede modificar el regalo
      //        else if (!EnableGift(row))
      //        {
      //          cancell = false;
      //          cell.Column.IsReadOnly = true;
      //        }
      //        else
      //        {
      //          cancell = false;
      //          cell.Column.IsReadOnly = false;
      //        }

      //        // Habilitamos las columnas
      //        DataGridCellInfo Adults = new DataGridCellInfo(grid.SelectedIndex, grid.Columns[3]);
      //        Adults.Column.IsReadOnly = false;
      //        DataGridCellInfo Minors = new DataGridCellInfo(grid.SelectedIndex, grid.Columns[4]);
      //        Minors.Column.IsReadOnly = false;
      //        DataGridCellInfo EAdults = new DataGridCellInfo(grid.SelectedIndex, grid.Columns[5]);
      //        EAdults.Column.IsReadOnly = false;

      //        break;
      //      // Numero de adultos, menores y adultos extra
      //      case "geAdults":
      //      case "geMinors":
      //      case "geExtraAdults":
      //        // si no se ha ingresado la cantidad
      //        if (row.igQty == 0)
      //        {
      //          UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
      //          cancell = true;
      //          GridHelper.SelectRow(grid, rowIndex, 0, true);
      //        }
      //        // si no se ha ingresado el regalo
      //        else if (row.iggi == null)
      //        {
      //          UIHelper.ShowMessage("Enter the Gift first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
      //          cancell = true;
      //          GridHelper.SelectRow(grid, rowIndex, 2, true);
      //        }
      //        else
      //        {
      //          // localizamos el regalo
      //          Gift _gift = frmHost._lstGifts.Where(x => x.giID == row.gegi).First();

      //          // se permite modificar si el regalo maneja Pax
      //          cell.Column.IsReadOnly = !_gift.giWPax;
      //          cancell = !_gift.giWPax;
      //        }
      //        break;
      //        break;
      //      default:
      //        break;
      //    }

      //}
    }

    #region EnableGift
    /// <summary>
    /// Habilita / deshabilita la columna de regalo
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 24/Junio/2016 Created
    /// </history>
    public static void EnableGift(InvitationGiftCustom row)
    {
      //// no se permite modificar regalos guardados en promociones de Sistur
      //if (row.igInPVPPromo == true)
      //{
      //  UIHelper.ShowMessage("You can not modify gifts have been given in Sistur promotions.", MessageBoxImage.Exclamation, "Intelligence Marketing");
      //  return false;
      //}
      //// no se permite modificar regalos guardados en Opera
      //else if (row.geInOpera == true)
      //{
      //  UIHelper.ShowMessage("You can not modify gifts have been given in Opera.", MessageBoxImage.Exclamation, "Intelligence Marketing");
      //  return false;
      //}
      //// no se permite modificar regalos usados como promocion de Opera
      //else if (IsUsedGuestPromotion(row))
      //{
      //  UIHelper.ShowMessage("You can not modify gifts have been used as promotion of Opera.", MessageBoxImage.Exclamation, "Intelligence Marketing");
      //  return false;
      //}
      //return true;
    }
    #endregion
  }
}

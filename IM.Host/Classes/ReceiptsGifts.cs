using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
    static Guest _guest = new Guest();

    #region StartEdit
    /// <summary>
    /// Determina si se puede editar la informacion del grid
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="row"></param>
    /// <param name="cell"></param>
    /// <param name="grid"></param>
    /// <history>
    /// [vipacheco] 24/junio/2016 Created
    /// </history>
    public static void StartEdit(Enums.EnumMode mode, GiftsReceiptDetail row, ref DataGridCellInfo cell, ref DataGrid grid)
    {
      // Obtenemos el index del row en edicion
      int rowIndex = grid.SelectedIndex != -1 ? grid.SelectedIndex : 0;

      switch (mode)
      {
        // Edicion total
        case Enums.EnumMode.modEdit:
          switch (cell.Column.SortMemberPath)
          {
            // Cantidad
            case "geQty":
              cell.Column.IsReadOnly = !EnableQuantity(row);
              break;
            // Regalo
            case "gegi":
              // si no se ha ingresado la cantidad
              if (row.geQty == 0)
              {
                UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                GridHelper.SelectRow(grid, rowIndex, 0, true);
              }
              // determinamos si se puede modificar el regalo
              else if (!EnableGift(row))
              {
                cell.Column.IsReadOnly = true;
              }
              else
              {
                cell.Column.IsReadOnly = false;
              }
              break;
            // Numero de adultos, menores y adultos extra
            case "geAdults":
            case "geMinors":
            case "geExtraAdults":
              // si no se ha ingresado la cantidad
              if (row.geQty == 0)
              {
                UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                GridHelper.SelectRow(grid, rowIndex, 0, true);
              }
              // si no se ha ingresado el regalo
              else if (row.gegi == null)
              {
                UIHelper.ShowMessage("Enter the Gift first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                GridHelper.SelectRow(grid, rowIndex, 2, true);
              }
              else
              {
                // localizamos el regalo
                Gift _gift = frmHost._lstGifts.Where(x => x.giID == row.gegi).First();

                // se permite modificar si el regalo maneja Pax
                cell.Column.IsReadOnly = _gift.giWPax;
              }
              break;
            // Folios
            case "geFolios":
              // si no se ha ingresado la cantidad
              if (row.geQty == 0)
              {
                UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                GridHelper.SelectRow(grid, rowIndex, 0, true);
              }
              // si no se ha ingresado el regalo
              else if (row.gegi == null)
              {
                UIHelper.ShowMessage("Enter the Gift first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                GridHelper.SelectRow(grid, rowIndex, 2, true);
              }
              else
              {
                // localizamos el regalo
                Gift _gift = frmHost._lstGifts.Where(x => x.giID == row.gegi).First();

                // se permite modificar si el regalo maneja Pax
                cell.Column.IsReadOnly = !_gift.giWFolio;
              }
              break;
            case "gect":
            case "geCxC":
            case "geCharge":
              // si no se ha ingresado el regalo, la cantidad de adultos y la cantidad de menores
              if (row.gegi == null && row.geMinors == 0 && row.geAdults == 0)
              {
                UIHelper.ShowMessage("No gift and quantity has been selected.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                GridHelper.SelectRow(grid, rowIndex, 0, true);
              }
              else
              {
                cell.Column.IsReadOnly = false;
              }
              break;
            // Costo de adultos y menores
            case "gePriceA":
            case "gePriceM":
              // localizamos el regalo
              Gift _giftA = frmHost._lstGifts.Where(x => x.giID == row.gegi).First();

              // se permite modificar si el regalo permite modificar los montos
              cell.Column.IsReadOnly = !_giftA.giAmountModifiable;
              break;
            // Regalos tipo venta y comentarios
            case "geSale":
            case "geComments":
              // si no se ha ingresado la cantidad
              if (row.geQty == 0)
              {
                UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                GridHelper.SelectRow(grid, rowIndex, 0, true);
              }
              else
              {
                cell.Column.IsReadOnly = false;
              }
              break;
            // las demas columnas no se permiten modificar
            default:
              cell.Column.IsReadOnly = true;
              break;
          }
          break;

        case Enums.EnumMode.modEditPartial:
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
                  GridHelper.SelectRow(grid, rowIndex, 0, true);
                }
                // si no se ha ingresado el regalo
                else if (row.gegi == null)
                {
                  UIHelper.ShowMessage("Enter the Gift first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                  GridHelper.SelectRow(grid, rowIndex, 2, true);
                }
                else
                {
                  // localizamos el regalo
                  Gift _gift = frmHost._lstGifts.Where(x => x.giID == row.gegi).First();

                  // se permite modificar si el regalo maneja folios
                  cell.Column.IsReadOnly = !_gift.giWFolio;
                }
                break;
              case "geComments":
                // si no se ha ingresado la cantidad
                if (row.geQty == 0)
                {
                  UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                  GridHelper.SelectRow(grid, rowIndex, 0, true);
                }
                // si no se ha ingresado el regalo
                else if (row.gegi == null)
                {
                  UIHelper.ShowMessage("Enter the Gift first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
                  GridHelper.SelectRow(grid, rowIndex, 2, true);
                }
                else
                {
                  cell.Column.IsReadOnly = false;
                }
                break;
              default:
                cell.Column.IsReadOnly = true;
                break;
            }
          }
          else
          {
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
    /// <param name="pRow"></param>
    /// <param name="pCancel"></param>
    /// <param name="pIsExchange"></param>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created
    /// </history>
    public async static void ValidateEdit(GiftsReceiptDetail pRow, bool pCancel, bool pIsExchange, DataGridCellInfo cell)
    {
      int LowerBound = 0;

      // si es un intercambio de regalos
      if (pIsExchange)
        LowerBound = -999;
      else
        LowerBound = 1;

      switch (cell.Column.SortMemberPath)
      {
        case "geAdults":
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.Adults, pRow, ref pCancel);
          break;
        case "geMinors":
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.Minors, pRow, ref pCancel);
          break;
        case "geExtraAdults":
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.ExtraAdults, pRow, ref pCancel);
          break;
        case "geQty":
          // validamos la cantidad maxima del regalo
          Gifts.ValidateMaxQuantityOnEntryQuantity(pRow, pIsExchange, LowerBound, ref pCancel);

          //si el regalo esta guardado como promocion de Opera
          if (pRow.geAsPromotionOpera == true)
          {
            // ejecutamos el procedimiento almacenado
            GuestPromotion guest = await BRGuestsPromotions.GetGuestPromotion(pRow.gegr, pRow.gegi);
            int quantity = guest.gpQty;

            // validamos que no se le de una cantidad inferior a la que se le habia dado antes
            if (quantity < pRow.geQty)
            {
              UIHelper.ShowMessage("The quantity can not be lower than the previous (" + quantity + ").", MessageBoxImage.Information);
              pCancel = true;
            }
          }
          break;
        case "geCxC":
          pCancel = false;
          break;
        case "geFolios":
          if (pRow.geFolios != "")
          {
            // validamos los folios

          }
          break;

      }
    }
    #endregion

    public static dynamic ValidateFolios(string pFolios)
    {
      bool isRange = false, Error = false;
      string initFolio = "", endFolio = "";

      // si la lista de folios solo contiene caracteres validos
      if (ValidateIntervals(pFolios))
      {
        var intervals = pFolios.Split(',');

        // recorremos los intervalos
        foreach (string currentInterval in intervals)
        {
          isRange = false;

          // Si es un rango
          if (currentInterval.Split('-').Count() > 0)
          {
            var Folios = currentInterval.Split('-');

            // si tiene mas de 2 folios en el rango
            if (Folios.Count() > 1)
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
          }
        }
      }

      return true;
    }

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

    #region LoadGuesStatusInfo
    /// <summary>
    /// Carga la informacion de GuestStatus para validaicon de nuevo schema de regalos
    /// </summary>
    /// <param name="receiptID"></param>
    /// <param name="pGuestID"></param>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// [vipacheco] 29/Junio/2016 Modified --> Migrado a esta clase  y agregado un parametro mas de referencia
    /// </history>
    public static void LoadGuesStatusInfo(int pReceiptID, int pGuestID, ref bool pApplyGuestStatusValidation, ref GuestStatusValidateData pGuestStatusInfo)
    {
      pApplyGuestStatusValidation = false;

      pGuestStatusInfo = BRGuestStatus.GetStatusValidateInfo(pGuestID, pReceiptID);

      // Solo si esta configurado se realiza la revision
      if (pGuestStatusInfo != null)
        if (pGuestStatusInfo.gsMaxQtyTours > 0)
          pApplyGuestStatusValidation = true;
    }
    #endregion

    #region CalculateCharge
    /// <summary>
    /// Calcula el cargo de regalos segun el tipo de calculo
    /// </summary>
    /// <history>
    /// [vipacheco] 14/Mayo/2016 Created
    /// </history>
    public static void CalculateCharge(int GuestID, ChargeTo ChargeTo, TextBox txtTotalCost, CheckBox chkgrExchange,
                                       TextBox txtgrgu, ref TextBox txtgrCxCGifts, ref TextBox txtTotalCxC, ref TextBox txtgrCxCAdj,
                                       ref bool ValidateMaxAuthGifts, ref TextBox txtgrls, ref TextBox txtgrMaxAuthGifts,
                                       ref TextBlock lblgrMaxAuthGifts)
    {
      decimal curCharge = 0;
      decimal curTotalCost = 0;
      decimal curMaxAuthGifts = 0;
      bool blnTour = false;

      //ChargeTo _ChargeTo = (ChargeTo)ChargeTo.SelectedItem;

      curTotalCost = txtTotalCost.Text != "" ? Convert.ToDecimal(txtTotalCost.Text.Trim(new char[] { '$' })) : 0;

      //Establecemos el monto maximo de regalos
      SetMaxAuthGifts(GuestID, ChargeTo, ref ValidateMaxAuthGifts, ref txtgrls, ref txtgrMaxAuthGifts, ref lblgrMaxAuthGifts);

      curMaxAuthGifts = txtgrMaxAuthGifts.Text != "" ? Convert.ToDecimal(txtgrMaxAuthGifts.Text.Trim(new char[] { '$' })) : 0;

      // Si no es un intercambio de regalos
      if (!chkgrExchange.IsChecked.Value)
      {
        // Localizamos a quien se carga
        switch (ChargeTo.ctCalcType)
        {
          // si el huesped tiene tour el cargo es por el total de regalos menos el monto maximo
          // autorizado. De lo contrario el cargo es por el total de regalos
          case "A":
            // Validamos si tiene tour
             LoadGuest(txtgrgu.Text); 
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
    public static void SetMaxAuthGifts(int GuestID, ChargeTo ChargeTo, ref bool ValidateMaxAuthGifts, ref TextBox txtgrls, ref TextBox txtgrMaxAuthGifts,
                                 ref TextBlock lblgrMaxAuthGifts)
    {
      decimal curMaxAuthGifts;
      bool blnWithMaxAuthGifts = false;

      curMaxAuthGifts = CalculateMaxAuthGifts(GuestID, txtgrls.Text, ChargeTo, ref blnWithMaxAuthGifts);
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
    /// <param name="leadSource"></param>
    /// <param name="withMaxAuthGifts"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// [vipacheco] 16/Mayo/2016 Modified --> Migrado a esta clase general de ReceiptGifts
    /// </history>
    public static decimal CalculateMaxAuthGifts(int GuestID, string leadSource, ChargeTo ChargeTo, ref bool withMaxAuthGifts)
    {
      decimal curMaxAuthGifts = 0;
      withMaxAuthGifts = true;

      switch (ChargeTo.ctCalcType)
      {
        //Monto maximo de regalos por Lead Source
        case "A":
          // Si tiene Lead Source
          if (leadSource != "")
          {
            LeadSource _leadSource = BRLeadSources.GetLeadSourceByID(leadSource);
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
      Gifts.CalculateTotalGifts(Grid, GiftsType, ref txtTotalCost, ref txtTotalPrice, ref txtTotalToPay, OnlyCancellled, CancelField);
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
    public async static void Save(List<KeyValuePair<EnumMode, GiftsReceiptDetail>> _LogGiftDetail, Dictionary<string, List<GiftsReceiptPackageItem>> Packs, int _GuestID, int ReceiptID, bool _blnInvitationGifts)
    {
      if (_blnInvitationGifts)  // Si las invitaciones vienen de InvitsGifts
      {
        if (_LogGiftDetail.Count > 0)
        {
          // guardamos en GiftsReceiptsC
          foreach (KeyValuePair<EnumMode, GiftsReceiptDetail> item in _LogGiftDetail)
          {
            await BREntities.OperationEntity(item.Value, EnumMode.add);

            // Encontramos el Gifts agregado
            Gift _Gift = frmHost._lstGifts.Where(x => x.giID == item.Value.gegi).Single();

            // verificamos si maneja paquetes
            if (_Gift.giPack)
            {
              // Obtenemos la lista de regalos del paquete


              // Agregamos los regalos del paquete
              GiftsReceiptsPacks.Update(ReceiptID, _Gift.giID, EnumMode.add);
            }

          }

          // Actualizamos los campos iggr de la tabla iggr de la tabla  InvitsGifts
          foreach (KeyValuePair<EnumMode, GiftsReceiptDetail> item in _LogGiftDetail)
          {
            InvitationGift _invitationGift = BRInvitsGifts.GetInvitGift(_GuestID, item.Value.gegi);

            if (_invitationGift != null)
              _invitationGift.iggr = ReceiptID;

            // Guardamos el cambio
            await BREntities.OperationEntity(_invitationGift, EnumMode.edit);
          }
        }
      }
      else
      {
        // guardamos en GiftsReceiptsC
        foreach (KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail> item in _LogGiftDetail)
          await BREntities.OperationEntity(item.Value, item.Key);
      }
    }
    #endregion

  }
}

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
using System.Windows.Controls;
using IM.Base.Classes;

namespace IM.Host.Classes
{
  /// <summary>
  /// Clase para el manejo de productos externos a cancelar
  /// </summary>
  /// <history>
  /// [vipacheco] 18/Mayo/2016 Created
  /// </history>
  public class GiftsCancel
  {

    #region Validate
    /// <summary>
    /// Valida los regalos a cancelar de tarjetas de regalos
    /// </summary>
    /// <param name="Grid"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 18/Mayo/2016 Created
    /// </history>
    public static bool Validate(DataGrid Grid, string CancelField)
    {
      int RowsCount = 0;
      // Validamos que haya al menos un regalo por cancelar
      foreach (var item in Grid.Items)
      {
        Type type = item.GetType();
        var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

        if (property.Count() > 0)
        {
          if (((bool)type.GetProperty(CancelField).GetValue(item, null)) != false)
          {
            RowsCount++;
            break;
          }
        }
      }

      if (RowsCount != 0) // Si existe al menos un regalo por cancelar.
        return true;
      else
      {
        UIHelper.ShowMessage("Specify at least one gift for cancel.", System.Windows.MessageBoxImage.Information);
        return false;
      }
    }
    #endregion

    #region CalculateTotalGifts
    /// <summary>
    /// Calcula el total de regalos
    /// </summary>
    /// <param name="OnlyCancellled"></param>
    /// <history>
    /// [vipacheco] 24/Mayo/2016 Created
    /// </history>
    public static void CalculateTotalGifts(DataGrid Grid, ref TextBox txtTotalCost, bool OnlyCancellled = false, string CancelField = "")
    {
      GiftsReceiptDetailCancel row = Grid.SelectedItem as GiftsReceiptDetailCancel;

      Gifts.CalculateTotalGifts(Grid, EnumGiftsType.ReceiptGifts, nameof(row.geQty), nameof(row.gegi), nameof(row.gePriceM), nameof(row.gePriceMinor), nameof(row.gePriceAdult), nameof(row.gePriceA), nameof(row.gePriceExtraAdult), txtTotalCost,  OnlyCancellled: OnlyCancellled, CancelField: CancelField);
    }
    #endregion

    #region GetItems
    /// <summary>
    /// Obtiene los elementos del Grid
    /// </summary>
    /// <param name="Grid"></param>
    /// <param name="CheckedStatus"></param>
    /// <param name="CancelField"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    public static List<string> GetItems(DataGrid Grid, EnumCheckedStatus CheckedStatus, string CancelField)
    {
      switch (CheckedStatus)
      {
        case EnumCheckedStatus.All:
          return GridHelper.GetItems(Grid, "gegi");
        case EnumCheckedStatus.Checked:
          return GridHelper.GetItemsChecked(Grid, "gegi", CancelField);
        case EnumCheckedStatus.Unchecked:
          return GridHelper.GetItemsChecked(Grid, "gegi", CancelField, false);
        default:
          return GridHelper.GetItems(Grid, "gegi");
      }
    } 
    #endregion

    #region Save
    /// <summary>
    /// guardamos los regalos cancelados
    /// </summary>
    /// <param name="pReceiptID"></param>
    /// <param name="pReceiptExchangeID"></param>
    /// <param name="pGiftsCancelled"></param>
    /// <param name="pUseCxCCost"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 18/Julio/2016 Created
    /// </history>
    public async static Task Save(int pReceiptID, int pReceiptExchangeID, List<string> pGiftsCancelled, bool pUseCxCCost, GiftsReceipt pGiftsReceipt, ObservableCollection<GiftsReceiptDetail> pGiftsReceiptsDetail, ObservableCollection<GiftsReceiptDetailCancel> pGiftsCancel, bool pIsExchange, string pCancelFiel)
    {
      // obtenemos los regalos del recibo original para marcarlos como cancelados
      List<GiftsReceiptDetail> lstResult = await BRGiftsReceiptDetail.GetGiftsReceiptDetail(pReceiptID);

      // Recorremos los regalos cancelados
      foreach (string iGift in pGiftsCancelled)
      {
        // Localizamos los regalos cancelados
        GiftsReceiptDetail giftupdate = lstResult.Where(x => x.gegi == iGift).Single();
        Gift gift = frmHost._lstGifts.Where(x => x.giID == giftupdate.gegi).Single();

        // marcamos el regalo como cancelado en el recibo original
        giftupdate.GetType().GetProperty(pCancelFiel).SetValue(giftupdate, true, null);

        // si se debe generar un recibo exchange
        if (pIsExchange)
        {
          // agregamos el regalo en el recibo exchange
          await AddGiftExchange(pReceiptExchangeID, giftupdate, pGiftsCancelled);
  }
}

    }
    #endregion

    #region AddGiftExchange
    /// <summary>
    /// Agrega un regalo a un recibo exchange
    /// </summary>
    /// <param name="pReceiptExchangeID"></param>
    /// <param name="pGiftReceipt"></param>
    /// <param name="pGiftsCancelled"></param>
    /// <param name="pUseCxCCost"></param>
    /// <history>
    /// [vipacheco] 21/Julio/2016 Created
    /// </history>
    public async static Task AddGiftExchange(int pReceiptExchangeID, GiftsReceiptDetail pGiftReceipt, List<string> pGiftsCancelled)
    {
      pGiftReceipt.gegr = pReceiptExchangeID;
      pGiftReceipt.geQty = pGiftReceipt.geQty * -1;
      pGiftReceipt.gePriceA = pGiftReceipt.gePriceA * -1;
      pGiftReceipt.gePriceM = pGiftReceipt.gePriceM * -1;
      pGiftReceipt.gePriceAdult = pGiftReceipt.gePriceAdult * -1;
      pGiftReceipt.gePriceMinor = pGiftReceipt.gePriceMinor * -1;
      pGiftReceipt.gePriceExtraAdult = pGiftReceipt.gePriceExtraAdult * -1;

      await BREntities.OperationEntity(pGiftReceipt, EnumMode.Add);
    } 
    #endregion

  }
}

using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IM.Host.Classes
{
  /// <summary>
  /// Clase para el manejo de productos externos de intercambio
  /// </summary>
  /// <history>
  /// [vipacheco] 18/Mayo/2016 Created
  /// </history>
  public class GiftsExchange
  {

    #region Validate
    /// <summary>
    /// Valida los regalos de intercambio de un recibo de regalos
    /// </summary>
    /// <returns></returns>
    public static bool Validate(DataGrid Grid)
    {
      return GridHelper.Validate(Grid, false, 1, "Gifts", "Gift", new List<string> { "gegi" }, Grid.ItemsSource.OfType<GiftsReceiptDetail>().ToList());
    } 
    #endregion

    #region GetItems
    /// <summary>
    /// Obtiene los elementos
    /// </summary>
    /// <param name="Grid"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    public static List<string> GetItems(DataGrid Grid)
    {
      return GridHelper.GetItems(Grid, "gegi");
    }
    #endregion

    #region Save
    /// <summary>
    /// Guarda recibos de intercambio
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <param name="Grid"></param>
    /// <history>
    /// [vipacheco] 01/Julio/2016 Created
    /// </history>
    public async static void Save(int ReceiptID, DataGrid Grid)
    {
      bool MustSave = false;
      // preparamos los regalos de paquetes de regalos del recibo de regalos para guardarse
      GiftsReceiptsPacks.PrepareToSave(ReceiptID, ref MustSave);

      // guardamos los regalos
      foreach (var Current in Grid.Items)
      {
        GiftsReceiptDetail _selected = null;
        if (Current is GiftsReceiptDetail)
        {
          _selected = Current as GiftsReceiptDetail;
          // Asignamos el ID generado
          _selected.gegr = ReceiptID;

          // Si se ingreso los campos obligatorios.
          if (_selected.geQty > 0 && _selected.gegi != null)
          {
            await BREntities.OperationEntity(_selected, Model.Enums.EnumMode.Add);

            // Buscamos el regalo
            Gift gift = frmHost._lstGifts.Where(x => x.giID == _selected.gegi).Single();

            // Verificamos si tiene regalos del paquete
            if (gift.giPack)
            {
              // Buscamos los regalos del paquete
              var packs = frmHost._lstGiftsPacks.Where(x => x.gpPack == gift.giID).ToList();
              var giftsPacks = packs.Select(x => new GiftsReceiptPackageItem
              {
                gkgr = ReceiptID,
                gkPack = x.gpPack,
                gkgi = x.gpgi,
                gkQty = 1,
                gkAdults = 1,
                gkMinors = 0,
                gkPriceA = frmHost._lstGifts.Where(f => f.giID == x.gpgi).Select(s => s.giPrice1).Single(),
                gkPriceM = 0
              }).ToList();

              // Guardamos los regalos
              await BREntities.OperationEntities(giftsPacks, Model.Enums.EnumMode.Add);
            }
          }
        }
      }
    }
    #endregion

  }
}

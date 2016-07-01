using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRGiftsReceiptDetail
  {

    #region GetGiftsReceiptsDetail
    /// <summary>
    /// Consulta los regalos de un recibo
    /// </summary>
    /// <param name="receipt"> Clave del recibo de regalos </param>
    /// <param name="package"> Indica si se desean los paquetes de regalos </param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 06/04/2016
    /// </history>
    public async static Task<List<GiftsReceiptDetail>> GetGiftsReceiptDetail(int receipt, bool package = false)
    {
      List<GiftsReceiptDetail> lstResult = new List<GiftsReceiptDetail>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          // Obtenemos los resultados del Stored
           List<GiftsReceiptDetailShort> lstShort = dbContext.USP_OR_GetGiftsReceiptDetail(receipt, package).ToList();

          // Contruimos la entidad pura
          lstShort.ForEach(x => lstResult.Add(dbContext.GiftsReceiptsDetails.Where(w => w.gegi == x.gegi && w.gegr == x.gegr).Single()));
        }
      });

      return lstResult;
    }
    #endregion

    #region GetGiftReceiptDetail
    /// <summary>
    /// Obtiene la instancia de un Gift Receip en especifico
    /// </summary>
    /// <param name="receipt"></param>
    /// <param name="giftID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 06/Marzo/2016 Created
    /// </history>
    public static GiftsReceiptDetail GetGiftReceiptDetail(int receipt, string giftID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.GiftsReceiptsDetails.Where(x => x.gegr == receipt && x.gegi == giftID).SingleOrDefault();
      }
    }
    #endregion

    #region GetGiftsReceiptDetailCancel
    /// <summary>
    /// Consulta los regalos de un recibo que tienen asociados productos externos y que pueden ser cancelados
    /// </summary>
    /// <param name="GuestID"></param>
    /// <param name="EnumExternal"></param>
    /// <param name="package"></param>
    /// <history>
    /// [vipacheco] 16/Mayo/2016 Created
    /// </history>
    public static List<GiftsReceiptDetailCancel> GetGiftsReceiptDetailCancel(int GuestID, EnumExternalProduct EnumExternal, bool? package = false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGiftsReceiptDetailCancel(GuestID, (byte)EnumExternal, package).ToList();
      }
    }
    #endregion

    #region GetGiftsReceiptDetailPromotionsPVP
    /// <summary>
    /// Consulta los regalos de un recibo que tienen asociados promociones de PVP
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    public static List<GiftsReceiptDetailPromotionsSistur> GetGiftsReceiptDetailPromotionsPVP(int ReceiptID)
    {
      //List<GiftsReceiptDetailPromotionsSistur> Result = null;
      //await Task.Run(() =>
      //{
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGiftsReceiptDetailPromotionsPVP(ReceiptID).ToList();
      }
      //});

      //return Result;
    }
    #endregion

    #region UpdateGiftPromotionSistur
    /// <summary>
    /// Indica que a un regalo de un recibo se le guardo su promocion en Sistur
    /// </summary>
    /// <param name="Receipt"></param>
    /// <param name="Gift"></param>
    /// <param name="Promotion"></param>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    public static void UpdateGiftPromotionSistur(int Receipt, string Gift, string Promotion)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_UpdateGiftsReceiptDetailPromotionPVP(Receipt, Gift, Promotion);
      }
    }
    #endregion

    #region GetGiftsReceiptDetailPromotionsOpera
    /// <summary>
    /// Obtiene los regalos con promociones de Opera
    /// </summary>
    /// <param name="Receipt"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 01/Junio/2016 Created
    /// </history>
    public static List<GiftsReceiptDetailPromotionsOpera> GetGiftsReceiptDetailPromotionsOpera(int Receipt)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGiftsReceiptDetailPromotionsOpera(Receipt).ToList();
      }
    }
    #endregion

    #region UpdateGiftsReceiptDetailPromotionOpera
    /// <summary>
    /// Indica que un regalo de un recibo se guardo como promocion de Opera
    /// </summary>
    /// <param name="Receipt">Clave del recibo de regalos</param>
    /// <param name="Gift">Clave del regalo</param>
    /// <param name="Promotion">Clave de la promocion de Opera</param>
    /// <history>
    /// [vipacheco] 01/Junio/2016 Created
    /// </history>
    public static void UpdateGiftsReceiptDetailPromotionOpera(int Receipt, string Gift, string Promotion)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_UpdateGiftsReceiptDetailPromotionOpera(Receipt, Gift, Promotion);
      }
    }
    #endregion

    #region UpdateGiftsReceiptDetailRoomChargeOpera
    /// <summary>
    /// Indica que a un regalo de un recibo se le guardo su cargo a habitacion en Opera
    /// </summary>
    /// <param name="Receipt"></param>
    /// <param name="Gift"></param>
    /// <param name="Transaction"></param>
    /// <history>
    /// [vipacheco] 01/Junio/2016 Created
    /// </history>
    public static void UpdateGiftsReceiptDetailRoomChargeOpera(int Receipt, string Gift, string Transaction)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_UpdateGiftsReceiptDetailRoomChargeOpera(Receipt, Gift, Transaction);
      }
    } 
    #endregion

    #region GetGiftsReceiptDetailRoomChargesOpera
    /// <summary>
    /// Obtiene los cargos a habitacion de Opera que no se han dado
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 01/Junio/2016 Created
    /// </history>
    public static List<GiftsReceiptDetailRoomChargesOpera> GetGiftsReceiptDetailRoomChargesOpera(int ReceiptID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGiftsReceiptDetailRoomChargesOpera(ReceiptID).ToList();
      }
    } 
    #endregion

  }
}

using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRGiftsReceipts
  {
    #region GetGiftsReceipts
    /// <summary>
    /// Obtienen los Gifts Receipts Short
    /// </summary>
    /// <param name="guestID"> Clave del huesped </param>
    /// <param name="salesRoom"> Clave de la sala de ventas </param>
    /// <param name="receipt"> Clave del recibo </param>
    /// <param name="folio"> Folio de Palace Resorts </param>
    /// <param name="dateFrom"> Fecha desde </param>
    /// <param name="dateTo"> Fecha hasta </param>
    /// <param name="name"> Nombre </param>
    /// <param name="reservation"> Folio de reservacion </param>
    /// <returns>Lista de tipo GiftsReceptsShort</returns>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    public async static Task<List<GiftsReceiptsShort>> GetGiftsReceipts(int? guestID = 0, string salesRoom = "ALL", int receipt = 0, string folio = "ALL",
                                                            DateTime? dateFrom = null, DateTime? dateTo = null, string name = "ALL",
                                                            string reservation = "ALL")
    {
      List<GiftsReceiptsShort> lstResult = new List<GiftsReceiptsShort>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          // Obtenemos los GiftsReceiptShort del Stored correspondiente con los campos correspondientes
          dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetGiftsReceipts_Timeout;
          lstResult = dbContext.USP_OR_GetGiftsReceipts(guestID, salesRoom, receipt, folio, dateFrom, dateTo, name, reservation).ToList();
        }
      });

      return lstResult;
    }
    #endregion

    #region GetGiftReceipt
    /// <summary>
    /// Obtiene la información de un Gifts Receipt especifico.
    /// </summary>
    /// <param name="GiftReceiptID"></param>
    /// <returns> Gifts Receipt </returns>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// [vipacheco] 07/Julio/2016 Modified --> se agrego asincronia
    /// </history>
    public async static Task<GiftsReceipt> GetGiftReceipt(int GiftReceiptID)
    {
      GiftsReceipt giftReceipt = new GiftsReceipt();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          giftReceipt = dbContext.GiftsReceipts.Where(x => x.grID == GiftReceiptID).SingleOrDefault();
        }
      });
      return giftReceipt;
    }
    #endregion

    #region ValidateGiftsReceipt
    /// <summary>
    /// Valida que los datos de un recibo de regalos existan
    /// </summary>
    /// <param name="changedBy">Clave del usuario que esta haciendo el cambio</param>
    /// <param name="password">Contraseña del usuario que esta haciendo el cambio</param>
    /// <param name="guest">Clave del huesped</param>
    /// <param name="location">Clave de la locacion</param>
    /// <param name="salesroom">Clave de la sala de ventas</param>
    /// <param name="giftshost">Clave del Host de regalos</param>
    /// <param name="personnel">Clave del personal que ofrecio los regalos</param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    public static ValidationData ValidateGiftsReceipt(string changedBy, string password, int guest, string location, string salesroom, string giftshost, string personnel)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_ValidateGiftsReceipt(changedBy, password, guest, location, salesroom, giftshost, personnel).SingleOrDefault();
      }
    }
    #endregion

    #region SaveGiftReceipt
    /// <summary>
    /// Guarda un nuevo Gift Receipt y retorna el ID asignado
    /// </summary>
    /// <param name="giftReceipt"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 2/Mayo/2016 Created
    /// [vipacheco] 07/Julio/2016 Modified --> Se agregó asincronia
    /// [vipacheco] 05/Agosto/2016 Modified --> Se agregó estandarizacion del return
    /// </history>
    public async static Task<int> SaveGiftReceipt(GiftsReceipt giftReceipt)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          // Guardamos el Gift Receipt Nuevo
          dbContext.Entry(giftReceipt).State = System.Data.Entity.EntityState.Added;
          dbContext.SaveChanges();

          return giftReceipt.grID; // Obtenemos el ID del nuevo Gift Receipt
        }
      });
    }
    #endregion

    #region GetGiftsReceiptsAdditional
    /// <summary>
    /// Obtiene los Gifts Receipts Adicionales de acuerdo al ID del Guest
    /// </summary>
    /// <param name="GuestID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 12/Mayo/2016 Created
    /// </history>
    public static List<GetGiftsReceiptsAdditional> GetGiftsReceiptsAdditional(int GuestID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_IM_GetGiftsReceiptsAdditional(guestID: GuestID).ToList();
      }
    }
    #endregion

    #region CancelGiftsReceipt
    /// <summary>
    /// Cancela un recibo de regalos
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <param name="DateServer"></param>
    /// <history>
    /// [vipacheco] 14/Mayo/2016 Created
    /// </history>
    public static void CancelGiftsReceipt(int ReceiptID, DateTime DateServer)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        dbContext.USP_OR_CancelGiftsReceipt(ReceiptID, DateServer);
      }
    }
    #endregion

    #region CalculateTotalsGiftsInvitation
    /// <summary>
    /// Calcula el total de los regalos de la invitacion
    /// </summary>
    /// <param name="GuestID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Mayo/2016 Created
    /// </history>
    public static decimal? CalculateTotalsGiftsInvitation(int GuestID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_CalculateTotalsGiftsInvitation(GuestID).SingleOrDefault();
      }
    }
    #endregion

    #region CancelGiftPromotionSistur
    /// <summary>
    /// Indica que a un regalo se le cancelo su promocion en Sistur
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <param name="Gift"></param>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// [vipacheco] 25/Julio/2016 Modified --> Se agrego asyncronia
    /// </history>
    public async static Task CancelGiftPromotionSistur(int ReceiptID, string Gift)
    {
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.USP_OR_UpdateGiftsReceiptDetailPromotionPVPCancel(ReceiptID, Gift);
        }
      });
    }
    #endregion

    #region UpdateCharge
    /// <summary>
    /// Actualiza el cargo de un recibo de regalos
    /// </summary>
    /// <param name="pGuestID"></param>
    /// <param name="pCharge"></param>
    /// <param name="pAdjustement"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 08/Julio/2016 Created
    /// </history>
    public async static Task UpdateCharge(int pGuestID, decimal pCharge, decimal pAdjustement)
    {
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.USP_OR_UpdateCharge(pGuestID, pCharge, pAdjustement);
        }
      });
    }
    #endregion

    #region getRptGiftsReceipt
    /// <summary>
    /// Obtiene la informacion para el repote GiftReceipt del Modulo Host.
    /// </summary>
    /// <param name="receiptID"> ID del recibo. </param>
    /// <param name="isCharge"> 0. Si no es cargo.
    ///  1. Si es cargo. </param>
    /// <returns> List of IEnumerable </returns>
    /// <history>
    /// [edgrodriguez] 12/Jul/2016 Created
    /// </history>
    public static async Task<List<IEnumerable>> GetRptGiftsReceipt(int receiptID, bool isCharge = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var results = dbContext.USP_OR_RptGiftsReceipt(receiptID, isCharge);
          if (isCharge)
          {
            return results
            .MultipleResults()
            .With<RptGiftsReceipt>()
            .GetValues();
          }
          else
            return results
            .MultipleResults()
            .With<RptGiftsReceipt>()
            .With<RptGiftsReceipt_Gifts>()
            .With<RptGiftsReceipt_ProductLegends>()
            .GetValues();
        }
      });
    }
    #endregion

  }
}

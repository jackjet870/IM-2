using System;
using System.Collections.Generic;
using System.Linq;
using IM.BusinessRules.Properties;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Enums;
using System.Collections;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRReportsBySalesRoom
  {

    #region Processor General

    #region Bookings

    #region GetRptBookingsBySalesRoomProgramTime

    /// <summary>
    /// Obtiene el reporte de Bookings por Sala de venta y Programa
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptBookingsBySalesRoomProgramTime </returns>
    /// <history>
    /// [edgrodriguez] 12/Mar/2016 Created
    /// </history>
    public static async Task<List<RptBookingsBySalesRoomProgramTime>> GetRptBookingsBySalesRoomProgramTime(DateTime dtmStart, string salesRooms = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_RptBookingsBySalesRoomProgramTime(dtmStart, salesRooms).ToList();
        }
      });
    }
    #endregion

    #region GetRptBookingsBySalesRoomProgramLeadSourceTime
    /// <summary>
    /// Obtiene el reporte de Bookings por Sala de Venta,Programa y Locaciones.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="salesRooms">Salas de Venta</param>
    /// <returns> List of RptBookingsBySalesRoomProgramLeadSourceTime </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static async Task<List<RptBookingsBySalesRoomProgramLeadSourceTime>> GetRptBookingsBySalesRoomProgramLeadSourceTime(DateTime dtmStart, string salesRooms = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_RptBookingsBySalesRoomProgramLeadSourceTime(dtmStart, salesRooms).ToList();
        }
      });
    }
    #endregion

    #endregion

    #region CxC

    #region GetRptCxC
    /// <summary>
    /// Obtiene el reporte de CxC por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms">Sala de Venta</param>
    /// <returns> List of RptCxC </returns>
    /// <history>
    /// [edgrodriguez] 17/May/2016 Created
    /// </history>
    public static async Task<List<RptCxCExcel>> GetRptCxC(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_RptCxCExcel(dtmStart, dtmEnd, salesRooms).ToList();
        }
      });
    }
    #endregion

    #region GetRptCxCByType
    /// <summary>
    /// Obtiene el reporte de CxC By Type por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms">Sala de Venta</param>
    /// <returns> List of RptCxC </returns>
    /// <history>
    /// [edgrodriguez] 22/Mar/2016 Created
    /// </history>
    public static async Task<List<RptCxC>> GetRptCxCByType(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_RptCxC(dtmStart, dtmEnd, salesRooms).ToList();
        }
      }); 
    }
    #endregion

    #region GetRptCxCDeposits
    /// <summary>
    /// Obtiene el reporte de CxCDeposits por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptCxCDeposits </returns>
    /// <history>
    /// [edgrodriguez] 22/Mar/2016 Created
    /// </history>
    public static async Task<List<object>> GetRptCxCDeposits(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstDeposits = dbContext.USP_OR_RptCxCDeposits(dtmStart, dtmEnd, salesRooms).ToList();
          var lstCurrencies = (from cxcDep in lstDeposits
                               join curr in dbContext.Currencies on cxcDep.grcuCxCPRDeposit equals curr.cuID
                               select curr).Distinct().ToList();

          return lstDeposits.Count > 0 ? new List<object> { lstDeposits, lstCurrencies } : new List<object>();
        }
      });
    }
    #endregion

    #region GetRptCxCGifts
    /// <summary>
    /// Obtiene el reporte de CxCGifts por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptCxCGifts </returns>
    /// <history>
    /// [edgrodriguez] 28/Mar/2016 Created
    /// </history>
    public static async Task<List<object>> GetRptCxCGifts(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de regalo.
          var lstGiftReceipt = dbContext.USP_OR_RptCxCGifts(dtmStart, dtmEnd, salesRooms).ToList();
          var gifts = (from gRcpt in lstGiftReceipt
                       join gift in dbContext.Gifts on gRcpt.Gift equals gift.giID
                       select gift).ToList();

          return lstGiftReceipt.Count > 0 ? new List<object> { lstGiftReceipt, gifts } : new List<object>();
        }
      });
    }
    #endregion

    #region GetRptCxCNotAuthorized
    /// <summary>
    /// Obtiene el reporte de CxCNotAuthorized por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptCxCNotAuthorized </returns>
    /// <history>
    /// [edgrodriguez] 01/Abr/2016 Created
    /// </history>
    public static async Task<List<RptCxCNotAuthorized>> GetRptCxCNotAuthorized(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de regalo.
          return dbContext.USP_OR_RptCxCNotAuthorized(dtmStart, dtmEnd, salesRooms).ToList();
        }
      });
    }
    #endregion

    #region GetRptCxCPayments
    /// <summary>
    /// Obtiene el reporte de CxC Payments por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptCxCPayments </returns>
    /// <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static async Task<List<RptCxCPayments>> GetRptCxCPayments(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de regalo.
          return dbContext.USP_OR_RptCxCPayments(dtmStart, dtmEnd, salesRooms).ToList();
        }
      });
    }
    #endregion

    #endregion

    #region Deposits

    #region GetRptDeposits
    /// <summary>
    /// Obtiene el reporte de Deposits por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of object </returns>
    /// <history>
    /// [edgrodriguez] 01/Abr/2016 Created
    /// </history>
    public static async Task<List<object>> GetRptDeposits(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de regalo.
          var giftReceipts = dbContext.USP_OR_RptDeposits(dtmStart, dtmEnd, salesRooms).ToList();
          var currencies = (from gRcpt in giftReceipts
                            join curr in dbContext.Currencies on gRcpt.grcu equals curr.cuID
                            select curr).Distinct().ToList();

          var paymentsType = (from gRcpt in giftReceipts
                              join payTyp in dbContext.PaymentTypes on gRcpt.grpt equals payTyp.ptID
                              select payTyp).Distinct().ToList();

          return giftReceipts.Count > 0 ? new List<object> { giftReceipts, currencies, paymentsType } : new List<object>();
        }
      });
    }
    #endregion

    #region GetRptDepositsBurned
    /// <summary>
    /// Obtiene el reporte de Deposits Burned por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of object </returns>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static async Task<List<object>> GetRptDepositsBurned(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        List<string> salesRoom = salesRooms.Split(',').ToList();
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de regalo.
          var giftReceipts = dbContext.USP_OR_RptDepositsBurned(dtmStart, dtmEnd, salesRooms).ToList();

          var currencies = (from gRcpt in giftReceipts
                            join curr in dbContext.Currencies on gRcpt.grcu equals curr.cuID
                            select curr).Distinct().ToList();

          var paymentsType = (from gRcpt in giftReceipts
                              join payTyp in dbContext.PaymentTypes on gRcpt.grpt equals payTyp.ptID
                              select payTyp).Distinct().ToList();

          var saleReceipts = (from gSale in dbContext.Sales
                              join gu in dbContext.Guests on gSale.sagu equals gu.guID
                              join gRctp in dbContext.GiftsReceipts on gu.guID equals gRctp.grgu
                              join per in dbContext.Personnels on gRctp.grpe equals per.peID
                              where salesRoom.Contains(gSale.sasr) &&
                              gSale.saD >= dtmStart && gSale.saD <= dtmEnd &&
                              gRctp.grDepositTwisted > 0
                              select new
                              {
                                gRctp.grID,
                                gSale
                              })
                              .GroupBy(c => c.grID)
                              .ToDictionary(c => c.Key, c => c.Select(s => s.gSale).ToList());

          return giftReceipts.Count > 0 ? new List<object> { giftReceipts, currencies, paymentsType, saleReceipts } : new List<object>();
        }
      });
    }
    #endregion

    #region GetRptDepositsBurnedByResort
    /// <summary>
    /// Obtiene el reporte de CxCNotAuthorized por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of object </returns>
    /// <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static async Task<List<object>> GetRptDepositsBurnedByResort(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        List<string> salesRoom = salesRooms.Split(',').ToList();
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de regalo.
          var giftReceipts = dbContext.USP_OR_RptDepositsBurnedByResort(dtmStart, dtmEnd, salesRooms).ToList();

          var currencies = (from gRcpt in giftReceipts
                            join curr in dbContext.Currencies on gRcpt.grcu equals curr.cuID
                            select curr).Distinct().ToList();

          var paymentsType = (from gRcpt in giftReceipts
                              join payTyp in dbContext.PaymentTypes on gRcpt.grpt equals payTyp.ptID
                              select payTyp).Distinct().ToList();

          var saleReceipts = (from gSale in dbContext.Sales
                              join gu in dbContext.Guests on gSale.sagu equals gu.guID
                              join gRctp in dbContext.GiftsReceipts on gu.guID equals gRctp.grgu
                              join per in dbContext.Personnels on gRctp.grpe equals per.peID
                              where salesRoom.Contains(gSale.sasr) &&
                              gSale.saD >= dtmStart && gSale.saD <= dtmEnd &&
                              (gRctp.grDepositTwisted > 0 || (gu.guHotelB != "" && gu.guHotelB != null))
                              select new
                              {
                                gRctp,
                                gSale,
                                gu,
                                per
                              })
                              .GroupBy(c => c.gRctp.grID)
                              .ToDictionary(c => c.Key, c => c.Select(s => new Tuple<RptDepositsBurnedByResort, Sale>(
                              new RptDepositsBurnedByResort
                              {
                                grID = s.gRctp.grID,
                                grNum = s.gRctp.grNum,
                                grD = s.gRctp.grD,
                                grgu = s.gRctp.grgu,
                                grGuest = s.gRctp.grGuest,
                                grHotel = s.gRctp.grHotel,
                                guHotelB = s.gu.guHotelB,
                                grlo = s.gRctp.grlo,
                                grsr = s.gRctp.grsr,
                                grpe = s.gRctp.grpe,
                                peN = s.per.peN,
                                grcu = s.gRctp.grcu,
                                grpt = s.gRctp.grpt,
                                grHost = s.gRctp.grHost,
                                grDepositTwisted = s.gRctp.grDepositTwisted
                              }, s.gSale)).ToList());



          return (giftReceipts.Count > 0) ? new List<object> { giftReceipts, currencies, paymentsType, saleReceipts } : new List<object>();
        }
      });
    }
    #endregion

    #region GetRptPaidDeposits

    /// <summary>
    /// Obtiene el reporte de Paid Deposits por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <param name="leadSources"></param>
    /// <returns> List of object </returns>
    /// <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static async Task<List<object>> GetRptPaidDeposits(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", string leadSources = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de regalo.
          var giftReceipts = dbContext.USP_OR_RptDepositsPaid(dtmStart, dtmEnd, leadSources, salesRooms).ToList();

          var currencies = (from gRcpt in giftReceipts
                            join curr in dbContext.Currencies on gRcpt.grcu equals curr.cuID
                            select curr).Distinct().ToList();

          var paymentsType = (from gRcpt in giftReceipts
                              join payTyp in dbContext.PaymentTypes on gRcpt.grpt equals payTyp.ptID
                              select payTyp).Distinct().ToList();

          return giftReceipts.Count > 0 ? new List<object> { giftReceipts, currencies, paymentsType } : new List<object>();
        }
      });
    }
    #endregion

    #endregion

    #region Gifts

    #region GetRptGiftsCancelledManifest
    /// <summary>
    /// Obtiene el reporte de Cancelled Gifts Manifest por Sala de ventas y una fecha en especifico.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptGiftsManifestCancel </returns>
    /// <history>
    /// [edgrodriguez] 06/Jun/2016 Created
    /// </history>
    public static async Task<List<IEnumerable>> GetRptGiftsCancelledManifest(DateTime? dtmStart,DateTime? dtmEnd, string salesRooms = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de regalo.
          var lstGiftManifestCancel= dbContext.USP_OR_RptGiftsManifestCancel(dtmStart, dtmEnd, salesRooms)
          .MultipleResults()
          .With<RptGiftsManifestCancel>()
          .With<RptGiftsManifestCancel_Gifts>()
          .With<CurrencyShort>()
          .With<PaymentTypeShort>()
          .GetValues();
          return ((lstGiftManifestCancel[0] as List<RptGiftsManifestCancel>).Count > 0) ? lstGiftManifestCancel : new List<IEnumerable>();
        }
      });
    }
    #endregion

    #region GetRptDailyGiftSimple
    /// <summary>
    /// Obtiene el reporte de Daily Gift Simple por Sala de ventas y una fecha en especifico.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptDailyGiftSimple </returns>
    /// <history>
    /// [edgrodriguez] 07/Abr/2016 Created
    /// </history>
    public static async Task<List<RptDailyGiftSimple>> GetRptDailyGiftSimple(DateTime? dtmStart, string salesRooms = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de regalo.
          var lstDailyGiftSimple = dbContext.USP_IM_RptDailyGiftSimple(dtmStart, salesRooms).ToList();
          return lstDailyGiftSimple;
        }
      });
    }
    #endregion

    #region GetRptGiftsByCategory
    /// <summary>
    /// Obtiene el reporte Gifts By Category
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptGiftsByCategory </returns>
    /// <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static async Task<List<RptGiftsByCategory>> GetRptGiftsByCategory(DateTime? dtmStart, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de regalo.
          var lstGiftByCat = dbContext.USP_OR_RptGiftsByCategory(dtmStart, dtmStart.Value.AddDays(6).Date, salesRooms).ToList();
          return lstGiftByCat;
        }
      });
    }

    #endregion

    #region GetRptGiftsByCategoryProgram
    /// <summary>
    /// Obtiene el reporte  Gifts By Category & Program
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns></returns>
    /// <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static async Task<List<RptGiftsByCategoryProgram>> GetRptGiftsByCategoryProgram(DateTime? dtmStart, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de regalo.
          var lstGiftByCatP = dbContext.USP_OR_RptGiftsByCategoryProgram(dtmStart, dtmStart.Value.AddDays(6).Date, salesRooms).ToList();
          return lstGiftByCatP;
        }
      });
    }

    #endregion

    #region GetRptGiftsCertificates
    /// <summary>
    /// Obtiene el reporte de Gifts Certificates.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <param name="categories"></param>
    /// <param name="gifts"></param>
    /// <returns> List of object </returns>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static async Task<List<object>> GetRptGiftsCertificates(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms, string categories = "ALL", string gifts = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los certificados de regalo.
          var dailyGiftCertificates = dbContext.USP_OR_RptGiftsCertificates(dtmStart, dtmEnd, salesRooms, categories, gifts).ToList();
          var currencies = (from gift in dailyGiftCertificates.Select(c => c.Currency).Distinct()
                            join curr in dbContext.Currencies on gift equals curr.cuID
                            select curr).ToList();
          var payType = (from gift in dailyGiftCertificates.Select(c => c.PaymentType).Distinct()
                         join payT in dbContext.PaymentTypes on gift equals payT.ptID
                         select payT).ToList();

          return dailyGiftCertificates.Count > 0 ? new List<object> { dailyGiftCertificates, currencies, payType } : new List<object>();
        }
      });
    }

    #endregion

    #region GetRptGiftsManifest
    /// <summary>
    /// Obtiene el reporte de Gifts Manifest.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <param name="categories"></param>
    /// <param name="gifts"></param>
    /// <param name="status"></param>
    /// <returns> List of object </returns>
    /// <history>
    /// [edgrodriguez] 10/May/2016 Created
    /// </history>
    public static async Task<List<IEnumerable>> GetRptGiftsManifest(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms, string categories = "ALL", string gifts = "ALL", EnumStatus status = EnumStatus.staAll)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los certificados de regalo.
          var lstGiftsManifest = dbContext.USP_OR_RptGiftsManifest(dtmStart, dtmEnd, salesRooms, categories, gifts, Convert.ToByte(status))
            .MultipleResults()
            .With<RptGiftsManifest>()
            .With<GiftForReceipts>()
            .With<CurrencyShort>()
            .With<PaymentTypeShort>()
            .GetValues();

          return ((lstGiftsManifest[0] as List<RptGiftsManifest>).Count > 0) ? lstGiftsManifest : new List<IEnumerable>();
        }
      });
    }

    #endregion

    #region GetRptGiftsMReceipts

    /// <summary>
    /// Obtiene el reporte de Gifts Receipts.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <param name="categories"></param>
    /// <param name="gifts"></param>
    /// <param name="status"></param>
    /// <param name="giftReceiptType"></param>
    /// <param name="guestId"></param>
    /// <returns> List of object </returns>
    /// <history>
    /// [edgrodriguez] 11/May/2016 Created
    /// </history>
    public static async Task<List<IEnumerable>> GetRptGiftsMReceipts(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms, string categories = "ALL", string gifts = "ALL", EnumStatus status = EnumStatus.staAll, EnumGiftsReceiptType giftReceiptType = EnumGiftsReceiptType.grtAll, int guestId = 0)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los certificados de regalo.
          var lstGiftsManifest = dbContext.USP_OR_RptGiftsReceipts(dtmStart, dtmEnd, salesRooms, categories, gifts, Convert.ToByte(status), Convert.ToByte(giftReceiptType), guestId)
            .MultipleResults()
            .With<RptGiftsReceipts>()
            .With<GiftForReceipts>()
            .With<CurrencyShort>()
            .With<PaymentTypeShort>()
            .GetValues();

          return ((lstGiftsManifest[0] as List<RptGiftsReceipts>).Count > 0) ? lstGiftsManifest : new List<IEnumerable>();

        }
      });
    }

    #endregion

    #region GetRptGiftsReceiptsPayments
    /// <summary>
    /// Obtiene el reporte de Gifts Receipts Payments.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of object </returns>
    /// <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static async Task<List<object>> GetRptGiftsReceiptsPayments(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los certificados de regalo.
          var lstGiftRcptPay = dbContext.USP_OR_RptGiftsReceiptsPayments(dtmStart, dtmEnd, salesRooms).ToList();

          var source = (from gift in lstGiftRcptPay.Select(c => c.Source).Distinct()
                        join src in dbContext.SourcePayments on gift equals src.sbID
                        select src).ToList();

          var currencies = (from gift in lstGiftRcptPay.Select(c => c.Currency).Distinct()
                            join curr in dbContext.Currencies on gift equals curr.cuID
                            select curr).ToList();
          var payType = (from gift in lstGiftRcptPay.Select(c => c.PaymentType).Distinct()
                         join payT in dbContext.PaymentTypes on gift equals payT.ptID
                         select payT).ToList();

          return lstGiftRcptPay.Count > 0 ? new List<object> { lstGiftRcptPay, source, currencies, payType } : new List<object>();
        }
      });
    }

    #endregion

    #region GetRptGiftsSale
    /// <summary>
    /// Obtiene el reporte de Gifts Sale.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <param name="categories"></param>
    /// <param name="gifts"></param>
    /// <param name="sale"></param>
    /// <returns> List of object </returns>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static async Task<List<IEnumerable>> GetRptGiftsSale(DateTime dtmStart, DateTime dtmEnd, string salesRooms = "ALL", string categories = "ALL", string gifts = "ALL", EnumGiftSale sale = EnumGiftSale.gsAll)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de venta.
          var query = dbContext.USP_OR_RptGiftsSale(dtmStart, dtmEnd, salesRooms, categories, gifts, Convert.ToByte(sale))
            .MultipleResults()
            .With<RptGiftsSale>()
            .With<RptGiftsSale_Payment>()
            .With<SourcePaymentShort>()
            .With<CurrencyShort>()
            .With<PaymentTypeShort>()
            .GetValues();

          return (query[0] as List<RptGiftsSale>).Count > 0 ? query : new List<IEnumerable>();
        }
      });
    }

    #endregion

    #region GetRptGiftsUsedBySistur
    /// <summary>
    /// Obtiene el reporte de Gifts Used By Sistur.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <param name="programs"></param>
    /// <param name="leadsources"></param>
    /// <returns> List of RptGiftsUsedBySistur </returns>
    /// <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static async Task<List<RptGiftsUsedBySistur>> GetRptGiftsUsedBySistur(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", string programs = "ALL", string leadsources = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los certificados de regalo.
          var lstGiftSist = dbContext.USP_OR_RptGiftsUsedBySistur(dtmStart, dtmEnd, salesRooms, programs, leadsources).ToList();

          return lstGiftSist;
        }
      });
    }

    #endregion

    #region GetRptWeeklyGiftsItemsSimple
    /// <summary>
    /// Obtiene el reporte de Weekly Gift Simple por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptWeeklyGiftsItemsSimple </returns>
    /// <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static async Task<List<RptWeeklyGiftsItemsSimple>> GetRptWeeklyGiftsItemsSimple(DateTime? dtmStart, string salesRooms = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //Obtenemos los recibos de regalo.
          var lstweeklyGiftSimple = dbContext.USP_IM_RptWeeklyGiftsItemsSimple(dtmStart, dtmStart.Value.AddDays(6), salesRooms).ToList();
          return lstweeklyGiftSimple;
        }
      });
    }
    #endregion

    #endregion

    #region Guests

    #region GetRptGuestCeco
    /// <summary>
    /// Obtiene el reporte de Weekly Gift Simple por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptGuestCeco </returns>
    /// <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static async Task<List<RptGuestCeco>> GetRptGuestCeco(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstGuestCeco = dbContext.USP_OR_RptGuestCeco(dtmStart, dtmEnd, salesRooms).ToList();
          return lstGuestCeco;
        }
      });
    }
    #endregion

    #region GetRptGuestNoBuyers
    /// <summary>
    /// Obtiene el reporte de Guest No Buyers por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptGuestsNoBuyers </returns>
    /// <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static async Task<List<RptGuestsNoBuyers>> GetRptGuestNoBuyers(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstGuestNoBuyers = dbContext.USP_OR_RptGuestsNoBuyers(dtmStart, dtmEnd, salesRooms).ToList();
          return lstGuestNoBuyers;
        }
      });
    }
    #endregion

    #region GetRptInOut
    /// <summary>
    /// Obtiene el reporte de In & Out por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptInOut </returns>
    /// <history>
    /// [edgrodriguez] 09/Abr/2016 Created
    /// </history>
    public static async Task<List<RptInOut>> GetRptInOut(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstInOut = dbContext.USP_OR_RptInOut(dtmStart, dtmEnd, salesRooms).ToList();
          return lstInOut;
        }
      });
    }
    #endregion

    #region GetRptManifestRange

    /// <summary>
    /// Obtiene el reporte Guest Manifest por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <param name="program"></param>
    /// <returns> List of RptInOut</returns>
    /// <history>
    /// [edgrodriguez] 06/Jun/2016 Created
    /// </history>
    public static async Task<List<RptManifestRange>> GetRptManifestRange(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms="ALL", EnumProgram program=EnumProgram.All)
    { 
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.USP_IM_RptManifestRange_Timeout;
          var lstManifestRange = dbContext.USP_IM_RptManifestRange(dtmStart, dtmEnd, salesRooms, EnumToListHelper.GetEnumDescription(program)).ToList();
          return lstManifestRange;
        }
      });
    }
    #endregion

    #region GetRptManifestRangeByLs

    /// <summary>
    /// Obtiene el reporte Guest Manifest por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <param name="program"></param>
    /// <returns> List of RptInOut</returns>
    /// <history>
    /// [edgrodriguez] 13/Jun/2016 Created
    /// [edgrodriguez] 22/Jun/2016 Modified. El método realiza el procesamiento.
    /// </history>
    public static async Task<List<IEnumerable>> GetRptManifestRangeByLs(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", EnumProgram program = EnumProgram.All)
    {
      return await Task.Run(() =>
      {
        var lstManifestRange = new List<IEnumerable>();
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.USP_IM_RptManifestRange_Timeout;
           lstManifestRange = dbContext.USP_IM_RptManifestByLSRange(dtmStart, dtmEnd, salesRooms)
          .MultipleResults()
          .With<RptManifestByLSRange>()
          .With<RptManifestByLSRange_Bookings>()
          .GetValues();
        }

        var lstRptManifest = lstManifestRange[0] as List<RptManifestByLSRange>;
        var lstBookings = lstManifestRange[1] as List<RptManifestByLSRange_Bookings>;
        if (lstBookings.Any())
        {
          var guloInvitList = lstBookings.Select(c => c.guloInvit).Distinct().ToList();
          guloInvitList.ForEach(c =>
          {
            lstBookings.Add(new RptManifestByLSRange_Bookings
            {
              guloInvit = c,
              LocationN = lstBookings.FirstOrDefault(b => b.guloInvit == c).LocationN,
              guBookT = "Total",
              Bookings = lstBookings.Where(b => b.guloInvit == c).Sum(b => b.Bookings)
            });
          });
          var NotExitsInManifest = lstBookings.Select(c => c.LocationN).Except(lstRptManifest.Where(c => c.SaleType == 0 || c.SaleType == 1 || c.SaleType == 2).Select(c => c.LocationN)).ToList();
          NotExitsInManifest.ForEach(c =>
          {
            lstRptManifest.Add(new RptManifestByLSRange
            {
              Location = lstRptManifest.FirstOrDefault(b => b.LocationN == c)?.Location ?? lstBookings.FirstOrDefault(b => b.LocationN == c).guloInvit,
              LocationN = c,
              SaleType = lstRptManifest.FirstOrDefault(b => b.LocationN == c)?.SaleType ?? 0,
              SaleTypeN = lstRptManifest.FirstOrDefault(b => b.LocationN == c)?.SaleTypeN ?? "MANIFEST"
            });
          });

          lstRptManifest = lstRptManifest
           .OrderBy(c => c.SaleType)
           .ThenBy(c => c.Location)
           .ThenBy(c => c.ShowProgramN)
           .ThenBy(c => c.Sequency)
           .ThenBy(c => c.TimeInT)
           .ThenBy(c => c.LastName)
           .ToList();
        }
        return (lstRptManifest.Any()) ? new List<IEnumerable>() { lstRptManifest, lstBookings } : new List<IEnumerable>();

      });
    }
    #endregion

    #region GetRptGuestNoShows

    /// <summary>
    /// Obtiene el reporte de Guest No Show por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List of RptGuestsNoShows </returns>
    /// <history>
    /// [edgrodriguez] 09/Abr/2016 Created
    /// </history>
    public static async Task<List<RptGuestsNoShows>> GetRptGuestNoShows(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstInOut = dbContext.USP_OR_RptGuestsNoShows(dtmStart, dtmEnd, salesRooms).ToList();
          return lstInOut;
        }
      });
    }
    #endregion

    #endregion

    #region Meal Tickets

    #region GetRptMealTickets

    /// <summary>
    /// Obtiene el reporte de Meal Tickets por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <param name="cancelled"></param>
    /// <param name="rateTypes"></param>
    /// <returns> List of RptMealTickets </returns>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static async Task<List<RptMealTickets>> GetRptMealTickets(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms, bool? cancelled, string rateTypes = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstMealTickets = dbContext.USP_OR_RptMealTickets(dtmStart, dtmEnd, salesRooms, cancelled, rateTypes).ToList();
          return lstMealTickets;
        }
      });
    }
    #endregion

    #region GetRptMealTicketsCost

    /// <summary>
    /// Obtiene el reporte de Meal Tickets Cost por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <param name="rateTypes"></param>
    /// <returns> List of RptMealTicketsCost </returns>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static async Task<List<RptMealTicketsCost>> GetRptMealTicketsCost(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms, string rateTypes = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstMealTicketsCost = dbContext.USP_OR_RptMealTicketsCost(dtmStart, dtmEnd, salesRooms, rateTypes).ToList();
          var mealTicketTypes = (from mtt in dbContext.MealTicketTypes
                                 join mtc in lstMealTicketsCost.Select(c => c.Type).Distinct() on mtt.myID equals mtc
                                 select mtt).ToList();
          lstMealTicketsCost.ForEach(c => c.Type = mealTicketTypes.First(t => t.myID == c.Type).myN);


          return lstMealTicketsCost;
        }
      });
    }
    #endregion

    #endregion

    #region Memberships

    #region GetRptMemberships
    /// <summary>
    /// Obtiene el reporte de MemberShips por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <param name="leadSources"> Filtro de Lead Sources</param>
    /// <returns> List of RptMemberships </returns>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static async Task<List<RptMemberships>> GetRptMemberships(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", string leadSources = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstMemberships = dbContext.USP_OR_RptMemberships(dtmStart, dtmEnd, salesRooms, leadSources).ToList();

          return lstMemberships;
        }
      });
    }
    #endregion

    #region GetRptMembershipsByAgencyMarket
    /// <summary>
    /// Obtiene el reporte de MemberShips por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <returns> List of RptMembershipsByAgencyMarket </returns>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static async Task<List<RptMembershipsByAgencyMarket>> GetRptMembershipsByAgencyMarket(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstMemberships = dbContext.USP_OR_RptMembershipsByAgencyMarket(dtmStart, dtmEnd, salesRooms).ToList();
          return lstMemberships;
        }
      });
    }
    #endregion

    #region GetRptMembershipsByHost
    /// <summary>
    /// Obtiene el reporte de MemberShips por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <param name="leadSources"> Filtro de Lead Sources</param>
    /// <returns> List of RptMembershipsByAgencyMarket </returns>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static async Task<List<RptMembershipsByHost>> GetRptMembershipsByHost(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", string leadSources = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstMemberships = dbContext.USP_OR_RptMembershipsByHost(dtmStart, dtmEnd, salesRooms, leadSources).ToList();
          return lstMemberships;
        }
      });
    }
    #endregion

    #endregion

    #region Production

    #region GetRptProductionBySalesRoom
    /// <summary>
    /// Obtiene el reporte Production By SalesRoom.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <param name="basedOnArrivals"> Filtro de Arrivals</param>
    /// <param name="considerQuinellas">Filtro de Quinellas</param>
    /// <returns> List of RptProductionBySalesRoom </returns>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created.
    /// </history>
    public static async Task<List<RptProductionBySalesRoom>> GetRptProductionBySalesRoom(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", bool considerQuinellas = false, bool basedOnArrivals = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.UPS_OR_RptProductionBySalesRoom_Timeout;
          var lstProductionBySr = dbContext.USP_OR_RptProductionBySalesRoom(dtmStart, dtmEnd, salesRooms, considerQuinellas, basedOnArrivals).ToList();
          return lstProductionBySr;
        }
      });
    }
    #endregion

    #region GetRptProductionBySalesRoomMarket
    /// <summary>
    /// Obtiene el reporte Production By SalesRoom & Market.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <param name="basedOnArrivals"> Filtro de Arrivals</param>
    /// <param name="considerQuinellas">Filtro de Quinellas</param>
    /// <returns> List of RptProductionBySalesRoomMarket </returns>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created.
    /// </history>
    public static async Task<List<RptProductionBySalesRoomMarket>> GetRptProductionBySalesRoomMarket(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", bool considerQuinellas = false, bool basedOnArrivals = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.UPS_OR_RptProductionBySalesRoomMarket_Timeout;
          var lstProductionBySrm = dbContext.USP_OR_RptProductionBySalesRoomMarket(dtmStart, dtmEnd, salesRooms, considerQuinellas, basedOnArrivals).ToList();
          return lstProductionBySrm;
        }
      });
    }
    #endregion

    #region GetRptProductionBySalesRoomMarketSubMarket
    /// <summary>
    /// Obtiene el reporte Production By SalesRoom,Market & Submarket.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <param name="basedOnArrivals"> Filtro de Arrivals</param>
    /// <param name="considerQuinellas">Filtro de Quinellas</param>
    /// <returns> List of RptProductionBySalesRoomProgramMarketSubmarket </returns>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created.
    /// </history>
    public static async Task<List<RptProductionBySalesRoomProgramMarketSubmarket>> GetRptProductionBySalesRoomMarketSubMarket(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", bool considerQuinellas = false, bool basedOnArrivals = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.UPS_OR_RptProductionBySalesRoomMarket_Timeout;
          var lstProductionBySrmSm = dbContext.USP_OR_RptProductionBySalesRoomProgramMarketSubmarket(dtmStart, dtmEnd, salesRooms, considerQuinellas, basedOnArrivals).ToList();
          return lstProductionBySrmSm;
        }
      });
    }
    #endregion

    #region GetRptProductionByShowProgram
    /// <summary>
    /// Obtiene el reporte Production By Show & Program.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <param name="basedOnArrivals"> Filtro de Arrivals</param>
    /// <param name="considerQuinellas">Filtro de Quinellas</param>
    /// <returns> List of RptProductionByShowProgram </returns>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created.
    /// </history>
    public static async Task<List<RptProductionByShowProgram>> GetRptProductionByShowProgram(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", bool considerQuinellas = false, bool basedOnArrivals = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.UPS_OR_RptProductionBySalesRoomMarket_Timeout;
          var lstProductionByShowP = dbContext.USP_OR_RptProductionByShowProgram(dtmStart, dtmEnd, salesRooms, considerQuinellas, basedOnArrivals).ToList();
          return lstProductionByShowP;
        }
      });
    }
    #endregion

    #region GetRptProductionByShowProgramProgram
    /// <summary>
    /// Obtiene el reporte Production By Show,Program & Program.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <param name="basedOnArrivals"> Filtro de Arrivals</param>
    /// <param name="considerQuinellas">Filtro de Quinellas</param>
    /// <returns> List of RptProductionByShowProgramProgram </returns>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created.
    /// </history>
    public static async Task<List<RptProductionByShowProgramProgram>> GetRptProductionByShowProgramProgram(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", bool considerQuinellas = false, bool basedOnArrivals = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.UPS_OR_RptProductionBySalesRoomMarket_Timeout;
          var lstProductionByShowPp = dbContext.USP_OR_RptProductionByShowProgramProgram(dtmStart, dtmEnd, salesRooms, considerQuinellas, basedOnArrivals).ToList();
          return lstProductionByShowPp;
        }
      });
    }
    #endregion

    #endregion

    #region Salesmen

    #region GetRptCloserStatistics
    /// <summary>
    /// Obtiene el reporte Production By SalesRoom.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <returns> List of RptCloserStatistics </returns>
    /// <history>
    /// [edgrodriguez] 12/May/2016 Created.
    /// </history>
    public static async Task<List<RptCloserStatistics>> GetRptCloserStatistics(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstCloserStatistics = dbContext.USP_IM_RptCloserStatistics(dtmStart, dtmEnd, salesRooms).ToList();
          return lstCloserStatistics;
        }
      });
    }
    #endregion

    #region GetRptLinerStatistics
    /// <summary>
    /// Obtiene el reporte Production By SalesRoom.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <returns> List of RptCloserStatistics </returns>
    /// <history>
    /// [edgrodriguez] 12/May/2016 Created.
    /// </history>
    public static async Task<List<RptLinerStatistics>> GetRptLinerStatistics(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstLinerStatistics = dbContext.USP_IM_RptLinerStatistics(dtmStart, dtmEnd, salesRooms).ToList();
          return lstLinerStatistics;
        }
      });
    }
    #endregion

    #region GetRptWeeklyMonthlyHostess
    /// <summary>
    /// Obtiene el reporte Weekly & Monthly Hostess By SalesRoom.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <returns> List of GetRptWeeklyMonthlyHostess</returns>
    /// <history>
    ///   [edgrodriguez] 06/May/2016 Created.
    /// </history>
    public static async Task<List<IEnumerable>> GetRptWeeklyMonthlyHostess(DateTime? dtmStart, string salesRooms = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstRptWeeklyMonthly = dbContext.USP_IM_RptWeeklyMonthlyHostess(dtmStart, dtmStart.Value.AddDays(6), salesRooms)
          .MultipleResults()
          .With<RptWeeklyMonthlyHostess_ByPR>()
          .With<RptWeeklyMonthlyHostess_ByTourTime>()
          .GetValues();
          return (!(lstRptWeeklyMonthly[0] as List<RptWeeklyMonthlyHostess_ByPR>).Any() && !(lstRptWeeklyMonthly[1] as List<RptWeeklyMonthlyHostess_ByTourTime>).Any()) ? new List<IEnumerable>() : lstRptWeeklyMonthly;
        }
      });
    }
    #endregion

    #endregion

    #region Taxis

    #region GetRptTaxisIn
    /// <summary>
    /// Obtiene el reporte Taxis In.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <history>
    ///   [edgrodriguez] 16/Abr/2016 Created.
    /// </history>
    public static async Task<List<RptTaxisIn>> GetRptTaxisIn(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstTaxisIn = dbContext.USP_OR_RptTaxisIn(dtmStart, dtmEnd, salesRooms).ToList();
          return lstTaxisIn;
        }
      });
    }
    #endregion

    #region GetRptTaxisOut
    /// <summary>
    /// Obtiene el reporte Taxis Out.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <history>
    ///   [edgrodriguez] 16/Abr/2016 Created.
    /// </history>
    public static async Task<List<RptTaxisOut>> GetRptTaxisOut(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lstTaxisOut = dbContext.USP_OR_RptTaxisOut(dtmStart, dtmEnd, salesRooms).ToList();
          return lstTaxisOut;
        }
      });
    }
    #endregion

    #endregion

    #endregion

    #region Processor Sales

    #region GetRptStatisticsByLocation
    /// <summary>
    /// Devuelve un listado de Statics by Location
    /// </summary>
    /// <param name="dtStart"></param>
    /// <param name="dtEnd"></param>
    /// <param name="salesRoom"></param>
    /// <history>
    /// [ecanul] 07/05/2016 Created
    /// [ecanul] 06/06/2016 Modified Implementado asincronia
    /// [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public async static Task<List<RptStatisticsByLocation>> GetRptStatisticsByLocation(DateTime dtStart, DateTime dtEnd,
      IEnumerable<string> salesRoom)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_RptStatsByLocation(dtStart, dtEnd, string.Join(",", salesRoom)).ToList();
        }
      });
    }
    #endregion

    #region StatsBySalesRoomLocation
    /// <summary>
    /// Deviuelve un listado de Statics by Sales Room Location 
    /// </summary>
    /// <param name="dtStart">Fecha de inicio</param>
    /// <param name="dtEnd">Fecha fin</param>
    /// <param name="salesRoom">Listado de Sales Room para crear el reporte</param>
    /// <history>
    /// [ecanul] 06/05/2016 Created
    /// [ecanul] 06/06/2016 Modified Implementado asincronia
    /// [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public async static Task<List<RptStatisticsBySalesRoomLocation>> GetRptStatisticsBySalesRoomLocation(DateTime dtStart,
      DateTime dtEnd, IEnumerable<string> salesRoom)
    {
      return await Task.Run(() =>
      {
        using (var dbcontext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbcontext.USP_OR_RptStatsBySalesRoomLocation(dtStart, dtEnd, string.Join(",", salesRoom)).ToList();
        }
      });
    }

    #endregion

    #region GetRptStaticsByLocationMonthly
    /// <summary>
    /// Devuelve un listado de StaticsByLocationMonthly
    /// </summary>
    /// <param name="dtStart">Fecha Inicio</param>
    /// <param name="dtEnd">Fecha Fin </param>
    /// <param name="salesRoom">Sales Room</param>
    /// <history>
    /// [ecanul] 10/05/2016 Created
    /// [ecanul] 06/06/2016 Modified Implementado asincronia
    /// [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public async static Task<List<RptStatisticsByLocationMonthly>> GetRptStaticsByLocationMonthly(DateTime dtStart, DateTime dtEnd,
      IEnumerable<string> salesRoom)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_RptStatsByLocationMonthly(dtStart, dtEnd, string.Join(",", salesRoom)).ToList();
        }
      });
    }
    #endregion

    #region GetRptSalesByLocationMonthly
    /// <summary>
    /// Devuelve un listado de SalesByLocationMonthly
    /// </summary>
    /// <param name="dtStart">Fecha Inicio</param>
    /// <param name="dtEnd">Fecha Fin </param>
    /// <param name="salesRoom">Sales Room</param>
    /// <history>
    /// [ecanul] 10/05/2016 Created
    /// [ecanul] 06/06/2016 Modified Implementado asincronia
    /// [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public async static Task<List<RptSalesByLocationMonthly>> GetRptSalesByLocationMonthly(DateTime dtStart, DateTime dtEnd,
      IEnumerable<string> salesRoom)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_RptSalesByLocationMonthly(dtStart, dtEnd, string.Join(",", salesRoom)).ToList();
        }
      });
    }
    #endregion

    #region GetRptDailySalesHeader
    /// <summary>
    /// Devuelve un listado de RptDailySalesDetail
    /// </summary>
    /// <param name="dtStart">Fecha Inicio</param>
    /// <param name="dtEnd">Fecha Fin </param>
    /// <param name="salesRoom">Sales Room</param>
    /// <history>
    /// [ecanul] 16/05/2016 Created
    /// [ecanul] 06/06/2016 Modified Implementado asincronia
    /// [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public async static Task<List<RptDailySalesHeader>> GetRptDailySalesHeader(DateTime dtStart, DateTime dtEnd, IEnumerable<string> salesRoom)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_RptDailySalesHeader(dtStart, dtEnd, string.Join(",", salesRoom)).ToList();
        }
      });
    }
    #endregion

    #region GetRptDailySalesDetail
    /// <summary>
    /// Devuelve un listado de RptDailySalesDetail
    /// </summary>
    /// <param name="dtStart">Fecha Inicio</param>
    /// <param name="dtEnd">Fecha Fin </param>
    /// <param name="salesRoom">Sales Room</param>
    /// <history>
    /// [ecanul] 11/05/2016 Created
    /// [ecanul] 06/06/2016 Modified Implementado asincronia
    /// [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public async static Task<List<RptDailySalesDetail>> GetRptDailySalesDetail(DateTime dtStart, DateTime dtEnd,
      IEnumerable<string> salesRoom)
    {
     return await Task.Run(() =>
     {
       using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
       {
         dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptDailySalesDetail_Timeout;
         return dbContext.USP_OR_RptDailySalesDetail(dtStart, dtEnd, string.Join(",", salesRoom)).ToList();
       }
     });
    }
    #endregion

    #region GetRptConcentrateDailySales
    /// <summary>
    /// Devuelve un listado de GetRptConcentrateDailySales
    /// </summary>
    /// <param name="dtStart">Fecha Inicio</param>
    /// <param name="dtEnd">Fecha Fin </param>
    /// <param name="salesRoom">Sales Room</param>
    /// <history>
    /// [ecanul] 11/05/2016 Created
    /// [ecanul] 06/06/2016 Modified Implementado asincronia
    /// [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public async static Task<List<RptConcentrateDailySales>> GetRptConcentrateDailySales(DateTime dtStart, DateTime dtEnd,
      IEnumerable<string> salesRoom)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.USP_IM_RptConcentrateDailySales_TimeOut;
          return dbContext.USP_IM_RptConcentrateDailySales(dtStart, dtEnd, string.Join(",", salesRoom)).ToList();
        }
      });
    }
    #endregion

    #region GetRptManiest
    /// <summary>
    /// Devuelve un listado de GetRptManifest
    /// </summary>
    /// <param name="dtStart">Fecha Inicio</param>
    /// <param name="dtEnd">Fecha Fin </param>
    /// <param name="salesRoom">Sales Room</param>
    /// <param name="salesmanID">ID del salesman</param>
    /// <param name="roles">Listado de roles de los vendedores</param>
    /// <param name="segments">segmentos</param>
    /// <param name="programs">programas</param>
    /// <param name="bySegmentsCategories">por segments categories</param>
    /// <param name="byLocationCategories">por location categories</param>
    /// <history>
    ///   [ecanul] 07/06/2016 Created
    ///   [ecanul] 12/07/2016 Modified. Cambie el parametro salesmanRoles (string) por  roles (EnumRole)
    ///   [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public async static Task<List<RptManifest>> GetRptManiest(DateTime dtStart, DateTime dtEnd, IEnumerable<string> salesRoom, string salesmanID = "ALL",
      List<EnumRole> roles = null, string segments = "ALL", string programs = "ALL", bool bySegmentsCategories = false, bool byLocationCategories = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          string r = roles == null ? "ALL" : roles.EnumListToString();
          return dbContext.USP_IM_RptManifest(dtStart, dtEnd, string.Join(",", salesRoom), salesmanID, roles ==  null ? "All" : roles.EnumListToString() , segments, programs, bySegmentsCategories, byLocationCategories).ToList();
        }
      });
    }
    #endregion

    #region GetRptFTMInOutHouse
    /// <summary>
    /// Devuelve un listado de RptFTMIn&OutHouse
    /// </summary>
    /// <param name="dtStart">Fecha Inicio</param>
    /// <param name="dtEnd">Fecha Fin </param>
    /// <param name="salesRoom">Sales Room</param>
    /// <param name="salesmanID">ID del salesman</param>
    /// <history>
    ///   [ecanul] 02/07/2016 Created
    ///   [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public async static Task<List<RptFTMInOutHouse>> GetRptFTMInOutHouse(DateTime dtStart, DateTime dtEnd, IEnumerable<string> salesRooms, string salesmanID = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_IM_RptFTMInOutHouse(dtStart, dtEnd, string.Join(",", salesRooms), salesmanID).ToList();
        }
      });
    }
    #endregion

    #region GetStatisticsBySegments
    /// <summary>
    /// Devuelve los datos para el reporte de estadisticas por segmentos
    /// </summary>
    /// <param name="ltsDtmStart">Fechas Inicio</param>
    /// <param name="ltsDtmEnd">Fechas Fin</param>
    /// <param name="salesRooms">Clave de las salas (El primero es la sala principal)</param>
    /// <param name="salesmanID">Clave de un vendedor</param>
    /// <param name="bySegmentsCategories">Indica si es por categorias de segmentos</param>
    /// <param name="own">Indica si trabajo solo</param>
    /// <param name="includeAllSalesmen">si se desea que esten todos los vendedores de la sala</param>
    /// <returns><list type="RptStatisticsBySegments"></list></returns>
    /// <history>
    ///   [aalcocer] 04/07/2016 Created
    ///   [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public static async Task<List<RptStatisticsBySegments>> GetStatisticsBySegments(IEnumerable<DateTime> ltsDtmStart, IEnumerable<DateTime> ltsDtmEnd, 
      IEnumerable<string> salesRooms, string salesmanID = "ALL", bool bySegmentsCategories=false, bool own = false, bool includeAllSalesmen= false )
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.USP_IM_RptStatisticsBySegments_Timeout;
          return dbContext.USP_IM_RptStatisticsBySegments(string.Join(",", ltsDtmStart.Select(x => $"{x:yyyyMMdd}")), string.Join(",", ltsDtmEnd.Select(x=> $"{x:yyyyMMdd}")), 
            string.Join(",", salesRooms), salesmanID, bySegmentsCategories, own, includeAllSalesmen).ToList();
        }
      });
    }
    #endregion GetStatsBySegment

    #region GetStatisticsByCloser
    /// <summary>
    /// Devuelve los datos para el reporte de estadisticas por Closer
    /// </summary>
    /// <param name="dtStart">Fecha Inicio</param>
    /// <param name="dtEnd">Fecha Fin</param>
    /// <param name="salesRoom">Clave de las sala</param>
    /// <param name="salesmanID">Clave de un vendedor</param>
    /// <param name="segments">Claves de segmentos</param>
    /// <param name="program">Programs</param>
    /// <param name="includeAllSalesmen">si se desea que esten todos los vendedores de la sala</param>
    /// <returns><list type="RptStatisticsByCloser"></list></returns>
    /// <history>
    ///   [aalcocer] 13/07/2016 Created
    ///   [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public static async Task<List<RptStatisticsByCloser>> GetStatisticsByCloser(DateTime dtStart, DateTime dtEnd, string salesRoom,
      string salesmanID = "ALL", IEnumerable<string> segments = null, EnumProgram program = EnumProgram.All, bool includeAllSalesmen = false)
    {
      if (segments == null || !segments.Any()) segments = new List<string> { "ALL" };
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.USP_IM_RptStatisticsByCloser_Timeout;
          return dbContext.USP_IM_RptStatisticsByCloser(dtStart, dtEnd, salesRoom, salesmanID, string.Join(",", segments),
            EnumToListHelper.GetEnumDescription(program), includeAllSalesmen, false).ToList();
        }
      });
    }
    #endregion GetStatisticsByCloser

    #region GetStatisticsByExitCloser
    /// <summary>
    /// Devuelve los datos para el reporte de estadisticas por Exit Closer
    /// </summary>
    /// <param name="dtStart">Fecha Inicio</param>
    /// <param name="dtEnd">Fecha Fin</param>
    /// <param name="salesRoom">Clave de las sala</param>
    /// <param name="salesmanID">Clave de un vendedor</param>
    /// <param name="segments">Claves de segmentos</param>
    /// <param name="program">Programs</param>
    /// <param name="includeAllSalesmen">si se desea que esten todos los vendedores de la sala</param>
    /// <returns><list type="RptStatisticsByExitCloser"></list></returns>
    /// <history>
    ///   [aalcocer] 18/07/2016 Created
    ///   [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public static async Task<List<RptStatisticsByExitCloser>> GetStatisticsByExitCloser(DateTime dtStart, DateTime dtEnd, string salesRoom,
      string salesmanID = "ALL", IEnumerable<string> segments = null, EnumProgram program = EnumProgram.All, bool includeAllSalesmen = false)
    {
      if (segments == null || !segments.Any()) segments = new List<string> { "ALL" };
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.USP_IM_RptStatisticsByExitCloser_Timeout;
          return dbContext.USP_IM_RptStatisticsByExitCloser(dtStart, dtEnd, salesRoom, salesmanID, string.Join(",", segments),
            EnumToListHelper.GetEnumDescription(program), includeAllSalesmen).ToList();
        }
      });
    }
    #endregion GetStatisticsByExitCloser

    #region GetRptSelfGen&SelfGenTeam
    /// <summary>
    /// Devuelve un listado con el reporte SelfGen&SelfGenTeam
    /// </summary>
    /// <param name="dtStart">Fecha Inicial</param>
    /// <param name="dtEnd">Fecha Fin</param>
    /// <param name="salesRoom">Sala(s) de ventas</param>
    /// <param name="salesmanID">ID del personal</param>
    /// <history>
    ///   [ecanul] 25/07/2016 Created
    ///   [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public static async Task<List<RptSelfGenTeam>> GetRptSelfGenAndSelfGenTeam (DateTime dtStart, DateTime dtEnd, IEnumerable<string> salesRoom, string salesmanID = "ALL")
    {
      return await Task.Run(()=>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_IM_RptSelfGenTeam(dtStart, dtEnd, string.Join(",", salesRoom), salesmanID).ToList();
        }
      });
    }
    #endregion

    #region GetStatisticsByFTB

    /// <summary>
    /// Devuelve los datos para el reporte de estadisticas por FTB
    /// </summary>
    /// <param name="dtStart">Fecha Inicio</param>
    /// <param name="dtEnd">Fecha Fin</param>
    /// <param name="salesRoom">Clave de las sala</param>
    /// <param name="salesmanID">Clave de un vendedor</param>
    /// <param name="segments">Claves de segmentos</param>
    /// <param name="program">Programs</param> 
    /// <param name="byLocations">Indica si es por locaciones</param>
    /// <param name="byLocationsCategories">Indica si es por categorias de locaciones</param>
    /// <param name="includeAllSalesmen">si se desea que esten todos los vendedores de la sala</param>
    /// <returns><list type="RptStatisticsByFTB"></list></returns>
    /// <history>
    ///   [michan] 21/07/2016 Created
    ///   [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public static async Task<List<RptStatisticsByFTB>> GetStatisticsByFTB(DateTime dtStart, DateTime dtEnd, string salesRoom,
      string salesmanID = "ALL", IEnumerable<string> segments = null, EnumProgram program = EnumProgram.All,
      bool byLocations= false, bool byLocationsCategories=false , bool includeAllSalesmen = false)
    {
      if (segments == null || !segments.Any()) segments = new List<string> { "ALL" };
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.USP_IM_RptStatisticsByFTB_Timeout;
          return dbContext.USP_IM_RptStatisticsByFTB(dtStart, dtEnd, salesRoom, salesmanID, string.Join(",", segments),
          EnumToListHelper.GetEnumDescription(program), false, includeAllSalesmen).ToList();
        }
      });
    }
    #endregion GetStatisticsByFTB

    #endregion Processor Sales


    #region GetRptUplist
    /// <summary>
    /// Devuelve un listado de RptUpList
    /// </summary>
    /// <param name="dtStart"> Fecha Inicial</param>
    /// <param name="salesRoom"> Salas de Venta</param>
    /// <param name="uplistType"> Tipo de Uplist
    /// 0. Up List Start
    /// 1. Up List End</param>
    /// <history>
    /// [edgrodriguez] 29/06/2016
    /// </history>
    public async static Task<List<RptUpList>> GetRptUplist(DateTime dtStart, string salesRoom = "ALL", int uplistType = 0 )
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_IM_RptUpList(dtStart, salesRoom, uplistType).ToList();
        }
      });
    }
    #endregion

  }
}
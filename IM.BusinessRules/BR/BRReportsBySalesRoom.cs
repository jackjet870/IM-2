﻿using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRReportsBySalesRoom
  {

    #region Bookings

    #region GetRptBookingsBySalesRoomProgramTime
    /// <summary>
    /// Obtiene el reporte de Bookings por Sala de venta y Programa
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="SalesRooms">Salas de Venta</param>
    /// <returns> List<RptBookingsBySalesRoomProgramTime> </returns>
    /// <history>
    /// [edgrodriguez] 12/Mar/2016 Created
    /// </history>
    public static List<RptBookingsBySalesRoomProgramTime> GetRptBookingsBySalesRoomProgramTime(DateTime dtmStart, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptBookingsBySalesRoomProgramTime(dtmStart, salesRooms).ToList();
      }
    }
    #endregion

    #region GetRptBookingsBySalesRoomProgramLeadSourceTime
    /// <summary>
    /// Obtiene el reporte de Bookings por Sala de Venta,Programa y Locaciones.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="SalesRooms">Salas de Venta</param>
    /// <returns> List<RptBookingsBySalesRoomProgramLeadSourceTime> </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static List<RptBookingsBySalesRoomProgramLeadSourceTime> GetRptBookingsBySalesRoomProgramLeadSourceTime(DateTime dtmStart, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptBookingsBySalesRoomProgramLeadSourceTime(dtmStart, salesRooms).ToList();
      }
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
    /// <returns> List<RptCxC> </returns>
    /// <history>
    /// [edgrodriguez] 22/Mar/2016 Created
    /// </history>
    public static List<RptCxC> GetRptCxC(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptCxC(dtmStart, dtmEnd, salesRooms).ToList();
      }
    }
    #endregion

    #region GetRptCxCDeposits
    /// <summary>
    /// Obtiene el reporte de CxCDeposits por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptCxCDeposits> </returns>
    /// <history>
    /// [edgrodriguez] 22/Mar/2016 Created
    /// </history>
    public static List<object> GetRptCxCDeposits(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstDeposits = dbContext.USP_OR_RptCxCDeposits(dtmStart, dtmEnd, salesRooms).ToList();
        var lstCurrencies = (from cxcDep in lstDeposits
                             join curr in dbContext.Currencies on cxcDep.grcuCxCPRDeposit equals curr.cuID
                             select curr).Distinct().ToList();

        return new List<object> { lstDeposits, lstCurrencies };
      }
    }
    #endregion

    #region GetRptCxCGifts
    /// <summary>
    /// Obtiene el reporte de CxCGifts por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptCxCGifts> </returns>
    /// <history>
    /// [edgrodriguez] 28/Mar/2016 Created
    /// </history>
    public static List<object> GetRptCxCGifts(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Obtenemos los recibos de regalo.
        var lstGiftReceipt = dbContext.USP_OR_RptCxCGifts(dtmStart, dtmEnd, salesRooms).ToList();
        var gifts = (from gRcpt in lstGiftReceipt
                     join gift in dbContext.Gifts on gRcpt.Gift equals gift.giID
                     select gift).ToList();

        return new List<object> { lstGiftReceipt, gifts };
      }
    }
    #endregion

    #region GetRptCxCNotAuthorized
    /// <summary>
    /// Obtiene el reporte de CxCNotAuthorized por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptCxCNotAuthorized> </returns>
    /// <history>
    /// [edgrodriguez] 01/Abr/2016 Created
    /// </history>
    public static List<RptCxCNotAuthorized> GetRptCxCNotAuthorized(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Obtenemos los recibos de regalo.
        return dbContext.USP_OR_RptCxCNotAuthorized(dtmStart, dtmEnd, salesRooms).ToList();
      }
    }
    #endregion

    #region GetRptCxCPayments
    /// <summary>
    /// Obtiene el reporte de CxC Payments por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptCxCPayments> </returns>
    /// <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static List<RptCxCPayments> GetRptCxCPayments(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Obtenemos los recibos de regalo.
        return dbContext.USP_OR_RptCxCPayments(dtmStart, dtmEnd, salesRooms).ToList();
      }
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
    /// <returns> List<object> </returns>
    /// <history>
    /// [edgrodriguez] 01/Abr/2016 Created
    /// </history>
    public static List<object> GetRptDeposits(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Obtenemos los recibos de regalo.
        var giftReceipts = dbContext.USP_OR_RptDeposits(dtmStart, dtmEnd, salesRooms).ToList();
        var currencies = (from gRcpt in giftReceipts
                          join curr in dbContext.Currencies on gRcpt.grcu equals curr.cuID
                          select curr).Distinct().ToList();

        var paymentsType = (from gRcpt in giftReceipts
                            join payTyp in dbContext.PaymentTypes on gRcpt.grpt equals payTyp.ptID
                            select payTyp).Distinct().ToList();

        return new List<object> { giftReceipts, currencies, paymentsType };
      }
    }
    #endregion

    #region GetRptDepositsBurned
    /// <summary>
    /// Obtiene el reporte de Deposits Burned por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<object> </returns>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static List<object> GetRptDepositsBurned(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      List<string> salesRoom = salesRooms.Split(',').ToList();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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

        return new List<object> { giftReceipts, currencies, paymentsType, saleReceipts };
      }
    }
    #endregion

    #region GetRptDepositsBurnedByResort
    /// <summary>
    /// Obtiene el reporte de CxCNotAuthorized por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<object> </returns>
    /// <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static List<object> GetRptDepositsBurnedByResort(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      List<string> salesRoom = salesRooms.Split(',').ToList();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
                              gRctp.grID,
                              gSale
                            })
                            .GroupBy(c => c.grID)
                            .ToDictionary(c => c.Key, c => c.Select(s => s.gSale).ToList());

        return new List<object> { giftReceipts, currencies, paymentsType, saleReceipts };
      }
    }
    #endregion

    #region GetRptPaidDeposits
    /// <summary>
    /// Obtiene el reporte de Paid Deposits por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<object> </returns>
    /// <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static List<object> GetRptPaidDeposits(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      List<string> salesRoom = salesRooms.Split(',').ToList();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Obtenemos los recibos de regalo.
        var giftReceipts = dbContext.USP_OR_RptDepositsPaid(dtmStart, dtmEnd, "ALL", salesRooms).ToList();

        var currencies = (from gRcpt in giftReceipts
                          join curr in dbContext.Currencies on gRcpt.grcu equals curr.cuID
                          select curr).Distinct().ToList();

        var paymentsType = (from gRcpt in giftReceipts
                            join payTyp in dbContext.PaymentTypes on gRcpt.grpt equals payTyp.ptID
                            select payTyp).Distinct().ToList();

        return new List<object> { giftReceipts, currencies, paymentsType };
      }
    }
    #endregion

    #endregion

    #region Gifts

    #region GetRptGiftsByCategory

    public static List<RptGiftsByCategory> GetRptGiftsByCategory(DateTime? dtmStart, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Obtenemos los recibos de regalo.
        var DailyGiftByCat = dbContext.USP_OR_RptGiftsByCategory(dtmStart, dtmStart.Value.AddDays(6).Date, salesRooms).ToList();
        return DailyGiftByCat;
      }
    }

    #endregion

    #region GetRptGiftsByCategoryProgram

    public static List<RptGiftsByCategoryProgram> GetRptGiftsByCategoryProgram(DateTime? dtmStart, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Obtenemos los recibos de regalo.
        var DailyGiftByCatP = dbContext.USP_OR_RptGiftsByCategoryProgram(dtmStart, dtmStart.Value.AddDays(6).Date, salesRooms).ToList();
        return DailyGiftByCatP;
      }
    }

    #endregion

    #region GetRptDailyGiftSimple
    /// <summary>
    /// Obtiene el reporte de Daily Gift Simple por Sala de ventas y una fecha en especifico.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptDailyGiftSimple> </returns>
    /// <history>
    /// [edgrodriguez] 07/Abr/2016 Created
    /// </history>
    public static List<RptDailyGiftSimple> GetRptDailyGiftSimple(DateTime? dtmStart, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Obtenemos los recibos de regalo.
        var DailyGiftSimple = dbContext.USP_IM_RptDailyGiftSimple(dtmStart, salesRooms).ToList();
        return DailyGiftSimple;
      }
    }
    #endregion

    #region GetRptWeeklyGiftsItemsSimple
    /// <summary>
    /// Obtiene el reporte de Weekly Gift Simple por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptWeeklyGiftsItemsSimple> </returns>
    /// <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static List<RptWeeklyGiftsItemsSimple> GetRptWeeklyGiftsItemsSimple(DateTime? dtmStart, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Obtenemos los recibos de regalo.
        var lstweeklyGiftSimple = dbContext.USP_IM_RptWeeklyGiftsItemsSimple(dtmStart, dtmStart.Value.AddDays(6), salesRooms).ToList();
        return lstweeklyGiftSimple;
      }
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
    /// <returns> List<RptGuestCeco> </returns>
    /// <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static List<RptGuestCeco> GetRptGuestCeco(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstGuestCeco = dbContext.USP_OR_RptGuestCeco(dtmStart, dtmEnd, salesRooms).ToList();
        return lstGuestCeco;
      }
    }
    #endregion

    #region GetRptGuestNoBuyers
    /// <summary>
    /// Obtiene el reporte de Guest No Buyers por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptGuestsNoBuyers> </returns>
    /// <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static List<RptGuestsNoBuyers> GetRptGuestNoBuyers(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstGuestNoBuyers = dbContext.USP_OR_RptGuestsNoBuyers(dtmStart, dtmEnd, salesRooms).ToList();
        return lstGuestNoBuyers;
      }
    }
    #endregion

    #region GetRptInOut
    /// <summary>
    /// Obtiene el reporte de In & Out por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptInOut> </returns>
    /// <history>
    /// [edgrodriguez] 09/Abr/2016 Created
    /// </history>
    public static List<RptInOut> GetRptInOut(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstInOut = dbContext.USP_OR_RptInOut(dtmStart, dtmEnd, salesRooms).ToList();
        return lstInOut;
      }
    }
    #endregion

    #region GetRptGuestNoShows
    /// <summary>
    /// Obtiene el reporte de Guest No Show por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptGuestsNoShows> </returns>
    /// <history>
    /// [edgrodriguez] 09/Abr/2016 Created
    /// </history>
    public static List<RptGuestsNoShows> GetRptGuestNoShows(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstInOut = dbContext.USP_OR_RptGuestsNoShows(dtmStart, dtmEnd, salesRooms).ToList();
        return lstInOut;
      }
    }
    #endregion

    #endregion

    #region Meal Tickets

    #region GetRptMealTickets
    /// <summary>
    /// Obtiene el reporte de Meal Tickets por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptMealTickets> </returns>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static List<RptMealTickets> GetRptMealTickets(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms, bool? cancelled, string rateTypes)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstMealTickets = dbContext.USP_OR_RptMealTickets(dtmStart, dtmEnd, salesRooms, cancelled, rateTypes).ToList();
        return lstMealTickets;
      }
    }
    #endregion

    #region GetRptMealTicketsCost
    /// <summary>
    /// Obtiene el reporte de Meal Tickets Cost por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptMealTicketsCost> </returns>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static List<RptMealTicketsCost> GetRptMealTicketsCost(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms, string rateTypes)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstMealTicketsCost = dbContext.USP_OR_RptMealTicketsCost(dtmStart, dtmEnd, salesRooms, rateTypes).ToList();
        var MealTicketTypes = (from mtt in dbContext.MealTicketTypes
                               join mtc in lstMealTicketsCost.Select(c => c.Type).Distinct() on mtt.myID equals mtc
                               select mtt).ToList();
        lstMealTicketsCost.ForEach(c => c.Type = MealTicketTypes.First(t => t.myID == c.Type).myN);


        return lstMealTicketsCost;
      }
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
    /// <returns> List<RptMemberships> </returns>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static List<RptMemberships> GetRptMemberships(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms, string leadSources = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstMemberships = dbContext.USP_OR_RptMemberships(dtmStart, dtmEnd, salesRooms, leadSources).ToList();

        return lstMemberships;
      }
    }
    #endregion

    #region GetRptMembershipsByAgencyMarket
    /// <summary>
    /// Obtiene el reporte de MemberShips por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms"> Filtro de Sales Room</param>
    /// <param name="leadSources"> Filtro de Lead Sources</param>
    /// <returns> List<RptMembershipsByAgencyMarket> </returns>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static List<RptMembershipsByAgencyMarket> GetRptMembershipsByAgencyMarket(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstMemberships = dbContext.USP_OR_RptMembershipsByAgencyMarket(dtmStart, dtmEnd, salesRooms).ToList();
        return lstMemberships;
      }
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
    /// <returns> List<RptMembershipsByAgencyMarket> </returns>
    /// <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static List<RptMembershipsByHost> GetRptMembershipsByHost(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", string leadSources = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstMemberships = dbContext.USP_OR_RptMembershipsByHost(dtmStart, dtmEnd, salesRooms, leadSources).ToList();
        return lstMemberships;
      }
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
    /// <returns> List<RptProductionBySalesRoom> </returns>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created.
    /// </history>
    public static List<RptProductionBySalesRoom> GetRptProductionBySalesRoom(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", bool considerQuinellas = false, bool basedOnArrivals = false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Properties.Settings.Default.UPS_OR_RptProductionBySalesRoom;
        var lstProductionBySR = dbContext.USP_OR_RptProductionBySalesRoom(dtmStart, dtmEnd, salesRooms, considerQuinellas, basedOnArrivals).ToList();
        return lstProductionBySR;
      }
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
    /// <returns> List<RptProductionBySalesRoomMarket> </returns>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created.
    /// </history>
    public static List<RptProductionBySalesRoomMarket> GetRptProductionBySalesRoomMarket(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", bool considerQuinellas = false, bool basedOnArrivals = false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Properties.Settings.Default.UPS_OR_RptProductionBySalesRoomMarket;
        var lstProductionBySRM = dbContext.USP_OR_RptProductionBySalesRoomMarket(dtmStart, dtmEnd, salesRooms, considerQuinellas, basedOnArrivals).ToList();
        return lstProductionBySRM;
      }
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
    /// <returns> List<RptProductionBySalesRoomProgramMarketSubmarket> </returns>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created.
    /// </history>
    public static List<RptProductionBySalesRoomProgramMarketSubmarket> GetRptProductionBySalesRoomMarketSubMarket(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", bool considerQuinellas = false, bool basedOnArrivals = false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Properties.Settings.Default.UPS_OR_RptProductionBySalesRoomMarket;
        var lstProductionBySRMSm = dbContext.USP_OR_RptProductionBySalesRoomProgramMarketSubmarket(dtmStart, dtmEnd, salesRooms, considerQuinellas, basedOnArrivals).ToList();
        return lstProductionBySRMSm;
      }
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
    /// <returns> List<RptProductionByShowProgram> </returns>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created.
    /// </history>
    public static List<RptProductionByShowProgram> GetRptProductionByShowProgram(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", bool considerQuinellas = false, bool basedOnArrivals = false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Properties.Settings.Default.UPS_OR_RptProductionBySalesRoomMarket;
        var lstProductionByShowP = dbContext.USP_OR_RptProductionByShowProgram(dtmStart, dtmEnd, salesRooms, considerQuinellas, basedOnArrivals).ToList();
        return lstProductionByShowP;
      }
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
    /// <returns> List<RptProductionByShowProgramProgram> </returns>
    /// <history>
    /// [edgrodriguez] 15/Abr/2016 Created.
    /// </history>
    public static List<RptProductionByShowProgramProgram> GetRptProductionByShowProgramProgram(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms = "ALL", bool considerQuinellas = false, bool basedOnArrivals = false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Properties.Settings.Default.UPS_OR_RptProductionBySalesRoomMarket;
        var lstProductionByShowPP = dbContext.USP_OR_RptProductionByShowProgramProgram(dtmStart, dtmEnd, salesRooms, considerQuinellas, basedOnArrivals).ToList();
        return lstProductionByShowPP;
      }
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
    public static List<RptTaxisIn> GetRptTaxisIn(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstTaxisIn = dbContext.USP_OR_RptTaxisIn(dtmStart, dtmEnd, salesRooms).ToList();
        return lstTaxisIn;
      }
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
    public static List<RptTaxisOut> GetRptTaxisOut(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstTaxisOut = dbContext.USP_OR_RptTaxisOut(dtmStart, dtmEnd, salesRooms).ToList();
        return lstTaxisOut;
      }
    }
    #endregion

    #endregion
  }
}
using IM.BusinessRules.Properties;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public class BRReportsByLeadSource
  {
    #region GetRptCostByPR

    /// <summary>
    /// Devuelve los datos para el reporte de costo por PR
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <returns> <list type="RptCostByPR"></list></returns>
    /// <history>
    /// [aalcocer] 23/03/2016 Created
    /// </history>
    public static List<RptCostByPR> GetRptCostByPR(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptCostByPR(dtmStart, dtmEnd, string.Join(",", leadSources), considerQuinellas == EnumQuinellas.quQuinellas).ToList();
      }
    }

    #endregion GetRptCostByPR

    #region GetRptCostByPRWithDetailGifts

    /// <summary>
    /// Devuelve el PR con Detalle de Gifts
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="detailGifts">Indica si desea Detail Gifts</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <returns> <list type="RptCostByPRWithDetailGifts"></list></returns>
    /// <history>
    /// [aalcocer] 23/03/2016 Created
    /// </history>
    public static List<RptCostByPRWithDetailGifts> GetRptCostByPRWithDetailGifts(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumDetailGifts detailGifts = EnumDetailGifts.dgDetailGifts)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptCostByPRWithDetailGifts(dtmStart, dtmEnd, string.Join(",", leadSources), considerQuinellas == EnumQuinellas.quQuinellas, detailGifts == EnumDetailGifts.dgDetailGifts).OrderBy(c => c.PR).
          ThenBy(c => c.AverageCost.HasValue && c.AverageCost.Value != 0).ThenBy(c => c.TotalCost.HasValue && c.TotalCost.Value != 0).ToList();
      }
    }

    #endregion GetRptCostByPRWithDetailGifts

    #region GetRptFollowUpByAgencies

    /// <summary>
    /// Devuelve los datos para el reporte de seguimiento por agencia
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns> <list type="RptFollowUpByAgency"></list></returns>
    /// <history>
    /// [aalcocer] 29/03/2016 Created
    /// </history>
    public static List<RptFollowUpByAgency> GetRptFollowUpByAgencies(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptFollowUpByAgency_Timeout;

        return dbContext.USP_OR_RptFollowUpByAgency(dtmStart, dtmEnd, string.Join(",", leadSources), considerQuinellas == EnumQuinellas.quQuinellas, basedOnArrival == EnumBasedOnArrival.boaBasedOnArrival).ToList();
      }
    }

    #endregion GetRptFollowUpByAgencies

    #region GetRptFollowUpByPR

    /// <summary>
    /// Devuelve los datos para el reporte de seguimiento por PR
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptFollowUpByPR"></list></returns>
    /// <history>
    /// [aalcocer] 29/03/2016 Created
    /// </history>
    public static List<RptFollowUpByPR> GeRptFollowUpByPRs(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptFollowUpByPR_Timeout;
        return dbContext.USP_OR_RptFollowUpByPR(dtmStart, dtmEnd, string.Join(",", leadSources), considerQuinellas == EnumQuinellas.quQuinellas, basedOnArrival == EnumBasedOnArrival.boaBasedOnArrival).ToList();
      }
    }

    #endregion GetRptFollowUpByPR

    #region GetProductionByAgeInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por edad (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByAgeInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 04/04/2016 Created
    /// </history>
    public static List<RptProductionByAgeInhouse> GetProductionByAgeInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByAgeInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByAgeInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), considerQuinellas == EnumQuinellas.quQuinellas, basedOnArrival == EnumBasedOnArrival.boaBasedOnArrival).ToList();
      }
    }

    #endregion GetProductionByAgeInhouses

    #region GetProductionByAgeMarketOriginallyAvailableInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por edad, mercado y originalmente disponible (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByAgeMarketOriginallyAvailableInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 04/04/2016 Created
    /// </history>
    public static List<RptProductionByAgeMarketOriginallyAvailableInhouse> GetProductionByAgeMarketOriginallyAvailableInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByAgeMarketOriginallyAvailableInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByAgeMarketOriginallyAvailableInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), considerQuinellas == EnumQuinellas.quQuinellas, basedOnArrival == EnumBasedOnArrival.boaBasedOnArrival).ToList();
      }
    }

    #endregion GetProductionByAgeMarketOriginallyAvailableInhouses

    #region GetRptProductionByAgencyInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de producccion por agencia (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="considerNights"> Indica si se debe considerar el numero de noches</param>
    /// <param name="nightsFrom">Numero de noches desde</param>
    /// <param name="nightsTo">Numero de noches hasta</param>
    /// <param name="salesByMembershipType">Indica si se desean las ventas por tipo de membresia</param>
    /// <param name="onlyQuinellas">Indica si se desean solo las quinielas</param>
    /// <param name="external">Filtro de invitaciones externas
    /// 0. Sin filtro
    /// 1. Excluir invitaciones externas
    /// 2. Solo invitaciones externas
    /// </param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByAgencyInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 04/04/2016 Created
    /// </history>
    public static List<RptProductionByAgencyInhouse> GetRptProductionByAgencyInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas,
      bool considerNights = false, int nightsFrom = 0, int nightsTo = 0, EnumSalesByMemberShipType salesByMembershipType = EnumSalesByMemberShipType.sbmNoDetail,
      bool onlyQuinellas = false, EnumExternalInvitation external = EnumExternalInvitation.extExclude, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByAgencyInhouse_Timeout;
        return
          dbContext.USP_OR_RptProductionByAgencyInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), considerQuinellas == EnumQuinellas.quQuinellas,
          considerNights, nightsFrom, nightsTo, salesByMembershipType == EnumSalesByMemberShipType.sbmDetail, onlyQuinellas, (int)external, basedOnArrival == EnumBasedOnArrival.boaBasedOnArrival).ToList();
      }
    }

    #endregion GetRptProductionByAgencyInhouses

    #region GetRptProductionByContractAgencyInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por edad, mercado y originalmente disponible (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="markets">Claves de mercados</param>
    /// <param name="agencies">Claves de agencias</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByContractAgencyInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 04/04/2016 Created
    /// </history>
    public static List<RptProductionByContractAgencyInhouse> GetRptProductionByContractAgencyInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      IEnumerable<string> markets = null, IEnumerable<string> agencies = null, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      if (markets == null || !markets.Any())
        markets = new List<string>() { "ALL" };

      if (agencies == null || !agencies.Any())
        agencies = new List<string>() { "ALL" };

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptProductionByContractAgencyInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", markets), string.Join(",", agencies), considerQuinellas == EnumQuinellas.quQuinellas, basedOnArrival == EnumBasedOnArrival.boaBasedOnArrival).ToList();
      }
    }

    #endregion GetRptProductionByContractAgencyInhouses

    #region GetRptProductionByContractAgencyMarketOriginallyAvailableInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de producccion por contrato, agencia, mercado y originalmente disponible (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="markets">Claves de mercados</param>
    /// <param name="agencies">Claves de agencias</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByContractAgencyMarketOriginallyAvailableInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 04/04/2016 Created
    /// </history>
    public static List<RptProductionByContractAgencyMarketOriginallyAvailableInhouse> GetRptProductionByContractAgencyMarketOriginallyAvailableInhouses(DateTime dtmStart,
      DateTime dtmEnd, IEnumerable<string> leadSources, IEnumerable<string> markets = null, IEnumerable<string> agencies = null, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      if (markets == null || !markets.Any())
        markets = new List<string>() { "ALL" };

      if (agencies == null || !agencies.Any())
        agencies = new List<string>() { "ALL" };

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptProductionByContractAgencyMarketOriginallyAvailableInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", markets), string.Join(",", agencies), considerQuinellas == EnumQuinellas.quQuinellas, basedOnArrival == EnumBasedOnArrival.boaBasedOnArrival).ToList();
      }
    }

    #endregion GetRptProductionByContractAgencyMarketOriginallyAvailableInhouses

    #region GetRptDepositsPaymentByPR
    /// <summary>
    ///  Obtiene los datos para el reporte de pago de depositos a los PRs (Comisiones de PRs de Outhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de LeadSorces</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="paymentTypes">Claves de formas de pago</param>
    /// <param name="filterDeposit">
    /// Filtro de positos. 0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito (Flyers),
    /// 3. Con deposito y shows sin deposito (Deposits & Flyers Show) 
    /// </param>
    /// <returns></returns>
    public static List<object> GetRptDepositsPaymentByPR(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, string program, string paymentTypes, byte filterDeposit)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstDespositsPaymentByPR = dbContext.USP_OR_RptDepositsPaymentByPR(dtmStart, dtmEnd, leadSources, PRs, program, paymentTypes, filterDeposit).ToList();
        var guests = (from dpsitPayByPR in lstDespositsPaymentByPR
                      join gu in dbContext.Guests on dpsitPayByPR.PR equals gu.guPRInvit1
                      select gu).ToList();
        var bookingDeposits = (from gu in guests
                               join bookdep in dbContext.BookingDeposits on gu.guID equals bookdep.bdgu
                               select bookdep).ToList();
        return new List<object> { lstDespositsPaymentByPR, guests, bookingDeposits };
      }
    }
    #endregion

    #region GetRptGiftsReceivedBySR
    /// <summary>
    ///  Obtiene los datos para el reporte de regalos recibidos por sala de ventas
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="chargeTo">Claves de cargar a</param>
    /// <param name="gifts">Claves de regalos</param>
    /// <history>
    ///   [vku] 11/Abr/2016 Created
    /// </history>
    public static List<object> GetRptGiftsReceivedBySR(DateTime dtmStart, DateTime dtmEnd, string leadSources, string chargeTo, string gifts)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstGiftsReceivedBySR = dbContext.USP_OR_RptGiftsReceivedBySR(dtmStart, dtmEnd, leadSources, chargeTo, gifts).ToList();
        var curriencies = (from gifRecBySR in lstGiftsReceivedBySR
                           join cu in dbContext.Currencies on gifRecBySR.Currency equals cu.cuID
                           select cu).Distinct().ToList();
        return new List<object> { lstGiftsReceivedBySR, curriencies };
      }
    }
    #endregion

    #region GetRptGuestsShowNoPresentedInvitation
    /// <summary>
    ///  Obtiene los datos para el reporte de los huespedes que no se presentaron invitación
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <history>
    ///   [vku] 07/Abr/2016 Created
    /// </history>
    public static List<GuestShowNoPresentedInvitation> GetRptGuestsShowNoPresentedInvitation(DateTime dtmStart, DateTime dtmEnd, string leadSources)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGuestsShowNoPresentedInvitation(dtmStart, dtmEnd, leadSources).ToList();
      }
    }
    #endregion

    #region GetRptProductionByAge
    public static List<RptProductionByAgeOuthouse> GetRptProductionByAge(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, string program, byte filterDeposits)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptProductionByAgeOutside(dtmStart, dtmEnd, leadSources, PRs, program, filterDeposits).ToList();
      }
    }
    #endregion
  }
}
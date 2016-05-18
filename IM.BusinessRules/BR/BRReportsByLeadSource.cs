﻿using IM.BusinessRules.Properties;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public static class BRReportsByLeadSource
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
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptCostByPR_Timeout;
        return dbContext.USP_OR_RptCostByPR(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas)).ToList();
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
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptCostByPRWithDetailGifts_Timeout;
        return dbContext.USP_OR_RptCostByPRWithDetailGifts(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(detailGifts)).
          OrderBy(c => c.PR).ThenBy(c => c.AverageCost.HasValue && c.AverageCost.Value != 0).ThenBy(c => c.TotalCost.HasValue && c.TotalCost.Value != 0).ToList();
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

        return dbContext.USP_OR_RptFollowUpByAgency(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
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
        return dbContext.USP_OR_RptFollowUpByPR(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptFollowUpByPR

    #region GetRptRepsPayments

    /// <summary>
    /// Devuelve los datos para el reporte de pago de agentes
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <returns><list type="RptRepsPayment"></list></returns>
    ///  <history>
    /// [aalcocer] 15/04/2016 Created
    /// </history>
    public static List<RptRepsPayment> GetRptRepsPayments(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptRepsPayment_Timeout;
        return dbContext.USP_OR_RptRepsPayment(dtmStart, dtmEnd, string.Join(",", leadSources)).ToList();
      }
    }

    #endregion GetRptRepsPayments

    #region GetRptRepsPaymentSummaries

    /// <summary>
    /// Devuelve los datos para el reporte del resumen pago de agentes
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <returns><list type="RptRepsPaymentSummary"></list></returns>
    ///  <history>
    /// [aalcocer] 15/04/2016 Created
    /// </history>
    public static List<RptRepsPaymentSummary> GetRptRepsPaymentSummaries(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptRepsPaymentSummary_Timeout;
        return dbContext.USP_OR_RptRepsPaymentSummary(dtmStart, dtmEnd, string.Join(",", leadSources)).ToList();
      }
    }

    #endregion GetRptRepsPaymentSummaries

    #region GetRptProductionByGiftQuantities

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por regalo y cantidad
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="giftsQuantitys">Lista de cantidades y regalos. Por ejemplo: 2-GORRAS,5-PLAYERAS,3-BATAS</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByGiftQuantity"></list></returns>
    /// <history>
    /// [aalcocer] 19/04/2016 Created
    /// </history>
    public static List<RptProductionByGiftQuantity> GetRptProductionByGiftQuantities(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      Dictionary<string, int> giftsQuantitys, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByGiftQuantity_Timeout;
        return dbContext.USP_OR_RptProductionByGiftQuantity(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", giftsQuantitys.Select(c => c.Value + "-" + c.Key)),
          Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByGiftQuantities

    #region GetGraphNotBookingArrival

    /// <summary>
    /// Devuelve los datos para el reporte de llegadas no booking
    /// </summary>
    /// <param name="dtmStart">Fecha desde de la semana</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <returns><!--Tuple<GraphTotals, List<GraphNotBookingArrivals>, List<GraphNotBookingArrivals>>--></returns>
    /// <history>
    /// [aalcocer] 21/04/2016 Created
    /// [aalcocer] 09/05/2016 Modified. Se implementa el metodo de EntityMultipleResults
    /// </history>
    public static Tuple<GraphTotals, List<GraphNotBookingArrivals>, List<GraphNotBookingArrivals>> GetGraphNotBookingArrival(DateTime dtmStart, IEnumerable<string> leadSources)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_GraphNotBookingArrivals_Timeout;
        var resGraphNotBookingArrivalsTotals = dbContext.USP_OR_GraphNotBookingArrivals(dtmStart, string.Join(",", leadSources))
           .MultipleResults()
          .With<GraphTotals>()
          .With<GraphNotBookingArrivals>()
          .With<GraphNotBookingArrivals>()
           .GetValues();

        GraphTotals graphTotals = resGraphNotBookingArrivalsTotals[0].Cast<GraphTotals>().SingleOrDefault();
        List<GraphNotBookingArrivals> graphNotBookingArrivalsWeek = resGraphNotBookingArrivalsTotals[1].Cast<GraphNotBookingArrivals>().ToList();
        List<GraphNotBookingArrivals> graphNotBookingArrivalsMonth = resGraphNotBookingArrivalsTotals[2].Cast<GraphNotBookingArrivals>().ToList();

        return (graphNotBookingArrivalsWeek.Any() || graphNotBookingArrivalsMonth.Any()) ? Tuple.Create(graphTotals, graphNotBookingArrivalsWeek, graphNotBookingArrivalsMonth) : null;
      }
    }

    #endregion GetGraphNotBookingArrival

    #region GetGraphUnavailableArrivals

    /// <summary>
    /// Devuelve los datos para la grafica de llegadas no disponibles
    /// </summary>
    /// <param name="dtmStart">Fecha desde de la semana</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <returns><!--Tuple<GraphTotals, List<GraphUnavailableArrivals>, List<GraphUnavailableArrivals>>--></returns>
    /// <history>
    /// [aalcocer] 21/04/2016 Created
    /// [aalcocer] 09/05/2016 Modified. Se implementa el metodo de EntityMultipleResults
    /// </history>
    public static Tuple<GraphTotals, List<GraphUnavailableArrivals>, List<GraphUnavailableArrivals>> GetGraphUnavailableArrivals(DateTime dtmStart, IEnumerable<string> leadSources)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_GraphUnavailableArrivals_Timeout;
        var resGraphUnavailableArrivals = dbContext.USP_OR_GraphUnavailableArrivals(dtmStart, string.Join(",", leadSources))
          .MultipleResults()
          .With<GraphTotals>()
          .With<GraphUnavailableArrivals>()
          .With<GraphUnavailableArrivals>()
          .GetValues();

        GraphTotals graphTotals = resGraphUnavailableArrivals[0].Cast<GraphTotals>().SingleOrDefault();
        List<GraphUnavailableArrivals> graphUnavailableArrivalsWeek = resGraphUnavailableArrivals[1].Cast<GraphUnavailableArrivals>().ToList();
        List<GraphUnavailableArrivals> graphUnavailableArrivalsMonth = resGraphUnavailableArrivals[2].Cast<GraphUnavailableArrivals>().ToList();

        return (graphUnavailableArrivalsWeek.Any() || graphUnavailableArrivalsMonth.Any()) ? Tuple.Create(graphTotals, graphUnavailableArrivalsWeek, graphUnavailableArrivalsMonth) : null;
      }
    }

    #endregion GetGraphUnavailableArrivals

    #region GetGraphProduction

    /// <summary>
    /// Devuelve los datos para la grafica de produccion
    /// </summary>
    /// <param name="dtmStart">Fecha desde de la semana</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><!--Tuple<GraphMaximum, List<GraphProduction_Weeks>, List<GraphProduction_Months>>--></returns>
    /// <history>
    /// [aalcocer] 21/04/2016 Created
    /// [aalcocer] 09/05/2016 Modified. Se implementa el metodo de EntityMultipleResults
    /// </history>
    public static Tuple<GraphMaximum, List<GraphProduction_Weeks>, List<GraphProduction_Months>> GetGraphProduction(DateTime dtmStart, IEnumerable<string> leadSources,
      EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_GraphProduction_Timeout;
        var resGraphProductionWeeks = dbContext.USP_OR_GraphProduction(dtmStart, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival))
          .MultipleResults()
          .With<GraphProduction_Weeks>()
          .With<GraphProduction_Months>()
          .With<GraphMaximum>()
           .GetValues();

        List<GraphProduction_Weeks> listGraphProductionWeeks = resGraphProductionWeeks[0].Cast<GraphProduction_Weeks>().ToList();
        List<GraphProduction_Months> listGraphProductionMonths = resGraphProductionWeeks[1].Cast<GraphProduction_Months>().ToList();
        GraphMaximum graphMaximum = resGraphProductionWeeks[2].Cast<GraphMaximum>().SingleOrDefault();

        return Tuple.Create(graphMaximum, listGraphProductionWeeks, listGraphProductionMonths);
      }
    }

    #endregion GetGraphProduction

    #region GetRptUnavailableMotivesByAgencies

    /// <summary>
    /// Devuelve los datos para el reporte de Unavailable Motives by Angency
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="markets">Claves de mercados</param>
    /// <param name="agencies">Claves de agencias</param>
    /// <returns><list type="RptUnavailableMotivesByAgency"></list></returns>
    /// <history>
    /// [aalcocer] 21/04/2016 Created
    /// </history>
    public static List<RptUnavailableMotivesByAgency> GetRptUnavailableMotivesByAgencies(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      IEnumerable<string> markets = null, IEnumerable<string> agencies = null)
    {
      if (markets == null || !markets.Any())
        markets = new[] { "ALL" };

      if (agencies == null || !agencies.Any())
        agencies = new[] { "ALL" };
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptUnavailableMotivesByAgency_Timeout;
        return dbContext.USP_OR_RptUnavailableMotivesByAgency(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", markets), string.Join(",", agencies)).ToList();
      }
    }

    #endregion GetRptUnavailableMotivesByAgencies

    #region GetRptShowFactorByBookingDates

    /// <summary>
    /// Devuelve los datos para el reporte de porcentaje de show por fecha de booking
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <returns><list type="RptShowFactorByBookingDate"></list></returns>
    /// <history>
    /// [aalcocer] 21/04/2016 Created
    /// </history>
    public static List<RptShowFactorByBookingDate> GetRptShowFactorByBookingDates(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptShowFactorByBookingDate_Timeout;
        return dbContext.USP_OR_RptShowFactorByBookingDate(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas)).ToList();
      }
    }

    #endregion GetRptShowFactorByBookingDates

    #region GetRptScoreByPrs

    /// <summary>
    /// Devuelve los datos para el reporte de puntuacion por PR
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <returns>ScoreByPRData</returns>
    /// <history>
    /// [aalcocer] 13/05/2016 Created
    /// </history>
    public static ScoreByPRData GetRptScoreByPrs(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas)
    {
      var scoreByPRData = new ScoreByPRData();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptScoreByPR_Timeout;
        //1. Reporte
        scoreByPRData.ScoreByPR = dbContext.USP_OR_RptScoreByPR(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas)).ToList();
        //2. Conceptos de puntuacion
        scoreByPRData.ScoreRuleConcept = dbContext.ScoreRulesConcepts.OrderBy(x => x.spID).ToList();
        //3. Reglas de puntuacion
        scoreByPRData.ScoreRule = (from lSbPr in scoreByPRData.ScoreByPR
                                   join sr in dbContext.ScoreRules on lSbPr.ScoreRule equals sr.suID.ToString()
                                   where lSbPr.ScoreRuleType == "G"
                                   select sr).Distinct().ToList();
        //4. Detalle de las reglas de puntuacion
        var suIDs = scoreByPRData.ScoreRule.Select(s => s.suID).ToList();
        scoreByPRData.ScoreRuleDetail = dbContext.ScoreRulesDetails.Where(x => suIDs.Contains(x.sisu)).OrderBy(x => x.sisu).ToList();
        //5. Reglas de puntuacion por Lead Source
        scoreByPRData.LeadSourceShort = (from lSbPr in scoreByPRData.ScoreByPR
                                         join ls in dbContext.LeadSources on lSbPr.ScoreRule equals ls.lsID
                                         where lSbPr.ScoreRuleType == "LS"
                                         select new LeadSourceShort { lsID = ls.lsID, lsN = ls.lsN }).ToList();
        //6. Detalle de las reglas de puntuacion por Lead Source
        scoreByPRData.ScoreRuleByLeadSourceDetail = dbContext.ScoreRulesByLeadSourceDetails.
          Where(x => scoreByPRData.LeadSourceShort.Select(ls => ls.lsID).Contains(x.sjls)).ToList();

        //7.Tipos de reglas de puntuacion
        scoreByPRData.ScoreRuleType = new List<ScoreRuleType>();
        if (scoreByPRData.ScoreRule.Any())
          scoreByPRData.ScoreRuleType.AddRange(dbContext.ScoreRulesTypes.Where(s => s.syID == "G"));
        if (scoreByPRData.LeadSourceShort.Any())
          scoreByPRData.ScoreRuleType.AddRange(dbContext.ScoreRulesTypes.Where(s => s.syID == "LS"));
      }
      return scoreByPRData;
    }

    #endregion GetRptScoreByPrs

    #region GetRptProductionByMonths

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por mes
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="markets">Clave del mercado</param>
    /// <param name="considerNights">Indica si se debe considerar el numero de noches</param>
    /// <param name="nightsFrom">Numero de noches desde</param>
    /// <param name="nightsTo">Numero de noches hasta</param>
    /// <param name="agencies">Clave de agencia</param>
    /// <param name="considerOriginallyAvailable">Indica si se debe considerar originalmente disponible</param>
    /// <param name="external">Filtro de invitaciones externas
    /// 0. Sin filtro
    /// 1. Excluir invitaciones externas
    /// 2. Solo invitaciones externas
    /// </param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByMonth"></list></returns>
    /// <history>
    /// [aalcocer] 13/05/2016 Created
    /// </history>
    public static List<RptProductionByMonth> GetRptProductionByMonths(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, IEnumerable<string> markets = null, bool considerNights = false,
      int nightsFrom = 0, int nightsTo = 0, IEnumerable<string> agencies = null, bool considerOriginallyAvailable = false,
      EnumExternalInvitation external = EnumExternalInvitation.extInclude, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      if (markets == null || !markets.Any())
        markets = new[] { "ALL" };
      if (agencies == null || !agencies.Any())
        agencies = new[] { "ALL" };
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByMonth_Timeout;
        return dbContext.USP_OR_RptProductionByMonth(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas),
          string.Join(",", markets), considerNights, nightsFrom, nightsTo, string.Join(",", agencies), considerOriginallyAvailable, Convert.ToInt16(external),
          Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByMonths

    #region Inhouse

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
        return dbContext.USP_OR_RptProductionByAgeInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
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
        return dbContext.USP_OR_RptProductionByAgeMarketOriginallyAvailableInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
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
    /// <returns>ProductionByAgencyInhouseData</returns>
    /// <history>
    /// [aalcocer] 04/04/2016 Created
    /// [aalcocer] 09/05/2016 Modified. Ahora devuelve una entidad compleja y se agregan los demas entidades que devuelve la consulta
    /// </history>
    public static ProductionByAgencyInhouseData GetRptProductionByAgencyInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas,
      bool considerNights = false, int nightsFrom = 0, int nightsTo = 0, EnumSalesByMemberShipType salesByMembershipType = EnumSalesByMemberShipType.sbmNoDetail,
      bool onlyQuinellas = false, EnumExternalInvitation external = EnumExternalInvitation.extExclude, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      var productionByAgencyInhouseData = new ProductionByAgencyInhouseData();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByAgencyInhouse_Timeout;
        var resProductionByAgencyInhouse = dbContext.USP_OR_RptProductionByAgencyInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), considerNights, nightsFrom, nightsTo,
            Convert.ToBoolean(salesByMembershipType), onlyQuinellas, Convert.ToInt16(external), Convert.ToBoolean(basedOnArrival)).MultipleResults()
         .With<RptProductionByAgencyInhouse>()
         .With<MembershipTypeShort>()
         .With<RptProductionByAgencyInhouse_SalesByMembershipType>()
         .GetValues();

        productionByAgencyInhouseData.ProductionByAgencyInhouses = resProductionByAgencyInhouse[0].Cast<RptProductionByAgencyInhouse>().ToList();
        productionByAgencyInhouseData.MembershipTypeShorts = resProductionByAgencyInhouse[1].Cast<MembershipTypeShort>().ToList();
        productionByAgencyInhouseData.ProductionByAgencyInhouse_SalesByMembershipTypes = resProductionByAgencyInhouse[2].Cast<RptProductionByAgencyInhouse_SalesByMembershipType>().ToList();
      }
      return productionByAgencyInhouseData;
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
        markets = new List<string> { "ALL" };

      if (agencies == null || !agencies.Any())
        agencies = new List<string> { "ALL" };

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByContractAgencyInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByContractAgencyInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", markets), string.Join(",", agencies), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
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
        markets = new List<string> { "ALL" };

      if (agencies == null || !agencies.Any())
        agencies = new List<string> { "ALL" };

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByContractAgencyMarketOriginallyAvailableInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByContractAgencyMarketOriginallyAvailableInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", markets), string.Join(",", agencies), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByContractAgencyMarketOriginallyAvailableInhouses

    #region GetRptProductionByNationalityInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por nacionalidad (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="pRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="filterSaveCourtesyTours">Filtro de tours de rescate y cortesia
    /// 0. Sin filtro
    /// 1. Excluir tours de rescate y cortesia
    /// 2. Excluir tours de rescate y cortesia sin venta
    /// </param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByNationalityInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 12/04/2016 Created
    /// </history>
    public static List<RptProductionByNationalityInhouse> GetRptProductionByNationalityInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources, IEnumerable<string> pRs = null,
      EnumProgram program = EnumProgram.All, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumSaveCourtesyTours filterSaveCourtesyTours = EnumSaveCourtesyTours.sctIncludeSaveCourtesyTours,
      EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      if (pRs == null || !pRs.Any()) pRs = new[] { "ALL" };
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByNationalityInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByNationalityInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", pRs), EnumToListHelper.GetEnumDescription(program),
          Convert.ToBoolean(considerQuinellas), Convert.ToByte(filterSaveCourtesyTours), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByNationalityInhouses

    #region GetRptProductionByNationalityMarketOriginallyAvailableInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por nacionalidad, mercado y originalmente disponible (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="pRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="filterSaveCourtesyTours">Filtro de tours de rescate y cortesia
    /// 0. Sin filtro
    /// 1. Excluir tours de rescate y cortesia
    /// 2. Excluir tours de rescate y cortesia sin venta
    /// </param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByNationalityMarketOriginallyAvailableInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 12/04/2016 Created
    /// </history>
    public static List<RptProductionByNationalityMarketOriginallyAvailableInhouse> GetRptProductionByNationalityMarketOriginallyAvailableInhouses(DateTime dtmStart, DateTime dtmEnd,
      IEnumerable<string> leadSources, IEnumerable<string> pRs = null,
      EnumProgram program = EnumProgram.All, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumSaveCourtesyTours filterSaveCourtesyTours = EnumSaveCourtesyTours.sctIncludeSaveCourtesyTours,
      EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      if (pRs == null || !pRs.Any()) pRs = new[] { "ALL" };
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByNationalityInhouse_Timeout;
        return
          dbContext.USP_OR_RptProductionByNationalityMarketOriginallyAvailableInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", pRs),
            EnumToListHelper.GetEnumDescription(program),
            Convert.ToBoolean(considerQuinellas), Convert.ToByte(filterSaveCourtesyTours), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByNationalityMarketOriginallyAvailableInhouses

    #region GetRptProductionByCoupleTypeInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por tipo de pareja (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="pRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByCoupleTypeInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 12/04/2016 Created
    /// </history>
    public static List<RptProductionByCoupleTypeInhouse> GetRptProductionByCoupleTypeInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources, IEnumerable<string> pRs = null,
      EnumProgram program = EnumProgram.All, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      if (pRs == null || !pRs.Any())
        pRs = new[] { "ALL" };
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByCoupleTypeInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByCoupleTypeInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", pRs),
          EnumToListHelper.GetEnumDescription(program), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByCoupleTypeInhouses

    #region GetRptProductionByDeskInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de producción por escritorio (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByDeskInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 13/04/2016 Created
    /// </history>
    public static List<RptProductionByDeskInhouse> GetRptProductionByDeskInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByDeskInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByDeskInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByDeskInhouses

    #region GetRptProductionByGroupInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de producción por grupo (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByGroupInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 13/04/2016 Created
    /// </history>
    public static List<RptProductionByGroupInhouse> GetRptProductionByGroupInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByGroupInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByGroupInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByGroupInhouses

    #region GetRptProductionByGuestStatusInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por estatus de huesped (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByGuestStatusInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 13/04/2016 Created
    /// </history>
    public static List<RptProductionByGuestStatusInhouse> GetRptProductionByGuestStatusInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByGuestStatusInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByGuestStatusInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByGuestStatusInhouses

    #region GetRptProductionByMemberTypeAgencyInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por estatus de huesped (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="markets">Claves de mercados</param>
    /// <param name="agencies">Claves de agencias</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByMemberTypeAgencyInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 13/04/2016 Created
    /// </history>
    public static List<RptProductionByMemberTypeAgencyInhouse> GetRptProductionByMemberTypeAgencyInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      IEnumerable<string> markets = null, IEnumerable<string> agencies = null,
      EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      if (markets == null || !markets.Any())
        markets = new[] { "ALL" };
      if (agencies == null || !agencies.Any())
        agencies = new[] { "ALL" };
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByMemberTypeAgencyInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByMemberTypeAgencyInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", markets), string.Join(",", agencies), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByMemberTypeAgencyInhouses

    #region GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de producccion por tipo de socio, agencia, mercado y originalmente disponible (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="markets">Claves de mercados</param>
    /// <param name="agencies">Claves de agencias</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 13/04/2016 Created
    /// </history>
    public static List<RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse> GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      IEnumerable<string> markets = null, IEnumerable<string> agencies = null,
      EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      if (markets == null || !markets.Any())
        markets = new[] { "ALL" };
      if (agencies == null || !agencies.Any())
        agencies = new[] { "ALL" };
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", markets), string.Join(",", agencies), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses

    #region GetRptProductionByPRInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por PR (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <param name="basedOnPRLocation">Indica si se debe basar en la locacion por default del PR</param>
    /// <returns><list type="RptProductionByPRInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 13/04/2016 Created
    /// </history>
    public static List<RptProductionByPRInhouse> GetRptProductionByPRInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival, bool basedOnPRLocation = false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByPRInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByPRInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival), basedOnPRLocation).ToList();
      }
    }

    #endregion GetRptProductionByPRInhouses

    #region GetRptProductionByPRGroupInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de producción por PR y grupo (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByPRGroupInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 13/04/2016 Created
    /// </history>
    public static List<RptProductionByPRGroupInhouse> GetRptProductionByPRGroupInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByPRGroupInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByPRGroupInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByPRGroupInhouses

    #region GetRptProductionByPRSalesRoomInhouses

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por Salas de Ventas y PR (Inhouse)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByPRSalesRoomInhouse"></list></returns>
    /// <history>
    /// [aalcocer] 13/04/2016 Created
    /// </history>
    public static List<RptProductionByPRSalesRoomInhouse> GetRptProductionByPRSalesRoomInhouses(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByPRSalesRoomInhouse_Timeout;
        return dbContext.USP_OR_RptProductionByPRSalesRoomInhouse(dtmStart, dtmEnd, string.Join(",", leadSources), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByPRSalesRoomInhouses

    #endregion Inhouse

    #region Outhouse

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
    /// <history>
    ///   [vku] 07/Abr/2016 Created
    /// </history>
    public static DepositsPaymentByPRData GetRptDepositsPaymentByPRData(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, string paymentTypes = "ALL", EnumFilterDeposit filterDeposit = EnumFilterDeposit.fdAll)
    {
      DepositsPaymentByPRData DepositsPaymentByPRData = new DepositsPaymentByPRData();
      DepositsPaymentByPRData.DepositsPaymentByPR = new List<RptDepositsPaymentByPR>();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var resDepositsPaymentByPR = dbContext.USP_OR_RptDepositsPaymentByPR(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), paymentTypes, Convert.ToByte(filterDeposit));
        DepositsPaymentByPRData.DepositsPaymentByPR = resDepositsPaymentByPR.ToList();

        var resDepositsPaymentByPRDeposits = resDepositsPaymentByPR.GetNextResult<RptDepositsPaymentByPR_Deposit>();
        DepositsPaymentByPRData.DepositsPaymentByPR_Deposit = resDepositsPaymentByPRDeposits.ToList();

        List<CurrencyShort> currencies = (from payByPRdep in DepositsPaymentByPRData.DepositsPaymentByPR_Deposit.Select(c => c.bdcu).Distinct()
                                          join cu in dbContext.Currencies on payByPRdep equals cu.cuID
                                          select new CurrencyShort
                                          {
                                            cuID = cu.cuID,
                                            cuN = cu.cuN,
                                          }).ToList();

        List<PaymentTypeShort> paymentType = (from payByPRdep in DepositsPaymentByPRData.DepositsPaymentByPR_Deposit.Select(c => c.bdpt).Distinct()
                                              join pT in dbContext.PaymentTypes on payByPRdep equals pT.ptID
                                              select new PaymentTypeShort
                                              {
                                                ptID = pT.ptID,
                                                ptN = pT.ptN,
                                              }).ToList();

        DepositsPaymentByPRData.Currencies = currencies;
        DepositsPaymentByPRData.PaymentTypes = paymentType;

        return DepositsPaymentByPRData;
      }
    }

    #endregion GetRptDepositsPaymentByPR

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
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptGiftsReceivedBySR_Timeout;
        var lstcurriencies = dbContext.Currencies.Where(c => c.cuA).ToList();
        var lstGiftsReceivedBySR = dbContext.USP_OR_RptGiftsReceivedBySR(dtmStart, dtmEnd, leadSources, chargeTo, gifts).ToList();

        var listGiftsRecBySRWithCu = (from gifRecBySR in lstGiftsReceivedBySR.Select(c => c.Currency).Distinct()
                                      join curr in lstcurriencies on gifRecBySR equals curr.cuID
                                      select curr).ToList();

        return new List<object> { lstGiftsReceivedBySR, listGiftsRecBySRWithCu };
      }
    }

    #endregion GetRptGiftsReceivedBySR

    #region GetRptGuestsShowNoPresentedInvitation

    /// <summary>
    ///  Obtiene los datos para el reporte de los huespedes que no se presentaron invitación
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    /// </history>
    public static List<GuestShowNoPresentedInvitation> GetRptGuestsShowNoPresentedInvitation(DateTime dtmStart, DateTime dtmEnd, string leadSources)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGuestsShowNoPresentedInvitation(dtmStart, dtmEnd, leadSources).ToList();
      }
    }

    #endregion GetRptGuestsShowNoPresentedInvitation

    #region GetRptProductionByPROuthouse

    /// <summary>
    /// Obtiene los datos para el reporte de produccion por PR (Outside)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de Programa</param>
    /// <param name="filterDeposit">
    /// Filtro de positos. 0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito (Flyers),
    /// 3. Con deposito y shows sin deposito (Deposits & Flyers Show)
    /// </param>
    /// <param name="basedOnBooking">Indica si se debe basar en la fecha de booking</param>
    /// <history>
    ///   [vku] 14/Abr/2016 Created
    /// </history>
    public static List<RptProductionByPROuthouse> GetRptProductionByPROuthouse(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram programa = EnumProgram.All, EnumFilterDeposit filterDeposit = EnumFilterDeposit.fdAll, EnumBasedOnBooking basedOnBooking = EnumBasedOnBooking.bobNoBasedOnBooking)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByPROutside_Timeout;
        return dbContext.USP_OR_RptProductionByPROutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(programa), Convert.ToByte(filterDeposit), Convert.ToBoolean(basedOnBooking)).ToList();
      }
    }

    #endregion GetRptProductionByPROuthouse

    #region GetRptProductionByAge

    /// <summary>
    ///  Obtiene los datos para el reporte de produccion por edad (Outside)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hast</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de Programa</param>
    /// <param name="filterDeposits">Filtro de depositos</param>
    /// <history>
    ///  [vku] 13/Abr/2016 Created
    /// </history>
    public static List<RptProductionByAgeOuthouse> GetRptProductionByAge(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, EnumFilterDeposit filterDeposits = EnumFilterDeposit.fdAll)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByAgeOutside_Timeout;
        return dbContext.USP_OR_RptProductionByAgeOutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), Convert.ToByte(filterDeposits)).ToList();
      }
    }

    #endregion GetRptProductionByAge

    #region GetRptProductionByAgeSalesRoomOuthouse

    /// <summary>
    ///   Obtiene los datos para el reporte de produccion por edad y sala (Outside)
    /// </summary>
    /// <param name="dtmStart">Fecha de desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="Program">Clave del Programa</param>
    /// <param name="filterDeposits">Filtro de depositos
    /// 0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito(Flyers), 3. Con deposito y shows sin deposito(Deposits & Flyers Show)
    /// </param>
    /// <history>
    ///   [vku] 13/Abr/2016 Created
    /// </history>
    public static List<RptProductionByAgeSalesRoomOuthouse> GetRptProductionByAgeSalesRoomOuthouse(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, EnumFilterDeposit filterDeposit = EnumFilterDeposit.fdAll)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByAgencySalesRoomOutside_Timeout;
        return dbContext.USP_OR_RptProductionByAgeSalesRoomOutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), Convert.ToByte(filterDeposit)).ToList();
      }
    }

    #endregion GetRptProductionByAgeSalesRoomOuthouse

    #region GetRptProductionByAgencyOuthouse

    /// <summary>
    ///   Obtiene los datos para el reporte de produccion por agencia (Outside)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="filtersDeposits">Filtro de depositos
    /// 0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito(Flyers), 3. Con deposito y shows sin deposito(Deposits & Flyers Show)
    /// </param>
    /// <param name="salesByMemberShipType">Indica si se desean las ventas por tipo de membresia</param>
    /// <history>
    ///   [vku] 15/Abr/2016 Created
    /// </history>
    public static ProductionByAgencyOuthouseData GetRptProductionByAgencyOuthouseData(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, EnumFilterDeposit filterDeposits = EnumFilterDeposit.fdAll, EnumSalesByMemberShipType salesByMemberShipType = EnumSalesByMemberShipType.sbmNoDetail)
    {
      ProductionByAgencyOuthouseData ProductionByAgencyOuthouseData = new ProductionByAgencyOuthouseData();
      ProductionByAgencyOuthouseData.ProductionByAgencyOuthouse = new List<RptProductionByAgencyOuthouse>();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByAgencyOutside_Timeout;

        var resProductionByAgency = dbContext.USP_OR_RptProductionByAgencyOutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), Convert.ToByte(filterDeposits), Convert.ToBoolean(salesByMemberShipType));
        ProductionByAgencyOuthouseData.ProductionByAgencyOuthouse = resProductionByAgency.ToList();

        if (Convert.ToBoolean(salesByMemberShipType))
        {
          var resProductionByAgencyMembershipType = resProductionByAgency.GetNextResult<MembershipTypeShort>();
          ProductionByAgencyOuthouseData.MembershipTypes = resProductionByAgencyMembershipType.ToList();

          var resProductionByAgencySalesMembershipType = resProductionByAgencyMembershipType.GetNextResult<RptProductionByAgencyOuthouse_SalesByMembershipType>();
          ProductionByAgencyOuthouseData.ProductionByAgencyOuthouse_SalesByMembershipType = resProductionByAgencySalesMembershipType.ToList();
        }
        return ProductionByAgencyOuthouseData;
      }
    }

    #endregion GetRptProductionByAgencyOuthouse

    #region GetRptProductionByAgencySalesRoomOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte de produccion por agencia y sala (Outside)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="filtersDeposits">Filtro de depositos
    /// 0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito(Flyers), 3. Con deposito y shows sin deposito(Deposits & Flyers Show)
    /// </param>
    /// <param name="salesByMemberShipType">Indica si se desean las ventas por tipo de membresia</param>
    /// <history>
    ///   [vku] 20/Abr/2016 Created
    /// </history>
    public static List<RptProductionByAgencySalesRoomOuthouse> GetRptProductionByAgencySalesRoomOuthouse(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, EnumFilterDeposit filterDeposits = EnumFilterDeposit.fdAll, EnumSalesByMemberShipType salesByMemberShipType = EnumSalesByMemberShipType.sbmNoDetail)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptProductionByAgencySalesRoomOutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), Convert.ToByte(filterDeposits), Convert.ToBoolean(salesByMemberShipType)).ToList();
      }
    }

    #endregion GetRptProductionByAgencySalesRoomOuthouse

    #region GetRptProductionByAgencyMarketHotelOuthouse

    /// <summary>
    /// Obtiene los datos para el reporte de produccion por agencia, mercado y hotel (Outside)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="filtersDeposits">Filtro de depositos
    /// 0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito(Flyers), 3. Con deposito y shows sin deposito(Deposits & Flyers Show)
    /// </param>
    /// <history>
    ///   [vku] 15/Abr/2016 Created
    /// </history>
    public static List<RptProductionByAgencyMarketHotelOuthouse> GetRptProductionByAgencyMarketHotelOuthouse(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, EnumFilterDeposit filterDeposit = EnumFilterDeposit.fdAll)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByAgencyMarketHotelOutside_Timeout;
        return dbContext.USP_OR_RptProductionByAgencyMarketHotelOutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), Convert.ToByte(filterDeposit)).ToList();
      }
    }

    #endregion GetRptProductionByAgencyMarketHotelOuthouse

    #region GetRptProductionByCoupleTypeOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte de produccion por tipo de pareja (Outside)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="filterDeposits">Filtro de depositos</param>
    /// 0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito(Flyers), 3. Con deposito y shows sin deposito(Deposits & Flyers Show)
    /// </param>
    /// <history>
    ///   [vku] 18/Abr/2016 Created
    /// </history>
    public static List<RptProductionByCoupleTypeOuthouse> GetRptProductionByCoupleTypeOuthouse(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, EnumFilterDeposit filterDeposits = EnumFilterDeposit.fdAll)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptProductionByCoupleTypeOutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), Convert.ToByte(filterDeposits)).ToList();
      }
    }

    #endregion GetRptProductionByCoupleTypeOuthouse

    #region GetRptProductionByCoupleTypeSalesRoomOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte de produccion por tipo de pareja y sala (Outside)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="filterDeposits">Filtro de depositos
    /// 0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito(Flyers), 3. Con deposito y shows sin deposito(Deposits & Flyers Show)
    /// </param>
    /// <history>
    ///   [vku] 18/Abr/2016 Created
    /// </history>
    public static List<RptProductionByCoupleTypeSalesRoomOuthouse> GetRptProductionByCoupleTypeSalesRoomOuthouse(DateTime dtmStart, DateTime dtmEnd, String leadSources, string PRs, EnumProgram program = EnumProgram.All, EnumFilterDeposit filterDeposits = EnumFilterDeposit.fdAll)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptProductionByCoupleTypeSalesRoomOutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), Convert.ToByte(filterDeposits)).ToList();
      }
    }

    #endregion GetRptProductionByCoupleTypeSalesRoomOuthouse

    #region GetRptProductionByGiftInvitation

    /// <summary>
    ///   Obtiene los datos para el reporte de produccion por regalo de invitacion
    /// </summary>
    /// <param name="dtmStart">Fecha de desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="gifts">Claves de regalos</param>
    /// <param name="filterDeposits">Filtro de depositos
    ///  0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito(Flyers), 3. Con deposito y shows sin deposito(Deposits & Flyers Show)
    /// </param>
    /// <history>
    ///   [vku] 21/Abr/2016 Created
    /// </history>
    public static List<RptProductionByGiftInvitation> GetRptProductionByGiftInvitation(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, string gifts = "ALL", EnumFilterDeposit filterDeposits = EnumFilterDeposit.fdAll)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByGiftInvitation_Timeout;
        return dbContext.USP_OR_RptProductionByGiftInvitation(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), gifts, Convert.ToByte(filterDeposits)).ToList();
      }
    }

    #endregion GetRptProductionByGiftInvitation

    #region GetRptProductionByGiftInvitationSalesRoom

    /// <summary>
    ///   Obtiene los datos para el reporte de produccion por regalo de invitacion y sala
    /// </summary>
    /// <param name="dtmStart">Fecha de desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="gifts">Claves de regalos</param>
    /// <param name="filterDeposits">Filtro de depositos
    ///  0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito(Flyers), 3. Con deposito y shows sin deposito(Deposits & Flyers Show)
    /// </param>
    /// <history>
    ///   [vku] 21/Abr/2016 Created
    /// </history>
    public static List<RptProductionByGiftInvitationSalesRoom> GetRptProductionByGiftInvitationSalesRoom(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, string gifts = "ALL", EnumFilterDeposit filterDeposits = EnumFilterDeposit.fdAll)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByGiftInvitationSalesRoom_Timeout;
        return dbContext.USP_OR_RptProductionByGiftInvitationSalesRoom(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), gifts, Convert.ToByte(filterDeposits)).ToList();
      }
    }

    #endregion GetRptProductionByGiftInvitationSalesRoom

    #region GetProductionByGuestStatusOuthouse

    /// <summary>
    ///   Obtiene los datos para el reporte de produccion por estatus de huesped (Outside)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="filterDeposits">
    ///  0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito(Flyers), 3. Con deposito y shows sin deposito(Deposits & Flyers Show)
    /// </param>
    /// <history>
    ///   [vku] 22/Abr/2016 Created
    /// </history>
    public static List<RptProductionByGuestStatusOuthouse> GetRptProductionByGuestStatusOuthouse(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, EnumFilterDeposit filterDeposits = EnumFilterDeposit.fdAll)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptProductionByGuestStatusOutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), Convert.ToByte(filterDeposits)).ToList();
      }
    }

    #endregion GetProductionByGuestStatusOuthouse

    #region GetProductionByNationalityOuthouse

    /// <summary>
    ///   Obtiene los datos para el reporte de produccion por nacionalidad (outside)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="filterDeposits">Filtro de depositos
    ///  0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito(Flyers), 3. Con deposito y shows sin deposito(Deposits & Flyers Show)
    /// </param>
    /// <param name="saveCourtesyTours">Filtro de tours de rescate y cortesia
    /// 0. Sin filtro, 1. Excluir tours de rescate y cortesia, 2. Excluir tours de rescate y cortesia sin venta
    /// </param>
    /// <history>
    ///   [vku] 22/Abr/2016 Created
    /// </history>
    public static List<RptProductionByNationalityOuthouse> GetRptProductionByNationalityOuthouse(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, EnumFilterDeposit filterDeposits = EnumFilterDeposit.fdAll, EnumSaveCourtesyTours saveCourtesyTours = EnumSaveCourtesyTours.sctIncludeSaveCourtesyTours)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByNationalityOutside_Timeout;
        return dbContext.USP_OR_RptProductionByNationalityOutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), Convert.ToByte(filterDeposits), Convert.ToByte(saveCourtesyTours)).ToList();
      }
    }

    #endregion GetProductionByNationalityOuthouse

    #region GetProductionByNationalitySalesRoomOuthouse

    /// <summary>
    ///  Obtienes los datos para el reporte de produccion por nacionalidad y sala (Outside)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="filterDeposits">Filtro de depositos
    /// 0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito(Flyers), 3. Con deposito y shows sin deposito(Deposits & Flyers Show)
    /// </param>
    /// <param name="saveCourtesyTours">Filtro de tours de rescate y cortesia
    /// 0. Sin filtro, 1. Excluir tours de rescate y cortesia, 2. Excluir tours de rescate y cortesia sin venta
    /// </param>
    /// <history>
    ///   [vku] 22/Abr/2016 Created
    /// </history>
    public static List<RptProductionByNationalitySalesRoomOuthouse> GetRptProductionByNationalitySalesRoomOuthouse(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, EnumFilterDeposit filterDeposits = EnumFilterDeposit.fdAll, EnumSaveCourtesyTours saveCourtesyTours = EnumSaveCourtesyTours.sctIncludeSaveCourtesyTours)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptProductionByNationalitySalesRoomOutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), Convert.ToByte(filterDeposits), Convert.ToByte(saveCourtesyTours)).ToList();
      }
    }

    #endregion GetProductionByNationalitySalesRoomOuthouse

    #region GetRptProductionbyPRSalesRoomOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte de produccion por PR y sala (Outside)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de Programa</param>
    /// <param name="filterDeposit">
    /// Filtro de positos. 0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito (Flyers),
    /// 3. Con deposito y shows sin deposito (Deposits & Flyers Show)
    /// </param>
    /// <param name="basedOnBooking">Indica si se debe basar en la fecha de booking</param>
    /// <history>
    ///   [vku] 25/Abr/2016 Created
    /// </history>
    public static List<RptProductionByPRSalesRoomOuthouse> GetRptProductionByPRSalesRoomOuthouse(DateTime dtmStart, DateTime dtmEnd, String leadSources, string PRs, EnumProgram program = EnumProgram.All, EnumFilterDeposit filterDeposit = EnumFilterDeposit.fdAll, EnumBasedOnBooking basedOnBooking = EnumBasedOnBooking.bobNoBasedOnBooking)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptProductionByPRSalesRoomOutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), Convert.ToByte(filterDeposit), Convert.ToBoolean(basedOnBooking)).ToList();
      }
    }

    #endregion GetRptProductionbyPRSalesRoomOuthouse

    #region GetRptProductionbyPRContacOuthouse

    /// <summary>
    ///  Obtiene los datos de production por PR de contactos (Outside)
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="filterDeposit">
    /// Filtro de positos. 0. Sin filtro, 1. Con deposito(Deposits), 2. Sin deposito (Flyers),
    /// 3. Con deposito y shows sin deposito (Deposits & Flyers Show)
    /// </param>
    /// <history>
    ///   [vku] 25/Abr/2016 Created
    /// </history>
    public static List<RptProductionByPRContactOuthouse> GetProductionByPRContactOuthouse(DateTime dtmStart, DateTime dtmEnd, string leadSources, string PRs, EnumProgram program = EnumProgram.All, EnumFilterDeposit filterDeposits = EnumFilterDeposit.fdAll)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptProductionByPRContactOutside(dtmStart, dtmEnd, leadSources, PRs, EnumToListHelper.GetEnumDescription(program), Convert.ToByte(filterDeposits)).ToList();
      }
    }

    #endregion GetRptProductionbyPRContacOuthouse

    #region GetRptFoliosInvitationByDateFolio

    /// <summary>
    ///   Obtiene los datos para el reporte de Folios invitations Outhouse
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="serie">Serie</param>
    /// <param name="folioFrom">Folio desde</param>
    /// <param name="folioTo">Folio hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <history>
    ///   [vku] 03/May/2016 Created
    /// </history>
    public static List<RptFoliosInvitationByDateFolio> GetRptFoliosInvitationByDateFolio(DateTime? dtmStart, DateTime? dtmEnd, string serie, string folioFrom, string folioTo, string leadSources = "ALL", string PRs = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptFoliosInvitationByDateFolio(dtmStart, dtmEnd, serie, Convert.ToInt32(folioFrom), Convert.ToInt32(folioTo), leadSources, PRs).ToList();
      }
    }

    #endregion GetRptFoliosInvitationByDateFolio

    #endregion Outhouse

    #region Processor General

    #region GetRptDepositsBurnedGuests

    /// <summary>
    /// Obtiene el reporte Deposits Burned Guests
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="leadSources"></param>
    /// <returns> List<RptDepositsBurnedGuests> </returns>
    /// <history>
    ///   [edgrodriguez] 19/04/2016 Created
    /// </history>
    public static List<object> GetRptDepositsBurnedGuests(DateTime dtmStart, DateTime dtmEnd, string leadSources)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstguests = dbContext.USP_OR_RptDepositsBurnedGuests(dtmStart, dtmEnd, leadSources).ToList();

        var currencies = (from gift in lstguests.Select(c => c.gucu).Distinct()
                          join curr in dbContext.Currencies on gift equals curr.cuID
                          select curr).ToList();
        var payType = (from gift in lstguests.Select(c => c.gupt).Distinct()
                       join payT in dbContext.PaymentTypes on gift equals payT.ptID
                       select payT).ToList();

        return (lstguests.Count > 0) ? new List<object> { lstguests, currencies, payType } : new List<object>();
      }
    }

    #endregion GetRptDepositsBurnedGuests

    #region GetRptDepositRefunds

    /// <summary>
    /// Obtiene el reporte Deposits Refunds
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="leadSources"></param>
    /// <returns> List<RptDepositsBurnedGuests> </returns>
    /// <history>
    /// [edgrodriguez] 19/04/2016 Created
    /// </history>
    public static List<RptDepositRefund> GetRptDepositRefunds(DateTime dtmStart, DateTime dtmEnd, string leadSources = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstDepRef = dbContext.USP_OR_RptDepositRefund(dtmStart, dtmEnd, leadSources).ToList();
        return lstDepRef;
      }
    }

    #endregion GetRptDepositRefunds

    #region GetRptDepositByPR

    /// <summary>
    /// Obtiene el reporte Deposits By PR
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="leadSources"></param>
    /// <returns> List<object> </returns>
    /// <history>
    /// [edgrodriguez] 19/04/2016 Created
    /// </history>
    public static List<object> GetRptDepositByPR(DateTime dtmStart, DateTime dtmEnd, string leadSources)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstDepPR = dbContext.USP_OR_RptDepositsByPR(dtmStart, dtmEnd, leadSources).ToList();

        var currencies = (from gift in lstDepPR.Select(c => c.gucu).Distinct()
                          join curr in dbContext.Currencies on gift equals curr.cuID
                          select curr).ToList();

        var payType = (from gift in lstDepPR.Select(c => c.gupt).Distinct()
                       join payT in dbContext.PaymentTypes on gift equals payT.ptID
                       select payT).ToList();

        return (lstDepPR.Count > 0) ? new List<object> { lstDepPR, currencies, payType } : new List<object>();
      }
    }

    #endregion GetRptDepositByPR

    #region GetRptDepositsNoShow

    /// <summary>
    /// Obtiene el reporte Deposits No Show
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="leadSources"></param>
    /// <returns> List<object> </returns>
    /// <history>
    /// [edgrodriguez] 19/04/2016 Created
    /// </history>
    public static List<object> GetRptDepositsNoShow(DateTime dtmStart, DateTime dtmEnd, string leadSources)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstDepNoShow = dbContext.USP_OR_RptDepositsNoShow(dtmStart, dtmEnd, leadSources).ToList();

        var currencies = (from gift in lstDepNoShow.Select(c => c.gucu).Distinct()
                          join curr in dbContext.Currencies on gift equals curr.cuID
                          select curr).ToList();

        var payType = (from gift in lstDepNoShow.Select(c => c.gupt).Distinct()
                       join payT in dbContext.PaymentTypes on gift equals payT.ptID
                       select payT).ToList();

        return (lstDepNoShow.Count > 0) ? new List<object> { lstDepNoShow, currencies, payType } : new List<object>();
      }
    }

    #endregion GetRptDepositsNoShow

    #region GetRptInOutByPR

    /// <summary>
    /// Obtiene el reporte In & Out By PR
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="leadSources"></param>
    /// <returns> List<RptInOutByPR> </returns>
    /// <history>
    /// [edgrodriguez] 19/04/2016 Created
    /// </history>
    public static List<RptInOutByPR> GetRptInOutByPR(DateTime dtmStart, DateTime dtmEnd, string leadSources)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstInOutPR = dbContext.USP_OR_RptInOutByPR(dtmStart, dtmEnd, leadSources).ToList();
        return lstInOutPR;
      }
    }

    #endregion GetRptInOutByPR

    #region GetRptPersonnelAccess

    /// <summary>
    /// Obtiene el reporte Personnel Access
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="leadSources"></param>
    /// <returns> List<RptPersonnelAccess> </returns>
    /// <history>
    /// [edgrodriguez] 19/04/2016 Created
    /// </history>
    public static List<RptPersonnelAccess> GetRptPersonnelAccess(string leadSources)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstPersonnelAccess = dbContext.USP_OR_RptPersonnelAccess(leadSources).ToList();
        return lstPersonnelAccess;
      }
    }

    #endregion GetRptPersonnelAccess

    #region GetRptSelfGen

    /// <summary>
    /// Obtiene el reporte Personnel Access
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="leadSources"></param>
    /// <returns> Tuple<List<RptSelfGen>,List<Sale>, List<SaleType>> </returns>
    /// <history>
    /// [edgrodriguez] 25/04/2016 Created
    /// </history>
    public static Tuple<List<RptSelfGen>, List<Sale>, List<Personnel>> GetRptSelfGen(DateTime dtmStart, DateTime dtmEnd, string leadSources)
    {
      List<string> lstLeadSources = leadSources.Split(',').ToList();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstRptSelfGen = dbContext.sprptSelfGen(dtmStart, dtmEnd, leadSources).ToList();

        var lstSales = (from s in dbContext.Sales
                        where (s.saD >= dtmStart && s.saD <= dtmEnd ||
                               s.saProcD >= dtmStart && s.saProcD <= dtmEnd ||
                               s.saCancelD >= dtmStart && s.saCancelD <= dtmEnd) &&
                              lstLeadSources.Contains(s.sals) && s.saSelfGen
                        select s
          ).ToList();

        var lstSaleType = (from s in lstSales.Select(c => c.sast).Distinct()
                           join st in dbContext.SaleTypes on s equals st.stID
                           select st
          ).ToList();

        lstSales.ForEach(c =>
        {
          c.sast = lstSaleType.First(st => st.stID == c.sast).ststc;
        });

        var lstPersonnel = (from pe in dbContext.Personnels
                            join sg in lstRptSelfGen.Select(c => c.guPRInvit1).Distinct() on pe.peID equals sg
                            select pe
          ).Distinct().ToList();

        lstPersonnel.AddRange((from pe in dbContext.Personnels
                               join s in lstSales.Select(c => c.saPR1).Distinct() on pe.peID equals s
                               select pe
          ).Distinct().ToList());

        return Tuple.Create(lstRptSelfGen, lstSales, lstPersonnel.Distinct().ToList());
      }
    }

    #endregion GetRptSelfGen

    #endregion Processor General
  }
}
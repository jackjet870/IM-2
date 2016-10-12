using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System.Collections.Generic;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;

namespace IM.ProcessorInhouse.Classes
{
  internal static class clsFormatReport
  {
    #region GetRptCostByPRFormat

    /// <summary>
    /// Formato para el reporte Cost by PR
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// [erosado] 02/09/2016  Modified. Se agregó el nuevo formato
    /// </history>
    internal static ColumnFormatList GetRptCostByPRFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("PR ID", "PR", aligment: ExcelHorizontalAlignment.Right);
      lst.Add("PR Name", "PRN");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, aligment: ExcelHorizontalAlignment.Right);
      lst.Add("Total Cost", "TotalCost", format: EnumFormatTypeExcel.Currency, aligment: ExcelHorizontalAlignment.Right);
      lst.Add("Average Cost", "AverageCost", format: EnumFormatTypeExcel.Currency, aligment: ExcelHorizontalAlignment.Right, isCalculated: true, formula: "IF([Shows]=0,0,[TotalCost]/[Shows])");
      return lst;
    }

    #endregion GetRptCostByPRFormat

    #region GetRptCostByPRWithDetailGiftsFormat

    /// <summary>
    /// Formato para el reporte Cost by PR With Detail Gifts
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptCostByPRWithDetailGiftsFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("PR", "PR", isGroup: true, isVisible: false);
      lst.Add("Qty","Qty", aligment: ExcelHorizontalAlignment.Right, format: EnumFormatTypeExcel.Number);
      lst.Add("Gift ID","GiftsID", aligment: ExcelHorizontalAlignment.Right, sort: eSortType.Descending);
      lst.Add("Gift Name","GiftsName");

      lst.Add("Shows","Shows", format: EnumFormatTypeExcel.Number, aligment: ExcelHorizontalAlignment.Right, function:DataFieldFunctions.Sum);
      lst.Add("Total Cost","TotalCost", format: EnumFormatTypeExcel.Currency, aligment: ExcelHorizontalAlignment.Right, function: DataFieldFunctions.Sum);
      lst.Add("Average Cost", "AverageCost", format: EnumFormatTypeExcel.Currency, aligment: ExcelHorizontalAlignment.Right, isCalculated: true, formula: "IF([Shows]=0,0,[TotalCost]/[Shows])");

      return lst;
    }

    #endregion GetRptCostByPRWithDetailGiftsFormat

    #region GetRptFollowUpByAgencyFormat

    /// <summary>
    /// Formato para el reporte Follow Up by Agency
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptFollowUpByAgencyFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("OriginallyAvailable", "OriginallyAvailable", isGroup: true, isVisible: false);
      lst.Add("Market", "Market", isGroup: true, isVisible: false);
      lst.Add("Agency ID", "Agency");
      lst.Add("Agency Name", "AgencyN");

      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Follow", "FollowUps", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("FU%", "FollowUpsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[FollowUps]/[Availables])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([FollowUps] =0,0,[Books]/[FollowUps])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Amount", "SalesAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows] =0,0, [SalesAmount]/[Shows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows] =0,0,[Sales]/[Shows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales] =0,0, [SalesAmount]/[Sales])");
      return lst;
    }

    #endregion GetRptFollowUpByAgencyFormat

    #region GetRptFollowUpByPRFormat

    /// <summary>
    /// Formato para el reporte Follow Up by PR
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptFollowUpByPRFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Status", "Status", isGroup: true, isVisible: false);
      lst.Add("PR ID", "PRID");
      lst.Add("PR Name", "PRN");

      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Follow", "FollowUps", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("FU%", "FollowUpsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[FollowUps]/[Availables])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([FollowUps] =0,0,[Books]/[FollowUps])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Amount", "SalesAmount", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows] =0,0, [SalesAmount]/[Shows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows] =0,0, [SalesAmount]/[Shows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales] =0,0, [SalesAmount]/[Sales])");
      return lst;
    }

    #endregion GetRptFollowUpByPRFormat

    #region GetRptProductionByAgeInhouseFormat

    /// <summary>
    /// Formato para el reporte Production by Age
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptProductionByAgeInhouseFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Age Range", "Age");
      lst.Add("Arrivs", "Arrivals", format: EnumFormatTypeExcel.Number, function:DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated:true, formula: "IF([Arrivals] =0,0,[Contacts]/[Arrivals])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[GrossBooks]/[Availables])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");

      lst.Add("Sales", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Sales", "Sales_PR", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Amount", "SalesAmount_PR", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Sales", "Sales_SELFGEN", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");
      lst.Add("Amount", "SalesAmount_SELFGEN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([GrossShows] =0,0,[SalesAmount_TOTAL]/[GrossShows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossShows] =0,0,[Sales_TOTAL]/[GrossShows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] = 0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      return lst;
    }

    #endregion GetRptProductionByAgeInhouseFormat

    #region GetProductionByAgeMarketOriginallyAvailableInhouseFormat

    /// <summary>
    /// Formato para el reporte Production by Age, Market & Originally Available
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetProductionByAgeMarketOriginallyAvailableInhouseFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("OriginallyAvailable", "OriginallyAvailable", isGroup: true, isVisible: false);
      lst.Add("Market", "Market", isGroup: true, isVisible: false);
      lst.Add("Age Range", "Age");
      lst.Add("Arrivs", "Arrivals", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals] =0,0,[Contacts]/[Arrivals])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[GrossBooks]/[Availables])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");

      lst.Add("Sales", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Sales", "Sales_PR", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Amount", "SalesAmount_PR", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Sales", "Sales_SELFGEN", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");
      lst.Add("Amount", "SalesAmount_SELFGEN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([GrossShows] =0,0,[SalesAmount_TOTAL]/[GrossShows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossShows] =0,0,[Sales_TOTAL]/[GrossShows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] = 0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      return lst;
    }

    #endregion GetProductionByAgeMarketOriginallyAvailableInhouseFormat

    #region GetRptProductionByContractAgencyInhouseFormat

    /// <summary>
    /// Formato para el reporte Production by Contract & Agency
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptProductionByContractAgencyInhouseFormat()
    {
      //Camvias
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("AgencyN", "AgencyN", isGroup: true, isVisible: false);
      lst.Add("Contract ID", "Contract");
      lst.Add("Contract Name", "ContractN");
      lst.Add("Arrivs", "Arrivals", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals] =0,0,[Contacts]/[Arrivals])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[GrossBooks]/[Availables])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");

      lst.Add("Sales", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Sales", "Sales_PR", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Amount", "SalesAmount_PR", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Sales", "Sales_SELFGEN", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");
      lst.Add("Amount", "SalesAmount_SELFGEN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([GrossShows] =0,0,[SalesAmount_TOTAL]/[GrossShows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossShows] =0,0,[Sales_TOTAL]/[GrossShows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] = 0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      lst.Add("Agency", "Agency", isVisible: false);
      return lst;
    }

    #endregion GetRptProductionByContractAgencyInhouseFormat

    #region GetRptProductionByContractAgencyMarketOriginallyAvailableInhouseFormat

    /// <summary>
    /// Formato para el reporte Production by Contract, Agency, Market & Originally Available
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptProductionByContractAgencyMarketOriginallyAvailableInhouseFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Originally Available", "OriginallyAvailable", isGroup: true, isVisible: false);
      lst.Add("Market", "Market", isGroup: true, isVisible: false);
      lst.Add("AgencyN", "AgencyN", isGroup: true, isVisible: false);
      lst.Add("Contract ID", "Contract");
      lst.Add("Contract Name", "ContractN");
      lst.Add("Arrivs", "Arrivals", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals] =0,0,[Contacts]/[Arrivals])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[GrossBooks]/[Availables])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");

      lst.Add("Sales", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Sales", "Sales_PR", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Amount", "SalesAmount_PR", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Sales", "Sales_SELFGEN", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");
      lst.Add("Amount", "SalesAmount_SELFGEN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([GrossShows] =0,0,[SalesAmount_TOTAL]/[GrossShows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossShows] =0,0,[Sales_TOTAL]/[GrossShows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] = 0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      lst.Add("Agency", "Agency", isVisible: false);

      return lst;
    }

    #endregion GetRptProductionByContractAgencyMarketOriginallyAvailableInhouseFormat

    #region GetRptProductionByCoupleTypeInhouseFormat

    /// <summary>
    /// Formato para el reporte Production by Couple Type
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptProductionByCoupleTypeInhouseFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Couple Type", "CoupleType");
      lst.Add("Arrivs", "Arrivals", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals] =0,0,[Contacts]/[Arrivals])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[GrossBooks]/[Availables])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");

      lst.Add("Sales", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Sales", "Sales_PR", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Amount", "SalesAmount_PR", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Sales", "Sales_SELFGEN", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");
      lst.Add("Amount", "SalesAmount_SELFGEN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([GrossShows] =0,0,[SalesAmount_TOTAL]/[GrossShows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossShows] =0,0,[Sales_TOTAL]/[GrossShows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] = 0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      return lst;
    }

    #endregion GetRptProductionByCoupleTypeInhouseFormat

    #region GetRptProductionByDeskInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by Desk
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptProductionByDeskInhouseFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Desk", "Desk");
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[GrossBooks]/[Availables])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");

      lst.Add("Sales", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Sales", "Sales_PR", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Amount", "SalesAmount_PR", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Sales", "Sales_SELFGEN", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");
      lst.Add("Amount", "SalesAmount_SELFGEN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([GrossShows] =0,0,[SalesAmount_TOTAL]/[GrossShows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossShows] =0,0,[Sales_TOTAL]/[GrossShows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] = 0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      return lst;
    }

    #endregion GetRptProductionByDeskInhouseFormat

    #region GetRptProductionByGroupInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by Group
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetRptProductionByGroupInhouseFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Groups", "Groups", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Rooms", "Integrants", format: EnumFormatTypeExcel.Number);
      lst.Add("T Rooms", "TotalIntegrants", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Arrivs", "Arrivals", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals] =0,0,[Contacts]/[Arrivals])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[GrossBooks]/[Availables])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");

      lst.Add("Sales", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Sales", "Sales_PR", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Amount", "SalesAmount_PR", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Sales", "Sales_SELFGEN", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");
      lst.Add("Amount", "SalesAmount_SELFGEN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([GrossShows] =0,0,[SalesAmount_TOTAL]/[GrossShows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossShows] =0,0,[Sales_TOTAL]/[GrossShows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] = 0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      return lst;
    }

    #endregion GetRptProductionByGroupInhouseFormat

    #region GetRptProductionByGuestStatusInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by Guest Status
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptProductionByGuestStatusInhouseFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Lead Source", "LeadSource", isGroup: true, isVisible: false);
      lst.Add("Guest Status", "GuestStatus");
      lst.Add("Arrivs", "Arrivals", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals] =0,0,[Contacts]/[Arrivals])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[GrossBooks]/[Availables])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");

      lst.Add("Sales", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Sales", "Sales_PR", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Amount", "SalesAmount_PR", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Sales", "Sales_SELFGEN", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");
      lst.Add("Amount", "SalesAmount_SELFGEN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([GrossShows] =0,0,[SalesAmount_TOTAL]/[GrossShows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossShows] =0,0,[Sales_TOTAL]/[GrossShows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] = 0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      return lst;
    }

    #endregion GetRptProductionByGuestStatusInhouseFormat

    #region GetRptProductionByMemberTypeAgencyInhouseFormat

    ///<summary>
    /// Formato para el reporte  Production by Member Type & Agency
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptProductionByMemberTypeAgencyInhouseFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("AgencyN", "AgencyN", isGroup: true, isVisible: false);
      lst.Add("Member Type", "MemberType");
      lst.Add("Arrivs", "Arrivals", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals] =0,0,[Contacts]/[Arrivals])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[GrossBooks]/[Availables])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");

      lst.Add("Sales", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Sales", "Sales_PR", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Amount", "SalesAmount_PR", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Sales", "Sales_SELFGEN", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");
      lst.Add("Amount", "SalesAmount_SELFGEN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([GrossShows] =0,0,[SalesAmount_TOTAL]/[GrossShows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossShows] =0,0,[Sales_TOTAL]/[GrossShows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] = 0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      lst.Add("Agency", "Agency", isVisible: false);
      return lst;
    }

    #endregion GetRptProductionByMemberTypeAgencyInhouseFormat

    #region GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by Member Type, Agency, Market & Originally Available
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouseFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Originally Available", "OriginallyAvailable", isGroup: true, isVisible: false);
      lst.Add("Market", "Market", isGroup: true, isVisible: false);
      lst.Add("AgencyN", "AgencyN", isGroup: true, isVisible: false);
      lst.Add("Member Type", "MemberType");
      lst.Add("Arrivs", "Arrivals", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals] =0,0,[Contacts]/[Arrivals])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[GrossBooks]/[Availables])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");

      lst.Add("Sales", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Sales", "Sales_PR", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Amount", "SalesAmount_PR", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Sales", "Sales_SELFGEN", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");
      lst.Add("Amount", "SalesAmount_SELFGEN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([GrossShows] =0,0,[SalesAmount_TOTAL]/[GrossShows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossShows] =0,0,[Sales_TOTAL]/[GrossShows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] = 0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      lst.Add("Agency", "Agency", isVisible: false);
      return lst;
    }

    #endregion GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouseFormat

    #region GetRptProductionByPRInhouseFormat

    ///<summary>
    /// Formato para el reporte  Production by PR
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptProductionByPRInhouseFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Status", "Status", isGroup: true, isVisible: false);
      lst.Add("PR ID", "PRID");
      lst.Add("PR Name", "PRN");
      lst.Add("Assigns", "Assigns", format: EnumFormatTypeExcel.Number, function:DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Assigns] =0,0,[Contacts]/[Assigns])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "BooksNoDirects", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[BooksNoDirects]/[Availables])");
      lst.Add("Sh-D", "ShowsNoDirects", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("IO", "InOuts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("WO", "WalkOuts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("TR", "Tours", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("CT", "CourtesyTours", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Save", "SaveTours", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Tours", "TotalTours", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("UPS", "UPS", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sales", "Sales_PROC", format: EnumFormatTypeExcel.Number, superHeader: "PROCESSABLE");
      lst.Add("Amount", "SalesAmount_PROC", format: EnumFormatTypeExcel.Currency, superHeader: "PROCESSABLE");
      lst.Add("Sales ", "Sales_PEND", format: EnumFormatTypeExcel.Number, superHeader: "PENDING");
      lst.Add("Amount ", "SalesAmount_PEND", format: EnumFormatTypeExcel.Currency, superHeader: "PENDING");
      lst.Add("Sales  ", "Sales_OOP", format: EnumFormatTypeExcel.Number, superHeader: "OUT OF PENDING");
      lst.Add("Amount  ", "SalesAmount_OOP", format: EnumFormatTypeExcel.Currency, superHeader: "OUT OF PENDING");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([UPS] =0,0,[SalesAmount_PROC]/[UPS])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([UPS] =0,0,[Sales_PROC]/[UPS])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_PROC]= 0,0,[SalesAmount_PROC]/[Sales_PROC])");
      lst.Add("Location", "Location", isVisible:false);
      lst.Add("Team", "Team", isVisible: false);
      lst.Add("Leader", "Leader", isVisible: false);
      return lst;
    }

    #endregion GetRptProductionByPRInhouseFormat

    #region GetRptProductionByPRGroupInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by PR & Group
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetRptProductionByPRGroupInhouseFormat()
    {
      return new List<ColumnFormat>
      {
        new ColumnFormat{Title = "PR",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default, Compact = true, Outline = true, InsertBlankRow = true},
        new ColumnFormat{Title = "Type",Order = 2, Axis = ePivotFieldAxis.Row, Outline = true},
        new ColumnFormat{Title = "Groups",Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat{Title = "Rooms",Order = 3, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat{Title = "T Rooms",Order = 1, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Assign", Order = 2, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Conts", Order = 3,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Avails",Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Directs", Order = 8, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "T Books", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Shows", Order = 11, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "T Shows", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ColumnFormat {Title = "Sales", PropertyName = "Sales Total", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ColumnFormat {Title = "Amount",  PropertyName = "Amount Total", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ColumnFormat {Title = "Sales ", PropertyName = "Sales PR", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ColumnFormat {Title = "Amount ", PropertyName = "Amount PR", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ColumnFormat {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 18, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ColumnFormat {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 19, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ColumnFormat {Title = "C%",Order = 4,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Assign =0,0,Conts/Assign)" },
        new ColumnFormat {Title = "A%",Order = 6 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ColumnFormat {Title = "Bk%",Order = 10 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ColumnFormat {Title = "Sh%",Order = 13 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ColumnFormat { Title = "Cl%", Order = 21, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ColumnFormat { Title = "Eff", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ColumnFormat { Title = "Avg Sale", Order = 22, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByPRGroupInhouseFormat

    #region GetRptProductionByPRSalesRoomInhouseFormat

    ///<summary>
    /// Formato para el reporte  Production by PR & Sales Room
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetRptProductionByPRSalesRoomInhouseFormat()
    {
      return new List<ColumnFormat>
      {
        new ColumnFormat{Title = "Sales Rom",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true,Compact = true},
        new ColumnFormat{Title = "Status",Order = 1, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true,Compact = true},
        new ColumnFormat{Title = "PR ID",Order = 2, Axis = ePivotFieldAxis.Row},
        new ColumnFormat{Title = "PR Name",Order = 3, Axis = ePivotFieldAxis.Row},

        new ColumnFormat {Title = "Assigns", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Sh-D", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Shows", Order = 11, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "IO", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "WO", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "RT", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "CT", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Save", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "T Tours", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "UPS", Order = 18, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Sales", PropertyName = "Sales Proc", Order = 19, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PROCESSABLE"},
        new ColumnFormat {Title = "Amount",  PropertyName = "Amount Proc", Order = 20, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="PROCESSABLE"},
        new ColumnFormat {Title = "Sales ", PropertyName = "Sales Pend", Order = 21, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PENDING"},
        new ColumnFormat {Title = "Amount ", PropertyName = "Amount Pend", Order = 22, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PENDING"},
        new ColumnFormat {Title = "Sales  ", PropertyName = "Sales Out Pend", Order = 23, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="OUT OF PENDING"},
        new ColumnFormat {Title = "Amount  ", PropertyName = "Amount Out Pend", Order = 24, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="OUT OF PENDING"},

        new ColumnFormat {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Assigns =0,0,Conts/Assigns)" },
        new ColumnFormat {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ColumnFormat {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ColumnFormat {Title = "Sh%",Order = 10 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ColumnFormat { Title = "Cl%", Order = 26, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF(UPS =0,0,'Sales Proc'/UPS)" },
        new ColumnFormat { Title = "Eff", Order = 25, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF(UPS =0,0,'Amount Proc'/UPS)" },
        new ColumnFormat { Title = "Avg Sale", Order = 27, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Proc' = 0,0,'Amount Proc'/'Sales Proc')" }
      };
    }

    #endregion GetRptProductionByPRSalesRoomInhouseFormat

    #region GetRptRepsPaymentFormat

    /// <summary>
    /// Formato para el reporte Reps Payment
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetRptRepsPaymentFormat()
    {
      return new List<ColumnFormat>
      {
        new ColumnFormat{Title = "Rep",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default},
        new ColumnFormat {Title = "Book Date", Order = 1, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.Date,Alignment = ExcelHorizontalAlignment.Center},
        new ColumnFormat {Title = "Guest Name", Order = 2,  Axis = ePivotFieldAxis.Row},
        new ColumnFormat {Title = "Q", Order = 3, Axis = ePivotFieldAxis.Row, Alignment = ExcelHorizontalAlignment.Center},
        new ColumnFormat {Title = "Show Pay", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency},
        new ColumnFormat {Title = "Sale", Order = 1, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Sale Pay", Order = 2, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency},
        new ColumnFormat {Title = "Total", Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency}
      };
    }

    #endregion GetRptRepsPaymentFormat

    #region GetRptRepsPaymentSummaryFormat

    /// <summary>
    /// Formato para el reporte Reps Payment Summary
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptRepsPaymentSummaryFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();

      lst.Add("Rep", "agrp");
      lst.Add("#", "TotalShow",  format: EnumFormatTypeExcel.Number, aligment: ExcelHorizontalAlignment.Center,superHeader:"Show", function: DataFieldFunctions.Sum);
      lst.Add("Shows Payment", "agShowPay",  format: EnumFormatTypeExcel.Currency,  function: DataFieldFunctions.Sum, superHeader: "Show");
      lst.Add("Sub Total", "SumagShowPay",  format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "Show");
      lst.Add("# ", "TotalSales",  format: EnumFormatTypeExcel.Number, aligment: ExcelHorizontalAlignment.Center, function: DataFieldFunctions.Sum, superHeader: "Sales");
      lst.Add("Sales Payment", "agSalePay",  format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "Sales");
      lst.Add("Sub Total ", "SumSalesPay",  format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "Sales");
      lst.Add("Total", "TotalPay",  format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      return lst;
    }

    #endregion GetRptRepsPaymentSummaryFormat

    #region GetRptProductionByNationalityInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by Nationality
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptProductionByNationalityInhouseFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Nationality", "Nationality");
      lst.Add("Arrivs", "Arrivals", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals] =0,0,[Contacts]/[Arrivals])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[GrossBooks]/[Availables])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");

      lst.Add("Sales", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Sales", "Sales_PR", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Amount", "SalesAmount_PR", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Sales", "Sales_SELFGEN", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");
      lst.Add("Amount", "SalesAmount_SELFGEN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([GrossShows] =0,0,[SalesAmount_TOTAL]/[GrossShows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossShows] =0,0,[Sales_TOTAL]/[GrossShows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] = 0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      return lst;
    }

    #endregion GetRptProductionByNationalityInhouseFormat

    #region GetProductionByNationalityMarketOriginallyAvailableInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by Nationality, Market & Originally Available
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetProductionByNationalityMarketOriginallyAvailableInhouseFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Originally Available", "OriginallyAvailable", isGroup: true, isVisible: false);
      lst.Add("Market", "Market", isGroup: true, isVisible: false);
      lst.Add("Nationality", "Nationality");
      lst.Add("Arrivs", "Arrivals", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals] =0,0,[Contacts]/[Arrivals])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[GrossBooks]/[Availables])");
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Books] =0,0,[Shows]/[Books])");

      lst.Add("Sales", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Sales", "Sales_PR", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Amount", "SalesAmount_PR", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "PR");
      lst.Add("Sales", "Sales_SELFGEN", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");
      lst.Add("Amount", "SalesAmount_SELFGEN", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, superHeader: "SELF GEN");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([GrossShows] =0,0,[SalesAmount_TOTAL]/[GrossShows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossShows] =0,0,[Sales_TOTAL]/[GrossShows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] = 0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      return lst;
    }

    #endregion GetProductionByNationalityMarketOriginallyAvailableInhouseFormat

    #region GetRptProductionByGiftQuantityFormat

    ///<summary>
    /// Formato para el reporte Production by Gift & Quantity
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 19/04/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetRptProductionByGiftQuantityFormat()
    {
      return new List<ColumnFormat>
      {
        new ColumnFormat{Title = "Gift",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true,Compact = true},
        new ColumnFormat{Title = "PR ID",Order = 1, Axis = ePivotFieldAxis.Row},
        new ColumnFormat{Title = "PR Name",Order = 2, Axis = ePivotFieldAxis.Row},
        new ColumnFormat {Title = "Quantity", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Amount", Order = 1, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency},
        new ColumnFormat {Title = "Books", Order = 2, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Directs", Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "T Books", Order = 4, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Deposits", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Shows", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
         new ColumnFormat {Title = "IO", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "T Shows", Order = 8, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "SG", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ColumnFormat {Title = "Sales", PropertyName = "Sales Total", Order = 11, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ColumnFormat {Title = "Amount ",  PropertyName = "Amount Total", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ColumnFormat {Title = "Sales ", PropertyName = "Sales PR", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ColumnFormat {Title = "Amount  ", PropertyName = "Amount PR", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ColumnFormat {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ColumnFormat {Title = "Amount   ", PropertyName = "Amount Self Gen", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

       new ColumnFormat {Title = "Sh%",Order = 9 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ColumnFormat { Title = "Cl%", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ColumnFormat { Title = "Eff", Order = 17, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ColumnFormat { Title = "Avg Sale", Order = 19, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByGiftQuantityFormat

    #region GetRptGiftsReceivedBySRFormat

    ///<summary>
    /// Formato para el reporte Gifts Received by Sales Room
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 22/04/2016 Created
    /// </history>
    internal static ColumnFormatList GetRptGiftsReceivedBySRFormat()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Sales Room", "SalesRoom", format: EnumFormatTypeExcel.General, axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Gift ID", "Gift", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row);
      lst.Add("Gift Name", "GiftN", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row);
      lst.Add("Quantity", "Quantity", format: EnumFormatTypeExcel.Number, aligment: ExcelHorizontalAlignment.Right, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Couples", "Couples", format: EnumFormatTypeExcel.Number, aligment: ExcelHorizontalAlignment.Right, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Adults", "Adults", format: EnumFormatTypeExcel.Number, aligment: ExcelHorizontalAlignment.Right, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Minors", "Minors", format: EnumFormatTypeExcel.Number, aligment: ExcelHorizontalAlignment.Right, axis: ePivotFieldAxis.Row,function: DataFieldFunctions.Sum);
      lst.Add("Amount", "Amount", format: EnumFormatTypeExcel.Currency, aligment: ExcelHorizontalAlignment.Right, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("Currency", "cuN", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Center, axis: ePivotFieldAxis.Column);
      lst.Add("cuID", "cuID",isVisible: false);
      return lst;
    }

    #endregion GetRptGiftsReceivedBySRFormat

    #region GetRptUnavailableMotivesByAgencyFormat

    ///<summary>
    /// Formato para el reporte  Unavailable Motives By Agency
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 22/04/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetRptUnavailableMotivesByAgencyFormat()
    {
      return new List<ColumnFormat>
      {
        new ColumnFormat{Title = "Unavail Motive",Order = 0, Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Default, Outline = true, Compact = true, InsertBlankRow = true},
        new ColumnFormat{Title = "Market",Order = 1, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default, Outline = true, Compact = true, InsertBlankRow = true},
        new ColumnFormat{Title = "AgencyID",Order = 2, Axis = ePivotFieldAxis.Row},
        new ColumnFormat {Title = "Agency", Order = 3, Axis = ePivotFieldAxis.Row},

        new ColumnFormat {Title = "Arrivals", Order = 0,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right },
        new ColumnFormat {Title = "% Arrivals",Order = 1, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Right },
        new ColumnFormat {Title = "ByUser", Order = 2, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right }
      };
    }

    #endregion GetRptUnavailableMotivesByAgencyFormat

    #region GetRptShowFactorByBookingDateFormat

    ///<summary>
    /// Formato para el reporte  Show Factor by Booking Date
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 22/04/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetRptShowFactorByBookingDateFormat()
    {
      return new List<ColumnFormat>
      {
        new ColumnFormat{Title = "Category",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default ,Outline = true ,Compact = true, InsertBlankRow = true},
        new ColumnFormat{Title = "Agency ID",Order = 1, Axis = ePivotFieldAxis.Row},
        new ColumnFormat{Title = "Agency Name",Order = 2, Axis = ePivotFieldAxis.Row},

        new ColumnFormat {Title = "Diff", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right},
        new ColumnFormat {Title = "Books", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right},
        new ColumnFormat {Title = "Shows",Order = 2, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right},
        new ColumnFormat {Title = "IO", Order = 4, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right},
        new ColumnFormat {Title = "WO", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right},

        new ColumnFormat {Title = "Sh%",Order = 3 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" , Alignment = ExcelHorizontalAlignment.Right}
      };
    }

    #endregion GetRptShowFactorByBookingDateFormat

    #region GetRptProductionByAgencyMonthlyFormat

    ///<summary>
    /// Formato para el reporte Production by Agency (Monthly)
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 25/04/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetRptProductionByAgencyMonthlyFormat()
    {
      return new List<ColumnFormat>
      {
        new ColumnFormat{Title = "Year",Order = 0, Axis = ePivotFieldAxis.Row, Outline = true,Compact = true},
        new ColumnFormat{Title = "Agency",Order = 1, Axis = ePivotFieldAxis.Row, Outline = true, Compact = true,InsertBlankRow = true},
        new ColumnFormat{Title = "Lead Source",Order = 2, Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Default, Outline = true, Compact = true, InsertBlankRow = true},
        new ColumnFormat{Title = "Month",Order = 3, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.Month,Sort = eSortType.Ascending},
        new ColumnFormat {Title = "Arrivs", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Avails",Order = 2, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Books", Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "T Shows", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ColumnFormat {Title = "Sales", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number },
        new ColumnFormat {Title = "VOL",  Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency},

        new ColumnFormat {Title = "C%",Order = 14,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,Sales/'T Shows')" },
        new ColumnFormat { Title = "Eff", Order = 15, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,VOL/'T Shows')" },
      };
    }

    #endregion GetRptProductionByAgencyMonthlyFormat

    #region GetGraphUnavailableArrivalsFormat

    ///<summary>
    /// Formato para el reporte Unavailable Arrivals (Graphic)
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 27/04/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetGraphUnavailableArrivalsFormat()
    {
      return new List<ColumnFormat>
      {
        new ColumnFormat{Title = "Motive",Order = 0, Axis = ePivotFieldAxis.Column},
        new ColumnFormat{Title = "%",Order = 1, Axis = ePivotFieldAxis.Row, Alignment = ExcelHorizontalAlignment.Center, Format = EnumFormatTypeExcel.Percent},
        new ColumnFormat{Title = "By User",Order = 2, Axis = ePivotFieldAxis.Row, Alignment = ExcelHorizontalAlignment.Right, Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Arrivals", Order = 3, Axis = ePivotFieldAxis.Values, Alignment = ExcelHorizontalAlignment.Right },
      };
    }

    #endregion GetGraphUnavailableArrivalsFormat

    #region GetGraphNotBookingArrivalsFormat

    ///<summary>
    /// Formato para el reporte Not Booking Arrivals (Graphic)
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 27/04/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetGraphNotBookingArrivalsFormat()
    {
      return new List<ColumnFormat>
      {
        new ColumnFormat{Title = "Motive",Order = 0, Axis = ePivotFieldAxis.Column},
        new ColumnFormat{Title = "%",Order = 1, Axis = ePivotFieldAxis.Row, Alignment = ExcelHorizontalAlignment.Center, Format = EnumFormatTypeExcel.Percent},
        new ColumnFormat{Title = "By User",Order = 2, Axis = ePivotFieldAxis.Values, Alignment = ExcelHorizontalAlignment.Right, Format = EnumFormatTypeExcel.Number},
      };
    }

    #endregion GetGraphNotBookingArrivalsFormat

    #region GetRptProductionByMemberFormat

    ///<summary>
    /// Formato para el reporte Production by Member
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 04/05/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetRptProductionByMemberFormat()
    {
      return new List<ColumnFormat>
      {
        new ColumnFormat{Title = "Wholesaler",Order = 0, Axis = ePivotFieldAxis.Row, Compact = true, Outline = true, InsertBlankRow = true, SubTotalFunctions = eSubTotalFunctions.Default},
        new ColumnFormat{Title = "Club",Order = 1, Axis = ePivotFieldAxis.Row,Compact = true, Outline = true, InsertBlankRow = true, SubTotalFunctions = eSubTotalFunctions.Default},
        new ColumnFormat{Title = "Guest Type",Order = 2, Axis = ePivotFieldAxis.Row,Compact = true, Outline = true, InsertBlankRow = true, SubTotalFunctions = eSubTotalFunctions.Default},
        new ColumnFormat {Title = "Co#", Order = 3, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Application", Order = 4, Axis = ePivotFieldAxis.Row},
        new ColumnFormat {Title = "Name", Order = 5, Axis = ePivotFieldAxis.Row},
        new ColumnFormat {Title = "Date", Order = 6, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.Date},
        new ColumnFormat {Title = "Amount   ", Order = 7, Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Currency},

        new ColumnFormat {Title = "Arrivs", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "T Shows", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ColumnFormat {Title = "Sales", PropertyName = "Sales Total", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ColumnFormat {Title = "Amount",  PropertyName = "Amount Total", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ColumnFormat {Title = "Sales ", PropertyName = "Sales PR", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ColumnFormat {Title = "Amount ", PropertyName = "Amount PR", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ColumnFormat {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ColumnFormat {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ColumnFormat {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Arrivs =0,0,Conts/Arrivs)" },
        new ColumnFormat {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ColumnFormat {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ColumnFormat {Title = "Sh%",Order = 11 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ColumnFormat { Title = "Cl%", Order = 19, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ColumnFormat { Title = "Eff", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ColumnFormat { Title = "Avg Sale", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByMemberFormat

    #region GetRptProductionByMonthFormat

    ///<summary>
    /// Formato para el reporte Ocupation,Contact, books & Shows
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 05/05/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetRptProductionByMonthFormat()
    {
      return new List<ColumnFormat>
      {
        new ColumnFormat{Title = "Year",Order = 0, Axis = ePivotFieldAxis.Column, SubTotalFunctions = eSubTotalFunctions.Default},
        new ColumnFormat{Title = "Month",Order = 1, Axis = ePivotFieldAxis.Column, Format = EnumFormatTypeExcel.Month},

        new ColumnFormat {Title = "Conts", Order = 0,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Avails",Order = 1, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Books", Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "T Books", Order = 4, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Shows", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "T Shows", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ColumnFormat {Title = "Sales", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Amount", Order = 11, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency},

        new ColumnFormat {Title = "A%",Order = 2 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ColumnFormat {Title = "Bk%",Order = 5 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ColumnFormat {Title = "Sh%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ColumnFormat { Title = "Cl%", Order = 10, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales'/'T Shows')" },
        new ColumnFormat { Title = "Eff", Order = 13, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount'/'T Shows')" },
        new ColumnFormat { Title = "Avg Sale", Order = 12, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales' = 0,0,'Amount'/'Sales')" }
      };
    }

    #endregion GetRptProductionByMonthFormat

    #region RptProductionByAgencyInhouse

    ///<summary>
    /// Formato para el reporte Production by Agency
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 16/05/2016 Created
    /// </history>
    internal static ColumnFormatList RptProductionByAgencyInhouse()
    {
      ColumnFormatList lst = new ColumnFormatList();

      lst.Add("OriginallyAvailable", "OriginallyAvailable", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Market", "MarketN", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Agency ID", "Agency", axis: ePivotFieldAxis.Row);
      lst.Add("Agency Name", "AgencyN", axis: ePivotFieldAxis.Row);
      lst.Add("Arrivs", "Arrivals", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Conts", "Contacts", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("C%", "ContactsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Arrivals]=0,0,[Contacts]/[Arrivals])");
      lst.Add("Avails", "Availables", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("A%", "AvailablesFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Contacts] =0,0,[Availables]/[Contacts])");
      lst.Add("Books", "GrossBooks", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Directs", "Directs", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("T Books", "Books", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Bk%", "BooksFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Availables] =0,0,[Books]/[Availables])");
      lst.Add("Shows", "GrossShows", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "Shows", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossBooks] =0,0,[GrossShows]/[GrossBooks])");
      lst.Add("IO", "InOuts", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("WO", "WalkOuts", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("RT", "Tours", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("CT", "CourtesyTours", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Save", "SaveTours", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("T Tours", "TotalTours", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("UPS", "UPS", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);

      lst.Add("Sales", "PartialSales", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Amount", "PartialSalesAmount", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);

      lst.Add("Sales", "Sales", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum, superHeader:"TOTAL");
      lst.Add("Amount", "SalesAmount", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum, superHeader: "TOTAL");

      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([UPS] =0,0,[SalesAmount]/[UPS])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([UPS] =0,0,[Sales]/[UPS])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales] =0,0,[SalesAmount]/[Sales])");
      lst.Add("mtN", "mtN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      return lst;
    }

    #endregion RptProductionByAgencyInhouse

    #region RptScoreByPR

    ///<summary>
    /// Formato para el reporte Score by PR
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 16/05/2016 Created
    /// </history>
    public static ColumnFormatList RptScoreByPR()
    {
      ColumnFormatList lst = new ColumnFormatList();

      lst.Add("PR ID", "PR", axis: ePivotFieldAxis.Row);
      lst.Add("PR Name", "PRN", axis: ePivotFieldAxis.Row);
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("Score", "Score", format: EnumFormatTypeExcel.DecimalNumber, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("T Shows", "TShows", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("T Score", "TScore", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum, sort: eSortType.Descending);
      lst.Add("ScoreRuleTypeN", "ScoreRuleTypeN", axis: ePivotFieldAxis.Column);
      lst.Add("ScoreRule", "ScoreRule", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending, isVisible: false);
      lst.Add("ScoreRuleN", "ScoreRuleN", axis: ePivotFieldAxis.Column);
      lst.Add("ScoreRuleConcept", "ScoreRuleConcept", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending, isVisible: false);
      lst.Add("ScoreRuleConceptN", "ScoreRuleConceptN", axis: ePivotFieldAxis.Column);
      lst.Add("SiScore", "SiScore", axis: ePivotFieldAxis.Column);
      return lst;
    }

    #endregion RptScoreByPR

    #region GetRptContactBookShowQuinellasFormat

    ///<summary>
    /// Formato para el reporte Ocupation,Contact, books & Shows (Quinellas)
    /// </summary>
    /// <returns><list type="ColumnFormat"></list></returns>
    /// <history>
    /// [aalcocer] 07/05/2016 Created
    /// </history>
    internal static List<ColumnFormat> GetRptContactBookShowQuinellasFormat()
    {
      return new List<ColumnFormat>
      {
        new ColumnFormat{Title = "Group",Order = 0, Axis = ePivotFieldAxis.Row},
        new ColumnFormat{Title = "Market",Order = 1, Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Default, InsertBlankRow=true},
        new ColumnFormat{Title = "Subgroup",Order = 1, Axis = ePivotFieldAxis.Row, InsertBlankRow=true },

        new ColumnFormat{Title = "Year",Order = 0, Axis = ePivotFieldAxis.Column, SubTotalFunctions = eSubTotalFunctions.Default},
        new ColumnFormat{Title = "Month",Order = 1, Axis = ePivotFieldAxis.Column, Format = EnumFormatTypeExcel.Month, SubTotalFunctions = eSubTotalFunctions.Default},
        new ColumnFormat{Title = "Zone",Order = 2, Axis = ePivotFieldAxis.Column, SubTotalFunctions = eSubTotalFunctions.Default},
        new ColumnFormat{Title = "LeadSource",Order = 3, Axis = ePivotFieldAxis.Column},

        new ColumnFormat {Title = "Llegadas", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Contactos", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Disponibles",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Total Books", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Shows", Order = 8, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Total Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ColumnFormat {Title = "No. Ventas", Order =12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ColumnFormat {Title = "Volumen de ventas", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency},
        new ColumnFormat { Title = "% Total", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, DataFieldShowDataAs=EnumTableDynamicFunction.PercentOfCol },

        new ColumnFormat {Title = "% Contactación",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Llegadas =0,0,Contactos/Llegadas)" },
        new ColumnFormat {Title = "% Disponibles",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Contactos =0,0,Disponibles/Contactos)" },
        new ColumnFormat {Title = "% Books",Order = 7 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Disponibles =0,0,'Total Books'/Disponibles)" },
       new ColumnFormat {Title = "% Shows",Order = 10,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
       new ColumnFormat {Title = "% Shows/Llegadas",Order = 11,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Llegadas =0,0,Shows/Llegadas)" },
        new ColumnFormat { Title = "% Cierre", Order = 13, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('Total Shows' =0,0,'No. Ventas'/'Total Shows')" },
        new ColumnFormat { Title = "Eficiencia", Order = 16, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Total Shows' =0,0,'Volumen de ventas'/'Total Shows')" },
        new ColumnFormat { Title = "Venta promedio", Order = 15, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('No. Ventas' = 0,0,'Volumen de ventas'/'No. Ventas')" },
        new ColumnFormat { Title = "Total", Order = 17, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number, Formula = "Llegadas" },
      };
    }

    #endregion GetRptContactBookShowQuinellasFormat
  }
}
﻿using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OfficeOpenXml.Table.PivotTable;
using System.Collections.Generic;

namespace IM.ProcessorInhouse.Classes
{
  internal static class clsFormatReport
  {
    #region GetRptCostByPRFormat

    /// <summary>
    /// Formato para el reporte Cost by PR
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptCostByPRFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable {Title = "PR ID", Axis = ePivotFieldAxis.Row, Order = 0, Alignment = ExcelHorizontalAlignment.Right},
        new ExcelFormatTable {Title = "PR Name", Axis = ePivotFieldAxis.Row, Order = 1},

        new ExcelFormatTable {Title = "Shows", Axis = ePivotFieldAxis.Values, Order  = 0, Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right},
        new ExcelFormatTable {Title = "Total Cost", Axis = ePivotFieldAxis.Values, Order  = 1, Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Right},

        new ExcelFormatTable {Title = "Average Cost", Axis = ePivotFieldAxis.Values, Order  = 2, Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Right, Formula ="IF('Shows'=0,0,'Total Cost'/'Shows')" }
      };
    }

    #endregion GetRptCostByPRFormat

    #region GetRptCostByPRWithDetailGiftsFormat

    /// <summary>
    /// Formato para el reporte Cost by PR With Detail Gifts
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptCostByPRWithDetailGiftsFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable {Title = "PR", Axis = ePivotFieldAxis.Row, Order =0, Compact = true, Outline = true},
         new ExcelFormatTable {Title = "Qty", Axis = ePivotFieldAxis.Row, Order = 4, Alignment = ExcelHorizontalAlignment.Right,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Gift ID", Axis = ePivotFieldAxis.Row, Order = 2, Alignment = ExcelHorizontalAlignment.Right, Sort = eSortType.Descending},
        new ExcelFormatTable {Title = "Gift Name", Axis = ePivotFieldAxis.Row, Order = 3},

        new ExcelFormatTable {Title = "Shows", Axis = ePivotFieldAxis.Values, Order = 0, Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right},
        new ExcelFormatTable {Title = "Total Cost", Axis = ePivotFieldAxis.Values, Order = 1, Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Right},

        new ExcelFormatTable {Title = "Average Cost", Axis = ePivotFieldAxis.Values, Order = 2, Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Right,Formula ="IF(Shows=0,0,'Total Cost'/Shows)" }
      };
    }

    #endregion GetRptCostByPRWithDetailGiftsFormat

    #region GetRptFollowUpByAgencyFormat

    /// <summary>
    /// Formato para el reporte Follow Up by Agency
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptFollowUpByAgencyFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "OriginallyAvailable", Axis =ePivotFieldAxis.Row, Order = 0,Compact = true,Outline = true,SubTotalFunctions = eSubTotalFunctions.Default},
        new ExcelFormatTable{Title = "Market", Axis  = ePivotFieldAxis.Row, Order = 1, Compact = true,Outline = true,SubTotalFunctions = eSubTotalFunctions.Default},
        new ExcelFormatTable {Title = "Agency ID", Axis  = ePivotFieldAxis.Row, Order = 2},
        new ExcelFormatTable {Title = "Agency Name", Axis  = ePivotFieldAxis.Row, Order = 3},

        new ExcelFormatTable {Title = "Conts",  Axis = ePivotFieldAxis.Values, Order = 0,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",  Axis = ePivotFieldAxis.Values, Order = 1,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Follow",  Axis = ePivotFieldAxis.Values, Order = 2,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books",  Axis = ePivotFieldAxis.Values, Order = 4,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows",  Axis = ePivotFieldAxis.Values, Order = 6,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Sales",  Axis = ePivotFieldAxis.Values, Order = 7,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable{Title = "Amount",  Axis = ePivotFieldAxis.Values, Order = 8,Format = EnumFormatTypeExcel.Currency},

        new ExcelFormatTable{Title = "FU%", Order = 3,Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Percent,Formula = "IF(Avails =0,0,Follow/Avails)"},
        new ExcelFormatTable { Title = "Bk%", Order = 5, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF(Follow =0,0,Books/Follow)" },
        new ExcelFormatTable { Title = "Eff", Order = 9, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF(Shows =0,0,Amount/Shows)" },
        new ExcelFormatTable { Title = "Cl%", Order = 10, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF(Shows =0,0,Sales/Shows)" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 11, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF(Sales =0,0,Amount/Sales)" }
      };
    }

    #endregion GetRptFollowUpByAgencyFormat

    #region GetRptFollowUpByPRFormat

    /// <summary>
    /// Formato para el reporte Follow Up by PR
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptFollowUpByPRFormat()
    {
      return new List<ExcelFormatTable>
      {
         new ExcelFormatTable { Title = "Status", Order = 0, Axis = ePivotFieldAxis.Row, Compact = true, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default },
        new ExcelFormatTable { Title = "PR ID", Order = 1, Axis = ePivotFieldAxis.Row },
        new ExcelFormatTable {Title = "PR Name", Order = 2,  Axis = ePivotFieldAxis.Row},

        new ExcelFormatTable {Title = "Conts", Order = 0, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails", Order = 1, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Follow", Order = 2, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 4, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 6, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable { Title = "Sales", Order = 7, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable { Title = "Amount", Order = 8, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency },

        new ExcelFormatTable { Title = "FU%", Order = 3, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,Follow/Avails)" },
        new ExcelFormatTable { Title = "Bk%", Order = 5, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF(Follow =0,0,Books/Follow)" },
         new ExcelFormatTable { Title = "Eff", Order = 9, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF(Shows =0,0,Amount/Shows)" },
        new ExcelFormatTable { Title = "Cl%", Order = 10, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF(Shows =0,0,Sales/Shows)" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 11, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF(Sales =0,0,Amount/Sales)" }
      };
    }

    #endregion GetRptFollowUpByPRFormat

    #region GetRptProductionByAgeInhouseFormat

    /// <summary>
    /// Formato para el reporte Production by Age
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByAgeInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable {Title = "Age Range", Order = 0 , Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Arrivs", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Arrivs =0,0,Conts/Arrivs)" },
        new ExcelFormatTable {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 11 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 19, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByAgeInhouseFormat

    #region GetProductionByAgeMarketOriginallyAvailableInhouseFormat

    /// <summary>
    /// Formato para el reporte Production by Age, Market & Originally Available
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetProductionByAgeMarketOriginallyAvailableInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "OriginallyAvailable",Order = 0, Axis = ePivotFieldAxis.Row, Compact = true,Outline = true,SubTotalFunctions = eSubTotalFunctions.Default},
        new ExcelFormatTable{Title = "Market",Order = 1, Axis = ePivotFieldAxis.Row,Compact = true, Outline = true,SubTotalFunctions = eSubTotalFunctions.Default},
        new ExcelFormatTable {Title = "Age Range", Order = 2 , Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Arrivs", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Arrivs =0,0,Conts/Arrivs)" },
        new ExcelFormatTable {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 11 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 19, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetProductionByAgeMarketOriginallyAvailableInhouseFormat

    #region GetRptProductionByContractAgencyInhouseFormat

    /// <summary>
    /// Formato para el reporte Production by Contract & Agency
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByContractAgencyInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Contract ID",Order = 1, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable{Title = "Contract Name",Order = 2, Axis = ePivotFieldAxis.Row},
         new ExcelFormatTable {Title = "Agency", Order = 0 , Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Default, Compact = true, Outline = true},
        new ExcelFormatTable {Title = "Arrivs", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Arrivs =0,0,Conts/Arrivs)" },
        new ExcelFormatTable {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 11 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 19, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByContractAgencyInhouseFormat

    #region GetRptProductionByContractAgencyMarketOriginallyAvailableInhouseFormat

    /// <summary>
    /// Formato para el reporte Production by Contract, Agency, Market & Originally Available
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 11/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByContractAgencyMarketOriginallyAvailableInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Originally Available",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true,Compact = true},
        new ExcelFormatTable{Title = "Market",Order = 1, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true,Compact = true},
        new ExcelFormatTable{Title = "Contract ID",Order = 3, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable{Title = "Contract Name",Order = 4, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Agency", Order = 2 , Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Default, Outline = true},
        new ExcelFormatTable {Title = "Arrivs", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Arrivs =0,0,Conts/Arrivs)" },
        new ExcelFormatTable {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 11 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 19, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByContractAgencyMarketOriginallyAvailableInhouseFormat

    #region GetRptProductionByCoupleTypeInhouseFormat

    /// <summary>
    /// Formato para el reporte Production by Couple Type
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByCoupleTypeInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Couple Type",Order = 0, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Arrivs", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Arrivs =0,0,Conts/Arrivs)" },
        new ExcelFormatTable {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 11 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 19, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByCoupleTypeInhouseFormat

    #region GetRptProductionByDeskInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by Desk
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByDeskInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Desk",Order = 0, Axis = ePivotFieldAxis.Row},

        new ExcelFormatTable {Title = "Conts", Order = 0,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 1, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 4, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 8, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 11, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "A%",Order = 2 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 6 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 9 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 17, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 16, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByDeskInhouseFormat

    #region GetRptProductionByGroupInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by Group
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByGroupInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Groups",Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable{Title = "Rooms",Order = 0, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable{Title = "T Rooms",Order = 1, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Arrivs", Order = 2, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 3,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 8, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 11, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 18, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 19, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "C%",Order = 4,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Arrivs =0,0,Conts/Arrivs)" },
        new ExcelFormatTable {Title = "A%",Order = 6 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 10 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 13 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 21, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 22, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByGroupInhouseFormat

    #region GetRptProductionByGuestStatusInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by Guest Status
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByGuestStatusInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Guest Status",Order = 1, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable{Title = "Lead Source",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Compact = true,Outline = true},
        new ExcelFormatTable {Title = "Arrivs", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Arrivs =0,0,Conts/Arrivs)" },
        new ExcelFormatTable {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 11 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 19, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByGuestStatusInhouseFormat

    #region GetRptProductionByMemberTypeAgencyInhouseFormat

    ///<summary>
    /// Formato para el reporte  Production by Member Type & Agency
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByMemberTypeAgencyInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Agency",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Compact = true,Outline = true},
        new ExcelFormatTable{Title = "Member Type",Order = 1, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Arrivs", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Arrivs =0,0,Conts/Arrivs)" },
        new ExcelFormatTable {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 11 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 19, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByMemberTypeAgencyInhouseFormat

    #region GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by Member Type, Agency, Market & Originally Available
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Originally Available",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true,Compact = true},
        new ExcelFormatTable{Title = "Market",Order = 1, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true, Compact = true},
        new ExcelFormatTable{Title = "Agency",Order = 2, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default, Outline = true},
        new ExcelFormatTable{Title = "Member Type",Order = 3, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Arrivs", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Arrivs =0,0,Conts/Arrivs)" },
        new ExcelFormatTable {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 11 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 19, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouseFormat

    #region GetRptProductionByPRInhouseFormat

    ///<summary>
    /// Formato para el reporte  Production by PR
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByPRInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Status",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true,Compact = true},
        new ExcelFormatTable{Title = "PR ID",Order = 1, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable{Title = "PR Name",Order = 2, Axis = ePivotFieldAxis.Row},

        new ExcelFormatTable {Title = "Assigns", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Sh-D", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 11, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "IO", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "WO", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "RT", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "CT", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Save", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Tours", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "UPS", Order = 18, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Proc", Order = 19, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PROCESSABLE"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Proc", Order = 20, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="PROCESSABLE"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales Pend", Order = 21, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PENDING"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount Pend", Order = 22, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PENDING"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Out Pend", Order = 23, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="OUT OF PENDING"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Out Pend", Order = 24, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="OUT OF PENDING"},

        new ExcelFormatTable {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Assigns =0,0,Conts/Assigns)" },
        new ExcelFormatTable {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 10 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 26, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF(UPS =0,0,'Sales Proc'/UPS)" },
        new ExcelFormatTable { Title = "Eff", Order = 25, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF(UPS =0,0,'Amount Proc'/UPS)" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 27, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Proc' = 0,0,'Amount Proc'/'Sales Proc')" }
      };
    }

    #endregion GetRptProductionByPRInhouseFormat

    #region GetRptProductionByPRGroupInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by PR & Group
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByPRGroupInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "PR",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Compact = true,Outline = true},
        new ExcelFormatTable{Title = "Type",Order = 2, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable{Title = "Groups",Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable{Title = "Rooms",Order = 3, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable{Title = "T Rooms",Order = 1, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Assing", Order = 2, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 3,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 8, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 11, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 18, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 19, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "C%",Order = 4,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Assing =0,0,Conts/Assing)" },
        new ExcelFormatTable {Title = "A%",Order = 6 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 10 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 13 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 21, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 22, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByPRGroupInhouseFormat

    #region GetRptProductionByPRSalesRoomInhouseFormat

    ///<summary>
    /// Formato para el reporte  Production by PR & Sales Room
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByPRSalesRoomInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Sales Rom",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true,Compact = true},
        new ExcelFormatTable{Title = "Status",Order = 1, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true,Compact = true},
        new ExcelFormatTable{Title = "PR ID",Order = 2, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable{Title = "PR Name",Order = 3, Axis = ePivotFieldAxis.Row},

        new ExcelFormatTable {Title = "Assigns", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Sh-D", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 11, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "IO", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "WO", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "RT", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "CT", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Save", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Tours", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "UPS", Order = 18, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Proc", Order = 19, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PROCESSABLE"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Proc", Order = 20, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="PROCESSABLE"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales Pend", Order = 21, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PENDING"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount Pend", Order = 22, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PENDING"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Out Pend", Order = 23, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="OUT OF PENDING"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Out Pend", Order = 24, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="OUT OF PENDING"},

        new ExcelFormatTable {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Assigns =0,0,Conts/Assigns)" },
        new ExcelFormatTable {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 10 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 26, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF(UPS =0,0,'Sales Proc'/UPS)" },
        new ExcelFormatTable { Title = "Eff", Order = 25, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF(UPS =0,0,'Amount Proc'/UPS)" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 27, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Proc' = 0,0,'Amount Proc'/'Sales Proc')" }
      };
    }

    #endregion GetRptProductionByPRSalesRoomInhouseFormat

    #region GetRptRepsPaymentFormat

    /// <summary>
    /// Formato para el reporte Reps Payment
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptRepsPaymentFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Rep",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default},
        new ExcelFormatTable {Title = "Book Date", Order = 1, Axis = ePivotFieldAxis.Row,Format = EnumFormatTypeExcel.Date,Alignment = ExcelHorizontalAlignment.Center},
        new ExcelFormatTable {Title = "Guest Name", Order = 2,  Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Q", Order = 3, Axis = ePivotFieldAxis.Row, Alignment = ExcelHorizontalAlignment.Center},
        new ExcelFormatTable {Title = "Show Pay", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency},
        new ExcelFormatTable {Title = "Sale", Order = 1, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Sale Pay", Order = 2, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency},
        new ExcelFormatTable {Title = "Total", Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency}
      };
    }

    #endregion GetRptRepsPaymentFormat

    #region GetRptRepsPaymentSummaryFormat

    /// <summary>
    /// Formato para el reporte Reps Payment Summary
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptRepsPaymentSummaryFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Rep",Order = 0, Axis = ePivotFieldAxis.Row},

        new ExcelFormatTable {Title = "#", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number,Alignment = ExcelHorizontalAlignment.Center,Formula = "SUBTOTAL(109,['#])"},
        new ExcelFormatTable {Title = "Shows Payment", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency, TotalsRowFunction = RowFunctions.Sum},
        new ExcelFormatTable {Title = "Sub Total", Order = 2, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency, TotalsRowFunction = RowFunctions.Sum},
        new ExcelFormatTable {Title = "# ", Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Center,Formula = "SUBTOTAL(109,['# ])"},
        new ExcelFormatTable {Title = "Sales Payment", Order = 4, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency, TotalsRowFunction = RowFunctions.Sum},
        new ExcelFormatTable {Title = "Sub Total ", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency,TotalsRowFunction = RowFunctions.Sum},
        new ExcelFormatTable {Title = "Total", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency, TotalsRowFunction = RowFunctions.Sum}
      };
    }

    #endregion GetRptRepsPaymentSummaryFormat

    #region GetRptProductionByNationalityInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by Nationality
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetRptProductionByNationalityInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Nationality",Order = 0, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Arrivs", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Arrivs =0,0,Conts/Arrivs)" },
        new ExcelFormatTable {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 11 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 19, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetRptProductionByNationalityInhouseFormat

    #region GetProductionByNationalityMarketOriginallyAvailableInhouseFormat

    ///<summary>
    /// Formato para el reporte Production by Nationality, Market & Originally Available
    /// </summary>
    /// <returns><list type="ExcelFormatTable"></list></returns>
    /// <history>
    /// [aalcocer] 18/04/2016 Created
    /// </history>
    internal static List<ExcelFormatTable> GetProductionByNationalityMarketOriginallyAvailableInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Originally Available",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true,Compact = true},
        new ExcelFormatTable{Title = "Market",Order = 1, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true, Compact = true},
        new ExcelFormatTable{Title = "Nationality",Order = 2, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Arrivs", Order = 0, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Conts", Order = 1,  Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Avails",Order = 3, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Books", Order = 5, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Directs", Order = 6, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Books", Order = 7, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Shows", Order = 9, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "T Shows", Order = 10, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number},

        new ExcelFormatTable {Title = "Sales", PropertyName = "Sales Total", Order = 12, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Amount",  PropertyName = "Amount Total", Order = 13, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency ,SuperHeader="TOTAL"},
        new ExcelFormatTable {Title = "Sales ", PropertyName = "Sales PR", Order = 14, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Amount ", PropertyName = "Amount PR", Order = 15, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="PR"},
        new ExcelFormatTable {Title = "Sales  ", PropertyName = "Sales Self Gen", Order = 16, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Number , SuperHeader="SELF GEN"},
        new ExcelFormatTable {Title = "Amount  ", PropertyName = "Amount Self Gen", Order = 17, Axis = ePivotFieldAxis.Values,Format = EnumFormatTypeExcel.Currency , SuperHeader="SELF GEN"},

        new ExcelFormatTable {Title = "C%",Order = 2,Axis = ePivotFieldAxis.Values, Format= EnumFormatTypeExcel.Percent, Formula = "IF(Arrivs =0,0,Conts/Arrivs)" },
        new ExcelFormatTable {Title = "A%",Order = 4 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Conts =0,0,Avails/Conts)" },
        new ExcelFormatTable {Title = "Bk%",Order = 8 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Avails =0,0,'T Books'/Avails)" },
       new ExcelFormatTable {Title = "Sh%",Order = 11 ,Axis = ePivotFieldAxis.Values,Format= EnumFormatTypeExcel.Percent, Formula = "IF(Books =0,0,Shows/Books)" },
        new ExcelFormatTable { Title = "Cl%", Order = 19, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Percent, Formula = "IF('T Shows' =0,0,'Sales Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Eff", Order = 18, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('T Shows' =0,0,'Amount Total'/'T Shows')" },
        new ExcelFormatTable { Title = "Avg Sale", Order = 20, Axis = ePivotFieldAxis.Values, Format = EnumFormatTypeExcel.Currency, Formula = "IF('Sales Total' = 0,0,'Amount Total'/'Sales Total')" }
      };
    }

    #endregion GetProductionByNationalityMarketOriginallyAvailableInhouseFormat
  }
}
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System.Collections.Generic;

namespace IM.ProcessorInhouse.Classes
{
  public static class clsFormatReport
  {
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

    internal static List<ExcelFormatTable> GetRptCostByPRWithDetailGiftsFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable {Title = "PR ID", Axis = ePivotFieldAxis.Row, Order  = 0, Alignment = ExcelHorizontalAlignment.Right},
        new ExcelFormatTable {Title = "PR Name", Axis = ePivotFieldAxis.Row, Order =1, Outline = true},//, InsertBlankRow = true},
         new ExcelFormatTable {Title = "Qty", Axis = ePivotFieldAxis.Row, Order = 4, Alignment = ExcelHorizontalAlignment.Right,Format = EnumFormatTypeExcel.Number},
        new ExcelFormatTable {Title = "Gift ID", Axis = ePivotFieldAxis.Row, Order = 2, Alignment = ExcelHorizontalAlignment.Right, Sort = eSortType.Descending},
        new ExcelFormatTable {Title = "Gift Name", Axis = ePivotFieldAxis.Row, Order = 3},

        new ExcelFormatTable {Title = "Shows", Axis = ePivotFieldAxis.Values, Order = 0, Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right},
        new ExcelFormatTable {Title = "Total Cost", Axis = ePivotFieldAxis.Values, Order = 1, Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Right},

        new ExcelFormatTable {Title = "Average Cost", Axis = ePivotFieldAxis.Values, Order = 2, Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Right,Formula ="IF(Shows=0,0,'Total Cost'/Shows)" }
      };
    }

    internal static List<ExcelFormatTable> GetRptFollowUpByAgencyFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "OriginallyAvailable", Axis =ePivotFieldAxis.Row, Order = 0,Compact = true,Outline = true,SubTotalFunctions = eSubTotalFunctions.Default},
        new ExcelFormatTable{Title = "Market", Axis  = ePivotFieldAxis.Row, Order = 1,Outline = true,SubTotalFunctions = eSubTotalFunctions.Default},
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

    internal static List<ExcelFormatTable> GetProductionByAgeMarketOriginallyAvailableInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "OriginallyAvailable",Order = 0, Axis = ePivotFieldAxis.Row, Compact = true,Outline = true,SubTotalFunctions = eSubTotalFunctions.Default},
        new ExcelFormatTable{Title = "Market",Order = 1, Axis = ePivotFieldAxis.Row, Outline = true,SubTotalFunctions = eSubTotalFunctions.Default},
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

    internal static List<ExcelFormatTable> GetRptProductionByContractAgencyInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Contract ID",Order = 2, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable{Title = "Contract Name",Order = 3, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Agency ID", Order = 0 , Axis = ePivotFieldAxis.Row},
         new ExcelFormatTable {Title = "Agency Name", Order = 1 , Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Default, Outline = true},
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

    public static List<ExcelFormatTable> GetRptProductionByContractAgencyMarketOriginallyAvailableInhouseFormat()
    {
      return new List<ExcelFormatTable>
      {
        new ExcelFormatTable{Title = "Originally Available",Order = 0, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true,Compact = true},
        new ExcelFormatTable{Title = "Market",Order = 1, Axis = ePivotFieldAxis.Row,SubTotalFunctions = eSubTotalFunctions.Default,Outline = true},
        new ExcelFormatTable{Title = "Contract ID",Order = 4, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable{Title = "Contract Name",Order = 5, Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable {Title = "Agency ID", Order = 2 , Axis = ePivotFieldAxis.Row},
         new ExcelFormatTable {Title = "Agency Name", Order = 3 , Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Default, Outline = true},
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
  }
}
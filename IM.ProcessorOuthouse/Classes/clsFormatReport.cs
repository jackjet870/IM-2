using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System.Collections.Generic;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;

namespace IM.ProcessorOuthouse.Classes
{
  public class clsFormatReport
  {
    #region rptDepositsPaymentByPR

    /// <summary>
    ///  Formato para el reporte DepositsPaymentByPR
    /// </summary>
    /// <history>
    ///   [vku] 07/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptDepositsPaymentByPR()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Category", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true,  SubTotalFunctions = eSubTotalFunctions.Default},
      new ExcelFormatTable() { Title = "Place", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,  SubTotalFunctions = eSubTotalFunctions.Default},
      new ExcelFormatTable() { Title = "PaymentSchema",  Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2,  Outline = true,  SubTotalFunctions = eSubTotalFunctions.Default},
      new ExcelFormatTable() { Title = "PR ID",  Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 3 },
      new ExcelFormatTable() { Title = "PR Name",  Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 4 },

      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 5, },
      new ExcelFormatTable() { Title = "IO", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 6,   },
      new ExcelFormatTable() { Title = "Total Book", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 7,  },
      new ExcelFormatTable() { Title = "Total Shows", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 9, },
      new ExcelFormatTable() { Title = "Sales Amount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 10 },

      new ExcelFormatTable() { Title = "Guest ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 12 },
      new ExcelFormatTable() { Title = "Guest Name", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 13 },
      new ExcelFormatTable() { Title = "Book Date", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 14 },
      new ExcelFormatTable() { Title = "Out. Inv.", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 15 },
      new ExcelFormatTable() { Title = "LS", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 16 },
      new ExcelFormatTable() { Title = "SR", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 17 },
      new ExcelFormatTable() { Title = "Hotel", Format=EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 18 },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Row, Order = 8, Formula = "IF('Total Book' =0,0,'Total Shows'/'Total Book')" },
      new ExcelFormatTable() { Title = "Eficiency",  Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 11, Formula = "IF('Total Book' =0,0,'Sales Amount'/'Total Book')" },

      new ExcelFormatTable() { Title = "Currency", PropertyName = "cuN", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Column, Order = 0, Sort = eSortType.Ascending},
      new ExcelFormatTable() { Title = "PaymentType", PropertyName = "ptN", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },

      new ExcelFormatTable() { Title = "Amount", PropertyName = "bdAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19 },
      new ExcelFormatTable() { Title = "Received", PropertyName = "bdReceived", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 20 },
      new ExcelFormatTable() { Title = "Payment", PropertyName = "topay", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 21 }
      };
    }

    #endregion rptDepositsPaymentByPR

    #region rptGiftsReceivedBySR

    /// <summary>
    ///  Formato para el reporte GiftsReceivedBySR
    /// </summary>
    /// <history>
    ///   [vku] 11/Abr/2016 Created
    /// </history>
    public static ExcelFormatItemsList rptGiftsRecivedBySR()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Sales Room", "SalesRoom", format: EnumFormatTypeExcel.General, axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false );
      lst.Add("Gift ID", "Gift", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row);
      lst.Add("Gift Name", "GiftN", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row);
      lst.Add("Quantity", "Quantity", format: EnumFormatTypeExcel.Number, aligment: ExcelHorizontalAlignment.Right, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Couples", "Couples", format: EnumFormatTypeExcel.Number, aligment: ExcelHorizontalAlignment.Right, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Adults", "Adults", format: EnumFormatTypeExcel.Number, aligment: ExcelHorizontalAlignment.Right, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Minors", "Minors", format: EnumFormatTypeExcel.Number, aligment: ExcelHorizontalAlignment.Right, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Amount", "Amount", format: EnumFormatTypeExcel.Currency, aligment: ExcelHorizontalAlignment.Right, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("cuID", "cuID", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Center, axis: ePivotFieldAxis.Column, sort: eSortType.Ascending, isVisible: false);
      lst.Add("Currency", "cuN", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Center, axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      return lst;
    }

    #endregion rptGiftsReceivedBySR

    #region rptGuestsShowNoPresentedInvitation

    /// <summary>
    ///  Formato para el reporte GuestsShowNoPresentedInvitation
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    ///   [vku] 05/Abr/2016 created
    /// </history>
    public static ExcelFormatItemsList rptGuestsShowNoPresentedInvitation()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Guest ID", "GuestID", format: EnumFormatTypeExcel.Id);
      lst.Add("Lead Source", "LeadSource");
      lst.Add("Out Invit", "OutInvit");
      lst.Add("Last Name", "LastName", format: EnumFormatTypeExcel.General);
      lst.Add("First Name", "FirstName");
      lst.Add("Show Date", "ShowDate", format: EnumFormatTypeExcel.Date);
      lst.Add("Deposit", "Deposit", format: EnumFormatTypeExcel.Currency);
      lst.Add("Received", "Received", format: EnumFormatTypeExcel.Currency);
      lst.Add("Currency", "Currency");
      lst.Add("Payment Type", "PaymentType");
      return lst;
    }

    #endregion rptGuestsShowNoPresentedInvitation

    #region rptProductionByPROuthouse

    /// <summary>
    ///  Formato para el reporte Production by PR
    /// </summary>
    /// <histotory>
    ///   [vku] 14/Abr/2016 Created
    /// </histotory>
    public static List<ExcelFormatTable> rptProductionByPR()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Status", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true, SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "PR ID",  Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "Books",Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0 },
      new ExcelFormatTable() { Title = "IO",  Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 1 },
      new ExcelFormatTable() { Title = "T Bk",  Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2 },
      new ExcelFormatTable() { Title = "Shows ", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3 },
      new ExcelFormatTable() { Title = "Dir", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5 },
      new ExcelFormatTable() { Title = "WO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 6 },
      new ExcelFormatTable() { Title = "CT", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 7 },
      new ExcelFormatTable() { Title = "Save", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 8 },
      new ExcelFormatTable() { Title = "T Sh", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 9 },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 10, SuperHeader="PROCESSABLE"},
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 11, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 12, SuperHeader="OUT OF PENDING"},
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 13, SuperHeader="OUT OF PENDING"},
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 14, SuperHeader="PENDING"   },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 16, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 17, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 18, SuperHeader="TOTAL"  },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 19, SuperHeader="TOTAL"  },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows '/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Shows ' =0,0,'SalesAmount_TOTAL'/'Shows ')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent,  Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF('Shows ' =0,0,'Sales_TOTAL'/'Shows ')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 23, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion rptProductionByPROuthouse

    #region rptProductionByAge

    /// <summary>
    /// Formato para el reporte ProductionByAge
    /// </summary>
    /// <history>
    ///   [vku] 13/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByAge()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Age Range",  Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0 },
      new ExcelFormatTable() { Title = "Books",Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum  },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"},
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion rptProductionByAge

    #region rptProductionByAgeSalesRoomOuthouse

    /// <summary>
    /// Formato para el reporte ProductionByAgeSalesRoomOuthouse
    /// </summary>
    /// <history>
    ///   [vku] 13/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByAgeSalesRoomOuthouse()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Age Range",  Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books",  Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0 },
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 1 },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2  },
      new ExcelFormatTable() { Title = "Shows",Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3 },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 5, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, SuperHeader="OUT OF PENDING"},
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 9,  SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 10,  SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 11, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 12, SuperHeader="TOTAL"  },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion rptProductionByAgeSalesRoomOuthouse

    #region rptProductionByAgencyOuthouse

    /// <summary>
    ///  Formato para el reporte ProductionByAgencyOuthouse
    /// </summary>
    /// <history>
    ///   [vku] 15/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByAgencyOuthouse()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0 },
      new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk",Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum  },
      new ExcelFormatTable() { Title = "Shows",  Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "WO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "RT", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 6,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "CT", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Save", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 8,  Function = DataFieldFunctions.Sum  },
      new ExcelFormatTable() { Title = "T Tours", Format = EnumFormatTypeExcel.Number,Axis = ePivotFieldAxis.Values, Order = 9,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "UPS", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 10,  Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"},
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING"},
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion rptProductionByAgencyOuthouse

    #region rptProductionByAgencySalesMembershipTypeOuthouse

    /// <summary>
    ///  Formato para el reporte productionByAgency details the Sales MembershipType
    /// </summary>
    /// <history>
    ///   [vku] 28/Abr/2016 Created
    /// </history>
    public static ExcelFormatItemsList rptProductionByAgencySalesMembershipTypeOuthouse()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();

      lst.Add("Agency ID", "Agency", format: EnumFormatTypeExcel.General, axis: ePivotFieldAxis.Row);
      lst.Add("Agency", "AgencyN", format: EnumFormatTypeExcel.General, axis: ePivotFieldAxis.Row);
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("IO", "InOuts", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("T Bk", "GrossBooks", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossBooks] =0,0,[Shows]/[GrossBooks])");
      lst.Add("WO", "WalkOuts", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("RT", "Tours", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("CT", "CourtesyTours", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Save", "SaveTours", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("T Tours", "TotalTours", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("UPS", "UPS", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);

      lst.Add("Sales", "Sales_PROC", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Amount", "SalesAmount_PROC", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);


      lst.Add("Sales Total", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount Total", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum, superHeader: "TOTAL");

      lst.Add("Ca%", "CancelFactor", format: EnumFormatTypeExcel.Percent);//, formula: "IF(([SalesAmountTotal]) =0,0,[SalesAmountCancel]/([SalesAmountTotal]+[SalesAmountCancel]))");
      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows] =0,0,[SalesAmount_TOTAL]/[Shows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows] =0,0,[Sales_TOTAL]/[Shows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] =0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      lst.Add("mtN", "mtN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      return lst;
    }

    #endregion rptProductionByAgencySalesMembershipTypeOuthouse

    #region rptProductionByAgencySalesRoomOuthouse

    /// <summary>
    ///  Formato para el reporte ProductionByAgencySalesRoom
    /// </summary>
    /// <history>
    ///   [vku] 20/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByAgencySalesRoomOuthouse()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Sales Room", Format = EnumFormatTypeExcel.General,  Axis = ePivotFieldAxis.Row, Order = 0, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.General,  Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Agency",  Format = EnumFormatTypeExcel.General,  Axis = ePivotFieldAxis.Row, Order = 2 },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 0},
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 1 },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2  },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3 },
      new ExcelFormatTable() { Title = "WO", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 5 },
      new ExcelFormatTable() { Title = "RT", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 6 },
      new ExcelFormatTable() { Title = "CT", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 7 },
      new ExcelFormatTable() { Title = "Save", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 8 },
      new ExcelFormatTable() { Title = "T Tours", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9  },
      new ExcelFormatTable() { Title = "UPS", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 10 },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 11, SuperHeader="PROCESSABLE"  },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 13, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 14, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number,Axis = ePivotFieldAxis.Values, Order = 15,  SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 16,  SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 17, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 18, SuperHeader="TOTAL" },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent,  Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T BK' =0,0,'Shows'/'T BK')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent,  Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%",  Format = EnumFormatTypeExcel.Percent,  Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion rptProductionByAgencySalesRoomOuthouse

    #region rptProductionByAgencySalesRoomSalesMembershipTypeOuthouse

    /// <summary>
    ///  Formato para el reporte ProductionByAgencySalesRoom  details the Sales MembershipType
    /// </summary>
    /// <history>
    ///   [vku] 20/Abr/2016 Created
    /// </history>
    public static ExcelFormatItemsList rptProductionByAgencySalesRoomSalesMembershipTypeOuthouse()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();

      lst.Add("Sales Room", "SalesRoomN", format: EnumFormatTypeExcel.General, axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("Agency ID", "Agency", format: EnumFormatTypeExcel.General, axis: ePivotFieldAxis.Row);
      lst.Add("Agency", "AgencyN", format: EnumFormatTypeExcel.General, axis: ePivotFieldAxis.Row);
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("IO", "InOuts", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("T Bk", "GrossBooks", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Sh%", "ShowsFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([GrossBooks] =0,0,[Shows]/[GrossBooks])");
      lst.Add("WO", "WalkOuts", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("RT", "Tours", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("CT", "CourtesyTours", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("Save", "SaveTours", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("T Tours", "TotalTours", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);
      lst.Add("UPS", "UPS", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum);

      lst.Add("Sales", "Sales_PROC", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);
      lst.Add("Amount", "SalesAmount_PROC", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum, aggregateFunction: DataFieldFunctions.Sum);

      lst.Add("Sales Total", "Sales_TOTAL", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum, superHeader: "TOTAL");
      lst.Add("Amount Total", "SalesAmount_TOTAL", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Row, function: DataFieldFunctions.Sum, superHeader: "TOTAL");

      lst.Add("Ca%", "CancelFactor", format: EnumFormatTypeExcel.Percent);//, isCalculated: true, formula: "IF(([SalesAmountTotal]+[SalesAmountCancel]) =0,0,[SalesAmountCancel]/([SalesAmountTotal]+[SalesAmountCancel]))");
      lst.Add("Eff", "Efficiency", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Shows] =0,0,[SalesAmount_TOTAL]/[Shows])");
      lst.Add("Cl%", "ClosingFactor", format: EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([Shows] =0,0,[Sales_TOTAL]/[Shows])");
      lst.Add("Avg Sale", "AverageSale", format: EnumFormatTypeExcel.Currency, isCalculated: true, formula: "IF([Sales_TOTAL] =0,0,[SalesAmount_TOTAL]/[Sales_TOTAL])");
      lst.Add("mtN", "mtN", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);

      return lst;
    }

    #endregion rptProductionByAgencySalesRoomOuthouse

    #region rptProductionByAgencyMarketHotelOuthouse

    /// <summary>
    ///   Formato para el reporte ProductionByAgencyMarketHotelOuthouse
    /// </summary>
    /// <history>
    ///   [vku] 15/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByAgencyMarketHotelOuthouse()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true,SubTotalFunctions = eSubTotalFunctions.Default },
      new ExcelFormatTable() { Title = "Market", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default },
      new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 3 },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"},
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum,SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion rptProductionByAgencyMarketHotelOuthouse

    #region rptProductionByCoupleTypeOuthouse

    /// <summary>
    ///  Formato para el reporte ProductionByCoupleTypeOuthouse
    /// </summary>
    /// <history>
    ///   [vku] 18/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByCoupleTypeOuthouse()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Couple Type", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0  },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" , Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion rptProductionByCoupleTypeOuthouse

    #region rptProductionByCoupleTypeSalesRoomOuthouse
    /// <summary>
    ///   Formato para el reporte ProductionByCoupleTypeSalesRoomOuthouse
    /// </summary>
    /// <history>
    ///   [vku] 18/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByCoupleTypeSalesRoomOuthouse()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Couple Type", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion rptProductionByCoupleTypeSalesRoomOuthouse

    #region rptProductionByFlightSalesRoom
    /// <summary>
    ///   Formato para el reporte ProductionByFlightSalesRoom
    /// </summary>
    /// <history>
    ///  [vku] 17/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByFlightSalesRoom()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Flight", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0 },
      new ExcelFormatTable() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"},
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum,  SuperHeader="PENDING"  },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum,  SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

    #region rptProductionByGiftInvitation

    /// <summary>
    ///  Formato para el reporte ProductionByGiftInvitation
    /// </summary>
    /// <history>
    ///   [vku] 21/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByGiftInvitation()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Gift ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0  },
      new ExcelFormatTable() { Title = "Gift Name", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1, },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion rptProductionByGiftInvitation

    #region rptProductionByGiftInvitationSalesRoom

    /// <summary>
    ///   Formato para el reporte de produccion por regalo de invitacion y sala
    /// </summary>
    /// <history>
    ///   [vku] 21/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByGiftInvitationSalesRoom()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Gift ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Gift Name", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"  },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion rptProductionByGiftInvitationSalesRoom

    #region rptProductionByGuestStatusOuthouse

    /// <summary>
    ///  Formato para el reporte de produccion por estatus de huesped
    /// </summary>
    /// <history>
    ///   [vku] 22/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByGuestStatusOuthouse()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "LeadSource", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "GuestStatus", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion rptProductionByGuestStatusOuthouse

    #region rptProductionByHotel
    /// <summary>
    ///   Formato para el reporte production by hotel
    /// </summary>
    /// <history>
    ///   [vku] 17/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByHotel()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0 },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED "},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

    #region rptProductionByHotelSalesRoom
    /// <summary>
    ///  Formato para el reporte de produccion por hotel y sala
    /// </summary>
    /// <history>
    ///   [vku] 19/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByHotelSalesRoom()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

    #region rptProductionByHotelGroup
    /// <summary>
    ///   Formato para el reporte de produccion por grupo hotelero
    /// </summary>
    /// <history>
    ///   [vku]  19/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByHotelGroup()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Hotel Group", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1 },

      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

    #region rptProductionByHotelGroupSalesRoom
    /// <summary>
    ///   Formato para el reporte de produccion por grupo hotelero y sala
    /// </summary>
    /// <history>
    ///   [vku] 19/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByHotelGroupSalesRoom()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Hotel Group", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true,SubTotalFunctions = eSubTotalFunctions.Default },
      new ExcelFormatTable() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2},
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3 , Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"},
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number,Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING"  },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion   

    #region rptProductionByNationalityOuthouse

    /// <summary>
    ///  Formato para el reporte de produccion por nacionalidad
    /// </summary>
    /// <history>
    ///   [vku] 22/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByNationalityOuthouse()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Nationality", PropertyName = "Nationality", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0  },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED "},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion rptProductionByNationalityOuthouse

    #region rptProductionByNationalitySalesRoomOuthouse

    /// <summary>
    ///  Formato para el reporte  por nacionalidad y sala
    /// </summary>
    /// <history>
    ///   [vku] 22/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByNationalitySalesRoomOuthouse()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoomN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Nationality", PropertyName = "Nationality", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED "},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion rptProductionByNationalitySalesRoomOuthouse

    #region rptProductionByPRSalesRoomOuthouse

    /// <summary>
    ///  Formato para el reportee ProductionByPRSalesRoom
    /// </summary>
    /// <history>
    ///   [vku] 25/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByPRSalesRoomOuthouse()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Status", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "PR ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 3  },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows ", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Dir", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "WO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 6,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "CT", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7,  Function = DataFieldFunctions.Sum  },
      new ExcelFormatTable() { Title = "Save", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 8,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Sh", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9,  Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"  },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows '/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Shows ' =0,0,'SalesAmount_TOTAL'/'Shows ')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF('Shows ' =0,0,'Sales_TOTAL'/'Shows ')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 23, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion rptProductionByPRSalesRoomOuthouse

    #region rptProductionByPRContactOuthouse

    /// <summary>
    ///  Formato para el reporte ProductionByPRContact
    /// </summary>
    /// <history>
    ///   [vku] 25/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByPRContactOuthouse()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Status", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "PR ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "Contacts" , Format = EnumFormatTypeExcel.DecimalNumberWithCero, Axis = ePivotFieldAxis.Values, Order = 0, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "Bk%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 2,  Formula = "IF('Contacts' =0,0,'Books'/'Contacts')"},
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 4, Function = DataFieldFunctions.Sum  },
      new ExcelFormatTable() { Title = "Shows ", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Dir", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "WO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 8,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "CT", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Save", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 10,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Sh", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11,  Function = DataFieldFunctions.Sum   },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"},
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 6, Formula = "IF('T Bk' =0,0,'Shows '/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Shows ' =0,0,'SalesAmount_TOTAL'/'Shows ')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF('Shows ' =0,0,'Sales_TOTAL'/'Shows ')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 23, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

    #region rptProductionByWave
    /// <summary>
    ///  Formato para el reporte de produccion por horario
    /// </summary>
    /// <history>
    ///   [vku] 19/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByWave()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Book Time", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"  },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion rptProductionByPRContactOuthouse

    #region rptProductionByWaveSalesRoom
    /// <summary>
    ///   Formato para el reporte de produccion por horario y sala
    /// </summary>
    /// <history>
    ///   [vku] 20/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByWaveSalesRoom()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Book Time", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default },
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoomID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"  },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ExcelFormatTable() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

    #region rptFoliosInvitationByDateFolio
    /// <summary>
    ///  Formato para el reporte FoliosInvitationByDateFolio
    /// </summary>
    /// <history>
    ///   [vku] 03/May/2016 Created
    /// </history>
    public static ExcelFormatItemsList rptFoliosInvitationByDateFolio()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Out. Inv.", "guOutInvitNum");
      lst.Add("PR ID", "PR");
      lst.Add("PR Name", "PRN");
      lst.Add("Last Name", "guLastName1");
      lst.Add("Book D", "guBookD");
      lst.Add("Lead Source", "lsN");
      return lst;
    }

    #endregion rptFoliosInvitationByDateFolio

    #region rptFoliosInvitationOuthouseByPR
    /// <summary>
    ///  Formato para el reporte FoliosInvitationOuthouseByPR
    /// </summary>
    /// <history>
    ///   [vku] 06/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptFoliosInvitationOuthouseByPR()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() { Title = "Name", Axis = ePivotFieldAxis.Row,},
        new ExcelFormatTable() { Title = "Status", Axis = ePivotFieldAxis.Row, },
        new ExcelFormatTable() { Title = "Serie", Axis = ePivotFieldAxis.Row },
        new ExcelFormatTable() { Title = "From", Axis = ePivotFieldAxis.Row },
        new ExcelFormatTable() { Title = "To", Axis = ePivotFieldAxis.Row },
        new ExcelFormatTable() { Title = "Lead Source", Axis = ePivotFieldAxis.Row},
        new ExcelFormatTable() { Title = "Last Name", Axis = ePivotFieldAxis.Row },
        new ExcelFormatTable() { Title = "Firt Name", Axis = ePivotFieldAxis.Row },
        new ExcelFormatTable() { Title = "Book D", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Date },
      };
    }
    #endregion

    #region rptFoliosCxCByPR
    /// <summary>
    ///   Formato para el reporte FoliosCxCByPR
    /// </summary>
    /// <history>
    ///   [vku] 06/May/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptFoliosCxCByPR()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() { Title = "Name", Axis = ePivotFieldAxis.Row, },
        new ExcelFormatTable() { Title = "Status", Axis = ePivotFieldAxis.Row, },
        new ExcelFormatTable() { Title = "From", Axis = ePivotFieldAxis.Row, },
        new ExcelFormatTable() { Title = "To", Axis = ePivotFieldAxis.Row, },
        new ExcelFormatTable() { Title = "Lead Source", Axis = ePivotFieldAxis.Row, },
        new ExcelFormatTable() { Title = "Last Name", Axis = ePivotFieldAxis.Row, },
        new ExcelFormatTable() { Title = "Firt Name", Axis = ePivotFieldAxis.Row, },
        new ExcelFormatTable() { Title = "Book D", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Date },
      };
    }
    #endregion

    #region rptFoliosCXC
    /// <summary>
    ///  Formato para el reporte de Folios CxC
    /// </summary>
    /// <history>
    ///   [vku] 07/May/2016 Created
    /// </history>
    public static ExcelFormatItemsList rptFoliosCXC()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Es Show", "EsShow", isGroup: true, isVisible: false);
      lst.Add("Tipo", "Tipo", isGroup: true, isVisible: false);
      lst.Add("Folio CXC", "bdFolioCXC");
      lst.Add("PR ID", "PR");
      lst.Add("PR Name", "PRN");
      lst.Add("Out. Inv.", "guOutInvitNum");
      lst.Add("Lead Source", "lsN");
      lst.Add("Last Name", "guLastName1");
      lst.Add("First Name", "guFirstName1");
      lst.Add("Book D", "guBookD", format: EnumFormatTypeExcel.Date);
      lst.Add("CXC User", "bdUserCXC");
      lst.Add("CXC User Name", "peN");
      lst.Add("Date CXC", "bdEntryDCXC", format: EnumFormatTypeExcel.Date);
      return lst;
    }
    #endregion

  }
}
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
    ///   [erosado] 27/09/2016  Modified. Se elimino la columna ToPay y se agregó la columna CxC
    /// </history>
    public static List<ColumnFormat> rptDepositsPaymentByPR()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Category", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true,  SubTotalFunctions = eSubTotalFunctions.Default},
      new ColumnFormat() { Title = "Place", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,  SubTotalFunctions = eSubTotalFunctions.Default},
      new ColumnFormat() { Title = "PaymentSchema",  Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2,  Outline = true,  SubTotalFunctions = eSubTotalFunctions.Default},
      new ColumnFormat() { Title = "PR ID",  Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 3 },
      new ColumnFormat() { Title = "PR Name",  Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 4 },

      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 5, },
      new ColumnFormat() { Title = "IO", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 6,   },
      new ColumnFormat() { Title = "Total Book", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 7,  },
      new ColumnFormat() { Title = "Total Shows", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 9, },
      new ColumnFormat() { Title = "Sales Amount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 10 },

      new ColumnFormat() { Title = "Guest ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 12 },
      new ColumnFormat() { Title = "Guest Name", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 13 },
      new ColumnFormat() { Title = "Book Date", Format = EnumFormatTypeExcel.Date, Axis = ePivotFieldAxis.Row, Order = 14 },
      new ColumnFormat() { Title = "Out. Inv.", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 15 },
      new ColumnFormat() { Title = "LS", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 16 },
      new ColumnFormat() { Title = "SR", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 17 },
      new ColumnFormat() { Title = "Hotel", Format=EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 18 },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Row, Order = 8, Formula = "IF('Total Book' =0,0,'Total Shows'/'Total Book')" },
      new ColumnFormat() { Title = "Eficiency",  Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 11, Formula = "IF('Total Book' =0,0,'Sales Amount'/'Total Book')" },

      new ColumnFormat() { Title = "Currency", PropertyName = "cuN", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Column, Order = 0, Sort = eSortType.Ascending},
      new ColumnFormat() { Title = "PaymentType", PropertyName = "ptN", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },

      new ColumnFormat() { Title = "Amount", PropertyName = "bdAmount", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19 },
      new ColumnFormat() { Title = "Received", PropertyName = "bdReceived", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 20 },
      new ColumnFormat() { Title = "CxC", PropertyName = "CxC", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 21 },
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
    public static ColumnFormatList rptGiftsRecivedBySR()
    {
      ColumnFormatList lst = new ColumnFormatList();
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
    /// <returns>List<ColumnFormat></returns>
    /// <history>
    ///   [vku] 05/Abr/2016 created
    /// </history>
    public static ColumnFormatList rptGuestsShowNoPresentedInvitation()
    {
      ColumnFormatList lst = new ColumnFormatList();
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
    public static List<ColumnFormat> rptProductionByPR()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Status", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true, SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "PR ID",  Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ColumnFormat() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ColumnFormat() { Title = "Books",Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0 },
      new ColumnFormat() { Title = "IO",  Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 1 },
      new ColumnFormat() { Title = "T Bk",  Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2 },
      new ColumnFormat() { Title = "Shows ", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3 },
      new ColumnFormat() { Title = "Dir", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5 },
      new ColumnFormat() { Title = "WO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 6 },
      new ColumnFormat() { Title = "CT", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 7 },
      new ColumnFormat() { Title = "Save", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 8 },
      new ColumnFormat() { Title = "T Sh", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 9 },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 10, SuperHeader="PROCESSABLE"},
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 11, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 12, SuperHeader="OUT OF PENDING"},
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 13, SuperHeader="OUT OF PENDING"},
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 14, SuperHeader="PENDING"   },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 16, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 17, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 18, SuperHeader="TOTAL"  },
      new ColumnFormat() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 19, SuperHeader="TOTAL"  },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows '/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Shows ' =0,0,'SalesAmount_TOTAL'/'Shows ')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent,  Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF('Shows ' =0,0,'Sales_TOTAL'/'Shows ')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 23, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByAge()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Age Range",  Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0 },
      new ColumnFormat() { Title = "Books",Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum  },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"},
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByAgeSalesRoomOuthouse()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "Age Range",  Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ColumnFormat() { Title = "Books",  Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0 },
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 1 },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2  },
      new ColumnFormat() { Title = "Shows",Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3 },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 5, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6,  SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, SuperHeader="OUT OF PENDING"},
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 9,  SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 10,  SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 11, SuperHeader="TOTAL"},
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 12, SuperHeader="TOTAL"  },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByAgencyOuthouse()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Agency ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0 },
      new ColumnFormat() { Title = "Agency", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk",Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum  },
      new ColumnFormat() { Title = "Shows",  Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "WO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5,  Function = DataFieldFunctions.Sum},
      new ColumnFormat() { Title = "RT", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 6,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "CT", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Save", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 8,  Function = DataFieldFunctions.Sum  },
      new ColumnFormat() { Title = "T Tours", Format = EnumFormatTypeExcel.Number,Axis = ePivotFieldAxis.Values, Order = 9,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "UPS", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 10,  Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"},
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING"},
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static ColumnFormatList rptProductionByAgencySalesMembershipTypeOuthouse()
    {
      ColumnFormatList lst = new ColumnFormatList();

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
    public static List<ColumnFormat> rptProductionByAgencySalesRoomOuthouse()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Sales Room", Format = EnumFormatTypeExcel.General,  Axis = ePivotFieldAxis.Row, Order = 0, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "Agency ID", Format = EnumFormatTypeExcel.General,  Axis = ePivotFieldAxis.Row, Order = 1  },
      new ColumnFormat() { Title = "Agency",  Format = EnumFormatTypeExcel.General,  Axis = ePivotFieldAxis.Row, Order = 2 },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 0},
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 1 },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2  },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3 },
      new ColumnFormat() { Title = "WO", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 5 },
      new ColumnFormat() { Title = "RT", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 6 },
      new ColumnFormat() { Title = "CT", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 7 },
      new ColumnFormat() { Title = "Save", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 8 },
      new ColumnFormat() { Title = "T Tours", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9  },
      new ColumnFormat() { Title = "UPS", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 10 },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 11, SuperHeader="PROCESSABLE"  },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 13, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 14, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number,Axis = ePivotFieldAxis.Values, Order = 15,  SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 16,  SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 17, SuperHeader="TOTAL"},
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 18, SuperHeader="TOTAL" },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent,  Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T BK' =0,0,'Shows'/'T BK')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent,  Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%",  Format = EnumFormatTypeExcel.Percent,  Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static ColumnFormatList rptProductionByAgencySalesRoomSalesMembershipTypeOuthouse()
    {
      ColumnFormatList lst = new ColumnFormatList();

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
    public static List<ColumnFormat> rptProductionByAgencyMarketHotelOuthouse()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true,SubTotalFunctions = eSubTotalFunctions.Default },
      new ColumnFormat() { Title = "Market", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default },
      new ColumnFormat() { Title = "Agency ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ColumnFormat() { Title = "Agency", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 3 },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"},
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum,SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByCoupleTypeOuthouse()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Couple Type", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0  },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" , Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency,  Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByCoupleTypeSalesRoomOuthouse()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "Couple Type", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByFlightSalesRoom()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Flight", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0 },
      new ColumnFormat() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"},
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum,  SuperHeader="PENDING"  },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum,  SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByGiftInvitation()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Gift ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0  },
      new ColumnFormat() { Title = "Gift Name", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1, },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByGiftInvitationSalesRoom()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "Gift ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ColumnFormat() { Title = "Gift Name", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"  },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByGuestStatusOuthouse()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "LeadSource", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "GuestStatus", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByHotel()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0 },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED "},
      new ColumnFormat() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByHotelSalesRoom()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING"},
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ColumnFormat() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByHotelGroup()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Hotel Group", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1 },

      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum},
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING"},
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ColumnFormat() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByHotelGroupSalesRoom()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Hotel Group", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true,SubTotalFunctions = eSubTotalFunctions.Default },
      new ColumnFormat() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2},
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3 , Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"},
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number,Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING"  },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL"},
      new ColumnFormat() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByNationalityOuthouse()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Nationality", PropertyName = "Nationality", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0  },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED "},
      new ColumnFormat() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByNationalitySalesRoomOuthouse()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Sales Room", PropertyName = "SalesRoomN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "Nationality", PropertyName = "Nationality", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED "},
      new ColumnFormat() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 13, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByPRSalesRoomOuthouse()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "Status", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "PR ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ColumnFormat() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 3  },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows ", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Dir", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "WO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 6,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "CT", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7,  Function = DataFieldFunctions.Sum  },
      new ColumnFormat() { Title = "Save", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 8,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Sh", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9,  Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"  },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows '/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Shows ' =0,0,'SalesAmount_TOTAL'/'Shows ')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF('Shows ' =0,0,'Sales_TOTAL'/'Shows ')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 23, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByPRContactOuthouse()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Status", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ColumnFormat() { Title = "PR ID", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ColumnFormat() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ColumnFormat() { Title = "Contacts" , Format = EnumFormatTypeExcel.DecimalNumberWithCero, Axis = ePivotFieldAxis.Values, Order = 0, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1,  Function = DataFieldFunctions.Sum},
      new ColumnFormat() { Title = "Bk%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 2,  Formula = "IF('Contacts' =0,0,'Books'/'Contacts')"},
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 4, Function = DataFieldFunctions.Sum  },
      new ColumnFormat() { Title = "Shows ", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Dir", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "WO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 8,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "CT", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Save", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 10,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Sh", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11,  Function = DataFieldFunctions.Sum   },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"},
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 6, Formula = "IF('T Bk' =0,0,'Shows '/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Shows ' =0,0,'SalesAmount_TOTAL'/'Shows ')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF('Shows ' =0,0,'Sales_TOTAL'/'Shows ')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 23, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByWave()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Book Time", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"  },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ColumnFormat> rptProductionByWaveSalesRoom()
    {
      return new List<ColumnFormat>()
      {
      new ColumnFormat() { Title = "Book Time", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default },
      new ColumnFormat() { Title = "Sales Room", PropertyName = "SalesRoomID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1, Outline = true, SubTotalFunctions = eSubTotalFunctions.Default },
      new ColumnFormat() { Title = "Books", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 0,  Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "IO", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "T Bk", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2, Function = DataFieldFunctions.Sum },
      new ColumnFormat() { Title = "Shows", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },

      new ColumnFormat() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ColumnFormat() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"  },
      new ColumnFormat() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ColumnFormat() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ColumnFormat() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ColumnFormat() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="TOTAL" },
      new ColumnFormat() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ColumnFormat() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('T Bk' =0,0,'Shows'/'T Bk')" },
      new ColumnFormat() { Title = "Ca%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ColumnFormat() { Title = "Eff", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Cl%", Format = EnumFormatTypeExcel.Percent, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ColumnFormat() { Title = "Avg Sale", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static ColumnFormatList rptFoliosInvitationByDateFolio()
    {
      ColumnFormatList lst = new ColumnFormatList();
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
    public static List<ColumnFormat> rptFoliosInvitationOuthouseByPR()
    {
      return new List<ColumnFormat>()
      {
        new ColumnFormat() { Title = "Name", Axis = ePivotFieldAxis.Row,},
        new ColumnFormat() { Title = "Status", Axis = ePivotFieldAxis.Row, },
        new ColumnFormat() { Title = "Serie", Axis = ePivotFieldAxis.Row },
        new ColumnFormat() { Title = "From", Axis = ePivotFieldAxis.Row },
        new ColumnFormat() { Title = "To", Axis = ePivotFieldAxis.Row },
        new ColumnFormat() { Title = "Lead Source", Axis = ePivotFieldAxis.Row},
        new ColumnFormat() { Title = "Last Name", Axis = ePivotFieldAxis.Row },
        new ColumnFormat() { Title = "Firt Name", Axis = ePivotFieldAxis.Row },
        new ColumnFormat() { Title = "Book D", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Date },
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
    public static List<ColumnFormat> rptFoliosCxCByPR()
    {
      return new List<ColumnFormat>()
      {
        new ColumnFormat() { Title = "Name", Axis = ePivotFieldAxis.Row, },
        new ColumnFormat() { Title = "Status", Axis = ePivotFieldAxis.Row, },
        new ColumnFormat() { Title = "From", Axis = ePivotFieldAxis.Row, },
        new ColumnFormat() { Title = "To", Axis = ePivotFieldAxis.Row, },
        new ColumnFormat() { Title = "Lead Source", Axis = ePivotFieldAxis.Row, },
        new ColumnFormat() { Title = "Last Name", Axis = ePivotFieldAxis.Row, },
        new ColumnFormat() { Title = "Firt Name", Axis = ePivotFieldAxis.Row, },
        new ColumnFormat() { Title = "Book D", Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Date },
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
    public static ColumnFormatList rptFoliosCXC()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Es Show", "EsShow", isGroup: true, isVisible: false);
      lst.Add("Tipo", "Tipo", isGroup: true, isVisible: false);
      lst.Add("PaymentSchema", "PaymentSchema", isGroup: true, isVisible: false);
      lst.Add("Folio CXC", "bdFolioCXC");
      lst.Add("PR ID", "PR");
      lst.Add("PR Name", "PRN");
      lst.Add("Out. Inv.", "guOutInvitNum");
      lst.Add("Lead Source", "lsN");
      lst.Add("Last Name", "guLastName1");
      lst.Add("First Name", "guFirstName1");
      lst.Add("Book D", "guBookD", format: EnumFormatTypeExcel.Date);
      lst.Add("Books", "Books", format: EnumFormatTypeExcel.DecimalNumber);
      lst.Add("T Books", "GrossBooks", format: EnumFormatTypeExcel.DecimalNumber);
      lst.Add("IO", "InOuts", format: EnumFormatTypeExcel.DecimalNumber);
      lst.Add("Shows", "Shows", format: EnumFormatTypeExcel.DecimalNumber);
      lst.Add("T Shows", "GrossShows", format: EnumFormatTypeExcel.DecimalNumber);
      lst.Add("Sh %", "ShowsFactor", format: EnumFormatTypeExcel.Percent);
      lst.Add("P. sch. %", "PaymentSchemaFactor", format: EnumFormatTypeExcel.Percent);
      lst.Add("CXC User", "bdUserCXC");
      lst.Add("CXC User Name", "peN");
      lst.Add("Date CXC", "bdEntryDCXC", format: EnumFormatTypeExcel.DateTime);

      return lst;
    }
    #endregion

  }
}
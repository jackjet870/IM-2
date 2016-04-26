using System.Collections.Generic;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;

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
      new ExcelFormatTable() { Title = "PR ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Total Show IO", Format=EnumFormatTypeExcel.DecimalNumber,Alignment=ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Total Book", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Sh%", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Total Shows", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "GUID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Book Date", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Out. Inv.", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "LS", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "SR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Hotel", Format=EnumFormatTypeExcel.General,Alignment=ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Refund in", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Sales Amount", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Eficiency", Format=EnumFormatTypeExcel.DecimalNumber, Alignment=ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Amount", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Received", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left }
      };
    }
    #endregion

    #region rptGiftsReceivedBySR
    /// <summary>
    ///  Formato para el reporte GiftsReceivedBySR
    /// </summary>
    /// <history>
    ///   [vku] 11/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptGiftsRecivedBySR()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoom", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1, Outline = true,SubTotalFunctions = eSubTotalFunctions.Default },
      new ExcelFormatTable() { Title = "Gift ID", PropertyName = "Gift", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2, },
      new ExcelFormatTable() { Title = "Gift Name", PropertyName= "GiftN", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 3, },
      new ExcelFormatTable() { Title = "Quantity", PropertyName = "Quantity" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 4,  },
      new ExcelFormatTable() { Title = "Couples", PropertyName = "Couples", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 5, },
      new ExcelFormatTable() { Title = "Adults", PropertyName = "Adults", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 6, },
      new ExcelFormatTable() { Title = "Minors", PropertyName = "Minors" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 7, },
      new ExcelFormatTable() { Title = "Currency", PropertyName = "cuN", Format=EnumFormatTypeExcel.General,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 2, Sort = eSortType.Ascending },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "Amount" ,Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum, },
      };
    }
    #endregion

    #region rptGuestsShowNoPresentedInvitation
    /// <summary>
    ///  Formato para el reporte GuestsShowNoPresentedInvitation
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    ///   [vku] 05/Abr/2016 created
    /// </history>
    public static List<ExcelFormatTable> rptGuestsShowNoPresentedInvitation()
    {
      return new List<ExcelFormatTable>() {
      new ExcelFormatTable() { Title = "Guest ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Lead Source", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Out Invit", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Last Name", Format=EnumFormatTypeExcel.General,Alignment=ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "First Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Show Date", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Deposit", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Received", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Currency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
      new ExcelFormatTable() { Title = "Payment Type", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left }
      };
    }
    #endregion

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
      new ExcelFormatTable() { Title = "Status", PropertyName = "Status", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "PR ID", PropertyName = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "PR Name", PropertyName = "PRN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "GrossShows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Dir", PropertyName = "Directs", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "WO", PropertyName = "WalkOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "CT", PropertyName = "CourtesyTours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10,  Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"},
      new ExcelFormatTable() { Title = "Save", PropertyName = "SaveTours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11,  Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"},
      new ExcelFormatTable() { Title = "T Sh", PropertyName = "Shows", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12,  Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING"},

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING"},
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"  },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"  },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 23, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 24, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion

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
      new ExcelFormatTable() { Title = "Age Range", PropertyName = "Age", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

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
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoomN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Age Range", PropertyName = "Age", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},

      };
    }
    #endregion

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
      new ExcelFormatTable() { Title = "Agency ID", PropertyName = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Agency", PropertyName = "Agency N", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "WO", PropertyName = "WalkOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "RT", PropertyName = "Tours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9,  Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "CT", PropertyName = "CourtesyTours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10,  Function = DataFieldFunctions.Sum,},
      new ExcelFormatTable() { Title = "Save", PropertyName = "SaveTours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11,  Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "T Tours", PropertyName = "TotalTours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12,  Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"  },
      new ExcelFormatTable() { Title = "UPS", PropertyName = "UPS", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13,  Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"},

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"  },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 23, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 24, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 25, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

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
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoomN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Agency ID", PropertyName = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Agency", PropertyName = "Agency N", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "WO", PropertyName = "WalkOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "RT", PropertyName = "Tours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9,  Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "CT", PropertyName = "CourtesyTours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10,  Function = DataFieldFunctions.Sum,},
      new ExcelFormatTable() { Title = "Save", PropertyName = "SaveTours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11,  Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "T Tours", PropertyName = "TotalTours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12,  Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"  },
      new ExcelFormatTable() { Title = "UPS", PropertyName = "UPS", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13,  Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"},

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"  },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 23, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 24, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 25, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

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
      new ExcelFormatTable() { Title = "Hotel", PropertyName = "Hotel", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true,SubTotalFunctions = eSubTotalFunctions.Default    },
      new ExcelFormatTable() { Title = "Market", PropertyName = "Market", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default },
      new ExcelFormatTable() { Title = "Agency ID", PropertyName = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "Agency", PropertyName = "AgencyN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 3  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5,  Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING"  },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

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
      new ExcelFormatTable() { Title = "Couple Type", PropertyName = "CoupleType", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

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
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoomN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Couple Type", PropertyName = "CoupleType", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
      new ExcelFormatTable() { Title = "Gift ID", PropertyName = "GiftID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0  },
      new ExcelFormatTable() { Title = "Gift Name", PropertyName = "GiftN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1, },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

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
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoomN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Gift ID", PropertyName = "Gift ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Gift Name", PropertyName = "Gift N", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"},
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING"   },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING"  },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum,  SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum,  SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
      #endregion

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
      new ExcelFormatTable() { Title = "LeadSource", PropertyName = "LeadSource", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "GuestStatus", PropertyName = "GuestStatus", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
      new ExcelFormatTable() { Title = "Nationality", PropertyName = "Nationality", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

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
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

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
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoomN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Status", PropertyName = "Status", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "PR ID", PropertyName = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "PR Name", PropertyName = "PRN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 3  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "GrossShows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Dir", PropertyName = "Directs", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "WO", PropertyName = "WalkOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10,  Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"},
      new ExcelFormatTable() { Title = "CT", PropertyName = "CourtesyTours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11,  Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Save", PropertyName = "SaveTours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12,  Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING"},
      new ExcelFormatTable() { Title = "T Sh", PropertyName = "Shows", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13,  Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"   },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"  },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 23, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 24, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 25, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }

    #endregion

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
      new ExcelFormatTable() { Title = "Status", PropertyName = "Status", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "PR ID", PropertyName = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "PR Name", PropertyName = "PRN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 3  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "GrossShows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Dir", PropertyName = "Directs", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "WO", PropertyName = "WalkOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10,  Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"},
      new ExcelFormatTable() { Title = "CT", PropertyName = "CourtesyTours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11,  Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"},
      new ExcelFormatTable() { Title = "Save", PropertyName = "SaveTours", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12,  Function = DataFieldFunctions.Sum,   SuperHeader="OUT OF PENDING"},
      new ExcelFormatTable() { Title = "T Sh", PropertyName = "Shows", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13,  Function = DataFieldFunctions.Sum,   SuperHeader="OUT OF PENDING"},

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="CANCELLED"},
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"  },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 23, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 24, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 25, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
      };
    }
    #endregion

  }
}


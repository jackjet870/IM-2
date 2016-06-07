﻿using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System.Collections.Generic;

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
      new ExcelFormatTable() { Title = "Category", PropertyName = "Category", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true,  SubTotalFunctions = eSubTotalFunctions.Default},
      new ExcelFormatTable() { Title = "Place", PropertyName = "pcN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1,  Outline = true,  SubTotalFunctions = eSubTotalFunctions.Default},
      new ExcelFormatTable() { Title = "PaymentSchema", PropertyName = "PaymentSchema", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2,  Outline = true,  SubTotalFunctions = eSubTotalFunctions.Default},
      new ExcelFormatTable() { Title = "PR ID", PropertyName = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 3 },
      new ExcelFormatTable() { Title = "PR Name", PropertyName = "PRN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 4 },

      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 5, },
      new ExcelFormatTable() { Title = "IO", PropertyName = "InOuts", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 6,   },
      new ExcelFormatTable() { Title = "Total Book", PropertyName = "GrossBooks", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 7,  },
      new ExcelFormatTable() { Title = "Total Shows", PropertyName = "GrossShows", Format=EnumFormatTypeExcel.Number, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 9, },
      new ExcelFormatTable() { Title = "Sales Amount", PropertyName = "SalesAmount", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 10 },

      new ExcelFormatTable() { Title = "Guest ID", PropertyName = "guID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left,  Axis = ePivotFieldAxis.Row, Order = 12 },
      new ExcelFormatTable() { Title = "Guest Name", PropertyName = "guName", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 13 },
      new ExcelFormatTable() { Title = "Book Date", PropertyName = "guBookD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 14 },
      new ExcelFormatTable() { Title = "Out. Inv.", PropertyName = "guOutInvitNum", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 15 },
      new ExcelFormatTable() { Title = "LS", PropertyName = "guls", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 16 },
      new ExcelFormatTable() { Title = "SR", PropertyName = "gusr", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 17 },
      new ExcelFormatTable() { Title = "Hotel", PropertyName = "guHotel", Format=EnumFormatTypeExcel.General,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 18 },
     
      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 8, Formula = "IF('GrossBooks' =0,0,'GrossShows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Eficiency", PropertyName = "Efficiency", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 11, Formula = "IF('GrossBooks' =0,0,'SalesAmount'/'GrossBooks')" },

      new ExcelFormatTable() { Title = "Currency", PropertyName = "cuN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Center, Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending},
      new ExcelFormatTable() { Title = "PaymentType", PropertyName = "ptN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Center, Axis = ePivotFieldAxis.Column, Order = 2, Sort = eSortType.Ascending },

      new ExcelFormatTable() { Title = "Amount", PropertyName = "bdAmount", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values },
      new ExcelFormatTable() { Title = "Received", PropertyName = "bdReceived", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values },
      new ExcelFormatTable() { Title = "Payment", PropertyName = "topay", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values }
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
    public static List<ExcelFormatTable> rptGiftsRecivedBySR()
    {
      return new List<ExcelFormatTable>
      {
      new ExcelFormatTable { Title = "Sales Room", PropertyName = "SalesRoom", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1, IsGroup = true  },
      new ExcelFormatTable { Title = "Gift ID", PropertyName = "Gift", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Row, Order = 2, },
      new ExcelFormatTable { Title = "Gift Name", PropertyName= "GiftN", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 3, },
      new ExcelFormatTable { Title = "Quantity", PropertyName = "Quantity" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right, Axis = ePivotFieldAxis.Row, Order = 4, SubTotalFunctions =eSubTotalFunctions.Sum },
      new ExcelFormatTable { Title = "Couples", PropertyName = "Couples", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right, Axis = ePivotFieldAxis.Row, Order = 5, SubTotalFunctions =eSubTotalFunctions.Sum},
      new ExcelFormatTable { Title = "Adults", PropertyName = "Adults", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right, Axis = ePivotFieldAxis.Row, Order = 6, SubTotalFunctions =eSubTotalFunctions.Sum},
      new ExcelFormatTable { Title = "Minors", PropertyName = "Minors" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Right, Axis = ePivotFieldAxis.Row, Order = 7, SubTotalFunctions =eSubTotalFunctions.Sum },
      new ExcelFormatTable { Title = "cuID", PropertyName = "cuID", Format=EnumFormatTypeExcel.General,Alignment=ExcelHorizontalAlignment.Center, Axis = ePivotFieldAxis.Column, Order = 8, Sort = eSortType.Ascending, IsVisible = false},
      new ExcelFormatTable { Title = "Currency", PropertyName = "cuN", Format=EnumFormatTypeExcel.General,Alignment=ExcelHorizontalAlignment.Center, Axis = ePivotFieldAxis.Column, Order = 9},
      new ExcelFormatTable { Title = "Amount", PropertyName = "Amount" ,Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Right, Axis = ePivotFieldAxis.Values, Order = 10, SubTotalFunctions =eSubTotalFunctions.Sum },
      };
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
    public static List<ExcelFormatTable> rptGuestsShowNoPresentedInvitation()
    {
      return new List<ExcelFormatTable>() {
      new ExcelFormatTable() { Title = "Guest ID", Format = EnumFormatTypeExcel.Id, Alignment = ExcelHorizontalAlignment.Left },
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
      new ExcelFormatTable() { Title = "Agency ID", PropertyName = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Agency", PropertyName = "AgencyN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2  },
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

    #endregion rptProductionByAgencyOuthouse

    #region rptProductionByAgencySalesMembershipTypeOuthouse

    /// <summary>
    ///  Formato para el reporte productionByAgency details the Sales MembershipType
    /// </summary>
    /// <history>
    ///   [vku] 28/Abr/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptProductionByAgencySalesMembershipTypeOuthouse()
    {
      return new List<ExcelFormatTable>()
      {
       new ExcelFormatTable() { Title = "mtN", PropertyName = "mtN",  Axis = ePivotFieldAxis.Column, Order = 1 },

      new ExcelFormatTable() { Title = "Agency ID", PropertyName = "Agency", Format = EnumFormatTypeExcel.General, Axis = ePivotFieldAxis.Row, Order = 1,  },
      new ExcelFormatTable() { Title = "Agency", PropertyName = "AgencyN", Format = EnumFormatTypeExcel.General,  Axis = ePivotFieldAxis.Row, Order = 2  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Row, Order = 3, Function = DataFieldFunctions.Sum} ,
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 4, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 5, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Row, Order = 6, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "WO", PropertyName = "WalkOuts", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 8,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "RT", PropertyName = "Tours", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Row, Order = 9, Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "CT", PropertyName = "CourtesyTours", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 10, Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "Save", PropertyName = "SaveTours", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Row, Order = 11, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Tours", PropertyName = "TotalTours", Format = EnumFormatTypeExcel.Number,  Axis = ePivotFieldAxis.Row, Order = 12, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "UPS", PropertyName = "UPS", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 13,  Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales" ,Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount", Format=EnumFormatTypeExcel.Currency,Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales Cancelled", PropertyName = "SalesCancel", Format=EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 16, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Amount Cancelled ", PropertyName = "SalesAmountCancel", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 17,Function = DataFieldFunctions.Sum },

      new ExcelFormatTable() { Title = "Sales Total", PropertyName = "SalesTotal", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Row, Order = 18, Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "Amount Total", PropertyName = "SalesAmountTotal", Format = EnumFormatTypeExcel.Currency, Axis = ePivotFieldAxis.Row, Order = 19, Function = DataFieldFunctions.Sum},

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Order = 7, IsCalculated = true, Formula = "IF([GrossBooks] =0,0,[Shows]/[GrossBooks])", },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent,  Order = 20, IsCalculated = true, Formula = "IF(([SalesAmountTotal]+[SalesAmountCancel]) =0,0,[SalesAmountCancel]/([SalesAmountTotal]+[SalesAmountCancel]))" },
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Order = 21, IsCalculated = true, Formula = "IF([Shows] =0,0,[SalesAmountTotal]/[Shows])" },
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Order = 22, IsCalculated = true, Formula = "IF([Shows] =0,0,[SalesTotal]/[Shows])" }, 
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency,  Order = 23, IsCalculated = true, Formula = "IF([SalesTotal] =0,0,[SalesAmountTotal]/[SalesTotal])",}, 
      };
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
      new ExcelFormatTable() { Title = "Flight", PropertyName = "FlightID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1 },
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoomID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
      new ExcelFormatTable() { Title = "Hotel", PropertyName = "HotelID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1 },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
      new ExcelFormatTable() { Title = "Hotel", PropertyName = "HotelID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1 },
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoomID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
      new ExcelFormatTable() { Title = "Hotel", PropertyName = "HotelID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1 },
      new ExcelFormatTable() { Title = "Hotel Group", PropertyName = "hoGroup", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
      new ExcelFormatTable() { Title = "Hotel Group", PropertyName = "hoGroup", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1, Outline = true,SubTotalFunctions = eSubTotalFunctions.Default },
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoomID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0,  Outline = true,SubTotalFunctions = eSubTotalFunctions.Default  },
      new ExcelFormatTable() { Title = "Hotel", PropertyName = "HotelID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 2},
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum, SuperHeader="PROCESSABLE"  },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum,  SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 22, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum,  SuperHeader="PENDING"},
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, SuperHeader="PENDING"   },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED"  },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 22, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 23, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 24, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 25, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 26, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 27, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
      new ExcelFormatTable() { Title = "Book Time", PropertyName = "BookTime", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true,SubTotalFunctions = eSubTotalFunctions.Default   },
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoomID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
      new ExcelFormatTable() { Title = "Book Time", PropertyName = "BookTime", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 0, Outline = true,SubTotalFunctions = eSubTotalFunctions.Default   },
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoomID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum,  SuperHeader="PROCESSABLE" },

      new ExcelFormatTable() { Title = "Sales", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum, SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.Currency, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum,  SuperHeader="OUT OF PENDING" },
      new ExcelFormatTable() { Title = "Sales ", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Amount ", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum, SuperHeader="PENDING" },
      new ExcelFormatTable() { Title = "Sales  ", PropertyName = "Sales_PEND", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Amount  ", PropertyName = "SalesAmount_PEND", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum, SuperHeader="CANCELLED" },
      new ExcelFormatTable() { Title = "Sales   ", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Amount   ", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum, SuperHeader="TOTAL"},
      new ExcelFormatTable() { Title = "Sales    ", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum, },
      new ExcelFormatTable() { Title = "Amount    ", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum,  },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal" , Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum,  },

      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Formula = "IF('GrossBooks' =0,0,'Shows'/'GrossBooks')" },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Formula = "IF(('SalesAmount_TOTAL'+'SalesAmount_CANCEL') =0,0,'SalesAmount_CANCEL'/('SalesAmount_TOTAL'+'SalesAmount_CANCEL'))"},
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Formula = "IF('Shows' =0,0,'SalesAmount_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Cl%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 20, Formula = "IF('Shows' =0,0,'Sales_TOTAL'/'Shows')"},
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 21, Formula = "IF('Sales_TOTAL' =0,0,'SalesAmount_TOTAL'/'Sales_TOTAL')"},
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
    public static List<ExcelFormatTable> rptFoliosInvitationByDateFolio()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() { Title = "Out. Inv.", Axis = ePivotFieldAxis.Row },
        new ExcelFormatTable() { Title = "PR ID", Axis = ePivotFieldAxis.Row },
        new ExcelFormatTable() { Title = "PR Name", Axis = ePivotFieldAxis.Row },
        new ExcelFormatTable() { Title = "Last Name", Axis = ePivotFieldAxis.Row },
        new ExcelFormatTable() { Title = "Book D", Axis = ePivotFieldAxis.Row },
        new ExcelFormatTable() { Title = "Lead Source", Axis = ePivotFieldAxis.Row },
      };
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
    public static List<ExcelFormatTable> rptFoliosCXC()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable { Title = "Es Show", PropertyName = "EsShow",  Order = 1, IsGroup = true},
        new ExcelFormatTable { Title = "Tipo", PropertyName = "Tipo",  Order = 2, IsGroup = true},
        new ExcelFormatTable { Title = "Folio CXC", PropertyName = "bdFolioCXC", Order = 1 },
        new ExcelFormatTable { Title = "PR ID", PropertyName = "PR", Order = 2 },
        new ExcelFormatTable { Title = "PR Name", PropertyName = "PRN", Order = 3 },
        new ExcelFormatTable { Title = "Out. Inv.", PropertyName = "guOutInvitNum", Order = 3  },
        new ExcelFormatTable { Title = "Lead Source", PropertyName = "lsN", Order = 3 },
        new ExcelFormatTable { Title = "Last Name", PropertyName = "guLastName1",  Order = 3 },
        new ExcelFormatTable { Title = "First Name", PropertyName = "guFirstName1", Order = 3  },
        new ExcelFormatTable { Title = "Book D", PropertyName = "guBookD",  Format = EnumFormatTypeExcel.Date, Order = 3 },
        new ExcelFormatTable { Title = "CXC User", PropertyName = "bdUserCXC",  },
        new ExcelFormatTable { Title = "CXC User Name", PropertyName = "peN", },
        new ExcelFormatTable { Title = "Date CXC", PropertyName = "bdEntryDCXC",Format = EnumFormatTypeExcel.Date, Order = 3 },
      };
    }
    #endregion

  }
}
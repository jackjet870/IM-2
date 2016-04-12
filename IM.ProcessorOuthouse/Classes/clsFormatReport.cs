using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using System.Windows;
using System.Data;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;

namespace IM.ProcessorOuthouse.Classes
{
  public class clsFormatReport
  {
    #region rptDepositsPaymentByPR
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
    public static List<ExcelFormatTable> rptGiftsRecivedBySR()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Sales Room", PropertyName = "SalesRoom", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1 },
      new ExcelFormatTable() { Title = "Gift ID", PropertyName = "Gift", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2, },
      new ExcelFormatTable() { Title = "Gift Name", PropertyName= "GiftN", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Quantity", PropertyName = "Quantity" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Couples", PropertyName = "Couples", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Adults", PropertyName = "Adults", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Minors", PropertyName = "Minors" , Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum },
       new ExcelFormatTable() { Title = "Currency", PropertyName = "Currency", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Column, Order = 1, Sort = eSortType.Ascending },
      new ExcelFormatTable() { Title = "Amount", PropertyName = "Amount" ,Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 1, Function = DataFieldFunctions.Sum },
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

    #region rptProductionByAge
    public static List<ExcelFormatTable> rptProductionByAge()
    {
      return new List<ExcelFormatTable>()
      {
      new ExcelFormatTable() { Title = "Age Range", PropertyName = "Age", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Row, Order = 1  },
      new ExcelFormatTable() { Title = "Books", PropertyName = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 2,  Function = DataFieldFunctions.Sum},
      new ExcelFormatTable() { Title = "IO", PropertyName= "InOuts", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 3, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "T Bk", PropertyName = "GrossBooks", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 4, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Shows", PropertyName = "Shows" , Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 5, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Sh%", PropertyName = "ShowsFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 6, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Sales TOTAL", PropertyName = "Sales_TOTAL", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 14, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Amount TOTAL", PropertyName = "SalesAmount_TOTAL" , Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 15, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Sales PROCESSABLE", PropertyName = "Sales_PROC" ,Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 7, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Amount PROCESSABLE", PropertyName = "SalesAmount_PROC", Format=EnumFormatTypeExcel.DecimalNumber, Alignment=ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 8, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Sales OUT-OF-PEDING", PropertyName = "Sales_OOP", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 9, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Amount OUT-OF-PEDING", PropertyName = "SalesAmount_OOP", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 10, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Sales CANCELLED", PropertyName = "Sales_CANCEL", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 11, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Amount CANCELLED", PropertyName = "SalesAmount_CANCEL", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 12, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Subtotal", PropertyName = "Subtotal", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 13, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Ca%", PropertyName = "CancelFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 16, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "CI%", PropertyName = "ClosingFactor", Format = EnumFormatTypeExcel.Percent, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 18, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Eff", PropertyName = "Efficiency", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 17, Function = DataFieldFunctions.Sum },
      new ExcelFormatTable() { Title = "Avg Sale", PropertyName = "AverageSale", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Axis = ePivotFieldAxis.Values, Order = 19, Function = DataFieldFunctions.Sum },
      };
    }
    #endregion
  }
}

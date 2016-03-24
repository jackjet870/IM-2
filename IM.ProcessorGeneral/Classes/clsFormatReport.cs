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

namespace IM.ProcessorGeneral.Classes
{
  public static class clsFormatReport
  {
    #region Reports by Sales Room

    #region rptBookingsBySalesRoomProgramTime
    // <summary>
    /// Formato para el reporte Bookings By Sales Room, Program & Time
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptBookingsBySalesRoomProgramTime()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Program", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Book Type", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title="Time", Format=EnumFormatTypeExcel.General,Alignment=ExcelHorizontalAlignment.Left},
new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left }
      };
    }
    #endregion

    #region rptBookingsBySalesRoomProgramLeadSourceTime
    /// <summary>
    /// Formato para el reporte Bookings By Sales Room, Program, Lead Sources & Time
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptBookingsBySalesRoomProgramLeadSourceTime()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "Sales Room", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Program", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Lead Source", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Book Type", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Time", Format=EnumFormatTypeExcel.General,Alignment=ExcelHorizontalAlignment.Left},
new ExcelFormatTable() { Title = "Books", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left }
      };
    }
    #endregion

    #region rptCxC

    /// <summary>
    /// Formato para el reporte CxC
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 22/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptCxC()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Group", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Chb", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "PP", Format=EnumFormatTypeExcel.Number,Alignment=ExcelHorizontalAlignment.Left},
new ExcelFormatTable() { Title = "Rcpt", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Date", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Guest ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Guest Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Qty", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Gift", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Ad", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Min", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Folios", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Total Gifts", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC Gift", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC Adj", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC Deposit", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Currency Deposit", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Ex. Rate Deposit", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Deposit US", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Deposit MN", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC Taxi Out", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Currency Taxi Out", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Ex. Rate Taxi Out", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Taxi Out US", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Taxi Out MN", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Total CxC", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC Paid US", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC Paid MN", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Ex. Rate", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC Comments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Receipt Comments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left }
      };
    }

    #endregion

    #region rptCxC Deposits
    /// <summary>
    /// Formato para el reporte CxC Deposits
    /// </summary>
    /// <returns> List<ExcelFormatTable> </returns>
    /// <history>
    /// [edgrodriguez] 23/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptCxCDeposits()
    {
      return new List<ExcelFormatTable>
      {
new ExcelFormatTable() { Title = "Ch B", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Chb PP", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Date", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "LS", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "GUID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Guest Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Hotel", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "PR Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Host", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Host Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CxC Currency", Format=EnumFormatTypeExcel.General, Alignment=ExcelHorizontalAlignment.Left}
      };
    } 
    #endregion

    #endregion

    #region General Reports
    #region rptGifts
    /// <summary>
    /// Formato para el reporte general de Gifts
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptGifts()
    {
      return new List<ExcelFormatTable>(){
new ExcelFormatTable() { Title = "Gift ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Gift Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Short N.", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Order", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Price", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Price Min.", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CXC", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "CXC Min.", Format = EnumFormatTypeExcel.Currency, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Package", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Category", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Inv.", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Folio", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Pax", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Active", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
      };
    }
    #endregion

    #region rptPersonnel
    /// <summary>
    /// Formato para el reporte general de Personnel
    /// </summary>
    /// <returns>List<ExcelFormatTable></returns>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static List<ExcelFormatTable> rptPersonnel()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "Status", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Dept", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Post", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Place", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "ID", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Active", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Collaborator", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "Captain", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left },
new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "PR Mem", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Liner", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Closer", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Exit", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Podium", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "PR Capt", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "PR Sup", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Ln Capt", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Clo Capt", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Entry H", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Gift H", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Exit H", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "VLO", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Manager", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center },
new ExcelFormatTable() { Title = "Admin", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Center }
      };
    }
    #endregion 
    #endregion
  }
}
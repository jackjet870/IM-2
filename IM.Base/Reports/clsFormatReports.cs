using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Base.Reports
{
  public static class clsFormatReports
  {
    #region RptManifestRangeByLs

    /// <summary>
    /// Formato para el reporte Guest Manifest Range de Host y Processor General
    /// </summary>
    /// <history>
    /// [edgrodriguez] 13/Jun/2016 Created
    /// </history>
    public static ExcelFormatItemsList RptManifestRangeByLs()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Date", "DateManifest", format: EnumFormatTypeExcel.Date);
      lst.Add("Group", "SaleTypeN", isGroup: true);
      lst.Add("LocationN", "LocationN", isGroup: true, isVisible: false);
      lst.Add("SR", "SalesRoom");
      lst.Add("LS", "LeadSource");
      lst.Add("GUID", "guID");
      lst.Add("Last Name", "LastName");
      lst.Add("First Name", "FirstName");
      lst.Add("Country", "CountryD");
      lst.Add("Show D", "ShowD", format: EnumFormatTypeExcel.Date);
      lst.Add("Hotel", "Hotel");
      lst.Add("Room", "Room");
      lst.Add("Agency", "AgencyN");
      lst.Add("Pax", "Pax", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("In", "TimeInT", format: EnumFormatTypeExcel.Time);
      lst.Add("Out", "TimeOutT", format: EnumFormatTypeExcel.Time);
      lst.Add("Host", "Hostess");
      lst.Add("Sh", "Show", format: EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count);
      lst.Add("Tour", "Tour", format: EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count);
      lst.Add("IO", "IO", format: EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count);
      lst.Add("WO", "WO", format: EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count);
      lst.Add("CT", "CTour", format: EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count);
      lst.Add("Sv", "STour", format: EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count);
      lst.Add("D", "Direct", format: EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count);
      lst.Add("R", "Resch", format: EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count);
      lst.Add("N°", "PR1");
      lst.Add("PR/SG", "PR1N");
      lst.Add("N°", "Liner1");
      lst.Add("Sales R.", "Liner1N");
      lst.Add("N°", "Closer1");
      lst.Add("FM1", "Closer1N");
      lst.Add("N°", "Closer2");
      lst.Add("FM2", "Closer2N");
      lst.Add("N°", "Exit1");
      lst.Add("JRFM", "Exit1N");
      lst.Add("Aff", "saMembershipNum");
      lst.Add("Sale", "ProcSales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Proc Orig", "ProcOriginal", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Proc New", "ProcNew", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Proc Gross", "ProcGross", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Pend Sales", "PendSales", format: EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum);
      lst.Add("Pend Original", "PendOriginal", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Pend New", "PendNew", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Pend Gross", "PendGross", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("Deposit Sale", "DepositSale");
      lst.Add("Deposit #", "DepositSaleNum");
      lst.Add("CC", "CC");
      lst.Add("Comments", "Comments");
      lst.Add("SaleType", "SaleType", isVisible: false);
      lst.Add("Location", "Location", isVisible: false);
      lst.Add("Sequency", "Sequency", isVisible: false);
      lst.Add("ShowProgram", "ShowProgramN", isVisible: false);
      lst.Add("Agency ID", "Agency", isVisible: false);
      lst.Add("Country ID", "Country", isVisible: false);
      lst.Add("PR 2", "PR2", isVisible: false);
      lst.Add("PR 2 Name", "PR2N", isVisible: false);
      lst.Add("PR 3", "PR3", isVisible: false);
      lst.Add("PR 3 Name", "PR3N", isVisible: false);
      lst.Add("Liner 2", "Liner2", isVisible: false);
      lst.Add("Liner 2 Name", "Liner2N", isVisible: false);
      lst.Add("Closer 3", "Closer3", isVisible: false);
      lst.Add("Closer 3 Name", "Closer3N", isVisible: false);
      lst.Add("Exit 2", "Exit2", isVisible: false);
      lst.Add("Exit 2 Name", "Exit2N", isVisible: false);
      lst.Add("% DP", "saDownPaymentPercentage", format: EnumFormatTypeExcel.Percent, isVisible: false);
      lst.Add("% EDP", "saDownPaymentPaidPercentage", format: EnumFormatTypeExcel.Percent, isVisible: false);
      lst.Add("saDownPayment", "saDownPayment", isVisible: false);
      lst.Add("saDownPaymentPaid", "saDownPaymentPaid", isVisible: false);
      return lst;
    }

    /// <summary>
    /// Formato para el reporte Guest Manifest Range de Host y Processor General
    /// </summary>
    /// <history>
    /// [edgrodriguez] 13/Jun/2016 Created
    /// </history>
    public static ExcelFormatItemsList RptManifestRangeByLs_Bookings()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("guloInvit", "guloInvit", isVisible: false);
      lst.Add("LocationN", "LocationN", axis: ePivotFieldAxis.Row);
      lst.Add("guBookT", "guBookT", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("guBookTime", "guBookTime", axis: ePivotFieldAxis.Values);
      lst.Add("Bookings", "Bookings", format: EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values);
      return lst;
    }

    #endregion RptManifestRangeByLs

    #region RptPremanifest
    /// <summary>
    /// Formato para el reporte Premanifest de Hosts e Inhouse
    /// </summary>
    /// <returns> List of ExcelFormatTable</returns>
    /// <history>
    /// [edgrodriguez] 21/Jun/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptPremanifest()
    {
      return new List<ExcelFormatTable>() {
      new ExcelFormatTable() { Title = "SR", PropertyName = "srN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, IsGroup = true, Order = 1 },
        new ExcelFormatTable() { Title = "LS", PropertyName = "loN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, IsGroup = true, Order = 2 },
        new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Format = EnumFormatTypeExcel.Id, Alignment = ExcelHorizontalAlignment.Left, Order = 1 },
        new ExcelFormatTable() { Title = "Hotel", PropertyName = "guHotel", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 2 },
        new ExcelFormatTable() { Title = "Room", PropertyName = "guRoomNum", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 3 },
        new ExcelFormatTable() { Title = "LastName", PropertyName = "guLastName1", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 4 },
        new ExcelFormatTable() { Title = "Pax", PropertyName = "guPax", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Order = 5 },
        new ExcelFormatTable() { Title = "Check-In", PropertyName = "guCheckInD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Order = 6 },
        new ExcelFormatTable() { Title = "Check-Out", PropertyName = "guCheckOutD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Order = 7 },
        new ExcelFormatTable() { Title = "Country ID", PropertyName = "guco", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 8 },
        new ExcelFormatTable() { Title = "Country", PropertyName = "coN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 9 },
        new ExcelFormatTable() { Title = "Agency ID", PropertyName = "guag", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 10 },
        new ExcelFormatTable() { Title = "Agency", PropertyName = "agN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 11 },
        new ExcelFormatTable() { Title = "Invit D", PropertyName = "guInvitD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Order = 12 },
        new ExcelFormatTable() { Title = "Invit T", PropertyName = "guInvitT", Format = EnumFormatTypeExcel.Time, Alignment = ExcelHorizontalAlignment.Left, Order = 13 },
        new ExcelFormatTable() { Title = "Book T", PropertyName = "guBookT", Format = EnumFormatTypeExcel.Time, Alignment = ExcelHorizontalAlignment.Left, Order = 14 },
        new ExcelFormatTable() { Title = "PR B", PropertyName = "guPRInvit1", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 15 },
        new ExcelFormatTable() { Title = "Clxd", PropertyName = "guBookCanc", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left, Order = 16 },
        new ExcelFormatTable() { Title = "Rsc", PropertyName = "guResch", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left, Order = 17 },
        new ExcelFormatTable() { Title = "Sh", PropertyName = "guShow", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left, Order = 18 },
        new ExcelFormatTable() { Title = "Sale", PropertyName = "guSale", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 19 },
        new ExcelFormatTable() { Title = "InvitType", PropertyName = "InvitType", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 20 },
        new ExcelFormatTable() { Title = "Deposits_Comments", PropertyName = "guComments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 21 }
      };
    }
    #endregion

    #region RptPremanifestWithGifts
    /// <summary>
    /// Formato para el reporte Premanifest With Gifts de Host e Inhouse
    /// </summary>
    /// <returns> List of ExcelFormatTable</returns>
    /// <history>
    /// [edgrodriguez] 21/Jun/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptPremanifestWithGifts()
    {
      return new List<ExcelFormatTable>()
      {
        new ExcelFormatTable() { Title = "Sales Room", PropertyName = "srN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, IsGroup = true, Order = 1 },
        new ExcelFormatTable() { Title = "Location", PropertyName = "loN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, IsGroup = true, Order = 2 },
        new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Format = EnumFormatTypeExcel.Id, Alignment = ExcelHorizontalAlignment.Left, Order = 1 },
      new ExcelFormatTable() { Title = "LS", PropertyName = "guls", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 2 },
      new ExcelFormatTable() { Title = "Room", PropertyName = "guRoomNum", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 3 },
      new ExcelFormatTable() { Title = "Last Name", PropertyName = "guLastName1", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 4 },
      new ExcelFormatTable() { Title = "County ID", PropertyName = "guco", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 5 },
      new ExcelFormatTable() { Title = "County", PropertyName = "coN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 6 },
      new ExcelFormatTable() { Title = "Hotel", PropertyName = "guHotel", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 7 },
      new ExcelFormatTable() { Title = "Pax", PropertyName = "guPax", Format = EnumFormatTypeExcel.DecimalNumber, Alignment = ExcelHorizontalAlignment.Left, Order = 8 },
      new ExcelFormatTable() { Title = "Invit D", PropertyName = "guInvitD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Order = 9 },
      new ExcelFormatTable() { Title = "Invit T", PropertyName = "guInvitT", Format = EnumFormatTypeExcel.Time, Alignment = ExcelHorizontalAlignment.Left, Order = 10 },
      new ExcelFormatTable() { Title = "Resch", PropertyName = "guReschDT", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Order = 11 },
      new ExcelFormatTable() { Title = "CheckIn", PropertyName = "guCheckInD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Order = 12 },
      new ExcelFormatTable() { Title = "CheckOut", PropertyName = "guCheckOutD", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left, Order = 13 },
      new ExcelFormatTable() { Title = "Agency ID", PropertyName = "guag", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 14 },
      new ExcelFormatTable() { Title = "Agency", PropertyName = "agN", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 15 },
      new ExcelFormatTable() { Title = "Comments", PropertyName = "guComments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 16 },
      new ExcelFormatTable() { Title = "Book T", PropertyName = "guBookT", Format = EnumFormatTypeExcel.Time, Alignment = ExcelHorizontalAlignment.Left, Order = 17 },
      new ExcelFormatTable() { Title = "PR", PropertyName = "guPRInvit1", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 18 },
      new ExcelFormatTable() { Title = "G.T./CC", PropertyName = "guO2", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 19 },
      new ExcelFormatTable() { Title = "CCType", PropertyName = "guCCType", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 20 },
      new ExcelFormatTable() { Title = "Sh", PropertyName = "guShow", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 21 },
      new ExcelFormatTable() { Title = "Sa", PropertyName = "guSale", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 22 },
      new ExcelFormatTable() { Title = "Canc Book", PropertyName = "guBookCanc", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 23 },
      new ExcelFormatTable() { Title = "Gifts", PropertyName = "Gifts", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left, Order = 24 }
    };
    }
    #endregion

    #region RptGuestLog
    /// <summary>
    /// Formato para el reporte de Guest Log
    /// </summary>
    /// <history>
    /// [jorcanche]  created 07072016 
    /// </history>    
    public static List<ExcelFormatTable> RptGuestLog()
    {
      return new List<ExcelFormatTable>
      {    
        new ExcelFormatTable { PropertyName = "glChangedBy",  Title = "By", Alignment = ExcelHorizontalAlignment.Left, Order = 2 },
        new ExcelFormatTable { PropertyName = "ChangedByN",  Title = "Name", Alignment = ExcelHorizontalAlignment.Left, Order = 3 },
        new ExcelFormatTable { PropertyName = "glID",  Title = "Update Date/Time", Alignment = ExcelHorizontalAlignment.Left, Order = 4 ,Format = EnumFormatTypeExcel.DateTime},
        new ExcelFormatTable { PropertyName = "glsr",  Title = "S R", Alignment = ExcelHorizontalAlignment.Left, Order = 5 },
        new ExcelFormatTable { PropertyName = "glLastName1",  Title = "Last Name", Alignment = ExcelHorizontalAlignment.Left, Order = 6 },
        new ExcelFormatTable { PropertyName = "glFirstName1",  Title = "First Name", Alignment = ExcelHorizontalAlignment.Left, Order = 7 },
        new ExcelFormatTable { PropertyName = "glLastName2",  Title = "Last Name 2", Alignment = ExcelHorizontalAlignment.Left, Order = 8 },
        new ExcelFormatTable { PropertyName = "glFirstName2",  Title = "First Name 2", Alignment = ExcelHorizontalAlignment.Left, Order = 9 },
        new ExcelFormatTable { PropertyName = "glHReservID",  Title = "Reserv. #", Alignment = ExcelHorizontalAlignment.Left, Order = 10 },
        new ExcelFormatTable { PropertyName = "glAvailBySystem",  Title = "Avl Sys", Alignment = ExcelHorizontalAlignment.Left, Order = 11 },
        new ExcelFormatTable { PropertyName = "glOriginAvail",  Title = "Orig Avl", Alignment = ExcelHorizontalAlignment.Left, Order = 12 },
        new ExcelFormatTable { PropertyName = "glAvail",  Title = "Avl", Alignment = ExcelHorizontalAlignment.Left, Order = 13 },
        new ExcelFormatTable { PropertyName = "umN",  Title = "Unavailable Motive", Alignment = ExcelHorizontalAlignment.Left, Order = 14 },
        new ExcelFormatTable { PropertyName = "glPRAvail",  Title = "PR Avail", Alignment = ExcelHorizontalAlignment.Left, Order = 15 },
        new ExcelFormatTable { PropertyName = "PRAvailN",  Title = "PR Avail Name", Alignment = ExcelHorizontalAlignment.Left, Order = 16 },
        new ExcelFormatTable { PropertyName = "glInfoD",  Title = "Info D", Alignment = ExcelHorizontalAlignment.Left, Order = 17, Format=EnumFormatTypeExcel.Date},
        new ExcelFormatTable { PropertyName = "glPRInfo",  Title = "PR Info", Alignment = ExcelHorizontalAlignment.Left, Order = 18 },
        new ExcelFormatTable { PropertyName = "PRInfoN",  Title = "PR Info Name", Alignment = ExcelHorizontalAlignment.Left, Order = 19 },
        new ExcelFormatTable { PropertyName = "glFollowD",  Title = "Follow D", Alignment = ExcelHorizontalAlignment.Left, Order = 20, Format=EnumFormatTypeExcel.Date },
        new ExcelFormatTable { PropertyName = "glPRFollow",  Title = "PR Follow", Alignment = ExcelHorizontalAlignment.Left, Order = 21 },
        new ExcelFormatTable { PropertyName = "PRFollowN",  Title = "PR Follow Name", Alignment = ExcelHorizontalAlignment.Left, Order = 22 },
        new ExcelFormatTable { PropertyName = "glBookD",  Title = "Book D", Alignment = ExcelHorizontalAlignment.Left, Order = 23 , Format=EnumFormatTypeExcel.Date},
        new ExcelFormatTable { PropertyName = "glBookT",  Title = "Book T", Alignment = ExcelHorizontalAlignment.Left, Order = 24 , Format=EnumFormatTypeExcel.Time},
        new ExcelFormatTable { PropertyName = "glReschD",  Title = "Resch D", Alignment = ExcelHorizontalAlignment.Left, Order = 25 , Format=EnumFormatTypeExcel.Date},
        new ExcelFormatTable { PropertyName = "glReschT",  Title = "Resch T", Alignment = ExcelHorizontalAlignment.Left, Order = 26 , Format=EnumFormatTypeExcel.Time},
        new ExcelFormatTable { PropertyName = "glPRInvit1",  Title = "PR B", Alignment = ExcelHorizontalAlignment.Left, Order = 27 },
        new ExcelFormatTable { PropertyName = "PRInvit1N",  Title = "PR Booking Name", Alignment = ExcelHorizontalAlignment.Left, Order = 28 },
        new ExcelFormatTable { PropertyName = "glPRInvit2",  Title = "PR B 2", Alignment = ExcelHorizontalAlignment.Left, Order = 29 },
        new ExcelFormatTable { PropertyName = "PRInvit2N",  Title = "PR Booking 2 Name", Alignment = ExcelHorizontalAlignment.Left, Order = 30 },
        new ExcelFormatTable { PropertyName = "glBookCanc",  Title = "C.Bk", Alignment = ExcelHorizontalAlignment.Left, Order = 31 },
        new ExcelFormatTable { PropertyName = "glLiner1",  Title = "Liner 1", Alignment = ExcelHorizontalAlignment.Left, Order = 32 },
        new ExcelFormatTable { PropertyName = "Liner1N",  Title = "Liner 1 Name", Alignment = ExcelHorizontalAlignment.Left, Order = 33 },
        new ExcelFormatTable { PropertyName = "glLiner2",  Title = "Liner 2", Alignment = ExcelHorizontalAlignment.Left, Order = 34 },
        new ExcelFormatTable { PropertyName = "Liner2N",  Title = "Liner 2 Name", Alignment = ExcelHorizontalAlignment.Left, Order = 35 },
        new ExcelFormatTable { PropertyName = "glCloser1",  Title = "Closer 1", Alignment = ExcelHorizontalAlignment.Left, Order = 36 },
        new ExcelFormatTable { PropertyName = "Closer1N",  Title = "Closer 1 Name", Alignment = ExcelHorizontalAlignment.Left, Order = 37 },
        new ExcelFormatTable { PropertyName = "glCloser2",  Title = "Closer 2", Alignment = ExcelHorizontalAlignment.Left, Order =  38},
        new ExcelFormatTable { PropertyName = "Closer2N",  Title = "Closer 2 Name", Alignment = ExcelHorizontalAlignment.Left, Order = 39 },
        new ExcelFormatTable { PropertyName = "glCloser3",  Title = "Closer 3'", Alignment = ExcelHorizontalAlignment.Left, Order = 40 },
        new ExcelFormatTable { PropertyName = "Closer3N",  Title = "Closer 3 Name", Alignment = ExcelHorizontalAlignment.Left, Order = 41 },
        new ExcelFormatTable { PropertyName = "glExit1",  Title = "Exit1", Alignment = ExcelHorizontalAlignment.Left, Order = 42 },
        new ExcelFormatTable { PropertyName = "Exit1N",  Title = "Exit 1 Name", Alignment = ExcelHorizontalAlignment.Left, Order = 43},
        new ExcelFormatTable { PropertyName = "glExit2",  Title = "Exit2", Alignment = ExcelHorizontalAlignment.Left, Order = 44 },
        new ExcelFormatTable { PropertyName = "Exit2N",  Title = "Exit 2 Name", Alignment = ExcelHorizontalAlignment.Left, Order = 45 },
        new ExcelFormatTable { PropertyName = "glPodium",  Title = "Podium", Alignment = ExcelHorizontalAlignment.Left, Order = 46 },
        new ExcelFormatTable { PropertyName = "PodiumN",  Title = "Podium Name", Alignment = ExcelHorizontalAlignment.Left, Order = 48 },
        new ExcelFormatTable { PropertyName = "glVLO",  Title = "VLO", Alignment = ExcelHorizontalAlignment.Left, Order = 49 },
        new ExcelFormatTable { PropertyName = "VLON",  Title = "VLO Name", Alignment = ExcelHorizontalAlignment.Left, Order = 50 },
        new ExcelFormatTable { PropertyName = "glShow",  Title = "Sh", Alignment = ExcelHorizontalAlignment.Left, Order = 51 },
        new ExcelFormatTable { PropertyName = "glShowD",  Title = "Show D", Alignment = ExcelHorizontalAlignment.Left, Order = 52 , Format=EnumFormatTypeExcel.Date },
        new ExcelFormatTable { PropertyName = "glQ",  Title = "Q", Alignment = ExcelHorizontalAlignment.Left, Order = 53 },
        new ExcelFormatTable { PropertyName = "glInOut",  Title = "IO", Alignment = ExcelHorizontalAlignment.Left, Order = 54 },
        new ExcelFormatTable { PropertyName = "glWalkOut",  Title = "WO", Alignment = ExcelHorizontalAlignment.Left, Order = 55 },
        new ExcelFormatTable { PropertyName = "glCTour",  Title = "CT", Alignment = ExcelHorizontalAlignment.Left, Order = 56 },
        new ExcelFormatTable { PropertyName = "glReimpresion",  Title = "Re-Printed", Alignment = ExcelHorizontalAlignment.Left, Order = 57 },
        new ExcelFormatTable { PropertyName = "rmN",  Title = "Re-Print Motive", Alignment = ExcelHorizontalAlignment.Left, Order = 58 }
      };
    } 
    #endregion
  }
}

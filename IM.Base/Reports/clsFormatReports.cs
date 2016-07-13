﻿using IM.Model.Classes;
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
    public static List<ExcelFormatTable> RptManifestRangeByLs()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "SaleType", PropertyName = "SaleType", IsVisible = false },
new ExcelFormatTable() { Title = "SaleTypeN", PropertyName = "SaleTypeN", IsGroup = true, Order = 1 },
new ExcelFormatTable() { Title = "Date", PropertyName = "DateManifest", Format = EnumFormatTypeExcel.Date, Order = 1 },
new ExcelFormatTable() { Title = "GUID", PropertyName = "guID", Order = 4 },
new ExcelFormatTable() { Title = "Location", PropertyName = "Location", IsVisible = false},
new ExcelFormatTable() { Title = "LocationN", PropertyName = "LocationN", IsGroup = true, Order = 2 },
new ExcelFormatTable() { Title = "ShowProgram", PropertyName = "ShowProgramN", IsVisible = false },
new ExcelFormatTable() { Title = "SR", PropertyName = "SalesRoom", Order = 2 },
new ExcelFormatTable() { Title = "LS", PropertyName = "LeadSource", Order  = 3 },
new ExcelFormatTable() { Title = "Sequency", PropertyName = "Sequency", IsVisible = false },
new ExcelFormatTable() { Title = "Hotel", PropertyName = "Hotel", Order = 9 },
new ExcelFormatTable() { Title = "Room", PropertyName = "Room", Order = 10 },
new ExcelFormatTable() { Title = "Pax", PropertyName = "Pax", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 13 },
new ExcelFormatTable() { Title = "Last Name", PropertyName = "LastName", Order = 5 },
new ExcelFormatTable() { Title = "First Name", PropertyName = "FirstName", Order = 6 },
new ExcelFormatTable() { Title = "Agency ID", PropertyName = "Agency", Order = 11 },
new ExcelFormatTable() { Title = "Agency", PropertyName = "AgencyN", Order = 12 },
new ExcelFormatTable() { Title = "Show D", PropertyName = "ShowD", IsVisible = false },
new ExcelFormatTable() { Title = "Country ID", PropertyName = "Country", Order = 7 },
new ExcelFormatTable() { Title = "Country", PropertyName = "CountryD", Order = 8 },
new ExcelFormatTable() { Title = "T In", PropertyName = "TimeInT", Format = EnumFormatTypeExcel.Time, Order = 14 },
new ExcelFormatTable() { Title = "T Out", PropertyName = "TimeOutT", Format = EnumFormatTypeExcel.Time, Order = 15 },
new ExcelFormatTable() { Title = "Sh", PropertyName = "Show", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 17 },
new ExcelFormatTable() { Title = "Tr", PropertyName = "Tour", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 18 },
new ExcelFormatTable() { Title = "IO", PropertyName = "IO", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 19 },
new ExcelFormatTable() { Title = "WO", PropertyName = "WO", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 20 },
new ExcelFormatTable() { Title = "CT", PropertyName = "CTour", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 21 },
new ExcelFormatTable() { Title = "Sve", PropertyName = "STour", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 22 },
new ExcelFormatTable() { Title = "D", PropertyName = "Direct", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 23 },
new ExcelFormatTable() { Title = "R", PropertyName = "Resch", Format = EnumFormatTypeExcel.Boolean, Function = DataFieldFunctions.Count, SubTotalFunctions = eSubTotalFunctions.Count, Order = 24 },
new ExcelFormatTable() { Title = "PR 1", PropertyName = "PR1", Order = 25 },
new ExcelFormatTable() { Title = "PR 1 Name", PropertyName = "PR1N", Order = 26 },
new ExcelFormatTable() { Title = "PR 2", PropertyName = "PR2" , Order = 27 },
new ExcelFormatTable() { Title = "PR 2 Name", PropertyName = "PR2N", Order = 28 },
new ExcelFormatTable() { Title = "PR 3", PropertyName = "PR3", Order = 29 },
new ExcelFormatTable() { Title = "PR 3 Name", PropertyName = "PR3N", Order = 30 },
new ExcelFormatTable() { Title = "Liner 1", PropertyName = "Liner1", Order = 31 },
new ExcelFormatTable() { Title = "Liner 1 Name", PropertyName = "Liner1N", Order = 32 },
new ExcelFormatTable() { Title = "Liner 2", PropertyName = "Liner2", Order = 33 },
new ExcelFormatTable() { Title = "Liner 2 Name", PropertyName = "Liner2N", Order = 34 },
new ExcelFormatTable() { Title = "Closer 1", PropertyName = "Closer1", Order = 35 },
new ExcelFormatTable() { Title = "Closer 1 Name", PropertyName = "Closer1N", Order = 36 },
new ExcelFormatTable() { Title = "Closer 2", PropertyName = "Closer2", Order = 37 },
new ExcelFormatTable() { Title = "Closer 2 Name", PropertyName = "Closer2N", Order = 38 },
new ExcelFormatTable() { Title = "Closer 3", PropertyName = "Closer3", Order = 39 },
new ExcelFormatTable() { Title = "Closer 3 Name", PropertyName = "Closer3N", Order = 40 },
new ExcelFormatTable() { Title = "Exit 1", PropertyName = "Exit1", Order = 41 },
new ExcelFormatTable() { Title = "Exit 1 Name", PropertyName = "Exit1N", Order = 42 },
new ExcelFormatTable() { Title = "Exit 2", PropertyName = "Exit2", Order = 43 },
new ExcelFormatTable() { Title = "Exit 2 Name", PropertyName = "Exit2N", Order = 44 },
new ExcelFormatTable() { Title = "Memb #", PropertyName = "saMembershipNum", Order = 45 },
new ExcelFormatTable() { Title = "Host", PropertyName = "Hostess", Order = 16 },
new ExcelFormatTable() { Title = "Sale", PropertyName = "ProcSales",Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 46 },
new ExcelFormatTable() { Title = "Proc Orig",  PropertyName = "ProcOriginal", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 47 },
new ExcelFormatTable() { Title = "Proc New", PropertyName = "ProcNew", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 48 },
new ExcelFormatTable() { Title = "Proc Gross", PropertyName = "ProcGross",  Format = EnumFormatTypeExcel.DecimalNumber,Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 49 },
new ExcelFormatTable() { Title = "Pend Sales", PropertyName = "PendSales", Format = EnumFormatTypeExcel.Number, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 50 },
new ExcelFormatTable() { Title = "Pend Original", PropertyName = "PendOriginal", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 51 },
new ExcelFormatTable() { Title = "Pend New", PropertyName = "PendNew", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 52 },
new ExcelFormatTable() { Title = "Pend Gross", PropertyName = "PendGross", Format = EnumFormatTypeExcel.DecimalNumber, Function = DataFieldFunctions.Sum, SubTotalFunctions = eSubTotalFunctions.Sum, Order = 53 },
new ExcelFormatTable() { Title = "% DP", PropertyName = "saDownPaymentPercentage", Format = EnumFormatTypeExcel.Percent, Order = 54 },
new ExcelFormatTable() { Title = "% EDP", PropertyName = "saDownPaymentPaidPercentage", Format = EnumFormatTypeExcel.Percent, Order = 55 },
new ExcelFormatTable() { Title = "Deposit Sale", PropertyName = "DepositSale", Order = 56 },
new ExcelFormatTable() { Title = "Deposit Sale Num", PropertyName = "DepositSaleNum", Order = 57 },
new ExcelFormatTable() { Title = "CC", PropertyName = "CC", Order = 58 },
new ExcelFormatTable() { Title = "Comments", PropertyName = "Comments", Order = 59 },
new ExcelFormatTable() { Title = "saDownPayment", PropertyName = "saDownPayment", IsVisible = false },
new ExcelFormatTable() { Title = "saDownPaymentPaid", PropertyName = "saDownPaymentPaid", IsVisible = false }
      };
    }

    /// <summary>
    /// Formato para el reporte Guest Manifest Range de Host y Processor General
    /// </summary>
    /// <history>
    /// [edgrodriguez] 13/Jun/2016 Created
    /// </history>
    public static List<ExcelFormatTable> RptManifestRangeByLs_Bookings()
    {
      return new List<ExcelFormatTable>() {
new ExcelFormatTable() { Title = "guloInvit", PropertyName = "guloInvit", IsVisible = false },
new ExcelFormatTable() { Title = "LocationN", PropertyName = "LocationN", Axis = ePivotFieldAxis.Row, Order = 1 },
new ExcelFormatTable() { Title = "guBookT", PropertyName = "guBookT", Order = 1, Axis = ePivotFieldAxis.Column, Sort = eSortType.Ascending },
new ExcelFormatTable() {Title = "guBookTime",PropertyName= "guBookTime", Axis = ePivotFieldAxis.Values, Order = 1 },
new ExcelFormatTable() { Title = "Bookings", PropertyName = "Bookings", Format = EnumFormatTypeExcel.Number, Axis = ePivotFieldAxis.Values, Order = 2 }
      };
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

using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;
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
      lst.Add("ShowProgram", "ShowProgramN", isGroup: true, isVisible: false);
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
      lst.Add("$ DP", "saDownPayment", format: EnumFormatTypeExcel.Currency, function:DataFieldFunctions.Sum);
      lst.Add("% DP", "saDownPaymentPercentage", format: EnumFormatTypeExcel.Percent, isCalculated:true, formula: "IF([ProcGross]=0,IF([PendGross]=0,0,[saDownPayment]/[PendGross]),[saDownPayment]/[ProcGross])");
      lst.Add("$ EDP", "saDownPaymentPaid", format: EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum);
      lst.Add("% EDP", "saDownPaymentPaidPercentage", format: EnumFormatTypeExcel.Percent,isCalculated:true, formula: "IF([ProcGross]=0,IF([PendGross]=0,0,[saDownPaymentPaid]/[PendGross]),[saDownPaymentPaid]/[ProcGross])");
      lst.Add("Deposit Sale", "DepositSale", format: EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum);
      lst.Add("Deposit #", "DepositSaleNum");
      lst.Add("CC", "CC");
      lst.Add("Comments", "Comments");
      lst.Add("SaleType", "SaleType", isVisible: false);
      lst.Add("Location", "Location", isVisible: false);
      lst.Add("Sequency", "Sequency", isVisible: false);
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
    public static ExcelFormatItemsList RptPremanifest()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("SR", "srN", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left, isGroup: true, isVisible: false);
      lst.Add("LS", "loN", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left, isGroup: true, isVisible: false);
      lst.Add("GUID", "guID", format: EnumFormatTypeExcel.Id, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Hotel", "guHotel", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Room", "guRoomNum", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("LastName", "guLastName1", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Pax", "guPax", format: EnumFormatTypeExcel.DecimalNumber, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Check-In", "guCheckInD", format: EnumFormatTypeExcel.Date, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Check-Out", "guCheckOutD", format: EnumFormatTypeExcel.Date, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Country ID", "guco", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Country", "coN", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Agency ID", "guag", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Agency", "agN", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Invit D", "guInvitD", format: EnumFormatTypeExcel.Date, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Invit T", "guInvitT", format: EnumFormatTypeExcel.Time, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Book T", "guBookT", format: EnumFormatTypeExcel.Time, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR B", "guPRInvit1", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Clxd", "guBookCanc", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Rsc", "guResch", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Sh", "guShow", format: EnumFormatTypeExcel.Boolean, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Sale", "guSale", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("InvitType", "InvitType", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Deposits_Comments", "guComments", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);

      return lst;
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
    public static ExcelFormatItemsList RptPremanifestWithGifts()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("Sales Room", "srN", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left, isGroup: true, isVisible: false);
      lst.Add("Location", "loN", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left, isGroup: true, isVisible: false);
      lst.Add("GUID", "guID", format: EnumFormatTypeExcel.Id, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("LS", "guls", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Room", "guRoomNum", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Last Name", "guLastName1", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("County ID", "guco", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("County", "coN", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Hotel", "guHotel", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Pax", "guPax", format: EnumFormatTypeExcel.DecimalNumber, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Invit D", "guInvitD", format: EnumFormatTypeExcel.Date, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Invit T", "guInvitT", format: EnumFormatTypeExcel.Time, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Resch", "guReschDT", format: EnumFormatTypeExcel.Date, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("CheckIn", "guCheckInD", format: EnumFormatTypeExcel.Date, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("CheckOut", "guCheckOutD", format: EnumFormatTypeExcel.Date, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Agency ID", "guag", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Agency", "agN", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Comments", "guComments", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Book T", "guBookT", format: EnumFormatTypeExcel.Time, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR", "guPRInvit1", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("G.T./CC", "guO2", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("CCType", "guCCType", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Sh", "guShow", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Sa", "guSale", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Canc Book", "guBookCanc", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Gifts", "Gifts", format: EnumFormatTypeExcel.General, aligment: ExcelHorizontalAlignment.Left);
      return lst;
    }
    #endregion

    #region RptGuestLog
    /// <summary>
    /// Formato para el reporte de Guest Log
    /// </summary>
    /// <history>
    /// [jorcanche]  created 07072016 
    /// </history>    
    public static ExcelFormatItemsList RptGuestLog()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("By", "glChangedBy", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Name", "ChangedByN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Update Date/Time", "glID", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.DateTime);
      lst.Add("SR", "glsr", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Last Name", "glLastName1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("First Name", "glFirstName1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Last Name 2", "glLastName2", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("First Name 2", "glFirstName2", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Reserv. #", "glHReservID", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Avl Sys", "glAvailBySystem", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Orig Avl", "glOriginAvail", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Avl", "glAvail", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Unavailable Motive", "umN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR Avail", "glPRAvail", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR Avail Name", "PRAvailN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Info D", "glInfoD", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Date);
      lst.Add("PR Info", "glPRInfo", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR Info Name", "PRInfoN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Follow D", "glFollowD", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Date);
      lst.Add("PR Follow", "glPRFollow", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR Follow Name", "PRFollowN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Book D", "glBookD", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Date);
      lst.Add("Book T", "glBookT", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Time);
      lst.Add("Resch D", "glReschD", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Date);
      lst.Add("Resch T", "glReschT", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Time);
      lst.Add("PR B", "glPRInvit1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR Booking Name", "PRInvit1N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR B 2", "glPRInvit2", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("PR Booking 2 Name", "PRInvit2N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("C.Bk", "glBookCanc", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Liner 1", "glLiner1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Liner 1 Name", "Liner1N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Liner 2", "glLiner2", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Liner 2 Name", "Liner2N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer 1", "glCloser1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer 1 Name", "Closer1N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer 2", "glCloser2", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer 2 Name", "Closer2N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer 3'", "glCloser3", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Closer 3 Name", "Closer3N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Exit1", "glExit1", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Exit 1 Name", "Exit1N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Exit2", "glExit2", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Exit 2 Name", "Exit2N", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Podium", "glPodium", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Podium Name", "PodiumN", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("VLO", "glVLO", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("VLO Name", "VLON", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Sh", "glShow", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Show D", "glShowD", aligment: ExcelHorizontalAlignment.Left, format: EnumFormatTypeExcel.Date);
      lst.Add("Q", "glQ", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("IO", "glInOut", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("WO", "glWalkOut", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("CT", "glCTour", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Re-Printed", "glReimpresion", aligment: ExcelHorizontalAlignment.Left);
      lst.Add("Re-Print Motive", "rmN", aligment: ExcelHorizontalAlignment.Left);
      return lst;
    }
    #endregion

    #region RptDepositByPr

    /// <summary>
    /// Formato para el reporte Deposit By PR
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// [jorcanche] 19/ago/2016 modified se cambio de lugar de IM.ProcessorGeneral.Classes a IM.Base.Reports
    /// [edgrodriguez] 05/Sep/2016 Modified. Se cambio el formato.
    /// </history>
    public static ExcelFormatItemsList RptDepositByPr()
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lst.Add("PR", "guPRInvit1", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("GUID", "guID", axis: ePivotFieldAxis.Row);
      lst.Add("Out Invit", "guOutInvitNum", axis: ePivotFieldAxis.Row);
      lst.Add("Guest", "Guest", axis: ePivotFieldAxis.Row);
      lst.Add("Hotel", "guHotel", axis: ePivotFieldAxis.Row);
      lst.Add("LS", "guls", axis: ePivotFieldAxis.Row);
      lst.Add("SR", "gusr", axis: ePivotFieldAxis.Row);
      lst.Add("Book Date", "guBookD", format: EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Deposited", "guDeposit", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("Received", "guDepositReceived", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("Burned", "guDepositTwisted", format: EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("Currency", "gucu", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Payment Type", "gupt", axis: ePivotFieldAxis.Column, sort: eSortType.Descending);
      lst.Add("guInvitD", "guInvitD", isVisible: false);
      lst.Add("PR Name", "peN", isVisible: false);
      return lst;
    }

    #endregion RptDepositByPr


  }
}

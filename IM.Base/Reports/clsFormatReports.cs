using IM.Model.Enums;
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
    /// <param name="module">Modulo desde donde se genera el reporte</param>
    /// <history>
    /// [edgrodriguez] 13/Jun/2016 Created
    /// [wtorres]      30/Sep/2016 Modified. Agregue el parametro module
    /// </history>
    public static ColumnFormatList RptManifestRangeByLs(EnumModule module)
    {
      //Hay columnas que no se deben desplegar en el manifiesto del modulo Host para ahorrar espacio
      bool isVisible = (module == EnumModule.ProcessorGeneral);

      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Date", "DateManifest", EnumFormatTypeExcel.Date, isVisible: isVisible);
      lst.Add("Group", "SaleTypeN", isGroup: true, isVisible: isVisible);
      lst.Add("LocationN", "LocationN", isGroup: true, isVisible: false);
      lst.Add("ShowProgram", "ShowProgramN", isGroup: true, isVisible: false);
      lst.Add("SR", "SalesRoom", isVisible: isVisible);
      lst.Add("LS", "LeadSource", isVisible: isVisible);
      lst.Add("GUID", "guID", width: 7);
      lst.Add("Last Name", "LastName", width: 12, wordWrap: true);
      lst.Add("First Name", "FirstName", width: 12, wordWrap: true);
      lst.Add("Country", "CountryD", width: 18, fontSize: 6, wordWrap: true);
      lst.Add("Show D", "ShowD", EnumFormatTypeExcel.Date, isVisible: isVisible);
      lst.Add("Hotel", "Hotel", width: 11, fontSize: 6, wordWrap: true);
      lst.Add("Room", "Room", width: 8);
      lst.Add("Agency", "AgencyN", fontSize: 6, wordWrap: true);
      lst.Add("Pax", "Pax", EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum, width: 7);
      lst.Add("In", "TimeInT", EnumFormatTypeExcel.Time, width: 8);
      lst.Add("Out", "TimeOutT", EnumFormatTypeExcel.Time, width: 8);
      lst.Add("Host", "Hostess", width: 6);
      lst.Add("Sh", "Show", EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count, width: 4);
      lst.Add("Tour", "Tour", EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count, width: 6);
      lst.Add("IO", "IO", EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count, width: 4);
      lst.Add("WO", "WO", EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count, width: 4);
      lst.Add("CT", "CTour", EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count, width: 4);
      lst.Add("Sv", "STour", EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count, width: 4);
      lst.Add("D", "Direct", EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count, width: 4);
      lst.Add("R", "Resch", EnumFormatTypeExcel.Boolean, function: DataFieldFunctions.Count, width: 4);
      lst.Add("N°", "PR1", width: 8);
      lst.Add("PR/SG", "PR1N", fontSize: 6, wordWrap: true);
      lst.Add("N°", "Liner1", width: 8);
      lst.Add("Sales R.", "Liner1N", fontSize: 6, wordWrap: true);
      lst.Add("N°", "Closer1", width: 8);
      lst.Add("FM1", "Closer1N", fontSize: 6, wordWrap: true);
      lst.Add("N°", "Closer2", width: 8);
      lst.Add("FM2", "Closer2N", fontSize: 6, wordWrap: true);
      lst.Add("N°", "Exit1", width: 8);
      lst.Add("JRFM", "Exit1N", fontSize: 6, wordWrap: true);
      lst.Add("Aff", "saMembershipNum", width: 7);
      lst.Add("Sale", "ProcSales", EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, width: 6);
      lst.Add("Proc Orig", "ProcOriginal", EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, width: 13);
      lst.Add("Proc New", "ProcNew", EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, width: 13);
      lst.Add("Proc Gross", "ProcGross", EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, width: 13);
      lst.Add("Sale", "PendSales", EnumFormatTypeExcel.Number, function: DataFieldFunctions.Sum, width: 6);
      lst.Add("Pend Original", "PendOriginal", EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, width: 13);
      lst.Add("Pend New", "PendNew", EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, width: 13);
      lst.Add("Pend Gross", "PendGross", EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, width: 13);
      lst.Add("$ DP", "saDownPayment", EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, width: 13);
      lst.Add("% DP", "saDownPaymentPercentage", EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([ProcGross]=0,IF([PendGross]=0,0,[saDownPayment]/[PendGross]),[saDownPayment]/[ProcGross])", width: 6);
      lst.Add("$ EDP", "saDownPaymentPaid", EnumFormatTypeExcel.Currency, function: DataFieldFunctions.Sum, width: 13);
      lst.Add("% EDP", "saDownPaymentPaidPercentage", EnumFormatTypeExcel.Percent, isCalculated: true, formula: "IF([ProcGross]=0,IF([PendGross]=0,0,[saDownPaymentPaid]/[PendGross]),[saDownPaymentPaid]/[ProcGross])", width: 7);
      lst.Add("Deposit Sale", "DepositSale", EnumFormatTypeExcel.DecimalNumber, function: DataFieldFunctions.Sum, width: 13);
      lst.Add("Deposit #", "DepositSaleNum", width: 10);
      lst.Add("CC", "CC", width: 5, fontSize: 6, wordWrap: true);
      lst.Add("Comments", "Comments", width:15, fontSize: 6, wordWrap: true);
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
    public static ColumnFormatList RptManifestRangeByLs_Bookings()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("guloInvit", "guloInvit", isVisible: false);
      lst.Add("LocationN", "LocationN", axis: ePivotFieldAxis.Row);
      lst.Add("guBookT", "guBookT", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("guBookTime", "guBookTime", axis: ePivotFieldAxis.Values);
      lst.Add("Bookings", "Bookings", EnumFormatTypeExcel.Number, axis: ePivotFieldAxis.Values);
      return lst;
    }

    #endregion RptManifestRangeByLs

    #region RptPremanifest
    /// <summary>
    /// Formato para el reporte Premanifest de Hosts e Inhouse
    /// </summary>
    /// <returns> List of ColumnFormat</returns>
    /// <history>
    /// [edgrodriguez] 21/Jun/2016 Created
    /// </history>
    public static ColumnFormatList RptPremanifest()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("SR", "srN", EnumFormatTypeExcel.General, isGroup: true, isVisible: false);
      lst.Add("LS", "loN", EnumFormatTypeExcel.General, isGroup: true, isVisible: false);
      lst.Add("GUID", "guID", EnumFormatTypeExcel.Id);
      lst.Add("Hotel", "guHotel", EnumFormatTypeExcel.General);
      lst.Add("Room", "guRoomNum", EnumFormatTypeExcel.General);
      lst.Add("LastName", "guLastName1", EnumFormatTypeExcel.General);
      lst.Add("Pax", "guPax", EnumFormatTypeExcel.DecimalNumber);
      lst.Add("Check-In", "guCheckInD", EnumFormatTypeExcel.Date);
      lst.Add("Check-Out", "guCheckOutD", EnumFormatTypeExcel.Date);
      lst.Add("Country ID", "guco", EnumFormatTypeExcel.General);
      lst.Add("Country", "coN", EnumFormatTypeExcel.General);
      lst.Add("Agency ID", "guag", EnumFormatTypeExcel.General);
      lst.Add("Agency", "agN", EnumFormatTypeExcel.General);
      lst.Add("Invit D", "guInvitD", EnumFormatTypeExcel.Date);
      lst.Add("Invit T", "guInvitT", EnumFormatTypeExcel.Time);
      lst.Add("Book T", "guBookT", EnumFormatTypeExcel.Time);
      lst.Add("PR B", "guPRInvit1", EnumFormatTypeExcel.General);
      lst.Add("Clxd", "guBookCanc", EnumFormatTypeExcel.Boolean);
      lst.Add("Rsc", "guResch", EnumFormatTypeExcel.Boolean);
      lst.Add("Sh", "guShow", EnumFormatTypeExcel.Boolean);
      lst.Add("Sale", "guSale", EnumFormatTypeExcel.General);
      lst.Add("InvitType", "InvitType", EnumFormatTypeExcel.General);
      lst.Add("Deposits_Comments", "guComments", EnumFormatTypeExcel.General);

      return lst;
    }
    #endregion

    #region RptPremanifestWithGifts
    /// <summary>
    /// Formato para el reporte Premanifest With Gifts de Host e Inhouse
    /// </summary>
    /// <returns> List of ColumnFormat</returns>
    /// <history>
    /// [edgrodriguez] 21/Jun/2016 Created
    /// </history>
    public static ColumnFormatList RptPremanifestWithGifts()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("Sales Room", "srN", EnumFormatTypeExcel.General, isGroup: true, isVisible: false);
      lst.Add("Location", "loN", EnumFormatTypeExcel.General, isGroup: true, isVisible: false);
      lst.Add("GUID", "guID", EnumFormatTypeExcel.Id);
      lst.Add("LS", "guls", EnumFormatTypeExcel.General);
      lst.Add("Room", "guRoomNum", EnumFormatTypeExcel.General);
      lst.Add("Last Name", "guLastName1", EnumFormatTypeExcel.General);
      lst.Add("County ID", "guco", EnumFormatTypeExcel.General);
      lst.Add("County", "coN", EnumFormatTypeExcel.General);
      lst.Add("Hotel", "guHotel", EnumFormatTypeExcel.General);
      lst.Add("Pax", "guPax", EnumFormatTypeExcel.DecimalNumber);
      lst.Add("Invit D", "guInvitD", EnumFormatTypeExcel.Date);
      lst.Add("Invit T", "guInvitT", EnumFormatTypeExcel.Time);
      lst.Add("Resch", "guReschDT", EnumFormatTypeExcel.Date);
      lst.Add("CheckIn", "guCheckInD", EnumFormatTypeExcel.Date);
      lst.Add("CheckOut", "guCheckOutD", EnumFormatTypeExcel.Date);
      lst.Add("Agency ID", "guag", EnumFormatTypeExcel.General);
      lst.Add("Agency", "agN", EnumFormatTypeExcel.General);
      lst.Add("Comments", "guComments", EnumFormatTypeExcel.General);
      lst.Add("Book T", "guBookT", EnumFormatTypeExcel.Time);
      lst.Add("PR", "guPRInvit1", EnumFormatTypeExcel.General);
      lst.Add("G.T./CC", "guO2", EnumFormatTypeExcel.General);
      lst.Add("CCType", "guCCType", EnumFormatTypeExcel.General);
      lst.Add("Sh", "guShow", EnumFormatTypeExcel.General);
      lst.Add("Sa", "guSale", EnumFormatTypeExcel.General);
      lst.Add("Canc Book", "guBookCanc", EnumFormatTypeExcel.General);
      lst.Add("Gifts", "Gifts", EnumFormatTypeExcel.General);
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
    public static ColumnFormatList RptGuestLog()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("By", "glChangedBy");
      lst.Add("Name", "ChangedByN");
      lst.Add("Update Date/Time", "glID", EnumFormatTypeExcel.DateTime);
      lst.Add("SR", "glsr");
      lst.Add("Last Name", "glLastName1");
      lst.Add("First Name", "glFirstName1");
      lst.Add("Last Name 2", "glLastName2");
      lst.Add("First Name 2", "glFirstName2");
      lst.Add("Reserv. #", "glHReservID");
      lst.Add("Avl Sys", "glAvailBySystem");
      lst.Add("Orig Avl", "glOriginAvail");
      lst.Add("Avl", "glAvail");
      lst.Add("Unavailable Motive", "umN");
      lst.Add("PR Avail", "glPRAvail");
      lst.Add("PR Avail Name", "PRAvailN");
      lst.Add("Info D", "glInfoD", EnumFormatTypeExcel.Date);
      lst.Add("PR Info", "glPRInfo");
      lst.Add("PR Info Name", "PRInfoN");
      lst.Add("Follow D", "glFollowD", EnumFormatTypeExcel.Date);
      lst.Add("PR Follow", "glPRFollow");
      lst.Add("PR Follow Name", "PRFollowN");
      lst.Add("Book D", "glBookD", EnumFormatTypeExcel.Date);
      lst.Add("Book T", "glBookT", EnumFormatTypeExcel.Time);
      lst.Add("Resch D", "glReschD", EnumFormatTypeExcel.Date);
      lst.Add("Resch T", "glReschT", EnumFormatTypeExcel.Time);
      lst.Add("PR B", "glPRInvit1");
      lst.Add("PR Booking Name", "PRInvit1N");
      lst.Add("PR B 2", "glPRInvit2");
      lst.Add("PR Booking 2 Name", "PRInvit2N");
      lst.Add("C.Bk", "glBookCanc");
      lst.Add("Liner 1", "glLiner1");
      lst.Add("Liner 1 Name", "Liner1N");
      lst.Add("Liner 2", "glLiner2");
      lst.Add("Liner 2 Name", "Liner2N");
      lst.Add("Closer 1", "glCloser1");
      lst.Add("Closer 1 Name", "Closer1N");
      lst.Add("Closer 2", "glCloser2");
      lst.Add("Closer 2 Name", "Closer2N");
      lst.Add("Closer 3'", "glCloser3");
      lst.Add("Closer 3 Name", "Closer3N");
      lst.Add("Exit1", "glExit1");
      lst.Add("Exit 1 Name", "Exit1N");
      lst.Add("Exit2", "glExit2");
      lst.Add("Exit 2 Name", "Exit2N");
      lst.Add("Podium", "glPodium");
      lst.Add("Podium Name", "PodiumN");
      lst.Add("VLO", "glVLO");
      lst.Add("VLO Name", "VLON");
      lst.Add("Sh", "glShow");
      lst.Add("Show D", "glShowD", EnumFormatTypeExcel.Date);
      lst.Add("Q", "glQ");
      lst.Add("IO", "glInOut");
      lst.Add("WO", "glWalkOut");
      lst.Add("CT", "glCTour");
      lst.Add("Re-Printed", "glReimpresion");
      lst.Add("Re-Print Motive", "rmN");
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
    public static ColumnFormatList RptDepositByPr()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("PR", "guPRInvit1", axis: ePivotFieldAxis.Row, isGroup: true, isVisible: false);
      lst.Add("GUID", "guID", axis: ePivotFieldAxis.Row);
      lst.Add("Out Invit", "guOutInvitNum", axis: ePivotFieldAxis.Row);
      lst.Add("Guest", "Guest", axis: ePivotFieldAxis.Row);
      lst.Add("Hotel", "guHotel", axis: ePivotFieldAxis.Row);
      lst.Add("LS", "guls", axis: ePivotFieldAxis.Row);
      lst.Add("SR", "gusr", axis: ePivotFieldAxis.Row);
      lst.Add("Book Date", "guBookD", EnumFormatTypeExcel.Date, axis: ePivotFieldAxis.Row);
      lst.Add("Deposited", "guDeposit", EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("Received", "guDepositReceived", EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, function: DataFieldFunctions.Sum);
      lst.Add("CxC", "CxC", EnumFormatTypeExcel.Currency, axis: ePivotFieldAxis.Values, isCalculated: true, formula: "[guDeposit]-[guDepositReceived]");
      lst.Add("Currency", "gucu", axis: ePivotFieldAxis.Column, sort: eSortType.Ascending);
      lst.Add("Payment Type", "gupt", axis: ePivotFieldAxis.Column, sort: eSortType.Descending);
      lst.Add("guInvitD", "guInvitD", isVisible: false);
      lst.Add("PR Name", "peN", isVisible: false);
      return lst;
    }

    #endregion RptDepositByPr

    #region RptGuestMovements
    /// <summary>
    /// Formato para el reporte de Guest Movements
    /// </summary>
    /// <history>
    /// [jorcanche]  created 07072016 
    /// </history>    
    public static ColumnFormatList RptGuestMovements()
    {
      ColumnFormatList lst = new ColumnFormatList();
      lst.Add("By", "gmpe");
      lst.Add("Name", "peN" );
      lst.Add("Date/Time", "gmDT");
      lst.Add("Movement", "gnN");
      lst.Add("Computer", "gmcp");
      lst.Add("Computer name", "cpN");
      lst.Add("IP Address", "gmIpAddress");   
      return lst;
    }
    #endregion
  }
}

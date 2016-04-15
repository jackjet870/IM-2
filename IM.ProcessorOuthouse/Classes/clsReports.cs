using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using System.IO;
using IM.Base.Helpers;
using System.Text.RegularExpressions;
using System.Data;

namespace IM.ProcessorOuthouse.Classes
{
  public class clsReports
  {
    #region ExportRptDepositsPaymentTypes
    /// <summary>
    ///  Obtiene los datos para exportar a excel el reporte DepositsPaymentTypesByPR
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptDepositsPaymentByPR">Lista de los depositos y pagos por PR</param>
    /// <history>
    ///   [vku] 07/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptDepositsPaymentByPR(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptDepositsPaymentByPR)
    {
      var lstDepPyPR = lstRptDepositsPaymentByPR[0] as List<RptDepositsPaymentByPR>;
      var lstGuests = lstRptDepositsPaymentByPR[1] as List<Guest>;
      var lstBookDep = lstRptDepositsPaymentByPR[3] as List<BookingDeposit>; 
      var depPayPRwhitGuestBookDep = (from deppyPR in lstDepPyPR
                               join gu in lstGuests on deppyPR.PR equals gu.guPRInvit1
                               join bookDep in lstBookDep on gu.guID equals bookDep.bdgu
                               select new
                               {
                                 deppyPR.PR,
                                 deppyPR.PRN,
                                 deppyPR.Books,
                                 deppyPR.InOuts,
                                 deppyPR.GrossBooks,
                                 deppyPR.ShowsFactor,
                                 deppyPR.GrossShows,
                                 gu.guID,
                                 guName = string.Format("{0}, {1}", gu.guLastName1, gu.guFirstName1),
                                 gu.guBookD,
                                 gu.guOutInvitNum,
                                 gu.guls,
                                 gu.gusr,
                                 gu.guHotel,
                                 bookDep.bdpc,
                                 deppyPR.SalesAmount,
                                 deppyPR.Efficiency,
                                 bookDep.bdAmount,
                                 bookDep.bdReceived,          
                              })
                              .ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptDepositsPaymentByPR);
      return EpplusHelper.CreateGeneralRptExcel(filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptDepositsPaymentByPR());
    }
    #endregion

    #region ExportRptGiftsReceivedBySR
   public static FileInfo ExportRptGiftsReceivedBySR(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptGiftsReceivedBySR)
     {

      var lstGiftsReceivedBySR = lstRptGiftsReceivedBySR[0] as List<RptGiftsReceivedBySR>;
      var curriencies = lstRptGiftsReceivedBySR[1] as List<Currency>;

      var lstGifRecBySRWithCu = (from giftRecBySR in lstGiftsReceivedBySR
                                 join cu in curriencies on giftRecBySR.Currency equals cu.cuID
                                 select new
                                 {
                                   giftRecBySR.SalesRoom,
                                   giftRecBySR.Gift,
                                   giftRecBySR.GiftN,
                                   giftRecBySR.Quantity,
                                   giftRecBySR.Couples,
                                   giftRecBySR.Adults,
                                   giftRecBySR.Minors,
                                   cu.cuN,
                                   giftRecBySR.Amount,
                                 }

                                 ).ToList(); 

      DataTable dtData = TableHelper.GetDataTableFromList(lstGifRecBySRWithCu);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptGiftsRecivedBySR(), showRowGrandTotal: true);
    }
     #endregion

    #region ExportRptGuestsShowNoPresentedInvitation
    /// <summary>
    ///  Obtiene los datos para exportar a excel el reporte GuestsShowNoPresentedInvitation
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptGuestsShowNoPresentedInvitation">Lista de los huespedes que no presentaron invitacion</param>
    /// <history>
    ///   [vku] 07/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGuestsShowNoPresentedInvitation(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<GuestShowNoPresentedInvitation> lstRptGuestsShowNoPresentedInvitation)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptGuestsShowNoPresentedInvitation);
      return EpplusHelper.CreateGeneralRptExcel(filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptGuestsShowNoPresentedInvitation());
    }
    #endregion

    #region ExportRptProductionByAge
    public static FileInfo ExportRptProductionByAge(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionByAgeOuthouse> lstRptProductionByAgeOuthouse)
    {        
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByAgeOuthouse);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptProductionByAge(), showRowGrandTotal: true);
    }
    #endregion
  }
}

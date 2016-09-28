using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace IM.Outhouse.Classes
{
  public class ReportsToExcel
  {
    #region Atributos

    private static List<Tuple<string, string>> _filters;   
    private static string _rptName;

    #endregion

    public async static void PremanifestToExcel(List<RptPremanifestOuthouse> lstpremanifest,Window window)
    {

      _filters = new List<Tuple<string, string>> {Tuple.Create("Lead Source", Context.User.LeadSource.lsID)};
      var date = BRHelpers.GetServerDate();
      if (lstpremanifest.Count > 0)
      {
        var premanifestAux = lstpremanifest.Select(c => new
        {
          c.srN,
          c.guID,
          c.guOutInvitNum,
          c.Deposit,
          c.guHotel,
          c.guRoomNum,
          c.guLastName1,
          c.guFirstName1,
          c.guco,
          c.coN,
          c.guBookD,
          c.guBookT,
          c.guPRInvit1,
          guShow = c.guShow ? "✓" : "",
          guSale = c.guSale ? "✓" : "",
          c.guComments,
        }).ToList();

        var dt = TableHelper.GetDataTableFromList(premanifestAux, true);
        _rptName = "Premanifest  Outhouse " + Context.User.LeadSource.lsN ;        
        var dateRange = DateHelper.DateRangeFileName(date, date);
        var format = new ExcelFormatItemsList();

        format.Add("SR", "srN", isGroup: true, isVisible: false);
        format.Add("Deposit", "Deposit", isGroup: true, isVisible: false);
        format.Add("GUID", "guID");
        format.Add("Out Invit", "guOutInvitNum");
        format.Add("Hotel", "guHotel");
        format.Add("Room", "guRoomNum");
        format.Add("Last Name", "guLastName1");
        format.Add("Firs tName", "guFirstName1");
        format.Add("Country ID", "guco");
        format.Add("Country", "coN");
        format.Add("Book D", "guBookD", format: EnumFormatTypeExcel.Date);
        format.Add("Book T", "guBookT", format: EnumFormatTypeExcel.Time);
        format.Add("PR B", "guPRInvit1");
        format.Add("Sh", "guShow", axis: ePivotFieldAxis.Values, aligment: ExcelHorizontalAlignment.Center, function: DataFieldFunctions.Count);
        format.Add("Sale", "guSale", axis: ePivotFieldAxis.Values, aligment: ExcelHorizontalAlignment.Center, function: DataFieldFunctions.Count);
        format.Add("Deposits / Comments", "guComments");

        var info = await ReportBuilder.CreateCustomExcel(dt, _filters, _rptName, dateRange, format, blnShowSubtotal: true, blnRowGrandTotal: true, addEnumeration: true);
        if (info != null)
        {
          frmDocumentViewer documentViewer = new frmDocumentViewer(info, Context.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly), false);
          documentViewer.Owner = window;
          documentViewer.ShowDialog();
        }
      }
    }
  }
}

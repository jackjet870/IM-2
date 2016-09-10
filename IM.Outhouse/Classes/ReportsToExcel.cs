using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Linq;
using IM.Base.Forms;

namespace IM.Outhouse.Classes
{
  public class ReportsToExcel
  {
    #region Atributos

    private static List<Tuple<string, string>> _filters;   
    private static string _rptName;

    #endregion

    public static void PremanifestToExcel(List<RptPremanifestOuthouse> lstpremanifest)
    {

      _filters = new List<Tuple<string, string>> {Tuple.Create("Lead Source", App.User.LeadSource.lsID)};
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
          c.guBookT,c.guPRInvit1,
          guShow = c.guShow ? "✓" : "",
          guSale = c.guSale ? "✓" : "",
          c.guComments,      
        }).ToList();

        var dt = TableHelper.GetDataTableFromList(premanifestAux, true);
        _rptName = "Premanifest  Outhouse " + App.User.LeadSource.lsN ;        
        var dateRange = DateHelper.DateRangeFileName(date, date);
        var format = new List<ExcelFormatTable>();

        format.Add(new ExcelFormatTable { Title = "SR",            PropertyName = "SR",           Order = 0, Axis = ePivotFieldAxis.Row, SubTotalFunctions = eSubTotalFunctions.Default, Compact = true,  Outline = true,  Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left  });      
        format.Add(new ExcelFormatTable { Title = "GUID" ,         PropertyName = "GUID",          Order = 1, Axis = ePivotFieldAxis.Row });           
        format.Add(new ExcelFormatTable { Title = "Out Invit Num", PropertyName = "Out Invit Num", Order = 3, Axis = ePivotFieldAxis.Row });
        format.Add(new ExcelFormatTable { Title = "Deposit",       PropertyName = "Deposit",       Order = 0, Axis = ePivotFieldAxis.Row,  SubTotalFunctions = eSubTotalFunctions.Default, Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable { Title = "Hotel",         PropertyName = "Hotel",       Order = 4, Axis = ePivotFieldAxis.Row });
        format.Add(new ExcelFormatTable { Title = "RoomNum",       PropertyName = "RoomNum",     Order = 2, Axis = ePivotFieldAxis.Row });               
        format.Add(new ExcelFormatTable { Title = "LastName",      PropertyName = "LastName1",   Order = 5, Axis = ePivotFieldAxis.Row });
        format.Add(new ExcelFormatTable { Title = "FirstName",     PropertyName = "FirstName1",  Order = 6 , Axis = ePivotFieldAxis.Row });
        format.Add(new ExcelFormatTable { Title = "Country ID",    PropertyName = "Country ID",          Order = 7, Axis = ePivotFieldAxis.Row });
        format.Add(new ExcelFormatTable { Title = "Country",       PropertyName = "Country",           Order = 8 , Axis = ePivotFieldAxis.Row });
        format.Add(new ExcelFormatTable { Title = "Book D",        PropertyName = "Book D",       Order = 9, Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable { Title = "Book T",        PropertyName = "Book T",       Order = 10, Axis = ePivotFieldAxis.Row, Format = EnumFormatTypeExcel.Time, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable { Title = "PR B",          PropertyName = "PR B",    Order = 11, Axis = ePivotFieldAxis.Row });
        format.Add(new ExcelFormatTable { Title = "Sh",            PropertyName = "Sh",      Order = 12, Axis = ePivotFieldAxis.Values, Alignment = ExcelHorizontalAlignment.Center, Function = DataFieldFunctions.Count });
        format.Add(new ExcelFormatTable { Title = "Sale",          PropertyName = "Sale",        Order = 13, Axis = ePivotFieldAxis.Values, Alignment = ExcelHorizontalAlignment.Center, Function = DataFieldFunctions.Count });
        format.Add(new ExcelFormatTable { Title = "Deposits / Comments", PropertyName = "Deposits / Comments", Order = 14, Axis = ePivotFieldAxis.Row });     

       var info = EpplusHelper.CreatePivotRptExcel(false,_filters, dt, _rptName, dateRange, format,true);
        if (info != null)
        {
          frmDocumentViewer documentViewer = new frmDocumentViewer(info, App.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly), false);
          documentViewer.ShowDialog();
        }
      }
    }
  }
}

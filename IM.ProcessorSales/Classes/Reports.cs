using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Base.Helpers;
using IM.Model;

namespace IM.ProcessorSales.Classes
{
  public class Reports
  {
    #region Reports By Sales Room

    #region ExportRptStatisticsBySalesRoomLocation

    public static FileInfo RptStatisticsBySalesRoomLocation(string report, string dateRangeFileName,
      List<Tuple<string, string>> filters, List<RptStatisticsBySalesRoomLocation> lstReport)
    {
      var dtData = TableHelper.GetDataTableFromList(lstReport,true,false);
      return EpplusHelper.CreateExcelCustom(dtData, filters, report, dateRangeFileName, FormatReport.RptStatisticsBySalesRoomLocation(),true,true,true);
    }

    #endregion

    #endregion
  }
}

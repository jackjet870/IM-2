using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRReportsTexts
  {
    #region GetReportTexts
    /// <summary>
    /// Retorna los textos de un reporte en el idioma seleccionado.
    /// </summary>
    /// <param name="Report"> Nombre del reporte.</param>
    /// <param name="language"> Lenguaje. </param>
    /// <returns> List of ReportText </returns>
    ///  <history>
    /// [edgrodriguez] 14/Jul/2016 Created
    /// </history>
    public static async Task<List<ReportText>> GetReportTexts(string Report = "ALL", string language = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return (from rptxt in dbContext.ReportTexts
                  where (Report == "ALL" || rptxt.reReport == Report)
                         && (language == "ALL" || rptxt.rela == language)
                  orderby rptxt.reO
                  select rptxt).ToList();
        }
      });
    } 
    #endregion
  }
}

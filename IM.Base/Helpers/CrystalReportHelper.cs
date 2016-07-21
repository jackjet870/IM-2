using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportAppServer.Controllers;
using IM.Base.Forms;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IM.Base.Helpers
{
  public class CrystalReportHelper
  {
    #region SetLanguage
    /// <summary>
    /// Configura el lenguaje de las etiquetas del reporte.
    /// </summary>
    /// <param name="report"></param>
    /// <param name="Language"></param>
    /// <history>
    /// [edgrodriguez] 13/Jul/2016 Created
    /// </history>
    public static void SetLanguage(ReportDocument report, string Language = "")
    {

      //Determinamos el Lenguaje
      LanguageHelper.IDLanguage = Language;

      //Buscamos en el reporte principal
      report.ReportDefinition.ReportObjects
        .Cast<object>()
        .Where(c => c.GetType() == typeof(FieldHeadingObject) || c.GetType() == typeof(TextObject))
        .Select(c => c)
        .ToList()
        .ForEach(c =>
        {
          if (c.GetType() == typeof(FieldHeadingObject))
          {
            var msg = LanguageHelper.GetMessage(((FieldHeadingObject)c).Name);
            if (!string.IsNullOrEmpty(msg))
              ((FieldHeadingObject)c).Text = msg;
          }
          else if (c.GetType() == typeof(TextObject))
          {
            var msg = LanguageHelper.GetMessage(((TextObject)c).Name);
            if (!string.IsNullOrEmpty(msg))
              ((TextObject)c).Text = msg;
          }
        });
      //Buscamos en los subreportes.
      report.Subreports
        .OfType<ReportDocument>()
        .ToList().ForEach(rpt =>
        {
          rpt.ReportDefinition.ReportObjects
          .Cast<object>()
          .Where(c => c.GetType() == typeof(FieldHeadingObject) || c.GetType() == typeof(TextObject))
          .Select(c => c)
          .ToList()
          .ForEach(c =>
          {
            if (c.GetType() == typeof(FieldHeadingObject))
            {
              var msg = LanguageHelper.GetMessage(((FieldHeadingObject)c).Name);
              if (!string.IsNullOrEmpty(msg))
                ((FieldHeadingObject)c).Text = msg;
            }
            else if (c.GetType() == typeof(TextObject))
            {
              var msg = LanguageHelper.GetMessage(((TextObject)c).Name);
              if (!string.IsNullOrEmpty(msg))
                ((TextObject)c).Text = msg;
            }
          });
        });
    }
    #endregion

    #region ShowReport
    /// <summary>
    /// Muestra el reporte en un mediante el visor de Crystal Reports o
    /// manda a imprimir el reporte directamente a la impresora configurada.
    /// </summary>
    /// <param name="report"> Documento .rpt </param>
    /// <param name="reportName"> Nombre del reporte. </param>
    /// <param name="isDialog"> Se mostrará en una ventana tipo Dialogo. </param>
    /// <param name="owner"> Ventana Principal que utiliza el visor. </param>
    /// <param name="isDirectPrint"> Bandera de impresion directa. </param>
    /// <history>
    /// [edgrodriguez] 16/Jul/2016 Created
    /// </history>
    public static void ShowReport(ReportDocument report, string reportName = "", bool isDialog = false, Window owner = null, EnumPrintDevice PrintDevice = EnumPrintDevice.pdScreen)
    {
      switch (PrintDevice)
      {
        case EnumPrintDevice.pdPrinter:
          var boPrintReportOptions = new PrintReportOptions();
          boPrintReportOptions.PrinterName = ConfigRegistryHelper.GetConfiguredPrinter("PrintInvit");
          if (string.IsNullOrEmpty(boPrintReportOptions.PrinterName))
          {
            UIHelper.ShowMessage($"The printer is not configured, please configure your printer.");
            return;
          }
          var boReportClientDocument = report.ReportClientDocument;
          var boPrintOutputController = boReportClientDocument.PrintOutputController;

          boPrintReportOptions.JobTitle = reportName;
          boPrintOutputController.PrintReport(boPrintReportOptions);
          break;
        case EnumPrintDevice.pdScreen:
          var crViewer = new frmViewer(report, reportName);
          crViewer.Owner = owner;
          if (isDialog)
          {
            crViewer.ShowDialog();
            break;
          }
          crViewer.Show();
          break;
      }
    } 
    #endregion
  }
}

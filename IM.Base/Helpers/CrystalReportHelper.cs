using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportAppServer.Controllers;
using CrystalDecisions.Shared;
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
    /// [edgrodriguez] 29/Jul/2016 Modified. Se agregó el reconocimiento para los controles tipo Parámetro.
    /// </history>
    public static void SetLanguage(ReportDocument report, string Language = "")
    {
      //Determinamos el Lenguaje
      LanguageHelper.IDLanguage = Language;    
      //Buscamos en el reporte principal
      report.ReportDefinition.ReportObjects
        .Cast<object>()
        .Where(c => c.GetType() == typeof(FieldHeadingObject) || c.GetType() == typeof(TextObject) || c.GetType()==typeof(FieldObject))
        .Select(c => c)
        .ToList()
        .ForEach(c =>
        {
          //Etiquetas de Campo
          if (c.GetType() == typeof(FieldHeadingObject))
          {
            var msg = LanguageHelper.GetMessage(((FieldHeadingObject)c).Text);
            if (!string.IsNullOrEmpty(msg))
              ((FieldHeadingObject)c).Text = msg;
          }
          //Etiquetas normales.
          else if (c.GetType() == typeof(TextObject))
          {
            var msg = LanguageHelper.GetMessage(((TextObject)c).Text);
            if (!string.IsNullOrEmpty(msg))
              ((TextObject)c).Text = msg;
          }
          //Etiquetas de tipo Parametro.
          else if (c.GetType() == typeof(FieldObject))
          {
            var ctrl = ((FieldObject)c).DataSource as ParameterFieldDefinition;
            if (ctrl != null && ctrl.DefaultValues.ToArray().Any())
            {
              var msg = LanguageHelper.GetMessage(ctrl.DefaultValues.Cast<ParameterDiscreteValue>().FirstOrDefault().Value.ToString());
              if (!string.IsNullOrEmpty(msg))
                report.SetParameterValue(ctrl.Name, msg);
            }
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
              var msg = LanguageHelper.GetMessage(((FieldHeadingObject)c).Text);
              if (!string.IsNullOrEmpty(msg))
                ((FieldHeadingObject)c).Text = msg;
            }
            else if (c.GetType() == typeof(TextObject))
            {
              var msg = LanguageHelper.GetMessage(((TextObject)c).Text);
              if (!string.IsNullOrEmpty(msg))
                ((TextObject)c).Text = msg;
            }
            else if (c.GetType() == typeof(FieldObject))
            {
              var ctrl = ((FieldObject)c).DataSource as ParameterFieldDefinition;
              if (ctrl != null && ctrl.DefaultValues.ToArray().Any())
              {
                var msg = LanguageHelper.GetMessage(ctrl.DefaultValues.Cast<CrystalDecisions.Shared.ParameterDiscreteValue>().FirstOrDefault().Value.ToString());
                if (!string.IsNullOrEmpty(msg))
                  rpt.SetParameterValue(((FieldObject)c).Name, msg);
              }
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
    public static void ShowReport(ReportDocument report, string reportName = "", bool isDialog = false, Window owner = null, EnumPrintDevice PrintDevice = EnumPrintDevice.pdScreen, bool IsInvitation=false, int numCopies=1)
    {
      switch (PrintDevice)
      {
        case EnumPrintDevice.pdPrinter:
          var boPrintReportOptions = new PrintReportOptions();
          boPrintReportOptions.PrinterName = (IsInvitation) ? ConfigRegistryHelper.GetConfiguredPrinter("PrintInvit") : boPrintReportOptions.PrinterName;
          if (IsInvitation && string.IsNullOrEmpty(boPrintReportOptions.PrinterName))
          {
            UIHelper.ShowMessage($"The printer is not configured, please configure your printer.");
            return;
          }
          var boReportClientDocument = report.ReportClientDocument;
          var boPrintOutputController = boReportClientDocument.PrintOutputController;

          boPrintReportOptions.JobTitle = reportName;
          boPrintReportOptions.NumberOfCopies = numCopies;
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

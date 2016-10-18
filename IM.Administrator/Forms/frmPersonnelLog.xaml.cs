using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Base.Forms;
using IM.Base.Classes;
using IM.Model.Enums;
using System.IO;
using IM.Base.Reports;
using IM.Administrator.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPersonnelLog.xaml
  /// </summary>
  public partial class frmPersonnelLog : Window
  {
    private string _idPersonnelLog;
    public frmPersonnelLog(string idPersonnelLog)
    {
      InitializeComponent();
      _idPersonnelLog = idPersonnelLog;
    }

    #region Window_Loaded
    /// <summary>
    /// Carga el log del personal
    /// </summary>
    /// <history>
    /// [emoguel] 17/10/2016 created
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Title = $"{Title} - {_idPersonnelLog}";
      List<GetPersonnelLog> lstPersonnelLog = await BRPersonnelLog.GetPersonnelLog(_idPersonnelLog);
      dtgPersonnelLog.ItemsSource = lstPersonnelLog;
      if(lstPersonnelLog.Count>0)
      {
        btnPrintlog.IsEnabled = true;
      }
    }
    #endregion

    #region btnPrintlog_Click
    /// <summary>
    /// Muestra el Personnel Log en el DocumentViewer
    /// </summary>
    /// <history>
    /// [emoguel] 17/10/2016 created
    /// </history>
    private async void btnPrintlog_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Mouse.OverrideCursor = Cursors.Wait;
        List<GetPersonnelLog> lstPersonnelLog = dtgPersonnelLog.ItemsSource as List<GetPersonnelLog>;

        FileInfo fileInfo = await ReportBuilder.CreateCustomExcelAsync(
          TableHelper.GetDataTableFromList(lstPersonnelLog, true, true, true),
          new List<Tuple<string, string>> { Tuple.Create("Personnel ID", _idPersonnelLog) },
          "Personnel Log",
          DateHelper.DateRangeFileName(DateTime.Today, DateTime.Today),
          EpplusHelper.OrderColumns(dtgPersonnelLog.Columns.ToList(), clsFormatReport.RptPersonnelLog()),autoFit:false);
        if (fileInfo != null)
        {
          frmDocumentViewer documentViewver = new frmDocumentViewer(fileInfo, Context.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly), false);
          documentViewver.Owner = this;
          documentViewver.ShowDialog();
        }

      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
      finally
      {
        Mouse.OverrideCursor = null;
      }
    } 
    #endregion
  }
}

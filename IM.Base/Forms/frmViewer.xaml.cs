using CrystalDecisions.CrystalReports.Engine;
using System.Windows;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmViewer.xaml
  /// </summary>
  /// <history>
  /// [edgrodriguez] 16/Jul/2016 Modified. Se cambio el tipo de la variable _rpt.
  /// </history>
  public partial class frmViewer : Window
  {
    private ReportDocument _rpt;
    private string _reportName;
    #region Constructor
    #region frmViewer
    public frmViewer()
    {
      InitializeComponent();
    }



    public frmViewer(ReportDocument rpt, string reportName ="") : this()
    {
      _rpt = rpt;
      _reportName = reportName;
    }
    #endregion
    #endregion

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      reportViewer.Owner = GetWindow(this);
      if (!string.IsNullOrEmpty(_reportName))
        _rpt.SummaryInfo.ReportTitle = _reportName;
      reportViewer.ViewerCore.ReportSource = _rpt;
    } 
    #endregion
  }
}
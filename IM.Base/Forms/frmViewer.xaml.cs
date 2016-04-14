using CrystalDecisions.CrystalReports.Engine;
using System.Windows;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmViewer.xaml
  /// </summary>
  public partial class frmViewer : Window
  {
    private ReportClass _rpt;

    public frmViewer()
    {
      InitializeComponent();
    }

    public frmViewer(ReportClass rpt) : this()
    {
      this._rpt = rpt;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      reportViewer.Owner = Window.GetWindow(this);
      reportViewer.ViewerCore.ReportSource = _rpt;
    }
  }
}
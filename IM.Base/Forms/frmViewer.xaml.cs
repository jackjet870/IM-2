using System.Windows;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmViewer.xaml
  /// </summary>
  public partial class frmViewer : Window
  {
    private object _rpt;

    public frmViewer()
    {
      InitializeComponent();
    }

    public frmViewer(object _rpt) : this()
    {
      this._rpt = _rpt;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      crystalReportsViewer1.ViewerCore.ReportSource = _rpt;
    }
  }
}
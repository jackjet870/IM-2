using IM.BusinessRules.BR;
using IM.MailOuts.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using IM.MailOuts.Reports;

namespace IM.MailOuts.Forms
{
  /// <summary>
  /// Interaction logic for frmViewer.xaml
  /// </summary>
  public partial class frmViewer : Window
  {
    private rptMailOuts _rptMailOuts;

    public frmViewer()
    {
      InitializeComponent();

    }

    public frmViewer(rptMailOuts _rptMailOuts) : this()
    {
      this._rptMailOuts = _rptMailOuts;
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      crystalReportsViewer1.ViewerCore.ReportSource = _rptMailOuts;
    }
  }
}
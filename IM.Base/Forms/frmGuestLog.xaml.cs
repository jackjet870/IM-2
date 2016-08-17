using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using IM.BusinessRules.BR;
using IM.Base.Forms;
using System.Threading.Tasks;
using IM.Base.Helpers;
using IM.Base.Reports;
using IM.Model;

namespace IM.Base.Forms
{
  /// <summary>
  /// Formulario que visualiza el Guest Log
  /// </summary>
  /// <history>
  /// [jorcanche] 20/06/2016 created
  /// </history>
  public partial class frmGuestLog : Window
  {
    #region Atributos
    private readonly int _idGuest;
    #endregion

    #region Constructores y destructores
    public frmGuestLog(int idGuest)
    {
      InitializeComponent();
      _idGuest = idGuest;   
    }
    #endregion

    #region Eventos del formulario

    #region Window_Loaded
    /// <summary>
    /// Inicializa el datagrid 
    /// </summary>
    /// <history>
    /// [jorcanche] 20/06/2016 created
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      dgGuestLog.DataContext = await BRGuestsLogs.GetGuestLog(_idGuest);
      var leadSourceguest = await BRLeadSources.GetLeadSourceByGuestId(_idGuest);
      Title = $"IM guest Log - Guest ID {_idGuest} / Lead Source {leadSourceguest}";
    }
   
    #endregion

    #region btnGuestMovents_Click
    /// <summary>
    /// Invoca al formulario de Guest Movents
    /// </summary>
    /// <history>
    /// [jorcanche] 20/06/2016 created
    /// </history>
    private void btnGuestMovents_Click(object sender, RoutedEventArgs e)
    {
      var guestMovements = new frmGuestsMovements(_idGuest) {Owner = this};
      guestMovements.ShowDialog();
    }
    #endregion

    #endregion

    #region btnPrintSaleLog_Click
    /// <summary>
    /// Imprime el el Log del Guest
    /// </summary>
    /// <history>
    /// [jorcanche]  created 07/07/2016
    /// </history>
    private void btnPrintSaleLog_Click(object sender, RoutedEventArgs e)
    {
      if (dgGuestLog.ItemsSource == null) return;
        var lstFormat = clsFormatReports.RptGuestLog();
      EpplusHelper.OrderColumns(dgGuestLog.Columns.ToList(),lstFormat);
        EpplusHelper.CreateExcelCustom(
          TableHelper.GetDataTableFromList((List<GuestLogData>)dgGuestLog.ItemsSource, true, true, true),
          new List<Tuple<string, string>> { Tuple.Create("Guest Id", _idGuest.ToString()) },
          "Guest Log", 
          DateHelper.DateRangeFileName(DateTime.Today, DateTime.Today), 
          lstFormat);
    } 
	#endregion
  }
}

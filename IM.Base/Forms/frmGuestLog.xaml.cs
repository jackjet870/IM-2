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
using System.Windows.Input;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using System.IO;
using IM.Base.Classes;
using IM.Model.Enums;

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
      Mouse.OverrideCursor = Cursors.Wait;    
      dgGuestLog.DataContext = await BRGuestsLogs.GetGuestLog(_idGuest);
      var leadSourceguest = await BRLeadSources.GetLeadSourceByGuestId(_idGuest);
      Title = $"Guest Log - Guest ID {_idGuest} / Lead Source {leadSourceguest}";
      Mouse.OverrideCursor = null;
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

    #region btnSalesChanges_Click
    /// <summary>
    /// Abre la ventana de salesmenChanges
    /// </summary>
    /// <history>
    /// [emoguel] 12/10/2016 created
    /// </history>
    private void btnSalesChanges_Click(object sender, RoutedEventArgs e)
    {
      frmSalesmenChanges frmSalesmenChages = new frmSalesmenChanges(_idGuest, "", "SH");
      frmSalesmenChages.ShowDialog();
    }
    #endregion

    #region btnPrintGuestLog_Click
    /// <summary>
    /// Imprime el el Log del Guest
    /// </summary>
    /// <history>
    /// [jorcanche]  created 07/07/2016
    /// [edgrodriguez] 05/09/2016 Modified. Se cambio el método CreateExcelCustom por CreatCustomExcel
    /// </history>
    private async void btnPrintGuestLog_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (dgGuestLog == null) return;
        if (dgGuestLog.Items.Count == 0)
        {
          UIHelper.ShowMessage("There is no info to make a report");
          return;
        }
        Mouse.OverrideCursor = Cursors.Wait;
        FileInfo fileInfo = await ReportBuilder.CreateCustomExcelAsync(
          TableHelper.GetDataTableFromList((List<GuestLogData>)dgGuestLog.ItemsSource, true, true, true),
          new List<Tuple<string, string>> { Tuple.Create("Guest Id", _idGuest.ToString()) },
          "Guest Log",
          DateHelper.DateRangeFileName(DateTime.Today, DateTime.Today),
          EpplusHelper.OrderColumns(dgGuestLog.Columns.ToList(), clsFormatReports.RptGuestLog()));
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

    #endregion
  }
}

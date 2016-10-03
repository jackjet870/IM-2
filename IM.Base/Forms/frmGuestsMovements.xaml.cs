using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IM.Base.Forms
{
  /// <summary>
  /// Formulario que visualiza el Guests Movements
  /// </summary>
  /// <history>
  /// [jorcanche] 20/06/2016 created
  /// </history>
  public partial class frmGuestsMovements : Window
  {
    private int _guestID = 0;
    public frmGuestsMovements(int guestId)
    {
      InitializeComponent();
      _guestID = guestId;     
    }

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
      Title = $"IM Guest Movements - Guest ID {_guestID}";
      var movement = await BRGuests.GetGuestMovement(_guestID);
      guestMovementsDataGrid.ItemsSource = movement;
      Mouse.OverrideCursor = null;
    }
    #endregion

    /// <summary>
    /// Imprime los movimientos del guest
    /// </summary>
    /// <history>
    /// [jorcanche] created 03/10/2016
    /// </history>
    private async void btnPrintGuestMovements_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (guestMovementsDataGrid == null) return;
        if (guestMovementsDataGrid.Items.Count == 0)
        {
          UIHelper.ShowMessage("There is no info to make a report");
          return;
        }
        Mouse.OverrideCursor = Cursors.Wait;
        FileInfo fileInfo = await ReportBuilder.CreateCustomExcel(
          TableHelper.GetDataTableFromList((List<GuestMovements>)guestMovementsDataGrid.ItemsSource, true, true, true),
          new List<Tuple<string, string>> { Tuple.Create("Guest Id", _guestID.ToString()) },
          "Guest Movements",
          DateHelper.DateRangeFileName(DateTime.Today, DateTime.Today),
          EpplusHelper.OrderColumns(guestMovementsDataGrid.Columns.ToList(), Reports.clsFormatReports.RptGuestMovements()));
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
  }
}
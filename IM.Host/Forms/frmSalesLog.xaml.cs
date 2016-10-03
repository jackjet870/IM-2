using IM.Base.Classes;
using IM.Base.Forms;
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

namespace IM.Host.Forms
{
  /// <summary>
  /// Formulario que visualiza el log del sale
  /// </summary>
  /// <history>
  /// [jorcanche]  created  07/07/2016
  /// </history>
  public partial class frmSalesLog : Window
  {
    private int _sale;
    private string _membership;
    public frmSalesLog(int sale, string membership)
    {
      InitializeComponent();
      _sale = sale;
      _membership = membership;
    }

    /// <summary>
    /// Carga e inicializa las variables del formulario 
    /// </summary>
    /// <history>
    /// [jorcanche]  created 07/07/2016
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Mouse.OverrideCursor = Cursors.Wait;
      Title = $"Sales Log - Sale ID {_sale} / Membership Number {_membership}"; 
      var saleLog = await BRSales.GetSaleLog(_sale);
      saleLogDataDataGrid.ItemsSource = saleLog;
      Mouse.OverrideCursor = null;
    }

    /// <summary>
    /// Muestra los movimientos de los Salesman
    /// </summary>
    /// <history>
    /// [jorcanche]  created 07072016
    /// </history>
    private void btnSalesMenChanges_Click(object sender, RoutedEventArgs e)
    {
      var salesmenchanges = new frmSalesmenChanges(_sale, _membership) {Owner = this};
      salesmenchanges.ShowDialog();      
    }

    /// <summary>
    /// Imprime el reporte del Sale Log
    /// </summary>
    /// <history>
    /// [jorcanche]  created 07072016
    /// [edgrodriguez] 05/09/2016 Modified. Se cambio el método CreateExcelCustom por CreatCustomExcel
    /// </history>
    private async void btnPrintSaleLog_Click(object sender, RoutedEventArgs e)
    {     
      try
      {
        if (saleLogDataDataGrid == null) return;
        if (saleLogDataDataGrid.Items.Count == 0)
        {
          UIHelper.ShowMessage("There is no info to make a report");
          return;
        }
        Mouse.OverrideCursor = Cursors.Wait;
        FileInfo fileInfo = await ReportBuilder.CreateCustomExcel(
          TableHelper.GetDataTableFromList((List<SaleLogData>)saleLogDataDataGrid.ItemsSource, true, true, true),
          new List<Tuple<string, string>> { Tuple.Create("Sale Id", _sale.ToString()) },
          "Sale Log",
          DateHelper.DateRangeFileName(DateTime.Today, DateTime.Today),
          EpplusHelper.OrderColumns(saleLogDataDataGrid.Columns.ToList(), Classes.clsFormatReport.RptSaleLog()));
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

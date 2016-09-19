using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

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
      Title = $"Sales Log - Sale ID {_sale} / Membership Number {_membership}"; 
      var saleLog = await BRSales.GetSaleLog(_sale);
     // var saleLogDataViewSource = (CollectionViewSource)FindResource("saleLogDataViewSource");
     // saleLogDataViewSource.Source = saleLog;
      saleLogDataDataGrid.ItemsSource = saleLog;
    }

    /// <summary>
    /// Muestra los movimientos de los Salesman
    /// </summary>
    /// <history>
    /// [jorcanche]  created 07072016
    /// </history>
    private void btnSalesMan_Click(object sender, RoutedEventArgs e)
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
    private async void btnPrintGuestLog_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (saleLogDataDataGrid.ItemsSource == null) return;

        FileInfo fileInfo = await EpplusHelper.CreateCustomExcel(
          TableHelper.GetDataTableFromList((List<SaleLogData>)saleLogDataDataGrid.ItemsSource, true, true, true),
          new List<Tuple<string, string>> { Tuple.Create("Sale Id", _sale.ToString()) },
          "Sale Log",
          DateHelper.DateRangeFileName(DateTime.Today, DateTime.Today),
          EpplusHelper.OrderColumns(saleLogDataDataGrid.Columns.ToList(), Classes.clsFormatReport.RptSaleLog()));
        if(fileInfo!=null)
        {
          frmDocumentViewer documentViewver = new frmDocumentViewer(fileInfo, Context.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly), false);
          documentViewver.Owner = this;
          documentViewver.ShowDialog();
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
  }
}

﻿using System.Windows;
using System.Windows.Data;
using IM.BusinessRules.BR;
using IM.Model.Classes;
using IM.Base.Helpers;
using System.Collections.Generic;
using System;
using IM.Model;
using System.Linq;
using IM.Host.Classes;
using System.Diagnostics;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmExchangeRateLog.xaml
  /// </summary>
  public partial class frmExchangeRateLog : Window
  {
    private string _currency;
    CollectionViewSource _cvsExchangeRate;
    public frmExchangeRateLog(string currency)
    {
      _currency = currency;

      InitializeComponent();

    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _cvsExchangeRate = ((CollectionViewSource)(this.FindResource("dsExchangeRateLog")));

      // Obtenemos el historico del currency
      _cvsExchangeRate.Source = BRExchangeRatesLogs.GetExchangeRateLog(_currency);
    }

    /// <summary>
    /// Función para contabilizar los recordset obtenidos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 14/03/2016 Created
    /// </history>
    private void getExchangeRateLogDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      if (getExchangeRateLogDataGrid.Items.Count == 0)
      {
        StatusBarReg.Content = "No Records";
        return;
      }
      StatusBarReg.Content = string.Format("{0}/{1}", getExchangeRateLogDataGrid.Items.IndexOf(getExchangeRateLogDataGrid.CurrentItem) + 1, getExchangeRateLogDataGrid.Items.Count);
    }

    #region btnPrint_Click
    /// <summary>
    /// Exporta los registros de grid a un archivo de excel.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/07/2016 Created
    /// </history>
    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      if (((List<ExchangeRateLogData>)_cvsExchangeRate.Source).Any())
      {
        #region format Excel
        List<ExcelFormatTable> lstFormat = clsFormatReport.RptExchangeRatesLog();
        #endregion
        EpplusHelper.OrderColumns(getExchangeRateLogDataGrid.Columns.ToList(), lstFormat);

        var fileinfo = EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList((List<ExchangeRateLogData>)_cvsExchangeRate.Source, true, true, true),
          new List<Tuple<string, string>> { Tuple.Create("Date Range", DateHelper.DateRange(DateTime.Today, DateTime.Today)), Tuple.Create("Gift ID", string.Join(",", ((List<ExchangeRateLogData>)_cvsExchangeRate.Source).Select(c => c.elcu).Distinct().ToList())) },
          "Exchange Rates Log", DateHelper.DateRangeFileName(DateTime.Today, DateTime.Today), lstFormat);

        if (fileinfo != null)
          Process.Start(fileinfo.FullName);
      }
      else
        UIHelper.ShowMessage("There is no info to make a report");
    } 
    #endregion
  }
}
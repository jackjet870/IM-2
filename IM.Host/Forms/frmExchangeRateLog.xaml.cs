using System.Windows;
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
using System.Windows.Input;
using System.Windows.Media;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmExchangeRateLog.xaml
  /// </summary>
  public partial class frmExchangeRateLog : Window
  {
    #region Variables
    private string _currency;
    CollectionViewSource _cvsExchangeRate; 
    #endregion

    #region Contructor
    public frmExchangeRateLog(string currency)
    {
      _currency = currency;

      InitializeComponent();
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// 
    /// </summary>
    /// <history>
    /// [vipacheco] 16/Agosto/2016 Modified -> Se agrego los metodos que validan los keypress
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _cvsExchangeRate = ((CollectionViewSource)(this.FindResource("dsExchangeRateLog")));

      // Obtenemos el historico del currency
      _cvsExchangeRate.Source = BRExchangeRatesLogs.GetExchangeRateLog(_currency);

      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region getExchangeRateLogDataGrid_SelectionChanged
    /// <summary>
    /// Función para contabilizar los recordset obtenidos
    /// </summary>
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
      StatusBarReg.Content = string.Format("{0}/{1}", getExchangeRateLogDataGrid.Items.IndexOf(getExchangeRateLogDataGrid.Items.CurrentItem) + 1, getExchangeRateLogDataGrid.Items.Count);
    }
    #endregion

    #region btnPrint_Click
    /// <summary>
    /// Exporta los registros de grid a un archivo de excel.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/07/2016 Created
    /// [edgrodriguez] 05/09/2016 Modified. Se cambio el método CreateExcelCustom por CreatCustomExcel
    /// </history>
    private async void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      if (((List<ExchangeRateLogData>)_cvsExchangeRate.Source).Any())
      {
        var fileinfo = await EpplusHelper.CreateCustomExcel(TableHelper.GetDataTableFromList((List<ExchangeRateLogData>)_cvsExchangeRate.Source, true, true, true),
         new List<Tuple<string, string>> { Tuple.Create("Date Range", DateHelper.DateRange(DateTime.Today, DateTime.Today)), Tuple.Create("Gift ID", string.Join(",", ((List<ExchangeRateLogData>)_cvsExchangeRate.Source).Select(c => c.elcu).Distinct().ToList())) },
         "Exchange Rates Log", DateHelper.DateRangeFileName(DateTime.Today, DateTime.Today), EpplusHelper.OrderColumns(getExchangeRateLogDataGrid.Columns.ToList(), clsFormatReport.RptExchangeRatesLog()));

        if (fileinfo != null)
          Process.Start(fileinfo.FullName);
      }
      else
        UIHelper.ShowMessage("There is no info to make a report");
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [vipacheco] 16/Agosto/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }
    #endregion

    #region KeyDefault
    /// <summary>
    /// Asigna valores por defecto a los StatusBarItem cuando se carga el form
    /// </summary>
    /// <param name="statusBar">StatusBarItem al que se le asignaran los valores</param>
    /// <history>
    /// [vipacheco] 16/Agosto/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }
    #endregion

    #region CkeckKeysPress
    /// <summary>
    /// Revisa si alguna de las teclas Bloq Mayús, Bloq Núm o Insert está activo
    /// </summary>
    /// <history>
    /// [vipacheco] 16/Agosto/2016 Created
    /// </history>
    private void CkeckKeysPress(System.Windows.Controls.Primitives.StatusBarItem statusBar, Key key)
    {
      var keyPess = Keyboard.GetKeyStates(key).ToString();

      if (keyPess.Contains("Toggled")) //si está activo el Bloq Mayús
      {
        statusBar.FontWeight = FontWeights.Bold;
        statusBar.Foreground = Brushes.Black;
      }
      else
      {
        KeyDefault(statusBar);
      }
    }
    #endregion

  }
}
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using IM.BusinessRules.BR;
using IM.Model.Classes;
using IM.Base.Helpers;
using System.Collections.Generic;
using IM.Host.Classes;
using System.Linq;
using IM.Model;
using System;
using System.Diagnostics;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmSalesRoomsLog.xaml
  /// </summary>
  public partial class frmSalesRoomsLog : Window
  {
    CollectionViewSource _salesRoomLog;
    public frmSalesRoomsLog()
    {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

      _salesRoomLog = ((CollectionViewSource)(this.FindResource("dsSalesRoomLog")));
      _salesRoomLog.Source = BRSalesRoomsLogs.GetSalesRoomLog(App.User.SalesRoom.srID);

      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [lchairezReload] 09/Feb/2016 Created
    /// </history>
    private void frmSalesRoomsLog_KeyDown(object sender, KeyEventArgs e)
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

    /// <summary>
    /// Revisa si alguna de las teclas Bloq Mayús, Bloq Núm o Insert está activo
    /// </summary>
    /// <param name="statusBar">StatusBarItem al que se le asignaran valores si está activa la tecla</param>
    /// <param name="key">tecla que revisaremos si se encuentra activa</param>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
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

    /// <summary>
    /// Asigna valores por defecto a los StatusBarItem cuando se carga el form
    /// </summary>
    /// <param name="statusBar">StatusBarItem al que se le asignaran los valores</param>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }

    private void dtgSalesRoomLog_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      StatusBarReg.Content = string.Format("{0}/{1}", dtgSalesRoomLog.Items.IndexOf(dtgSalesRoomLog.CurrentItem) + 1, dtgSalesRoomLog.Items.Count);
    }

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
      if (((List<SalesRoomLogData>)_salesRoomLog.Source).Any())
      {
        var fileinfo = await EpplusHelper.CreateCustomExcel(TableHelper.GetDataTableFromList((List<SalesRoomLogData>)_salesRoomLog.Source, true, true, true),
          new List<Tuple<string, string>> { Tuple.Create("Date Range", DateHelper.DateRange(DateTime.Today, DateTime.Today)), Tuple.Create("Sales Room ID", App.User.SalesRoom.srID) },
          "Sales Rooms Log", DateHelper.DateRangeFileName(DateTime.Today, DateTime.Today), EpplusHelper.OrderColumns(dtgSalesRoomLog.Columns.ToList(), clsFormatReport.RptCloseSalesRoomLog()));

        if (fileinfo != null)
          Process.Start(fileinfo.FullName);
      }
      else
        UIHelper.ShowMessage("There is no info to make a report");
    }
    #endregion
  }
}

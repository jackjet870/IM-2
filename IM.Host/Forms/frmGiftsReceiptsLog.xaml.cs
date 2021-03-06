﻿using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Classes;
using IM.Model;
using IM.Model.Enums;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmGiftsReceiptsLog.xaml
  /// </summary>
  public partial class frmGiftsReceiptsLog : Window
  {

    int _ReceiptID = 0;
    CollectionViewSource _dsGifsReceiptLog;

    #region Contructores
    public frmGiftsReceiptsLog(int ReceiptID = 0)
    {
      _ReceiptID = ReceiptID;

      InitializeComponent();
    } 
    #endregion

    #region btnClose_Click
    /// <summary>
    /// Cierra el formulario
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga el historico
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _dsGifsReceiptLog = ((CollectionViewSource)(this.FindResource("dsGifsReceiptLog")));

      _busyIndicator.IsBusy = true;
      _dsGifsReceiptLog.Source = await BRGiftsReceiptLog.GetGiftsReceiptLog(_ReceiptID);
      _busyIndicator.IsBusy = false;
    }
    #endregion

    #region StatusBar

    #region Window_KeyDown
    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [vipacheco] 03/Junio/2016 Created
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

    #region CkeckKeysPress
    /// <summary>
    /// Verifica que la tecla se encuentre activa/inactiva, para cambiar el estilo de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [vipacheco] 03/Junio/2016 Created
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

    #region KeyDefault
    /// <summary>
    /// Configuracion inicial de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [vipacheco] 03/Junio/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }
    #endregion

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [vipacheco] 03/Junio/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

    #endregion

    #endregion

    #region grdLog_SelectionChanged
    /// <summary>
    /// Funcion que se encarga de validar el total de datos obtenidos en el datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void grdLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (grdLog.Items.Count == 0)
      {
        StatusBarReg.Content = "0 / 0";
        return;
      }
      StatusBarReg.Content = $"{grdLog.SelectedIndex + 1} / {grdLog.Items.Count}";
    }
    #endregion

    #region btnPrint_Click
    /// <summary>
    /// Exporta los registros de grid a un archivo de excel.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/07/2016 Created
    /// [edgrodriguez] 05/09/2016 Modified. Se cambio el método CreateExcelCustom por CreatCustomExcel
    /// [emoguel] 08/09/2016 Modified. Ahora se habre el visor de reportes
    /// </history>
    private async void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      if (((List<GiftsReceiptLogData>)_dsGifsReceiptLog.Source).Any())
      {
        var fileinfo = await ReportBuilder.CreateCustomExcelAsync(TableHelper.GetDataTableFromList((List<GiftsReceiptLogData>)_dsGifsReceiptLog.Source, true, true, true),
          new List<Tuple<string, string>> { Tuple.Create("Date Range", DateHelper.DateRange(DateTime.Today, DateTime.Today)), Tuple.Create("Gift Receipt ID", string.Join(",", ((List<GiftsReceiptLogData>)_dsGifsReceiptLog.Source).Select(c => c.goID).Distinct().ToList())) },
          "Gift Receipts Log", DateHelper.DateRangeFileName(DateTime.Today, DateTime.Today), EpplusHelper.OrderColumns(grdLog.Columns.ToList(), clsFormatReport.RptGiftReceiptsLog()));

        if (fileinfo != null)
        {
          frmDocumentViewer documentViewver=new frmDocumentViewer(fileinfo,Context.User.HasPermission(EnumPermission.RptExcel,EnumPermisionLevel.ReadOnly),false);
          documentViewver.Owner = this;
          documentViewver.ShowDialog();
        }          
      }
      else
        UIHelper.ShowMessage("There is no info to make a report");
    }
    #endregion

  }
}

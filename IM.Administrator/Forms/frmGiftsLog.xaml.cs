﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Administrator.Classes;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmGiftsLog.xaml
  /// </summary>
  public partial class frmGiftsLog : Window
  {

    #region Variables
    public string idGift = "";
    #endregion
    public frmGiftsLog()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/072016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Title = Title + " - Gift ID " + idGift;
      LoadGifLogs();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/07/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region window keyDown
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/07/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
            break;
          }
        case Key.Insert:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
            break;
          }
        case Key.NumLock:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
            break;
          }
      }
    }
    #endregion

    #region Print
    /// <summary>
    /// Guarda un excel con los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/07/2016
    /// </history>
    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      #region format Excel
      List<ExcelFormatTable> lstFormat = clsFormatReport.RptGiftLog();
      #endregion
      EpplusHelper.OrderColumns(dgrGifsLog.Columns.ToList(), lstFormat);    

      var fileinfo = EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList((List<GiftLogData>)dgrGifsLog.ItemsSource, true, true,true), new List<Tuple<string, string>> { Tuple.Create("GIFT ID",idGift)},
        "Gifts Log", DateHelper.DateRangeFileName(DateTime.Today,DateTime.Today), lstFormat);
    }
    #endregion

    #region dgrGifsLog_SelectionChanged
    /// <summary>
    /// Nos indica en la barra de estatus el numero de elementos (Index) que esta seleccionado en el datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [erosado] 05/07/2016  Created
    /// </history>
    private void dgrGifsLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarReg.Content = string.Format("{0}/{1}", (dgrGifsLog.SelectedIndex + 1).ToString(), dgrGifsLog.Items.Count.ToString());
    }
    #endregion
    #endregion

    #region Methods
    #region LoadGifLogs
    /// <summary>
    /// Llena el grid de logs
    /// </summary>
    /// <history>
    /// [emoguel] created 01/07/2016
    /// </history>
    private async void LoadGifLogs()
    {
      try
      {
        dgrGifsLog.ItemsSource = await BRGifts.GetGiftsLog(idGift);
        GridHelper.SelectRow(dgrGifsLog, 0);
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gifts");
      }
    }
    #endregion

    
    #endregion
  }
}
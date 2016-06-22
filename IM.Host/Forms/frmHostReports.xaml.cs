﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Reports;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmHostReports.xaml
  /// </summary>
  public partial class frmHostReports : Window
  {
    #region Atributos
    public UserLogin User { get; set; }
    private DateTime _date; 
    #endregion

    #region Constructor
    public frmHostReports(DateTime selectedDate)
    {
      InitializeComponent();
      DataContext = this;
      User = App.User.User;
      _date = selectedDate;
    } 
    #endregion

    #region Métodos Privados

    #region lstHostReports_Select
    /// <summary>
    /// Exporta el reporte seleccionado a un archivo de excel.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 21/Jun/2016 Created
    /// </history>
    private async void lstHostReports_Select(object sender, RoutedEventArgs e)
    {
      if (e.GetType() == typeof(KeyEventArgs) && ((KeyEventArgs)e).Key != Key.Enter) return;
      var filters = new List<Tuple<string, string>>();
      var daterange = DateHelper.DateRange(dtpDate.Value.Value, dtpDate.Value.Value);
      var dateFileName = DateHelper.DateRangeFileName(dtpDate.Value.Value, dtpDate.Value.Value);
      switch ((lstHostReports.SelectedItem as ListBoxItem).Content.ToString())
      {
        case "Premanifest":
          var lstPremanifest = await BRGeneralReports.GetRptPremanifest(dtpDate.Value.Value, salesRoom: App.User.SalesRoom.srID);
          if (lstPremanifest.Any())
          {
            filters.Add(Tuple.Create("Filter Range", daterange));
            filters.Add(Tuple.Create("Sales Room", App.User.SalesRoom.srID));
            EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstPremanifest, true, true), filters, "Premanifest", dateFileName, clsFormatReports.RptPremanifest());
          }
          else
            UIHelper.ShowMessage("There is no data for make a report");
          break;
        case "Premanifest With Gifts":
          var lstPremanifestWithG = await BRGeneralReports.GetRptPremanifestWithGifts(dtpDate.Value.Value, salesRoom: App.User.SalesRoom.srID);
          if (lstPremanifestWithG.Any())
          {
            filters.Add(Tuple.Create("Filter Range", daterange));
            filters.Add(Tuple.Create("Sales Room", App.User.SalesRoom.srID));
            EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstPremanifestWithG, true, true), filters, "PremanifestWithGifts", dateFileName, clsFormatReports.RptPremanifestWithGifts());
          }
          else
            UIHelper.ShowMessage("There is no data for make a report");
          break;
        case "Up List End":
          break;
        case "Up List Start":
          break;
      }
    }
    #endregion 
    #endregion

    #region Eventos del Formulario
    #region Window_Loaded
    /// <summary>
    /// Inicio del formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 22/Jun/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      dtpDate.Value = _date;
    }

    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [edgrodriguez] 22/Jun/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
          break;
        case Key.Insert:
          KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
          break;
        case Key.NumLock:
          KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
          break;
      }
    }

    #endregion

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [edgrodriguez] 22/Jun/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion 
    #endregion
  }
}

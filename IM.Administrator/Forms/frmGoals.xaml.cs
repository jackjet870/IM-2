using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System.Globalization;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmGoals.xaml
  /// </summary>
  public partial class frmGoals : Window
  {
    List<Goal> _lstAllGoals = new List<Goal>();//Lista inicial de goals
    Goal _goal = new Goal();//Goal con los filtros
    public frmGoals()
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
    /// [emoguel] created 10/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadPlacesTypes();
      LoadPeriods();      
      ComboBoxHelper.ConfigureDates(cmbRangeDate,EnumPeriod.Monthly,selectedCmb:1);      
      cmbPlace.SelectedValue = "LS";
      LoadGoals();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/05/2016
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
    /// [emoguel] created 10/05/2016
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

    #region Save
    /// <summary>
    /// Guarda los cambios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/05/2016
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      btnSave.Focus();
      List<Goal> lstGoals = (List<Goal>)dgrGoals.ItemsSource;
      if (lstGoals.Count > 0)
      {
        bool blnValid=false;
        lstGoals.ForEach(go =>
        {
          if (go.goPlaceID == null)
          {
            int nIndex = lstGoals.IndexOf(go);
            GridHelper.SelectRow(dgrGoals, nIndex, 0, true);
            UIHelper.ShowMessage("Goal " + cmbPlace.Text + " Can not be empty.");
            blnValid = true;
            return;
          }
        });  
        if(blnValid)
        {
          return;
        }
        List<Goal> lstAdd = lstGoals.Where(go => !_lstAllGoals.Any(goA => goA.goPlaceID == go.goPlaceID)).ToList();
        List<Goal> lstUpdate= lstGoals.Where(go => _lstAllGoals.Any(goA => goA.goPlaceID == go.goPlaceID)).ToList();
        List<Goal> lstDel = _lstAllGoals.Where(go =>!lstGoals.Any(goA=>goA.goPlaceID==go.goPlaceID)).ToList();

        int nRes = BRGoals.SaveGoal(_goal, lstAdd, lstUpdate, lstDel);
        UIHelper.ShowMessageResult("Goals", nRes);
        if (nRes > 0)
        {
          _lstAllGoals = new List<Goal>();
          lstGoals.ForEach(go => {
            Goal goal = new Goal();
            ObjectHelper.CopyProperties(goal, go);
            _lstAllGoals.Add(goal);
          });
          cmbPlace.SelectedValue = _goal.gopy;
          cmbPeriod.SelectedValue = (_goal.gopd == "M") ? EnumPeriod.Monthly : EnumPeriod.Weekly;
          dtpFrom.Value = _goal.goDateFrom;
          dtpTo.Value = _goal.goDateTo;
        }
      }
      else
      {
        UIHelper.ShowMessage("Can not saved empty.");
      }
    }
    #endregion

    #region Show
    /// <summary>
    /// muesra los GOals segun los filtros seleccionados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/05/2016
    /// </history>
    private void btnShow_Click(object sender, RoutedEventArgs e)
    {
      LoadGoals();
    }
    #endregion

    #region cmbPeriod_SelectionChanged
    /// <summary>
    /// Llena el combo de range dates dependiendo lo seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/05/2016
    /// </history>
    private void cmbPeriod_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {      
      EnumPeriod period = ((EnumPeriod)cmbPeriod.SelectedValue);
      ComboBoxHelper.ConfigureDates(cmbRangeDate, period, selectedCmb: 1);
    }
    #endregion

    #region cmbRangeDate_SelectionChanged
    /// <summary>
    /// Carga las fechas dependiendo del rango seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbRangeDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count > 0)
      {        
        EnumPredefinedDate enumPeriod = ((EnumPredefinedDate)cmbRangeDate.SelectedValue);
        Tuple<DateTime, DateTime> dateRange = DateHelper.GetDateRange(enumPeriod);

        dtpFrom.Value = dateRange.Item1;
        dtpTo.Value = dateRange.Item2;
        lblDates.Content = dateRange.Item1.ToString("MMMM", CultureInfo.InvariantCulture) + " " + dateRange.Item1.Day + " - " + dateRange.Item2.Day + ", "+ dateRange.Item1.Year;
        if(enumPeriod==EnumPredefinedDate.DatesSpecified)
        {
          dtpFrom.IsEnabled = true;
          dtpTo.IsEnabled = true;
          dtpFrom.ValueChanged += dtp_valueDateChanged;
          dtpTo.ValueChanged += dtp_valueDateChanged;
        }
        else
        {
          dtpFrom.IsEnabled = false;
          dtpTo.IsEnabled = false;
          dtpFrom.ValueChanged -= dtp_valueDateChanged;
          dtpTo.ValueChanged -= dtp_valueDateChanged;
        }        

      }
    }
    #endregion    

    #region TextBox_PreviewTextInput
    /// <summary>
    /// Permite numero y un punto decimal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/05/2016
    /// </history>
    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if (!(e.Text == "." && !txt.Text.Trim().Contains(".")))
      {
        e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
      }
    }
    #endregion

    #region dtp_valueDateChanged
    /// <summary>
    /// Cambia el texto del label de rango de fechas
    /// segun lo seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 11/05/2016 created
    /// </history>
    private void dtp_valueDateChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      DateTime dtFrom = Convert.ToDateTime(dtpFrom.Value);
      DateTime dtTo = Convert.ToDateTime(dtpFrom.Value);
      lblDates.Content = dtFrom.ToString("MMMM", CultureInfo.InvariantCulture) + " " + dtFrom.Day + " - " + dtTo.Day + ", " + dtFrom.Year;
    }
    #endregion
    #endregion

    #region Methods

    #region LoadPlacesTypes
    /// <summary>
    /// Llena el combobox de places Types
    /// </summary>
    /// <history>
    /// [emoguel] created 10/05/2016
    /// </history>
    private async void LoadPlacesTypes()
    {
      try
      {
        List<PlaceType> lstPlacesTypes =await BRPlaceTypes.GetPlaceTypes(1);
        cmbPlace.ItemsSource = lstPlacesTypes;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadPeriods
    /// <summary>
    /// Llena el combobox de periods
    /// </summary>
    /// <history>
    /// [emoguel] created 10/05/2016
    /// </history>
    private void LoadPeriods()
    {
      cmbPeriod.ItemsSource = EnumToListHelper.GetList<EnumPeriod>().Where(ep => ep.Value != "None").OrderBy(ep => ep.Value);
      cmbPeriod.SelectedIndex = 0;
    }
    #endregion

    #region Locations
    /// <summary>
    /// Llena el combobox de Locations
    /// </summary>
    /// <history>
    /// [emoguel] created 10/05/2016
    /// </history>
    private async void LoadLocations()
    {
      try
      {
        List<Location> lstLocations = await BRLocations.GetLocations(1);
        cmbLoc.ItemsSource = lstLocations;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LeadSources
    /// <summary>
    /// Carga el combobox de LeadSources
    /// </summary>
    /// <history>
    /// [emoguel] created 10/05/2016
    /// [emoguel] modified 25/05/2016 se volvio Asyncrono el método 
    /// </history>
    private async void LoadLeadSources()
    {
      try
      {
        List<LeadSource> lstLeadSources = await BRLeadSources.GetLeadSources(1, -1);
        cmbLoc.ItemsSource = lstLeadSources;
      }
      catch
      (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadSalesRooms
    /// <summary>
    /// Llena el combo de loc
    /// </summary>
    /// <history>
    /// [emoguel] created 10/05/2016
    /// [erosado] 24/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadSalesRooms()
    {
      try
      {
        List<SalesRoomShort> lstSalesRoom = await BRSalesRooms.GetSalesRooms(1);
        cmbLoc.ItemsSource = lstSalesRoom;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadWarehouses
    /// <summary>
    /// LoadWarehouses
    /// </summary>
    /// <history>
    /// [emoguel] created 11/05/2016
    /// </history>
    private async void LoadWarehouses()
    {
      try
      {
        List<Warehouse> lstWarehouses =await BRWarehouses.GetWareHouses(1);
        cmbLoc.ItemsSource = lstWarehouses;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    } 
    #endregion

    #endregion

    #region LoadGoals
    /// <summary>
    /// Llena el grid de goals
    /// </summary>
    /// <history>
    /// [emoguel] created 11/05/2016
    /// </history>
    private async void LoadGoals()
    {
      try
      {
        status.Visibility = Visibility.Visible;
        EnumPeriod period = ((EnumPeriod)cmbPeriod.SelectedValue);
        _goal.gopy = cmbPlace.SelectedValue.ToString();
        _goal.gopd = (period == EnumPeriod.Monthly) ? "M" : "W";
        _goal.goDateFrom = Convert.ToDateTime(dtpFrom.Value);
        _goal.goDateTo = Convert.ToDateTime(dtpTo.Value);

        #region DataGridCombobox
        switch (_goal.gopy)
        {
          case "LS":
            {
              cmbLoc.SelectedValuePath = "lsID";
              cmbLoc.DisplayMemberPath = "lsN";
              LoadLeadSources();
              break;
            }
          case "LO":
            {
              cmbLoc.SelectedValuePath = "loID";
              cmbLoc.DisplayMemberPath = "loN";
              LoadLocations();
              break;
            }

          case "SR":
            {
              cmbLoc.SelectedValuePath = "srID";
              cmbLoc.DisplayMemberPath = "srN";
              LoadSalesRooms();
              break;
            }
          case "WH":
            {
              cmbLoc.SelectedValuePath = "whID";
              cmbLoc.DisplayMemberPath = "whN";
              LoadWarehouses();
              break;
            }
        }
        #endregion

        List<Goal> lstGoal =await BRGoals.GetGoals(_goal);
        _lstAllGoals = new List<Goal>();
        lstGoal.ForEach(go =>
        {
          Goal goal = new Goal();
          ObjectHelper.CopyProperties(goal, go);
          _lstAllGoals.Add(goal);
        });
        if (lstGoal.Count == 0)
        {
          LoadGoalsDefault(_goal);
        }
        else
        {
          dgrGoals.ItemsSource = lstGoal;
        }
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }

    #endregion

    #region LoadGoalsDefault
    /// <summary>
    /// Carga datos predefinidos en caso de que no haya una lista de Goals
    /// </summary>
    /// <param name="goal"></param>
    /// <history>
    /// [emoguel] created 11/05/2016
    /// </history>
    private void LoadGoalsDefault(Goal goal)
    {
      if (UIHelper.ShowMessage("There are no goals for this period. Would you like to generate?", MessageBoxImage.Question) == MessageBoxResult.Yes)
      {
        List<Goal> lstGoals = new List<Goal>();
        switch (goal.gopy)
        {
          case "LS":
            {
              List<LeadSource> lstLeadSources = (List<LeadSource>)cmbLoc.ItemsSource;
              lstLeadSources.ForEach(ls =>
              {
                lstGoals.Add(new Goal { gopy = goal.gopy, goGoal = 0, goPlaceID = ls.lsID });
              });
              break;
            }
          case "LO":
            {
              List<Location> lstLocations = (List<Location>)cmbLoc.ItemsSource;
              lstLocations.ForEach(lo =>
              {
                lstGoals.Add(new Goal { gopy = goal.gopy, goGoal = 0, goPlaceID = lo.loID });
              });
              break;
            }
          case "SR":
            {
              List<SalesRoomShort> lstSalesRoom = (List<SalesRoomShort>)cmbLoc.ItemsSource;
              lstSalesRoom.ForEach(sr =>
              {
                lstGoals.Add(new Goal { gopy = goal.gopy, goGoal = 0, goPlaceID = sr.srID });
              });
              break;
            }

          case "WH":
            {
              List<Warehouse> lstWareHouse = (List<Warehouse>)cmbLoc.ItemsSource;
              lstWareHouse.ForEach(wh =>
              {
                lstGoals.Add(new Goal { gopy = goal.gopy, goGoal = 0, goPlaceID = wh.whID });
              });
              break;
            }
        }

        dgrGoals.ItemsSource = lstGoals;
      }
    }
    #endregion

    #region dgrGoals_CellEditEnding
    /// <summary>
    /// Verifica que el LS|LO|SR|WH no se repita en la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/05/2016
    /// </history>
    private void dgrGoals_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))//Verificar si se está cancelando
      {
        if (e.Column is DataGridComboBoxColumn)//verificar si es el datagridcolumn
        {
          List<Goal> lstGoals = (List<Goal>)dgrGoals.ItemsSource;
          Goal goalSel = (Goal)dgrGoals.SelectedItem;
          switch (_goal.gopy)
          {

            #region LeadSource
            case "LS":
              {

                LeadSource leadSource = (LeadSource)((ComboBox)e.EditingElement).SelectedItem;
                if (leadSource != null)
                {
                  if (goalSel.goPlaceID != leadSource.lsID)
                  {
                    Goal goalVal = lstGoals.Where(go => !go.Equals(goalSel) && go.goPlaceID == leadSource.lsID).FirstOrDefault();
                    if (goalVal != null)
                    {
                      UIHelper.ShowMessage("Lead Source must not be repeated");
                      e.Cancel = true;
                    }
                  }
                }
                break;
              }
            #endregion

            #region Locations
            case "LO":
              {
                Location location = (Location)((ComboBox)e.EditingElement).SelectedItem;
                if (location != null)
                {
                  if (goalSel.goPlaceID != location.loID)
                  {
                    Goal goalVal = lstGoals.Where(go => !go.Equals(goalSel) && go.goPlaceID == location.loID).FirstOrDefault();
                    if (goalVal != null)
                    {
                      UIHelper.ShowMessage("Location must not be repeated");
                      e.Cancel = true;
                    }
                  }
                }
                break;
              }
            #endregion

            #region SalesRoom
            case "SR":
              {
                SalesRoom saleRoom = (SalesRoom)((ComboBox)e.EditingElement).SelectedItem;
                if (saleRoom != null)
                {
                  if (goalSel.goPlaceID != saleRoom.srID)
                  {
                    Goal goalVal = lstGoals.Where(go => !go.Equals(goalSel) && go.goPlaceID == saleRoom.srID).FirstOrDefault();
                    if (goalVal != null)
                    {
                      UIHelper.ShowMessage("Sales Room must not be repeated");
                      e.Cancel = true;
                    }
                  }
                }
                break;
              }
            #endregion

            #region Warehouse
            case "WH":
              {
                Warehouse warehouse = (Warehouse)((ComboBox)e.EditingElement).SelectedItem;
                if (warehouse != null)
                {
                  if (goalSel.goPlaceID != warehouse.whID)
                  {
                    Goal goalVal = lstGoals.Where(go => !go.Equals(goalSel) && go.goPlaceID == warehouse.whID).FirstOrDefault();
                    if (goalVal != null)
                    {
                      UIHelper.ShowMessage("Warehouse must not be repeated");
                      e.Cancel = true;
                    }
                  }
                }
                break;
              }
              #endregion
          }
        }
      }
    }
    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Classes;
using IM.Model.Helpers;
using System.Globalization;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSeasonDetail.xaml
  /// </summary>
  public partial class frmSeasonDetail : Window
  {
    #region Atributos
    public Season season = new Season();
    public Season oldSeason = new Season();
    private List<SeasonDate> _lstOldSeasonDates = new List<SeasonDate>();//Lista inicial de season dates
    private List<SeasonDate> _lstSeasonDates = new List<SeasonDate>();//Lista de season dates para el viewSource 
    private List<SeasonDate> _lstSeasonDatesRange = new List<SeasonDate>();//Lista de rango de fechas del año
    public EnumMode enumMode;
    private TextBox changedTextBox;
    private bool _isClosing = false;
    private bool isCancel = false;
    private bool isEdit = false;
    private DateTime _year;
    #endregion
    public frmSeasonDetail()
    {
      InitializeComponent();
    }

    #region Metodos
    #region LoadSeasonDates
    /// <summary>
    ///   Carga las fechas de la temporada
    /// </summary>
    /// <history>
    ///   [vku] 27/Jul/2016 Created
    /// </history>
    private async void LoadSeasonDates()
    {
      try
      {
        List<SeasonDate> lstSeasonDates = await BRSeasons.GetSeasonDates();
        _lstSeasonDates = lstSeasonDates.Where(sd => sd.sdss == season.ssID && sd.sdStartD.Year == _year.Year).ToList();
        dgrDates.ItemsSource = _lstSeasonDates;


        _lstOldSeasonDates.Clear();
        List<SeasonDate> lstOldSeasonDates = await BRSeasons.GetSeasonDates();
        _lstOldSeasonDates = lstOldSeasonDates.Where(sd => sd.sdss == season.ssID && sd.sdStartD.Year == _year.Year).ToList();


        _lstSeasonDatesRange = lstSeasonDates.Where(sd => sd.sdStartD.Year == _year.Year).ToList();

        LoadSeasonsDatesUnassigned();
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadSeasonsDatesUnassigned
    /// <summary>
    ///   Carga las fechas no asignadas de la temporada
    /// </summary>
    /// <history>
    ///   [vku] 28/Jul/2016 Created
    /// </history>
    private void LoadSeasonsDatesUnassigned()
    {
      bool blnInitRange = false;
      DateTime dtmStartRange = new DateTime();
      DateTime dtmEndRange = new DateTime();
      DateTime dtmStart = new DateTime(_year.Year, 1, 1);
      DateTime dtmEnd = new DateTime(_year.Year, 12, 31);
      for (DateTime i = dtmStart; i < dtmEnd; i = i.AddDays(1))
      {
        if (IsDateAssigned(i))
        {
          if (blnInitRange)
          {
            blnInitRange = false;
            dgrDatesUnassigned.Items.Add(new ItemSeasonDates { Num = 1, Start = dtmStartRange, Finish = dtmEndRange });
          }
        }
        else
        {
          if (blnInitRange)
          {
            dtmEndRange = i;
          }
          else
          {
            blnInitRange = true;
            dtmStartRange = i;
            dtmEndRange = i;
          }
        }
      }

      if (blnInitRange)
      {
        dgrDatesUnassigned.Items.Add(new ItemSeasonDates { Num = 1, Start = dtmStartRange, Finish = dtmEndRange });
      }

      dgrDatesUnassigned.Items.Refresh();
    }

    #endregion

    #region IsDateAssigned
    /// <summary>
    ///   Determina si una fecha ya esta asignada a un rango de fechas
    /// </summary>
    /// <param name="dtmFecha">Fecha que se esta insertando</param>
    /// <history>
    ///   [vku] 27/Jul/2016 Created
    /// </history>
    private bool IsDateAssigned(DateTime dtmFecha)
    {
      bool blnAssigned = false;
      if(_lstSeasonDatesRange.Count > 0)
      {
        foreach (SeasonDate sd in _lstSeasonDatesRange)
        {
          if (sd.sdStartD <= dtmFecha && dtmFecha <= sd.sdEndD)
          {
            blnAssigned = true;
            break;
          }
        }
      }
      return blnAssigned;
    }
    #endregion

    #region ValidateRangeDates
    /// <summary>
    ///   Valida que el rango de fechas actual no se traslape consigo mismo o con otro del grid
    /// </summary>
    /// <param name="ssDate">Fecha que se esta insertando o editando</param>
    /// <param name="strColumn">Nombre de la propiedad bindiada</param>
    /// <param name="isNewItem">Si es una nueva fila</param>
    /// <param name="index">Numero de fila</param>
    /// <history>
    ///   [vku] 28/Jul/2016 Created
    /// </history>
    public void ValidateRangeDates(DateTime ssDate, string strColumn, bool isNewItem, int index)
    {
      if (strColumn == "sdEndD")
      {
        if (((SeasonDate)dgrDates.SelectedItem).sdStartD > ssDate)
        {
          isCancel = true;
          UIHelper.ShowMessage("Start Date can not be greater than End Date.", MessageBoxImage.Information, "IM.Administrator");
        }
      }
      if(!isCancel)
      {
        if ((strColumn == "sdStartD" && !isNewItem && ((SeasonDate)dgrDates.SelectedItem).sdStartD == ssDate))
          isCancel = true;
        else
        {
          if (strColumn == "sdEndD" && !isNewItem && ((SeasonDate)dgrDates.SelectedItem).sdEndD == ssDate)
          {
            isCancel = true;
          }
          else
          {
            int cont = 0;
            foreach (SeasonDate ssd in _lstSeasonDates)
            {
              if (ssd.sdStartD <= ssDate && ssDate <= ssd.sdEndD && index!=cont)
              {
                isCancel = true;
                UIHelper.ShowMessage("The date is in the range of dates (" + ssd.sdStartD.ToString("dd/MM/yyyy") + " to " + ssd.sdEndD.ToString("dd/MM/yyyy") + "). Specify another date.");
                changedTextBox.Text = string.Empty;
                break;
              }
              cont++;
            }
          }
        }
      }
    }
    #endregion

    #region ValidateData
    /// <summary>
    ///   Valida los datos de la fila del grid que se esta editando antes de hacer commit
    /// </summary>
    /// <param name="dg">DataGrid de fechas de temporada</param>
    /// <returns></returns>
    /// <history>
    ///   [vku] 04/Ago/2016 Created
    /// </history>
    private bool ValidateData(DataGrid dg)
    {
      bool blnValid = true;

      if (((SeasonDate)dg.SelectedItem).sdStartD == DateTime.MinValue)
        blnValid = false;
      else if (((SeasonDate)dg.SelectedItem).sdEndD == DateTime.MinValue)
        blnValid = false;
      return blnValid;
    }
    #endregion

    #region ValidateAllRangeDates
    /// <summary>
    ///   Valida todos datos del grid antes de guardar
    /// </summary>
    /// <returns></returns>
    /// <history>
    ///   [vku] 04/Ago/2016 Created
    /// </history>
    private bool ValidateAllRangeDates()
    {
      bool blnValid = true;
 
      foreach(SeasonDate sd in dgrDates.ItemsSource)
      {
        if (sd.sdStartD == DateTime.MinValue)
          blnValid = false;
        else if (sd.sdEndD == DateTime.MinValue)
          blnValid = false;
      }
      return blnValid;
    }

    #endregion

    #endregion

    #region Eventos
    #region btnAccept_Click
    /// <summary>
    ///   guarda o actualiza el registro dependiendo del modo en que se abrió
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 28/Jul/2016 Created
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (ValidateAllRangeDates())
      {
        List<SeasonDate> lstSeasonDates = (List<SeasonDate>)dgrDates.ItemsSource;
        if (ObjectHelper.IsEquals(season, oldSeason) && enumMode != EnumMode.Add && ObjectHelper.IsListEquals(_lstSeasonDates, _lstOldSeasonDates))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          string sMsj = ValidateHelper.ValidateForm(this, "Season");
          if (sMsj == "")
          {
            List<SeasonDate> lstAdd = lstSeasonDates.Where(sd => !_lstOldSeasonDates.Any(sdo => sdo.sdss == sd.sdss)).ToList();
            List<SeasonDate> lstDel = _lstOldSeasonDates.Where(sd => !lstSeasonDates.Any(sdo => sdo.sdss == sd.sdss && sdo.sdStartD == sd.sdStartD && sdo.sdEndD == sd.sdEndD)).ToList();
            List<SeasonDate> lstChanged = lstSeasonDates.Where(sd => !_lstOldSeasonDates.Any(sdo => sdo.sdss == sd.sdss && sdo.sdStartD == sd.sdStartD && sdo.sdEndD == sd.sdEndD)).ToList();
            int nRes = await BRSeasons.SaveSeason(season, (enumMode == EnumMode.Edit), lstAdd, lstDel, lstChanged);
            skpStatus.Visibility = Visibility.Collapsed;
            UIHelper.ShowMessageResult("Season", nRes);
            if (nRes > 0)
            {
              _isClosing = true;
              DialogResult = true;
              Close();
            }
          }
          else
          {
            UIHelper.ShowMessage(sMsj);
          }
        }
      }
      else
      {
        UIHelper.ShowMessage("Specify 'From' and 'To' for all range dates");
      }
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    ///   Cierra la ventana pero antes verifica que no se tengan cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 28/Jul/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      if (enumMode != EnumMode.ReadOnly)
      {
        if (!ObjectHelper.IsEquals(season, oldSeason))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            if (!_isClosing) { _isClosing = true; Close(); }
          }
          else
          {
            _isClosing = false;
          }
        }
        else
        {
          if (!_isClosing) { _isClosing = true; Close(); }
        }
      }
      else
      {
        if (!_isClosing) { _isClosing = true; Close(); }
      }
    }
    #endregion

    #region btnBack_Click
    /// <summary>
    ///   Retrocede un año para poder desplegar las fechas de una temporada de ese año
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 27/Jul/2016 Created
    /// </history>
    private void btnBack_Click(object sender, RoutedEventArgs e)
    {
      _year = _year.AddYears(-1);
      lblYear.Content = _year.Year;
      dgrDatesUnassigned.Items.Clear();
      dgrDatesUnassigned.Items.Refresh();
      LoadSeasonDates();
    }
    #endregion

    #region btnNext_Click
    /// <summary>
    ///   Avanza un año para poder desplegar las fechas de una temporada de ese año
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 27/Jul/2016 Created
    /// </history>
    private void btnNext_Click(object sender, RoutedEventArgs e)
    {
      _year =  _year.AddYears(+1);
      lblYear.Content = _year.Year;
      dgrDatesUnassigned.Items.Clear();
      dgrDatesUnassigned.Items.Refresh();
      LoadSeasonDates();
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    ///   Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 27/Jul/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(season, oldSeason);
      _year = BRHelpers.GetServerDate();
      lblYear.Content = _year.Year;
      LoadSeasonDates();
      if (enumMode != EnumMode.ReadOnly)
      {
        btnAccept.Visibility = Visibility.Visible;
        txtssID.IsEnabled = (enumMode == EnumMode.Add);
        txtDescrip.IsEnabled = true;
        txtClosFac.IsEnabled = true;
        chkActive.IsEnabled = true;
        UIHelper.SetUpControls(season, this);
      }
      DataContext = season;
      skpStatus.Visibility = Visibility.Collapsed;
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    ///  Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 28/Jul/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    ///   Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 28/Jul/2016 Created
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        _isClosing = true;
        btnCancel_Click(null, null);
        if (!_isClosing)
        {
          e.Cancel = true;
        }
        else
        {
          _isClosing = false;
        }
      }
    }
    #endregion

    #region dgrDates_CellEditEding
    /// <summary>
    ///   Valida las fechas ingresadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 28/Jul/2016 Created
    /// </history>
    private void dgrDates_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Cancel)
      {
        isCancel = true;
      }
      else
      {
        isCancel = false;
        changedTextBox = e.EditingElement as TextBox;
        if (changedTextBox.Text.ToString() != "")
        {
          if (ValidateHelper.IsDate(changedTextBox.Text.ToString()))
          {
            string ssd = changedTextBox.Text.ToString();
            DateTime ssDate = Convert.ToDateTime(ssd);
            if (ssDate.Year == _year.Year)
            {
              ValidateRangeDates(ssDate, e.Column.SortMemberPath.ToString(), e.Row.IsNewItem, Convert.ToInt32(e.Row.GetIndex().ToString()));
              if (!isCancel)
              {
                List<RangeDatesTraslape> lstRangeDates = new List<RangeDatesTraslape>();
                RangeDatesTraslape lstRangeTranslape = new RangeDatesTraslape();
                if (isEdit)
                {
                  lstRangeDates = BRSeasons.GetRangeDatesForValidateTraslapeIsEdit(ssDate, season.ssID);
                  lstRangeTranslape = lstRangeDates.Cast<RangeDatesTraslape>().FirstOrDefault();
                }
                else
                {
                  lstRangeDates = BRSeasons.GetRangeDatesForValidateTraslape(ssDate);
                  lstRangeTranslape = lstRangeDates.Cast<RangeDatesTraslape>().FirstOrDefault();
                }
                if (lstRangeDates.Count > 0)
                {
                  isCancel = true;

                  UIHelper.ShowMessage("The date is in the range of dates " + "(" + lstRangeTranslape.sdStartD.ToShortDateString() + " to " + lstRangeTranslape.sdEndD.ToShortDateString() + ")" + " of season " + "'"+lstRangeTranslape.ssN+"'" + ". " + "Specify another date.");
                  SeasonDate data = e.Row.DataContext as SeasonDate;
                  if (isEdit)
                  {
                    string strColumn = e.Column.SortMemberPath.ToString();
                    switch (strColumn)
                    {
                      case "sdStartD":
                        changedTextBox.Text = data.sdStartD.ToShortDateString();
                        break;
                      case "sdEndD":
                        changedTextBox.Text = data.sdEndD.ToShortDateString();
                        break;
                    }
                  }
                  else
                    changedTextBox.Text = string.Empty;
                }
                else
                {
                  GridHelper.UpdateSourceFromARow(sender as DataGrid);
                }
              }
            }
            else
            {
              isCancel = true;
              UIHelper.ShowMessage("The date does not belong to the year being edited " + _year.Year, MessageBoxImage.Exclamation, "IM.Administrator");
              changedTextBox.Text = string.Empty;
            }
          }
          else
          {
            isCancel = true;
            UIHelper.ShowMessage("Invalid Date", MessageBoxImage.Error, "IM.Administrator");
            changedTextBox.Text = string.Empty;
          }
        }
        else
        {
          if(e.Column.SortMemberPath == "sdEndD")
          {
            UIHelper.ShowMessage("Specify a Date", MessageBoxImage.Error, "IM.Administrator");
            e.Cancel = true;
          }
          else
          {
            isCancel = true;
          }
        }
      }
    }
    #endregion

    #region dgrDates_RowEditEnding
    /// <summary>
    ///   Actualiza la fila seleccionada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 28/Jul/2016 Created
    /// </history>
    private void dgrDates_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        dgrDates.RowEditEnding -= dgrDates_RowEditEnding;
        if (isCancel)
        {
          e.Cancel = true;
          dgrDates.CancelEdit();
        }
        else
        {
          DataGrid dg = sender as DataGrid;
          if (ValidateData(dg))
          {
            dgrDates.CommitEdit();
            dgrDates.Items.Refresh();
            GridHelper.SelectRow(dgrDates, dgrDates.SelectedIndex);
          }
          else
          {
            e.Cancel = true;
          }     
        }
        dgrDates.RowEditEnding += dgrDates_RowEditEnding;
      }
    }
    #endregion

    #region dgrDates_BeginningEdit
    /// <summary>
    ///   Determina si se puede editar la informacion del grid de fechas de temporadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 28/Jul/2016 Created
    /// </history>
    private void dgrDates_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if (!GridHelper.IsInEditMode(sender as DataGrid))
      {
        if (e.Column.SortMemberPath.ToString() == "sdEndD")
        {
          SeasonDate sd = (SeasonDate)dgrDates.SelectedItem;
          if (sd.sdStartD == DateTime.MinValue.Date)
          {
            e.Cancel = true;
            UIHelper.ShowMessage("Enter the 'From' date first.", MessageBoxImage.Exclamation, "IM.Administrator");
          }
        }
        if (!e.Row.IsNewItem)
        {
          isEdit = true;
        }
        else
        {
          isEdit = false;
        }
      }
      else
      {
        e.Cancel = true;
      }
    }
    #endregion

    #endregion
  }
}

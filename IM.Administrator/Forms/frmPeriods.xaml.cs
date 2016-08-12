using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPeriods.xaml
  /// </summary>
  public partial class frmPeriods : Window
  {
    #region Variables
    private Period _periodFilter = new Period();//objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//boleano para saber si se tiene permiso para editar
    #endregion
    public frmPeriods()
    {
      InitializeComponent();
    }

    #region Method Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(Model.Enums.EnumPermission.Sales, Model.Enums.EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadPeriods();
    } 
    #endregion

    #region Cell Double click
    /// <summary>
    /// Muestra la ventada detalle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/04/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Period period = (Period)dgrPeriods.SelectedItem;
      frmPeriodDetail frmPeriodDetail = new frmPeriodDetail();
      frmPeriodDetail.Owner = this;
      frmPeriodDetail.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly;
      frmPeriodDetail.oldPeriod = period;
      if(frmPeriodDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<Period> lstPeriods = (List<Period>)dgrPeriods.ItemsSource;
        if(ValidateFilter(frmPeriodDetail.period))//Verificamos si cumple con los filtros
        {
          ObjectHelper.CopyProperties(period, frmPeriodDetail.period);//Asignamos los nuevos valores
          lstPeriods.Sort((x, y) => string.Compare(x.pdN, y.pdN));//ordenamos la lista
          nIndex = lstPeriods.IndexOf(period);//Obtenemos la posición del registro
        }
        else
        {
          lstPeriods.Remove(period);//removemos el registro
        }
        dgrPeriods.Items.Refresh();//actualizamos la vista
        GridHelper.SelectRow(dgrPeriods, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstPeriods.Count + "Periods.";//Actualizamos el contador
      }
    }

    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [created] 07/04/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            Cell_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }

      e.Handled = blnHandled;
    }

    #endregion

    #region KeyBoardFocuChange
    /// <summary>
    /// Verifica las teclas presinadas al cambio de ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/04/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region window Key Down
    /// <summary>
    /// Verifica las teclas presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.strID = _periodFilter.pdID;
      frmSearch.strDesc = _periodFilter.pdN;
      frmSearch.nStatus = _nStatus;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _periodFilter.pdID = frmSearch.strID;
        _periodFilter.pdN = frmSearch.strDesc;
        LoadPeriods();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmPeriodDetail frmPeriodDetail = new frmPeriodDetail();
      frmPeriodDetail.Owner = this;
      frmPeriodDetail.enumMode = EnumMode.Add;
      if(frmPeriodDetail.ShowDialog()==true)
      {
        Period period = frmPeriodDetail.period;
        if(ValidateFilter(period))//Verificamos que cumpla con los filtros actuales
        {
          List<Period> lstPeriod = (List<Period>)dgrPeriods.ItemsSource;
          lstPeriod.Add(period);//Agregamos el registro a la lista
          lstPeriod.Sort((x, y) => string.Compare(x.pdN, y.pdN));//ordenamos la lista
          int nIndex = lstPeriod.IndexOf(period);//obtenemos la posición del nuevo registro
          dgrPeriods.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrPeriods, nIndex);//Seleccionamos la lista
          StatusBarReg.Content = lstPeriod.Count + " Periods.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Period period = (Period)dgrPeriods.SelectedItem;
      LoadPeriods(period);
    } 
    #endregion
    #endregion
    #region Methods
    #region LoadPeriods
    /// <summary>
    /// Llena el grid de periods
    /// </summary>
    /// <param name="period">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private async void LoadPeriods(Period period = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Period> lstPeriods =await BRPeriods.GetPeriods(_nStatus, _periodFilter);
        dgrPeriods.ItemsSource = lstPeriods;
        if (lstPeriods.Count > 0 && period != null)
        {
          period = lstPeriods.Where(pd => pd.pdID == period.pdID).FirstOrDefault();
          nIndex = lstPeriods.IndexOf(period);
        }
        GridHelper.SelectRow(dgrPeriods, nIndex);
        StatusBarReg.Content = lstPeriods.Count + " Periods.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Periods");
      }
    }
    #endregion

    #region validateFilter
    /// <summary>
    /// Valida que un registro cumpla con los filtros actuales
    /// </summary>
    /// <param name="period">Objeto a validar</param>
    /// <history>
    /// [emoguel] created 07/04/2016
    /// </history>
    /// <returns></returns>
    private bool ValidateFilter(Period period)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(period.pdA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_periodFilter.pdID))//Filtro por ID
      {
        if(period.pdID!=_periodFilter.pdID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_periodFilter.pdN))//Filtro por Descripcion
      {
        if(!period.pdN.Contains(_periodFilter.pdN,StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }
      return true;
    }
    #endregion
    #endregion
  }
}

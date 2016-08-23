using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmMaritalStatus.xaml
  /// </summary>
  public partial class frmMaritalStatus : Window
  {
    #region Variable
    private MaritalStatus _MaritaStaFilter = new MaritalStatus();//Objeto con los filtros de la ventana
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para edita|agregar
    #endregion
    public frmMaritalStatus()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
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

    #region DoubleClick cell
    /// <summary>
    /// Muestra la ventana detalle en modo ReadOnly|edicion
    /// </summary>
    /// <history>
    /// [emoguel] 01/04/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      MaritalStatus maritalStatus = (MaritalStatus)dgrMaritalStatus.SelectedItem;
      frmMaritalStatusDetail frmMarStatus = new frmMaritalStatusDetail();
      frmMarStatus.Owner = this;
      frmMarStatus.oldMaritalStatus = maritalStatus;
      frmMarStatus.enumMode = (_blnEdit==true)?EnumMode.Edit:EnumMode.ReadOnly;
      if(frmMarStatus.ShowDialog()==true)
      {
        int nIndex = 0;
        List<MaritalStatus> lstMaritalStatus = (List<MaritalStatus>)dgrMaritalStatus.ItemsSource;
        if(!ValidateFilter(frmMarStatus.maritalStatus))
        {
          lstMaritalStatus.Remove(maritalStatus);//Quitamos el registro de la lista
          StatusBarReg.Content = lstMaritalStatus.Count+" Marital Status.";
        }
        else
        {
          ObjectHelper.CopyProperties(maritalStatus, frmMarStatus.maritalStatus);//Le asignamos los nuevos valores
          lstMaritalStatus.Sort((x, y) => string.Compare(x.msN, y.msN));//ordenamos la lista
          nIndex = lstMaritalStatus.IndexOf(maritalStatus);//obtenemos la posicion del registro
        }
        dgrMaritalStatus.Items.Refresh();
        GridHelper.SelectRow(dgrMaritalStatus, nIndex);
      }
    }

    #endregion

    #region Window KeyDown
    /// <summary>
    /// Verifica botones presionados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
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

    #region IsKeyboardFocuChanged
    /// <summary>
    /// Verifica que botones fueron presionados con la ventana inactiva
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/06
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.MaritalStatus, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadMaritalStatus();
    }

    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.nStatus = _nStatus;
      frmSearch.strID = _MaritaStaFilter.msID;
      frmSearch.strDesc = _MaritaStaFilter.msN;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _MaritaStaFilter.msID = frmSearch.strID;
        _MaritaStaFilter.msN = frmSearch.strDesc;
        LoadMaritalStatus();
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
    /// [emoguel] created 01/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmMaritalStatusDetail frmMarStatusDet = new frmMaritalStatusDetail();
      frmMarStatusDet.Owner = this;
      frmMarStatusDet.enumMode = EnumMode.Add;
      if(frmMarStatusDet.ShowDialog()==true)
      {
        MaritalStatus maritalStatus = frmMarStatusDet.maritalStatus;
        if (ValidateFilter(maritalStatus))//validamos que cumpla con los filtros
        {
          List<MaritalStatus> lstMaritalStatus = (List<MaritalStatus>)dgrMaritalStatus.ItemsSource;
          lstMaritalStatus.Add(maritalStatus);//Agregamos el registro a la lista
          lstMaritalStatus.Sort((x, y) => string.Compare(x.msN, y.msN));//ordenamos la lista
          int nIndex = lstMaritalStatus.IndexOf(maritalStatus);//Obtenemos la posicion del nuevo registro
          dgrMaritalStatus.Items.Refresh();//Actualizamos la vista del grid
          GridHelper.SelectRow(dgrMaritalStatus, nIndex);//Seleccionamos el nuevo registro
          StatusBarReg.Content = lstMaritalStatus.Count + " Marital Status.";
        }
      }
    } 
    #endregion

    #region Refresh
    /// <summary>
    /// actualiza el grid de Marital Status
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      MaritalStatus maritalStatus = (MaritalStatus)dgrMaritalStatus.SelectedItem;
      LoadMaritalStatus(maritalStatus);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadMaritalStatus
    /// <summary>
    /// Llena el grid de Marital status
    /// </summary>
    /// <param name="maritalStatus">Objeto para seleccionar con el recharge</param>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadMaritalStatus(MaritalStatus maritalStatus = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<MaritalStatus> lstMaritalStatus = await BRMaritalStatus.GetMaritalStatus(_nStatus, _MaritaStaFilter);
        dgrMaritalStatus.ItemsSource = lstMaritalStatus;
        if (lstMaritalStatus.Count > 0 && maritalStatus != null)
        {
          maritalStatus = lstMaritalStatus.Where(ms => ms.msID == maritalStatus.msID).FirstOrDefault();
          nIndex = lstMaritalStatus.IndexOf(maritalStatus);
        }
        GridHelper.SelectRow(dgrMaritalStatus, nIndex);
        StatusBarReg.Content = lstMaritalStatus.Count + " Marital Status.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Marital Status");
      }
    }
    #endregion

    #region validateFilter
    /// <summary>
    /// Valida que un registro cumpla con los filtros actuales
    /// </summary>
    /// <param name="maritalStatus">objeto a valida</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 01/040/2016
    /// </history>
    private bool ValidateFilter(MaritalStatus maritalStatus)
    {
      if(_nStatus!=-1)//Filtro por Estatus
      {
        if(maritalStatus.msA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_MaritaStaFilter.msID))//Filtro por ID
      {
        if(_MaritaStaFilter.msID!=maritalStatus.msID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_MaritaStaFilter.msN))//Filtro por descripcion
      {
        if(!_MaritaStaFilter.msN.Contains(maritalStatus.msN,StringComparison.OrdinalIgnoreCase))
        {
          return true;
        }
      }

      return true;
    }
    #endregion
    #endregion
  }
}

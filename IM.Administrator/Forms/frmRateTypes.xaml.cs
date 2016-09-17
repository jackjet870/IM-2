using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmRateTypes.xaml
  /// </summary>
  public partial class frmRateTypes : Window
  {
    #region Variables
    private RateType _rateTypeFilter = new RateType();//Objeto con los filtros adicionales
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion
    public frmRateTypes()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 13/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadRateTypes();
    } 
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 14/04/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region KeyDown Form
    /// <summary>
    /// Verfica teclas precionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
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

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
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
    /// Muestra la ventana detalle en modo edit
    /// </summary>
    /// <history>
    /// [emoguel] 14/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      RateType rateType = (RateType)dgrRateTypes.SelectedItem;
      frmRateTypeDetail frmRateTypeDetail = new frmRateTypeDetail();
      frmRateTypeDetail.Owner = this;
      frmRateTypeDetail.enumMode = EnumMode.Edit;
      frmRateTypeDetail.oldRateType = rateType;
      if (frmRateTypeDetail.ShowDialog() == true)
      {
        int nIndex = 0;
        List<RateType> lstRateTypes = (List<RateType>)dgrRateTypes.ItemsSource;
        if (ValidateFilter(frmRateTypeDetail.rateType))
        {
          ObjectHelper.CopyProperties(rateType, frmRateTypeDetail.rateType);//Actualizamos los datos del registro
          lstRateTypes.Sort((x, y) => string.Compare(x.raN, y.raN));//Ordenamos los registros
          nIndex=lstRateTypes.IndexOf(rateType);//obtenemos la posición del registro          
        }
        else
        {
          lstRateTypes.Remove(rateType);//Eliminamos el registro
        }
        dgrRateTypes.Items.Refresh();
        GridHelper.SelectRow(dgrRateTypes, nIndex);
        StatusBarReg.Content = lstRateTypes.Count + " Rate Types.";
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
    /// [emoguel] created 13/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.enumWindow = EnumWindow.DefaultInt;
      frmSearch.strID = (_rateTypeFilter.raID > 0) ? _rateTypeFilter.ToString() : "";
      frmSearch.strDesc = _rateTypeFilter.raN;
      frmSearch.nStatus = _nStatus;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _rateTypeFilter.raID = Convert.ToInt32(frmSearch.strID);
        _rateTypeFilter.raN = frmSearch.strDesc;
        LoadRateTypes();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en Modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmRateTypeDetail frmRateTypeDetail = new frmRateTypeDetail();
      frmRateTypeDetail.Owner = this;
      frmRateTypeDetail.enumMode = EnumMode.Add;
      if(frmRateTypeDetail.ShowDialog()==true)
      {
        RateType rateType = frmRateTypeDetail.rateType;
        if(ValidateFilter(rateType))
        {
          List<RateType> lstRateTypes = (List<RateType>)dgrRateTypes.ItemsSource;
          lstRateTypes.Add(rateType);//Agregamos el registro a la lista
          lstRateTypes.Sort((x, y) => string.Compare(x.raN, y.raN));//Ordenamos la lista
          int nIndex = lstRateTypes.IndexOf(rateType);//Obtenemos el index del nuevo registro
          dgrRateTypes.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrRateTypes, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstRateTypes.Count + " Rate Types.";//Actualizamos el contador
        }
      }
    } 
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los registros del grid de Rate Types
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 13/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      RateType rateType = (RateType)dgrRateTypes.SelectedItem;
      LoadRateTypes(rateType);
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadRateTypes
    /// <summary>
    /// Llena el grid de Rate Types
    /// </summary>
    /// <param name="ratType">Objeto a seleccionar en el grid</param>
    /// <history>
    /// [emoguel] created 13/04/2016
    /// [edgrodriguez] 21/05/2016 Modified. El método GetRateTypes se volvió asincrónico.
    /// </history>
    private async void LoadRateTypes(RateType rateType = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<RateType> lstRateTypes = await BRRateTypes.GetRateTypes(_rateTypeFilter, _nStatus, orderByraN: true);
        dgrRateTypes.ItemsSource = lstRateTypes;
        if (lstRateTypes.Count > 0 && rateType != null)
        {
          rateType = lstRateTypes.Where(ra => ra.raID == rateType.raID).FirstOrDefault();
          nIndex = lstRateTypes.IndexOf(rateType);
        }
        GridHelper.SelectRow(dgrRateTypes, nIndex);
        StatusBarReg.Content = lstRateTypes.Count + " Rate Types.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un objeto tipo Rate Type cumpla con los filtros actuales
    /// </summary>
    /// <param name="rateType">objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 13/04/2016
    /// </history>
    private bool ValidateFilter(RateType rateType)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(rateType.raA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(_rateTypeFilter.raID>0)//Filtro por ID
      {
        if(_rateTypeFilter.raID!=rateType.raID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_rateTypeFilter.raN))//Filtro por estatus
      {
        if(!rateType.raN.Contains(_rateTypeFilter.raN,StringComparison.OrdinalIgnoreCase))
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

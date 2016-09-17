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
  /// Interaction logic for frmRefundTypes.xaml
  /// </summary>
  public partial class frmRefundTypes : Window
  {
    #region Variables
    private RefundType _refundTypeFilter = new RefundType();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion
    public frmRefundTypes()
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
    /// [emoguel] created 14/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadRefundTypes();
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
      RefundType refunType = (RefundType)dgrRefundTypes.SelectedItem;
      frmRefundTypeDetail frmRefundTypeDetail = new frmRefundTypeDetail();
      frmRefundTypeDetail.Owner = this;
      frmRefundTypeDetail.enumMode = EnumMode.Edit;
      frmRefundTypeDetail.oldRefundType = refunType;
      if(frmRefundTypeDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<RefundType> lstRefundTypes = (List<RefundType>)dgrRefundTypes.ItemsSource;
        if(ValidateFilter(frmRefundTypeDetail.refundType))//Validamos que cumpla con los filtros actuales
        {
          ObjectHelper.CopyProperties(refunType, frmRefundTypeDetail.refundType);//Actualizamos los datos
          lstRefundTypes.Sort((x, y) => string.Compare(x.rfN, y.rfN));//ordenamos la lista
          nIndex = lstRefundTypes.IndexOf(refunType);//Obtenemos la posición del registro
        }
        else
        {
          lstRefundTypes.Remove(refunType);//Quitamos el registro de la lista
        }
        dgrRefundTypes.Items.Refresh();//Actualizamos la lista
        GridHelper.SelectRow(dgrRefundTypes, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstRefundTypes.Count + " Refund Types.";//Actualizamos el contador
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
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _refundTypeFilter.rfID;
      frmSearch.strDesc = _refundTypeFilter.rfN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _refundTypeFilter.rfID = frmSearch.strID;
        _refundTypeFilter.rfN = frmSearch.strDesc;
        LoadRefundTypes();
      }
    } 
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      RefundType refundType = (RefundType)dgrRefundTypes.SelectedItem;
      LoadRefundTypes(refundType);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmRefundTypeDetail frmRefundTypeDetail = new frmRefundTypeDetail();
      frmRefundTypeDetail.Owner = this;
      frmRefundTypeDetail.enumMode = EnumMode.Add;
      if(frmRefundTypeDetail.ShowDialog()==true)
      {
        RefundType refundType = frmRefundTypeDetail.refundType;
        if(ValidateFilter(refundType))//Validamos si cumple con los filtros actualess
        {
          List<RefundType> lstRefundTypes = (List<RefundType>)dgrRefundTypes.ItemsSource;
          lstRefundTypes.Add(refundType);//Agregamos el registro a la lista
          lstRefundTypes.Sort((x, y) => string.Compare(x.rfN, y.rfN));//ordenamos la lista
          int nIndex = lstRefundTypes.IndexOf(refundType);//Obtenemos la posicion del registro
          dgrRefundTypes.Items.Refresh();//Actualizamos la vista del grid
          GridHelper.SelectRow(dgrRefundTypes, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstRefundTypes.Count + " Refun Types.";//Actualizamos el contador
        }
      }
    } 
    #endregion
    #endregion


    #region Methods
    #region LoadRefundTypes
    /// <summary>
    /// Llena el grid de Refund Types
    /// </summary>
    /// <param name="refundType">objeto a seleccionar</param>
    /// <history>
    /// [emoguel[] created 14/04/2016
    /// </history>
    private async void LoadRefundTypes(RefundType refundType = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<RefundType> lstRefundTypes =await BRRefundTypes.GetRefundTypes(_nStatus, _refundTypeFilter);
        dgrRefundTypes.ItemsSource = lstRefundTypes;
        if (lstRefundTypes.Count > 0 && refundType != null)
        {
          refundType = lstRefundTypes.Where(rf => rf.rfID == refundType.rfID).FirstOrDefault();
          nIndex = lstRefundTypes.IndexOf(refundType);
        }
        GridHelper.SelectRow(dgrRefundTypes, nIndex);
        StatusBarReg.Content = lstRefundTypes.Count + " Refund Types.";
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
    /// Valida que un objeto refund type cumpla con los filtros actuales
    /// </summary>
    /// <param name="refundType">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private bool ValidateFilter(RefundType refundType)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(refundType.rfA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_refundTypeFilter.rfID))//filtro por ID
      {
        if(_refundTypeFilter.rfID!=refundType.rfID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_refundTypeFilter.rfN))//Filtro por descripción
      {
        if(!refundType.rfN.Contains(_refundTypeFilter.rfN,StringComparison.OrdinalIgnoreCase))
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

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
  /// Interaction logic for frmSaleTypes.xaml
  /// </summary>
  public partial class frmSaleTypes : Window
  {
    #region Variables
    private SaleType _saletTypeFilter = new SaleType {ststc="" };//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion
    public frmSaleTypes()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 19/04/2016
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
    /// [emoguel] created 19/04/2016
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
    /// [emoguel] created 19/04/2016
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
    /// [emoguel] 19/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      SaleType saleType = (SaleType)dgrSaleTypes.SelectedItem;
      frmSaleTypeDetail frmSaleTypeDetail = new frmSaleTypeDetail();
      frmSaleTypeDetail.Owner = this;
      frmSaleTypeDetail.oldSaleType = saleType;
      frmSaleTypeDetail.enumMode = EnumMode.edit;
      if(frmSaleTypeDetail.ShowDialog()==true)
      {
        List<SaleType> lstSaleTypes = (List<SaleType>)dgrSaleTypes.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmSaleTypeDetail.saleType))//Validamos que cumpla con los filtros actuales
        {
          ObjectHelper.CopyProperties(saleType, frmSaleTypeDetail.saleType);//Actualizamos los datos del objeto
          lstSaleTypes.Sort((x, y) => string.Compare(x.stN, y.stN));//Ordenamos la lista
          nIndex = lstSaleTypes.IndexOf(saleType);//Obtenemos 
        }
        else
        {
          lstSaleTypes.Remove(saleType);//Quitamos el registro
        }
        dgrSaleTypes.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrSaleTypes, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstSaleTypes.Count + " Sale Types.";//Actualizamos al contador
      }
    }
    #endregion

    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadSalesTypes();
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana detalle en modo Busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSaleTypeDetail frmSaleTypeDetail = new frmSaleTypeDetail();
      frmSaleTypeDetail.Owner = this;
      frmSaleTypeDetail.enumMode = EnumMode.search;
      frmSaleTypeDetail.oldSaleType = _saletTypeFilter;
      frmSaleTypeDetail.nStatus = _nStatus;
      if(frmSaleTypeDetail.ShowDialog()==true)
      {
        _saletTypeFilter = frmSaleTypeDetail.saleType;
        _nStatus = frmSaleTypeDetail.nStatus;
        LoadSalesTypes();
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
    /// [emoguel] created 19/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmSaleTypeDetail frmSaleTypeDetail = new frmSaleTypeDetail();
      frmSaleTypeDetail.Owner = this;
      frmSaleTypeDetail.enumMode = EnumMode.add;
      if(frmSaleTypeDetail.ShowDialog()==true)
      {
        SaleType saleType = frmSaleTypeDetail.saleType;
        if(ValidateFilter(saleType))//Validamos que cumpla con los filtros actuales
        {
          List<SaleType> lstSaleTypes = (List<SaleType>)dgrSaleTypes.ItemsSource;
          lstSaleTypes.Add(saleType);//Agregamos el objeto a la lista
          lstSaleTypes.Sort((x, y) => string.Compare(x.stN, y.stN));//Ordenamos la lista
          int nIndex = lstSaleTypes.IndexOf(saleType);//Obtenemos la posición del registro
          dgrSaleTypes.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrSaleTypes, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstSaleTypes.Count + " Sale Types.";//Actualizamos el contador
        }
      }
    } 
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      SaleType saleType = (SaleType)dgrSaleTypes.SelectedItem;
      LoadSalesTypes(saleType);
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadSalesTypes
    /// <summary>
    /// Llena el grid de salesTypes
    /// </summary>
    /// <param name="saleType">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private async void LoadSalesTypes(SaleType saleType = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<SaleType> lstSalestypes = await BRSaleTypes.GetSalesTypes(_nStatus, _saletTypeFilter);
        dgrSaleTypes.ItemsSource = lstSalestypes;
        if (lstSalestypes.Count > 0 && saleType != null)
        {
          saleType = lstSalestypes.Where(st => st.stID == saleType.stID).FirstOrDefault();
          nIndex = lstSalestypes.IndexOf(saleType);
        }

        GridHelper.SelectRow(dgrSaleTypes, nIndex);
        StatusBarReg.Content = lstSalestypes.Count + " Sale Types.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Sale Types");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Verifica que un saleType cumpla con los filtros actuales
    /// </summary>
    /// <param name="saleType">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 19/040/2016
    /// </history>
    private bool ValidateFilter(SaleType saleType)
    {
      if(_nStatus!=-1)//filtro por estatus
      {
        if(saleType.stA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_saletTypeFilter.stID))//Filtro por ID
      {
        if(_saletTypeFilter.stID!=saleType.stID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_saletTypeFilter.stN))//Filtro por Descripción
      {
        if(!saleType.stN.Contains(_saletTypeFilter.stN,StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_saletTypeFilter.ststc))//Filtro por Categoria
      {
        if (saleType.ststc != _saletTypeFilter.ststc)
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

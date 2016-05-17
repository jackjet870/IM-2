using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSalesAmountRanges.xaml
  /// </summary>
  public partial class frmSalesAmountRanges : Window
  {
    #region Variables 
    private SalesAmountRange _salesAmountRangeFilter = new SalesAmountRange();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los filtros del grid
    #endregion
    public frmSalesAmountRanges()
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
    /// [emoguel] created 20/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadSaleAmountRange();
    }
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 20/04/2016
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
    /// [emoguel] created 20/04/2016
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
    /// [emoguel] created 20/04/2016
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
    /// [emoguel] 20/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      SalesAmountRange salesAmountRange = (SalesAmountRange)dgrSalesAmountRanges.SelectedItem;
      frmSalesAmountRangeDetail frmSalAmoRanDetail = new frmSalesAmountRangeDetail();
      frmSalAmoRanDetail.Owner = this;
      frmSalAmoRanDetail.oldSalAmoRan = salesAmountRange;
      frmSalAmoRanDetail.enumMode = EnumMode.edit;
      if(frmSalAmoRanDetail.ShowDialog()==true)
      {
        List<SalesAmountRange> lstSalesAmountRange = (List<SalesAmountRange>)dgrSalesAmountRanges.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmSalAmoRanDetail.salesAmountRange))
        {
          ObjectHelper.CopyProperties(salesAmountRange, frmSalAmoRanDetail.salesAmountRange);//Actualizamos los datos del registro
          lstSalesAmountRange.Sort((x, y) => string.Compare(x.snN, y.snN));//Ordenamos la lista
          nIndex = lstSalesAmountRange.IndexOf(salesAmountRange);//Buscamos la posicion del registro
        }
        else
        {
          lstSalesAmountRange.Remove(salesAmountRange);//Eliminamos el registro de la lista
        }
        dgrSalesAmountRanges.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrSalesAmountRanges, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstSalesAmountRange.Count + " Sales Amount Range.";//Actualizamos el contador
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana detalle en modo busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSalesAmountRangeDetail frmSalAmoRanDetail = new frmSalesAmountRangeDetail();
      frmSalAmoRanDetail.Owner = this;
      frmSalAmoRanDetail.enumMode = EnumMode.search;
      frmSalAmoRanDetail.oldSalAmoRan = _salesAmountRangeFilter;
      frmSalAmoRanDetail.nStatus = _nStatus;
      if (frmSalAmoRanDetail.ShowDialog()==true)
      {
        _salesAmountRangeFilter = frmSalAmoRanDetail.salesAmountRange;
        _nStatus = frmSalAmoRanDetail.nStatus;
        LoadSaleAmountRange();
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
    /// [emoguel] created 20/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmSalesAmountRangeDetail frmSalAmoRanDetail = new frmSalesAmountRangeDetail();
      frmSalAmoRanDetail.Owner = this;
      frmSalAmoRanDetail.enumMode = EnumMode.add;
      if(frmSalAmoRanDetail.ShowDialog()==true)
      {
        SalesAmountRange salesAmountRange = frmSalAmoRanDetail.salesAmountRange;
        if(ValidateFilter(salesAmountRange))//Validamos que cumpla con los filtros
        {
          List<SalesAmountRange> lstSalesAmountRange = (List<SalesAmountRange>)dgrSalesAmountRanges.ItemsSource;
          lstSalesAmountRange.Add(salesAmountRange);//Agregamos el registro a la lista
          lstSalesAmountRange.Sort((x, y) => string.Compare(x.snN, y.snN));//ordenamos la lista
          int nIndex = lstSalesAmountRange.IndexOf(salesAmountRange);//Buscamos la posición del registro
          dgrSalesAmountRanges.Items.Refresh();//Actualizamos la vista del grid
          GridHelper.SelectRow(dgrSalesAmountRanges, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstSalesAmountRange.Count + " Sales Amount Ranges.";//Actualizamos el contador

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
    /// <history>
    /// [emoguel] created 20/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      SalesAmountRange salesAmountRange = (SalesAmountRange)dgrSalesAmountRanges.SelectedItem;
      LoadSaleAmountRange(salesAmountRange);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadSaleAmountRange
    /// <summary>
    /// Llena el grid SalesAmoutRanges
    /// </summary>
    /// <param name="salesAmountRange">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 20/04/2016
    /// </history>
    private void LoadSaleAmountRange(SalesAmountRange salesAmountRange = null)
    {
      int nIndex = 0;
      List<SalesAmountRange> lstSalesAmountRange = BRSaleAmountRanges.GetSalesAmountRanges(_nStatus, _salesAmountRangeFilter);
      dgrSalesAmountRanges.ItemsSource = lstSalesAmountRange;
      if (lstSalesAmountRange.Count > 0 && salesAmountRange != null)
      {
        salesAmountRange = lstSalesAmountRange.Where(sn => sn.snID == salesAmountRange.snID).FirstOrDefault();
        nIndex = lstSalesAmountRange.IndexOf(salesAmountRange);
      }
      GridHelper.SelectRow(dgrSalesAmountRanges, nIndex);
      StatusBarReg.Content = lstSalesAmountRange.Count + " Sales Amount Ranges.";
    }
    #endregion
    #region ValidateFilter
    /// <summary>
    /// Valida que un objeto SalesAMountRange cumpla con los filtros actuales
    /// </summary>
    /// <param name="salesAmountRange"></param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 21/04/2016
    /// </history>
    private bool ValidateFilter(SalesAmountRange salesAmountRange)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(salesAmountRange.snA!=Convert.ToBoolean(salesAmountRange))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_salesAmountRangeFilter.snN))//Filtro por Descripción
      {
        if(!salesAmountRange.snN.Contains(_salesAmountRangeFilter.snN))
        {
          return false;
        }
      }

      if(_salesAmountRangeFilter.snID>0)//Filtro por ID
      {
        if(_salesAmountRangeFilter.snID!=salesAmountRange.snID)
        {
          return false;
        }
      }

      if(_salesAmountRangeFilter.snFrom>0 && _salesAmountRangeFilter.snTo>0)//Filtro por Rango
      {
        if (_salesAmountRangeFilter.snFrom == _salesAmountRangeFilter.snTo)
        {
          if (!(_salesAmountRangeFilter.snFrom >= salesAmountRange.snFrom && _salesAmountRangeFilter.snTo <= _salesAmountRangeFilter.snTo))
          {
            return false;
          }
        }
        else
        {
          if (!(salesAmountRange.snFrom >= _salesAmountRangeFilter.snFrom && salesAmountRange.snTo <= _salesAmountRangeFilter.snTo))
          {
            return false;
          }
        }
      }
      return true;
    }
    #endregion
    #endregion
  }
}

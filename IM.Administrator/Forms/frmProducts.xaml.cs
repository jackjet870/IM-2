using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Extensions;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmProducts.xaml
  /// </summary>
  public partial class frmProducts : Window
  {
    #region Variables
    private Product _productFilter = new Product();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Sirve para saber si se tiene permiso para editar|guardar
    #endregion
    public frmProducts()
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
    /// [emoguel] created 20/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      status.Visibility = Visibility.Visible;
      _blnEdit = App.User.HasPermission(EnumPermission.Gifts, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadProducts();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/05/2016
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
    /// [emoguel] created 20/05/2016
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
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/05/2016
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

    #region DobleClic Grid
    /// <summary>
    /// Abre la ventana detalle en modo "detalle" o "edición" dependiendo de sus permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/05/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Product product = (Product)dgrProducts.SelectedItem;
      frmProductDetail frmProductDetail = new frmProductDetail();
      frmProductDetail.Owner = this;
      frmProductDetail.enumMode = (_blnEdit) ? EnumMode.edit : EnumMode.preview;
      frmProductDetail.oldProduct = product;
      if (frmProductDetail.ShowDialog() == true)
      {
        int nIndex = 0;
        List<Product> lstproducts = (List<Product>)dgrProducts.ItemsSource;
        if(ValidateFilter(frmProductDetail.product))//Verificar que cumpla con los filtros actuales
        {
          ObjectHelper.CopyProperties(product, frmProductDetail.product);//Actualizar los datos
          lstproducts.Sort((x, y) => string.Compare(x.prN, y.prN));//Actualizar la vista
          nIndex = lstproducts.IndexOf(product);//Buscamos la posición del registro
        }
        else
        {
          lstproducts.Remove(product);//Quuitar el registro
        }
        dgrProducts.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrProducts, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstproducts.Count + " Products";//Actualizamos el contador
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
    /// [emoguel] created 20/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      status.Visibility = Visibility.Visible;
      Product product = (Product)dgrProducts.SelectedItem;
      LoadProducts(product);
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/05/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _productFilter.prID;
      frmSearch.strDesc = _productFilter.prN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _productFilter.prID = frmSearch.strID;
        _productFilter.prN = frmSearch.strDesc;
        LoadProducts();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmProductDetail frmProductDetail = new frmProductDetail();
      frmProductDetail.enumMode = EnumMode.add;
      frmProductDetail.Owner = this;
      if(frmProductDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmProductDetail.product))
        {
          List<Product> lstProducts = (List<Product>)dgrProducts.ItemsSource;
          lstProducts.Add(frmProductDetail.product);//Agregar el registro a la lista
          lstProducts.Sort((x, y) => string.Compare(x.prN, y.prN));//Ordenamos la lista
          int nIndex = lstProducts.IndexOf(frmProductDetail.product);//Obtenemos la posición del registro
          dgrProducts.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrProducts, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstProducts.Count + " Products";//Actualizamos el contador
        }
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadProducts
    /// <summary>
    /// Llena el grid de products
    /// </summary>
    /// <param name="product">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 20/05/2016
    /// </history>
    private async void LoadProducts(Product product = null)
    {
      try
      {
        int nIndex = 0;
        List<Product> lstProducts = await BRProducts.GetProducts(_nStatus, _productFilter);
        dgrProducts.ItemsSource = lstProducts;
        if (lstProducts.Count > 0 && product != null)
        {
          product = lstProducts.Where(pr => pr.prID == product.prID).FirstOrDefault();
          nIndex = lstProducts.IndexOf(product);
        }
        GridHelper.SelectRow(dgrProducts, nIndex);
        StatusBarReg.Content = lstProducts.Count + " Products.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error,"Products");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Verifica que nu Product cumpla con los filtros actuales
    /// </summary>
    /// <param name="product">producto a validar</param>
    /// <returns>True. Si cumpple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 23/05/2016
    /// </history>
    private bool ValidateFilter(Product product)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(product.prA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_productFilter.prID))//Filtro por ID
      {
        if(_productFilter.prID!=product.prID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_productFilter.prN))//Filtro por Descripción
      {
        if (!product.prN.Contains(_productFilter.prN, StringComparison.OrdinalIgnoreCase))
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

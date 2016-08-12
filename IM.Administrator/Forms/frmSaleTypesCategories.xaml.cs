using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Extensions;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSaleTypesCategories.xaml
  /// </summary>
  public partial class frmSaleTypesCategories : Window
  {
    #region Variables
    private SaleTypeCategory _saleTypeCategoryFilter = new SaleTypeCategory();
    private int _nStatus = -1;
    #endregion
    public frmSaleTypesCategories()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window_Loaded
    /// <summary>
    /// Carga los datos iniciales de la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 30/07/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadSaleTypesCategories();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <history>
    /// [emoguel] created 30/07/2016
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
    /// <history>
    /// [emoguel] created 30/07/2016
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
    /// <history>
    /// [emoguel] created 30/07/2016
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
    /// <history>
    /// [emoguel] created 30/07/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      frmSaleTypeCategoryDetail frmSaleTypeCategory = new frmSaleTypeCategoryDetail();
      frmSaleTypeCategory.Owner = this;
      frmSaleTypeCategory.enumMode = EnumMode.Edit;
      SaleTypeCategory saleTypeCategory=dgrSaleTypesCategories.SelectedItem as SaleTypeCategory;
      frmSaleTypeCategory.oldSaleTypeCategory = saleTypeCategory;
      if(frmSaleTypeCategory.ShowDialog()==true)
      {
        List<SaleTypeCategory> lstSaleTypes = dgrSaleTypesCategories.ItemsSource as List<SaleTypeCategory>;
        int nIndex = 0;
        if(ValidateFilter(frmSaleTypeCategory.saleTypeCategory))
        {
          ObjectHelper.CopyProperties(saleTypeCategory, frmSaleTypeCategory.saleTypeCategory);//Actualizamos los datos
          lstSaleTypes.Sort((x, y) => string.Compare(x.stcN, y.stcN));//Ordenamos la lista
          nIndex = lstSaleTypes.IndexOf(saleTypeCategory);//Buscamos la posición
        }
        else
        {
          lstSaleTypes.Remove(saleTypeCategory);
        }
        dgrSaleTypesCategories.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrSaleTypesCategories, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = "Categories (" + lstSaleTypes.Count + ")";//Actualizamos el contador
      }
    }
    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <history>
    /// [emoguel] created 30/07/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.strID = _saleTypeCategoryFilter.stcID;
      frmSearch.strDesc = _saleTypeCategoryFilter.stcN;
      frmSearch.nStatus = _nStatus;
      frmSearch.enumWindow = EnumWindow.Default;
      frmSearch.Owner = this;
      if(frmSearch.ShowDialog()==true)
      {
        _saleTypeCategoryFilter.stcID = frmSearch.strID;
        _saleTypeCategoryFilter.stcN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadSaleTypesCategories();
      }
    }
    #endregion

    #region btnAdd_Click
    /// <summary>
    /// Abre la ventana detalle en modo edición
    /// </summary>
    /// <history>
    /// [emoguel] created 30/07/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmSaleTypeCategoryDetail frmSaleTypeCategory = new frmSaleTypeCategoryDetail();
      frmSaleTypeCategory.Owner = this;
      frmSaleTypeCategory.enumMode = EnumMode.Add;
      if(frmSaleTypeCategory.ShowDialog()==true)
      {
        if(ValidateFilter(frmSaleTypeCategory.saleTypeCategory))//Validamos el filtro
        {
          List<SaleTypeCategory> lstSaleTypeCategories = dgrSaleTypesCategories.ItemsSource as List<SaleTypeCategory>;
          lstSaleTypeCategories.Add(frmSaleTypeCategory.saleTypeCategory);//Agregamos el registro a la lista
          int nIndex = lstSaleTypeCategories.IndexOf(frmSaleTypeCategory.saleTypeCategory);//Buscamos la posición del registro
          dgrSaleTypesCategories.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrSaleTypesCategories, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = "Categories (" + lstSaleTypeCategories.Count + ")";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region btnRef_Click
    /// <summary>
    /// Recarga los datos de la lista
    /// </summary>
    /// <history>
    /// [emoguel] created 30/07/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      SaleTypeCategory saleTypeCategory = dgrSaleTypesCategories.SelectedItem as SaleTypeCategory;
      LoadSaleTypesCategories(saleTypeCategory);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadSaleTypesCategories
    /// <summary>
    /// carga el grid con registros
    /// </summary>
    /// <param name="saleTypeCategory">Objeto a seleccionar en caso de que se envíe uno</param>
    /// <history>
    /// [emoguel] created 30/07/2016
    /// </history>
    private async void LoadSaleTypesCategories(SaleTypeCategory saleTypeCategory = null)
    {
      try
      {
        status.Visibility = Visibility.Collapsed;
        int nIndex = 0;
        List<SaleTypeCategory> lstSaleTypesCategories = await BRSaleTypesCategories.GetSaleCategories(_nStatus, _saleTypeCategoryFilter);
        dgrSaleTypesCategories.ItemsSource = lstSaleTypesCategories;
        if (lstSaleTypesCategories.Count > 0 && saleTypeCategory != null)
        {
          saleTypeCategory = lstSaleTypesCategories.Where(stc => stc.stcID == saleTypeCategory.stcID).FirstOrDefault();
          nIndex = lstSaleTypesCategories.IndexOf(saleTypeCategory);
        }
        GridHelper.SelectRow(dgrSaleTypesCategories, nIndex);
        StatusBarReg.Content = "Categories (" + lstSaleTypesCategories.Count + ")";
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region Validate Filter
    /// <summary>
    /// Valida si un SaleTypeCategory cumple con los filtros actuales
    /// </summary>
    /// <param name="saleTypeCategory">objeto a validar</param>
    /// <returns>
    /// True. Si cumple con los filtros
    /// False. No cumple con los filtros actuales
    /// </returns>
    /// <history>
    /// [emoguel] created 30/07/2016
    /// </history>
    private bool ValidateFilter(SaleTypeCategory saleTypeCategory)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(saleTypeCategory.stcA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_saleTypeCategoryFilter.stcID))//Filtro por ID
      {
        if(_saleTypeCategoryFilter.stcID!=saleTypeCategory.stcID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_saleTypeCategoryFilter.stcN))//Filtro por descripción
      {
        if(!saleTypeCategory.stcN.Contains(_saleTypeCategoryFilter.stcN,StringComparison.OrdinalIgnoreCase))
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

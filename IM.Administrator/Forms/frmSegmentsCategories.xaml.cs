using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Extensions;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSegmentsCategories.xaml
  /// </summary>
  public partial class frmSegmentsCategories : Window
  {
    #region variables
    private SegmentCategory _segmentCategoryFilter = new SegmentCategory();//objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar
    #endregion
    public frmSegmentsCategories()
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
    /// [emoguel] created 02/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(EnumPermission.Agencies, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadSegmentsCategories();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
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
    /// [emoguel] created 02/06/2016
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
    /// [emoguel] created 02/06/2016
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
    /// [emoguel] created 02/06/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      SegmentCategory segmentCategory = (SegmentCategory)dgrSegmentsCategories.SelectedItem;
      frmSegmentCategoryDetail frmSegmentCategoryDet = new frmSegmentCategoryDetail();
      frmSegmentCategoryDet.Owner = this;
      frmSegmentCategoryDet.oldSegmentCategory = segmentCategory;
      frmSegmentCategoryDet.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly;
      if (frmSegmentCategoryDet.ShowDialog() == true)
      {
        List<SegmentCategory> lstSegmentsCategories = (List<SegmentCategory>)dgrSegmentsCategories.ItemsSource;
        int nIndex = 0;
        if (ValidateFilter(frmSegmentCategoryDet.segmentCategory))//verificamos que cumpla con los filtros
        {
          ObjectHelper.CopyProperties(segmentCategory, frmSegmentCategoryDet.segmentCategory);//Actualizamos los datos
          lstSegmentsCategories.Sort((x, y) => string.Compare(x.scN, y.scN));//Ordenamos la lista
          nIndex = lstSegmentsCategories.IndexOf(frmSegmentCategoryDet.segmentCategory);//Buscamos la posición del regsitro
        }
        else
        {
          lstSegmentsCategories.Remove(frmSegmentCategoryDet.segmentCategory);//Quitamos el registro
        }
        dgrSegmentsCategories.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrSegmentsCategories, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstSegmentsCategories.Count + " Segments Categories";//Actualizamos el contador
      }
    }
    #endregion

    #region Order
    /// <summary>
    /// Abre la ventana Order Categories
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void btnSort_Click(object sender, RoutedEventArgs e)
    {
      frmSegmentsCategoriesOrder frmSegmentsOrder = new frmSegmentsCategoriesOrder();
      frmSegmentsOrder.Owner = this;
      frmSegmentsOrder.ShowDialog();
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _segmentCategoryFilter.scID;
      frmSearch.strDesc = _segmentCategoryFilter.scN;
      frmSearch.nStatus = _nStatus;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _segmentCategoryFilter.scID = frmSearch.strID;
        _segmentCategoryFilter.scN = frmSearch.strDesc;
        LoadSegmentsCategories();
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
    /// [emoguel] created 02/06/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmSegmentCategoryDetail frmSegmentCategory = new frmSegmentCategoryDetail();
      frmSegmentCategory.Owner = this;
      frmSegmentCategory.enumMode = EnumMode.Add;
      if(frmSegmentCategory.ShowDialog()==true)
      {
        if(ValidateFilter(frmSegmentCategory.segmentCategory))//Verificamos que cumpla con los filtros
        {
          List<SegmentCategory> lstSegmentsCategory = (List<SegmentCategory>)dgrSegmentsCategories.ItemsSource;
          lstSegmentsCategory.Add(frmSegmentCategory.segmentCategory);//Agregamos el registro
          int nIndex = lstSegmentsCategory.IndexOf(frmSegmentCategory.segmentCategory);//Buscamos la posición del registro
          dgrSegmentsCategories.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrSegmentsCategories, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstSegmentsCategory.Count + " Segments Categories";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Recarga los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      SegmentCategory segmentCategory = (SegmentCategory)dgrSegmentsCategories.SelectedItem;
      LoadSegmentsCategories(segmentCategory);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadSegmentsCategories
    /// <summary>
    /// Llena el grid de segmentCategory
    /// </summary>
    /// <param name="segmentCategory">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private async void LoadSegmentsCategories(SegmentCategory segmentCategory = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<SegmentCategory> lstSegmentCategories = await BRSegmentsCategories.GetSegmentsCategories(_nStatus,_segmentCategoryFilter);
        dgrSegmentsCategories.ItemsSource = lstSegmentCategories;
        if(lstSegmentCategories.Count>0 && segmentCategory!=null)
        {
          segmentCategory = lstSegmentCategories.Where(sc => sc.scID == segmentCategory.scID).FirstOrDefault();
          nIndex = lstSegmentCategories.IndexOf(segmentCategory);
        }
        GridHelper.SelectRow(dgrSegmentsCategories, nIndex);
        StatusBarReg.Content = lstSegmentCategories.Count + " Segments Categories.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Segments categories");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un segmentCategory cumpla con los filtros actuales
    /// </summary>
    /// <param name="segmentCategory">Objeto a seleccionar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private bool ValidateFilter(SegmentCategory segmentCategory)
    {
      if(_nStatus!=-1)//filtro por estatus
      {
        if(segmentCategory.scA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_segmentCategoryFilter.scID))//Filtro por ID
      {
        if(_segmentCategoryFilter.scID!=segmentCategory.scID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_segmentCategoryFilter.scN))//Filtro por descripción
      {
        if(!segmentCategory.scN.Contains(_segmentCategoryFilter.scN,StringComparison.OrdinalIgnoreCase))
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

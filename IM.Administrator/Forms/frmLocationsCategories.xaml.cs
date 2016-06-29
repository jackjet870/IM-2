using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmLocationsCategories.xaml
  /// </summary>
  public partial class frmLocationsCategories : Window
  {
    #region Variables
    private LocationCategory _locationCatFilter = new LocationCategory();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion

    public frmLocationsCategories()
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
    /// [emoguel] created 17/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadLocationCategories();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/05/2016
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
    /// [emoguel] created 17/05/2016
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
    /// [emoguel] created 17/05/2016
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
    /// [emoguel] created 17/05/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      LocationCategory locationCategory = (LocationCategory)dgrLocationCategories.SelectedItem;
      frmLocationCategoryDetail frmLocationCategoryDetail = new frmLocationCategoryDetail();
      frmLocationCategoryDetail.Owner = this;
      frmLocationCategoryDetail.enumMode = EnumMode.edit;
      frmLocationCategoryDetail.oldLocationCategory = locationCategory;
      if (frmLocationCategoryDetail.ShowDialog() == true)
      {
        List<LocationCategory> lstLocationCategory = (List<LocationCategory>)dgrLocationCategories.ItemsSource;
        int nIndex = 0;
        if (ValidateFilter(frmLocationCategoryDetail.locationCategory))//Verificamos que cumpla con los filtros
        {
          ObjectHelper.CopyProperties(locationCategory, frmLocationCategoryDetail.locationCategory);//Actualizamos los datos
          lstLocationCategory.Sort((x, y) => string.Compare(x.lcN, y.lcN));//ordenamos la lista
          nIndex = lstLocationCategory.IndexOf(locationCategory);//Obtenemos la posición del registro
        }
        else
        {
          lstLocationCategory.Remove(locationCategory);//Quitamos el registro
        }
        dgrLocationCategories.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrLocationCategories, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstLocationCategory.Count + " Location categories.";//Actualizamos el contador
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los registros de la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LocationCategory locationCategory = (LocationCategory)dgrLocationCategories.SelectedItem;
      LoadLocationCategories(locationCategory);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmLocationCategoryDetail frmLocartionCategoryDetail = new frmLocationCategoryDetail();
      frmLocartionCategoryDetail.Owner = this;
      frmLocartionCategoryDetail.enumMode = EnumMode.add;
      if(frmLocartionCategoryDetail.ShowDialog()==true)
      {
        if (ValidateFilter(frmLocartionCategoryDetail.locationCategory))//Verificamos que cumpla con los filtros
        {
          List<LocationCategory> lstLocationCategory = (List<LocationCategory>)dgrLocationCategories.ItemsSource;
          lstLocationCategory.Add(frmLocartionCategoryDetail.locationCategory);//Agregamos el registro
          lstLocationCategory.Sort((x, y) => string.Compare(x.lcN, y.lcN));//Ordenamos la lista
          int nIndex = lstLocationCategory.IndexOf(frmLocartionCategoryDetail.locationCategory);//Buscamos la posición del registro
          dgrLocationCategories.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrLocationCategories, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstLocationCategory.Count + " Location Category.";//Actualizamos el contador
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
    /// [emoguel] created 17/05/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.nStatus = _nStatus;
      frmSearch.strID = _locationCatFilter.lcID;
      frmSearch.strDesc = _locationCatFilter.lcN;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _locationCatFilter.lcID = frmSearch.strID;
        _locationCatFilter.lcN = frmSearch.strDesc;
        LoadLocationCategories();
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadLocationCategories
    /// <summary>
    /// Llena el grid de location categories
    /// </summary>
    /// <param name="locationCategory">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 17/05/2016
    /// </history>
    private async void LoadLocationCategories(LocationCategory locationCategory = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        List<LocationCategory> lstLocationCategories = await BRLocationsCategories.GetLocationsCategories(_nStatus, _locationCatFilter);
        dgrLocationCategories.ItemsSource = lstLocationCategories;
        int nIndex = 0;
        if (lstLocationCategories.Count > 0 && locationCategory != null)
        {
          locationCategory = lstLocationCategories.Where(lc => lc.lcID == locationCategory.lcID).FirstOrDefault();
          nIndex = lstLocationCategories.IndexOf(locationCategory);
        }
        GridHelper.SelectRow(dgrLocationCategories, nIndex);
        StatusBarReg.Content = lstLocationCategories.Count + " Location Categories.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Location Categories");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un registro cumpla con los filtros del grid
    /// </summary>
    /// <param name="locationCategory">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 17/05/2016
    /// </history>
    private bool ValidateFilter(LocationCategory locationCategory)
    {
      if (_nStatus != -1)//Filtro po estatus
      {
        if (locationCategory.lcA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_locationCatFilter.lcID))//Filtro por ID
      {
        if (_locationCatFilter.lcID != locationCategory.lcID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_locationCatFilter.lcN))//Filtro por descripción
      {
        if (!locationCategory.lcN.Contains(_locationCatFilter.lcN,StringComparison.OrdinalIgnoreCase))
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

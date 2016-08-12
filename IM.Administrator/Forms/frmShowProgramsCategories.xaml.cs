using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmShowProgramsCategories.xaml
  /// </summary>
  public partial class frmShowProgramsCategories : Window
  {
    #region variables
    private ShowProgramCategory _showProgramCategoryFilter = new ShowProgramCategory();//Objeto con filtros adicionales
    private int _nStatus = -1;//estatus de los registros del grid
    #endregion
    public frmShowProgramsCategories()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los detalles de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadShowProgramsCategories();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/06/2016
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
    /// [emoguel] created 03/06/2016
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
    /// [emoguel] created 03/06/2016
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
    /// [emoguel] created 03/06/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      ShowProgramCategory showProgramCategory = (ShowProgramCategory)dgrShowProgramscategories.SelectedItem;
      frmShowProgramCategoryDetail frmShowProgramCategoryDet = new frmShowProgramCategoryDetail();
      frmShowProgramCategoryDet.Owner = this;
      frmShowProgramCategoryDet.enumMode = EnumMode.Edit;
      frmShowProgramCategoryDet.oldShowProgramCategory = showProgramCategory;
      if(frmShowProgramCategoryDet.ShowDialog()==true)
      {
        int nIndex = 0;
        List<ShowProgramCategory> lstShowProgramcategories = (List<ShowProgramCategory>)dgrShowProgramscategories.ItemsSource;
        if(ValidateFilter(frmShowProgramCategoryDet.showProgramCategory))//Verificamos que cumpla con los filtros
        {
          ObjectHelper.CopyProperties(showProgramCategory, frmShowProgramCategoryDet.showProgramCategory);//Actualizamos los datos
          lstShowProgramcategories.Sort((x, y) => string.Compare(x.sgN, y.sgN));//Ordenamos la lista
          nIndex = lstShowProgramcategories.IndexOf(showProgramCategory);//Obtenemos la posición del registro
        }
        else
        {
          lstShowProgramcategories.Remove(showProgramCategory);//Quitamos el registro
        }
        dgrShowProgramscategories.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrShowProgramscategories, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstShowProgramcategories.Count + " Show Program Categories.";//Actualizamos el contador
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
    /// [emoguel] created 03/06/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      ShowProgramCategory showProgramcategory = (ShowProgramCategory)dgrShowProgramscategories.SelectedItem;
      LoadShowProgramsCategories(showProgramcategory);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/06/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmShowProgramCategoryDetail frmShowProgramCategoryDet = new frmShowProgramCategoryDetail();
      frmShowProgramCategoryDet.Owner = this;
      frmShowProgramCategoryDet.enumMode = EnumMode.Add;
      if(frmShowProgramCategoryDet.ShowDialog()==true)
      {
        if(ValidateFilter(frmShowProgramCategoryDet.showProgramCategory))//Verificamos que cumpla con los filtros 
        {
          List<ShowProgramCategory> lstShowProgramCategories = (List<ShowProgramCategory>)dgrShowProgramscategories.ItemsSource;
          lstShowProgramCategories.Add(frmShowProgramCategoryDet.showProgramCategory);//Agregamos el registro a la lista
          lstShowProgramCategories.Sort((x, y) => string.Compare(x.sgN, y.sgN));//Ordenamos la lista
          int nIndex = lstShowProgramCategories.IndexOf(frmShowProgramCategoryDet.showProgramCategory);//Obtenemos la posición del registro
          dgrShowProgramscategories.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrShowProgramscategories, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstShowProgramCategories.Count + " Show Program Categories.";//Actualizamos el contador
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
    /// [emoguel] created 03/06/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _showProgramCategoryFilter.sgID;
      frmSearch.strDesc = _showProgramCategoryFilter.sgN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _showProgramCategoryFilter.sgID = frmSearch.strID;
        _showProgramCategoryFilter.sgN = frmSearch.strDesc;
        LoadShowProgramsCategories();
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadShowProgramsCategories
    /// <summary>
    /// Llena el grid de show program categories
    /// </summary>
    /// <param name="showProgramCategory">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 03/06/2016
    /// </history>
    private async void LoadShowProgramsCategories(ShowProgramCategory showProgramCategory=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<ShowProgramCategory> lstShowPrograms = await BRShowProgramsCategories.GetShowProgramsCategories(_nStatus, _showProgramCategoryFilter);
        dgrShowProgramscategories.ItemsSource = lstShowPrograms;
        if (lstShowPrograms.Count > 0 && showProgramCategory != null)
        {
          showProgramCategory = lstShowPrograms.Where(sg => sg.sgID == showProgramCategory.sgID).FirstOrDefault();
          nIndex = lstShowPrograms.IndexOf(showProgramCategory);
        }
        GridHelper.SelectRow(dgrShowProgramscategories, nIndex);
        StatusBarReg.Content = lstShowPrograms.Count + " Show Programs Categories.";
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Show Programs Categories");
      }

    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que el objeto cumpla con los filtros establecidos
    /// </summary>
    /// <param name="showProgramCategory"></param>
    /// <returns>True. Si cumple | False. no cumple</returns>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private bool ValidateFilter(ShowProgramCategory showProgramCategory)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(showProgramCategory.sgA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_showProgramCategoryFilter.sgID))//Filtro por ID
      {
        if(_showProgramCategoryFilter.sgID!=showProgramCategory.sgID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_showProgramCategoryFilter.sgN))
      {
        if(!showProgramCategory.sgN.Contains(_showProgramCategoryFilter.sgN,StringComparison.OrdinalIgnoreCase))
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

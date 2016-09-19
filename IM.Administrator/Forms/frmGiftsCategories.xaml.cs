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
  /// Interaction logic for frmGiftsCategories.xaml
  /// </summary>
  public partial class frmGiftsCategories : Window
  {
    #region Variables
    private GiftCategory _giftCategoryFilter = new GiftCategory();//Objeto con los filtros de la ventana
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar|agregar
    #endregion
    public frmGiftsCategories()
    {
      InitializeComponent();
    }

    #region Eventos del formulario
    #region WindowKeyDown
    /// <summary>
    /// Verifica las teclas presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
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

    #region IsKeyboardFocusedChanged
    /// <summary>
    /// Verifica las teclas INS,NumLock,Cap cuando la ventana estuvo minimizada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Window Load
    /// <summary>
    /// Llena el formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(Model.Enums.EnumPermission.Gifts, Model.Enums.EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadGiftsCategories();
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana da busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _giftCategoryFilter.gcID;
      frmSearch.strDesc = _giftCategoryFilter.gcN;
      frmSearch.nStatus = _nStatus;

      if(frmSearch.ShowDialog()==true)
      {
        _giftCategoryFilter.gcID = frmSearch.strID;
        _giftCategoryFilter.gcN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadGiftsCategories();
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
    /// [emoguel] created 23/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmGiftCategoryDetail frmGiftCategoryDetail = new frmGiftCategoryDetail();
      frmGiftCategoryDetail.Owner = this;
      frmGiftCategoryDetail.enumMode =EnumMode.Add;
      if(frmGiftCategoryDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmGiftCategoryDetail.giftCategory))//Verificamos si cumple con los filtros actuales
        {
          List<GiftCategory> lstGiftsCateg = (List<GiftCategory>)dgrGiftsCateg.ItemsSource;
          lstGiftsCateg.Add(frmGiftCategoryDetail.giftCategory);//Agregamos el nuevo registro
          lstGiftsCateg.Sort((x,y)=>string.Compare(x.gcN,y.gcN));//Ordenamos la lista
          int nIndex = lstGiftsCateg.IndexOf(frmGiftCategoryDetail.giftCategory);//Obtenemos la posicion del nuevo registro
          dgrGiftsCateg.Items.Refresh();//Refrescamos la lista
          GridHelper.SelectRow(dgrGiftsCateg, nIndex);//Seleccionamos el nuevo registro
          StatusBarReg.Content = lstGiftsCateg.Count + " Gift Categories.";//Actualizamos el contador
        }
      }
    } 
    #endregion

    #region Refresh
    /// <summary>
    /// Recarga el grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      GiftCategory giftCategory = (GiftCategory)dgrGiftsCateg.SelectedItem;
      LoadGiftsCategories(giftCategory);
    } 
    #endregion

    #region Cell Double click
    /// <summary>
    /// Muestra la ventada detalle 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      GiftCategory giftCategory = (GiftCategory)dgrGiftsCateg.SelectedItem;
      frmGiftCategoryDetail frmGiftCategoryDetail = new frmGiftCategoryDetail();
      frmGiftCategoryDetail.Owner = this;
      frmGiftCategoryDetail.enumMode = ((_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly);
      frmGiftCategoryDetail.oldGiftCategory = giftCategory;
      if(frmGiftCategoryDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<GiftCategory> lstGiftsCateg = (List<GiftCategory>)dgrGiftsCateg.ItemsSource;
        if(!ValidateFilter(frmGiftCategoryDetail.giftCategory))//Verificamos si cumple con los filtros
        {
          lstGiftsCateg.Remove(giftCategory);//Removemos el registro
          StatusBarReg.Content = lstGiftsCateg.Count + " Gift Categories.";//Actualizamos el contador
        }
        else
        {
          ObjectHelper.CopyProperties(giftCategory, frmGiftCategoryDetail.giftCategory);//Actualizamos el registros
          lstGiftsCateg.Sort((x,y)=>string.Compare(x.gcN,y.gcN));//Ordenamos la lista
          nIndex = lstGiftsCateg.IndexOf(giftCategory);
        }
        dgrGiftsCateg.Items.Refresh();
        GridHelper.SelectRow(dgrGiftsCateg, nIndex);//Seleccionamos el registro si no se eliminó
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
    /// [created] 23/03/2016
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
    #endregion

    #region Methods
    #region LoadGiftCategories
    /// <summary>
    /// Llena el grid de GiftCategories
    /// </summary>
    /// <param name="nIndex">Indice a seleccionar en el grid</param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private async void LoadGiftsCategories(GiftCategory giftCategory=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<GiftCategory> lstGiftsCategories = await BRGiftsCategories.GetGiftsCategories(_giftCategoryFilter, _nStatus);
        dgrGiftsCateg.ItemsSource = lstGiftsCategories;
        if (giftCategory != null && lstGiftsCategories.Count > 0)
        {
          giftCategory = lstGiftsCategories.Where(gc => gc.gcID == giftCategory.gcID).FirstOrDefault();
          nIndex = lstGiftsCategories.IndexOf(giftCategory);
        }
        GridHelper.SelectRow(dgrGiftsCateg, nIndex);
        StatusBarReg.Content = lstGiftsCategories.Count + " Gift Categories.";
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
    /// Verifica si un objeto tipo GiftCategory con los filtros del grid
    /// </summary>
    /// <param name="newGiftCategory"></param>
    /// <returns>True. SI cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private bool ValidateFilter(GiftCategory newGiftCategory)
    {
      if (_nStatus != -1)//Filtro por ID
      {
        if (newGiftCategory.gcA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_giftCategoryFilter.gcID))//Filtro por ID
      {
        if(_giftCategoryFilter.gcID!=newGiftCategory.gcID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_giftCategoryFilter.gcN))//Filtro por descripcion
      {
        if (!newGiftCategory.gcN.Contains(_giftCategoryFilter.gcN,StringComparison.OrdinalIgnoreCase))
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

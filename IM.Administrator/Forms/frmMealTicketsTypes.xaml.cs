using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.Model;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmMealTicketsTypes.xaml
  /// </summary>
  public partial class frmMealTicketsTypes : Window
  {
    #region variables
    private MealTicketType _mealTkTypeFilter = new MealTicketType();//Objetos con los filtros del grid
    private int _nWpax = -1;//Filtro para WPax
    #endregion
    public frmMealTicketsTypes()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadMealTkTypes();
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
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
    /// Muestra la ventana detalle en modo preview|edicion
    /// </summary>
    /// <history>
    /// [emoguel] 04/04/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      MealTicketType mealTicketType = (MealTicketType)dgrMealTkTypes.SelectedItem;
      frmMealTicketTypeDetail frmMealTkType = new frmMealTicketTypeDetail();
      frmMealTkType.Owner = this;
      frmMealTkType.oldMealTicketType = mealTicketType;
      frmMealTkType.enumMode = EnumMode.edit;
      if(frmMealTkType.ShowDialog()==true)
      {
        int nIndex = 0;
        List<MealTicketType> lstMealTkTypes = (List<MealTicketType>)dgrMealTkTypes.ItemsSource;
        if(!ValidateFilter(frmMealTkType.mealTicketType))//verificamos si no cumple con los filtros
        {
          lstMealTkTypes.Remove(mealTicketType);//Quitamos el registro de la lista
        }
        else
        {
          ObjectHelper.CopyProperties(mealTicketType, frmMealTkType.mealTicketType);//Copiamos las nuevas propiedades
          lstMealTkTypes.Sort((x, y) =>string.Compare(x.myN,y.myN));//ordenamos la lista
          nIndex = lstMealTkTypes.IndexOf(mealTicketType);//obtenemos la posicion del registro
        }
        dgrMealTkTypes.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrMealTkTypes, nIndex);//Selecionamos el registro
        StatusBarReg.Content = lstMealTkTypes.Count + " Meal Ticket Types.";

      }
    }

    #endregion

    #region Window KeyDown
    /// <summary>
    /// Verifica botones presionados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
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

    #region IsKeyboardFocuChanged
    /// <summary>
    /// Verifica que botones fueron presionados con la ventana inactiva
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/06
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _mealTkTypeFilter.myID;
      frmSearch.strDesc = _mealTkTypeFilter.myN;
      frmSearch.nStatus = _nWpax;
      frmSearch.lblSta.Content = "With Pax";
      if(frmSearch.ShowDialog()==true)
      {
        _nWpax = frmSearch.nStatus;
        _mealTkTypeFilter.myID = frmSearch.strID;
        _mealTkTypeFilter.myN = frmSearch.strDesc;
        LoadMealTkTypes();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana de detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmMealTicketTypeDetail frmMealTkTypeDetail = new frmMealTicketTypeDetail();
      frmMealTkTypeDetail.Owner = this;
      frmMealTkTypeDetail.enumMode = EnumMode.add;
      if(frmMealTkTypeDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmMealTkTypeDetail.mealTicketType))//validamos si cumple con los filtros
        {
          MealTicketType mealTicketType = frmMealTkTypeDetail.mealTicketType;
          List<MealTicketType> lstMealTkTypes = (List<MealTicketType>)dgrMealTkTypes.ItemsSource;
          lstMealTkTypes.Add(mealTicketType);//Agregamos el objeto
          lstMealTkTypes.Sort((x, y) => string.Compare(x.myN, y.myN));//ordenamos la lista
          int nIndex = lstMealTkTypes.IndexOf(mealTicketType);//buscamos la posicion del nuevo registro
          dgrMealTkTypes.Items.Refresh();//actualizamos la vista del grid
          GridHelper.SelectRow(dgrMealTkTypes, nIndex);//Seleccionamo el nuevo registro
          StatusBarReg.Content = lstMealTkTypes.Count+" Meal Ticket Types.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Recarga la lista de mealTicketTypes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      MealTicketType mealTktype = (MealTicketType)dgrMealTkTypes.SelectedItem;
      LoadMealTkTypes(mealTktype);
    } 
    #endregion
    #endregion

    #region Methods 
    #region LoadMealTkTypes
    /// <summary>
    /// Llena el grid lstMealTkTypes
    /// </summary>
    /// <param name="mealTicketType">Objeto a selecionar</param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void LoadMealTkTypes(MealTicketType mealTicketType = null)
    {
      int nIndex = 0;
      List<MealTicketType> lstMealTkTypes = BRMealTicketTypes.GetMealTicketType(_mealTkTypeFilter, _nWpax);
      dgrMealTkTypes.ItemsSource = lstMealTkTypes;

      if (lstMealTkTypes.Count > 0 && mealTicketType != null)
      {
        mealTicketType = lstMealTkTypes.Where(my => my.myID == mealTicketType.myID).FirstOrDefault();
        nIndex = lstMealTkTypes.IndexOf(mealTicketType);
      }
      GridHelper.SelectRow(dgrMealTkTypes, nIndex);
      StatusBarReg.Content = lstMealTkTypes.Count + " Meal Ticket Types.";
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida si un MealticketType cumple con los filtros actuales
    /// </summary>
    /// <param name="mealTicketType">objeto para validar</param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private bool ValidateFilter(MealTicketType mealTicketType)
    {
      if (_nWpax != -1)//Validacion wPax
      {
        if (mealTicketType.myWPax != Convert.ToBoolean(_nWpax))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_mealTkTypeFilter.myID))//Filtro por ID
      {
        if(_mealTkTypeFilter.myID!=mealTicketType.myID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_mealTkTypeFilter.myN))//Filtro por descripcion
      {
        if (!mealTicketType.myN.Contains(_mealTkTypeFilter.myN))
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

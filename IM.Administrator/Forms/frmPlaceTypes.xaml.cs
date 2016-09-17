using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPlaceTypes.xaml
  /// </summary>
  public partial class frmPlaceTypes : Window
  {
    #region Variables
    private PlaceType _placeTypeFIlter = new PlaceType();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid

    #endregion
    public frmPlaceTypes()
    {
      InitializeComponent();
    }

    #region MethodsForm
    #region Window Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadPlaceTypes();
    } 
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 11/04/2016
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
    /// [emoguel] created 11/04/2016
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
    /// Muestra la ventana detalle
    /// </summary>
    /// <history>
    /// [emoguel] 11/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      PlaceType placeType = (PlaceType)dgrPlaceTypes.SelectedItem;
      frmPlaceTypeDetail frmPlaceTypeDetail = new frmPlaceTypeDetail();
      frmPlaceTypeDetail.Owner = this;
      frmPlaceTypeDetail.enumMode = EnumMode.Edit;
      frmPlaceTypeDetail.oldPlaceType = placeType;
      if(frmPlaceTypeDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<PlaceType> lstPlaceTypes = (List<PlaceType>)dgrPlaceTypes.ItemsSource;
        if (ValidateFilter(frmPlaceTypeDetail.placeType))//Verificar si cumple con los filtros
        {
          ObjectHelper.CopyProperties(placeType, frmPlaceTypeDetail.placeType);//Actualizamos los datos del registro
          lstPlaceTypes.Sort((x, y) => string.Compare(x.pyN, y.pyN));//ornedamos los registros
          nIndex = lstPlaceTypes.IndexOf(placeType);//Buscamos la posicion del registro
        }
        else
        {
          lstPlaceTypes.Remove(frmPlaceTypeDetail.placeType);
        }
        dgrPlaceTypes.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrPlaceTypes, nIndex);//Seleccionamos el registros
        StatusBarReg.Content = lstPlaceTypes.Count + " Place Types.";
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
    /// [emoguel] created 11/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _placeTypeFIlter.pyID;
      frmSearch.strDesc = _placeTypeFIlter.pyN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _placeTypeFIlter.pyID = frmSearch.strID;
        _placeTypeFIlter.pyN = frmSearch.strDesc;
        LoadPlaceTypes();
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
    /// [emoguel] created 11/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmPlaceTypeDetail frmPlaceTypeDetail = new frmPlaceTypeDetail();
      frmPlaceTypeDetail.Owner = this;
      frmPlaceTypeDetail.enumMode = EnumMode.Add;
      if(frmPlaceTypeDetail.ShowDialog()==true)
      {
        PlaceType placeType = frmPlaceTypeDetail.placeType;
        if(ValidateFilter(placeType))//verificamos si cumple con los filtros actuales
        {
          List<PlaceType> lstPlacesTypes = (List<PlaceType>)dgrPlaceTypes.ItemsSource;
          lstPlacesTypes.Add(placeType);//Agregamos el registro a la lista
          lstPlacesTypes.Sort((x, y) => string.Compare(x.pyN, y.pyN));//ordenamos la lista
          int nIndex = lstPlacesTypes.IndexOf(placeType);//obtenemos la posición del registro
          dgrPlaceTypes.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrPlaceTypes, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstPlacesTypes.Count + " Place Types.";//Actualizamos el contador
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
    /// [emoguel] created 11/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      PlaceType placeType = (PlaceType)dgrPlaceTypes.SelectedItem;
      LoadPlaceTypes(placeType);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadPlaceTypes
    /// <summary>
    /// Llena el grid de Place Types
    /// </summary>
    /// <param name="placeType">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private async void LoadPlaceTypes(PlaceType placeType = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<PlaceType> lstPlaceTypes = await BRPlaceTypes.GetPlaceTypes(_nStatus, _placeTypeFIlter);
        dgrPlaceTypes.ItemsSource = lstPlaceTypes;
        if (placeType != null && lstPlaceTypes.Count > 0)
        {
          placeType = lstPlaceTypes.Where(py => py.pyID == placeType.pyID).FirstOrDefault();
          nIndex = lstPlaceTypes.IndexOf(placeType);//Obtenemos la posicioón
        }
        GridHelper.SelectRow(dgrPlaceTypes, nIndex);
        StatusBarReg.Content = lstPlaceTypes.Count + " Place Types.";
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
    /// Valida que un objeto PlaceType cumpla con los filtros actuales
    /// </summary>
    /// <param name="placeType">Objeto a validar</param>
    /// <returns>Truw. SI cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private bool ValidateFilter(PlaceType placeType)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(placeType.pyA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_placeTypeFIlter.pyID))//Filtro por ID
      {
        if(_placeTypeFIlter.pyID!=placeType.pyID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_placeTypeFIlter.pyN))//filtro por estatus
      {
        if(!placeType.pyN.Contains(_placeTypeFIlter.pyN,StringComparison.OrdinalIgnoreCase))
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

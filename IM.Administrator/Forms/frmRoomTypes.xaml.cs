using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmRoomTypes.xaml
  /// </summary>
  public partial class frmRoomTypes : Window
  {
    #region variables
    private RoomType _roomTypeFilter = new RoomType();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion
    public frmRoomTypes()
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
    /// [emoguel] created 19/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadRoomTypes();
    }
    #endregion

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
      RoomType roomType = (RoomType)dgrRoomTypes.SelectedItem;
      frmRoomTypeDetail frmRoomTypeDetail = new frmRoomTypeDetail();
      frmRoomTypeDetail.Owner = this;
      frmRoomTypeDetail.enumMode = EnumMode.Edit;
      frmRoomTypeDetail.oldRoomType = roomType;
      if(frmRoomTypeDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<RoomType> lstRoomTypes = (List<RoomType>)dgrRoomTypes.ItemsSource;        
        if(ValidateFilter(frmRoomTypeDetail.roomType))//Validamos si cumple con los filtros actuales
        {
          ObjectHelper.CopyProperties(roomType, frmRoomTypeDetail.roomType);//Actualizamos los datos del registro
          lstRoomTypes.Sort((x, y) => string.Compare(x.rtN, y.rtN));//Ordenamos la lista
          nIndex = lstRoomTypes.IndexOf(roomType);//buscamos la posición del nuevo registro
        }
        else
        {
          lstRoomTypes.Remove(roomType);//Quitamos el registro
        }
        dgrRoomTypes.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrRoomTypes, nIndex);//Seleccionamos el registro nuevo
        StatusBarReg.Content = lstRoomTypes.Count + " Room Types.";//Actualizamos el contador
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
    /// [emoguel] created 19/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.nStatus = _nStatus;
      frmSearch.strID = _roomTypeFilter.rtID;
      frmSearch.strDesc = _roomTypeFilter.rtN;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _roomTypeFilter.rtID = frmSearch.strID;
        _roomTypeFilter.rtN = frmSearch.strDesc;
        LoadRoomTypes();
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
      frmRoomTypeDetail frmRoomTypeDetail = new frmRoomTypeDetail();
      frmRoomTypeDetail.Owner = this;
      frmRoomTypeDetail.enumMode = EnumMode.Add;
      if(frmRoomTypeDetail.ShowDialog()==true)
      {
        RoomType roomType = frmRoomTypeDetail.roomType;
        if(ValidateFilter(roomType))//Validamos que cumpla con los filtros actuales
        {
          List<RoomType> lstRoomTypes = (List<RoomType>)dgrRoomTypes.ItemsSource;
          lstRoomTypes.Add(roomType);//Agrega el registro nuevo a la lista
          lstRoomTypes.Sort((x, y) => string.Compare(x.rtN, y.rtN));//Ordenamos la lista
          int nIndex = lstRoomTypes.IndexOf(roomType);//Obtenemos la posicion del registro
          dgrRoomTypes.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrRoomTypes, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstRoomTypes.Count + " Room Types.";//Actualizamos el contador
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
    /// [emoguel] created 19/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      RoomType roomType = (RoomType)dgrRoomTypes.SelectedItem;
      LoadRoomTypes(roomType);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadRoomTypes
    /// <summary>
    /// Llena el grid de Room Types
    /// </summary>
    /// <param name="roomType">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private async void LoadRoomTypes(RoomType roomType = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<RoomType> lstRoomTypes = await BRRoomTypes.GetRoomTypes(_nStatus, _roomTypeFilter);
        dgrRoomTypes.ItemsSource = lstRoomTypes;
        if (lstRoomTypes.Count > 0 && roomType != null)
        {
          roomType = lstRoomTypes.Where(rt => rt.rtID == roomType.rtID).FirstOrDefault();
          nIndex = lstRoomTypes.IndexOf(roomType);
        }
        GridHelper.SelectRow(dgrRoomTypes, nIndex);
        StatusBarReg.Content = lstRoomTypes.Count + " Room Types.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Room Types");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Verifica que un objeto Roomtype cumpla con los filtros actuales
    /// </summary>
    /// <param name="roomType">Objeto a validar</param>
    /// <returns>True. Si cumples | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private bool ValidateFilter(RoomType roomType)
    {

      if(_nStatus!=-1)//filtro por estatus
      {
        if(roomType.rtA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_roomTypeFilter.rtID))//Filtro por ID
      {
        if(_roomTypeFilter.rtID!=roomType.rtID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_roomTypeFilter.rtN))//Filtro por Descripción
      {
        if(!roomType.rtN.Contains(_roomTypeFilter.rtN,StringComparison.OrdinalIgnoreCase))
        {
          return true;
        }
      }
      return true;
    }
    #endregion

    #endregion
  }
}

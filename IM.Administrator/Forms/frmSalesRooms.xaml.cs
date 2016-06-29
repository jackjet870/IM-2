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
  /// Interaction logic for frmSalesRooms.xaml
  /// </summary>
  public partial class frmSalesRooms : Window
  {
    #region Variables
    private SalesRoom _salesRoomFilter = new SalesRoom { srcu=""};//Contiene los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    private int _nAppointment = -1;//Filtro de appoinment
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar
    #endregion
    public frmSalesRooms()
    {
      InitializeComponent();
    }

    #region Methoss Form
    #region Loaded
    /// <summary>
    /// Carga los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Locations, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadSalesRooms();
    } 
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 21/04/2016
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
    /// [emoguel] created 21/04/2016
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
    /// [emoguel] created 21/04/2016
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
    /// [emoguel] 21/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      SalesRoom salesRoom = (SalesRoom)dgrSalesRooms.SelectedValue;
      frmSalesRoomDetail frmSalesRoomDetail = new frmSalesRoomDetail();
      frmSalesRoomDetail.Owner = this;
      frmSalesRoomDetail.enumMode = (_blnEdit) ? EnumMode.edit : EnumMode.preview;
      frmSalesRoomDetail.oldSalesRoom = salesRoom;
      if(frmSalesRoomDetail.ShowDialog()==true)
      {
        List<SalesRoom> lstSalesRoom = (List<SalesRoom>)dgrSalesRooms.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmSalesRoomDetail.salesRoom))//Validamos si tiene filtro
        {
          ObjectHelper.CopyProperties(salesRoom, frmSalesRoomDetail.salesRoom);//Actualizamos los datos del registro
          lstSalesRoom.Sort((x, y) => string.Compare(x.srN, y.srN));//Ordenamos la lista
          nIndex = lstSalesRoom.IndexOf(salesRoom);//Obtenemos la posición del registro
        }
        else
        {
          lstSalesRoom.Remove(salesRoom);//Quitamos el registro
        }
        dgrSalesRooms.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrSalesRooms, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstSalesRoom.Count + " Sales Rooms.";//Actualizamos el contador
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana detalle en modo Busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSalesRoomDetail frmSalesRoomDetail = new frmSalesRoomDetail();
      frmSalesRoomDetail.Owner = this;
      frmSalesRoomDetail.oldSalesRoom = _salesRoomFilter;
      frmSalesRoomDetail.nStatus = _nStatus;
      frmSalesRoomDetail.nAppointment = _nAppointment;
      frmSalesRoomDetail.enumMode = EnumMode.search;
      if(frmSalesRoomDetail.ShowDialog()==true)
      {
        _salesRoomFilter = frmSalesRoomDetail.salesRoom;
        _nStatus = frmSalesRoomDetail.nStatus;
        _nAppointment = frmSalesRoomDetail.nAppointment;
        LoadSalesRooms();
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
    /// [emoguel] created 21/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmSalesRoomDetail frmSalesRoomDetail = new frmSalesRoomDetail();
      frmSalesRoomDetail.Owner = this;
      frmSalesRoomDetail.enumMode = EnumMode.add;
      if(frmSalesRoomDetail.ShowDialog()==true)
      {
        SalesRoom salesRoom = frmSalesRoomDetail.salesRoom;
        if(ValidateFilter(salesRoom))//Validamos si cumple con los filtros
        {
          List<SalesRoom> lstSalesRooms = (List<SalesRoom>)dgrSalesRooms.ItemsSource;
          lstSalesRooms.Add(salesRoom);//Agregamos el registro
          int nIndex = lstSalesRooms.IndexOf(salesRoom);//Buscamos la posición del registro
          dgrSalesRooms.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrSalesRooms, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstSalesRooms.Count + " Sales Room";//Actualizamos el contador
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
    /// [emoguel] created 21/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      SalesRoom salesRoom = (SalesRoom)dgrSalesRooms.SelectedItem;
      LoadSalesRooms(salesRoom);
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadSalesRooms
    /// <summary>
    /// Llena el grid de Sales Rooms
    /// </summary>
    /// <param name="salesRoom">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 21/04/2016
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void LoadSalesRooms(SalesRoom salesRoom = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<SalesRoom> lstSalesRooms =await BRSalesRooms.GetSalesRooms(_nStatus, _nAppointment, _salesRoomFilter);
        dgrSalesRooms.ItemsSource = lstSalesRooms;
        if (lstSalesRooms.Count > 0 && salesRoom != null)
        {
          salesRoom = lstSalesRooms.Where(sr => sr.srID == salesRoom.srID).FirstOrDefault();
          nIndex = lstSalesRooms.IndexOf(salesRoom);
        }
        GridHelper.SelectRow(dgrSalesRooms, nIndex);
        StatusBarReg.Content = lstSalesRooms.Count + " Sales Rooms.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Sales Room");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valia que un objeto salesRoom cumpla con los filtros actuales
    /// </summary>
    /// <param name="salesRoom">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 21/04/2016
    /// </history>
    private bool ValidateFilter(SalesRoom salesRoom)
    {

      if(_nStatus!=-1)//Filtro por estatus
      {
        if(salesRoom.srA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(_nAppointment!=-1)
      {
        if(salesRoom.srAppointment!=Convert.ToBoolean(_nAppointment))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_salesRoomFilter.srID))//Filtro por ID
      {
        if(_salesRoomFilter.srID!=salesRoom.srID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_salesRoomFilter.srN))//Filtro por descripción
      {
        if(!salesRoom.srN.Contains(_salesRoomFilter.srN,StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_salesRoomFilter.srar))//Filtro por Area
      {
        if(_salesRoomFilter.srar!=salesRoom.srar)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_salesRoomFilter.srcu))//Filtro por Currency
      {
        if(_salesRoomFilter.srcu!=salesRoom.srcu)
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

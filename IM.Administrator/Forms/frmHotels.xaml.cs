using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;
using System.Linq;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmHotels.xaml
  /// </summary>
  public partial class frmHotels : Window
  {
    private Hotel _hotelFilter = new Hotel();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar
    public frmHotels()
    {
      InitializeComponent();
    }

    #region Event Form
    #region Window loaded
    /// <summary>
    /// Llena los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(Model.Enums.EnumPermission.Locations, Model.Enums.EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadHotels();
    }
    #endregion

    #region KeyBoard focus changed
    /// <summary>
    /// Verifica teclas presionadas con la ventana minimizada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    } 
    #endregion

    #region KeyDown form
    /// <summary>
    /// Verifica los botones presionados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/03/2016
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

    #region Refresh
    /// <summary>
    /// Actualiza el grid de Hotels
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Hotel hotel = (Hotel)dgrHotels.SelectedItem;
      LoadHotels(hotel);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoquel] created 29/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmHotelDetail frmHotelDetail = new frmHotelDetail();
      frmHotelDetail.Owner = this;
      frmHotelDetail.enumMode = EnumMode.add;
      if(frmHotelDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmHotelDetail.hotel))//Validamos que cumpla con los filtros actuales
        {
          List<Hotel> lstHotels = (List<Hotel>)dgrHotels.ItemsSource;
          lstHotels.Add(frmHotelDetail.hotel);//Agregar el registro
          lstHotels.Sort((x, y) => string.Compare(x.hoID, y.hoID));//Ordenamos la lista
          dgrHotels.Items.Refresh();//Actualizamos la vista de grid
          int nIndex = lstHotels.IndexOf(frmHotelDetail.hotel);//Obtenemos el index
          GridHelper.SelectRow(dgrHotels,nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstHotels.Count + " Hotels.";//Actualizamos el contador
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
    /// [emoguel] created 29/03/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmHotelDetail frmHotelDetail = new frmHotelDetail();
      frmHotelDetail.Owner = this;
      frmHotelDetail.oldHotel = _hotelFilter;
      frmHotelDetail.enumMode = EnumMode.search;
      frmHotelDetail.nStatus = _nStatus;
      if (frmHotelDetail.ShowDialog() == true)
      {
        _nStatus = frmHotelDetail.nStatus;
        _hotelFilter = frmHotelDetail.hotel;
        LoadHotels();
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
    /// [emoguel] created 29/03/2016
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
    /// [emoguel] created 29/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Hotel hotel = (Hotel)dgrHotels.SelectedItem;
      frmHotelDetail frmHotelDetail = new frmHotelDetail();
      frmHotelDetail.Owner = this;
      frmHotelDetail.oldHotel = hotel;
      frmHotelDetail.enumMode = ((_blnEdit == true) ? EnumMode.edit : EnumMode.preview);
      if(frmHotelDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<Hotel> lstHotels = (List<Hotel>)dgrHotels.ItemsSource;
        if(!ValidateFilter(frmHotelDetail.hotel))//Verificamos que cumpla con los registros actuales
        {
          lstHotels.Remove(hotel);//Quitamos el registro
        }
        else
        {
          ObjectHelper.CopyProperties(hotel, frmHotelDetail.hotel);//Actualizamos los datos del registro en el grid                    
          nIndex = dgrHotels.SelectedIndex;
        }
        dgrHotels.Items.Refresh();
        GridHelper.SelectRow(dgrHotels, nIndex);
        StatusBarReg.Content = lstHotels.Count+" Hotels.";
      }
    }

    #endregion
    #endregion

    #region Methods
    #region LoadHotels
    /// <summary>
    /// Llena el grid de Hotels
    /// </summary>
    /// <param name="nIndex">Index que va a seleccionar</param>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    private void LoadHotels(Hotel hotel=null)
    {
      List<Hotel> lstHotels = BRHotels.GetHotels(_hotelFilter, _nStatus,true);
      dgrHotels.ItemsSource = lstHotels;
      int nIndex =0;
      if(hotel!=null && lstHotels.Count>0)
      {
        hotel = lstHotels.Where(ho => ho.hoID == hotel.hoID).FirstOrDefault();
        nIndex = lstHotels.IndexOf(hotel);
      }
      GridHelper.SelectRow(dgrHotels, nIndex);

      StatusBarReg.Content = lstHotels.Count + " Hotels.";
    }
    #endregion

    #region Validate Filter
    private bool ValidateFilter(Hotel hotel)
    {
      if (_nStatus != -1)//Filtro por estatus
      {
        if (_hotelFilter.hoA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_hotelFilter.hoID))//Filtro por ID
      {
        if (_hotelFilter.hoID != hotel.hoID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_hotelFilter.hoar))//filtro por Area
      {
        if (_hotelFilter.hoar != hotel.hoar)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_hotelFilter.hoGroup))//Filtro por grupo
      {
        if (_hotelFilter.hoGroup != hotel.hoGroup)
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

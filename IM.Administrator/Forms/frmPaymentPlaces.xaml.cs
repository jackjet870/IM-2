using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPaymentPlaces.xaml
  /// </summary>
  public partial class frmPaymentPlaces : Window
  {
    #region Variables
    private PaymentPlace _paymentPlaceFilter = new PaymentPlace();//Objeto con los filtros de la ventan
    private int _nStatus=-1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar|agregar
    #endregion
    public frmPaymentPlaces()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Windows Loaded
    /// <summary>
    /// carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(Model.Enums.EnumPermission.Sales, Model.Enums.EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadPaymentPlaces();
    } 
    #endregion

    #region Cell Double click
    /// <summary>
    /// Muestra la ventada detalle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      PaymentPlace paymentPlace = (PaymentPlace)dgrPaymentPlace.SelectedItem;
      frmPaymentPlaceDetail frmPaymentPlaceDetail = new frmPaymentPlaceDetail();
      frmPaymentPlaceDetail.Owner = this;
      frmPaymentPlaceDetail.enumMode = (_blnEdit) ? EnumMode.edit : EnumMode.preview;
      frmPaymentPlaceDetail.oldPaymentPlace = paymentPlace;
      if(frmPaymentPlaceDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<PaymentPlace> lstPaymentPlaces = (List<PaymentPlace>)dgrPaymentPlace.ItemsSource;
        if(!ValidateFIlter(frmPaymentPlaceDetail.paymentPlace))//Validamos que cumpla con los filtros
        {
          lstPaymentPlaces.Remove(paymentPlace);//removemos de la lista
        }
        else
        {
          ObjectHelper.CopyProperties(paymentPlace, frmPaymentPlaceDetail.paymentPlace);//Actualizamos los datos del registro
          lstPaymentPlaces.Sort((x, y) => string.Compare(x.pcN, y.pcN));//Ordenamos la lista
          nIndex = lstPaymentPlaces.IndexOf(paymentPlace);//Obtenemos la posicion del registro
        }
        dgrPaymentPlace.Items.Refresh();
        GridHelper.SelectRow(dgrPaymentPlace, nIndex);
        StatusBarReg.Content = lstPaymentPlaces.Count + " Payment Places.";
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
    /// [created] 05/04/2016
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

    #region KeyBoardFocuChange
    /// <summary>
    /// Verifica las teclas presinadas al cambio de ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region window Key Down
    /// <summary>
    /// Verifica las teclas presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.nStatus = _nStatus;
      frmSearch.strID = _paymentPlaceFilter.pcID;
      frmSearch.strDesc = _paymentPlaceFilter.pcN;

      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _paymentPlaceFilter.pcID = frmSearch.strID;
        _paymentPlaceFilter.pcN = frmSearch.strDesc;
        LoadPaymentPlaces();
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
    /// [emoguel] created 06/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmPaymentPlaceDetail frmPaymentPlaceDetail = new frmPaymentPlaceDetail();
      frmPaymentPlaceDetail.Owner = this;
      frmPaymentPlaceDetail.enumMode = EnumMode.add;
      if(frmPaymentPlaceDetail.ShowDialog()==true)
      {
        PaymentPlace paymentPlace = frmPaymentPlaceDetail.paymentPlace;
        if(ValidateFIlter(paymentPlace))//Validamos si cumple con el filtro
        {
          List<PaymentPlace> lstPaymentPlaces = (List<PaymentPlace>)dgrPaymentPlace.ItemsSource;
          lstPaymentPlaces.Add(paymentPlace);//Agregamos el registro
          lstPaymentPlaces.Sort((x, y) => string.Compare(x.pcN, y.pcN));//Ordenamos la lista
          int nIndex = lstPaymentPlaces.IndexOf(paymentPlace);//Obtenemos el index
          dgrPaymentPlace.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrPaymentPlace, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstPaymentPlaces.Count + " Payment Places.";
        }
      }

    } 
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza el grid de payment Places
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      PaymentPlace paymentPlace = (PaymentPlace)dgrPaymentPlace.SelectedItem;
      LoadPaymentPlaces(paymentPlace);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadPaymentPlaces
    /// <summary>
    /// Llena el grid de Payment Places
    /// </summary>
    /// <param name="paymentPlace">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadPaymentPlaces(PaymentPlace paymentPlace = null)
    {
      int nIndex = 0;
      List<PaymentPlace> lstPaymentPlace =await BRPaymentPlaces.GetPaymentPlaces(_nStatus, _paymentPlaceFilter);
      dgrPaymentPlace.ItemsSource = lstPaymentPlace;
      if (lstPaymentPlace.Count > 0 && paymentPlace != null)
      {
        paymentPlace = lstPaymentPlace.Where(pc => pc.pcID == paymentPlace.pcID).FirstOrDefault();
        nIndex = lstPaymentPlace.IndexOf(paymentPlace);
      }
      GridHelper.SelectRow(dgrPaymentPlace, nIndex);
      StatusBarReg.Content = lstPaymentPlace.Count + " Payment Places.";
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Verifica que un objeto Payment Place cumpla con los filtros actuales
    /// </summary>
    /// <param name="paymentPlace">Objeto a validar</param>
    /// <returns>True. SI cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private bool ValidateFIlter(PaymentPlace paymentPlace)
    {
      if(_nStatus!=-1)//filtro por estatus
      {
        if(paymentPlace.pcA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_paymentPlaceFilter.pcID))//Filtro por ID
      {
        if(paymentPlace.pcID!=_paymentPlaceFilter.pcID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_paymentPlaceFilter.pcN))//Filtro por descripción
      {
        if(!paymentPlace.pcN.Contains(_paymentPlaceFilter.pcN))
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

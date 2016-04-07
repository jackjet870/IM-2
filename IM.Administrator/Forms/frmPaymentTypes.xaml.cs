using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.Administrator.Enums;
using IM.BusinessRules.BR;


namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPaymentTypes.xaml
  /// </summary>
  public partial class frmPaymentTypes : Window
  {
    #region Variables
    private PaymentType _paymentTypeFilter = new PaymentType();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid    
    #endregion
    public frmPaymentTypes()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadPaymentTypes();
    }
    #endregion

    #region Cell Double click
    /// <summary>
    /// Muestra la ventada detalle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      PaymentType paymentType = (PaymentType)dgrPaymentTypes.SelectedItem;
      frmPaymentTypeDetail frmPaymentTypeDetail = new frmPaymentTypeDetail();
      frmPaymentTypeDetail.Owner = this;
      frmPaymentTypeDetail.enumMode = EnumMode.edit;
      frmPaymentTypeDetail.oldPaymentType = paymentType;
      if(frmPaymentTypeDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<PaymentType> lstPaymentType = (List<PaymentType>)dgrPaymentTypes.ItemsSource;
        if(ValidateFilter(frmPaymentTypeDetail.paymentType))//Verificamos si cumple con los filtros
        {
          ObjectHelper.CopyProperties(paymentType, frmPaymentTypeDetail.paymentType);//Actualizamos los datos del registro
          lstPaymentType.Sort((x, y) => string.Compare(x.ptN, y.ptN));//ordenamos la lista
          nIndex = lstPaymentType.IndexOf(paymentType);//Obtenemos la posición del registro
        }
        else
        {
          lstPaymentType.Remove(paymentType);
        }
        dgrPaymentTypes.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrPaymentTypes, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstPaymentType.Count + " Payment Types.";
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
    /// [created] 06/04/2016
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
    /// [emoguel] created 06/04/2016
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
      frmSearch.strID = _paymentTypeFilter.ptID;
      frmSearch.strDesc = _paymentTypeFilter.ptN;
      frmSearch.nStatus = _nStatus;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _paymentTypeFilter.ptID = frmSearch.strID;
        _paymentTypeFilter.ptN = frmSearch.strDesc;
        LoadPaymentTypes();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmPaymentTypeDetail frmPaymentTypeDetail = new frmPaymentTypeDetail();
      frmPaymentTypeDetail.Owner = this;
      frmPaymentTypeDetail.enumMode = EnumMode.add;
      if(frmPaymentTypeDetail.ShowDialog()==true)
      {
        PaymentType paymentType = frmPaymentTypeDetail.paymentType;
        if(ValidateFilter(paymentType))//Validamos si cumple con los filtros actuales
        {
          List<PaymentType> lstPaymentTypes = (List<PaymentType>)dgrPaymentTypes.ItemsSource;
          lstPaymentTypes.Add(paymentType);//Agregamos a la lista
          lstPaymentTypes.Sort((x, y) => string.Compare(x.ptN, y.ptN));//ordenamos la lista
          int nIndex = lstPaymentTypes.IndexOf(paymentType);//obtenemos la posicion del registro
          dgrPaymentTypes.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrPaymentTypes, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstPaymentTypes.Count + " Payment Types.";//Actualizamos el contador
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
    /// [emoguel] created 06/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      PaymentType paymentType = (PaymentType)dgrPaymentTypes.SelectedItem;
      LoadPaymentTypes(paymentType);
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadPaymentTypes
    /// <summary>
    /// Llena el grid de Payment Types
    /// </summary>
    /// <param name="paymentType">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void LoadPaymentTypes(PaymentType paymentType = null)
    {
      int nIndex = 0;
      List<PaymentType> lstPaymentTypes = BRPaymentTypes.GetPaymentTypes(_nStatus, _paymentTypeFilter);
      dgrPaymentTypes.ItemsSource = lstPaymentTypes;
      if (lstPaymentTypes.Count > 0 && paymentType != null)
      {
        paymentType = lstPaymentTypes.Where(pt => pt.ptID == paymentType.ptID).FirstOrDefault();
        nIndex = lstPaymentTypes.IndexOf(paymentType);
      }
      GridHelper.SelectRow(dgrPaymentTypes, nIndex);
      StatusBarReg.Content = lstPaymentTypes.Count + " Payment Types.";
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida si un objeto tipo Payment Type cumple con los filtros actuales
    /// </summary>
    /// <param name="paymentType">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private bool ValidateFilter(PaymentType paymentType)
    {
      if(_nStatus!=-1)//filtro por estatus
      {
        if(paymentType.ptA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_paymentTypeFilter.ptID))//Filtro por ID
      {
        if(_paymentTypeFilter.ptID!=paymentType.ptID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_paymentTypeFilter.ptN))//Filtro por descripción
      {
        if(!paymentType.ptN.Contains(_paymentTypeFilter.ptN))
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

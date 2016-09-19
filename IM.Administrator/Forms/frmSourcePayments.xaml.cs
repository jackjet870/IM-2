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
  /// Interaction logic for frmSourcePayments.xaml
  /// </summary>
  public partial class frmSourcePayments : Window
  {
    #region Variables
    private SourcePayment _sourcePaymentFilter = new SourcePayment();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit=false;//Boleano para saber si se tiene permiso para editar
    #endregion
    public frmSourcePayments()
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
    /// [emoguel] created 26/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadSourcePayments();
    }
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 26/04/2016
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
    /// [emoguel] created 26/04/2016
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
    /// [emoguel] created 26/04/2016
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
    /// [emoguel] 26/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      SourcePayment sourcePayment = (SourcePayment)dgrSourcePayments.SelectedItem;
      frmSourcePaymentDetail frmSourcePayment = new frmSourcePaymentDetail();
      frmSourcePayment.Owner = this;
      frmSourcePayment.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly;
      frmSourcePayment.oldSourcePayment = sourcePayment;
      if(frmSourcePayment.ShowDialog()==true)
      {
        int nIndex = 0;
        List<SourcePayment> lstSourcePayments = (List<SourcePayment>)dgrSourcePayments.ItemsSource;
        if(ValidateFilter(frmSourcePayment.sourcePayment))//Validamos si cumple con los filtros actuales
        {
          ObjectHelper.CopyProperties(sourcePayment, frmSourcePayment.sourcePayment);//Actualizamos los datos
          lstSourcePayments.Sort((x, Y) => string.Compare(x.sbN, Y.sbN));//Ordenamos la lista
          nIndex = lstSourcePayments.IndexOf(sourcePayment);//Obtenemos la posición del registro
        }
        else
        {
          lstSourcePayments.Remove(sourcePayment);//Quitamos el registro
        }
        dgrSourcePayments.Items.Refresh();//Actualizamos la vista del contador
        GridHelper.SelectRow(dgrSourcePayments, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstSourcePayments.Count + " Source Payments.";//Actualizamos el contador
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
    /// [emoguel] created 26/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _sourcePaymentFilter.sbID;
      frmSearch.strDesc = _sourcePaymentFilter.sbN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _sourcePaymentFilter.sbID = frmSearch.strID;
        _sourcePaymentFilter.sbN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadSourcePayments();
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
    /// [emoguel] created 26/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmSourcePaymentDetail frmSourcePaymentDetail = new frmSourcePaymentDetail();
      frmSourcePaymentDetail.Owner = this;
      frmSourcePaymentDetail.enumMode = EnumMode.Add;
      if(frmSourcePaymentDetail.ShowDialog()==true)
      {
        SourcePayment sourcePayment = frmSourcePaymentDetail.sourcePayment;
        if(ValidateFilter(sourcePayment))//Validamos que cumpla con los filtros actuales
        {
          List<SourcePayment> lstSourcePayment = (List<SourcePayment>)dgrSourcePayments.ItemsSource;
          lstSourcePayment.Add(sourcePayment);//Agregamos el registro a la lista
          lstSourcePayment.Sort((x, y) => string.Compare(x.sbN, y.sbN));//Ordenamos la lista
          int nInde = lstSourcePayment.IndexOf(sourcePayment);//Obtenemos la posición del registro 
          dgrSourcePayments.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrSourcePayments, nInde);//Seleccionamos el registro
          StatusBarReg.Content = lstSourcePayment.Count + " Source Payments.";//Actualizamos el contador
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
    /// [emoguel] created 26/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      SourcePayment sourcePayment = (SourcePayment)dgrSourcePayments.SelectedItem;
      LoadSourcePayments(sourcePayment);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadSourcePayments
    /// <summary>
    /// Carga el grid de Source payments
    /// </summary>
    /// <param name="sourcePayment">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 26/04/2016
    /// </history>
    private async void LoadSourcePayments(SourcePayment sourcePayment = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<SourcePayment> lstSourcePayments = await BRSourcePayments.GetSourcePayments(_nStatus, _sourcePaymentFilter);
        dgrSourcePayments.ItemsSource = lstSourcePayments;
        if (lstSourcePayments.Count > 0 && sourcePayment != null)
        {
          sourcePayment = lstSourcePayments.Where(sb => sb.sbID == sourcePayment.sbID).FirstOrDefault();
          nIndex = lstSourcePayments.IndexOf(sourcePayment);
        }
        GridHelper.SelectRow(dgrSourcePayments, nIndex);
        StatusBarReg.Content = lstSourcePayments.Count + " Source Payments.";
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
    /// Valida que un registro tipo sourcePayment cumpla con los filtros actuales
    /// </summary>
    /// <param name="sourcePayment">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 26/04/2016
    /// </history>
    public bool ValidateFilter(SourcePayment sourcePayment)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(sourcePayment.sbA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_sourcePaymentFilter.sbID))//Filtro por ID
      {
        if(sourcePayment.sbID!=_sourcePaymentFilter.sbID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_sourcePaymentFilter.sbN))//Filtro por descripción
      {
        if(!sourcePayment.sbN.Contains(_sourcePaymentFilter.sbN,StringComparison.OrdinalIgnoreCase))
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

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
  /// Interaction logic for frmPaymentSchemas.xaml
  /// </summary>
  public partial class frmPaymentSchemas : Window
  {
    #region Variables
    private PaymentSchema _paymentSchemaFilter = new PaymentSchema();//Filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion
    public frmPaymentSchemas()
    {
      InitializeComponent();
    }

    #region Window Loaded
    /// <summary>
    /// carga los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadPaymentSchemas();
    } 
    #endregion

    #region Methods Form
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
      PaymentSchema paymentSchema = (PaymentSchema)dgrPaymentSchemas.SelectedItem;
      frmPaymentSchemaDetail frmPaymentScheDetail = new frmPaymentSchemaDetail();
      frmPaymentScheDetail.Owner = this;
      frmPaymentScheDetail.enumMode = EnumMode.Edit;
      frmPaymentScheDetail.oldPaymentSchema = paymentSchema;

      if(frmPaymentScheDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<PaymentSchema> lstPaymentSchemas = (List<PaymentSchema>)dgrPaymentSchemas.ItemsSource;
        if(!ValidateFilter(frmPaymentScheDetail.paymentSchema))
        {
          lstPaymentSchemas.Remove(paymentSchema);//Removemos el registro
        }
        else
        {
          ObjectHelper.CopyProperties(paymentSchema, frmPaymentScheDetail.paymentSchema);//Actualizamos los datos del registro
          lstPaymentSchemas.Sort((x, y) => string.Compare(x.pasN, y.pasN));//Ordenamos la lista
          nIndex = lstPaymentSchemas.IndexOf(paymentSchema);//Obtenemos la posicion del registro
        }
        dgrPaymentSchemas.Items.Refresh();//Actualizamos la lista
        GridHelper.SelectRow(dgrPaymentSchemas, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstPaymentSchemas.Count + " Payment Schemas.";//Actualizamos el contador
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
      frmSearch.enumWindow = EnumWindow.DefaultInt;
      frmSearch.strID = (_paymentSchemaFilter.pasID>0)?_paymentSchemaFilter.pasID.ToString():"";
      frmSearch.strDesc = _paymentSchemaFilter.pasN;
      frmSearch.nStatus = _nStatus;

      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _paymentSchemaFilter.pasID = Convert.ToInt32(frmSearch.strID);
        _paymentSchemaFilter.pasN = frmSearch.strDesc;
        LoadPaymentSchemas();
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
    /// [emoguel] created 06/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmPaymentSchemaDetail frmPaymentScheDetail = new frmPaymentSchemaDetail();
      frmPaymentScheDetail.Owner = this;
      frmPaymentScheDetail.enumMode = EnumMode.Add;
      if(frmPaymentScheDetail.ShowDialog()==true)
      {
        PaymentSchema paymentSchema = frmPaymentScheDetail.paymentSchema;
        if(ValidateFilter(paymentSchema))//Verificamos que cumpla con los filtros
        {
          List<PaymentSchema> lstPaymentSchemas = (List<PaymentSchema>)dgrPaymentSchemas.ItemsSource;
          lstPaymentSchemas.Add(paymentSchema);//Agregamos el registro a la lista
          lstPaymentSchemas.Sort((x, y) => string.Compare(x.pasN, y.pasN));//ordenamos la lista
          int nIndex = lstPaymentSchemas.IndexOf(paymentSchema);//obtenemos la posición del registro
          dgrPaymentSchemas.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrPaymentSchemas, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstPaymentSchemas.Count + " Payment Schemas.";
        }
      }
    } 
    #endregion

    #region Refesh
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
      PaymentSchema paymentSchema = (PaymentSchema)dgrPaymentSchemas.SelectedItem;
      LoadPaymentSchemas(paymentSchema);
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadPaymentSchemas
    /// <summary>
    /// Llena el grid de PaymentSchemas
    /// </summary>
    /// <param name="paymentSchemas">Objeto para seleccionar</param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private async void LoadPaymentSchemas(PaymentSchema paymentSchemas=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<PaymentSchema> lstPaymentSchemas = await BRPaymentSchemas.GetPaymentSchemas(_nStatus, _paymentSchemaFilter);
        dgrPaymentSchemas.ItemsSource = lstPaymentSchemas;
        if (lstPaymentSchemas.Count > 0 && paymentSchemas != null)
        {
          paymentSchemas = lstPaymentSchemas.Where(pas => pas.pasID == paymentSchemas.pasID).FirstOrDefault();
          nIndex = lstPaymentSchemas.IndexOf(paymentSchemas);
        }
        GridHelper.SelectRow(dgrPaymentSchemas, nIndex);
        StatusBarReg.Content = lstPaymentSchemas.Count + " Payment Schemas.";
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
    /// Valida que un objeto paymentSchema cumpla con los filtros actuales
    /// </summary>
    /// <param name="paymentSchema">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private bool ValidateFilter(PaymentSchema paymentSchema)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(paymentSchema.pasA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(_paymentSchemaFilter.pasID>0)//Filtro por ID
      {
        if(paymentSchema.pasID!=_paymentSchemaFilter.pasID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_paymentSchemaFilter.pasN))//Filtro por estatus
      {
        if(!paymentSchema.pasN.Contains(_paymentSchemaFilter.pasN,StringComparison.OrdinalIgnoreCase))
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

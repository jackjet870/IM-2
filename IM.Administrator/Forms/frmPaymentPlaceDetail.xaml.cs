using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPaymentPlaceDetail.xaml
  /// </summary>
  public partial class frmPaymentPlaceDetail : Window
  {
    #region Variables
    public PaymentPlace paymentPlace = new PaymentPlace();//objeto a guardar
    public PaymentPlace oldPaymentPlace = new PaymentPlace();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    private bool _isClosing = false;
    #endregion
    public frmPaymentPlaceDetail()
    {
      InitializeComponent();
    }

    #region Accept
    /// <summary>
    /// Ejecuta una funcion dependiendo del modo de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/05/2016
    /// [emoguel] modified 30/05/2016 se vovlió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (ObjectHelper.IsEquals(paymentPlace, oldPaymentPlace) && enumMode != EnumMode.Add)
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Payment Place");
          if (strMsj == "")
          {
            int nRes = await BREntities.OperationEntity(paymentPlace, enumMode);
            UIHelper.ShowMessageResult("Payment Place", nRes);
            if (nRes > 0)
            {
              _isClosing = true;
              DialogResult = true;
              Close();
            }
          }
          else
          {
            UIHelper.ShowMessage(strMsj);
          }
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    } 
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();
    } 
    #endregion

    #region Loaded
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
      ObjectHelper.CopyProperties(paymentPlace, oldPaymentPlace);
      DataContext = paymentPlace;
      txtpcID.IsEnabled = (enumMode != EnumMode.Edit);      
      if (enumMode != EnumMode.ReadOnly)
      {
        btnAccept.Visibility = Visibility.Visible;
        txtpcN.IsEnabled = true;
        chkpcA.IsEnabled = true;
        UIHelper.SetUpControls(paymentPlace, this);
      }
    } 
    #endregion

    #region Windwow KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        Close();
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica cambios antes cerrar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if(!_isClosing)
      {
        if (enumMode != EnumMode.ReadOnly)
        {
          if (!ObjectHelper.IsEquals(paymentPlace, oldPaymentPlace))
          {
            MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
            if (result != MessageBoxResult.Yes)
            {
              e.Cancel = true;
            }
          }
        }
      }
    } 
    #endregion
  }
}

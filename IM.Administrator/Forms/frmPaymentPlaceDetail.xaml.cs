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
        if (ObjectHelper.IsEquals(paymentPlace, oldPaymentPlace) && enumMode != EnumMode.add)
        {
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
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Payment Places"); 
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
      if (enumMode != EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(paymentPlace, oldPaymentPlace))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            Close();
          }
        }
        else
        {
          Close();
        }
      }
      else
      {
        Close();
      }
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
      txtpcID.IsEnabled = (enumMode != EnumMode.edit);      
      if (enumMode != EnumMode.preview)
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
        btnCancel_Click(null, null);
      }
    }
    #endregion
  }
}

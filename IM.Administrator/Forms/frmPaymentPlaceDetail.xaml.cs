using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;

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
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
        if(ObjectHelper.IsEquals(paymentPlace,oldPaymentPlace) && enumMode!=EnumMode.add)
        {
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Payment Place");
          if(strMsj=="")
          {
          int nRes = BREntities.OperationEntity(paymentPlace, enumMode);
            UIHelper.ShowMessageResult("PaymentPlaces", nRes);
            if(nRes>0)
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

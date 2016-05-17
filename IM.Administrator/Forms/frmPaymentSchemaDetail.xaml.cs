using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPaymentSchemaDetail.xaml
  /// </summary>
  public partial class frmPaymentSchemaDetail : Window
  {
    #region Variables
    public PaymentSchema paymentSchema = new PaymentSchema();//Objeto a guardar
    public PaymentSchema oldPaymentSchema=new PaymentSchema();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    #endregion
    public frmPaymentSchemaDetail()
    {
      InitializeComponent();
    }

    #region KeyDown
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
      if(e.Key==Key.Escape)
      {
        btnCancel.Focus();
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Loaded
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
      ObjectHelper.CopyProperties(paymentSchema, oldPaymentSchema);
      DataContext = paymentSchema;
      UIHelper.SetUpControls(paymentSchema, this);
    } 
    #endregion

    #region Accept
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo paymentSchema
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (ObjectHelper.IsEquals(paymentSchema, oldPaymentSchema) && enumMode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Payment Schema");
        if(paymentSchema.pasCoupleTo<paymentSchema.pasCoupleFrom)
        {
          strMsj +=(strMsj!="")?"\n ":""+ " Couples To must be greatter than couples From.";
        }

        if(strMsj=="")
        {
          int nRes = BREntities.OperationEntity(paymentSchema, enumMode);
          UIHelper.ShowMessageResult("Payment Schema", nRes);
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
    /// Cierra la ventanan verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (!ObjectHelper.IsEquals(paymentSchema, oldPaymentSchema))
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
    #endregion
  }
}

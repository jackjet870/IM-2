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
  /// Interaction logic for frmSourcePaymentDetail.xaml
  /// </summary>
  public partial class frmSourcePaymentDetail : Window
  {
    #region Variables
    public SourcePayment sourcePayment = new SourcePayment();//Objeto a guardar
    public SourcePayment oldSourcePayment = new SourcePayment();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    #endregion
    public frmSourcePaymentDetail()
    {
      InitializeComponent();
    }

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
      ObjectHelper.CopyProperties(sourcePayment, oldSourcePayment);
      UIHelper.SetUpControls(sourcePayment, this);
      if(enumMode!=EnumMode.preview)
      {
        btnAccept.Visibility = Visibility.Visible;
      }
      txtsbID.IsEnabled = (enumMode == EnumMode.add);
      txtsbN.IsEnabled = (enumMode != EnumMode.preview);
      chksbA.IsEnabled = (enumMode != EnumMode.preview);
      DataContext = sourcePayment;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/04/2016
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

    #region Accept
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo SourcePayments
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/04/2016
    /// [emoguel] modified 30/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode != EnumMode.add && ObjectHelper.IsEquals(sourcePayment, oldSourcePayment))
        {
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Source Payment");
          if (strMsj == "")
          {
            int nRes = await BREntities.OperationEntity<SourcePayment>(sourcePayment, enumMode);
            UIHelper.ShowMessageResult("Source Payment", nRes);
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
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Source Payment");
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
    /// [emoguel] created 26/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if(enumMode!=EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(sourcePayment, oldSourcePayment))
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
  }
}

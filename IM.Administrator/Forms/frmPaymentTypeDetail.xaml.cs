using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPaymentTypeDetail.xaml
  /// </summary>
  public partial class frmPaymentTypeDetail : Window
  {
    #region variables
    public PaymentType paymentType = new PaymentType();//Objeto a guardar
    public PaymentType oldPaymentType = new PaymentType();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    #endregion
    public frmPaymentTypeDetail()
    {
      InitializeComponent();
    }

    #region Window KeyDown
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
      ObjectHelper.CopyProperties(paymentType, oldPaymentType);
      DataContext = paymentType;
      txtID.IsEnabled = (enumMode != EnumMode.edit);
    } 
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando los cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (!ObjectHelper.IsEquals(paymentType, oldPaymentType))
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

    #region Accept
    /// <summary>
    /// Agregar|Actualiza un registro en el catalogo PaymentTypes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (ObjectHelper.IsEquals(paymentType, oldPaymentType) && enumMode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Payment Type");
        if (strMsj == "")
        {
          int nRes = BRPaymentTypes.SavePaymentType(paymentType, (enumMode == EnumMode.edit));
          UIHelper.ShowMessageResult("Payment Type", nRes);
          if (nRes == 1)
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
  }
}

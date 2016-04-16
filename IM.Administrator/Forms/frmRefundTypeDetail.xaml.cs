using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmRefundTypeDetail.xaml
  /// </summary>
  public partial class frmRefundTypeDetail : Window
  {
    #region
    public RefundType refundType = new RefundType();//Objeto a guardar
    public RefundType oldRefundType = new RefundType();//objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    #endregion
    public frmRefundTypeDetail()
    {
      InitializeComponent();
    }

    #region Window Loaded
    /// <summary>
    /// carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(refundType, oldRefundType);
      txtrfID.IsEnabled = (enumMode == EnumMode.add);
      UIHelper.SetMaxLength(refundType, this);
      DataContext = refundType;
    }
    #endregion

    #region Window keyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [created] 14/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Down)
      {
        btnCancel.Focus();
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda|Actuliza un registro en el catalogo RefundTypes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      if(enumMode!=EnumMode.add && ObjectHelper.IsEquals(refundType,oldRefundType))
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Refund Type");
        if(strMsj=="")
        {
          int nRes = BRRefundTypes.SaveRefundType(refundType, (enumMode == EnumMode.edit));
          UIHelper.ShowMessageResult("Refund Type", nRes);
          if(nRes==1)
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
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (!ObjectHelper.IsEquals(refundType, oldRefundType))
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

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
  /// Interaction logic for frmRefundTypeDetail.xaml
  /// </summary>
  public partial class frmRefundTypeDetail : Window
  {
    #region
    public RefundType refundType = new RefundType();//Objeto a guardar
    public RefundType oldRefundType = new RefundType();//objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    private bool _isClosing = false;
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
      txtrfID.IsEnabled = (enumMode == EnumMode.Add);
      UIHelper.SetUpControls(refundType, this);
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
        Close();
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
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(refundType, oldRefundType))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Refund Type");
          if (strMsj == "")
          {
            int nRes =await BREntities.OperationEntity(refundType, enumMode);
            UIHelper.ShowMessageResult("Refund Type", nRes);
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
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Refund Type");
      }
    } 
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica cambios pendientes antes de cerrar la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        if (!ObjectHelper.IsEquals(refundType, oldRefundType))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result != MessageBoxResult.Yes)
          {
            e.Cancel = true;
          }
        }
      }
    } 
    #endregion
  }
}

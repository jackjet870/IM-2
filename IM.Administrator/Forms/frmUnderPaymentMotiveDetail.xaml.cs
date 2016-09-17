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
  /// Interaction logic for frmUnderPaymentMotiveDetail.xaml
  /// </summary>
  public partial class frmUnderPaymentMotiveDetail : Window
  {
    #region variables
    public UnderPaymentMotive underPaymentMotive = new UnderPaymentMotive();//Objeto a guardar
    public UnderPaymentMotive oldUnderPaymentMOtive = new UnderPaymentMotive();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrió la ventana
    private bool _isClosing = false;
    #endregion
    public frmUnderPaymentMotiveDetail()
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
    /// [emoguel] created 28/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(underPaymentMotive, oldUnderPaymentMOtive);
      UIHelper.SetUpControls(underPaymentMotive, this);
      if(enumMode!=EnumMode.ReadOnly)
      {
        btnAccept.Visibility = Visibility.Visible;
        txtupN.IsEnabled = true;
        chkupA.IsEnabled = true;
      }
      DataContext = underPaymentMotive;

    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
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

    #region Accept
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo underPaymentMotives
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// [emoguel] modified 30/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(underPaymentMotive, oldUnderPaymentMOtive))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Under Payment Motive");
          if (strMsj == "")
          {
            int nRes =await BREntities.OperationEntity(underPaymentMotive, enumMode);
            UIHelper.ShowMessageResult("Under Payment Motives", nRes);
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
    /// [emoguel] created 28/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();      
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
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
          if (!ObjectHelper.IsEquals(underPaymentMotive, oldUnderPaymentMOtive))
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

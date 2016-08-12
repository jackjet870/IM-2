using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmCreditCardTypesDetail.xaml
  /// </summary>
  public partial class frmCreditCardTypesDetail : Window
  {
    public EnumMode mode;
    public CreditCardType creditCardType=new CreditCardType();//Objeto a guardar
    public CreditCardType oldCreditCard = new CreditCardType();//Objeto con los datos iniciales
    private bool _isClosing = false;
    public frmCreditCardTypesDetail()
    {
      InitializeComponent();
    }
    
    #region eventos de los controles
    #region windowLoaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(creditCardType, oldCreditCard);
      if (mode != EnumMode.ReadOnly)
      {
        txtccID.IsEnabled = (mode == EnumMode.Add);
        txtccN.IsEnabled = true;
        chkA.IsEnabled = true;
        chkIsCC.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
        UIHelper.SetUpControls(creditCardType, this);
      }      
      DataContext = creditCardType;
    }
    #endregion

    #region WindowKeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] 07/003/2016
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

    #region Accept
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (ObjectHelper.IsEquals(creditCardType, oldCreditCard) && mode != EnumMode.Add)
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string sMsj = ValidateHelper.ValidateForm(this, "Credit Card Type");
          int nRes = 0;
          if (sMsj == "")//Todos los campos estan llenos
          {
            nRes = await BREntities.OperationEntity<CreditCardType>(creditCardType, mode);
            UIHelper.ShowMessageResult("Credit Card Type", nRes);
            if (nRes > 0)
            {
              _isClosing = true;
              DialogResult = true;
              Close();
            }
          }
          else
          {//Hace falta llenar campos
            UIHelper.ShowMessage(sMsj);
          }
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Credit Card Types");
      }
    }

    #endregion
    #region Cancel
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      if (mode != EnumMode.ReadOnly)
      {
        if (!ObjectHelper.IsEquals(creditCardType, oldCreditCard))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            if (!_isClosing) { _isClosing = true; Close(); }
          }
          else
          {
            _isClosing = false;
          }
        }
        else
        {
          if (!_isClosing) { _isClosing = true; Close(); }
        }
      }
      else
      {
        if (!_isClosing) { _isClosing = true; Close(); }
      }
    }
    #endregion

    #endregion
    #region Closing

    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        _isClosing = true;
        btnCancel_Click(null, null);
        if (!_isClosing)
        {
          e.Cancel = true;
        }
        else
        {
          _isClosing = false;
        }
      }
    } 
    #endregion
  }
}

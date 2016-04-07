using System.Windows;
using System.Windows.Input;
using IM.Administrator.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

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
    public frmCreditCardTypesDetail()
    {
      InitializeComponent();
    }

    #region métodos
    #region OpenMode
    /// <summary>
    /// Abre la ventana dependiendo del modo que elija el usuario
    /// Preview|Edit|Add
    /// </summary>
    /// <history>
    /// [emoguel] 07/03/2016 Created
    /// </history>
    protected void OpenMode()
    {
      DataContext = creditCardType;
      switch (mode)
      {
        case EnumMode.preview://preview
          {
            btnAccept.Visibility = Visibility.Hidden;
            break;
          }
        case EnumMode.add://add
          {
            txtID.IsEnabled = true;
            LockControls(true);
            break;
          }
        case EnumMode.edit://Edit
          {
            txtID.IsEnabled = false;
            LockControls(true);
            break;
          }
      }

    }

    #endregion
    #region LockControls
    /// <summary>
    /// Bloquea|Desbloquea los botones dependiendo del modo en que se habra
    /// </summary>
    /// <param name="bValue">true para desbloquear| false para bloquear</param>
    /// <history>
    /// [emoguel] 07/03/2016 Created
    /// </history>
    protected void LockControls(bool blnValue)
    {
      txtN.IsEnabled = blnValue;
      chkIsCC.IsEnabled = blnValue;
      chkA.IsEnabled = blnValue;
    }

    #endregion
    #endregion
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
      OpenMode();
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
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(ObjectHelper.IsEquals(creditCardType,oldCreditCard) && mode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        string sMsj = ValidateHelper.ValidateForm(this, "Credit Card Type");
        int nRes = 0;
        if (sMsj == "")//Todos los campos estan llenos
        {
          nRes = BRCreditCardTypes.SaveCreditCardType(creditCardType, (mode == EnumMode.edit));
          UIHelper.ShowMessageResult("Credit Card Type", nRes);
          if(nRes==1)
          {
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

    #endregion
    #region Cancel
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (mode != EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(creditCardType, oldCreditCard))
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
    #endregion

  }
}

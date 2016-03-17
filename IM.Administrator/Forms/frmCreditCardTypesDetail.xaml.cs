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
    public ModeOpen mode;
    public CreditCardType creditCardType=new CreditCardType();
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
        case ModeOpen.preview://preview
          {
            btnAccept.Visibility = Visibility.Hidden;
            break;
          }
        case ModeOpen.add://add
          {
            txtID.IsEnabled = true;
            LockControls(true);
            break;
          }
        case ModeOpen.edit://Edit
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
        DialogResult = false;
        Close();
      }
    }
    #endregion

    #region Accept
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      string sMsj = "";
      #region validar campos
      if (string.IsNullOrWhiteSpace(txtID.Text))
      {
        sMsj += "Specify the Credit Card Type ID. \n";
      }

      if (string.IsNullOrWhiteSpace(txtN.Text))
      {
        sMsj += "Specify the Credit Card Type Description. \n";
      }

      #endregion
      int nRes = 0;
      if (sMsj == "")//Todos los campos estan llenos
      {
        switch (mode)
        {
          case ModeOpen.add://Agregar
            { 
              nRes = BRCreditCardTypes.SaveCreditCardType(creditCardType, false);
              break;
            }
          case ModeOpen.edit://Editar
            {
              nRes = BRCreditCardTypes.SaveCreditCardType(creditCardType, true);
              break;
            }
        }

        #region respuesta
        switch (nRes)//Se valida la respuesta de la operacion
        {
          case 0:
            {
              UIHelper.ShowMessage("Credit Card Type not saved");
              break;
            }
          case 1:
            {
              UIHelper.ShowMessage("Credit Card Type successfully saved");
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              UIHelper.ShowMessage("Credit Card Type ID already exist please select another one");
              break;
            }
        }
        #endregion
      }
      else
      {//Hace falta llenar campos
        UIHelper.ShowMessage(sMsj.TrimEnd('\n'));
      }
    }

    #endregion
    #region Cancel
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close();
    } 
    #endregion
    #endregion

  }
}

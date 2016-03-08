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
using IM.Administrator.Enums;
using IM.Model;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmCreditCardTypesDetail.xaml
  /// </summary>
  public partial class frmCreditCardTypesDetail : Window
  {
    public ModeOpen mode;
    public CreditCardType creditCardType;
    public frmCreditCardTypesDetail()
    {
      InitializeComponent();
    }

    #region métodos
    /// <summary>
    /// Abre la ventana dependiendo del modo que elija el usuario
    /// Preview|Edit|Add
    /// </summary>
    /// <history>
    /// [emoguel] 07/03/2016 Created
    /// </history>
    protected void OpenMode()
    {
      switch (mode)
      {
        case ModeOpen.preview://preview
          {
            btnAccept.Visibility = Visibility.Hidden;
            btnCancel.Content = "OK";
            DataContext = creditCardType;
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
            DataContext = creditCardType;
            break;
          }
      }

    }

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
    #region eventos de los controles
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      OpenMode();
    }

    /// <summary>
    /// Cierra la ventana con el boton escape dependiendo del modo en que abrio
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] 07/003/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key==Key.Escape)
      {
        if(mode==ModeOpen.preview)
        {          
          this.Close();
        }
        else
        {
          MessageBoxResult msgResult = MessageBox.Show("Are you sure close this window?","Close confirmation",MessageBoxButton.OKCancel,MessageBoxImage.Question);
          if(msgResult==MessageBoxResult.OK)
          {
            DialogResult = false;
            this.Close();
          }
        }
      }
    }

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

              creditCardType = new CreditCardType {ccID=txtID.Text,ccN=txtN.Text,ccA=chkA.IsChecked.Value,ccIsCreditCard=chkIsCC.IsChecked.Value };
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
              MessageBox.Show("Credit Card Type not saved", "", MessageBoxButton.OK, MessageBoxImage.Warning);
              break;
            }
          case 1:
            {
              MessageBox.Show("Credit Card Type successfully saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              MessageBox.Show("Credit Card Type ID already exist please select another one", "", MessageBoxButton.OK, MessageBoxImage.Warning);
              break;
            }
        }
        #endregion
      }
      else
      {//Hace falta llenar campos
        MessageBox.Show(sMsj);
      }
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      this.Close();
    }
    #endregion

  }
}

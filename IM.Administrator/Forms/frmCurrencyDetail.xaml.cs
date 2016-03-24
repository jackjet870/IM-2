using System.Windows;
using System.Windows.Input;
using IM.Administrator.Enums;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmCurrrencyDetail.xaml
  /// </summary>
  public partial class frmCurrencyDetail : Window
  {
    public Currency currency=new Currency();//objeto para agrear|actualizar
    public EnumMode mode;//modo en el que se abrirá la ventana add|preview|edit 
    public frmCurrencyDetail()
    {
      InitializeComponent();
    }

    #region event controls
    #region WindowKeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/03/2016
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

    #region Loaded
    /// <summary>
    /// llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      OpenMode();
    }
    #endregion
    #region Accept

    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      string sMsj = "";
      int nRes = 0;
      #region validar campos
      if (string.IsNullOrWhiteSpace(txtID.Text))
      {
        sMsj += "Specify the Currency ID. \n";
      }
      if (string.IsNullOrWhiteSpace(txtN.Text))
      {
        sMsj += "Specify the Currency Description.";
      }
      #endregion

      if (sMsj == "")//Validar si hay cmapos vacios
      {

        nRes = BRCurrencies.saveCurrency(currency, (mode==EnumMode.edit));
        
        #region respuesta
        switch (nRes)
        {
          case 0:
            {
              UIHelper.ShowMessage("Currency not saved");
              break;
            }
          case 1:
            {
              UIHelper.ShowMessage("Currency successfully saved");
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
             UIHelper.ShowMessage("Currency ID already exist please select another one");
              break;
            }
        }
        #endregion
      }
      else
      {
       UIHelper.ShowMessage(sMsj.TrimEnd('\n'));
      }

    }

    #endregion
    
    #endregion

    #region metods
    #region OpenMode
    /// <summary>
    /// Abre la ventana dependiendo del modo que elija el usuario
    /// Preview|Edit|Add
    /// </summary>
    /// <history>
    /// [emoguel] 08/03/2016 Created
    /// </history>
    protected void OpenMode()
    {
      DataContext = currency;
      switch (mode)
      {
        case EnumMode.preview://show
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
    /// [emoguel] created 08/03/2016
    /// </history>
    protected void LockControls(bool blnValue)
    {
      txtN.IsEnabled = blnValue;
      chkA.IsEnabled = blnValue;
    } 
    #endregion
    #endregion
  }
}

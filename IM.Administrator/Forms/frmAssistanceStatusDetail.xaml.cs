using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAssistanceStatusDetail.xaml
  /// </summary>
  public partial class frmAssistanceStatusDetail : Window
  {
    public EnumMode mode;
    public AssistanceStatus assistance=new AssistanceStatus();
    public frmAssistanceStatusDetail()
    {
      InitializeComponent();
    }

    #region metodos
    #region  Modo de la ventana
    /// <summary>
    /// Abre la ventana dependiendo del modo que elija el usuario
    /// Preview|Edit|Add
    /// </summary>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    protected void OpenMode()
    {
      DataContext = assistance;
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
    /// <param name="blnValue">true para desbloquear| false para bloquear</param>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    protected void LockControls(bool blnValue)
    {
      txtN.IsEnabled = blnValue;
      chkA.IsEnabled = blnValue;
    }
    #endregion
    #endregion

    #region eventos de controles
    #region Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      OpenMode();
    }

    #endregion
    #region Accept click
    /// <summary>
    /// Agrega o actualiza los registros del catalogo AssistantStatus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {

      string sMsj = "";
      int nRes = 0;
      #region validar campos
      if (string.IsNullOrWhiteSpace(txtID.Text))
      {
        sMsj += "Specify the Assistance Status ID. \n";
      }
      if (string.IsNullOrWhiteSpace(txtN.Text))
      {
        sMsj += "Specify the Assistance Status Description.";
      }
      #endregion

      if (sMsj == "")//Validar si hay cmapos vacios
      {
        nRes = BRAssistancesStatus.SaveAssitanceStatus((mode==EnumMode.edit), assistance);        

        #region respuesta
        switch (nRes)
        {
          case 0:
            {
              UIHelper.ShowMessage("Assistance Status not saved");
              break;
            }
          case 1:
            {
              UIHelper.ShowMessage("Assistance Status successfully saved");
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              UIHelper.ShowMessage("Assistance Status ID already exist please select another one");
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
    
    #region KeyDown Form
    /// <summary>
    /// Cierra la ventana dependiendo del modo en el que fue abierta
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        DialogResult = false;
        Close();        
      }
    } 
    #endregion
    #endregion
  }
}

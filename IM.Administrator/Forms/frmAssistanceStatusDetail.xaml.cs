using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;
namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAssistanceStatusDetail.xaml
  /// </summary>
  public partial class frmAssistanceStatusDetail : Window
  {
    public ModeOpen mode;
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
        case ModeOpen.preview://show
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
        switch (mode)
        {
          #region insert
          case ModeOpen.add://add
            { 
              nRes = BRAssistancesStatus.SaveAssitanceStatus(false, assistance);

              break;
            }
          #endregion
          #region edit
          case ModeOpen.edit://Edit
            {
              nRes = BRAssistancesStatus.SaveAssitanceStatus(true, assistance);
              break;
            }
            #endregion
        }

        #region respuesta
        switch (nRes)
        {
          case 0:
            {
              MessageBox.Show("Assistance Status not saved", "", MessageBoxButton.OK, MessageBoxImage.Warning);
              break;
            }
          case 1:
            {
              MessageBox.Show("Assistance Status successfully saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              MessageBox.Show("Assistance Status ID already exist please select another one", "", MessageBoxButton.OK, MessageBoxImage.Warning);
              break;
            }
        }
        #endregion
      }
      else
      {
        MessageBox.Show(sMsj.TrimEnd('\n'),"Inteligense Marketing");
      }

    }

    #endregion


    #region Cancel click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = false;
      this.Close();
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

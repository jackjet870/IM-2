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
    public AssistanceStatus assistance=new AssistanceStatus();//Objeto a guardar|actualizar
    public AssistanceStatus oldAssistance = new AssistanceStatus();//Objeto con los datos iniciales
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
      ObjectHelper.CopyProperties(assistance, oldAssistance);
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
      btnAccept.Focus();
      if(ObjectHelper.IsEquals(assistance,oldAssistance) && mode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        string sMsj = ValidateHelper.ValidateForm(this,"Assistance Status");
        int nRes = 0;        

        if (sMsj == "")//Validar si hay cmapos vacios
        {
          nRes = BRAssistancesStatus.SaveAssitanceStatus((mode == EnumMode.edit), assistance);

          UIHelper.ShowMessageResult("Assistance Status", nRes);
          if(nRes==1)
          {
            Close();
          }
        }
        else
        {
          UIHelper.ShowMessage(sMsj);
        }
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
        btnCancel.Focus();
        btnCancel_Click(null, null);       
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana pero antes verifica que no se tengan cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if(mode!=EnumMode.preview)
      {        
        if (!ObjectHelper.IsEquals(assistance, oldAssistance))
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

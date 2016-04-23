using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Enums;
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
      DataContext = assistance;
      if(mode!=EnumMode.preview)
      {
        txtatN.IsEnabled = true;
        chkA.IsEnabled = true;
        txtatID.IsEnabled = (mode == EnumMode.add);
        btnAccept.Visibility = Visibility.Visible;
        UIHelper.SetUpControls(assistance, this);
      }
      
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

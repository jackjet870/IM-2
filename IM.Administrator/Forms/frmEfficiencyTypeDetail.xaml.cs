using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmEfficiencyTypeDetail.xaml
  /// </summary>
  public partial class frmEfficiencyTypeDetail : Window
  { 
    public EfficiencyType efficiencyType = new EfficiencyType();//objeto a guardar|actualizar
    public EfficiencyType oldEffType = new EfficiencyType();//objeto con los datos inciales
    public EnumMode enumMode;//Modo en que se abre la ventana;

    public frmEfficiencyTypeDetail()
    {
      InitializeComponent();
    }
    #region Eventos del formulario
    #region Window KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
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
    #region Window Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(efficiencyType, oldEffType);
      DataContext = efficiencyType;
      if (enumMode != EnumMode.preview)
      {
        txtetN.IsEnabled = true;
        chkA.IsEnabled = true;
        txtetID.IsEnabled = (enumMode == EnumMode.add);
        btnAccept.Visibility = Visibility.Visible;
        UIHelper.SetUpControls(efficiencyType, this);
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda o Actualiza dependiendo del modo de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(ObjectHelper.IsEquals(efficiencyType,oldEffType)&& enumMode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        int nRes = 0;
        string strMsj = ValidateHelper.ValidateForm(this, "Efficiency Type");
        if (strMsj == "")
        {
          nRes = BREntities.OperationEntity(efficiencyType, enumMode);
          UIHelper.ShowMessageResult("Efficiency Type", nRes);
          if(nRes>0)
          {
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
      if (enumMode!=EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(efficiencyType, oldEffType))
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

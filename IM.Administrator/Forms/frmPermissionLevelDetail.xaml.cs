using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPermissionLevelDetail.xaml
  /// </summary>
  public partial class frmPermissionLevelDetail : Window
  {
    #region Variables
    public PermissionLevel permissionLevel = new PermissionLevel();//Objeto a guardar
    public PermissionLevel oldPermissionLevel = new PermissionLevel();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    #endregion
    public frmPermissionLevelDetail()
    {
      InitializeComponent();
    }

    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(permissionLevel, oldPermissionLevel);      
      UIHelper.SetUpControls(permissionLevel, this);
      DataContext = permissionLevel;
      if (enumMode == EnumMode.add)
      {
        txtplID.IsEnabled = true;
      }
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/04/2016
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

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando los cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if(!ObjectHelper.IsEquals(permissionLevel,oldPermissionLevel))
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
    #endregion

    #region Accept
    /// <summary>
    /// Agrega|Actualiza un registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (ObjectHelper.IsEquals(permissionLevel, oldPermissionLevel) && enumMode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Permission Level");
        if(permissionLevel.plID>0)
        {
          strMsj += (strMsj == "") ? "" : " \n " + "Permission Level can not be 0.";
        }
        if (strMsj=="")
        {
          int nRes = BRPermissionsLevels.SavePermissions(permissionLevel, (enumMode == EnumMode.edit));
          UIHelper.ShowMessageResult("Permission Level", nRes);
          if(nRes==1)
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
  }
}

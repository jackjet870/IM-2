using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmRoleDetail.xaml
  /// </summary>
  public partial class frmRoleDetail : Window
  {
    #region variables
    public Role role = new Role();//objeto a guardar
    public Role oldRole = new Role();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    private bool _isClosing = false;
    #endregion
    public frmRoleDetail()
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
    /// [emoguel] created 19/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(role, oldRole);
      DataContext = role;
      txtroID.IsEnabled = (enumMode == EnumMode.Add);
      UIHelper.SetUpControls(role, this);
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventan con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        Close();
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda|Actualiza un registro en el catalogo Roles
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// [emoguel] modified 30/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(role, oldRole))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Roles");
          if (strMsj == "")
          {
            int nRes =await BREntities.OperationEntity(role, enumMode);
            UIHelper.ShowMessageResult("Roles", nRes);
            if (nRes > 0)
            {
              _isClosing = true;
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
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica cambios antes de cerrar la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if(!_isClosing)
      {
        if (!ObjectHelper.IsEquals(role, oldRole))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result != MessageBoxResult.Yes)
          {
            e.Cancel = true;
          }
        }
      }
    } 
    #endregion
  }
}

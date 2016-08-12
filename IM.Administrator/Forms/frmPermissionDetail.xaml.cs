using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPermissionDetail.xaml
  /// </summary>
  public partial class frmPermissionDetail : Window
  {
    #region Variables
    public Permission permission = new Permission();//objeto a guardar
    public Permission oldPermission = new Permission();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana  
    private bool _isClosing = false;
    #endregion
    public frmPermissionDetail()
    {
      InitializeComponent();
    }

    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(permission, oldPermission);
      DataContext = permission;
      txtpmID.IsEnabled = (enumMode == EnumMode.Add);
      UIHelper.SetUpControls(permission, this);
    } 
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
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
    /// Guarda|Actualiza un registro en el catalogo Permissions
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// [emoguel] modified 30/05/2016 sel volvió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (ObjectHelper.IsEquals(permission, oldPermission) && enumMode != EnumMode.Add)
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Permission");
          if (strMsj == "")
          {
            int nRes = await BREntities.OperationEntity(permission, enumMode);
            UIHelper.ShowMessageResult("Permission", nRes);
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
      }catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Permission");
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        if (!ObjectHelper.IsEquals(permission, oldPermission))
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

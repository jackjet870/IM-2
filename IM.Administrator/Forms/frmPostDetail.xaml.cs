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
  /// Interaction logic for frmPostDetail.xaml
  /// </summary>
  public partial class frmPostDetail : Window
  {
    #region Variables
    public Post post = new Post();//Objeto a guardar
    public Post oldPost = new Post();//objeto con los datos iniciales de la ventana
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    private bool _isClosing = false;
    #endregion
    public frmPostDetail()
    {
      InitializeComponent();
    }
    #region Window Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] creted 11/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(post, oldPost);
      DataContext = post;
      if (enumMode != EnumMode.preview)
      {
        chkA.IsEnabled = true;
        txtpoN.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
        txtpoID.IsEnabled = (enumMode == EnumMode.add);
      }
      UIHelper.SetUpControls(post, this);
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
        Close();
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Agrega|Actualiza registros en el catalogo Posts
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// [emoguel] modified 30/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode != EnumMode.add && ObjectHelper.IsEquals(post, oldPost))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Post");
          if (strMsj == "")
          {
            int nRes = await BREntities.OperationEntity(post, enumMode);
            UIHelper.ShowMessageResult("Post", nRes);
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
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Permission");
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verficando cambios en la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();      
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica cambios pendientes antes de cerrar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        if (enumMode != EnumMode.preview)
        {
          if (!ObjectHelper.IsEquals(post, oldPost))
          {
            MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
            if (result == MessageBoxResult.Yes)
            {
              e.Cancel = true;
            }
          }
        }
      }
    } 
    #endregion
  }
}

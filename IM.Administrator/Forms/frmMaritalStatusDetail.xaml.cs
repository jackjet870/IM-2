using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmMaritalStatusDetail.xaml
  /// </summary>
  public partial class frmMaritalStatusDetail : Window
  {
    #region
    public MaritalStatus maritalStatus = new MaritalStatus();//Objeto a guardar|actualizar
    public MaritalStatus oldMaritalStatus = new MaritalStatus();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en el que se abrirá la ventana
    #endregion
    public frmMaritalStatusDetail()
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
    /// [emoguel] created 01/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(maritalStatus, oldMaritalStatus);
      DataContext = maritalStatus;
      if (enumMode != EnumMode.preview)
      {
        btnAccept.Visibility = Visibility.Visible;
        txtmsID.IsEnabled = (enumMode==EnumMode.add);
        txtmsN.IsEnabled = true;
        chkmsA.IsEnabled = true;
        UIHelper.SetMaxLength(maritalStatus, this);
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
    /// [emoguel] created 01/04/2016
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

    #region Accept
    /// <summary>
    /// Guarda|actualiza un registro en el catalogo MSStatus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      if(enumMode!=EnumMode.add && ObjectHelper.IsEquals(maritalStatus,oldMaritalStatus))
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Marital Status");
        if(strMsj=="")
        {
          int nRes = BRMaritalStatus.SaveMaritalStatus(maritalStatus, (enumMode == EnumMode.edit));
          UIHelper.ShowMessageResult("Marital Status",nRes);
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

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando los cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (enumMode != EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(maritalStatus, oldMaritalStatus))
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
  }
}

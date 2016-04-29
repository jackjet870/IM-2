using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.Model;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmLanguageDetail.xaml
  /// </summary>
  public partial class frmLanguageDetail : Window
  {
    #region Variables
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    public Language language=new Language();//Objeto a guardar| actualizar
    public Language oldLanguage=new Language { laMrMrs= "Mr. & Mrs.",laRoom= "Room" };//Objeto con los datos iniciales 
    #endregion
    public frmLanguageDetail()
    {
      InitializeComponent();
    }

    #region WindowLoaded
    /// <summary>
    /// Carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(language, oldLanguage);
      DataContext = language;
      if (enumMode != EnumMode.preview)
      {
        txtlaN.IsEnabled = true;
        chkA.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
        if (enumMode == EnumMode.add)
        {
          txtlaID.IsEnabled = true;
        }
        UIHelper.SetUpControls(language, this);
      }

    } 
    #endregion

    #region WindowKeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/03/2016
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
    /// Cierra la ventana verificando que no se tengan cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/03/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (enumMode != EnumMode.preview && enumMode != EnumMode.search)
      {
        if (!ObjectHelper.IsEquals(language, oldLanguage))
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

    #region Accept
    /// <summary>
    /// Actualiza | Guarda un registro en el catalogo languages
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (ObjectHelper.IsEquals(language, oldLanguage) && enumMode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Language");
        int nRes = 0;

        if (strMsj == "")//Validar si hay cmapos vacios
        {
          nRes = BREntities.OperationEntity(language, enumMode);
          UIHelper.ShowMessageResult("Language", nRes);
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
  }
}

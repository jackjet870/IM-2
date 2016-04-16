using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmRateTypeDetail.xaml
  /// </summary>
  public partial class frmRateTypeDetail : Window
  {
    #region variables
    public RateType rateType = new RateType();//Objeto a guardar
    public RateType oldRateType = new RateType();//Objeto con los datos iniciales del objeto
    public EnumMode enumMode;//Modo de la ventana
    #endregion
    public frmRateTypeDetail()
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
    /// [emoguel] created 14/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(rateType, oldRateType);
      UIHelper.SetMaxLength(rateType, this);
      DataContext = rateType;
    }
    #endregion

    #region Key Down
    /// <summary>
    /// Cierra ka ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
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
    /// Guarda| ACtualiza un registro en el catalogo Rate Types
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(enumMode!=EnumMode.add && ObjectHelper.IsEquals(rateType,oldRateType))
      {
        Close();
      }
      else
      {
        string strMsj = "";
        strMsj = ValidateHelper.ValidateForm(this, "Rate Type");
        if(strMsj=="")
        {
          int nRes= BRRateTypes.SaveRateType(rateType, (enumMode == EnumMode.edit));
          UIHelper.ShowMessageResult("Rate Type", nRes);
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
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (!ObjectHelper.IsEquals(rateType, oldRateType))
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
  }
}

using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmNotBookingMotiveDetail.xaml
  /// </summary>
  public partial class frmNotBookingMotiveDetail : Window
  {
    #region variables
    public NotBookingMotive notBookingMotive = new NotBookingMotive();//Objeto a guardar
    public NotBookingMotive oldNotBookingMotive = new NotBookingMotive();//objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventada
    #endregion

    public frmNotBookingMotiveDetail()
    {
      InitializeComponent();
    }

    #region Window Loaded
    /// <summary>
    /// carga los datos de la venatana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(notBookingMotive, oldNotBookingMotive);
      DataContext = notBookingMotive;
      txtN.IsEnabled = (enumMode != EnumMode.preview);
      chkA.IsEnabled = (enumMode != EnumMode.preview);
      if(enumMode!=EnumMode.preview)
      {
        btnAccept.Visibility = Visibility.Visible;
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
    /// [emoguel] created 05/04/2016
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
    /// Guarda|Actualiza un registro en el catalogo NotBookingMotives
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (ObjectHelper.IsEquals(notBookingMotive, oldNotBookingMotive) && enumMode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Not Booking Motive");
        if (strMsj == "")
        {
          int nRes = BRNotBookingMotives.SaveNotBookingMotive(notBookingMotive, (enumMode == EnumMode.edit));
          UIHelper.ShowMessageResult("Not Booking Motive", nRes);
          if (nRes == 1)
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
    /// [emoguel] created 05/04/2014
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (enumMode != EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(notBookingMotive, oldNotBookingMotive))
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

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
  /// Interaction logic for frmNotBookingMotiveDetail.xaml
  /// </summary>
  public partial class frmNotBookingMotiveDetail : Window
  {
    #region variables
    public NotBookingMotive notBookingMotive = new NotBookingMotive();//Objeto a guardar
    public NotBookingMotive oldNotBookingMotive = new NotBookingMotive();//objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventada
    private bool _isClosing = false;
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
      txtnbN.IsEnabled = (enumMode != EnumMode.ReadOnly);
      chknbA.IsEnabled = (enumMode != EnumMode.ReadOnly);
      if(enumMode!=EnumMode.ReadOnly)
      {
        btnAccept.Visibility = Visibility.Visible;
        UIHelper.SetUpControls(notBookingMotive, this);
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
        Close();
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
    /// [emoguel] modified 30/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (ObjectHelper.IsEquals(notBookingMotive, oldNotBookingMotive) && enumMode != EnumMode.Add)
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Not Booking Motive");
          if (strMsj == "")
          {
            int nRes = await BREntities.OperationEntity(notBookingMotive, enumMode);
            UIHelper.ShowMessageResult("Not Booking Motive", nRes);
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
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Not Booking Motives");
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
      btnCancel.Focus();   
      Close();
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica cambios en la ventana antes de cerrar
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
        if (enumMode != EnumMode.ReadOnly)
        {
          if (!ObjectHelper.IsEquals(notBookingMotive, oldNotBookingMotive))
          {
            MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
            if (result != MessageBoxResult.Yes)
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

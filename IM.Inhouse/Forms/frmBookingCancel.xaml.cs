using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using System;
using System.Windows;

namespace IM.Inhouse.Forms
{
  /// <summary>
  /// Formulario para cancelar una invitacion
  /// </summary>
  /// <history>
  /// [jorcanche] 30/03/2016 created
  /// </history> 
  public partial class frmBookingCancel : Window
  {
    #region Atributos

    readonly UserLogin _user;
    readonly int _guestId;
    private Guest _guest;
    public bool? Cancelado;

    #endregion

    #region Contructores y Destructores
    public frmBookingCancel(int guestId, UserLogin user)
    {
      InitializeComponent();
      _user = user;
      _guestId = guestId;
      lblUserName.Text = user.peN;
      Title = $"Booking Cancellation - Guest ID: {guestId}";
    }
    #endregion

    #region btnLog_Click
    /// <summary>
    /// Habre el formulario de Log 
    /// </summary>
    /// <history>
    /// [jorcanche] 30/03/2016 created
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      var frmGuestLog = new frmGuestLog(_guestId) {Owner = this};
      frmGuestLog.ShowDialog();
    }

    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Modifica la variable de cancelado del booking y cierra
    /// </summary>
    /// <history>
    /// [jorcanche] 30/03/2016 created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      Cancelado = chkguBookCanc.IsChecked;
      Close();
    }
    #endregion

    #region btnOK_Click
    /// <summary>
    /// Valida  y guarda los cambios
    /// </summary>
    /// <history>
    /// [jorcanche] 30/03/2016 created
    /// </history>
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      ValidateSave();  
    }
    #endregion

    #region ValidateSave

    /// <summary>
    /// Guarda los cambios
    /// </summary>
    /// <history>
    /// [jorcanche] 30/03/2016 created
    /// </history>
    private async void ValidateSave()
    {
      try
      {
        //Invertimos el valor del Check si cancelo o lo descancelo
        Cancelado = chkguBookCanc.IsChecked = chkguBookCanc.IsChecked != null && !chkguBookCanc.IsChecked.Value;
        //Guardamos el BookCanceled
        _guest.guBookCanc = Cancelado.Value;

        //Enviamos los parametros para que guarde los cambios del guest y el log del Guest.
        //Si hubo un erro al ejecutar el metodo SaveChangedOfGuest nos devolvera 0, indicando que ningun paso 
        //se realizo, es decir ni se guardo el Guest ni el Log, y siendo así ya no modificamos la variable
        //_wasSaved que es el que indica que se guardo el Avail.
        if (await BRGuests.SaveChangedOfGuest(_guest, Context.User.LeadSource.lsHoursDif, _user.peID) == 0)
        {
          //De no ser así informamos que no se guardo la información por algun motivo
          UIHelper.ShowMessage("There was an error saving the information, consult your system administrator",
            MessageBoxImage.Error, "Information can not keep");
          //Regresamos el Valor a como estaba
          Cancelado = chkguBookCanc.IsChecked = !chkguBookCanc.IsChecked.Value;
        }
        Close();  
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
     
    }
    #endregion

    #region Window_Closed
    /// <summary>
    /// Cierra el formualario 
    /// </summary>
    /// <history>
    /// [jorcanche]  created 06072016
    /// </history>
    private void Window_Closed(object sender, EventArgs e)
    {
      Cancelado = chkguBookCanc.IsChecked;
    }
    #endregion

    #region FrmBookingCancel_OnLoaded
    /// <summary>
    /// Carga la variable de Guest
    /// </summary>
    /// <history>
    /// [jorcanche]  created 06072016
    /// </history>
    private async void FrmBookingCancel_OnLoaded(object sender, RoutedEventArgs e)
    {
      _guest = await BRGuests.GetGuest(_guestId);
      chkguBookCanc.IsChecked = _guest.guBookCanc;
    } 
    #endregion
  }
}

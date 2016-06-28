using System;
using System.Windows;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Base.Forms;

namespace IM.Inhouse.Forms
{
  /// <summary>
  /// Interaction logic for frmBookingCancel.xaml
  /// Formulario para cancelar una invitacion
  /// </summary>
  public partial class frmBookingCancel : Window
  {

    #region Atributos

    UserLogin _user;
    int _guestID;
    Guest _guest;
    public bool? _cancelado;

    #endregion

    #region Contructores y Destructores
    public frmBookingCancel(int guestID, UserLogin user)
    {
      InitializeComponent();
      this._user = user;
      this._guestID = guestID;
      _guest = BRGuests.GetGuest(guestID);
      lblUserName.Text = user.peN;
      chkguBookCanc.IsChecked = _guest.guBookCanc;
    }
    #endregion

    #region btnLog_Click

    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      frmGuestLog frmGuestLog = new frmGuestLog(_guestID, App.User.LeadSource.lsN);
      frmGuestLog.Owner = this;
      frmGuestLog.ShowDialog();
    }

    #endregion

    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      _cancelado = chkguBookCanc.IsChecked;
      this.Close();
    }
    #endregion

    #region btnOK_Click
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      ValidateSave();
    }
    #endregion

    #region ValidateSave
   
    /// <summary>
    /// Guarda los cambios
    /// </summary>
    /// <history>[jorcanche] 30/03/2016</history>
    private void ValidateSave()
    {
      //Invertimos el valor del Check si cancelo o lo descancelo
      _cancelado = chkguBookCanc.IsChecked = !chkguBookCanc.IsChecked.Value;
      //Guardamos el BookCanceled
      _guest.guBookCanc = _cancelado.Value;
      
      //Enviamos los parametros para que guarde los cambios del guest y el log del Guest.
      //Si hubo un erro al ejecutar el metodo SaveChangedOfGuest nos devolvera 0, indicando que ningun paso 
      //se realizo, es decir ni se guardo el Guest ni el Log, y siendo así ya no modificamos la variable
      //_wasSaved que es el que indica que se guardo el Avail.
      if (BRGuests.SaveChangedOfGuest(_guest, App.User.LeadSource.lsHoursDif, _user.peID) == 0)
      {
        //De no ser así informamos que no se guardo la información por algun motivo
        UIHelper.ShowMessage("There was an error saving the information, consult your system administrator",
          MessageBoxImage.Error, "Information can not keep");
        //Regresamos el Valor a como estaba
        _cancelado = chkguBookCanc.IsChecked = !chkguBookCanc.IsChecked.Value;
      }
      this.Close();

      //BRGuests.SaveGuest(_guest);
      //BRGuestsLogs.SaveGuestLog(_guestID, App.User.LeadSource.lsHoursDif, _user.peID);
    }
    #endregion

    #region Window_Closed
    private void Window_Closed(object sender, EventArgs e)
    {
      _cancelado = chkguBookCanc.IsChecked;
    } 
    #endregion
  }
}

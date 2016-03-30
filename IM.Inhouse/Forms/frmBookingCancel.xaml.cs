using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.Model;
using IM.BusinessRules.BR;

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
      frmGuestLog frmGuestLog = new frmGuestLog(_guestID);
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
      _cancelado = chkguBookCanc.IsChecked = chkguBookCanc.IsChecked.Value ? false : true;

      //Guardamos el BookCanceled
      _guest.guBookCanc = _cancelado.Value;
      BRGuests.SaveGuest(_guest);

      //guardamos la informacion de contacto
      BRGuestsLogs.SaveGuestLog(_guestID, App.User.LeadSource.lsHoursDif, _user.peID);

      this.Close();
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

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.Model.Classes;
using System.Windows.Input;
using System.Linq;
using IM.Base.Forms;
using IM.Model;
using System.Collections.Generic;
using IM.Outhouse.Classes;

namespace IM.Outhouse.Forms
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class frmOuthouse
  {
    #region Atributos

    private CollectionViewSource _outPremanifestViewSource;
    private DateTime _serverDate;
    private bool _bookInvit = true;

    #endregion

    public frmOuthouse()
    {
      InitializeComponent();
    }

    #region ValidateContact
    /// <summary>
    /// Valida los datos para desplegar el formulario de contactacion
    /// </summary>
    /// <param name="checkIn">Si ya hizo CheckIn</param>
    /// <param name="contact"> Si ya esta contactado</param>
    /// <param name="checkOutD">Fecha de contactación</param>
    /// <returns></returns>
    ///<history>[jorcanche] 13/03/2016</history>
    private bool ValidateContact(bool checkIn, bool contact, DateTime checkOutD)
    {
      //validamos que el huesped haya hecho Check In
      if (!checkIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-in.", MessageBoxImage.Asterisk);
        return false;
      }
      // no se permite contactar si ya hizo Check Out o si ya esta contactado el Guest
      if (!contact && checkOutD < BRHelpers.GetServerDate().Date)
      {
        UIHelper.ShowMessage("Guest already made Check-out.", MessageBoxImage.Asterisk);
        return false;
      }
      //validamos que el usuario tenga permiso de lectura
      if (!App.User.HasPermission(EnumPermission.Register, EnumPermisionLevel.ReadOnly))
      {
        UIHelper.ShowMessage("Access denied.", MessageBoxImage.Asterisk);
        return false;
      }
      return true;
    }
    #endregion

    private void dtpDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      //Obtener el valor actual del que tiene dtpDate
      var picker = sender as Xceed.Wpf.Toolkit.DateTimePicker;
      if (picker != null && !picker.Value.HasValue)
      {
        //cuando el ususario ingresa un afecha invalida
        MessageBox.Show("Specify  the Date", "Date invalidates", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //Y le asignamos la fecha del servidor (la hora actual)
        dtpDate.Value = BRHelpers.GetServerDate();
      }
      else
      {
        //le asignamos el valor del dtpDate a la variable global para que otro control tenga acceso al valor actual
        if (picker != null) _serverDate = picker.Value.Value;
        //Cargamos el grid 
        LoadGrid();
      }
    }
    #region LoadGrid
    private void LoadGrid()
    {
      if (_outPremanifestViewSource != null)
        _outPremanifestViewSource.Source = BRGuests.GetGuestPremanifestOuthouse(_bookInvit, _serverDate, App.User.Location.loID);
    }
    #endregion

    private void rbt_Checked(object sender, RoutedEventArgs e)
    {
      var rb = sender as RadioButton;
      if (rb != null) _bookInvit = Convert.ToBoolean(rb.TabIndex);
      LoadGrid();
    }

    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
      LoadGrid();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadOuthouse();
    }

    private void LoadOuthouse()
    {
      //Cargamos las variables del usuario
      txtUser.Text = App.User.User.peN;
      txtLocation.Text = App.User.Location.loN;

      //Cargamos la fecha actual del servidor
      dtpDate.Value = BRHelpers.GetServerDate().Date;

      //Inicializamos la variable del datagrid
      _outPremanifestViewSource = (CollectionViewSource)FindResource("outPremanifestViewSource");

      //Indicamos al statusbar que me muestre cierta informacion cuando oprimimos cierto teclado
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);

      //Cargamos el DataGrid  
      LoadGrid();
    }
    private void Info_Click(object sender, RoutedEventArgs e)
    {
      var chkguInfo = sender as CheckBox;
      var outPre = dgGuestPremanifest.Items[dgGuestPremanifest.Items.CurrentPosition] as GuestPremanifestOuthouse;
      //Invertimos el valor del Check para que no se refleje si no que hasta que se halla terminado la solicitud 
      chkguInfo.IsChecked = !chkguInfo.IsChecked.Value;
      if (outPre != null && ValidateContact(outPre.guCheckIn, outPre.guInfo, outPre.guCheckOutD))
      {
        StaStart("Loading Contact´s Info...");
        frmContact frmCont = new frmContact(outPre.guID, App.User);
        frmCont.Owner = this;
        frmCont.ShowInTaskbar = false;
        StaEnd();
        if (!frmCont.ShowDialog().Value)
        {
          if (frmCont._wasSave)
          {
            StaStart("Save Contact's Info...");
            dgGuestPremanifest.SelectedItems.OfType<GuestPremanifestOuthouse>().ToList().ForEach(item =>
            { item.guPRInfo = frmCont.PRInfo; item.guInfoD = frmCont.InfoD; item.guCheckIn = true; item.guInfo = true; });
            dgGuestPremanifest.Items.Refresh();
            StaEnd();
          }
        }
        StaEnd();
      }
      StaEnd();
    }

    #region StaStart
    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <param name="message">mensaje a mostrar</param>
    /// <history>[jorcanche] 05/04/2016 Created </history>
    private void StaStart(string message)
    {
      lblStatusBarMessage.Text = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      this.Cursor = Cursors.Wait;
    }
    #endregion

    #region StaEnd
    /// <summary>
    /// Indica en la barra de estado que se finalizo un proceso
    /// </summary>
    /// <history>[jorcanche] 05/04/2016 Created</history>
    private void StaEnd()
    {
      lblStatusBarMessage.Text = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      this.Cursor = null;
    }
    #endregion

    private void guCommentsColumnArrival_LostFocus(object sender, RoutedEventArgs e)
    {
      var txt = sender as TextBox;
      var row = dgGuestPremanifest.SelectedItem as GuestPremanifestOuthouse;
      Guest pre = BRGuests.GetGuest(row.guID);
      pre.guComments = txt.Text;
      BRGuests.SaveGuest(pre);
    }

    private void guCommentsColumnArrival_Loaded(object sender, RoutedEventArgs e)
    {
      var txt = sender as TextBox;
      txt.Focus();
    }

    private void Invit_Click(object sender, RoutedEventArgs e)
    {
      var row = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.CurrentPosition) as GuestPremanifestOuthouse;
      var chk = sender as CheckBox;
      if (!row.guCheckIn)
      {
        MessageBox.Show("Guest has not made Check In");
        chk.IsChecked = false;
        return;
      }

      var isChecked = chk.IsChecked.HasValue && chk.IsChecked.Value;
      chk.IsChecked = row.guInvit;
      //var userData = BRPersonnel.Login(EnumLoginType.Location, App.User.User.peID, App.User.Location.loID);
      var invit = new frmInvitationBase(EnumInvitationType.OutHouse, App.User, row.guID,
        !isChecked ? EnumInvitationMode.modOnlyRead : EnumInvitationMode.modAdd)
      {
        Owner = this,
        ShowInTaskbar = false
      };
      var res = invit.ShowDialog();
      row.guInvit = row.guInvit || (res.HasValue && res.Value);
      chk.IsChecked = row.guInvit;
    }

    //Valida los datos para desplegar el formulario de invitacion
    private bool ValidateRow(bool guCheckIn)
    {
      //Validamos que el huesped haya hecho Check In 
      if (!guCheckIn)
      {
        return false;
      }
      //Validamos que no sea un huesped adicional
      //if ()
      //{

      //}
      return true;
    }


    #region Window_KeyDown
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }
    #endregion

    #region dgGuestPremanifest_PreviewKeyDown
    private void dgGuestPremanifest_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (dgGuestPremanifest != null && e.Key == Key.Delete)
      {
        var row = dgGuestPremanifest.SelectedItem as GuestPremanifestOuthouse;
        if (!row.guShow)
        {
          DataGridRow dgr = (DataGridRow)dgGuestPremanifest.ItemContainerGenerator.ContainerFromIndex(dgGuestPremanifest.SelectedIndex);
          if (e.Key == Key.Delete && !dgr.IsEditing)
          {
            // User is attempting to delete the row
            var result = MessageBox.Show("Are you sure you want to delete this invitation?", "Delete",
              MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (!(e.Handled = result == MessageBoxResult.No))
            {
              BRGuests.DeleteGuest(row.guID);
            }
          }
        }
        e.Handled = true;
      }
    }
    #endregion

    #region btnLogin_Click
    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
      // frmLogin log = new frmLogin(null,false, EnumLoginType.Location, true);
      frmLogin log = new frmLogin(null, EnumLoginType.Location, program: EnumProgram.Outhouse, changePassword: false, autoSign: true, modeSwitchLoginUser: true);
      if (App.User.AutoSign)
      {
        log.UserData = App.User;
      }
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
        App.User = log.UserData;
        LoadOuthouse();
      }
    }
    #endregion

    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      if (dgGuestPremanifest.Items.Count > 0)
      {
        var remanifestOutside = BRGeneralReports.GetRptPremanifestOutSide(dtpDate.Value.Value, App.User.LeadSource.lsID);
        ReportsToExcel.PremanifestToExcel(remanifestOutside);//, dtpDate.Value.Value);
      }
      else
      {
        MessageBox.Show("There is no data", "IM Outhouse");
      }
    }

    private void btnRptDsr_Click(object sender, RoutedEventArgs e)
    {

    }

    #region btnNewInv_Click
    private void btnNewInv_Click(object sender, RoutedEventArgs e)
    {
      var invit = new frmInvitationBase(EnumInvitationType.OutHouse, App.User, 0, EnumInvitationMode.modAdd)
      {
        Owner = this,
        ShowInTaskbar = false
      };
      invit.ShowDialog();
    }
    #endregion

    #region btnAbout_Click
    private void btnAbout_Click(object sender, RoutedEventArgs e)
    {
      frmAbout formAbout = new frmAbout();
      formAbout.ShowInTaskbar = false;
      formAbout.ShowDialog();
    }
    #endregion

    private void btnTransfer_Click(object sender, RoutedEventArgs e)
    {

    }

    #region btnAssistance_Click
    private void btnAssistance_Click(object sender, RoutedEventArgs e)
    {
      frmAssistance frmAssistance = new frmAssistance(EnumPlaceType.LeadSource, App.User);
      frmAssistance.ShowDialog();
    }
    #endregion

    #region btnDaysOff_Click
    private void btnDaysOff_Click(object sender, RoutedEventArgs e)
    {
      frmDaysOff frmDaysOff = new frmDaysOff(EnumTeamType.TeamPRs, App.User);
      frmDaysOff.ShowDialog();
    }
    #endregion

    private void btnReports_Click(object sender, RoutedEventArgs e)
    {

    }

  }
}



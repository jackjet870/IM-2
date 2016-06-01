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
using System.Threading.Tasks;

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

    #region Contructores Y destructores
    /// <summary>
    /// Contructor de Outhouse
    /// </summary>
    /// <history>
    /// [jorcanche] 05/05/2016 created
    /// </history>
    public frmOuthouse()
    {
      InitializeComponent();
    }
    #endregion

    #region Metodos

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

    #region ValidateInvitation
    /// <summary>
    /// Valida los datos para desplegar el formulario de invitaciones
    /// </summary>
    /// <param name="gucheckIn">Indica si ya hizo CheckIn el huesped</param>
    /// <returns>bool</returns>
    /// <history>
    /// [jorcanche] 06/05/2016 created
    /// </history>
    private bool ValidateInvitation(bool gucheckIn)
    {
      //Validamos que el huesped haya hecho Chek In
      if (!gucheckIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-In", title: "Outhouse");
        return false;
      }
      //Validamos que tenga minimo permisos de lectura de invitaciones 
      if (App.User.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.ReadOnly))
      {
        UIHelper.ShowMessage("You has not Permission of 'PR Invitation'");
        return false;
      }
      return true;
    }
    #endregion

    #region LoadGrid
    /// <summary>
    /// Carga el DataGrid que contiene los Guest Premanifest de Outhouse
    /// </summary>
    /// <history>
    /// [jorcanche] 05/05/2016 created
    /// </history>
    private void LoadGrid()
    {
      if (_outPremanifestViewSource != null)
        //_outPremanifestViewSource.Source = BRGuests.GetGuestPremanifestOuthouse(_bookInvit, _serverDate, App.User.Location.loID);
        Task.Factory.StartNew(() => BRGuests.GetGuestPremanifestOuthouse(_bookInvit, _serverDate, App.User.Location.loID))
         .ContinueWith(
         (LoadGuestsOutSide) =>
         {
           if (LoadGuestsOutSide.IsFaulted)
           {
             UIHelper.ShowMessage(LoadGuestsOutSide.Exception.InnerException.Message, MessageBoxImage.Error);
             StaEnd();
             return false;
           }
           else
           {
             if (LoadGuestsOutSide.IsCompleted)
             {
               LoadGuestsOutSide.Wait(1000);
               _outPremanifestViewSource.Source = LoadGuestsOutSide.Result;
             }
             StaEnd();
             return false;
           }
         },
         TaskScheduler.FromCurrentSynchronizationContext()
          );
    }
    #endregion
 
    #region LoadOuthouse
    /// <summary>
    /// Carga e incializa la variables cuando es invocado el metodo
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
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
    #endregion

    #region StaStart
    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <param name="message">mensaje a mostrar</param>
    /// <history>
    /// [jorcanche] 05/04/2016 Created 
    /// </history>
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

    #endregion

    #region Eventos

    #region dtpDate_ValueChanged
    /// <summary>
    /// Valida que se ingrese un fecha con formato correcto
    /// </summary>
    /// <history>
    /// [jorcanche] 05/05/2016 created
    /// </history>
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
    #endregion

    #region rbt_Checked
    /// <summary>
    /// Manipula los Guest premanifest de Outhouse segun el check que seleccione
    /// ya sea Book D o Invit D
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void rbt_Checked(object sender, RoutedEventArgs e)
    {
      var rb = sender as RadioButton;
      if (rb != null) _bookInvit = Convert.ToBoolean(rb.TabIndex);
      LoadGrid();
    }
    #endregion

    #region btnRefresh_Click
    /// <summary>
    /// Funciona para refrescar el Grid por si hay una modificación en la base
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
      LoadGrid();
    }

    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga todos los paramatros e inicializa las variables
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadOuthouse();
    }
    #endregion

    #region Info_Click
    /// <summary>
    /// Valida la el check y lo que necesite antes de abrir el formulario de contactación
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
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
    #endregion

    #region guCommentsColumnArrival_LostFocus
    /// <summary>
    /// Cuando se pierde el Focus de los comentarios lo guarda en la base lo que se halla escrito
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void guCommentsColumnArrival_LostFocus(object sender, RoutedEventArgs e)
    {
      var txt = sender as TextBox;    
      var row = dgGuestPremanifest.SelectedItem as GuestPremanifestOuthouse;
      Guest pre = BRGuests.GetGuest(row.guID);
      pre.guComments = txt.Text;
      BRGuests.SaveGuest(pre);
    }
    #endregion

    #region guCommentsColumnArrival_Loaded
    /// <summary>
    /// Cuando se incializa le asigna el focus a la celda de comentarios 
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void guCommentsColumnArrival_Loaded(object sender, RoutedEventArgs e)
    {
      var txt = sender as TextBox;
      txt.Focus();
    }
    #endregion

    #region Invit_Click
    /// <summary>
    /// Valida e Inicializa el formulario de invitación
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void Invit_Click(object sender, RoutedEventArgs e)
    {
      var row = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.CurrentPosition) as GuestPremanifestOuthouse;
      var chkInv = sender as CheckBox;
      chkInv.IsChecked = !chkInv.IsChecked.Value;
      //Si los datos son validos
      if (ValidateInvitation(row.guCheckIn))
      {
        var inv = new  frmInvitationBase(EnumInvitationType.OutHouse,App.User,row.guID,EnumInvitationMode.modEdit);
        inv.ShowDialog();       
      }
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Se aplica cada vez que se presiona las teclas de Numlock, Insert y Capital
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
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
    /// <summary>
    /// Se aplica cada vez que se quiere eliminar un guest en el grid presionando la tecla Delete
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
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
    /// <summary>
    /// Inicializa de nuevo el login para cambioar de LeadSource o de Usuario
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// [erosado] 01/06/2016  Modified. se agrego async
    /// </history>
    private async  void btnLogin_Click(object sender, RoutedEventArgs e)
    {
      // frmLogin log = new frmLogin(null,false, EnumLoginType.Location, true);
      frmLogin log = new frmLogin(null, EnumLoginType.Location, program: EnumProgram.Outhouse, changePassword: false, autoSign: true, modeSwitchLoginUser: true);
      await log.getAllPlaces();
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

    #region btnPrint_Click
    /// <summary>
    /// Imprime el reporte de Guests Premanifest Outhouse según lo que esta visualizando el DataGrid
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
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
    #endregion

    #region btnNewInv_Click
    /// <summary>
    /// Abre el formulario de invitación para agrear una nueva
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
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
    /// <summary>
    /// Abre la ventana de About en donde informa datos generales del IM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAbout_Click(object sender, RoutedEventArgs e)
    {
      frmAbout formAbout = new frmAbout();
      formAbout.ShowInTaskbar = false;
      formAbout.ShowDialog();
    }
    #endregion

    #region btnTransfer_Click
    /// <summary>
    /// Abre el formulario que funciona para buscar un Guest y lo transfiere al OutHouse que esta actual
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void btnTransfer_Click(object sender, RoutedEventArgs e)
    {
      frmSearchGuest frmsearchGuest = new frmSearchGuest(App.User, EnumProgram.Outhouse);
      frmsearchGuest.Owner = this;
      frmsearchGuest.ShowInTaskbar = false;
      //Si ya se cerro la ventana
      //Valida que haya sido un OK y no un Cancel
      frmsearchGuest.ShowDialog();
      if (!frmsearchGuest.cancel)
      {
        var guest = frmsearchGuest.lstGuestAdd[0];
        guest.guls = App.User.LeadSource.lsID;
        guest.guBookCanc = false;
        guest.guloInvit = App.User.LeadSource.lsID;
        guest.gulsOriginal = App.User.LeadSource.lsID;

        //Enviamos los parametros para que guarde los cambios del guest y el log del Guest.
        //Si hubo un error al ejecutar el metodo SaveChangedOfGuest nos devolvera 0, indicando que ningun paso 
        //se realizo, es decir ni se guardo el Guest ni el Log
        if (BRGuests.SaveChangedOfGuest(guest, App.User.LeadSource.lsHoursDif, App.User.User.peID) == 0)
        {
          //De no ser así informamos que no se guardo la información por algun motivo
          UIHelper.ShowMessage("There was an error saving the information, consult your system administrator",
            MessageBoxImage.Error, "Information can not keep");
        }
      }
    }
    #endregion

    #region btnAssistance_Click
    /// <summary>
    /// Abre el formulario de asistencia 
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void btnAssistance_Click(object sender, RoutedEventArgs e)
    {
      frmAssistance frmAssistance = new frmAssistance(EnumPlaceType.LeadSource, App.User);
      frmAssistance.ShowDialog();
    }
    #endregion

    #region btnDaysOff_Click
    /// <summary>
    /// Abre el formulario de DaysOff
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void btnDaysOff_Click(object sender, RoutedEventArgs e)
    {
      frmDaysOff frmDaysOff = new frmDaysOff(EnumTeamType.TeamPRs, App.User);
      frmDaysOff.ShowDialog();
    }
    #endregion

    #endregion

  }
}



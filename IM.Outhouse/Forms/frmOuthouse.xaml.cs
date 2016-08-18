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
      if (!contact && checkOutD < BRHelpers.GetServerDate())
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
    /// Valida los datos para desplegar el formulario de invitacion
    /// </summary>
    /// <param name="guCheckIn">Si ya hizo Check In el Huesped </param>    
    /// <history>
    /// [jorcanche] 16/ago/2016 Created
    /// </history>
    private bool ValidateInvitation(bool guCheckIn)
    {
      //Validamos que el huesped haya hecho Check In
      if (!guCheckIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-In.");
        return false;
      }
      //Validamos que no sea un huesped adicional 
     
      //Validamos que tenga permiso de lectura de invitaciones
      if (App.User.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.ReadOnly)) return true;
      UIHelper.ShowMessage("Access denied.");
      return false;
    }

    #endregion

    #region LoadGrid

    /// <summary>
    /// Carga el DataGrid que contiene los Guest Premanifest de Outhouse
    /// </summary>
    /// <history>
    /// [jorcanche] 05/05/2016 created
    /// </history>
    private async void LoadGrid()
    {
      if (_outPremanifestViewSource == null) return;
      //BusyIndicator.IsBusy = true;
      _outPremanifestViewSource.Source =
        await BRGuests.GetGuestPremanifestOuthouse(_bookInvit, _serverDate, App.User.Location.loID);
      StaEnd();
     // BusyIndicator.IsBusy = false;
    }

    #endregion

    #region LoadOuthouse

    /// <summary>
    /// Carga e incializa la variables cuando es invocado el metodo
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void LoadOuthouse()
    {
      //Cargamos las variables del usuario
      txtUser.Text = App.User.User.peN;
      txtLocation.Text = App.User.Location.loN;

      //Cargamos la fecha actual del servidor
      dtpDate.Value = BRHelpers.GetServerDate();
      dtpDate_ValueChanged(null, null);

      //Inicializamos la variable del datagrid
      _outPremanifestViewSource = (CollectionViewSource) FindResource("outPremanifestViewSource");

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
      Cursor = null;
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
      if (!IsInitialized) return;
      if (dtpDate.Text == string.Empty) dtpDate.Value = _serverDate;
      if (dtpDate.Value == null || _serverDate == dtpDate.Value.Value) return;
      StaStart("Loading OutHouse...");
      _serverDate = dtpDate.Value.Value;
      LoadGrid();    
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
            {
              item.guPRInfo = frmCont.PRInfo;
              item.guInfoD = frmCont.InfoD;
              item.guCheckIn = true;
              item.guInfo = true;
            });
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
    private async void guCommentsColumnArrival_LostFocus(object sender, RoutedEventArgs e)
    {
      var txt = sender as TextBox;
      var row = dgGuestPremanifest.SelectedItem as GuestPremanifestOuthouse;
      Guest pre = await BRGuests.GetGuest(row.guID);
      pre.guComments = txt.Text;
      await BRGuests.SaveGuest(pre);
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
      var guest = dgGuestPremanifest.Items.GetItemAt(dgGuestPremanifest.Items.CurrentPosition) as GuestPremanifestOuthouse;
      var chk = sender as CheckBox;
      //Validamos Valores nulos 
      if (chk?.IsChecked == null || guest == null) return;

      //Invertimos el valor del Check para que no se modifique. El formulario Invitación definira si hubo invitación o no
      chk.IsChecked = !chk.IsChecked.Value;

      //Despliega el formulario de Invitación
      ShowInvitation(guest.guCheckIn, guest.guID, chk.IsChecked.Value);
    }

    #endregion

    #region ShowInvitation
    /// <summary>
    /// Despliega el formulario de invitacion
    /// </summary>
    /// <param name="guCheckIn">Check In del Huesped</param>    
    /// <param name="guId">Id del Guest</param>
    /// <param name="isInvit">Si ya se invito el guest</param>
    /// <history>
    /// [jorcanche] 16/ago/2016 Created
    /// </history>
    private async void ShowInvitation(bool guCheckIn, int guId, bool isInvit)
    {
      if (ValidateInvitation(guCheckIn))
      {
        frmLogin login = null;
        if (!isInvit)
        {
          login = new frmLogin(loginType: EnumLoginType.Location, program: EnumProgram.Inhouse, validatePermission: true, permission: EnumPermission.PRInvitations, permissionLevel: EnumPermisionLevel.Standard, switchLoginUserMode: true, invitationMode: true, invitationPlaceId: App.User.Location.loID);

          if (App.User.AutoSign)
          {
            login.UserData = App.User;
          }
          await login.getAllPlaces();
          login.ShowDialog();
        }
        if (isInvit || login.IsAuthenticated)
        {
          var invitacion = new frmInvitation(EnumModule.OutHouse, EnumInvitationType.existing, login != null ? login.UserData : App.User, guId) { Owner = this };
          invitacion.ShowDialog();
        }
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
      switch (e.Key)
      {
        case Key.Capital:
          KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
          break;
        case Key.Insert:
          KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
          break;
        case Key.NumLock:
          KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
          break;
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
    private async void dgGuestPremanifest_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      var row = dgGuestPremanifest.SelectedItem as GuestPremanifestOuthouse;

      //Si no es una tecla Delete ó si row es igual a null Retornamos
      if(e.Key != Key.Delete || row  == null) return;

      // Si se esta editando alguna celda del row no dejamos que elimine el registro
      if (((DataGridRow)dgGuestPremanifest.ItemContainerGenerator.ContainerFromItem(row)).IsEditing) return;

      //Si se presiono la tecla Delte y si  diferente de Null el row y si tiene Show el guest 
      //No lo permitimos eliminar y retornamos 
      if (row.guShow)
      {
        e.Handled = true;
        UIHelper.ShowMessage("You can not delete the Guest because has 'Show Up'");
        return;
      }
    
      var result = MessageBox.Show("Are you sure you want to delete this invitation?", "Delete", MessageBoxButton.YesNo,
        MessageBoxImage.Question, MessageBoxResult.No);           

      if (result == MessageBoxResult.Yes )
      {
        await BRGuests.DeleteGuest(row.guID);
      }
      else
      {
        e.Handled = true;
      }
    }

    #endregion

    #region OnIsKeyboardFocusWithinChanged

    /// <summary>
    /// comprobará si existen cambios en el teclado (Si fueron oprimidas las teclas MAYUS,INSERT y BLOQ NUM) y refresca nuestro StatusBar
    /// </summary>
    /// <history>
    /// [jorcanche]  created
    /// </history>
    private void OnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
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
    private async void btnLogin_Click(object sender, RoutedEventArgs e)
    {
      // frmLogin log = new frmLogin(null,false, EnumLoginType.Location, true);
      frmLogin log = new frmLogin(null, EnumLoginType.Location, program: EnumProgram.Outhouse, changePassword: false,
        autoSign: true, switchLoginUserMode: true);
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
        ReportsToExcel.PremanifestToExcel(remanifestOutside); //, dtpDate.Value.Value);
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
    private async void btnNewInv_Click(object sender, RoutedEventArgs e)
    {
      //TODO: Jorge revisar la instancia al nuevo formulario, 
      //var invit = new frmInvitationBase(EnumModule.OutHouse, App.User, 0, EnumInvitationMode.modAdd)
      //{
      //  Owner = this,
      //  ShowInTaskbar = false
      //};
      //invit.ShowDialog();

      //External Invitation
      var login = new frmLogin(loginType: EnumLoginType.Location, program: EnumProgram.Outhouse,
        validatePermission: true, permission: EnumPermission.PRInvitations, permissionLevel: EnumPermisionLevel.Standard,
        switchLoginUserMode: true, invitationMode: true, invitationPlaceId: App.User.Location.loID);

      if (App.User.AutoSign)
      {
        login.UserData = App.User;
      }
      await login.getAllPlaces();
      login.ShowDialog();

      if (login.IsAuthenticated)
      {
        var invitacion = new frmInvitation(EnumModule.OutHouse, EnumInvitationType.newOutHouse, login.UserData)
        {
          Owner = this
        };
        invitacion.ShowDialog();
      }

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
      frmAbout formAbout = new frmAbout {Owner = this};
      formAbout.ShowDialog();
    }

    #endregion

    #region btnTransfer_Click

    /// <summary>
    /// Abre el formulario que funciona para buscar un Guest y lo transfiere al OutHouse que esta actual
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// [vipacheco] 04/Agosto/2016 Modified -> Se agrego el parametro EnumProgram al frmSearchGuest
    /// </history>
    private async void btnTransfer_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        var frmsearchGuest = new frmSearchGuest(App.User, EnumProgram.Outhouse)
        {
          Owner = this,
          ShowInTaskbar = false
        };
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
          if (await BRGuests.SaveChangedOfGuest(guest, App.User.LeadSource.lsHoursDif, App.User.User.peID) == 0)
          {
            //De no ser así informamos que no se guardo la información por algun motivo
            UIHelper.ShowMessage("There was an error saving the information, consult your system administrator",
              MessageBoxImage.Error, "Information can not keep");
          }
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
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
      var frmAssistance = new frmAssistance(EnumPlaceType.LeadSource, App.User) {Owner = this};
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
      var frmDaysOff = new frmDaysOff(EnumTeamType.TeamPRs, App.User) { Owner = this }; ;
      frmDaysOff.ShowDialog();
    }

    #endregion

    #region dgGuestPremanifest_SelectionChanged

    /// <summary>
    /// Actualiza el conteo de los registros del Data Grid
    /// </summary>
    /// <history>
    /// [jorcanche] created 14/07/2016
    /// </history>
    private void dgGuestPremanifest_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var dg = sender as DataGrid;
      if (dg != null) StatusBarReg.Content = $"{dg.Items.CurrentPosition + 1}/{dg.Items.Count}";
    }

    #endregion

    #endregion
  }
}



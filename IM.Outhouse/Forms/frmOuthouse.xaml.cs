﻿using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Outhouse.Classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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

    #endregion Atributos

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

    #endregion Contructores Y destructores

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
      if (!Context.User.HasPermission(EnumPermission.Register, EnumPermisionLevel.ReadOnly))
      {
        UIHelper.ShowMessage("Access denied.", MessageBoxImage.Asterisk);
        return false;
      }
      return true;
    }

    #endregion ValidateContact

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
      if (Context.User.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.ReadOnly)) return true;
      UIHelper.ShowMessage("Access denied.");
      return false;
    }

    #endregion ValidateInvitation

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
      try
      {        
        _outPremanifestViewSource.Source =
        await BRGuests.GetGuestPremanifestOuthouse(_bookInvit, _serverDate, Context.User.Location.loID);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
      finally
      {
        StaEnd();
      }
    }

    #endregion LoadGrid

    #region LoadOuthouse

    /// <summary>
    /// Carga e incializa la variables cuando es invocado el metodo
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void LoadOuthouse()
    {
      StaStart("Loading OutHouse...");

      //Cargamos las variables del usuario
      txtUser.Text = Context.User.User.peN;
      txtLocation.Text = Context.User.Location.loN;

      //Cargamos la fecha actual del servidor
      dtpDate.Value = BRHelpers.GetServerDate();
      dtpDate_ValueChanged(null, null);

      //Inicializamos la variable del datagrid
      _outPremanifestViewSource = (CollectionViewSource)FindResource("outPremanifestViewSource");

      //Indicamos al statusbar que me muestre cierta informacion cuando oprimimos cierto teclado
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);

      //Cargamos el DataGrid
      LoadGrid();
    }

    #endregion LoadOuthouse

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
      if (lblStatusBarMessage == null) return;
      lblStatusBarMessage.Text = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      this.Cursor = Cursors.Wait;
    }

    #endregion StaStart

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

    #endregion StaEnd

    #endregion Metodos

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

    #endregion dtpDate_ValueChanged

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
      StaStart("Loading OutHouse...");
      LoadGrid();
    }

    #endregion rbt_Checked

    #region btnRefresh_Click

    /// <summary>
    /// Funciona para refrescar el Grid por si hay una modificación en la base
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
      StaStart("Loading OutHouse...");
      LoadGrid();
    }

    #endregion btnRefresh_Click

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

    #endregion Window_Loaded

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
        frmContact frmCont = new frmContact(outPre.guID, Context.User);
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

    #endregion Info_Click

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
      await BREntities.OperationEntity(pre, EnumMode.Edit);
    }

    #endregion guCommentsColumnArrival_LostFocus

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

    #endregion guCommentsColumnArrival_Loaded

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

    #endregion Invit_Click

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
        frmLogin login = new frmLogin(loginType: EnumLoginType.Location, program: EnumProgram.Outhouse, validatePermission: true,
            permission: EnumPermission.PRInvitations, permissionLevel: EnumPermisionLevel.Standard, switchLoginUserMode: true,
            invitationMode: true, invitationPlaceId: Context.User.Location.loID);
        if (!isInvit)
        {
          if (Context.User.AutoSign) login.UserData = Context.User;
          await login.getAllPlaces();
          login.ShowDialog();

          if (!login.IsAuthenticated) return;
        }
        else if (Context.User.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.Standard) && !Context.User.AutoSign)
        {
          login = new frmLogin(loginType: EnumLoginType.Location, program: EnumProgram.Outhouse, validatePermission: true,
            permission: EnumPermission.PRInvitations, permissionLevel: EnumPermisionLevel.ReadOnly, switchLoginUserMode: true,
            invitationMode: true, invitationPlaceId: Context.User.Location.loID);
          await login.getAllPlaces();
          login.ShowDialog();

          if (!login.IsAuthenticated) return;
        }
        else
        {
          login.UserData = Context.User;
        }              

        if (isInvit || login.IsAuthenticated)
        {
          var invitacion = new frmInvitation
            (EnumModule.OutHouse, EnumInvitationType.existing, login != null ? login.UserData : Context.User, guId) { Owner = this };
          invitacion.ShowDialog();
          if (invitacion.SaveGuestInvitation)
          {
            //actualizamos los datos del grid            
            UpdateGridInvitation(invitacion.dbContext.Guest, invitacion._module, dgGuestPremanifest);
          }
          
        }
      }
    }

    #endregion ShowInvitation

    #region UpdateGridInvitation

    /// <summary>
    /// Actualiza el datagrid de inhouse
    /// </summary>
    /// <param name="invitacion">Formulario de Incitación</param>
    /// <param name="module">En que modulo se esta invocando</param>
    /// <param name="dg">DataGrid Current</param>
    private void UpdateGridInvitation(Guest invitacion, EnumModule module, DataGrid dg)
    {
      var item = dg.SelectedItem;
      var t = item.GetType();
      var lstProperties = t.GetProperties().ToList();

      //***********************************************Disponible ***********************************************
      if (lstProperties.Any(c => c.Name == "guInvit"))
      {
        t.GetProperty("guInvit").SetValue(item, true);
      }

      //***********************************************Seguimiento*********************************************** 
      if (lstProperties.Any(c => c.Name == "guFollow"))
      {
        //Si estaba contactado y no como invitado 
        if ((bool)t.GetProperty("guInfo").GetValue(item) &&
            (bool)t.GetProperty("guInvit").GetValue(item) == false)
        {
          //Con seguimiento 
          t.GetProperty("guFollow").SetValue(item, true);

          //PR y Fecha de seguimiento
          if (string.IsNullOrEmpty(t.GetProperty("guPRFollow").GetValue(item)?.ToString()))
          {
            t.GetProperty("guPRFollow").SetValue(item, invitacion.guPRInvit1);
            t.GetProperty("guFollowD").SetValue(item, invitacion.guInvitD);
          }
        }
      }
      //***********************************************Contactacion ***********************************************
      if (lstProperties.Any(c => c.Name == "guInfo"))
      {
        //Contactado
        t.GetProperty("guInfo").SetValue(item, true);

        //PR y fecha de contactacion
        if (string.IsNullOrEmpty(t.GetProperty("guPRInfo").GetValue(item)?.ToString()))
        {
          t.GetProperty("guPRInfo").SetValue(item, invitacion.guPRInvit1);
          t.GetProperty("guInfoD").SetValue(item, invitacion.guInvitD);
        }
      }

      //***********************************************Invitacion ***********************************************
      //Invitado
      t.GetProperty("guInvit").SetValue(item, true);

      //PR de Invitación
      if (lstProperties.Any(c => c.Name == "guPRInvit1"))
      {
        t.GetProperty("guPRInvit1").SetValue(item, invitacion.guPRInvit1);
      }

      //Invitacion no cancelada
      if (lstProperties.Any(c => c.Name == "guBookCanc"))
      {
        t.GetProperty("guBookCanc").SetValue(item, false);
      }

      //Fecha de no booking
      if (lstProperties.Any(c => c.Name == "guBookD"))
      {
        t.GetProperty("guBookD").SetValue(item, invitacion.guBookD);
      }

      //Hora de booking
      if (lstProperties.Any(c => c.Name == "guBookT"))
      {
        if (module != EnumModule.OutHouse)
        {
          if (invitacion.guResch)
          {
            if (invitacion.guReschD == invitacion.guReschT)
            {
              t.GetProperty("guBookT").SetValue(item, invitacion.guReschT);
            }
          }
          else
          {
            t.GetProperty("guBookT").SetValue(item, invitacion.guBookT);
          }
        }
        else
        {
          t.GetProperty("guBookT").SetValue(item, invitacion.guBookT);
        }
      }
      //Quiniela
      if (lstProperties.Any(c => c.Name == "guQuinella"))
      {
        t.GetProperty("guQuinella").SetValue(item, invitacion.guQuinella);
      }

      dg.Items.Refresh();
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

    #endregion Window_KeyDown

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
      if (e.Key != Key.Delete || row == null) return;

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

      var result = UIHelper.ShowMessage("Are you sure you want to delete this invitation?",
        MessageBoxImage.Question);

      if (result == MessageBoxResult.Yes)
      {
        await BRGuests.DeleteGuest(row.guID);
      }
      else
      {
        e.Handled = true;
      }
    }

    #endregion dgGuestPremanifest_PreviewKeyDown

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

    #endregion OnIsKeyboardFocusWithinChanged

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
      if (Context.User.AutoSign)
      {
        log.UserData = Context.User;
      }
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
        Context.User = log.UserData;
        LoadOuthouse();
      }
    }

    #endregion btnLogin_Click

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
        var remanifestOutside = BRGeneralReports.GetRptPremanifestOutSide(dtpDate.Value.Value, Context.User.LeadSource.lsID);
        ReportsToExcel.PremanifestToExcel(remanifestOutside,this);
      }
      else
      {
        UIHelper.ShowMessage("There is no data");
      }
    }

    #endregion btnPrint_Click

    #region btnNewInv_Click

    /// <summary>
    /// Abre el formulario de invitación para agrear una nueva
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private async void btnNewInv_Click(object sender, RoutedEventArgs e)
    {
      var login = new frmLogin(loginType: EnumLoginType.Location, program: EnumProgram.Outhouse,
        validatePermission: true, permission: EnumPermission.PRInvitations, permissionLevel: EnumPermisionLevel.Standard,
        switchLoginUserMode: true, invitationMode: true, invitationPlaceId: Context.User.Location.loID);

      if (Context.User.AutoSign)
      {
        login.UserData = Context.User;
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

    #endregion btnNewInv_Click

    #region btnAbout_Click

    /// <summary>
    /// Abre la ventana de About en donde informa datos generales del IM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAbout_Click(object sender, RoutedEventArgs e)
    {
      frmAbout formAbout = new frmAbout { Owner = this };
      formAbout.ShowDialog();
    }

    #endregion btnAbout_Click

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
        var frmsearchGuest = new frmSearchGuest(Context.User, EnumProgram.Outhouse)
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
          guest.guls = Context.User.LeadSource.lsID;
          guest.guBookCanc = false;
          guest.guloInvit = Context.User.LeadSource.lsID;
          guest.gulsOriginal = Context.User.LeadSource.lsID;

          //Enviamos los parametros para que guarde los cambios del guest y el log del Guest.
          //Si hubo un error al ejecutar el metodo SaveChangedOfGuest nos devolvera 0, indicando que ningun paso
          //se realizo, es decir ni se guardo el Guest ni el Log
          if (await BRGuests.SaveChangedOfGuest(guest, Context.User.LeadSource.lsHoursDif, Context.User.User.peID) == 0)
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

    #endregion btnTransfer_Click

    #region btnAssistance_Click

    /// <summary>
    /// Abre el formulario de asistencia
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void btnAssistance_Click(object sender, RoutedEventArgs e)
    {
      var frmAssistance = new frmAssistance(EnumPlaceType.LeadSource, Context.User) { Owner = this };
      frmAssistance.ShowDialog();
    }

    #endregion btnAssistance_Click

    #region btnDaysOff_Click

    /// <summary>
    /// Abre el formulario de DaysOff
    /// </summary>
    /// <history>
    /// [jorcanche] created 05/05/2016
    /// </history>
    private void btnDaysOff_Click(object sender, RoutedEventArgs e)
    {
      var frmDaysOff = new frmDaysOff(EnumTeamType.TeamPRs, Context.User) { Owner = this }; ;
      frmDaysOff.ShowDialog();
    }

    #endregion btnDaysOff_Click

    #region btnReport_Click
    /// <summary>
    /// Muestra el diseño del reporte
    /// </summary>
    /// <history>
    /// [jorcanche]  created 31/ago/2016
    /// </history>
    private void btnReport_Click(object sender, RoutedEventArgs e)
    {
      new frmReportsOutside(dtpDate.Value.Value) {  Owner = this  }.ShowDialog();
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

    #endregion dgGuestPremanifest_SelectionChanged

    #endregion Eventos
  
  }
}
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using IM.Base.Interfaces;
using IM.Base.Forms;
using IM.Model;
using IM.Host.Forms;
using IM.Model.Classes;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.Host.Enums;

namespace IM.Host
{
  /// <summary>
  /// Interaction logic for frmHost.xaml
  /// </summary>
  /// 
  public partial class frmHost : Window, IMetodosPublicos
  {
    private DateTime? _dtpCurrent = null;
    public static DateTime _dtpServerDate = new DateTime();
    //private SalesRoomLogin _salesRoomLogin = new SalesRoomLogin();
    private UserLogin _userLogin = new UserLogin();
    CollectionViewSource _guestHost;

    public frmHost()
    {
      //_salesRoomLogin = App.User.SalesRoom;
      _userLogin = App.User.User;

      InitializeComponent();

      txtUser.Text = App.User.User.peN.ToString();
      txtSalesRoom.Text = App.User.SalesRoom.srN.ToString();
    }

    #region Métodos de controles en el formulario

    /// <summary>
    /// Realiza las configuraciones de los controles de la forma
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void frmHost_Loaded(object sender, RoutedEventArgs e)
    {
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);

      _dtpServerDate = BRHelpers.GetServerDate();

      // Se verifica que el tipo de permiso del usuario para habilitar y/o deshabilitar opciones necesarias.
      if (App.User.HasPermission(EnumPermission.Host, EnumPermisionLevel.ReadOnly))//  .Permissions.Exists(c => c.pppm == "HOST" && c.pppl == 1))
      {
        guShowSeqColumn.IsReadOnly = true;
        guQuinellaColumn.IsReadOnly = true;
        guWCommentsColumn.IsReadOnly = true;
      }

      // Actualizamos los tipos de cambio de monedas hasta el dia de hoy
      BRExchangeRate.InsertExchangeRate(_dtpServerDate);

      // Actualizamos las fechas de temporada hasta el año actual
      BRSeasons.UpdateSeasonDates(_dtpServerDate.Year);

    }

    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [lchairezReload] 09/Feb/2016 Created
    /// </history>
    private void frmHost_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }

    /// <summary>
    /// Llama la ventana de Login
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
      var login = new frmLogin();
      login.Owner = this;
      login.ShowDialog();
    }

    /// <summary>
    /// Llama la ventana de About
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void btnAbout_Click(object sender, RoutedEventArgs e)
    {
      var about = new frmAbout();
      about.Owner = this;
      about.ShowDialog();
    }

    /// <summary>
    /// Imprime el reporte
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      PrintReport();
    }

    /// <summary>
    /// Muestra una vista previa del reporte
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void btnPreview_Click(object sender, RoutedEventArgs e)
    {
      ShowReport();
    }

    /// <summary>
    /// Muestra el diseño del reporte.
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void btnReport_Click(object sender, RoutedEventArgs e)
    {
      ShowReportDesigner();
    }

    /// <summary>
    /// Funcion del evento Changed del DatePicker
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 26/02/2016 Created
    /// </history>
    private void dtpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      if (_dtpCurrent != dtpDate.SelectedDate)
      {
        // Asignamos la fecha seleccionada.
        _dtpCurrent = dtpDate.SelectedDate.Value.Date;

        _guestHost = ((CollectionViewSource)(this.FindResource("dsPremanifestHost")));
        _guestHost.Source = BRGuests.GetPremanifestHost(_dtpCurrent, App.User.SalesRoom.srID);
      }
    }

    /// <summary>
    /// Función para mostrar el formulario Close sales Room
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 26-02-2016 Created
    /// </history>
    private void btnCloseSalesRoom_Click(object sender, RoutedEventArgs e)
    {
      // Validamos que tenga permiso de lectura de cierre de sala de ventas
      if (!App.User.HasPermission(EnumPermission.CloseSalesRoom, EnumPermisionLevel.ReadOnly)) //  _userData.Permissions.Exists(c => c.pppm == "CLOSESR" && c.pppl >= 1))
      {
        MessageBox.Show("Access denied.", "Close Sales Room");
        return;
      }

      // Mostramos el formulario Close Sales Room
      frmCloseSalesRoom mfrCloseSalesRoom = new frmCloseSalesRoom(this, _dtpServerDate);
      mfrCloseSalesRoom.ShowInTaskbar = false;
      mfrCloseSalesRoom.Owner = this;
      AplicarEfecto(this);
      mfrCloseSalesRoom.ShowDialog();
    }

    /// <summary>
    /// Funcion que se encarga de validar el total de datos obtenidos en el datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/03/2016 Created
    /// </history>
    private void dtgPremanifestHost_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (dtgPremanifestHost.Items.Count == 0)
      {
        StatusBarReg.Content = "No Records";
        return;
      }
      StatusBarReg.Content = string.Format("{0}/{1}", dtgPremanifestHost.Items.IndexOf(dtgPremanifestHost.CurrentItem) + 1, dtgPremanifestHost.Items.Count);
    }

    /// <summary>
    /// Función encargada de aplicar efecto a la ventana
    /// </summary>
    /// <param name="win"></param>
    /// <history>
    /// [vipacheco] 26-02-2016 Created
    /// </history>
    private void AplicarEfecto(Window win)
    {
      var objBlur = new System.Windows.Media.Effects.BlurEffect();
      objBlur.Radius = 5;
      win.Effect = objBlur;
    }

    /// <summary>
    /// Función para aumentar o decremetar la fecha con la teclas de navegacion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/003/2016 Created
    /// </history>
    private void dtpDate_PreviewKeyUp(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Down:
          dtpDate.SelectedDate = dtpDate.SelectedDate.Value.AddDays(-1);
          break;

        case Key.Up:
          dtpDate.SelectedDate = dtpDate.SelectedDate.Value.AddDays(1);
          break;
      }
    }

    /// <summary>
    /// Función para actualizar el datapicker con el sroll
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/03/2016 Created
    /// </history>
    private void upd_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
    {
      switch (e.ScrollEventType)
      {
        case System.Windows.Controls.Primitives.ScrollEventType.SmallIncrement:
          dtpDate.SelectedDate = dtpDate.SelectedDate.Value.AddDays(-1);
          break;
        case System.Windows.Controls.Primitives.ScrollEventType.SmallDecrement:
          dtpDate.SelectedDate = dtpDate.SelectedDate.Value.AddDays(1);
          break;
      }
    }

    #endregion

    #region Métodos privados

    /// <summary>
    /// Manda llamar a todos los métodos de configuración de los controles al ser cargada la forma
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void ConfigControls()
    {
      ConfigDataGrid();
      KeyDefault(StatusBarCap);
      KeyDefault(StatusBarIns);
      KeyDefault(StatusBarNum);
    }

    /// <summary>
    /// Configura el Datagrid al cargar la forma
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void ConfigDataGrid()
    {
      var heigthgrdMenu = grdPanel.ActualHeight;
      var heightStatusBar = stbStatusBar.ActualHeight;
    }

    /// <summary>
    /// Asigna valores por defecto a los StatusBarItem cuando se carga el form
    /// </summary>
    /// <param name="statusBar">StatusBarItem al que se le asignaran los valores</param>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }

    /// <summary>
    /// Revisa si alguna de las teclas Bloq Mayús, Bloq Núm o Insert está activo
    /// </summary>
    /// <param name="statusBar">StatusBarItem al que se le asignaran valores si está activa la tecla</param>
    /// <param name="key">tecla que revisaremos si se encuentra activa</param>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void CkeckKeysPress(System.Windows.Controls.Primitives.StatusBarItem statusBar, Key key)
    {
      var keyPess = Keyboard.GetKeyStates(key).ToString();

      if (keyPess.Contains("Toggled")) //si está activo el Bloq Mayús
      {
        statusBar.FontWeight = FontWeights.Bold;
        statusBar.Foreground = Brushes.Black;
      }
      else
      {
        KeyDefault(statusBar);
      }
    }

    #endregion

    #region Métodos públicos

    public void PrintReport()
    {
      throw new NotImplementedException();
    }

    public void ShowReport()
    {
      throw new NotImplementedException();
    }

    public void ShowReportDesigner()
    {
      throw new NotImplementedException();
    }

    #endregion

    private void btnExtRate_Click(object sender, RoutedEventArgs e)
    {
      //Verificamos si el usuario cuenta con los permisos suficientes
      if (!App.User.HasPermission(EnumPermission.ExchangeRates, EnumPermisionLevel.ReadOnly)) // _userData.Permissions.Exists(c => c.pppm == "EXCHRATES" && c.pppl >= 1))
      {
        MessageBox.Show("User doesn't have access", "Exchange Rate");
        return;
      }

      frmExchangeRate _frExtChangeRate = new frmExchangeRate(_dtpServerDate);
      _frExtChangeRate.ShowInTaskbar = false;
      _frExtChangeRate.Owner = this;
      _frExtChangeRate.ShowDialog();
    }

    private void ChkguShow_Click(object sender, RoutedEventArgs e)
    {
      // Obtenemos el row seleccionado!

      DataGridRow row = sender as DataGridRow;
      CheckBox _chekedValue = sender as CheckBox;
      GuestPremanifestHost guestHost = (GuestPremanifestHost)_chekedValue.DataContext;

      //CheckBox _chekedValue = sender as CheckBox;

      //Validamos que sea un invitado valido
      if (ValidateGuest(guestHost, EnumPermission.Show, "guShow"))
      {
        // Desplegamos el formulario Show
        frmShow _frmShow = new frmShow();
        _frmShow.ShowInTaskbar = false;
        _frmShow.Owner = this;
        _frmShow.ShowDialog();

        // Verificamos si se cambio el status de show

      }
      else
      {
        if (!guestHost.guShow)
        {
          _chekedValue.IsChecked = false;
        }
      }
    }

    /// <summary>
    /// Función encargada de validar que sea un usuario valido con sus respectivos permisos
    /// </summary>
    /// <param name="guestHost"></param>
    /// <returns> true - user valid | false - user no valid </returns>
    /// <history>
    /// [vipacheco] 02/15/2016 Created
    /// </history>
    private bool ValidateGuest(GuestPremanifestHost guestHost, EnumPermission permission, string strField)
    {
      if (guestHost.guBookCanc) // Validamos que no sea un booking cancelado
      {
        UIHelper.ShowMessage("Cancelled booking.", MessageBoxImage.Exclamation);
        return false;
      }
      else if (!App.User.HasPermission(permission, EnumPermisionLevel.ReadOnly)) // validamos los permisos del usuario - SIN PERMISOS
      {
        UIHelper.ShowMessage("Access denied.", MessageBoxImage.Exclamation);
        return false;
      }
      else if (!App.User.HasPermission(permission, EnumPermisionLevel.Standard)) // PERMISO - Solo Lectura
      {
        if (!guestHost.guMealTicket)
        {
          UIHelper.ShowMessage("You have read access.", MessageBoxImage.Exclamation);
          return false;
        }
      }
      else if (guestHost.guShow == false && (strField == "guMealTicket" || strField == "guSale"))
      {
        UIHelper.ShowMessage("This option should have a show marked.", MessageBoxImage.Exclamation);
        return false;
      }
      return true;
    }

    /// <summary>
    /// Función para madar ejecutar el formulario Gifts Receipts
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/17/2016 Created
    /// </history>
    private void btnGiftsReceipts_Click(object sender, RoutedEventArgs e)
    {
      frmGiftsReceipts _frmGiftsReceipts = new frmGiftsReceipts();
      _frmGiftsReceipts.ShowInTaskbar = false;
      _frmGiftsReceipts.Owner = this;
      _frmGiftsReceipts.ShowDialog();
    }

    private void btnMealTickets_Click(object sender, RoutedEventArgs e)
    {
      // Se verifica si el usuario tiene permisos de edicion!
      bool modeEdit = App.User.HasPermission(EnumPermission.MealTicket, EnumPermisionLevel.Standard);

      // Se invoca el formulario de acuerdo al permiso del usuario!
      frmMealTickets _frmMealTickets = new frmMealTickets();
      _frmMealTickets.ShowInTaskbar = false;
      _frmMealTickets.modeOpen = ((modeEdit == true) ? EnumModeOpen.Edit : EnumModeOpen.Search);
      _frmMealTickets.Owner = this;
      _frmMealTickets.ShowDialog();
    }

    private void guMealTickets_Click(object sender, RoutedEventArgs e)
    {
      // Obtenemos el row seleccionado!
      CheckBox _chekedValue = sender as CheckBox;
      GuestPremanifestHost guestHost = (GuestPremanifestHost)_chekedValue.DataContext;

      //Validamos que sea un invitado valido
      if (ValidateGuest(guestHost, EnumPermission.MealTicket, "guMealTickets"))
      {
        // Desplegamos el formulario Show
        frmMealTickets _frmMealTickets = new frmMealTickets(guestHost.guID);
        _frmMealTickets.ShowInTaskbar = false;

        if (guestHost.guMealTicket)
          _frmMealTickets.modeOpen = EnumModeOpen.Preview;
        else
          _frmMealTickets.modeOpen = EnumModeOpen.Edit;

        _frmMealTickets.Owner = this;
        _frmMealTickets.ShowDialog();

      }
    }
  }

}

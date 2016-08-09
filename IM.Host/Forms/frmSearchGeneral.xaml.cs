using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Enums;
using IM.Model;
using IM.Model.Enums;
using PalaceResorts.Common.PalaceTools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchGeneral.xaml
  /// </summary>
  /// <history>
  /// [vipacheco] 06/Junio/2016 Created
  /// </history>
  public partial class frmSearchGeneral : Window
  {
    EnumModule _module;

    #region Contructor
    public frmSearchGeneral(EnumModule module = EnumModule.Search)
    {
      _module = module;

      InitializeComponent();

      // Fechas
      dtpStart.Value = frmHost.dtpServerDate.AddDays(-15);
      dtpEnd.Value = frmHost.dtpServerDate;
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      CollectionViewSource dsSalesRoom = ((CollectionViewSource)(this.FindResource("dsSalesRoom")));
      CollectionViewSource dsLeadSource = ((CollectionViewSource)(this.FindResource("dsLeadSource")));

      // Lead Sources
      dsSalesRoom.Source = frmHost._lstLeadSources;
      // Sales Room 
      dsSalesRoom.Source = frmHost._lstSalesRoom;


      switch (_module)
      {
        case EnumModule.Search:
          btnOk.Visibility = Visibility.Collapsed;
          break;
        case EnumModule.Transfer:
          stkButtons.Visibility = Visibility.Collapsed;
          break;
        case EnumModule.Invit:
          stkButtons.Visibility = Visibility.Collapsed;
          break;
      }

      // Impedimos modificar los datos si el sistema esta en modo de solo lectura
      if (ConfigHelper.GetString("ReadOnly").ToUpper().Equals("TRUE"))
      {
        btnShow.Visibility = Visibility.Hidden;
        btnMealTicket.Visibility = Visibility.Hidden;
        btnReceipt.Visibility = Visibility.Hidden;
        btnSale.Visibility = Visibility.Hidden;
      }
    }
    #endregion

    #region Load_Grid
    /// <summary>
    /// Carga los resultados en el grid
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private async void Load_Grid()
    {
      grdGuest.ItemsSource = await BRGuests.GetSearchGuestGeneral(dtpStart.Value.Value.Date, dtpEnd.Value.Value.Date, string.IsNullOrEmpty(txtguID.Text) ? 0 : Convert.ToInt32(txtguID.Text),
                                                                  string.IsNullOrEmpty(txtName.Text) ? "" : txtName.Text, cboLeadSource.SelectedValue.ToString(), cboSalesRoom.SelectedValue.ToString(),
                                                                  string.IsNullOrEmpty(txtRoomNum.Text) ? "" : txtRoomNum.Text, string.IsNullOrEmpty(txtReservation.Text) ? "" : txtReservation.Text,                                                                  string.IsNullOrEmpty(txtPR.Text) ? "" : txtPR.Text);
    }
    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Permite buscar huespedes
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      // si los datos son validos
      if (ValidateCriteria())
      {
        Load_Grid();
      }
    }
    #endregion

    #region ValidateCriteria
    /// <summary>
    /// Valida los criterios de busqueda
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 06/Junio/2016 created
    /// </history>
    private bool ValidateCriteria()
    {
      // validamos que se haya ingresado algun criterio de busqueda
      if (cboLeadSource.SelectedValue == null && cboSalesRoom.SelectedValue == null && txtRoomNum.Text == "" && txtReservation.Text == "" &&
          txtPR.Text == "" && txtguID.Text == "" && dtpStart.Value.Value == null && dtpEnd.Value.Value == null)
      {
        UIHelper.ShowMessage("Please specify a search criteria.", MessageBoxImage.Information, "Search General");
        return false;
      }

      #region Fechas
      // validamos la fecha inicial
      if (dtpStart.Value == null)
      {
        UIHelper.ShowMessage("Specify the Start Date.", MessageBoxImage.Information, "Self Gen");
        return false;
      }
      // validamos la fecha final
      else if (dtpEnd.Value == null)
      {
        UIHelper.ShowMessage("Specify the End Date.", MessageBoxImage.Information, "Self Gen");
        return false;
      }
      // validamos que no se traslapen las fechas
      else if (dtpStart.Value.Value > dtpEnd.Value.Value)
      {
        UIHelper.ShowMessage("Start Date can not be greater than End Date.", MessageBoxImage.Information, "Self Gen");
        return false;
      }
      #endregion

      return true;
    }
    #endregion

    #region grdGuest_SelectionChanged
    /// <summary>
    /// Funcion que se encarga de validar el total de datos obtenidos en el datagrid
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void grdGuest_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (grdGuest.Items.Count == 0)
      {
        StatusBarReg.Content = "0 Guests";
        return;
      }
      StatusBarReg.Content = $"{grdGuest.Items.Count} Guests";
    }
    #endregion

    #region StatusBar

    #region Window_KeyDown
    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [vipacheco] 03/Junio/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
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
    #endregion

    #region CkeckKeysPress
    /// <summary>
    /// Verifica que la tecla se encuentre activa/inactiva, para cambiar el estilo de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [vipacheco] 03/Junio/2016 Created
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

    #region KeyDefault
    /// <summary>
    /// Configuracion inicial de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [vipacheco] 03/Junio/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }
    #endregion

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [vipacheco] 03/Junio/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #endregion

    #region ValidatePermissions
    /// <summary>
    /// Valida los permisos
    /// </summary>
    /// <param name="enumPermission"></param>
    /// <param name="GuestID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private bool ValidatePermissions(EnumPermission enumPermission, ref int GuestID)
    {
      // validamos que haya seleccionado al menos una invitacion
      Guest selected = grdGuest.SelectedItem as Guest;

      if (selected == null)
      {
        UIHelper.ShowMessage("Select at least one Guest.", MessageBoxImage.Information, "Search General");
        return false;
      }

      // Validamos el permisos del usuario
      else if (!App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard))
      {
        UIHelper.ShowMessage("You have read access.", MessageBoxImage.Information, "Search General");
        return false;
      }
      else
      {
        GuestID = selected.guID;
      }

      return true;
    }
    #endregion

    #region btnReceipt_Click
    /// <summary>
    /// Despliega el formulario de recibos de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnReceipt_Click(object sender, RoutedEventArgs e)
    {
      int GuestID = 0;
      if (ValidatePermissions(EnumPermission.GiftsReceipts, ref GuestID))
      {
        bool _edit = App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard);

        frmGiftsReceipts giftsReceipt = new frmGiftsReceipts(GuestID)
        {
          Owner = this,
          modeOpenBy = EnumOpenBy.Checkbox,
          modeOpen = (_edit) ? EnumModeOpen.Edit : EnumModeOpen.Preview
        };
        giftsReceipt.ShowDialog();
      }
    }
    #endregion

    #region btnShow_Click
    /// <summary>
    /// Despliega el formulario de show
    /// </summary>
    /// <hsitory>
    /// [vipacheco] 06/Junio/2016 Created
    /// </hsitory>
    private void btnShow_Click(object sender, RoutedEventArgs e)
    {
      int GuestID = 0;
      if (ValidatePermissions(EnumPermission.Show, ref GuestID))
      {
        frmShow _frmShow = new frmShow(GuestID);
        _frmShow.ShowInTaskbar = false;
        _frmShow.Owner = this;
        _frmShow.ShowDialog();
      }
    }

    #endregion

    #region btnMealTicket_Click
    /// <summary>
    /// Despliega el formulario de tickets de comida
    /// </summary>
    /// <hsitory>
    /// [vipacheco] 06/Junio/2016 Created
    /// </hsitory>
    private void btnMealTicket_Click(object sender, RoutedEventArgs e)
    {
      int GuestID = 0;
      if (ValidatePermissions(EnumPermission.MealTicket, ref GuestID))
      {
        frmMealTickets mealticket = new frmMealTickets(GuestID) { Owner = this };
        mealticket.ShowDialog();
      }
    }
    #endregion

    #region btnSale_Click
    /// <summary>
    /// Despliega el formulario de ventas
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnSale_Click(object sender, RoutedEventArgs e)
    {
      int GuestID = 0;
      if (ValidatePermissions(EnumPermission.Sales, ref GuestID))
      {

      }
    }
    #endregion

  }
}

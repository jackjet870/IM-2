using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Enums;
using IM.Model;
using IM.Model.Enums;
using PalaceResorts.Common.PalaceTools;
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

    #region Variables

    CollectionViewSource _dsSalesRoom;
    CollectionViewSource _dsLeadSource; 

    #endregion

    #region Contructor
    public frmSearchGeneral()
    {
      InitializeComponent();

      // Fechas
      dtpStart.Value = frmHost._dtpServerDate.AddDays(-15);
      dtpEnd.Value = frmHost._dtpServerDate;
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
      _dsSalesRoom = ((CollectionViewSource)(this.FindResource("dsSalesRoom")));
      _dsLeadSource = ((CollectionViewSource)(this.FindResource("dsLeadSource")));

      // Lead Sources
      _dsLeadSource.Source = frmHost._lstLeadSources;

      // Sales
      _dsSalesRoom.Source = frmHost._lstSalesRoom;

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
      int GuestID = string.IsNullOrEmpty(txtguID.Text) ? 0 : Convert.ToInt32(txtguID.Text);
      string GuestName = string.IsNullOrEmpty(txtName.Text) ? "" : txtName.Text;
      string LeadSource = cboLeadSource.SelectedValue.ToString();
      string SalesRoom = cboSalesRoom.SelectedValue.ToString();
      string RoomNum = string.IsNullOrEmpty(txtRoomNum.Text) ? "" : txtRoomNum.Text;
      string Reservation = string.IsNullOrEmpty(txtReservation.Text) ? "" : txtReservation.Text;
      string PR = string.IsNullOrEmpty(txtPR.Text) ? "" : txtPR.Text;


      grdGuest.ItemsSource = await BRGuests.GetSearchGuest_General(dtpStart.Value.Value.Date, dtpEnd.Value.Value.Date, GuestID, GuestName, LeadSource, SalesRoom, RoomNum, Reservation, PR);
    }
    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Permite buscar huespedes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
      if (dtpStart.Value.Value == null)
      {
        UIHelper.ShowMessage("Specify the Start Date.", MessageBoxImage.Information, "Self Gen");
        return false;
      }
      // validamos la fecha final
      else if (dtpEnd.Value.Value == null)
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnReceipt_Click(object sender, RoutedEventArgs e)
    {
      int GuestID = 0;
      if (ValidatePermissions(EnumPermission.GiftsReceipts, ref GuestID))
      {
        bool _edit = App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard);

        frmGiftsReceipts _frmGiftsReceipt = new frmGiftsReceipts(GuestID);
        _frmGiftsReceipt.ShowInTaskbar = false;
        _frmGiftsReceipt.modeOpenBy = EnumOpenBy.Checkbox;
        _frmGiftsReceipt.modeOpen = (_edit) ? EnumModeOpen.Edit : EnumModeOpen.Preview;
        _frmGiftsReceipt.Owner = this;
        _frmGiftsReceipt.ShowDialog();
      }
    }
    #endregion

    #region btnShow_Click
    /// <summary>
    /// Despliega el formulario de show
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <hsitory>
    /// [vipacheco] 06/Junio/2016 Created
    /// </hsitory>
    private void btnMealTicket_Click(object sender, RoutedEventArgs e)
    {
      int GuestID = 0;
      if (ValidatePermissions(EnumPermission.MealTicket, ref GuestID))
      {
        frmMealTickets _frmMealTicket = new frmMealTickets(GuestID);
        _frmMealTicket.ShowInTaskbar = false;
        _frmMealTicket.Owner = this;
        _frmMealTicket.ShowDialog();
      }
    }
    #endregion

    #region btnSale_Click
    /// <summary>
    /// Despliega el formulario de ventas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

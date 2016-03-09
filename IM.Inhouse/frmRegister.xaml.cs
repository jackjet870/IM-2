using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Classes;

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmRegister.xaml//6515-8201 Eric Chihun 
  /// </summary>
  public partial class frmRegister : Window
  {
    #region Atributos

    private UserData _userData;
    private LocationLogin _locationLogin = new LocationLogin();
    private UserLogin _userLogin = new UserLogin();
    private CollectionViewSource _premanifestViewSource;
    private CollectionViewSource _guestArrivalViewSource;
    private CollectionViewSource _availableViewSource;
    private DateTime _serverDate;
    private int _available, _invited, _onGroup, _info = 0;
    private string _markets = "ALL", _LeadSource = string.Empty;

    #endregion

    #region Constructores y destructores

    public frmRegister(UserData userData)
    {
      InitializeComponent();
      _userData = userData;
      _userLogin = userData.User;
      _locationLogin = userData.Location;
    }

    #endregion

    #region Metodos

    #region EnabledCtrls

    /// <summary>
    /// Configura los controles para que esten habilitados o deshabilidatos cuando se presionan los TabsControl
    /// </summary>
    /// <param name="Av">gprAvailable Falso / Verdadero</param>
    /// <param name="Da">dtpDate  Falso / Verdadero></param>
    /// <param name="Inf">gprInfo Falso / Verdadero </param>
    /// <param name="Inv">gprInvited Falso/ Verdadero </param>
    /// <history>
    /// [jorcanche] 23/02/2015 Created
    /// </history>
    /// <returns>Void</returns>
    private void EnabledCtrls(bool Av, bool Da, bool Inf, bool Inv)
    {
      gprAvailable.IsEnabled = Av;
      dtpDate.IsEnabled = Da;
      gprInfo.IsEnabled = Inf;
      gprInvited.IsEnabled = Inv;
    }

    #endregion

    #region LoadGrid

    /// <summary>
    /// Metodo que sirve para carga el DataGrid's en el 
    /// </summary>
    private void LoadGrid()
    {
      if (_guestArrivalViewSource != null && _premanifestViewSource != null && _availableViewSource != null)
      {
        if (tabArrivals.IsSelected)
        {
          _guestArrivalViewSource.Source = BRGuests.GetGuestsArrivals(_serverDate, _LeadSource, _markets, _available, _info, _invited, _onGroup);
        }
        else
        {
          if (tabAvailables.IsSelected)
          {
            _availableViewSource.Source = BRGuests.GetGuestsAvailables(BRHelpers.GetServerDate().Date, _LeadSource, _markets, _info, _invited, _onGroup);
          }
          else
          {
            _premanifestViewSource.Source = BRGuests.GetGuestsPremanifest(_serverDate, _LeadSource, _markets, _onGroup);
          }
        }
      }
    }

    #endregion

    #region LoadListMarkets

    private void LoadListMarkets()
    {
      listMarkets.ItemsSource = BRMarkets.GetMarkets(1);
    }

    #endregion
    
    #endregion

    #region Eventos del formulario

    #region Window_Loaded

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

      _LeadSource = _locationLogin.loID.ToString();
      txtUser.Text = _userLogin.peN.ToString();
      txtLocation.Text = _locationLogin.loN.ToString();
      //Le asignamos la fecha del servidor
      dtpDate.SelectedDate = BRHelpers.GetServerDate().Date;
      
      _guestArrivalViewSource = ((CollectionViewSource)(this.FindResource("guestArrivalViewSource")));
      _availableViewSource = ((CollectionViewSource)(this.FindResource("availableViewSource")));
      _premanifestViewSource = ((CollectionViewSource)(this.FindResource("premanifestViewSource")));
      LoadGrid();
      LoadListMarkets();
    }

    #endregion

    #region listMarkets_SelectionChanged

    private void listMarkets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      int cont = 0; _markets = string.Empty;
      var selectedItems = listMarkets.SelectedItems;
      foreach (MarketShort selectedItem in selectedItems)
      {
        cont = cont + 1;
        _markets += selectedItem.mkID.ToString();
        if (selectedItems.Count > 1 && cont < selectedItems.Count)
        {
          _markets = _markets + ",";
        }
      }
      LoadGrid();
    }

    #endregion

    #region tabArrivals_Clicked

    private void tabArrivals_Clicked(object sender, MouseButtonEventArgs e)
    {
      EnabledCtrls(true, true, true, true);
    }

    #endregion

    #region tabAvailables_Clicked

    private void tabAvailables_Clicked(object sender, MouseButtonEventArgs e)
    {
      EnabledCtrls(false, false, true, true);
    }

    #endregion

    #region tabPremanifiest_Clicked

    private void tabPremanifiest_Clicked(object sender, MouseButtonEventArgs e)
    {
      EnabledCtrls(false, true, false, false);
    }
    #endregion

    #region dtpDate_SelectedDateChanged

    private void dtpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      //Obtener el valor actual del que tiene el dtpDate
      var picker = sender as DatePicker;// DateTime? date = picker.SelectedDate;
      if (!picker.SelectedDate.HasValue)
      {
        //Cuando el usuario ingresa una fecha invalida
        MessageBox.Show("Favor de ingresar una fecha valida", "Fecha Invalida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //Y le asignamos la fecha del servidor (la actual hora actual)
        dtpDate.SelectedDate = BRHelpers.GetServerDate();
      }
      else
      {
        //le asignamos el valor del dtpDate a la variable global para que otro control tenga acceso al valor actual del dtpDate
        _serverDate = picker.SelectedDate.Value;// date.Value;
        //Cargamos el grid del tab que esta seleccionado
        LoadGrid();
        //gprInfo.BindingGroup.GetValue                 
      }
    }

    bool? guCheckIn = true;

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {

    }

    private void ChkguCheckIn_Click(object sender, RoutedEventArgs e)
    {
      var ChkguCheckIn = sender as CheckBox;
      guCheckIn = ChkguCheckIn.IsChecked;
    }
    private void rb_Checked(object sender, RoutedEventArgs e)
    {
      var ck = sender as RadioButton;
      switch (ck.Name)
      {
        case "rbYesAvailable":
          _available = 1;
          break;
        case "rbNoAvailable":
          _available = 0;
          break;
        case "rbBothAvailable":
          _available = 2;
          break;
        case "rbYesInvited":
          _invited = 1;
          break;
        case "rbNoInvited":
          _invited = 0;
          break;
        case "rbBothInvited":
          _invited = 2;
          break;
        case "rbYesOnGroup":
          _onGroup = 1;
          break;
        case "rbNoOnGroup":
          _onGroup = 0;
          break;
        case "rbBothOnGroup":
          _onGroup = 2;
          break;
        case "rbYesInfo":
          _info = 1;
          break;
        case "rbNoInfo":
          _info = 0;
          break;
        case "rbBothInfo":
          _info = 2;
          break;
      }
      LoadGrid();
    }
    private void ChkguAvail_Click(object sender, RoutedEventArgs e)
    {
      GuestArrival itema = dgGuestArrival.Items.GetItemAt(dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)) as GuestArrival;
      var chkguAvail = sender as CheckBox;
      chkguAvail.IsChecked = itema.guAvail;

      //bool? con = ck.IsChecked;      
      if (!guCheckIn.Value)
      {
        MessageBox.Show(this, "Guest has not made Check-in.", "Check-in", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
        chkguAvail.IsChecked = itema.guAvail;
      }
      else
      {
        if (_userData.HasPermission("AVAIL",Model.Enums.EnumPermisionLevel.ReadOnly))
        {
          frmAvailability _frmAvai = new frmAvailability(_userData);
          _frmAvai.Owner = this;
          _frmAvai.ShowDialog();

        }
        else
          chkguAvail.IsChecked = itema.guAvail;
      }
      #region codigo 
      GuestArrival item = dgGuestArrival.Items[dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem)] as GuestArrival;
      int Column = dgGuestArrival.CurrentCell.Column.DisplayIndex;
      int CurrentColumn = dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentColumn);
      int CurrentCell = dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentCell);
      int CurrentItem = dgGuestArrival.Items.IndexOf(dgGuestArrival.CurrentItem);
      #endregion
    }
    private void ChkguInfoColumn_Click(object sender, RoutedEventArgs e)
    {
      var chk = sender as CheckBox;    //bool? con = ck.IsChecked;
      if (chk.IsChecked.Value)
      {
        frmContact _frmCont = new frmContact(_userData);
        _frmCont.Owner = this;
        _frmCont.ShowDialog();
      }
    }

    #endregion
  }
  #endregion
}
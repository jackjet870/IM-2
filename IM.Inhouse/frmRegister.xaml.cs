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
using System.Data.Entity.Core.Objects;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Entities;

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmRegister.xaml//6515-8201 Eric Chihun 
  /// </summary>
  public partial class frmRegister : Window
  {
    public frmRegister(UserData userData)
    {
      InitializeComponent();
      _userLogin = userData.User;
      _locationLogin = userData.Location;
    }
    private locationLogin _locationLogin = new locationLogin();
    private userLogin _userLogin = new userLogin();
    private CollectionViewSource _premanifestViewSource;
    private CollectionViewSource _guestArrivalViewSource;
    private CollectionViewSource _availableViewSource;
    private DateTime _serverDate;
    private int _available, _invited, _onGroup, _info = 0;
    private string _markets = "ALL", LeadSource = string.Empty;
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Le asignamos la fecha del servidor
      LeadSource = _locationLogin.loID.ToString();
      txtUser.Text = _userLogin.peN.ToString();
      txtLocation.Text = _locationLogin.loN.ToString();
      dtpDate.SelectedDate = GetDateServer();
      _guestArrivalViewSource = ((CollectionViewSource)(this.FindResource("guestArrivalViewSource")));
      _availableViewSource = ((CollectionViewSource)(this.FindResource("availableViewSource")));
      _premanifestViewSource = ((CollectionViewSource)(this.FindResource("premanifestViewSource")));
      LoadGrid();
      LoadGroupMarkets();
    }
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
    /// <summary>
    /// Metodo que sirve para carga el DataGrid's en el 
    /// </summary>
    private void LoadGrid()
    {
      if (_guestArrivalViewSource != null && premanifestDataGrid != null && _availableViewSource != null)
      {
        if (tabArrivals.IsSelected)
        {
          _guestArrivalViewSource.Source = BRGuest.GetGuestsArrivals(_serverDate, "MPS", _markets, _available, _info, _invited, _onGroup);
        }
        else
        {
          if (tabAvailables.IsSelected)
          {
            _availableViewSource.Source = BRGuest.GetGuestsAvailables(GetDateServer(), "MPS", _markets, _info, _invited, _onGroup);
          }
          else
          {
            _premanifestViewSource.Source = BRGuest.GetGuestsPremanifests(_serverDate, "MPS", _markets, _onGroup);
          }
        }
      }
    }
    private void LoadGroupMarkets()
    {
      List<GetMarkets> Mercados = IM.BusinessRules.BR.BRMarkets.GetMarkets(1).ToList<GetMarkets>();
      listMarkets.ItemsSource = Mercados;
    }
    private DateTime GetDateServer()
    {
      return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
    }
    #region Eventos
    private void listMarkets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      int cont = 0; _markets = string.Empty;
      var selectedItems = listMarkets.SelectedItems;
      foreach (GetMarkets selectedItem in selectedItems)
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
    private void tabArrivals_Clicked(object sender, MouseButtonEventArgs e)
    {
      EnabledCtrls(true, true, true, true);
      LoadGrid();
    }
    private void tabAvailables_Clicked(object sender, MouseButtonEventArgs e)
    {
      EnabledCtrls(true, true, true, true);
      LoadGrid();
    }
    private void tabPremanifiest_Clicked(object sender, MouseButtonEventArgs e)
    {
      EnabledCtrls(true, true, true, true);
      LoadGrid();
    }

    private void dtpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      //Obtener el valor actual del que tiene el dtpDate
      var picker = sender as DatePicker;
      DateTime? date = picker.SelectedDate;
      if (date == null)
      {
        //Cuando el usuario ingresa una fecha invalida
        MessageBox.Show("Favor de ingresar una fecha valida", "Fecha Invalida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //Y le asignamos la fecha del servidor (la actual hora actual)
        dtpDate.SelectedDate = GetDateServer();
      }
      else
      {
        //le asignamos el valor del dtpDate a la variable global para que otro control tenga acceso al valor actual del dtpDate
        _serverDate = date.Value;
        //Cargamos el grid del tab que esta seleccionado
        LoadGrid();
        //gprInfo.BindingGroup.GetValue                 
      }
    }
    private void rbAvailable_Checked(object sender, RoutedEventArgs e)
    {
      RadioButton ck = sender as RadioButton;
      if (ck.Name == "rbYesAvailable")
      {
        _available = 1;
      }
      if (ck.Name == "rbNoAvailable")
      {
        _available = 0;
      }
      if (ck.Name == "rbBothAvailable")
      {
        _available = 2;
      }

      LoadGrid();
    }
    private void rbInvited_Checked(object sender, RoutedEventArgs e)
    {
      RadioButton ck = sender as RadioButton;
      if (ck.Name == "rbYesInvited")
      {
        _invited = 1;
      }
      if (ck.Name == "rbNoInvited")
      {
        _invited = 0;
      }
      if (ck.Name == "rbBothInvited")
      {
        _invited = 2;
      }

      LoadGrid();
    }
    private void rbOnGroup_Checked(object sender, RoutedEventArgs e)
    {
      RadioButton ck = sender as RadioButton;
      if (ck.Name == "rbYesOnGroup")
      {
        _onGroup = 1;
      }
      if (ck.Name == "rbNoOnGroup")
      {
        _onGroup = 0;
      }
      if (ck.Name == "rbBothOnGroup")
      {
        _onGroup = 2;
      }

      LoadGrid();
    }
    private void rbInfo_Checked(object sender, RoutedEventArgs e)
    {
      RadioButton ck = sender as RadioButton;
      if (ck.Name == "rbYesInfo")
      {
        _info = 1;
      }
      if (ck.Name == "rbNoInfo")
      {
        _info = 0;
      }
      if (ck.Name == "rbBothInfo")
      {
        _info = 2;
      }

      LoadGrid();
    }
  }
}
        #endregion

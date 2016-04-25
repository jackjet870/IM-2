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
using IM.Host.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmMealTicketsDetail.xaml
  /// </summary>
  public partial class frmMealTicketsDetail : Window
  {
    #region Variables
    public EnumModeOpen modeOpen;
    private int _meQty = 1;
    public MealTicket _mealTicketCurrency = new MealTicket();

    CollectionViewSource _dsRateType;
    CollectionViewSource _dsPersonnel;
    CollectionViewSource _dsAgency;
    CollectionViewSource _dsMealTicketType;
    #endregion

    #region Contructor
    public frmMealTicketsDetail()
    {
      InitializeComponent();
    } 
    #endregion

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _dsRateType = ((CollectionViewSource)(this.FindResource("dsRateType")));
      _dsPersonnel = ((CollectionViewSource)(this.FindResource("dsPersonnel")));
      _dsAgency = ((CollectionViewSource)(this.FindResource("dsAgency")));
      _dsMealTicketType = ((CollectionViewSource)(this.FindResource("dsMealTicketType")));

      #region switch
      switch (modeOpen)
      {
        #region EnumModeOpen.Add
        case EnumModeOpen.Add:
          dtpDate.Value = frmHost._dtpServerDate.Date;
          break;
        #endregion
        #region EnumModeOpen.Edit
        case EnumModeOpen.Edit:
          DataContext = _mealTicketCurrency;
          dtpDate.Value = _mealTicketCurrency.meD.Date;
          break;
        #endregion
        /*#region EnumModeOpen.Preview
        case EnumModeOpen.Preview:
          DataContext = _mealTicketCurrency;
          dtpDate.Value = (_mealTicketCurrency.meD.Date <= new DateTime(0001, 01, 1)) ? frmHost._dtpServerDate : _mealTicketCurrency.meD.Date;
          lblRateType.Visibility = Visibility.Hidden;
          cboRateType.Visibility = Visibility.Hidden;
          cboRateType.SelectedItem = null;
          lblAgency.Visibility = Visibility.Hidden;
          cboAgency.Visibility = Visibility.Hidden;
          cboAgency.SelectedItem = null;
          lblCollaborator.Visibility = Visibility.Hidden;
          cboCollaborator.Visibility = Visibility.Hidden;
          cboCollaborator.SelectedItem = null;
          lblRepresentative.Visibility = Visibility.Hidden;
          txtRepresentative.Visibility = Visibility.Hidden;
          txtRepresentative.Text = "";
          break;
        #endregion*/
        case EnumModeOpen.PreviewEdit: 
          HiddenControls();
          dtpDate.Value = frmHost._dtpServerDate.Date;
          break;
        case EnumModeOpen.Preview: 
          HiddenControls();
          DataContext = _mealTicketCurrency;
          dtpDate.Value = (_mealTicketCurrency.meD.Date <= new DateTime(0001, 01, 1)) ? frmHost._dtpServerDate : _mealTicketCurrency.meD.Date;
          break;
      } 
      #endregion

      //  Obtenemos los tipos de tarifa
      _dsRateType.Source = BRRateTypes.GetRateTypes(new RateType { raID = 1 }, 1, true, true);
      // Obtenemos los colaboradores
      _dsPersonnel.Source = BRPersonnel.GetPersonnel("ALL", "ALL", "ALL", 1);
      // Obtenemos las agencias
      _dsAgency.Source = BRAgencies.GetAgencies(1);
      // Obtenemos los tipos de cupones de comida.
      _dsMealTicketType.Source = BRMealTicketTypes.GetMealTicketType();
    }
    #endregion

    #region HiddenControls
    /// <summary>
    /// Oculta los controles necesarios segun el caso lo requiera.
    /// </summary>
    /// <history>
    /// [vipacheco] 04/04/2016 Created
    /// </history>
    private void HiddenControls()
    {
      lblRateType.Visibility = Visibility.Hidden;
      cboRateType.Visibility = Visibility.Hidden;
      cboRateType.SelectedItem = null;
      lblAgency.Visibility = Visibility.Hidden;
      cboAgency.Visibility = Visibility.Hidden;
      cboAgency.SelectedItem = null;
      lblCollaborator.Visibility = Visibility.Hidden;
      cboCollaborator.Visibility = Visibility.Hidden;
      cboCollaborator.SelectedItem = null;
      lblRepresentative.Visibility = Visibility.Hidden;
      txtRepresentative.Visibility = Visibility.Hidden;
      txtRepresentative.Text = "";
    } 
    #endregion

    #region cboRateType_SelectionChanged
    /// <summary>
    /// Funcion para inicializar los controles
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 23/03/2016 Created
    /// </history>
    private void cboRateType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      RateType _rateType = (RateType)cboRateType.SelectedItem;

      if (modeOpen != EnumModeOpen.Preview && modeOpen != EnumModeOpen.PreviewEdit)
      {
        if (_rateType != null) // Se verifica que el SelectedItem no sea null
        {
          if (_rateType.raID >= 2 && _rateType.raID < 4 ) // Si es diferente de tipo External!
          {
            controlVisibility(Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
          }
          else if (_rateType.raID == 4) // Es external
          {
            controlVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Visible);
          }
        }
        else// if (modeOpen != EnumModeOpen.Preview)
        {
          controlVisibility(Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
        }

        // Se verifica que no sean un nuevo MealTicket
        if (_rateType != null && txtAdults.Text != "" && txtMinors.Text != "" && _meQty >= 1)
        {
          txtTAdults.Text = string.Format("{0:$0.00}", CalculateAdult(((_rateType == null) ? 0 : _rateType.raID), ((_meQty >= 0) ? _meQty : 0), ((txtAdults.Text != "") ? Convert.ToInt32(txtAdults.Text) : 0)));
        }
      }
    }
    #endregion

    #region controlVisibility
    /// <summary>
    /// Función para la evaluacion de visibilidad de controles en el UI
    /// </summary>
    /// <param name="agency"></param>
    /// <param name="collaborator"></param>
    /// <param name="representative"></param>
    /// <history>
    /// [vipacheco] 23/03/2016 Created
    /// </history>
    private void controlVisibility(Visibility agency, Visibility collaborator, Visibility representative)
    {
      lblAgency.Visibility = agency;
      cboAgency.Visibility = agency;
      lblCollaborator.Visibility = collaborator;
      cboCollaborator.Visibility = collaborator;
      lblRepresentative.Visibility = representative;
      txtRepresentative.Visibility = representative;
    }
    #endregion

    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
    #endregion

    #region txtEvaluation_PreviewTextInput
    /// <summary>
    /// Funcion para evaluar que el texto introducido sea solamente numerico
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 23/03/2016
    /// </history>
    private void txtEvaluation_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      string _value = e.Text;

      // Se valida que solamente sean Digitos Numericos
      if (!char.IsDigit(Convert.ToChar(_value)))
      {
        e.Handled = true;
      }
    }
    #endregion

    #region CalculateAdult
    /// <summary>
    /// Función que devuelve el valor de adulto en base al RateType
    /// </summary>
    /// <param name="intRatetype"></param>
    /// <param name="intQt"></param>
    /// <param name="intAdults"></param>
    /// <history>
    /// [vipacheco] 23/03/2016 Created
    /// </history>
    private double CalculateAdult(int intRatetype, int intAdults, int intQt = 1)
    {
      MealTicketType _mealTicketType = (MealTicketType)cboType.SelectedItem;
      double Amt = 0;

      if (_mealTicketType != null)
      {

        if (intRatetype == 1)
        {
          Amt = intQt * intAdults * Convert.ToDouble(_mealTicketType.myPriceA);
        }
        else if (intRatetype == 2)
        {
          Amt = intQt * intAdults * Convert.ToDouble(_mealTicketType.myCollaboratorWithCost);
        }
        else if (intRatetype >= 3)
        {
          Amt = intQt * intAdults * Convert.ToDouble(_mealTicketType.myCollaboratorWithoutCost);
        }
      }

      return Amt;
    }
    #endregion

    #region validateFields
    /// <summary>
    /// Valida los campos obligatorios segun sea requerido
    /// </summary>
    /// <returns> False - Algun Campo Vacio | True - Campos llenados correctamente </returns>
    /// <history>
    /// [vipacheco] 01/04/2016 Created
    /// </history>
    private bool validateFields()
    {
      MealTicketType _mealType = (MealTicketType)cboType.SelectedItem; //

      if (frmMealTickets._pguId == 0)
      {
        RateType _rateType = (RateType)cboRateType.SelectedItem; //mera
        PersonnelShort _personnel = (PersonnelShort)cboCollaborator.SelectedItem;
        AgencyShort _agency = (AgencyShort)cboAgency.SelectedItem;

        if (_rateType == null)
        {
          UIHelper.ShowMessage("Select an option of rate type, please", MessageBoxImage.Information);
          return false;
        }
        else if (_rateType.raID >= 2 && _rateType.raID < 4 && _personnel == null)
        {
          UIHelper.ShowMessage("Select a collaborator, please", MessageBoxImage.Information);
          return false;
        }
        else if (_rateType.raID == 4 && (_agency == null || txtRepresentative.Text == ""))
        {
          UIHelper.ShowMessage("Select an agency and write the representative name in the field for External option.", MessageBoxImage.Information);
          return false;
        }
        else if (_mealType == null)
        {
          UIHelper.ShowMessage("Select an option of meal type, please", MessageBoxImage.Information);
          return false;
        }
      }
      else
      {
        if (_mealType == null)
        {
          UIHelper.ShowMessage("Select an option of meal type, please", MessageBoxImage.Information);
          return false;
        }
      }

      return true;
    } 
    #endregion

    #region cboType_SelectionChanged
    private void cboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      RateType _rateType = (RateType)cboRateType.SelectedItem;
      MealTicketType _mealTicketType = (MealTicketType)cboType.SelectedItem;

      if (_mealTicketType != null)
      {
        if (!_mealTicketType.myWPax)
        {
          txtTAdults.Text = string.Format("{0:$0.00}", CalculateAdult(((_rateType == null) ? 0 : _rateType.raID), ((_meQty >= 0) ? _meQty : 0), ((txtAdults.Text != "") ? Convert.ToInt32(txtAdults.Text) : 0)));
          decimal _minors = (_meQty * ((txtMinors.Text != "") ? Convert.ToInt32(txtMinors.Text) : 0) * ((_mealTicketType != null) ? _mealTicketType.myPriceM : 0));
          txtTMinors.Text = string.Format("{0:$0.00}", Convert.ToDouble(_minors));
        }
        else
        {
          txtAdults.Text = ((txtAdults.Text != "") ? txtAdults.Text : "0");
          txtMinors.Text = ((txtMinors.Text != "") ? txtMinors.Text : "0");
          txtTAdults.Text = ((txtTAdults.Text != "") ? txtTAdults.Text : "$0.00");
          txtTMinors.Text = ((txtTMinors.Text != "") ? txtTMinors.Text : "$0.00");
          txtAdults_LostFocus(null, null);
          txtMinors_LostFocus(null, null);
        }
      }
    }
    #endregion

    #region txtAdults_LostFocus
    private void txtAdults_LostFocus(object sender, RoutedEventArgs e)
    {
      RateType _rateType = (RateType)cboRateType.SelectedItem;
      MealTicketType _mealTicketType = (MealTicketType)cboType.SelectedItem;

      if (_mealTicketType != null)
      {
        if (!_mealTicketType.myWPax)
        {
          txtTAdults.Text = string.Format("{0:$0.00}", CalculateAdult(((_rateType == null) ? 0 : _rateType.raID), ((_meQty >= 0) ? _meQty : 0), ((txtAdults.Text != "") ? Convert.ToInt32(txtAdults.Text) : 0)));
        }
        else
        {
          txtTAdults.Text = string.Format("{0:$0.00}", CalculateAdult(((_rateType == null) ? 0 : _rateType.raID), ((_meQty >= 0) ? _meQty : 0), ((txtAdults.Text != "") ? Convert.ToInt32(txtAdults.Text) : 0)));
        }
      }
    }
    #endregion

    #region txtMinors_LostFocus
    private void txtMinors_LostFocus(object sender, RoutedEventArgs e)
    {
      RateType _rateType = (RateType)cboRateType.SelectedItem;
      MealTicketType _mealTicketType = (MealTicketType)cboType.SelectedItem;

      if (_mealTicketType != null)
      {
        if (!_mealTicketType.myWPax)
        {
          decimal _minors = (_meQty * ((txtMinors.Text != "") ? Convert.ToInt32(txtMinors.Text) : 0) * ((_mealTicketType != null) ? _mealTicketType.myPriceM : 0));
          txtTMinors.Text = string.Format("{0:$0.00}", Convert.ToDouble(_minors));
        }
        else
        {
          decimal _minors = (_meQty * ((txtMinors.Text != "") ? Convert.ToInt32(txtMinors.Text) : 0) * ((_mealTicketType != null) ? _mealTicketType.myPriceM : 0));
          txtTMinors.Text = string.Format("{0:$0.00}", Convert.ToDouble(_minors));
        }
      }
    }
    #endregion

    #region btnSave_Click
    /// <summary>
    /// Guarda la informacion proporcionada!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 23/03/2016 Created
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (validateFields())
      {
        RateType _rateType = (RateType)cboRateType.SelectedItem; 
        MealTicketType _mealType = (MealTicketType)cboType.SelectedItem; 
        PersonnelShort _personnel = (PersonnelShort)cboCollaborator.SelectedItem;
        AgencyShort _agency = (AgencyShort)cboAgency.SelectedItem;

        int _meAdults = Convert.ToInt32((txtAdults.Text == "") ? "0" : txtAdults.Text);
        int _meMinors = Convert.ToInt32((txtAdults.Text == "") ? "0" : txtMinors.Text);
        string _meTAdultsString = txtTAdults.Text.Substring(1, txtTAdults.Text.Length - 1);
        string _meTMinorsString = txtTMinors.Text.Substring(1, txtTMinors.Text.Length - 1);

        // Row New with GuestID == 0
        if (modeOpen == EnumModeOpen.Add && frmMealTickets._pguId == 0)
        {
          // Obtenemos el folio a asignar
          int folioNew = 1 + BRMealTicketFolios.GetMaxMealTicketFolio(App.User.SalesRoom.srID, _mealType.myID, _rateType.raID);

          MealTicket _newMealticket = CreateMealTicket(_rateType, _mealType, _personnel, _agency, _meAdults, _meMinors, _meTAdultsString, _meTMinorsString, folioNew);

          //Actualizamos el folio!
          BRMealTicketFolios.UpdateMealTicketFolio(App.User.SalesRoom.srID, _mealType.myID, _rateType.raID, folioNew + "");

          //Guardamos el Meal Ticket Creado
          BRMealTickets.InsertNewMealTicket(_newMealticket);
        }
        // Edition Row with GuestID == 0
        else if (modeOpen == EnumModeOpen.Edit && frmMealTickets._pguId == 0)
        {
          int folio = Convert.ToInt32(_mealTicketCurrency.meFolios);

          // Creamos el Meal Ticket con el guestID
          _mealTicketCurrency.meD = dtpDate.Value.Value.Date;
          _mealTicketCurrency.megu = frmMealTickets._pguId;
          _mealTicketCurrency.meQty = frmMealTickets._pQty;
          _mealTicketCurrency.meType = _mealType.myID;
          _mealTicketCurrency.meAdults = _meAdults;
          _mealTicketCurrency.meMinors = _meMinors;
          _mealTicketCurrency.meFolios = folio + "";
          _mealTicketCurrency.meTAdults = Convert.ToDecimal(_meTAdultsString);
          _mealTicketCurrency.meTMinors = Convert.ToDecimal(_meTMinorsString);
          _mealTicketCurrency.meComments = txtComments.Text;
          _mealTicketCurrency.mesr = App.User.SalesRoom.srID;
          _mealTicketCurrency.meCanc = chkCancel.IsChecked.Value;
          _mealTicketCurrency.mera = _rateType.raID;
          _mealTicketCurrency.mepe = cboCollaborator.IsVisible ? _personnel.peID : null;
          _mealTicketCurrency.mePrinted = chkPrinted.IsChecked.Value;
          _mealTicketCurrency.meag = cboAgency.IsVisible ? _agency.agID : null;
          _mealTicketCurrency.merep = txtRepresentative.IsVisible ? txtRepresentative.Text : null;
          _mealTicketCurrency.meAuthorizedBy = App.User.User.peID;

          //Actualizamos el folio!
          BRMealTicketFolios.UpdateMealTicketFolio(App.User.SalesRoom.srID, _mealType.myID, _rateType.raID, _mealTicketCurrency.meFolios);

          // Insertamos el nuevo Meal Ticket con el folio asignado
          BRMealTickets.UpdateMealTicket(_mealTicketCurrency);
        }
        // Row New with GuestID != 0
        if (modeOpen == EnumModeOpen.PreviewEdit && frmMealTickets._pguId != 0)
        {
          // Obtenemos el folio a asignar
          int folioNew = 1 + BRMealTicketFolios.GetMaxMealTicketFolio(App.User.SalesRoom.srID, _mealType.myID, 1);

          MealTicket _newMealticket = CreateMealTicket(_rateType, _mealType, _personnel, _agency, _meAdults, _meMinors, _meTAdultsString, _meTMinorsString, folioNew);

          //Actualizamos el folio!
          BRMealTicketFolios.UpdateMealTicketFolio(App.User.SalesRoom.srID, _mealType.myID, 1, folioNew + "");

          //Guardamos el Meal Ticket Creado
          BRMealTickets.InsertNewMealTicket(_newMealticket);
        }
        // Edition Row with GuestID != 0
        else if (modeOpen == EnumModeOpen.Preview && frmMealTickets._pguId != 0)
        {
          int folio = Convert.ToInt32(_mealTicketCurrency.meFolios);

          // Creamos el Meal Ticket con el guestID
          _mealTicketCurrency.meD = dtpDate.Value.Value.Date;
          _mealTicketCurrency.megu = frmMealTickets._pguId;
          _mealTicketCurrency.meQty = frmMealTickets._pQty;
          _mealTicketCurrency.meType = _mealType.myID;
          _mealTicketCurrency.meAdults = _meAdults;
          _mealTicketCurrency.meMinors = _meMinors;
          _mealTicketCurrency.meFolios = folio + "";
          _mealTicketCurrency.meTAdults = Convert.ToDecimal(_meTAdultsString);
          _mealTicketCurrency.meTMinors = Convert.ToDecimal(_meTMinorsString);
          _mealTicketCurrency.meComments = txtComments.Text;
          _mealTicketCurrency.mesr = App.User.SalesRoom.srID;
          _mealTicketCurrency.meCanc = chkCancel.IsChecked.Value;
          _mealTicketCurrency.mera = 1;
          _mealTicketCurrency.mepe = cboCollaborator.IsVisible ? _personnel.peID : null;
          _mealTicketCurrency.mePrinted = chkPrinted.IsChecked.Value;
          _mealTicketCurrency.meag = cboAgency.IsVisible ? _agency.agID : null;
          _mealTicketCurrency.merep = txtRepresentative.IsVisible ? txtRepresentative.Text : null;
          _mealTicketCurrency.meAuthorizedBy = App.User.User.peID;

          //Actualizamos el folio!
          BRMealTicketFolios.UpdateMealTicketFolio(App.User.SalesRoom.srID, _mealType.myID, 1, _mealTicketCurrency.meFolios);

          // Insertamos el nuevo Meal Ticket con el folio asignado
          BRMealTickets.UpdateMealTicket(_mealTicketCurrency);
        }
        //Actualizamos el campo guMealTicket del Guest
        BRGuests.UpdateFieldguMealTicket(true, frmMealTickets._pguId);

        Close();
      }
    }

    private MealTicket CreateMealTicket(RateType _rateType, MealTicketType _mealType, PersonnelShort _personnel, AgencyShort _agency, int _meAdults, int _meMinors, string _meTAdultsString, string _meTMinorsString, int folioNew)
    {
      return new MealTicket
      {
        meD = dtpDate.Value.Value.Date,
        megu = frmMealTickets._pguId,
        meQty = frmMealTickets._pQty,
        meType = _mealType.myID,
        meAdults = _meAdults,
        meMinors = _meMinors,
        meFolios = folioNew + "",
        meTAdults = Convert.ToDecimal(_meTAdultsString),
        meTMinors = Convert.ToDecimal(_meTMinorsString),
        meComments = txtComments.Text,
        mesr = App.User.SalesRoom.srID,
        meCanc = chkCancel.IsChecked.Value,
        mera = cboRateType.IsVisible ? _rateType.raID : 1,
        mepe = cboCollaborator.IsVisible ? _personnel.peID : null,
        mePrinted = chkPrinted.IsChecked.Value,
        meag = cboAgency.IsVisible ? _agency.agID : null,
        merep = txtRepresentative.IsVisible ? txtRepresentative.Text : null,
        meAuthorizedBy = App.User.User.peID
      };
    }
    #endregion
  }
}

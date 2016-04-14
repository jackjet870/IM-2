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
          DataContext = _mealTicketCurrency;
          dtpDate.Value = frmHost._dtpServerDate.Date;
          break;
        #endregion
        #region EnumModeOpen.Edit
        case EnumModeOpen.Edit:
          DataContext = _mealTicketCurrency;
          dtpDate.Value = _mealTicketCurrency.meD.Date;
          break;
        #endregion
        #region EnumModeOpen.Preview
        case EnumModeOpen.Preview:
          DataContext = _mealTicketCurrency;
          dtpDate.Value = (_mealTicketCurrency.meD.Date <= new DateTime(0001, 01, 1)) ? frmHost._dtpServerDate : _mealTicketCurrency.meD.Date;
          lblRateType.Visibility = Visibility.Hidden;
          cboRateType.Visibility = Visibility.Hidden;
          lblAgency.Visibility = Visibility.Hidden;
          cboAgency.Visibility = Visibility.Hidden;
          lblCollaborator.Visibility = Visibility.Hidden;
          cboCollaborator.Visibility = Visibility.Hidden;
          lblRepresentative.Visibility = Visibility.Hidden;
          txtRepresentative.Visibility = Visibility.Hidden;
          break;
          #endregion
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

      //if (controlRateType == false)
      //{
        if (_rateType != null) // Se verifica que el SelectedItem no sea null
        {
          if (_rateType.raID != 4 && modeOpen != EnumModeOpen.Preview) // Si es diferente de tipo External!
          {
            controlVisibility(Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
          }
          else if (modeOpen != EnumModeOpen.Preview) // Es external
          {
            controlVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Visible);
          }
        }
        else if (modeOpen != EnumModeOpen.Preview)
        {
          controlVisibility(Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
        }


        // Se verifica que no sean un nuevo MealTicket
        if (_rateType != null && txtAdults.Text != "" && txtMinors.Text != "" && _meQty >= 1)
        {
          txtTAdults.Text = string.Format("{0:$0.00}", CalculateAdult(((_rateType == null) ? 0 : _rateType.raID), ((_meQty >= 0) ? _meQty : 0), ((txtAdults.Text != "") ? Convert.ToInt32(txtAdults.Text) : 0)));
        }
        //controlRateType = true;
      //}
      //else
      //  controlRateType = false;
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

    private void validateFields()
    {
      if (frmMealTickets._pguId == 0 ) // Row New
      {
        RateType _rateType = (RateType)cboRateType.SelectedItem; //mera
        MealTicketType _mealType = (MealTicketType)cboType.SelectedItem; //
        PersonnelShort _personnel = (PersonnelShort)cboCollaborator.SelectedItem;
        AgencyShort _agency = (AgencyShort)cboAgency.SelectedItem;

        if (_rateType == null)
        {
          UIHelper.ShowMessage("Select an option of rate type, please", MessageBoxImage.Information);
          return;
        }
        else if (_rateType.raID >= 2 && _rateType.raID < 4 && _personnel == null)
        {
          UIHelper.ShowMessage("Select a collaborator, please", MessageBoxImage.Information);
          return;
        }
        else if (_rateType.raID == 4 && (_agency == null || _personnel == null))
        {
          UIHelper.ShowMessage("Select an agency and write the representative name in the field for External option.", MessageBoxImage.Information);
          return;
        }
        else if (_mealType ==  null)
        {
          UIHelper.ShowMessage("Select an option of meal type, please", MessageBoxImage.Information);
          return;
        }

        // Obtenemos el folio a asignar
        int folioNew = 1 + BRMealTicketFolios.GetMaxMealTicketFolio(App.User.SalesRoom.srID, _mealType.myID, _rateType.raID);  // SetFolios(frmMealTickets._pQty, _mealType.myID, _rateType.raID);

        int _meAdults = Convert.ToInt32((txtAdults.Text == "") ? "0" : txtAdults.Text.Substring(1, txtAdults.Text.Length - 1));
        int _meMinors = Convert.ToInt32((txtAdults.Text == "") ? "0" : txtMinors.Text.Substring(1, txtMinors.Text.Length - 1));

        MealTicket _newMealticket = new MealTicket
        {
          meD = dtpDate.Value.Value.Date,
          megu = frmMealTickets._pguId,
          meQty = frmMealTickets._pQty,
          meType = _mealType.myID,
          meAdults = _meAdults,
          meMinors = _meMinors

        };


      }
      else // Edicion, Agregar a un guestID con MealTicket creado!
      {

      }
    }

    private int SetFolios(int pmeQty, string pmeType, int pmera)
    {
      return 0;
    }

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

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      validateFields();
    }
  }
}

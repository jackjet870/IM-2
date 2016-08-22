using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
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
    public EnumMode _modeOpen;
    private EnumOpenBy _openBy;
    private int _meQty = 1;
    public MealTicket _mealTicketCurrency = new MealTicket();

    CollectionViewSource _dsRateType;
    CollectionViewSource _dsPersonnel;
    CollectionViewSource _dsAgency;
    CollectionViewSource _dsMealTicketType;
    #endregion

    #region Contructor
    /// <summary>
    /// 
    /// </summary>
    /// <param name="openBy"></param>
    /// <history>
    /// [vipacheco] 15/Agosto/2016 Modified -> Se agrego el enumerado como parametro.
    /// </history>
    public frmMealTicketsDetail(EnumOpenBy openBy)
    {
      InitializeComponent();
      _openBy = openBy;
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// 
    /// </summary>
    /// <history>
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// [edgrodriguez] 21/05/2016 Modified. El método GetRateTypes se volvió asincrónico.
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _dsRateType = ((CollectionViewSource)(this.FindResource("dsRateType")));
      _dsPersonnel = ((CollectionViewSource)(this.FindResource("dsPersonnel")));
      _dsAgency = ((CollectionViewSource)(this.FindResource("dsAgency")));
      _dsMealTicketType = ((CollectionViewSource)(this.FindResource("dsMealTicketType")));

      //  Obtenemos los tipos de tarifa
      _dsRateType.Source = frmHost._lstRateType;
      // Obtenemos los colaboradores
      _dsPersonnel.Source = frmHost._lstPersonnel;
      // Obtenemos las agencias
      _dsAgency.Source = frmHost._lstAgencies;
      // Obtenemos los tipos de cupones de comida.
      _dsMealTicketType.Source = frmHost._lstMealTicketType;

      // Verificamos si es usuario tiene la opcion de Reimpresion
      chkPrinted.IsEnabled = App.User.HasPermission(EnumPermission.MealTicketReprint, EnumPermisionLevel.Standard) ? true : false;

      #region switch
      switch (_openBy)
      {
        case EnumOpenBy.Checkbox:
          if (_modeOpen == EnumMode.Add)
          {
            stkHead.Visibility = Visibility.Collapsed;
            Height = 149; Width = 593.59;
            dtpDate.Value = frmHost.dtpServerDate.Date;
            chkPrinted.IsEnabled = chkCancel.IsEnabled = false;
          }
          else if (_modeOpen == EnumMode.Edit)
          {
            stkHead.Visibility = Visibility.Collapsed;
            Height = 149; Width = 593.59;
            DataContext = _mealTicketCurrency;
            dtpDate.Value = (_mealTicketCurrency.meD.Date <= new DateTime(0001, 01, 1)) ? frmHost.dtpServerDate : _mealTicketCurrency.meD.Date;
          }
          break;
        case EnumOpenBy.Button:
          Height = 200; Width = 593.59;
          if (_modeOpen == EnumMode.Add)
          {
            dtpDate.Value = frmHost.dtpServerDate.Date;
            chkPrinted.IsEnabled = chkCancel.IsEnabled = false;
            // Verificamos si es un custom
            if (_mealTicketCurrency.mera == 1)
            {
              stkHead.Visibility = Visibility.Collapsed;
              Height = 149; Width = 593.59;
            }
          }
          else if (_modeOpen == EnumMode.Edit)
          {
            DataContext = _mealTicketCurrency;

            // Verificamos si es un custom
            if (_mealTicketCurrency.mera == 1)
            {
              stkHead.Visibility = Visibility.Collapsed;
              Height = 149; Width = 593.59;
            }
            cboRateType_SelectionChanged(null, null);
            dtpDate.Value = _mealTicketCurrency.meD.Date;
          }
          break;
      }
      #endregion
    }
    #endregion

    #region cboRateType_SelectionChanged
    /// <summary>
    /// Funcion para inicializar los controles
    /// </summary>
    /// <history>
    /// [vipacheco] 23/03/2016 Created
    /// </history>
    private void cboRateType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      RateType _rateType = cboRateType.SelectedItem as RateType;

      if (_openBy == EnumOpenBy.Button)
      {
        if (_rateType != null) // Se verifica que el SelectedItem no sea null
        {
          if (_rateType.raID >= 2 && _rateType.raID < 4 ) // Si es diferente de tipo External!
          {
            controlVisibility(Visibility.Collapsed, Visibility.Visible, Visibility.Collapsed);
          }
          else if (_rateType.raID == 4) // Es external
          {
            controlVisibility(Visibility.Visible, Visibility.Collapsed, Visibility.Visible);
          }
        }
        else if (_modeOpen != EnumMode.ReadOnly)
        {
          controlVisibility(Visibility.Collapsed, Visibility.Visible, Visibility.Collapsed);
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
      stkAgency.Visibility = agency;
      stkCollaborator.Visibility = collaborator;
      stkRepresentative.Visibility = representative;
    }
    #endregion

    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close();
    }
    #endregion

    #region txtEvaluation_PreviewTextInput
    /// <summary>
    /// Funcion para evaluar que el texto introducido sea solamente numerico
    /// </summary>
    /// <history>
    /// [vipacheco] 23/03/2016
    /// </history>
    private void txtEvaluation_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyDecimals(e.Text, sender as TextBox);
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
      MealTicketType mealTicket = cboType.SelectedItem as MealTicketType;
      double Amt = 0;

      if (mealTicket != null)
      {
        if (intRatetype == 1)
        {
          Amt = intQt * intAdults * Convert.ToDouble(mealTicket.myPriceA);
        }
        else if (intRatetype == 2)
        {
          Amt = intQt * intAdults * Convert.ToDouble(mealTicket.myCollaboratorWithCost);
        }
        else if (intRatetype >= 3)
        {
          Amt = intQt * intAdults * Convert.ToDouble(mealTicket.myCollaboratorWithoutCost);
        }
      }

      return Amt;
    }
    #endregion

    #region validateGeneral
    /// <summary>
    /// Valida los campos obligatorios segun sea requerido
    /// </summary>
    /// <returns> False - Algun Campo Vacio | True - Campos llenados correctamente </returns>
    /// <history>
    /// [vipacheco] 01/04/2016 Created
    /// [vipahceco] 15/Agosto/2016 Modified -> Se valida que el ticket tenga pax asociados.
    /// </history>
    private bool ValidateGeneral()
    {
      MealTicketType ticketCurrent = cboType.SelectedItem as MealTicketType;

      if (frmMealTickets._guestID == 0)
      {
        RateType rateType = cboRateType.SelectedItem as RateType;
        PersonnelShort personnel = cboCollaborator.SelectedItem as PersonnelShort;
        AgencyShort agency = cboAgency.SelectedItem as AgencyShort;

        if (rateType == null)
        {
          UIHelper.ShowMessage("Select an option of rate type, please", MessageBoxImage.Information);
          return false;
        }
        else if (rateType.raID >= 2 && rateType.raID < 4 && personnel == null)
        {
          UIHelper.ShowMessage("Select a collaborator, please", MessageBoxImage.Information);
          return false;
        }
        else if (rateType.raID == 4 && (agency == null || txtRepresentative.Text == ""))
        {
          UIHelper.ShowMessage("Select an agency and write the representative name in the field for External option.", MessageBoxImage.Information);
          return false;
        }
        else if (ticketCurrent == null)
        {
          UIHelper.ShowMessage("Select an option of meal type, please", MessageBoxImage.Information);
          return false;
        }
        // Verificamos el Pax
        int adults, minors = 0;
        if (string.IsNullOrEmpty(txtAdults.Text) && string.IsNullOrEmpty(txtMinors.Text))
        {
          UIHelper.ShowMessage("Set the Pax information, adults or minors", MessageBoxImage.Information);
          return false;
        }
        else if (int.TryParse(txtAdults.Text, out adults) && int.TryParse(txtMinors.Text, out minors))
        {
          // Verificamos que no sean ambos 0
          if (adults == 0 && minors == 0 )
          {
            UIHelper.ShowMessage("Set the Pax information, adults or minors", MessageBoxImage.Information);
            return false;
          }
        }

      }
      else
      {
        if (ticketCurrent == null)
        {
          UIHelper.ShowMessage("Select an option of meal type, please", MessageBoxImage.Information);
          return false;
        }
      }

      return true;
    } 
    #endregion

    #region cboType_SelectionChanged
    /// <summary>
    /// Realiza los calculos de acuerdo a los parametros introducidos
    /// </summary>
    /// <history>
    /// [vipacheco] 31/Marzo/2016 Created
    /// </history>
    private void cboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      RateType _rateType = cboRateType.SelectedItem as RateType;
      MealTicketType _mealTicketType = cboType.SelectedItem as MealTicketType;

      if (_mealTicketType != null)
      {
        if (!_mealTicketType.myWPax)
        {
          txtAdults.Text = $"{1}";
          txtMinors.Text = $"{0}";
          txtAdults.IsEnabled = txtMinors.IsEnabled = false;
          txtTAdults.Text = string.Format("{0:$0.00}", CalculateAdult(((_rateType == null) ? 1 : _rateType.raID), ((_meQty >= 0) ? _meQty : 0), ((txtAdults.Text != "") ? Convert.ToInt32(txtAdults.Text) : 0)));
          decimal _minors = (_meQty * ((txtMinors.Text != "") ? Convert.ToInt32(txtMinors.Text) : 0) * ((_mealTicketType != null) ? _mealTicketType.myPriceM : 0));
          txtTMinors.Text = string.Format("{0:$0.00}", Convert.ToDouble(_minors));
        }
        else
        {
          txtAdults.IsEnabled = txtMinors.IsEnabled = true;
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
    /// <summary>
    /// Calcula el total de adulto de acuerdo a los datos ingresados.
    /// </summary>
    /// <history>
    /// [vipacheco] 31/Marzo/2016 Created
    /// </history>
    private void txtAdults_LostFocus(object sender, RoutedEventArgs e)
    {
      CalculateTotalAdults();
    }
    #endregion

    #region CalculateTotalAdults
    /// <summary>
    /// Calcula el total de adulto
    /// </summary>
    /// <history>
    /// [vipacheco] 19/Agosto/2016 Created
    /// </history>
    private void CalculateTotalAdults()
    {
      RateType _rateType = cboRateType.SelectedItem as RateType;
      MealTicketType _mealTicketType = cboType.SelectedItem as MealTicketType;

      if (_mealTicketType != null)
      {
        if (!_mealTicketType.myWPax)
        {
          txtTAdults.Text = string.Format("{0:$0.00}", CalculateAdult(((_rateType == null) ? 1 : _rateType.raID), ((_meQty >= 0) ? _meQty : 0), ((txtAdults.Text != "") ? Convert.ToInt32(txtAdults.Text) : 0)));
        }
        else
        {
          txtTAdults.Text = string.Format("{0:$0.00}", CalculateAdult(((_rateType == null) ? 1 : _rateType.raID), ((_meQty >= 0) ? _meQty : 0), ((txtAdults.Text != "") ? Convert.ToInt32(txtAdults.Text) : 0)));
        }
      }
    } 
    #endregion

    #region txtMinors_LostFocus
    /// <summary>
    /// Calcula los totales de los minors segun los datos ingresados.
    /// </summary>
    /// <history>
    /// [vipacheco] 31/Marzo/2016 Created
    /// </history>
    private void txtMinors_LostFocus(object sender, RoutedEventArgs e)
    {
      CalculateTotalMinors();
    }
    #endregion

    #region CalculateTotalMinors
    /// <summary>
    /// Calcula los totales de los minors
    /// </summary>
    /// <history>
    /// [vipacheco] 19/Agosto/2016 Created
    /// </history>
    private void CalculateTotalMinors()
    {
      RateType _rateType = cboRateType.SelectedItem as RateType;
      MealTicketType _mealTicketType = cboType.SelectedItem as MealTicketType;

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
    /// <history>
    /// [vipacheco] 23/03/2016 Created
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateGeneral())
      {
        RateType _rateType = cboRateType.SelectedItem as RateType; 
        MealTicketType _mealType = cboType.SelectedItem as MealTicketType; 
        PersonnelShort _personnel = cboCollaborator.SelectedItem as PersonnelShort;
        AgencyShort _agency = cboAgency.SelectedItem as AgencyShort;

        int _meAdults = Convert.ToInt32((txtAdults.Text == "") ? "0" : txtAdults.Text);
        int _meMinors = Convert.ToInt32((txtMinors.Text == "") ? "0" : txtMinors.Text);
        string _meTAdultsString = txtTAdults.Text.TrimStart('$');
        string _meTMinorsString = txtTMinors.Text.TrimStart('$');

        // Agrega un nuevo Meal Ticket
        if (_modeOpen == EnumMode.Add)
        {
          // Obtenemos el folio a asignar
          int folioNew = 1 + BRMealTicketFolios.GetMaxMealTicketFolio(App.User.SalesRoom.srID, _mealType.myID, _rateType == null ? 1 : _rateType.raID);

          _mealTicketCurrency = CreateMealTicket(_rateType, _mealType, _personnel, _agency, _meAdults, _meMinors, _meTAdultsString, _meTMinorsString, folioNew);

          //Actualizamos el folio!
          BRMealTicketFolios.UpdateMealTicketFolio(App.User.SalesRoom.srID, _mealType.myID, _rateType == null ? 1 : _rateType.raID, $"{folioNew}");

          //Guardamos el Meal Ticket Creado
          BRMealTickets.InsertNewMealTicket(_mealTicketCurrency);
        }
        // Modo edicion
        else
        {
          int folio = Convert.ToInt32(_mealTicketCurrency.meFolios);

          // Creamos el Meal Ticket con el guestID
          _mealTicketCurrency.meD = dtpDate.Value.Value.Date;
          _mealTicketCurrency.megu = _mealTicketCurrency.megu != 0 ? _mealTicketCurrency.megu : frmMealTickets._guestID;
          _mealTicketCurrency.meQty = frmMealTickets._Qty;
          _mealTicketCurrency.meType = _mealType.myID;
          _mealTicketCurrency.meAdults = _meAdults;
          _mealTicketCurrency.meMinors = _meMinors;
          _mealTicketCurrency.meFolios = $"{folio}";
          _mealTicketCurrency.meTAdults = Convert.ToDecimal(_meTAdultsString);
          _mealTicketCurrency.meTMinors = Convert.ToDecimal(_meTMinorsString);
          _mealTicketCurrency.meComments = txtComments.Text;
          _mealTicketCurrency.mesr = App.User.SalesRoom.srID;
          _mealTicketCurrency.meCanc = chkCancel.IsChecked.Value;
          _mealTicketCurrency.mera = frmMealTickets._guestID > 0 ? 1 : _rateType.raID;
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
        //Actualizamos el campo guMealTicket del Guest
        BRGuests.UpdateFieldguMealTicket(true, frmMealTickets._guestID);

        DialogResult = true;
        Close();
      }
    }

    /// <summary>
    /// Crea un nuevo mealticket
    /// </summary>
    /// <param name="_rateType"></param>
    /// <param name="_mealType"></param>
    /// <param name="_personnel"></param>
    /// <param name="_agency"></param>
    /// <param name="_meAdults"></param>
    /// <param name="_meMinors"></param>
    /// <param name="_meTAdultsString"></param>
    /// <param name="_meTMinorsString"></param>
    /// <param name="folioNew"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 25/Abril/2016 Created
    /// </history>
    private MealTicket CreateMealTicket(RateType _rateType, MealTicketType _mealType, PersonnelShort _personnel, AgencyShort _agency, int _meAdults, int _meMinors, string _meTAdultsString, string _meTMinorsString, int folioNew)
    {
      return new MealTicket
      {
        meD = dtpDate.Value.Value.Date,
        megu = frmMealTickets._guestID,
        meQty = frmMealTickets._Qty,
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
    #region txtAdults_PreviewKeyDown
    /// <summary>
    /// Calcula el total de adults cuando se presiona Enter
    /// </summary>
    /// <history>
    /// [vipacheco] 19/Agosto/2016 Created
    /// </history>
    private void txtAdults_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        CalculateTotalAdults();
      }
    }
    #endregion

    #region txtMinors_PreviewKeyDown
    /// <summary>
    /// calcula el total de minors cuando se presione enter
    /// </summary>
    /// <history>
    /// [vipacheco] 19/Agosto/2016 Created
    /// </history>
    private void txtMinors_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        CalculateTotalMinors();
      }
    } 
    #endregion
  }
}

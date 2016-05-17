using IM.BusinessRules.BR;
using IM.Host.Enums;
using IM.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model.Helpers;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmMealTickets.xaml
  /// </summary>
  /// <history>
  /// [vipacheco] 03/17/2016 Created
  /// </history>
  public partial class frmMealTickets : Window
  {

    #region VARIABLES
    public static int _pguId = 0;
    public static int _pQty = 1;
    public static int _rateTypeChild = -1;
    public bool _reanOnly = true;
    public EnumModeOpen modeOpen;
    public GuestPremanifestHost _guestPremanifestHost = new GuestPremanifestHost();
    CollectionViewSource _dsRateType;
    CollectionViewSource _dsMealTicket;
    CollectionViewSource _dsPersonnel;
    CollectionViewSource _dsAgency;
    CollectionViewSource _dsMealTicketType;
    #endregion

    #region Contructor
    public frmMealTickets(int pguID = 0)
    {
      _pguId = pguID;

      // Se verifica si tiene permisos de edición!
      InitializeComponent();

      dtpFrom.Value = frmHost._dtpServerDate;
      dtpTo.Value = frmHost._dtpServerDate;
    }
    #endregion

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _dsRateType = ((CollectionViewSource)(this.FindResource("dsRateType")));
      _dsMealTicket = ((CollectionViewSource)(this.FindResource("dsMealTicket")));
      _dsPersonnel = ((CollectionViewSource)(this.FindResource("dsPersonnel")));
      _dsAgency = ((CollectionViewSource)(this.FindResource("dsAgency")));
      _dsMealTicketType = ((CollectionViewSource)(this.FindResource("dsMealTicketType")));

      //  Obtenemos los tipos de tarifa
      _dsRateType.Source = BRRateTypes.GetRateTypes(new RateType { raID=1}, 1, true, true);

      // Obtenemos los colaboradores
      _dsPersonnel.Source = BRPersonnel.GetPersonnel("ALL", "ALL", "ALL", 1);

      // Obtenemos las agencias
      _dsAgency.Source = BRAgencies.GetAgencies(1);

      // Obtenemos los tipos de cupones de comida.
      _dsMealTicketType.Source = BRMealTicketTypes.GetMealTicketType();

      // Verificamos si es usuario estandar o de solo lectura!
      switch (modeOpen)
      {
        case EnumModeOpen.Add:
        case EnumModeOpen.Edit:
          ControlsVisibility(Visibility.Visible, Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
          break;
        case EnumModeOpen.Search:
          ControlsVisibility(Visibility.Hidden, Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
          break;
        case EnumModeOpen.Preview:
          // Se muestran y ocultan los controles necesarios
          ControlsVisibility(Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Hidden);

          // Se busca la informacion con guestID proporcionado
          _dsMealTicket.Source = BRMealTickets.GetMealTickets(_pguId);

          AdjustsControlsAndColumns();
          break;
        case EnumModeOpen.PreviewEdit:
          ControlsVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Visible, Visibility.Hidden);

          // Se busca la informacion con guestID proporcionado
          _dsMealTicket.Source = BRMealTickets.GetMealTickets(_pguId);

          AdjustsControlsAndColumns();
          break;
      }
    } 
    #endregion

    #region btnClose_Click
    /// <summary>
    /// Función para cerrar la ventana Meal Tickets
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 18/03/2016 Created
    /// </history>
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      Close();
    } 
    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Función para la busqueda de un Meal Tickets de acuerdo a los datos ingresados!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 18/03/2016 Created
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      int _mealTicket = 0;
      string _folio = "";
      int _rateType = 0;
      DateTime? _dtpFrom = null;
      DateTime? _dtpTo = null;

      if (txtMealTicket.Text != "") // Obtenemos el ID de Meal Ticket
      {
        _mealTicket = Convert.ToInt32(txtMealTicket.Text);
      }
      if (txtFolio.Text != "") // Obtenemos el folio ID si se ingreso
      {
        _folio = txtFolio.Text;
      }
      if (cboRateType.SelectedItem != null) // Obtenemos el Rate Type seleccionado
      {
        RateType _valueCheck = (RateType)cboRateType.SelectedItem;
        _rateType = _valueCheck.raID;
      }
      if (dtpFrom.Value.Value.Date != null) // Obtenemos la fecha FROM ingresada
      {
        _dtpFrom = dtpFrom.Value.Value.Date;
      }
      if (dtpTo.Value.Value.Date != null) // Obtenemos la fecha TO ingresada
      {
        _dtpTo = dtpTo.Value.Value.Date;
      }

      // Realizamos la busqueda con los parametros ingresados!
      _dsMealTicket.Source = BRMealTickets.GetMealTickets(_mealTicket, _folio, _rateType, _dtpFrom, _dtpTo);
    } 
    #endregion

    #region btnAdd_Click
    /// <summary>
    /// Función para agregar un nuevo Meal Ticket
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 22/03/2016 Created
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmMealTicketsDetail _frmMealTicketsDetail = new frmMealTicketsDetail();
      _frmMealTicketsDetail.ShowInTaskbar = false;
      _frmMealTicketsDetail.Owner = this;
      _frmMealTicketsDetail.modeOpen = (modeOpen == EnumModeOpen.Edit) ? EnumModeOpen.Add : EnumModeOpen.PreviewEdit;
      _frmMealTicketsDetail.Title += "ADD";

      _frmMealTicketsDetail.ShowDialog();

      if (modeOpen == EnumModeOpen.PreviewEdit)
      {
        _dsMealTicket.Source = BRMealTickets.GetMealTickets(_pguId);
      }
      else
        btnSearch_Click(null, null);
    }
    #endregion

    #region Cell_DoubleClick
    /// <summary>
    /// Función encargada de verificar si cuenta con los permisos de edicion!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 23/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      // Se verifica que tenga permisos de editar
      if (modeOpen != EnumModeOpen.Search && modeOpen != EnumModeOpen.Preview)
      {
        MealTicket mealTicket = (MealTicket)dgMealTicket.SelectedItem;

        //_pguId = mealTicket.megu;
        frmMealTicketsDetail _frmMealTicketsDetail = new frmMealTicketsDetail();
        ObjectHelper.CopyProperties(_frmMealTicketsDetail._mealTicketCurrency, mealTicket);

        _frmMealTicketsDetail.ShowInTaskbar = false;
        _frmMealTicketsDetail.modeOpen = (modeOpen == EnumModeOpen.Edit) ? EnumModeOpen.Edit : EnumModeOpen.Preview;
        _frmMealTicketsDetail.Owner = this;
        _frmMealTicketsDetail.Title += "ID " + mealTicket.meID;

        if (_frmMealTicketsDetail.ShowDialog() == true)
        {
          ObjectHelper.CopyProperties(mealTicket, _frmMealTicketsDetail._mealTicketCurrency);
        }

        if (modeOpen == EnumModeOpen.PreviewEdit)
        {
          _dsMealTicket.Source = BRMealTickets.GetMealTickets(_pguId);
        }

      }
    } 
    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            Cell_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }

      e.Handled = blnHandled;
    }

    #endregion

    #region ControlsVisibility
    /// <summary>
    /// Función encargada de ocultar o mostrar parametros segun sea necesario
    /// </summary>
    /// <param name="controlAdd"> Control de tipo Boton para Agregar </param>
    /// <param name="selectionCriteria"> Group Box Selection Criteria </param>
    /// <param name="controlPrinted"> Control de tipo Boton para Imprimir </param>
    /// <param name="checkTicket"> Control de tipo checkBox para ticket </param>
    /// <history>
    /// [vipacheco] 22/03/2016 Created
    /// </history>
    private void ControlsVisibility(Visibility controlAdd, Visibility selectionCriteria, Visibility controlPrinted, Visibility checkTicket)
    {
      btnAdd.Visibility = controlAdd;
      btnPrint.Visibility = controlPrinted;
      grbSelectionCriteria.Visibility = selectionCriteria;
      chkTicket.Visibility = checkTicket;
    }
    #endregion

    #region cboRateType_SelectionChanged
    /// <summary>
    /// Modifica el grid segun la seleccion del RateType
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 23/03/2016 Created
    /// </history>
    private void cboRateType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      RateType _rateType = (RateType)cboRateType.SelectedItem;

      if (_rateType != null) // Se verifica que el SelectedItem no sea null
      {
        _dsMealTicket.Source = null;

        if (_rateType.raID != 4 && modeOpen != EnumModeOpen.Preview) // Si es diferente de tipo External!
        {
          controlColumnVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
        }
        else if (modeOpen != EnumModeOpen.Preview) // Es external
        {
          controlColumnVisibility(Visibility.Hidden, Visibility.Visible, Visibility.Visible);
        }
      }
    } 
    #endregion

    #region controlColumnVisibility
    /// <summary>
    /// Oculta las columnas del grid segun sea necesario
    /// </summary>
    /// <param name="vCollaborator"></param>
    /// <param name="vRepresentative"></param>
    /// <param name="vAgency"></param>
    /// <history>
    /// [vipacheco] 01/04/2016 Created
    /// </history>
    private void controlColumnVisibility(Visibility vCollaborator, Visibility vRepresentative, Visibility vAgency)
    {
      mepeColumn.Visibility = vCollaborator;
      meagColumn.Visibility = vAgency;
      merepColumn.Visibility = vRepresentative;
    }
    #endregion

    #region AdjustsControlsAndColumns
    /// <summary>
    /// Funcion encargada de alinear el margen y ocultar columnas cuando el modeOpen es Preview y PreviewEdit
    /// </summary>
    /// <history>
    /// [vipacheco] 02/04/2016 Created
    /// </history>
    private void AdjustsControlsAndColumns()
    {
      Thickness _margin = dgMealTicket.Margin;
      _margin.Top = 18;
      dgMealTicket.Margin = _margin;
      // Se ocultan la primeras 4 columnas!
      meraColumn.Visibility = Visibility.Hidden;
      mepeColumn.Visibility = Visibility.Hidden;
      meagColumn.Visibility = Visibility.Hidden;
      merepColumn.Visibility = Visibility.Hidden;
    } 
    #endregion

  }
}

using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Enums;
using IM.Model;
using IM.Model.Enums;
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
      _dsRateType.Source = BRRateTypes.GetRateType(1, true, true, true);

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

          if (_pguId != 0)
          {
            List<MealTicket> _valueEdit = BRMealTickets.GetMealTickets(_pguId);

            if (_valueEdit.Count > 0)
            {
              _dsMealTicket.Source = _valueEdit;
            }
            else
            {
              //MealTicket mealTicket = (MealTicket)dgMealTicket.SelectedItem;
              frmMealTicketsDetail _frmMealTicketsDetail = new frmMealTicketsDetail();
              _frmMealTicketsDetail.ShowInTaskbar = false;
              _frmMealTicketsDetail.Owner = this;
              _frmMealTicketsDetail.modeOpen = EnumModeOpen.Preview;
              _frmMealTicketsDetail.Title += "ADD";
              _frmMealTicketsDetail.ShowDialog();
            }
          }


          break;
        case EnumModeOpen.Search:
          ControlsVisibility(Visibility.Hidden, Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
          break;
        case EnumModeOpen.Preview:
          ControlsVisibility(Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Hidden);

          if (_pguId != 0)
          {
            List<MealTicket> _valuePreview = BRMealTickets.GetMealTickets(_pguId);

            if (_valuePreview.Count > 0 )
            {
              _dsMealTicket.Source = BRMealTickets.GetMealTickets(_pguId);

              Thickness margin = dgMealTicket.Margin;
              margin.Top = 18;
              dgMealTicket.Margin = margin;
              // Se ocultan la primeras 4 columnas!
              meraColumn.Visibility = Visibility.Hidden;
              mepeColumn.Visibility = Visibility.Hidden;
              meagColumn.Visibility = Visibility.Hidden;
              merepColumn.Visibility = Visibility.Hidden;
            }
            else
            {
              MealTicket mealTicket = (MealTicket)dgMealTicket.SelectedItem;
              frmMealTicketsDetail _frmMealTicketsDetail = new frmMealTicketsDetail();
              _frmMealTicketsDetail.ShowInTaskbar = false;
              _frmMealTicketsDetail.Owner = this;
              _frmMealTicketsDetail.modeOpen = EnumModeOpen.Preview;
              _frmMealTicketsDetail.Title += "ADD";
              _frmMealTicketsDetail.ShowDialog();
            }
          }
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
      MealTicket mealTicket = (MealTicket)dgMealTicket.SelectedItem;
      _pguId = (mealTicket == null) ? 0 : mealTicket.megu;
      frmMealTicketsDetail _frmMealTicketsDetail = new frmMealTicketsDetail();
      _frmMealTicketsDetail.ShowInTaskbar = false;
      _frmMealTicketsDetail.Owner = this;
      _frmMealTicketsDetail.modeOpen = EnumModeOpen.Add;
      _frmMealTicketsDetail.Title += "ADD";
      _frmMealTicketsDetail.ShowDialog();

    }
    #endregion

    #region Cell_DoubleClick
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      // Se verifica que tenga permisos de editar
      if (modeOpen != EnumModeOpen.Search && App.User.HasPermission(EnumPermission.MealTicket, EnumPermisionLevel.Standard))
      {
        MealTicket mealTicket = (MealTicket)dgMealTicket.SelectedItem;

        // si alguno de sus cupones de comida es de una fecha cerrada, impedimos modificar los datos
        SalesRoomCloseDates _closeSalesRoom = BRSalesRooms.GetSalesRoom(App.User.SalesRoom.srID);
        //With grd
        //    For i = .FixedRows To.Rows - 1
        //        If basCommon.IsClosed(.TextMatrix(i, .ColIndex("meD")), mdtmClose) Then
        //            OnlyReadMode
        //            Exit For
        //        End If
        //    Next
        //End With

        //if (App.User.SalesRoom.srID mealTicket.meD)
        if(!IsClosed(mealTicket.meD, _closeSalesRoom.srMealTicketsCloseD))
        {
          _pguId = mealTicket.megu;
          frmMealTicketsDetail _frmMealTicketsDetail = new frmMealTicketsDetail();
          ObjectHelper.CopyProperties(_frmMealTicketsDetail._mealTicketCurrency, mealTicket);

          _frmMealTicketsDetail.ShowInTaskbar = false;
          _frmMealTicketsDetail.modeOpen = modeOpen;
          _frmMealTicketsDetail.Owner = this;
          _frmMealTicketsDetail.Title += "ID " + mealTicket.meID;

          if (_frmMealTicketsDetail.ShowDialog() == true)
          {
            ObjectHelper.CopyProperties(mealTicket, _frmMealTicketsDetail._mealTicketCurrency);
          }
        }
      }
    } 
    #endregion

    private bool IsClosed(DateTime pdtmDate, DateTime pdtmClose)
    {
      bool blnClosed = false;
      DateTime _pdtmDate;

      if (DateTime.TryParse(pdtmDate+"", out _pdtmDate))
      {
        if (_pdtmDate <= pdtmClose)
        {
          blnClosed = true;
        }
      }
      return blnClosed;
    }

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

    private void cboRateType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      RateType _rateType = (RateType)cboRateType.SelectedItem;

      if (_rateType != null) // Se verifica que el SelectedItem no sea null
      {
        if (_rateType.raID != 4 && modeOpen != EnumModeOpen.Preview) // Si es diferente de tipo External!
        {
          controlVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
        }
        else if (modeOpen != EnumModeOpen.Preview) // Es external
        {
          controlVisibility(Visibility.Hidden, Visibility.Visible, Visibility.Visible);
        }
      }
    }

    private void controlVisibility(Visibility vCollaborator, Visibility vRepresentative, Visibility vAgency)
    {
      mepeColumn.Visibility = vCollaborator;
      meagColumn.Visibility = vAgency;
      merepColumn.Visibility = vRepresentative;
    }
  }
}

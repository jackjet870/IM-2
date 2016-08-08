using IM.BusinessRules.BR;
using IM.Host.Enums;
using IM.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model.Helpers;
using System.Linq;
using IM.Base.Helpers;
using System.Collections.Generic;
using IM.Host.Classes;
using IM.Model.Enums;

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
    List<MealTicket> lstMealTicket;
    CollectionViewSource dsMealTicket;
    #endregion

    #region Contructor
    public frmMealTickets(int pguID = 0)
    {
      _pguId = pguID;

      if (pguID != 0)
      {
        lstMealTicket = BRMealTickets.GetMealTickets(pguID);
        SalesRoomCloseDates closeSalesRoom = BRSalesRooms.GetSalesRoom(App.User.SalesRoom.srID);

        // Verificamos si alguno de sus cupones de comida es de una fecha cerrada, impedimos modificar los datos
        lstMealTicket.ForEach(x =>
        {
          if (Common.IsClosed(x.meD, closeSalesRoom.srMealTicketsCloseD))
          {
            modeOpen = EnumModeOpen.Preview;
            return;
          }
        });
      }

      if (modeOpen == 0)
      {
        // Verificamos los permisos del usuario
        if (App.User.HasPermission(EnumPermission.MealTicket, EnumPermisionLevel.Standard))
        {
          modeOpen = EnumModeOpen.PreviewEdit;
        }
        else
        {
          modeOpen = EnumModeOpen.Preview;
        }
      }

      // Se verifica si tiene permisos de edición!
      InitializeComponent();

      dtpFrom.Value = frmHost.dtpServerDate;
      dtpTo.Value = frmHost.dtpServerDate;
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [erosado] 19/05/2016  Modified. Se agregó Asincronía
    /// [edgrodriguez] 21/05/2016 Modified. El método GetRateTypes se volvió asincrónico.
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      CollectionViewSource rateType = ((CollectionViewSource)(this.FindResource("dsRateType")));
      dsMealTicket = ((CollectionViewSource)(this.FindResource("dsMealTicket")));
      CollectionViewSource personnel = ((CollectionViewSource)(this.FindResource("dsPersonnel")));
      CollectionViewSource agency = ((CollectionViewSource)(this.FindResource("dsAgency")));
      CollectionViewSource mealTicketType = ((CollectionViewSource)(this.FindResource("dsMealTicketType")));

      //  Obtenemos los tipos de tarifa
      rateType.Source = frmHost._lstRateType;

      // Obtenemos los colaboradores
      personnel.Source = frmHost._lstPersonnel;

      // Obtenemos las agencias
      agency.Source = frmHost._lstAgencies;

      // Obtenemos los tipos de cupones de comida.
      mealTicketType.Source = frmHost._lstMealTicketType;

      // Verificamos si es usuario estandar o de solo lectura!
      switch (modeOpen)
      {
        case EnumModeOpen.Add:
        case EnumModeOpen.Edit:
          ControlsVisibility(true, Visibility.Visible, false, Visibility.Hidden);
          break;
        case EnumModeOpen.Search:
          ControlsVisibility(false, Visibility.Visible, false, Visibility.Hidden);
          break;
        case EnumModeOpen.Preview:
          // Se muestran y ocultan los controles necesarios
          ControlsVisibility(false, Visibility.Hidden, true, Visibility.Hidden);
          AdjustsControlsAndColumns();
          dsMealTicket.Source = lstMealTicket;
          break;
        case EnumModeOpen.PreviewEdit:
          ControlsVisibility(true, Visibility.Hidden, true, Visibility.Hidden);
          AdjustsControlsAndColumns();
          dsMealTicket.Source = lstMealTicket;
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
    private async void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      int _mealTicket = 0;
      string _folio = "";
      int _rateType = 0;
      DateTime? _dtpFrom = null;
      DateTime? _dtpTo = null;

      _busyIndicator.IsBusy = true;

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
      dsMealTicket.Source = await BRMealTickets.GetMealTickets(_mealTicket, _folio, _rateType, _dtpFrom, _dtpTo);

      _busyIndicator.IsBusy = false;
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
        dsMealTicket.Source = BRMealTickets.GetMealTickets(_pguId);
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
          dsMealTicket.Source = BRMealTickets.GetMealTickets(_pguId);
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
    private void ControlsVisibility(bool controlAdd, Visibility selectionCriteria, bool controlPrinted, Visibility checkTicket)
    {
      btnAdd.IsEnabled = controlAdd;
      btnPrint.IsEnabled = controlPrinted;
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
        dsMealTicket.Source = null;

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

    #region btnPrint_Click
    /// <summary>
    /// Imprime el ticket de comida.
    /// </summary>
    ///  <history>
    /// [edgrodriguez] 15/07/2016 Created
    /// </history>
    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      List<string> notPrinted = new List<string>();
      (dsMealTicket.Source as List<MealTicket>)
        .Where(c => !c.mePrinted)
        .ToList().ForEach(async c =>
        {
          int folio = 0;
          if (!int.TryParse(c.meFolios, out folio)) { notPrinted.Add(c.meFolios); return; }
          var mT = await BRMealTicketFolios.GetMealTicket(c.meID, Convert.ToInt32(c.meFolios), $"{App.User.User.peID} - {App.User.User.peN}");
          if (mT != null)
          {
            StringHelper.Items = new List<string>();
            LanguageHelper.IDLanguage = mT.gula;
            if (!string.IsNullOrEmpty(mT.raN))
              StringHelper.Items.Add(mT.raN.PadCenter());
            StringHelper.Items.Add($"{((char)27)}{((char)97)}1");//Centrado
            StringHelper.Items.Add("PALACE ELITE");
            StringHelper.Items.Add("VALE POR 1 BUFFET");
            StringHelper.Items.Add(mT.myN);
            StringHelper.Items.Add("\r\n");
            StringHelper.Items.Add($"{((char)27)}{((char)97)}0");//Izquierda
            if (!string.IsNullOrEmpty(mT.REFERENCE))
            {
              StringHelper.Items.Add(mT.REFERENCE);
              StringHelper.Items.Add(mT.REFERENCENAME);
            }
            else
            {
              StringHelper.Items.Add($"Guest ID: {mT.guID}");
              StringHelper.Items.Add($"{LanguageHelper.GetMessage("msgLblNameMT")}: { mT.Name}");
            }
            StringHelper.Items.Add($"{LanguageHelper.GetMessage("msgLblAdults")}: {mT.meAdults} {LanguageHelper.GetMessage("msgLblMinors")}: {mT.meMinors}");
            StringHelper.Items.Add($"{LanguageHelper.GetMessage("msgLblDate")}: {mT.meD.ToShortDateString()}");
            StringHelper.Items.Add($"{LanguageHelper.GetMessage("msgLblAuthorizedBy")}: {mT.Authorized}");
            StringHelper.Items.Add($"SalesRoom: {mT.SalesRoomID} - {mT.SalesRoomName}");
            StringHelper.Items.Add($"\r\n");
            StringHelper.Items.Add($"{((char)27)}{((char)97)}2");//Derecha
            StringHelper.Items.Add($"Folio: {mT.meFolio}");
            StringHelper.Items.Add($"{((char)27)}{((char)97)}1");
            StringHelper.Items.Add($"{LanguageHelper.GetMessage("msgLblDate")}: {DateTime.Today.ToString("dd/MM/yyyy h:mm:ss tt")}");
            StringHelper.Items.Add("\r\n");
            StringHelper.Items.Add($"{((char)29)}VB{((char)1)}");
            if (PrinterHelper.SendStringToPrinter(StringHelper.ToTicketString()))
            {
              c.mePrinted = true;
              BRMealTickets.UpdateMealTicket(c);
              dgMealTicket.Items.Refresh();
            }
          }
        });
      if (notPrinted.Any()) { UIHelper.ShowMessage($"Could not print these meal tickets.\n{string.Join("\n", notPrinted)}"); }
    }
    #endregion
  }
}
using IM.BusinessRules.BR;
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
    private EnumMode _modeOpen;
    private EnumOpenBy _openBy;
    List<MealTicket> lstMealTicket;
    public bool HasMealTicket;
    CollectionViewSource dsMealTicket;
    #endregion

    #region Contructor
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guestID">Clave del guest</param>
    /// <history>
    /// [vipacheco] 31/Marzo/2016 Created
    /// [vipacheco] 08/Agosto/2016 Modified -> Se agrega validacion isclosed
    /// </history>
    public frmMealTickets(EnumOpenBy openBy, int guestID = 0)
    {
      _pguId = guestID;
      _openBy = openBy;

      bool isclosed = false;
      // Si no esta en modo busqueda
      if (guestID != 0)
      {
        lstMealTicket = BRMealTickets.GetMealTickets(guestID);
        SalesRoomCloseDates closeSalesRoom = BRSalesRooms.GetSalesRoom(App.User.SalesRoom.srID);

        // Verificamos si alguno de sus cupones de comida es de una fecha cerrada, impedimos modificar los datos
        lstMealTicket.ForEach(x =>
        {
          if (Common.IsClosed(x.meD, closeSalesRoom.srMealTicketsCloseD))
          {
            isclosed = true;
            return;
          }
        });

        // Si no esta en una fecha cerrada
        if (!isclosed)
        {
          // Verificamos los permisos del usuario
          if (App.User.HasPermission(EnumPermission.MealTicket, EnumPermisionLevel.Standard)) { _modeOpen = EnumMode.Edit; }
          else { _modeOpen = EnumMode.ReadOnly; }
        }
      }
      // Esta en modo busqueda
      else
      {
        // Verificamos los permisos del usuario
        if (App.User.HasPermission(EnumPermission.MealTicket, EnumPermisionLevel.Standard))
        {
          _modeOpen = EnumMode.Edit;
        }
        else
        {
          _modeOpen = EnumMode.ReadOnly;
        }
      }

      // Se verifica si tiene permisos de edición!
      InitializeComponent();

      dtpFrom.Value = frmHost.dtpServerDate.AddDays(-(frmHost.dtpServerDate.Day) + 1);
      dtpTo.Value = frmHost.dtpServerDate;
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// 
    /// </summary>
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

      switch (_openBy)
      {
        case EnumOpenBy.Button:
          if (_modeOpen == EnumMode.ReadOnly)
          {
            btnAdd.IsEnabled = false;

            //ControlsVisibility(false, Visibility.Visible, false);
          }
          else if (_modeOpen == EnumMode.Edit)
          {
            btnAdd.IsEnabled = true;
            //ControlsVisibility(true, Visibility.Visible, false);
          }
          break;

        case EnumOpenBy.Checkbox:
          if (_modeOpen == EnumMode.ReadOnly)
          {
            btnAdd.IsEnabled = false;
            stkSearch.Visibility = Visibility.Collapsed;
            //ControlsVisibility(false, Visibility.Collapsed, true);
          }
          else if (_modeOpen == EnumMode.Edit)
          {
            btnAdd.IsEnabled = true;
            stkSearch.Visibility = Visibility.Collapsed;
            //ControlsVisibility(true, Visibility.Collapsed, true);
          }

          AdjustsControlsAndColumns();
          dsMealTicket.Source = lstMealTicket;
          break;
      }

      //// Verificamos si es usuario estandar o de solo lectura!
      //switch (_modeOpen)
      //{
      //  case EnumMode.Add:
      //  case EnumMode.Edit:
      //    ControlsVisibility(true, Visibility.Visible, false, Visibility.Hidden);
      //    break;
      //  case EnumMode.Search:
      //    ControlsVisibility(false, Visibility.Visible, false, Visibility.Hidden);
      //    break;
      //  case EnumMode.ReadOnly:
      //    // Se muestran y ocultan los controles necesarios
      //    ControlsVisibility(false, Visibility.Hidden, true, Visibility.Hidden);
      //    AdjustsControlsAndColumns();
      //    dsMealTicket.Source = lstMealTicket;
      //    break;
      //  case EnumMode.PreviewEdit:
      //    ControlsVisibility(true, Visibility.Hidden, true, Visibility.Hidden);
      //    AdjustsControlsAndColumns();
      //    dsMealTicket.Source = lstMealTicket;
      //    break;
      //}
    }
    #endregion

    #region btnClose_Click
    /// <summary>
    /// Función para cerrar la ventana Meal Tickets
    /// </summary>
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
    /// <history>
    /// [vipacheco] 18/03/2016 Created
    /// </history>
    private async void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      int mlTicket = 0;
      string folio = "";
      int rateType = 0;
      DateTime? fromDate = null;
      DateTime? toDate = null;

      // Verificamos que tipo de RateType tiene para ocultar las columas necesarias
      RateType itemRate = cboRateType.SelectedItem as RateType;

      _busyIndicator.IsBusy = true;

      // Obtenemos el ID de Meal Ticket
      if (txtMealTicket.Text != "") { mlTicket = Convert.ToInt32(txtMealTicket.Text); }
      // Obtenemos el folio ID si se ingreso
      if (txtFolio.Text != "") { folio = txtFolio.Text; }
      // Obtenemos el Rate Type seleccionado
      if (cboRateType.SelectedItem != null) { rateType = itemRate.raID; }
      // Obtenemos la fecha FROM ingresada
      if (dtpFrom.Value.Value.Date != null) { fromDate = dtpFrom.Value.Value.Date; }
      // Obtenemos la fecha TO ingresada
      if (dtpTo.Value.Value.Date != null) { toDate = dtpTo.Value.Value.Date; }

      // Realizamos la busqueda con los parametros ingresados!
      dsMealTicket.Source = await BRMealTickets.GetMealTickets(mlTicket, folio, rateType, fromDate, toDate);

      #region ColumnsVisibility
      // Se verifica que el SelectedItem no sea null
      if (itemRate != null)
      {
        // Si es diferente de tipo External!
        if (itemRate.raID != 4) { controlColumnVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Hidden); }
        // Es external
        else { controlColumnVisibility(Visibility.Hidden, Visibility.Visible, Visibility.Visible); }
      } 
      #endregion

      _busyIndicator.IsBusy = false;
    }
    #endregion

    #region btnAdd_Click
    /// <summary>
    /// Función para agregar un nuevo Meal Ticket
    /// </summary>
    /// <history>
    /// [vipacheco] 22/03/2016 Created
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmMealTicketsDetail _frmMealTicketsDetail = new frmMealTicketsDetail(_openBy);
      _frmMealTicketsDetail.Owner = this;
      _frmMealTicketsDetail._modeOpen = EnumMode.Add;//(_modeOpen == EnumMode.Edit) ? EnumMode.Add : EnumMode.Edit;
      _frmMealTicketsDetail.Title += "ADD";

      _frmMealTicketsDetail.ShowDialog();

      if (_modeOpen == EnumMode.Edit && _openBy == EnumOpenBy.Checkbox)
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
    /// <history>
    /// [vipacheco] 23/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      // Se verifica que tenga permisos de editar
      if (_modeOpen != EnumMode.ReadOnly)
      {
        MealTicket mealTicket = (MealTicket)grdMealTicket.SelectedItem;

        //_pguId = mealTicket.megu;
        frmMealTicketsDetail _frmMealTicketsDetail = new frmMealTicketsDetail(_openBy) { Owner = this };
        ObjectHelper.CopyProperties(_frmMealTicketsDetail._mealTicketCurrency, mealTicket);
        //_frmMealTicketsDetail._modeOpen = (_modeOpen == EnumMode.Edit) ? EnumMode.Edit : EnumMode.ReadOnly;
        _frmMealTicketsDetail._modeOpen = EnumMode.Edit;
        _frmMealTicketsDetail.Title += "ID " + mealTicket.meID;

        if (_frmMealTicketsDetail.ShowDialog() == true)
        {
          ObjectHelper.CopyProperties(mealTicket, _frmMealTicketsDetail._mealTicketCurrency);
        }

        if (_modeOpen == EnumMode.Edit && _openBy == EnumOpenBy.Checkbox)
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
    /// <history>
    /// [vipacheco] 23/04/2016 Created
    /// </history>
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
    /// <history>
    /// [vipacheco] 22/03/2016 Created
    /// </history>
    private void ControlsVisibility(bool controlAdd, Visibility selectionCriteria, bool controlPrinted)
    {
      btnAdd.IsEnabled = controlAdd;
      btnPrint.IsEnabled = controlPrinted;
      stkSearch.Visibility = selectionCriteria;
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
    /// Funcion encargada de alinear el margen y ocultar columnas cuando el modeOpen es ReadOnly y PreviewEdit
    /// </summary>
    /// <history>
    /// [vipacheco] 02/04/2016 Created
    /// </history>
    private void AdjustsControlsAndColumns()
    {
      Thickness _margin = grdMealTicket.Margin;
      _margin.Top = 18;
      grdMealTicket.Margin = _margin;
      // Se ocultan la primeras 4 columnas!
      //meraColumn.Visibility = Visibility.Hidden;
      //mepeColumn.Visibility = Visibility.Hidden;
      //meagColumn.Visibility = Visibility.Hidden;
      //merepColumn.Visibility = Visibility.Hidden;
    }
    #endregion

    #region btnPrint_Click
    /// <summary>
    /// Imprime el ticket de comida.
    /// </summary>
    ///  <history>
    /// [edgrodriguez] 15/07/2016 Created
    /// [vipacheco] 13/Agosto/2016 Modified -> se elimino el recorrido de todos los items del grid, por la seleccion actual.
    /// </history>
    private async void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      MealTicket mtck = grdMealTicket.SelectedItem as MealTicket;
      int folio = 0;
      if (!int.TryParse(mtck.meFolios, out folio)) { UIHelper.ShowMessage($"Could not print this meal ticket.\n{string.Join("\n", mtck.meFolios)}"); ; return; }
      var mT = await BRMealTicketFolios.GetMealTicket(mtck.meID, Convert.ToInt32(mtck.meFolios), $"{App.User.User.peID} - {App.User.User.peN}");
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
          mtck.mePrinted = true;
          BRMealTickets.UpdateMealTicket(mtck);
          grdMealTicket.Items.Refresh();
        }
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica si ya cuenta con un MealTicket guardado
    /// </summary>
    /// <history>
    /// [vipacheco] 10/Agosto/2016 Created
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (_openBy == EnumOpenBy.Checkbox)
      {
        var lstResult = grdMealTicket.Items.OfType<MealTicket>().ToList();

        if (lstResult.Any(x => x.meID > 0))
        { HasMealTicket = true; }
        else
        {
          HasMealTicket = false;
        }
      }
    }
    #endregion

    private void grdMealTicket_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      DataGrid grd = sender as DataGrid;

      if (grd != null && grd.Items.Count > 0)
      {
        MealTicket mtck = grd.SelectedItem as MealTicket;
        // verificamos si ya se imprimio
        if (mtck.mePrinted) { btnPrint.IsEnabled = false; }
        else
        {
          // verificamos que no se modo lectura
          if (_modeOpen != EnumMode.ReadOnly) { btnPrint.IsEnabled = true; }
          else { btnPrint.IsEnabled = false; }
        }
      }
    }
  }
}
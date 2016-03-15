using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Data;
using IM.Base.Helpers;
using System.IO;
using System.Dynamic;

namespace IM.ProcessorGeneral.Forms
{
  /// <summary>
  /// Interaction logic for frmProcessor.xaml
  /// </summary>
  public partial class frmProcessorGeneral : Window
  {
    #region Atributos
    private bool _skipSelectionChanged = true;//Bandera para evitar que el evento SelectionChanged se dispare al asignar el datasource.
    private frmFilterDateRange _frmFilter;
    private bool _blnOneDate;
    private bool _blnOnlyOneRegister;

    public List<int> _lstGifts = new List<int>();
    public List<int> _lstGiftsCate = new List<int>();
    public List<int> _lstSalesRoom = new List<int>();
    public List<int> _lstLeadSources = new List<int>();
    public List<int> _lstPrograms = new List<int>();
    public List<int> _lstRateTypes = new List<int>();
    public string _cboDateSelected;
    public DateTime _dtmStart = DateTime.Now.Date;
    public DateTime _dtmEnd= DateTime.Now.Date;
    public EnumBasedOnArrival _enumBasedOnArrival=EnumBasedOnArrival.boaNoBasedOnArrival;
    public EnumBasedOnBooking _enumBasedOnBooking=EnumBasedOnBooking.bobNoBasedOnBooking;
    public EnumQuinellas _enumQuinellas=EnumQuinellas.quNoQuinellas;
    public EnumDetailGifts _enumDetailsGift=EnumDetailGifts.dgNoDetailGifts;
    public EnumSalesByMemberShipType _enumSalesByMemberShipType=EnumSalesByMemberShipType.sbmNoDetail;
    public EnumStatus _enumStatus=EnumStatus.staActives;
    public EnumGiftsReceiptType _enumGiftsReceiptType=EnumGiftsReceiptType.grtAll;
    public string _GuestID="";
    public EnumGiftSale _enumGiftSale=EnumGiftSale.gsAll;
    public EnumSaveCourtesyTours _enumSaveCourtesyTours=EnumSaveCourtesyTours.sctExcludeSaveCourtesyTours;
    public EnumExternalInvitation _enumExternalInvitation=EnumExternalInvitation.extExclude;

    #endregion

    #region Constructor
    public frmProcessorGeneral()
    {
      InitializeComponent();
      //_frmFilter = new frmFilterDateRange();
      // _frmFilter.CloseAllowed = false;
    }
    #endregion

    #region Eventos Formulario

    #region Window_Loaded
    /// <summary>
    /// Método de inicio del formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/Mar/2016 Created
    /// </history>
    private void Window_Loaded(object sender, EventArgs e)
    {
      configurarGrids();
      lblUserName.Content = App.userData.User.peN;      
    }
    #endregion

    #region grdrptSalesRooms_dblClick
    /// <summary>
    /// Método para abrir la ventana de filtros
    /// al hacer doble clic sobre alguno registro del
    /// gridSalesRooms.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/Mar/2016 Created
    /// </history>
    private void grdrptSalesRooms_dblClick(object sender, RoutedEventArgs e)
    {
      PrepraReportBySalesRoom();
    }
    #endregion

    #region grdrptSalesRooms_SelectionChanged
    /// <summary>
    /// Activa/Desactiva el botón de Diseñador de reportes. 
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// </history>
    private void grdrptSalesRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!_skipSelectionChanged)
      {
        string _rptNombre = ((dynamic)e.AddedItems[0]).rptNombre;
        switch (_rptNombre)
        {
          case "Gifts Manifest (Excel)":
          case "Gifts Receipts (Excel)":
          case "Weekly and Monthly Hostess":
          case "Weekly and Monthly Hostess (Golf & Sunrise)":
          case "Gifts Sale (Excel)":
            btnRptDsrSR.IsEnabled = false;
            break;
          default:
            btnRptDsrSR.IsEnabled = true;
            break;
        }
      }
      else
        grdrptSalesRooms.SelectedIndex = -1;
    }
    #endregion

    #region grdrptSalesRooms_PreviewKeyDown
    /// <summary>
    /// Activa/Desactiva el botón de Diseñador de reportes. 
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// </history>
    private void grdrptSalesRooms_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        if (grdrptSalesRooms.SelectedIndex >= 0)
        {
          PrepraReportBySalesRoom();
        }
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Cambia la propiedad CloseAllowed de la ventana frmFilterDateRange
    /// para permitir el cierre de la misma.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 09/Mar/2016 Created
    /// </history>
    private void Window_Closing(object sender, CancelEventArgs e)
    {      
      _frmFilter.Close();
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [edgrodriguez] 14/Mar/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }
    #endregion

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [edgrodriguez] 14/Mar/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    } 
    #endregion

    #region btnPrintSR_Click
    /// <summary>
    /// Mediante el botónPrint abre el formulario FilterDateRange
    /// para posteriormente guardar el reporte en excel.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/Mar/2016 Created
    /// </history>
    private void btnPrintSR_Click(object sender, RoutedEventArgs e)
    {
      PrepraReportBySalesRoom();
    }
    #endregion

    #region btnExit_Click
    /// <summary>
    /// Cierra la aplicacion Processor General.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
      App.Current.Shutdown();
    }
    #endregion

    #endregion

    #region Métodos Privados

    #region PrepareReportsBySalesRoom
    /// <summary>
    /// Prepara un reporte por Sala de venta.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// </history>
    private void PrepraReportBySalesRoom()
    {
      string strReport = "";
      //Validamos que haya un reporte seleccionado.
      if (grdrptSalesRooms.SelectedIndex < 0)
        return;

      WaitMessage(true, "Loading Date Range Window...");

      //Obtenemos el nombre del reporte.
      strReport = ((dynamic)grdrptSalesRooms.SelectedItem).rptNombre;
      #region Configurando Fecha
      switch (strReport)
      {
        //Reportes que utilizan solo una fecha
        case "Gifts By Category":
        case "Gifts By Category & Program":
        case "Daily Gifts (Simple)":
        case "Weekly Gifts (ITEMS) (Simple)":
        case "Weekly and Monthly Hostess":
        case "Weekly and Monthly Hostess (Golf & Sunrise)":
        case "Bookings By Sales Room, Program & Time":
        case "Bookings By Sales Room, Program, Lead Source & Time":
          _blnOneDate = true;
          break;
        default:
          _blnOneDate = false;
          break;
      }
      #endregion
      #region Configurando Grids Modo de Seleccion
      switch (strReport)
      {
        //Reportes que permiten seleccionar solo un registro.
        case "CxC":
        case "CxC (Excel)":
        case "CxC Not Authorized":
        case "CxC Payments":
          _blnOnlyOneRegister = true;
          break;
        default:
          _blnOnlyOneRegister = false;
          break;
      }
      #endregion
      AbrirFilterDateRange(strReport);
    }

    /// <summary>
    /// Abre la ventana frmFilterDateRange configurando
    /// los controles segun el reporte seleccionado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// </history>
    private void AbrirFilterDateRange(string strReport)
    {
       _frmFilter= new frmFilterDateRange();
      _frmFilter.frmPG = this;
      #region Abriendo FrmFilter segun reporte seleccionado.
      switch (strReport)
      {
        case "Weekly and Monthly Hostess (Golf & Sunrise)":
          _frmFilter.ConfigurarFomulario(blnOneDate: _blnOneDate);
          break;
        case "Deposits":
        case "Guests No Buyers":
        case "Memberships":
        case "Memberships by Host":
        case "Bookings By Sales Room, Program & Time":
        case "Bookings By Sales Room, Program, Lead Source & Time":
        case "In & Out":
        case "No Shows":
        case "Burned Deposits":
        case "Burned Deposits by Resort":
        case "Paid Deposits":
        case "Taxis In":
        case "Taxis Out":
        case "Gifts By Category":
        case "Gifts By Category & Program":
        case "Memberships by Agency & Market":
        case "Liner Statistics":
        case "Closer Statistics":
        case "Cancelled Gifts Manifest":
        case "CxC Not Authorized":
        case "CxC Gifts":
        case "CxC Deposits":
        case "Gifts Receipts Payments":
        case "CxC Payments":
        case "Guest CECO":
          _frmFilter.ConfigurarFomulario(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnSalesRoom: true);          
          break;
        case "Gifts Manifest (Excel)":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnGifts: true, blnAllGifts: true, blnCategories: true, blnAllCategories: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister,
            blncbStatus: true);
          break;
        case "Gifts Receipts (Excel)":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnGifts: true, blnAllGifts: true, blnCategories: true, blnAllCategories: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister,
             blncbStatus: true, blnGiftReceiptType: true, blnGuestID: true);
          break;
        case "Gifts Certificates":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnGifts: true, blnAllGifts: true, blnCategories: true, blnAllCategories: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister);
          break;
        case "Gifts Sale":
        case "Gifts Sale (Excel)":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnGifts: true, blnAllGifts: true, blnCategories: true, blnAllCategories: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister,
             blnGiftSale: true);
          break;
        case "Manifest":
        case "Manifest (Excel)":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnPrograms: true, blnAllPrograms: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister);
          break;
        case "Gifts Used by Sistur":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnPrograms: true, blnAllPrograms: true, blnLeadSources: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister);
          break;
        case "Production by Sales Room":
        case "Production by Sales Room & Market":
        case "Production by Sales Room, Program, Market & Submarket":
        case "Production by Show Program":
        case "Production by Show Program & Program":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, enumBasedOnArrival: EnumBasedOnArrival.boaBasedOnArrival, enumQuinellas: EnumQuinellas.quQuinellas);
          break;
        case "Meal Tickets":
        case "Meal Tickets by Host":
        case "Meal Tickets Cancelled":
        case "Meal Tickets with Cost":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnRatetypes: true, blnAllRatetypes: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister);
          break;
        default:
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister);
          break;
      }
      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        showSalesRoomReport(strReport);
        _frmFilter.Close();
      }
      #endregion
    }

    /// <summary>
    /// Muestra el reporte seleccionado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 09/Mar/2016 Created
    /// </history>
    private void showSalesRoomReport(string strReport)
    {
      bool blnOk = false;
      string dateRange = (_blnOneDate) ? _frmFilter.dtmStart.Value.Value.ToLongDateString() : string.Concat(_frmFilter.dtmStart.Value.Value.ToLongDateString(), " - ", _frmFilter.dtmEnd.Value.Value.ToLongDateString());
      DataTable dtData = new DataTable();
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
      switch (strReport)
      {
        case "Bookings By Sales Room, Program & Time":
          List<RptBookingsBySalesRoomProgramTime> lstRptBBSalesRoom = BRReportsBySalesRoom.getRptBookingsBySalesRoomProgramTime(_frmFilter.dtmStart.Value.Value, string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstRptBBSalesRoom.Count > 0)
          {
            dtData = GridHelper.GetDataTableFromGrid(lstRptBBSalesRoom);
            var pivot = lstRptBBSalesRoom.ToPivotArray(
              c => c.Time,
              c => new { c.SalesRoom, c.Program, c.BookType },
              c => c.Any() ? c.Sum(b => b.Books) : 0);
            Tuple<string, string> Reportname = new Tuple<string, string>(Regex.Replace(strReport, "[^a-zA-Z0-9_]+", " "), dateRange);
            filters.Add(new Tuple<string, string>("SalesRoom", _frmFilter.chkAllSalesRoom.IsChecked == true ? "ALL" : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => c.srID).ToList())));
            blnOk= EpplusHelper.CreatePivotRptExcel(filters, dtData, Reportname, new List<string> { "Time" }, new List<string> { "SalesRoom", "Program", "BookType" }, new List<string> { "Books" }, ((ExpandoObject)pivot[0]).Select(c => c.Key).Count());
          }
          break;
      }

      if (blnOk) UIHelper.ShowMessage("The file was succesfully saved.");
    }
    #endregion

    #region ConfigurarGrids
    /// <summary>
    /// Se configuran los treeview, agregando 
    /// los reportes.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/Mar/2016 Created
    /// </history>
    private void configurarGrids()
    {
      try
      {
        _skipSelectionChanged = true;
        #region Grid SalesRooms
        ListCollectionView lstrptSalesRooms = new ListCollectionView(new List<dynamic> {
        new { rptNombre = "Bookings By Sales Room, Program & Time", rptGroup="Bookings"},
        new { rptNombre="Bookings By Sales Room, Program, Lead Source & Time", rptGroup="Bookings"},

        new { rptNombre="CxC", rptGroup="CxC"},
        new { rptNombre="CxC Gifts" ,rptGroup="CxC"},
        new { rptNombre="CxC Deposits",rptGroup="CxC"},
        new{ rptNombre="CxC (Excel)",rptGroup="CxC"},
        new{ rptNombre="CxC Not Authorized",rptGroup="CxC"},
        new{ rptNombre="CxC Payments",rptGroup="CxC"},

        new{ rptNombre="Deposits" ,rptGroup="Deposits" },
        new{ rptNombre="Paid Deposits",rptGroup="Deposits" },
        new{ rptNombre="Burned Deposits",rptGroup="Deposits" },
        new{ rptNombre="Burned Deposits by Resort",rptGroup="Deposits" },

        new{ rptNombre="Daily Gifts (Simple)",rptGroup="Gifts" },
        new{ rptNombre="Gifts Manifest (Excel)",rptGroup="Gifts" },
        new{ rptNombre="Cancelled Gifts Manifest",rptGroup="Gifts" },
        new{ rptNombre="Gifts By Category",rptGroup="Gifts" },
        new{ rptNombre="Gifts By Category & Program",rptGroup="Gifts" },
        new{ rptNombre="Weekly Gifts (ITEMS) (Simple)",rptGroup="Gifts" },
        new{ rptNombre="Gifts Certificates",rptGroup="Gifts" },
        new{ rptNombre="Gifts Receipts (Excel)",rptGroup="Gifts" },
        new{ rptNombre="Gifts Sale",rptGroup="Gifts" },
        new{ rptNombre="Gifts Receipts Payments",rptGroup="Gifts" },
        new{ rptNombre="Gifts Sale (Excel)",rptGroup="Gifts" },
        new{ rptNombre="Gifts Used by Sistur",rptGroup="Gifts" },

        new { rptNombre="Manifest",rptGroup="Guests" },
        new{ rptNombre="Manifest (Excel)",rptGroup="Guests" },
        new{ rptNombre="No Shows",rptGroup="Guests" },
        new{ rptNombre="Guests No Buyers",rptGroup="Guests" },
        new{ rptNombre="In & Out",rptGroup="Guests" },
        new{ rptNombre="Guest CECO",rptGroup="Guests" },

        new{ rptNombre="Meal Tickets",rptGroup="Meal Tickets" },
        new{ rptNombre="Meal Tickets by Host",rptGroup="Meal Tickets" },
        new{ rptNombre="Meal Tickets Cancelled",rptGroup="Meal Tickets" },
        new{ rptNombre="Meal Tickets with Cost",rptGroup="Meal Tickets" },

        new{ rptNombre="Memberships",rptGroup="Memberships" },
        new{ rptNombre="Memberships by Host",rptGroup="Memberships" },
        new{ rptNombre="Memberships by Agency & Market",rptGroup="Memberships" },

        new{ rptNombre="Production by Sales Room",rptGroup="Production" },
        new{ rptNombre="Production by Sales Room & Market",rptGroup="Production" },
        new{ rptNombre="Production by Sales Room, Program, Market & Submarket",rptGroup="Production" },
        new{ rptNombre="Production by Show Program",rptGroup="Production" },
        new{ rptNombre="Production by Show Program & Program",rptGroup="Production" },

        new{ rptNombre="Liner Statistics",rptGroup="Salesmen" },
        new{ rptNombre="Closer Statistics",rptGroup="Salesmen" },
        new{ rptNombre="Weekly and Monthly Hostess",rptGroup="Salesmen" },
        new{ rptNombre="Weekly and Monthly Hostess (Golf & Sunrise)",rptGroup="Salesmen" },

        new { rptNombre="Taxis In",rptGroup="Taxis" },
        new{ rptNombre="Taxis Out",rptGroup="Taxis" }
      }.OrderBy(c => c.rptGroup).ThenBy(c => c.rptNombre).ToList());

        lstrptSalesRooms.GroupDescriptions.Add(new PropertyGroupDescription("rptGroup"));
        grdrptSalesRooms.ItemsSource = lstrptSalesRooms;
        #endregion
        #region Grid LeadSources
        List<dynamic> lstrptLeadSources = new List<dynamic>
      {
        new { rptNombre= "Deposits by PR" },
        new {rptNombre="Deposits No Show" },
        new {rptNombre="Paid Deposits by PR"},
        new {rptNombre="Memberships"},
        new {rptNombre="Memberships by Host"},
        new {rptNombre="Self Gen"},
        new {rptNombre="Personnel Access"},
        new {rptNombre="In & Out by PR"},
        new {rptNombre="Burned Deposits Guests"},
        new {rptNombre="Deposit Refund" }
      }.OrderBy(c => c.rptNombre).ToList();
        grdrptLeadSources.ItemsSource = lstrptLeadSources;
        #endregion
        #region Grid General
        List<dynamic> lstrptGeneral = new List<dynamic>
      {
        new {rptNombre="Personnel" },
        new {rptNombre="Reps"},
        new {rptNombre="Agencies"},
        new {rptNombre="Gifts"},
        new {rptNombre="Gifts Kardex"},
        new {rptNombre="Warehouse Movements"},
        new {rptNombre="Logins Log" },
        new {rptNombre="Production Referral"},
        new {rptNombre="Gifts Given In Kind Electronic Purse"},
        new {rptNombre="Sales By Program, Lead Source & Market"},
        new {rptNombre="Production by Lead Source & Market (Monthly)" }
      }.OrderBy(c => c.rptNombre).ToList();
        grdrptGeneral.ItemsSource = lstrptGeneral;

        #endregion

        StatusBarReg.Content = string.Format("{0} Reports", lstrptGeneral.Count() + lstrptLeadSources.Count + lstrptSalesRooms.Count); 
      }
      finally
      {
        _skipSelectionChanged = false;
      }
    }
    #endregion

    #region WaitMessage
    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/Mar/2016 Created
    /// </history>
    private void WaitMessage(bool show, String message = "")
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = (show) ? Visibility.Visible : Visibility.Hidden;
      this.Cursor = (show) ? Cursors.Wait : null;
      UIHelper.ForceUIToUpdate();
    }
    #endregion

    #endregion
  }
}
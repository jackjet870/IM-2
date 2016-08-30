using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using IM.InventoryMovements.Clases;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.Model.Enums;

namespace IM.InventoryMovements
{
  /// <summary>
  /// Interaction logic for frmInventoryMovements.xaml
  /// </summary>
  public partial class frmInventoryMovements : Window
  {
    #region Atributos

    private DateTime? _dtmcurrent; //Variable para manejar un problema del control DatePicker
    private List<objWhsMovs> _lstobjWhsMovs;//Lista para nuevos registros de WhsMovs
    private SalesRoomCloseDates _salesRoom = new SalesRoomCloseDates();
    private DateTime _dtmServerdate;
    private CollectionViewSource _objWhsMovsViewSource;
    private CollectionViewSource _getGiftsViewSource;
    private CollectionViewSource _whsMovViewSource;
    bool _HasError;
    public ExecuteCommandHelper DateFocus { get; set; }
    public ExecuteCommandHelper SaveData { get; set; }
    public ExecuteCommandHelper CancelEdit { get; set; }
    public ExecuteCommandHelper CloseWindow { get; set; }
    #endregion

    #region Constructores y destructores

    #region frmInventoryMovements
    /// <summary>
    /// Constructor
    /// </summary>
    /// <history>
    /// [wtorres]  14/Mar/2016 Modified. Elimine el parametro UserData
    /// </history>
    public frmInventoryMovements()
    {
      InitializeComponent();
      DateFocus = new ExecuteCommandHelper(c => dtpDate.Focus());
      SaveData = new ExecuteCommandHelper(c => btnSave_Click(null, null));
      CancelEdit = new ExecuteCommandHelper(c => btnCancel_Click(null, null));
      CloseWindow = new ExecuteCommandHelper(c => btnClose_Click(null, null));

    }
    #endregion

    #endregion

    #region Metodos del formulario

    #region frmInventoryMovements_Loaded
    /// <summary>
    /// Inicio y configuracion del formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Feb/2016 Created
    /// </history>
    private void frmInventoryMovements_Loaded(object sender, RoutedEventArgs e)
    {
      _salesRoom = BRSalesRooms.GetSalesRoom(App.User.Warehouse.whID);
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
      lblUserName.Content = App.User.User.peN;
      lblWareHouse.Content = App.User.Warehouse.whN;
      lblCloseDate.Content = "Close Receipts Date: " + _salesRoom.srGiftsRcptCloseD.ToString("dd/MMM/yyyy");
      InitializeGrdNew();
      _dtmServerdate = BRHelpers.GetServerDate();
      dtpDate_SelectedDateChanged(null, null);
      GridHelper.SetUpGrid(grdNew, new WarehouseMovement());
      if (((EnumPermisionLevel)App.User.Permissions.FirstOrDefault(c => c.pppm == "GIFTSRCPTS")?.pppl) >=
          EnumPermisionLevel.Special)
      {
        fraDate.IsEnabled = true;
      }
    }
    #endregion

    #region frmInventoryMovements_KeyDown
    /// <summary>
    /// Se verifica la tecla que el usuario presionó y aplica el cambio de estilo
    /// a los StatusBarItem.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Feb/2016 Created
    /// </history>
    private void frmInventoryMovements_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
          break;
        case Key.Insert:
          KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
          break;
        case Key.NumLock:
          KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
          break;
      }
    }
    #endregion

    #region btnClose_Click
    /// <summary>
    /// Cierra el formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Feb/2016 Created
    /// </history>
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
    #endregion

    #region dtpDate_SelectedDateChanged
    /// <summary>
    /// Método que se ejecuta al dispararse el evento SelectedDateChanged,
    /// si existió un cambio se realiza la consulta a la base de datos para
    /// actualizar el grid.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Feb/2016 Created
    /// </history>
    private void dtpDate_SelectedDateChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      // [wtorres] 15/Mar/2016 validamos que el formulario ya se haya inicializado
      if (!IsInitialized || string.IsNullOrEmpty(dtpDate.Text)) return;

      if (dtpDate.Value != null && _dtmcurrent != dtpDate.Value.Value)
      {
        _whsMovViewSource = ((CollectionViewSource)FindResource("whsMovViewSource"));
        // Load data by setting the CollectionViewSource.Source property:
        _whsMovViewSource.Source = BRWarehouseMovements.GetWarehouseMovements(App.User.Warehouse.whID, dtpDate.Value.Value.Date);
        StatusBarReg.Content = $"{grd.SelectedItems.Count}/{_whsMovViewSource.View.SourceCollection.Cast<WarehouseMovementShort>().Count()}";
      }
      if (dtpDate.Value != null) _dtmcurrent = dtpDate.Value.Value;
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Método para cancelar el proceso de creación de nuevos movimientos de inventario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Feb/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      InitializeGrdNew();
    }
    #endregion

    #region btnSave_Click
    /// <summary>
    /// Método para guardar los nuevos movimientos de inventario a la base de datos.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 22/Feb/2016 Created
    /// </history>
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (!ValidateCurrentDate() || _lstobjWhsMovs.Count <= 0 ||
          _lstobjWhsMovs.Any(c => c.wmQty == 0 || c.wmgi == null || c.wmgi == "")) return;

      _lstobjWhsMovs.ForEach(c =>
      {
        c.wmD = dtpDate.Value.Value;
        c.wmwh = App.User.Warehouse.whID;
        c.wmpe = App.User.User.peID;
      });

      List<WarehouseMovement> lstWhsMov = _lstobjWhsMovs.Select(c => new WarehouseMovement
      {
        wmComments = c.wmComments,
        wmD = c.wmD,
        wmgi = c.wmgi,
        wmID = c.wmID,
        wmpe = c.wmpe,
        wmQty = c.wmQty,
        wmwh = c.wmwh
      }).ToList();

      try
      {
        int nRes = await BREntities.OperationEntities(lstWhsMov, EnumMode.Add);
        if (nRes > 0)
        {
          UIHelper.ShowMessage("The warehouse movements was saved successfully.", title: "Intelligence Marketing");
          InitializeGrdNew();
          _whsMovViewSource.Source = BRWarehouseMovements.GetWarehouseMovements(App.User.Warehouse.whID,
            dtpDate.Value.Value.Date);
        }
        else
          UIHelper.ShowMessage("The warehouse movements was not saved.", MessageBoxImage.Error,
            "Intelligence Marketing");
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }

    }
    #endregion

    #region btnAbout_Click
    /// <summary>
    /// Método para mostrar el formulario About.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 19/Feb/2016 Created
    /// </history>
    private void btnAbout_Click(object sender, RoutedEventArgs e)
    {
      frmAbout frmAbout = new frmAbout();
      frmAbout.Owner = this;
      frmAbout.ShowDialog();
    }
    #endregion

    #region grd_SelectionChanged
    /// <summary>
    /// Método que actualiza la StatusBarReg con la cantidad
    /// de registros seleccionados.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 25/Feb/2016 Created
    /// </history>
    private void grd_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarReg.Content = string.Format("{0}/{1}", grd.SelectedItems.Count, _whsMovViewSource.View.SourceCollection.Cast<WarehouseMovementShort>().Count());
    }
    #endregion

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [edgrodriguez] 29/Jul/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region grdNew_CellEditEnding
    /// <summary>
    /// Proceso que se activa al terminar de editar una celda.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Ago/2016 Created
    /// </history>
    private void grdNew_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      grdNew.CellEditEnding -= grdNew_CellEditEnding;
      var _objWhsMov = e.Row.Item as objWhsMovs;
      var lstwhsmovs = (List<objWhsMovs>)_objWhsMovsViewSource.Source;
      var column = e.Column.SortMemberPath;
      switch (column)
      {
        case "wmQty":
          var textbox = e.EditingElement as TextBox;
          if (string.IsNullOrWhiteSpace(textbox.Text) || textbox.Text == "0")
          {
            UIHelper.ShowMessage("Enter a quantity.", MessageBoxImage.Exclamation, "Intelligence Marketing");
            grdNew.CurrentCell = new DataGridCellInfo(_objWhsMov, grdNew.Columns.FirstOrDefault());
            GridHelper.SelectRow(grdNew, lstwhsmovs.IndexOf(_objWhsMov), blnEdit: true);
            _HasError = true;
            e.Cancel = true;
          }
          break;
      }
      grdNew.CellEditEnding += grdNew_CellEditEnding;
    }
    #endregion

    #region grdNew_BeginningEdit
    /// <summary>
    /// Proceso que se activa antes de comenzar a editar una celda.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Ago/2016 Created
    /// </history>
    private void grdNew_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      var column = e.Column.SortMemberPath;
      var lstwhsmovs = (List<objWhsMovs>)_objWhsMovsViewSource.Source;
      objWhsMovs _objWhsMov = grdNew.CurrentCell.Item as objWhsMovs;
      if (_HasError) { e.Cancel = true; _HasError = false; return; }
      switch (column)
      {
        case "wmgi":
          if (_objWhsMov != null && _objWhsMov.wmQty == 0)
          {
            UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
            _HasError = true;
            e.Cancel = true;
            //grdNew.CurrentCell = new DataGridCellInfo(_objWhsMov, grdNew.Columns.FirstOrDefault());
            GridHelper.SelectRow(grdNew, lstwhsmovs.IndexOf(_objWhsMov), blnEdit: true);
          }
          break;
      }
    }
    #endregion

    #endregion

    #region Metodos Privados

    #region ValidateCurrentDate
    /// <summary>
    /// Método para validar que la fecha de cierre de recibos no sea mayor que
    /// la fecha del servidor o la fecha seleccionada.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 22/Feb/2016 Created
    /// </history>
    private bool ValidateCurrentDate()
    {
      bool blnValid = true;
      DateTime dtmsrGiftsRcptCloseD = _salesRoom.srGiftsRcptCloseD;
      if (dtmsrGiftsRcptCloseD >= dtpDate.Value.Value)
      {
        UIHelper.ShowMessage("Date already close. New movements can not be added.");
        dtpDate.Value = dtmsrGiftsRcptCloseD.AddDays(1);
        blnValid = false;
      }
      else if (dtpDate.Value.Value > _dtmServerdate)
      {
        UIHelper.ShowMessage("Date can not be greater than today.");
        dtpDate.Value = _dtmServerdate;
        blnValid = false;
      }

      return blnValid;
    }
    #endregion

    #region InitializeGrdNew
    /// <summary>
    /// Método para asignar valores a los controles del formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Feb/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void InitializeGrdNew()
    {
      _lstobjWhsMovs = new List<objWhsMovs>();
      _objWhsMovsViewSource = ((CollectionViewSource)FindResource("objWhsMovsViewSource"));
      // Load data by setting the CollectionViewSource.Source property:
      _objWhsMovsViewSource.Source = _lstobjWhsMovs;
      _getGiftsViewSource = ((CollectionViewSource)FindResource("getGiftsViewSource"));
      // Load data by setting the CollectionViewSource.Source property:
      _getGiftsViewSource.Source = await BRGifts.GetGiftsShort(App.User.Warehouse.whID, 1);
    }
    #endregion

    #endregion
  }
}
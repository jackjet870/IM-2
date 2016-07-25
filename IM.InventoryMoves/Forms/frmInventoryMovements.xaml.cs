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
    #endregion

    #region Constructores y destructores

    /// <summary>
    /// Constructor
    /// </summary>
    /// <history>
    /// [wtorres]  14/Mar/2016 Modified. Elimine el parametro UserData
    /// </history>
    public frmInventoryMovements()
    {
      InitializeComponent();
    }

    #endregion

    #region Metodos del formulario

    /// <summary>
    /// Inicio y configuracion del formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Feb/2016 Created
    /// </history>
    private void frmInventoryMovements_Loaded(object sender, RoutedEventArgs e)
    {
      _salesRoom = BRSalesRooms.GetSalesRoom(App.User.Warehouse.whID);
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
      lblUserName.Content = App.User.User.peN;
      lblWareHouse.Content = App.User.Warehouse.whN;
      lblCloseDate.Content = "Close Receipts Date: " + _salesRoom.srGiftsRcptCloseD.ToString("dd/MMM/yyyy");
      InicializarGrdNew();
      _dtmServerdate = BRHelpers.GetServerDate();
      dtpDate_SelectedDateChanged(null, null);
      if (((EnumPermisionLevel) App.User.Permissions.FirstOrDefault(c => c.pppm == "GIFTSRCPTS").pppl) >=
          EnumPermisionLevel.Special)
      {
        fraDate.IsEnabled = true;
      }
    }

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
          CkeckKeysPress(StatusBarCap, Key.Capital);
          break;
        case Key.Insert:
          CkeckKeysPress(StatusBarIns, Key.Insert);
          break;
        case Key.NumLock:
          CkeckKeysPress(StatusBarNum, Key.NumLock);
          break;
      }
    }

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

    /// <summary>
    /// Método para cancelar el proceso de creación de nuevos movimientos de inventario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Feb/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      InicializarGrdNew();
    }

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

        //BRWarehouseMovements.SaveWarehouseMovements(ref lstWhsMov);
        int nRes = await BREntities.OperationEntities(lstWhsMov, EnumMode.add);
        if (nRes > 0)
        {
          UIHelper.ShowMessage("The warehouse movements was saved successfully.", title: "Intelligence Marketing");
          InicializarGrdNew();
          _whsMovViewSource.Source = BRWarehouseMovements.GetWarehouseMovements(App.User.Warehouse.whID,
            dtpDate.Value.Value.Date);
        }
        else
          UIHelper.ShowMessage("The warehouse movements was not saved.", MessageBoxImage.Error,
            "Intelligence Marketing");
      
    }

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

    #region Metodos Privados

    /// <summary>
    /// Verifica que la tecla se encuentre activa/inactiva, para cambiar el estilo de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Feb/2016 Created
    /// </history>
    private void CkeckKeysPress(System.Windows.Controls.Primitives.StatusBarItem statusBar, Key key)
    {
      var keyPess = Keyboard.GetKeyStates(key).ToString();

      if (keyPess.Contains("Toggled")) //si está activo el Bloq Mayús
      {
        statusBar.FontWeight = FontWeights.Bold;
        statusBar.Foreground = Brushes.Black;
      }
      else
      {
        KeyDefault(statusBar);
      }
    }

    /// <summary>
    /// Configuracion inicial de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Feb/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
     
    }

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
        MessageBox.Show("Date already close. New movements can not be added.", "Caution", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        dtpDate.Value = dtmsrGiftsRcptCloseD.AddDays(1);
        blnValid = false;
      }
      else if (dtpDate.Value.Value > _dtmServerdate)
      {
        MessageBox.Show("Date can not be greater than today.", "Caution", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        dtpDate.Value = _dtmServerdate;
        blnValid = false;
      }

      return blnValid;
    }

    /// <summary>
    /// Método para asignar valores a los controles del formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Feb/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void InicializarGrdNew()
    {
      _lstobjWhsMovs = new List<objWhsMovs>();
      _objWhsMovsViewSource = ((CollectionViewSource)FindResource("objWhsMovsViewSource"));
      // Load data by setting the CollectionViewSource.Source property:
      _objWhsMovsViewSource.Source = _lstobjWhsMovs;
      _getGiftsViewSource = ((CollectionViewSource)FindResource("getGiftsViewSource"));
      // Load data by setting the CollectionViewSource.Source property:
      _getGiftsViewSource.Source =await BRGifts.GetGiftsShort(App.User.Warehouse.whID, 1);
    }

    /// <summary>
    /// Valida las celdas del gridNew
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/Jul/2016 Created
    /// </history>
    private void grdNew_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
      objWhsMovs _objWhsMov = e.Row.DataContext as objWhsMovs;

      // Obtenemos la columna actual
      DataGridCellInfo cell = grdNew.CurrentCell;

      switch (cell.Column.SortMemberPath)
      {
        case "wmgi":
          if (_objWhsMov != null && _objWhsMov.wmQty == 0)
          {
            UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");

            GridHelper.SelectRow(grdNew, grdNew.SelectedIndex, blnEdit: true);
          }
          break;
      }
    }
    #endregion
  }
}
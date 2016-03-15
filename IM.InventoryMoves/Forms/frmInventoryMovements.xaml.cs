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
using IM.InventoryMovements.Clases;
using IM.Model;
using IM.Model.Classes;
using IM.BusinessRules.BR;
using IM.BusinessRules.Entities;
using IM.Base.Forms;
using System.Data.Entity.SqlServer;
using System.ComponentModel;

namespace IM.InventoryMovements
{
  /// <summary>
  /// Interaction logic for frmInventoryMovements.xaml
  /// </summary>
  public partial class frmInventoryMovements : Window
  {
    #region Atributos

    private DateTime? _dtmcurrent = null; //Variable para manejar un problema del control DatePicker
    private List<objWhsMovs> _lstobjWhsMovs = null;//Lista para nuevos registros de WhsMovs
    private SalesRoomCloseDates _salesRoom = new SalesRoomCloseDates();
    private DateTime _dtmServerdate = new DateTime();
    CollectionViewSource objWhsMovsViewSource;
    CollectionViewSource getGiftsViewSource;
    CollectionViewSource whsMovViewSource;
    #endregion

    #region Constructores y destructores

    /// <summary>
    /// Constructor
    /// </summary>
    /// <history>
    /// [wtorres]  14/Mar/2016 Modified. Elimine el parametro userData
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
      if (e.Key == Key.Capital)
      {
        CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }

    /// <summary>
    /// Aumenta o decrementa los dias del control de fecha.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Feb/2016 Created
    /// </history>
    //private void upd_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
    //{
    //switch (e.ScrollEventType)
    //{
    //  case System.Windows.Controls.Primitives.ScrollEventType.SmallIncrement:
    //    dtpDate.SelectedDate = dtpDate.SelectedDate.Value.AddDays(-1);
    //    break;
    //  case System.Windows.Controls.Primitives.ScrollEventType.SmallDecrement:
    //    dtpDate.SelectedDate = dtpDate.SelectedDate.Value.AddDays(1);
    //    break;
    //}
    //}

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
      if (!this.IsInitialized) return;

      if (_dtmcurrent != dtpDate.Value.Value)
      {
        whsMovViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("whsMovViewSource")));
        // Load data by setting the CollectionViewSource.Source property:
        whsMovViewSource.Source = BRWarehouseMovements.GetWarehouseMovements(App.User.Warehouse.whID, dtpDate.Value.Value);
        StatusBarReg.Content = string.Format("{0}/{1}", grd.SelectedItems.Count, whsMovViewSource.View.SourceCollection.Cast<WarehouseMovementShort>().Count());
      }
      _dtmcurrent = dtpDate.Value.Value;
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
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateCurrentDate())
      {
        if (_lstobjWhsMovs.Count > 0)
        {
          if (!_lstobjWhsMovs.Any(c => c.wmQty == 0 || c.wmgi == null || c.wmgi == ""))
          {
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
            BRWarehouseMovements.SaveWarehouseMovements(ref lstWhsMov);
            InicializarGrdNew();
            grd.ItemsSource = BRWarehouseMovements.GetWarehouseMovements(App.User.Warehouse.whID, dtpDate.Value.Value);
          }
        }
      }
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
      StatusBarReg.Content = string.Format("{0}/{1}", grd.SelectedItems.Count, whsMovViewSource.View.SourceCollection.Cast<WarehouseMovementShort>().Count());
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
    /// </history>
    private void InicializarGrdNew()
    {
      _lstobjWhsMovs = new List<objWhsMovs>();
      objWhsMovsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("objWhsMovsViewSource")));
      // Load data by setting the CollectionViewSource.Source property:
      objWhsMovsViewSource.Source = _lstobjWhsMovs;
      getGiftsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("getGiftsViewSource")));
      // Load data by setting the CollectionViewSource.Source property:
      getGiftsViewSource.Source = BRGifts.GetGifts(App.User.Warehouse.whID, 1);
    }
    #endregion
  }
}
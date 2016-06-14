using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host;
using IM.Host.Forms;
using IM.Model.Enums;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchSelfGen.xaml
  /// </summary>
  public partial class frmSearchSelfGen : Window
  {

    #region Contructores
    public frmSearchSelfGen()
    {
      InitializeComponent();

      // Fecha inicial 
      dtpStart.Value = new DateTime(frmHost._dtpServerDate.Year, frmHost._dtpServerDate.Month, 1);

      // Fecha Final
      dtpEnd.Value = frmHost._dtpServerDate;
    }
    #endregion

    #region StatusBar

    #region Window_KeyDown
    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [vipacheco] 03/Junio/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
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
    #endregion

    #region CkeckKeysPress
    /// <summary>
    /// Verifica que la tecla se encuentre activa/inactiva, para cambiar el estilo de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [vipacheco] 03/Junio/2016 Created
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
    #endregion

    #region KeyDefault
    /// <summary>
    /// Configuracion inicial de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [vipacheco] 03/Junio/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }
    #endregion

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [vipacheco] 03/Junio/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #endregion

    #region optPending_Initialized
    /// <summary>
    /// Inicializa el radio button de la opcion 3
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param> 
    /// <history>
    /// [vipacheco] 04/Junio/2016 Created
    /// </history>
    private void optPending_Initialized(object sender, EventArgs e)
    {
      RadioButton _optPending = (RadioButton)sender;
      _optPending.IsChecked = true;
    }
    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Busca los invitados que cumplan los criterios de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <historty>
    /// [vipacheco] 04/Junio/2016 Created
    /// </historty>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateCriteria())
      {
        Load_Grid();
      }
    }
    #endregion

    #region Load_Grid
    /// <summary>
    /// Carga los resultados encontrados en el grid
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private async void Load_Grid()
    {
      EnumCaseSelfGen Option = EnumCaseSelfGen.PendingByClassifying;

      // Verificamos el radio button
      if (optAll.IsChecked.Value)
        Option = EnumCaseSelfGen.All;
      else if (optClassified.IsChecked.Value)
        Option = EnumCaseSelfGen.Classified;

      string GuestID = string.IsNullOrEmpty(txtguID.Text) ? "" : txtguID.Text;
      string RoomNum = string.IsNullOrEmpty(txtRoomNum.Text) ? "" : txtRoomNum.Text;
      string Liner = string.IsNullOrEmpty(txtLiner.Text) ? "" : txtLiner.Text;

      grdGuest.ItemsSource = await  BRGuests.GetSelfGen(txtGuestName.Text, App.User.SalesRoom.srID, dtpStart.Value.Value.Date, dtpEnd.Value.Value.Date, GuestID, Option, RoomNum, Liner);
    }
    #endregion

    #region ValidateCriteria
    /// <summary>
    /// Valida los criterios de busqueda
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 04/Junio/2016 Created
    /// </history>
    private bool ValidateCriteria()
    {
      #region Fechas
      // validamos la fecha inicial
      if (dtpStart.Value.Value == null)
      {
        UIHelper.ShowMessage("Specify the Start Date.", MessageBoxImage.Information, "Self Gen");
        return false;
      }
      // validamos la fecha final
      else if (dtpEnd.Value.Value == null)
      {
        UIHelper.ShowMessage("Specify the End Date.", MessageBoxImage.Information, "Self Gen");
        return false;
      }
      // validamos que no se traslapen las fechas
      else if (dtpStart.Value.Value > dtpEnd.Value.Value)
      {
        UIHelper.ShowMessage("Start Date can not be greater than End Date.", MessageBoxImage.Information, "Self Gen");
        return false;
      }
      #endregion

      return true;
    }
    #endregion

    #region grdGuest_SelectionChanged
    /// <summary>
    /// Funcion que se encarga de validar el total de datos obtenidos en el datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/03/2016 Created
    /// </history>
    private void grdGuest_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (grdGuest.Items.Count == 0)
      {
        StatusBarReg.Content = "0 Guests";
        return;
      }
      StatusBarReg.Content = $"{grdGuest.Items.Count} Guests";
    }

    #endregion

    #region grdGuest_DoubleClick
    /// <summary>
    /// Despliega el formulario de show del invitado seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void grdGuest_DoubleClick(object sender, RoutedEventArgs e)
    {
      if (grdGuest.Items.Count < 1)
        UIHelper.ShowMessage("Select at least one Guest.", MessageBoxImage.Information, "Search Self Gen");
      else if (!App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard))
        UIHelper.ShowMessage("Access denied. You have read access.", MessageBoxImage.Information, "Search Self Gen");
      else
      {
        var Selected_Row = grdGuest.SelectedItem;

        if (Selected_Row != null)
        {
          Type type = Selected_Row.GetType();
          var property = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();

          int GuestID = 0;
          if (property.Count > 0)
          {
            GuestID = (int)type.GetProperty("guID").GetValue(Selected_Row, null);
          }

          frmShow _frmShow = new frmShow(GuestID);
          _frmShow.ShowInTaskbar = false;
          _frmShow.Owner = this;
          _frmShow.ShowDialog();
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
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            grdGuest_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }

      e.Handled = blnHandled;
    }
    #endregion

  }
}

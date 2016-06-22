using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;


namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmCxCAuthorization.xaml
  /// </summary>
  public partial class frmCxCAuthorization : Window
  {
    #region Propiedades, Atributos

    public ExecuteCommandHelper LoadComboPR { get; set; }
    public ExecuteCommandHelper LoadComboLS { get; set; }
    public static DateTime _dtpServerDate = DateTime.Now;
    List<CxCData> lstCxCData; // lista de CxC
    
    //List<UnderPaymentMotive> lstUnderPaymentMotive = new List<UnderPaymentMotive>();
    bool blnFilterAuthorized = false; // bandera para determinar el tab en el que se encuentra cargado el datagrid 
    bool blnDatePiker = false; // bandera para determinar si el datepiker esta siendo utilizado
    bool blnOpenCalendar = false; // bandera para determinar si el datepiker fue pulsado
    DateTime dtmFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);// Fecha inicial seleccionada en el datepiker
    DateTime dtmTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).AddDays(-1); // Fecha final seleccionada en el datepiker 
    private DateTime? _dtmClose = null; // Fecha de corte 
    string strSalesRoom = App.User.SalesRoom.srID;// Sale Room del usuario logueago
    string strUserID = App.User.User.peID; // ID del usuario logueado
    string strUserName = App.User.User.peN; // Nombre del usuario logueado
    string strLeadSource = "ALL"; // Leadsource para la busqueda del CxC
    string strPR = "ALL"; // Personel para combobox y busqueda de CxC
    CollectionViewSource CxCDataViewSource; // Coleccion para el datagrid de  los CxC
    CollectionViewSource underPaymentMotiveViewSource; // Coleccion para llenar el combobox de Motivos
    int firtPage = 1; // primera pagina
    int lastPage = 0; // ultima pagina
    int minPage = 1; // pagina actual del Paginador
    int totalRows = 0; // total de registros
    #endregion

    #region Constructores y Destructores
    public frmCxCAuthorization()
    {
      InitializeComponent();
      dtpkFrom.SelectedDate = dtmFrom;
      dtpkTo.SelectedDate = dtmTo;
      _dtpServerDate = BRHelpers.GetServerDate();
      LoadComboPR = new ExecuteCommandHelper(x => LoadPersonnel());
      CxCDataViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cxCDataViewSource")));
      LoadAtributes();
      underPaymentMotiveViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("underPaymentMotiveViewSource")));
    }
    #endregion

    #region setNewUserLogin
    /// <summary>
    /// Este metodo se encarga de validar y actualizar los permisos del usuario logeado sobre el sistema
    /// </summary>
    /// <history>
    /// [michan] 9/Junio/2016 Created
    /// </history>
    public async Task<int> setNewUserLogin()
    {
      var index = 0;
      if (ComboBoxPermision(cbxPersonnel, EnumPermission.PRInvitations, EnumPermisionLevel.Special))
      {
        await Task.Run(() =>
        {
          var lstPS = cbxPersonnel.ItemsSource as List<PersonnelShort>;
          index = lstPS.FindIndex(x => x.peID.Equals(App.User.User.peID));
        });
        selectInCombobox(cbxPersonnel, index: index);
      }

      return index;

    }
    #endregion

    #region selectInCombobox
    /// <summary>
    /// Busca en una lista y selecciona al personal
    /// </summary>
    /// <param name="cbx">Combobox</param>
    /// <param name="index">Item a seleccionar</param>
    /// <history>
    /// [michan] 9/ Junio /2016 Created
    /// </history>
    private void selectInCombobox(ComboBox cbx, int? index = -1)
    {
      cbx.SelectedIndex = index.Value != -1 ? index.Value : 0;
    }
    #endregion

    #region LoadPersonnel
    /// <summary>
    /// Carga personal en el combobox
    /// </summary>
    /// <history>
    /// [michan] 01/06/2016  Created
    /// </history>
    public void LoadPersonnel()
    {
      GetPersonnel(Model.Helpers.EnumToListHelper.GetEnumDescription(EnumRole.PR));
    }
    #endregion

    #region GetPersonnel
    /// <summary>
    /// Obtiene la lista del personal
    /// </summary>
    /// <param name="leadSources">filtro leadsources</param>
    /// <param name="roles">rol del usuario loggeado</param>
    /// <history>
    /// [michan] 01/06/2016 Created
    /// </history>
    public async void GetPersonnel(string roles)
    {
      try
      {
        var data = await BRPersonnel.GetPersonnel(roles: roles);
        if (data.Count > 0)
        {
          data.Insert(0, new PersonnelShort() { peID = "ALL", peN = "ALL", deN = "ALL" });
          cbxPersonnel.ItemsSource = data;
        }
        await setNewUserLogin();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.InnerException.Message, MessageBoxImage.Error);
      }
    }
    #endregion

    #region LoadLeadSource
    /// <summary>
    /// Carga los leadsource al combobox
    /// </summary>
    /// <history>
    /// [michan] 01/06/2016  Created
    /// </history>
    public void LoadLeadSource()
    {
      GetLeadSources();
    }
    #endregion

    #region ComboBoxPermision
    /// <summary>
    /// Valida el permiso para mostrar el combobox
    /// </summary>
    /// <param name="cbx">Combobox a validar</param>
    /// <param name="enumPermission">Permiso</param>
    /// <param name="enumPermisionLevel">Nivel de permiso</param>
    /// <returns></returns>
    /// <history>
    /// [michan] 04/06/2016 Created
    /// </history>
    public bool ComboBoxPermision(ComboBox cbx, EnumPermission enumPermission, EnumPermisionLevel enumPermisionLevel)
    {

      //Validamos permisos y restricciones para el combobox
      if (App.User.HasPermission(enumPermission, enumPermisionLevel))
      {
        cbx.IsEnabled = true;
        if (cbxLeadSource.Items.Count > 0)
        {
          return true;
        }
        else
        {
          cbx.Text = "No data found - Press Ctrl+F5 to load Data";
          return false;
        }
      }
      else
      {
        cbx.IsEnabled = false;
        if (cbx.Items.Count > 0)
        {
          return true;
        }
        else
        {
          cbx.Text = "No data found - Press Ctrl+F5 to load Data";
          return false;
        }
      }
    }
    #endregion

    #region GetLeadSources
    public async void GetLeadSources()
    {
      try
      {
        var data = await BRLeadSources.GetLeadSourcesByUser(user: App.User.User.peID);
        if (data.Count > 0)
        {

          data.Insert(0, new LeadSourceByUser() { lsID = "ALL", lsN = "ALL", lspg = "ALL" });
          cbxLeadSource.ItemsSource = data;
          cbxLeadSource.IsEnabled = true;
        }
        if (ComboBoxPermision(cbxLeadSource, EnumPermission.PRInvitations, EnumPermisionLevel.Special))
        {
          selectInCombobox(cbxLeadSource);
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.InnerException.Message, MessageBoxImage.Error);
      }
    }
    #endregion

    #region LoadAtributes
    /// <summary>
    /// Carga los parametros para las validaciones y carga de CxC Authorized
    /// </summary>
    /// <history>
    /// [michan] 03/06/2016 Created
    /// </history>
    public void LoadAtributes()
    {
      dtmFrom = dtpkFrom.SelectedDate.Value;
      dtmTo = dtpkTo.SelectedDate.Value;
      strSalesRoom = App.User.SalesRoom.srID;
      strUserID = App.User.User.peID;
      var personnelShort = cbxPersonnel.SelectedValue as PersonnelShort;
      var leadSource = cbxLeadSource.SelectedValue as LeadSourceByUser;
      strPR = (personnelShort != null) ? personnelShort.peID : "ALL";
      strLeadSource = (leadSource != null) ? leadSource.lsID : "ALL";
      _dtmClose = BRSalesRooms.GetCloseSalesRoom(EnumSalesRoomType.CxC, strSalesRoom);
    }
    #endregion

    #region GetCxCAuthorized
    /// <summary>
    /// Obtiene los CxC Authorized
    /// </summary>
    /// <param name="blnAuthorized">Tipo de CxCAuthorized </param>
    /// <history>
    /// [michan] 06/06/2016 Created
    /// </history>
    public async void GetCxCAuthorized(bool? blnAuthorized = false)
    {
      lstCxCData = await BRCxC.GetCxC(blnAuthorized.Value, strSalesRoom, strUserID, dtmFrom, dtmTo, strLeadSource, strPR);
      totalRows = lstCxCData.Count();
      lastPage = (totalRows > 0) ? (int)lstCxCData.Max(pagina => pagina.Page.Value) : 1;
      firtPage = (totalRows > 0) ? (int)lstCxCData.Min(pagina => pagina.Page.Value) : 1;
      ConfigButtons();
    }
    #endregion

    #region ConfigButtons
    /// <summary>
    /// Configura los botones de la ventana
    /// </summary>
    /// <history>
    /// [michan] 07/06/2016 Created
    /// </history>
    public void ConfigButtons()
    {
      bool blnFP = (minPage == 1) ? false : true;
      bool blnNL = (lastPage == minPage) ? false : true;
      btnFrist.IsEnabled = blnFP;
      btnPrev.IsEnabled = blnFP;
      btnNext.IsEnabled = blnNL;
      btnLast.IsEnabled = blnNL;
      btnFrist.Visibility = btnNext.Visibility = btnPrev.Visibility = btnLast.Visibility = (lastPage == firtPage) ? Visibility.Hidden : Visibility.Visible;
      var CxCFilter = lstCxCData.Where(cxc => cxc.Page.Value == minPage).ToList();
      CxCDataViewSource.Source = null;
      CxCDataViewSource.Source = CxCFilter;
      cxCDataDataGrid.Items.Refresh();
      var totalCollumns = cxCDataDataGrid.Items.Count;
      var from = (totalRows > 100 && minPage > 1) ? (100 * (minPage)) + 1 : 1;
      var to = (totalRows > 100 && minPage > 1) ? (100 * minPage) + totalCollumns : (totalCollumns * minPage);
      lblPagText.Content = $"{from} / {to} of {totalRows}";
    }
    #endregion

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadPersonnel();
      GetLeadSources();
      underPaymentMotiveViewSource.Source = BRUnderPaymentMotives.getUnderPaymentMotives(1);

    }
    #endregion

    #region AuthorizedBox_Click
    /// <summary>
    /// Accion para el manejo de las autorzaciones asi como desautorizar los CxC
    /// </summary>
    /// <history>
    /// [michan] 09/06/2016 Created
    /// </history>
    private void AuthorizedBox_Click(object sender, RoutedEventArgs e)
    {
      var item = cxCDataDataGrid.SelectedItem as CxCData;
      var columns = cxCDataDataGrid.CurrentCell.Column.DisplayIndex;

      decimal convertToDecimal = 0;
      if (item.Authorized.Value)
      {
        var defaultValue = String.Format("{0:0.00}", item.CxC);
        InputBox _inputBox = new InputBox("Amount to pay", "Insert the amount to pay", defaultvalue: defaultValue, isString: false, maxLength: (double)item.CxC);
        if (_inputBox.ShowDialog() == true)
        {
          decimal amountPay = (Decimal.TryParse(_inputBox.Input.Text, out convertToDecimal)) ? convertToDecimal : 0;
          item.grCxCAppD = _dtpServerDate.Date;
          item.grAuthorizedBy = strUserID;
          item.grAuthorizedName = strUserName;
          item.grAmountToPay = amountPay;
          item.grBalance = amountPay;
        }
        else
        {
          item.Authorized = false;
        }
        _inputBox.Close();
      }
      else
      {
        item.Authorized = false;
        item.grCxCAppD = null;
        item.grAuthorizedBy = "";
        item.grAuthorizedName = "";
        item.grup = null;
        item.grcxcAuthComments = "";
        item.grAmountToPay = 0;
        item.grBalance = 0;
      }
      cxCDataDataGrid.CommitEdit();
      cxCDataDataGrid.CancelEdit();
      cxCDataDataGrid.Items.Refresh();
      //lstCxCData = cxCDataDataGrid.ItemsSource as List<CxCData>;
      //var ert = cxCDataDataGrid.GetCell().IsEnabled = false;
    }
    #endregion

    #region CxCAppD_SelectedDateChanged
    /// <summary>
    /// Valida la fecha seleccionada en el datepiker
    /// </summary>
    /// <history>
    /// [michan] 13/06/2016 Created
    /// </history>
    private void CxCAppD_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {

      if (blnDatePiker && blnOpenCalendar)
      {
        var item = cxCDataDataGrid.SelectedItem as CxCData;
        var dtp = sender as DatePicker;

        if (item.grCxCAppD != null)
        {
          // validamos que la fecha de autorizacion no sea despues de hoy
          if (item.grCxCAppD.Value.Date <= _dtpServerDate.Date)
          {
            // validamos que la fecha de autorizacion no sea antes de la fecha de cierre de CxC de la sala
            if (item.grCxCAppD.Value.Date > _dtmClose.Value.Date)
            {
              if (UIHelper.ShowMessage("To apply the changes you have made?", MessageBoxImage.Question, "CxC Autorizathion") == MessageBoxResult.Yes)
              {
                cxCDataDataGrid.CommitEdit();
                cxCDataDataGrid.CancelEdit();
                cxCDataDataGrid.Items.Refresh();
                blnDatePiker = false;
                blnOpenCalendar = false;
                UIHelper.ShowMessage("Saving Process Completed.", MessageBoxImage.Information, "CxC Application");
              }
            }
            else
            {
              UIHelper.ShowMessage("CxC Application Date can not be smaller than last CxC close date.", MessageBoxImage.Error, "CxC Application");
              e.Handled = true;
              dtp.IsDropDownOpen = true;
            }
          }
          else
          {
            UIHelper.ShowMessage("CxC Application Date can not be greater than today.", MessageBoxImage.Error, "CxC Application");
            e.Handled = true;
            dtp.IsDropDownOpen = true;
          }
        }
        else
        {
          UIHelper.ShowMessage("Selected one date.", MessageBoxImage.Error, "CxC Application");
          e.Handled = true;
          dtp.IsDropDownOpen = true;
        }
      }
      blnDatePiker = true;

    }
    #endregion

    #region CxCAppD_CalendarOpened
    /// <summary>
    /// Evento de datepiker para avilitar la validacion de la fecha a seleccionar
    /// </summary>
    /// <history>
    /// [michan] 13/06/2016 Created
    /// </history>
    private void CxCAppD_CalendarOpened(object sender, RoutedEventArgs e)
    {
      var calendar = sender as DatePicker;
      calendar.DisplayDateStart = new DateTime(_dtmClose.Value.Year, _dtmClose.Value.Month, _dtmClose.Value.Day).AddDays(+1);
      calendar.DisplayDateEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
      blnOpenCalendar = true;
    }
    #endregion

    #region Authorized SelectionChanged
    /// <summary>
    /// Evento del Tabcontrol para determinar la informacion a mostrar en el grid Autorizados y no autorizados
    /// </summary>
    /// <history>
    /// [michan] 08/06/2016 Created
    /// </history>
    private void Authorized_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // ... Get TabControl reference.
      switch (((TabControl)sender).SelectedIndex)
      {
        case 0:
          blnFilterAuthorized = false;
          break;
        case 1:
          blnFilterAuthorized = true;
          break;
        default:
          break;
      }
      ColumnVisibility("Log", blnFilterAuthorized);
      ColumnVisibility("Pay", blnFilterAuthorized);
      GetCxCAuthorized(blnFilterAuthorized);
    }
    #endregion

    #region ColumnVisibility
    /// <summary>
    /// Metodo para ocultar y mostrar las columnas Pay y Log
    /// </summary>
    /// <param name="header">Header de la columna</param>
    /// <param name="visibility">True - Visible | False - Hidden</param>
    /// <history>
    /// [michan] 16/06/2016 Created
    /// </history>
    public void ColumnVisibility(string header, bool visibility)
    {
      cxCDataDataGrid.Columns.SingleOrDefault(c => c.Header.ToString() == header).Visibility = (visibility) ? Visibility.Visible : Visibility.Hidden;
    }
    #endregion

    #region SaveGiftsReceipts
    /// <summary>
    /// Metodo para guardar los CxC Autorizados y Desautorizados
    /// </summary>
    /// <history>
    /// [michan] 18/06/2016 Created
    /// </history>
    public async void SaveGiftsReceipts()
    {
      foreach (CxCData cxcData in cxCDataDataGrid.Items)
      {
        if (cxcData.Authorized.Value == !blnFilterAuthorized)
        {
          GiftsReceipt _giftsReceipt = SetCxCDataToGiftsReceipt(cxcData);
          int nRes = await BREntities.OperationEntity(_giftsReceipt, EnumMode.edit);
          await SaveGiftsReceiptsLog(_giftsReceipt.grID, strUserID);
        }
      }
      GetCxCAuthorized(blnFilterAuthorized);
      UIHelper.ShowMessage("Saving Process Completed.", MessageBoxImage.Information, "CxC Authorized");
    }
    #endregion

    #region SetCxCDataToGiftsReceipt
    /// <summary>
    /// Obtiene el Gift y envie los datos modificados en el grid
    /// </summary>
    /// <param name="cxcData">Registro del grid modificado</param>
    /// <returns></returns>
    /// <history>
    /// [michan] 17/06/2016 Created
    /// </history>
    public GiftsReceipt SetCxCDataToGiftsReceipt(CxCData cxcData)
    {
      GiftsReceipt giftsReceipt = BRGiftsReceipts.GetGiftReceipt(cxcData.grID);
      //var cxcGR = lstCxCData.SingleOrDefault(cxc => cxc.grID == giftsReceipt.grID);
      giftsReceipt.grCxCAppD = cxcData.grCxCAppD;
      giftsReceipt.grAuthorizedBy = (String.IsNullOrEmpty(cxcData.grAuthorizedBy)) ? null : cxcData.grAuthorizedBy;
      giftsReceipt.grAmountToPay = cxcData.grAmountToPay;
      giftsReceipt.grup = cxcData.grup;
      giftsReceipt.grcxcAuthComments = cxcData.grcxcAuthComments;
      giftsReceipt.grBalance = cxcData.grBalance;
      return giftsReceipt;
    }
    #endregion

    #region SaveGiftsReceiptsLog
    /// <summary>
    /// Guarda el historico de recibos de regalos
    /// </summary>
    /// <param name="intReceiptID">Clave del recibo de regalos</param>
    /// <param name="strChangedBy">Clave del usuario que esta haciendo el cambio</param>
    /// <returns></returns>
    /// <history>
    /// [michan] 18/06/2016 Created
    /// </history>
    public async Task SaveGiftsReceiptsLog(int intReceiptID, string strChangedBy)
    {
      await BRGiftsReceiptLog.SaveGiftsReceiptsLog(intReceiptID, strChangedBy);
    }
    #endregion 

    #region Button Click
    /// <summary>
    /// Metodo que controla la accion de los botones de Pay y Log en el datagrid
    /// </summary>
    /// <history>
    /// [michan] 20/06/2016 Created
    /// </history>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var item = cxCDataDataGrid.SelectedItem as CxCData;
      switch (((Button)sender).Name)
      {
        case "btnPay":
          frmCxCPayments _frmCxCPayments = new frmCxCPayments(item.grID);
          _frmCxCPayments.ShowInTaskbar = false;
          _frmCxCPayments.ShowDialog();
          break;
        case "btnLog":
          frmGiftsReceiptsLog _frmGiftsReceiptsLog = new frmGiftsReceiptsLog(item.grID);
          _frmGiftsReceiptsLog.ShowInTaskbar = false;
          _frmGiftsReceiptsLog.ShowDialog();
          break;
        default:
          break;
      }
     
    }
    #endregion

    #region Acción de Botones de Paginador
    /// <summary>
    /// Metodo que controla la accion de los botones del paginador.
    /// </summary>
    /// <history>
    /// [michan] 18/06/2016 Created
    /// </history>
    private void btn_Click(object sender, RoutedEventArgs e)
    {
      switch (((Button)sender).Name)
      {
        case "btnFrist":
          minPage = 1;
          break;
        case "btnPrev":
          minPage = (minPage > 1) ? minPage - 1 : 1;
          break;
        case "btnNext":
          minPage = (minPage < lastPage) ? minPage + 1 : lastPage;
          break;
        case "btnLast":
          minPage = lastPage;
          break;
        default:
          break;
      }
      ConfigButtons();
    }
    #endregion

    #region Acción de Botones
    /// <summary>
    /// Metodos que controla lacciones de los botones de buscar, guardar y salir
    /// </summary>
    /// <history>
    /// [michan] 14/06/2016 Created
    /// </history>
    private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      switch (((Border)sender).Name)
      {
        case "imgButtonExit":
          Close();
          break;
        case "imgButtonSave":
          SaveGiftsReceipts();
          break;
        case "imgButtonSearch":
          LoadAtributes();
          GetCxCAuthorized(blnFilterAuthorized);
          break;
        default:
          break;
      }
    }
    #endregion

    #region AuthComments LostFocus
    /// <summary>
    /// Metodo para rellenar el Texbox de comentarios
    /// </summary>
    /// <history>
    /// [michan] 20/06/2016 Created
    /// </history>
    private void AuthComments_LostFocus(object sender, RoutedEventArgs e)
    {
      var item = cxCDataDataGrid.SelectedItem as CxCData;
      var txt = sender as TextBox;
      item.grcxcAuthComments = txt.Text;
    }
    #endregion

    #region AuthComments Loaded
    /// <summary>
    /// Metodo que se ejecuta despues de que el Texbox de comentarios ha finalizado
    /// </summary>
    /// <history>
    /// [michan] 21/06/2016 Created
    /// </history>
    private void AuthComments_Loaded(object sender, RoutedEventArgs e)
    {
      var txt = sender as TextBox;
      txt.Focus();
    }
    #endregion

  }
}

using IM.Base.Classes;
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
using System.Windows.Input;

namespace IM.Host.Forms
{

  /// <summary>
  /// Interaction logic for frmCxCAuthorization.xaml
  /// </summary>
  public partial class frmCxCAuthorization : Window
  {
    #region Propiedades, Atributos

    public ExecuteCommandHelper LoadCombo { get; set; }
    public ExecuteCommandHelper LoadComboLS { get; set; }
    public static DateTime _dtpServerDate = DateTime.Today;
    List<CxCData> lstCxCData; // lista de CxC
    //List<UnderPaymentMotive> lstUnderPaymentMotive = new List<UnderPaymentMotive>();
    bool blnFilterAuthorized = false; // bandera para determinar el tab en el que se encuentra cargado el datagrid 
    bool blnDatePiker = false; // bandera para determinar si el datepiker esta siendo utilizado
    bool blnOpenCalendar = false; // bandera para determinar si el datepiker fue pulsado
   

    DateTime dtmFrom = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);// Fecha inicial seleccionada en el datepiker
    DateTime dtmTo = DateTime.Today; // Fecha final seleccionada en el datepiker 
    private DateTime? _dtmClose = null; // Fecha de corte 
    string strSalesRoom = Context.User.SalesRoom.srID;// Sale Room del usuario logueago
    string strUserID = Context.User.User.peID; // ID del usuario logueado
    string strUserName = Context.User.User.peN; // Nombre del usuario logueado
    string strLeadSource = "ALL"; // Leadsource para la busqueda del CxC
    string strPR = "ALL"; // Personel para combobox y busqueda de CxC
    CollectionViewSource CxCDataViewSource; // Coleccion para el datagrid de  los CxC
    
    CollectionViewSource underPaymentMotiveViewSource; // Coleccion para llenar el combobox de Motivos
    List<UnderPaymentMotive> lstUnderPaymentMotive = new List<UnderPaymentMotive>();// Lista de Motivos

    int iTotalchanges = 0; // Total de cambios
    
    int firtPage = 1; // primera pagina
    int lastPage = 0; // ultima pagina
    int minPage = 1; // pagina actual del Paginador
    int totalRows = 0; // total de registros
    #endregion

    #region Constructores y Destructores
    public frmCxCAuthorization()
    {
      InitializeComponent();
      
      dtpkFrom.Value = dtmFrom;
      dtpkTo.Value = dtmTo;
      _dtpServerDate = BRHelpers.GetServerDate();
      LoadCombo = new ExecuteCommandHelper(x => LoadPersonnel());
      CxCDataViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cxCDataViewSource")));
      LoadAtributes();
      underPaymentMotiveViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("underPaymentMotiveViewSource")));
      if (!Context.User.HasPermission(EnumPermission.CxCAuthorization, EnumPermisionLevel.Standard))
      {
        imgButtonSave.IsEnabled = false;
        dtgCxC.Columns.SingleOrDefault(c => c.Header.ToString() == "Auth.").IsReadOnly = true;
      }
    }
    #endregion

    #region setNewUserLogin
    /// <summary>
    /// Este metodo se encarga de validar y actualizar los permisos del usuario logeado sobre el sistema
    /// </summary>
    /// <history>
    /// [michan] 9/Junio/2016 Created
    /// </history>
    public async Task setNewUserLogin()
    {
      var index = 0;
      if (ComboBoxPermision(cbxPersonnel, EnumPermission.CxCAuthorization, EnumPermisionLevel.ReadOnly))
      {
        await Task.Run(() =>
        {
          var lstPS = cbxPersonnel.ItemsSource as List<PersonnelShort>;
          index = lstPS.FindIndex(x => x.peID.Equals(Context.User.User.peID));
        });
        selectInCombobox(cbxPersonnel, index: index);
      }
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
      StaStart("Loading personnel...");
      DoGetPersonnel("PR");
    }
    #endregion

    #region DoGetPersonnel
    /// <summary>
    /// Obtiene la lista del personal
    /// </summary>
    /// <param name="roles">rol del usuario loggeado</param>
    /// <history>
    /// [michan] 01/06/2016 Created
    /// </history>
    public async void DoGetPersonnel(string roles)
    {
      try
      {
        var data = await BRPersonnel.GetPersonnel(roles: roles);
        if (data.Count > 0)
        {
          data.Insert(0, new PersonnelShort() { peID = "ALL", peN = "ALL", deN = "ALL" });
          cbxPersonnel.ItemsSource = data;
        }
        SetNewUserLogin();
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex.InnerException.Message, MessageBoxImage.Error);
      }
    }
    #endregion

    #region SetNewUserLogin
    /// <summary>
    /// Este metodo se encarga de validar y actualizar los permisos del usuario logeado sobre el sistema
    /// </summary>
    /// <history>
    /// [michan] 9/Junio/2016 Created
    /// </history>
    public void SetNewUserLogin()
    {
      //Agregamos la informacion del usuario en la interfaz
      
      cbxPersonnel.IsEnabled = true;
      if (cbxPersonnel.Items.Count > 0)
      {
        cbxPersonnel.SelectedIndex = 0;
        selectPersonnelInCombobox(Context.User.User.peID);
      }
      else
      {
        cbxPersonnel.Text = "No data found - Press Ctrl+F5 to load Data";
      }
     
    }
    #endregion

    #region selectPersonnelInCombobox
    /// <summary>
    /// Busca en una lista y selecciona al personal
    /// </summary>
    /// <param name="user">ID del Usuario</param>
    /// <history>
    /// [michan] 9/ Junio /2016 Created
    /// </history>
    private void selectPersonnelInCombobox(string user)
    {
      var lstPS = cbxPersonnel.ItemsSource as List<PersonnelShort>;
      var index = lstPS.FindIndex(x => x.peID.Equals(user));
      if (index != -1)
      {
        cbxPersonnel.SelectedIndex = index;
      }
      else
      {
        cbxPersonnel.SelectedItem = 0;
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
      if (Context.User.HasPermission(enumPermission, enumPermisionLevel))
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
        var data = await BRLeadSources.GetLeadSourcesByUser(user: Context.User.User.peID);
        if (data.Count > 0)
        {

          data.Insert(0, new LeadSourceByUser() { lsID = "ALL", lsN = "ALL", lspg = "ALL" });
          cbxLeadSource.ItemsSource = data;
          cbxLeadSource.IsEnabled = true;
        }
        selectInCombobox(cbxLeadSource);
        
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
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
      
      dtmFrom = dtpkFrom.Value.Value.Date;
      dtmTo = dtpkTo.Value.Value.Date;
      strSalesRoom = Context.User.SalesRoom.srID;
      strUserID = Context.User.User.peID;
      var personnelShort = cbxPersonnel.SelectedValue as PersonnelShort;
      var leadSource = cbxLeadSource.SelectedValue as LeadSourceByUser;
      strPR = (personnelShort != null) ? personnelShort.peID : "ALL";
      strLeadSource = (leadSource != null) ? leadSource.lsID : "ALL";
      _dtmClose = BRSalesRooms.GetCloseSalesRoom(EnumEntities.CxC, strSalesRoom);
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
    public async Task GetCxCAuthorized(bool? blnAuthorized = false)
    {
      imgButtonSearch.IsEnabled = false;
      StaStart("Searching records... Please wait");
      lstCxCData = await BRCxC.GetCxC(blnAuthorized.Value, strSalesRoom, strUserID, dtmFrom, dtmTo, strLeadSource, strPR);
      totalRows = lstCxCData.Count();
      ColumnVisibility("Log", blnAuthorized.Value);
      ColumnVisibility("Pay", blnAuthorized.Value);
      lastPage = (totalRows > 0) ? (int)lstCxCData.Max(pagina => pagina.Page.Value) : 1;
      firtPage = (totalRows > 0) ? (int)lstCxCData.Min(pagina => pagina.Page.Value) : 1;
      ConfigButtons();
      StaEnd();
      iTotalchanges = 0;
      imgButtonSearch.IsEnabled = true;
      
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
      //var CxCFilter = lstCxCData.Where(cxc => cxc.Page.Value == minPage).ToList();
      CxCDataViewSource.Source = null;
      CxCDataViewSource.Source = lstCxCData.Where(cxc => cxc.Page.Value == minPage).ToList(); ;
      dtgCxC.Items.Refresh();
      var totalCollumns = dtgCxC.Items.Count;
      var from = (totalRows > 100 && minPage > 1) ? (100 * (minPage - 1)) + 1 : 1;
      var to = (totalRows > 100 && minPage > 1) ? (minPage != lastPage) ? (100 * minPage) : totalRows : (totalCollumns * minPage);
      lblPagText.Content = $"{from} / {to} of {totalRows}";
      StatusBarReg.Content = $"{minPage} / {lastPage}";
      
    }
    #endregion

    #region Window_Loaded
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadPersonnel();
      GetLeadSources();
      underPaymentMotiveViewSource.Source = lstUnderPaymentMotive = await BRUnderPaymentMotives.getUnderPaymentMotives(1);
      SetNewUserLogin();
      await GetCxCAuthorized();

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
      var item = dtgCxC.SelectedItem as CxCData;
            
      decimal convertToDecimal = 0;

      string convert = (item.grAmountPaid != null) ? item.grAmountPaid.Value.ToString() : "0";
      decimal amount = (Decimal.TryParse(convert, out convertToDecimal)) ? convertToDecimal : 0;
      if (amount > 0 && blnFilterAuthorized)
      {
        UIHelper.ShowMessage("the record can not be Unauthorized because there has been a paymen.", MessageBoxImage.Information, "CxC Authorized");
        item.Authorized = true;
      }
      else
      {
        if (item.Authorized.Value)
        {
          var defaultValue = String.Format("{0:0.00}", item.CxC);
          frmInputbox _inputBox = new frmInputbox("Amount to pay", "Insert the amount to pay", defaultvalue: defaultValue, isString: false, maxLength: (double)item.CxC);
          if (_inputBox.ShowDialog() == true)
          {
            decimal amountPay = (Decimal.TryParse(_inputBox.Input.Text, out convertToDecimal)) ? convertToDecimal : 0;
            item.grCxCAppD = _dtpServerDate;
            item.grAuthorizedBy = strUserID;
            item.grAuthorizedName = strUserName;
            item.grAmountToPay = amountPay;
            item.grBalance = amountPay;
            if ((lstUnderPaymentMotive.Count > 0) && (amountPay < item.CxC)) item.grup = lstUnderPaymentMotive.FirstOrDefault().upID;
            iTotalchanges++;
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
          iTotalchanges = (blnFilterAuthorized) ? iTotalchanges + 1 : iTotalchanges - 1;
        }
      }
      dtgCxC.CommitEdit();
      dtgCxC.CancelEdit();
      dtgCxC.Items.Refresh();

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
        var item = dtgCxC.SelectedItem as CxCData;
        var dtp = sender as DatePicker;

        if (item.grCxCAppD != null)
        {
          // validamos que la fecha de autorizacion no sea despues de hoy
          if (item.grCxCAppD.Value.Date <= _dtpServerDate)
          {
            // validamos que la fecha de autorizacion no sea antes de la fecha de cierre de CxC de la sala
            if (item.grCxCAppD.Value.Date > _dtmClose.Value.Date)
            {
              if (UIHelper.ShowMessage("To apply the changes you have made?", MessageBoxImage.Question, "CxC Autorizathion") == MessageBoxResult.Yes)
              {
                dtgCxC.CommitEdit();
                dtgCxC.CancelEdit();
                dtgCxC.Items.Refresh();
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
    private async void TabItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var newTab = FindFirstParent<TabItem>(sender as FrameworkElement);
      if (newTab != tbcAuthorized.SelectedItem)
      {
        e.Handled = true;
        var listCxCModifield = (dtgCxC.Items.SourceCollection).Cast<CxCData>().ToList();
        
        if (iTotalchanges > 0)
        {
          UIHelper.ShowMessage("You have pending changes to save.", MessageBoxImage.Information, "Unsaved changes");
        }
        else
        {
          tbcAuthorized.SelectedItem = newTab;
          blnFilterAuthorized = (tbcAuthorized.SelectedIndex == 1) ? true : false;
          
          await GetCxCAuthorized(blnFilterAuthorized);
        }
      }
    }
    #endregion

    #region FindFirstParent
    /// <summary>
    /// Metodo para recuperar el item a seleccionar.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="control"></param>
    /// <returns></returns>
    public static T FindFirstParent<T>(FrameworkElement control) where T : FrameworkElement
    {
      if (control == null)
        return null;

      if (control is T)
        return (T)control;

      return FindFirstParent<T>(control.Parent as FrameworkElement);
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
      dtgCxC.Columns.SingleOrDefault(c => c.Header.ToString() == header).Visibility = (visibility) ? Visibility.Visible : Visibility.Hidden;
    }
    #endregion

    #region SaveGiftsReceipts
    /// <summary>
    /// Metodo para guardar los CxC Autorizados y Desautorizados
    /// </summary>
    /// <history>
    /// [michan] 18/06/2016 Created
    /// </history>
    public async Task SaveGiftsReceipts()
    {
      imgButtonSave.IsEnabled = false;
      StaStart("Saving changes... Please wait");
      foreach (CxCData cxcData in dtgCxC.Items)
      {
        if (cxcData.Authorized.Value == !blnFilterAuthorized)
        {
          GiftsReceipt _giftsReceipt = await SetCxCDataToGiftsReceipt(cxcData);
          int nRes = await BREntities.OperationEntity(_giftsReceipt, EnumMode.Edit);
          await SaveGiftsReceiptsLog(_giftsReceipt.grID, strUserID);
        }
      }
      
      await GetCxCAuthorized(blnFilterAuthorized);
      
      UIHelper.ShowMessage("Saving Process Completed.", MessageBoxImage.Information, "CxC Authorized");
      StaEnd();
      imgButtonSave.IsEnabled = true;
      
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
    public async Task<GiftsReceipt> SetCxCDataToGiftsReceipt(CxCData cxcData)
    {
      GiftsReceipt giftsReceipt = await BRGiftsReceipts.GetGiftReceipt(cxcData.grID);
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
    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      var item = dtgCxC.SelectedItem as CxCData;
      var idexselected = dtgCxC.SelectedIndex;
      switch (((Button)sender).Name)
      {
        case "btnPay":
          if(item.grAmountToPay != null && item.grAmountToPay.Value > 0)
          {
            frmCxCPayments _frmCxCPayments = new frmCxCPayments(item.grID, item.grAmountToPay.Value, (item.grAmountPaid != null) ? item.grAmountPaid.Value : 0, (item.grBalance != null) ? item.grBalance.Value : 0);
            _frmCxCPayments.ShowInTaskbar = false;
            _frmCxCPayments.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //_frmCxCPayments.ShowDialog();
            if (_frmCxCPayments.ShowDialog() == true)
            {
              await GetCxCAuthorized(blnFilterAuthorized);
              GridHelper.SelectRow(dtgCxC, idexselected);
            };
          }
          else
          {
            UIHelper.ShowMessage("It is not valid amount to pay", MessageBoxImage.Error, "CxC Payments" );
          }
          break;
        case "btnLog":
          frmGiftsReceiptsLog _frmGiftsReceiptsLog = new frmGiftsReceiptsLog(item.grID);
          _frmGiftsReceiptsLog.ShowInTaskbar = false;
          _frmGiftsReceiptsLog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
    private async void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      switch (((Border)sender).Name)
      {
        case "imgButtonExit":
          Close();
          break;
        case "imgButtonSave":
          await SaveGiftsReceipts();
          break;
        case "imgButtonSearch":
          if (DateHelper.ValidateValueDate(dtpkFrom, dtpkTo))
          {
            LoadAtributes();
            await GetCxCAuthorized(blnFilterAuthorized);
          }
          break;
        default:
          break;
      }
    }
    #endregion


    #region Status
        
    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <history>
    /// [michan] 21/Julio/2016 Created
    /// </history>
    /// <param name="message">mensaje</param>
    private void StaStart(String message)
    {
        lblStatusBarMessage.Content = message;
        imgStatusBarMessage.Visibility = Visibility.Visible;
        this.Cursor = Cursors.Wait;
    }

    /// <summary>
    /// Indica en la barra de estado que se termina un proceso
    /// </summary>
    /// <history>
    /// [michan] 21/Julio/2016 Created
    /// </history>
    /// <param name="message">mensaje</param>
    private void StaEnd()
    {
        lblStatusBarMessage.Content = null;
        imgStatusBarMessage.Visibility = Visibility.Hidden;
        this.Cursor = null;
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/04/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region window keyDown
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
            break;
          }
        case Key.Insert:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
            break;
          }
        case Key.NumLock:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
            break;
          }
      }
    }
    #endregion

    
    private void dtgCxC_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var dg = sender as DataGrid;
      if (dg != null) StatusBarReg.Content = $"{dg.Items.CurrentPosition + 1}/{dg.Items.Count}";
    }
    
  }
  
}

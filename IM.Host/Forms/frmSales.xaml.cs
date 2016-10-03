using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.BusinessRules.BRIC;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

namespace IM.Host.Forms
{
  /// <summary>
  /// Formulario de Sales
  /// </summary>
  /// <history>
  /// [jorcanche]  created 05072016
  /// </history>
  public partial class frmSales : Window
  {
    //Id del huesped
    int _guId;

    //Fechas servidor y Fecha de Cierre de la venta
    DateTime _serverDate, _closeD;

    // Para indicar si se presiono el boton Undo
    private bool isPressedbtnUndo;

    // Indica si se esta cargando el detalle de una venta
    private bool _loading;   

    // Respaldamos el monto de la venta original
    private decimal _saleAmountOriginal;

    //El Monto de la venta
    private decimal _saleAmount;

    //Vendedores
    private List<SalesSalesman> _saleMen;

    //Objeto con los valores iniciales
    private Sale _saleOld = new Sale();

    //Objeto para llenar el formulario
    private Sale _saleNew = new Sale();

    //Payments
    private System.Collections.ObjectModel.ObservableCollection<Payment> _payments;

    //Sale Type Category 
    private string _saleTypeCategory;

    //Es una actualización de saled 
    private bool _isSaleUpdate;   

    //Listado de SalesSalesman de Intelligence Contracts
    private List<MemberSalesmen> _lstMemberSalesmens;   

    #region frmSales    

    public frmSales(EnumOpenBy openBy, int guId = 0)
    {
      InitializeComponent();     
      gprCriteria.Visibility = openBy == EnumOpenBy.Checkbox ? Visibility.Collapsed : Visibility.Visible;
      _guId = guId;
    }

    #endregion

    #region btnSalesmen_Click

    /// <summary>
    /// Muestra el grid que contiene la info de los Salesmen
    /// </summary>
    /// <history>
    /// [jorcanche]  created  29062016
    /// </history>
    private void btnSalesmen_Click(object sender, RoutedEventArgs e)
    {
      Panel.SetZIndex(gGeneral, 0);
    }

    #endregion

    #region btnGeneral_Click

    /// <summary>
    /// Muestra el grid que contiene la info General
    /// </summary>       
    private void btnGeneral_Click(object sender, RoutedEventArgs e)
    {
      Panel.SetZIndex(gGeneral, 1);
    }

    #endregion

    #region Window_Loaded

    /// <summary>
    /// Carga e inicializa las variables del formulario
    /// </summary>
    /// <history>
    /// [jorcanche]  created 29062016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {      
      UIHelper.SetUpControls(new Sale(), this);
      LoadCombosPayment();
      //Cargamos los ComboBox
      LoadPr();
      LoadLiner();
      LoadCloser();
      LoadExit();
      //Cargamos el DataGrid
      if (gprCriteria.Visibility == Visibility.Collapsed)
      {
        LoadGrid();
      }
      else
      {
        SetMode(EnumMode.ReadOnly);
      }

      //obtenemos la fecha de cierre
      CloseDate();
      _serverDate = BRHelpers.GetServerDate();
      //si el sistema esta en modo de solo lectura, no permitimos modificar, ni eliminar ventas 
    }

    #endregion

    #region LoadExit

    /// <summary>
    /// Carga los ComboBox de los exit
    /// </summary>
    /// <history>
    /// [jorcanche] created 12/07/2016
    /// </history> 
    private async void LoadExit()
    {
      //Exit Closers
      cmbsaExit1.ItemsSource =
        cmbsaExit2.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: Context.User.SalesRoom.srID, roles: "EXIT");
    }

    #endregion

    #region LoadCloser

    /// <summary>
    /// Carga el ComboBox de Closer
    /// </summary>
    /// <history>
    /// [jorcanche]  created  12/07/2016
    /// </history>
    private async void LoadCloser()
    {
      //Closers
      cmbsaCloser1.ItemsSource =
        cmbsaCloser2.ItemsSource =
          cmbsaCloser3.ItemsSource =
            await BRPersonnel.GetPersonnel(salesRooms: Context.User.SalesRoom.srID, roles: "CLOSER");
      //Capitan de Closers
      cmbsaCloserCaptain1.ItemsSource =
        await BRPersonnel.GetPersonnel(salesRooms: Context.User.SalesRoom.srID, roles: "CLOSERCAPT");
    }

    #endregion

    #region LoadPR

    /// <summary>
    /// Carga los combos de los Vendedores, Capitanes, Los Global Sales, Los VLO y los Podium
    /// </summary>
    /// <history>
    /// [jorcanche]  created 28062016
    /// </history>
    private async void LoadPr()
    {
      //Tipo de membresia y Tipo de membresia Global
      cbosamtGlobal.ItemsSource = cbosamt.ItemsSource = await BRMemberShipTypes.GetMemberShipTypes(1);
      //Tipo de venta
      cbosast.ItemsSource = await BRSaleTypes.GetSalesTypes(1);
      //PR´s
      cmbsaPR1.ItemsSource =
        cmbsaPR2.ItemsSource =
          cmbsaPR3.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: Context.User.SalesRoom.srID, roles: "PR");
      //Capitanes de PRs
      cmbsaPRCaptain1.ItemsSource =
        cmbsaPRCaptain2.ItemsSource =
          cmbsaPRCaptain3.ItemsSource =
            await BRPersonnel.GetPersonnel(salesRooms: Context.User.SalesRoom.srID, roles: "PRCAPT");
    }

    #endregion

    #region LoadLiner

    /// <summary>
    /// Carga los ComboBox de los Liner
    /// </summary>
    /// <history>
    /// [jorcanche] created 12/07/2016
    /// </history>
    private async void LoadLiner()
    {
      //Liners
      cmbsaLiner1.ItemsSource =
        cmbsaLiner2.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: Context.User.SalesRoom.srID, roles: "LINER");
      //Capitan de Liners
      cmbsaLinerCaptain1.ItemsSource =
        await BRPersonnel.GetPersonnel(salesRooms: Context.User.SalesRoom.srID, roles: "LINERCAPT");
      //Podium
      cmbsaPodium.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: Context.User.SalesRoom.srID, roles: "PODIUM");
      //VLO
      cmbsaVLO.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: Context.User.SalesRoom.srID, roles: "VLO");
    }

    #endregion

    #region LoadGrid

    /// <summary>
    /// Carga el grid de ventas
    /// </summary>
    /// <history>
    /// [jorcanche] created 29062016
    /// </history>
    private async void LoadGrid()
    {
      BusyIndicator.IsBusy = true;
      //si se esta buscando
      if (_guId.Equals(0))
      {
        //establecemos los criterios de busqueda
        dtgSale.ItemsSource =
          await BRSales.GetSalesShort(string.IsNullOrEmpty(txtCsagu.Text) ? 0 : Convert.ToInt32(txtCsagu.Text),
            string.IsNullOrEmpty(txtCsaID.Text) ? 0 : Convert.ToInt32(txtCsaID.Text),
            string.IsNullOrEmpty(txtCsaMembershipNum.Text) ? "ALL" : txtCsaMembershipNum.Text,
            string.IsNullOrEmpty(txtCName.Text) ? "ALL" : txtCName.Text,
            string.IsNullOrEmpty(txtCsals.Text) ? "ALL" : txtCsals.Text,
            string.IsNullOrEmpty(txtCsasr.Text) ? "ALL" : txtCsasr.Text,
            dtpCsaDFrom.Value, dtpCsaDTo.Value);
      }
      else
      {
        dtgSale.ItemsSource = await BRSales.GetSalesShort(_guId);
      }
      //Establecemos el modo de solo lectura
      SetMode(EnumMode.ReadOnly);
      //Cargamos el detalle de la venta
      //de la primera venta que aparece en el grid
      await LoadRecord();
      BusyIndicator.IsBusy = false;
    }

    #endregion

    #region LoadRecord

    /// <summary>
    /// Carga la informacion segun el datagrid del sale
    /// </summary>
    /// <history>
    /// [jorcanche]  created 28062016
    /// </history>
    private async Task LoadRecord()
    {
      //Si hay alguna venta
      if (dtgSale.Items.Count > 0)
      {
        //Limpiamos los Source ya que seran nuevos Datos para los Targets
        _saleOld = new Sale();
        _saleNew = new Sale();

        //Cargamos el OldSale Que sera la info Original q esta en la base   
        var sale = dtgSale.SelectedItem != null
          ? (dtgSale.SelectedItem as SaleShort)?.saID
          : (dtgSale.Items[0] as SaleShort)?.saID;
        _saleOld = await BRSales.GetSalesbyId(sale);

        //Y Cargamos el Sale que aditará y sera el que se guardara en la base                        
        ObjectHelper.CopyProperties(_saleNew, _saleOld);

        //Asignamos el datacontext para cargar los controles        
        _loading = true;
        grdSaleGeneral.DataContext = _saleNew;
        _loading = false;     

        //Indicamos si es una venta Out of Pendsing
        SetOutOfPending();

        //Cargamos los pagos de la venta      
        _payments = new System.Collections.ObjectModel.ObservableCollection<Payment>(await BRPayments.GetPaymentsbySale(_saleNew.saID));
        dgpayment.DataContext = _payments;

        //Nombre del huesped
        GetGuestName(_saleNew.sagu);     

        //Respaldamos el monto de la venta original
        _saleAmount = _saleAmountOriginal = GetSaleAmount();

        //Obtenemos los vendedores
        GetSalemen();

        //si la venta es de una fecha cerrada, no permitomos eliminar ventas
        if (IsClosed()) btnDelete.IsEnabled = false;
      }
      else
      {
        //Limpiamos los Source ya que seran nuevos Datos para los Trgets
        _saleOld = new Sale();
        _saleNew = new Sale();

        //Asignamos el datacontext para cargar los controles en este caso seran vacios       
        _loading = true;
        grdSaleGeneral.DataContext = _saleNew;
        _loading = false;

        //Dejamos vacios los tipos de pago     
        _payments = new System.Collections.ObjectModel.ObservableCollection<Payment>();
        dgpayment.DataContext = _payments;

        //Limpiamos el nombre
        lblGuestName.Text = string.Empty;
      }
    }

    #endregion

    #region LoadComboPayment

    /// <summary>
    /// Carga las combos del Datagrid de los    
    /// </summary>
    /// <history>
    /// [jorcanche] created 22/06/2016
    /// </history>
    private async void LoadCombosPayment()
    {
      paptColumn.ItemsSource = await BRPaymentTypes.GetPaymentTypes(1);
      paccColumn.ItemsSource = await BRCreditCardTypes.GetCreditCardTypes(nStatus: 1);
    }

    #endregion

    #region GetGuestName

    /// <summary>
    /// Obtiene el nombre completo del Guest del Sale
    /// </summary>
    /// <param name="guestId">Id del Guest</param>
    ///<history>
    /// [jorcanche] created 24/06/2016
    ///</history> 
    private async void GetGuestName(int? guestId)
    {
      if (guestId == null) return;
      var guest = await BRGuests.GetGuest((int)guestId);
      if (guest != null && guestId != 0)
      {
        lblGuestName.Text = $"{guest.guLastName1} {guest.guFirstName1}";
      }
      else
      {
        txtsagu.Text = "0";
        txtsagu.SelectAll();
        txtsagu.Focus();
        lblGuestName.Text = string.Empty;
        UIHelper.ShowMessage("Guest not found");
      }
    }

    #endregion

    #region GetSalemen

    /// <summary>
    /// Obtiene los vendedores
    /// </summary>
    ///<history>
    ///[jorcanche] created 24/06/2016
    ///</history> 
    private void GetSalemen()
    {
      var salemen = new List<SalesSalesman>();

      if (!string.IsNullOrEmpty(cmbsaLiner1.Text) && salemen.Find(sm => sm.smpe == cmbsaLiner1.Text) == null)
        salemen.Add(new SalesSalesman { smpe = cmbsaLiner1.Text });
      if (!string.IsNullOrEmpty(cmbsaLiner2.Text) && salemen.Find(sm => sm.smpe == cmbsaLiner2.Text) == null)
        salemen.Add(new SalesSalesman { smpe = cmbsaLiner2.Text });
      if (!string.IsNullOrEmpty(cmbsaCloser1.Text) && salemen.Find(sm => sm.smpe == cmbsaCloser1.Text) == null)
        salemen.Add(new SalesSalesman { smpe = cmbsaCloser1.Text });
      if (!string.IsNullOrEmpty(cmbsaCloser2.Text) && salemen.Find(sm => sm.smpe == cmbsaCloser2.Text) == null)
        salemen.Add(new SalesSalesman { smpe = cmbsaCloser2.Text });
      if (!string.IsNullOrEmpty(cmbsaCloser3.Text) && salemen.Find(sm => sm.smpe == cmbsaCloser3.Text) == null)
        salemen.Add(new SalesSalesman { smpe = cmbsaCloser3.Text });
      if (!string.IsNullOrEmpty(cmbsaExit1.Text) && salemen.Find(sm => sm.smpe == cmbsaExit1.Text) == null)
        salemen.Add(new SalesSalesman { smpe = cmbsaExit1.Text });
      if (!string.IsNullOrEmpty(cmbsaExit2.Text) && salemen.Find(sm => sm.smpe == cmbsaExit2.Text) == null)
        salemen.Add(new SalesSalesman { smpe = cmbsaExit2.Text });
      //Agregamos los datos del los SaleMen

      _saleMen = new List<SalesSalesman>();

      foreach (var item in salemen)
      {
        var salesSalesMens =BRSalesSalesmen.GetSalesSalesmens(new SalesSalesman { smpe = item.smpe, smsa = Convert.ToInt32(txtsaID.Text) });
        if (salesSalesMens != null)
        {
          salesSalesMens.Personnel = BRPersonnel.GetPersonnelById(salesSalesMens.smpe);
          _saleMen.Add(salesSalesMens);
        }
        else
        {
          var saleAmount = GetSaleAmount();
          item.smSaleAmountOwn = item.smSaleAmountWith = saleAmount < 0 ? -saleAmount : saleAmount;
          item.Personnel = BRPersonnel.GetPersonnelById(item.smpe);
          item.smSale = true;
          item.smsa = _saleNew.saID;
          _saleMen.Add(item);
        }
      }
    }

    #endregion

    #region GetSaleAmount

    /// <summary>
    /// devuelve el monto de la venta que se debe considerar
    /// </summary>
    /// <history>
    /// [jorcanche] created 25062016
    /// </history>
    private decimal GetSaleAmount()
    {
      //si la venta no es un Dwngrade       
      GetSaleTypeCategory();
      return _saleTypeCategory != "DG" ? _saleNew.saGrossAmount: _saleNew.saNewAmount ?? 0;
    }

    #endregion

    #region GetSaleTypeCategory

    /// <summary>
    /// Obtiene la categoria de un tipo de venta
    /// </summary>
    /// <history>
    /// [jorcanche] created 25062016
    /// </history>
    private async void GetSaleTypeCategory()
    {
      _saleTypeCategory = await BRSaleTypes.GetStstcOfSaleTypeById(_saleNew.sast);
    }

    #endregion

    #region SetOutOfPending

    /// <summary>
    /// Indica si es un venta que salio de pendiente
    /// </summary>
    /// <history>
    /// [jorcanche]  created 25062016
    /// </history>
    private void SetOutOfPending()
    {
      //Una  venta es Out Of Pendin si es procesable con fecha de venta distinta de la fecha de procesable
      lblOutOfPending.Visibility = (chksaProc.IsChecked == true) && (txtsaD.Text != txtsaProcD.Text)
        ? Visibility.Visible
        : Visibility.Hidden;
    }

    #endregion

    #region SetMode

    /// <summary>
    /// Habilita / deshabilita los controles del formulario segun el modo de datos
    /// </summary>
    /// <param name="mode">Enumerado EnumMode</param>
    /// <history>
    /// [jorcanche] 20/05/2016
    /// </history>
    private void SetMode(EnumMode mode)
    {    
      var blnEnable = mode != EnumMode.ReadOnly ? true : false;
      //Grid principal
      dtgSale.IsEnabled = !blnEnable;
      //criterios de busqueda
      //brincamos este paso ya que segun el modo si es Global o Sale se ocultara los criterios de busqueda asi que no ha necesidad de inabilitarlos
      //Botones
      btnEdit.IsEnabled = (!blnEnable && Context.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.Standard) &&
                           dtgSale.Items.Count > 0);
      //Solo permite eliminar ventanas si tiene permiso super especial de ventas
      btnDelete.IsEnabled = (!blnEnable && Context.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.SuperSpecial) &&
                             dtgSale.Items.Count > 0);
      btnSave.IsEnabled = btnUndo.IsEnabled = blnEnable;
      btnClose.IsEnabled = !blnEnable;
      //Autenticacion automatica
      txtPwd.IsEnabled = blnEnable;
      txtChangedBy.IsEnabled = blnEnable;
      btnLog.IsEnabled = true;

      if (blnEnable && Context.User.AutoSign)
      {
        txtChangedBy.Text = Context.User.User.peID;
        txtPwd.Password = Context.User.User.pePwd;
      }
      else
      {
        txtChangedBy.Text = txtPwd.Password = string.Empty;
      }
      //no permitimos modificar la ventana si es de una fecha cerrada
      var closen = !IsClosed();
      blnEnable = blnEnable && closen;
      //deshabilitamos los datos generales
      EnabledGeneral(false);
      //Vendedores
      grdSalesmen.IsEnabled = 
      cmbsaPR1.IsEnabled =
      cmbsaPR2.IsEnabled =
      cmbsaPR3.IsEnabled =
      cmbsaLiner1.IsEnabled =
      cmbsaLiner2.IsEnabled =
      cmbsaCloser1.IsEnabled =
      cmbsaCloser2.IsEnabled =
      cmbsaCloser3.IsEnabled =
      cmbsaExit1.IsEnabled =
      cmbsaExit2.IsEnabled =
      cmbsaPodium.IsEnabled =
      cmbsaVLO.IsEnabled =
      cmbsaPRCaptain1.IsEnabled =
      cmbsaPRCaptain2.IsEnabled =
      cmbsaPRCaptain3.IsEnabled =
      cmbsaPRCaptain3.IsEnabled =
      cmbsaCloserCaptain1.IsEnabled =
      cmbsaLinerCaptain1.IsEnabled =
      blnEnable;
      //boton de venta de vendedores
      btnSalesSalesmen.IsEnabled = blnEnable;
      //Capitanes
      grbCaptains.IsEnabled = blnEnable;
      //habilitamos los campos que no se obtienen de Clubes
      if (blnEnable)
      {
        // gGeneral.IsEnabled = true;
        //Numero de memebresia
        //txtsaMembershipNum.IsEnabled = true;
        //Guest ID
        txtsagu.IsEnabled = true;
        //Venta por telefono
        chksaByPhone.IsEnabled = true;
        //Pagos
        grbPayments.IsEnabled = true;
        dgpayment.CanUserAddRows = true;

        //Comentarios
        txtsaComments.IsEnabled = true;
      }
      //Si la venta es de una fecha cerrada
      if (mode != EnumMode.ReadOnly && IsClosed())
      {
        //Permitimos modificar la venta para marcarla como procesabel o para desmarcarla de procesable 
        // si su fecha de cancelacion no esta en una fecha cerrada
        if (chksaProc.IsChecked != null && (!chksaProc.IsChecked.Value || !DiferentDate(txtsaProcD.Text, _closeD)))
        {
          fraProcessable.IsEnabled = true;
        }
        //Permitimos modificar la venta para cancelarla o para descancelarla
        //si su fecha de cancelacion no esta en una fecha cerrada
        if (chksaCancel.IsChecked != null && (!chksaCancel.IsChecked.Value || !DiferentDate(txtsaCancelD.Text, _closeD)))
        {
          //fraCancelled.IsEnabled = true;
          chksaCancel.IsEnabled = true;
        }
        //Comentarios
       txtsaComments.IsEnabled = true;


       
      }

      //Se Inhabilita el membershipNum
      txtsaMembershipNum.IsEnabled = false;
    }

    #endregion

    #region DiferentDate

    /// <summary>
    /// Diferencia entre dos fechas 
    /// </summary>
    /// <param name="date"> Fecha en de tipo string</param>
    /// <param name="closed"> Fecha de venta cerrada de tipo DateTime</param>
    /// <history>
    /// [jorcanche]  created 28062016
    /// </history>
    private static bool DiferentDate(string date, DateTime closed)
    {
      DateTime result;
      //Si tiene una fecha especifica      
      if (DateTime.TryParse(date, out result))
      {
        //si la fecha es menor o igual a la fecha de cierre
        return result <= closed;
      }
      return false;
    }

    #endregion

    #region EnabledGeneral

    /// <summary>
    /// Habilita o deshabilita segun el modo de edicion 
    /// </summary>
    /// <param name="enabled">True = si se habilidata, False = si se deshabilatara</param>
    /// <history>
    /// [jorcanche]  created 28062016
    /// </history>
    private void EnabledGeneral(bool enabled)
    {
      txtsaID.IsEnabled = fraProcessable.IsEnabled = txtsaMembershipNum.IsEnabled =
        txtsaD.IsEnabled = txtsaCancelD.IsEnabled = chksaCancel.IsEnabled = chksaByPhone.IsEnabled = txtsagu.IsEnabled =
          lblGuestName.IsEnabled = txtsals.IsEnabled = txtsalo.IsEnabled = txtsasr.IsEnabled =
            chksaUpdated.IsEnabled = cbosast.IsEnabled = txtsaRefMember.IsEnabled = txtsaProcRD.IsEnabled =
              cbosamt.IsEnabled = cbosamtGlobal.IsEnabled = txtsaLastName1.IsEnabled = txtsaLastName2.IsEnabled =
                txtsaFirstName1.IsEnabled =
                  txtsaFirstName2.IsEnabled = grbAmounts.IsEnabled = txtsaClosingCost.IsEnabled =
                    txtsaOverPack.IsEnabled =
                      grbPayments.IsEnabled =
                        grbAmounts_Copy.IsEnabled = chksaByPhone.IsEnabled = txtsaComments.IsEnabled = enabled;
    }

    #endregion

    #region IsClosed

    /// <summary>
    /// Determina si una venta esta en una fecha cerrada
    /// </summary>
    /// <history>
    /// [jorcanche]  created 28062016
    /// </history>
    private bool IsClosed()
    {
      bool retornar = (DiferentDate(txtsaD.Text, _closeD.Date) || DiferentDate(txtsaProcD.Text, _closeD.Date) ||
                       DiferentDate(txtsaCancelD.Text, _closeD.Date));
      return retornar;
    }

    #endregion

    #region btnSearch_Click

    /// <summary>
    /// Buscar los Sales segun los criterios de busqueda 
    /// </summary>
    /// <history>
    /// [jorcanche]  created 28062016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      LoadGrid();
    }

    #endregion

    #region ValidateSalesSalesmen

    /// <summary>
    /// Valida los datos necesarios para definir las ventas de los vendedores
    /// </summary>
    /// <history>
    /// [jorcanche]  created 28062016
    /// </history>
    private bool ValidateSalesSalesmen()
    {
      //Validamos el tipo de venta
      if (string.IsNullOrEmpty(_saleNew.sast))
      {
        UIHelper.ShowMessage("Specify a Sale Type");
        cbosast.Focus();
        return false;
      }
      //Validamos el nuevo monto
      if (_saleNew.saNewAmount != 0) return true;
      UIHelper.ShowMessage("Specify the new amount");
      txtsaNewAmount.Focus();
      return false;
    }

    #endregion

    #region btnSalesSalesmen_Click

    /// <summary>
    /// Invoca el formulario de frmSalesSalesmen
    /// </summary>
    /// <history>
    /// [jorcanche]  created 28062016
    /// </history>
    private void btnSalesSalesmen_Click(object sender, RoutedEventArgs e)
    {
      //si existe la venta 
      if (string.IsNullOrEmpty(txtsaID.Text)) return;
      //si es una secretaria 
      if (Context.User.HasRole(EnumRole.Secretary))
      {
        if (ValidateSalesSalesmen())
        {
          //Obtenermos los vendedores
          GetSalemen();
          var saleAmount = GetSaleAmount();
          var salessalesmen = new frmSalesSalesmen(_saleMen, _saleNew, saleAmount < 0 ? -saleAmount : saleAmount, _saleAmountOriginal)
          { Owner = this };
          salessalesmen.ShowDialog();
        }
        else
        {
          UIHelper.ShowMessage("Save the sale first.");
        }
      }
      else
      {
        UIHelper.ShowMessage("Access denied.");
      }
    }

    #endregion  

    #region cbo_SelectionChanged

    /// <summary>
    /// Valida cuando se cambia de Item los Combox de los vendedores y de los capitanes 
    /// </summary>
    /// <history>
    /// [jorcanche]  created 24062016
    /// </history>
    private void cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      
      var cmb = sender as ComboBox;
      if (cmb == null || _loading) return;
      
      var personnelValidando = cmb.Name.Substring(5).Replace("Captain", "CAPT").ToUpper();
      var lstcmb = UIHelper.GetChildParentCollection<ComboBox>(grdSalesmen);
      if (cmb.SelectedIndex == -1) return;
      foreach (var item in lstcmb)
      {
        var personnelFound = item.Name.Substring(5).Replace("Captain", "CAPT").ToUpper();
        //Validacion que sirve para saber si no es mismo ComboBox que se esta validando, PR1 == PR1 
        if (personnelFound != personnelValidando)
        {
          //Validacion que sirve para siempre se compare los del mismo rol PR == PR
          if (personnelValidando.Trim('1', '2', '3') == personnelFound.Trim('1', '2', '3'))
          {
            //Ahora como ya se sabe que no es mismo ComboBox y es el mismo rol entonces ya podemos hacer 
            //la validacion de ser el mismo texto no permitimos que se seleccione
            var rol = personnelValidando.Trim('1', '2', '3');
            if (cmb.SelectedValue.ToString() == item.Text)
            {              
              UIHelper.ShowMessage($"Please select another person. \nThe person with the Id: {item.Text} already selected with the role of {rol}");
              cmb.SelectedIndex = -1;
              break;
            }
          }
        }
      }
    }  

    #endregion

    #region CloseDate
    /// <summary>
    /// Obtiene la fecha de cierre
    /// </summary>
    /// <history>
    /// [jorcanche]  created 01072016
    /// </history>
    public async void CloseDate()
    {
      _closeD = await BRSales.GetSalesCloseD(Context.User.SalesRoom.srID);
    }
    #endregion

    #region ValidatePagos

    /// <summary>
    /// validamos los pagos
    /// </summary>
    ///<history>
    ///[jorcanche]  created 24062016
    ///</history>
    private bool ValidatePagos()
    {
      bool foundbad = false;
      _payments.ToList().ForEach(payment =>
      {
        payment.pasa = _saleNew.saID;
        payment.pacc = string.IsNullOrEmpty(payment.pacc) ? string.Empty : payment.pacc;

        if (string.IsNullOrEmpty(payment.papt) && string.IsNullOrEmpty(payment.pacc))
        {
          _payments.Remove(payment);
        }
        if (payment.papt == "CC" && string.IsNullOrEmpty(payment.pacc))
        {
          foundbad = true;
        }                
      });

      if (!foundbad) return true;

      UIHelper.ShowMessage("Please specify the Credit Card Type");
      return false;
    }
    #endregion

    #region IsSaleUpdate
    /// <summary>
    /// Indica si un tipo de venta es una actualizacion de otra venta
    /// </summary>
    ///<history>
    ///[jorcanche] 24062016
    ///</history>
    private async void IsSaleUpdate()
    {
      if (cbosast.SelectedValue != null)
        _isSaleUpdate = await BRSaleTypes.GetstUpdateOfSaleTypeById(_saleNew.sast, 1);
    }
    #endregion

    #region ValidateCancelDate

    /// <summary>
    /// Valida la fecha de cancelacion
    /// </summary>
    ///<history>
    ///[jorcanche] 24062016
    ///</history>
    private bool ValidateCancelDate()
    {
      //sim esta cancelada 
      if (chksaCancel.IsChecked == null || !chksaCancel.IsChecked.Value) return true;
      //Validamos que la fecha de procesable no este en un fecha cerrada
      if (!ValidateCloseDate(txtsaCancelD, "Cancellation date")) return false;
      if (Convert.ToDateTime(txtsaCancelD.Text) < Convert.ToDateTime(txtsaD.Text))
      {
        UIHelper.ShowMessage("Cancellation date can not be before sale date.");
        txtsaCancelD.Focus();
        return false;
      }
      //validamos que la fecha de cancelacio no sea menor a la fecha procesable
      if (chksaProc.IsChecked == null || !chksaProc.IsChecked.Value) return true;
      if (Convert.ToDateTime(txtsaCancelD.Text) >= Convert.ToDateTime(txtsaProcD.Text)) return true;
      UIHelper.ShowMessage("Processable date can no be after Cancellation date.");
      txtsaProcD.Focus();
      return false;
    }

    #endregion

    #region ValidateMembershipTypeGlobal

    /// <summary>
    /// Valida el tipo de membresia global
    /// </summary>
    /// <history>
    /// [jorcanche]  created 04072016
    /// </history>
    private async Task<bool> ValidateMembershipTypeGlobal()
    {
      //Validamos que ingrese el tipo de membresia global
      if (cbosamtGlobal.SelectedIndex == -1)
      {
        UIHelper.ShowMessage("Specify the Membership Type Globale");
        cbosamtGlobal.Focus();
        //_validateMembershipTypeGlobal = false;
        return false;
      }
      IsSaleUpdate();
      if (_isSaleUpdate)
      {
        //Tipo de membresia de la emembresia anterior        
        var membershipTypePrevious = await BRSales.GetMembershipType(txtsaRefMember.Text);

        //si encontramos la membresia anterior 
        if (!string.IsNullOrEmpty(membershipTypePrevious))
        {
          //obtenermos el nivel de la memebresia antrior                
          var membershipPreviousLevel = await BRMemberShipTypes.GetLevelOfMemberShipTypes(membershipTypePrevious, 1);

          //obtenermos el nivel de la membresia       
          var membershipLevel =
            await BRMemberShipTypes.GetLevelOfMemberShipTypes(cbosamtGlobal.SelectedValue.ToString(), 1);

          GetSaleTypeCategory();
          switch (_saleTypeCategory)
          {
            case "UG": //Upgrade
              //Validamos el tipo de membresia no sea menor que el tipo de membresia anterior 
              if (membershipLevel < membershipPreviousLevel)
              {
                UIHelper.ShowMessage(
                  "In a Upgrade, the Membership Type Global can't be less than the Membership Type Previous");
                //_validateMembershipTypeGlobal = false;
                return false;
              }
              break;
            case "DG": //Downgrade
              //Validamos el tipo de membresia no sea mayo que el tipo de membresia anterior}
              if (membershipLevel > membershipPreviousLevel)
              {
                UIHelper.ShowMessage(
                  "In a Downgrade, the Membership Type Global can't be greater than the Membership Type Previous");
                return false;
              }
              break;
          }
        } //Si no encontramos la membresia anterior 
        else
        {
          UIHelper.ShowMessage("Membership Number Previous not found");
          txtsaRefMember.Focus();

          return false;
        }
      }

      return true;
    }

    #endregion

    #region ValidateProcessableDate

    /// <summary>
    /// Valida la fecha procesable
    /// </summary>
    ///<history>
    ///[jorcanche] 24062016
    ///</history>
    private bool ValidateProcessableDate()
    {
      //si es procesable
      if (chksaProc.IsChecked == null || !chksaProc.IsChecked.Value) return true;
      //Validamos que la fecha de prosesable no este en una fecha cerrada 
      if (!ValidateCloseDate(txtsaProcD)) return false;
      //Validamos que la fecha de procesable no sea menor a la fecha de venta
      if (Convert.ToDateTime(txtsaProcD.Text).Date >= Convert.ToDateTime(txtsaD.Text).Date) return true;
      UIHelper.ShowMessage("Procesable date can not be before sale date");
      txtsaD.Focus();
      return false;
    }

    #endregion

    #region Validate

    /// <summary>
    /// Validate General
    /// </summary>
    /// <history>
    /// [jorcanche]  created 24062016
    /// </history>
    private async Task<bool> Validate()
    {
      //Validamos quien hizo el cambio de contraseña    
      var validate = ValidateHelper.ValidateForm(grbChangedBy, "Login");
      if (!string.IsNullOrEmpty(validate))
      {
        UIHelper.ShowMessage(validate);
        return false;
      }

      //Validamos los datos generales
      if (!await ValidateGeneral())
      {
        Panel.SetZIndex(gGeneral, 1);    
        return false;
      }

      //Validamos los vendedores
      //Ojo: No validar cuando sea PHONE SALE
      if (chksaByPhone.IsChecked != null && !chksaByPhone.IsChecked.Value)
      {
        var validateSalesmen = ValidateHelper.ValidateForm(grdSalesmen, "SalesSalesmen");
        if (!string.IsNullOrEmpty(validateSalesmen))
        {
          UIHelper.ShowMessage(validateSalesmen);
          Panel.SetZIndex(gGeneral, 1);        
          return false;
        }
      }

      //Validamos que los datos der la ventana existan     
      if (await ValidateExist()) return true;
      Panel.SetZIndex(gGeneral, 1);
      return false;
    }

    #endregion

    #region ValidateExist

    /// <summary>
    /// Valida que los datos de la ventana existan
    /// </summary>   
    /// <history>
    /// [jorcanche] 06/06/2016
    /// </history>
    private async Task<bool> ValidateExist()
    {
      var validateExist =
        await BRSales.ValidateSale(txtChangedBy.Text, EncryptHelper.Encrypt(txtPwd.Password), _saleNew.saID,
          _saleNew.saMembershipNum, _saleNew.sagu, _saleNew.sast, _saleNew.sasr, _saleNew.salo, _saleNew.saPR1, _saleNew.saPR2, _saleNew.saPR3,
          _saleNew.saPRCaptain1, _saleNew.saPRCaptain2, _saleNew.saPRCaptain2, _saleNew.saLiner1, _saleNew.saLiner2,
          _saleNew.saLinerCaptain1,
          _saleNew.saCloser1, _saleNew.saCloser2, _saleNew.saCloser3, _saleNew.saCloserCaptain1, _saleNew.saExit1, _saleNew.saExit2,
          _saleNew.saPodium, _saleNew.saVLO);

      if (string.IsNullOrEmpty(validateExist.Focus)) return true;
      //Desplegamos el mensaje de error
      UIHelper.ShowMessage(validateExist.Message);
      //Establecemos el foco en el control que tiene el error      

      #region switch ValidationData

      switch (validateExist.Focus)
      {
        case "ChangedBy":
          txtChangedBy.Focus();
          break;
        case "Password":
          txtPwd.Focus();
          break;
        case "MembershipNumber":
          txtsaMembershipNum.Focus();
          break;
        case "Guest":
          txtsagu.Focus();
          break;
        case "SalesRoom":
          txtsasr.Focus();
          break;
        case "Location":
          txtsalo.Focus();
          break;
        case "PR1":
          cmbsaPR1.Focus();
          break;
        case "PR2":
          cmbsaPR2.Focus();
          break;
        case "PR3":
          cmbsaPR3.Focus();
          break;
        case "PRCaptain1":
          cmbsaPRCaptain1.Focus();
          break;
        case "PRCaptain2":
          cmbsaPRCaptain2.Focus();
          break;
        case "PRCaptain3":
          cmbsaPRCaptain3.Focus();
          break;
        case "Liner1":
          cmbsaLiner1.Focus();
          break;
        case "Liner2":
          cmbsaLiner2.Focus();
          break;
        case "LinerCaptain":
          cmbsaLinerCaptain1.Focus();
          break;
        case "Closer1":
          cmbsaCloser1.Focus();
          break;
        case "Closer2":
          cmbsaCloser2.Focus();
          break;
        case "Closer3":
          cmbsaCloser3.Focus();
          break;
        case "CloserCaptain":
          cmbsaCloserCaptain1.Focus();
          break;
        case "Exit1":
          cmbsaExit1.Focus();
          break;
        case "Exit2":
          cmbsaExit2.Focus();
          break;
        case "Podium":
          cmbsaPodium.Focus();
          break;
        case "VLO":
          cmbsaVLO.Focus();
          break;
      }
      return false;

      #endregion
    }

    #endregion

    #region ValidateGeneral

    /// <summary>
    /// Valida los datos Generales
    /// </summary>
    /// <history>
    /// [jorcanche]  created 2406216
    /// </history>
    private async Task<bool> ValidateGeneral()
    {
      //obtenemos la fecha de cierre
      CloseDate();
      //Validamos la fecha de venta no este en una fecha cerrada
      if (!ValidateCloseDate(txtsaD)) return false;

      //validamos la fecha procesable
      if (!ValidateProcessableDate()) return false;

      //Validamos la fecha de cancelacion 
      if (!ValidateCancelDate()) return false;

      //Validamos el si existe el ID del huesped
      if (_saleNew.sagu == 0)
      {
        txtsagu.Focus();
        return false;
      }

      //Validamo la membresia anterior si es un Upgrade o un Downgrade
      IsSaleUpdate();
      txtsaRefMember.Tag = _isSaleUpdate ? "Reference Member" : null;

      //Validamos el tipo de mebresia global

      //if (!_validateMembershipTypeGlobal) return false;
      if (!await ValidateMembershipTypeGlobal()) return false;

      //Validamos los campos necesarios que estan dentro dell Grid gGeneral:
      //El numero de membresia
      //El huesped
      //La sala de ventas
      //El location
      //El tipo de membresia 
      //El primer apellido
      //El priemr nombre
      //El Sale ID 
      //El Reference Member sí es un Upgrade o Un Downgrade
      //El Nuevo monto
      //El tipo de venta
      var validate = ValidateHelper.ValidateForm(gGeneral, "Sale");
      if (!string.IsNullOrEmpty(validate))
      {
        UIHelper.ShowMessage(validate);
        return false;
      }

      //Validamos el nuevo monto que no sea igual a 0
      if (_saleNew.saNewAmount == 0)
      {
        UIHelper.ShowMessage("Specify the new amount");
        txtsaNewAmount.Focus();
        return false;
      }

      //validamos los pagos
      return ValidatePagos();
    }

    #endregion

    #region ValidateCloseDate

    /// <summary>
    /// Validamos la fecha de venta no este en una fecha cerrada
    /// </summary>
    /// <returns></returns>
    private bool ValidateCloseDate(TextBox txt, string description = "")
    {
      DateTime result;
      if (!DateTime.TryParse(txt.Text, out result)) return true;
      //Si la condicion es valida y el editor esta habilitado
      if (!txt.IsEnabled) return true;
      var des = string.IsNullOrEmpty(description) ? "Sales date" : description;

      //Validamos la fecha
      if (string.IsNullOrEmpty(txt.Text))
      {
        UIHelper.ShowMessage("Specify a Sale Date.");
        return false;
      }
      //validamos que la fecha no sea mayor a la fecha actual
      if (Convert.ToDateTime(txt.Text) > BRHelpers.GetServerDate().Date)
      {
        UIHelper.ShowMessage("Can not be after today.");
        return false;
      }
      //validamos que la fecha no sea menor o igual a la fecha de cierre
      if (Convert.ToDateTime(txt.Text) <= _closeD)
      {
        UIHelper.ShowMessage($"It,s not allowed to make {des} for a closed date");
        txt.Focus();
        return false;
      }
      return true;
    }

    #endregion

    #region btnEdit_Click

    /// <summary>
    /// Modo edicion
    /// </summary>
    ///<history>
    ///[jorcanche] created  24062016
    ///</history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      SetMode(EnumMode.Edit);
    }

    #endregion

    #region btnDelete_Click

    /// <summary>
    /// Elimina una Sale
    /// </summary>
    /// <history>
    /// [jorcanche]  created 30062016
    /// </history>    
    private async void btnDelete_Click(object sender, RoutedEventArgs e)
    {
      //Preguntamos al usuario si en verdad desea eliminar la venta
      var result = UIHelper.ShowMessage("Are you sure you want to delete this sale ?");
      if (result != MessageBoxResult.OK) return;
      await BRSales.DeleteSale(Convert.ToInt32(txtsaID.Text));
      //LoadRecord();
      LoadGrid();
    }

    #endregion

    #region btnClose_Click

    /// <summary>
    /// Cierra la ventana
    /// </summary>
    ///<history>
    ///[jorcanche] 24062016
    ///</history>
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    #endregion

    #region btnUndo_Click

    /// <summary>
    /// Deshace los cambios hechos a una venta
    /// </summary>
    ///<history>
    ///[jorcanche] created 24/06/2016
    ///</history>
    private async void btnUndo_Click(object sender, RoutedEventArgs e)
    {
      BusyIndicator.IsBusy = true;
      //si no hay ventas
      if (dtgSale.Items.Count == 0)
      {
        //si no se esta buscando
        if (_guId > 0)
        {
          _saleNew = new Sale();
          DataContext = null;
          return;
        }
      }
      //Si hay ventas ó Si se esta buscando
      await LoadRecord();

      //Establecemos el mode de solo lectura
      SetMode(EnumMode.ReadOnly);

      BusyIndicator.IsBusy = false;
    }

    #endregion

    #region chksaCancel_Checked

    /// <summary>
    /// Agrega la fecha gdel servidor (actual) al txtsaCancelID cuando hace check al chksaCancel y viseversa
    /// </summary>
    ///<history>
    ///[jorcanche] created 24062016
    ///</history>
    private void chksaCancel_Checked(object sender, RoutedEventArgs e)
    {
      if (_loading) return;
      _saleNew.saCancelD = _saleNew.saCancel ? (DateTime?)BRHelpers.GetServerDate() : null;
      txtsaCancelD.Text = _saleNew.saCancelD.ToString();
    }

    #endregion

    #region btnSave_Click

    /// <summary>
    /// Permite guardar una venta
    /// </summary>
    /// <history>
    /// [jorcanche] 03/06/2016
    /// </history>
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      var salesmenChanges = new List<SalesmenChanges>();
      var authorizedBy = string.Empty;
      //Validamos los datos     
      if (!await Validate()) return;
      //obtenermos los cambios de vendedores
      if (!GetSalesmenChanges(ref salesmenChanges, ref authorizedBy)) return;
      //Guardamos la venta
      Save(salesmenChanges, authorizedBy);
    }

    #endregion

    #region Save

    /// <summary>
    /// Guarda una venta
    /// </summary>
    /// <history>
    /// [jorcanche] 07/06/2016 created
    /// </history>
    private async void Save(IEnumerable<SalesmenChanges> salesmenChanges, string authorizedBy)
    {
      BusyIndicator.IsBusy = true;
      try
      {
        if (_saleNew != _saleOld)
        {
          //Procesable     
          if (_saleNew.saProcD == null) _saleNew.saProc = false;
          //Cancelada
          if (_saleNew.saCancelD == null) _saleNew.saCancel = false;

          //Establecemos los procentajes de volumen de venta de los vendedores
          SetVolumenPercentage();
          IsSaleUpdate();
          //Establecemos la fecha de procesable de referencia
          if (!_isSaleUpdate && _saleNew.saProcRD != _saleNew.saProcD)
          {
            _saleNew.saProcRD = _saleNew.saProcD;
          }
          if (_isSaleUpdate && _saleNew.saProcRD != null)
          {
            _saleNew.saProcRD = _saleNew.saProcD;
          }

          //Guardamos todos los movimientos que estan relacionados con Sale
          var saleAmounts = GetSaleAmount();
          var res = await BRSales.SaveSale(_saleOld, _saleNew, _payments, txtsaRefMember.IsEnabled, Context.User.SalesRoom.srHoursDif,
                                             txtChangedBy.Text, saleAmounts < 0 ? -saleAmounts : saleAmounts, _saleMen, _saleAmountOriginal,
                                             ComputerHelper.GetIpMachine(), salesmenChanges, authorizedBy);

          //Si no ocurrio un problema al momento de guardar, mostramos el mensaje
          //de los contrario se iria al catch y alli nos mostraria el mensaje en especifico
          if (res != 0) UIHelper.ShowMessageResult("Sale", res);

          //Recargamos la información
          await LoadRecord();
        }

        //Validamos si existe el MemberShipNum en Intelligence Contracts
        if (await BRMemberSalesman.ExistsMembershipNum(_saleNew.saMembershipNum))
        {
          //Actualizamos vendedores en Intelligence Contracts

          //Agregamos  
          List<MemberSalesmanClubles> lstMemberSalesmenClubes = new List<MemberSalesmanClubles>();
          List<Personnel> lstPersonels = new List<Personnel>();
          //PR's
          lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "OPC", Job = "OPC", Id = _saleNew.saPR1 });
          lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "OPC", Job = "OPC", Id = _saleNew.saPR2 });
          lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "OPC", Job = "OPC", Id = _saleNew.saPR3 });
          //Liners
          lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "LINER", Job = "LIN", Id = _saleNew.saLiner1 });
          lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "LINER", Job = "LIN", Id = _saleNew.saLiner2 });
          //Closers
          lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "CLOSER", Job = "CLOS", Id = _saleNew.saCloser1 });
          lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "CLOSER", Job = "CLOS", Id = _saleNew.saCloser2 });
          lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "CLOSER", Job = "CLOS", Id = _saleNew.saCloser3 });
          //Exit Closers
          lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "EXIT", Job = "JR", Id = _saleNew.saExit1 });
          lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "EXIT", Job = "JR", Id = _saleNew.saExit1 });

          foreach (var mbc in lstMemberSalesmenClubes)
          {
            if (!string.IsNullOrEmpty(mbc.Id))
            {
              lstPersonels.Add(BRPersonnel.GetPersonnelById(mbc.Id));
            }
          }

          List<string> mensajes = await BRMemberSalesman.SaveMemberSalesmenClubes(_saleNew, txtChangedBy.Text, lstMemberSalesmenClubes, lstPersonels);
          foreach (var item in mensajes)
          {
            UIHelper.ShowMessage(item);
          }
        }
        else
        {
          UIHelper.ShowMessage($"The Membership Num {_saleNew.saMembershipNum} do not exists on Intelligence Contracts \n" +
                                "Can not save the Salesmens in Intelligence Contracts");
        }
        //Establecemos el modos de solo lectura
        SetMode(EnumMode.ReadOnly);
        BusyIndicator.IsBusy = false;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }

    #endregion

    #region SaveMemberSalesmenClubes

    /// <summary>
    /// Guarda vendedores en Intelligence Contracts
    /// </summary>
    /// <history>/
    /// [jorcanche] created 08/07/2016
    /// </history>
    private async Task SaveMemberSalesmenClubes()
    {     
      string sPRs = string.Empty, sLiners = string.Empty, sClosers = string.Empty, sExits = string.Empty;

      //Obtenemos los vendedores de Intelligence Contracts 
      _lstMemberSalesmens =
        await BRMemberSalesman.GetMemberSalesmen(_saleNew.saMembershipNum, "ALL", "ALL", "OPC,LINER,CLOSER,EXIT");

      //Agregamos  Actualizamos los vendedores de Origos den Intelligence contracts

      //PR's
      sPRs = await SaveMemberSalesmanClubesByRole("OPC", "OPC", _saleNew.saPR1, sPRs);
      sPRs = await SaveMemberSalesmanClubesByRole("OPC", "OPC", _saleNew.saPR2, sPRs);
      sPRs = await SaveMemberSalesmanClubesByRole("OPC", "OPC", _saleNew.saPR3, sPRs);

      //Liners
      sLiners = await SaveMemberSalesmanClubesByRole("LINER", "LIN", _saleNew.saLiner1, sLiners);
      sLiners = await SaveMemberSalesmanClubesByRole("LINER", "LIN", _saleNew.saLiner2, sLiners);

      //Closers
      sClosers = await SaveMemberSalesmanClubesByRole("CLOSER", "CLOS", _saleNew.saCloser1, sClosers);
      sClosers = await SaveMemberSalesmanClubesByRole("CLOSER", "CLOS", _saleNew.saCloser2, sClosers);
      sClosers = await SaveMemberSalesmanClubesByRole("CLOSER", "CLOS", _saleNew.saCloser3, sClosers);

      //Exit Closers
      sExits = await SaveMemberSalesmanClubesByRole("EXIT", "JR", _saleNew.saExit1, sExits);
      sExits = await SaveMemberSalesmanClubesByRole("EXIT", "JR", _saleNew.saExit2, sExits);

      //Eliminamos los vendedores en Intelligence Contracts que no estan en Intelligence Marketing
      DeleteSalesmenClubes("OPC", sPRs);
      DeleteSalesmenClubes("LINER", sLiners);
      DeleteSalesmenClubes("CLOSER", sClosers);
      DeleteSalesmenClubes("EXIT", sExits);
    }

    #endregion

    #region DeleteSalesmenClubes

    /// <summary>
    /// Elimina los vendedores de una afiliacion en Intelligence Contracts si no estan en Intelligence marketing
    /// </summary>
    /// <param name="role"></param>
    /// <param name="salesmen"></param>
    /// <history>
    /// [jorcanche] created 11/07/2016
    /// </history>
    private async void DeleteSalesmenClubes(string role, string salesmen)
    {
      //Obtenemos vendedores actuales de Intelligence Contracts
      var memberSalesmens = await BRMemberSalesman.GetMemberSalesmen(_saleNew.saMembershipNum, "ALL", "ALL", role);
      if (memberSalesmens.Any())
      {
        //si el vendedor no esta en la lista de vendedores de Origos, lo eliminamos en Intellligence Contracts
        memberSalesmens.ForEach(async ms =>
        {
          if (!salesmen.Contains(ms.OPC))
          {
            await BRMemberSalesman.DeleteMemberSalesman(ms.CLMEMOPC_ID);
          }
        });
      }
    }

    #endregion

    #region SaveMemberSalesmanClubesByRole

    /// <summary>
    /// Guarda un vendedor por rol en Intelligence Contracts
    /// </summary>
    /// <history>
    /// [jorcanche] created 08/07/2016
    /// </history>
    private async Task<string> SaveMemberSalesmanClubesByRole(string role, string job, string id, string salesmen)
    {
      //si tiene una clave de vendedor de origos
      if (!string.IsNullOrEmpty(id))
      {      
        //agregamos la clave de vendedor en origos
        salesmen = salesmen + id + " ";
        //select peID, peN, peSalesmanID from Personnel order by peN
        //obtenemos la clave del vendedor de Intelligence Contracts relaionando al vendedor de Origos
        var personel = BRPersonnel.GetPersonnelById(id);
        //Validamos si existe el vendedor en IM
        if (personel == null)
        {
          UIHelper.ShowMessage($"The personnel {id} does not exists on Intelligence Marketing");
          return salesmen;
        }
        //Validamos si el vendedor de origos tiene relacionada una clave de Intelligence Contracts
        var salemanId = personel.peSalesmanID;
        if (string.IsNullOrEmpty(salemanId))
        {
          UIHelper.ShowMessage($"The personnel {id} is not associated with a Intelligence Contract's salesman.");
          return salesmen;
        }
        //Validamos si existe la clave de intelligence Contracts
        var zone = _saleNew.sasr;
        if (!await BRMemberSalesman.ExistsSalesman(zone, salemanId))
        {
          UIHelper.ShowMessage($"The salesman  {salemanId} from zone {zone} does not exists on Intelligence Contracts");
          return salesmen;
        }               
        //Localizamos el vendedor de Intelligence contracts
        var memberSalesmens = _lstMemberSalesmens.FirstOrDefault(sa => sa.OPC == salemanId && sa.CLAOPC_ID == job);
        //si no se localizo 
        if (memberSalesmens == null)
        {
          //lo agregamos en Intelligence contracts
          await AddSalemanClubes(job, salemanId, zone);
        }
        else
        {
          // si tiene el rol solicitado
          if (memberSalesmens.Role == role)
          {
            memberSalesmens.CLAOPC_ID = job;
            memberSalesmens.STATUS = "A";
            memberSalesmens.ZONA = _saleNew.sasr;
            //Actualizamos el vendedor de Intelligence contracts 
            await BRMemberSalesman.SaveMemberSalesman(memberSalesmens.RECNUM, memberSalesmens, txtChangedBy.Text);
            //Context.User.User.peID);
          }
          else //Si no tiene el rol solicitado
          {
            //Agregamos el vendedor de intelligence contracts
            await AddSalemanClubes(job, salemanId, zone);
          }
        }
      }
      return salesmen;
    }

    #endregion

    #region AddSalemanClubes

    /// <summary>
    /// Agrega un vendedor a un afiliacion en Intelligence Contracts
    /// </summary>
    /// <param name="job"></param>
    /// <param name="salemanId"></param>
    /// <param name="zone"></param>
    /// <history>
    /// [jorcanche] 11/07/2016 created 
    /// </history>
    private async Task<MemberSalesmen> AddSalemanClubes(string job, string salemanId, string zone)
    {
      var memberSalesmen = new MemberSalesmen
      {
        CLMEMOPC_ID = 0,
        APPLICATION = _saleNew.saMembershipNum,
        CLAOPC_ID = job,
        OPC1 = string.Empty,
        OPC_PCT = 0,
        OPC_PCT2 = 0,
        OPC_CPT3 = 0,
        OPC_PCT4 = 0,
        STATUS = "A",
        ZONA = zone,
        OPC = salemanId
      };
      _lstMemberSalesmens.Add(memberSalesmen);
      //Agregamos el vendedor en intelligence Contracts
      return await BRMemberSalesman.SaveMemberSalesman(0, memberSalesmen, txtChangedBy.Text);
      //Context.User.User.peID);      
    }

    #endregion

    #region PaymentsSave
    /// <summary>
    /// Guarda los cambios del DataGrid y valida que no se repitan los rows
    /// </summary>
    /// <history>
    /// [jorcanche] 07/06/2016 created
    /// </history>
    private async Task PaymentsSave()
    {
      //Disparamos el mensaje que nos arroja  la operación del eliminado

      int resDelete = await BRPayments.DeletePaymentsbySale(_saleNew.saID);      
      if(resDelete < 1) UIHelper.ShowMessageResult("Payments", resDelete);
      foreach (var paysale in _payments)
      {       
        int resOperationEntity = await BREntities.OperationEntity(paysale, Model.Enums.EnumMode.Add);
        //Disparamos el mensaje que nos arroja  la operación del guardado
        if (resOperationEntity > 0) continue;
        UIHelper.ShowMessageResult("Payments", resOperationEntity);
      }
    }
    #endregion

    #region SetVolumenPercentage

    /// <summary>
    /// Establece los porcentajes de volumen de venta de los vendedores
    /// </summary>
    /// <history>
    /// [jorcanche] created 24062016
    /// </history>
    private void SetVolumenPercentage()
    {
      var closers = 1;
      //Obtenemos el numero de Closer + Exits
      if (_saleNew.saCloser2 != null) closers = closers + 1;
      if (_saleNew.saCloser3 != null) closers = closers + 1;
      if (_saleNew.saExit1 != null) closers = closers + 1;
      if (_saleNew.saExit2 != null) closers = closers + 1;
      //Obtenemos el porcentaje incial del volumen
      var vol = closers == 3 ? 34 : 100 / closers;

      //Porcentaje de volumen del closer 1
      _saleNew.saCloser1P = (byte)vol;

      //Porcentaje de volumen del closer 2
      if (vol == 34) vol = 33;
      _saleNew.saCloser2P = (byte)(_saleNew.saCloser2 != null ? vol : 0);

      //Porcentaje de volumen del Closer 3
      _saleNew.saCloser3P = (byte)(_saleNew.saCloser3 != null ? vol : 0);

      //Porcentaje de volumen del Exit 1
      _saleNew.saExit1P = (byte)(_saleNew.saExit1 != null ? vol : 0);

      //Porcentaje de volumen del Exit 2
      _saleNew.saExit2P = (byte)(_saleNew.saExit2 != null ? vol : 0);
    }

    #endregion

    #region GetSalesmenChanges

    /// <summary>
    /// Obtiene los cambios de vendedores
    /// </summary>
    /// <param name="salesmenChanges">Lista de cambios en los vendedores</param>
    /// <param name="authorizedBy">autorizado por o cambiado por</param>
    /// <history>
    /// [jorcanche] 03/06/2016 created 
    /// </history>    
    private bool GetSalesmenChanges(ref List<SalesmenChanges> salesmenChanges, ref string authorizedBy)
    {
      //PR´s
      SalesmanChanged(cmbsaPR1, cmbsaPR1.Text, 1, "PR", ref salesmenChanges);
      SalesmanChanged(cmbsaPR2, cmbsaPR2.Text, 2, "PR", ref salesmenChanges);
      SalesmanChanged(cmbsaPR3, cmbsaPR3.Text, 3, "PR", ref salesmenChanges);
      //Liners
      SalesmanChanged(cmbsaLiner1, cmbsaLiner1.Text, 1, "LINER", ref salesmenChanges);
      SalesmanChanged(cmbsaLiner2, cmbsaLiner2.Text, 2, "LINER", ref salesmenChanges);
      //Closers
      SalesmanChanged(cmbsaCloser1, cmbsaCloser1.Text, 1, "CLOSER", ref salesmenChanges);
      SalesmanChanged(cmbsaCloser2, cmbsaCloser2.Text, 2, "CLOSER", ref salesmenChanges);
      SalesmanChanged(cmbsaCloser3, cmbsaCloser3.Text, 3, "CLOSER", ref salesmenChanges);
      //Exit Closers
      SalesmanChanged(cmbsaExit1, cmbsaExit1.Text, 1, "EXIT", ref salesmenChanges);
      SalesmanChanged(cmbsaExit2, cmbsaExit2.Text, 2, "EXIT", ref salesmenChanges);

      //si hubo cambios de vendedores y no es el dia de la venta 
      if (salesmenChanges.Count <= 0 || _saleNew.saD >= _serverDate) return true;
      //Desplegamos el formulario para solicitar la persona que autorizo los cambios
      var frmEntryFieldData = new frmEntryFieldsData(salesmenChanges) {Owner = this };
      frmEntryFieldData.ShowDialog();
      authorizedBy = frmEntryFieldData.AuthorizedBy;
      // si esta vacio el cambio de autorizado por 

      if (string.IsNullOrEmpty(authorizedBy)) return false;
      //si se presiono el boton de cancelar 
      if (!frmEntryFieldData.cancel) return false;
      return true;
    }

    #endregion

    #region SalesmanChanged

    /// <summary>
    /// Indentifica si hubo un cambio en los SalesSalesmen de se así lo agrega en el Listado SalesmenChanges
    /// </summary>
    /// <param name="txt">TextBox que contiene el Id de la persona</param>
    /// <param name="newSalesman">ComboBox que contiene el Nombre de la persona</param>
    /// <param name="position">Posicion ejemplo Liner1 ó Liner 2 ó Liner 3</param>
    /// <param name="role">Role que tiene la persona</param>
    /// <param name="salesmenChanges">Listado de la persona que se modificaron agregando las persona anteriores</param>
    /// <history>
    /// [jorcanche] created 24062016
    /// </history>
    private void SalesmanChanged(ComboBox txt, string newSalesman, byte position, string role,
      ref List<SalesmenChanges> salesmenChanges)
    {
      var salesSalesmenOld = _saleOld.GetType().GetProperty(txt.Name.Substring(3)).GetValue(_saleOld);
      var strSalesSalesmen = salesSalesmenOld?.ToString() ?? string.Empty;
      if (txt.Text != strSalesSalesmen)
      {
        salesmenChanges.Add(new SalesmenChanges
        {
          NewSalesmanN = txt.Text,
          schNewSalesman = newSalesman,
          OldSalesmanN =
            string.IsNullOrEmpty(strSalesSalesmen) ? string.Empty : BRPersonnel.GetPersonnelById(strSalesSalesmen).peN,
          schOldSalesman = strSalesSalesmen,
          schPosition = position,
          roN = role
        });
      }
    }

    #endregion

    #region btnLog_Click

    /// <summary>
    /// Muestra el Log del actual Sale
    /// </summary>
    /// <history>
    /// [jorcanche] created 24062016
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      var salesLog = new frmSalesLog(_saleNew.saID, _saleNew.saMembershipNum) { Owner = this };
      salesLog.Show();
    }

    #endregion

    #region cbosast_SelectionChanged

    /// <summary>
    /// 
    /// </summary>
    ///<history>
    ///[jorcanche]  created 29062016 
    ///</history>
    private void cbosast_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //Definimos si es una venta de Upgrade o Downgrade    
      IsSaleUpdate();

      // habilitamos el editor de membresia anterior si es un Upgrade o un Downgrade
      txtsaRefMember.IsEnabled = _isSaleUpdate;

      // si no es un Upgrade o un Downgrade
      if (!_isSaleUpdate)
      {
        _saleNew.saReference = null;
        _saleNew.saOriginalAmount = 0;
        //se resta y se agrega a _sale.saGrossAmount, Como el resultado puede ser nulo hacemos una validación antes de asignarla
        var monto = _saleNew.saNewAmount - _saleNew.saOriginalAmount;
        if (monto != null)
          _saleNew.saGrossAmount = (decimal)monto;
      }
      //habilitamos el editor de tipo de membresia global si es un Upgrade o un Downgrade
      cbosamtGlobal.IsEnabled = _isSaleUpdate;

      //Si no se esta cargando el detalle de la venta 
      if (_loading)
      {
        //sugerimos que el tipo de membresia global sea igual al tipo de membresia de la venta 
        cbosamtGlobal.SelectedValue = cbosamt.SelectedValue;
      }
    }

    #endregion

    #region btnUndo_PreviewMouseLeftButtonDown

    /// <summary>
    /// Indica si se presiono el boton btnUndo
    /// </summary>
    /// <history>
    /// [jorcanche]  created 30/jul/2015
    /// </history>
    private void btnUndo_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      isPressedbtnUndo = true;
    }

    #endregion

    #region dtgSale_SelectionChanged
    /// <summary>
    /// Carga el Sale segun la seleccion del datagrid
    /// </summary>
    /// <history>
    /// [jorcanche] 05/jul/2016  created 
    /// </history>
    private async void dtgSale_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (btnClose.IsEnabled)
      {
        await LoadRecord();
      }
    }
    #endregion

    #region txt_PreviewTextInput
    /// <summary>
    /// Valida que solo se ingresen Numeros en los campos
    /// </summary>
    /// <history>
    /// [jorcanche] 05/jul/2016  created 
    /// </history>
    private void txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !char.IsDigit(Convert.ToChar(e.Text));
    }

    #endregion

    #region txt_KeyDown
    /// <summary>
    /// Tiene dos funciones 
    /// 1.- Si es un ComboBox funcionara nada mas cuando presionen la tecla eliminar
    ///     para quitar el registro que esta actualmente seleccionado y dejarlo vacio
    /// 2.- Si es un DateTimePicker ó un TextBox funcionara nada mas cuando presionen la tecla enter y 
    ///     cargara el Datagrid segun los Criterios ingresados.
    /// </summary>
    /// <history>
    /// [jorcanche] 05/jul/2016  created 
    /// </history>
    private void txt_KeyDown(object sender, KeyEventArgs e)
    {
      if (sender is ComboBox && e.Key == Key.Delete)
      {
        ((ComboBox)sender).SelectedIndex = -1;
      }
      else if ((sender is DateTimePicker || sender is TextBox) && e.Key == Key.Enter)
      {
        LoadGrid();
      }
    }
    #endregion

    #region Txtsagu_OnLostFocus

    /// <summary>
    /// Valida si existe el Id del Guest
    /// </summary>
    /// <history>
    /// [jorcanche]  created 05072016
    /// </history>
    private void Txtsagu_OnLostFocus(object sender, RoutedEventArgs e)
    {
      if (!isPressedbtnUndo) GetGuestName(_saleNew.sagu);
      isPressedbtnUndo = false;
    }

    #endregion

    #region Dgpayment_OnRowEditEnding

    /// <summary>
    /// Elimina los Rows Vacios, y como no se cancela el evento al final me agrega un nuevo row
    /// dejando así al final siempre un Row vacio
    /// </summary>
    /// <history>
    /// [jorcanche]  created 05/Jul/016
    /// </history>
    private void Dgpayment_OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      _payments.ToList().ForEach(payment =>
      {
        if (string.IsNullOrEmpty(payment.papt) && string.IsNullOrEmpty(payment.pacc))
        {
          _payments.Remove(payment);
        }
      });
    }

    #endregion

    #region Dgpayment_OnLostKeyboardFocus
    /// <summary>
    ///Habilitamos el Combobox para que luego el evento LoadedPaccColumn_OnHandler
    ///decida si lo habilita o lo dehabilita
    /// </summary>
    /// <history>
    /// [jorcanche]  created 05/Jul/016
    /// </history>
    private void Dgpayment_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      paccColumn.IsReadOnly = false;
    }
    #endregion

    #region LoadedPaccColumn_OnHandler

    /// <summary>
    /// Habilita ó deshabilita la columna Credit Card según si selecciono Tarjeta de Credito (CC)
    /// </summary>
    /// <history>
    /// [jorcanche]  created 05/Jul/016
    /// </history>
    private void LoadedPaccColumn_OnHandler(object sender, RoutedEventArgs e)
    {
      var payment = (Payment)dgpayment.CurrentItem;
      paccColumn.IsReadOnly = payment.papt != "CC";
    }

    #endregion

    #region SelectionChangedPaptColumn_OnHandler

    /// <summary>
    /// Valida la primera columna papt de modo que:
    /// 1.- Si selecciona CC habilita la segunda columana pacc y selecciona por default el primer registro.
    /// 2.- Si selecciona uno diferente a CC deshabilita la segunda columna pacc y quita la seleccion dejandolo en SelectedIndex = -1;
    /// </summary>
    /// <history>
    /// [jorcanche]  created 05/Jul/016
    /// </history>
    private void SelectionChangedPaptColumn_OnHandler(object sender, SelectionChangedEventArgs e)
    {
      var payment = (ComboBox)e.Source;
      if (payment.SelectedValue == null) return;

      var drSelected = dgpayment.ItemContainerGenerator.ContainerFromIndex(dgpayment.SelectedIndex) as DataGridRow;
      var dcCreditCard = dgpayment.Columns[1].GetCellContent(drSelected).Parent as DataGridCell;
      var combo = dcCreditCard.Content as ComboBox;

      //Si se escogio el Credit Card en la columana Payment Type automaticamente en la columan Credit Card te escogera
      //el primer registro de ser lo contrario lo dejara vacio
      if (combo != null) combo.SelectedIndex = payment.SelectedValue.ToString() != "CC" ? -1 : 1;
       
      //Valida que no se repitan en la columan Payment Type.
      //Al momento de seleccionar uno que ya existe te manda un mensaje 
      //y deja el combobox.SelectedIndex con  el valor -1 para que quede vacio el control
      if (_payments.Count(pay => pay.papt == payment.SelectedValue.ToString()) != 1)
      {
        UIHelper.ShowMessage("Payment Type must not be repeated");
        payment.SelectedIndex = -1;
        if (combo != null) combo.SelectedIndex = -1;
      }
    }

    #endregion    
  }
}

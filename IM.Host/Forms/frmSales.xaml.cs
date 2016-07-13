using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IM.Host.Enums;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model;
using System.Threading.Tasks;
using IM.Model.Helpers;
using IM.BusinessRules.BRIC;

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
    int _guId;
    DateTime _ServerDate, _CloseD;
    ////int txtsaCompany = 0;

    // Indica si se esta cargando el detalle de una venta
    private bool _loading;

    // Numero original de membresia anterior
    ////private string _membershipPreviousOriginal;

    // Clave original de la venta anterior
    /**************************************/
    //private int? _salePreviousOriginal;

    //Clave del huesped original
    /**************************************/
    //private int? _GuestIDOriginal;

    //Clave original de la venta anterior
    //private int _txtsaReference;

    // Respaldamos el monto de la venta original
    private decimal _SaleAmountOriginal;

    //El Monto de la venta
    private decimal _SaleAmount;

    //Vendedores
    List<SalesSalesman> _saleMen;

    //Objeto con los valores iniciales
    Sale _oldSale = new Sale();

    //Objeto para llenar el formulario
    Sale _sale = new Sale();

    //Payments
    List<Payment> _payments = new List<Payment>();

    //Sale Type Category 
    private string _saleTypeCategory;

    //Es una actualización de saled 
    private bool _isSaleUpdate;

    // Variable que indica si se encontro el tipo de membresia global
    private bool _validateMembershipTypeGlobal;

    private bool _searchPRbyTxt;

    #region frmSales

    public frmSales(EnumSale typeSale, int guId = 0)
    {
      InitializeComponent();
      typeSale = EnumSale.Sale;
      gprCriteria.Visibility = typeSale == EnumSale.Sale ? Visibility.Collapsed : Visibility.Visible;
      _guId = guId = 7751984;
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
      LoadCombosPayment();
      //Cargamos los ComboBox
      LoadPr();
      LoadLiner();
      LoadCloser();
      LoadExit();
      //Cargamos el DataGrid
      LoadGrid();
      
      //obtenemos la fecha de cierre
      CloseDate();
     
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
      cbosaExit1.ItemsSource = cbosaExit2.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "EXIT");
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
      cbosaCloser1.ItemsSource =
        cbosaCloser2.ItemsSource =
          cbosaCloser3.ItemsSource =
            await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "CLOSER");
      //Capitan de Closers
      cbosaCloserCaptain1.ItemsSource =
        await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "CLOSERCAPT");
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
      cbosaPR1.ItemsSource =
        cbosaPR2.ItemsSource =
          cbosaPR3.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "PR");
      //Capitanes de PRs
      cbosaPRCaptain1.ItemsSource =
        cbosaPRCaptain2.ItemsSource =
          cbosaPRCaptain3.ItemsSource =
            await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "PRCAPT");
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
      cbosaLiner1.ItemsSource =
        cbosaLiner2.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "LINER");
      //Capitan de Liners
      cbosaLinerCaptain1.ItemsSource =
        await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "LINERCAPT");
      //Podium
      cbosaPodium.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "PODIUM");
      //VLO
      cbosaVLO.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "VLO");
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
      //si se esta buscando
      if (_guId.Equals(0))
      {
        //establecemos los criterios de busqueda
        grdSale.ItemsSource =
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
        grdSale.ItemsSource = await BRSales.GetSalesShort(_guId);
      }
      //Establecemos el modo de solo lectura
      SetMode(Enums.EnumMode.modDisplay);
      //Cargamos el detalle de la venta
      //de la primera venta que aparece en el grid
      LoadRecord();
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
      if (grdSale.Items.Count > 0)
      {
        //Limpiamos los Source ya que seran nuevos Datos para los Trgets
        _oldSale = new Sale();
        _sale = new Sale();

        //Cargamos el OldSale Que sera la info Original q esta en la base   
        var sale = grdSale.SelectedItem != null ? (grdSale.SelectedItem as SaleShort)?.saID : (grdSale.Items[0] as SaleShort)?.saID;
        _oldSale = await BRSales.GetSalesbyId(sale);

        //Y Cargamos el Sale que aditará y sera el que se guardara en la base                        
        ObjectHelper.CopyProperties(_sale, _oldSale);

        //Asignamos el datacontext para cargar los controles        
        _loading = true;
        DataContext = _sale;
        _loading = false;

        //Se respalda el reference
        //_salePreviousOriginal = _sale.saReference; 

        //Indicamos si es una venta Out of Pendsing
        SetOutOfPending();

        //Cargamos los pagos de la venta      
        _payments = await BRPayments.GetPaymentsbySale(_sale.saID);
        dgpayment.DataContext = _payments;

        //Nombre del huesped
        GetGuestName(_sale.sagu);

        //Respaldamos el Guest ID Original        
        //_GuestIDOriginal = _sale.sagu;

        //Respaldamos el monto de la venta original
        _SaleAmount = _SaleAmountOriginal = GetSaleAmount();

        //Obtenemos los vendedores
        GetSalemen();

        //si la venta es de una fecha cerrada, no permitomos eliminar ventas
        if (IsClosed()) btnDelete.IsEnabled = false;
      }
    }

    #endregion

    #region LoadSale
    /// <summary>
    /// Carga la variable de Sale
    /// </summary>
    /// <history>
    /// [jorcanche]  created 12/07/2016
    /// </history>
    private async void LoadSale()
    {
      var sale = grdSale.SelectedItem != null ? (grdSale.SelectedItem as SaleShort)?.saID : (grdSale.Items[0] as SaleShort)?.saID;
      _oldSale = await BRSales.GetSalesbyId(sale);
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
    ///[jorcanche] created 24/06/2016
    ///</history> 
    private async void GetGuestName(int? guestId)
    {
      if (guestId == null) return;
      var guest = await BRGuests.GetGuest((int) guestId);
      if (guest != null)
      {
        lblGuestName.Text = $"{guest.guLastName1} {guest.guFirstName1}";
      }
      else
      {
        _sale.sagu = null;
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

      if (!string.IsNullOrEmpty(txtsaLiner1.Text) && salemen.Find(sm => sm.smpe == txtsaLiner1.Text) == null)
        salemen.Add(new SalesSalesman {smpe = txtsaLiner1.Text});
      if (!string.IsNullOrEmpty(txtsaLiner2.Text) && salemen.Find(sm => sm.smpe == txtsaLiner2.Text) == null)
        salemen.Add(new SalesSalesman {smpe = txtsaLiner2.Text});
      if (!string.IsNullOrEmpty(txtsaCloser1.Text) && salemen.Find(sm => sm.smpe == txtsaCloser1.Text) == null)
        salemen.Add(new SalesSalesman {smpe = txtsaCloser1.Text});
      if (!string.IsNullOrEmpty(txtsaCloser2.Text) && salemen.Find(sm => sm.smpe == txtsaCloser2.Text) == null)
        salemen.Add(new SalesSalesman {smpe = txtsaCloser2.Text});
      if (!string.IsNullOrEmpty(txtsaCloser3.Text) && salemen.Find(sm => sm.smpe == txtsaCloser3.Text) == null)
        salemen.Add(new SalesSalesman {smpe = txtsaCloser3.Text});
      if (!string.IsNullOrEmpty(txtsaExit1.Text) && salemen.Find(sm => sm.smpe == txtsaExit1.Text) == null)
        salemen.Add(new SalesSalesman {smpe = txtsaExit1.Text});
      if (!string.IsNullOrEmpty(txtsaExit2.Text) && salemen.Find(sm => sm.smpe == txtsaExit2.Text) == null)
        salemen.Add(new SalesSalesman {smpe = txtsaExit2.Text});
      //Agregamos los datos del los SaleMen

      _saleMen = new List<SalesSalesman>();

      foreach (var item in salemen)
      {
        var salesSalesMens =
          BRSalesSalesmen.GetSalesSalesmens(new SalesSalesman {smpe = item.smpe, smsa = Convert.ToInt32(txtsaID.Text)});
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
          item.smsa = _sale.saID;
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
      return _saleTypeCategory != "DG"
        ? _sale.saGrossAmount : _sale.saNewAmount ?? 0;
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
      _saleTypeCategory = await BRSaleTypes.GetStstcOfSaleTypeById(_sale.sast);
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
    private void SetMode(Enums.EnumMode mode)
    {
      bool blnEnable = mode != Enums.EnumMode.modDisplay ? true : false;
      //Grid principal
      grdSale.IsEnabled = !blnEnable;
      //criterios de busqueda
      //brincamos este paso ya que segun el modo si es Global o Sale se ocultara los criterios de busqueda asi que no ha necesidad de inabilitarlos
      //Botones
      btnEdit.IsEnabled = (!blnEnable && App.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.Standard) &&
                           grdSale.Items.Count > 0);
      //Solo permite eliminar ventanas si tiene permiso super especial de ventas
      btnDelete.IsEnabled = (!blnEnable && App.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.SuperSpecial) &&
                             grdSale.Items.Count > 0);
      btnSave.IsEnabled = btnUndo.IsEnabled = blnEnable;
      btnClose.IsEnabled = !blnEnable;
      //Autenticacion automatica
      grbChangedBy.IsEnabled = blnEnable;
      if (blnEnable && App.User.AutoSign)
      {
        txtChangedBy.Text = App.User.User.peID;
        txtPwd.Password = App.User.User.pePwd;
      }
      else
      {
        txtChangedBy.Text = txtPwd.Password = string.Empty;
      }
      //no permitimos modificar la ventana si es de una fecha cerrada
      bool closen = !IsClosed();
      blnEnable = (blnEnable && closen);
      //deshabilitamos los datos generales
      EnabledGeneral(false);
      //Vendedores
      gSalesmen.IsEnabled = blnEnable;
      //boton de venta de vendedores
      btnSalesSalesmen.IsEnabled = blnEnable;
      //Capitanes
      grbCaptains.IsEnabled = blnEnable;
      //habilitamos los campos que no se obtienen de Clubes
      if (blnEnable)
      {
        // gGeneral.IsEnabled = true;
        //Numero de memebresia
        txtsaMembershipNum.IsEnabled = true;
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
      if (!(mode == Enums.EnumMode.modDisplay) && IsClosed())
      {
        //Permitimos modificar la venta para marcarla como procesabel o para desmarcarla de procesable 
        // si su fecha de cancelacion no esta en una fecha cerrada
        if (!chksaProc.IsChecked.Value || !difdate(txtsaProcD.Text, _CloseD))
        {
          fraProcessable.IsEnabled = true;
        }
        //Permitimos modificar la venta para cancelarla o para descancelarla
        //si su fecha de cancelacion no esta en una fecha cerrada
        if (!chksaCancel.IsChecked.Value || !difdate(txtsaCancelD.Text, _CloseD))
        {
          //fraCancelled.IsEnabled = true;
          chksaCancel.IsEnabled = true;
        }
        //Comentarios
        txtsaComments.IsEnabled = false;
      }
    }

    #endregion

    #region difdate

    /// <summary>
    /// Diferencia entre dos fechas 
    /// </summary>
    /// <param name="date"> Fecha en de tipo string</param>
    /// <param name="Closed"> Fecha de venta cerrada de tipo DateTime</param>
    /// <history>
    /// [jorcanche]  created 28062016
    /// </history>
    private bool difdate(string date, DateTime Closed)
    {
      DateTime result;
      //Si tiene una fecha especifica      
      if (DateTime.TryParse(date, out result))
      {
        //si la fecha es menor o igual a la fecha de cierre
        return result <= Closed ? true : false;
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
      bool retornar = (difdate(txtsaD.Text, _CloseD.Date) || difdate(txtsaProcD.Text, _CloseD.Date) ||
                       difdate(txtsaCancelD.Text, _CloseD.Date));
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
      //Validamos el tipo de venta       //Validamos el nuevo monto
      return !ValidateSaleType() ? false : !ValidateNewAmount() ? false : true;
    }

    #endregion

    #region ValidateNewAmount

    /// <summary>
    /// Valida el nuevo monto
    /// </summary>
    /// <history>
    /// [jorcanche]  created 28062016
    /// </history>
    private bool ValidateNewAmount()
    {
      if (Convert.ToDecimal(txtsaNewAmount.Text.Trim('$')) == 0)
      {
        UIHelper.ShowMessage("Specify the new amount");
        txtsaNewAmount.Focus();
      }
      return true;
    }

    #endregion

    #region ValidateSaleType

    /// <summary>
    /// Valida el tipo de venta 
    /// </summary>
    /// <history>
    /// [jorcanche]  created 28062016
    /// </history>
    private bool ValidateSaleType()
    {
      if (cbosast.SelectedIndex.Equals(-1))
      {
        UIHelper.ShowMessage("Specify a Sale Type");
        cbosast.Focus();
        return false;
      }
      return true;
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
      if (!string.IsNullOrEmpty(txtsaID.Text))
      {
        //si es una secretaria 
        // if (App.User.HasRole(EnumRole.Secretary))
        //{
        //if (ValidateSalesSalesmen())
        //{
        //Obtenermos los vendedores
        GetSalemen();
        var saleAmount = GetSaleAmount();
        var salessalesmen = new frmSalesSalesmen
          (_saleMen, _sale.saID,saleAmount < 0 ? -saleAmount : saleAmount, _SaleAmountOriginal)
        { Owner = this};
        salessalesmen.ShowDialog();
        //}
        //}
      }
    }

    #endregion

    #region grdSale_MouseLeftButtonDown

    /// <summary>
    /// Cuando se selecciona un row en el grid 
    /// </summary>
    /// <history>
    /// [jorcanche]  created 28062016
    /// </history>
    private void grdSale_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (btnClose.IsEnabled)
      {
        LoadRecord();
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
      var cbo = sender as ComboBox;
      var txt = (TextBox) FindName(cbo.Name.Replace("cbo", "txt"));
      if (txt == null) return;
      if (cbo.SelectedIndex != -1 || txt.Text == string.Empty)
      {
        if (cbo.SelectedValue != null)
        {
          if (!_searchPRbyTxt)
          {
            txt.Text = cbo.SelectedValue.ToString();
          }
        }
      }
    }

    #endregion

    #region txt_LostFocus

    /// <summary>
    /// Valida cuando se pierde el focus de los vendedores y de los capitanes 
    /// </summary>
    /// <history>
    /// [jorcanche]  created 24062016
    /// </history>
    private void txt_LostFocus(object sender, RoutedEventArgs e)
    {
      var txt = (TextBox) sender;
      var cbo = (ComboBox) FindName(txt.Name.Replace("txt", "cbo"));
      _searchPRbyTxt = true;
      if (!string.IsNullOrEmpty(txt.Text))
      {
        // validamos que el motivo de indisponibilidad exista en los activos
        var name = txt.Name.Trim('1', '2', '3').Substring(5).Replace("Captain", "CAPT").ToUpper();
        var pr = BRPersonnel.GetPersonnelById(txt.Text, name);
        if (pr == null)
        {
          UIHelper.ShowMessage($"The {name.Replace("CAPT", " Captain")} not exist");
          txt.Text = string.Empty;
          txt.Focus();
        }
        else
        {
          cbo.SelectedValue = pr.peID;
          txt.Text = pr.peID;
        }
      }
      else
      {
        cbo.SelectedIndex = -1;
      }
      _searchPRbyTxt = false;
    }

    #endregion




    /// <summary>
    /// Obtiene la fecha de cierre
    /// </summary>
    /// <history>
    /// [jorcanche]  created 01072016
    /// </history>
    public async void CloseDate()
    {
      _CloseD = await BRSales.GetSalesCloseD(App.User.SalesRoom.srID);
    }

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
      if (_payments.Count == 0) return true;
      foreach (var item in _payments)
      {
        if (item.papt == "CC" && item.pacc == null)
        {
          foundbad = true;
        }
      }
      if (foundbad)
      {
        UIHelper.ShowMessage("Please specify the Credit Card Type");
        return false;
      }
      return true;
    }

    #endregion

    /// <summary>
    /// Valida el Grid:
    /// 1.- Que tenga al menos unregistro
    /// 2.- Que no tenga registros repetidos
    /// </summary>
    /// <returns></returns>
    private bool ValidateGrid()
    {
      return true;
    }

    /// <summary>
    /// Indica si un tipo de venta es una actualizacion de otra venta
    /// </summary>
    ///<history>
    ///[jorcanche] 24062016
    ///</history>
    private async void IsSaleUpdate()
    {
      if (cbosast.SelectedValue != null)
        _isSaleUpdate = await BRSaleTypes.GetstUpdateOfSaleTypeById(cbosast.SelectedValue.ToString(), 1);
    }

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
    private async void ValidateMembershipTypeGlobal()
    {
      //Validamos que ingrese el tipo de membresia global
      if (cbosamtGlobal.SelectedIndex == -1)
      {
        UIHelper.ShowMessage("Specify the Membership Type Globale");
        cbosamtGlobal.Focus();
        _validateMembershipTypeGlobal = false;
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
                _validateMembershipTypeGlobal = false;
              }
              break;
            case "DG": //Downgrade
              //Validamos el tipo de membresia no sea mayo que el tipo de membresia anterior}
              if (membershipLevel > membershipPreviousLevel)
              {
                UIHelper.ShowMessage(
                  "In a Downgrade, the Membership Type Global can't be greater than the Membership Type Previous");
                _validateMembershipTypeGlobal = false;
              }
              break;
          }
        } //Si no encontramos la membresia anterior 
        else
        {
          UIHelper.ShowMessage("Membership Number Previous not found");
          txtsaRefMember.Focus();
          _validateMembershipTypeGlobal = false;
        }
      }
      _validateMembershipTypeGlobal = true;
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
    private bool Validate()
    {
      //Validamos quien hizo el cambio de contraseña
      if (!ValidateChangedBy(txtChangedBy, txtPwd)) return false;

      //Validamos los datos generales
      if (!ValidateGeneral())
      {
        Panel.SetZIndex(gGeneral, 1);
        return false;
      }
      //Validamos los vendedores
      if (!ValidateSalesmen())
      {
        Panel.SetZIndex(gGeneral, 1);
        return false;
      }
      //Validamos que los datos der la ventana existan
      if (!ValidateExist())
      {
        Panel.SetZIndex(gGeneral, 1);
        return false;
      }
      return true;
    }

    #endregion

    #region ValidateExist

    private ValidationData _validateExist;

    /// <summary>
    /// Valida que los datos de la ventana existan
    /// </summary>   
    /// <history>
    /// [jorcanche] 06/06/2016
    /// </history>
    private bool ValidateExist()
    {
      var saId = !string.IsNullOrEmpty(txtsaID.Text) ? Convert.ToInt32(txtsaID.Text) : 0;
      ValidateExist(saId);

      if (_validateExist == null) return true;
      //Desplegamos el mensaje de error
      UIHelper.ShowMessage(_validateExist.Message);
      //Establecemos el foco en el control que tiene el error      

      #region switch ValidationData

      switch (_validateExist.Focus)
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
          txtsaPR1.Focus();
          break;
        case "PR2":
          txtsaPR2.Focus();
          break;
        case "PR3":
          txtsaPR3.Focus();
          break;
        case "PRCaptain1":
          txtsaPRCaptain1.Focus();
          break;
        case "PRCaptain2":
          txtsaPRCaptain2.Focus();
          break;
        case "PRCaptain3":
          txtsaPRCaptain3.Focus();
          break;
        case "Liner1":
          txtsaLiner1.Focus();
          break;
        case "Liner2":
          txtsaLiner2.Focus();
          break;
        case "LinerCaptain":
          txtsaLinerCaptain1.Focus();
          break;
        case "Closer1":
          txtsaCloser1.Focus();
          break;
        case "Closer2":
          txtsaCloser2.Focus();
          break;
        case "Closer3":
          txtsaCloser3.Focus();
          break;
        case "CloserCaptain":
          txtsaCloserCaptain1.Focus();
          break;
        case "Exit1":
          txtsaExit1.Focus();
          break;
        case "Exit2":
          txtsaExit2.Focus();
          break;
        case "Podium":
          txtsaPodium.Focus();
          break;
        case "VLO":
          txtsaVLO.Focus();
          break;
      }
      return false;
      #endregion
    }

    #endregion

    public async void ValidateExist(int saId)
    {
      _validateExist = await BRSales.ValidateSale(txtChangedBy.Text, EncryptHelper.Encrypt(txtPwd.Password), saId,
        txtsaMembershipNum.Text, Convert.ToInt32(txtsagu.Text), cbosast.SelectedValue.ToString(),
        txtsasr.Text, txtsalo.Text, txtsaPR1.Text, txtsaPR2.Text, txtsaPR3.Text, txtsaPRCaptain1.Text,
        txtsaPRCaptain2.Text, txtsaPRCaptain2.Text, txtsaLiner1.Text, txtsaLiner2.Text, txtsaLinerCaptain1.Text,
        txtsaCloser1.Text,
        txtsaCloser2.Text, txtsaCloser3.Text, txtsaCloserCaptain1.Text, txtsaExit1.Text, txtsaExit2.Text,
        txtsaPodium.Text, txtsaVLO.Text);
    }

    #region ValidateSalesmen

    /// <summary>
    /// Valida los vendedores
    /// </summary>
    /// <history>
    /// [jorcanche]  created 24062016
    /// </history>
    private bool ValidateSalesmen()
    {
      //no validar cuando sea PHONE SALE
      if (!chksaByPhone.IsChecked.Value)
      {
        //Validamos el PR1
        if (string.IsNullOrEmpty(txtsaPR1.Text))
        {
          UIHelper.ShowMessage("Specify the PR 1");
          return false;
        }
        //Validamos el Liner1
        if (string.IsNullOrEmpty(txtsaLiner1.Text))
        {
          UIHelper.ShowMessage("Specify the Liner 1");
          return false;
        }
      }
      return true;
    }

    #endregion

    #region ValidateChangedBy

    /// <summary>
    /// Valida que no este vacio el usuario y el Password
    /// </summary>
    /// <param name="txtChangedBy">Usuario Id</param>
    /// <param name="txtPwd">Passwors del usuario</param>
    /// <returns></returns>
    private bool ValidateChangedBy(TextBox txtChangedBy, PasswordBox txtPwd)
    {
      //Validamos quien hizo el cambio 
      if (string.IsNullOrEmpty(txtChangedBy.Text))
      {
        UIHelper.ShowMessage("Specify who is making the change");
        return false;
      }
      //Validamos la contraseña de quien hizo el cambio
      if (string.IsNullOrEmpty(txtPwd.Password))
      {
        UIHelper.ShowMessage("Specify your password.");
        return false;
      }
      return true;
    }

    #endregion

    #region ValidateGeneral

    /// <summary>
    /// Valida los datos Generales
    /// </summary>
    /// <history>
    /// [jorcanche]  created 2406216
    /// </history>
    private bool ValidateGeneral()
    {
      //obtenemos la fecha de cierre
      CloseDate();
      //Validamos la fecha de venta no este en una fecha cerrada
      if (!ValidateCloseDate(txtsaD)) return false;
      //Validamos el numero de membresia
      if (string.IsNullOrEmpty(txtsaMembershipNum.Text))
      {
        UIHelper.ShowMessage("Specify the Membership Number");
        txtsaMembershipNum.Focus();
        return false;
      }
      //validamos la fecha procesable
      if (!ValidateProcessableDate()) return false;
      //Validamos la fecha de cancelacion 
      if (!ValidateCancelDate()) return false;
      //Validamos el huesped
      if (string.IsNullOrEmpty(txtsagu.Text))
      {
        UIHelper.ShowMessage("Specify the Guest ID");
        txtsagu.Focus();
        return false;
      }
      //Validamos la sala de ventas
      if (string.IsNullOrEmpty(txtsasr.Text))
      {
        UIHelper.ShowMessage("Specify the Sales Room");
        txtsasr.Focus();
        return false;
      }
      //Validamos la location
      if (string.IsNullOrEmpty(txtsalo.Text))
      {
        UIHelper.ShowMessage("Specify the Locarion");
        txtsalo.Focus();
        return false;
      }
      //Validamos el tipo de venta 
      if (!ValidateSaleType()) return false;
      //Validamo la membresia anterior si es un Upgrade o un Downgrade
      IsSaleUpdate();
      if (_isSaleUpdate)
      {
        if (string.IsNullOrEmpty(txtsaRefMember.Text))
        {
          UIHelper.ShowMessage("Specify the Previous Memebership");
          txtsaRefMember.Focus();
          return false;
        }
      }
      //Validamos el tipo de membresia 
      if (cbosamt.SelectedIndex == -1)
      {
        UIHelper.ShowMessage("Specify the Membership Type");
        cbosamt.Focus();
        return false;
      }
      ValidateMembershipTypeGlobal();
      //Validamos el tipo de mebresia global
      if (!_validateMembershipTypeGlobal)
      {
        return false;
      }
      //Validamos el apellido
      if (string.IsNullOrEmpty(txtsaLastName1.Text))
      {
        UIHelper.ShowMessage("Specify the Last Name");
        txtsaRefMember.Focus();
        return false;
      }
      //Validamos el nombre
      if (string.IsNullOrEmpty(txtsaFirstName1.Text))
      {
        UIHelper.ShowMessage("Specify the First Name");
        txtsaRefMember.Focus();
        return false;
      }
      //Validamos el nuevo monto
      if (!ValidateNewAmount())
      {
        return false;
      }
      //validamos los pagos
      if (!ValidatePagos())
      {
        return false;
      }
      return true;
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
      if (DateTime.TryParse(txt.Text, out result))
      {
        //Si la condicion es valida y el editor esta habilitado
        if (txt.IsEnabled)
        {
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
          if (Convert.ToDateTime(txt.Text) <= _CloseD)
          {
            UIHelper.ShowMessage($"It,s not allowed to make {des} for a closed date");
            txt.Focus();
            return false;
          }
        }
      }
      return true;
    }

    #endregion

    /// <summary>
    /// Modo edicion
    /// </summary>
    ///<history>
    ///[jorcanche] created  24062016
    ///</history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      SetMode(Enums.EnumMode.modEdit);
    }

    /// <summary>
    /// Elimina una Sale
    /// </summary>
    /// <history>
    /// [jorcanche]  created 30062016
    /// </history>    
    private async void btnDelete_Click(object sender, RoutedEventArgs e)
    {
      //Preguntamos al usuario si en verdad desea eliminar la venta
      var result = MessageBox.Show("Are you sure you want to delete this sale ?", "Delete", MessageBoxButton.OKCancel,
        MessageBoxImage.Exclamation, MessageBoxResult.OK);
      if (result != MessageBoxResult.OK) return;
      await BRSales.DeleteSale(Convert.ToInt32(txtsaID.Text));
      //LoadRecord();
      LoadGrid();
    }

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


    #region btnUndo_Click

    /// <summary>
    /// Deshace los cambios hechos a una venta
    /// </summary>
    ///<history>
    ///[jorcanche] created 24/06/2016
    ///</history>
    private void btnUndo_Click(object sender, RoutedEventArgs e)
    {
      //si no hay ventas
      if (grdSale.Items.Count == 0)
      {
        //si no se esta buscando
        if (_guId > 0)
        {
          _sale = new Sale();
          DataContext = null;
          return;
        }
      }
      //Si hay ventas ó Si se esta buscando
      LoadRecord();

      //Establecemos el mode de solo lectura
      SetMode(Enums.EnumMode.modDisplay);
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
      //txtsaCancelD.Text = chksaCancel.IsChecked != null && chksaCancel.IsChecked.Value ? BRHelpers.GetServerDate().ToString("dd/MM/yyyy") : string.Empty;
      //MessageBox.Show(_sale.saCancelD.ToString());   
      if (_loading) return;
      _sale.saCancelD = _sale.saCancel ? (DateTime?) BRHelpers.GetServerDate() : null;
      txtsaCancelD.Text = _sale.saCancelD.ToString();
    }

    #endregion

    #region btnSave_Click

    /// <summary>
    /// Permite guardar una venta
    /// </summary>
    /// <history>
    /// [jorcanche] 03/06/2016
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      var salesmenChanges = new List<SalesmenChanges>();
      string authorizedBy = string.Empty;
      //Validamos los datos
      if (!Validate()) return;
      //obtenermos los cambios de vendedores
      if (GetSalesmenChanges(ref salesmenChanges, ref authorizedBy))
      {
        //Guardamos la venta
        Save(salesmenChanges, authorizedBy);
      }
    }

    #endregion

    /// <summary>
    /// Guarda una venta
    /// </summary>
    /// <history>
    /// [jorcanche] 07/06/2016 created
    /// </history>
    private async void Save(List<SalesmenChanges> salesmenChanges, string authorizedBy)
    {
      //Procesable     
      if (_sale.saProcD == null) _sale.saProc = false;
      //Cancelada
      if (_sale.saCancelD == null) _sale.saCancel = false;

      //Establecemos los procentajes de volumen de venta de los vendedores
      SetVolumenPercentage();
      IsSaleUpdate();
      //Establecemos la fecha de procesable de referencia
      if (!_isSaleUpdate && _sale.saProcRD != _sale.saProcD)
      {
        _sale.saProcRD = _sale.saProcD;
      }
      if (_isSaleUpdate && _sale.saProcRD != null)
      {
        _sale.saProcRD = _sale.saProcD;
      }
      //Si son diferentes los Objetos es que hubo cambios 
      if (!ObjectHelper.IsEquals(_sale, _oldSale))
      {
        //Validamos
        var sMsjSaleGeneral = ValidateHelper.ValidateForm(gSaleGeneral, "Sale");
        if (sMsjSaleGeneral == "")
        {
          var nRes = await BREntities.OperationEntity(_sale, Model.Enums.EnumMode.edit);
          //Disparamos el mensaje que nos arroja  la operación del guardado
          UIHelper.ShowMessageResult("Sale", nRes);
        }
        else
        //Mostramos el mensaje de la validación
          UIHelper.ShowMessage(sMsjSaleGeneral);
      }
      //Guardamosa los pagos
      PaymentsSave();

      //Si cambio de venta anterior
      if (txtsaRefMember.IsEnabled && _sale.saReference != _oldSale.saReference)
      {
        //si tenia venta anterior, la marcamos como actualizada
        if (_oldSale.saReference != null)
        {
          //marcamos la venta anterior actual como actualizada
          await BRSales.UpdateSaleUpdated(_oldSale.saReference, false);
        }
        //actualizamos los vendedores del huesped en base a los vendedores de la venta 
        await BRSales.UpdateSaleUpdated(_sale.saReference, true);
      }

      //Actualiza los vendedores del huesped en base a los vendedores de la venta 
      await BRSalesSalesmen.UpdateGuestSalesmen(_sale.sagu, _sale.saID);

      //Si cambio de guest ID      
      if (_oldSale.sagu != _sale.sagu)
      {
        //marcamos como venta el guest Id Nuevo
        await BRSales.UpdateGuestSale(_sale.sagu, true);

        //Desmarcamos como venta el Guest Id anterior si ya no le quedan ventas
        var sales = await BRSales.GetCoutSalesbyGuest(_sale.sagu);
        if (sales == 0)
        {
          await BRSales.UpdateGuestSale(_oldSale.sagu, false);
        }
      }
      //Recargamos la información
      await LoadRecord();

      //Guardamos el historico de la venta
      await BRSales.SaveSaleLog(_sale.sagu, App.User.SalesRoom.srHoursDif, App.User.User.peID);

      //Guardamos las ventas de los vendedores 
      var saleAmount = GetSaleAmount();
      Classes.SalesSalesmen.SaveSalesSalesmen(_saleMen, _sale.saID, saleAmount < 0 ? -saleAmount : saleAmount,
        _SaleAmountOriginal);

      //Guardamos el movimiento de venta del huesped
      await
        BRGuests.SaveGuestMovement(_sale.sagu, EnumGuestsMovementsType.Sale, txtChangedBy.Text, Environment.MachineName,
          ComputerHelper.GetIpMachine());

      //Guardamos los cambios de vendedores y la persona que autorizo los cambiuos
      if (!string.IsNullOrEmpty(authorizedBy))
      {
        //SaveSalesmenChanges(salesmenChanges, authorizedBy);
        foreach (var salesmenChange in salesmenChanges)
        {
          await BRSalesSalesmen.SaveSalesmenChanges
            (_sale.sagu, authorizedBy, txtChangedBy.Text, salesmenChange.roN, salesmenChange.schPosition,
              salesmenChange.schOldSalesman, salesmenChange.schNewSalesman);
        }
      }
      //Actualizamos vendedores en Intelligence Contracts
      SaveMemberSalesmenClubes();
    }

    /// <summary>
    /// Guarda vendedores en Intelligence Contracts
    /// </summary>
    /// <history>/
    /// [jorcanche] created 08/07/2016
    /// </history>
    private async void SaveMemberSalesmenClubes()
    {
      string sPRs = string.Empty, sLiners = string.Empty, sClosers = string.Empty, sExits = string.Empty;

      //Obtenemos los vendedores de Intelligence Contracts 
      var salesman = await BRMemberSalesman.GetMemberSalesmen(_sale.saMembershipNum, "ALL", "ALL", "OPC,LINER,CLOSER,EXIT" );

      //Agregamos / Actualizamos los vendedores de Origos den Intelligence contracts
     
      //PR's
      sPRs = await SaveMemberSalesmanClubesByRole(salesman, "OPC", "OPC", _sale.saPR1, sPRs);
      sPRs = await SaveMemberSalesmanClubesByRole(salesman, "OPC", "OPC", _sale.saPR2, sPRs);
      sPRs = await SaveMemberSalesmanClubesByRole(salesman, "OPC", "OPC", _sale.saPR3, sPRs);

      //Liners
      sLiners = await SaveMemberSalesmanClubesByRole(salesman, "LINER", "LIN", _sale.saLiner1, sLiners);
      sLiners = await SaveMemberSalesmanClubesByRole(salesman, "LINER", "LIN", _sale.saLiner2, sLiners);

      //Closers
      sClosers = await SaveMemberSalesmanClubesByRole(salesman, "CLOSER", "CLOS", _sale.saCloser1, sClosers);
      sClosers = await SaveMemberSalesmanClubesByRole(salesman, "CLOSER", "CLOS", _sale.saCloser2, sClosers);
      sClosers = await SaveMemberSalesmanClubesByRole(salesman, "CLOSER", "CLOS", _sale.saCloser3, sClosers);

      //Exit Closers
      sExits = await SaveMemberSalesmanClubesByRole(salesman, "EXIT", "JR", _sale.saExit1, sExits);
      sExits = await SaveMemberSalesmanClubesByRole(salesman, "EXIT", "JR", _sale.saExit2, sExits);

      //Eliminamos los vendedores en Intelligence Contracts que no estan en Intelligence Marketing
      DeleteSalesmenClubes("OPC", sPRs);
      DeleteSalesmenClubes("LINER", sLiners);
      DeleteSalesmenClubes("CLOSER", sClosers);
      DeleteSalesmenClubes("EXIT", sExits);
    }

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
      var lstMemberSalesmens = await BRMemberSalesman.GetMemberSalesmen(_sale.saMembershipNum, "ALL", "ALL", role);
      if (lstMemberSalesmens.Any())
      {
        //si el vendedor no esta en la lista de vendedores de Origos, lo eliminamos en Intellligence Contracts
        lstMemberSalesmens.ForEach(async ms =>
        {
          if (!salesmen.Contains(ms.OPC))
          {
            await BRMemberSalesman.DeleteMemberSalesman(ms.CLMEMOPC_ID);
          }
        });
      }
    }


    /// <summary>
    /// Guarda un vendedor por rol en Intelligence Contracts
    /// </summary>
    /// <history>
    /// [jorcanche] created 08/07/2016
    /// </history>
    private async Task<string> SaveMemberSalesmanClubesByRole(List<MemberSalesmen> lstSalesmen, string role, string job,string id, string salesmen)
    {
      string salemanId, zone;
      //si tiene una clave de vendedor de origos
      if (!string.IsNullOrEmpty(id))
      {
        //agregamos la clave de vendedor en origos
        salesmen = salesmen + id + " ";
        //select peID, peN, peSalesmanID from Personnel order by peN
        //obtenemos la clave del vendedor de Intelligence Contracts relaionando al vendedor de Origos
        var personel = BRPersonnel.GetPersonnelById(id);
        //Validamos si existe el vendedor en IM
        if (personel != null)
        {
          UIHelper.ShowMessage($"The personnel {id} does not exists on Origos");
          return salesmen;
        }
        //Validamos si el vendedor de origos tiene relacionada una clave de Intelligence Contracts
        salemanId = personel.peSalesmanID;
        if (string.IsNullOrEmpty(salemanId))
        {
          UIHelper.ShowMessage($"The personnel {id} is not associated with a Intelligence Contract's salesman.");
          return salesmen;
        }
        //Validamos si existe la clave de intelligence Contracts
        zone = _sale.sasr;
        if (!await BRMemberSalesman.ExistsSalesman(salemanId, zone))
        {
          UIHelper.ShowMessage($"The salesman  {salemanId} from zone {zone} does not exists on Intelligence Contracts");
          return salesmen;
        }
        //Localizamos el vendedor de Intelligence contracts
        var memberSalesmens = lstSalesmen.FirstOrDefault(sa => sa.OPC == salemanId && sa.CLAOPC_ID == job);
        //si no se localizo 
        if (memberSalesmens == null)
        {
          //lo agregamos en Intelligence contracts
          AddSalemanClubes(ref lstSalesmen, job, salemanId, zone);
        }
        else
        {
          // si tiene el rol solicitado
          if (memberSalesmens.Role == role)
          {
            memberSalesmens.CLAOPC_ID = job;
            memberSalesmens.STATUS = "A";
            memberSalesmens.ZONA = _sale.sasr;
            //Actualizamos el vendedor de Intelligence contracts 
            await BRMemberSalesman.SaveMemberSalesman(memberSalesmens.RECNUM, memberSalesmens, App.User.User.peID);
          }
          else //Si no tiene el rol solicitado
          {
            //Agregamos el vendedor de intelligence contracts
            AddSalemanClubes(ref lstSalesmen,job,salemanId,zone); 
          }
        }
      }
      return salesmen;
    }

    /// <summary>
    /// Agrega un vendedor a un afiliacion en Intelligence Contracts
    /// </summary>
    /// <param name="lstSalesmen"></param>
    /// <param name="job"></param>
    /// <param name="salemanId"></param>
    /// <param name="zone"></param>
    /// <history>
    /// [jorcanche] 11/07/2016 created 
    /// </history>
    private void AddSalemanClubes(ref List<MemberSalesmen> lstSalesmen, string job, string salemanId, string zone)
    {
      var memberSalesmen = new MemberSalesmen
      {
        CLMEMOPC_ID = 0,
        APPLICATION = _sale.saMembershipNum,
        CLAOPC_ID = job,
        OPC1 = string.Empty,
        OPC_PCT = 0,
        OPC_PCT2 = 0,
        OPC_CPT3 = 0,
        OPC_PCT4 = 4,
        STATUS = "A",
        ZONA = zone,
        OPC = salemanId
      };
      lstSalesmen.Add(memberSalesmen);
      //Agregamos el vendedor en intelligence Contracts
      var membersalesaleman = BRMemberSalesman.SaveMemberSalesman(0,memberSalesmen,  App.User.User.peID);
    }


    /// <summary>
    /// Guarda los cambios del DataGrid y valida que no se repitan los rows
    /// </summary>
    /// <history>
    /// [jorcanche] 07/06/2016 created
    /// </history>
    private async void PaymentsSave()
    {
      //Disparamos el mensaje que nos arroja  la operación del eliminado
      if (_payments.Any())
      {
        int resDelete = await BRPayments.DeletePaymentsbySale(_sale.saID);
        if (resDelete > 0) return;
        UIHelper.ShowMessageResult("Payments", resDelete);
      }     
      foreach (var paysale in _payments)
      {
        paysale.pasa = _sale.saID;
        int resOperationEntity = await BREntities.OperationEntity(paysale, Model.Enums.EnumMode.add);
        //Disparamos el mensaje que nos arroja  la operación del guardado
        if (resOperationEntity > 0) return;
        UIHelper.ShowMessageResult("Payments", resOperationEntity);
      }
    }

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
      if (_sale.saCloser2 != null) closers = closers + 1;
      if (_sale.saCloser3 != null) closers = closers + 1;
      if (_sale.saExit1 != null) closers = closers + 1;
      if (_sale.saExit2 != null) closers = closers + 1;
      //Obtenemos el porcentaje incial del volumen
      var vol = closers == 3 ? 34 : 100/closers;

      //Porcentaje de volumen del closer 1
      _sale.saCloser1P = (byte) vol;

      //Porcentaje de volumen del closer 2
      if (vol == 34) vol = 33;
      _sale.saCloser2P = (byte) (_sale.saCloser2 != null ? vol : 0);

      //Porcentaje de volumen del Closer 3
      _sale.saCloser3P = (byte) (_sale.saCloser3 != null ? vol : 0);

      //Porcentaje de volumen del Exit 1
      _sale.saExit1P = (byte) (_sale.saExit1 != null ? vol : 0);

      //Porcentaje de volumen del Exit 2
      _sale.saExit2P = (byte) (_sale.saExit2 != null ? vol : 0);
    }

    #endregion

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
      SalesmanChanged(txtsaPR1, cbosaPR1.Text, 1, "PR", ref salesmenChanges);
      SalesmanChanged(txtsaPR2, cbosaPR2.Text, 2, "PR", ref salesmenChanges);
      SalesmanChanged(txtsaPR3, cbosaPR3.Text, 3, "PR", ref salesmenChanges);
      //Liners
      SalesmanChanged(txtsaLiner1, cbosaLiner1.Text, 1, "LINER", ref salesmenChanges);
      SalesmanChanged(txtsaLiner2, cbosaLiner2.Text, 2, "LINER", ref salesmenChanges);
      //Closers
      SalesmanChanged(txtsaCloser1, cbosaCloser1.Text, 1, "CLOSER", ref salesmenChanges);
      SalesmanChanged(txtsaCloser2, cbosaCloser2.Text, 2, "CLOSER", ref salesmenChanges);
      SalesmanChanged(txtsaCloser3, cbosaCloser3.Text, 3, "CLOSER", ref salesmenChanges);
      //Exit Closers
      SalesmanChanged(txtsaExit1, cbosaExit1.Text, 1, "EXIT", ref salesmenChanges);
      SalesmanChanged(txtsaExit2, cbosaExit2.Text, 2, "EXIT", ref salesmenChanges);

      //si hubo cambios de vendedores y no es el dia de la venta 
      if (salesmenChanges.Count > 0 && Convert.ToDateTime(txtsaD.Text) < _ServerDate)
      {
        //Desplegamos el formulario para solicitar la persona que autorizo los cambios
        var frmEntryFieldData = new frmEntryFieldsData(salesmenChanges);
        frmEntryFieldData.ShowDialog();
        authorizedBy = frmEntryFieldData.AuthorizedBy;
        // si esta vacio el cambio de autorizado por 
        if (string.IsNullOrEmpty(authorizedBy)) return false;
        //si se presiono el boton de cancelar 
        if (!frmEntryFieldData.cancel) return false;
      }
      return true;
    }

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
    private void SalesmanChanged(TextBox txt, string newSalesman, byte position, string role,
      ref List<SalesmenChanges> salesmenChanges)
    {
      var salesSalesmenOld = _oldSale.GetType().GetProperty(txt.Name.Substring(3)).GetValue(_oldSale);
      string strSalesSalesmen = salesSalesmenOld != null ? salesSalesmenOld.ToString() : string.Empty;
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
      var salesLog = new frmSalesLog(_sale.saID, _sale.saMembershipNum) {Owner = this};
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
        _sale.saReference = null;
        _sale.saOriginalAmount = 0;
        //se resta y se agrega a _sale.saGrossAmount, Como el resultado puede ser nulo hacemos una validación antes de asignarla
        var monto = _sale.saNewAmount - _sale.saOriginalAmount;
        if (monto != null)
          _sale.saGrossAmount = (decimal) monto;
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





    private void Dgpayment_OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      //List<string> distinctStatuses = items.Select(item => item.Status)
      //                               .Distinct()
      //                               .ToList();
      //If you only want the count, you can simplify this further:

      //int distinctStatusCount = items.Select(item => item.Status).Distinct().Count();
    }

    /// <summary>
    /// Valida que no se repita registros en el datagrid
    /// </summary>
    /// <history>
    /// [jorcanche]  created 05072016
    /// </history>
    private void dgpayment_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.Column.Header.ToString() != "Payment Type") return;
      var cbo = (ComboBox) e.EditingElement;
      if (cbo.SelectedValue == null) return;
      var papt = cbo.SelectedValue.ToString();
      // if (papt == string.Empty) return;
      var firstOrDefault = _payments.FirstOrDefault(pay => pay.papt == papt);
      if (firstOrDefault == null) return;
      UIHelper.ShowMessage("Payment Type must not be repeated");
      cbo.SelectedIndex = -1;
      e.Cancel = true;
    }

    #region dgpayment_BeginningEdit

    /// <summary>
    /// Valida antes de editar el grid de pagos 
    /// </summary>
    /// <history>
    /// [jorcanche] created 24062016
    /// </history>
    private void dgpayment_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if (e.Column.SortMemberPath != "pacc") return;
      var payment = (Payment) e.Row.Item;
      e.Cancel = payment.papt != "CC";
    }

    #endregion

    /// <summary>
    /// Valida si existe el Id del Guest
    /// </summary>
    /// <history>
    /// [jorcanche]  created 05072016
    /// </history>
    private void Txtsagu_OnLostFocus(object sender, RoutedEventArgs e)
    {          
      GetGuestName(_sale.sagu);
    }
  }
}

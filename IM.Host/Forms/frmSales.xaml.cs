using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IM.Host.Enums;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model;
using SaleMan = IM.Host.Classes.SaleMan;
using System.Threading.Tasks;
using IM.Model.Helpers;
using System.Windows.Data;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmSales.xaml
  /// </summary>
  public partial class frmSales : Window
  {
    int _guId;
    DateTime _ServerDate, _CloseD;
   // int txtsaCompany = 0;
    //Clave original de la venta anterior
   // string txtsaReference;
    //Clave del huesped original
    int? GuestIDOriginal;
    // Respaldamos el monto de la venta original
    decimal SaleAmountOriginal;
    //El Monto de la venta
    decimal SaleAmount;

    //Vendedores
    List<SalesSalesman> _saleMen;

    //Objeto con los valores iniciales
    Sale _oldSale = new Sale();

    //Objeto para llenar el formulario
    Sale _sale = new Sale();

    //Payments
    List<Payment> _payments = new List<Payment>();


    string txtsaCloser1P;
    string txtsaCloser2P;
    string txtsaCloser3P;
    string txtsaExit1P;
    string txtsaExit2P;
    private bool _searchPRbyTxt;

    public frmSales(EnumSale typeSale, int guId = 0)
    {
      InitializeComponent();
      typeSale = EnumSale.Sale;
      gprCriteria.Visibility = typeSale == EnumSale.Sale ? Visibility.Collapsed : Visibility.Visible;
      _guId = guId = 7752186;
    }

    private void btnSalesmen_Click(object sender, RoutedEventArgs e)
    {
      Panel.SetZIndex(gGeneral, 0);
    }

    /// <summary>
    /// Muestra el grid que contiene la info General
    /// </summary>       
    private void btnGeneral_Click(object sender, RoutedEventArgs e)
    {
      Panel.SetZIndex(gGeneral, 1);

    }

    private bool ValidateGuest(bool guBookCanc, bool guSale, bool guShow)
    {
      //Validamos que no sea un booking cancelado 
      if (guBookCanc)
      {
        UIHelper.ShowMessage("Cancelled booking");
        return false;
      }
      //validamos  los permisos del usuario
      if (App.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.None))
      {
        UIHelper.ShowMessage("Access denied.");
        return false;
      }
      //Solo lectura
      if (App.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.ReadOnly))
      {
        //si no tiene un registro en el formulario que desea abrir
        if (!guSale)
        {
          UIHelper.ShowMessage("You have read access.");
          return false;
        }
        else
        {
          //int guid = 12345;
        }
      }
      else
      {
        //int guid = 1234;
      }

      //Validamos que tenga show para darle cupones de comida o ventas
      if (!guShow)
      {
        UIHelper.ShowMessage("This option should have a show marked.");
        return false;
      }
      return true;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      loadcombo();
      //Cargamos los ComboBox
      LoadComboBox();
      //Cargamos el DataGrid
      LoadGrid();
      //obtenemos la fecha de cierre
      _CloseD = BRSales.GetSalesCloseD(App.User.SalesRoom.srID);
      //Fecha inicial 
      dtpCsaDFrom.Value = Convert.ToDateTime("2012/01/01");
      //Cargamos la fecha del servidor
      dtpCsaDTo.Value = _ServerDate = BRHelpers.GetServerDate();
      //si el sistema esta en modo de solo lectura, no permitimos modificar, ni eliminar ventas           
    }

    /// <summary>
    /// Carga el grid de ventas
    /// </summary>
    private void LoadGrid()
    {
      //si se esta buscando
      if (_guId.Equals(0))
      {
        //establecemos los criterios de busqueda
        grdSale.ItemsSource = BRSales.GetSalesShort(string.IsNullOrEmpty(txtCsagu.Text) ? 0 : Convert.ToInt32(txtCsagu.Text),
                                                    string.IsNullOrEmpty(txtCsaID.Text) ? 0 : Convert.ToInt32(txtCsaID.Text),
                                                    string.IsNullOrEmpty(txtCsaMembershipNum.Text) ? "ALL" : txtCsaMembershipNum.Text,
                                                    string.IsNullOrEmpty(txtCName.Text) ? "ALL" : txtCName.Text,
                                                    string.IsNullOrEmpty(txtCsals.Text) ? "ALL" : txtCsals.Text,
                                                    string.IsNullOrEmpty(txtCsasr.Text) ? "ALL" : txtCsasr.Text,
                                                    dtpCsaDFrom.Value, dtpCsaDTo.Value);
      }
      else
      {
        grdSale.ItemsSource = BRSales.GetSalesShort(_guId);
      }
      //Establecemos el modo de solo lectura
      SetMode(Enums.EnumMode.modDisplay);
      //Cargamos el detalle de la venta
      //de la primera venta que aparece en el grid
      LoadRecord();
    }

    private void txtsaCancelID(bool cancel)
    {
      if (string.IsNullOrEmpty(txtsaCancelD.Text))
      {
        if (Convert.ToDateTime(txtsaCancelD.Text) > BRHelpers.GetServerDate().Date)
        {
          UIHelper.ShowMessage("Cancellation date can not be greater than today.");
          cancel = true;
        }
        else
        {
          if (Convert.ToDateTime(txtsaCancelD.Text) > Convert.ToDateTime(txtsaD.Text))
          {
            UIHelper.ShowMessage("Cancellation date can not be before sale date.");
            cancel = true;
          }
          else
          {
            if (!chksaCancel.IsChecked.Value)
            {
              chksaCancel.IsChecked = true;
            }
          }
        }
        if (!chksaCancel.IsChecked.Value)
        {
          chksaCancel.IsChecked = true;
        }
      }
    }

    private void LoadRecord()
    {
      //Si hay alguna venta
      if (grdSale.Items.Count > 0)
      {
        //Limpiamos los Source ya que seran nuevos Datos para los Trgets
        _oldSale = new Sale();
        _sale = new Sale();

        //Cargamos el OldSale Que sera la info Original q esta en la base               
        _oldSale = BRSales.GetSalesbyID(grdSale.SelectedItem != null ? (grdSale.SelectedItem as SaleShort).saID : (grdSale.Items[0] as SaleShort).saID);

        //Y Cargamos el Sale que aditará y sera el que se guardara en la base        
        ObjectHelper.CopyProperties(_sale, _oldSale);

        //Asignamos el datacontext para cargar los controles
        DataContext = _sale;

        //Indicamos si es una venta Out of Pendsing
        SetOutOfPending();

        //Cargamos los pagos de la venta      
        _payments = BRPayments.GetPaymentsbySale(_sale.saID);
        dgpayment.DataContext = _payments;

        //Nombre del huesped
        lblGuestName.Text = GetGuestName(_sale.sagu);

        //Respaldamos el Guest ID Original
        GuestIDOriginal = _sale.sagu;

        //Respaldamos el monto de la venta original
        SaleAmount = SaleAmountOriginal = GetSaleAmount();

        //Obtenemos los vendedores
        GetSalemen();

        //si la venta es de una fecha cerrada, no permitomos eliminar ventas
        if (IsClosed()) btnDelete.IsEnabled = false;
      }
    }

    /// <summary>
    /// Carga las combos del Datagrid de los    
    /// </summary>
    /// <history>
    /// [jorcanche] created 22/06/2016
    /// </history>
    private async void loadcombo()
    {
      paptColumn.ItemsSource = await BRPaymentTypes.GetPaymentTypes(1);
      paccColumn.ItemsSource = await BRCreditCardTypes.GetCreditCardTypes(nStatus: 1);
    }

    /// <summary>
    /// Obtiene el nombre completo del Guest del Sale
    /// </summary>
    /// <param name="guestID">Id del Guest</param>
    ///<history>
    ///[jorcanche] created 24/06/2016
    ///</history> 
    private string GetGuestName(int? guestID)
    {
      if (guestID == null)
      {
        UIHelper.ShowMessage("Guest ID not found");
        return string.Empty;
      }
      Guest guest = BRGuests.GetGuestById(Convert.ToInt32(guestID));
      if (guest == null)
      {
        UIHelper.ShowMessage("Guest not found");
        return string.Empty;
      }
      return $"{guest.guLastName1} {guest.guFirstName1}";
    }

    #region GetSalemen
    /// <summary>
    /// Obtiene los vendedores
    /// </summary>
    ///<history>
    ///[jorcanche] created 24/06/2016
    ///</history> 
    private void GetSalemen()
    {
      List<SalesSalesman> salemen = new List<SalesSalesman>();

      if (!string.IsNullOrEmpty(txtsaLiner1.Text) && salemen.Find(delegate (SalesSalesman sm) { return sm.smpe == txtsaLiner1.Text; }) == null)
        salemen.Add(new SalesSalesman { smpe = txtsaLiner1.Text });
      if (!string.IsNullOrEmpty(txtsaLiner2.Text) && salemen.Find(delegate (SalesSalesman sm) { return sm.smpe == txtsaLiner2.Text; }) == null)
        salemen.Add(new SalesSalesman { smpe = txtsaLiner2.Text });
      if (!string.IsNullOrEmpty(txtsaCloser1.Text) && salemen.Find(delegate (SalesSalesman sm) { return sm.smpe == txtsaCloser1.Text; }) == null)
        salemen.Add(new SalesSalesman { smpe = txtsaCloser1.Text });
      if (!string.IsNullOrEmpty(txtsaCloser2.Text) && salemen.Find(delegate (SalesSalesman sm) { return sm.smpe == txtsaCloser2.Text; }) == null)
        salemen.Add(new SalesSalesman { smpe = txtsaCloser2.Text });
      if (!string.IsNullOrEmpty(txtsaCloser3.Text) && salemen.Find(delegate (SalesSalesman sm) { return sm.smpe == txtsaCloser3.Text; }) == null)
        salemen.Add(new SalesSalesman { smpe = txtsaCloser3.Text });
      if (!string.IsNullOrEmpty(txtsaExit1.Text) && salemen.Find(delegate (SalesSalesman sm) { return sm.smpe == txtsaExit1.Text; }) == null)
        salemen.Add(new SalesSalesman { smpe = txtsaExit1.Text });
      if (!string.IsNullOrEmpty(txtsaExit2.Text) && salemen.Find(delegate (SalesSalesman sm) { return sm.smpe == txtsaExit2.Text; }) == null)
        salemen.Add(new SalesSalesman { smpe = txtsaExit2.Text });
      //Agregamos los datos del los SaleMen

      _saleMen = new List<SalesSalesman>();

      foreach (var item in salemen)
      {
        var salesSalesMens = BRSalesSalesmen.GetSalesSalesmens(new SalesSalesman { smpe = item.smpe, smsa = Convert.ToInt32(txtsaID.Text) });
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



    /// <summary>
    /// devuelve el monto de la venta que se debe considerar
    /// </summary>
    /// <history>
    /// [jorcanche] created 25062016
    /// </history>
    private decimal GetSaleAmount()
    {
      //si la venta no es un Dwngrade       
      return GetSaleTypeCategory() != "DG" ? _sale.saGrossAmount //decimal.Parse( txtsaGrossAmount.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"))
                                           : _sale.saNewAmount == null ? 0 : _sale.saNewAmount.Value; //ecimal.Parse(txtsaNewAmount.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
    }

    /// <summary>
    /// Obtiene la categoria de un tipo de venta
    /// </summary>
    /// <history>
    /// [jorcanche] created 25062016
    /// </history>
    private string GetSaleTypeCategory()
    {
      //cbosast.SelectedIndex = 0;
      //return BRSaleTypes.GetSalesTypes(1, new SaleType() { stID = _sale.sast }).FirstOrDefault().ststc;//cbosast.SelectedValue.ToString() }).FirstOrDefault().ststc;
      //TODO: jorcanche repara ésta función
      return "";
    }

    /// <summary>
    /// Indica si es un venta que salio de pendiente
    /// </summary>
    /// <history>
    /// [jorcanche]  created 25062016
    /// </history>
    private void SetOutOfPending()
    {
      //Una  venta es Out Of Pendin si es procesable con fecha de venta distinta de la fecha de procesable
      lblOutOfPending.Visibility = (chksaProc.IsChecked == true) && (txtsaD.Text != txtsaProcD.Text) ? Visibility.Visible : Visibility.Hidden;
    }

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
      btnEdit.IsEnabled = (!blnEnable && App.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.Standard) && grdSale.Items.Count > 0);
      //Solo permite eliminar ventanas si tiene permiso super especial de ventas
      btnDelete.IsEnabled = (!blnEnable && App.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.SuperSpecial) && grdSale.Items.Count > 0);
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
        ///ss
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
    private void EnabledGeneral(bool enabled)
    {
      txtsaID.IsEnabled = fraProcessable.IsEnabled = txtsaMembershipNum.IsEnabled =
      txtsaD.IsEnabled = txtsaCancelD.IsEnabled = chksaCancel.IsEnabled = chksaByPhone.IsEnabled = txtsagu.IsEnabled =
      lblGuestName.IsEnabled = txtsals.IsEnabled = txtsalo.IsEnabled = txtsasr.IsEnabled =
      chksaUpdated.IsEnabled = cbosast.IsEnabled = txtsaRefMember.IsEnabled = txtsaProcRD.IsEnabled =
      cbosamt.IsEnabled = cbosamtGlobal.IsEnabled = txtsaLastName1.IsEnabled = txtsaLastName2.IsEnabled =
      txtsaFirstName1.IsEnabled = txtsaFirstName2.IsEnabled = grbAmounts.IsEnabled = txtsaClosingCost.IsEnabled =
      txtsaOverPack.IsEnabled = grbPayments.IsEnabled = grbAmounts_Copy.IsEnabled = chksaByPhone.IsEnabled = txtsaComments.IsEnabled = enabled;
    }

    /// <summary>
    /// Determina si una venta esta en una fecha cerrada
    /// </summary>
    /// <returns></returns>
    private bool IsClosed()
    {
      //DateTime Result;
      //if (!DateTime.TryParse(txtsaD.Text, out Result) || !DateTime.TryParse(txtsaProcD.Text, out Result)|| !DateTime.TryParse(txtsaCancelD.Text, out Result)) 
      bool retornar = (difdate(txtsaD.Text, _CloseD.Date) || difdate(txtsaProcD.Text, _CloseD.Date) || difdate(txtsaCancelD.Text, _CloseD.Date));
      return retornar;

    }

    private async void LoadComboBox()
    {
      //Tipo de membresia y Tipo de membresia Global
      cbosamtGlobal.ItemsSource = cbosamt.ItemsSource = await BRMemberShipTypes.GetMemberShipTypes(1);
      //Tipo de venta
      cbosast.ItemsSource = await BRSaleTypes.GetSalesTypes(1);
      //PR´s
      cbosaPR1.ItemsSource = cbosaPR2.ItemsSource = cbosaPR3.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "PR");
      //Capitanes de PRs
      cbosaPRCaptain1.ItemsSource = cbosaPRCaptain2.ItemsSource = cbosaPRCaptain3.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "PRCAPT");
      //Liners
      cbosaLiner1.ItemsSource = cbosaLiner2.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "LINER");
      //Capitan de Liners
      cbosaLinerCaptain1.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "LINERCAPT");
      //Closers
      cbosaCloser1.ItemsSource = cbosaCloser2.ItemsSource = cbosaCloser3.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "CLOSER");
      //Exit Closers
      cbosaExit1.ItemsSource = cbosaExit2.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "EXIT");
      //Capitan de Closers
      cbosaCloserCaptain1.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "CLOSERCAPT");
      //Podium
      cbosaPodium.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "PODIUM");
      //VLO
      cbosaVLO.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "VLO");

    }

    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      LoadGrid();
    }


    /// <summary>
    /// Valida los datos necesarios para definir las ventas de los vendedores
    /// </summary>
    /// <returns></returns>
    private bool ValidateSalesSalesmen()
    {
      //Validamos el tipo de venta       //Validamos el nuevo monto
      return !ValidateSaleType() ? false : !ValidateNewAmount() ? false : true;
    }

    /// <summary>
    /// Valida el nuevo monto
    /// </summary>
    /// <returns></returns>
    private bool ValidateNewAmount()
    {
      if (Convert.ToDecimal(txtsaNewAmount.Text.Trim('$')) == 0)
      {
        UIHelper.ShowMessage("Specify the new amount");
        txtsaNewAmount.Focus();
      }
      return true;
    }

    /// <summary>
    /// Valida el tipo de venta 
    /// </summary>
    /// <returns></returns>
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
        frmSalesSalesmen salessalesmen = new frmSalesSalesmen(_saleMen, _sale.saID, (saleAmount < 0) ? -saleAmount : saleAmount, SaleAmountOriginal);
        salessalesmen.ShowDialog();
        //}
        //}
      }
    }


    private void grdSale_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (btnClose.IsEnabled)
      {
        LoadRecord();
      }
    }

    private void grdSale_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
      //if (btnClose.IsEnabled)
      //{
      //  LoadRecord();
      //}
    }

    /// <summary>
    /// Valida cuando se cambia de Item los Combox de los vendedores y de los capitanes 
    /// </summary>
    /// <history>
    /// [jorcanche]  created 24062016
    /// </history>
    private void cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var cbo = sender as ComboBox;
      var txt = (TextBox)FindName(cbo.Name.Replace("cbo", "txt"));
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

    /// <summary>
    /// Valida cuando se pierde el focus de los vendedores y de los capitanes 
    /// </summary>
    /// <history>
    /// [jorcanche]  created 24062016
    /// </history>
    private void txt_LostFocus(object sender, RoutedEventArgs e)
    {
      var txt = (TextBox)sender;
      var cbo = (ComboBox)FindName(txt.Name.Replace("txt", "cbo"));
      _searchPRbyTxt = true;
      if (!string.IsNullOrEmpty(txt.Text))
      {
        // validamos que el motivo de indisponibilidad exista en los activos
        var name = txt.Name.Trim(new char[] { '1', '2', '3' }).Substring(5).Replace("Captain", "CAPT");
        Personnel PR = BRPersonnel.GetPersonnelById(txt.Text, name);
        if (PR == null)
        {
          UIHelper.ShowMessage($"The {name.Replace("CAPT", " Captain")} not exist");
          txt.Text = string.Empty;
          txt.Focus();
        }
        else
        {
          cbo.SelectedValue = PR.peID;
          txt.Text = PR.peID;
        }
      }
      else
      {
        cbo.SelectedIndex = -1;
      }
      _searchPRbyTxt = false;
    }

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

    /// <summary>
    /// Valida que los datos de la ventana existan
    /// </summary>   
    /// <history>
    /// [jorcanche] 06/06/2016
    /// </history>
    private bool ValidateExist()
    {
      int saID = !string.IsNullOrEmpty(txtsaID.Text) ? Convert.ToInt32(txtsaID.Text) : 0;
      List<ValidationData> result = BRSales.ValidateSale(txtChangedBy.Text, EncryptHelper.Encrypt(txtPwd.Password), saID, txtsaMembershipNum.Text, Convert.ToInt32(txtsagu.Text), cbosast.SelectedValue.ToString(),
                            txtsasr.Text, txtsalo.Text, txtsaPR1.Text, txtsaPR2.Text, txtsaPR3.Text, txtsaPRCaptain1.Text, txtsaPRCaptain2.Text,
                            txtsaPRCaptain2.Text, txtsaLiner1.Text, txtsaLiner2.Text, txtsaLinerCaptain1.Text, txtsaCloser1.Text, txtsaCloser2.Text,
                            txtsaCloser3.Text, txtsaCloserCaptain1.Text, txtsaExit1.Text, txtsaExit2.Text, txtsaPodium.Text, txtsaVLO.Text);

      if (!string.IsNullOrEmpty(result.FirstOrDefault().Focus))
      {
        //Desplegamos el mensaje de error
        UIHelper.ShowMessage(result.FirstOrDefault().Message);
        //Establecemos el foco en el control que tiene el error

        switch (result.FirstOrDefault().Focus)
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
      }
      return true;
    }

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

    /// <summary>
    /// Valida los datos Generales
    /// </summary>
    /// <history>
    /// [jorcanche]  created 2406216
    /// </history>
    private bool ValidateGeneral()
    {
      //obtenemos la fecha de cierre
      _CloseD = BRSales.GetSalesCloseD(App.User.SalesRoom.srID);
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
      if (IsSaleUpdate())
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
      //Validamos el tipo de mebresia global
      if (!ValidateMembershipTypeGlobal())
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
    string MembershipTypePrevious = string.Empty;
    byte? MembershipPreviousLevel = 0;
    byte? MembershipLevel = 0;

    /// <summary>
    /// Valida el tipo de membresia global
    /// </summary>
    /// <returns></returns>
    private bool ValidateMembershipTypeGlobal()
    {
      //Validamos que ingrese el tipo de membresia global
      if (cbosamtGlobal.SelectedIndex == -1)
      {
        UIHelper.ShowMessage("Specify the Membership Type Globale");
        cbosamtGlobal.Focus();
        return false;
      }
      if (IsSaleUpdate())
      {
        //Tipo de membresia de la emembresia anterior
        MembershipTypePrevious = GetMembershipType(txtsaRefMember.Text);
        //si encontramos la membresia anterior 
        if (!string.IsNullOrEmpty(MembershipTypePrevious))
        {
          //obtenermos el nivel de la memebresia antrior
          //MembershipPreviousLevel = await  LoadMemberShipTypes(MembershipTypePrevious); 
          Membership();

          switch (GetSaleTypeCategory())
          {
            //Upgrade
            case "UG":
              //Validamos el tipo de membresia no sea menor que el tipo de membresia anterior 
              if (MembershipLevel < MembershipPreviousLevel)
              {
                UIHelper.ShowMessage("In a Upgrade, the Membership Type Global can't be less than the Membership Type Previous");
                return false;
              }
              break;
            //Downgrade
            case "DG":
              //Validamos el tipo de membresia no sea mayo que el tipo de membresia anterior}
              if (MembershipLevel > MembershipPreviousLevel)
              {
                UIHelper.ShowMessage("In a Downgrade, the Membership Type Global can't be greater than the Membership Type Previous");
                return false;
              }
              break;
          }
        }//Si no encontramos la membresia anterior 
        else
        {
          UIHelper.ShowMessage("Membership Number Previous not found");
          txtsaRefMember.Focus();
          return false;
        }
      }
      return true;
    }

    private async void Membership()
    {
      var member = await BRMemberShipTypes.GetMemberShipTypes(1, new MembershipType { mtID = MembershipTypePrevious }); ;
      MembershipPreviousLevel = member.FirstOrDefault().mtLevel;

      //obtenermos el nivel de la membresia       
      var membershipType = await BRMemberShipTypes.GetMemberShipTypes(1, new MembershipType { mtID = cbosamtGlobal.SelectedValue.ToString() });
      MembershipLevel = membershipType.FirstOrDefault().mtLevel;
    }

    private async Task<byte?> LoadMemberShipTypes(string MembershipTypePrevious = "")
    {
      var TypePrevious = string.IsNullOrEmpty(MembershipTypePrevious) ? cbosamtGlobal.SelectedValue.ToString() : MembershipTypePrevious;
      var membershipType = await BRMemberShipTypes.GetMemberShipTypes(1, new MembershipType { mtID = TypePrevious });
      return membershipType.FirstOrDefault().mtLevel;
    }
    /// <summary>
    /// Obtiene el tipo de una membresia
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private string GetMembershipType(string MembershipNum)
    {
      return BRSales.GetSalesbyID(memebershipNum: MembershipNum).samtGlobal;
    }

    /// <summary>
    /// Indica si un tipo de venta es una actualizacion de otra venta
    /// </summary>
    ///<history>
    ///[jorcanche] 24062016
    ///</history>
    private bool IsSaleUpdate()
    {
      //return BRSaleTypes.GetSalesTypes(1, new SaleType { stID = cbosast.SelectedValue.ToString() }).
        //FirstOrDefault().stUpdate;
        //TODO: revisa ésta función
      return true;
    }


    /// <summary>
    /// Valida la fecha de cancelacion
    /// </summary>
    ///<history>
    ///[jorcanche] 24062016
    ///</history>
    private bool ValidateCancelDate()
    {
      //sim esta cancelada 
      if (chksaCancel.IsChecked.Value)
      {
        //Validamos que la fecha de procesable no este en un fecha cerrada
        if (!ValidateCloseDate(txtsaCancelD, "Cancellation date")) return false;
        if (Convert.ToDateTime(txtsaCancelD.Text) < Convert.ToDateTime(txtsaD.Text))
        {
          UIHelper.ShowMessage("Cancellation date can not be before sale date.");
          txtsaCancelD.Focus();
          return false;
        }
        //validamos que la fecha de cancelacio no sea menor a la fecha procesable
        if (chksaProc.IsChecked.Value)
        {
          if (Convert.ToDateTime(txtsaCancelD.Text) < Convert.ToDateTime(txtsaProcD.Text))
          {
            UIHelper.ShowMessage("Processable date can no be after Cancellation date.");
            txtsaProcD.Focus();
            return false;
          }
        }
      }
      return true;
    }


    /// <summary>
    /// Valida la fecha procesable
    /// </summary>
    ///<history>
    ///[jorcanche] 24062016
    ///</history>
    private bool ValidateProcessableDate()
    {
      //si es procesable
      if (chksaProc.IsChecked.Value)
      {
        //Validamos que la fecha de prosesable no este en una fecha cerrada 
        if (!ValidateCloseDate(txtsaProcD)) return false;
        //Validamos que la fecha de procesable no sea menor a la fecha de venta
        if (Convert.ToDateTime(txtsaProcD.Text).Date < Convert.ToDateTime(txtsaD.Text).Date)
        {
          UIHelper.ShowMessage("Procesable date can not be before sale date");
          txtsaD.Focus();
          return false;
        }
      }
      return true;
    }
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

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
      //Preguntamos al usuario si en verdad desea eliminar la venta
      var result = MessageBox.Show("Are you sure you want to delete this sale ?", "Delete", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.OK);
      if (result == MessageBoxResult.OK)
      {
        BRSales.DeleteSale(Convert.ToInt32(txtsaID.Text));
        //grdSale.Items.Remove(grdSale.SelectedItem);
        //LoadRecord();
        LoadGrid();
      }
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

    /// <summary>
    /// Agrega la fecha gdel servidor (actual) al txtsaCancelID cuando hace check al chksaCancel y viseversa
    /// </summary>
    ///<history>
    ///[jorcanche] created 24062016
    ///</history>
    private void chksaCancel_Checked(object sender, RoutedEventArgs e)
    {
      txtsaCancelD.Text = chksaCancel.IsChecked.Value ? BRHelpers.GetServerDate().ToString("dd/MM/yyyy") : string.Empty;
    }

    /// <summary>
    /// Permite guardar una venta
    /// </summary>
    /// <history>
    /// [jorcanche] 03/06/2016
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      var salesmenChanges = new List<SaleMan>();
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

    /// <summary>
    /// Guarda una venta
    /// </summary>
    /// <history>
    /// [jorcanche] 07/06/2016 created
    /// </history>
    private async void Save(List<SaleMan> salesmenChanges, string authorizedBy)
    {
      //int sales = 0;
      //Procesable
      if (string.IsNullOrEmpty(txtsaProcD.Text)) chksaProc.IsChecked = false;
      //Cancelada
      if (string.IsNullOrEmpty(txtsaCancelD.Text)) chksaCancel.IsChecked = false;

      //Establecemos los procentajes de volumen de venta de los vendedores
      SetVolumenPorcentage();

      //Establecemos la fecha de procesable de referencia
      if (!IsSaleUpdate() && Convert.ToDateTime(txtsaProcD.Text) != Convert.ToDateTime(txtsaProcD.Text))
      {
        txtsaProcRD.Text = Convert.ToDateTime(txtsaProcD.Text).ToShortDateString();
      }
      if (IsSaleUpdate() && string.IsNullOrEmpty(txtsaProcRD.Text))
      {
        txtsaProcRD.Text = Convert.ToDateTime(txtsaProcD.Text).ToShortDateString();
      }

      //Es una modificacion
      //Actualizamos el DataContext con el focus del btnSave
      btnSave.Focus();
      //Si son diferentes los Objetos es que hubo cambios 
      int nRes = 0;
      if (!ObjectHelper.IsEquals(_sale, _oldSale))
      {
        //Validamos
        string sMsjSaleGeneral = ValidateHelper.ValidateForm(gSaleGeneral, "Sale");
        if (sMsjSaleGeneral == "")
        {
          nRes = await BREntities.OperationEntity(_sale, Model.Enums.EnumMode.edit);
          //Disparamos el mensaje que nos arroja  la operación del guardado
          UIHelper.ShowMessageResult("Sale", nRes);
        }
        else
          //Mostramos el mensaje de la validación
          UIHelper.ShowMessage(sMsjSaleGeneral);
      }
      //Guardamosa los pagos
      PaymentsSave();
    }

    /// <summary>
    /// Guarda los cambios del DataGrid y valida que no se repitan los rows
    /// </summary>
    /// <history>
    /// [jorcanche] 07/06/2016 created
    /// </history>
    private async void PaymentsSave()
    {
      BRPayments.DeletePaymentsbySale(_sale.saID);
      foreach (var paysale in _payments)
      {
        paysale.pasa = _sale.saID;
        int nRes = await BREntities.OperationEntity(paysale, Model.Enums.EnumMode.add);
        //Disparamos el mensaje que nos arroja  la operación del guardado
        if (nRes > 0) return;
        UIHelper.ShowMessageResult("Paymentes", nRes);
      }
    }





    /// <summary>
    /// Establece los porcentajes de volumen de venta de los vendedores
    /// </summary>
    /// <history>
    /// [jorcanche] created 24062016
    /// </history>
    private void SetVolumenPorcentage()
    {
      int closers = 1;
      int vol = 0;
      //Obtenemos el numero de Closer + Exits
      if (!string.IsNullOrEmpty(txtsaCloser2.Text)) closers = closers + 1;
      if (!string.IsNullOrEmpty(txtsaCloser3.Text)) closers = closers + 1;
      if (!string.IsNullOrEmpty(txtsaExit1.Text)) closers = closers + 1;
      if (!string.IsNullOrEmpty(txtsaExit2.Text)) closers = closers + 1;

      //Obtenemos el porcentaje incial del volumen
      vol = closers == 3 ? 34 : 100 / closers;

      //Porcentaje de volumen del closer 1
      txtsaCloser1P = vol.ToString();

      //Porcentaje de volumen del closer 2
      if (vol == 34) vol = 33;
      txtsaCloser2P = !string.IsNullOrEmpty(txtsaCloser2.Text) ? vol.ToString() : "0";

      //Porcentaje de volumen del Closer 3
      txtsaCloser3P = !string.IsNullOrEmpty(txtsaCloser3.Text) ? vol.ToString() : "0";

      //Porcentaje de volumen del Exit 1
      txtsaExit1P = !string.IsNullOrEmpty(txtsaExit1.Text) ? vol.ToString() : "0";

      //Porcentaje de volumen del Exit 2
      txtsaExit2P = !string.IsNullOrEmpty(txtsaExit2.Text) ? vol.ToString() : "0";
    }

    /// <summary>
    /// Obtiene los cambios de vendedores
    /// </summary>
    /// <param name="salesmenChanges">Lista de cambios en los vendedores</param>
    /// <param name="authorizedBy">autorizado por o cambiado por</param>
    /// <history>
    /// [jorcanche] 03/06/2016 created 
    /// </history>    
    private bool GetSalesmenChanges(ref List<SaleMan> salesmenChanges, ref string authorizedBy)
    {
      //PR´s
      SalesmanChanged(txtsaPR1, cbosaPR1, 1, "PR", ref salesmenChanges);
      SalesmanChanged(txtsaPR2, cbosaPR2, 2, "PR", ref salesmenChanges);
      SalesmanChanged(txtsaPR3, cbosaPR3, 3, "PR", ref salesmenChanges);
      //Liners
      SalesmanChanged(txtsaLiner1, cbosaLiner1, 1, "LINER", ref salesmenChanges);
      SalesmanChanged(txtsaLiner2, cbosaLiner2, 2, "LINER", ref salesmenChanges);
      //Closers
      SalesmanChanged(txtsaCloser1, cbosaCloser1, 1, "CLOSER", ref salesmenChanges);
      SalesmanChanged(txtsaCloser2, cbosaCloser2, 2, "CLOSER", ref salesmenChanges);
      SalesmanChanged(txtsaCloser3, cbosaCloser3, 3, "CLOSER", ref salesmenChanges);
      //Exit Closers
      SalesmanChanged(txtsaExit1, cbosaExit1, 1, "EXIT", ref salesmenChanges);
      SalesmanChanged(txtsaExit2, cbosaExit2, 2, "EXIT", ref salesmenChanges);

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
    /// <summary>
    /// Indentifica si hubo un cambio en los SalesSalesmen de se así lo agrega en el Listado SalesmenChanges
    /// </summary>
    /// <param name="txt">TextBox que contiene el Id de la persona</param>
    /// <param name="cbo">ComboBox que contiene el Nombre de la persona</param>
    /// <param name="position">Posicion ejemplo Liner1 ó Liner 2 ó Liner 3</param>
    /// <param name="role">Role que tiene la persona</param>
    /// <param name="salesmenChanges">Listado de la persona que se modificaron agregando las persona anteriores</param>
    /// <history>
    /// [jorcanche] created 24062016
    /// </history>
    private void SalesmanChanged(TextBox txt, ComboBox cbo, int position, string role, ref List<SaleMan> salesmenChanges)
    {
      var salesSalesmenOld = _oldSale.GetType().GetProperty(txt.Name.Substring(3)).GetValue(_oldSale);
      string strSalesSalesmen = salesSalesmenOld != null ? salesSalesmenOld.ToString() : string.Empty;
      if (txt.Text != strSalesSalesmen)
      {
        salesmenChanges.Add(new SaleMan
        {
          newSalesmanDisplay = txt.Text,
          newSalesmanValue = cbo.Text,
          oldSalesmanDisplay = string.IsNullOrEmpty(strSalesSalesmen) ? string.Empty : BRPersonnel.GetPersonnelById(strSalesSalesmen).peN,
          oldSalesmanValue = strSalesSalesmen,
          position = position,
          role = role
        });
      }
    }

    private void dgpayment_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
     // var payment = (Payment)dgpayment.CurrentCell.Item;
      if (e.Column.Header.ToString() == "Payment Type")
      {
        //if (e.Editing != "CC")
        //{
        //  dgpayment
        //    }
      }
    }

    /// <summary>
    /// Muestra el Log del actual Sale
    /// </summary>
    /// <history>
    /// [jorcanche] created 24062016
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      frmSalesLog salesLog = new frmSalesLog(Convert.ToInt32(txtsaID.Text), Convert.ToInt32(txtsaMembershipNum.Text));
      salesLog.Owner = this;
      salesLog.Show();
    }

    private void dgpayment_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
     var payment = (Payment)dgpayment.SelectedItem;
      //var cell =((DataGrid)sender).CurrentCell;           
      //var propInfo = cell.Item.GetType().GetProperty("papt").GetValue(cell.Item, null);    

      if (e.Column.Header.ToString() == "Credit Card")
      {
        if ( payment.papt == "CC")
        //if (propInfo.ToString() == "CC")
        {
          e.Cancel = false;
        }
        else
        {
          e.Cancel = true;
        }
      }     
    }




    /// <summary>
    /// Validamos la fecha de venta no este en una fecha cerrada
    /// </summary>
    /// <returns></returns>
    private bool ValidateCloseDate(TextBox txt, string Description = "")
    {
      DateTime Result;
      int DiffDay = 0;
      TimeSpan ts;
      string des = string.Empty;
      string items = string.Empty;
      if (DateTime.TryParse(txt.Text, out Result))
      {
        //Dias Trasncurridos
        DiffDay = (ts = Convert.ToDateTime(txt.Text) - _CloseD).Days;
        //Si la condicion es valida y el editor esta habilitado
        if (txt.IsEnabled)
        {
          des = string.IsNullOrEmpty(Description) ? "Sales date" : Description;

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
          if (Convert.ToDateTime(txt.Text) <=_CloseD)
          {
            UIHelper.ShowMessage($"It,s not allowed to make {des} for a closed date");
            txt.Focus();
            return false;
          }
        }
      }     
      return true;
    }
  }
}

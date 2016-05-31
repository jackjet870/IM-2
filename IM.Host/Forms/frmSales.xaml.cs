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
using IM.Model.Classes;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmSales.xaml
  /// </summary>
  public partial class frmSales : Window
  {
    int _guId;
    DateTime _ServerDate, _CloseD;
    int txtsaCompany = 0;
    //Clave original de la venta anterior
    string txtsaReference;
    //Clave del huesped original
    string GuestIDOriginal;
    // Respaldamos el monto de la venta original
    string SaleAmountOriginal;
    //El Monto de la venta
    string SaleAmount;
    //Vendedores
    List<string> _saleMen;

    string txtsaCloser1P;
    string txtsaCloser2P;
    string txtsaCloser3P;
    string txtsaExit1P;
    string txtsaExit2P;
    public frmSales(EnumSale typeSale, int guId = 0)
    {
      InitializeComponent();

      gprCriteria.Visibility = typeSale == EnumSale.Sale ? Visibility.Collapsed : Visibility.Visible;
      _guId = guId;
    }

    private void btnSalesmen_Click(object sender, RoutedEventArgs e)
    {
      Panel.SetZIndex(gGeneral, 0);
    }
    private void btnGeneral_Click(object sender, RoutedEventArgs e)
    {
      Panel.SetZIndex(gGeneral, 1);
      //cbosamtGlobal.SelectedValue
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
      System.Windows.Data.CollectionViewSource paymentViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("paymentViewSource")));
      // Load data by setting the CollectionViewSource.Source property:
      // paymentViewSource.Source = [generic data source]
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

    private void LoadRecord()
    {
      //Si hay alguna venta
      if (grdSale.Items.Count > 1)
      {
        //Cargamos los controles
        LoadControls();

        // actualizamos los combos de vendedores
        UpdateCombosSalesmen();

        //Indicamos si es una venta Out of Pendsing
        SetOutOfPending();

        //Cargamos los pagos de la venta
        dgpayment.ItemsSource = BRPayments.GetPaymentsbySale(Convert.ToInt32(txtsaID.Text));

        //Nombre del huesped
        lblGuestName.Text = GetGuestName(txtsagu.Text);

        //Respaldamos el Guest ID Original
        GuestIDOriginal = txtsagu.Text;

        //Respaldamos el monto de la venta original
        SaleAmount = SaleAmountOriginal = GetSaleAmount().ToString();

        //Obtenemos los vendedores
        GetSalemen();

        //si la venta es de una fecha cerrada, no permitomos eliminar ventas
        if (IsClosed()) btnDelete.IsEnabled = false;
      }    
    }

    private string GetGuestName(string guestID)
    {     
      if (string.IsNullOrEmpty(guestID))
      {
        UIHelper.ShowMessage("Guest ID not found");
        return guestID;
      }
      Guest guest = BRGuests.GetGuestById(Convert.ToInt32(guestID));
      if (guest == null)
      {
        UIHelper.ShowMessage("Guest not found");
        return guestID;
      }
      return $"{guest.guLastName1} {guest.guFirstName1}";
    }

    /// <summary>
    /// Obtiene los vendedores
    /// </summary>
    private void GetSalemen()
    {
      _saleMen = new List<string>();

      if (!string.IsNullOrEmpty(txtsaLiner1.Text) && !_saleMen.Contains(txtsaLiner1.Text))
        _saleMen.Add(txtsaLiner1.Text);

      if (!string.IsNullOrEmpty(txtsaLiner2.Text) && !_saleMen.Contains(txtsaLiner2.Text))
        _saleMen.Add(txtsaLiner2.Text);

      if (!string.IsNullOrEmpty(txtsaCloser1.Text) && !_saleMen.Contains(txtsaCloser1.Text))
        _saleMen.Add(txtsaCloser1.Text);

      if (!string.IsNullOrEmpty(txtsaCloser2.Text) && !_saleMen.Contains(txtsaCloser2.Text))
        _saleMen.Add(txtsaCloser2.Text);

      if (!string.IsNullOrEmpty(txtsaCloser3.Text) && !_saleMen.Contains(txtsaCloser2.Text))
        _saleMen.Add(txtsaCloser3.Text);

      if (!string.IsNullOrEmpty(txtsaExit1.Text) && !_saleMen.Contains(txtsaExit1.Text))
        _saleMen.Add(txtsaExit1.Text);

      if (!string.IsNullOrEmpty(txtsaExit2.Text) && !_saleMen.Contains(txtsaExit2.Text))
        _saleMen.Add(txtsaExit2.Text);
    }

    /// <summary>
    /// devuelve el monto de la venta que se debe considerar
    /// </summary>
    /// <returns></returns>
    private decimal GetSaleAmount()
    {
      //si la venta no es un Dwngrade      
     return  GetSaleTypeCategory() == "DG"? Convert.ToDecimal(txtsaGrossAmount.Text.TrimStart(' ', '$')) : Convert.ToDecimal(txtsaNewAmount.Text.TrimStart(' ', '$'));
    }

    /// <summary>
    /// Obtiene la categoria de un tipo de venta
    /// </summary>
    /// <returns></returns>
    private string GetSaleTypeCategory()
    {
      cbosast.SelectedIndex = 0;
      return BRSaleTypes.GetSalesTypes(1, new SaleType() { stID = cbosast.SelectedValue.ToString() }).FirstOrDefault().ststc;            
    }

    /// <summary>
    /// Indica si es un venta que salio de pendiente
    /// </summary>
    private void SetOutOfPending()
    {
      //Una  venta es Out Of Pendinf si es procesable con fecha de venta distinta de la fecha de procesable
        lblOutOfPending.Visibility = (chksaProc.IsChecked == true) && (txtsaD.Text != txtsaProcD.Text) ? Visibility.Visible : Visibility.Hidden;      
    }

    /// <summary>
    /// Establece las claves de los vendedores en sus Combos
    /// </summary>
    private void UpdateCombosSalesmen()
    {
      cbosaPR1.SelectedValue = txtsaPR1.Text;
      cbosaPR2.SelectedValue = txtsaPR2.Text;
      cbosaPR3.SelectedValue = txtsaPR3.Text;
      cbosaPRCaptain1.SelectedValue = txtsaPRCaptain1.Text;
      cbosaPRCaptain2.SelectedValue = txtsaPRCaptain2.Text;
      cbosaPRCaptain3.SelectedValue = txtsaPRCaptain3.Text;
      cbosaLiner1.SelectedValue = txtsaLiner1.Text;
      cbosaLiner2.SelectedValue = txtsaLiner2.Text;
      cbosaLinerCaptain1.SelectedValue = txtsaLinerCaptain1.Text;
      cbosaCloser1.SelectedValue = txtsaCloser1.Text;
      cbosaCloser2.SelectedValue = txtsaCloser2.Text;
      cbosaCloser3.SelectedValue = txtsaCloser3.Text;
      cbosaCloserCaptain1.SelectedValue = txtsaCloserCaptain1.Text;
      cbosaExit1.SelectedValue = txtsaExit1.Text;
      cbosaExit2.SelectedValue = txtsaExit2.Text;
      cbosaPodium.SelectedValue = txtsaPodium.Text;
      cbosaVLO.SelectedValue = txtsaVLO.Text;
    }

    private void LoadControls()
    {   
      var sale = BRSales.GetSalesbyID(grdSale.SelectedItem != null ? (grdSale.SelectedItem as SaleShort).saID : (grdSale.Items[0] as SaleShort).saID);
      txtsaCloserCaptain1.Text = sale.saCloserCaptain1;
      txtsaLinerCaptain1.Text = sale.saCloserCaptain1;
      txtsaPRCaptain2.Text = sale.saPRCaptain2;
      txtsaPRCaptain1.Text = sale.saPRCaptain1;
      txtsaPRCaptain3.Text = sale.saPRCaptain3;
      txtsaCompany = sale.saCompany;
      txtsaProcRD.Text = sale.saProcRD?.ToString("dd/MM/yyyy");
      txtsals.Text = sale.sals;
      chksaUpdated.IsChecked = sale.saUpdated;
      chksaByPhone.IsChecked = sale.saByPhone;
      txtsaRefMember.Text = sale.saRefMember;
      txtsaClosingCost.Text = sale.saClosingCost.ToString("C2");
      txtsaOverPack.Text = sale.saOverPack.ToString("C2");
      txtsaOriginalAmount.Text = sale.saOriginalAmount.ToString("C2");
      txtsaNewAmount.Text = sale.saNewAmount?.ToString("C2");
      //txtsaNewAmount.Text = sale.saNewAmount.ToString();
      txtsaGrossAmount.Text = sale.saGrossAmount.ToString("C2");
      txtsasr.Text = sale.sasr;
      txtsaComments.Text = sale.saComments;
      txtsalo.Text = sale.salo;
      txtsaID.Text = sale.saID.ToString();
      txtsaFirstName2.Text = sale.saFirstName2;
      txtsaLastName2.Text = sale.saLastName2;
      txtsagu.Text = sale.sagu.ToString();
      txtsaCancelD.Text = sale.saCancelD.ToString();
      //txtsaCancel.Text = sale.saCancel.ToString();
      txtsaD.Text = sale.saD.ToString("dd/MM/yyyy");
      txtsaLastName1.Text = sale.saLastName1;
      txtsaFirstName1.Text = sale.saFirstName1;
      txtsaMembershipNum.Text = sale.saMembershipNum;
      txtsaProcD.Text = sale.saProcD?.ToString("dd/MM/yyyy");
      //txtsaProc.Text = sale.saProc;
      txtsaDownPayment.Text = sale.saDownPayment.ToString("C2");
      txtsaDownPaymentPercentage.Text = $"{sale.saDownPaymentPercentage.ToString("F")}%";
      txtsaDownPaymentPaid.Text = sale.saDownPaymentPaid.ToString("C2");
      txtsaDownPaymentPaidPercentage.Text = $"{sale.saDownPaymentPaidPercentage.ToString("F")}%";
      txtsaGrossAmountWithVAT.Text = sale.saGrossAmountWithVAT.ToString("C2");
      txtsaNewAmountWithVAT.Text = sale.saNewAmountWithVAT.ToString("C2");
      txtsaOriginalAmountWithVAT.Text = sale.saOriginalAmountWithVAT.ToString("C2");
      cbosamt.SelectedValue = sale.samt;
      cbosamtGlobal.SelectedValue = sale.samtGlobal;
      //txtsaSelfGen.Text = sale.saSelfGen;
      //txtsast.Text = sale.sast;
      txtsaExit2.Text = sale.saExit2;
      txtsaPR3.Text = sale.saPR3;
      txtsaLiner1.Text = sale.saLiner1;
      //txtsaLiner1Type.Text = sale.saLiner1Type;
      txtsaPR1.Text = sale.saPR1;
      txtsaPR2.Text = sale.saPR2;
      txtsaLiner2.Text = sale.saLiner2;
      txtsaCloser1.Text = sale.saCloser1;
      txtsaCloser2.Text = sale.saCloser2;
      txtsaExit1.Text = sale.saExit1;
      txtsaPodium.Text = sale.saPodium;
      txtsaVLO.Text = sale.saVLO;
      txtsaCloser3.Text = sale.saCloser3;
      txtsaExit2P = sale.saExit2P.ToString();
      txtsaExit1P = sale.saExit1P.ToString();
      txtsaCloser3P = sale.saCloser3P.ToString();
      txtsaCloser2P = sale.saCloser2P.ToString();
      txtsaCloser1P = sale.saCloser1P.ToString();
      txtsaReference = sale.saReference.ToString();
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
      bool blnEnable = !(mode == Enums.EnumMode.modDisplay);
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
      blnEnable = (blnEnable && !IsClosed());
      //deshabilitamos los datos generales
      gGeneral.IsEnabled = false;
      //Vendedores
      gSalesmen.IsEnabled = blnEnable;
      //boton de venta de vendedores
      btnSalesSalesmen.IsEnabled = blnEnable;
      //Capitanes
      grbCaptains.IsEnabled = blnEnable;
      //habilitamos los campos que no se obtienen de Clubes
      if (blnEnable)
      {
        //Numero de memebresia
        txtsaMembershipNum.IsEnabled = true;
        //Guest ID
        txtsagu.IsEnabled = true;
        //Venta por telefono
        chksaByPhone.IsEnabled = true;
        //Pagos
        grbPayments.IsEnabled = true;
        //Comentarios
        txtsaComments.IsEnabled = false; 
      }
      //Si la venta es de una fecha cerrada
      if (!(mode == Enums.EnumMode.modDisplay) && IsClosed())
      {
        //Permitimos modificar la venta para marcarla como procesabel o para desmarcarla de procesable 
        // si su fecha de cancelacion no esta en una fecha cerrada
        if (!chksaProc.IsChecked.Value || !(Convert.ToDateTime(txtsaProcD.Text) <= _CloseD))
        {
          fraProcessable.IsEnabled = true;
        }
        //Permitimos modificar la venta para cancelarla o para descancelarla
        //si su fecha de cancelacion no esta en una fecha cerrada
        if (!chksaCancel.IsChecked.Value || !(Convert.ToDateTime(txtsaCancelD.Text) <= _CloseD))
        {
          fraCancelled.IsEnabled = true;
        }
        //Comentarios
        txtsaComments.IsEnabled = false;
      }

    }


    /// <summary>
    /// Determina si una venta esta en una fecha cerrada
    /// </summary>
    /// <returns></returns>
    private bool IsClosed()
    {
      return (Convert.ToDateTime(txtsaD.Text) <= _CloseD || Convert.ToDateTime(txtsaProcD.Text) <= _CloseD || Convert.ToDateTime(txtsaCancelD.Text) <= _CloseD);
    }

    private async void LoadComboBox()
    {
      //Tipo de membresia y Tipo de membresia Global
       cbosamtGlobal.ItemsSource = cbosamt.ItemsSource =await BRMemberShipTypes.GetMemberShipTypes(1);
      //Tipo de venta
      cbosast.ItemsSource = BRSaleTypes.GetSalesTypes(1);
      //PR´s
      cbosaPR1.ItemsSource = cbosaPR2.ItemsSource = cbosaPR3.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "PR");
      //Capitanes de PRs
      cbosaPRCaptain1.ItemsSource = cbosaPRCaptain2.ItemsSource = cbosaPRCaptain3.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "PRCAPT");
      //Liners
      cbosaLiner1.ItemsSource = cbosaLiner2.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "LINER");
      //Capitan de Liners
      cbosaLinerCaptain1.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "LINERCAPT");
      //Closers
      cbosaCloser1.ItemsSource = cbosaCloser2.ItemsSource = cbosaCloser2.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "CLOSER");
      //Exit Closers
      cbosaExit1.ItemsSource = cbosaExit2.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "EXIT");
      //Capitan de Closers
      cbosaCloserCaptain1.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "CLOSERCAPT");
      //Podium
      cbosaPodium.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "PODIUM");
      //VLO
      cbosaVLO.ItemsSource = await  BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "VLO");

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
      if (Convert.ToDecimal(txtsaNewAmount.Text) == 0)
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
      if (string.IsNullOrEmpty(txtsaID.Text))
      {
        //si es una secretaria 
        if (App.User.HasRole(EnumRole.Secretary))
        {
          if (ValidateSalesSalesmen())
          {
            //Obtenermos los vendedores
            GetSalemen();
          }
        }
      }
    }


    private void grdSale_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
    
    }

    private void grdSale_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
      if (btnClose.IsEnabled)
      {
        LoadRecord();
      }
    }

    private void cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //var cbo = sender as ComboBox;
       //UIHelper.ShowMessage($"Valor seleccionado: {cbo.SelectedItem.GetType().GetProperty(cbo.SelectedValuePath).GetValue(cbo.SelectedItem)} \n"
        //                  + $"Nombre seleccionado: {cbo.SelectedItem.GetType().GetProperty(cbo.DisplayMemberPath).GetValue(cbo.SelectedItem)} ");
      
    }
  }
}

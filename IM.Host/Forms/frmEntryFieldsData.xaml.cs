using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Classes;
using IM.Model;
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

namespace IM.Host.Forms
{
  /// <summary>
  /// Formulario para la autorizacion de los salesman 
  /// </summary>
  /// <history>
  /// [jorcanche] created 16062016
  /// </history>
  public partial class frmEntryFieldsData : Window
  {
    public string AuthorizedBy { get; internal set; }
    public bool cancel { get; internal set; }

    List<SalesmenChanges> _saleman = null;

    /// <summary>
    /// Contructor que inicializa la variable del lisdo de los Sales man que se modificaron 
    /// </summary>
    /// <param name="saleman">Listado de los SalesMans que se modificaron</param>
    /// <history>
    /// [jorcanche] created 16062016
    /// </history>
    public frmEntryFieldsData(List<SalesmenChanges> saleman)
    {
      InitializeComponent();
      _saleman = saleman;
    }

    /// <summary>
    /// Carga el formulario
    /// </summary>
    /// <history>
    /// [jorcanche] created 16062016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      cmbAuthorizedBy.IsEnabled = true;
      LoadCloserCapt();
      var sales = new List<string>();
      foreach (var item in _saleman)
      {        
        sales.Add($" {item.roN} {item.schPosition} from '{item.schOldSalesman} - {item.OldSalesmanN}' to '{item.schNewSalesman} - {item.NewSalesmanN}' ");
      }
      lviSaleMan.ItemsSource = sales;    
    }


    /// <summary>
    /// Carga el combobox de los Closer Captain 
    /// </summary>
    /// <history>
    /// [jorcanche] created 16062016
    /// </history>
    private async void LoadCloserCapt()
    {
      //Capitan de Closers
      cmbAuthorizedBy.ItemsSource = await BRPersonnel.GetPersonnel(salesRooms: App.User.SalesRoom.srID, roles: "CLOSERCAPT");
    }


    /// <summary>
    /// Cierra el formulario
    /// </summary>
    /// <history>
    /// [jorcanche] created 16062016
    /// </history>
    private void Window_Closed(object sender, EventArgs e)
    {
      cancel = true;
    }

    /// <summary>
    /// Afirma quien lo autorizo 
    /// </summary>
    /// <history>
    /// [jorcanche] created 16062016
    /// </history>
    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      if (!cmbAuthorizedBy.SelectedIndex.Equals(-1))
      {
        AuthorizedBy = cmbAuthorizedBy.SelectedValue.ToString();
        cancel = false;
        Close();
      }
      else
      {
        UIHelper.ShowMessage("You must choose the a captain of sales");
      }      
    }


    /// <summary>
    /// Cancela la operación y el save del sale 
    /// </summary>
    /// <history>
    /// [jorcanche] created 16/06/2016 created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      cancel = true;
      Close();
    }
  }
}

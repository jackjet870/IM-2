using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.BusinessRules.BR;
using IM.Model;
using System.Linq;
using IM.Model.Helpers;
using IM.Base.Helpers;
using System;
using System.Windows.Data;
using IM.Host.Classes;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmSalesSalesmen.xaml
  /// </summary>
  public partial class frmSalesSalesmen : Window
  {
    List<SalesSalesman> _salesSalesmans;
    List<SalesSalesman> _oldList = new List<SalesSalesman>();
    decimal _amount,_amountOriginal, cantidadWithOwn;
    int _sale;
    CollectionViewSource _saleLogDataViewSource;

    /// <summary>
    /// Contructor del formulario 
    /// </summary>
    /// <history>
    /// [jorcanche] 14/06/2016 created
    /// </history>
    public frmSalesSalesmen(List<SalesSalesman> salesSalesmans, int sale, decimal amount, decimal amountOriginal)
    {
      InitializeComponent();
      _salesSalesmans = salesSalesmans;
      _amount = amount;
      _amountOriginal = amountOriginal;
      _sale = sale;
    }

    //"smsa, smpe, peN, smSale, smSaleAmountOwn, smSaleAmountWith"
    /// <summary>
    /// Carga e inicializa las variables y listados 
    /// </summary>
    /// <history>
    /// [jorcanche] 14/06/2016 created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _saleLogDataViewSource = ((CollectionViewSource)(FindResource("salesSalesmanViewSource")));
      _saleLogDataViewSource.Source = _salesSalesmans;
      btnCancel.IsEnabled = btnSave.IsEnabled = false; smSaleColumn.IsReadOnly = smSaleAmountOwnColumn.IsReadOnly = smSaleAmountWithColumn.IsReadOnly = true;

    }  

    /// <summary>
    /// Se produce el evento cuando se selecciona la celda y agrega el focus
    /// </summary>
    /// <history>
    /// [jorcanche] 14/06/2016 created
    /// </history>
    private void TextBox_Loaded(object sender, RoutedEventArgs e)
    {
      var txt = (TextBox)sender;
      txt.Focus();
      cantidadWithOwn = Convert.ToDecimal(txt.Text);
    }

    /// <summary>
    ///Modo edicion  
    /// </summary>
    /// <history>
    /// [jorcanche] 14/06/2016 created
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.IsEnabled = true; btnEdit.IsEnabled = false; btnSave.IsEnabled = true;
      smSaleColumn.IsReadOnly = smSaleAmountOwnColumn.IsReadOnly = smSaleAmountWithColumn.IsReadOnly = false;
      _oldList = new List<SalesSalesman>();
      //Se hace un Foreach a SalesSalesmen  y llenamos a _oldList, Se hace de esta forma para que no tengan la misma referencia
      // y asi se puede modificar uno sin q el otro cambie igual
      _salesSalesmans.ForEach(ag =>
      {
        var ss = new SalesSalesman();
        ObjectHelper.CopyProperties(ss, ag, true);
        _oldList.Add(ss);
      });
    }

    /// <summary>
    /// Modo para guardar
    /// </summary>
    /// <history>
    /// [jorcanche] 14/06/2016 created
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.IsEnabled = btnSave.IsEnabled = false; btnEdit.IsEnabled = true;
      smSaleColumn.IsReadOnly = smSaleAmountOwnColumn.IsReadOnly = smSaleAmountWithColumn.IsReadOnly = true;
      //Guardamos los SalesSalesmen
      SalesSalesmen.SaveSalesSalesmen(_salesSalesmans,_sale,_amount,_amountOriginal);
    }

    /// <summary>
    /// Modo cancel
    /// </summary>
    /// <history>
    /// [jorcanche] 14/06/2016 created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.IsEnabled = btnSave.IsEnabled = false; btnEdit.IsEnabled = true;
      smSaleColumn.IsReadOnly = smSaleAmountOwnColumn.IsReadOnly = smSaleAmountWithColumn.IsReadOnly = true;
      _salesSalesmans = new List<SalesSalesman>();     
      _salesSalesmans = _oldList;
      _saleLogDataViewSource.Source = _salesSalesmans;

    }

    /// <summary>
    /// Valida la tecla presionada antes de que se visualice en el textbox
    /// </summary>
    /// <history>
    /// [jorcanche] creted 22/06/2016
    /// </history>    
    private void smSaleAmountOwnWith_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox txt = sender as TextBox;
      if (!(e.Text == "." && !txt.Text.Trim().Contains(".")))
      {
        e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
      }
    }

    /// <summary>
    /// Cuando se pierde el focus del  smSaleAmountWith o del smSaleAmountOwn
    /// </summary>
    /// <history>
    /// [jorcanche] 14/06/2016 created
    /// </history>
    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      var txt = (TextBox)sender;
      if (txt.Text.Equals("")) txt.Text = "0";
      if (Convert.ToDecimal(txt.Text) > _amount)
      {
        var ownwith = txt.Name.Equals("smSaleAmountWith") ? "With" : "Own";
        UIHelper.ShowMessage($"Vol. {ownwith} can not be greater than {_amount}");
        e.Handled = true;
        txt.Text = cantidadWithOwn.ToString();
      }
    }
  }
}

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
    int _sale = 0;
    CollectionViewSource saleLogDataViewSource;

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
      saleLogDataViewSource = ((CollectionViewSource)(FindResource("salesSalesmanViewSource")));
      saleLogDataViewSource.Source = _salesSalesmans;
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
      // y asi se puede moficar uno sin q el otro cambie igual
      _salesSalesmans.ForEach(ag =>
      {
        SalesSalesman ss = new SalesSalesman();
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
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.IsEnabled = btnSave.IsEnabled = false; btnEdit.IsEnabled = true;
      smSaleColumn.IsReadOnly = smSaleAmountOwnColumn.IsReadOnly = smSaleAmountWithColumn.IsReadOnly = true;

      //1.-Extraemos el Listado de los SaleMan que se modificaron 
      //2.-Se elimina la propiedad virtual Persaonel para que no marque error de repeticion de llaves,
      // ya que personel tiene la llave de peID y la llave de la tabla SalesSaleman tiene igual la llave smpe y marcan conflicto
      // var lstModSalesSaleman = lstSaleman.Where(sa => !_oldList.Any(agg => agg.smpe == sa.smpe && ObjectHelper.IsEquals(sa, agg))).ToList();
      var lstModSalesSaleman = new List<SalesSalesman>();
      _salesSalesmans.Where(ss => !ss.smSale || ss.smSaleAmountOwn != _amount || ss.smSaleAmountWith != _amount).
      ToList().ForEach(x =>
       {
         SalesSalesman ss = new SalesSalesman();
         ObjectHelper.CopyProperties(ss, x);
         lstModSalesSaleman.Add(ss);
       }
      );
      //Eliminamos todos los registros de la tabla SalesSalesmen que sean de este sale
      await BRSalesSalesmen.DeleteSalesSalesmenbySaleId(_sale);
      //Se guardan los SalesSaleman que se modificaron       
      foreach (var salessalesmen in lstModSalesSaleman)
      {
        await BREntities.OperationEntity(salessalesmen, Model.Enums.EnumMode.add);
      }
    
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
      saleLogDataViewSource.Source = _salesSalesmans;

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

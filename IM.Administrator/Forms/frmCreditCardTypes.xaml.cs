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
using IM.Administrator.Enums;
using IM.Administrator.Helpers;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmCreditCardTypes.xaml
  /// </summary>
  public partial class frmCreditCardTypes : Window
  {
    private CreditCardType _creditCardTypeFilter=new CreditCardType();//Objeto a filtrar en la lista
    private int _nStatus = -1;//Status del grid a filtrar
    private bool _blnEdit = false;//para saber sis se cuenta con permiso paraa editar y agregar
    public frmCreditCardTypes()
    {
      InitializeComponent();
    }


    #region Métodos
    /// <summary>
    /// llena el datagrid de credit card types
    /// </summary>
    /// <history>
    /// [Emoguel] created 07/003/2016
    /// </history>
    protected void LoadCreditCardTypes()
    {
      List<CreditCardType> lstCreditCardTypes = BRCreditCardTypes.GetCreditCardTypes(_creditCardTypeFilter,_nStatus);
      if(lstCreditCardTypes.Count>0)
      {
        btnEdit.IsEnabled = _blnEdit;
        dgrCreditCard.SelectedIndex = 0;
      }
      else
      {
        btnEdit.IsEnabled = false;
      }
      dgrCreditCard.ItemsSource = lstCreditCardTypes;
      StatusBarReg.Content = lstCreditCardTypes.Count() + "  Credit Card Types.";
    }
    #endregion

    #region eventos del formlario
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 07/03/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.nStatus = _nStatus;
      frmSearch.sID = _creditCardTypeFilter.ccID;
      frmSearch.sDesc = _creditCardTypeFilter.ccN;
      frmSearch.Owner = this;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _creditCardTypeFilter.ccID = frmSearch.sID;
        _creditCardTypeFilter.ccN = frmSearch.sDesc;
        LoadCreditCardTypes();
      }
    }

    /// <summary>
    /// Abre la ventana de detalle en modo edición
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 07/03/2016
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      CreditCardType creditCardType = (CreditCardType)dgrCreditCard.SelectedItem;
      frmCreditCardTypesDetail frmCreditCard = new frmCreditCardTypesDetail();
      frmCreditCard.Owner = this;
      frmCreditCard.creditCardType = creditCardType;
      frmCreditCard.mode = ModeOpen.edit;
      if(frmCreditCard.ShowDialog()==true)
      {
        LoadCreditCardTypes();
      }
    }

    /// <summary>
    /// Abre la ventana de detalle en modo agregar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 07/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmCreditCardTypesDetail frmCreditCard = new frmCreditCardTypesDetail();
      frmCreditCard.Owner = this;
      frmCreditCard.mode = ModeOpen.add;
      if(frmCreditCard.ShowDialog()==true)
      {
        LoadCreditCardTypes();
      }
    }

    /// <summary>
    /// Evento del boton refresh---recarga los datos de la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LoadCreditCardTypes();
    }

    /// <summary>
    /// llena los datos del formulario 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = PermisionHelper.EditPermision("SALES");
      btnAdd.IsEnabled = _blnEdit;
      LoadCreditCardTypes();
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

    /// <summary>
    /// Valida las teclas presionadas para cambiar la barra de estado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 07/03/2016
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

    /// <summary>
    /// Muestra la ventada detalle en modo preview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 03/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      DataGridRow row = sender as DataGridRow;
      CreditCardType creditCardType = (CreditCardType)row.DataContext;
      frmCreditCardTypesDetail frmCrediCard = new frmCreditCardTypesDetail();
      frmCrediCard.Owner = this;
      frmCrediCard.mode = ModeOpen.preview;
      frmCrediCard.creditCardType = creditCardType;
      frmCrediCard.ShowDialog();
    }
    #endregion
  }
}

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
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;

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
    #region Load CreditCardType
    /// <summary>
    /// llena el datagrid de credit card types
    /// </summary>
    /// <history>
    /// [Emoguel] created 07/003/2016
    /// </history>
    protected void LoadCreditCardTypes()
    {
      List<CreditCardType> lstCreditCardTypes = BRCreditCardTypes.GetCreditCardTypes(_creditCardTypeFilter, _nStatus);
      dgrCreditCard.ItemsSource = lstCreditCardTypes;
      if (lstCreditCardTypes.Count > 0)
      {
        dgrCreditCard.Focus();
        GridHelper.SelectRow(dgrCreditCard, 0);
      }      
      StatusBarReg.Content = lstCreditCardTypes.Count() + "  Credit Card Types.";
    }
    #endregion
    #endregion

    #region eventos del formlario
    #region Search
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
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _creditCardTypeFilter.ccID = frmSearch.sID;
        _creditCardTypeFilter.ccN = frmSearch.sDesc;
        LoadCreditCardTypes();
      }
    }

    #endregion

    #region Add
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
      if (frmCreditCard.ShowDialog() == true)
      {
        List<CreditCardType> lstCreditCradTypes = (List<CreditCardType>)dgrCreditCard.ItemsSource;
        lstCreditCradTypes.Add(frmCreditCard.creditCardType);//Agregamos el registro nuevo
        lstCreditCradTypes.Sort((x, y) => string.Compare(x.ccN, y.ccN));//Ordenamos la lista
        int nIndex = lstCreditCradTypes.IndexOf(frmCreditCard.creditCardType);//Obtenemos el index del registro nuevo
        dgrCreditCard.Items.Refresh();//refrescamos la lista
        GridHelper.SelectRow(dgrCreditCard, nIndex);
        StatusBarReg.Content = lstCreditCradTypes.Count + " Credit Card Types.";
      }
    } 
    #endregion

    #region refresh
    /// <summary>
    /// Evento del boton refresh---recarga los datos de la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LoadCreditCardTypes();
    }
    #endregion

    #region Loaded Form
    /// <summary>
    /// llena los datos del formulario 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadCreditCardTypes();
    }
    #endregion
    #region KeyboardFocusChange
    /// <summary>
    /// verfica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {

      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion
    #region KeyDownForm
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

    #endregion
    #region DoubleClick
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
      CreditCardType creditCardType = (CreditCardType)dgrCreditCard.SelectedItem;
      frmCreditCardTypesDetail frmCrediCard = new frmCreditCardTypesDetail();
      frmCrediCard.Owner = this;
      frmCrediCard.mode = ((_blnEdit == true) ? ModeOpen.edit : ModeOpen.preview);
      ObjectHelper.CopyProperties(frmCrediCard.creditCardType,creditCardType);
      if(frmCrediCard.ShowDialog()==true)
      {
        ObjectHelper.CopyProperties(creditCardType, frmCrediCard.creditCardType);
      }
    }
    #endregion
    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            Cell_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }

      e.Handled = blnHandled;
    }

    #endregion
    #endregion


  }
}

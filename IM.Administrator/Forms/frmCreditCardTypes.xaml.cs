using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Model.Extensions;

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
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    protected async void LoadCreditCardTypes(CreditCardType creditCardType=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<CreditCardType> lstCreditCardTypes = await BRCreditCardTypes.GetCreditCardTypes(_creditCardTypeFilter, _nStatus);
        dgrCreditCard.ItemsSource = lstCreditCardTypes;
        if (creditCardType != null && lstCreditCardTypes.Count > 0)
        {
          creditCardType = lstCreditCardTypes.Where(cc => cc.ccID == creditCardType.ccID).FirstOrDefault();
          nIndex = lstCreditCardTypes.IndexOf(creditCardType);
        }
        GridHelper.SelectRow(dgrCreditCard, nIndex);
        StatusBarReg.Content = lstCreditCardTypes.Count() + "  Credit Card Types.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Credit Card Types");
      }
    }
    #endregion

    #region Validate Filters
    /// <summary>
    /// Valida si una entidad de tipo CreditCardType coincide con los filtros
    /// </summary>
    /// <param name="newCreditCard">Objeto a validar</param>
    /// <returns>true. Si se muestra | false. Nose muestra</returns>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private bool ValidateFilters(CreditCardType newCreditCard)
    {
      if (_nStatus != -1)
      {
        if (newCreditCard.ccA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_creditCardTypeFilter.ccID))
      {
        if (_creditCardTypeFilter.ccID != newCreditCard.ccID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_creditCardTypeFilter.ccN))
      {
        if (!newCreditCard.ccN.Contains(_creditCardTypeFilter.ccN,StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }

      return true;
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
      frmSearch.strID = _creditCardTypeFilter.ccID;
      frmSearch.strDesc = _creditCardTypeFilter.ccN;
      frmSearch.Owner = this;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _creditCardTypeFilter.ccID = frmSearch.strID;
        _creditCardTypeFilter.ccN = frmSearch.strDesc;
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
      frmCreditCard.mode = EnumMode.add;
      if (frmCreditCard.ShowDialog() == true)
      {
        if (ValidateFilters(frmCreditCard.creditCardType))//Validamos que cumpla con los filtros
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
      CreditCardType creditCardType = (CreditCardType)dgrCreditCard.SelectedItem;
      LoadCreditCardTypes(creditCardType);
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
      frmCrediCard.mode = ((_blnEdit == true) ? EnumMode.edit : EnumMode.preview);
      frmCrediCard.oldCreditCard = creditCardType;
      if(frmCrediCard.ShowDialog()==true)
      {
        int nIndex = 0;
        List<CreditCardType> lstCreditCradTypes = (List<CreditCardType>)dgrCreditCard.ItemsSource;
        if(!ValidateFilters(frmCrediCard.creditCardType))//Validamos si cumple con los registros
        {
          lstCreditCradTypes.Remove(creditCardType);//Quitamos el registro de la lista
        }        
        else
        {
          ObjectHelper.CopyProperties(creditCardType, frmCrediCard.creditCardType);
          lstCreditCradTypes.Sort((x, y) => string.Compare(x.ccN, y.ccN));//Ordenamos la lista   
          nIndex = lstCreditCradTypes.IndexOf(creditCardType);
        }             
        dgrCreditCard.Items.Refresh();//refrescamos la lista
        GridHelper.SelectRow(dgrCreditCard, nIndex);
        StatusBarReg.Content = lstCreditCradTypes.Count + " Credit Card Types.";
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

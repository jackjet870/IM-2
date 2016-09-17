using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Extensions;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmContracts.xaml
  /// </summary>
  public partial class frmContracts : Window
  {
    private Contract _contractFilter = new Contract();//entidad con los datos a filtrar
    private int _nStatus = -1;//Status del grid a filtrar
    private bool _blnEdit = false;//para saber sis se cuenta con permiso paraa editar y agregar
    public frmContracts()
    {
      InitializeComponent();
    }

    #region event controls
    #region KeyDown Form
    /// <summary>
    /// Valida las teclas presionadas para cambiar la barra de estado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 03/03/2016
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
    #region Loade Form
    /// <summary>
    /// Se llenan los datos de la ventana al abrirla
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 03/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(EnumPermission.Contracts, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadContracts();
    }
    #endregion
    #region KeyBoardFocusChange
    /// <summary>
    /// Verifica que teclas están presionadas
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
    #region Refresh
    /// <summary>
    /// Recarga el grid de contratos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Contract contract = (Contract)dgrContracts.SelectedItem;
      LoadContracts(contract);
    }
    #endregion

    #region Cell Double Click
    /// <summary>
    /// Muestra la ventana Detalle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 03/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Contract contract = (Contract)dgrContracts.SelectedItem;
      frmContractsDetail frmContractDetail = new frmContractsDetail();
      frmContractDetail.mode = ((_blnEdit == true) ? EnumMode.Edit : EnumMode.ReadOnly);
      frmContractDetail.oldContract = contract;
      frmContractDetail.Owner = this;
      if (frmContractDetail.ShowDialog() == true)
      {
        List<Contract> lstContracts = (List<Contract>)dgrContracts.ItemsSource;
        int nIndex = 0;
        if (!ValidateFilters(frmContractDetail.contract))//Validamos si cumple con los filtros
        {
          lstContracts.Remove(frmContractDetail.contract);//lo quitamos de la lista
        }
        else
        {
          ObjectHelper.CopyProperties(contract, frmContractDetail.contract);
          lstContracts.Sort((x, y) => string.Compare(x.cnID, y.cnID));//ordenamos la lista        
          nIndex = lstContracts.IndexOf(contract);
        }
        dgrContracts.Items.Refresh();
        GridHelper.SelectRow(dgrContracts, nIndex);
        StatusBarReg.Content = lstContracts.Count + " Contracts.";
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana de detalle para agregar un nuevo registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmContractsDetail frmContractDetail = new frmContractsDetail();
      frmContractDetail.mode = EnumMode.Add;
      frmContractDetail.Owner = this;
      if (frmContractDetail.ShowDialog() == true)
      {
        if (ValidateFilters(frmContractDetail.contract))//Validamos que cumpla con los filtros actuales
        {
          List<Contract> lstContracts = (List<Contract>)dgrContracts.ItemsSource;
          lstContracts.Add(frmContractDetail.contract);//Agregamos el registro nuevo
          lstContracts.Sort((x, y) => string.Compare(x.cnID, y.cnID));//ordenamos la lista        
          int nIndex = lstContracts.IndexOf(frmContractDetail.contract);//obtenemos el index del registro nuevo
          dgrContracts.Items.Refresh();//refrescamos la lista
          GridHelper.SelectRow(dgrContracts, nIndex);
          StatusBarReg.Content = lstContracts.Count + " Contracts.";
        }
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.nStatus = _nStatus;
      frmSearch.strDesc = _contractFilter.cnN;
      frmSearch.strID = _contractFilter.cnID;
      frmSearch.Owner = this;
      if (frmSearch.ShowDialog() == true)
      {
        _contractFilter.cnID = frmSearch.strID;
        _contractFilter.cnN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadContracts();
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

    #region Metodos
    #region LoadContracts
    /// <summary>
    /// Llena el grid de contratos
    /// </summary>
    /// <history>
    /// [Emoguel] created 03/03/2016
    /// [emoguel] modified 30/05/2016 Se volvió async
    /// </history>
    protected async void LoadContracts( Contract contract = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Contract> lstContracts =await BRContracts.getContracts(_contractFilter, _nStatus);
        dgrContracts.ItemsSource = lstContracts;
        if (contract != null && lstContracts.Count > 0)
        {
          contract = lstContracts.Where(co => co.cnID == contract.cnID).FirstOrDefault();
          nIndex = lstContracts.IndexOf(contract);
        }
        GridHelper.SelectRow(dgrContracts, nIndex);
        StatusBarReg.Content = lstContracts.Count + " Contracts.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Contracts");
      }
    }
    #endregion

    #region Validate Filters
    /// <summary>
    /// Valida si una entidad de tipo Contract coincide con los filtros
    /// </summary>
    /// <param name="newContract">Objeto a validar</param>
    /// <returns>true. Si se muestra | false. Nose muestra</returns>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private bool ValidateFilters(Contract newContract)
    {
      if (_nStatus != -1)
      {
        if (newContract.cnA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_contractFilter.cnID))
      {
        if (_contractFilter.cnID != newContract.cnID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_contractFilter.cnN))
      {
        if (!newContract.cnN.Contains(_contractFilter.cnN,StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }

      return true;
    }
    #endregion
    #endregion


  }
}

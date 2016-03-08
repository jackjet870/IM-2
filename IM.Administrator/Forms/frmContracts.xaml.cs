using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Administrator.Enums;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Base.Helpers;

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
      _blnEdit = Helpers.PermisionHelper.EditPermision("CONTRACTS");
      btnAdd.IsEnabled = _blnEdit;
      LoadContracts();
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

    /// <summary>
    /// Recarga el grid de contratos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LoadContracts();
    }

    /// <summary>
    /// Muestra la ventada Charge To preview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 03/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      DataGridRow row = sender as DataGridRow;
      Contract contract = (Contract)row.DataContext;
      frmContractsDetail frmContractDetail = new frmContractsDetail();
      frmContractDetail.mode = ModeOpen.preview;
      frmContractDetail.contract = contract;
      frmContractDetail.Owner = this;
      frmContractDetail.ShowDialog();     
    }
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmContractsDetail frmContractDetail = new frmContractsDetail();
      frmContractDetail.mode = ModeOpen.add;
      frmContractDetail.Owner = this;
      if(frmContractDetail.ShowDialog()==true)
      {
        LoadContracts();
      }
    }

    /// <summary>
    /// Abre la ventana de detalles en modo edicion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 03/03/2016
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      Contract contract = (Contract)dgrContracts.SelectedItem;
      frmContractsDetail frmContractDetail = new frmContractsDetail();
      frmContractDetail.contract = contract;
      frmContractDetail.mode = ModeOpen.edit;
      frmContractDetail.Owner = this;
      if(frmContractDetail.ShowDialog()==true)
      {
        LoadContracts();
      }
    }

    /// <summary>
    /// Abre la ventana de 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.nStatus = _nStatus;
      frmSearch.sDesc = _contractFilter.cnN;
      frmSearch.sID = _contractFilter.cnID;
      frmSearch.Owner = this;
      if(frmSearch.ShowDialog()==true)
      {
        _contractFilter.cnID = frmSearch.sID;
        _contractFilter.cnN = frmSearch.sDesc;
        _nStatus = frmSearch.nStatus;
        LoadContracts();
      }
    }
    #endregion

    #region Metodos
    /// <summary>
    /// Llena el grid de contratos
    /// </summary>
    /// <history>
    /// [Emoguel] created 03/03/2016
    /// </history>
    protected void LoadContracts()
    {
      
      List<Contract> lstContracts = BRContracts.getContracts(_contractFilter,_nStatus);
      dgrContracts.ItemsSource = lstContracts;
      if(lstContracts.Count>0)
      {
        btnEdit.IsEnabled = _blnEdit;
        dgrContracts.SelectedIndex = 0;        
      }
      else
      {
        btnEdit.IsEnabled = false;
      }
      StatusBarReg.Content = lstContracts.Count + " Contracts.";
    }
    #endregion
  }
}

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Administrator.Enums;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Base.Helpers;
using IM.Model.Enums;

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
      _blnEdit = App.User.HasPermission(EnumPermission.Contracts, EnumPermisionLevel.Standard);
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
      LoadContracts();
    }
    #endregion

    #region Cell Double Click
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
      frmContractDetail.mode = ((_blnEdit == true) ? ModeOpen.edit : ModeOpen.preview);
      ObjectHelper.CopyProperties(frmContractDetail.contract,contract);
      frmContractDetail.Owner = this;
      if(frmContractDetail.ShowDialog()==true)
      {
        LoadContracts();
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
      frmContractDetail.mode = ModeOpen.add;
      frmContractDetail.Owner = this;
      if (frmContractDetail.ShowDialog() == true)
      {
        LoadContracts();
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
      frmSearch.sDesc = _contractFilter.cnN;
      frmSearch.sID = _contractFilter.cnID;
      frmSearch.Owner = this;
      if (frmSearch.ShowDialog() == true)
      {
        _contractFilter.cnID = frmSearch.sID;
        _contractFilter.cnN = frmSearch.sDesc;
        _nStatus = frmSearch.nStatus;
        LoadContracts();
      }
    } 
    #endregion
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
        dgrContracts.SelectedIndex = 0;        
      }
      StatusBarReg.Content = lstContracts.Count + " Contracts.";
    }
    #endregion

    
  }
}

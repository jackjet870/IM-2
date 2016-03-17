using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;
using IM.Base.Helpers;
using IM.Model.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAssistanceStatus.xaml
  /// </summary>
  public partial class frmAssistancesStatus : Window
  {
    private AssistanceStatus _assistanceFilter = new AssistanceStatus();//Objeto a filtrar en el grid
    private int _nStatus = -1;//Estatus de los registros que se muestran en el grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permisos para editar|agregar
    public frmAssistancesStatus()
    {
      InitializeComponent();
    }

    #region eventos de los controles
    #region Loaded form
    /// <summary>
    /// carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadAssitance();      
    }

    #endregion

    #region KeyBoardChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 09/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region KeyDown Form
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

    #region DoubleClick cell
    /// <summary>
    /// Muestra la ventana de AssistanceStatusDetail en modo preview
    /// </summary>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      AssistanceStatus Assistance = (AssistanceStatus)dgrAssitances.SelectedItem;
      frmAssistanceStatusDetail frmAssistanceDetail = new frmAssistanceStatusDetail();
      ObjectHelper.CopyProperties(frmAssistanceDetail.assistance,Assistance);
      frmAssistanceDetail.Owner = this;
      frmAssistanceDetail.mode = ((_blnEdit == true) ? ModeOpen.edit : ModeOpen.preview);
      if(frmAssistanceDetail.ShowDialog()==true)
      {
        ObjectHelper.CopyProperties(Assistance, frmAssistanceDetail.assistance);
      }

    }

    #endregion

    #region Refresh grid
    /// <summary>
    /// Actualiza la lista de Status
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created.
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LoadAssitance();
    }
    #endregion

    #region Add
    /// <summary>
    /// Muestra la ventana de AssistanceStatusDetail para agregar un registro nuevo
    /// </summary>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {

      frmAssistanceStatusDetail frmAssistanceDetail = new frmAssistanceStatusDetail();
      frmAssistanceDetail.assistance = new AssistanceStatus();
      frmAssistanceDetail.Owner = this;
      frmAssistanceDetail.mode = ModeOpen.add;//insertar
      if (frmAssistanceDetail.ShowDialog() == true)
      {
        List<AssistanceStatus> lstAssistancesStatus = (List<AssistanceStatus>)dgrAssitances.ItemsSource;
        lstAssistancesStatus.Add(frmAssistanceDetail.assistance);//Agregamos el registro nuevo
        lstAssistancesStatus.Sort((x, y) => string.Compare(x.atN, y.atN));//ordenamos la lista
        int nIndex = lstAssistancesStatus.IndexOf(frmAssistanceDetail.assistance);//Obtenemos el index del registro nuevo
        dgrAssitances.Items.Refresh();//regrescamos el grid
        GridHelper.SelectRow(dgrAssitances, nIndex);
        StatusBarReg.Content = lstAssistancesStatus.Count+ " Assistances Status.";//Actualizamos el contador
      }

    }

    #endregion
    
    #region Search
    /// <summary>
    /// Abre la ventana de busqueda cargando 
    /// los datos de busqueda que ya se hayan realizado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 3/Mar/2016 Created
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.nStatus = _nStatus;
      frmSearch.sID = _assistanceFilter.atID;
      frmSearch.sDesc = _assistanceFilter.atN;
      frmSearch.Owner = this;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _assistanceFilter.atID = frmSearch.sID;
        _assistanceFilter.atN = frmSearch.sDesc;

        LoadAssitance();
      }
    }
    #endregion
    #endregion

    #region metodos
    #region Load Assistance
    /// <summary>
    /// carga la lista de Assistance Status
    /// </summary>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>    
    protected void LoadAssitance()
    {
      List<AssistanceStatus> lstAssistance = BRAssistancesStatus.GetAssitanceStatus(_assistanceFilter, _nStatus);
      dgrAssitances.ItemsSource = lstAssistance;
      if (lstAssistance.Count > 0)
      {
        dgrAssitances.Focus();
        GridHelper.SelectRow(dgrAssitances, 0);        
      }
      StatusBarReg.Content = lstAssistance.Count + " Assistance Status.";
    }
    #endregion

    #endregion
    
  }
}

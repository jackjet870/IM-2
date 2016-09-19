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
  /// Interaction logic for frmEfficiencyTypes.xaml
  /// </summary>
  public partial class frmEfficiencyTypes : Window
  {
    private EfficiencyType _efficiencyTypeFilter = new EfficiencyType();//Objeto con los filtros del grid
    private int _nStatus = -1;//estatus de los registros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar|Agregar

    public frmEfficiencyTypes()
    {
      InitializeComponent();
    }

    #region Eventos del formulario
    #region Window Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadEfficiencyTypes();
      _blnEdit = Context.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
    }
    #endregion

    #region Window KeyDown
    /// <summary>
    /// Verifica las teclas que se presionem
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
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

    #region KeyBoardFocusChanged
    /// <summary>
    /// Verifica las teclas INS/Capital/NumLock
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _efficiencyTypeFilter.etID;
      frmSearch.strDesc = _efficiencyTypeFilter.etN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _efficiencyTypeFilter.etID = frmSearch.strID;
        _efficiencyTypeFilter.etN = frmSearch.strDesc;
        LoadEfficiencyTypes();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    { 
      frmEfficiencyTypeDetail frmEfyTypeDetail = new frmEfficiencyTypeDetail();
      frmEfyTypeDetail.Owner = this;
      frmEfyTypeDetail.enumMode = EnumMode.Add;     
      if(frmEfyTypeDetail.ShowDialog()==true)
      {
        EfficiencyType efficiencyType = frmEfyTypeDetail.efficiencyType;
        if(ValidateFilter(efficiencyType))
        {
          List<EfficiencyType> lstEfyType = (List<EfficiencyType>)dgrEfcyTypes.ItemsSource;
          lstEfyType.Add(efficiencyType);
          lstEfyType.Sort((x, y) => string.Compare(x.etN, y.etN));
          int nIndex = lstEfyType.IndexOf(efficiencyType);
          dgrEfcyTypes.Items.Refresh();
          GridHelper.SelectRow(dgrEfcyTypes, nIndex);
        }
      }
          
    }
    #endregion

    #region Refresh
    /// <summary>
    /// recarga la lista de efficiencyTypes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      EfficiencyType effType = (EfficiencyType)dgrEfcyTypes.SelectedItem;
      LoadEfficiencyTypes(effType);
    }
    #endregion

    #region Cell Double Click
    /// <summary>
    /// Muestra la ventada efficiency detail dependiendo de los permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 18/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      EfficiencyType efficiencyType = (EfficiencyType)dgrEfcyTypes.SelectedItem;
      frmEfficiencyTypeDetail frmEfyTypeDetail = new frmEfficiencyTypeDetail();
      frmEfyTypeDetail.Owner = this;
      frmEfyTypeDetail.enumMode = ((_blnEdit == true) ? EnumMode.Edit : EnumMode.ReadOnly);
      frmEfyTypeDetail.oldEffType = efficiencyType;

      if (frmEfyTypeDetail.ShowDialog() == true)
      {
        int nIndex = 0;
        List<EfficiencyType> lstEfcyType = (List<EfficiencyType>)dgrEfcyTypes.ItemsSource;
        if(!ValidateFilter(frmEfyTypeDetail.efficiencyType))//Si cumple con los filtros
        {
          lstEfcyType.Remove(efficiencyType);
        }
        else
        {
          ObjectHelper.CopyProperties(efficiencyType, frmEfyTypeDetail.efficiencyType);
          lstEfcyType.Sort((x, y) => string.Compare(x.etN, y.etN));
          nIndex = lstEfcyType.IndexOf(efficiencyType);
        }
        dgrEfcyTypes.Items.Refresh();
        GridHelper.SelectRow(dgrEfcyTypes,nIndex);
        StatusBarReg.Content = lstEfcyType.Count + " Efficiency Types.";
        
      }
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
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

    #region Métodos
    #region Load EfficiencyTypes
    /// <summary>
    /// Llena el grid de Efficiency Types
    /// </summary>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private async void LoadEfficiencyTypes(EfficiencyType effType=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<EfficiencyType> lstEfcyTypes = await BREfficiencyTypes.GetEfficiencyTypes(_efficiencyTypeFilter, _nStatus);
        dgrEfcyTypes.ItemsSource = lstEfcyTypes;
        if (effType != null & lstEfcyTypes.Count > 0)
        {
          effType = lstEfcyTypes.Where(ef => ef.etID == effType.etID).FirstOrDefault();
          nIndex = lstEfcyTypes.IndexOf(effType);
        }
        GridHelper.SelectRow(dgrEfcyTypes, nIndex);
        StatusBarReg.Content = lstEfcyTypes.Count + " Efficiency Types.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Verificia si el objeto cumple con los filtros establecidos
    /// </summary>
    /// <param name="efficiencyType">Objeto a validar</param>
    /// <returns>true. cumple con los filtros | false. No cumple con los filtros</returns>
    /// <history>
    /// [emoguel] created 19/03/2016
    /// </history>
    private bool ValidateFilter(EfficiencyType efficiencyType)
    {
      if(_nStatus!=-1)//Validamos el estatus
      {
        if(efficiencyType.etA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_efficiencyTypeFilter.etID))//Validamos el ID
      {
        if(efficiencyType.etID!=_efficiencyTypeFilter.etID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_efficiencyTypeFilter.etN))//Validamos la descripcion
      {
        if(!efficiencyType.etN.Contains(_efficiencyTypeFilter.etN,StringComparison.OrdinalIgnoreCase))
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

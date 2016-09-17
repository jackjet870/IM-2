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
  /// Interaction logic for frmLeadSources.xaml
  /// </summary>
  public partial class frmLeadSources : Window
  {
    #region Variables
    private LeadSource _leadSourceFilter = new LeadSource {lspg="",lssr="",lsar="",lsrg="",lsso="" };//Objeto con los filtros de la ventana
    private int _nStatus = -1;//Estatus de los registros del grid
    private int _nRegen = -1;//Si es regen
    private int _nAnimation = -1;//Si es Animation
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para Editar|Agregar
    #endregion
    public frmLeadSources()
    {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(EnumPermission.Locations, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      loadLeadSources();
    }

    #region Methods Form
    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 13/05/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region window keyDown
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 13/05/2016
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

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// cambia de fila con el boton tab
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

    #region DobleClic Grid
    /// <summary>
    /// Abre la ventana detalle en modo "detalle" o "edición" dependiendo de sus permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 13/05/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      LeadSource leadSource = (LeadSource)dgrLeadSources.SelectedItem;
      frmLeadSourceDetail frmLeadSourceDetail = new frmLeadSourceDetail();
      frmLeadSourceDetail.Owner = this;
      frmLeadSourceDetail.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly;
      frmLeadSourceDetail.oldLeadSource = leadSource;
      if(frmLeadSourceDetail.ShowDialog()==true)
      {
        List<LeadSource> lstLeadSources = (List<LeadSource>)dgrLeadSources.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmLeadSourceDetail.leadSource))
        {
          ObjectHelper.CopyProperties(leadSource, frmLeadSourceDetail.leadSource,true);//Actualizar los datos
          lstLeadSources.Sort((x, y) => string.Compare(x.lsN, y.lsN));//Ordenar la lista
          nIndex = lstLeadSources.IndexOf(leadSource);//Obtener el indice del registro
        }
        else
        {
          lstLeadSources.Remove(leadSource);
        }
        dgrLeadSources.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrLeadSources, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstLeadSources.Count + " Lead Sources";//Actualizmos el contador
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 13/05/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmLeadSourceDetail frmLeadSourceDetail = new frmLeadSourceDetail();
      frmLeadSourceDetail.Owner = this;
      frmLeadSourceDetail.enumMode = EnumMode.Search;
      frmLeadSourceDetail.nStatus = _nStatus;
      frmLeadSourceDetail.nRegen = _nRegen;
      frmLeadSourceDetail.nAnimation = _nAnimation;
      frmLeadSourceDetail.oldLeadSource = _leadSourceFilter;
      if(frmLeadSourceDetail.ShowDialog()==true)
      {
        _leadSourceFilter = frmLeadSourceDetail.leadSource;
        _nStatus = frmLeadSourceDetail.nStatus;
        _nRegen = frmLeadSourceDetail.nRegen;
        _nAnimation = frmLeadSourceDetail.nAnimation;
        loadLeadSources();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en Modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 13/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmLeadSourceDetail frmLeadSourceDetail = new frmLeadSourceDetail();
      frmLeadSourceDetail.Owner = this;
      frmLeadSourceDetail.enumMode = EnumMode.Add;
      if(frmLeadSourceDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmLeadSourceDetail.leadSource))
        {
          List<LeadSource> lstLeadSource = (List<LeadSource>)dgrLeadSources.ItemsSource;
          lstLeadSource.Add(frmLeadSourceDetail.leadSource);//Agregamos el registro
          lstLeadSource.Sort((x, y) => string.Compare(x.lsN, y.lsN));//Ordenamos la lista
          int nIndex = lstLeadSource.IndexOf(frmLeadSourceDetail.leadSource);//BUscamos la posición del registro
          dgrLeadSources.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrLeadSources, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstLeadSource.Count + " Lead Sources.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los registros de la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 13/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LeadSource leadSources = (LeadSource)dgrLeadSources.SelectedItem;
      loadLeadSources(leadSources);
    }
    #endregion
    #endregion

    #region Methods
    #region loadLeadSources
    /// <summary>
    /// Llena el grid de LeadSources
    /// </summary>
    /// <param name="leadSources">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 13/05/2016
    /// [emoguel] modified 25/05/2016 se volvio async el método
    /// </history>
    private async void loadLeadSources(LeadSource leadSources = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<LeadSource> lstLeadSource = await BRLeadSources.GetLeadSources(_nStatus, _nRegen, _nAnimation, _leadSourceFilter, true);
        if (lstLeadSource.Count > 0 && leadSources != null)
        {
          leadSources = lstLeadSource.Where(ls => ls.lsID == leadSources.lsID).FirstOrDefault();
          nIndex = lstLeadSource.IndexOf(leadSources);
        }
        dgrLeadSources.ItemsSource = lstLeadSource;
        GridHelper.SelectRow(dgrLeadSources, nIndex);
        StatusBarReg.Content = lstLeadSource.Count + " Lead Sources.";
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
    /// Valida que un leadsource cumpla con los filtros actuales
    /// </summary>
    /// <param name="leadSource">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No ciumple</returns>
    /// <history>
    /// [emoguel] created 17/05/2016
    /// </history>
    private bool ValidateFilter(LeadSource leadSource)
    {
      if (_nStatus != -1)//Filtro por Estatus
      {
        if(leadSource.lsA!=(Convert.ToBoolean(_nStatus)))
        {
          return false;
        }
      }

      if (_nRegen != -1)
      {
        if (leadSource.lsRegen != (Convert.ToBoolean(_nRegen)))
        {
          return false;
        }
      }

      if (_nAnimation != -1)//Filtro por Animation
      {
        if (leadSource.lsAnimation != (Convert.ToBoolean(_nAnimation)))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_leadSourceFilter.lsID))//Filtro por ID
      {        
        if (_leadSourceFilter.lsID != leadSource.lsID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_leadSourceFilter.lsN))//Filtro por descripción
      {
        if(!leadSource.lsN.Contains(_leadSourceFilter.lsN,StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_leadSourceFilter.lspg))//Filtro por Program
      {
        if(_leadSourceFilter.lspg!=leadSource.lspg)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_leadSourceFilter.lssr))//Filtro por Sales Room
      {
        if(leadSource.lssr!=_leadSourceFilter.lssr)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_leadSourceFilter.lsar))//Filtro por Area
      {
        if(_leadSourceFilter.lsar!=leadSource.lsar)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_leadSourceFilter.lsrg))//Filtro por region
      {
        if(_leadSourceFilter.lsrg!=leadSource.lsrg)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_leadSourceFilter.lsso))//Filtro por Segment
      {
        if(_leadSourceFilter.lsso!=leadSource.lsso)
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

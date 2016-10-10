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
  /// Interaction logic for frmAgencies.xaml
  /// </summary>
  public partial class frmAgencies : Window
  {
    private Agency _agencyFilter=new Agency {agse="" };//Objeto para filtrar en la BD
    private int _nStatus = -1;//Status a filtrar en la lista
    private bool _blnEdit = false;//boleano para saber si se tiene minimo permiso para editar|agregar 

    public frmAgencies()
    {
      InitializeComponent();
    }

    #region event controls
    #region Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(EnumPermission.Agencies, EnumPermisionLevel.Standard);
      LoadAgencies();
      btnAdd.IsEnabled = _blnEdit;
      dtgAgencies.CurrentCellChanged += GridHelper.dtg_CurrentCellChanged;
    }
    #endregion

    #region keyboardFocusChage
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

    #region window keyDown
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/03/2016
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

    #region refresh
    /// <summary>
    /// Recarga los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Agency agency = (Agency)dtgAgencies.SelectedItem;
      LoadAgencies(agency);
    }
    #endregion

    #region Boton agregar
    /// <summary>
    /// Abre la ventana detalle en modo agregar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmAgencyDetail frmAgencyDetail = new frmAgencyDetail();
      frmAgencyDetail.enumMode = EnumMode.Add;//Insertar
      frmAgencyDetail.Owner = this;
      frmAgencyDetail.oldAgency = new Agency();
      if(frmAgencyDetail.ShowDialog()==true)
      {
        if (ValidateFilters(frmAgencyDetail.agency))//Validamos si el nuevo registro cumple con los requisitos
        {
          List<Agency> lstAgencies = (List<Agency>)dtgAgencies.ItemsSource;//cateamos el itemsource
          lstAgencies.Add(frmAgencyDetail.agency);//Agregamos el registro nuevo
          lstAgencies.Sort((x, y) => string.Compare(x.agN, y.agN));//ordenamos la lista
          int nIndex = lstAgencies.IndexOf(frmAgencyDetail.agency);//obtenemos el index del registro nuevo        
          dtgAgencies.Items.Refresh();//Refrescamos la lista
          GridHelper.SelectRow(dtgAgencies, nIndex);
          StatusBarReg.Content = lstAgencies.Count + " Agencies.";//Actualizamos el contador
        }
      }     
    }
    #endregion
    
    #region Boton Buscar

    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.strID = _agencyFilter.agID;
      frmSearch.strDesc = _agencyFilter.agN;
      frmSearch.nStatus = _nStatus;
      frmSearch.sSegment = _agencyFilter.agse;
      frmSearch.enumWindow = EnumWindow.Agencies;
      frmSearch.Owner = this;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _agencyFilter.agID = frmSearch.strID;
        _agencyFilter.agN = frmSearch.strDesc;
        _agencyFilter.agse = frmSearch.sSegment;
        LoadAgencies();
      }
    }
    #endregion

    #region DobleClic Grid
    /// <summary>
    /// Abre la ventana detalle en modo "detalle" o "edición" dependiendo de sus permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Agency agency = (Agency)dtgAgencies.SelectedItem;      
      frmAgencyDetail frmAgencyDetail = new frmAgencyDetail();      
      frmAgencyDetail.oldAgency = agency;
      frmAgencyDetail.Owner = this;
      frmAgencyDetail.enumMode = ((_blnEdit == true) ? EnumMode.Edit : EnumMode.ReadOnly);
      if(frmAgencyDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<Agency> lstAgencies = (List<Agency>)dtgAgencies.ItemsSource;//cateamos el itemsource
        if (!ValidateFilters(frmAgencyDetail.agency))
        {
          lstAgencies.Remove(agency);//quitamos el registro de la lista          
        }
        else
        {
          ObjectHelper.CopyProperties(agency, frmAgencyDetail.agency);
          lstAgencies.Sort((x, y) => string.Compare(x.agN, y.agN));//ordenamos la lista    
          nIndex = lstAgencies.IndexOf(agency);
        }
        
        dtgAgencies.Items.Refresh();
        StatusBarReg.Content = lstAgencies.Count + " Agencies.";//Actualizamos el contador
        GridHelper.SelectRow(dtgAgencies, nIndex);
      }
    }
    #endregion
    #endregion
    #region métodos
    #region Load Agencies
    /// <summary>
    /// Llena el grid de Agencies dependiendo de los filtros seleccionados
    /// </summary>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    protected async void LoadAgencies(Agency agency = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Agency> lstAgencies = await BRAgencies.GetAgencies(_agencyFilter, _nStatus);
        dtgAgencies.ItemsSource = lstAgencies;
        if (agency != null && lstAgencies.Count > 0)
        {
          agency = lstAgencies.FirstOrDefault(ag => ag.agID == agency.agID);
          nIndex = lstAgencies.IndexOf(agency);
        }
        GridHelper.SelectRow(dtgAgencies, nIndex);
        StatusBarReg.Content = lstAgencies.Count + " Agencies.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region Validate Filters
    /// <summary>
    /// Valida si una entidad de tipo Agency coincide con los filtros
    /// </summary>
    /// <param name="newAgency">Objeto a validar</param>
    /// <returns>true. Si se muestra | false. Nose muestra</returns>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private bool ValidateFilters(Agency newAgency)
    {

      if (_nStatus != -1)
      {
        if (newAgency.agA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_agencyFilter.agID))
      {
        if (_agencyFilter.agID != newAgency.agID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_agencyFilter.agN))
      {
        if (!newAgency.agN.Contains(_agencyFilter.agN,StringComparison.OrdinalIgnoreCase))
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

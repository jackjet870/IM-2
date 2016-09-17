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
  /// Interaction logic for frmSegmentsByLeadSource.xaml
  /// </summary>
  public partial class frmSegmentsByLeadSource : Window
  {
    #region Variables
    private SegmentByLeadSource _segmentByLSFilter = new SegmentByLeadSource();//Objeto con los filtros adicionales
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Para saber si tiene permiso para editar
    #endregion
    public frmSegmentsByLeadSource()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(EnumPermission.Agencies, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadSegmentsByLeadSource();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
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
    /// [emoguel] created 02/06/2016
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
    /// <history>
    /// [emoguel] created 02/06/2016
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

    #region DobleClic Grid
    /// <summary>
    /// Abre la ventana detalle en modo "detalle" o "edición" dependiendo de sus permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      SegmentByLeadSource segmentByLS = (SegmentByLeadSource)dgrSegments.SelectedItem;
      frmSegmentByLeadSourceDetail frmSegmentByLS = new frmSegmentByLeadSourceDetail();
      frmSegmentByLS.Owner = this;
      frmSegmentByLS.oldSegmentByLeadSource = segmentByLS;
      frmSegmentByLS.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly;
      if(frmSegmentByLS.ShowDialog()==true)
      {
        int nIndex = 0;
        List<SegmentByLeadSource> lstSegmentByLS = (List<SegmentByLeadSource>)dgrSegments.ItemsSource;
        if(ValidateFilter(frmSegmentByLS.segmentByLeadSource))//Validar que cumpla con los filtros
        {
          ObjectHelper.CopyProperties(segmentByLS, frmSegmentByLS.segmentByLeadSource);//Actualizamos los datos
          lstSegmentByLS.Sort((x, y) => string.Compare(x.soN, y.soN));//Ordenamos la lista
          nIndex = lstSegmentByLS.IndexOf(segmentByLS);//Obtenemos la posición del registro
        }
        else
        {
          lstSegmentByLS.Remove(segmentByLS);//Quitamos el registro
        }
        dgrSegments.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrSegments, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstSegmentByLS.Count + " Segments.";//Actualizamos el contador
      }
    }
    #endregion

    #region refresh
    /// <summary>
    /// Recarga los datos de la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      SegmentByLeadSource segmentByLS = (SegmentByLeadSource)dgrSegments.SelectedItem;
      LoadSegmentsByLeadSource(segmentByLS);
    }

    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmSegmentByLeadSourceDetail frmSegmentsByLS = new frmSegmentByLeadSourceDetail();
      frmSegmentsByLS.Owner = this;
      frmSegmentsByLS.enumMode = EnumMode.Add;
      if(frmSegmentsByLS.ShowDialog()==true)
      {
        if(ValidateFilter(frmSegmentsByLS.segmentByLeadSource))//Verificar si cumple con los filtros
        {
          List<SegmentByLeadSource> lstSegments = new List<SegmentByLeadSource>();
          lstSegments.Add(frmSegmentsByLS.segmentByLeadSource);//Agregamos el objeto
          lstSegments.Sort((x, y) => string.Compare(x.soN, y.soN));//Ordenamos la lista
          int nIndex = lstSegments.IndexOf(frmSegmentsByLS.segmentByLeadSource);//Buscamos la posición del registro
          dgrSegments.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrSegments, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstSegments.Count + " Segments.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _segmentByLSFilter.soID;
      frmSearch.strDesc = _segmentByLSFilter.soN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _segmentByLSFilter.soID = frmSearch.strID;
        _segmentByLSFilter.soN = frmSearch.strDesc;
        LoadSegmentsByLeadSource();
      }
    }
    #endregion

    #region Sort Segment
    /// <summary>
    /// Abre la ventana de ordenar segments
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void btnSort_Click(object sender, RoutedEventArgs e)
    {
      frmSegmentsOrder frmSegmentsOrder = new frmSegmentsOrder();
      frmSegmentsOrder.Owner = this;
      frmSegmentsOrder.ShowDialog();
    }
    #endregion
    #endregion

    #region Methods
    #region LoadSegmentsByLeadSource
    /// <summary>
    /// Llena el grid de segments By Lead Source
    /// </summary>
    /// <param name="leadSource">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private async void LoadSegmentsByLeadSource(SegmentByLeadSource segmentByLS = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<SegmentByLeadSource> lstSegmentsByLS = await BRSegmentsByLeadSource.GetSegmentsByLeadSource(_nStatus,_segmentByLSFilter);
        dgrSegments.ItemsSource = lstSegmentsByLS;
        if(lstSegmentsByLS.Count>0 && segmentByLS!=null)
        {
          segmentByLS = lstSegmentsByLS.Where(so => so.soID == segmentByLS.soID).FirstOrDefault();
          nIndex = lstSegmentsByLS.IndexOf(segmentByLS);
        }
        GridHelper.SelectRow(dgrSegments, nIndex);
        StatusBarReg.Content = lstSegmentsByLS.Count + " Segments.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Segments By Lead Source");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un Segement cumpla con los filtros actuales
    /// </summary>
    /// <param name="segmentByLS"></param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private bool ValidateFilter(SegmentByLeadSource segmentByLS)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(segmentByLS.soA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_segmentByLSFilter.soID))//Filtro por ID
      {
        if(segmentByLS.soID!=_segmentByLSFilter.soID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_segmentByLSFilter.soN))//Filtro por descripcion
      {
        if(!segmentByLS.soN.Contains(_segmentByLSFilter.soN,StringComparison.OrdinalIgnoreCase))
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

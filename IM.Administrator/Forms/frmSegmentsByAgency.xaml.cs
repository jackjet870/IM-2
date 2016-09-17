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
  /// Interaction logic for frmSegmentsByAgency.xaml
  /// </summary>
  public partial class frmSegmentsByAgency : Window
  {
    #region Variables
    private SegmentByAgency _segmentByAgencyFilter = new SegmentByAgency();//Objeto con los filtros adicionales
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Para saber si tiene permiso para editar
    #endregion
    public frmSegmentsByAgency()
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
    /// [emoguel] created 31/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(EnumPermission.Agencies, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadSegmentsByAgency();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/05/2016
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
    /// [emoguel] created 31/05/2016
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
    /// [emoguel] created 31/05/2016
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
    /// [emoguel] created 31/05/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {      
      SegmentByAgency segmentByAgency = (SegmentByAgency)dgrSegments.SelectedItem;
      frmSegmentByAgencyDetail frmSegmentByAgency = new frmSegmentByAgencyDetail();
      frmSegmentByAgency.Owner = this;
      frmSegmentByAgency.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.Add;
      frmSegmentByAgency.oldSegmentByAgency = segmentByAgency;
      if(frmSegmentByAgency.ShowDialog()==true)
      {
        int nIndex = 0;
        List<SegmentByAgency> lstSegmentsByAgency = (List<SegmentByAgency>)dgrSegments.ItemsSource;
        if(ValidateFilter(frmSegmentByAgency.segmentByAgency))//Verificamos que cumpla con los filtros
        {
          ObjectHelper.CopyProperties(segmentByAgency, frmSegmentByAgency.segmentByAgency);//Actualizamos los datos
          lstSegmentsByAgency.Sort((x, y) => string.Compare(x.seN, y.seN));//Ordenamos la lista
          nIndex = lstSegmentsByAgency.IndexOf(segmentByAgency);//Obtenemos la posición del registro
        }
        else
        {
          lstSegmentsByAgency.Remove(segmentByAgency);//Quitamos el registro
        }
        dgrSegments.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrSegments, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstSegmentsByAgency.Count + " Segments";//Actualizamos el contador
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
    /// [emoguel] created 31/05/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.nStatus = _nStatus;
      frmSearch.strID = _segmentByAgencyFilter.seID;
      frmSearch.strDesc = _segmentByAgencyFilter.seN;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _segmentByAgencyFilter.seID = frmSearch.strID;
        _segmentByAgencyFilter.seN = frmSearch.strDesc;
        LoadSegmentsByAgency();
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Recarga los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      SegmentByAgency segmentbyAgency = (SegmentByAgency)dgrSegments.SelectedItem;
      LoadSegmentsByAgency(segmentbyAgency);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmSegmentByAgencyDetail frmSegmentByAgency = new frmSegmentByAgencyDetail();
      frmSegmentByAgency.Owner = this;
      frmSegmentByAgency.enumMode = EnumMode.Add;
      if (frmSegmentByAgency.ShowDialog() == true)
      {
        if (ValidateFilter(frmSegmentByAgency.segmentByAgency))//Verificamos que cumpla con los filtros
        {
          List<SegmentByAgency> lstSegmentByAgency = (List<SegmentByAgency>)dgrSegments.ItemsSource;
          lstSegmentByAgency.Add(frmSegmentByAgency.segmentByAgency);//Agregamos el registro
          lstSegmentByAgency.Sort((x, y) => string.Compare(x.seN, y.seN));//Ordenamos la lista
          int nIndex = lstSegmentByAgency.IndexOf(frmSegmentByAgency.segmentByAgency);//Buscamos la posición del registro
          dgrSegments.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrSegments, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstSegmentByAgency.Count + " Segments";
        }
      }
    }
    #endregion

    #region btnSort_Click
    /// <summary>
    /// Abre la ventana de Segments sort
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/05/2016
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
    #region LoadSegmentsByAgency
    /// <summary>
    /// Carga el grid de segmentByAgency
    /// </summary>
    /// <param name="segmentByAgency">objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private async void LoadSegmentsByAgency(SegmentByAgency segmentByAgency = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<SegmentByAgency> lstSegmentsByAgency = await BRSegmentsByAgency.GetSegMentsByAgency(_segmentByAgencyFilter, _nStatus);
        dgrSegments.ItemsSource = lstSegmentsByAgency;
        if (lstSegmentsByAgency.Count > 0 && segmentByAgency != null)
        {
          segmentByAgency = lstSegmentsByAgency.Where(sg => sg.seID == segmentByAgency.seID).FirstOrDefault();
          nIndex = lstSegmentsByAgency.IndexOf(segmentByAgency);
        }
        GridHelper.SelectRow(dgrSegments, nIndex);
        StatusBarReg.Content = lstSegmentsByAgency.Count + " Segments.";
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Segments By Agency");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un segment by Agency cumpla con los filtros actuales
    /// </summary>
    /// <param name="segmentByAgecy">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private bool ValidateFilter(SegmentByAgency segmentByAgecy)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(segmentByAgecy.seA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_segmentByAgencyFilter.seID))//Filtro por ID
      {
        if(segmentByAgecy.seID!=_segmentByAgencyFilter.seID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_segmentByAgencyFilter.seN))//Filtro por descripcion
      {
        if(!segmentByAgecy.seN.Contains(_segmentByAgencyFilter.seN,StringComparison.OrdinalIgnoreCase))
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

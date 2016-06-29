using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSegmentByLeadSourceDetail.xaml
  /// </summary>
  public partial class frmSegmentByLeadSourceDetail : Window
  {
    #region Variables
    public SegmentByLeadSource segmentByLeadSource= new SegmentByLeadSource();//Objeto a guardar
    public SegmentByLeadSource oldSegmentByLeadSource = new SegmentByLeadSource();//Objeto con los datos iniciales
    private List<LeadSource> _lstOldLeadSources = new List<LeadSource>();//Lista inicial de LeadSources
    public EnumMode enumMode;
    private bool _isCellCancel = false;
    private bool _isClosing = false;
    #endregion
    public frmSegmentByLeadSourceDetail()
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
      ObjectHelper.CopyProperties(segmentByLeadSource, oldSegmentByLeadSource);
      UIHelper.SetUpControls(segmentByLeadSource, this);
      LoadLeadSources();
      if (enumMode != EnumMode.preview)
      {
        chksoA.IsEnabled = true;
        txtsoN.IsEnabled = true;
        txtsoID.IsEnabled = (enumMode == EnumMode.add);
        dgrLeadSources.IsReadOnly = false;
      }
      DataContext = segmentByLeadSource;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        _isClosing = true;
        btnCancel_Click(null, null);
        if (!_isClosing)
        {
          e.Cancel = true;
        }
        else
        {
          _isClosing = false;
        }
      }
    }
    #endregion

    #region RowEndEdit
    /// <summary>
    /// Verifica que no se agreguen registros vacios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void dgrLeadSources_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (_isCellCancel)
      {
        dgrLeadSources.RowEditEnding -= dgrLeadSources_RowEditEnding;
        dgrLeadSources.CancelEdit();
        dgrLeadSources.RowEditEnding += dgrLeadSources_RowEditEnding;
      }
      else
      {
        cmbLeadSources.Header = "Lead Source (" + (dgrLeadSources.Items.Count - 1) + ")";
      }
    }
    #endregion

    #region CellEndEdit
    /// <summary>
    /// Verifica que no se repita un leadSource
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void dgrLeadSources_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))
      {
        _isCellCancel = false;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrLeadSources, true);
        e.Cancel = isRepeat;
      }
      else
      {
        _isCellCancel = true;
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda un segment by Lead Source
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        List<LeadSource> lstLeadSources = (List<LeadSource>)dgrLeadSources.ItemsSource;
        if (enumMode != EnumMode.add && ObjectHelper.IsEquals(segmentByLeadSource, oldSegmentByLeadSource) && ObjectHelper.IsListEquals(_lstOldLeadSources, lstLeadSources))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          string strMsj = ValidateHelper.ValidateForm(this, "Segment By Lead Source");
          if (strMsj == "")
          {
            List<LeadSource> lstAdd = lstLeadSources.Where(ls => !_lstOldLeadSources.Any(lss => lss.lsID == ls.lsID)).ToList();
            List<LeadSource> lstDel = _lstOldLeadSources.Where(ls => !lstLeadSources.Any(lss => lss.lsID == ls.lsID)).ToList();
            int nRes = await BRSegmentsByLeadSource.SaveSegmentByLeadSource(segmentByLeadSource, lstAdd, lstDel, (enumMode == EnumMode.edit));
            UIHelper.ShowMessageResult("Segment By Lead Source", nRes);
            if (nRes > 0)
            {
              _isClosing = true;
              DialogResult = true;
              Close();
            }
          }
          else
          {
            UIHelper.ShowMessage(strMsj);
          }
          skpStatus.Visibility = Visibility.Collapsed;
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Clubs");
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      if (enumMode != EnumMode.preview)
      {
        List<LeadSource> lstLeadSources = (List<LeadSource>)dgrLeadSources.ItemsSource;
        if (!ObjectHelper.IsEquals(segmentByLeadSource, oldSegmentByLeadSource) || !ObjectHelper.IsListEquals(lstLeadSources, _lstOldLeadSources))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            if (!_isClosing) { _isClosing = true; Close(); }
          }
          else
          {
            _isClosing = false;
          }
        }
        else
        {
          if (!_isClosing) { _isClosing = true; Close(); }
        }
      }
      else
      {
        if (!_isClosing) { _isClosing = true; Close(); }
      }
    }
    #endregion

    #region RowKeyDown
    /// <summary>
    /// Cambia el valor del contador
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        var item = dgrLeadSources.SelectedItem;
        if (item.GetType().Name == "LeadSource")
        {
          cmbLeadSources.Header = "Lead Source (" + (dgrLeadSources.Items.Count - 2) + ")";
        }
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadLeadSources
    /// <summary>
    /// Carga el grid y el combobox de LeadSource
    /// </summary>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private async void LoadLeadSources()
    {
      try
      {
        List<LeadSource> lstAllLeadSources = await BRLeadSources.GetLeadSources(-1, -1, -1);        
        cmbLeadSources.ItemsSource = lstAllLeadSources;
        List<LeadSource> lstLeadSources = (!string.IsNullOrWhiteSpace(segmentByLeadSource.soID)) ? lstAllLeadSources.Where(ls => ls.lsso == segmentByLeadSource.soID).ToList() : new List<LeadSource>();
        dgrLeadSources.ItemsSource = lstLeadSources;
        _lstOldLeadSources = lstLeadSources.ToList();
        if (enumMode != EnumMode.preview)
        {
          btnAccept.Visibility = Visibility.Visible;
        }
        cmbLeadSources.Header = "Lead Source (" + lstLeadSources.Count + ")";
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Segments by Lead Source");
      }
    } 
    #endregion

    #endregion
  }
}

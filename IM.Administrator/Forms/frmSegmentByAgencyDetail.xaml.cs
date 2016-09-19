using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSegemenByAgencyDetail.xaml
  /// </summary>
  public partial class frmSegmentByAgencyDetail : Window
  {
    #region Variables
    public SegmentByAgency segmentByAgency = new SegmentByAgency();//Objeto a guardar
    public SegmentByAgency oldSegmentByAgency = new SegmentByAgency();//Objeto con los datos iniciales
    private List<Agency> _lstOldAgencies = new List<Agency>();//Lista inicial de Agencias
    public EnumMode enumMode;
    private bool _isCellCancel=false;
    private bool _isClosing = false;
    #endregion
    public frmSegmentByAgencyDetail()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(segmentByAgency, oldSegmentByAgency);
      UIHelper.SetUpControls(segmentByAgency,this);
      LoadAgencies();
      if(enumMode!=EnumMode.ReadOnly)
      {        
        chkseA.IsEnabled = true;
        txtseN.IsEnabled = true;
        txtseID.IsEnabled = (enumMode == EnumMode.Add);
        dgrAgencies.IsReadOnly = false;
      }
      DataContext = segmentByAgency;
      dgrAgencies.BeginningEdit += GridHelper.dgr_BeginningEdit;
    }
    #endregion    

    #region Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing && enumMode != EnumMode.ReadOnly)
      {
        btnCancel.Focus();
        List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
        if (!ObjectHelper.IsEquals(segmentByAgency, oldSegmentByAgency) || !ObjectHelper.IsListEquals(lstAgencies, _lstOldAgencies))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            dgrAgencies.CancelEdit();
          }
        }
      }
    }
    #endregion

    #region RowEndEdit
    /// <summary>
    /// Verifica que no se creen registros vacios
    /// </summary>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private void dgrAgencies_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        dgrAgencies.RowEditEnding -= dgrAgencies_RowEditEnding;
        if (_isCellCancel)
        {
          dgrAgencies.CancelEdit();
        }
        else
        {
          dgrAgencies.CommitEdit();
          dgrAgencies.Items.Refresh();
          GridHelper.SelectRow(dgrAgencies, dgrAgencies.SelectedIndex);
          cmbAgenciesID.Header = "Agency (" + (dgrAgencies.Items.Count - 1) + ")";
        }
        dgrAgencies.RowEditEnding += dgrAgencies_RowEditEnding;
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
        var item = dgrAgencies.SelectedItem;
        if (item.GetType().Name == "Agency")
        {
          cmbAgenciesID.Header = "Agency (" + (dgrAgencies.Items.Count - 2) + ")";
        }
      }
    } 
    #endregion

    #region EndCellEdit
    /// <summary>
    /// Verifica que no se repita ni un registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private void dgrAgencies_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if(!Keyboard.IsKeyDown(Key.Escape))
      {
        _isCellCancel = false;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrAgencies,true);
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
    /// Guarda los cambios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
        if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(segmentByAgency, oldSegmentByAgency) && ObjectHelper.IsListEquals(_lstOldAgencies, lstAgencies))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          string strMsj = ValidateHelper.ValidateForm(this, "Segment By Agency");
          if (strMsj == "")
          {
            List<Agency> lstAdd = lstAgencies.Where(ag => !_lstOldAgencies.Any(agg => agg.agID == ag.agID)).ToList();
            List<Agency> lstDel = _lstOldAgencies.Where(ag => !lstAgencies.Any(agg => agg.agID == ag.agID)).ToList();
            int nRes = await BRSegmentsByAgency.SaveSegmentByAgency(segmentByAgency, lstAdd, lstDel, (enumMode == EnumMode.Edit));
            UIHelper.ShowMessageResult("Segment By Agency", nRes);
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
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadAgencies
    /// <summary>
    /// Llena el grid y el combobox de Agencies
    /// </summary>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private async void LoadAgencies()
    {
      try
      {
        List<Agency> lstAllAgencies = await BRAgencies.GetAgencies(null);
        List<Agency> lstAgencies = (!string.IsNullOrWhiteSpace(segmentByAgency.seID)) ? lstAllAgencies.Where(ag => ag.agse == segmentByAgency.seID).ToList() : new List<Agency>();
        dgrAgencies.ItemsSource = lstAgencies;        
        cmbAgenciesID.ItemsSource = lstAllAgencies;
        lstAgencies.ForEach((Action<Agency>)(ag => {
          Agency agency = new Agency();
          ObjectHelper.CopyProperties(agency, ag);
          _lstOldAgencies.Add(agency);
        }));        
        if(enumMode!=EnumMode.ReadOnly)
        {
          btnAccept.Visibility = Visibility.Visible;
        }
        cmbAgenciesID.Header = "Agency (" + lstAgencies.Count + ")";
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    } 
    #endregion
    #endregion
  }
}

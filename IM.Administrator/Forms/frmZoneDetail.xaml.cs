using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmZoneDetail.xaml
  /// </summary>
  public partial class frmZoneDetail : Window
  {
    public Zone zone = new Zone();//Objeto a guardar
    public Zone oldZone = new Zone();//Objeto con los datos iniciales
    private List<LeadSource> _lstOldLeadSources = new List<LeadSource>();//Lista inicial de LeadSources
    public EnumMode enumMode;//Modo de la ventana
    private bool _isClosing = false;
    private bool _isCellCancel = false;
    public frmZoneDetail()
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
    /// [emoguel] created 09/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(zone, oldZone);
      UIHelper.SetUpControls(zone, this);
      LoadLeadSources();
      if(enumMode!=EnumMode.preview)
      {
        txtznID.IsEnabled = (enumMode == EnumMode.add);
        txtznN.IsEnabled = true;
        chkznA.IsEnabled = true;
        dgrLeadSources.IsReadOnly = false;
      }
      DataContext = zone;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/06/2016
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
    /// [emoguel] created 09/06/2016
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

    #region Accept
    /// <summary>
    /// Guarda los cambios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/06/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<LeadSource> lstLeadSources = (List<LeadSource>)dgrLeadSources.ItemsSource;
      if (enumMode != EnumMode.add && ObjectHelper.IsEquals(zone, oldZone) && ObjectHelper.IsListEquals(lstLeadSources, _lstOldLeadSources))
      {
        _isClosing = true;
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Zone");
        if (strMsj == "")
        {
          txtStatus.Text = "Saving Data...";
          skpStatus.Visibility = Visibility.Visible;
          List<LeadSource> lstAdd = lstLeadSources.Where(ls => !_lstOldLeadSources.Any(lss => lss.lsID == ls.lsID)).ToList();
          List<LeadSource> lstDel = _lstOldLeadSources.Where(ls => !lstLeadSources.Any(lss => lss.lsID == ls.lsID)).ToList();
          int nRes = await BRZones.SaveZone(zone,lstAdd,lstDel,(enumMode==EnumMode.edit));
          UIHelper.ShowMessageResult("Zone", nRes);
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
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/06/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if(enumMode!=EnumMode.preview)
      {
        btnCancel.Focus();
        List<LeadSource> lstLeadSources = (List<LeadSource>)dgrLeadSources.ItemsSource;
        if (!ObjectHelper.IsEquals(zone, oldZone) || !ObjectHelper.IsListEquals(lstLeadSources, _lstOldLeadSources))
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

    #region CellEditing
    /// <summary>
    /// Verifica que no se repita un registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/06/2016
    /// </history>
    private void dgrZones_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if(!Keyboard.IsKeyDown(Key.Escape))
      {
        _isCellCancel = false;
        bool blnIsRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrLeadSources);
        e.Cancel = blnIsRepeat;
      }
      else
      {
        _isCellCancel = true;
      }
    }
    #endregion

    #region RowEditEnding
    /// <summary>
    /// Verfiica que no se guarden registros vacios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/06/2016
    /// </history>
    private void dgrZones_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (_isCellCancel)
      {
        dgrLeadSources.RowEditEnding -= dgrZones_RowEditEnding;
        dgrLeadSources.CancelEdit();
        dgrLeadSources.RowEditEnding += dgrZones_RowEditEnding;
      }
      else
      {
        cmbLeadSources.Header = "Lead Source (" + (dgrLeadSources.Items.Count - 1) + ")";
      }
    } 
    #endregion

    #region KeyDown
    /// <summary>
    /// Cambia el contador cuando se eliminan registros
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/06/2016
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
    /// Llena el grid y el comobobox de LeadSource
    /// </summary>
    /// <history>
    /// [emoguel] created 09/06/2016
    /// </history>
    private async void LoadLeadSources()
    {
      try
      {
        List<LeadSource> lstAllLeadSoources = await BRLeadSources.GetLeadSources(-1, -1, -1);
        cmbLeadSources.ItemsSource = lstAllLeadSoources;
        List<LeadSource> lstLeadSources = (!string.IsNullOrWhiteSpace(zone.znID)) ? lstAllLeadSoources.Where(ls => ls.lszn == zone.znID).ToList() : new List<LeadSource>();
        dgrLeadSources.ItemsSource = lstLeadSources;
        _lstOldLeadSources = lstLeadSources.ToList();
        cmbLeadSources.Header = "Lead Source (" + lstLeadSources.Count + ")";
        if(enumMode!=EnumMode.preview)
        {
          btnAccept.Visibility = Visibility.Visible;
        }
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Zones");
      }
    }

    #endregion
    #endregion
  }
}

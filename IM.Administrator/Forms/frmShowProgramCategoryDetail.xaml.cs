using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Helpers;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmShowProgramCategoryDetail.xaml
  /// </summary>
  public partial class frmShowProgramCategoryDetail : Window
  {
    #region Variables
    public ShowProgramCategory showProgramCategory = new ShowProgramCategory();//Objeto a guardar
    public ShowProgramCategory oldShowProgramCategory = new ShowProgramCategory();//Objeto con los datos iniciales
    private List<ShowProgram> _lstOldShowPrograms = new List<ShowProgram>();//Lista inicial de ShowPrograms
    public EnumMode enumMode;//Modo de la ventana
    private bool _isClosing = false;
    private bool _isCellCancel = false;
    #endregion
    public frmShowProgramCategoryDetail()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(showProgramCategory, oldShowProgramCategory);
      UIHelper.SetUpControls(showProgramCategory, this);
      txtsgID.IsEnabled = (enumMode == EnumMode.Add);
      DataContext = showProgramCategory;
      LoadShowPrograms();
      dgrShowPrograms.BeginningEdit += GridHelper.dgr_BeginningEdit;
    }
    #endregion

    #region Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        btnCancel.Focus();
        List<ShowProgram> lstShowPrograms = (List<ShowProgram>)dgrShowPrograms.ItemsSource;
        if (!ObjectHelper.IsEquals(showProgramCategory, oldShowProgramCategory) || !ObjectHelper.IsListEquals(lstShowPrograms, _lstOldShowPrograms))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            dgrShowPrograms.CancelEdit();
          }
        }
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda los cambios
    /// </summary>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        List<ShowProgram> lstShowPrograms = (List<ShowProgram>)dgrShowPrograms.ItemsSource;
        if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(showProgramCategory, oldShowProgramCategory) && ObjectHelper.IsListEquals(lstShowPrograms, _lstOldShowPrograms))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Show Program Category", blnDatagrids: true);
          if (strMsj == "")
          {
            txtStatus.Text = "Saving Data...";
            skpStatus.Visibility = Visibility.Visible;
            btnAccept.Visibility = Visibility.Collapsed;
            List<ShowProgram> lstAdd = lstShowPrograms.Where(sk => !_lstOldShowPrograms.Any(skk => skk.skID == sk.skID)).ToList();
            List<ShowProgram> lstDel = _lstOldShowPrograms.Where(sk => !lstShowPrograms.Any(skk => skk.skID == sk.skID)).ToList();
            int nRes = await BRShowProgramsCategories.SaveShowProgramCategory(showProgramCategory, lstAdd, lstDel, (enumMode == EnumMode.Edit));
            UIHelper.ShowMessageResult("Show Program Category", nRes);
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
          btnAccept.Visibility = Visibility.Visible;
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region CellEditEnding
    /// <summary>
    /// No permite que se repita un mismo registro
    /// </summary>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private void dgrShowPrograms_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        bool blnRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrShowPrograms);
        e.Cancel = blnRepeat;
      }
      else
      {
        _isCellCancel = true;
      }
    }
    #endregion

    #region dgrShowPrograms_RowEditEnding
    /// <summary>
    /// Verifica que no se agreguen registros vacios al grid
    /// </summary>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private void dgrShowPrograms_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCancel)
        {
          dgrShowPrograms.RowEditEnding -= dgrShowPrograms_RowEditEnding;
          dgrShowPrograms.CancelEdit();
          dgrShowPrograms.RowEditEnding += dgrShowPrograms_RowEditEnding;
        }
        else
        {
          cmbShowPrograms.Header = "Show Programs (" + (dgrShowPrograms.Items.Count - 1) + ")";
        }
        dgrShowPrograms.RowEditEnding -= dgrShowPrograms_RowEditEnding;        
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
    /// [emoguel] created 04/06/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        var item = dgrShowPrograms.SelectedItem;
        if (item.GetType().Name == "ShowProgram")
        {
          cmbShowPrograms.Header = "ShowProgram (" + (dgrShowPrograms.Items.Count - 2) + ")";
        }
      }
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadShowPrograms
    /// <summary>
    /// Llena el grid y el combobox de categories
    /// </summary>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private async void LoadShowPrograms()
    {
      try
      {
        List<ShowProgram> lstAllShowPrograms = await BRShowPrograms.GetShowPrograms(-1);
        cmbShowPrograms.ItemsSource = lstAllShowPrograms;
        List<ShowProgram> lstShowPrograms = (!string.IsNullOrWhiteSpace(showProgramCategory.sgID)) ? lstAllShowPrograms.Where(sk => sk.sksg == showProgramCategory.sgID).ToList() : new List<ShowProgram>();
        dgrShowPrograms.ItemsSource = lstShowPrograms;
        _lstOldShowPrograms = lstShowPrograms.ToList();
        cmbShowPrograms.Header = "Show Program (" + lstShowPrograms.Count + ")";
        skpStatus.Visibility = Visibility.Collapsed;
        btnAccept.Visibility = Visibility.Visible;
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

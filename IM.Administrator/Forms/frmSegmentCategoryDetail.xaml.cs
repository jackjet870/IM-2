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
using IM.Model.Classes;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSegmentCategoryDetail.xaml
  /// </summary>
  public partial class frmSegmentCategoryDetail : Window
  {
    #region Variables
    public SegmentCategory segmentCategory = new SegmentCategory();//Objeto a guardar
    public SegmentCategory oldSegmentCategory = new SegmentCategory();//Objeto con los datos iniciales
    private List<Item> _lstOldItems = new List<Item>();//Lista inicial de Agencias
    public EnumMode enumMode;
    private bool _isCellCancel = false;
    private bool _isClosing = false;
    #endregion
    public frmSegmentCategoryDetail()
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
      ObjectHelper.CopyProperties(segmentCategory, oldSegmentCategory);
      UIHelper.SetUpControls(segmentCategory, this);
      LoadSegmentesOrder();
      if (enumMode != EnumMode.preview)
      {
        chkscA.IsEnabled = true;
        txtscN.IsEnabled = true;
        txtscID.IsEnabled = (enumMode == EnumMode.add);
        dgrSegmentsCategory.IsReadOnly = false;
      }
      DataContext = segmentCategory;
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
        var item = dgrSegmentsCategory.SelectedItem;
        if (item.GetType().Name == "Item")
        {
          cmbSegmentsCat.Header = "Segment (" + (dgrSegmentsCategory.Items.Count - 2) + ")";
        }
      }
    }
    #endregion

    #region RowEditEnding
    /// <summary>
    /// Verifica que no se agregue un resgistro vacio
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void dgrSegmentsCategory_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      dgrSegmentsCategory.RowEditEnding -= dgrSegmentsCategory_RowEditEnding;
      if (_isCellCancel)
      {
        dgrSegmentsCategory.CancelEdit();
      }
      else
      {
        dgrSegmentsCategory.CommitEdit();
        dgrSegmentsCategory.Items.Refresh();
        GridHelper.SelectRow(dgrSegmentsCategory, dgrSegmentsCategory.SelectedIndex);
        cmbSegmentsCat.Header = "Segment (" + (dgrSegmentsCategory.Items.Count - 1) + ")";
      }
      dgrSegmentsCategory.RowEditEnding += dgrSegmentsCategory_RowEditEnding;
    }
    #endregion

    #region CellEditEnding
    /// <summary>
    /// Verifica que no se repita un registro ya agregado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private void dgrSegmentsCategory_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))
      {
        _isCellCancel = false;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrSegmentsCategory,true);        
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
    /// Guarda un segmentCategory
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
        List<Item> lstItems = (List<Item>)dgrSegmentsCategory.ItemsSource;
        if (enumMode != EnumMode.add && ObjectHelper.IsEquals(segmentCategory, oldSegmentCategory) && ObjectHelper.IsListEquals(_lstOldItems, lstItems,"UserId"))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          string strMsj = ValidateHelper.ValidateForm(this, "Segment Category");
          if (strMsj == "")
          {            
            List<Item> lstAdd = lstItems.Where(it => !_lstOldItems.Any(itt => itt.UserId == it.UserId)).ToList();
            List<Item> lstDel = _lstOldItems.Where(it => !lstItems.Any(itt => itt.UserId == it.UserId)).ToList();
            int nRes = await BRSegmentsCategories.SaveSegmentCategory(segmentCategory, lstAdd, lstDel,(enumMode==EnumMode.edit));
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
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Segment Category");
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
        List<Item> lstItems = (List<Item>)dgrSegmentsCategory.ItemsSource;
        if (!ObjectHelper.IsEquals(segmentCategory, oldSegmentCategory) || !ObjectHelper.IsListEquals(lstItems, _lstOldItems,"UserId"))
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
    
    #endregion

    #region Methods
    #region LoadSegmentesOrder
    /// <summary>
    /// Llena el grid y el combobox de Segmentcategory
    /// </summary>
    /// <history>
    /// [emoguel] created 02/06/2016
    /// </history>
    private async void LoadSegmentesOrder()
    {
      try
      {
        List<Item> lstAllItems = await BRSegmentsOrder.GetSegmentsOrder();
        cmbSegmentsCat.ItemsSource = lstAllItems;
        List<Item> lstItems = (!string.IsNullOrWhiteSpace(segmentCategory.scID)) ? lstAllItems.Where(it => it.Category == segmentCategory.scID).ToList() : new List<Item>();
        dgrSegmentsCategory.ItemsSource = lstItems;
        _lstOldItems = lstItems.ToList();
        cmbSegmentsCat.Header = "Segment (" + lstItems.Count + ")";
        if(enumMode != EnumMode.preview)
        {
          btnAccept.Visibility = Visibility.Visible;
        }
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Segments Categories");
      }
    } 
    #endregion
    #endregion
  }
}

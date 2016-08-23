using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using System.Windows.Input;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSaleTypeCategoryDetail.xaml
  /// </summary>
  public partial class frmSaleTypeCategoryDetail : Window
  {
    #region Variables
    public EnumMode enumMode;
    public SaleTypeCategory saleTypeCategory = new SaleTypeCategory();
    public SaleTypeCategory oldSaleTypeCategory = new SaleTypeCategory();
    private List<SaleType> _lstOldSaleTypes = new List<SaleType>();
    private bool _isClosing = false;
    private bool _isCellCancel = false;
    #endregion
    public frmSaleTypeCategoryDetail()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window_Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 30/07/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      txtstcID.IsEnabled = (enumMode == EnumMode.Add);
      ObjectHelper.CopyProperties(saleTypeCategory, oldSaleTypeCategory);
      UIHelper.SetUpControls(saleTypeCategory, this);
      DataContext = saleTypeCategory;
      LoadSaleTypes();
      dgrSaleType.BeginningEdit += GridHelper.dgr_BeginningEdit;
    }
    #endregion

    #region Closing
    /// <summary>
    /// Verifica que no haya cambios pendientes antes de cerrar la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 30/07/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if(!_isClosing)
      {
        btnCancel.Focus();
        List<SaleType> lstSaleTypeCategories = dgrSaleType.ItemsSource as List<SaleType>;
        if(!ObjectHelper.IsEquals(saleTypeCategory,oldSaleTypeCategory) || !ObjectHelper.IsListEquals(lstSaleTypeCategories,_lstOldSaleTypes))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            dgrSaleType.CancelEdit();
          }
        }
      }
    }
    #endregion

    #region dgrSaleType_RowEditEnding
    /// <summary>
    /// finaliza la edición de la fila
    /// </summary>
    /// <history>
    /// [emoguel] created 01/08/2016
    /// </history>
    private void dgrSaleType_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCancel)
        {
          dgrSaleType.RowEditEnding -= dgrSaleType_RowEditEnding;
          dgrSaleType.CancelEdit();
          dgrSaleType.RowEditEnding += dgrSaleType_RowEditEnding;
        }
        else
        {
          cmbSalesTypes.Header = "Sale Types (" + (dgrSaleType.Items.Count - 1) + ")";
        }
      }
    }
    #endregion

    #region dgrSaleType_CellEditEnding
    /// <summary>
    /// Verifica que no se repitan registros
    /// </summary>
    /// <history>
    /// [emoguel] created 01/08/2016
    /// </history>
    private void dgrSaleType_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        _isCellCancel = false;
        var combobox = e.EditingElement as ComboBox;
        bool blnIsRepeat = GridHelper.HasRepeatItem(combobox, dgrSaleType, typeName: "Sale Type");
        if(!blnIsRepeat)
        {
          ObjectHelper.CopyProperties(e.Row.Item as SaleType, combobox.SelectedItem as SaleType);
        }
        else
        {
          e.Cancel = true;
        }
      }
      else
      {
        _isCellCancel = true;
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda los cambios de un sale type o agrega uno nuevo dependiendo del modo en que se haya abierto la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 01-Ago-2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<SaleType> lstSaleTypes = dgrSaleType.ItemsSource as List<SaleType>;
      if(ObjectHelper.IsEquals(saleTypeCategory,oldSaleTypeCategory) && ObjectHelper.IsListEquals(lstSaleTypes,_lstOldSaleTypes))
      {
        _isClosing = true;
        Close();
      }
      else
      {
        btnAccept.Visibility = Visibility.Collapsed;
        txtStatus.Text = "Saving Data...";
        skpStatus.Visibility = Visibility.Visible;
        ValidateHelper.ValidateForm(this, "Sale Type category");
        string strMsj = "";
        if(strMsj=="")
        {
          var lstAddSaleType = lstSaleTypes.Where(st => !_lstOldSaleTypes.Any(stt => stt.stID == st.stID)).ToList();
          var lstDelSaleType = _lstOldSaleTypes.Where(st => !lstSaleTypes.Any(stt => stt.stID == st.stID)).ToList();

          int nRes = await BRSaleTypesCategories.SaveSaleTypeCategory(saleTypeCategory, (enumMode == EnumMode.Edit), lstAddSaleType, lstDelSaleType);
          if(nRes>0)
          {
            _isClosing = true;
            DialogResult = true;
            Close();
          }
          else
          {
            UIHelper.ShowMessageResult("Sale Type Category",nRes);
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
    #endregion

    #region Row KeyDown
    /// <summary>
    /// Cambia el contador de sale types
    /// </summary>
    /// <history>
    /// [emoguel] created 01/08/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        cmbSalesTypes.Header = "Sale Types (" + (dgrSaleType.Items.Count - 2) + ")";
      }      
    }
    #endregion
    #endregion

    #region Methods
    #region LoadSaleTypes
    /// <summary>
    /// Carga los sale Types del combobox y el Datagrid
    /// </summary>
    /// <history>
    /// [emoguel] created 01-Ago-2016
    /// </history>
    private async void LoadSaleTypes()
    {
      try
      {
        List<SaleType> lstSaleTypes = await BRSaleTypes.GetSalesTypes();
        _lstOldSaleTypes = (!string.IsNullOrWhiteSpace(saleTypeCategory.stcID)) ? lstSaleTypes.Where(st => st.ststc == saleTypeCategory.stcID).Select(st=>st).ToList() : new List<SaleType>();
        cmbSalesTypes.ItemsSource = lstSaleTypes;
        dgrSaleType.ItemsSource = _lstOldSaleTypes.ToList();
        btnAccept.Visibility = Visibility.Visible;
        skpStatus.Visibility = Visibility.Collapsed;
        cmbSalesTypes.Header = "Sale Types (" + _lstOldSaleTypes.Count + ")";
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

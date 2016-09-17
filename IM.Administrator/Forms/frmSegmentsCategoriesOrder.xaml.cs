using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSegmentsCategoriesOrder.xaml
  /// </summary>
  public partial class frmSegmentsCategoriesOrder : Window
  {
    #region Variables
    private List<SegmentCategory> _lstSegmentscategory = new List<SegmentCategory>();
    #endregion
    public frmSegmentsCategoriesOrder()
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
      LoadSegmentsCategoriesOrder();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/06/2016
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
    /// [emoguel] created 03/06/2016
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

    #region btnUp_Click
    /// <summary>
    /// Cambia de orden el registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/06/2016
    /// </history>
    private void btnUp_Click(object sender, RoutedEventArgs e)
    {
      if (dgrSegmentsCategories.SelectedIndex != 0)
      {
        List<SegmentCategory> lstItems = (List<SegmentCategory>)dgrSegmentsCategories.ItemsSource;
        SegmentCategory itemUp = lstItems[dgrSegmentsCategories.SelectedIndex];
        SegmentCategory itemDown = lstItems[dgrSegmentsCategories.SelectedIndex - 1];
        int value = Convert.ToInt32(itemDown.scO);
        itemUp.scO = value;
        itemDown.scO = value + 1;
        lstItems.Sort((x, y) => x.scO.CompareTo(y.scO));
        dgrSegmentsCategories.Items.Refresh();
        GridHelper.SelectRow(dgrSegmentsCategories, dgrSegmentsCategories.SelectedIndex);
      }
    }
    #endregion

    #region btnDown_Click
    /// <summary>
    /// Cambia de orden el registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/06/2016
    /// </history>
    private void btnDown_Click(object sender, RoutedEventArgs e)
    {
      if (dgrSegmentsCategories.SelectedIndex != dgrSegmentsCategories.Items.Count)
      {
        List<SegmentCategory> lstItems = (List<SegmentCategory>)dgrSegmentsCategories.ItemsSource;
        SegmentCategory itemUp = lstItems[dgrSegmentsCategories.SelectedIndex];
        SegmentCategory itemDown = lstItems[dgrSegmentsCategories.SelectedIndex + 1];
        int value = Convert.ToInt32(itemDown.scO);
        itemUp.scO = value;
        itemDown.scO = value - 1;
        lstItems.Sort((x, y) => x.scO.CompareTo(y.scO));
        dgrSegmentsCategories.Items.Refresh();
        GridHelper.SelectRow(dgrSegmentsCategories, dgrSegmentsCategories.SelectedIndex);
      }
    }
    #endregion

    #region Save
    /// <summary>
    /// Guarda los cambios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        txtStatus.Text = "Saving Data...";
        List<SegmentCategory> lstSegmentCategories = (List<SegmentCategory>)dgrSegmentsCategories.ItemsSource;
        if (ValidateChanges(lstSegmentCategories))
        {
          int nRes = await BRSegmentsCategories.ChangeSegmentsCategoryOrder(lstSegmentCategories);
          UIHelper.ShowMessageResult("Segments Categories Order", nRes);
          if(nRes>0)
          {
            _lstSegmentscategory = new List<SegmentCategory>();
            lstSegmentCategories.ForEach(sc => {
              SegmentCategory segmenteCateg = new SegmentCategory();
              ObjectHelper.CopyProperties(segmenteCateg, sc);
              _lstSegmentscategory.Add(segmenteCateg);
            });
          }
        }
        btnEdit.IsEnabled = true;
        btnCancel.IsEnabled = false;
        SetEditMode(false);
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region btnEdit_Click
    /// <summary>
    /// Habilita el modo edición
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      btnEdit.IsEnabled = false;
      btnCancel.IsEnabled = true;
      SetEditMode(true);
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cancela la edicion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnEdit.IsEnabled = true;
      btnCancel.IsEnabled = false;
      SetEditMode(false);
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica cambios pendientes antes de cerrar la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      List<SegmentCategory> lstItems = (List<SegmentCategory>)dgrSegmentsCategories.ItemsSource;
      if (ValidateChanges(lstItems))
      {
        MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
        e.Cancel = (result == MessageBoxResult.No);
      }
    }
    #endregion
#endregion

    #region Methods
    #region LoadSegmentsCategoriesOrder
    /// <summary>
    /// Llena el grid de Segments order
    /// </summary>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private async void LoadSegmentsCategoriesOrder()
    {
      try
      {
        status.Visibility = Visibility.Visible;
        txtStatus.Text = "Loading...";
        List<SegmentCategory> lstObjects = await BRSegmentsCategories.GetSegmentsCategories();
        lstObjects = lstObjects.OrderBy(sc => sc.scO).ToList();
        dgrSegmentsCategories.ItemsSource = lstObjects;
        lstObjects.ForEach(sc =>
        {
          SegmentCategory segmentCategory = new SegmentCategory();
          ObjectHelper.CopyProperties(segmentCategory, sc);
          _lstSegmentscategory.Add(segmentCategory);
        });
        GridHelper.SelectRow(dgrSegmentsCategories, 0);
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region SetEditMode
    /// <summary>
    /// habilita y dehabilita los botones
    /// </summary>
    /// <param name="blnMode">true. habilita | False. Deshabilita</param>
    /// <history>
    /// [emoguel] created 03/06/2016
    /// </history>
    private void SetEditMode(bool blnMode)
    {
      btnUp.IsEnabled = blnMode;
      btnDown.IsEnabled = blnMode;
      btnSave.IsEnabled = blnMode;
    }
    #endregion

    #region ValidateChanged
    /// <summary>
    /// Verifica si huno cambios en el formulario
    /// </summary>
    /// <param name="lstItems">Lista de Items</param>
    /// <returns>True. Si hubo cambios | False. No hubo cambios</returns>
    /// <history>
    /// [emoguel] created 03/06/2016
    /// </history>
    private bool ValidateChanges(List<SegmentCategory> lstItems)
    {
      var lstChanges= _lstSegmentscategory.Where(sc => lstItems.Any(scc => sc.scID == scc.scID && sc.scO == scc.scO)).ToList();

      return (lstChanges.Count() != _lstSegmentscategory.Count);
    }
    #endregion

    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Classes;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSegmentsOrder.xaml
  /// </summary>
  public partial class frmSegmentsOrder : Window
  {
    #region Variables
    List<Item> _lstOldSegments = new List<Item>();
    #endregion
    public frmSegmentsOrder()
    {
      InitializeComponent();
    }

    #region MethodsForm
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
      LoadSegmentsOrder();
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

    #region btnUp_Click
    /// <summary>
    /// Cambia de orden el registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private void btnUp_Click(object sender, RoutedEventArgs e)
    {
      if (dgrSegments.SelectedIndex != 0)
      {
        List<Item> lstItems = (List<Item>)dgrSegments.ItemsSource;
        Item itemUp = lstItems[dgrSegments.SelectedIndex];
        Item itemDown = lstItems[dgrSegments.SelectedIndex-1];
        int value = Convert.ToInt32(itemDown.Id);
        itemUp.Id = value.ToString();
        itemDown.Id = (value+1).ToString();
        lstItems.Sort((x, y) => Convert.ToInt32(x.Id).CompareTo(Convert.ToInt32(y.Id)));
        dgrSegments.Items.Refresh();        
        GridHelper.SelectRow(dgrSegments, dgrSegments.SelectedIndex);
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
    /// [emoguel] created 31/05/2016
    /// </history>
    private void btnDown_Click(object sender, RoutedEventArgs e)
    {
      if (dgrSegments.SelectedIndex != dgrSegments.Items.Count)
      {
        List<Item> lstItems = (List<Item>)dgrSegments.ItemsSource;
        Item itemUp = lstItems[dgrSegments.SelectedIndex];
        Item itemDown = lstItems[dgrSegments.SelectedIndex + 1];
        int value = Convert.ToInt32(itemDown.Id);
        itemUp.Id = value.ToString();
        itemDown.Id = (value - 1).ToString();
        lstItems.Sort((x, y) => Convert.ToInt32(x.Id).CompareTo(Convert.ToInt32(y.Id)));
        dgrSegments.Items.Refresh();
        GridHelper.SelectRow(dgrSegments, dgrSegments.SelectedIndex);
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
        List<Item> lstItem = (List<Item>)dgrSegments.ItemsSource;
        if (ValidateChanges(lstItem))
        {
          List<Item> lstAgencies = lstItem.Where(it => it.By == "Agency").ToList();
          List<Item> lstLeadSources = lstItem.Where(it => it.By != "Agency").ToList();
          int nRes = await BRSegmentsOrder.SaveSegmentsOrder(lstAgencies, lstLeadSources);
          UIHelper.ShowMessageResult("Segments Order", nRes);
          if(nRes>0)
          {
            _lstOldSegments = new List<Item>();
            lstItem.ForEach(it => {
              Item ite = new Item();
              ObjectHelper.CopyProperties(ite, it);
              _lstOldSegments.Add(ite);
            });
          }
        }
        btnEdit.IsEnabled = true;
        btnCancel.IsEnabled = false;
        SetEditMode(false);
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Segments Order");
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
      List<Item> lstItems = (List<Item>)dgrSegments.ItemsSource;
      if (ValidateChanges(lstItems))
      {
        MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
        e.Cancel = (result == MessageBoxResult.No);
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadSegmentsOrder
    /// <summary>
    /// Llena el grid de Segments order
    /// </summary>
    /// <history>
    /// [emoguel] created 31/05/2016
    /// </history>
    private async void LoadSegmentsOrder()
    {
      try
      {
        status.Visibility = Visibility.Visible;
        txtStatus.Text = "Loading...";
        List<Item> lstObjects = await BRSegmentsOrder.GetSegmentsOrder();
        lstObjects = lstObjects.OrderBy(it => int.Parse(it.Id)).ToList();
        dgrSegments.ItemsSource = lstObjects;
        lstObjects.ForEach(it =>
        {
          Item item = new Item();
          ObjectHelper.CopyProperties(item,it);
          _lstOldSegments.Add(item);
        });        
        GridHelper.SelectRow(dgrSegments, 0);
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Segments Order");
      }
    }
    #endregion

    #region SetEditMode
    /// <summary>
    /// habilita y dehabilita los botones
    /// </summary>
    /// <param name="blnMode">true. habilita | False. Deshabilita</param>
    /// <history>
    /// [emoguel] created 01/06/2016
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
    /// [emoguel] created 01/06/2016
    /// </history>
    private bool ValidateChanges(List<Item> lstItems)
    {
      var lstChanges = _lstOldSegments.Where(it => lstItems.Any(itt => it.UserId == itt.UserId && it.Id == itt.Id)).ToList();

      return (lstChanges.Count != _lstOldSegments.Count);
    }
    #endregion

    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for GiftsOrder.xaml
  /// </summary>
  public partial class frmGiftsOrder : Window
  {
    public List<Gift> lstGifts = new List<Gift>();//Lista a ordenar

    public frmGiftsOrder()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window_Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadOrder();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/06/2016
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
    /// [emoguel] created 30/06/2016
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

    #region Cancel
    /// <summary>
    /// Cancela la edicion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/06/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      SetEditMode(false);
    }
    #endregion

    #region Down
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
      if (dgrGiftsOrder.SelectedIndex != -1)
      {
        if (dgrGiftsOrder.SelectedIndex != dgrGiftsOrder.Items.Count)
        {
          List<Gift> lstItems = (List<Gift>)dgrGiftsOrder.ItemsSource;
          Gift itemUp = lstItems[dgrGiftsOrder.SelectedIndex];
          Gift itemDown = lstItems[dgrGiftsOrder.SelectedIndex + 1];
          int value = Convert.ToInt32(itemDown.giO);
          itemUp.giO = value;
          itemDown.giO = value - 1;
          lstItems.Sort((x, y) => x.giO.CompareTo(y.giO));
          dgrGiftsOrder.Items.Refresh();
          GridHelper.SelectRow(dgrGiftsOrder, dgrGiftsOrder.SelectedIndex);
        }
      }
    }
    #endregion

    #region Up
    /// <summary>
    /// Cambia de orden el registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/06/2016
    /// </history>
    private void btnUp_Click(object sender, RoutedEventArgs e)
    {
      if (dgrGiftsOrder.SelectedIndex != -1)
      {
        if (dgrGiftsOrder.SelectedIndex != 0)
        {
          List<Gift> lstItems = (List<Gift>)dgrGiftsOrder.ItemsSource;
          Gift itemUp = lstItems[dgrGiftsOrder.SelectedIndex];
          Gift itemDown = lstItems[dgrGiftsOrder.SelectedIndex - 1];
          int value = Convert.ToInt32(itemDown.giO);
          itemUp.giO = value;
          itemDown.giO = value + 1;
          lstItems.Sort((x, y) => x.giO.CompareTo(y.giO));
          dgrGiftsOrder.Items.Refresh();
          GridHelper.SelectRow(dgrGiftsOrder, dgrGiftsOrder.SelectedIndex);
        }
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
    /// [emoguel] created 30/06/2016
    /// </history>
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        txtStatus.Text = "Saving Data...";
        status.Visibility = Visibility.Visible;

        List<Gift> lstGift = (List<Gift>)dgrGiftsOrder.ItemsSource;

        var lstSave = lstGift.Where(gi => lstGifts.Any(gii => gi.giID==gii.giID && gi.giO!=gii.giO)).ToList();
        int nRes = await BREntities.OperationEntities(lstSave,EnumMode.Edit);
        if(nRes>0)
        {
          UIHelper.ShowMessage("Gifts Order saved");
          lstSave.ForEach(gi =>
          {
            Gift Gift = lstGifts.Where(gii => gii.giID == gi.giID).FirstOrDefault();
            Gift.giO = gi.giO;
          });
        }
        btnEdit.IsEnabled = true;
        btnCancel.IsEnabled = false;
        SetEditMode(false);
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gifts Order");
      }
    }
    #endregion

    #region Edit
    /// <summary>
    /// habilita el modo edición
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/06/2016
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      SetEditMode(true);
    }
    #endregion

    #region Closing
    /// <summary>
    /// Verifica si se tiene cambios pendientes antes de cerrar la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      List<Gift> lstGift = (List<Gift>)dgrGiftsOrder.ItemsSource;

      var lstSave = lstGift.Where(gi => lstGifts.Any(gii => gi.giID == gii.giID && gi.giO != gii.giO)).ToList();

      if(lstSave.Count>0)
      {
        MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
        e.Cancel = (result == MessageBoxResult.No);
      }
    }
    #endregion
    #endregion

    #region Methods

    #region LoadOrder
    /// <summary>
    /// Llena el grid de order
    /// </summary>
    /// <history>
    /// [emoguel] created 30/06/2016
    /// </history>
    private async  void LoadOrder()
    {
      status.Visibility = Visibility.Visible;
      dgrGiftsOrder.ItemsSource= await Task.Run(() =>
      {
        List<Gift> lstG = new List<Gift>();
        lstGifts.ForEach(gi =>
        {
          Gift giftNew = new Gift();
          ObjectHelper.CopyProperties(giftNew, gi);
          lstG.Add(giftNew);
        });
        return lstG.OrderBy(gi=>gi.giO).ToList();
      });      
      status.Visibility = Visibility.Collapsed;
    } 
    #endregion

    #region SetEditMode
    /// <summary>
    /// habilita y dehabilita los botones
    /// </summary>
    /// <param name="blnMode">true. habilita | False. Deshabilita</param>
    /// <history>
    /// [emoguel] created 30/06/2016
    /// </history>
    private void SetEditMode(bool blnMode)
    {
      btnUp.IsEnabled = blnMode;
      btnDown.IsEnabled = blnMode;
      btnSave.IsEnabled = blnMode;
    }
    #endregion
    #endregion
  }
}

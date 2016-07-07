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
using IM.Model.Enums;
using IM.Model.Extensions;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;
using IM.Model;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmGifts.xaml
  /// </summary>
  public partial class frmGifts : Window
  {
    #region Variables
    private int _nStatus = -1;//estatus de los registros del grid
    private int _nPackage = -1;//Para saber si son paketes
    private Gift _giftFilter = new Gift {giProductGiftsCard="ALL",giPVPPromotion="ALL",giOperaTransactionType="ALL",gigc="",giPromotionOpera="ALL" };//Filtro de los registros del grid
    #endregion
    public frmGifts()
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
    /// [emoguel] modified 30/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadGifts();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/06/2016
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
    /// [emoguel] created 28/06/2016
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
    /// [emoguel] 28/06/2016
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
    /// [emoguel] created 28/06/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region btnRef_Click
    /// <summary>
    /// Recarga los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/06/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Gift gift = (Gift)dgrGifs.SelectedItem;
      LoadGifts(gift);
    }
    #endregion

    #region btnAdd_Click
    /// <summary>
    /// Recarga los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] cretaed 29/06/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Abre la ventana de buqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/06/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearchGifts frmSearchGifts = new frmSearchGifts();
      frmSearchGifts.nStatus = _nStatus;
      frmSearchGifts.nPackage = _nPackage;
      ObjectHelper.CopyProperties(frmSearchGifts.gift, _giftFilter);
      frmSearchGifts.Owner = this;
      if(frmSearchGifts.ShowDialog()==true)
      {
        _nStatus = frmSearchGifts.nStatus;
        _nPackage = frmSearchGifts.nPackage;
        ObjectHelper.CopyProperties(_giftFilter, frmSearchGifts.gift);
        LoadGifts();
      }
    }
    #endregion

    #region btnLog_Click
    /// <summary>
    /// Abre la ventana de Log
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/06/2016
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      if(dgrGifs.SelectedIndex!=-1)
      {
        Gift gift = (Gift)dgrGifs.SelectedItem;
        frmGiftsLog frmGiftsLog = new frmGiftsLog();
        frmGiftsLog.Owner = this;
        frmGiftsLog.idGift = gift.giID;
        frmGiftsLog.ShowDialog();
      }
      else
      {
        UIHelper.ShowMessage("Please Select a Gift");
      }
    }
    #endregion

    #region btnOrder_Click
    /// <summary>
    /// Abre la ventana de Order
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/06/2016
    /// </history>
    private void btnOrder_Click(object sender, RoutedEventArgs e)
    {
      List<Gift> lstGifts = (List<Gift>)dgrGifs.ItemsSource;
      frmGiftsOrder frmGiftsOrder = new frmGiftsOrder();
      frmGiftsOrder.Owner = this;
      frmGiftsOrder.lstGifts = lstGifts;
      frmGiftsOrder.ShowDialog();
    }
    #endregion
    #endregion

    #region Methods
    #region LoadGifts
    /// <summary>
    /// Llena el grid de gifts
    /// </summary>
    /// <param name="gift">Filtro a seleccionar</param>
    /// <history>
    /// [emoguel] created 30/06/2016
    /// </history>
    private async void LoadGifts(Gift gift = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Gift> lstGifts = await BRGifts.GetGifts(_nStatus, _giftFilter,_nPackage);
        dgrGifs.ItemsSource = lstGifts;
        if (lstGifts.Count > 0 && gift != null)
        {
          gift = lstGifts.Where(gi => gi.giID == gift.giID).FirstOrDefault();
          nIndex = lstGifts.IndexOf(gift);
        }
        GridHelper.SelectRow(dgrGifs, nIndex);
        StatusBarReg.Content = lstGifts.Count + " Gifts.";
        status.Visibility = Visibility.Collapsed;
        btnOrder.IsEnabled = true;
        btnLog.IsEnabled = true;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gifts");
      }
    } 
    #endregion
    #endregion
  }
}

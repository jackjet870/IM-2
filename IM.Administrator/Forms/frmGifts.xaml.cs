using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmGifts.xaml
  /// </summary>
  public partial class frmGifts : Window
  {
    #region Variables
    private bool _blnEdit = false;
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
      _blnEdit = Context.User.HasPermission(EnumPermission.Gifts, EnumPermisionLevel.Standard);
      LoadGifts();
      dgrGifs.CurrentCellChanged += GridHelper.dtg_CurrentCellChanged;
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
      Gift gift = (Gift)dgrGifs.SelectedItem;
      frmGiftDetail frmGiftDetail = new frmGiftDetail();
      frmGiftDetail.Owner = this;
      frmGiftDetail._oldGift = gift;
      frmGiftDetail.enumMode = _blnEdit ? EnumMode.Edit : EnumMode.ReadOnly;
      if(frmGiftDetail.ShowDialog()==true)
      {
        List<Gift> lstGifts = (List<Gift>)dgrGifs.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmGiftDetail.gift))
        {
          ObjectHelper.CopyProperties(gift, frmGiftDetail.gift);//Actualizamos los datos
          lstGifts.Sort((x, y) => string.Compare(x.giN, y.giN));//Ordenamos la lista
          nIndex = lstGifts.IndexOf(gift);//Obtenemos la posición del registro
        }
        else
        {
          lstGifts.Remove(gift);//Quitamos el registro
        }
        dgrGifs.Items.Refresh();
        GridHelper.SelectRow(dgrGifs, nIndex);

        StatusBarReg.Content = lstGifts.Count + " Gifts.";//Actualizamos el contador
      }
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
      frmGiftDetail frmGiftDetail = new frmGiftDetail();
      frmGiftDetail.enumMode = EnumMode.Add;
      frmGiftDetail.Owner = this;
      if(frmGiftDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmGiftDetail.gift))
        {
          List<Gift> lstGifts = (List<Gift>)dgrGifs.ItemsSource;
          lstGifts.Add(frmGiftDetail.gift);//Agregamos el registro
          lstGifts.Sort((x, y) => string.Compare(x.giN, y.giN));//Ordenamos la lista
          int nIndex = lstGifts.IndexOf(frmGiftDetail.gift);//Buscamos el index del registro
          dgrGifs.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrGifs, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstGifts.Count + " Gifts.";//Actualizamos el contador
        }
      }
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
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un gift cumpla con los filtros actuales
    /// </summary>
    /// <param name="gift">Objeto a validar</param>
    /// <returns>True. Si cumple | false. No cumple</returns>
    /// <history>
    /// [emoguel] created 22/07/2016
    /// </history>
    private bool ValidateFilter(Gift gift)
    {
      #region Status
      if (_nStatus != 1)
      {
        if (gift.giA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }
      #endregion

      #region Categories
      if (!string.IsNullOrWhiteSpace(_giftFilter.gigc))
      {
        if (gift.gigc != _giftFilter.gigc)
        {
          return false;
        }
      }
      #endregion

      #region GiftCard
      if (!string.IsNullOrEmpty(_giftFilter.giProductGiftsCard) && _giftFilter.giProductGiftsCard != "ALL")
      {
        switch (_giftFilter.giProductGiftsCard)
        {
          case "ANY":
            {
              if (string.IsNullOrWhiteSpace(gift.giProductGiftsCard))
              {
                return false;
              }
              break;
            }
          case "NONE":
            {
              if (!string.IsNullOrWhiteSpace(gift.giProductGiftsCard))
              {
                return false;
              }
              break;
            }
          default:
            {
              if (gift.giProductGiftsCard != _giftFilter.giProductGiftsCard)
              {
                return false;
              }
              break;
            }
        }
      }
      #endregion

      #region Promotion
      if (!string.IsNullOrEmpty(_giftFilter.giPVPPromotion) && _giftFilter.giPVPPromotion != "ALL")
      {
        switch (_giftFilter.giPVPPromotion)
        {
          case "ANY":
            {
              if (string.IsNullOrWhiteSpace(gift.giPVPPromotion))
              {
                return false;
              }
              break;
            }
          case "NONE":
            {
              if (!string.IsNullOrWhiteSpace(gift.giPVPPromotion))
              {
                return false;
              }
              break;
            }
          default:
            {
              if (gift.giPVPPromotion != _giftFilter.giPVPPromotion)
              {
                return false;
              }
              break;
            }
        }
      }
      #endregion

      #region Transacction
      if (!string.IsNullOrEmpty(_giftFilter.giOperaTransactionType) && _giftFilter.giOperaTransactionType != "ALL")
      {
        switch (_giftFilter.giOperaTransactionType)
        {
          case "ANY":
            {
              if (string.IsNullOrWhiteSpace(gift.giOperaTransactionType))
              {
                return false;
              }
              break;
            }
          case "NONE":
            {
              if (!string.IsNullOrWhiteSpace(gift.giOperaTransactionType))
              {
                return false;
              }
              break;
            }
          default:
            {
              if (gift.giOperaTransactionType != _giftFilter.giOperaTransactionType)
              {
                return false;
              }
              break;
            }
        }
      }
      #endregion

      #region Promotion Opera
      if (!string.IsNullOrEmpty(_giftFilter.giPromotionOpera) && _giftFilter.giPromotionOpera != "ALL")
      {
        switch (_giftFilter.giPromotionOpera)
        {
          case "ANY":
            {
              if (string.IsNullOrWhiteSpace(gift.giPromotionOpera))
              {
                return false;
              }
              break;
            }
          case "NONE":
            {
              if (!string.IsNullOrWhiteSpace(gift.giPromotionOpera))
              {
                return false;
              }
              break;
            }
          default:
            {
              if (gift.giPromotionOpera != _giftFilter.giPromotionOpera)
              {
                return false;
              }
              break;
            }
        }
      }
      #endregion

      return true;
    } 
    #endregion
    #endregion
  }
}

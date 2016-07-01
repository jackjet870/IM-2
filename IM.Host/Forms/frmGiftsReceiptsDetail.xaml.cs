using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Enums;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Linq;
using IM.Model.Helpers;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmGiftsReceipts.xaml
  /// </summary>
  public partial class frmGiftsReceiptsDetail : Window
  {
    #region Variables
    public bool _blnPublicOREmpleado = false, _CancelExternal = false; // FALSE - PUBLIC || TRUE - EMPLEADO
    private int _guestID = 0;
    public EnumModeOpen modeOpen;
    public GiftsReceiptDetail _giftCurrent = new GiftsReceiptDetail();
    private GiftsReceiptDetail _GiftOrigin;
    private GiftsReceiptDetail _GiftReceiptC;
    private int _GiftReceipt;
    private ObservableCollection<GiftsReceiptDetail> _obsGiftsCurrent, _obsGiftsComplet;
    #endregion

    #region CONSTRUCTOR
    /// <summary>
    ///  Contructor para edicion de Gift Detail
    /// </summary>
    /// <param name="dbContext"> Contexto general del formulario </param>
    /// <param name="obsGiftsTemp"> Lista Observable temporal </param>
    /// <param name="obsGiftsComplet"> Lista Observable Completa </param>
    /// <param name="lstGifts"> Lista de Gifts </param>
    /// <param name="giftDetail"> Gifs a Editar </param>
    /// <param name="guestID"> Identificador del Guest </param>
    /// <param name="publicOrEmpleado"> Bandera para verificar el tipo de precio a utilizar </param>
    /// <history>
    /// [vipacheco]  22/Abril/2016 Created
    /// </history>
    public frmGiftsReceiptsDetail(ref ObservableCollection<GiftsReceiptDetail> obsGiftsTemp,
                                  ObservableCollection<GiftsReceiptDetail> obsGiftsComplet,
                                  int guestID,
                                  int giftReceipt = 0,
                                  GiftsReceiptDetail GiftSelected = null,
                                  bool publicOrEmpleado = false,
                                  bool isCancelExternal = false)
    {
      _guestID = guestID;
      _GiftOrigin = GiftSelected; // Se guarda como llego originalmente, para futuras comparaciones.
      _obsGiftsCurrent = obsGiftsTemp;
      _obsGiftsComplet = obsGiftsComplet;
      _GiftReceipt = giftReceipt;
      _blnPublicOREmpleado = publicOrEmpleado;
      _CancelExternal = isCancelExternal;

      InitializeComponent();
    }
    #endregion

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      cboGift.ItemsSource  = frmHost._lstGiftsWithPackage;

      switch (modeOpen)
      {
        case EnumModeOpen.Edit:
          if (_giftCurrent.geInPVPPromo)
          {
            GiftsReceiptDetail _request = BRGiftsReceiptDetail.GetGiftReceiptDetail(_giftCurrent.gegr, _giftCurrent.gegi);

            if (_request != null)
            {
              txtQty.IsReadOnly = true;
              cboGift.IsEnabled = false;
            }
          }

          DataContext = _giftCurrent;
          break;
      }

      // Si se abrio del formulario Cancel External
      if (_CancelExternal)
      {
        // Se ocultan controles innecesarios
        chkAsPromotion.Visibility = Visibility.Hidden;
        chkCancelSistur.Visibility = Visibility.Hidden;
        chkInOpera.Visibility = Visibility.Hidden;
        chkInSistur.Visibility = Visibility.Hidden;
      }
    }
    #endregion

    #region ValidateNumber
    /// <summary>
    /// Valida que el texto introducido sea solamente de tipo numero
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void ValidateNumber(object sender, TextCompositionEventArgs e)
    {
      string _value = e.Text;

      // Se valida que solamente sean Digitos Numericos
      if (!char.IsDigit(Convert.ToChar(_value)))
      {
        e.Handled = true;
      }
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Cancela y cierra el formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 25/Abril/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
    #endregion

    #region btnSave_Click
    /// <summary>
    /// Guarda la informacion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      string _gift = cboGift.SelectedValue as string;
      Gift _Selected = frmHost._lstGifts.Where(x => x.giID == _gift).First();

      if (_Selected != null)
      {
        if (modeOpen == EnumModeOpen.Edit && _Selected.giID != _GiftOrigin.gegi || (modeOpen == EnumModeOpen.Add)) // Si es el mismo Gift en modo edicion no se realiza la validacion de repeticion.
        {
          // Verificamos que el Gift no se encuentre repetido
          if (_obsGiftsCurrent != null)
          {
            _obsGiftsCurrent.ToList().ForEach(x => {
                                                    if (x.gegi == _Selected.giID)
                                                    {
                                                      UIHelper.ShowMessage("Gifts must not be repeated. \r\n Gift repetead is '" + _Selected.giN + "'.", MessageBoxImage.Exclamation);
                                                      return;
                                                    }
                                                  });
          }
        }
      }
      else
      {
        UIHelper.ShowMessage("Choose a gift, please", MessageBoxImage.Information);
        return;
      }

      switch (modeOpen)
      {
        case EnumModeOpen.Add:
          GiftsReceiptDetail _giftsNew = new GiftsReceiptDetail
          {
            gegr = _GiftReceipt, // Se le asigna un valor 0 temporal hasta que le de guardar al Gift Receipt, se le asigna el ID del Gift Receipt correspondiente
            gegi = cboGift.SelectedValue.ToString(),
            gect = "MARKETING",
            geQty = Convert.ToInt32(txtQty.Text),
            geAdults = Convert.ToInt32(txtAdults.Text),
            geMinors = Convert.ToInt32(txtMinors.Text),
            geFolios = txtFolios.Text,
            gePriceA = Math.Round(Convert.ToDecimal(txtCAdults.Text), 4),
            gePriceM = Math.Round(Convert.ToDecimal(txtCMinors.Text), 4),
            geCharge = (decimal)Math.Round(0.0, 4),
            gecxc = null,
            geComments = txtComments.Text,
            geInElectronicPurse = false,
            geConsecutiveElectronicPurse = null,
            geCancelElectronicPurse = false,
            geExtraAdults = Convert.ToInt32(txtEAdults.Text),
            geInPVPPromo = chkInSistur.IsChecked.Value,
            geCancelPVPPromo = chkCancelSistur.IsChecked.Value,
            geInOpera = chkInOpera.IsChecked.Value,
            geAsPromotionOpera = chkAsPromotion.IsChecked.Value,
            gePriceAdult = Math.Round(Convert.ToDecimal(txtPAdults.Text), 4),
            gePriceMinor = Math.Round(Convert.ToDecimal(txtPMinors.Text), 4),
            gePriceExtraAdult = Math.Round(Convert.ToDecimal(txtPEAdults.Text), 4),
            geSale = chkSale.IsChecked.Value
          };

          // Verificamos que no se encuentre en la lista Actual
          if (_obsGiftsCurrent != null && _obsGiftsCurrent.Count > 0)
          {
            var _cointainsCurrent = _obsGiftsCurrent.Where(x => x.gegi == _giftsNew.gegi).SingleOrDefault();

            if (_cointainsCurrent == null)// Si no se encuentra en la lista Actual se agrega!
            {
              // Verificamos que no se encuentre en la lista original
              var _cointainsOrigin = _obsGiftsComplet.Where(x => x.gegi == _giftsNew.gegi).SingleOrDefault();

              if (_cointainsOrigin == null)
              {
                // Construimos la entity de acuerdo a ComplexType
                _GiftReceiptC = BuildGiftReceiptC();

                _obsGiftsCurrent.Add(_giftsNew);

                frmGiftsReceipts.logGiftDetail.Add(new KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>(Model.Enums.EnumMode.add, _GiftReceiptC));
              }
              else // Verificamos si hubo cambio en algun campo
              {
                if (!ObjectHelper.IsEquals(_cointainsOrigin, _giftsNew))
                {
                  _obsGiftsCurrent.Add(_giftsNew);

                  // Construimos la entity de acuerdo a ComplexType
                  _GiftReceiptC = BuildGiftReceiptC();
                  List<KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>> _result = frmGiftsReceipts.logGiftDetail.Where(x => x.Value.gegi == _GiftReceiptC.gegi).ToList();

                  if (_result != null && _result.Count > 0)
                  {
                    // eliminamos todas las acciones anteriores
                    foreach (KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail> item in _result)
                    {
                      frmGiftsReceipts.logGiftDetail.Remove(item);
                    }
                  }

                  frmGiftsReceipts.logGiftDetail.Add(new KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>(Model.Enums.EnumMode.edit, _GiftReceiptC));
                }
              }
            }
          }
          else // Es un Gift receipt NUEVO!
          {
            _GiftReceiptC = BuildGiftReceiptC();

            _obsGiftsCurrent.Add(_giftsNew);
            frmGiftsReceipts.logGiftDetail.Add(new KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>(Model.Enums.EnumMode.add, _GiftReceiptC));
          }

          break;
        case EnumModeOpen.Edit:
          _GiftReceiptC = BuildGiftReceiptC();

          if (!ObjectHelper.IsEquals(_giftCurrent, _GiftOrigin))
          {
            if (_giftCurrent.gegi == _GiftOrigin.gegi) // si es el mismo gift no se actualizo algun campo.
              frmGiftsReceipts.logGiftDetail.Add(new KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>(Model.Enums.EnumMode.edit, _GiftReceiptC));
            else // Se cambio de gift
              frmGiftsReceipts.logGiftDetail.Add(new KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>(Model.Enums.EnumMode.add, _GiftReceiptC));

            int i = _obsGiftsCurrent.IndexOf(_GiftOrigin);
            _obsGiftsCurrent.RemoveAt(i);
            _obsGiftsCurrent.Insert(i, _giftCurrent);
          }
          break;
      }

      Close();
    }
    #endregion

    #region BuildGiftReceiptC
    /// <summary>
    /// Construye un nuevo GiftsReceiptDetail
    /// </summary>
    /// <returns> GiftsReceiptDetail </returns>
    /// <history>
    /// [vipacheco] 27/Abril/2016 created
    /// </history>
    private GiftsReceiptDetail BuildGiftReceiptC()
    {
      return new GiftsReceiptDetail
      {
        gegr = _GiftReceipt, // Se le asigna un valor 0 temporal hasta que le de guardar al Gift Receipt, se le asigna el ID del Gift Receipt correspondiente
        gegi = cboGift.SelectedValue.ToString(),
        gect = "MARKETING",
        geQty = Convert.ToInt32(txtQty.Text),
        geAdults = Convert.ToInt32(txtAdults.Text),
        geMinors = Convert.ToInt32(txtMinors.Text),
        geFolios = string.IsNullOrEmpty(txtFolios.Text) ? null : txtFolios.Text,
        gePriceA = Math.Round(Convert.ToDecimal(txtCAdults.Text), 4),
        gePriceM = Math.Round(Convert.ToDecimal(txtCMinors.Text), 4),
        geCharge = (decimal)Math.Round(0.0, 4),
        gecxc = null,
        geComments = string.IsNullOrEmpty(txtComments.Text) ? null : txtComments.Text,
        geInElectronicPurse = false,
        geConsecutiveElectronicPurse = null,
        geCancelElectronicPurse = false,
        geExtraAdults = Convert.ToInt32(txtEAdults.Text),
        geInPVPPromo = chkInSistur.IsChecked.Value,
        geCancelPVPPromo = chkCancelSistur.IsChecked.Value,
        geInOpera = chkInOpera.IsChecked.Value,
        geAsPromotionOpera = chkAsPromotion.IsChecked.Value,
        gePriceAdult = Math.Round(Convert.ToDecimal(txtPAdults.Text), 4),
        gePriceMinor = Math.Round(Convert.ToDecimal(txtPMinors.Text), 4),
        gePriceExtraAdult = Math.Round(Convert.ToDecimal(txtPEAdults.Text), 4),
        geSale = chkSale.IsChecked.Value
      };
    }
    #endregion

    #region cboGift_SelectionChanged
    /// <summary>
    /// Realiza las validaciones necesarias cuando se cambia el tipo de Gift
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 27/Abril/2016 Created
    /// </history>
    private void cboGift_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      string _gift = cboGift.SelectedValue as string;

      Gift _Selected = frmHost._lstGifts.Where(x => x.giID == _gift).Single();

      if (_Selected != null)
      {
        if (_Selected.giWFolio)
          txtFolios.IsReadOnly = false;
        else
          txtFolios.IsReadOnly = true;
        if (_Selected.giWPax)
        {
          txtAdults.IsReadOnly = false;
          txtMinors.IsReadOnly = false;
          txtEAdults.IsReadOnly = false;
        }
        else
        {
          txtAdults.IsReadOnly = true;
          txtMinors.IsReadOnly = true;
          txtEAdults.IsReadOnly = true;
        }

        if (Convert.ToInt32(txtQty.Text) > _Selected.giMaxQty && _Selected.giMaxQty != 0)
        {
          UIHelper.ShowMessage("The maximun quantity authorized of the gift " + _Selected.giN + " has been exceeded.\r\n" +
                                "Max Authorized = " + _Selected.giQty, MessageBoxImage.Information);
          txtQty.Text = "1";
        }
        else
        {
          UpdateFields(_Selected);
        }

        // Verificamos si tiene paquete de regalos
        if (_Selected.giPack)
        {
          // Buscamos el paquete de regalos y lo agregamos a la lista
          //List<GiftsReceiptPackageItem> lstResult = await BRGiftsReceiptsPacks.GetGiftsReceiptPackage(_GiftReceipt, _gift.giID);
          //lstPacks = lstResult;//lstResult.Select(x => new { Qty = x.gkQty, Name = frmHost._lstGifts.Where(w => w.giID == x.gkgi).Select(s => s.giN).Single()});
          //lstGiftsPacks.Visibility = Visibility.Visible;
        }
        else
        {
          //lstGiftsPacks.Visibility = Visibility.Hidden;
        }

      }
    }
    #endregion

    #region UpdateFields
    /// <summary>
    /// Actualiza los campos necesarios
    /// </summary>
    /// <param name="_gift"></param>
    /// <history>
    /// [vipacheco] 27/Abril/2016 Created
    /// </history>
    private void UpdateFields(Gift _gift)
    {
      txtAdults.Text = "1";
      txtMinors.Text = "0";
      txtEAdults.Text = "0";

      UpdateCost(_gift);

      chkSale.IsChecked = _gift.giSale;
      chkInSistur.IsChecked = string.IsNullOrEmpty(_gift.giPVPPromotion) ? false : true;
      chkAsPromotion.IsChecked = string.IsNullOrEmpty(_gift.giPromotionOpera) ? false : true;
    }
    #endregion

    #region txtQty_LostFocus
    /// <summary>
    /// Calcula los costos al momento de perder el foco el campo Qty
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 27/Abril/2016 Created
    /// </history>
    private void txtQty_LostFocus(object sender, RoutedEventArgs e)
    {
      string _gift = cboGift.SelectedValue as string;
      Gift _selected = frmHost._lstGifts.Where(x => x.giID == _gift).First();

      if (Convert.ToInt32(txtQty.Text) > _selected.giMaxQty && _selected.giMaxQty != 0)
      {
        UIHelper.ShowMessage("The maximun quantity authorized of the gift " + _selected.giN + " has been exceeded.\r\n" +
                             "Max Authorized = " + _selected.giQty, MessageBoxImage.Information);
        txtQty.Text = "1";
        return;
      }
      else
      {
        UpdateCost(_selected);
      }
    }
    #endregion

    #region UpdateCost
    /// <summary>
    /// Actualiza los costos de acuerdo al gift seleccionado
    /// </summary>
    /// <param name="_gift"></param>
    /// <history>
    /// [vipacheco] 27/Abril/2016
    /// </history>
    private void UpdateCost(Gift _gift)
    {

      if (cboGift.SelectedItem != null)
      {
        // Obtenemos los valores de los campos
        int _qty = Convert.ToInt32(txtQty.Text);
        int _adults = Convert.ToInt32(txtAdults.Text);
        int _minors = Convert.ToInt32(txtMinors.Text);
        int _eAdults = Convert.ToInt32(txtEAdults.Text);

        if (_blnPublicOREmpleado) // TRUE es precio empleado
        {
          txtCAdults.Text = $"{ Math.Round(((_adults + _eAdults) * _gift.giPrice3) * _qty, 2)}";
          txtCMinors.Text = $"{Math.Round((_minors * _gift.giPrice4) * _qty, 2)}";
        }
        else // Precio publico 
        {
          txtCAdults.Text = $"{Math.Round(((_adults + _eAdults) * _gift.giPrice1) * _qty, 2)}";
          txtCMinors.Text = $"{Math.Round((_minors * _gift.giPrice2) * _qty, 2)}";
        }

        txtPAdults.Text = $"{Math.Round((_adults * _gift.giPublicPrice) * _qty, 2)}";
        txtPMinors.Text = $"{Math.Round((_minors * _gift.giPriceMinor) * _qty, 2)}";
        txtPEAdults.Text = $"{Math.Round((_eAdults * _gift.giPriceExtraAdult) * _qty, 2)}";
      }
    }
    #endregion

    #region Element_GotFocus
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 27/Abril/2016 created
    /// </history>
    private void Element_GotFocus(object sender, RoutedEventArgs e)
    {
      switch (modeOpen)
      {
        case EnumModeOpen.Add:
          if (chkInSistur.IsChecked.Value)
          {
            UIHelper.ShowMessage("You can not modify the quantity of gifts have been given in Sistur promotions.", MessageBoxImage.Information);
          }
          break;
        case EnumModeOpen.Edit:
          if (_giftCurrent.geInPVPPromo)
            UIHelper.ShowMessage("You can not modify the quantity of gifts have been given in Sistur promotions.", MessageBoxImage.Information);
          break;
      }
    }
    #endregion

    #region Element_LostFocus
    /// <summary>
    ///  Calcula los costos del gift seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 27/Abril/2016 Created
    /// </history>
    private void Element_LostFocus(object sender, RoutedEventArgs e)
    {
      string _gift = cboGift.SelectedValue as string;

      // Buscamos el Gift seleccionado
      Gift _Selected = frmHost._lstGifts.Where(x => x.giID == _gift).Single();

      // Actualizamos costos
      UpdateCost(_Selected);
    }
    #endregion
  }

}

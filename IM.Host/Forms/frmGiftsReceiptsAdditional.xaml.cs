using IM.BusinessRules.BR;
using IM.Host.Classes;
using IM.Model;
using PalaceResorts.Common.PalaceTools;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmGiftsReceiptsAdditional.xaml
  /// </summary>
  public partial class frmGiftsReceiptsAdditional : Window
  {
    private int _GuestID;
    public static bool Result = false;
    private List<GetGiftsReceiptsAdditional> _lstGiftsAdditional;
    public frmGiftsReceipts _frmGiftsReceipt;

    #region frmGiftsReceiptsAdditional
    /// <summary>
    /// 
    /// </summary>
    /// <param name="frmGiftsParent"></param>
    /// <param name="GuestID"></param>
    /// <history>
    /// [vipacheco] 12/Mayo/2016 Created
    /// </history>
    public frmGiftsReceiptsAdditional(frmGiftsReceipts frmGiftsParent, int GuestID = 0)
    {
      _GuestID = GuestID;
      _frmGiftsReceipt = frmGiftsParent;

      InitializeComponent();

      // Impedimos modificar los datos si el sistema esta en modo lectura
      if (ConfigHelper.GetString("ReadOnly").ToUpper().Equals("TRUE"))
        btnSave.Visibility = Visibility.Hidden;

    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <history>
    /// [vipacheco] 12/Mayo/2016 Created
    /// </history>
    private void btnCancel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Close();
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga los componentes y la informacion inicial
    /// </summary>
    /// <history>
    /// [vipacheco] [vipacheco] 12/Mayo/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      CollectionViewSource _dsGiftsReceiptsAdditional = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptsAdditional")));

      // Obtenemos los Gifts Adicionales
      _lstGiftsAdditional = BRGiftsReceipts.GetGiftsReceiptsAdditional(_GuestID);

      // Veririficamos si se encontro alguna informacion
      if (_lstGiftsAdditional != null)
      {
        if (_lstGiftsAdditional.Count > 0)
        {
          _dsGiftsReceiptsAdditional.Source = _lstGiftsAdditional;
        }
        else
        {
          btnCancel.Visibility = Visibility.Collapsed;
          btnSave.Visibility = Visibility.Collapsed;
        }
      }
      else
      {
        btnCancel.Visibility = Visibility.Collapsed;
        btnSave.Visibility = Visibility.Collapsed;
      }
    }
    #endregion

    #region btnSave_Click
    /// <summary>
    /// Guarda los Gifts Receipt Additional ingresados
    /// </summary>
    /// <history>
    /// [vipacheco] 12/Mayo/2016 Created
    /// </history>
    private async void btnSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      int lngReceiptID = 0;
      int lngGuestID = 0;

      foreach (GetGiftsReceiptsAdditional currentRow in dtgGuestsAdditional.Items)
      {
        // si se desea generar el recibo de regalos
        if (currentRow.Generate.Value)
        {
          lngGuestID = currentRow.gaAdditional;

          // Agregamos el recibo de regalos
          lngReceiptID = await AddReceipt(lngGuestID);

          GiftsReceiptsShort giftReceiptShort = new GiftsReceiptsShort() { grID = lngReceiptID, grNum = "", grExchange = false };
          _frmGiftsReceipt.obsGiftsReceipt.Add(giftReceiptShort);

          // Agregamos los regalos del recibo
          AddGifts(lngGuestID, lngReceiptID);

          // Indica que el huesped ya tiene recibo de regalos
          Guest _Guest = await BRGuests.GetGuest(lngGuestID,true);
          _Guest.guGiftsReceived = true;
          await BREntities.OperationEntity(_Guest, Model.Enums.EnumMode.Edit);

          // Actualizamos los regalos de la invitacion
          UpdateInvitsGifts(lngReceiptID, lngGuestID);

          // Guardamos el historico del recibo
          await BRGiftsReceiptLog.SaveGiftsReceiptsLog(lngReceiptID, _frmGiftsReceipt.txtChangedBy.Text);
        }
      }
      Close();
    }
    #endregion

    #region AddReceipt
    /// <summary>
    /// Agrega un recibo de regalos
    /// </summary>
    /// <param name="GuestID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 13/Mayo/2016 Created
    /// </history>
    private async Task<int> AddReceipt(int GuestID)
    {
      // Obtenemos los datos del huesped
      Guest _Guest = await BRGuests.GetGuest(GuestID);
      string _FullName = Common.GetFullName(_Guest.guLastName1 ?? "", _Guest.guFirstName1 ?? "");
      string _FullName2 = Common.GetFullName(_Guest.guLastname2 ?? "", _Guest.guFirstName2 ?? "");

      bool boolTemp = false;

      // Creamos el recibo de regalos
      GiftsReceipt _GiftsReceipt = new GiftsReceipt
      {
        grNum = null,
        grD = _frmGiftsReceipt.dtpgrD.Value.Value,
        grgu = _Guest.guID,
        grGuest = _FullName.Length > _frmGiftsReceipt.txtgrGuest.MaxHeight ? _FullName.Substring(0, 20) : _FullName,
        grPax = _Guest.guPax,
        grHotel = _Guest.guHotel,
        grRoomNum = _Guest.guRoomNum,
        grpe = (_frmGiftsReceipt.cmbgrpe.SelectedItem as PersonnelShort).peID,
        grlo = _frmGiftsReceipt.cmbgrlo.SelectedValue.ToString(),
        grls = _frmGiftsReceipt.cmbgrlo.SelectedValue.ToString(),
        grsr = _frmGiftsReceipt.cmbSalesRoom.SelectedValue.ToString(),
        grWh = _frmGiftsReceipt.cmbSalesRoom.SelectedValue.ToString(),
        grMemberNum = null,
        grHost = (_frmGiftsReceipt.cmbgrHost.SelectedItem as PersonnelShort).peID,
        grComments = null,
        grDeposit = 0,
        grDepositTwisted = 0,
        grcu = "US",
        grcxcPRDeposit = 0,
        grcucxcPRDeposit = "US",
        grCxCClosed = false,
        grExchangeRate = 0,
        grct = _frmGiftsReceipt.cmbgrct.SelectedValue.ToString(),
        grMaxAuthGifts = _frmGiftsReceipt.CalculateMaxAuthGifts(_frmGiftsReceipt.cmbgrct.SelectedValue.ToString(), _Guest.guls, ref boolTemp),
        grcxcGifts = 0,
        grcxcAdj = 0,
        grcxcComments = null,
        grTaxiIn = 0,
        grTaxiOut = 0,
        grCancel = false,
        grClosed = false,
        grCxCAppD = null,
        grTaxiOutDiff = 0,
        grGuest2 = _FullName2.Length > _frmGiftsReceipt.txtgrGuest2.MaxHeight ? _FullName2.Substring(0, 20) : _FullName2,
        grpt = "CS",
        grReimpresion = 0,
        grrm = null,
        grAuthorizedBy = null,
        grAmountToPay = null,
        grup = null,
        grcxcTaxiOut = 0,
        grcucxcTaxiOut = "US",
        grcxcAuthComments = null,
        grCancelD = null,
        grAmountPaid = 0,
        grBalance = 0
      };

      // Guardamos el ReceiptGifts y Obtenemos el ID generado.
      return await BRGiftsReceipts.SaveGiftReceipt(_GiftsReceipt);
    }
    #endregion


    #region AddGifts
    /// <summary>
    /// Agrega los regalos de un recibo de regalos
    /// </summary>
    /// <param name="GuestID"></param>
    /// <param name="ReceiptID"></param>
    /// <history>
    /// [vipacheco] 13/Mayo/2016 Created
    /// </history>
    private async void AddGifts(int GuestID, int ReceiptID)
    {
      // Obtenemos los regalos del huesped
      List<InvitationGift> _lstInvitationGifts = await BRInvitsGifts.GetInvitsGiftsByGuestID(GuestID);
      List<GiftsReceiptDetail> _lstGiftReceiptDetail = new List<GiftsReceiptDetail>();

      // Recorremos los regalos de la invitacion
      foreach (InvitationGift current in _lstInvitationGifts)
      {
        GiftsReceiptDetail _newGiftDetail = new GiftsReceiptDetail
        {
          gegr = ReceiptID,
          gegi = current.iggi,
          gect = "MARKETING",
          geQty = current.igQty,
          geAdults = current.igAdults,
          geMinors = current.igMinors,
          gePriceA = current.igPriceA,
          gePriceM = current.igPriceM,
          geCharge = 0,
          geInElectronicPurse = false,
          geCancelElectronicPurse = false,
          geExtraAdults = 0,
          geInPVPPromo = false,
          geCancelPVPPromo = false,
          geInOpera = false,
          gePromotionOpera = null,
          gePriceAdult = 0,
          gePriceMinor = 0,
          gePriceExtraAdult = 0,
          geSale = false
        };

        _lstGiftReceiptDetail.Add(_newGiftDetail);
      }

      // Aplicamos los cambios en la BD
      if (_lstGiftReceiptDetail.Count > 0)
      {
        _lstGiftReceiptDetail.ForEach(async item => await BREntities.OperationEntity(item, Model.Enums.EnumMode.Add));
      }

    }
    #endregion

    #region UpdateInvitsGifts
    /// <summary>
    /// Actualiza los regalos de la invitacion
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <param name="GuestID"></param>
    /// <history>
    /// [vipacheco] 13/Mayo/2016 Created
    /// </history>
    private async void UpdateInvitsGifts(int ReceiptID, int GuestID)
    {
      // Seleccionamos los regalos de la invitacion
      List<InvitationGift> _lstInvitationGifts = await BRInvitsGifts.GetInvitsGiftsByGuestID(GuestID);

      foreach (InvitationGift Current in _lstInvitationGifts)
      {
        Current.iggr = ReceiptID;
        await BREntities.OperationEntity(Current, Model.Enums.EnumMode.Edit);
      }
    }
    #endregion

  }
}

using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Services.Helpers;
using IM.Services.SisturService;
using PalaceResorts.Common.PalaceTools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IM.Services.Helpers
{
  /// <summary>
  /// Clase para el manejo del cliente que consume el servicio web Servicios Turisticos
  /// </summary>
  /// <history>
  /// [vipacheco] 26/Mayo/2016 Created
  /// </history>
  public class SisturHelper
  {

    #region Atributos

    private static PromocionesControllerService ServiceSistur;

    #endregion

    #region Current
    /// <summary>
    /// Recupera una instancia singleton del servicio web
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    public static PromocionesControllerService Current(EnumPromotionsSystem enumPromotionsSystems = EnumPromotionsSystem.Sistur)
    {
      if (ServiceSistur == null)
      {
        // Creamos una instancia del servicio
        ServiceSistur = new PromocionesControllerService();
        ServiceSistur.Url = ConfigHelper.GetString("Sistur.URL");

        return ServiceSistur;
      }
      else
        return ServiceSistur;
    } 
    #endregion

    #region CancelPromotionSistur
    /// <summary>
    /// Cancela una promoción de Sistur
    /// </summary>
    /// <param name="Gift"></param>
    /// <param name="Promotion"></param>
    /// <param name="GiftsCancelled"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    public static bool CancelPromotionSistur(string Gift, string Promotion, string Program, string LeadSource, string PropertyOpera, int ReceiptID, TextBox txtReservation, EnumPromotionsSystem enumPromotionsSystem, ref List<string> GiftsCancelled)
    {
      ActualizaEstatusPromoForzadaResponse Response = new ActualizaEstatusPromoForzadaResponse();
      string Hotel, Reservation;

      // si es inhouse
      if (Program == "IH")
      {
        // si es una invitacion inhouse
        if (txtReservation.Text != null)
        {
          Hotel = LeadSource;
          Reservation = txtReservation.Text;
        }
        // si es una invitacion externa
        else
        {
          Hotel = PropertyOpera;
          Reservation = $"{ReceiptID}";
        }
      }
      // Si es outhouse
      else
      {
        Hotel = PropertyOpera;
        Reservation = $"{ReceiptID}";
      }

      // Si se puede cancelar la promocion de sistur
      if (AllowCancelPromotionSistur(Hotel, Reservation, Promotion, Gift, enumPromotionsSystem))
      {
        // Se da de baja la promocion en el sistema de promociones
        Response = ActualizaEstatusPromoForzadaFolio(Hotel, Reservation, Promotion, enumPromotionsSystem);

        if (!Response.hasErrors)
          return true;
      }

      return false;
    } 
    #endregion

    #region ActualizaEstatusPromoForzadaFolio
    /// <summary>
    /// Cancela una promocion
    /// </summary>
    /// <param name="hotel"></param>
    /// <param name="folio"></param>
    /// <param name="promotion"></param>
    /// <param name="enumPromotionsSystem"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    private static ActualizaEstatusPromoForzadaResponse ActualizaEstatusPromoForzadaFolio(string hotel, string folio, string promotion, EnumPromotionsSystem enumPromotionsSystem)
    {
      ActualizaEstatusPromoForzadaRequest Request = new ActualizaEstatusPromoForzadaRequest();
      ActualizaEstatusPromoForzadaResponse Response = null;

      try
      {
        // configuramos el request
        Request.hotel = hotel;
        Request.folio = folio;
        Request.promo = promotion;
        Request.estatus = "BAJA";

        // Invocamos al servicio web
        Response = Current(enumPromotionsSystem).ActualizaEstatusPromoForzadaFolio(Request);

        // si ocurrio un error
        if (Response.hasErrors)
          UIHelper.ShowMessage(Response.message + "\r\n"+ Response.errorInfo, MessageBoxImage.Error, "ActualizaEstatusPromoForzadaFolio");
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "ActualizaEstatusPromoForzadaFolio");
      }

      return Response;
    }
    #endregion

    #region AllowCancelPromotionSistur
    /// <summary>
    /// Determina si se puede cancelar una promocion de Sistur
    /// </summary>
    /// <param name="Hotel"></param>
    /// <param name="Reservation"></param>
    /// <param name="Promotion"></param>
    /// <param name="Gift"></param>
    /// <param name="enumPromotionsSystem"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    private static bool AllowCancelPromotionSistur(string Hotel, string Reservation, string Promotion, string Gift, EnumPromotionsSystem enumPromotionsSystem)
    {
      string Error = "";
      OstdtpromosResponse[] Details;
      bool blnAllow = false;
      OstpromosResponse Header = null;
      float Used = 0;

      Details = ObtenerDetalleEstadoCuenta(Hotel, Reservation, Promotion, ref Error, enumPromotionsSystem);

      // si ocurrio un error al consultar el detalle del estado de cuenta
      if (Error != "")
        return false;

      // si no tiene detalle
      if (Details == null)
        blnAllow = true;
      else if ((Details.Length == 0))
        blnAllow = true;
      // si tiene detalle
      else
      {
        Header = ObtenerCabeceraEstadoCuenta(Hotel, Convert.ToInt32(Reservation), Promotion, ref Error, enumPromotionsSystem);

        // si ocurrio un error al consultar el detalle del estado de cuenta
        if (Error != "")
        {
          return false;
        }
          

        //  calculamos el monto usado del detalle
        foreach (OstdtpromosResponse Detail in Details)
        {
          if (Detail.idpromo == Promotion)
          {
            if (Detail.operacion.Equals("C"))
              Used += Detail.monto;
            else
              Used -= Detail.monto;
          }
        }
      }

      return blnAllow;
    }
    #endregion

    #region ObtenerDetalleEstadoCuenta
    /// <summary>
    /// Consulta el detalle del estado de cuenta de una promocion
    /// </summary>
    /// <param name="hotel"></param>
    /// <param name="folio"></param>
    /// <param name="promotion"></param>
    /// <param name="error"></param>
    /// <param name="enumPromotionsSystem"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    public static OstdtpromosResponse[] ObtenerDetalleEstadoCuenta(string hotel, string folio, string promotion, ref string error, EnumPromotionsSystem enumPromotionsSystem)
    {
      OstdtpromosRequest Request = new OstdtpromosRequest();
      OstdtpromosResponse[] Response = null;

      try
      {
        // configuramos el request
        Request.hotel = hotel;
        Request.folio = folio;
        Request.idpromo = promotion;

        // Invocamos al servicio web
        Response = Current(enumPromotionsSystem).ObtenerDetalleEstadoCuenta(Request);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "ObtenerDetalleEstadoCuenta");
        error = ex.Message;
      }

      return Response;
    }
    #endregion

    #region ObtenerCabeceraEstadoCuenta
    /// <summary>
    /// Consulta el encabezado del estado de cuenta de una promocion
    /// </summary>
    /// <param name="hotel"></param>
    /// <param name="folio"></param>
    /// <param name="promotion"></param>
    /// <param name="error"></param>
    /// <param name="enumPromotionsSystem"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    private static OstpromosResponse ObtenerCabeceraEstadoCuenta(string hotel, int folio, string promotion, ref string error, EnumPromotionsSystem enumPromotionsSystem)
    {
      BeneficiosxFolioRequest Request = new BeneficiosxFolioRequest();
      OstpromosResponse[] Response = null;
      OstpromosResponse Header = null;

      try
      {
        // Configuramos el request
        Request.hotel = hotel;
        Request.folio = folio;
        Request.idpromo = promotion;

        // invocamos al servicio web
        Response = Current(enumPromotionsSystem).ObtenerCabeceraEstadoCuenta(Request);

        if (Response != null)
        {
          Header = Response[0];
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "ObtenerCabeceraEstadoCuenta");
        error = ex.Message;
      }

      return Header;
    }
    #endregion

    #region SavePromotionsSistur
    /// <summary>
    /// Guarda en el sistema de promociones las promociones de regalo
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <param name="ChangeBy"></param>
    /// <param name="UserID"></param>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    public static void SavePromotionsSistur(int ReceiptID, string UserID, string ChangeBy = "")
    {
      List<GiftType> aGifts = new List<GiftType>();
      bool Error = false;
      EnumPromotionsSystem System = EnumPromotionsSystem.Sistur;
      GuardaPromocionForzadaRequest[] aPromotions = null;
      GuardaPromocionForzadaRequest Request = null;
      GuardaPromocionForzadaResponse Response = null;
      string GiftsSaved = "", GiftsNoSaved = "";

      // obtenemos los regalos de promociones de Sistur
      aPromotions = GetGiftsPromotionsSistur(ReceiptID, ChangeBy, UserID, ref aGifts, ref System);

      // si algun regalo tiene promociones de Sistur
      if (aPromotions != null)
      {
        if (aPromotions.Length > 0)
        {
          for (int i = 0; i <= aPromotions.Length - 1; i++)
          {
            Request = aPromotions[i];

            // Invocamos al servicio web para insertar cada regalo que se pretende asignar
            Response = GuardarPromocionForzada(Request, ref Error, System);

            // si se guardo exitosamente en Sistur
            if (!Error)
            {
              // actualizamos los regalos para identificarlos como que se guardaron en Sistur
              BRGiftsReceiptDetail.UpdateGiftPromotionSistur(aGifts[i].Receipt, aGifts[i].ID, aGifts[i].Promotion);
              GiftsSaved += aGifts[i].Descripcion + "\r\n";
            }
            else
              GiftsNoSaved += aGifts[i].Descripcion + "\r\n";
          }

          // si se pudo guardar todas las promociones
          if (string.IsNullOrEmpty(GiftsSaved) || GiftsSaved == "")
            UIHelper.ShowMessage("Gifts were successfully saved in Sistur as Promotion", MessageBoxImage.Information);
          // si no se pudo guardar ninguna promocion
          else if (string.IsNullOrEmpty(GiftsNoSaved) || GiftsNoSaved == "")
            UIHelper.ShowMessage("None gift was saved in Sistur as Promotion", MessageBoxImage.Information);
          // si no se pudo guardar alguna promocion
          else
            UIHelper.ShowMessage("The following gifts were saved in Sistur as Promotion:\r\n" + GiftsSaved + "\r\n\r\n" + "But the following gifts were not saved: \r\n" + GiftsNoSaved, MessageBoxImage.Information);

        }

      }
    } 
    #endregion

    #region GuardarPromocionForzada
    /// <summary>
    /// Guarda una promocion
    /// </summary>
    /// <param name="Request"></param>
    /// <param name="Error"></param>
    /// <param name="enumPromotionsSystem"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    private static GuardaPromocionForzadaResponse GuardarPromocionForzada(GuardaPromocionForzadaRequest Request, ref bool Error, EnumPromotionsSystem enumPromotionsSystem)
    {
      GuardaPromocionForzadaResponse Response = null;

      // invocamos al servicio web
      Response = Current(enumPromotionsSystem).GuardarPromocionForzada(Request);

      // Si ocurrio un error
      if (Response.hasErrors)
      {
        UIHelper.ShowMessage(Response.message, MessageBoxImage.Error, "GuardarPromocionForzada");
        Error = true;
      }

      return Response;
    } 
    #endregion

    #region GetGiftsPromotionsSistur
    /// <summary>
    /// Obtiene los regalos de promociones de Sistur
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <param name="ChangeBy"></param>
    /// <param name="UserID"></param>
    /// <param name="aGifts"></param>
    /// <param name="enumPromotionsSystem"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    public static GuardaPromocionForzadaRequest[] GetGiftsPromotionsSistur(int ReceiptID, string ChangeBy, string UserID, ref List<GiftType> aGifts, ref EnumPromotionsSystem enumPromotionsSystem)
    {
      GuardaPromocionForzadaRequest[] aPromotions = null;
      string Hotel = string.Empty;
      string Folio = string.Empty;
      //int Company = 0;
      string Application = string.Empty;
      string MembershipType = string.Empty;

      // obtenemos las promociones de Sistur que no se han dado
      List<GiftsReceiptDetailPromotionsSistur> Result = BRGiftsReceiptDetail.GetGiftsReceiptDetailPromotionsPVP(ReceiptID);

      // por default se van a guardar al sistema de Promociones de Sistur
      enumPromotionsSystem = EnumPromotionsSystem.Sistur;

      if (Result.Count > 0)
      {
        GiftsReceiptDetailPromotionsSistur Detail = Result[0];

        // Si es inhouse
        if (Detail.lspg.Equals("IH"))
        {
          // Si es una invitacion inhouse
          if (Detail.guHReservID != null)
          {
            Hotel = Detail.gulsOriginal;
            Folio = Detail.guHReservID;
          }
          // si es una invitacion externa
          else
          {
            Hotel = Detail.lsPropertyOpera;
            Folio = $"{ReceiptID}";
          }

          // Si el Lead Source usa sistur
          if (Detail.lsUseSistur)
            enumPromotionsSystem = EnumPromotionsSystem.Sistur;
        }
        // Si es outhouse
        else
        {
          Hotel = Detail.srPropertyOpera;
          Folio = $"{ReceiptID}";

          // Si la sala de ventas usa sistur
          if (Detail.srUseSistur)
            enumPromotionsSystem = EnumPromotionsSystem.Sistur;
        }

        // si es una invitacion externa u outhouse
        if ((Detail.lspg.Equals("IH") && Detail.guHReservID == null) || (Detail.lspg.Equals("OUT")))
        {
          // Obtenemos la afiliacion de intelligence contracts
          //Membe


          // Guardamos la reservacion ficticia
          //WirePRHelper.Origos_reservas_ficticias_Guardar(Hotel, Folio, , , Company, Application, MembershipType);

        }
      }

      for (int i = 1; i < Result.Count - 1; i++)
      {
        GiftsReceiptDetailPromotionsSistur _Detail = Result[i];

        // preparamos los regalos con promociones que se guardaran en Sistur
        AddPromotion(ref aPromotions, Hotel, Folio, _Detail.giPVPPromotion, _Detail.gegi, ReceiptID, (!string.IsNullOrEmpty(ChangeBy) ? ChangeBy : UserID), enumPromotionsSystem);

        // preparamos los regalos con promociones que se guardaran en Origos
        GiftType typGift = new GiftType();
        typGift.ID = _Detail.gegi;
        typGift.Descripcion = _Detail.giN;
        typGift.Quantity = 0;
        typGift.Receipt = ReceiptID;
        typGift.Promotion = _Detail.giPVPPromotion;
        typGift.TransactionTypeOpera = "";

        // Agregamos a la lista
        aGifts.Add(typGift);
      }

      return aPromotions;
    } 
    #endregion

    #region AddPromotion
    /// <summary>
    /// Agrega una promocion
    /// </summary>
    /// <param name="Promotions"></param>
    /// <param name="Hotel"></param>
    /// <param name="Folio"></param>
    /// <param name="Promotion"></param>
    /// <param name="GiftID"></param>
    /// <param name="ReceiptID"></param>
    /// <param name="User"></param>
    /// <param name="enumPromotionsSystem"></param>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    public static void AddPromotion(ref GuardaPromocionForzadaRequest[] Promotions, string Hotel, string Folio, string Promotion, string GiftID, int ReceiptID, string User, EnumPromotionsSystem enumPromotionsSystem)
    {
      int iLength = -1;
      iLength = Promotions.Length;
      iLength++;

      Array.Resize(ref Promotions, iLength);
      Promotions[iLength] = new GuardaPromocionForzadaRequest();
      Promotions[Promotions.Length].hotel = Hotel;
      Promotions[Promotions.Length].folio = Folio;

      if (enumPromotionsSystem == EnumPromotionsSystem.PVP)
        Promotions[Promotions.Length].adicional = "1";
      else
        Promotions[Promotions.Length].adicional = "0";

      Promotions[Promotions.Length].idpromo = Promotion;
      Promotions[Promotions.Length].GiftID = GiftID;
      Promotions[Promotions.Length].GiftsReceiptID = $"{ReceiptID}";
      Promotions[Promotions.Length].usralta = User;
      Promotions[Promotions.Length].fechaalta = BRHelpers.GetServerDate();
      Promotions[Promotions.Length].idtipoasignacion = "CONT";
      Promotions[Promotions.Length].status = "A";
      Promotions[Promotions.Length].tipomov = "";
      //// verificar hotelnew --->
      //// verificar folionew --->
      Promotions[Promotions.Length].usrmodif = "";
    } 
    #endregion

  }
}

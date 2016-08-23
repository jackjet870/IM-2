using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Services.SisturService;
using PalaceResorts.Common.PalaceTools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Linq;

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
    public static PromocionesControllerService Current()
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
    /// [vipacheco] 25/Julio/2016 Modified --> se agrego asyncronia
    /// </history>
    public async static Task<bool> CancelPromotionSistur(string pGift, string Promotion, string Program, string LeadSource, string PropertyOpera, int ReceiptID, TextBox txtReservation, List<string> pGiftsCancelled)
    {
      try
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
        if (await AllowCancelPromotionSistur(Hotel, Reservation, Promotion, pGift))
        {
          // Se da de baja la promocion en el sistema de promociones
          Response = await ActualizaEstatusPromoForzadaFolio(Hotel, Reservation, Promotion);
          //Si ocurrio un error 
          if (Response.hasErrors)
            throw new Exception(Response.errorInfo);

          // cancelamos el regalo en Origos
          await BRGiftsReceiptDetail.UpdateGiftsPromotionSisturCancel(ReceiptID, pGift);

          // Agregamos el regalo a la lista de regalos cancelados
          pGiftsCancelled.Add(pGift);

          return true;
        }
        return false;
      }
      catch (Exception)
      {
        throw;
      }

    }
    #endregion

    #region ActualizaEstatusPromoForzadaFolio
    /// <summary>
    /// Cancela una promocion
    /// </summary>
    /// <param name="hotel"></param>
    /// <param name="folio"></param>
    /// <param name="promotion"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    private async static Task<ActualizaEstatusPromoForzadaResponse> ActualizaEstatusPromoForzadaFolio(string hotel, string folio, string promotion)
    {
      return await Task.Run(() =>
      {
        try
        {
          ActualizaEstatusPromoForzadaRequest Request = new ActualizaEstatusPromoForzadaRequest();
          ActualizaEstatusPromoForzadaResponse Response = null;
          // configuramos el request
          Request.hotel = hotel;
          Request.folio = folio;
          Request.promo = promotion;
          Request.estatus = "BAJA";

          // Invocamos al servicio web
          Response = Current().ActualizaEstatusPromoForzadaFolio(Request);

          // si ocurrio un error
          if (Response.hasErrors)
            throw new Exception(Response.errorInfo);
          return Response;
        }
        catch (Exception)
        {
          throw;
        }
      });
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
    private async static Task<bool> AllowCancelPromotionSistur(string Hotel, string Reservation, string Promotion, string Gift)
    {

      OstdtpromosResponse[] Details;
      bool blnAllow = false;
      OstpromosResponse Header = null;
      float Used = 0;

      Details = await ObtenerDetalleEstadoCuenta(Hotel, Reservation, Promotion);

      // si no tiene detalle
      if (Details == null)
        blnAllow = true;
      else if ((Details.Length == 0))
        blnAllow = true;
      // si tiene detalle
      else
      {
        Header = ObtenerCabeceraEstadoCuenta(Hotel, Convert.ToInt32(Reservation), Promotion);
        // si ocurrio un error al consultar el detalle del estado de cuenta
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
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// [vipacheco] 25/Julio/2016 Modified --> Se agreggo asyncronia
    /// </history>
    public async static Task<OstdtpromosResponse[]> ObtenerDetalleEstadoCuenta(string hotel, string folio, string promotion)
    {
      return await Task.Run(() =>
      {
        OstdtpromosRequest Request = new OstdtpromosRequest();
        OstdtpromosResponse[] Response = null;

        // configuramos el request
        Request.hotel = hotel;
        Request.folio = folio;
        Request.idpromo = promotion;

        // Invocamos al servicio web
        Response = Current().ObtenerDetalleEstadoCuenta(Request);
        return Response;
      });
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
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    private static OstpromosResponse ObtenerCabeceraEstadoCuenta(string hotel, int folio, string promotion)
    {
      BeneficiosxFolioRequest Request = new BeneficiosxFolioRequest();
      OstpromosResponse[] Response = null;
      OstpromosResponse Header = null;
      // Configuramos el request
      Request.hotel = hotel;
      Request.folio = folio;
      Request.idpromo = promotion;

      // invocamos al servicio web
      Response = Current().ObtenerCabeceraEstadoCuenta(Request);

      if (Response != null)
      {
        Header = Response[0];
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
    public async static Task<string> SavePromotionsSistur(int ReceiptID, string UserID, string ChangeBy = "")
    {
      return await Task.Run(async () =>
      {
        List<GiftType> aGifts = new List<GiftType>();
        List<GuardaPromocionForzadaRequest> aPromotions = new List<GuardaPromocionForzadaRequest>();
        GuardaPromocionForzadaRequest Request = null;
        GuardaPromocionForzadaResponse Response = null;
        string GiftsSaved = string.Empty;
        string GiftsNoSaved = string.Empty;

        // obtenemos los regalos de promociones de Sistur
        aPromotions = await GetGiftsPromotionsSistur(ReceiptID, ChangeBy, UserID, aGifts);

        // si algun regalo tiene promociones de Sistur
        if (aPromotions != null)
        {
          if (aPromotions.Count > 0)
          {
            for (int i = 0; i <= aPromotions.Count - 1; i++)
            {
              Request = aPromotions[i];

              // Invocamos al servicio web para insertar cada regalo que se pretende asignar
              Response = await GuardarPromocionForzada(Request);

              // si se guardo exitosamente en Sistur
              if (!Response.hasErrors)
              {
                // actualizamos los regalos para identificarlos como que se guardaron en Sistur
                BRGiftsReceiptDetail.UpdateGiftPromotionSistur(aGifts[i].Receipt, aGifts[i].ID, aGifts[i].Promotion);
                GiftsSaved += aGifts[i].Descripcion + "\r\n";
              }
              else
                GiftsNoSaved += aGifts[i].Descripcion + "\r\n";
            }
            // si se pudo guardar todas las promociones
            if (string.IsNullOrEmpty(GiftsNoSaved)) { return "Gifts were successfully saved in Sistur as Promotion"; }
            // si no se pudo guardar ninguna promocion
            else if (string.IsNullOrEmpty(GiftsSaved)) { return "None gift was saved in Sistur as Promotion"; }
            // si no se pudo guardar alguna promocion
            else { return "The following gifts were saved in Sistur as Promotion:\r\n" + GiftsSaved + "\r\n\r\n" + "But the following gifts were not saved: \r\n" + GiftsNoSaved; }
          }
        }
        return string.Empty;
      }
      );
    }
    #endregion

    #region GuardarPromocionForzada
    /// <summary>
    /// Guarda una promocion
    /// </summary>
    /// <param name="Request"></param>
    /// <param name="Error"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    private async static Task<GuardaPromocionForzadaResponse> GuardarPromocionForzada(GuardaPromocionForzadaRequest Request)
    {
      return await Task.Run(() =>
      {
        GuardaPromocionForzadaResponse Response = new GuardaPromocionForzadaResponse();

        // invocamos al servicio web
        Response = Current().GuardarPromocionForzada(Request);
        // Si ocurrio un error
        if (Response.hasErrors)
        {
          throw new Exception(Response.errorInfo);
        }

        return Response;
      });
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
    /// [vipacheco] 25/Julio/2016 Modified --> Se agreggo asyncronia
    /// </history>
    public async static Task<List<GuardaPromocionForzadaRequest>> GetGiftsPromotionsSistur(int ReceiptID, string ChangeBy, string UserID, List<GiftType> aGifts)
    {
      List<GuardaPromocionForzadaRequest> aPromotions = new List<GuardaPromocionForzadaRequest>();
      string Hotel = string.Empty;
      string Folio = string.Empty;
      //int Company = 0;
      string Application = string.Empty;
      string MembershipType = string.Empty;

      // obtenemos las promociones de Sistur que no se han dado
      List<GiftsReceiptDetailPromotionsSistur> Result = await BRGiftsReceiptDetail.GetGiftsReceiptDetailPromotionsPVP(ReceiptID);

      // por default se van a guardar al sistema de Promociones de Sistur
      EnumPromotionsSystem enumPromotionsSystem = EnumPromotionsSystem.Sistur;

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
        if ((Detail.lspg.Equals("IH") && Detail.guHReservID == null) || Detail.lspg.Equals("OUT"))
        {
          // guardamos la reservacion ficticia
          WirePRHelper.Origos_reservas_ficticias_Guardar(Hotel, Convert.ToInt32(Folio), Detail.guFirstName1, Detail.guLastName1);
        }
      }

      for (int i = 0; i < Result.Count; i++)
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
    /// [vipacheco] 23/Agosto/2016 Modified -> se cambio el tipo del parametro Promotions
    /// </history>
    public static void AddPromotion(ref List<GuardaPromocionForzadaRequest> Promotions, string Hotel, string Folio, string Promotion, string GiftID, int ReceiptID, string User, EnumPromotionsSystem enumPromotionsSystem)
    {
      GuardaPromocionForzadaRequest request = new GuardaPromocionForzadaRequest()
      {
        hotel = Hotel,
        folio = Folio,
        adicional = (enumPromotionsSystem == EnumPromotionsSystem.PVP) ? "1" : "0",
        idpromo = Promotion,
        GiftID = GiftID,
        GiftsReceiptID = $"{ReceiptID}",
        usralta = User,
        fechaalta = BRHelpers.GetServerDate(),
        idtipoasignacion = "CONT",
        status = "A",
        tipomov = "",
        usrmodif = ""
      };

      Promotions.Add(request);
    }
    #endregion

    #region getPromotionsType
    /// <summary>
    /// Obtiene el catalogo de Promociones tipo
    /// </summary>
    /// <history>
    /// [emoguel] created 04/07/2016
    /// </history>
    public async static Task<List<PromocionesTipoResponse>> getPromotionsType(string tipoPromo, string status)
    {
      return await Task.Run(() =>
      {
        PromocionesTipoRequest request = new PromocionesTipoRequest();
        PromocionesTipoResponse[] response = null;
        PromocionesTipoResponse Promotion = null;
        request.tipoPromo = tipoPromo;
        request.estatus = status;

        response = Current().ObtenerPromocionesTipo(request);

        if (response != null)
        {
          Promotion = response[0];
          if (Promotion.hasErrors)
          {
            throw new Exception(Promotion.errorInfo);
          }
        }
        return response.OrderBy(ptr => ptr.nombre).ToList();
      });
    }
    #endregion

  }
}

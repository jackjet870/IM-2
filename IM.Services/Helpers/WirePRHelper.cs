using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Services.WirePRService;
using PalaceResorts.Common.PalaceTools;
using System;
using System.Collections.Generic;
using System.Windows;

namespace IM.Services.Helpers
{
  /// <summary>
  /// Clase para el manejo del cliente que consume el servicio web de Wire PR
  /// </summary>
  /// <history>
  /// [jorcanche] 31/03/2016 Created 
  /// </history>
  public class WirePRHelper
  {  
    #region Atributos

    private static OrigosServiceInterface _service;

    #endregion

    #region Propiedades
    /// <summary>
    /// Recupera una instacia Singleton del servicio web
    /// </summary>
    /// <history>
    /// [jorcanche] 31/03/2016
    /// </history>
    public static OrigosServiceInterface Current
    {
      get
      {
        if (_service == null)
        {
          //creamos una instancia del servicio
          _service = new OrigosServiceInterface();
          _service.Url = ConfigHelper.GetString("WirePR.URL");
          return _service;
        }
        else
        {
          return _service;
        }
      }
    }
    #endregion

    #region Metodos

    #region HandlerError
    /// <summary>
    /// HandlerError
    /// </summary>
    /// <param name="result"></param>
    /// <param name="method"></param>
    /// <returns>string strError</returns>
    /// <history>
    /// [jorcanche] 31/03/2016
    /// </history>
    private string HandlerError(ResultStatus result, string method)
    {
      string errorMessage = "";
      Text Text;    

      //si ocurrio un error
      if (result.resultStatusFlag == ResultStatusFlag.FAIL)
      {
        var Texts = result.Text;
        //si tiene mensaje
        if (Texts != null)
        {
          for (int i = 0; i == Texts.Length; i++)
          {
            Text = Texts[1];
            errorMessage = errorMessage + Text.Value;
          }
        }
        else
        {
          errorMessage = "Error";
        }
      }
      return errorMessage;
    }

    #endregion

    #region GetID
    /// <summary>
    /// Obtiene el id de un response
    /// </summary>
    /// <param name="result"></param>
    /// <returns>string strID </returns>
    /// <history>
    /// [jorcanche] 31/03/2016
    /// </history>
    private string GetID(ResultStatus result)
    {
      string strID = string.Empty;

      //si el resultado es exitosos
      if (result.resultStatusFlag == ResultStatusFlag.SUCCESS)
      {
        var IDs = result.IDs;
        if (IDs == null)
        {
          IDPair ID = IDs[0];
          strID = ID.operaId.ToString();
        }
      }
      return strID;
    }
    #endregion

    #region GetRptReservationOrigos
    /// <summary>
    /// Obtiene el reporte de una reservacion para Origos
    /// </summary>
    /// <param name="hotel">Clave del Hotel</param>
    /// <param name="folio">Folio de la reservacion</param>
    /// <history>
    /// [jorcanche] 31/03/2016  Created
    /// </history>
    public static RptReservationOrigos GetRptReservationOrigos(string hotel, string folio)
    {
      RptReservationOrigosRequest request = new RptReservationOrigosRequest();
      RptReservationOrigosResponse response = null;
      RptReservationOrigos report = null;

      //configuramos el Request
      request.RptReservationOrigos = new RptReservationOrigos();
      request.RptReservationOrigos.Hotel = hotel;
      request.RptReservationOrigos.Folio = folio;

      //invocamos al servicio web
      response = Current.GetRptReservationOrigos(request);

      //Si ocurrio un error 
      if (response.HasErrors)
      {
        UIHelper.ShowMessage(response.ExceptionInfo.Message, MessageBoxImage.Error, "GetRptReservationOrigos");
      }
      else
      {
        var Data = response.Data;
        if (Data.Length > 0)
        {
          report = Data[0];
        }
      }
      return report;
    }
    #endregion

    #region Origos_reservas_ficticias_Guardar
    /// <summary>
    /// Guarda una reservacion ficticia en Opera para clientes Outhouse
    /// </summary>
    /// <param name="hotel"></param>
    /// <param name="folio"></param>
    /// <param name="FirstName"></param>
    /// <param name="LastName"></param>
    /// <param name="Company"></param>
    /// <param name="Application"></param>
    /// <param name="MembershipType"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    public static long Origos_reservas_ficticias_Guardar(string hotel, int folio, string FirstName, string LastName, string Company, string Application, string MembershipType)
    {
      origos_reservas_ficticiasRequest Request = new origos_reservas_ficticiasRequest();
      IntegerResponse Response = null;
      long Data = 0;

      // Configuramos el Request
      Request.data.hotel = hotel;
      Request.data.folio = folio;
      Request.data.FirstName = FirstName;
      Request.data.LastName = LastName;
      Request.data.company = Company;
      Request.data.application = Application;
      Request.data.program = MembershipType;

      // Invocamos al servicio web
      Response = Current.Origos_reservas_ficticias_Guardar(Request);

      // Si ocurrio un error
      if (Response.HasErrors)
        UIHelper.ShowMessage(Response.ExceptionInfo.Message, MessageBoxImage.Error, "Origos_reservas_ficticias_Guardar");

      // recuperamos el ID que se inserto
      Data = Response.Numero;

      return Data;
    }
    #endregion

    #region SaveRoomChargesOpera
    /// <summary>
    /// Guarda en el sistema de Opera los cargos a habitacion
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <history>
    /// [vipacheco] 01/Junio/2016 Created
    /// </history>
    public static void SaveRoomChargesOpera(int ReceiptID, string ChargeTo)
    {
      CargoHabitacionTRNRequest Request = new CargoHabitacionTRNRequest();
      RmmsttrnRowResponse Response = null;
      CargoHabitacionRequest[] aRoomCharges = new CargoHabitacionRequest[] { };
      List<GiftType> aGifts = new List<GiftType>();
      bool Error = false;

      // obtenemos los regalos con cargo a habitacion
      aRoomCharges = GetGiftsRoomCharges(ReceiptID, ref aGifts, ref Request, ChargeTo);

      // Si algun regalo tiene cargo a habitacion
      if (aRoomCharges.Length > 0)
      {
        // invocamos al servicio web para insertar cada regalo que se pretende asignar
        Response = InsertarCargoHabitacionTRN(Request, ref Error);

        // si se guardo exitosamente en Opera
        if (!Error)
        {
          // actualizamos el consecutivo de cargos a habitacion
          BRRoomCharges.UpdateRoomChargeConsecutive(Request.Hotel, $"{Request.Folio}");

          // actualizamos los regalos para identificarlos como que se guardaron en Opera
          foreach (GiftType _Gift in aGifts)
          {
            BRGiftsReceiptDetail.UpdateGiftsReceiptDetailRoomChargeOpera(_Gift.Receipt, _Gift.ID, _Gift.TransactionTypeOpera);
          }
          UIHelper.ShowMessage("Gifts were successfully saved in Opera as Room Charges", MessageBoxImage.Information);
        }
        else
        {
          UIHelper.ShowMessage("None gift was saved in Opera as Room Charge", MessageBoxImage.Error);
        }
      }

    }
    #endregion

    #region InsertarCargoHabitacionTRN
    /// <summary>
    /// Agrega un cargo a habitacion
    /// </summary>
    /// <param name="Request"></param>
    /// <param name="Error"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 01/Junio/2016 Created
    /// </history>
    public static RmmsttrnRowResponse InsertarCargoHabitacionTRN(CargoHabitacionTRNRequest Request, ref bool Error)
    {
      RmmsttrnRowResponse Response = null;
      // invocamos al servicio web
      Response = Current.InsertarCargoHabitacionTRN(Request);

      // Si ocurrio un error
      if (Response.HasErrors)
      {
        UIHelper.ShowMessage(Response.ExceptionInfo.Message, MessageBoxImage.Error, "InsertarCargoHabitacionTRN");
        Error = true;
      }

      return Response;
    } 
    #endregion

    #region GetGiftsRoomCharges
    /// <summary>
    /// Obtiene los regalos de cargos a habitacion
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <param name="aGifts"></param>
    /// <param name="Request"></param>
    /// <param name="ChargeTo"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 01/Junio/2016 Created
    /// </history>
    public static CargoHabitacionRequest[] GetGiftsRoomCharges(int ReceiptID, ref List<GiftType> aGifts, ref CargoHabitacionTRNRequest Request, string ChargeTo)
    {
      CargoHabitacionRequest[] aRoomCharges = new CargoHabitacionRequest[] { };

      // obtenemos los cargos a habitacion de Opera que no se han dado
      List<GiftsReceiptDetailRoomChargesOpera> lstResult = BRGiftsReceiptDetail.GetGiftsReceiptDetailRoomChargesOpera(ReceiptID);

      if (lstResult.Count > 0)
      {
        GiftsReceiptDetailRoomChargesOpera First = lstResult[0];

        // Configuramos el Request
        Request.Hotel = First.gulsOriginal;
        Request.Folio = Convert.ToInt32(First.guHReservID);
        Request.PostDate = BRHelpers.GetServerDate();
        Request.Batch = First.rhConsecutive != null ? $"{First.rhConsecutive}" : "";

        foreach (GiftsReceiptDetailRoomChargesOpera Current in lstResult)
        {
          // agregamos el cargo a habitacion
          AddRoomCharge(ref aRoomCharges, Current.gulsOriginal, Convert.ToInt32(Current.guHReservID), ChargeTo, ReceiptID, Current.giN, Current.gePriceA, Current.giOperaTransactionType);

          // preparamos los regalos con cargos a habitacion que se guardaran en Origos
          GiftType _Gift = new GiftType();
          _Gift.ID = Current.gegi;
          _Gift.Descripcion = Current.giN;
          _Gift.Quantity = 0;
          _Gift.Receipt = ReceiptID;
          _Gift.Promotion = "";
          _Gift.TransactionTypeOpera = Current.giOperaTransactionType;

          // Agregamos a la lista
          aGifts.Add(_Gift);
        }

        Request.CargosHabitacion = aRoomCharges;
      }

      return aRoomCharges;
    } 
    #endregion

    #region AddRoomCharge
    /// <summary>
    /// Agrega un cargo a habitacion
    /// </summary>
    /// <param name="RoomCharges"></param>
    /// <param name="Hotel"></param>
    /// <param name="Folio"></param>
    /// <param name="User"></param>
    /// <param name="Receipt"></param>
    /// <param name="Gift"></param>
    /// <param name="Amount"></param>
    /// <param name="TransactionType"></param>
    /// <history>
    /// [vipacheco] 01/Junio/2016 Created
    /// </history>
    public static void AddRoomCharge(ref CargoHabitacionRequest[] RoomCharges, string Hotel, int Folio, string User, int Receipt, string Gift, decimal Amount, string TransactionType)
    {
      int iLength = -1;

      iLength = RoomCharges.Length;
      iLength = iLength + 1;
      Array.Resize(ref RoomCharges, iLength);

      RoomCharges[iLength].Hotel = Hotel;
      RoomCharges[iLength].Folio = Folio;
      RoomCharges[iLength].Type = TransactionType;
      RoomCharges[iLength].Cashier = User;
      RoomCharges[iLength].Ent_oper = User;
      RoomCharges[iLength].Note = "Gifts Receipt " + Receipt + ". Gift '" + Gift + "'. Room charge from Origos.";
      RoomCharges[iLength].Currency = "USD";
      RoomCharges[iLength].Auth = "0";
      RoomCharges[iLength].Loc = "Origos";
      RoomCharges[iLength].Inv_chk = "0";
      RoomCharges[iLength].Bill_to = "F";
      RoomCharges[iLength].PostDate = BRHelpers.GetServerDate();
      RoomCharges[iLength].Entry_amt = Amount;
    } 
    #endregion

    #endregion
  }
}
                                  
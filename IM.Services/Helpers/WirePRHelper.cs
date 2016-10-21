using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Services.WirePRService;
using PalaceResorts.Common.PalaceTools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
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

    #region GetRptReservationOrigos
    /// <summary>
    /// Obtiene el reporte de una reservacion para Origos
    /// </summary>
    /// <param name="hotel">Clave del Hotel</param>
    /// <param name="folio">Folio de la reservacion</param>
    /// <history>
    /// [jorcanche] 31/03/2016  Created
    /// </history>
    public static async Task<RptReservationOrigos> GetRptReservationOrigos(string hotel, string folio)
    {
      return await Task.Run(() =>
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
          throw new Exception(response.ExceptionInfo.Message);

        var Data = response.Data;
        if (Data.Length > 0)
        {
          report = Data[0];
        }

        return report;
      });
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
    public static long Origos_reservas_ficticias_Guardar(string hotel, int folio, string FirstName, string LastName)
    {
      IntegerResponse Response = null;

      // Configuramos el Request
      origos_reservas_ficticiasRequest Request = new origos_reservas_ficticiasRequest()
      {
        data = new origos_reservas_ficticias
        {
          hotel = hotel,
          folio = folio,
          FirstName = FirstName,
          LastName = LastName
        }
      };

      // Invocamos al servicio web
      Response = Current.Origos_reservas_ficticias_Guardar(Request);

      // Si ocurrio un error
      if (Response.HasErrors)
        MessageBox.Show(Response.ExceptionInfo.Message, "Origos_reservas_ficticias_Guardar");


      return Response.Numero;
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
    public static string SaveRoomChargesOpera(int ReceiptID, string ChargeTo)
    {
      CargoHabitacionTRNRequest Request = new CargoHabitacionTRNRequest();
      RmmsttrnRowResponse Response = null;
      List<CargoHabitacionRequest> aRoomCharges = new List<CargoHabitacionRequest>();
      List<GiftType> aGifts = new List<GiftType>();

      // obtenemos los regalos con cargo a habitacion
      aRoomCharges = GetGiftsRoomCharges(ReceiptID, ref aGifts, ref Request, ChargeTo);

      // Si algun regalo tiene cargo a habitacion
      if (aRoomCharges.Count > 0)
      {
        // invocamos al servicio web para insertar cada regalo que se pretende asignar
        Response = InsertarCargoHabitacionTRN(Request);

        // actualizamos el consecutivo de cargos a habitacion
        BRRoomCharges.UpdateRoomChargeConsecutive(Request.Hotel, $"{Request.Folio}");

        // actualizamos los regalos para identificarlos como que se guardaron en Opera
        foreach (GiftType _Gift in aGifts)
        {
          BRGiftsReceiptDetail.UpdateGiftsReceiptDetailRoomChargeOpera(_Gift.Receipt, _Gift.ID, _Gift.TransactionTypeOpera);
        }
        return "Gifts were successfully saved in Opera as Room Charges";
      }
      return string.Empty;
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
    public static RmmsttrnRowResponse InsertarCargoHabitacionTRN(CargoHabitacionTRNRequest Request)
    {

      RmmsttrnRowResponse Response = null;
      // invocamos al servicio web
      Response = Current.InsertarCargoHabitacionTRN(Request);

      // Si ocurrio un error
      if (Response.HasErrors)
      {
        throw new Exception(Response.ExceptionInfo.Message);
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
    public static List<CargoHabitacionRequest> GetGiftsRoomCharges(int ReceiptID, ref List<GiftType> aGifts, ref CargoHabitacionTRNRequest Request, string ChargeTo)
    {
      List<CargoHabitacionRequest> aRoomCharges = new List<CargoHabitacionRequest>();

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

        Request.CargosHabitacion = aRoomCharges.ToArray();
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
    public static void AddRoomCharge(ref List<CargoHabitacionRequest> RoomCharges, string Hotel, int Folio, string User, int Receipt, string Gift, decimal Amount, string TransactionType)
    {
      CargoHabitacionRequest newItem = new CargoHabitacionRequest()
      {
        Hotel = Hotel,
        Folio = Folio,
        Type = TransactionType,
        Cashier = User,
        Ent_oper = User,
        Note = "Gifts Receipt " + Receipt + ". Gift '" + Gift + "'. Room charge from Origos.",
        Currency = "USD",
        Auth = "0",
        Loc = "Origos",
        Inv_chk = "0",
        Bill_to = "F",
        PostDate = BRHelpers.GetServerDate(),
        Entry_amt = Amount
      };
      RoomCharges.Add(newItem);
    }
    #endregion

    #region GetTransactionTypes
    /// <summary>
    /// Obtiene las transacciones de opera
    /// </summary>
    /// <history>
    /// [emoguel] created 04/07/2016
    /// </history>
    public async static Task<List<TransactionTypes>> GetTransactionTypes()
    {

      return await Task.Run(() =>
      {
        TransactionTypesRequest request = new TransactionTypesRequest();
        TransactionTypesResponse response = null;

        #region Request              
        request.TransactionTypes = new TransactionTypes();
        request.TransactionTypes.Tc_Group = ConfigHelper.GetString("Opera.TransactionTypesGroups");
        request.TransactionTypes.Tc_Subgroup = ConfigHelper.GetString("Opera.TransactionTypesSubgroups");
        #endregion
        response = Current.GetTransactionTypes(request);
        //si ocurrio un error
        if (response.HasErrors)
          throw new Exception(response.ExceptionInfo.Message);

        return response.Data.OrderBy(t => t.Description).ToList();
      });
    }
    #endregion

    #region GetReservationsOrigos
    /// <summary>
    /// Obtiene reservaciones dado un conjunto de criterios
    /// </summary>
    /// <param name="hotel">clave del hotel</param>
    /// <param name="folio">folio de la reseravacion</param>
    /// <param name="room">Numero de room</param>
    /// <param name="name">Nombre del huesped</param>
    /// <param name="dtpStart">Fecha inicial</param>
    /// <param name="dtpEnd">Fecha final</param>
    /// <returns> Array de ReservationOrigos</returns>
    /// <history>
    /// [vipacheco] 17/Agosto/2016 Created
    /// </history>
    public static async Task<ReservationOrigos[]> GetReservationsOrigos(string hotel, string folio, string room, string name, DateTime dtpStart, DateTime dtpEnd)
    {
      return await Task.Run(() =>
      {
        QueryRequest request = new QueryRequest();
        ReservationOrigosResponse response = null;

        // configuramos el request
        request.Query = new Query()
        {
          Hotel = hotel,
          Folio = folio,
          Room = room,
          Name = name,
          DateFrom = dtpStart,
          DateTo = dtpEnd
        };

        // Invocamos al servicio web
        response = Current.GetReservationsOrigos(request);
        if (response.HasErrors)
          throw new Exception(response.ExceptionInfo.Message);

        return response.Data;

      });
    }
    #endregion

    #endregion
  }
}

using IM.Base.Helpers;
using IM.Services.WirePRService;
using PalaceResorts.Common.PalaceTools;
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

    #endregion
  }
}

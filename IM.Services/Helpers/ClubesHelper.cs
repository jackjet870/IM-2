using PalaceResorts.Common.PalaceTools;
using IM.Services.ClubesService;
using IM.Model.Enums;
using IM.Base.Helpers;
using System.Windows;

namespace IM.Services.Helpers
{
  /// <summary>
  /// Clase para el manejo de reportes de Equity desde un servicio web
  /// </summary>
  /// <history>
  /// [ecanul] 05/Abr/2016 Created
  /// </history>
  public class ClubesHelper
  {
    #region Atributos

    private static OrigosServiceInterface _serviceElite;
    private static OrigosServiceInterface _servicePremier;

    #endregion

    #region Propiedades

    /// <summary>
    /// Recupera una instacia Singleton del servicio web Clubes
    /// </summary>
    /// <history>[ECANUL] 05-04-2016 Created</history>
    public static OrigosServiceInterface Current(EnumClub club = EnumClub.PalaceElite)
    {
      // si es Palace Elite
      if (club == EnumClub.PalaceElite)
      {
        if (_serviceElite == null)
        {
          // creamos una instancia del servicio
          _serviceElite = new OrigosServiceInterface();
          _serviceElite.Url = ConfigHelper.GetString("ClubesElite.URL");

          // insertamos las cabeceras de autenticacion
          RequestHeader requestHeader = new RequestHeader();
          requestHeader.Headers = ServiceHelper.GetSecurityTokenHeaders();
          _serviceElite.RequestHeaderValue = requestHeader;
        }
        return _serviceElite;
      }
      // si es Palace Premier
      else
      {
        if (_servicePremier == null)
        {
          // creamos una instancia del servicio
          _servicePremier = new OrigosServiceInterface();
          _servicePremier.Url = ConfigHelper.GetString("ClubesPremier.URL");

          // insertamos las cabeceras de autenticacion
          RequestHeader requestHeader = new RequestHeader();
          requestHeader.Headers = ServiceHelper.GetSecurityTokenHeaders();
          _servicePremier.RequestHeaderValue = requestHeader;
        }
        return _servicePremier;
      }
    }

    #endregion

    #region Metodos

    #region GetRptEquity

    /// <summary>
    /// Obtiene el reporte de Equity
    /// </summary>
    /// <param name="membershipNum">Numero de membrecia</param>
    /// <param name="company">Compania del guest</param>
    /// <param name="club">Club</param>
    /// <history>
    /// [ecanul] 05/04/2016 Created
    /// </history>
    public static RptEquity GetRptEquity(string membershipNum, int company, EnumClub club)
    {
      QueryRequest request = new QueryRequest();
      RptEquityResponse response = null;
      RptEquity report = null;

      //Se configura el request
      request.InfoQuery = new InfoQuery();
      request.InfoQuery.Application = membershipNum;
      request.InfoQuery.Compania = company.ToString();

      //se invoca al servicio
      response = Current(club).GetRptEquity(request);

      //si ocurrio un error
      if (response.HasErrors)
        UIHelper.ShowMessage(response.ExceptionInfo.Message, MessageBoxImage.Error, "GetRptEquity");
      else
        report = response.Data;

      return report;
    }

    #endregion

    #endregion
  }
}

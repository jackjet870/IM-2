using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalaceResorts.Common.PalaceTools;
using IM.Services.CallCenterService;
using IM.Model.Enums;
using IM.Base.Helpers;
using System.Windows;

namespace IM.Services.Helpers
{
  public class CallCenterHelper
  {
    #region Atributos

    private static CallCenterServiceInterface _serviceElite;
    private static CallCenterServiceInterface _servicePremier;

    #endregion

    #region Propiedades

    /// <summary>
    /// Recupera una instacia Singleton del servicio web  Call Center
    /// </summary>
    /// <param name="club">Enumerado del tipo de club a recuperar</param>
    /// <history>
    /// [ecanul] 07/04/2016 Created
    /// </history>
    public static CallCenterServiceInterface Current(EnumClub club = EnumClub.PalaceElite)
    {
      // si es Palace Elite
      if (club == EnumClub.PalaceElite)
      {
        if (_serviceElite == null)
        {
          // creamos nueva instancia del servicio 
          _serviceElite = new CallCenterServiceInterface();
          _serviceElite.Url = ConfigHelper.GetString("CallCenterElite.URL");

          // establecemos el token de autenticacion
          RequestHeader requestHeader = new RequestHeader();
          requestHeader.Headers = ServiceHelper.GetSecurityTokenHeaders();
          _serviceElite.RequestHeaderValue = requestHeader;
        }
        return _serviceElite;
      }
      // si es palace Premier
      else
      {
        if(_servicePremier == null)
        {
          //Creamos una instancia del servicio 
          _servicePremier = new CallCenterServiceInterface();
          _servicePremier.Url = ConfigHelper.GetString("CallCenterPremier.URL");

          //Insertamos las cabeceras de autenticacion
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
    /// Obtiene el reporte de equity
    /// </summary>
    /// <param name="membershipNum">Numero de membrecia</param>
    /// <param name="company">Compania del guest</param>
    /// <param name="club">Club</param>
    /// <history>
    /// [ecanul] 07/04/207
    /// </history>
    public static RptEquity GetRptEquity(string membershipNum, int company, EnumClub club)
    {
      MemberRequest request = new MemberRequest();
      RptEquityResponse response = null;
      RptEquity report = null;

      //configuramos el request
      request.Member = new Member();
      request.Member.Application = membershipNum;
      request.Member.Company = company;

      // invocamos el servicio web
      response = Current(club).GetRptEquity(request);

      // Si ocurrio un error
      if (response.HasErrors)
        UIHelper.ShowMessage(response.ExceptionInfo.Message, MessageBoxImage.Error, "GetRptEquity");

      var data = response.Data;
      if (data.Length > -1)
        report = data[0];
      return report;
    }

    #endregion

    #endregion

  }
}

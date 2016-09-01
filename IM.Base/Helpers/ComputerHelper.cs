using System;
using System.Net;

namespace IM.Base.Helpers
{
  /// <summary>
  /// Clase para informacion sobre la red
  /// </summary>
  /// <history>
  /// [jorcanche] created 16/07/2016
  /// </history>
  public class ComputerHelper
  {
    #region GetIpMachine
    /// <summary>
    /// Obtiene la ip de la Maquina en la que esta actualmnente el usuario
    /// </summary>
    /// <history>
    ///[jorcanche]  created 05032016
    ///[emoguel] 01/09/2016 modified--->Ahora devuelve la ip del cliente
    /// </history>
    public static string GetIpMachine()
    {
      var ipMachine = string.Empty;
      if (hasSessionCitrix())//Verificar si está abierto desde Citrix
      {
        var ipAddress = Dns.GetHostAddresses(Environment.ExpandEnvironmentVariables("%CLIENTNAME%"));
        if(ipAddress.Length>0)
        {
          ipMachine = ipAddress[0].ToString();
        }
      }
      else//devolver la Ip Local
      {        
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
          if (ip.AddressFamily.ToString() == "InterNetwork")
          {
            ipMachine = ip.ToString();
          }
        }        
      }
      return ipMachine;
    } 
    #endregion

    #region GetMachineName
    /// <summary>
    /// Devuelve el nombre de la maquina en donde se está ejecutando la Aplicación
    /// </summary>
    /// <returns>Nombre de la Maquina</returns>
    /// <history>
    /// [erodriguez] 18/08/2016  created
    /// [emoguel[ 01/09/2016 modified
    /// </history>
    public static string GetMachineName()
    {
      if (hasSessionCitrix())//Verificar si está abierto desde Citrix
      {        
        return Environment.ExpandEnvironmentVariables("%CLIENTNAME%");
      }
      else//Devolver el nomber de la maquina local
      {
        return Environment.MachineName;
      }
    } 
    #endregion

    #region hasSessionCitrix
    /// <summary>
    /// Verifica si la aplicación se está ejecutando desde Citrix
    /// </summary>
    /// <returns>
    /// True. Se está ejecutando desde Citrix
    /// False. Nose está ejecutando desde Citrix
    /// </returns>
    /// <history>
    /// [emoguel] 01/09/2016 created
    /// </history>
    public static bool hasSessionCitrix()
    {
      string sessionName = (Environment.GetEnvironmentVariable("SessionName") ?? "").ToUpper();
      return sessionName.StartsWith("ICA");
    } 
    #endregion
  }
}

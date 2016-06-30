﻿using System.Net;

namespace IM.Base.Helpers
{
  public class ComputerHelper
  {
    /// <summary>
    /// Obtiene la ip de la Maquina en la que esta actualmnente el usuario
    /// </summary>
    /// <history>
    ///[jorcanche]  created 05032016
    /// </history>
    public static string GetIpMachine()
    {
      var localIp = string.Empty;
      var host = Dns.GetHostEntry(Dns.GetHostName());
      foreach (IPAddress ip in host.AddressList)
      {
        if (ip.AddressFamily.ToString() == "InterNetwork")
        {
          localIp = ip.ToString();
        }
      }
      return localIp;
    }
  }
}

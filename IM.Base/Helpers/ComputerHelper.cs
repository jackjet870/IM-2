using System.Net;

namespace IM.Base.Helpers
{
  public class ComputerHelper
  {
    public static string GetIPMachine()
    {
      IPHostEntry host;
      string localIP = "";
      host = Dns.GetHostEntry(Dns.GetHostName());
      foreach (IPAddress ip in host.AddressList)
      {
        if (ip.AddressFamily.ToString() == "InterNetwork")
        {
          localIP = ip.ToString();
        }
      }
      return localIP;
    }
  }
}

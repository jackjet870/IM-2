using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Base.Helpers
{
  public static class StringHelper
  {
    static public List<string> Items { get; set; }

    #region PadCenter
    /// <summary>
    /// Extension para centrar un texto.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="length"></param>
    /// <returns>String</returns>
    /// <history>
    ///   [edgrodriguez] 17/Jul/2016 Created
    /// </history>
    public static string PadCenter(this string str, int length = 40)
    {
      int spaces = length - str.Length;
      int padLeft = spaces / 2 + str.Length;
      return str.PadLeft(padLeft).PadRight(length);
    } 
    #endregion
    #region ToTicketString
    /// <summary>
    /// Obtiene la cadena de texto a partir de un format de Ticket.
    /// </summary>
    /// <returns>string</returns>
    /// <history>
    ///   [edgrodriguez] 17/Jul/2016 Created
    /// </history>
    public static string ToTicketString(int iLimit = 40)
    {
      string strTicket = "";
      foreach (var str in Items)
      {
        if (str.Length <= iLimit)
          strTicket += str + "\r\n";
        else if (str.Length > iLimit)
        {
          var firstSpace = str.ToCharArray()
                .Select((v, i) => new { value = v, index = i })
                .Where(element => element.value == ' ')
                .LastOrDefault(element => element.index <= iLimit).index;
          strTicket += str.Substring(0, firstSpace) + "\r\n";
          strTicket += str.Substring(firstSpace) + "\r\n";
        }
      }

      Items.Clear();
      return strTicket;
    } 
    #endregion
  }
}
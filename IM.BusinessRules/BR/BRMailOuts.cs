using IM.Model;
using System;

namespace IM.BusinessRules.BR
{
  public class BRMailOuts
  {
    /// <summary>
    /// Procesa los MailOuts por la  Clave del Lead Source
    /// </summary>
    /// <param name="leadSourceID">Clave del Lead Source</param>
    /// <param name="date"> Opcional:Fecha </param>
    /// <history>
    ///   [aalcocer] 24/02/2016 Created
    /// </history>
    public static void ProcessMailOuts(string leadSourceID, DateTime? date = null)
    {
      using (var cn = new IMEntities())
      {
        cn.spProcessMailOuts(leadSourceID, date);
      }
    }
  }
}
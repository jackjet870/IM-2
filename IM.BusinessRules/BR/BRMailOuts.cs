using System;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRMailOuts
  {
    #region ProcessMailOuts

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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.spProcessMailOuts(leadSourceID, date);
      }
    }

    #endregion
  }
}
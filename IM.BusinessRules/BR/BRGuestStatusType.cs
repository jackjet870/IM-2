using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRGuestStatusType
  {
    #region GetGuestStatusTypeById
    /// <summary>
    /// Obtiene un estado del cliente por medio de su identificador
    /// </summary>
    /// <param name="guestStatusTypeId">Identificador del estado del invitado</param>
    /// <returns></returns>
    /// <history>
    /// [lchairez] 30/03/2016 Created.
    /// </history>
    public  static GuestStatusType GetGuestStatusTypeById(string guestStatusTypeId)
    {
      using (var dbContect = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContect.GuestsStatusTypes.SingleOrDefault(g=> g.gsID == guestStatusTypeId);
      }
    }
    #endregion
  }
}

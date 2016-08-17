using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRGuestsGroupsIntegrants
  {
    /// <summary>
    /// Obtiene un Guests Group segun el GUID que se envie 
    /// </summary>
    /// <param name="GUID">ID del Guest</param>
    /// <history>
    ///   [ecanul] 04/04/2016 Created
    ///   [ecanul] 15/08/2016 Modified, Modificada asincronia, ahora retorna los datos de manera directa
    /// </history>
    public async static Task<GuestsGroup> GetGuestGroupByGuest(int GUID)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return (from gu in dbContext.Guests
                      from ggi in gu.GuestsGroups
                      join gg in dbContext.GuestsGroups
                      on ggi.gxID equals gg.gxID
                      where gu.guID == GUID
                      select gg).Single();
        }
      });
    }

  }
}

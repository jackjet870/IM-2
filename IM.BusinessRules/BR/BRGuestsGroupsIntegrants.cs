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

    public static GuestsGroup GetGuestGroupByGuest(int GUID)
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
    }

  }
}

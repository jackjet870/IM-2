using IM.Model;
using IM.Model.Classes;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRGuestOutPremanifest
  {
    #region GetNoteGuest
    /// <summary>
    /// Devuelve una Nota o varias dependiendo de cuantas tanga el Guest
    /// </summary>
    /// <param name="guId">Id a Guest</param>
    /// <returns>Un listado de PRNotes</returns>
    /// <history>
    /// [jorcanche] created 22/03/2016
    /// </history>    
    public static List<OutPremanifest> GetGuestOutPremanifest(bool bookinvit, DateTime Date, string LeadSource)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from G in dbContext.Guests
                join Co in dbContext.Countries on G.guco equals Co.coID
                join Ag in dbContext.Agencies on G.guag equals Ag.agID
                where (bookinvit ? G.guBookD : G.guInvitD) == Date
                where G.guls == LeadSource
                orderby G.guBookT, G.guLastName1
                select new OutPremanifest
                {
                  guStatus = G.guStatus,
                  guID = G.guID,
                  guCheckIn = G.guCheckIn,
                  guRoomNum = G.guRoomNum,
                  guLastName1 = G.guLastName1,
                  guFirstName1 = G.guFirstName1,
                  guCheckInD = G.guCheckInD,
                  guCheckOutD = G.guCheckOutD,
                  guco = G.guco,
                  coN = Co.coN,
                  guag = G.guag,
                  agN = Ag.agN,
                  guAvail = G.guAvail,
                  guInfo = G.guInfo,
                  guPRInfo = G.guPRInfo,
                  guInfoD = G.guInfoD,
                  guInvit = G.guInvit,
                  guInvitD = G.guInvitD,
                  guBookD = G.guBookD,
                  guBookT = G.guBookT,
                  guPRInvit1 = G.guPRInvit1,
                  guMembershipNum = G.guMembershipNum,
                  guBookCanc = G.guBookCanc,
                  guShow = G.guShow,
                  guSale = G.guSale,
                  guComments = G.guComments,
                  guPax = G.guPax
                }).ToList();
      }

    }
  }
  #endregion
}

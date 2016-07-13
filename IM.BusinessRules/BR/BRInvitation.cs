using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Classes;
using System.Resources;
using IM.Model.Enums;

namespace IM.BusinessRules.BR
{
  /// <summary>
  /// Clase para el manejo de la informacion de la Invitación
  /// </summary>
  /// <history>
  /// [jorcanche ] created 01/mayo/2016
  /// </history>  
  public class BRInvitation
  {
    /// <summary>
    /// Trae la informacion de una invitación
    /// </summary>
    /// <param name="guestId"> Id del Huesped</param>
    /// <history>
    /// [jorcanche ] created 01/mayo/2016
    /// </history>    
    public static async Task<InvitationData> RptInvitationData(int guestId)
    {
      var invitationData = new InvitationData();
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var resInvitation = dbContext.USP_OR_RptInvitation(guestId);
          invitationData.Invitation = resInvitation.FirstOrDefault();

          var resInviationGuest = resInvitation.GetNextResult<RptInvitation_Guest>();
          invitationData.InvitationGuest = resInviationGuest.ToList();

          var resInvitationDeposit = resInviationGuest.GetNextResult<RptInvitation_Deposit>();
          invitationData.InvitationDeposit = resInvitationDeposit.ToList();

          var resInvitationGift = resInvitationDeposit.GetNextResult<RptInvitation_Gift>();
          invitationData.InvitationGift = resInvitationGift.ToList();

          return invitationData;
        }
      });
    }


    /// <summary>
    /// Trae la informacion genearal la  invitacion por LeadSource y por Lenguaje 
    /// </summary>
    /// <param name="leadsource">Lead Source</param>
    /// <param name="language">Lenguage</param>
    /// <history>
    /// [jorcanche ] created 01/mayo/2016
    /// </history> 
    public static async Task<InvitationText> GetInvitationFooterHeader(string leadsource, string language)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.InvitationsTexts.SingleOrDefault(it => it.itls == leadsource && it.itla == language);
        }
      });
    }
  }
}

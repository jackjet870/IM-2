using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Classes;
using System.Resources;

namespace IM.BusinessRules.BR
{
  public class BRInvitation
  {
    public static InvitationData RptInvitationData(int GuestID, string ChangedBy)
    {
      var InvitationData = new InvitationData();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString) )
      {
        var resInvitation =  dbContext.USP_OR_RptInvitation(GuestID);
        InvitationData.Invitation = resInvitation.FirstOrDefault();
        //InvitationData.Invitation.guResch = true;
 
        var resInviationGuest = resInvitation.GetNextResult<RptInvitation_Guest>();
        InvitationData.InvitationGuest = resInviationGuest.ToList();

        var resInvitationDeposit = resInviationGuest.GetNextResult<RptInvitation_Deposit>();
        InvitationData.InvitationDeposit = resInvitationDeposit.ToList();

        var resInvitationGift = resInvitationDeposit.GetNextResult<RptInvitation_Gift>();
        InvitationData.InvitationGift = resInvitationGift.ToList();

        return InvitationData;
      }
    }

    public static List<object> RptInvitationObj(int GuestID, string ChangedBy)
    {     
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var resInvitation = dbContext.USP_OR_RptInvitation(GuestID);
        RptInvitation Invitation = resInvitation.FirstOrDefault();

        var resInviationGuest = resInvitation.GetNextResult<RptInvitation_Guest>();
        RptInvitation_Guest InvitationGuest = resInviationGuest.FirstOrDefault();

        var resInvitationDeposit = resInviationGuest.GetNextResult<RptInvitation_Deposit>();
        RptInvitation_Deposit InvitationDeposit = resInvitationDeposit.FirstOrDefault();

        var resInvitationGift = resInvitationDeposit.GetNextResult<RptInvitation_Gift>();
        RptInvitation_Gift InvitationGift = resInvitationGift.FirstOrDefault();
       
        List<object> lstObj = new List<object>();
        lstObj.Add(Invitation);
        lstObj.Add(InvitationGuest);
        lstObj.Add(InvitationDeposit);
        lstObj.Add(InvitationGift);

        return lstObj;
      }
    }
    //public static RptInvitation_Guest ValidateInvitationGuest(RptInvitation_Guest invGuest)
    //{
    //  invGuest.LastName = string.IsNullOrEmpty(invGuest.LastName)?"az ":invGuest.LastName;
    //  invGuest.FirstName = string.IsNullOrEmpty(invGuest.FirstName) ? "az " : invGuest.FirstName;
    //  //invGuest.Age = invGuest.Age == 0 ? 1 : invGuest.Age;
    //  invGuest.MaritalStatus = string.IsNullOrEmpty(invGuest.MaritalStatus) ? "az " : invGuest.MaritalStatus;
    //  invGuest.Occupation = string.IsNullOrEmpty(invGuest.Occupation) ? "az " : invGuest.Occupation;
    //  return invGuest;
    //}
   // ResourceManager rm = new ResourceManager("CultureResources.StringResources", typeof(ResourceLanguage).Assembly);
    

  }
}

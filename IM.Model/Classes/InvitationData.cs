using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Classes
{
  /// <summary>
  /// Entidad que trae la información para el reporte de invitacion
  /// </summary>
  /// <history>
  /// [jorcanche] Created 12/04/2016
  /// </history>
  public class InvitationData
  {    
    public RptInvitation Invitation { get; set; }
    public List<RptInvitation_Guest> InvitationGuest { get; set; }
    public List<RptInvitation_Deposit> InvitationDeposit { get; set; }
    public List<RptInvitation_Gift> InvitationGift { get; set; }

  }
}

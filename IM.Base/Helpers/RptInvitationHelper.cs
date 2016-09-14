using IM.Base.Forms;
using IM.Base.Reports;
using IM.BusinessRules.BR;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace IM.Base.Helpers
{
  /// <summary>
  /// Clase que sirve para la invitación
  /// </summary>
  /// <history>
  /// [jorcanche]  created  13/05/2016
  /// </history>
  public class RptInvitationHelper
  {

    #region RPTInvitation
    /// <summary>
    ///Prepara el reporte de invitación para ser visualizado 
    /// </summary>
    /// <history>
    /// [jorcanche] 16/04/2016 created
    /// [jorcanche] 12/05/2016 Se cambio de frmInvitaciona RptinvitationHelper
    /// </history>
    public static async void RptInvitation(int guest = 0, string peID = "USER", Window window = null)
    {
      //Traemos la informacion del store y la almacenamos en un procedimiento
      InvitationData invitationData = await BRInvitation.RptInvitationData(guest);

      //Le damos memoria al reporte de Invitacion
      var rptInvi = new rptInvitation();

      /************************************************************************************************************
                                  Información Adiocional sobre el DataSource del Crystal                          
      *************************************************************************************************************
       Para que el DataSource acepte una entidad primero se debe de converir a lista                              
       1.- ObjectHelper.ObjectToList(invitationData.Invitation)                                                    
       Pero sí al convertirlo hay propiedades nulas, el DataSource no lo aceptara y marcara error; para evitar esto
       se debera convertir a DateTable para que no tenga nulos.
       2.- TableHelper.GetDataTableFromList(ObjectHelper.ObjectToList(invitationData.Invitation))
      *************************************************************************************************************/

      //Le agregamos la informacion 
      rptInvi.SetDataSource(TableHelper.GetDataTableFromList(ObjectHelper.ObjectToList(new IM.Base.Classes.RptInvitationIM(invitationData.Invitation))));
      //Cargamos los subreportes
      rptInvi.Subreports["rptInvitationGuests.rpt"].SetDataSource(TableHelper.GetDataTableFromList(invitationData.InvitationGuest?.Select(c => new IM.Base.Classes.RptInvitationGuestsIM(c)).ToList() ?? new List<Classes.RptInvitationGuestsIM>()));
      rptInvi.Subreports["rptInvitationDeposits.rpt"].SetDataSource(invitationData.InvitationDeposit?.Select(c => new IM.Base.Classes.RptInvitationDepositsIM(c)).ToList() ?? new List<Classes.RptInvitationDepositsIM>());
      rptInvi.Subreports["rptInvitationGifts.rpt"].SetDataSource(TableHelper.GetDataTableFromList(invitationData.InvitationGift?.Select(c => new IM.Base.Classes.RptInvitationGiftsIM(c)).ToList() ?? new List<Classes.RptInvitationGiftsIM>()));

      //Cambiamos el lenguaje de las etiquetas.
      CrystalReportHelper.SetLanguage(rptInvi, invitationData.Invitation.gula);

      //Fecha y hora
      rptInvi.SetParameterValue("lblDateTime", BRHelpers.GetServerDateTime());

      //Cambiado por
      rptInvi.SetParameterValue("lblChangedBy", peID);

      //Cargamos el Viewer
      var frmViewer = new frmViewer(rptInvi) { Owner = window};
      frmViewer.ShowDialog();   
    }
    #endregion

   

  }
}

using IM.Base.Forms;
using IM.Base.Reports;
using IM.BusinessRules.BR;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;

namespace IM.Base.Helpers
{
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
    public static void RPTInvitation(int guest = 0, string peID = "USER")
    {
      //Traemos la informacion del store y la almacenamos en un procedimiento
      InvitationData invitationData = BRInvitation.RptInvitationData(guest);

      //Determinamos el Lenguaje
      LanguageHelper.IDLanguage = invitationData.Invitation.gula;
    
      //Le damos memoria al reporte de Invitacion
      var rptInvi = new rptInvitation();

      /************************************************************************************************************
                                  Información Adiocional sobre el DataSource del Crystal                          
      *************************************************************************************************************
       Para que el DataSource acepte una entidad primero se debe de converir a lista                              
       1.- ObjectHelper.ObjectToList(invitationData.Invitation)                                                    
       Pero sí al convertirlo hay propiedades nulas, el DataSource no lo aceptara y marcara error; para evirar esto
       se debera convertir a DateTable para que no tenga nulos.
       2.- TableHelper.GetDataTableFromList(ObjectHelper.ObjectToList(invitationData.Invitation))
      *************************************************************************************************************/

      //Le agregamos la informacion 
      rptInvi.SetDataSource(TableHelper.GetDataTableFromList(ObjectHelper.ObjectToList(invitationData.Invitation)));
      //Cargamos los subreportes
      rptInvi.Subreports["rptInvitationGuests.rpt"].SetDataSource(TableHelper.GetDataTableFromList(invitationData.InvitationGuest));
      rptInvi.Subreports["rptInvitationDeposits.rpt"].SetDataSource(invitationData.InvitationDeposit);
      rptInvi.Subreports["rptInvitationGifts.rpt"].SetDataSource(TableHelper.GetDataTableFromList(invitationData.InvitationGift));

      //Cargamos las Etiquetas 
      lblInvitation(rptInvi,peID);

      //Cargamos el Viewer
      var _frmViewer = new frmViewer(rptInvi);
      _frmViewer.ShowDialog();
    }
    #endregion

    #region lblInvitation
    /// <summary>
    /// Cargas la etiquetas del Reporte
    /// </summary>
    /// <param name="rptInv">Instancia del reporte Invitacion </param>
    /// <history>
    /// [jorcanche] 16/04/2016 created
    /// [jorcanche] 12/05/2016 Se cambio de frmInvitaciona RptinvitationHelper
    /// </history>
    private static void lblInvitation(rptInvitation rptInv, string _peID)
    {
      //Etiquetas de Depositos
      rptInv.SetParameterValue("lblDeposits", LanguageHelper.GetMessage(EnumMessage.msgLblDeposits));
      rptInv.SetParameterValue("lblDeposit", LanguageHelper.GetMessage(EnumMessage.msgLblDeposit));
      rptInv.SetParameterValue("lblCurrency", LanguageHelper.GetMessage(EnumMessage.msgLblCurrency));
      //Depositos quemados
      rptInv.SetParameterValue("lblDepositBurned", LanguageHelper.GetMessage(EnumMessage.msgLblDepositBurned));
      //************************************************************************************************

      //Etiquetas de Regalos      
      rptInv.SetParameterValue("lblGifts", LanguageHelper.GetMessage(EnumMessage.msgLblGifts));
      rptInv.SetParameterValue("lblQuantityGifts", LanguageHelper.GetMessage(EnumMessage.msgLblQuantityAbbreviation));
      rptInv.SetParameterValue("lblGift", LanguageHelper.GetMessage(EnumMessage.msgLblGift));
      //************************************************************************************************

      //Etiquetas Invitados
      rptInv.SetParameterValue("lblGuests", LanguageHelper.GetMessage(EnumMessage.msgLblGuests));
      rptInv.SetParameterValue("lblLastName", LanguageHelper.GetMessage(EnumMessage.msgLblLastName));
      rptInv.SetParameterValue("lblFirstName", LanguageHelper.GetMessage(EnumMessage.msgLblFirstName));
      rptInv.SetParameterValue("lblAge", LanguageHelper.GetMessage(EnumMessage.msgLblAge));
      rptInv.SetParameterValue("lblMaritalStatus", LanguageHelper.GetMessage(EnumMessage.msgLblMaritalStatus));
      rptInv.SetParameterValue("lblOccupation", LanguageHelper.GetMessage(EnumMessage.msgLblOccupation));
      //************************************************************************************************

      //Numero de membresia
      rptInv.SetParameterValue("lblMembershipNum", LanguageHelper.GetMessage(EnumMessage.msgLblMembershipNum));
      //Fecha
      rptInv.SetParameterValue("lblDate", LanguageHelper.GetMessage(EnumMessage.msgLblDate));
      //Hora
      rptInv.SetParameterValue("lblTime", LanguageHelper.GetMessage(EnumMessage.msgLblTime));
      //Agencia
      rptInv.SetParameterValue("lblAgency", LanguageHelper.GetMessage(EnumMessage.msgLblAgency));
      //Pais
      rptInv.SetParameterValue("lblCountry", LanguageHelper.GetMessage(EnumMessage.msgLblCountry));
      //Hotel
      rptInv.SetParameterValue("lblHotel", LanguageHelper.GetMessage(EnumMessage.msgLblHotel));
      //Numero de habitación
      rptInv.SetParameterValue("lblRoomNum", LanguageHelper.GetMessage(EnumMessage.msgLblRoomNum));
      //Pax
      rptInv.SetParameterValue("lblPax", LanguageHelper.GetMessage(EnumMessage.msgLblPax));
      //Location
      rptInv.SetParameterValue("lblLocation", LanguageHelper.GetMessage(EnumMessage.msgLblLocation));
      //Guest Service
      rptInv.SetParameterValue("lblGuestService", LanguageHelper.GetMessage(EnumMessage.msgLblGuestService));
      //Notas
      rptInv.SetParameterValue("lblNotes", LanguageHelper.GetMessage(EnumMessage.msgLblNotes));
      //Fecha y hora
      rptInv.SetParameterValue("lblDateTime", BRHelpers.GetServerDateTime());
      //Cambiado por
      rptInv.SetParameterValue("lblChangedBy", _peID);
    }

    #endregion

  }
}

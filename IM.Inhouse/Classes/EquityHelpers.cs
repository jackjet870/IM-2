using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.Inhouse.Reports;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Services.CallCenterService;
using IM.Services.Helpers;
using System;
using System.Linq;
using System.Windows;

namespace IM.Inhouse.Classes
{
  public class EquityHelpers
  {
    #region GetIVAByOffice

    /// <summary>
    /// Convierte un dato Membership.Office al IVA correspondiente Correspondiente
    /// </summary>
    /// <param name="office"></param>
    /// <history>
    /// [ecanul] 13/04/2016 Created
    /// </history>
    public static decimal GetIVAByOffice(string office)
    {
      decimal iva = 0;
      switch (office)
      {
        case "7":
          iva = Convert.ToDecimal(1.15);
          break;
        case "12":
          iva = 1;
          break;
        default:
          iva = Convert.ToDecimal(1.1);
          break;
      }
      return iva;
    }

    #endregion

    #region EquityReport

    /// <summary>
    /// Obtiene un reporte de Equity
    /// </summary>
    /// <param name="membershipNum">numero de membresia</param>
    /// <param name="company">compania</param>
    /// <param name="clubAgency">agencia</param>
    /// <param name="clubGuest">club</param>
    /// <history>
    /// [ecanul] 06/04/2016 Created
    /// [ecanul] 07/04/2016 Modificated Agregado Validaciones y "show" del reporte
    /// [ecanul] 20/04/2016 Modificated Metodo Movido de frmInhouse a EquityHelpers
    /// [ecanul] 06/07/2016 Modified. Agregue subreporte RptEquityMembershipsPrevious
    /// [ecanul] 08/09/2016 Modified. Agregado nombre del web service y Club en el titulo del mensaje que genero un error.
    /// </history>
    public static void EquityReport(string membershipNum, Decimal company, int? clubAgency, int? clubGuest)
    {
      EnumClub club;
      // si tiene membrecia
      if (membershipNum != null && membershipNum != "")
      {//si tiene permiso para el reporte de equity
        if (Context.User.HasPermission(EnumPermission.Equity, EnumPermisionLevel.ReadOnly))
        {
          // // // ShowReport
          // determinamos el club
          if (clubAgency != null && clubAgency != 0)
          {
            club = (EnumClub)clubAgency;
          }
          else if(clubGuest != null && clubGuest != 0)
          {
            club = (EnumClub)clubGuest;
          }
          else
          {
            //Si no se encuentra un club en la tabla de agencies o en la tabla del guest se manda un error diciendo esto y se sale del metodo
            UIHelper.ShowMessage("Agency Not Found", MessageBoxImage.Error, "Equity Report");
            return;
          }

          Services.ClubesService.RptEquity rptClubes = null;

          var clubesError = $"Equity Report - Clubes - {EnumToListHelper.GetEnumDescription(club)}";
          var callCenterError = $"Equity Report - Credito y Cobranza - {EnumToListHelper.GetEnumDescription(club)}";
          try
          {
            //obtenemos los datos del reporte del servicio de Clubes
           rptClubes = ClubesHelper.GetRptEquity(membershipNum, Convert.ToInt32(company), club);
          }
          catch (Exception ex)
          {
            UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, clubesError);
            return;
          }
         

          //si no se pudo generenar el reporte en clubes y nos salimos
          if (rptClubes == null)
          {
            UIHelper.ShowMessage("This report did not return data", MessageBoxImage.Exclamation, clubesError);
            return;
          }
          //si no encontramos la membrecia en clubles, nos salimos
          if (rptClubes.Membership == null)
          {
            UIHelper.ShowMessage("Membership not found", MessageBoxImage.Exclamation, clubesError);
            return;
          }
          Services.CallCenterService.RptEquity rptCallCenter = null;
          try
          {
            //Obtenemos la membrecia en el servicio de Call Center
            rptCallCenter = CallCenterHelper.GetRptEquity(membershipNum, Convert.ToInt32(company), club);
          }
          catch (Exception ex)
          {
            UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, callCenterError);
            return;
          }   

          // si no se pudo generar reporte en Call Center nos salimos 
          if (rptCallCenter == null)
          {
            UIHelper.ShowMessage("This report did not return data", MessageBoxImage.Exclamation, callCenterError);
            return;
          }

          // si no encontramos la membresia en Credito y Cobranza Reserva, nos salimos
          if (rptCallCenter.Membership == null)
          {
            UIHelper.ShowMessage("Membership not found", MessageBoxImage.Exclamation, callCenterError);
            return;
          }

          // indicamos que ciertas listas del reporte no tienen registros
          RptEquityHeader header = new RptEquityHeader();
          header.HasGolfFields = rptClubes.GolfFieldsDetail.Length > 0;
          header.IsElite = (club == EnumClub.PalaceElite);

          var equity = new rptEquity();
          //datos generales de equityM:\PalaceResorts\Client4.6\Win\IntelligenceMarketing\IM.Inhouse\Classes\EquityHelpers.cs
          equity.Database.Tables["Membership"].SetDataSource(ObjectHelper.ObjectToList(rptClubes.Membership));
          equity.Database.Tables["Member"].SetDataSource(ObjectHelper.ObjectToList(rptCallCenter.Membership));
          //Si no tiene un Salesman  con OPC no se envia nada
          Services.ClubesService.RptEquitySalesman[] salesmanOPC = rptClubes.Salesmen.Where(sm => sm.Title.Trim() == "OPC").ToArray();
          if (salesmanOPC.Length > 0)
            equity.Database.Tables["SalesmanOPC"].SetDataSource(salesmanOPC);
          equity.Database.Tables["Hotel"].SetDataSource(ObjectHelper.ObjectToList(rptClubes.Hotels));
          equity.Database.Tables["GolfFieldsHeader"].SetDataSource(ObjectHelper.ObjectToList(rptClubes.GolfFieldsHeader));
          if (rptClubes.Verification != null)
            rptClubes.Verification.VOLUMENGOLF = rptCallCenter.Membership.Golf - (rptClubes.Verification.VOLUMENGOLF / EquityHelpers.GetIVAByOffice(rptClubes.Membership.OFFICE));
          equity.Database.Tables["Verification"].SetDataSource(ObjectHelper.ObjectToList(rptClubes.Verification));
          //Subreportes
          if (club == EnumClub.PalaceElite)
          {
            header.HasMembershipsPrevious = rptClubes.MembershipsPrevious.Length > 0;
            if (header.HasMembershipsPrevious)
              equity.Subreports["rptEquityMembershipsPrevious.rpt"].SetDataSource(rptClubes.MembershipsPrevious);
          }
          equity.Subreports["rptEquitySalesman.rpt"].SetDataSource(rptClubes.Salesmen);
          equity.Subreports["rptEquityCoOwners.rpt"].SetDataSource(rptClubes.CoOwners);

          equity.Subreports["rptEquityBeneficiaries.rpt"].Database.Tables["Reporte"].SetDataSource(rptClubes.Beneficiaries);
          equity.Subreports["rptEquityBeneficiaries.rpt"].Database.Tables["Membership"].SetDataSource(ObjectHelper.ObjectToList(rptCallCenter.Membership));

          equity.Subreports["rptEquityGolfFields.rpt"].SetDataSource(rptClubes.GolfFieldsDetail);
          equity.Subreports["rptEquityRoomTypes.rpt"].SetDataSource(rptClubes.RoomTypes);

          RptEquityProvision[] provisionsSNORM = rptCallCenter.Provisions.Where(p => p.IsSNORM == true).ToArray();
          header.HasSNORM = provisionsSNORM.Length > 0;
          if (header.HasSNORM)
            equity.Subreports["rptEquityProvisionsSNORM.rpt"].SetDataSource(provisionsSNORM);

          RptEquityProvision[] provisionsSAIRF = rptCallCenter.Provisions.Where(p => p.IsSAIRF == true).ToArray();
          header.HasSAIRF = provisionsSAIRF.Length > 0;
          equity.Subreports["rptEquityProvisionsSAIRF.rpt"].SetDataSource(provisionsSAIRF);

          RptEquityProvision[] provisionsSRCI = rptCallCenter.Provisions.Where(p => p.IsSRCI == true).ToArray();
          header.HasSRCI = provisionsSRCI.Length > 0;
          equity.Subreports["rptEquityProvisionsSRCI.rpt"].SetDataSource(provisionsSRCI);

          RptEquityProvision[] provisionsSCOMP = rptCallCenter.Provisions.Where(p => p.IsSCOMP == true).ToArray();
          header.HasSCOMP = provisionsSCOMP.Length > 0;
          equity.Subreports["rptEquityProvisionsSCOMP.rpt"].SetDataSource(provisionsSCOMP);

          RptEquityProvision[] provisionsSCRG = rptCallCenter.Provisions.Where(p => p.IsSCRG == true).ToArray();
          header.HasSCRG = provisionsSCRG.Length > 0;
          equity.Subreports["rptEquityProvisionsSCRG.rpt"].SetDataSource(provisionsSCRG);

          RptEquityProvision[] provisionsSIGR = rptCallCenter.Provisions.Where(p => p.IsSIGR == true).ToArray();
          header.HasSIGR = provisionsSIGR.Length > 0;
          equity.Subreports["rptEquityProvisionsSIGR.rpt"].SetDataSource(provisionsSIGR);

          RptEquityProvision[] provisionsSVEC = rptCallCenter.Provisions.Where(p => p.IsSVEC == true).ToArray();
          header.HasSVEC = provisionsSVEC.Length > 0;
          equity.Subreports["rptEquityProvisionsSVEC.rpt"].SetDataSource(provisionsSVEC);

          RptEquityProvision[] provisionsSREF = rptCallCenter.Provisions.Where(p => p.IsSREF == true).ToArray();
          header.HasSREF = provisionsSREF.Length > 0;
          equity.Subreports["rptEquityProvisionsSREF.rpt"].SetDataSource(provisionsSREF);

          equity.Database.Tables["Header"].SetDataSource(ObjectHelper.ObjectToList(header));
          if (rptCallCenter.Reservations.Length != 0)
            equity.Subreports["rptEquityReservations.rpt"].SetDataSource(rptCallCenter.Reservations);

          if (rptClubes.BalanceElectronicPurseHeaders.Length == 0)
            equity.Subreports["rptEquityBalanceElectronicPurse.rpt"].ReportDefinition.Sections["GroupHeaderSection1"].SectionFormat.EnableSuppress = true;
          equity.Subreports["rptEquityBalanceElectronicPurse.rpt"].Database.Tables["EquityBalanceElectronicPurseDetail"].SetDataSource(rptClubes.BalanceElectronicPurseDetails);
          equity.Subreports["rptEquityBalanceElectronicPurse.rpt"].Database.Tables["BalanceElectronicPurseHeader"].SetDataSource(rptClubes.BalanceElectronicPurseHeaders);
          var sum = rptCallCenter.Membership.Down - rptCallCenter.Membership.Down_escrow - rptCallCenter.Membership.Down_bal;
          if (sum > 1)
            equity.Subreports["rptEquityPaymentPromises.rpt"].SetDataSource(rptClubes.PaymentPromises);
          if (club == EnumClub.PalaceElite)
          {
            equity.Subreports["rptEquityWeeksNights.rpt"].Database.Tables["WeeksNightsDetail"].SetDataSource(rptClubes.WeeksNightsDetails);
            equity.Subreports["rptEquityWeeksNights.rpt"].Database.Tables["WeeksNightsHeader"].SetDataSource(rptClubes.WeeksNightsHeaders);
          }
          if (club == EnumClub.PalaceElite)
            equity.Subreports["rptEquityGolfRCI.rpt"].SetDataSource(rptClubes.GolfRCI);
          else
            equity.ReportDefinition.ReportObjects["srptGolfRCI"].ObjectFormat.EnableSuppress = true;
          if (club == EnumClub.PalaceElite)
            equity.Subreports["rptEquityPromotions.rpt"].SetDataSource(rptClubes.Promotions);

          if (club == EnumClub.PalaceElite)
            if (rptClubes.MemberExtension != null)
              if (rptClubes.MemberExtension.WHOLESALER)
                equity.ReportDefinition.Sections["Section2"].SectionFormat.EnableSuppress = false;

          var _frmViewer = new frmViewer(equity);
          _frmViewer.ShowDialog();
        }
        else
          UIHelper.ShowMessage("Access denied");
      }
    }

    #endregion

  }
}

using IM.Model;
using IM.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRInvitationBase
  {
    /// <summary>
    /// Método para obtener el lenguaje en le formulario Invitatio Base
    /// </summary>
    public static IEnumerable<GetLanguages> GetLanguage()
    {
      try
      {
        using (var dbContext = new IMEntities())
        {

          var languajes = dbContext.USP_OR_GetLanguages(0).ToList();
          return languajes;
        }
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public static CatalogInvitInHouse GetCatalogInvitInHouse(string location, string user, string leadSource)
    {
      var catalog = new CatalogInvitInHouse();
      using (var dbContext = new IMEntities())
      {
        var markets = dbContext.USP_OR_GetCatalogsInhouse(location, user, leadSource);
        catalog.Markets = markets.ToList();

        var unAvailMots = markets.GetNextResult<unAvailMot>();
        catalog.UnAvailMots = unAvailMots.ToList();

        var currencies = unAvailMots.GetNextResult<currency>();
        catalog.Currencies = currencies.ToList();

        var languages = currencies.GetNextResult<GetLanguages>();
        catalog.Languages = languages.ToList();

        var maritalStatus = languages.GetNextResult<maritalStatus>();
        catalog.MaritalStatus = maritalStatus.ToList();

        var allGifts = maritalStatus.GetNextResult<allGift>();
        catalog.AllGifts = allGifts.ToList();

        var locationGifts = allGifts.GetNextResult<locationGift>();
        catalog.LocationGifts = locationGifts.ToList();

        var packagesGifts = locationGifts.GetNextResult<packagesGift>();
        catalog.PackagesGifts = packagesGifts.ToList();

        var salesRooms = packagesGifts.GetNextResult<GetSalesRooms>();
        catalog.SalesRooms = salesRooms.ToList();

        var leadSources = salesRooms.GetNextResult<leadSourcesInvitInHouse>();
        catalog.LeadSources = leadSources.ToList();

        var personnels = leadSources.GetNextResult<GetPersonnel>();
        catalog.PersonnelByLocation = personnels.ToList();

        var personnelActive = personnels.GetNextResult<personnelActive>();
        catalog.PersonnelActive = personnelActive.ToList();

        var agencies = personnelActive.GetNextResult<agency>();
        catalog.Agencies = agencies.ToList();

        var hotel = agencies.GetNextResult<hotel>();
        catalog.Hotels = hotel.ToList();

        var guestStatus = hotel.GetNextResult<guestStatus>();
        catalog.GuestStatus = guestStatus.ToList();

        var creditCardTypes = guestStatus.GetNextResult<creditCardType>();
        catalog.CreditCards = creditCardTypes.ToList();

        var notBookingMotives = creditCardTypes.GetNextResult<notBookingMotiv>();
        catalog.NotBookingMotives = notBookingMotives.ToList();

        var countries = notBookingMotives.GetNextResult<GetCountries>();
        catalog.Countries = countries.ToList();

        var assistanceStatus = countries.GetNextResult<assistanceStatus>();
        catalog.AssistanceStatus = assistanceStatus.ToList();

        var dayOFF = assistanceStatus.GetNextResult<dayOFF>();
        catalog.DaysOff = dayOFF.ToList();

        var paymentTypes = dayOFF.GetNextResult<paymentType>();
        catalog.PaymentTypes = paymentTypes.ToList();

        var refundTypes = paymentTypes.GetNextResult<refundType>();
        catalog.RefundTypes = refundTypes.ToList();

      }
      return catalog;
    }
  }
}

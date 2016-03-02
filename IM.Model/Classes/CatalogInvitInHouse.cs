using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Entities
{
  public class CatalogInvitInHouse
  {
    public IEnumerable<GetCatalogsInhouse> Markets { get; set; }

    public IEnumerable<unAvailMots> UnAvailMots { get; set; }

    public IEnumerable<currencies> Currencies { get; set; }

    public IEnumerable<GetLanguages> Languages { get; set; }

    public IEnumerable<maritalStatus> MaritalStatus { get; set; }

    public IEnumerable<allGifts> AllGifts { get; set; }

    public IEnumerable<locationGifts> LocationGifts { get; set; }

    public IEnumerable<packagesGifts> PackagesGifts { get; set; }

    public IEnumerable<GetSalesRooms> SalesRooms { get; set; }

    public IEnumerable<leadSources> LeadSources { get; set; }

    public IEnumerable<GetPersonnel> PersonnelByLocation { get; set; }

    public IEnumerable<personnelActive> PersonnelActive { get; set; }

    public IEnumerable<agencies> Agencies { get; set; }

    public IEnumerable<hotel> Hotels { get; set; }
    
    public IEnumerable<guestStatus> GuestStatus { get; set; }

    public IEnumerable<creditCardTypes> CreditCards { get; set; }

    public IEnumerable<notBookingMotives> NotBookingMotives { get; set; }

    public IEnumerable<GetCountries> Countries { get; set; }

    public IEnumerable<assistanceStatus> AssistanceStatus { get; set; }

    public IEnumerable<dayOFF> DaysOff { get; set; }

    public IEnumerable<paymentTypes> PaymentTypes { get; set; }

    public IEnumerable<refundTypes> RefundTypes { get; set; }

  }
}

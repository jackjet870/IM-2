using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Entities
{
  public class CatalogInvitOutHouse
  {
    public IEnumerable<currency> Currencies { get; set; }

    public IEnumerable<GetLanguages> Languages { get; set; }

    public IEnumerable<maritalStatus> MaritalStatus { get; set; }

    public IEnumerable<allGift> AllGifts { get; set; }

    public IEnumerable<locationGift> LocationGifts { get; set; }

    public IEnumerable<packagesGift> PackagesGifts { get; set; }

    public IEnumerable<GetSalesRooms> SalesRooms { get; set; }

    public IEnumerable<leadSourcesInvitInHouse> LeadSources { get; set; }

    public IEnumerable<GetPersonnel> PersonnelByLocation { get; set; }

    public IEnumerable<personnelActive> PersonnelActive { get; set; }

    public IEnumerable<agency> Agencies { get; set; }

    public IEnumerable<hotel> Hotels { get; set; }

    public IEnumerable<GetCountries> Countries { get; set; }

    public IEnumerable<guestStatus> GuestStatus { get; set; }

    public IEnumerable<paymentType> PaymentTypes { get; set; }

    public IEnumerable<assistanceStatus> AssistanceStatus { get; set; }

    public IEnumerable<dayOFF> DaysOff { get; set; }





    public IEnumerable<GetCatalogsInhouse> Markets { get; set; }
    
    public IEnumerable<unAvailMot> UnAvailMots { get; set; }

    

    public IEnumerable<creditCardType> CreditCards { get; set; }

    public IEnumerable<notBookingMotiv> NotBookingMotives { get; set; }

    

    

    

    public IEnumerable<refundType> RefundTypes { get; set; }
    
  }
}

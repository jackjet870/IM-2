//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IM.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Personnel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Personnel()
        {
            this.Assistances = new HashSet<Assistance>();
            this.CxCPayments = new HashSet<CxCPayment>();
            this.ExchangeRateLogs = new HashSet<ExchangeRateLog>();
            this.FoliosCxCCancellations = new HashSet<FolioCxCCancellation>();
            this.FoliosCxCByPR = new HashSet<FolioCxCPR>();
            this.FoliosInvitationsOuthouseByPR = new HashSet<FolioInvitationOuthousePR>();
            this.FoliosInvitationsOuthouseByPRCancellations = new HashSet<FolioInvitationOuthousePRCancellation>();
            this.GiftsLogs = new HashSet<GiftLog>();
            this.GiftsReceipts_AuthorizedBy = new HashSet<GiftsReceipt>();
            this.GiftsReceipts_Host = new HashSet<GiftsReceipt>();
            this.GiftsReceipts_PR = new HashSet<GiftsReceipt>();
            this.GiftsReceiptsLogs_AuthorizedBy = new HashSet<GiftsReceiptLog>();
            this.GiftsReceiptsLogs_ChangedBy = new HashSet<GiftsReceiptLog>();
            this.GiftsReceiptsLogs_Host = new HashSet<GiftsReceiptLog>();
            this.GiftsReceiptsLogs_PR = new HashSet<GiftsReceiptLog>();
            this.GiftsReceiptsPayments = new HashSet<GiftsReceiptPayment>();
            this.GuestLogs_ChangedBy = new HashSet<GuestLog>();
            this.GuestLogs_Closer1 = new HashSet<GuestLog>();
            this.GuestLogs_Closer2 = new HashSet<GuestLog>();
            this.GuestLogs_Closer3 = new HashSet<GuestLog>();
            this.GuestLogs_Exit1 = new HashSet<GuestLog>();
            this.GuestLogs_Exit2 = new HashSet<GuestLog>();
            this.GuestLogs_Liner1 = new HashSet<GuestLog>();
            this.GuestLogs_Liner2 = new HashSet<GuestLog>();
            this.GuestLogs_Podium = new HashSet<GuestLog>();
            this.GuestLogs_PR1 = new HashSet<GuestLog>();
            this.GuestLogs_PR2 = new HashSet<GuestLog>();
            this.GuestLogs_PRAvailability = new HashSet<GuestLog>();
            this.GuestLogs_PRContact = new HashSet<GuestLog>();
            this.GuestLogs_PRFollow = new HashSet<GuestLog>();
            this.GuestLogs_VLO = new HashSet<GuestLog>();
            this.Guests_Closer1 = new HashSet<Guest>();
            this.Guests_Closer2 = new HashSet<Guest>();
            this.Guests_Closer3 = new HashSet<Guest>();
            this.Guests_CloserCaptain1 = new HashSet<Guest>();
            this.Guests_EntryHost = new HashSet<Guest>();
            this.Guests_Exit1 = new HashSet<Guest>();
            this.Guests_Exit2 = new HashSet<Guest>();
            this.Guests_ExitHost = new HashSet<Guest>();
            this.Guests_GiftsHost = new HashSet<Guest>();
            this.Guests_Liner1 = new HashSet<Guest>();
            this.Guests_Liner2 = new HashSet<Guest>();
            this.Guests_LinerCaptain1 = new HashSet<Guest>();
            this.Guests_Podium = new HashSet<Guest>();
            this.Guests_PR1 = new HashSet<Guest>();
            this.Guests_PR2 = new HashSet<Guest>();
            this.Guests_PR3 = new HashSet<Guest>();
            this.Guests_PRAssign = new HashSet<Guest>();
            this.Guests_PRAvailability = new HashSet<Guest>();
            this.Guests_PRCaptain1 = new HashSet<Guest>();
            this.Guests_PRCaptain2 = new HashSet<Guest>();
            this.Guests_PRCaptain3 = new HashSet<Guest>();
            this.Guests_PRContact = new HashSet<Guest>();
            this.Guests_PRFolllow = new HashSet<Guest>();
            this.Guests_PRNoBook = new HashSet<Guest>();
            this.Guests_VLO = new HashSet<Guest>();
            this.GuestsMovements = new HashSet<GuestMovement>();
            this.LeadSources = new HashSet<LeadSource>();
            this.LoginsLogs = new HashSet<LoginLog>();
            this.MealTickets = new HashSet<MealTicket>();
            this.Configuration_Administrator = new HashSet<Configuration>();
            this.Configuration_Boss = new HashSet<Configuration>();
            this.PersonnelAccess = new HashSet<PersonnelAccess>();
            this.Personnel_Liners = new HashSet<Personnel>();
            this.PersonnelPermissions = new HashSet<PersonnelPermission>();
            this.PostsLogs = new HashSet<PostLog>();
            this.PostsLogs_ChangedBy = new HashSet<PostLog>();
            this.PRNotes = new HashSet<PRNote>();
            this.Sales_Closer1 = new HashSet<Sale>();
            this.Sales_Closer2 = new HashSet<Sale>();
            this.Sales_Closer3 = new HashSet<Sale>();
            this.Sales_CloserCaptain = new HashSet<Sale>();
            this.Sales_Exit1 = new HashSet<Sale>();
            this.Sales_Exit2 = new HashSet<Sale>();
            this.Sales_Liner1 = new HashSet<Sale>();
            this.Sales_Liner2 = new HashSet<Sale>();
            this.Sales_LinerCaptain1 = new HashSet<Sale>();
            this.Sales_Podium = new HashSet<Sale>();
            this.Sales_PR1 = new HashSet<Sale>();
            this.Sales_PR2 = new HashSet<Sale>();
            this.Sales_PR3 = new HashSet<Sale>();
            this.Sales_PRCaptain1 = new HashSet<Sale>();
            this.Sales_PRCaptain2 = new HashSet<Sale>();
            this.Sales_PRCaptain3 = new HashSet<Sale>();
            this.Sales_VLO = new HashSet<Sale>();
            this.SalesLogs_ChangedBy = new HashSet<SaleLog>();
            this.SalesLogs_Closer1 = new HashSet<SaleLog>();
            this.SalesLogs_Closer2 = new HashSet<SaleLog>();
            this.SalesLogs_Closer3 = new HashSet<SaleLog>();
            this.SalesLogs_CloserCaptain1 = new HashSet<SaleLog>();
            this.SalesLogs_Exit1 = new HashSet<SaleLog>();
            this.SalesLogs_Exit2 = new HashSet<SaleLog>();
            this.SalesLogs_Liner1 = new HashSet<SaleLog>();
            this.SalesLogs_Liner2 = new HashSet<SaleLog>();
            this.SalesLogs_LinerCaptain1 = new HashSet<SaleLog>();
            this.SalesLogs_Podium = new HashSet<SaleLog>();
            this.SalesLogs_PR1 = new HashSet<SaleLog>();
            this.SalesLogs_PR2 = new HashSet<SaleLog>();
            this.SalesLogs_PR3 = new HashSet<SaleLog>();
            this.SalesLogs_PRCaptain1 = new HashSet<SaleLog>();
            this.SalesLogs_PRCaptain2 = new HashSet<SaleLog>();
            this.SalesLogs_VLO = new HashSet<SaleLog>();
            this.SalesmenChanges_AuthorizedBy = new HashSet<SalesmanChange>();
            this.SalesmenChanges_MadeBy = new HashSet<SalesmanChange>();
            this.SalesmenChanges_NewSalesman = new HashSet<SalesmanChange>();
            this.SalesmenChanges_OldSalesman = new HashSet<SalesmanChange>();
            this.SalesRooms = new HashSet<SalesRoom>();
            this.SalesRoomsLogs = new HashSet<SalesRoomLog>();
            this.SalesSalesmen = new HashSet<SalesSalesman>();
            this.ShowsSalesmen = new HashSet<ShowSalesman>();
            this.TeamsGuestServices = new HashSet<TeamGuestServices>();
            this.TeamsLogs = new HashSet<TeamLog>();
            this.TeamsLogs_ChangedBy = new HashSet<TeamLog>();
            this.TeamsSalesmen = new HashSet<TeamSalesmen>();
            this.WarehouseMovements = new HashSet<WarehouseMovement>();
            this.Efficiencies = new HashSet<Efficiency>();
            this.Roles = new HashSet<Role>();
            this.BookingDeposits = new HashSet<BookingDeposit>();
            this.Guests_FTB1 = new HashSet<Guest>();
            this.Guests_FTB2 = new HashSet<Guest>();
            this.Sales_FTB1 = new HashSet<Sale>();
            this.Sales_FTB2 = new HashSet<Sale>();
            this.Guests_Closer4 = new HashSet<Guest>();
            this.Guests_Exit3 = new HashSet<Guest>();
            this.Guests_FTM1 = new HashSet<Guest>();
            this.Guests_FTM2 = new HashSet<Guest>();
            this.Guests_Liner3 = new HashSet<Guest>();
            this.Sales_Closer4 = new HashSet<Sale>();
            this.Sales_Exit3 = new HashSet<Sale>();
            this.Sales_FTM1 = new HashSet<Sale>();
            this.Sales_FTM2 = new HashSet<Sale>();
            this.Sales_Liner3 = new HashSet<Sale>();
        }
    
        public string peID { get; set; }
        public string peN { get; set; }
        public string pePwd { get; set; }
        public string peCaptain { get; set; }
        public bool peA { get; set; }
        public string peps { get; set; }
        public System.DateTime pePwdD { get; set; }
        public int pePwdDays { get; set; }
        public string peTeamType { get; set; }
        public string pePlaceID { get; set; }
        public string peTeam { get; set; }
        public string pepo { get; set; }
        public string peLinerID { get; set; }
        public string pede { get; set; }
        public string peSalesmanID { get; set; }
        public string peCollaboratorID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Assistance> Assistances { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CxCPayment> CxCPayments { internal get; set; }
        public virtual DayOff DaysOff { internal get; set; }
        public virtual Dept Dept { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExchangeRateLog> ExchangeRateLogs { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FolioCxCCancellation> FoliosCxCCancellations { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FolioCxCPR> FoliosCxCByPR { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FolioInvitationOuthousePR> FoliosInvitationsOuthouseByPR { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FolioInvitationOuthousePRCancellation> FoliosInvitationsOuthouseByPRCancellations { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftLog> GiftsLogs { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftsReceipt> GiftsReceipts_AuthorizedBy { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftsReceipt> GiftsReceipts_Host { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftsReceipt> GiftsReceipts_PR { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftsReceiptLog> GiftsReceiptsLogs_AuthorizedBy { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftsReceiptLog> GiftsReceiptsLogs_ChangedBy { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftsReceiptLog> GiftsReceiptsLogs_Host { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftsReceiptLog> GiftsReceiptsLogs_PR { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftsReceiptPayment> GiftsReceiptsPayments { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_ChangedBy { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_Closer1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_Closer2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_Closer3 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_Exit1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_Exit2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_Liner1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_Liner2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_Podium { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_PR1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_PR2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_PRAvailability { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_PRContact { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_PRFollow { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestLog> GuestLogs_VLO { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_Closer1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_Closer2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_Closer3 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_CloserCaptain1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_EntryHost { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_Exit1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_Exit2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_ExitHost { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_GiftsHost { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_Liner1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_Liner2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_LinerCaptain1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_Podium { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_PR1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_PR2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_PR3 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_PRAssign { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_PRAvailability { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_PRCaptain1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_PRCaptain2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_PRCaptain3 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_PRContact { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_PRFolllow { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_PRNoBook { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_VLO { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestMovement> GuestsMovements { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeadSource> LeadSources { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LoginLog> LoginsLogs { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MealTicket> MealTickets { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Configuration> Configuration_Administrator { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Configuration> Configuration_Boss { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonnelAccess> PersonnelAccess { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Personnel> Personnel_Liners { internal get; set; }
        public virtual Personnel Personnel_Liner { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonnelPermission> PersonnelPermissions { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostLog> PostsLogs { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostLog> PostsLogs_ChangedBy { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRNote> PRNotes { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_Closer1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_Closer2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_Closer3 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_CloserCaptain { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_Exit1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_Exit2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_Liner1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_Liner2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_LinerCaptain1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_Podium { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_PR1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_PR2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_PR3 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_PRCaptain1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_PRCaptain2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_PRCaptain3 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_VLO { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_ChangedBy { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_Closer1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_Closer2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_Closer3 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_CloserCaptain1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_Exit1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_Exit2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_Liner1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_Liner2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_LinerCaptain1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_Podium { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_PR1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_PR2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_PR3 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_PRCaptain1 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_PRCaptain2 { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_VLO { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesmanChange> SalesmenChanges_AuthorizedBy { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesmanChange> SalesmenChanges_MadeBy { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesmanChange> SalesmenChanges_NewSalesman { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesmanChange> SalesmenChanges_OldSalesman { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesRoom> SalesRooms { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesRoomLog> SalesRoomsLogs { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesSalesman> SalesSalesmen { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShowSalesman> ShowsSalesmen { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeamGuestServices> TeamsGuestServices { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeamLog> TeamsLogs { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeamLog> TeamsLogs_ChangedBy { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeamSalesmen> TeamsSalesmen { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WarehouseMovement> WarehouseMovements { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Efficiency> Efficiencies { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Roles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingDeposit> BookingDeposits { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_FTB1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_FTB2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_FTB1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_FTB2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_Closer4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_Exit3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_FTM1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_FTM2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests_Liner3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_Closer4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_Exit3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_FTM1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_FTM2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales_Liner3 { get; set; }
    }
}

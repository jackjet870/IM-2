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
    
    public partial class Gift
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Gift()
        {
            this.GiftsInventories = new HashSet<GiftInventory>();
            this.GiftsLogs = new HashSet<GiftLog>();
            this.GiftsPackagesItems = new HashSet<GiftPackageItem>();
            this.GiftsPackages = new HashSet<GiftPackageItem>();
            this.GiftsReceiptsDetails = new HashSet<GiftsReceiptDetail>();
            this.GiftsReceiptsPackagesItems = new HashSet<GiftsReceiptPackageItem>();
            this.GiftsReceiptsPackages = new HashSet<GiftsReceiptPackageItem>();
            this.GuestsPromotions = new HashSet<GuestPromotion>();
            this.InvitationsGifts = new HashSet<InvitationGift>();
            this.WarehouseMovements = new HashSet<WarehouseMovement>();
            this.Agencies = new HashSet<Agency>();
            this.Locations = new HashSet<Location>();
        }
    
        public string giID { get; set; }
        public string giN { get; set; }
        public string giShortN { get; set; }
        public decimal giPrice1 { get; set; }
        public decimal giPrice2 { get; set; }
        public decimal giPrice3 { get; set; }
        public decimal giPrice4 { get; set; }
        public bool giPack { get; set; }
        public string gigc { get; set; }
        public bool giInven { get; set; }
        public bool giA { get; set; }
        public bool giWFolio { get; set; }
        public bool giWPax { get; set; }
        public int giO { get; set; }
        public bool giUnpack { get; set; }
        public int giMaxQty { get; set; }
        public bool giMonetary { get; set; }
        public decimal giAmount { get; set; }
        public string giProductGiftsCard { get; set; }
        public bool giCountInPackage { get; set; }
        public bool giCountInCancelledReceipts { get; set; }
        public string gipr { get; set; }
        public string giPVPPromotion { get; set; }
        public bool giWCost { get; set; }
        public decimal giPublicPrice { get; set; }
        public bool giAmountModifiable { get; set; }
        public string giOperaTransactionType { get; set; }
        public string giPromotionOpera { get; set; }
        public bool giSale { get; set; }
        public decimal giPriceMinor { get; set; }
        public decimal giPriceExtraAdult { get; set; }
        public int giQty { get; set; }
        public Nullable<bool> giDiscount { get; set; }
    
        public virtual Product Product { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftInventory> GiftsInventories { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftLog> GiftsLogs { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftPackageItem> GiftsPackagesItems { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftPackageItem> GiftsPackages { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftsReceiptDetail> GiftsReceiptsDetails { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftsReceiptPackageItem> GiftsReceiptsPackagesItems { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftsReceiptPackageItem> GiftsReceiptsPackages { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestPromotion> GuestsPromotions { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvitationGift> InvitationsGifts { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WarehouseMovement> WarehouseMovements { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agency> Agencies { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Location> Locations { internal get; set; }
    }
}

using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
    {
    public class CpmUnitViewModel : ViewModelBase
    {
        //[Required]
        //[Display(Name = "Project Type")]
        //public CpmProjectTypeEnum? PropertyType { get; set; }

        //public string PropertyTypeText
        //{
        //    get { return PropertyType.Description(); }
        //}
        [Required]
        [Display(Name = "Project Name")]
        public long? PropertyId { get; set; }
        [Display(Name = "Project Name")]
        public string PropertyName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }      

        
        [Display(Name = "Unit ")]
        public string UnitWithApartmentType { get; set; }

        [Required]
        [Display(Name = "Unit Number")]
        public string UnitNumber { get; set; }
        

        [Required]
        [Display(Name = "Unit Type")]
        public string UnitType { get; set; }

        [Display(Name = "Unit Type")]
        public string UnitTypeName { get; set; }

        [Required]
        [Display(Name = "Size sqft")]
        public string SizeSqft { get; set; }

        [Required]
        [Display(Name = "Rent (Annual)")]
        //public string Rent { get; set; }
        public float Rent { get; set; }

        [Display(Name = "Floor")]
        public string FloorNumber { get; set; }

        [Display(Name = "Unit Area")]
        public string UnitArea { get; set; }

        [Display(Name = "Beds")]
        public string NumberOfBedrooms { get; set; }

        [Display(Name = "Beds")]
        public string NumberOfBedroomsName { get; set; }

        [Display(Name = "Baths")]
        public string NumberOfBaths { get; set; }

        [Required]
        [Display(Name = "Deposit")]
        public string Deposit { get; set; }

        [Display(Name = "Electricity No")]
        public string ElectricityNumber { get; set; }

        [Display(Name = "Municipality No")]
        public string MunicipalityNumber { get; set; }

        [Display(Name = "Compound No")]
        public string CompoundNumber { get; set; }

        [Display(Name = "Number of Parkings")]
        public string NumberOfParking { get; set; }

        [Display(Name = "Parking No")]
        public string ParkingNumber { get; set; }


        [Display(Name = "Management Fee type ")]
        public ManagementFeeTypeEnum? ManagementFeeType  { get; set; }

        [Display(Name = "Unit Status")]
        public UnitStatusTypeEnum? UnitStatus { get; set; }

        [Display(Name = "Furnished")]
        public bool Furnished { get; set; }

        [Display(Name = "SmokingAllowed")]
        public bool SmokingAllowed { get; set; }

        [Display(Name = "Electricity")]
        public bool Electricity { get; set; }

        [Display(Name = "Internet")]
        public bool Internet { get; set; }

        [Display(Name = "Water")]
        public bool Water { get; set; }

        [Display(Name = "Air Conditioning")]
        public bool AirConditioning { get; set; }

        [Display(Name = "Carpet")]
        public bool Carpet { get; set; }

        [Display(Name = "Fireplace")]
        public bool FirePlace { get; set; }

        [Display(Name = "Management Fee")]
        public string ManagementFee { get; set; }

        [Display(Name = "Total Area")]
        public string TotalArea { get; set; }

        [Display(Name = "Balcony Area")]
        public string BalconyArea { get; set; }

        [Display(Name = "Parking")]
        public int? NoOfParking { get; set; }

        [Display(Name = "Marketing Title")]
        public string MarketingTitle { get; set; }

        [Display(Name = "Marketing Description")]
        public string MarketingDescription { get; set; }
        [Required]
        [Display(Name = "Owner")]
        public string OwnerName { get; set; }
        [Required]
        [Display(Name = "Owner")]
        public long? OwnerId { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Expiry Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Unit Pictures ")]
        public string PropertyAttachmentId { get; set; }

        [Display(Name = "Property Photo")]
        public FileViewModel PropertyAttachmentSelectedFile { get; set; }

        [Display(Name = "Listing Attachment")]
        public string ListingAttachmentId { get; set; }

        [Display(Name = "Listing Images")]
        public FileViewModel ListingAttachmentSelectedFile { get; set; }

        [Display(Name = "Document Attachment")]
        public string DocumentsAttachmentId { get; set; }

        [Display(Name = "Documents")]
        public FileViewModel DocumentsSelectedFile { get; set; }
        public IList<FileViewModel> UnitDocuments { get; set; }
        public IList<FileViewModel> UnitListDocuments { get; set; }

        [Display(Name = " Attachment Description")]
        public string DocumentDescription { get; set; }

        [Display(Name = "Attachment Description")]
        public string PictureDescription { get; set; }
        public DateTime? StatusChangeDate { get; set; }

        [Display(Name = "Unit Classification ")]
        [Required]
        public CpmUnitClassification? UnitClassification { get; set; }

    }
}

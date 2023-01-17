using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class OrganizationViewModel : DatedViewModelBase
    {
        [Display(Name = "Department Name")]
        public long OrganizationId { get; set; }

        //[Display(Name = "Organization Name")]
        [Display(Name = "FullName", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string FullName { get; set; }

        //[Display(Name = "Organization Name (Arabic)")]
        [Display(Name = "NameLocal", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string NameLocal { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Name")]
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string Name { get; set; }

        public string Code { get; set; }

        //[Display(Name = "Sequence No")]
        [Display(Name = "SequenceNo", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public long? SequenceNo { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string Description { get; set; }

        [Display(Name = "Remarks", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string Remarks { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Cost Center Name")]
        [Display(Name = "CostCenterId", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public long? CostCenterId { get; set; }


        [Display(Name = "Responsibility Center")]
        public long? ResponsibilityCenterId { get; set; }

        [Display(Name = "Responsibility Center Name")]
        public string ResponsibilityCenterName { get; set; }

        //[Display(Name = "Cost Center Name")]
        [Display(Name = "CostCenterName", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string CostCenterName { get; set; }


        //[Display(Name = "Location")]
        [Display(Name = "LocationId", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public long? LocationId { get; set; }

        //[Display(Name = "Location")]
        [Display(Name = "LocationName", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string LocationName { get; set; }

        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Organization Type")]
        [Display(Name = "OrganizationTypeCode", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string OrganizationTypeCode { get; set; }

        //[Display(Name = "Organization Type")]
        [Display(Name = "OrganizationTypeName", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string OrganizationTypeName { get; set; }

        //[Display(Name = "Parent Organization Name")]
        [Display(Name = "ParentOrganizationName", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string ParentOrganizationName { get; set; }

        //[Display(Name = "Parent Organization Name")]
        [Display(Name = "ParentOrganizationId", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public long? ParentOrganizationId { get; set; }

        [Display(Name = "HierarchyId", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public long? HierarchyId { get; set; }

        //[Display(Name = "Organization Category")]
        [Display(Name = "OrganizationCategoryCode", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string OrganizationCategoryCode { get; set; }

        //[Display(Name = "Organization Category")]
        //public OrganizationCategoryEnum OrganizationCategoryName { get; set; }
        [Display(Name = "OrganizationCategory", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public OrganizationCategoryEnum? OrganizationCategory { get; set; }

        //[Display(Name = "Organization Admin")]
        [Display(Name = "AdminUserId", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public long? AdminUserId { get; set; }

        [Display(Name ="Department Admin Name")]
        public string AdminUserName { get; set; }

        //[Display(Name = "Brand")]
        [Display(Name = "BrandId", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public long? BrandId { get; set; }

        //[Display(Name = "Brand")]
        [Display(Name = "BrandName", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string BrandName { get; set; }

        //[Display(Name = "Restaurant")]
        [Display(Name = "RestaurantId", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public long? RestaurantId { get; set; }

        //[Display(Name = "Restaurant")]
        [Display(Name = "RestaurantName", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string RestaurantName { get; set; }

        [Display(Name = "Legal Entity")]
        public long? LegalEntityId { get; set; }

        [Display(Name = "Legal Entity")]
        // [Display(Name = "CompanyName", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string LegalEntityName { get; set; }

        [Display(Name = "PayrollOrganizationId", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public long? PayrollOrganizationId { get; set; }
        [Display(Name = "PayrollOrganizationName", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public string PayrollOrganizationName { get; set; }
        [Display(Name = "IsPayrollOrganization", ResourceType = typeof(ERP.Translation.Hrs.Organization))]
        public bool? IsPayrollOrganization { get; set; }

        [Display(Name = "Restaurant Style")]
        public RestaurantTypeEnum? RestaurantType { get; set; }
        [Display(Name = "Document Code")]
        public string DocumentCode { get; set; }
        public string NewName { get; set; }
    }
}

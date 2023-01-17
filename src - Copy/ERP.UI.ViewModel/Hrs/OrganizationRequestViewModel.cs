using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class OrganizationRequestViewModel : BaseRequestViewModel
    {
        public int Id { get; set; }

        public DateTime AsOnDate { get; set; }

        public TransactionMode Mode { get; set; }

        public int? HierarchyNameId { get; set; }

        public int? ParentOrganizationId { get; set; }

        [Display(Name = "Parent Organization")]
        public string ParentOrganizationName { get; set; }

        [Display(Name = "Group Name")]
        public string Group { get; set; }

        [Display(Name = "Department Name")]
        public string Department { get; set; }

        [Display(Name = "Division Name")]
        public string Division { get; set; }

        [Display(Name = "Section Name")]
        public string Section { get; set; }

        [Display(Name = "Unit Name")]
        public string Unit { get; set; }

        public  int OrganizationId { get; set; }

        [Display(Name = "GRP Organization Id")]
        public  int? GRPOrganizationId { get; set; }

        //[Required]
        [Display(Name = "Reporting Name")]
        public  string Name { get; set; }

        [Display(Name = "Organization Name (Arabic)")]
        public  string NameAr { get; set; }

      
        [Required]
        [Display(Name = "Organization Name")]
        public  string FullName { get; set; }

        [Display(Name = "Description")]
        public  string Description { get; set; }

        [Display(Name = "Remarks")]
        public  string Remarks { get; set; }

        [Display(Name = "Location")]
        [Required]
        public  int? LocationId { get; set; }

        [Display(Name = "Location")]
        public string LocationName { get; set; }

      
        [Display(Name = "Cost Center Name")]
        [Required]
        public  int? CostCenterId { get; set; }

        [Display(Name = "Cost Center Name")]
        public string CostCenterName  { get; set; }

        [Display(Name = "Cost Center Code")]
        public  string CostCenterCode { get; set; }

        [Required]
        [Display(Name = "Organization Type")]
        public int? OrganizationTypeId { get; set; }

        [Display(Name = "Organization Type")]
        public string OrganizationTypeName { get; set; }

        [Display(Name = "Sequence No")]
        public  int? SequenceNo { get; set; }

        [Display(Name = "Sequence No")]
        public string SequenceNoName { get; set; }



        [Display(Name = "Organization Effective Start Date")]
        public override DateTime? EffectiveFromDate
        {
            get
            {
                return base.EffectiveFromDate;
            }

            set
            {
                base.EffectiveFromDate = value;
            }
        }


        [Display(Name = "Organization Effective End Date")]
        public override DateTime? EffectiveToDate
        {
            get
            {
                return base.EffectiveToDate;
            }

            set
            {
                base.EffectiveToDate = value;
            }
        }



    }
}

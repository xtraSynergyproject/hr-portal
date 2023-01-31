using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System;
using System.Collections.Generic;

namespace ERP.UI.ViewModel
{
    public class OrganizationMoveViewModel : BaseRequestViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Hierarchy Name")]
        public int? HierarchyNameId { get; set; }

        [Display(Name = "Hierarchy Name")]
        public string HierarchyName { get; set; }

        [Display(Name = "Organization Id")]
        public int? OrganizationId { get; set; }

        public int? OrganizationHierarchyId { get; set; }




        [Display(Name = "Organization Name")]
        public string OrganizationName { get; set; }

        [Display(Name = "Parent Organization Name")]
        public int? ParentOrganizationId { get; set; }

        [Display(Name = "Parent Organization Name")]
        public string ParentOrganizationName { get; set; }

        [Required]
        [Display(Name = "Proposed Parent Organization Name")]
        public int? ProposedParentOrganizationId { get; set; }

        [Display(Name = "Proposed Parent Organization Name")]
        public string ProposedParentOrganizationName { get; set; }


        public string Remarks { get; set; }
        public DateTime AsOnDate { get; set; }

        public int Level { get; set; }
        public TransactionMode Mode { get; set; }
        public bool EnableFromDate { get; set; }
        public bool IsSuccess { get; set; }
        public GridSelectOption HistoryGridSelectOption { get; set; }
        public bool OrganizationRemoveReasonCode { get; set; }
        [Display(Name = "Organization Name")]
        public string OrganizationNameWithTitle { get; set; }

        public int? OrganizationTypeId { get; set; }

        public string OrganizationType { get; set; }

        [Display(Name = "Organization Move Effective Start Date")]
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
        [Display(Name = "Organization Move Effective To Date")]
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

using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System;

namespace ERP.UI.ViewModel
{
    public class OrganizationRemoveViewModel : BaseRequestViewModel
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Hierarchy Name")]
        public int? HierarchyNameId { get; set; }

       
        [Required]
        [Display(Name = "Organization")]
        public int? OrganizationId { get; set; }


        [Display(Name = "Parent Organization")]
        public int? ParentOrganizationId { get; set; }

        [Display(Name = "Parent Organization")]
        public string ParentOrganizationName { get; set; }

        

        //[Display(Name = "Proposed Supervisor")]
        //public int? ProposedSupervisorId { get; set; }

        //[Display(Name = "Proposed Supervisor")]
        //public string ProposedSupervisorName { get; set; }

        //[Required]
        //[Display(Name = "Proposed Parent Position")]
        //public int? ProposedParentPositionId { get; set; }

        //[Display(Name = "Proposed Parent Position")]
        //public string ProposedParentPositionName { get; set; }


        public string Remarks { get; set; }

        public int Level { get; set; }
        public TransactionMode Mode { get; set; }
        public bool EnableFromDate { get; set; }
        public bool IsSuccess { get; set; }
        public GridSelectOption HistoryGridSelectOption { get; set; }
        [Display(Name = "Reason For Removal")]
        [Required]
        public string OrganizationRemoveReasonCode { get; set; }
        public DateTime AsOnDate { get; set; }



        [Display(Name = "Organization Remove Effective Start Date")]
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


        [Display(Name = "Organization Remove Effective End Date")]
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

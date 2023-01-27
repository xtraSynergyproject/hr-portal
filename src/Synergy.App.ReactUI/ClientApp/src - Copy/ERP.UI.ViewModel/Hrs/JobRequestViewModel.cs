using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class JobRequestViewModel : BaseRequestViewModel
    {
        public int Id { get; set; }

        public int? JobId { get; set; }

        public int? ManpowerOrganizationRootId { get; set; }

        public int? JDNoteId { get; set; }
        public string JDReferenceId { get; set; }
        public string JDVersionNo { get; set; }


        [Display(Name = "Evaluated Job Title")]
        public string Name { get; set; }


        [Display(Name = "Job Title (Arabic)")]
        public string NameAr { get; set; }


        
        [Display(Name = "Proposed Job Title")]
        public string ProposedJobName { get; set; }

        public int? PositionId { get; set; }


        [Display(Name = "Position Name")]
        public string PositionName { get; set; }

        public int? ParentPositionId { get; set; }


        [Display(Name = "Parent Position Name")]
        public string ParentPositionName { get; set; }



        public int? OrganizationId { get; set; }


        [Display(Name = "Proposed Grade")]
        public int? ProposedGradeId { get; set; }

        [Display(Name = "Evaluated Grade")]
        public int? GradeId { get; set; }
        public int? OldJobId { get; set; }
        public int? SequenceNo { get; set; }
        public int? JobFamilyId { get; set; }

        public string Description { get; set; }



        //public int? GRPPositionId { get; set; }



        //[Display(Name = "Position Name")]
        //public string PositionName { get; set; }


        //public int? OrgHierarchyNameId { get; set; }


        //[Display(Name = "Position Title")]
        //public string PositionTitle { get; set; }



        //[Display(Name = "Position Description")]
        //public string Description { get; set; }
        //public string Remarks { get; set; }
        //[Display(Name = "Serial No")]
        //public string SerialNo { get; set; }


        //[Required]
        //[Display(Name = "Job Name")]
        //public int? PositionJobId { get; set; }

        ////[Required]
        ////[Display(Name = "MyDAS Job Name")]
        ////public int? PositionMyDASJobId { get; set; }

        ////[Display(Name = "MyDAS Job Name")]
        ////public string PositionMyDASJobName { get; set; }

        //[Display(Name = "Position Grade Name")]
        ////   [Required]
        //public int? PositionGradeId { get; set; }

        //[Display(Name = "Position Grade Name")]
        //public string PositionGradeName { get; set; }

        //[Display(Name = "Airport Location Name")]
        //public int? PositionLocationId { get; set; }

        //[Display(Name = "Airport Location Name")]
        //public string PositionLocationName { get; set; }

        //[Display(Name = "Position Budget Type")]
        //public string PositionBudgetType { get; set; }

        //[Required]
        //[Display(Name = "Position Budget Type")]
        //public string PositionBudgetTypeCode { get; set; }

        //[Display(Name = "Action Reason")]
        //public string PositionActionReason { get; set; }


        //[Display(Name = "Action Reason")]
        //public string PositionActionReasonCode { get; set; }

        //[Display(Name = "Parent Position")]
        //public string ParentPositionName { get; set; }

        //[Display(Name = "Position Name")]
        //public virtual string PositionNameWithTitle { get; set; }


        //[Display(Name = "Parent Employee Name")]
        //public string ParentEmployeeName { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeFullName { get; set; }

        //[Required]
        //[Display(Name = "Group Name")]
        //public int? GroupId { get; set; }

        //[Required]
        //[Display(Name = "Department Name")]
        //public int? DepartmentId { get; set; }

        //[Required]
        //[Display(Name = "Division Name")]
        //public int? DivisionId { get; set; }

        //[Display(Name = "Section Name")]
        //public int? SectionId { get; set; }

        //[Required]
        //[Display(Name = "Unit Name")]
        //public int? UnitId { get; set; }

        [Display(Name = "Group")]
        public string Group { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Division")]
        public string Division { get; set; }

        [Display(Name = "Section")]
        public string Section { get; set; }

        [Display(Name = "Unit")]
        public string Unit { get; set; }

        public TransactionMode Mode { get; set; }

        //[Display(Name = "Organization Name")]
        //public int? PositionOrganizationId { get; set; }

        //[Display(Name = "Organization Name")]
        //public string PositionOrganizationFullName { get; set; }



        //[Display(Name = "Job Description(JD)")]
        //public string JobDescriptionName { get; set; }

        //[Required]
        //[Display(Name = "Position Employment Type")]
        //public int? PositionTypeId { get; set; }

        //[Display(Name = "Position Employment Type")]
        //public string PositionTypeName { get; set; }

        //[Required]
        //[Display(Name = "Position Validity Status")]
        //public int? PositionStatusId { get; set; }

        //[Display(Name = "Position Validity Status")]
        //public string PositionStatusName { get; set; }

        //[Required]
        //[Display(Name = "Position Occupancy Status")]
        //public int? PositionHiringStatusId { get; set; }

        //[Display(Name = "Position Occupancy Status")]
        //public string PositionHiringStatusName { get; set; }

        //public bool? NeedDummyParent { get; set; }


        //[Required]
        //[Display(Name = "Parent Position Name")]
        //public virtual int? ParentPositionId { get; set; }
        public int? HierarchyNameId { get; set; }
        public DateTime AsOnDate { get; set; }
        [Display(Name = "Job Effective Start Date")]
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


        [Display(Name = "Job Effective End Date")]
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

        public string Source { get; set; }


    }
}

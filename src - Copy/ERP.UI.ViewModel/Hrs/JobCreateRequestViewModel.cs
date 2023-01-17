using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class JobCreateRequestViewModel : BaseRequestViewModel
    {
        public int Id { get; set; }

        public int? JobId { get; set; }

        public int? ManpowerOrganizationRootId { get; set; }

        public int? JDNoteId { get; set; }
        public string JDReferenceId { get; set; }
        public string JDVersionNo { get; set; }


        [Display(Name = "Job Title")]
        public string Name { get; set; }


        [Display(Name = "Job Name (Arabic)")]
        public string NameAr { get; set; }


        [Required]
        [Display(Name = "Proposed Job Title")]
        public string ProposedJobName { get; set; }

        [Display(Name = "Proposed Job Title (Arabic)")]
        [Required]
        public string ProposedJobNameAr { get; set; }


        public int? PositionId { get; set; }


        [Display(Name = "Position Name")]
        public string PositionName { get; set; }

        public int? ParentPositionId { get; set; }


        [Display(Name = "Parent Position Name")]
        public string ParentPositionName { get; set; }



        public int? OrganizationId { get; set; }

        [Required]
        [Display(Name = "Proposed Grade")]
        public int? ProposedGradeId { get; set; }

        [Display(Name = "Grade")]
        public int? GradeId { get; set; }
        public int? OldJobId { get; set; }
        public int? SequenceNo { get; set; }
        public int? JobFamilyId { get; set; }

        public string Description { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeFullName { get; set; }

        //  [Required]
        [Display(Name = "Group")]
        public int? GroupId { get; set; }

        // [Required]
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        //[Required]
        [Display(Name = "Division")]
        public int? DivisionId { get; set; }

        [Display(Name = "Section")]
        public int? SectionId { get; set; }

        [Required]
        [Display(Name = "Unit")]
        public int? UnitId { get; set; }

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
         
        public int? HierarchyNameId { get; set; }
        public DateTime AsOnDate { get; set; }
        [Display(Name = "Job Effective Start Date")]
        [Required]
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

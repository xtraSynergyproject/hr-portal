using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class PositionRequestViewModel : BaseRequestViewModel
    {

        public int Id { get; set; }

        public int? PositionId { get; set; }
        public int? GRPPositionId { get; set; }
        public int? ManpowerOrganizationRootId { get; set; }


        [Display(Name = "Position Name")]
        public string PositionName { get; set; }


        public int? OrgHierarchyNameId { get; set; }


        [Display(Name = "Position Title")]
        public string PositionTitle { get; set; }

        [Display(Name = "Position Title (Arabic)")]
        public string PositionTitleAr { get; set; }

        [Display(Name = "Position Description")]
        public string Description { get; set; }
        public string Remarks { get; set; }
        [Display(Name = "Serial No")]
        public string SerialNo { get; set; }


        [Required]
        [Display(Name = "Job Title")]
        public int? PositionJobId { get; set; }

        [Display(Name = "Job Title")]
        public string PositionJobName { get; set; }

        //[Required]
        //[Display(Name = "MyDAS Job Name")]
        //public int? PositionMyDASJobId { get; set; }

        //[Display(Name = "MyDAS Job Name")]
        //public string PositionMyDASJobName { get; set; }

        [Display(Name = "Position Grade")]
        //   [Required]
        public int? PositionGradeId { get; set; }

        [Display(Name = "Position Grade")]
        public string PositionGradeName { get; set; }

        [Display(Name = "Airport Location")]
        public int? PositionLocationId { get; set; }

        [Display(Name = "Airport Location")]
        public string PositionLocationName { get; set; }

        [Display(Name = "Position Budget Type")]
        public string PositionBudgetType { get; set; }

        [Required]
        [Display(Name = "Position Budget Type")]
        public string PositionBudgetTypeCode { get; set; }

        [Display(Name = "Action Reason")]
        public string PositionActionReason { get; set; }


        [Display(Name = "Action Reason")]
        public string PositionActionReasonCode { get; set; }

        [Display(Name = "Parent Position")]
        public string ParentPositionName { get; set; }

        [Display(Name = "Position Name")]
        public virtual string PositionNameWithTitle { get; set; }


        [Display(Name = "Parent Employee Name")]
        public string ParentEmployeeName { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeFullName { get; set; }

        [Display(Name = "Group")]
        public int? GroupId { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

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

        [Display(Name = "Organization Name")]
        public int? PositionOrganizationId { get; set; }

        [Display(Name = "Organization Name")]
        public string PositionOrganizationFullName { get; set; }

        public int? JobDescriptionId { get; set; }

        [Display(Name = "Job Description (JD)")]
        public string JobDescriptionName { get; set; }

        [Required]
        [Display(Name = "Position Employment Type")]
        public int? PositionTypeId { get; set; }

        [Display(Name = "Position Employment Type")]
        public string PositionTypeName { get; set; }

        [Required]
        [Display(Name = "Position Validity Status")]
        public int? PositionStatusId { get; set; }

        [Display(Name = "Position Validity Status")]
        public string PositionStatusName { get; set; }

        [Required]
        [Display(Name = "Position Occupancy Status")]
        public int? PositionHiringStatusId { get; set; }

        [Display(Name = "Position Occupancy Status")]
        public string PositionHiringStatusName { get; set; }

        public bool? NeedDummyParent { get; set; }


        [Required]
        [Display(Name = "Parent Position Name")]
        public virtual int? ParentPositionId { get; set; }
        public int? HierarchyNameId { get; set; }
        public DateTime AsOnDate { get; set; }
        [Display(Name = "Position Effective Start Date")]
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


        [Display(Name = "Position Effective End Date")]
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



        //[Display(Name = "Hierarchy Effective Start Date")]
        //[Required]
        //[DataType(DataType.Date)]
        //[DateRange]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = AnnotationDeclaration.DateFormat)]
        //public DateTime? HierarchyEffectiveFromDate { get; set; }



        //[Display(Name = "Hierarchy Effective End Date")]
        //[DataType(DataType.Date)]
        //[DateRange]
        //[DateCompare("HierarchyEffectiveFromDate")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = AnnotationDeclaration.DateFormat)]
        //public DateTime? HierarchyEffectiveToDate { get; set; }


    }
}

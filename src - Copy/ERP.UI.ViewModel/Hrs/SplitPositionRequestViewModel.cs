using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class SplitPositionRequestViewModel : BaseRequestViewModel
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Split From")]
        public int? PositionId { get; set; }

        [Display(Name = "Split To Positions")]
        [Required(ErrorMessage = "Atleast select one position to split to")]
        public string SplitToPositions { get; set; }
        public List<IdNameViewModel> SplitPositionList { get; set; }


        public int? GRPPositionId { get; set; }
        public int? ManpowerOrganizationRootId { get; set; }

        [Display(Name = "Position Name")]
        public string PositionName { get; set; }
        public int? ParentPositionId { get; set; }


        [Display(Name = "Position Title")]
        public string PositionTitle { get; set; }

        [Display(Name = "Position Title (Arabic)")]
        public string PositionTitleAr { get; set; }

        [Display(Name = "Position Description")]
        public string Description { get; set; }
        public string Remarks { get; set; }


        [Display(Name = "Parent Position")]
        public string ParentPositionName { get; set; }

        [Display(Name = "Position Name")]
        public virtual string PositionNameWithTitle { get; set; }


        [Display(Name = "Parent Employee Name")]
        public string ParentEmployeeFullName { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeFullName { get; set; }

        [Display(Name = "Group Name")]
        public int? GroupId { get; set; }

        [Display(Name = "Department Name")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Division Name")]
        public int? DivisionId { get; set; }

        [Display(Name = "Section Name")]
        public int? SectionId { get; set; }

        [Display(Name = "Unit Name")]
        public int? UnitId { get; set; }

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

        public TransactionMode Mode { get; set; }

        [Display(Name = "Organization Name")]
        public int? PositionOrganizationId { get; set; }


        public int? OrgHierarchyNameId { get; set; }

        [Display(Name = "Organization Name")]
        public string PositionOrganizationFullName { get; set; }

        public int? JobDescriptionId { get; set; }

        [Display(Name = "Job Description(JD)")]
        public string JobDescriptionName { get; set; }





        [Display(Name = "Position Occupancy Status")]
        public string PositionHiringStatusName { get; set; }

        public bool? NeedDummyParent { get; set; }



        public int? HierarchyNameId { get; set; }
        public DateTime AsOnDate { get; set; }
        [Display(Name = "Split Effective Start Date")]
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


    }
}

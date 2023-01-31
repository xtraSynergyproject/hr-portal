using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class FlowchartViewModel : DataModelBase
    {
        public virtual long HierarchyId { get; set; }
        public virtual long PosHierarchyId { get; set; }

        public virtual string ParentId { get; set; }

        public virtual long? UserId { get; set; }
        public virtual long? PersonId { get; set; }
        public virtual long EmployeeCount { get; set; }

        public virtual string EmployeeNo { get; set; }
        public virtual string IqamahNo { get; set; }

        public virtual string DisplayName { get; set; }
        public virtual string UserName { get; set; }
        [Display(Name = "Department Name")]
        public virtual long? OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public virtual string OrganizationName { get; set; }
        public virtual long? JobId { get; set; }
        public virtual string JobName { get; set; }
        public virtual bool IsReplacementRequested { get; set; }
        public virtual long? ParentJobId { get; set; }


        public virtual long? GradeId { get; set; }
        public virtual string GradeName { get; set; }

        public virtual long? NationalityGroupId { get; set; }
        public virtual bool? IsNational { get; set; }
        public virtual long Level { get; set; }

        public virtual string ReportingLine { get; set; }
        [Display(Name = "Department Reporting Line")]
        public virtual string OrganizationReportingLine { get; set; }

        public virtual bool? NeedDummyParent { get; set; }

        public virtual long DirectChildCount { get; set; }
        public virtual long AllChildCount { get; set; }


        public virtual long? PositionTypeId { get; set; }
        public virtual long? PositionHiringStatusId { get; set; }
        public virtual long? EmployeeTypeId { get; set; }
        public virtual long? EmployeeStatusId { get; set; }
        public virtual string PositionBudgetTypeCode { get; set; }

        public virtual long? PhotoVersion { get; set; }
        public virtual long? PhotoId { get; set; }

        public virtual long? SequenceNo { get; set; }
        public virtual string NodeTypeCode { get; set; }

        public virtual bool? IsExpanded { get; set; }

        public virtual bool? Collapsed { get; set; }


        public virtual string TransactionType { get; set; }
        public virtual string TransactionStatus { get; set; }
        public virtual int NoOfPositions { get; set; }
        public virtual long? JobDescriptionId { get; set; }
        public virtual long PositionId { get; set; }
        public virtual string PositionName { get; set; }
        public virtual string PositionTitle { get; set; }
        public virtual string ParentPositionName { get; set; }
        public virtual string ParentEmployeeNo { get; set; }

        public virtual string ParentDisplayName { get; set; }
        public virtual string LocationName { get; set; }

        public virtual long? AssignmentId { get; set; }
        public virtual bool HasUser
        {
            get { return PersonId != null && PersonId > 0; }
        }
        public virtual string CssClass
        {
            get
            {
                return HasUser ? (IsNational.HasValue && IsNational.Value ? "org-node-2" : "org-node-1") : "org-node-3";
            }
        }

        public virtual long HierarchyRootNodeId { get; set; }
        public virtual long AllowedRootNodeId { get; set; }
        public virtual bool CanAddRootNode { get; set; }
        public virtual long AllowedRootNodeLevel { get; set; }

        public virtual string AsOnDate { get; set; }

        [Display(Name = "Is Succession Plan Enabled")]
        public virtual long SpsCount { get; set; }

        [Display(Name = "Is Succession Plan Enabled")]
        public string SpsEnabled { get { return SpsCount > 0 ? "Y" : "N"; } }

        public string NationalityName { get; set; }

        public string ReligionName { get; set; }
        public string BirthCountryName { get; set; }
        [Display(Name = "Gender")]
        public string GenderName { get; set; }
        public string TitlesName { get; set; }
        [Display(Name = "Marital Status")]
        public string MaritalStatusName { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        [Display(Name = "Cost Center Code")]
        public string CostCenterCode { get; set; }
        [Display(Name = "Department Type Name")]
        public string OrganizationTypeName { get; set; }
        [Display(Name = "Department Admin")]
        public string OrganizationAdmin { get; set; }

        public string SpsStep1Status { get; set; }
        public string SpsStep2Status { get; set; }
        public string SpsStep3Status { get; set; }

        public string SpsId { get; set; }

    }
}

﻿using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System;
using System.Collections.Generic;

namespace ERP.UI.ViewModel
{
    public class PositionMergeViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Hierarchy Name")]
        public int? HierarchyNameId { get; set; }

        [Display(Name = "Hierarchy Name")]
        public string HierarchyName { get; set; }

        [Display(Name = "Position")]
        public int? PositionId { get; set; }

        public int? PositionHierarchyId { get; set; }

        [Display(Name = "Direct Reportees")]
        public string DirectReportees { get; set; }

        [Display(Name = "Position")]
        public string PositionName { get; set; }

        [Display(Name = "Parent Position Name")]
        public int? ParentPositionId { get; set; }

        [Display(Name = "  Parent Position Name")]
        public string ParentPositionName { get; set; }

        [Required]
        [Display(Name = "Merge Position 1")]
        public int? MergeFromPositionId { get; set; }

        [Display(Name = "Merge Position 1")]
        public string MergeFromPositionName { get; set; }

        [Required]
        [Display(Name = "Merge Position To")]
        public int? MergeToPositionId { get; set; }

        [Display(Name = "Merge Position To")]
        public string MergeToPositionName { get; set; }


        public string Remarks { get; set; }
        public DateTime AsOnDate { get; set; }

        public int Level { get; set; }
        public TransactionMode Mode { get; set; }
        public bool EnableFromDate { get; set; }
        public bool IsSuccess { get; set; }
        public GridSelectOption HistoryGridSelectOption { get; set; }
        public bool PositionRemoveReasonCode { get; set; }
        [Display(Name = "Position Name")]
        public virtual string PositionNameWithTitle { get; set; }
    
        [Display(Name = "Employee Name")]
        public string EmployeeFullName { get; set; }

        [Display(Name = "Parent Employee Name")]
        public string ParentEmployeeFullName { get; set; }

        [Display(Name = "Parent Employee Name")]
        public int ParentEmployeeId { get; set; }

        [Display(Name = "Propsed Parent Employee Name")]
        public string ProposedParentEmployeeFullName { get; set; }

        [Display(Name = "Propsed Parent Employee Name")]
        public int ProposedParentEmployeeId { get; set; }

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
        [Display(Name = "Position Effective To Date")]
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
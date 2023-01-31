using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class PositionViewModel : DatedViewModelBase
    {

        public long PositionId { get; set; }
        [Required]
        [Display(Name = "Position Name")]
        public string Name { get; set; }
       
        [Display(Name = "Parent Position Name")]
        public string ParentPositionName { get; set; }

        [Display(Name = "Position Title")]
        public string PositionTitle { get; set; }

        [Display(Name = "Position Title (Arabic)")]
        public string PositionTitleAr { get; set; }

        public string Description { get; set; }
        public string Remarks { get; set; }
        [Display(Name = "Serial No")]
        public long SerialNo { get; set; }

        [Required]
        [Display(Name = "Job Name")]
        public long? JobId { get; set; }

        [Display(Name = "Job Name")]
        public string JobName { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }

        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }

        public long? JobDescriptionId { get; set; }

        [Display(Name = "Job Description(JD)")]
        public string JobDescriptionName { get; set; }

       
        [Display(Name = "Position Type")]
        public long? PositionTypeId { get; set; }
 
        [Display(Name = "Position Type")]
        public string PositionTypeCode { get; set; }

        [Display(Name = "Position Type")]
        public string PositionTypeName { get; set; }


        [Display(Name = "Position Status")]
        public long? PositionStatusId { get; set; }

        [Display(Name = "Position Status")]
        public string PositionStatusName { get; set; }


        [Display(Name = "Position Hiring Status")]
        public long? PositionHiringStatusId { get; set; }
        [Display(Name = "Position Hiring Status Code")]
        public string PositionHiringStatusCode { get; set; }
        [Display(Name = "Position Hiring Status")]
        public string PositionHiringStatusName { get; set; }
        public bool? NeedDummyParent { get; set; }

        [Display(Name = "Parent Position Name")]
        public long? ParentPositionId { get; set; }
        public long? HierarchyId { get; set; }
        public string HierarchyName { get; set; }
        public Nullable<long> SequenceNo { get; set; }

        public long? AssignmentId { get; set; }

        [Display(Name = "NAV Section")]
        public long? NAVSectionId { get; set; }

        public long? RelationId { get; set; }

    }
}

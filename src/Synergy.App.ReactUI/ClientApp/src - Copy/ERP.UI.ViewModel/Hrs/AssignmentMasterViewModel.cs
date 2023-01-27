using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.ComponentModel;

namespace ERP.UI.ViewModel
{
    public class AssignmentMasterViewModel : DatedViewModelBase
    {
       
        public long AssignmentMasterId { get; set; }

        [Required]
        [Display(Name = "Employee Name")]
        //public int? PersonId { get; set; }
        public bool IsPrimaryAssignment { get; set; }
        [Display(Name = "Date Of Join")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfJoin { get; set; }
        [Display(Name = "Probation End Date")]
        public Nullable<DateTime> ProbationEndDate { get; set; }
        public Nullable<DateTime> NoticeStartDate { get; set; }
        public Nullable<DateTime> ActualTerminationDate { get; set; }

    }
}

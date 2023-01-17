using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class UserSearchViewModel : SearchViewModelBase
    {

        public long UserId { get; set; }

        [StringLength(200)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public long? GradeId { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        public long? JobId { get; set; }
        public long? PositionId { get; set; }
        public long? PersonId { get; set; }

        public GridSelectOption SelectOption { get; set; }
        public SearchType SearchType { get; set; }

        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string JobName { get; set; }
        public string GradeName { get; set; }
        public string PositionName { get; set; }
        public string EmployeeName { get; set; }
        public string IqamahNo { get; set; }

        public bool IsEmpMap { get; set; }
        public long TeamUserId { get; set; }
        public long TeamMemberId { get; set; }

        //public long TeamUserId { get; set; }

        public string Type { get; set; }

        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime ToDate { get; set; }
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime FromDate { get; set; }

    }
}

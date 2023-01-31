using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class ManpowerLeaveSummaryViewModel
    {
        [Display(Name = "Person Name")]
        public string PersonId { get; set; }

        [Display(Name = "Person Name")]
        public string PersonName { get; set; }
        [Display(Name = "Iqamah No")]
        public string SponsorshipNo { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string JobName { get; set; }
        public string Relationship { get; set; }
        public string RelativeFullName { get; set; }
        public string Gender { get; set; }
        public string CalendarId { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime DateOfBirth { get; set; }
       // [DataType(DataType.Date)]
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ProbationEndDate { get; set; }
       // [DataType(DataType.Date)]
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime DateOfJoin { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }

       // [DataType(DataType.Date)]
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        public string LeaveDuration { get; set; }

        public double LeaveBalance { get; set; }
        public string Description { get; set; }
        public string PersonNo { get; set; }

        public string ServiceId { get; set; }

        [Display(Name = "Annual Leave")]
        public double AnnualLeaveDays { get; set; }
        public double SickLeaveDays { get; set; }
        [Display(Name = "Unpaid Leave")]
        public double UnpaidLeaveDays { get; set; }
        [Display(Name = "Other Leave")]
        public double OtherLeaveDays { get; set; }

        [Display(Name = "Total Leave")]
        public double TotalLeaveDays
        {
            get
            {
                return AnnualLeaveDays + SickLeaveDays + UnpaidLeaveDays/* + OtherLeaveDays*/;

            }
        }
        [Display(Name = "Total Working Days")]
        public double TotalWorkingDays { get; set; }
       // [Display(Name = "Total Working Hours")]
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? TotalWorkingHours { get; set; }
        [Display(Name = "Employee Total Working Days")]
        public double EmployeeWorkingDays { get; set; }
        //[Display(Name = "Employee Total Working Hours")]
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? EmployeeWorkingHours { get; set; }
    }
}

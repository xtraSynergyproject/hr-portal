using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class DMSViewModel
    {
        
        public string category { get; set; }
        public long WorkspaceId { get; set; }
        public string WorkspaceName { get; set; }
        public long DocumentTypeId { get; set; }
        public long DocumentTemplateId { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceName { get; set; }
        public long? ServiceId { get; set; }
        public string ServiceStatus { get; set; }
        public string ServiceStatusCode { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? FromDate { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ToDate { get; set; }
        [Display(Name = "Completed")]
        public long? Completed { get; set; }
        [Display(Name = "Active")]
        public long? InProgress { get; set; }
        [Display(Name = "Restarted")]
        public long? Draft { get; set; }
        [Display(Name = "OverDue")]
        public long? OverDue { get; set; }
        [Display(Name = "Cancelled")]
        public long? Cancelled { get; set; }
        public long? Total { get; set; }
        [Display(Name = "Status")]
        public double FilterStatusCount { get; set; }

        //public int Status { get; set; }

        public string[] StepList { get; set; }

        public string Step1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepStart1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepEnd1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepComplete1 { get; set; }
        public string StepStatus1 { get; set; }

        public string Step2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepStart2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepEnd2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepComplete2 { get; set; }
        public string StepStatus2 { get; set; }

        public string Step3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepStart3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepEnd3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepComplete3 { get; set; }
        public string StepStatus3 { get; set; }

        public string Step4 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepStart4 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepEnd4 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepComplete4 { get; set; }
        public string StepStatus4 { get; set; }

        public string Step5 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepStart5 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepEnd5 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepComplete5 { get; set; }
        public string StepStatus5 { get; set; }

        public string Step6 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepStart6 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepEnd6 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepComplete6 { get; set; }
        public string StepStatus6 { get; set; }

        public string Step7 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepStart7 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepEnd7 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepComplete7 { get; set; }
        public string StepStatus7 { get; set; }

        public string Step8 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepStart8 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepEnd8 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepComplete8 { get; set; }
        public string StepStatus8 { get; set; }

        public string Step9 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepStart9 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepEnd9 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepComplete9 { get; set; }
        public string StepStatus9 { get; set; }

        public string Step10 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepStart10 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepEnd10 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StepComplete10 { get; set; }
        public string StepStatus10 { get; set; }


    }
}

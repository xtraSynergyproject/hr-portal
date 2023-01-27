using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class NtsExcelViewModel
    {
        //public long Id { get; set; }
        //[Display(Name = "Reference Id")]
        //public long ChecklistReference { get; set; }
        //[Display(Name = "Pre Opening Checklist")]
        public string Checklist { get; set; }
        [Display(Name = "ChecklistStartDate", ResourceType = typeof(ERP.Translation.Nts.Report))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime ChecklistStartDate { get; set; }
        [Display(Name = "ChecklistEndDate", ResourceType = typeof(ERP.Translation.Nts.Report))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime ChecklistEndDate { get; set; }
        [Display(Name = "ChecklistStatus", ResourceType = typeof(ERP.Translation.Nts.Report))]
        public string ChecklistStatus { get; set; }

        [Display(Name = "TaskTitle", ResourceType = typeof(ERP.Translation.Nts.Report))]
        public string TaskTitle { get; set; }
        [Display(Name = "TaskStartDate", ResourceType = typeof(ERP.Translation.Nts.Report))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime TaskStartDate { get; set; }
        [Display(Name = "TaskEndDate", ResourceType = typeof(ERP.Translation.Nts.Report))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]       
        public DateTime TaskEndDate { get; set; }
        [Display(Name = "TaskStatus", ResourceType = typeof(ERP.Translation.Nts.Report))]
        public string TaskStatus { get; set; }

        public string AssignTo { get; set; }
        //public string Url { get; set; }



    }
}

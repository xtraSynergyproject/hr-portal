using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;
using ERP.Data.GraphModel;

namespace ERP.UI.ViewModel
{
    public class TaskSearchViewModel : SearchViewModelBase
    {
        //[Display(Name = "Task No")] 
        [Display(Name = "TaskNo", ResourceType = typeof(ERP.Translation.Nts.TaskSearch))]
        public string TaskNo { get; set; }
        //[Display(Name = "Subject")]
        [Display(Name = "Subject", ResourceType = typeof(ERP.Translation.Nts.TaskSearch))]
        public string Subject { get; set; }
        //[Display(Name = "Start Date")]
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Nts.TaskSearch))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        //[Display(Name = "Due Date")]
        [Display(Name = "DueDate", ResourceType = typeof(ERP.Translation.Nts.TaskSearch))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DueDate { get; set; }
        //[Display(Name = "Assigned To Type")]
        [Display(Name = "AssignedToType", ResourceType = typeof(ERP.Translation.Nts.TaskSearch))]
        public GEN_ListOfValue AssignedToType { get; set; }
        //[Display(Name = "Creation Date")]
        [Display(Name = "CreationDate", ResourceType = typeof(ERP.Translation.Nts.TaskSearch))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? CreationDate { get; set; }
        //[Display(Name = "Completion Date")]
        [Display(Name = "CompletionDate", ResourceType = typeof(ERP.Translation.Nts.TaskSearch))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? CompletionDate { get; set; }
        //[Display(Name = "Task Status")]
        [Display(Name = "TaskStatus", ResourceType = typeof(ERP.Translation.Nts.TaskSearch))]
        public string TaskStatus { get; set; }

        public string Mode { get; set; }
        public string Text { get; set; }
        public long? AssignTo { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public long? UserId { get; set; }

        public string UserRole { get; set; }
        public long? ServiceId { get; set; }
        public string TemplateMasterCode { get; set; }
        public string Description { get; set; }

        public string RequestSource { get; set; }
        public string Layout { get; set; }
        public string ReturnUrl { get; set; }
        public long? TemplateMasterId { get; set; }
        public NtsTypeEnum? NTSType { get; set; }
        public long[] Userfilter { get; set; }
        public long? Projectfilter { get; set; }
        public int Days { get; set; }
        public int ChartFilter { get; set; }
        public int PieChartFilter { get; set; }
        public string Period { get; set; }
        public string Taskfilter { get; set; }
        public DateTime? Datefilter { get; set; }
        public DateTime[] Datefilter1 { get; set; }
        public List<long> UserfilterCVR { get; set; }

    }
}

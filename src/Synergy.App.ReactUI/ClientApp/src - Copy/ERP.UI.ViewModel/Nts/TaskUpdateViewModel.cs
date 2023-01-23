using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class TaskUpdateViewModel : ViewModelBase
    {        
        public long? TaskId { get; set; }

        
        [Display(Name = "AssignedTo", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public long? AssignedTo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
       
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Nts.Task))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }

      
        [Display(Name = "DueDate", ResourceType = typeof(ERP.Translation.Nts.Task))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DueDate { get; set; }

      
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
            
        [Display(Name = "SLA", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public TimeSpan? SLA { get; set; }
      

        public List<long> taskList { get; set; }
        public string tasks { get; set; }

    }

}



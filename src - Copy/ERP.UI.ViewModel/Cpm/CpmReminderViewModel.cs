using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.Cpm
{
    public class CpmReminderViewModel : ViewModelBase
    {

        public string Title { get; set; }

        [Display(Name = "To Do")]
        public string ToDo { get; set; }

        public long? AssignTo { get; set; }

        [Display(Name = "Assign To")]
        public string AssignToName { get; set; }

        public CpmPriorityEnum? Priority { get; set; }

        [Display(Name = "Reminder Date Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ReminderDateTime { get; set; }



    }
}

using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.Help
{
    public class HelpDeskReportViewModel
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public long? TeamId { get; set; }

        public string Team { get; set; }
        public string TeamName1 { get; set; }


        [Display(Name = "TaskNo", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string TaskNo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string OwnerName { get; set; }
        public string AssigneeDisplayName { get; set; }
        public string TaskStatusName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string OrganizationName { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        public long? AssignedTo { get; set; }
        public string AssignedByJobTitle { get; set; }
        public string AssignedToJobTitle { get; set; }
        public string AssignedToName { get; set; }

        public string Rating { get; set; }
        public string AssigneeTeam { get; set; }

    }
}

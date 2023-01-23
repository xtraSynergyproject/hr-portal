using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.Help
{
    public class UserSupportViewModel : ViewModelBase
    {
        //public int id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int InProgressCount { get; set; }

        public int OverDueCount { get; set; }

        public int CompletedCount { get; set; }
        public long CancelTicketCount { get; set; }
        public long DraftTicketCount { get; set; }

        public long TotalTicketCount { get; set; }

        public long TotalNotificationCount { get; set; }

        public int NotificationReadCount { get; set; }

        public int NotificationUnReadCount { get; set; }
        public string TaskStatusCode { get; set; }


        public long? UserId { get; set; }
        public string TeamId { get; set; }
        public long? TeamSupportCategoryId { get; set; }
        public string Mode { get; set; }


        [Display(Name = "TaskNo", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string TaskNo { get; set; }
        public string Subject { get; set; }
        // public string Description { get; set; }
        public string OwnerName { get; set; }
        public string AssigneeDisplayName { get; set; }
        public string TaskStatusName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string OrganizationName { get; set; }
        public string JobName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        public long? AssignedTo { get; set; }

        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string SLA { get; set; }
        public string TaskStatus { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public long? RestaurantId { get; set; }
        public string Category { get; set; }
        public string MasterCategoryType { get; set; }

        public string Type { get; set; }
        public long? OwnerTeamId { get; set; }
        [Display(Name = "Owner Team Name")]
        public string OwnerTeamName { get; set; }
        public string AssignedToTeamName { get; set; }
        public string Imagecontent { get; set; }
        public string AssignTeam { get; set; }
    }
}

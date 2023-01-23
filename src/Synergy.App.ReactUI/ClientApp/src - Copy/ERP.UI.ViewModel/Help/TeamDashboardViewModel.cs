﻿using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.Help
{
    public class TeamDashboardViewModel : ViewModelBase
    {
        //public int id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TotalProgressCount { get; set; }

        public int TotalOverDueCount { get; set; }

        public int TotalCompletedCount { get; set; }

        public long TotalTicketCount { get; set; }
        public long? ParentId { get; set; }
        public int Level { get; set; }

        public string Content { get; set; }
        public long? DiretctChildCount { get; set; }




        public long TotalCancelTicketCount { get; set; }
        public long TotalDraftTicketCount { get; set; }

        public string TaskStatusCode { get; set; }
        //  public long? TemplateId { get; set; }
        //  public long? TemplateCategoryId { get; set; }

        public ModuleEnum ModuleName { get; set; }
        // public IList<TicketTypeViewModel> Tickets { get; set; }
        public IList<ParentchildViewModel> Tickets { get; set; }

        public long TeamId { get; set; }





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
        //public ModuleEnum? ModuleName { get; set; }

        public string AssignToTeamName { get; set; }


        public string Mode { get; set; }
        public long? UserId { get; set; }
        //  public string TeamId { get; set; }
        public long? OwnerTeamId { get; set; }
        public long? TeamSupportCategoryId { get; set; }
        public string Type { get; set; }
        public long? RestaurantId { get; set; }
        public long? TemplateCategoryId { get; set; }
        public int TotalNotificationCount { get; set; }
        public int TotalNotificationReadCount { get; set; }
        public int TotalNotificationUnReadCount { get; set; }
        public long? TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string OwnerTeamName { get; set; }
    }

    public class ParentchildViewModel : ViewModelBase
    {
        //public int id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TotalProgressCount { get; set; }

        public int TotalOverDueCount { get; set; }

        public int TotalCompletedCount { get; set; }

        public long TotalTicketCount { get; set; }
        public long TotalCancelTicketCount { get; set; }
        public long TotalDraftTicketCount { get; set; }

        public long TotalNotificationCount { get; set; }

        public int TotalNotificationReadCount { get; set; }
        public long? TemplateId { get; set; }
        public int TotalNotificationUnReadCount { get; set; }
        public string TaskStatusCode { get; set; }

        public long? TemplateCategoryId { get; set; }
        public long TeamId { get; set; }

    }


}

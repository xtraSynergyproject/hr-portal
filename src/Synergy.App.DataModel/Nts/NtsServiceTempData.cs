using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsServiceTempData : DataModelBase
    {
        public string ServiceNo { get; set; }
        public string ServiceSubject { get; set; }
        public string ServiceDescription { get; set; }
        public string TemplateCode { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        //public string SLA { get; set; }

        public TimeSpan ServiceSLA { get; set; }
        public TimeSpan ActualSLA { get; set; }
        public DateTime? ReminderDate { get; set; }

        public string ServiceStatusId { get; set; }

        public string ServicePriorityId { get; set; }


        public string RequestedByUserId { get; set; }

        public string OwnerUserId { get; set; }
         

        public string ParentNoteId { get; set; }
        public string ParentTaskId { get; set; }
        public string ParentServiceId { get; set; }
        public bool IsVersioning { get; set; }
        public string ReferenceId { get; set; }
        public ReferenceTypeEnum ReferenceType { get; set; } 

        public string ServicePlusId { get; set; }
        public string NotePlusId { get; set; }
        public string TaskPlusId { get; set; }
        public string WorkflowStatus { get; set; }
        public bool IsReopened { get; set; }
        public string NextStepTaskComponentId { get; set; }
        public string UdfData { get; set; }
    }
    
}

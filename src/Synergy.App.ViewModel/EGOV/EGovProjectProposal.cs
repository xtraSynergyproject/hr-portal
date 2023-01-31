using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{

    public class EGovProjectProposal
    {
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectSource { get; set; }
        public string ProjectTypeCode { get; set; }
        public string ProjectWard { get; set; }
        public long Like { get; set; }
        public long Comment { get; set; }
        public long Attachment { get; set; }
        public string Image { get; set; } 

        public string ProjectCategory { get; set; }
        public string ProjectSubCategory { get; set; }
        public string ServiceNo { get; set; }
        public string WorkflowStatus { get; set; }
        public string TaskNo { get; set; }
        public string TaskSubject { get; set; }
        public string TaskStatusName { get; set; }
        public string ServiceOwner { get; set; }
        public string AssigneeUserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Id { get; set; }
        public string TemplateCode { get; set; }
        public string TaskActionId { get; set; }
        public string TemplateMasterCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ProjectStatusName { get; set; }

    }
}

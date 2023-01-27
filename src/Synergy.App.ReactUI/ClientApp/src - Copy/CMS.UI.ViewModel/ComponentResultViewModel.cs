using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class ComponentResultViewModel : ComponentResult
    {
        public string ComponentStatusName { get; set; }
        public string TaskNo { get; set; }
        public string ServiceSubject { get; set; }
        public string TemplateMasterCode { get; set; }
        public string TaskId { get; set; }
        public string Assignee { get; set; }
        public string AssigneeId { get; set; }
        public string AssigneePhotoId { get; set; }
        public string Email { get; set; }
        public string ComponentStatusCode { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
    }
}

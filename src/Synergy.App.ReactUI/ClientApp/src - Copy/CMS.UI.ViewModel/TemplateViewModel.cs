using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class TemplateViewModel : Template
    {
        public string TemplateCategoryName { get; set; }
        public string ImportFileId { get; set; }
        public TaskTypeEnum TaskType { get; set; }
        public string CategoryCode { get; set; }
        public string ModuleCodes { get; set; }
        public string IconFileId { get; set; }      
        public string TemplateColor { get; set; }
        public string UserId { get; set; }      
        public BusinessLogicExecutionTypeEnum? Type { get; set; }
        public string Prms { get; set; }
        public string CallBackMethodName { get; set; }
        public string TemplateIds { get; set; }
        public string CategoryIds { get; set; }
        public TemplateCategoryTypeEnum TemplateCategoryType { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set;}
        public string AreaName { get; set; }
        public string Parameter { get; set; }
        public bool Select { get; set; }
        public bool AllBooks { get; set; }
        public string PortalNames { get; set; }
        public ServiceTypeEnum ServiceType { get; set; }
            public string CustomIcon { get; set; }
    }
}

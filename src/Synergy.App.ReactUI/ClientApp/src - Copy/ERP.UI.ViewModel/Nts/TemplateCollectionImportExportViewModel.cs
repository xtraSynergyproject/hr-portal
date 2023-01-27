using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class TemplateCollectionImportExportViewModel
    {
        public List<TemplateImportExportViewModel> Templates { get; set; }
    }
    [Serializable]
    public class TemplateImportExportViewModel
    {

        public TemplateCategoryViewModel TemplateCategory { get; set; }
        public TemplateMasterViewModel TemplateMaster { get; set; }
        public TemplateViewModel Template { get; set; }
        public List<TemplateFieldViewModel> TemplateFields { get; set; }
        public List<ServiceTaskTemplateViewModel> ServiceStepTaskTemplates { get; set; }
        public List<ServiceTaskTemplateViewModel> ServiceAdhocTaskTemplates { get; set; }
        public List<ServicePlusServiceTemplateViewModel> ServicePlusServiceTemplates { get; set; }
        public List<NotificationTemplateViewModel> NotificationTemplates { get; set; }
    }
}

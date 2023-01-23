﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
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
        public string ControllerName { get; set; }
        public string AreaName { get; set; }
        public string Parameter { get; set; }
        public bool Select { get; set; }
        public bool AllBooks { get; set; }
        public string PortalNames { get; set; }
        public ServiceTypeEnum ServiceType { get; set; }
        public string CustomIcon { get; set; }

        public List<TemplateViewModel> TemplatesList { get; set; }
        public List<IdNameViewModel> ModuleList { get; set; }
        public string PageName { get; set; }
        public bool SkipIntro { get; set; }
        public string DataJson { get; set; }

        public string TemplateCodes { get; set; }
        public string CategoryCodes { get; set; }
        public string GroupCodes { get; set; }
        public string TemplateId { get; set; }
        public bool ShowAllServicesForAdmin { get; set; }

        public bool IsChildTable { get; set; }
        public string StepTaskJson { get; set; }
        public string[] Templates { get; set; }
        public string ClassName { get; set; }
        public List<EGOVBannerViewModel> Notifications { get; set; }
        public List<EGOVBannerViewModel> Tenders { get; set; }
        public List<EGOVBannerViewModel> OrderCirculars { get; set; }
        public string SelectedCategoryCode { get; set; }
        public string SelectedCategoryName { get; set; }
    }
}

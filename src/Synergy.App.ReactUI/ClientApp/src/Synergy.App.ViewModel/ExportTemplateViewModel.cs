using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class ExportTemplateViewModel 
    {
      
        public TemplateCategoryViewModel TemplateCategory;
        public TemplateViewModel Template;
        public ServiceTemplateViewModel ServiceTemplate;
        public NoteTemplateViewModel NoteTemplate;
        public TaskTemplateViewModel TaskTemplate;
        public FormTemplateViewModel FormTemplate;
        public PageTemplateViewModel PageTemplate;
        public CustomTemplateViewModel CustomTemplate;
    }

    public class CopyTemplateViewModel
    {
        public TemplateCategoryViewModel TemplateCategory;
        public TemplateViewModel Template;
        public ServiceTemplateViewModel ServiceTemplate;
        public NoteTemplateViewModel NoteTemplate;
        public TaskTemplateViewModel TaskTemplate;
        public FormTemplateViewModel FormTemplate;
        public PageTemplateViewModel PageTemplate;
        public CustomTemplateViewModel CustomTemplate;
        public ServiceIndexPageTemplateViewModel ServiceIndexPageTemplate;
        public NoteIndexPageTemplateViewModel NoteIndexPageTemplate;
        public TaskIndexPageTemplateViewModel TaskIndexPageTemplate;
        public FormIndexPageTemplateViewModel FormIndexPageTemplate;
        public List<ResourceLanguageViewModel> ResourceLanguage;

        public ProcessDesignViewModel ProcessDesign;
        public List<ComponentViewModel> Component;
        public List<DecisionScriptComponentViewModel> DecisionScriptComponent;
        public List<BusinessRuleModelViewModel> BusinessRuleModelForDecisionScript;
        public List<TemplateViewModel> StepTaskTemplate;

        public List<CopyTemplateViewModel> StepTaskDetails;

        public List<StepTaskComponentViewModel> StepTaskComponent;
        public List<StepTaskSkipLogicViewModel> StepTaskSkipLogic;
        public List<BusinessRuleModelViewModel> BusinessRuleModelForSkipLogic;
        public List<StepTaskAssigneeLogicViewModel> StepTaskAssigneeLogic;
        public List<BusinessRuleModelViewModel> BusinessRuleModelForAssignee;
        public List<BusinessRuleViewModel> BusinessRule;
        public List<BusinessRuleNodeViewModel> BusinessNode;
        public List<BreResultViewModel> BreResult;
        public List<BusinessRuleConnectorViewModel> BusinessRuleConnector;
        public List<BusinessRuleModelViewModel> BusinessRuleModelForBusinessDecision;
        public List<FormIndexPageColumnViewModel> FormIndexPageColumn;
        public List<ServiceIndexPageColumnViewModel> ServiceIndexPageColumn;
        public List<NoteIndexPageColumnViewModel> NoteIndexPageColumn;
        public List<TaskIndexPageColumnViewModel> TaskIndexPageColumn;

        public List<ColumnMetadataViewModel> ColumnMetadata;
        public List<UdfPermissionViewModel> UdfPermission;
    }


}

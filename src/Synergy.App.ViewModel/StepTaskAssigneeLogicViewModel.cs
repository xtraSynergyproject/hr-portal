using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class StepTaskAssigneeLogicViewModel : StepTaskAssigneeLogic
    {
       public string Assignee { get; set; }
        public string AssignedToType { get; set; }
        public string ParentId { get; set; }
        public string TemplateId { get; set; }
    }
}

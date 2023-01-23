using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class StepTaskSkipLogicViewModel : StepTaskSkipLogic
    {
        public string ParentId { get; set; }
        public string TemplateId { get; set; }
    }
}

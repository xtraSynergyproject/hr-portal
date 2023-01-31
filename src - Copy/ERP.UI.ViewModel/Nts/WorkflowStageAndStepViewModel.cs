using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class WorkflowStageAndStepViewModel : ViewModelBase
    {       
        public string Name { get; set; }
        public string Description { get; set; }
        public WorkflowStageStepTypeEnum Type { get; set; }

    }
}

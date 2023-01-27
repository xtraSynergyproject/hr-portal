using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class RuntimeWorkflowDataViewModel : RuntimeWorkflowData
    {
        public string AssignedToTypeName { get; set; }
        public string TeamAssignmentTypeName { get; set; }
        public string AssignedToTeamName { get; set; }
        public string AssignedToUserName { get; set; }
        public string AssignedToHierarchyMasterName { get; set; }
        public string AssignedToHierarchyMasterLevelName { get; set; }
        public string AssignedToTypeCode { get; set; }
    }   
}

using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class StepTaskComponentViewModel : StepTaskComponent
    {
        public string blockid { get; set; }
        public string blockprop { get; set; }
        public string AssignedToTeamUserId { get; set; }
        public string AssignedToTypeName { get; set; }
        public double? SLASeconds { get; set; }
        public string NoteUDFs { get; set; }
        public string AssignedToTypeCode { get; set; }
        public string ParentId { get; set; }
        public string ProcessDesignId { get; set; }
        public bool IsDiagram { get; set; }
        public string ServiceTemplateId { get; set; }
        public string ModuleId { get; set; }
    }
}

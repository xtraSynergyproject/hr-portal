using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class StepTaskAssigneeLogic : DataModelBase
    {
        public string StepTaskComponentId { get; set; }
        public string Name { get; set; }
      
        public string ExecutionLogicDisplay { get; set; }
        public string ExecutionLogic { get; set; }
        public bool SuccessResult { get; set; }

        [ForeignKey("AssignedToType")]
        public string AssignedToTypeId { get; set; }
        public LOV AssignedToType { get; set; }

        [ForeignKey("AssignedToUser")]
        public string AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; }

        [ForeignKey("AssignedToTeam")]
        public string AssignedToTeamId { get; set; }
        public Team AssignedToTeam { get; set; }

        [ForeignKey("AssignedToHierarchyMaster")]
        public string AssignedToHierarchyMasterId { get; set; }
        public HierarchyMaster AssignedToHierarchyMaster { get; set; }

        public int AssignedToHierarchyMasterLevelId { get; set; }
        public WorkAssignmentTypeEnum TeamAssignmentType { get; set; }
    }
    [Table("StepTaskAssigneeLogicLog", Schema = "log")]
    public class StepTaskAssigneeLogicLog : StepTaskAssigneeLogic
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
        public DateTime LogEndDateTime { get; set; }
        public bool IsDatedLatest { get; set; }
        public bool IsVersionLatest { get; set; }

    }
}

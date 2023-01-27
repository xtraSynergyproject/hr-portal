using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCGrievanceWorkflow : ViewModelBase
    {
        public string DepartmentId { get; set; }
        public string WardId { get; set; }
        public string[] WardIds { get; set; }
        public string WorkflowLevelId { get; set; }
        public string Level1AssignedToTypeId { get; set; }
        public string Level2AssignedToTypeId { get; set; }
        public string Level3AssignedToTypeId { get; set; }
        public string Level4AssignedToTypeId { get; set; }
       
        public string Level1AssignedToTeamId { get; set; }
        public string Level2AssignedToTeamId { get; set; }
        public string Level3AssignedToTeamId { get; set; }
        public string Level4AssignedToTeamId { get; set; }
        public string Level1AssignedToTeamUserId { get; set; }
        public string Level2AssignedToTeamUserId { get; set; }
        public string Level3AssignedToTeamUserId { get; set; }
        public string Level4AssignedToTeamUserId { get; set; }
        public string Level1AssignedToUserId { get; set; }
        public string Level2AssignedToUserId { get; set; }
        public string Level3AssignedToUserId { get; set; }
        public string Level4AssignedToUserId { get; set; }

        public string DepartmentName { get; set; }
        public string WardName { get; set; }
        public string WorkflowLevel { get; set; }


    }
}

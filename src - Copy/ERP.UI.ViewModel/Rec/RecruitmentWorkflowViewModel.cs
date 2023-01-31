using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class RecruitmentWorkflowViewModel
    {
        public string JobRequestStage { get; set; }
        public long? JobRequestId { get; set; }

        public string JobAddStage { get; set; }
        public long? AddId { get; set; }

        public string JobApplicationStage { get; set; }
        public long? AppId { get; set; }

        public string ShortlistedStage { get; set; }
        public long? ShortListedId { get; set; }

        public string InterviewScheduleStage { get; set; }
        public long? InterviewScheduleId { get; set; }

        public string JobOfferedStage { get; set; }
        public long? JobOfferedId { get; set; }

        public string PreJoiningFormalityStage { get; set; }
        public long? PreJoiningFormailityId { get; set; }

        public string PreJoiningChecklistStage { get; set; }
        public long? PreJoiningChecklistId { get; set; }

        public string JoiningChecklistStage { get; set; }
        public long? JoiningChecklistId { get; set; }

        public string JoinedStage { get; set; }
        public long? JoinedId { get; set; }
        public ApplicationStateEnum? JobApplicationState { get; set; }
        public ApplicationStatusEnum? JobApplicationStatus { get; set; }
    }
}

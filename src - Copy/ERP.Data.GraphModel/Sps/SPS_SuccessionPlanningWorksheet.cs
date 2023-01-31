
using ERP.Utility;
using System;
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
    public partial class SPS_SuccessionPlanningWorksheet : NodeBase
    {
         public string Name { get; set; }

        public string Position { get; set; }

        public string StartDate { get; set; }

        public string InductionManual { get; set; }

        public string DateOfSPRewview { get; set; }

        public long PotentialNextRole { get; set; }

        public bool IsReadyForNextRole { get; set; }

        public long PotentialReplacement { get; set; }

        public string DevelopmentNeeds { get; set; }

        public string TraininigSessionAgreed { get; set; }

        public bool TrainerAssigned { get; set; }

        public bool TrainingCompleted { get; set; }

        public string Stage1 { get; set; }

        public string Stage2 { get; set; }

        public string Stage3 { get; set; }
    }
    public class R_SuccessionPlanningWorksheet_Service : RelationshipBase
    {

    }
    public class R_SuccessionPlanningWorksheet_Replacement_Job : RelationshipBase
    {

    }
    public class R_SuccessionPlanningWorksheet_Replacement_PersonRoot : RelationshipBase
    {

    }
    
}

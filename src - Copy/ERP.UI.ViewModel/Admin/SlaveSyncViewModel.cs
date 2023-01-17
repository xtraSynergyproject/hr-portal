using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class SlaveSyncViewModel :  ViewModelBase
    {
        public DateTime? PullCutOffDate { get; set; }
        public DateTime? PullCutOffDateTo { get; set; }
        public DateTime PushCutOffDate { get; set; }
        public bool IsPullSuccessful { get; set; }
        public bool IsPushSuccessful { get; set; }
        public SyncStageStatusEnum? StageStatus { get; set; }
        public string ErrorMessage { get; set; }
    }
}



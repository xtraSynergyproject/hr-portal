using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class NtsSummaryViewModel
    {

        public long InProgressCount { get; set; }
        public long OverDueCount { get; set; }
        public long DraftCount { get; set; }
        public long RejectedCount { get; set; }
        public long CanceledCount { get; set; }
        public long ClosedCount { get; set; }
        public long CompletedCount { get; set; }
        public long Pending
        {
            get
            {
                return InProgressCount + OverDueCount;
            }
        }
        public long Completed
        {
            get
            {
                return CompletedCount + ClosedCount;
            }
        }
        public long Rejected
        {
            get
            {
                return RejectedCount + CanceledCount;
            }
        }


    }
}

using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class WorklistDashboardSummaryViewModel 
    {
        public long S_RequestedByMe { get; set; }
        public long S_RequestOverdue { get; set; }
        public long S_RequestCompleted { get; set; }
        public long S_RequestDraft { get; set; }
        public long S_RequestPending { get; set; }
        public long S_RequestCancel { get; set; }
        public long S_SharedWithMe { get; set; }
        public long S_ShareWithPending { get; set; }
        public long S_ShareWithCompleted { get; set; }
        public long S_ShareWithOverdue { get; set; }
        public long S_ShareWithCancel { get; set; }

        public long T_RequestedByMe { get; set; }
        public long T_RequestOverdue { get; set; }
        public long T_RequestCompleted { get; set; }
        public long T_RequestDraft { get; set; }
        public long T_RequestPending { get; set; }
        public long T_AssignedToMe { get; set; }
        public long T_AssignPlanned { get; set; }
        public long T_AssignPlannedOverdue { get; set; }
        public long T_AssignPending { get; set; }
        public long T_AssignPendingOverdue { get; set; }
        public long T_AssignCompleted { get; set; }
        public long T_AssignOverdue { get; set; }
        public long T_AssignReject { get; set; }
        public long T_AssignCancel { get; set; }
        public long T_AssignAll { get; set; }
        public long T_SharedWithMe { get; set; }
        public long T_ShareWithPending { get; set; }
        public long T_ShareWithCompleted { get; set; }
        public long T_ShareWithOverdue { get; set; }
        public long T_ShareWithReject { get; set; }


        public long N_CreatedByMe { get; set; }
        public long N_CreatedByMeExpired { get; set; }
        public long N_CreatedByMeActive { get; set; }
        public long N_CreatedByMeDraft { get; set; }
        public long N_CreatedByMeAll { get; set; }
        public long N_CreatedByMeOverdue { get; set; }
        public long N_CreatedByMeCompleted { get; set; }
        public long ReadCount { get; set; }
        public long UnReadCount { get; set; }
        public long ArchivedCount { get; set; }
        public long AllCount { get; set; }

        public long PS_CreatedByMe { get; set; }
        public long PS_CreatedByMeExpired { get; set; }
        public long PS_CreatedByMeActive { get; set; }
        public long PS_CreatedByMeDraft { get; set; }
        public long PS_CreatedByMeAll { get; set; }
        public long PS_CreatedByMeOverdue { get; set; }
        public long PS_CreatedByMeCompleted { get; set; }
    }
    public class DashboardNoteViewModel
    {
        public long createdByMe { get; set; }
        public long createdByMeExpired { get; set; }
        public long createdByMeActive { get; set; }
        public long createdByMeDraft { get; set; }
        public long createdByMeOverdue { get; set; }
        public long createdByMeCompleted { get; set; }

        public long sharedByMe { get; set; }
        public long sharedByMeExpired { get; set; }
        public long sharedByMeActive { get; set; }
        public long sharedByMeDraft { get; set; }

        public long shareWithMe { get; set; }
        public long sharedWithMeExpired { get; set; }
        public long sharedWithMeActive { get; set; }
        public long sharedWithMeDraft { get; set; }


    }
}

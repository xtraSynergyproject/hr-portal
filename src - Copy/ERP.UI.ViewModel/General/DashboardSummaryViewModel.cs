using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class DashboardSummaryViewModel : ViewModelBase
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
        public long T_AssignPending { get; set; }
        public long T_AssignCompleted { get; set; }
        public long T_AssignOverdue { get; set; }
        public long T_AssignReject { get; set; }
        public long T_SharedWithMe { get; set; }
        public long T_ShareWithPending { get; set; }
        public long T_ShareWithCompleted { get; set; }
        public long T_ShareWithOverdue { get; set; }
        public long T_ShareWithReject { get; set; }
        public long AllNotification { get; set; }
        public long UnReadNotification { get; set; }
        public long ReadNotification { get { return AllNotification - UnReadNotification; } }


        public long JobRequestCount { get; set; }
        public long JobAdvertisementCount { get; set; }
        public long JobApplicationCount { get; set; }
        public long ShortlistCount { get; set; }
        public long InterviewedCount { get; set; }
        public long JobOfferedCount { get; set; }
        public long PrejoiningCount { get; set; }
        public long PrejoiningChecklistCount { get; set; }
        public long JoiningChecklistCount { get; set; }
        public long JoinedCount { get; set; }
        public long? ManpowerRequestId { get; set; }
        public string ManpowerRequestNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? PostedDate { get; set; }
        public ApplicationStateEnum? ApplicationState { get; set; }
        public string ApplicationStatus { get; set; }
        public string JobName { get; set; }

    }
}


using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class RecruitmentProcessViewModel : ViewModelBase
    {

        public ProcessStatusEnum ProcessStatus { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime ProcessStartDate { get; set; }
        public long? JobRequestId { get; set; }
        public long? RemoveServiceId { get; set; }
        public long? AddServiceId { get; set; }

        public long? CandidateId { get; set; }
        public long? ApplicationId { get; set; }
        public string Applications { get; set; }
        public long? RequestedId { get; set; }
        public string RequestedName { get; set; }
        public long? HRSelected { get; set; }
        public long? LMSelected { get; set; }

        public string JobName { get; set; }
        public int NoOfPositions { get; set; }
        public string JobDescription { get; set; }
        public string SkillSet { get; set; }

        public string PendingList { get; set; }
        public string WaitingList { get; set; }
        public string RejectedList { get; set; }
        public string SelectedList { get; set; }

        public long? RecruiterServiceId { get; set; }
        public long? LineManagerServiceId { get; set; }
    }

}



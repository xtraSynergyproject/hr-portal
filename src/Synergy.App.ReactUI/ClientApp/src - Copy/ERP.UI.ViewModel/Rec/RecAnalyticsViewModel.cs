using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using static ERP.Utility.Constant;

namespace ERP.UI.ViewModel
{
    public class RecAnalyticsViewModel : ViewModelBase
    {


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? Month { get; set; }
        public string Metrics { get; set; }
        public string HiringRate { get; set; }
        public string NewEmployees { get; set; }
        public string Employees { get; set; }
        public string Division { get; set; }
        public string Region { get; set; }

        public string Department { get; set; }
        public string Position { get; set; }
        public string JobCode { get; set; }
        public int Positions { get; set; }
        public int DaysOpen { get; set; }
        public int ReceivedResumes { get; set; }
        public int InterviewedCandidates { get; set; }
        public int SentOffers { get; set; }
        public int AcceptedOffers { get; set; }
        public int RejectedOffers { get; set; }
        public string Superviser { get; set; }
        public string Employee { get; set; }
        public string Title { get; set; }
        public string EmployeeID { get; set; }
        public string WorkExperience { get; set; }
        public string RecruitingSource { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? HireDate { get; set; }
        public string JobOpenReason { get; set; }
        public long? Year { get; set; }
        public long? DepartmentId { get; set; }
        public long? PositionId { get; set; }
    }

}



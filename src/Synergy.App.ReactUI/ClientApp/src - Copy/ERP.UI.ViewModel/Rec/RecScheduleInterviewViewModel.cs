using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class RecScheduleInterviewViewModel
    {
        public long? AppId { get; set; }
        public string ApplicationNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string UserEmail { get; set; }

        public string InterviewScheduledStartDate { get; set; }
        public string AssessmentInterviewDuration { get; set; }
        public string AssessmentInterviewUrl { get; set; }
        public string AssessmentInterviewPanel1 { get; set; }
        public string AssessmentInterviewPanel2 { get; set; }
        public string AssessmentInterviewPanel3 { get; set; }
        public string AssessmentInterviewPanel4 { get; set; }
        public string AssessmentInterviewPanel5 { get; set; }
    }

    public class RecScheduleInterviewSubmit
    {
        public List<RecScheduleInterviewViewModel> Created { get; set; }
        public List<RecScheduleInterviewViewModel> Updated { get; set; }
        public List<RecScheduleInterviewViewModel> Destroyed { get; set; }
    }
}

using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class UserAssessmentScheduleViewModel
    {
        public long SlotId { get; set; }
        public string SlotType { get; set; }
        public string SlotName { get; set; }
        public string PlanStartDate { get; set; }
        public string PlanEndDate { get; set; }
        public string JobName { get; set; }
        public string Ministry { get; set; }
        public string SlotProctor { get; set; }
        public string SlotInterviewer { get; set; }
        public string Url { get; set; }
        public string AssessmentSet { get; set; }
        public string AssignAssessmentSet { get; set; }

        public string SendInvite { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrefferedLanguage { get; set; }

        public string AssessmentTitle { get; set; }
        public string TechnicalAssessmentName { get; set; }
        public string AssessmentScheduledStartDate { get; set; }
        public string TechnicalAssessmentUrl { get; set; }

        public string AssessmentTitle2 { get; set; }
        public string TechnicalAssessmentName2 { get; set; }
        public string AssessmentScheduledStartDate2 { get; set; }
        public string TechnicalAssessmentUrl2{ get; set; }

        public string AssessmentTitle3 { get; set; }
        public string TechnicalAssessmentName3 { get; set; }
        public string AssessmentScheduledStartDate3 { get; set; }
        public string TechnicalAssessmentUrl3 { get; set; }

        public string AssessmentTitle4 { get; set; }
        public string TechnicalAssessmentName4 { get; set; }
        public string AssessmentScheduledStartDate4 { get; set; }
        public string TechnicalAssessmentUrl4 { get; set; }

        public string AssessmentTitle5 { get; set; }
        public string TechnicalAssessmentName5 { get; set; }
        public string AssessmentScheduledStartDate5 { get; set; }
        public string TechnicalAssessmentUrl5 { get; set; }

        public string AssessmentTitle6 { get; set; }
        public string TechnicalAssessmentName6 { get; set; }
        public string AssessmentScheduledStartDate6 { get; set; }
        public string TechnicalAssessmentUrl6 { get; set; }

        public string AssessmentTitle7 { get; set; }
        public string TechnicalAssessmentName7 { get; set; }
        public string AssessmentScheduledStartDate7 { get; set; }
        public string TechnicalAssessmentUrl7 { get; set; }

        public string AssessmentTitle8 { get; set; }
        public string TechnicalAssessmentName8 { get; set; }
        public string AssessmentScheduledStartDate8 { get; set; }
        public string TechnicalAssessmentUrl8 { get; set; }

        public string AssessmentTitle9 { get; set; }
        public string TechnicalAssessmentName9 { get; set; }
        public string AssessmentScheduledStartDate9 { get; set; }
        public string TechnicalAssessmentUrl9 { get; set; }

        public string AssessmentTitle10 { get; set; }
        public string TechnicalAssessmentName10 { get; set; }
        public string AssessmentScheduledStartDate10 { get; set; }
        public string TechnicalAssessmentUrl10 { get; set; }

       // public string CaseStudyTitle { get; set; }
        public string CaseStudyName { get; set; }

        public string CaseStudyScheduledStartDate { get; set; }

        public string CaseStudyUrl { get; set; }
     
        public string MinistryName { get; set; }
        public string JobTitle { get; set; }
        public string MobileNo { get; set; }
        public string Proctor { get; set; }
        public string LineManager { get; set; }
        public string AssessmentInterviewDuration { get; set; }
        public string AssessmentInterviewUrl { get; set; }

        public string AssessmentInterviewPanel { get; set; }      
        public string InterviewScheduledStartDate { get; set; }

        public string EnableLanguageChange { get; set; }

        public string IsArchive { get; set; }

        public long? UserId { get; set; }
        public bool? isAssessmentCreated { get; set; }

    }

    public class ApplicationAssessmentScheduleViewModel
    {
        public long ApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }

        public string PrefferedLanguage { get; set; }

        public string Test1TypeId { get; set; }
        public string Test1Name { get; set; }
        public string Test1ScheduledStartDate { get; set; }
        public string Test1Url { get; set; }


        public string Test2TypeId { get; set; }
        public string Test2Name { get; set; }
        public string Test2ScheduledStartDate { get; set; }
        public string Test2Url { get; set; }


        public string Test3TypeId { get; set; }
        public string Test3Name { get; set; }
        public string Test3ScheduledStartDate { get; set; }
        public string Test3Url { get; set; }

        public string Test4TypeId { get; set; }
        public string Test4Name { get; set; }
        public string Test4ScheduledStartDate { get; set; }
        public string Test4Url { get; set; }

        public string Test5TypeId { get; set; }
        public string Test5Name { get; set; }
        public string Test5ScheduledStartDate { get; set; }
        public string Test5Url { get; set; }

    }

    public class ApplicantAssessmentViewModel
    {
        public long ApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }

        public string PrefferedLanguage { get; set; }

        public string Test1TypeId { get; set; }
        public string Test1Name { get; set; }
        public string Test1ScheduledStartDate { get; set; }
        public string Test1ScheduledEndDate { get; set; }
        public string Test1ActualStartDate { get; set; }
        public string Test1ActualEndDate { get; set; }
        public string Test1Url { get; set; }


        public string Test2TypeId { get; set; }
        public string Test2Name { get; set; }
        public string Test2ScheduledStartDate { get; set; }
        public string Test2ScheduledEndDate { get; set; }
        public string Test2ActualStartDate { get; set; }
        public string Test2ActualEndDate { get; set; }
        public string Test2Url { get; set; }


        public string Test3TypeId { get; set; }
        public string Test3Name { get; set; }
        public string Test3ScheduledStartDate { get; set; }
        public string Test3ScheduledEndDate { get; set; }
        public string Test3ActualStartDate { get; set; }
        public string Test3ActualEndDate { get; set; }
        public string Test3Url { get; set; }

        public string Test4TypeId { get; set; }
        public string Test4Name { get; set; }
        public string Test4ScheduledStartDate { get; set; }
        public string Test4ScheduledEndDate { get; set; }
        public string Test4ActualStartDate { get; set; }
        public string Test4ActualEndDate { get; set; }
        public string Test4Url { get; set; }

        public string Test5TypeId { get; set; }
        public string Test5Name { get; set; }
        public string Test5ScheduledStartDate { get; set; }
        public string Test5ScheduledEndDate { get; set; }
        public string Test5ActualStartDate { get; set; }
        public string Test5ActualEndDate { get; set; }
        public string Test5Url { get; set; }

    }
}

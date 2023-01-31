using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class SurveyScheduleViewModel : NoteTemplateViewModel
    {
        public string SurveyScheduleName { get; set; }
        public string SurveyScheduleDescription { get; set; }
        public string SurveyId { get; set; }
        public string SurveyLink { get; set; }
        public string SurveyCode { get; set; }
        public string SurveyName { get; set; }
        public DateTime? SurveyExpiryDate { get; set; }
        public string StartingInstruction { get; set; }
        public string ClosingInstruction { get; set; }
        public string UserDetails { get; set; }
        public bool IsOpenSurveySchedule { get; set; }
        public SurveyStatusEnum SurveyStatus { get; set; }
        public string SurveyStatusName { get; set; }
        public string PreferredLanguage { get; set; }
        public string PreferredLanguageName { get; set; }
        public string Message { get; set; }
        public string SurveyResultId { get; set; }      
        public DateTime? SurveyEndDate { get; set; }
        public string SurveyScheduleUserId { get; set; }        
        public string SurveyScheduleId { get; set; }        
        public string SurveyUserId { get; set; }
        public DateTime? SurveyStartDate { get; set; }        
        public string CurrentQuestionId { get; set; }
        public string CurrentTopicId { get; set; }   
        public string ServiceId { get; set; }
        public string LanguageCode { get; set; }
        public string PortalLogoId { get; set; }
        public string SurveyUserName { get; set; }
        public string SurveyUserEmail { get; set; }
        public string Link { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Comment { get; set; }



    }
}

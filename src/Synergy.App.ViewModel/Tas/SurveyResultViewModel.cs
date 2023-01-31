using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class SurveyResultViewModel:NoteTemplateViewModel
    {
        public string SurveyId { get; set; }
        public string SurveyScheduleId { get; set; }
        public string SurveyScheduleUserId { get; set; }
        public string SurveyUserId { get; set; }
        public DateTime? SurveyStartDate { get; set; }
        public DateTime? SurveyEndDate { get; set; }
        public string CurrentQuestionId { get; set; }
        public string CurrentTopicId { get; set; }
        public string PreferredLanguage { get; set; }
        

    }
}

using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class SurveyTopicViewModel : NoteTemplateViewModel
    {
        public string TopicId { get; set; }
        public string TopicName { get; set; }

        public string NextId { get; set; }
        public string PreviousId { get; set; }
        public string FirstId { get; set; }
        public string LastId { get; set; }
        public string PreferredLanguageCode { get; set; }
        public List<SurveyQuestionViewModel> QuestionList { get; set; }                
        public string QuestionAnswerList { get; set; }                

        public string ServiceId { get; set; }
        public string SurveyId { get; set; }
        public string CurrentTopicId { get; set; }

        public string ServiceStatusCode { get; set; }
        public string SurveyScheduleUserId { get; set; }

    }
}

using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class SurveyQuestionViewModel
    {
        public string QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string AnswerId { get; set; }
        public string AnswerItemId { get; set; }
        public string AnswerName { get; set; }
        public string CurrentAnswerId { get; set; }
        public string AnswerComment { get; set; }
        public List<AssessmentQuestionsOptionViewModel> OptionList { get; set; }

        public string Division { get; set; }
        public string CareerLevel { get; set; }
        public string Market { get; set; }
        public string HRFunction { get; set; }

    }
}

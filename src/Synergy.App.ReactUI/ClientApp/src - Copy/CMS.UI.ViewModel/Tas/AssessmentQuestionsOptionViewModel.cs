using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class AssessmentQuestionsOptionViewModel : NoteTemplateViewModel
    {
        public string OptionId { get; set; }
        public string Option { get; set; }
        public string OptionArabic { get; set; }
        public bool IsRightAnswer { get; set; }
        public double Score { get; set; }
        public string AnswerKey { get; set; }
        public OptionTypeEnum OptionType { get; set; }
        public string AnswerId { get; set; }
        public string AnswerItemId { get; set; }
        public string AnswerComment { get; set; }
    }
}

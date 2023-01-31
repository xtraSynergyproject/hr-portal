using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class AssessmentAnswerViewModel : NoteTemplateViewModel
    {
        public string AssessmentId { get; set; }
        public string AssessmentSetId { get; set; }
        public string UserId { get; set; } 
        public string QuestionId { get; set; }        
        public string OptionId { get; set; }
        public string Comment { get; set; }
        public string NtsNoteId { get; set; }
        public BoolStatus Accurate { get; set; }
        public string Question { get; set; }
        public string QuestionAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string RightAnswer { get; set; }
        public string RightAnswerAr { get; set; }
        public string CandidateAnswer { get; set; }
        public string CandidateAnswerAr { get; set; }
        public string CandidateAnswerComment { get; set; }
        public string CandidateAnswerCommentAr { get; set; }
        public long SeraillNo { get; set; }
        public Int32 MaxoptionNo { get; set; }
        public string CaseStudyComment { get; set; }

        public string Option1 { get; set; }
        public string Option1Ar { get; set; }
        public string Option2 { get; set; }
        public string Option2Ar { get; set; }
        public string Option3 { get; set; }
        public string Option3Ar { get; set; }
        public string Option4 { get; set; }
        public string Option4Ar { get; set; }

        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public string Option7 { get; set; }
        public string Option8 { get; set; }
        public string Option9 { get; set; }
        public string Option10 { get; set; }

        public double? Score { get; set; }
        public string InterviewerComment { get; set; }
        public long? AnswerId { get; set; }
        public string FileId { get; set; }
        public string FileIdAr { get; set; }

        public double? ScoreOption { get; set; }
        public string ScoreOption1 { get; set; }
        public string ScoreOption2 { get; set; }
        public string ScoreOption3 { get; set; }
        public string ScoreOption4 { get; set; }
        public string ScoreOption5 { get; set; }
        public string ScoreOption6 { get; set; }
        public string ScoreOption7 { get; set; }
        public string ScoreOption8 { get; set; }
        public string ScoreOption9 { get; set; }
        public string ScoreOption10 { get; set; }


    }
}

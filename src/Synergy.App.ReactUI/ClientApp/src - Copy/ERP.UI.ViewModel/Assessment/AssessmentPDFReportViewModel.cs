
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;
using Kendo.Mvc.UI;

namespace ERP.UI.ViewModel
{
    public class AssessmentPDFReportViewModel
    {
        public string UserName { get; set; }
        public string JobName { get; set; }
        public double? TotalScore
        {
            get
            {
                return ((TechnicalQuestionnaireScore * 0.2) + (CaseAnalysisScore * 0.2) + (TechnicalInterviewScore * 0.6));
            }

        }
        public string BeginnerScore { get; set; }
        public string FoundationScore { get; set; }
        public string IntermediateScore { get; set; }
        public string AdvancedScore { get; set; }
        public string ExpertScore { get; set; }
        public double? TechnicalQuestionnaireScore { get; set; }
        public double? CaseAnalysisScore { get; set; }
        public double? TechnicalInterviewScore { get; set; }
        public double? TechnicalWeightage { get; set; }
        public double? CaseStudyWeightage { get; set; }
        public double? InterviewWeightage { get; set; }
        public long? AssessmentTechnicalDuratrion { get; set; }
        public long? AssessmentCaseStudyDuratrion { get; set; }
        public int? AssessmentInterviewDuratrion { get; set; }
        public string TechnicalQuestionCount { get; set; }
        public string CaseStudyQuestionCount { get; set; }
    }
}
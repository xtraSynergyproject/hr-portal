using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class UserAssessmentResultViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string TotalScore { get; set; }
        public string TechnicalAssessment10Percent { get; set; }
        public double? TechnicalAssessmentScore { get; set; }
        public string CaseStudy10Percent { get; set; }
        public double? CaseStudyScore { get; set; }
        public double? TechnicalInterview15Percent { get; set; }
        public double? TechnicalInterviewScore { get; set; }
        public string ILM1 { get; set; }
        public string ILM2 { get; set; }
        public string ILM3 { get; set; }
        public string ILM4 { get; set; }
        public string ILM5 { get; set; }
        public string ILM6 { get; set; }
        public string ILM7 { get; set; }
        public string ILM8 { get; set; }
        public string ILM9 { get; set; }
        public double? ILMScore { get; set; }
        public string Control { get; set; }
        public string Commitment { get; set; }
        public string TheChallenge { get; set; }
        public string Altafah { get; set; }
        public double? MTQScore { get; set; }
    }
}

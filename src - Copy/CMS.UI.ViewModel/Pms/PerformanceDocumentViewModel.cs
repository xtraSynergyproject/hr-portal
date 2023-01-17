using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class PerformanceDocumentViewModel : NoteTemplateViewModel
    {
        public string Name { get; set; }           
        public string DocId { get; set; }       
        public string ServiceId { get; set; }       
        public string DocumentMasterId { get; set; }
        public string CalculatedRatingStageId { get; set; }
        public string MasterName { get; set; }
        public string JobName { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeNo { get; set; }
        public string UserId { get; set; }
        public string ReadyForPerformanceRating { get; set; }
        public int? Year { get; set; }
        public int? GoalWeightage { get; set; }
        public int? CompetencyWeightage { get; set; }
        public double? TotalGoalWeightage { get; set; }
        public double? TotalCompetencyWeightage { get; set; }
        public string GoalNoOfEndYearReviewCompleted { get; set; }
        public string CompetencyNoOfEndYearReviewCompleted { get; set; }
        public int? TotalGoal { get; set; }
        public int? TotalCompetency { get; set; }
        public string GoalNoOfMidYearReviewCompleted { get; set; }
        public string CompetencyNoOfMidYearReviewCompleted { get; set; }
        public DateTime? EndDate { get; set; }
        public PerformanceDocumentStatusEnum? DocumentStatus { get; set; }
        public string ServiceStatusName { get; set; }
        public string JobTitle { get; set; }
        public string CalculatedRating { get; set; }
        public string CalculatedRatingRounded { get; set; }
        public string AdjustedRating { get; set; }
        public string AdjustedRatingRounded { get; set; }
        public string FinalRating { get; set; }
        public string FinalRatingRounded { get; set; }
        public string NoOfMidYearReviewCompletedCompetency { get; set; }
        public string NoOfMidYearReviewCompletedGoal { get; set; }
        public string NoOfEndYearReviewCompletedCompetency { get; set; }
        public string NoOfEndYearReviewCompletedGoal { get; set; }


        public string PerformanceRatingId { get; set; }
        public string LetterTemplate { get; set; }
        public string FinalRatingSourceId { get; set; }
        public string GradeId { get; set; }
        public string GradeName { get; set; }
        public string RatingId { get; set; }
        public double RatingCode { get; set; }
        public string RatingName { get; set; }
        public double? BonusPercentage { get; set; }
        public double? IncrementPercentage { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentId_DepartmentName { get; set; }
        public string MasterStageId { get; set; }
        public string MasterStageName { get; set; }
    }
}

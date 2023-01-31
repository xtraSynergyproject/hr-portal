using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.Sps
{
    public class SuccessionPlanningWorksheetViewModel : ViewModelBase
    {
        [Display(Name = "SpsWorksheetId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public long? SpsWorksheetId { get; set; }
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public string Name { get; set; }
        [Display(Name = "Position", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public string Position { get; set; }
        [Display(Name = "Subject", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Cycle Name")]
        public string Subject { get; set; }
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //[Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "InductionManual", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Induction Manual")]
        public string InductionManual { get; set; }
        [Display(Name = "DateOfSPRewview", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //[Display(Name = "Date Of SP Review")]
        public string DateOfSPRewview { get; set; }
        [Display(Name = "PotentialNextRole", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Potential Next Role")]
        public long? PotentialNextRole { get; set; }
        [Display(Name = "IsReadyForNextRole", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Is Ready For Next Role")]
        public bool? IsReadyForNextRole { get; set; }
        [Display(Name = "PotentialReplacement", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Potential Replacement")]
        public long? PotentialReplacement { get; set; }
        [Display(Name = "DevelopmentNeeds", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Development Needs")]
        public string DevelopmentNeeds { get; set; }
        [Display(Name = "TraininigSessionAgreed", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Traininig Session Agreed")]
        public string TraininigSessionAgreed { get; set; }
        [Display(Name = "TrainerAssigned", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Trainer Assigned")]
        public bool? TrainerAssigned { get; set; }
        [Display(Name = "TrainingCompleted", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Training Completed")]
        public bool? TrainingCompleted { get; set; }
        [Display(Name = "Stage1", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Stage 1")]
        public string Stage1 { get; set; }
        [Display(Name = "Stage2", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Stage 2")]
        public string Stage2 { get; set; }
        [Display(Name = "Stage3", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Stage 3")]
        public string Stage3 { get; set; }
        [Display(Name = "ServiceId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public long? ServiceId { get; set; }
        [Display(Name = "JobName", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Potential Next Role")]
        public string JobName { get; set; }
        [Display(Name = "FirstName", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public string FirstName { get; set; }
        [Display(Name = "LastName", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public string LastName { get; set; }
        [Display(Name = "RoleChallenge", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "What parts of your role do you find the most challenging? ")]
        public string RoleChallenge { get; set; }
        [Display(Name = "RoleEnjoy", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "What parts of your role do you enjoy the most? ")]
        public string RoleEnjoy { get; set; }
        [Display(Name = "LongTermGoal", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "What are your mid to long-term goals/objectives? ")]
        public string LongTermGoal { get; set; }
        [Display(Name = "AchieveGoal", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "What would help you achieve these goals / objectives ? ")]
        public string AchieveGoal { get; set; }
        [Display(Name = "WorkSheetCount", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "WorkSheet Count")]
        public long? WorkSheetCount { get; set; }
        [Display(Name = "Skills", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "What skills/training can help you be more effective in your current role/overcome the challenges? Use the 4 cornerstones Finance, People Management, Costs Control, Quality & Consistency ")]
        public string Skills { get; set; }
        [Display(Name = "Training", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "What training and development do you feel you need for your future development (This may include exposure to/involvement in different aspects of the business)?")]
        public string Training { get; set; }
        [Display(Name = "MakeWorkMoreIntresting", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Frustrations and/or ideas of how to make the work more interesting, rewarding ")]
        public string MakeWorkMoreIntresting { get; set; }
        [Display(Name = "SpsId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public long? SpsId { get; set; }
        [Display(Name = "FullName", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Potential Replacement")]//
        public string FullName { get { return FirstName + " " + LastName;  } }
        [Display(Name = "OrganizationId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public long? OrganizationId { get; set; }
        [Display(Name = "UserId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public long? UserId { get; set; }
        [Display(Name = "WhereDoYouSee", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Where do you see yourself in 6/12/18 months? What areas in the business would you like to be developed in? ")]
        public string WhereDoYouSee { get; set; }
        [Display(Name = "TrainingNeeds", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
                //[Display(Name = "Specify training needs and identify mentor to support the action plan and timelines ")]
        public string TrainingNeeds { get; set; }
        [Display(Name = "Ideas", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Share any advice to help us improve the succession planning ")]
        public string Ideas { get; set; }
        [Display(Name = "WhatSuccess", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "What was successfully completed/outstanding/areas of concern ")]
        public string WhatSuccess { get; set; }
        [Display(Name = "Achievements", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Achievements and areas that need more focus ")]
        public string Achievements { get; set; }
        [Display(Name = "OverallComments", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Overall Comments ")]
        public string OverallComments { get; set; }
        [Display(Name = "Review", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public string Review { get; set; }
        [Display(Name = "IdeasSuggestions", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Ideas/Suggestions	")]
        public string IdeasSuggestions { get; set; }
        [Display(Name = "DevelopmentPlans", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Development Plans	")]
        public string DevelopmentPlans { get; set; }
        [Display(Name = "Objectives", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        //[Display(Name = "Objectives")]
        public string Objectives { get; set; }
        [Display(Name = "SpsStep1Status", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public string SpsStep1Status { get; set; }
        [Display(Name = "SpsStep2Status", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public string SpsStep2Status { get; set; }
        [Display(Name = "SpsStep3Status", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public string SpsStep3Status { get; set; }
        [Display(Name = "SponsorshipNo", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningWorksheet))]
        public string SponsorshipNo { get; set; }


    }
}

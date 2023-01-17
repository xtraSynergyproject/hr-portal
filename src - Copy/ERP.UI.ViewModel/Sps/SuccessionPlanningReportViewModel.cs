using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.Sps
{
    public class SuccessionPlanningReportViewModel : ViewModelBase
    {
        public long? SpsWorksheetId { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        [Display(Name = "Cycle Name")]
        public string Subject { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Induction Manual")]
        public string InductionManual { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Date Of SP Review")]
        public string DateOfSPRewview { get; set; }

        [Display(Name = "Potential Next Role")]
        public string PotentialNextRole { get; set; }

        [Display(Name = "Is Ready For Next Role")]
        public bool? IsReadyForNextRole { get; set; }

        [Display(Name = "Potential Replacement")]
        public long? PotentialReplacement { get; set; }

        [Display(Name = "Development Needs")]
        public string DevelopmentNeeds { get; set; }

        [Display(Name = "Traininig Session Agreed")]
        public string TraininigSessionAgreed { get; set; }

        [Display(Name = "Trainer Assigned")]
        public bool? TrainerAssigned { get; set; }

        [Display(Name = "Training Completed")]
        public bool? TrainingCompleted { get; set; }

        [Display(Name = "Stage 1")]
        public string Stage1 { get; set; }
        [Display(Name = "Stage 2")]
        public string Stage2 { get; set; }
        [Display(Name = "Stage 3")]
        public string Stage3 { get; set; }
        public long? ServiceId { get; set; }
        public long? TaskId { get; set; }
        public string TemplateField { get; set; }
        public string Value { get; set; }

        [Display(Name = "Potential Next Role")]
        public string JobName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        [Display(Name = "What parts of your role do you find the most challenging? ")]
        public string RoleChallenge { get; set; }

        [Display(Name = "What parts of your role do you enjoy the most? ")]
        public string RoleEnjoy { get; set; }
        [Display(Name = "What are your mid to long-term goals/objectives? ")]
        public string LongTermGoal { get; set; }
        [Display(Name = "What would help you achieve these goals / objectives ? ")]
        public string AchieveGoal { get; set; }
        [Display(Name = "WorkSheet Count")]
        public long? WorkSheetCount { get; set; }
        [Display(Name = "What skills/training can help you be more effective in your current role/overcome the challenges? Use the 4 cornerstones Finance, People Management, Costs Control, Quality & Consistency ")]
        public string Skills { get; set; }
        [Display(Name = "What training and development do you feel you need for your future development (This may include exposure to/involvement in different aspects of the business)?")]
        public string Training { get; set; }
        [Display(Name = "Frustrations and/or ideas of how to make the work more interesting, rewarding ")]
        public string MakeWorkMoreIntresting { get; set; }

        public long? SpsId { get; set; }

        [Display(Name = "Potential Replacement")]
        public string FullName { get { return FirstName + " " + LastName;  } }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }

        public long? UserId { get; set; }

        [Display(Name = "Where do you see yourself in 6/12/18 months? What areas in the business would you like to be developed in? ")]
        public string WhereDoYouSee { get; set; }
        [Display(Name = "Specify training needs and identify mentor to support the action plan and timelines ")]
        public string TrainingNeeds { get; set; }
        [Display(Name = "Share any advice to help us improve the succession planning ")]
        public string Ideas { get; set; }
        [Display(Name = "What was successfully completed/outstanding/areas of concern ")]
        public string WhatSuccess { get; set; }
        [Display(Name = "Achievements and areas that need more focus ")]
        public string Achievements { get; set; }
        [Display(Name = "Overall Comments ")]
        public string OverallComments { get; set; }
        public string Review { get; set; }
        [Display(Name = "Ideas/Suggestions	")]
        public string IdeasSuggestions { get; set; }
        [Display(Name = "Development Plans	")]
        public string DevelopmentPlans { get; set; }
        [Display(Name = "Objectives")]
        public string Objectives { get; set; }

        public string Training1 { get; set; }
        public string Training2 { get; set; }
        public string Training3 { get; set; }
        public string Training4 { get; set; }
        public string Training5 { get; set; }
        public string TrainingOther { get; set; }

        public string SpsStep1Status { get; set; }
        public string SpsStep2Status { get; set; }
        public string SpsStep3Status { get; set; }
        public string SponsorshipNo { get; set; }

        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string Job { get; set; }
        public string Sponsor { get; set; }
        public string Nationality { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ContractEndDate { get; set; }




    }
}

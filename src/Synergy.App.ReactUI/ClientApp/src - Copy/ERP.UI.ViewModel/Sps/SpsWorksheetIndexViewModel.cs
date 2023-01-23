using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.Sps
{
    public class SpsWorksheetIndexViewModel : ViewModelBase
    {
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string Name { get; set; }
        [Display(Name = "Position", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string Position { get; set; }
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        [DisplayFormat(DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime StartDate { get; set; }
        [Display(Name = "InductionManual", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string InductionManual { get; set; }
        [Display(Name = "DateOfSPRewview", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        [DisplayFormat(DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime DateOfSPRewview { get; set; }
        [Display(Name = "PotentialNextRole", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public long PotentialNextRole { get; set; }
        [Display(Name = "IsReadyForNextRole", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string IsReadyForNextRole { get; set; }
        [Display(Name = "PotentialReplacement", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public long PotentialReplacement { get; set; }
        [Display(Name = "DevelopmentNeeds", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string DevelopmentNeeds { get; set; }
        [Display(Name = "TraininigSessionAgreed", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string TraininigSessionAgreed { get; set; }
        [Display(Name = "TrainerAssigned", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string TrainerAssigned { get; set; }
        [Display(Name = "TrainingCompleted", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string TrainingCompleted { get; set; }
        [Display(Name = "Stage1", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string Stage1 { get; set; }
        [Display(Name = "Stage2", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string Stage2 { get; set; }
        [Display(Name = "Stage3", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string Stage3 { get; set; }
        [Display(Name = "ServiceId", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public long ServiceId { get; set; }
        [Display(Name = "JobName", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string JobName { get; set; }
        [Display(Name = "FirstName", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string FirstName { get; set; }
        [Display(Name = "LastName", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string LastName { get; set; }
        [Display(Name = "RoleChallenge", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string RoleChallenge { get; set; }
        [Display(Name = "LongTermGoal", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string LongTermGoal { get; set; }
        [Display(Name = "AchieveGoal", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string AchieveGoal { get; set; }
        [Display(Name = "Skills", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string Skills { get; set; }
        [Display(Name = "Training", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string Training { get; set; }
        [Display(Name = "MakeWorkMoreIntresting", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public string MakeWorkMoreIntresting { get; set; }
        [Display(Name = "SpsId", ResourceType = typeof(ERP.Translation.Sps.SpsWorksheetIndex))]
        public long SpsId { get; set; }


    }
}

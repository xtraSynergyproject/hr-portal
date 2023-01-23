using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        public long UserId { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public override long? CompanyId { get; set; }

        public string RemoteWorkingUserId { get; set; }

        [Display(Name = "Employee Name")]
        public long? PersonId { get; set; }

        public string UserNameWithEmail { get; set; }

        public bool IsAdmin { get; set; }
        public bool? IsDmsAdmin { get; set; }

        [Display(Name = "Enable Remote SignIn")]
        public bool? EnableRemoteSignIn { get; set; }

        [Required]
        [Display(Name = "User Authentication Type")]
        public UserAuthTypeEnum UserAuthType { get; set; }

        [Required]
        [Display(Name = "User Login Type")]
        public UserLoginTypeEnum UserLoginType { get; set; }

        //public UserTypeEnum UserType { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [StringLength(200)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        [Display(Name = "Personal Id")]
        public string IqamahNo { get; set; }

        [StringLength(50)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [StringLength(50)]
        [Display(Name = "Re-type Password")]
        public string ConfirmPassword { get; set; }

        [StringLength(50)]
        [Display(Name = "OTP")]
        public string ForgotPasswordOTP { get; set; }
        [Display(Name = "User Roles")]
        public string UserRoleVal { get; set; }

        public long[] UserRoleData { get; set; }
        public long[] ActionData { get; set; }

        [Display(Name = "Geo Location")]
        public string GeoLocations { get; set; }
        [Display(Name = "Grade")]
        public string Grades { get; set; }
        [Display(Name = "Organization")]
        public string Orgs { get; set; }
        [Display(Name = "Organization")]
        public string OrgsReport { get; set; }
        [Display(Name = "Job")]
        public string Jobs { get; set; }
        [Display(Name = "Position")]
        public string Positions { get; set; }
        [Display(Name = "Person")]
        public string Persons { get; set; }

        [Display(Name = "Workflow Templates")]
        public string WorklistTemplateMasters { get; set; }

        [Display(Name = "Team Selection")]
        public string TemplateTeams { get; set; }

        public string Actions { get; set; }
        public string Fields { get; set; }
        public string FieldsEditable { get; set; }

        public long[] GeoLocationData { get; set; }
        public long[] GradeData { get; set; }
        public long[] OrgData { get; set; }
        public long[] JobData { get; set; }
        public long[] PosData { get; set; }
        public long[] PerData { get; set; }
        public long[] OrgDataReports { get; set; }
        public long[] WorklistTemplateMasterData { get; set; }
        public long[] TemplateTeamData { get; set; }

        public List<ModuleViewModel> Modules { get; set; }
        public List<ModuleViewModel> ModulesReports { get; set; }

        public List<string> UserRoleDetails { get; set; }
        public string UserRolepermission { get; set; }

        [Display(Name = "Enable Email Summary")]
        public bool EnableEmailSummary { get; set; }
        [Display(Name = "Enable Regular Email")]
        public bool EnableRegularEmail { get; set; }


        public string SponsorshipNo { get; set; }
        public string BiometricId { get; set; }
        public string MobileDeviceToken { get; set; }
        [Display(Name = "Legal Entity")]
        public long LegalEntityId { get; set; }

        [Display(Name = "Legal Entity")]
        public List<long?> LegalEntities { get; set; }

        public bool DontSendEmail { get; set; }


        [Display(Name = "Verification Status")]
        public UserVerificationStatusEnum? VerificationStatus { get; set; }
        public string VerificationStatusText { get; set; }
        [Display(Name = "General Workspace")]
        public long? WorkspaceId { get; set; }

        [Display(Name = "General Workspace")]
        public List<long?> WorkspaceIds { get; set; }
        [Display(Name = "Workspace Permission Group")]
        public List<long?> WorkspacePermissionGroupIds { get; set; }

        [Display(Name = "Admin Workspace")]
        public long? AdminWorkspaceId { get; set; }

        [Display(Name = "Admin Workspace")]
        public List<long?> AdminWorkspaceIds { get; set; }

        public long? MyWorkspaceId { get; set; }

        public string[] UserRoleStr { get; set; }
        public string UserRoles
        {
            get
            {
                var str = "";
                if (UserRoleStr != null)
                    str = string.Join(",", UserRoleStr);
                return str;
            }
        }
        public long? UserRoleId { get; set; }

        public string UserRoleText { get; set; }
        [Display(Name = "Photo")]
        public long? PhotoId { get; set; }
        public FileViewModel SelectedFile { get; set; }

      
        [Display(Name = "Assessment Case Study")]
        public long? AssessmentCaseStudyId { get; set; }

        [Display(Name = "Assessment Survey")]
        public long? AssessmentSurveyId { get; set; }
        [Display(Name = "Assessment Survey")]
        public long? AssessmentInterviewId { get; set; }
        [Display(Name = "Interview Panel")]
        public long? InterviewPanelId { get; set; }

        [Display(Name = "Assessment Question Naire")]
        public long? AssessmentQuistionNaireId { get; set; }
        [Display(Name = "Assessment Scheduled Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormatOnly)]
        public DateTime? AssessmentScheduledStartDate { get; set; }
        [Display(Name = "Technical Assessment Url")]
        public string TechnicalAssessmentUrl { get; set; }
        public string AssessmentTitle { get; set; }

        public long? AssessmentQuistionNaireId2 { get; set; }
        public DateTime? AssessmentScheduledStartDate2 { get; set; }
        public string TechnicalAssessmentUrl2 { get; set; }
        public string AssessmentTitle2 { get; set; }

        public long? AssessmentQuistionNaireId3 { get; set; }
        public DateTime? AssessmentScheduledStartDate3 { get; set; }
        public string TechnicalAssessmentUrl3 { get; set; }
        public string AssessmentTitle3 { get; set; }

        public long? AssessmentQuistionNaireId4 { get; set; }
        public DateTime? AssessmentScheduledStartDate4 { get; set; }
        public string TechnicalAssessmentUrl4 { get; set; }
        public string AssessmentTitle4 { get; set; }

        public long? AssessmentQuistionNaireId5 { get; set; }
        public DateTime? AssessmentScheduledStartDate5 { get; set; }
        public string TechnicalAssessmentUrl5 { get; set; }
        public string AssessmentTitle5 { get; set; }

        public long? AssessmentQuistionNaireId6 { get; set; }
        public DateTime? AssessmentScheduledStartDate6 { get; set; }
        public string TechnicalAssessmentUrl6 { get; set; }
        public string AssessmentTitle6 { get; set; }

        public long? AssessmentQuistionNaireId7 { get; set; }
        public DateTime? AssessmentScheduledStartDate7 { get; set; }
        public string TechnicalAssessmentUrl7 { get; set; }
        public string AssessmentTitle7 { get; set; }

        public long? AssessmentQuistionNaireId8 { get; set; }
        public DateTime? AssessmentScheduledStartDate8 { get; set; }
        public string TechnicalAssessmentUrl8 { get; set; }
        public string AssessmentTitle8 { get; set; }

        public long? AssessmentQuistionNaireId9 { get; set; }
        public DateTime? AssessmentScheduledStartDate9 { get; set; }
        public string TechnicalAssessmentUrl9 { get; set; }
        public string AssessmentTitle9 { get; set; }

        public long? AssessmentQuistionNaireId10 { get; set; }
        public DateTime? AssessmentScheduledStartDate10 { get; set; }
        public string TechnicalAssessmentUrl10 { get; set; }
        public string AssessmentTitle10 { get; set; }

        public string CaseStudyTitle { get; set; }
        [Display(Name = "Case Study Scheduled Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormatOnly)]
        public DateTime? CaseStudyScheduledStartDate { get; set; }

        [Display(Name = "Case Study Url")]
        public string CaseStudyUrl { get; set; }

        [Display(Name = "Assessment Survey Scheduled Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormatOnly)]
        public DateTime? AssessmentSurveyScheduledStartDate { get; set; }

        [Display(Name = "Assessment Survey Url")]
        public string AssessmentSurveyUrl { get; set; }


        [Display(Name = "Assessment Interview Scheduled Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormatOnly)]
        public DateTime? AssessmentInterviewScheduledStartDate { get; set; }

        [Display(Name = "Assessment Interview Url")]
        public string AssessmentInterviewUrl { get; set; }
        public int? AssessmentInterviewDuratrion { get; set; }

        [Display(Name = "Preferred Language")]
        public Language? PreferredLanguage { get; set; }

        public BoolStatus? EnableLanguageChange { get; set; }
        public double? AssessmentInterviewWeightage { get; set; }

        public string JobTitle { get; set; }
        public long? MinistrySponsorId { get; set; }
        public long? ProctorUserId { get; set; }
        public long? ReportToUserId { get; set; }
        public string UserCode { get; set; }
        public string Color { get; set; }
        public bool? DisableWebAccess { get; set; }
        public bool? DisableDesktopAccess { get; set; }
        public bool? DisableMobileAccess { get; set; }
        public string AlternateEmail { get; set; }

        public long? AssessmentSlotId { get; set; }
        public UserHierarchyViewModel UserHierarchy { get; set; }

        
    }
}



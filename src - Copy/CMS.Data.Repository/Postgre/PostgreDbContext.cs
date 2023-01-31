using CMS.Common;
using CMS.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Data.Repository
{
    public class PostgreDbContext : DbContext
    {
        public PostgreDbContext(DbContextOptions<PostgreDbContext> options) : base(options)
        {

        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // => optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=BRE;Username=postgres;Password=!Welcome123");
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<NtsTaskSequence>().HasIndex(p => p.SequenceDate).IsUnique();
            // builder.Entity<NtsTaskSequence>() .HasIndex(p => new { p.FirstColumn, p.SecondColumn }).IsUnique();
            //  builder.HasCollation("cms_collation_ci", locale: "en-u-ks-primary", provider: "icu", deterministic: false);
            builder.UseDefaultColumnCollation("cms_collation_ci");
            builder.HasDefaultSchema("public");
            //   builder.HasDefaultSchema("cms");
            //builder.Entity<Permission>()
            //        .Property(d => d.PageTypes).HasColumnType("text[]");
            //builder.Entity<Permission>()
            //       .Property(d => d.UserPermissionTypes).HasColumnType("text[]");

            DisableDatabaseInheritence(builder);
            base.OnModelCreating(builder);
        }



        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }

        public DbSet<Company> Company { get; set; }
        public DbSet<Portal> Portal { get; set; }
        public DbSet<TableMetadata> TableMetadata { get; set; }
        public DbSet<ColumnMetadata> ColumnMetadata { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<NtsCategory> NtsCategory { get; set; }
        public DbSet<TemplateCategory> TemplateCategory { get; set; }
        public DbSet<Page> Page { get; set; }
        public DbSet<PageIndex> PageIndex { get; set; }
        public DbSet<PageIndexColumn> PageIndexColumn { get; set; }
        public DbSet<PageNote> PageNote { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }
        public DbSet<UserRolePermission> UserRolePermission { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<SubModule> SubModule { get; set; }
        public DbSet<MenuGroup> MenuGroup { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Template> Template { get; set; }
        public DbSet<FormTemplate> FormTemplate { get; set; }
        public DbSet<FormIndexPageTemplate> FormIndexPageTemplate { get; set; }
        public DbSet<FormIndexPageColumn> FormIndexPageColumn { get; set; }
        public DbSet<PageTemplate> PageTemplate { get; set; }

        public DbSet<NoteTemplate> NoteTemplate { get; set; }
        public DbSet<TaskTemplate> TaskTemplate { get; set; }
        public DbSet<RecTaskTemplate> RecTaskTemplate { get; set; }
        public DbSet<ServiceTemplate> ServiceTemplate { get; set; }
        public DbSet<NtsNote> NtsNote { get; set; }
        public DbSet<NtsTask> NtsTask { get; set; }
        public DbSet<RecTask> RecTask { get; set; }
        public DbSet<NtsService> NtsService { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<TeamUser> TeamUser { get; set; }
        public DbSet<UserRoleUser> UserRoleUser { get; set; }
        public DbSet<ProcessDesign> ProcessDesign { get; set; }
        public DbSet<ProcessDesignVariable> ProcessDesignVariable { get; set; }
        public DbSet<AdhocTaskComponent> AdhocTaskComponent { get; set; }
        public DbSet<StepTaskComponent> StepTaskComponent { get; set; }
        public DbSet<NoteIndexPageTemplate> NoteIndexPageTemplate { get; set; }
        public DbSet<NoteIndexPageColumn> NoteIndexPageColumn { get; set; }

        public DbSet<TaskIndexPageTemplate> TaskIndexPageTemplate { get; set; }
        public DbSet<TaskIndexPageColumn> TaskIndexPageColumn { get; set; }
        public DbSet<ServiceIndexPageTemplate> ServiceIndexPageTemplate { get; set; }
        public DbSet<ServiceIndexPageColumn> ServiceIndexPageColumn { get; set; }
        public DbSet<HierarchyMaster> HierarchyMaster { get; set; }
        public DbSet<UserHierarchy> UserHierarchy { get; set; }

        public DbSet<ManpowerRecruitmentSummary> ManpowerRecruitmentSummary { get; set; }
        public DbSet<ManpowerRecruitmentSummaryVersion> ManpowerRecruitmentSummaryVersion { get; set; }
        public DbSet<ManpowerSummaryComment> ManpowerSummaryComment { get; set; }

        public DbSet<CandidateProfile> CandidateProfile { get; set; }
        public DbSet<CandidateComputerProficiency> CandidateComputerProficiency { get; set; }
        public DbSet<CandidateDrivingLicense> CandidateDrivingLicense { get; set; }
        public DbSet<CandidateEducational> CandidateEducational { get; set; }
        public DbSet<CandidateExperience> CandidateExperience { get; set; }
        public DbSet<CandidateExperienceByCountry> CandidateExperienceByCountry { get; set; }
        public DbSet<CandidateExperienceByJob> CandidateExperienceByJob { get; set; }
        public DbSet<CandidateExperienceByNature> CandidateExperienceByNature { get; set; }
        public DbSet<CandidateExperienceBySector> CandidateExperienceBySector { get; set; }
        public DbSet<CandidateLanguageProficiency> CandidateLanguageProficiency { get; set; }
        public DbSet<CandidateProject> CandidateProject { get; set; }
        public DbSet<CandidateReferences> CandidateReferences { get; set; }
        public DbSet<ApplicationState> ApplicationState { get; set; }
        public DbSet<ApplicationStatus> ApplicationStatus { get; set; }
        public DbSet<Application> Application { get; set; }
        public DbSet<ApplicationComputerProficiency> ApplicationComputerProficiency { get; set; }
        public DbSet<ApplicationDrivingLicense> ApplicationDrivingLicense { get; set; }
        public DbSet<ApplicationEducational> ApplicationEducational { get; set; }
        public DbSet<ApplicationeExperienceByNature> ApplicationeExperienceByNature { get; set; }
        public DbSet<ApplicationExperience> ApplicationExperience { get; set; }
        public DbSet<ApplicationExperienceByCountry> ApplicationExperienceByCountry { get; set; }
        public DbSet<ApplicationExperienceByJob> ApplicationExperienceByJob { get; set; }
        public DbSet<ApplicationExperienceBySector> ApplicationExperienceBySector { get; set; }
        public DbSet<ApplicationLanguageProficiency> ApplicationLanguageProficiency { get; set; }
        public DbSet<ApplicationProject> ApplicationProject { get; set; }
        public DbSet<ApplicationReferences> ApplicationReferences { get; set; }
        public DbSet<ApplicationSalaryDetail> ApplicationSalaryDetail { get; set; }
        public DbSet<Batch> Batch { get; set; }
        public DbSet<JobAdvertisement> JobAdvertisement { get; set; }
        public DbSet<JobCriteria> JobCriteria { get; set; }
        public DbSet<ListOfValue> ListOfValue { get; set; }
        public DbSet<JobAdvertisementTrack> JobAdvertisementTrack { get; set; }
        public DbSet<JobCriteriaTrack> JobCriteriaTrack { get; set; }
        public DbSet<ApplicationStateTrack> ApplicationStateTrack { get; set; }
        public DbSet<CandidateSalaryDetail> CandidateSalaryDetail { get; set; }
        public DbSet<CustomTemplate> CustomTemplate { get; set; }
        public DbSet<ApplicationJobCriteria> ApplicationJobCriteria { get; set; }
        public DbSet<ApplicationStateComment> ApplicationStateComment { get; set; }
        public DbSet<CandidateEvaluation> CandidateEvaluation { get; set; }
        public DbSet<ApplicationExperienceByOther> ApplicationExperienceByOther { get; set; }
        public DbSet<CandidateExperienceByOther> CandidateExperienceByOther { get; set; }
        public DbSet<AppointmentApprovalRequest> AppointmentApprovalRequest { get; set; }
        public DbSet<RecruitmentPayElement> RecruitmentPayElement { get; set; }
        public DbSet<RecruitmentCandidateElementInfo> RecruitmentCandidateElementInfo { get; set; }
        public DbSet<NtsTaskVersion> NtsTaskVersion { get; set; }
        public DbSet<ApplicationBeneficiary> ApplicationBeneficiary { get; set; }
        public DbSet<UserPortal> UserPortal { get; set; }
        public DbSet<UserRolePortal> UserRolePortal { get; set; }
        public DbSet<HiringManager> HiringManager { get; set; }
        public DbSet<RecTaskVersion> RecTaskVersion { get; set; }
        public DbSet<Agency> Agency { get; set; }
        public DbSet<UserDataPermission> UserDataPermission { get; set; }
        public DbSet<UserRoleDataPermission> UserRoleDataPermission { get; set; }
        public DbSet<HiringManagerOrganization> HiringManagerOrganization { get; set; }

        public DbSet<JobDescription> JobDescription { get; set; }
        public DbSet<ExecutionScriptComponent> ExecutionScriptComponent { get; set; }
        public DbSet<DecisionScriptComponent> DecisionScriptComponent { get; set; }
        public DbSet<TrueComponent> TrueComponent { get; set; }
        public DbSet<FalseComponent> FalseComponent { get; set; }
        public DbSet<LegalEntity> LegalEntity { get; set; }
        public DbSet<UserPagePreference> UserPagePreference { get; set; }
        public DbSet<HeadOfDepartment> HeadOfDepartments { get; set; }
        public DbSet<HeadOfDepartmentOrganization> HeadOfDepartmentOrganization { get; set; }
        public DbSet<EmailSetting> EmailSetting { get; set; }
        public DbSet<NtsTaskShared> NtsTaskShared { get; set; }
        public DbSet<NtsTaskSequence> NtsTaskSequence { get; set; }
        public DbSet<UserRoleStageParent> UserRoleStageParent { get; set; }
        public DbSet<UserRoleStageChild> UserRoleStageChild { get; set; }
        public DbSet<UserRoleStatusLabelCode> UserRoleStatusLabelCode { get; set; }
        public DbSet<NtsNotePrecedence> NtsNotePrecedence { get; set; }
        public DbSet<NtsTaskPrecedence> NtsTaskPrecedence { get; set; }
        public DbSet<NtsServicePrecedence> NtsServicePrecedence { get; set; }
        public DbSet<ComponentParent> ComponentParent { get; set; }
        public DbSet<NtsServiceShared> NtsServiceShared { get; set; }
        public DbSet<NtsTaskComment> NtsTaskComment { get; set; }
        public DbSet<NtsServiceSequence> NtsServiceSequence { get; set; }
        public DbSet<NtsNoteShared> NtsNoteShared { get; set; }
        public DbSet<NtsServiceComment> NtsServiceComment { get; set; }
        public DbSet<NtsNoteComment> NtsNoteComment { get; set; }
        public DbSet<NtsNoteSequence> NtsNoteSequence { get; set; }
        public DbSet<UdfPermission> UdfPermission { get; set; }
        public DbSet<NtsTaskTimeEntry> NtsTaskTimeEntry { get; set; }
        public DbSet<ProcessDesignResult> ProcessDesignResult { get; set; }
        public DbSet<ComponentResult> ComponentResult { get; set; }
        public DbSet<JobDescriptionCriteria> JobDescriptionCriteria { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplate { get; set; }
        public DbSet<ProjectEmailSetup> ProjectEmailSetup { get; set; }
     

        //BRE
        public DbSet<BusinessRuleGroup> BusinessRuleGroup { get; set; }
        public DbSet<BusinessArea> BusinessArea { get; set; }
        public DbSet<BusinessData> BusinessData { get; set; }
        public DbSet<BusinessRule> BusinessRule { get; set; }
        public DbSet<BusinessRuleConnector> BusinessRuleConnector { get; set; }
        public DbSet<BusinessRuleModel> BusinessRuleModel { get; set; }
        public DbSet<BusinessRuleNode> BusinessRuleNode { get; set; }
        public DbSet<BusinessSection> BusinessSection { get; set; }
        public DbSet<DataIntegration> DataIntegration { get; set; }
        public DbSet<BreResult> BreResult { get; set; }
        public DbSet<BreMasterTableMetadata> BreMasterTableMetadata { get; set; }
        public DbSet<BreMasterColumnMetadata> BreMasterColumnMetadata { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<UserGroupUser> UserGroupUser { get; set; }
        public DbSet<NtsGroup> NtsGroup { get; set; }
        public DbSet<NtsGroupTemplate> NtsGroupTemplate { get; set; }
        public DbSet<NtsGroupUserGroup> NtsGroupUserGroup { get; set; }

        public DbSet<UserSet> UserSet { get; set; }
        public DbSet<PayrollBatch> PayrollBatch { get; set; }
        public DbSet<PayrollTransaction> PayrollTransaction { get; set; }
        public DbSet<PayrollRun> PayrollRun { get; set; }

        public DbSet<GrantAccess> GrantAccess { get; set; }
        public DbSet<ResourceLanguage> ResourceLanguage { get; set; }

        public DbSet<NtsTag> NtsTag { get; set; }
        public DbSet<DocumentPermission> DocumentPermission { get; set; }

        public DbSet<UserHierarchyPermission> UserHierarchyPermission { get; set; }
        public DbSet<UserEntityPermission> UserEntityPermission { get; set; }
        public DbSet<OrganizationDocument> OrganizationDocument { get; set; }
        public DbSet<MenuGroupDetails> MenuGroupDetails { get; set; }
        public DbSet<PageDetails> PageDetails { get; set; }
        public DbSet<UdfPermissionHeader> UdfPermissionHeader { get; set; }
        public DbSet<NtsNoteCommentUser> NtsNoteCommentUser { get; set; }
        public DbSet<NtsServiceCommentUser> NtsServiceCommentUser { get; set; }
        public DbSet<NtsTaskCommentUser> NtsTaskCommentUser { get; set; }
        public DbSet<NtsLogPageColumn> NtsLogPageColumn { get; set; }

        public DbSet<CustomIndexPageTemplate> CustomIndexPageTemplate { get; set; }
        public DbSet<CustomIndexPageColumn> CustomIndexPageColumn { get; set; }

        public DbSet<TASUserReport> TASUserReport { get; set; }
        public DbSet<UserPromotion> UserPromotion { get; set; }
        public DbSet<ApplicationError> ApplicationError { get; set; }
        public DbSet<NtsRating> NtsRating { get; set; }
        public DbSet<UserSession> UserSession { get; set; }
        public DbSet<ApplicationAccess> ApplicationAccess { get; set; }
        public DbSet<CompanySetting> CompanySetting { get; set; }
        public DbSet<HybridHierarchy> HybridHierarchy { get; set; }
        public DbSet<ApplicationDocument> ApplicationDocument { get; set; }
        public DbSet<OCRMapping> OCRMapping { get; set; }
        public DbSet<NtsStaging> NtsStaging { get; set; }

        #region Log
        public DbSet<CompanyLog> CompanyLog { get; set; }
        public DbSet<EmailLog> EmailLog { get; set; }
        public DbSet<FileLog> FileLog { get; set; }
        public DbSet<HierarchyMasterLog> HierarchyMasterLog { get; set; }
        public DbSet<LegalEntityLog> LegalEntityLog { get; set; }
        public DbSet<MemberLog> MemberLog { get; set; }
        public DbSet<MemberGroupLog> MemberGroupLog { get; set; }
        public DbSet<MenuGroupLog> MenuGroupLog { get; set; }
        public DbSet<ModuleLog> ModuleLog { get; set; }
        public DbSet<NotificationLog> NotificationLog { get; set; }
        public DbSet<SubModuleLog> SubModuleLog { get; set; }
        public DbSet<TeamLog> TeamLog { get; set; }
        public DbSet<TeamUserLog> TeamUserLog { get; set; }

        public DbSet<UserLog> UserLog { get; set; }
        public DbSet<UserDataPermissionLog> UserDataPermissionLog { get; set; }
        public DbSet<UserHierarchyLog> UserHierarchyLog { get; set; }
        public DbSet<UserPagePreferenceLog> UserPagePreferenceLog { get; set; }
        public DbSet<UserPermissionLog> UserPermissionLog { get; set; }
        public DbSet<UserPortalLog> UserPortalLog { get; set; }
        public DbSet<UserRoleLog> UserRoleLog { get; set; }
        public DbSet<UserRoleDataPermissionLog> UserRoleDataPermissionLog { get; set; }
        public DbSet<UserRolePermissionLog> UserRolePermissionLog { get; set; }
        public DbSet<UserRolePortalLog> UserRolePortalLog { get; set; }
        public DbSet<UserRoleStageChildLog> UserRoleStageChildLog { get; set; }
        public DbSet<UserRoleStageParentLog> UserRoleStageParentLog { get; set; }
        public DbSet<UserRoleUserLog> UserRoleUserLog { get; set; }
        public DbSet<NtsCategoryLog> NtsCategoryLog { get; set; }
        public DbSet<NtsNoteCommentLog> NtsNoteCommentLog { get; set; }
        public DbSet<NtsNotePrecedenceLog> NtsNotePrecedenceLog { get; set; }
        public DbSet<NtsNoteSequenceLog> NtsNoteSequenceLog { get; set; }
        public DbSet<NtsNoteSharedLog> NtsNoteSharedLog { get; set; }
        public DbSet<NtsNoteStatusTrack> NtsNoteStatusTrack { get; set; }
        public DbSet<NtsServiceCommentLog> NtsServiceCommentLog { get; set; }
        public DbSet<NtsServicePrecedenceLog> NtsServicePrecedenceLog { get; set; }
        public DbSet<NtsServiceSequenceLog> NtsServiceSequenceLog { get; set; }
        public DbSet<NtsServiceSharedLog> NtsServiceSharedLog { get; set; }
        public DbSet<NtsServiceStatusTrackLog> NtsServiceStatusTrackLog { get; set; }
        public DbSet<NtsTaskAttachmentLog> NtsTaskAttachmentLog { get; set; }
        public DbSet<NtsTaskCommentLog> NtsTaskCommentLog { get; set; }
        public DbSet<NtsTaskPrecedenceLog> NtsTaskPrecedenceLog { get; set; }
        // public DbSet<NtsTaskSequenceLog> NtsTaskSequenceLog { get; set; }
        public DbSet<NtsTaskSharedLog> NtsTaskSharedLog { get; set; }
        public DbSet<NtsTaskStatusTrackLog> NtsTaskStatusTrackLog { get; set; }
        public DbSet<NtsTaskTimeEntry> NtsTaskTimeEntryLog { get; set; }
        public DbSet<UdfPermissionLog> UdfPermissionLog { get; set; }
        public DbSet<AdhocTaskComponentLog> AdhocTaskComponentLog { get; set; }
        public DbSet<BusinessExecutionComponentLog> BusinessExecutionComponentLog { get; set; }
        public DbSet<BusinessLogicComponentLog> BusinessLogicComponentLog { get; set; }
        public DbSet<CompleteEventComponentLog> CompleteEventComponentLog { get; set; }
        public DbSet<ComponentLog> ComponentLog { get; set; }
        public DbSet<ComponentParentLog> ComponentParentLog { get; set; }
        public DbSet<ComponentResultLog> ComponentResultLog { get; set; }
        public DbSet<DecisionScriptComponentLog> DecisionScriptComponentLog { get; set; }
        public DbSet<EmailComponentLog> EmailComponentLog { get; set; }
        public DbSet<ExecutionScriptComponentLog> ExecutionScriptComponentLog { get; set; }
        public DbSet<FalseComponentLog> FalseComponentLog { get; set; }
        public DbSet<ProcessDesignLog> ProcessDesignLog { get; set; }
        public DbSet<ProcessDesignComponentLog> ProcessDesignComponentLog { get; set; }
        public DbSet<ProcessDesignResultLog> ProcessDesignResultLog { get; set; }
        public DbSet<ProcessDesignVariableLog> ProcessDesignVariableLog { get; set; }
        //public DbSet<StartEventComponentLog> StartEventComponentLog { get; set; }
        public DbSet<StepTaskComponentLog> StepTaskComponentLog { get; set; }
        public DbSet<TrueComponentLog> TrueComponentLog { get; set; }
        public DbSet<CustomTemplateLog> CustomTemplateLog { get; set; }
        public DbSet<FormIndexPageColumnLog> FormIndexPageColumnLog { get; set; }
        public DbSet<FormIndexPageTemplateLog> FormIndexPageTemplateLog { get; set; }
        public DbSet<FormTemplateLog> FormTemplateLog { get; set; }
        public DbSet<NoteIndexPageColumnLog> NoteIndexPageColumnLog { get; set; }
        public DbSet<NoteIndexPageTemplateLog> NoteIndexPageTemplateLog { get; set; }
        public DbSet<NoteTemplateLog> NoteTemplateLog { get; set; }
        public DbSet<NotificationTemplateLog> NotificationTemplateLog { get; set; }
        public DbSet<PageTemplateLog> PageTemplateLog { get; set; }
        public DbSet<ServiceIndexPageColumnLog> ServiceIndexPageColumnLog { get; set; }
        public DbSet<ServiceIndexPageTemplateLog> ServiceIndexPageTemplateLog { get; set; }
        public DbSet<ServiceTemplateLog> ServiceTemplateLog { get; set; }
        public DbSet<TaskIndexPageColumnLog> TaskIndexPageColumnLog { get; set; }
        public DbSet<TaskIndexPageTemplateLog> TaskIndexPageTemplateLog { get; set; }
        public DbSet<TaskTemplateLog> TaskTemplateLog { get; set; }
        public DbSet<TemplateLog> TemplateLog { get; set; }
        public DbSet<TemplateBusinessLogicLog> TemplateBusinessLogicLog { get; set; }
        public DbSet<TemplateCategoryLog> TemplateCategoryLog { get; set; }
        public DbSet<ColumnMetadataLog> ColumnMetadataLog { get; set; }
        public DbSet<ContextVariableLog> ContextVariableLog { get; set; }
        public DbSet<EditorLog> EditorLog { get; set; }
        public DbSet<EditorTypeLog> EditorTypeLog { get; set; }
        public DbSet<LOVLog> LOVLog { get; set; }
        public DbSet<PageIndexLog> PageIndexLog { get; set; }
        public DbSet<PageIndexColumnLog> PageIndexColumnLog { get; set; }
        public DbSet<PageMemberLog> PageMemberLog { get; set; }
        public DbSet<PageMemberGroupLog> PageMemberGroupLog { get; set; }
        public DbSet<PageNoteLog> PageNoteLog { get; set; }
        public DbSet<PermissionLog> PermissionLog { get; set; }
        public DbSet<PortalLog> PortalLog { get; set; }
        public DbSet<TableMetadataLog> TableMetadataLog { get; set; }

        public DbSet<ProjectEmailSetupLog> ProjectEmailSetupLog { get; set; }
        public DbSet<UserSetLog> UserSetLog { get; set; }
        public DbSet<GrantAccessLog> GrantAccessLog { get; set; }
        public DbSet<NtsTagLog> NtsTagLog { get; set; }
        public DbSet<UserHierarchyPermissionLog> UserHierarchyPermissionLog { get; set; }
        public DbSet<UserEntityPermissionLog> UserEntityPermissionLog { get; set; }
        public DbSet<NtsNoteLog> NtsNoteLog { get; set; }
        public DbSet<NtsTaskLog> NtsTaskLog { get; set; }
        public DbSet<NtsServiceLog> NtsServiceLog { get; set; }
        public DbSet<UserGroupLog> UserGroupLog { get; set; }
        public DbSet<DocumentPermissionLog> DocumentPermissionLog { get; set; }
        public DbSet<UserGroupUserLog> UserGroupUserLog { get; set; }
        public DbSet<MenuGroupDetailsLog> MenuGroupDetailsLog { get; set; }
        public DbSet<PageDetailsLog> PageDetailsLog { get; set; }

        public DbSet<UdfPermissionHeaderLog> UdfPermissionHeaderLog { get; set; }
        public DbSet<NtsLogPageColumnLog> NtsLogPageColumnLog { get; set; }
        public DbSet<CustomIndexPageTemplateLog> CustomIndexPageTemplateLog { get; set; }
        public DbSet<CustomIndexPageColumnLog> CustomIndexPageColumnLog { get; set; }
        public DbSet<NtsRatingLog> NtsRatingLog { get; set; }

        public DbSet<CompanySettingLog> CompanySettingLog { get; set; }
        public DbSet<HybridHierarchyLog> HybridHierarchyLog { get; set; }
        public DbSet<OCRMappingLog> OCRMappingLog { get; set; }

        #endregion
        private void DisableDatabaseInheritence(ModelBuilder builder)
        {
            builder.Entity<CustomIndexPageTemplateLog>().HasBaseType((Type)null);
            builder.Entity<CustomIndexPageColumnLog>().HasBaseType((Type)null);
            builder.Entity<CompanyLog>().HasBaseType((Type)null);
            builder.Entity<EmailLog>().HasBaseType((Type)null);
            builder.Entity<FileLog>().HasBaseType((Type)null);
            builder.Entity<HierarchyMasterLog>().HasBaseType((Type)null);
            builder.Entity<LegalEntityLog>().HasBaseType((Type)null);
            builder.Entity<MemberLog>().HasBaseType((Type)null);
            builder.Entity<MemberGroupLog>().HasBaseType((Type)null);
            builder.Entity<MenuGroupLog>().HasBaseType((Type)null);
            builder.Entity<ModuleLog>().HasBaseType((Type)null);
            builder.Entity<NotificationLog>().HasBaseType((Type)null);
            builder.Entity<SubModuleLog>().HasBaseType((Type)null);
            builder.Entity<TeamLog>().HasBaseType((Type)null);
            builder.Entity<TeamUserLog>().HasBaseType((Type)null);
            builder.Entity<UserLog>().HasBaseType((Type)null);
            builder.Entity<UserDataPermissionLog>().HasBaseType((Type)null);
            builder.Entity<UserHierarchyLog>().HasBaseType((Type)null);
            builder.Entity<UserPagePreferenceLog>().HasBaseType((Type)null);
            builder.Entity<UserPermissionLog>().HasBaseType((Type)null);
            builder.Entity<UserPortalLog>().HasBaseType((Type)null);
            builder.Entity<UserRoleLog>().HasBaseType((Type)null);
            builder.Entity<UserRoleDataPermissionLog>().HasBaseType((Type)null);
            builder.Entity<UserRolePermissionLog>().HasBaseType((Type)null);
            builder.Entity<UserRolePortalLog>().HasBaseType((Type)null);
            builder.Entity<UserRoleStageChildLog>().HasBaseType((Type)null);
            builder.Entity<UserRoleStageParentLog>().HasBaseType((Type)null);
            builder.Entity<UserRoleUserLog>().HasBaseType((Type)null);
            builder.Entity<NtsCategoryLog>().HasBaseType((Type)null);
            builder.Entity<NtsNoteCommentLog>().HasBaseType((Type)null);
            builder.Entity<NtsNotePrecedenceLog>().HasBaseType((Type)null);
            builder.Entity<NtsNoteSequenceLog>().HasBaseType((Type)null);
            builder.Entity<NtsNoteSharedLog>().HasBaseType((Type)null);
            builder.Entity<NtsNoteStatusTrackLog>().HasBaseType((Type)null);
            builder.Entity<NtsServiceCommentLog>().HasBaseType((Type)null);
            builder.Entity<NtsServicePrecedenceLog>().HasBaseType((Type)null);
            builder.Entity<NtsServiceSequenceLog>().HasBaseType((Type)null);
            builder.Entity<NtsServiceSharedLog>().HasBaseType((Type)null);
            builder.Entity<NtsServiceStatusTrackLog>().HasBaseType((Type)null);
            builder.Entity<NtsTaskAttachmentLog>().HasBaseType((Type)null);
            builder.Entity<NtsTaskCommentLog>().HasBaseType((Type)null);
            builder.Entity<NtsTaskPrecedenceLog>().HasBaseType((Type)null);
            builder.Entity<NtsTaskSequenceLog>().HasBaseType((Type)null);
            builder.Entity<NtsTaskSharedLog>().HasBaseType((Type)null);
            builder.Entity<NtsTaskStatusTrackLog>().HasBaseType((Type)null);
            builder.Entity<NtsTaskTimeEntryLog>().HasBaseType((Type)null);
            builder.Entity<UdfPermissionLog>().HasBaseType((Type)null);
            builder.Entity<AdhocTaskComponentLog>().HasBaseType((Type)null);
            builder.Entity<BusinessExecutionComponentLog>().HasBaseType((Type)null);
            builder.Entity<BusinessLogicComponentLog>().HasBaseType((Type)null);
            builder.Entity<CompleteEventComponentLog>().HasBaseType((Type)null);
            builder.Entity<ComponentLog>().HasBaseType((Type)null);
            builder.Entity<ComponentParentLog>().HasBaseType((Type)null);
            builder.Entity<ComponentResultLog>().HasBaseType((Type)null);
            builder.Entity<DecisionScriptComponentLog>().HasBaseType((Type)null);
            builder.Entity<EmailComponentLog>().HasBaseType((Type)null);
            builder.Entity<ExecutionScriptComponentLog>().HasBaseType((Type)null);
            builder.Entity<FalseComponentLog>().HasBaseType((Type)null);
            builder.Entity<ProcessDesignLog>().HasBaseType((Type)null);
            builder.Entity<ProcessDesignComponentLog>().HasBaseType((Type)null);
            builder.Entity<ProcessDesignResultLog>().HasBaseType((Type)null);
            builder.Entity<ProcessDesignVariableLog>().HasBaseType((Type)null);
            builder.Entity<StartEventComponentLog>().HasBaseType((Type)null);
            builder.Entity<StepTaskComponentLog>().HasBaseType((Type)null);
            builder.Entity<TrueComponentLog>().HasBaseType((Type)null);
            builder.Entity<CustomTemplateLog>().HasBaseType((Type)null);
            builder.Entity<FormIndexPageColumnLog>().HasBaseType((Type)null);
            builder.Entity<FormIndexPageTemplateLog>().HasBaseType((Type)null);
            builder.Entity<FormTemplateLog>().HasBaseType((Type)null);
            builder.Entity<NoteIndexPageColumnLog>().HasBaseType((Type)null);
            builder.Entity<NoteIndexPageTemplateLog>().HasBaseType((Type)null);
            builder.Entity<NoteTemplateLog>().HasBaseType((Type)null);
            builder.Entity<NotificationTemplateLog>().HasBaseType((Type)null);
            builder.Entity<PageTemplateLog>().HasBaseType((Type)null);
            builder.Entity<ServiceIndexPageColumnLog>().HasBaseType((Type)null);
            builder.Entity<ServiceIndexPageTemplateLog>().HasBaseType((Type)null);
            builder.Entity<ServiceTemplateLog>().HasBaseType((Type)null);
            builder.Entity<TaskIndexPageColumnLog>().HasBaseType((Type)null);
            builder.Entity<TaskIndexPageTemplateLog>().HasBaseType((Type)null);
            builder.Entity<TaskTemplateLog>().HasBaseType((Type)null);
            builder.Entity<TemplateLog>().HasBaseType((Type)null);
            builder.Entity<TemplateBusinessLogicLog>().HasBaseType((Type)null);
            builder.Entity<TemplateCategoryLog>().HasBaseType((Type)null);
            builder.Entity<ColumnMetadataLog>().HasBaseType((Type)null);
            builder.Entity<ContextVariableLog>().HasBaseType((Type)null);
            builder.Entity<EditorLog>().HasBaseType((Type)null);
            builder.Entity<EditorTypeLog>().HasBaseType((Type)null);
            builder.Entity<LOVLog>().HasBaseType((Type)null);
            builder.Entity<PageIndexLog>().HasBaseType((Type)null);
            builder.Entity<PageIndexColumnLog>().HasBaseType((Type)null);
            builder.Entity<PageMemberLog>().HasBaseType((Type)null);
            builder.Entity<PageMemberGroupLog>().HasBaseType((Type)null);
            builder.Entity<PageNoteLog>().HasBaseType((Type)null);
            builder.Entity<PermissionLog>().HasBaseType((Type)null);
            builder.Entity<PortalLog>().HasBaseType((Type)null);
            builder.Entity<TableMetadataLog>().HasBaseType((Type)null);
            builder.Entity<UserSetLog>().HasBaseType((Type)null);
            builder.Entity<GrantAccessLog>().HasBaseType((Type)null);
            builder.Entity<NtsTagLog>().HasBaseType((Type)null);
            builder.Entity<UserHierarchyPermissionLog>().HasBaseType((Type)null);
            builder.Entity<UserEntityPermissionLog>().HasBaseType((Type)null);
            builder.Entity<NtsNoteLog>().HasBaseType((Type)null);
            builder.Entity<NtsTaskLog>().HasBaseType((Type)null);
            builder.Entity<NtsServiceLog>().HasBaseType((Type)null);
            builder.Entity<UserGroupLog>().HasBaseType((Type)null);
            builder.Entity<DocumentPermissionLog>().HasBaseType((Type)null);
            builder.Entity<UserGroupUserLog>().HasBaseType((Type)null);
            builder.Entity<MenuGroupDetailsLog>().HasBaseType((Type)null);
            builder.Entity<PageDetailsLog>().HasBaseType((Type)null);
            builder.Entity<UdfPermissionHeaderLog>().HasBaseType((Type)null);
            builder.Entity<NtsLogPageColumnLog>().HasBaseType((Type)null);
            builder.Entity<NtsRatingLog>().HasBaseType((Type)null);
            builder.Entity<CompanySettingLog>().HasBaseType((Type)null);
            builder.Entity<HybridHierarchyLog>().HasBaseType((Type)null);
            builder.Entity<OCRMappingLog>().HasBaseType((Type)null);
        }
    }

}

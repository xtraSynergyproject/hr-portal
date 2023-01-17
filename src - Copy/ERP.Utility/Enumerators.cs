using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System;

namespace ERP.Utility
{
    public enum StatusEnum
    {
        Active,
        Inactive
    }
    public enum ActionEnum
    {
        Copy = 1,
        Move = 2
    }
    public enum LicenseValidityEnum
    {
        Inavlid = 0,
        Expired = 1,
        Valid = 2
    }
    public enum NtsModifiedStatusEnum
    {
        Created = 0,
        Modified = 1,
        Locked = 2,
        Deleted=3,
        Archived=4,
        RestoredDeleted=5,
        RestoredArchived = 6,
        SyncError = 7,
        None=8
    }
    public enum SyncStageStatusEnum
    {
        Folders = 0,
        Documents = 1,
        Services = 2,
        Tasks = 3,
        PullScript=4,
        PullDelRel=5,
        PatchScript=6,
        Completed=7
    }
    public enum SLARequestStatusEnum
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
    public enum ProjectTypeEnum
    {
        CayanProject = 0,
        Others = 1
    }
    public enum WbsViewEnum
    {
        Treeview = 0,
        Gridview = 1
    }
    public enum SalProjectTypeEnum
    {
        [Display(Name = "Sales Cayan Project")]
        [Description("Sales Cayan Project")]
        SalesCayanProject = 0,
        [Display(Name = "Sales Non Cayan Project")]
        [Description("Sales Non Cayan Project")]
        SalesNonCayanProject = 1,
        [Display(Name = "Engineering Project")]
        [Description("Engineering Project")]
        EngineeringProject = 2
    }

    public enum ItemStatusEnum
    {
        Active,
        StatusInactive,
        DatedInactive,
        FutureDated,
    }
    public enum ImportActionType
    {
        Create,
        Edit,
        DoNothing
    }
    public enum ApprovalStatusEnum : int
    {
        Approved = 0,
        Rejected = 1,
        Cancelled = 2
    }
    public enum CommunicationTypeEnum : int
    {
        Ethernet = 0
    }
    public enum PunchingTypeEnum : int
    {
        [Description("Sign In")]
        Checkin = 0,
        [Description("Sign Out")]
        Checkout = 1,
        Both = 2
    }

    public enum AccessLogSourceEnum : int
    {
        Biometric = 0,
        Mobile = 1,
        Service = 2,
        Manual = 3,
    }


    public enum SpsTrainingCategoryEnum : int
    {
        People = 0,
        [Description("Quality And Consistency")]
        QualityAndConsistency = 1,
        [Description("Cost Control")]
        CostControl = 2,
        [Description("Revenue Generation")]
        RevenueGeneration = 3,
        [Description("Pre Openings")]
        PreOpenings = 4,
        [Description("Business Development")]
        BusinessDevelopment = 5,
        [Description("Concept Development And Design")]
        ConceptDevelopmentAndDesign = 6
    }

    public enum PayrollPostedStatusEnum : int
    {
        Submitted = 0,
        Posted = 1
    }
    public enum VisaTypeEnum : int
    {
        [Description("Residence Visa")]
        ResidenceVisa = 0,
        [Description("Business Visa")]
        BusinessVisa = 1,
        [Description("Visit Visa")]
        VisitVisa = 2,
        Other = 3
    }
    public enum VisaEntryTypeEnum : int
    {
        Single = 0,
        Multiple = 1
    }

    public enum OrganizationCategoryEnum : int
    {
        [Description("Department")]
        Organization = 0,
        Brand = 1,
        //Restaurant = 2,
        LegalEntity = 3,
        Corporate = 4,
        //Sale = 5,
        Company = 6,
    }
    public enum RestaurantTypeEnum : int
    {
        QuickServiceRestaurant = 0,
        FastCasual = 1,
        Casual = 2,
        Upscale = 3,
        Retail = 4,
        Delivery = 5
    }

    public enum PersonTitleEnum : int
    {
        Mr = 0,
        Mrs = 1,
        Ms = 2,
        Miss = 3,
        Dr = 4,
    }
    public enum GenderEnum : int
    {
        Male = 0,
        Female = 1,
    }
    public enum EosTypeEnum : int
    {
        Resignation = 0,
        Termination = 1,
    }
    public enum EmployeeStatusEnum : int
    {
        Active = 0,
        Terminated = 1,
        Resigned = 2,
        EndOfContract = 3,
        Inactive = 4,
        NoticePeriod = 5,
    }
    public enum EmployeeTransactionEnum : int
    {
        [Display(Name = "Assignment Update")]
        [Description("Assignment Update")]
        AssignmentUpdate = 0,
        [Display(Name = "Salary Update")]
        [Description("Salary Update")]
        SalaryUpdate = 1,
        [Display(Name = "End Of Salary")]
        [Description("End Of Salary")]
        EndOfSalary = 2,
        [Display(Name = "Hiring Information")]
        [Description("Hiring Information")]
        HiringInformation = 3,
    }
    public enum SkillTypeEnum : int
    {
        Communication = 0,
        IT = 1,
        HR = 2,
        Other = 3
    }
    public enum MaritalStatusEnum : int
    {
        Single = 0,
        Married = 1,
        Divorcee = 2,
        Widow = 3,
        Widower = 4,
    }
    public enum GuestTypeEnum : int
    {
        Spouse = 0,
        Kid = 1,
        Roommate = 2,
        Relative = 3,
        Maid = 4,
    }
    public enum PetTypeEnum : int
    {
        Dog = 0,
        Cat = 1,
        Bird = 2,
        Other = 3,
    }
    public enum VehicleTypeEnum : int
    {
        Automobile = 0,
        Motorcycle = 1,
        Truck = 2,
        Van = 3,
    }
    public enum ReligionEnum : int
    {
        Hindu = 0,
        Christian = 1,
        Islam = 2,
        Other = 3,
    }

    public enum RelationshipTypeEnum : int
    {
        Husband = 0,
        Wife = 1,
        Son = 2,
        Daughter = 3,
        Father = 4,
        Mother = 5,
        Other = 6,
        Brother = 7,
        Sister = 8
    }
    public enum DependantRelationshipTypeEnum : int
    {
        Husband = 0,
        Wife = 1,
        Son = 2,
        Daughter = 3,
        Father = 4,
        Mother = 5,
    }
    public enum RelativeTypeEnum : int
    {
        Relative = 0,
        NonRelative = 1,
    }
    public enum IDTypeEnum : int
    {
        IqamahID = 0,
        NationalID = 1,
        UAEID = 2,
    }
    public enum DocumentOwnerTypeEnum : int
    {
        Self = 0,
        Dependent = 1,
    }

    public enum DelimeterEnum : int
    {
        Space = 0,
        Comma = 1,
        SemiColon = 2,
        Pipe = 3
    }
    public enum ContractTypeEnum : int
    {
        [Display(Name = "Limited Contract")]
        [Description("Limited Contract")]
        LimitedContract = 0,
        [Display(Name = "OutSource")]
        [Description("OutSource")]
        OutSource = 1,
        [Display(Name = "Unlimited Contract")]
        [Description("Unlimited Contract")]
        UnlimitedContract = 2,
        [Display(Name = "Trainee")]
        [Description("Trainee")]
        Trainee = 3,
    }
    public enum JobTypeEnum : int
    {
        Contract = 0,
        Permanent = 1
    }
    public enum JobRequestStateEnum : int
    {
        Draft = 0,
        Posted = 1,
        UnPosted = 2,
        Hiring = 3,
    }
    public enum HiringStatusEnum : int
    {
        //NotStarted = 0,
        Hired = 0,
        Rejected = 1,
    }
    public enum JobRequestStatusEnum : int
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2,
    }
    public enum JobRequestCriteriaTypeEnum : int
    {
        YesOrNo = 0,
        Scale = 1
    }

    public enum RecruitmentApplicationStatusEnum : int
    {
        Applied = 0,
        Withdrawn = 1
    }
    public enum RecruitmentEmailStatusEnum : int
    {
        NotStarted = 0
    }
    public enum ElementTypeEnum : int
    {
        Cash = 0,
        Info = 1,
        Accrual = 2
    }
    public enum ElementEntryTypeEnum : int
    {
        SingleEntry = 0,
        DailyEntry = 1
    }
    public enum ElementValueTypeEnum : int
    {
        Value = 0,
        Percentage = 1
    }
    public enum UserStatusEnum : int
    {
        Active = 0,
        Inactive = 1
    }
    public enum PersonStatusEnum : int
    {
        Active = 0,
        Inactive = 1
    }
    public enum NoteStatusEnum : int
    {
        Active = 0,
        Inactive = 1
    }
    public enum TaskStatusEnum : int
    {
        Active = 0,
        Inactive = 1
    }
    public enum BooleanStatus : int
    {
        False = 0,
        True = 1
    }
    public enum TaskSearchEnum : int
    {
        [Description("Draft")]
        DRAFT = 0,
        [Description("Canceled")]
        CANCELED = 1,
        [Description("Completed")]
        COMPLETED = 2,
        [Description("Over Due")]
        OVER_DUE = 3,
        [Description("In Progress")]
        IN_PROGRESS = 4,
        [Description("Pending")]
        PENDING = 5,
        [Description("Rejected")]
        REJECTED = 6
    }
    public enum PaymentModeEnum : int
    {
        [Display(Name = "Bank Transfer")]
        [Description("Bank Transfer")]
        BankTransfer = 0,
        Cheque = 1,
        Cash = 2
    }
    public enum QuotationPaymentModeEnum : int
    {
        Cash = 1,
        Cheque = 2,

    }
    public enum OTPaymentTypeEnum : int
    {
        Pay = 0,
        TimeOff = 1
    }
    public enum PayrollIntervalEnum : int
    {
        Monthly = 0
        //,        Weekly = 1
    }
    public enum FlightTicketFrequentEnum : int
    {
        [Display(Name = "One per year")]
        One = 1,
        [Display(Name = "Once in two years")]
        Two = 2,
    }
    public enum PayrollRunTypeEnum : int
    {
        Salary = 0,
        Adhoc = 1
    }
    public enum PayrollProcessStatusEnum : int
    {
        NotProcessed = 0,
        Processed = 1,
        Draft = 2,
    }
    public enum PayrollPostedSourceEnum : int
    {
        Service = 0,
        Manual = 1,
        // Salary = 2,
        Payroll = 3,
        Accrual = 4
    }
    public enum PayrollUomEnum : int
    {
        Hour = 0,
        Day = 1,
        Week = 2,
        Month = 3,
        Other = 4,

    }

    public enum PayrollExecutionStatusEnum : int
    {
        NotStarted = 0,
        Submitted = 1,
        InProgress = 2,
        Completed = 3,
        Error = 4
    }
    public enum PayrollStatusEnum : int
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2
    }
    public enum ExecutionStatusEnum : int
    {
        Success = 0,
        Error = 1
    }


    public enum PayrollStateEnum : int
    {
        NotStarted = 0,
        ExecutePayroll = 1,
        RollBack = 2,
        InitiateService = 3,
        FreezePayroll = 4,
        GeneratePayslip = 5,
        PrepareBankLetter = 6,
        PublishPayroll = 7,
        PostToAccounting = 8,
        ClosePayroll = 9,
        AddToPayroll = 10

    }
    public enum PayrollRunDateTypeEnum : int
    {
        LastDayOfMonth = 0,
        Custom = 1
    }
    public enum PayrollRunModeEnum : int
    {
        Manual = 0
    }

    public enum ElementCategoryEnum : int
    {
        Standard = 0,
        NonStandard = 1
    }
    public enum ElementClassificationEnum : int
    {
        Earning = 0,
        Deduction = 1,
        Information = 3
    }

    public enum DocumentStatusEnum : int
    {
        Draft = 0,
        Published = 1,

    }
    public enum EmailCampaignDeliveryStatusEnum : int
    {
        [Display(Name = "Not Started")]
        [Description("Not Started")]
        NotStarted = 0,
        InProgress = 1,
        Completed = 2,
        Error = 3
    }


    public enum PerformanceDocumentStatusEnum : int
    {
        Closed = 0,
        Active = 1
    }
    public enum PerformanceDocumentTargetEnum : int
    {
        Person = 0,
        Organization = 1
    }
    public enum PerformanceDocumentDurationEnum : int
    {
        Yearly = 0,
        Quarterly = 1,
        Monthly = 2,
        HalfYearly = 3
    }


    public enum SuccessionPlanningStatusEnum : int
    {
        Closed = 0,
        Active = 1
    }
    public enum SuccessionPlanningTargetEnum : int
    {
        Person = 0,
        Organization = 1
    }
    public enum SuccessionPlanningDurationEnum : int
    {
        Yearly = 0,
        Quarterly = 1,
        Monthly = 2,
        HalfYearly = 3
    }

    public enum ApplicationEnvironmentEnum : int
    {
        DEV = 0,
        TEST = 1,
        UAT = 2,
        PROD = 3
    }
    public enum ApplicationEnum : int
    {
        Synergy = 0,
        Career = 1
    }
    public enum BoolStatus : int
    {
        No = 0,
        Yes = 4,
    }
    public enum ErrorTypeEnum : int
    {
        Error = 0,
        Confirm = 1,
        Warning = 2
    }

    public enum GridSelectOption : int
    {
        Disable = 0,
        Single = 1,
        Multiple = 2
    }
    public enum SearchType : int
    {
        All = 0,
        Active = 1
    }

    public enum IncludeExclude : int
    {
        Include = 0,
        Exclude = 1
    }

    public enum ControlHierarchy : int
    {
        Module = 0,
        SubModule = 1,
        Screen = 2,
        Tab = 3,
        Block = 4,
        Action = 5,
        Data = 6
    }
    public enum AccessRuleType : int
    {
        Grade = 0,
        Organization = 1,
        Job = 2,
        Position = 3,
        Employe4

    }
    public enum TransactionMode : int
    {
        Insert = 0,
        Update = 1,
        Delete = 2,
        Close = 3,
        Correction = 4,
        Open = 5,
        Rename = 6
    }

    public enum PageViewMode : int
    {
        Position = 0,
        Organization = 1
    }
    public enum ScheduleMonthlyTypeEnum : int
    {
        Day = 0,
        Week = 1
    }
    //public enum MonthNameEnum : int
    //{
    //    January = 1,
    //    February = 2,
    //    MArch = 2,
    //}
    //
    // Summary:

    public enum WeekDay : int
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }
    public enum DocumentApprovalStatuTypeEnum : int
    {
        [Description("Approve Through DMS")]
        ApproveThroughDMS = 0,
        [Description("Manually Approved")]
        ManuallyApproved = 1,
        [Description("Approval Not Required")]
        ApprovalNotRequired = 2
    }
    public enum DocumentApprovalStatusEnum : int
    {
        [Description("Approval Not Required")]
        ApprovalNotRequired = 0,
        [Description("Approved Manually")]
        ApprovedManually = 1,
        [Description("Unapproved")]
        Unapproved = 2,
        [Description("Approval In Progress")]
        ApprovalInProgress = 3,
        [Description("Approved")]
        Approved = 4,
        [Description("Rejected")]
        Rejected = 5
    }
    public enum StageStatuTypeEnum : int
    {
        [Description("Galfar")]
        Galfar = 0,
        [Description("QP")]
        QP = 1,
        [Description("Technip")]
        Technip = 2,
        [Description("Completed")]
        Completed = 3,
        [Description("Closed")]
        Closed = 4,
        [Description("Re Issued")]
        ReIssued = 5,
        [Description("Cancelled")]
        Cancelled = 6,
        [Description("Internal Review")]
        InternalReview = 7,
        [Description("Vendor")]
        Vendor = 8,
        [Description("Return")]
        Return = 9,
    }
    public enum MonthEnum : int
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12,
    }
    public enum RecurrencePatternEnum : int
    {
        Daily = 0,
        Weekly = 1,
        Monthly = 2,
        Yearly = 3
    }
    public enum DailyRecurrenceTypeEnum : int
    {
        EveryDay = 0,
        WeekDay = 1
    }
    public enum YearlyRecurrenceTypeEnum : int
    {
        Monthly = 0,
        WeekDayOfMonth = 1
    }
    public enum ScheduleEndTypeEnum : int
    {
        NoEndDate = 0,
        EndDateAfter = 1,
        EndBy = 2
    }
    public enum MonthlyRecurrenceTypeEnum : int
    {
        Day = 0,
        WeekDay = 1
    }
    public enum ClockServerSchedulerActionEnum : int
    {
        DownloadLog = 0,
        DownloadUser = 1,
        MapUserToDevice = 2
    }
    public enum PayrollSchedulerActionEnum : int
    {
        ExecutePayroll = 0,
        LoadDailySalaryEntry = 1
    }
    public enum ServiceSchedulerActionEnum : int
    {
        DailySystemUpdate = 0,
        //  BiometricIntegration = 1,
        MinutelySystemUpdate = 2,
        NtsBackgroundCreation = 3,
        NtsRecurrenceAndReminder = 4,
        DailySystemBackup = 5
    }


    public enum LicenseTypeEnum : int
    {
        Subscription = 0,
        Perpetual = 1,
    }
    public enum ScheduleTypeEnum : int
    {
        OneTime = 0,
        Daily = 1,
        Hourly = 2,
        Minutely = 3,
        Weekly = 4,
        Monthly = 5,
        Manual = 6,
        None = 7
    }
    public enum ScheduleCategoryEnum : int
    {
        Schedule = 0,
        Task = 1
    }
    public enum DeviceTypeEnum : int
    {
        Biometric = 0,
        Mobile = 1
    }

    public enum ScheduleExecutionStatusEnum : int
    {
        Unknown = 0,
        Open = 1,
        Running = 2,
        Succeeded = 3,
        Error = 4
    }
    public enum AccessLogTypeEnum : int
    {
        SessionStart = 0,
        SessionEnd = 1,
        UnauthorizedAccess = 2,
        Other = 3,
        Login = 4,
    }

    public enum DataOperation : int
    {
        Create = 0,
        Update = 1,
        Correct = 2,
        Delete = 3,
        Read = 4
    }
    public enum FilePreviewType : int
    {
        Pdf = 0,
        Image = 1,
        Other = 2
    }

    public enum SupplierTypeEnum : int
    {
        Domestic = 0,
        Overseas = 1
    }
    public enum DiscountTypeEnum : int
    {
        Percentge = 0,
        [EnumHidden]
        Flat = 1
    }
    public enum CustomerTypeEnum : int
    {
        Customer = 0,
        Reseller = 1,
        Supplier = 2
    }
    public enum VatTypeEnum : int
    {
        Percentge = 0,
        Flat = 1
    }

    public enum DataOperationEvent : int
    {
        MetadataView = 0,
        DocumentShare = 1,
        DocumentPreview = 2,
        DocumentDownload = 3,
        Archive = 4,
        Delete = 5,
        Copy = 6,
        Move = 7,
        CheckIn = 8,
        CheckOut = 9,
        MetadataInsert = 10,
        PermissionAdded = 11,
        PermissionEdited = 12,
        PermissionDeleted = 13,
        DisabledInheritance = 14,
        EnableInheritance = 15,
        TagAssigned = 16,
        TagRemoved = 17,
        Create = 18,
        Edit = 19,
        Update = 20,
        View = 21,
        Login = 22,
        Logoff = 23,
        Restore = 24,
        Reply = 25,

    }
    public enum MedicalCardTypeEnum : int
    {
        Gold = 0,
        Silver = 1,
        Bronze = 2
    }
    public enum TravelClassEnum : int
    {
        Business = 0,
        Economy = 1
    }

    public enum Mode : int
    {
        Normal = 0,
        Select = 1
    }
    public enum SelectionMode : int
    {
        Single = 0,
        Multiple = 1
    }
    public enum ChartViewType : int
    {
        PositionPhoto = 0,
        PositionNormal = 1,
        PositionTree = 2,
        PositionExcel = 3,
        OrgNormal = 4,
        OrgTree = 5,
        OrgExcel = 6,
        TopNode = 7,
        MyNode = 8
    }
    [Serializable]
    public enum NtsUserTypeEnum : int
    {
        Owner = 0,
        Assignee = 1,
        Shared = 2,
        All = 3,
        StepServiceOwner = 4,
        ServiceStepAssignee = 5,
        Requester = 6,
        ManualUser = 7,
        Holder = 8,
        PermittedUser = 9,
    }
    public enum FieldDisplayModeEnum : int
    {
        Editable = 0,
        Readonly = 1,
        View = 2
    }


    public enum TaskCreationMode : int
    {
        Automatic = 0,
        Manual = 1
    }

    public enum AssignedTypeEnum : int
    {
        Team = 0,
        User = 1,
    }

    public enum AssignToTypeEnum : int
    {
        User = 0,
        Query = 1,
        Team = 2,
        Organization = 3,
        ApprovalHierarchy = 4,
        DynamicMethod = 5
    }
    public enum AssignedQueryTypeEnum : int
    {
        Position = 0,
        Person = 1,
        User = 2,
        Team = 3
    }
    public enum LockStatusEnum : int
    {
        CheckedIn = 0,
        CheckedOut = 1
    }
    public enum SearchTypeEnum : int
    {
        [Description("Smart Search")]
        Fuzzy = 0,
        [Description("Plain Search")]
        Plain = 1
    }
    public enum AppliedToEnum : int
    {
        Metadata = 0,
        Content = 1,
        //Comments=2
    }
    public enum NoteReferenceTypeEnum : int
    {
        // Position,
        Self = 0,
        User = 1,
        Person = 2,
        Position = 3,
        Job = 4,
        [Description("Department")]
        Organization = 5,
        Team = 6,
        Project = 7,
        Dependant = 8,
    }


    public enum RatingTypeEnum : int
    {
        System = 0,
        User = 1
    }


    public enum FieldInfoTypeEnum : int
    {
        Label = 0,
        Tooltip = 1,
        Help = 2
    }

    public enum NotificationStatusEnum : int
    {
        NotSent = 0,
        Sent = 1,
        Error = 2,
        Cancelled = 3
    }
    public enum PersonNumberGenerationTypeEnum : int
    {
        SequenceNo = 0,
        DateOfJoinSequence = 1,
        Manual = 2
    }

    public enum EmailStatusEnum : int
    {
        NotSent = 0,
        Sent = 1,
        Error = 2,
        Cancelled = 3
    }
    //public enum SmsStatusEnum : int
    //{
    //    NotSent = 0,
    //    Sent = 1,
    //    Error = 2,
    //    Cancelled = 3 
    //}


    public enum ReadStatusEnum : int
    {
        NotRead = 0,
        Read = 1
    }

    public enum TeamTypeEnum : int
    {
        General = 0,
        AdminTeam = 1,
        SupportTeam = 2,
        SupportRootTeam = 3
    }

    public enum ClientUserTypeEnum : int
    {
        NetWorkCredential = 0,
        WindowsAccount = 1,
        MachineName = 2
    }
    public enum WorkAssignmentTypeEnum : int
    {
        AssignToOwner = 0,
        RoundRobin = 1,
        LeastAssigned = 2
    }
    public enum NtsTypeEnum : int
    {
        Note = 0,
        Task = 1,
        Service = 2
        // ServicePlus = 3
        //NoteComment = 3,
        //TaskComment = 4,
        //ServiceComment = 5,
    }
    public enum PullTypeEnum : int
    {
        Script = 0,
        DeleteRelScript = 1
       
    }
    public enum TemplateMasterTypeEnum : int
    {
        Note = 0,
        Task = 1,
        Service = 2,
        Document = 3
    }
    public enum WorkflowStageStepTypeEnum : int
    {
        Stage = 0,
        Step = 1,
    }
    public enum TaskPlanEnum : int
    {
        ToDo = 0,
        Today = 1,
        Tomorrow = 2,
        Future = 3,
        History = 4

    }

    public enum GridViewModeEnum : int
    {
        Grid = 0,
        List = 1
    }

    public enum NtsLockStatusEnum : int
    {
        Released = 0,
        Locked = 1,
    }
    public enum CareerGenderEnum : int
    {
        Male = 0,
        Female = 1,
    }
    public enum CareerMaritalStatusEnum : byte
    {

        Single = 0,
        Married = 1,
        Divorced = 2,
        Widowed = 3,
        Divorcedwithdependant = 4,
        Widower = 5,
        Widowerwithdependant = 6
    }
    public enum AttachmentTypeEnum : int
    {
        File = 0,
        Note = 1,
        Task = 2,
        Service = 3
    }
    public enum EDRMDRFileTypeEnum : int
    {
        EDR = 0,
        MDR = 1,
    }
    public enum Language : int
    {
        English = 0,
        Arabic = 1
    }
    public enum ReportLanguageEnum : int
    {
        English = 0,
        Arabic = 1,
        Combined = 2
    }

    public enum NodeEnum : int
    {
        NTS_Note = 0,
        NTS_Task = 1,
        NTS_Service = 2,
        PMS_PerformanceDocument = 3,
        //PMS_Competency = 4,
        SPS_SuccessionPlanning = 5,
        HRS_Grade = 6,
        HRS_Job = 7,
        HRS_Organization = 8,
        HRS_Position = 9,
        HRS_Assignment = 10,
        HRS_Person = 11,
        GEN_Attachment = 12,
        NTS_NoteComment = 13,
        NTS_TaskComment = 14,
        NTS_ServiceComment = 15,
        HRS_GradeRoot = 16,
        HRS_JobRoot = 17,
        HRS_OrganizationRoot = 18,
        HRS_PositionRoot = 19,
        HRS_AssignmentRoot = 20,
        HRS_PersonRoot = 21,
        ADM_User = 22,
        PMT_ProjectManagement = 23,
        ADM_Team = 24,
        TAA_Attendance = 25,
        REC_Candidate = 26,
        REC_RecruitmentProcess = 27,
        SAL_TokenMoney = 28,
        REC_Application = 29,
        SAL_Unit = 30,
        SAL_Lead = 31,
        REC_JobRequest = 32,
        HRS_DependentRoot = 34,
        PAY_PayrollRun = 35,
        CLK_AccessLog = 36,
        PAY_PayrollTransaction = 37,
        GEN_ListOfValue = 38,
        HRS_Sponsor = 39,
        ADM_LegalEntity = 40,
        NTS_TemplateMaster = 41,
        ADM_WebinarRegistration = 42,
        NTS_NotePermission = 43,
        GEN_File = 44,
        NTS_NoteTagView = 45,
        ADM_WorkspacePermissionGroup = 46,
        GEN_Notification = 47,
        ADM_License = 48,
        ADM_UserRole = 49,
        ADM_Module = 50,
        GEN_Company = 51,
        HRS_Dependent = 52,
    }

    public enum SharingModeEnum : int
    {
        System = 0,
        Manual = 1
    }
    public enum ParentServiceTypeEnum : int
    {
        Parent = 0,
        LeaveReturnToWork = 1,
        LeaveCancel = 2,
        LeaveHandOver = 3
    }
    public enum ProjectTaskTypeEnum
    {
        Main = 1,
        SubTask = 2,
        Email = 3,
        Group = 4,
    }
    public enum QuotationItemTypeEnum : int
    {
        Fixed = 0,
        Temporary = 1
    }
    public enum TemplateLabelPropertyEnum : int
    {

    }
    public enum TemplateFieldLabelPropertyEnum : int
    {
        LabelDisplayName = 0,
        AdditionalInfo = 1,
        HelpInfo = 2,
        PoupTitle = 3,
        Tooltip = 4,
    }
    public enum ReferenceTypeEnum : int
    {
        NTS_Note = 0,
        NTS_Task = 1,
        NTS_Service = 2,
        PMS_Goal = 3,
        PMS_Competency = 4,
        SPS_SuccessionCycle = 5,
        HRS_Grade = 6,
        HRS_Job = 7,
        HRS_Organization = 8,
        HRS_Position = 9,
        HRS_Assignment = 10,
        HRS_Person = 11,
        HRS_PersonPhoto = 12,
        NTS_NoteComment = 13,
        NTS_TaskComment = 14,
        NTS_ServiceComment = 15,
        PMS_PeerReview = 16,
        SPS_DevelopmentPlan = 17,
        NTS_NoteSchedule = 18,
        NTS_NoteReminder = 19,
        ADM_User = 20,
        HRS_PersonPhotoResized = 21,
        PMT_ProjectManagement = 22,
        GEN_Schedule_Reminder = 23,
        GEN_Schedule_Recurrence = 24,
        TAA_Attendance = 25,
        REC_Candidate_CV = 26,
        REC_Candidate_Photo = 27,
        REC_RecruitmentProcess = 28,
        SAL_TokenMoney = 29,
        REC_InterviewAssessment = 30,
        REC_JobOffer = 31,
        REC_Application = 32,
        REC_ApplicationShortlist = 33,
        REC_JobRequest = 34,
        REC_CollectDocument = 35,
        REC_ContractNDA = 36,
        REC_OnBoard = 37,
        REC_WelcomePack = 38,
        REJECTION_LETTER = 39,
        SAL_Lead = 40,
        REC_Application_CV = 41,
        REC_Application_Photo = 42,
        ADM_TeamAttachment = 43,
        HRS_PersonDocument = 44,
        HRS_DependentDocument = 45,
        PAY_PayrollRun = 46,
        REC_CandidateEducation = 47,
        REC_CandidateExperience = 48,
        REC_CandidateTraining = 49,
        REC_ApplicationEducation = 50,
        REC_ApplicationExperience = 51,
        REC_ApplicationTraining = 52,
        HRS_Dependent = 53,
        SEP_EosService = 54,
        CLK_AccessLog = 55,
        SAL_Comission = 56,
        CPM_Leasing = 57,
        PAY_PayrollTransaction = 58,
        GEN_ListOfValue = 59,
        HRS_SponsorLogo = 60,
        HRS_SponsorLogoResized = 61,
        ADM_LegalEntity = 62,
        NTS_TemplateMaster = 63,
        GEN_TagTo = 64,
        ADM_WebinarRegistration = 65,
        ADM_UserPhoto = 66,
        SEP_ClearanceForm = 67,
        ADM_UserIdentification = 68,
        ADM_TechnicalAssessmentRecord = 69,
        ADM_CaseStudyAssessmentRecord = 70,
        ADM_InterviewRecord = 71,
        CPM_Owner = 72,
        NTS_EmailFolder,

    }
    public enum TagTypeEnum : int
    {
        Static = 0,
        Master = 1,
        Integrated = 2,
    }
    public enum NtsScheduleEnum : int
    {
        Reminder = 0,
        Nts = 1
    }
    public enum ScheduleEnum : int
    {
        Reminder = 0,
        Recurrence = 1
    }
    public enum DataIncludeExcludeEnum : int
    {
        Include = 0,
        Exclude = 1
    }
    public enum DependencyEnum : int
    {
        Predecessor = 0,
        Successor = 1,
    }

    public enum NtsClassificationEnum : int
    {
        Standard = 0,
        Adhoc = 1,
        Step = 2,
        Other = 3
    }
    public enum NtsServiceTaskTypeEnum : int
    {
        StepTask = 0,
        AdhocTask = 1
    }
    public enum NtsServicePlusServiceTypeEnum : int
    {
        StepService = 0,
        AdhocService = 1
    }
    public enum SLACalculationMode : int
    {
        Default = 0,
        BackwardServiceDueDate = 1
    }
    public enum RosterDutyTypeEnum : int
    {
        Pattern = 0,
        Custom = 1,
        DayOff = 2,
        PublicHoliday = 3
    }
    public enum AttendanceTypeEnum : int
    {
        Absent = 0,
        Present = 1,
    }
    public enum AttendanceLeaveTypeEnum : int
    {
        UnpaidLeave = 0,
        SickLeave = 1,
        AnnualLeave = 2,
        UnderTime = 3,
        AnnualLeaveHalfDay = 4,
        OtherLeave = 5,
    }

    public enum AddDeductEnum : int
    {
        Add = 0,
        Deduct = 1
    }
    public enum HalfDayLeaveType : int
    {
        Morning = 0,
        AfterNoon = 1
    }

    public enum ServiceNoPrefixEnum : int
    {
        S = 0,
        RS = 1
    }
    public enum QuotationTypeEnum : int
    {
        [Description("SalesQuotation")]
        SalesQuotation = 0,
        [Description("Purchase Order")]
        ProcurementQuotation = 1,
        [Description("Additional Work")]
        AdditionalWork = 2,
        [Description("Installation Quotation")]
        InstallationQuotation = 3,
        [Description("Maintenance Contract")]
        MaintenanceContract = 4,
        [Description("Starting Form")]
        StartingForm = 5,
        [Description("Sales Invoice")]
        Invoice = 6,
        [Description("Payment Receipt")]
        PaymentReceipt = 7,
        [Description("Delivery Note")]
        DeliveryNote = 8,
        [Description("Wholesale Quotation")]
        WholesaleQuotation = 9,
        [Description("Customer Delivery Note")]
        CustomerDeliveryNote = 10,
        [Description("Maintenance Quotation")]
        MaintenanceQuotation = 11


    }
    public enum InventoryTypeEnum : int
    {
        PurchaseOrder = 0,
        GooddReceivedNote = 1,
    }
    public enum AssessmentReportTemplateEnum : int
    {
        FederalReport = 0,
        GeneralReport = 1,
    }

    public enum FiscalYearTypeEnum : int
    {
        StartsPreviousYear = 0,
        EndsNextYear = 1,
    }
    public enum NtsDynamicClassTypeEnum : int
    {
        PreScript = 0,
        PostScript = 1,
        ValueScript = 2,
        SaveChangesButtonVisibilityScript = 3,
        PrintButtonVisibilityScript = 4,
        MasterSourceScript = 5,
        EditButtonVisibilityScript = 6,
        LoadScript = 7
    }

    public enum NtsActionEnum : int
    {
        Draft = 0,
        Submit = 1,
        Complete = 2,
        Return = 3,
        View = 4,
        Cancel = 5,
        Back = 6,
        Reject = 7,
        Delegate = 8,
        Close = 9,
        Expire = 10,
        Overdue = 11,
        Reminder = 12,
        NotStarted = 13,
        SaveChanges = 14,
        EditAsNewVersion = 15,
        All = 16,
        Resubmit = 17,
        Share = 18,
        PostComment = 19,
        Reassign = 20,
        SLAChangeRequest = 21,
        ApproveSLAChangeRequest = 22,
        RejectSLAChangeRequest = 23,
        NotApplicable = 24,
        Reopen = 25,
        CancelEdit = 26,
        PendingWithSubTask = 27
    }

    public enum NotificationTemplateTypeEnum : byte
    {
        Webinar = 1,
        Calender = 2,
    }

    public enum LoggedInAsTypeEnum : byte
    {
        LoginCredential = 1,
        LoginAsDifferentUser = 2,
        SwitchProfile = 3
    }
    public enum SourceSystemOwnerEnum : byte
    {
        SYSTEM = 1,
        CSV = 2
    }
    public enum GrantStatusEnum : byte
    {
        Granted = 1,
        Revoked = 2
    }
    public enum AssessmentTypeEnum : byte
    {
        TechnicalAssessment = 1,
        CaseStudyAssessment = 2,
        AssessmentSurvey = 3,
        AssessmentInterview = 4
    }
    public enum PaymentTypeEnum : byte
    {
        AdvancePayment = 1,
        ProgressPayment = 2,
        FinalPayment = 3,
        FullPayment = 4,
        DownPayment = 5
    }

    public enum PMSMasterStageStatusEnum : byte
    {
        Closed = 0,
        Open = 1
    }
    public enum PMSStageStatusEnum : byte
    {
        NotStarted = 0,
        Started = 1,
        Completed = 2,
        Cancelled = 3,
        Overdue = 4
    }

    public enum DmsPermissionTypeEnum : int
    {
        [Description("Allow")]
        Allow = 0,
        [Description("Deny")]
        Deny = 1
    }
    public enum DocumentQueryTypeEnum : int
    {
        Folder = 0,
        Document = 1,
        Tag = 3
    }

    public enum FolderTypeEnum : int
    {
        LegalEntity = 0,
        Workspace = 1,
        Folder = 2,
        TagCategory = 3,
        Tag = 4,
        Document = 5
    }


    public enum DmsAccessEnum : int
    {
        [Description("Read Only")]
        ReadOnly = 0,
        Modify = 1,
        [Description("Full Access")]
        FullAccess = 2
    }
    public enum DmsAppliesToEnum : int
    {
        [Description("Only This Folder")]
        OnlyThisFolder = 0,
        [Description("This Folder And Files")]
        ThisFolderAndFiles = 1,
        [Description("ThisFolder, SubFolders And Files")]
        ThisFolderSubFoldersAndFiles = 2,
        [Description("Only This Document")]
        OnlyThisDocument = 3,
        [Description("All Documents in this Folder and Sub Folders")]
        AllDocuments = 4,
    }

    public enum HierarchyPermissionEnum : int
    {
        All = 0,
        Parent = 1,
        Self = 2,
        LegalEntity = 3,
        Custom = 4
    }

    public enum HierarchyTypeEnum : int
    {
        Position = 0,
        Organization = 1,
        User = 2,
        Person = 3
    }
    public enum UserTypeEnum : int
    {
        Internal = 0,
        External = 1,
    }
    public enum DataSourceEnum : int
    {
        Web = 0,
        Mobile = 1
    }
    public enum NtsPriorityEnum : int
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
    public enum DocumentTypeEnum : int
    {
        File = 0,
        Folder = 1,
    }
    public enum DmsDocumentViewTypeEnum : int
    {
        DocumentGridView = 0,
        DocumentListView = 1,
        DocumentTileView = 2,
        DocumentPreView = 3,
        DocumentCalendarView = 4
    }

    public enum ModuleEnum : int
    {
        [Description("Admin")]
        Admin = 0,
        [Description("General")]
        General = 1,
        [Description("Manpower")]
        Hrs = 2,
        [Description("Performance Management")]
        Pms = 3,
        [Description("Worklist")]
        Nts = 4,
        [Description("Succession Planning")]
        Sps = 6,
        [Description("Project Management")]
        Pmt = 7,
        [Description("Payroll")]
        Pay = 8,
        [Description("Time & Attendance")]
        Taa = 9,
        [Description("Leave")]
        Leave = 10,
        [Description("Recruitment")]
        Rec = 11,
        [Description("Sales")]
        Sal = 12,
        [Description("Document")]
        Document = 13,
        [Description("Separation")]
        Separation = 14,
        [Description("Cpm")]
        Cpm = 15,
        [Description("Support")]
        Support = 16,
        [Description("Report")]
        Report = 17,
        [Description("Document Management System")]
        Dms = 18,
        Inspection = 19, //added by arshad 
        Finance = 20,
        [Description("Data Management System")]
        Ims = 21
    }

    public enum ServiceSearchHomeEnum : int
    {
        [Description("Draft")]
        DRAFT = 0,
        [Description("Canceled")]
        CANCELED = 1,
        [Description("Completed")]
        COMPLETED = 2,
        [Description("Overdue")]
        OVER_DUE = 3,
        [Description("In Progress")]
        IN_PROGRESS = 4,
        [Description("Requested by me")]
        REQ_BY = 5,
        [Description("Shared with me/Team")]
        SHARE_TO = 6,
    }


    public enum TaskSearchHomeEnum : int
    {
        [Description("Draft")]
        DRAFT = 0,
        [Description("Canceled")]
        CANCELED = 1,
        [Description("Completed")]
        COMPLETED = 2,
        [Description("Overdue")]
        OVER_DUE = 3,
        [Description("In Progress")]
        IN_PROGRESS = 4,
        [Description("Assigned to me")]
        ASSIGN_TO = 5,
        [Description("Requested by me")]
        ASSIGN_BY = 6,
        [Description("Shared with me/Team")]
        SHARE_TO = 7,
    }
    public enum PmtTaskSearchHomeEnum : int
    {
        [Description("Draft")]
        DRAFT = 0,
        [Description("Canceled")]
        CANCELED = 1,
        [Description("Completed")]
        COMPLETED = 2,
        [Description("Overdue")]
        OVER_DUE = 3,
        [Description("In Progress")]
        IN_PROGRESS = 4,
        [Description("Assigned to me")]
        ASSIGN_TO = 5,
        [Description("Requested by me")]
        ASSIGN_BY = 6,
        [Description("Shared with me/Team")]
        SHARE_TO = 7,
        [Description("Group by Owner")]
        GROUP_OWNER = 8,
        [Description("Group by Date Modified")]
        GROUP_DATEMODIFIED = 9,
        [Description("Group by Project Name")]
        GROUP_PROJECTNAME = 10,
    }
    public enum NoteSearchHomeEnum : int
    {
        [Description("Draft")]
        DRAFT = 0,
        [Description("Canceled")]
        CANCELED = 1,
        [Description("Completed")]
        COMPLETED = 2,
        [Description("Overdue")]
        OVER_DUE = 3,
        [Description("Active")]
        ACTIVE = 4,
        //[Description("Shared with me/Team")]
        //SHARE_TO = 5,
        [Description("Owned by me")]
        OWNED_BY = 6,
    }
    public enum DependencyTypeEnum : int
    {
        FinishFinish = 0,
        FinishStart = 1,
        StartFinish = 2,
        StartStart = 3
    }

    public enum AreaEnum : int
    {
        Admin = 0,
        General = 1,
        Hrs = 2,
        Pms = 3,
        Nts = 4,
        Sps = 6,
        Pmt = 7,
        Pay = 8,
        Taa = 9,
        Leave = 10,
        Rec = 11,
        Sal = 12,
        Cpm = 13,
        Rpt = 14,
        Dms = 15,
        Fin = 16

    }
    public enum HyperlinkTargetEnum : int
    {
        Popup = 0,
        NewTab = 1
    }
    public enum UdfValueTypeEnum : int
    {
        Code = 0,
        Value = 1
    }


    public enum NotificationTypeEnum : int
    {
        Regular = 0,
        Summary = 1
    }
    public enum LayoutModeEnum : int
    {
        Main = 0,
        Iframe = 1,
        Popup = 2,
        Tab = 3,
        Card = 4
    }
    public enum CalendarInvitationTypeEnum : int
    {
        [Description("Sent")]
        Create = 0,
        [Description("Updated")]
        Update = 1,
        [Description("Canceled")]
        Cancel = 2
    }

    public enum UserAuthTypeEnum : int
    {
        SingleSignOn = 0,
        FormsAuthentication = 1
    }
    public enum UserLoginTypeEnum : int
    {
        Email = 0,
        MobileNo = 1,
        [Display(Name = "Porsonal Id")]
        [Description("Porsonal Id")]
        IqamahNo = 2,
    }
    public enum UserVerificationStatusEnum : int
    {
        Invited = 0,
        Invoke = 1,
        Verified = 2,
    }
    public enum BroadcastTypeEnum : int
    {
        Team = 0,
        Organization = 1,
        Job = 2,
        Position = 3,
        Person = 4
    }
    public enum TransactionStatusEnum : int
    {
        OPEN_7 = 0,
        IN_PRGRSS_7 = 1,
        APPRVD_7 = 2,
        CMPLTD_7 = 3,
        CNCLD_7 = 4
    }

    public enum PmsAccessType : int
    {
        Full = 0,
        View = 1,
        Restricted = 2
    }

    public enum SalLeadStatusEnum : int
    {
        New = 0,
        Active = 1,
        [Display(Name = "Not Interested")]
        [Description("Not Interested")]
        NotInterested = 2,
        //[Display(Name = "Deal In Progress")]
        //[Description("Deal In Progress")]
        //DealInProgress = 3,
        //Broker = 3,
        [Display(Name = "Closed Deal")]
        [Description("Closed Deal")]
        ClosedDeal = 4
    }
    public enum SalMeasurementUnitEnum : int
    {
        [Display(Name = "Square Feet(Sq.Ft)")]
        [Description("Sq. ft")]
        Sqft = 0,
        [Display(Name = "Square Meter(Sq.Mt)")]
        [Description("Sq. Meter")]
        SqMeter = 1
    }

    public enum SalSourceOfLeadEnum : int
    {
        [Display(Name = "Self Generated By Property Consultant")]
        [Description("Self Generated By Property Consultant")]
        SelfGeneratedByPropertyConsultant = 0,
        Broker = 1,
        Website = 2,
        [Display(Name = "Social Media")]
        [Description("Social Media")]
        SocialMedia = 3,
        // Email = 4,
        //Referrals = 5,
        [Display(Name = "Sales Campaign")]
        [Description("Sales Campaign")]
        SalesCampaign = 6
    }

    public enum SalPaymentModeEnum : int
    {
        [Display(Name = "Bank Transfer")]
        [Description("Bank Transfer")]
        BankTransfer = 0,
        Cheque = 1,
        [Display(Name = "Sales Point")]
        [Description("Sales Point")]
        SalesPoint = 2,
        [Display(Name = "Credit Card")]
        [Description("Credit Card")]
        CreditCard = 3
    }

    public enum SalActivityTypeEnum : int
    {
        //[Description("Sale")]
        //Sales = 0,
        //[Description("Lead")]
        //Leads = 1,
        //[Description("Broker")]
        //Brokers = 2,
        //[Description("Call")]
        //Calls = 3,
        //[Description("Meeting")]
        //Meetings = 4,

        //Call = 0,
        //[Description("Meeting")]
        //Meeting = 1,
        //[Description("Email")]
        //Email = 2,
        //[Display(Name = "Mobile Message")]
        //[Description("Mobile Message")]
        //MobileSMS = 3

        [Description("Call")]
        Call = 0,
        [Description("Meeting")]
        Meeting = 1,
        [Description("Email")]
        Email = 2,
        [Display(Name = "Mobile Message")]
        [Description("Mobile Message")]
        MobileSMS = 3

    }

    //public enum SalActivityEnum : int
    //{
    //    [Description("Call")]
    //    Call = 0,
    //    [Description("Meeting")]
    //    Meeting = 1,
    //    [Description("Email")]
    //    Email = 2,
    //    [Display(Name = "Mobile Message")]
    //    [Description("Mobile Message")]
    //    MobileSMS = 3

    //}
    public enum SalActivityOutcomeEnum : int
    {
        [Display(Name = "Schedule Meeting")]
        [Description("Schedule Meeting")]
        ScheduleMeeting = 0,
        [Display(Name = "Follow Up")]
        [Description("Follow Up")]
        FollowUp = 1,
        [Display(Name = "Send Email")]
        [Description("Send Email")]
        SendEmail = 2,
        [Display(Name = "Send Mobile Message")]
        [Description("Send Mobile Message")]
        SendMobileSMS = 3,
        [Display(Name = "Not Interested")]
        [Description("Not Interested")]
        NotInterested = 4
    }
    public enum ProcessStatusEnum : int
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2
    }
    public enum UserInfoImageTypeEnum : int
    {
        FingerPrint = 0,
        Face = 1
    }

    public enum RecruitmentProcessStatusEnum : int
    {
        HrSelection = 0,
        LineManagerSelection = 1
    }

    public enum SalGoalTypeEnum : int
    {

        [Description("User Goal's")]
        UserGoal = 0,
        [Description("Team Goal's")]
        TeamGoal = 1

    }
    public enum SalGoalMetricEnum : int
    {
        //Unit = 0,
        //Calls = 1,
        //Meetings = 2,
        //Emails = 3,
        //SMS = 4,
        [Description("Deals")]
        Sales = 0,
        Leads = 1,
        Brokers = 2,
        Calls = 3,
        Meetings = 4,

    }

    public enum ReportFrequencyEnum : int
    {
        Daily = 0,
        Weekly = 1,
        Monthly = 2,
        Quarterly = 3,
        Yearly = 5
    }
    public enum SalGoalIntervalEnum : int
    {
        Daily = 0,
        Weekly = 1,
        Monthly = 2,
        Quarterly = 3,
        [Display(Name = "Half Yearly")]
        [Description("Half Yearly")]
        HalfYearly = 4,
        Yearly = 5
    }


    public enum CpmIntervalEnum : int
    {
        // Daily = 0,
        // Weekly = 1,
        Monthly = 0,
        Quarterly = 1,
        [Display(Name = "Half Yearly")]
        [Description("Half Yearly")]
        HalfYearly = 2,
        Yearly = 3
    }
    public enum SalUnitStatusEnum : int
    {
        Available = 0,
        Blocked = 1,
        //Booked = 1,
        Reserved = 2,
        Sold = 3,

    }

    public enum SalUnitStatusChangeEnum : int
    {
        Available = 0,
        Blocked = 1,
        //Booked = 1,
        //Reserved = 2,
        //Sold = 3,

    }

    public enum SalActivityStatusEnum : int
    {
        //Close = 1,
        //Open = 0

        [Description("In Progress")]
        InProgress = 0,

        [Description("Not Interested")]
        NotInterested = 1,

        [Description("Close Deal")]
        CloseDeal = 2
    }
    public enum BookingStatusEnum : int
    {
        [Display(Name = "New Deal")]
        [Description("New Deal")]
        NewDeal = 0,
        [Display(Name = "In Progress")]
        [Description("In Progress")]
        InProgress = 1,
        [Display(Name = "Under Execution")]
        [Description("Under Execution")]
        UnderExecution = 2,
        [Display(Name = "Executed")]
        [Description("Executed")]
        Executed = 3,

    }
    public enum DepositProofEnum : int
    {

        Cheque = 0,
        Transfer = 1,
        [Description("Point of sale")]
        POS = 2,

    }
    public enum RatingEnum : int
    {
        Poor = 0,
        Fair = 1,
        Good = 2,
        VeryGood = 3,
        Excellent = 4,
    }
    public enum InspectionEnum : int
    {
        Bad = 0,
        Good = 1,
    }
    public enum CriteriaTypeEnum : int
    {
        YesOrNo = 0,
        Scale = 1,
    }
    public enum OtherInfoTypeEnum : int
    {
        YesOrNo = 0,
        Input = 1,
    }
    public enum ProbationPeriodEnum : int
    {
        No = 0,
        [Description("1 Month")]
        OneMonth = 1,
        [Description("2 Months")]
        TwoMonths = 2,
        [Description("3 Months")]
        ThreeMonths = 3,
        [Description("6 Months")]
        SixMonths = 4,
    }
    public enum ReasonForLeavingEnum : int
    {
        Retirement = 0,
        BetterOpportunity = 1,
        Relocation = 2,
        PersonlaReason = 3,
        OtherReason = 4,
    }
    public enum WbsItemStatusEnum : int
    {
        Active = 0,
        Deleted = 1,
    }
    public enum DistributionTypeEnum
    {
        [Description("Internal User")]
        InternalUser = 0,
        [Description("External Party")]
        ExternalParty = 1
    }
    //public enum CpmProjectTypeEnum : int
    //{
    //    Cayan = 0,
    //    [Description("Non Cayan")]
    //    [Display(Name = "Non Cayan")]
    //    Non_Cayan = 1
    //}
    //public enum DataModelEnum
    //{
    //    Grade,
    //    Organization,
    //    Job,
    //    Position,
    //    Person,
    //    Assignment,
    //    PositionHierarchy,
    //    OrgHierarchy,
    //    TemplateMaster,
    //    Template,
    //    TemplateField
    //}


    //public enum NtsFieldType
    //{
    //    NTS_Display = 2,
    //    NTS_TextBox = 1,
    //    NTS_TextArea = 3,
    //    NTS_RichTextBox = 4,
    //    NTS_DatePicker = 5,
    //    NTS_DropdownList = 6,
    //    NTS_NumericTextBox = 7,
    //    NTS_IntegerTextBox = 8,
    //}
    //public enum NtsFieldDisplayMode
    //{
    //    NTS_Create = 1,
    //    NTS_Edit = 2,
    //    NTS_View = 3
    //}
    //public enum NtsTaskOperationMode
    //{
    //    Create = 1,
    //    Edit = 2,
    //    View = 3,
    //    Delete = 4,
    //    Assignee = 5
    //}
    //public enum ActionEventEnum : byte
    //{
    //    OnCreation = 1,
    //    OnCompletion = 2
    //}
    //public enum FieldConfigTypeEnum : byte
    //{
    //    Required = 1,
    //    Visibility = 2,
    //    Readonly = 3,
    //    DefaultValue = 4,
    //    Compare = 5,
    //    Summary = 6
    //}
    //public enum EmailStatus
    //{
    //    Sent,
    //    Cancelled,
    //    Error
    //}
    //public enum TaskStatus : byte
    //{
    //    NOT_STRT_22 = 0,
    //    OPEN_22 = 1,
    //    RUN_22 = 2,
    //    SUCCSS_22 = 3,
    //    ERROR_22 = 4,
    //    PEND_REV_22 = 5,
    //    COMPLTD_22 = 6
    //}
    //public enum PositionOperation
    //{
    //    Insert,
    //    Update,
    //    Delete,
    //    Read
    //}


    //public enum OrganizationTypeEnum : byte
    //{
    //    Unknown = 0,
    //    CEO = 1,
    //    Group = 2,
    //    Department = 3,
    //    Division = 4,
    //    Section = 5,
    //    Unit = 6,
    //}
    //public enum PositionTypeEnum : byte
    //{
    //    Unknown = 0,
    //    Pooled = 1,
    //    [Description("Single Incumbent")]
    //    SingleIncumbent = 2,
    //    Shared = 3,
    //    None = 4
    //}

    //public enum TransactionRequestModeEnum : byte
    //{
    //    CRT_21 = 1,
    //    UPD_21 = 2,
    //    DEL_21 = 3
    //}
    //public enum TransactionProcessStatusEnum : byte
    //{
    //    OPEN_6 = 1,
    //    IN_PRGRSS_6 = 2,
    //    APPRVD_6 = 3,
    //    CMPLTD_6 = 4,
    //    CNCLD_6 = 5
    //}
    //public enum TransactionTypeEnum : byte
    //{

    //    CRT_POS_IN_HRCHY_2 = 1,
    //    MOV_POS_FRM_HRCHY_2 = 2,
    //    DEL_POS_FRM_HRCHY_2 = 3,
    //    REN_ORG_2 = 4,
    //    UPD_POS_2 = 5,
    //    REQ_NEW_POS_2 = 6,
    //    REQ_REP_POS_2 = 7,
    //    CRT_ORG_IN_HRCHY_2 = 8,
    //    MOV_ORG_2 = 9,
    //    REM_ORG_2 = 10,
    //    // NEW_JOB_2 = 11,
    //    NEW_MYDAS_JOB_2 = 12,
    //    UPD_ORG_2 = 13,
    //    ADD_POS_IN_HRCHY_2 = 14,
    //    REQ_NEW_JOB_2 = 15
    //}

    //public enum RequestStatusEnum : byte
    //{
    //    AWT_APPRVL_5 = 1,
    //    APPRVD_5 = 2,
    //    RJCTD_5 = 3
    //}
    //public enum ManpowerHiringStatus : byte
    //{
    //    NOT_STRT_4 = 1,
    //    IN_PRG_4 = 2,
    //    HIRD_4 = 3,
    //    CNCLD_4 = 4
    //}
    //public enum DataLoadMode : byte
    //{
    //    Merge = 1,
    //    Delete = 2
    //}


    //public enum HiringStatus : byte
    //{
    //    Unknown = 0,
    //    Active = 1,
    //    Proposed = 2,
    //    Eliminated = 3,
    //    Frozen = 4
    //}




    //public enum EmployeeSearchType : byte
    //{
    //    EmployeeId = 1,
    //    EmployeeName = 2
    //}

    //public enum PositionSearchType : byte
    //{
    //    EmployeeNumber = 1,
    //    EmployeeName = 2,
    //    JobTitle = 3,
    //    Organization = 4
    //}

    //public enum OrganizationSearchType : byte
    //{
    //    OrgName = 1
    //}

    //public enum NationalityType : byte
    //{
    //    National = 1,
    //    Expatriates = 2
    //}
    //public enum ShowPhotoSettings : byte
    //{
    //    Off = 0,
    //    All = 1,
    //    AllFemale = 2,
    //    MuslimFemale = 3

    //}
    //public enum GenderEnum : byte
    //{
    //    DataNotAvailable = 0,
    //    Male = 1,
    //    Female = 2
    //}
    //public enum MaritalStatusEnum : byte
    //{
    //    DataNotAvailable = 0,
    //    Single = 1,
    //    Married = 2,
    //    Divorced = 3,
    //    Widowed = 4,
    //    [Description("Divorced with Dependant")]
    //    DivorcedwithDependant = 5,
    //    Widower = 6,
    //    [Description("Widower with Dependan")]
    //    WidowerwithDependant = 7
    //}
    //public enum Salutation : byte
    //{
    //    Mr = 1,
    //    Mrs = 2,
    //    Miss = 3
    //}



    //public enum QuarterlyReportMode : byte
    //{
    //    Normal = 1,
    //    Organization = 2
    //}
    public enum HelpDeskCategoryEnum : int
    {
        Hardware = 0,
        Network = 1,
        Internet = 2,
        Software = 3,
        [Display(Name = "IP & Telephone")]
        IPTelephone = 4,
        [Display(Name = "User Setup")]
        [Description("User Setup")]
        UserSetup = 5,
        Email = 6,
        [Display(Name = "Cameras & NVR")]
        [Description("Cameras & NVR")]
        CamerasNVR = 7,
        Printer = 8,
        [Display(Name = "POS & Terminal Printer")]
        [Description("POS & Terminal Printer")]
        POSTerminalPrinter = 9,
        [Display(Name = "Web Site")]
        [Description("Web Site")]
        WebSite = 10,
        [Display(Name = "MS NAV POS")]
        [Description("MS NAV POS")]
        MS_NAV_POS = 11,
        [Display(Name = "MS NAV Back Office")]
        [Description("MS NAV Back Office")]
        MS_NAV_BackOffice = 12,
        Others = 13
    }

    public enum HelpDeskSubCategoryEnum : int
    {
        EquipmentMove = 0,
        KeyboardMouse = 1,
        Laptop = 2,
        Monitor = 3,
        Printer = 4,
        HardDisk = 5,
        RAM = 6,
        Request = 7,
        UPS = 8,
        Connectivity = 9,
        VPN = 10,
        Wireless = 11,
        Crashes = 12,
        Upgrades = 13,
        InternetSpeedSlow = 14,
        EXE = 15,
        Phone = 16,
        NameChange = 17,
        NewEmployee = 18,
        PermissionChange = 19,
        Remove = 20,
        ResetPassword = 21,
        NewEmailID = 22,
        AddCamera = 23,
        CAMFootage = 24,
        AddNewPrinter = 25,
        ConfigureScanner = 26,
        TonerReplacement = 27,
        AddNewPOS = 28,
        ReceiptErrors = 29,
        AddItems = 30,
        ChangePrice = 31,
        AllowWebsites = 32,
        MenuChange = 33,
        PrinterMapping = 34,
        ChangePassword = 35,
        AddNewuser = 36,
        PermissionAccess = 37,
        Install_NAV_Back_office = 38,
        LoginIssues = 39,
        Others = 40
    }

    public enum ApplicationStatusEnum : int
    {
        Pending = 0,
        Waiting = 1,
        Rejected = 2,
        Selected = 3,
    }
    public enum ApplicationStateEnum : int
    {
        Applied = 0,
        Canceled = 1,
        Rejected = 2,
        ShortlistedByRecruiter = 3,
        ShortlistedByLineManager = 4,
        InterviewScheduled = 5,
        Selected = 6,
        JobOffered = 7,
        JobRejected = 8,
        PreJoinFormalities = 9,
        PreJoinChecklist = 10,
        JoiningChecklist = 11,
        Joined = 12,
        AssessmentScheduled = 13,
    }
    public enum ManagerialLevel
    {
        Above = 0,
        Manager = 1,
        Below = 2
    }
    public enum IntegerEnum : int
    {
        [Display(Name = "1")]
        [Description("1")]
        one = 1,
        [Display(Name = "2")]
        [Description("2")]
        Two = 2,
        [Display(Name = "3")]
        [Description("3")]
        Three = 3,
        [Display(Name = "4")]
        [Description("4")]
        Four = 4,
        [Display(Name = "5")]
        [Description("5")]
        Five = 5,
        [Display(Name = "6")]
        [Description("6")]
        six = 6,
        [Display(Name = "7")]
        [Description("7")]
        seven = 7,
        [Display(Name = "8")]
        [Description("8")]
        eight = 8,
        [Display(Name = "9")]
        [Description("9")]
        nine = 9,
        [Display(Name = "10")]
        [Description("10")]
        ten = 10,
        [Display(Name = "11")]
        [Description("11")]
        eleven = 11,
        [Display(Name = "12")]
        [Description("12")]
        twelve = 12,
        [Display(Name = "13")]
        [Description("13")]
        thirteen = 13,
        [Display(Name = "14")]
        [Description("14")]
        forteen = 14,
        [Display(Name = "15")]
        [Description("15")]
        fifteen = 15,
        [Display(Name = "16")]
        [Description("16")]
        sixteen = 16,
        [Display(Name = "17")]
        [Description("17")]
        seventeen = 17,
        [Display(Name = "18")]
        [Description("18")]
        eighteen = 18,
        [Display(Name = "19")]
        [Description("19")]
        nineteen = 19,
        [Display(Name = "20")]
        [Description("20")]
        twenty = 20
    }

    public enum TimePermissionType : int
    {
        [Description("Late In")]
        LateIn = 0,
        [Description("Early Out")]
        EarlyOut = 1,
        [Description("During The Day")]
        DuringTheDay = 2,
    }

    public enum DeductionType : int
    {
        [Description("Deduct From Annual Leave Balance")]
        DeductFromAnnualLeaveBalance = 0,
        [Description("Deduct From Hourly Rate Of Salary")]
        DeductFromHourlyRateOfSalary = 1,
    }

    public enum LoanRecoveryTypeEnum : int
    {
        [Description("Deduct From Salary")]
        DeductFromSalary = 0,
        //[Description("Deduct From End Of Service")]
        //DeductFromEndOfService = 1,
        [Description("Monthly Repay By Employee")]
        MonthlyRepayByEmployee = 2,
    }

    public enum ExitRentryVisaPaymentType : int
    {
        [Description("Pay By Self")]
        PayBySelf = 0,
        [Description("Deduct From Next Salary")]
        DeductFromNextSalary = 1,
    }
    public enum KingdomEnum : int
    {
        [Description("Within Kingdom")]
        WithInKingdom = 0,
        [Description("Outside Kingdom")]
        OutsideKingdom = 1,
    }

    public enum VendorCategoryTypeEnum : int
    {
        [Description("Air Conditioner")]
        AirConditioner = 0,
        [Description("Common Area")]
        CommonArea = 1,
        Electricals = 2,
        Plumbing = 3,
        Other = 5,
    }

    public enum UnitTypeEnum : int
    {
        Apartment = 0,
        Villa = 1,
        TownHouse = 2,
        PentHouse = 3,
        Compound = 4,
        Duplex = 5,
        FullFloor = 6,
        Bungalow = 7,
        Room = 8,
        Suit = 9,
        Office = 10,
        Shop = 11,
        ParkingSpace = 12,
        Commercial = 13,
        House = 14,
        Studio = 15
    }

    public enum ManagementFeeTypeEnum : int
    {
        Percentage = 0,
        Fixed = 1,
    }

    public enum UnitStatusTypeEnum : int
    {
        Vacant = 0,
        Occupied = 1,
        Sold = 2,
        [Description("Under Renovation")]
        UnderRenovation = 3,
        Reserved = 4
    }

    public enum MoneyHeldByTypeEnum : int
    {
        Owner = 0,
        Company = 1,
    }

    public enum InvoiceScheduleTypeEnum : int
    {
        [Description("Every Month")]
        EveryMonth = 0,
        [Description("2 Month")]
        TwoMonth = 1,
        [Description("3 Month")]
        ThreeMonth = 2,
        [Description("4 Month")]
        FourMonth = 3,
        //[Description("5 Month")]
        //FiveMonth = 4,
        [Description("6 Month")]
        SixMonth = 5,
        Yearly = 6

    }

    public enum ContractPaymentTypeEnum : int
    {
        Cheque = 0,
        Cash = 1,
        [Description("Credit Card")]
        CreditCard = 2,
        Online = 3,
        [Description("Bank Transfer")]
        BankTransfer = 4,
        [Description("Cheque & Cash")]
        ChequeAndCash = 5,
        [Description("Cheque & BankTransfer")]
        ChequeBankTransfer = 6,

    }
    public enum CpmPayeeBeneficiaryEnum : int
    {
        Tenant = 0,
        Owner = 1,
        Company = 2,
        Vendor = 3,
    }
    public enum ContractPaymentStatusEnum : int
    {
        UnPaid = 1,
        Paid = 2,
        Bounced = 3,
    }
    public enum PaymenTypeEnum : int
    {
        //Rent = 0,
        Expense = 1,
        Deposite = 2,
        Income = 3,
        [Description("Money Hold")]
        [Display(Name = "Money Hold")]
        MoneyHold = 4,
    }
    public enum InspectionTypeEnum : int
    {
        [Display(Name = "Move In")]
        [Description("Move In")]
        MoveIn = 0,
        [Display(Name = "Move Out")]
        [Description("Move Out")]
        MoveOut = 1,
        //Quarterly = 2,
        //General = 3,
    }
    public enum ContractAccountEnum : int
    {
        [Description("Retention Funds (Emergency Maintainence Payment)")]
        RetentionFunds = 1,
        [Description("PM Fees")]
        PMFees = 2,
    }
    public enum TimeSlotTypeEnum : int
    {
        AnyTime = 0,
        [Description("8am-12pm")]
        EightToTwelve = 1,
        [Description("12pm-4pm")]
        TwelveToFour = 2,
        [Description("4pm-8pm")]
        FourToEight = 3,
    }

    public enum MaintenanceStatusEnum : int
    {
        [Description("New Request")]
        New = 0,
        [Description("InProgress")]
        InProgress = 1,
        [Description("Resolved")]
        Resolved = 2,
        [Description("Rejected")]
        Rejected = 3,
    }
    public enum CpmDueDateEnum : int
    {
        [Display(Name = "1st")]
        [Description("1st")]
        one = 1,
        [Display(Name = "2nd")]
        [Description("2nd")]
        Two = 2,
        [Display(Name = "3rd")]
        [Description("3rd")]
        Three = 3,
        [Display(Name = "4th")]
        [Description("4th")]
        Four = 4,
        [Display(Name = "5th")]
        [Description("5th")]
        Five = 5,
        [Display(Name = "6th")]
        [Description("6th")]
        six = 6,
        [Display(Name = "7th")]
        [Description("7th")]
        seven = 7,
        [Display(Name = "8th")]
        [Description("8th")]
        eight = 8,
        [Display(Name = "9th")]
        [Description("9th")]
        nine = 9,
        [Display(Name = "10th")]
        [Description("10th")]
        ten = 10,
        [Display(Name = "11th")]
        [Description("11th")]
        eleven = 11,
        [Display(Name = "12th")]
        [Description("12th")]
        twelve = 12,
        [Display(Name = "13th")]
        [Description("13th")]
        thirteen = 13,
        [Display(Name = "14th")]
        [Description("14th")]
        forteen = 14,
        [Display(Name = "15th")]
        [Description("15th")]
        fifteen = 15,
        [Display(Name = "16th")]
        [Description("16th")]
        sixteen = 16,
        [Display(Name = "17th")]
        [Description("17th")]
        seventeen = 17,
        [Display(Name = "18th")]
        [Description("18th")]
        eighteen = 18,
        [Display(Name = "19th")]
        [Description("19th")]
        nineteen = 19,
        [Display(Name = "20th")]
        [Description("20th")]
        twenty = 20,
        [Display(Name = "21st")]
        [Description("21st")]
        twentyOne = 21,
        [Display(Name = "22nd")]
        [Description("22nd")]
        twentytwo = 22,
        [Display(Name = "23rd")]
        [Description("23rd")]
        twentythree = 23,
        [Display(Name = "24th")]
        [Description("24th")]
        twentyFour = 24,
        [Display(Name = "25th")]
        [Description("25th")]
        twentyFive = 25,
        [Display(Name = "26th")]
        [Description("26th")]
        twentySix = 26,
        [Display(Name = "27th")]
        [Description("27th")]
        twentySeven = 27,
        [Display(Name = "28th")]
        [Description("28th")]
        twentyEight = 28,
        [Display(Name = "29th")]
        [Description("29th")]
        twentyNine = 29,
        [Display(Name = "30th")]
        [Description("30th")]
        thirty = 30,
        [Display(Name = "31st")]
        [Description("31st")]
        thirtyOne = 31,
    }
    public enum CpmAccountType : int
    {
        Income = 0,
        [Display(Name = "Operating expenses")]
        [Description("Operating expenses")]
        OperatingExpenses = 1,
        [Display(Name = "Current asset")]
        [Description("Current asset")]
        CurrentAsset = 2,
        [Display(Name = "Fixed asset")]
        [Description("Fixed asset")]
        FixedAsset = 3,
        [Display(Name = "Current liability")]
        [Description("Current liability")]
        CurrentLiability = 4,
        [Display(Name = "LongTerm liability")]
        [Description("LongTerm liability")]
        LongTermLiability = 5,
        Equity = 6,
        [Display(Name = "Non-Operating Income")]
        [Description("Non-Operating Income")]
        NonOperatingIncome = 7,
        [Display(Name = "Non-Operating expenses")]
        [Description("Non-Operating expenses")]
        NonOperatingExpenses = 8,
    }

    public enum CpmPriorityEnum : int
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
    public enum CpmContractStatusEnum : int
    {
        [Display(Name = "Active")]
        [Description("Active")]
        Active = 0,
        [Display(Name = "Complete")]
        [Description("Complete")]
        Complete = 1,
        [Display(Name = "Renew")]
        [Description("Renew")]
        Renew = 2

    }

    public enum CpmDemoghraphicTypeEnum : int
    {
        [Display(Name = "Single")]
        [Description("Single")]
        Single = 0,
        [Display(Name = "Family")]
        [Description("Family")]
        Family = 1,

    }

    public enum CpmUnitClassification : int
    {
        Residential = 0,
        Commercial = 1,

    }

    public enum CpmVendorAccountType : int
    {
        [Display(Name = "Business Account")]
        BusinessAccount = 0,
        [Display(Name = "Saving Account")]
        SavingAccount = 1,

    }

    public enum ElasticSearchType : int
    {
        [Display(Name = "Full Text Search")]
        FullTextSearch = 0,
        [Display(Name = "Smart Search")]
        FuzzySearch = 1,
        [Display(Name = "In File Search")]
        InFileSearch = 2,

    }

    public enum WBSItemTypeEnum : int
    {
        [Display(Name = "Normal Item")]
        NormalItem = 0,
        Note = 1,
        Task = 2,
        Service = 3,
        Document = 4,
    }


    public enum AssessmentScheduleTypeEnum : int
    {
        Assessment = 0,
        Interview = 1,
    }
    
    public enum FilterTypeEnum : int
    {
        Contains = 0,
    }
    public enum TriggerTypeEnum : int
    {
        Parallel = 0,
        Sequential = 1
    }
}

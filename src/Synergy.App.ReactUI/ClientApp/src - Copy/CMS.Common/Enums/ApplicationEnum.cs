using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CMS.Common
{
    public enum ExitRentryVisaPaymentTypeEnum : int
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
    public enum SurveyStatusEnum : int
    {
        [Description("Not Scheduled")]
        NoteScheduled = 0,
        [Description("Schedule Inprogress")]
        ScheduleInprogress = 1,
        [Description("Schedule Completed")]
        ScheduleCompleted = 2
    }
    public enum HrDataExecutionStatus : int
    {
        [Description("Not Started")]
        NotStarted = 0,
        [Description("Inprogress")]
        InProgress = 1,
        [Description("Completed")]
        Completed = 2,
        [Description("Error")]
        Error = 3
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
    public enum AttendanceLeaveTypeEnum : int
    {
        UnpaidLeave = 0,
        SickLeave = 1,
        AnnualLeave = 2,
        UnderTime = 3,
        AnnualLeaveHalfDay = 4,
        OtherLeave = 5,
    }
    public enum AttendanceTypeEnum : int
    {
        Absent = 0,
        Present = 1,
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
    public enum PayrollPostedStatusEnum : int
    {
        Submitted = 0,
        Posted = 1
    }
    public enum PerformanceStageEnum : int
    {
        [Description("Goal")]
        PMS_GOAL = 0,
        [Description("Competency")]
        PMS_COMPENTENCY = 1,
        [Description("Development")]
        PMS_DEVELOPMENT = 2,
        [Description("PeerReview")]
        PMS_PEER_REVIEW = 3,
        All = 4

    }

    public enum PerformanceStageEnumUsers : int
    {
        [Description("Goal")]
        PMS_GOAL = 0,
        [Description("Competency")]
        PMS_COMPENTENCY = 1,


    }
    public enum SchemaEnum : int
    {

        cms = 0,
        bre = 1,
        [Display(Name = "public")]
        [Description("public")]
        gen = 2,
        rec = 3,

    }
    public enum BreExecuteMethodTypeEnum
    {
        PredefinedMethod = 1,
        CustomMethod = 2,
    }
    public enum DocumentTypeEnum
    {
        Root = 1,
        DocumentType = 2,
        CompositionRoot = 3,
        Composition = 4
    }
    public enum BreadcrumbLoadTypeEnum
    {
        Reload = 1,
        PartialView = 2,
        Page = 3
    }

    public enum PerformanceDocumentStatusEnum : int
    {
        Draft = 0,
        Active = 1,
        Closed = 2,
        Publishing = 3,
        Freezed = 4,
        Released = 5
    }
    public enum PerformanceObjectiveStageEnum : int
    {
        DocumentObjective = 1,
        StageObjective = 2,
    }

    public enum PMSMasterStageStatusEnum : byte
    {
        Closed = 0,
        Open = 1
    }
    public enum NtsFieldType
    {
        NTS_Display = 2,
        NTS_TextBox = 1,
        NTS_TextArea = 3,
        NTS_RichTextBox = 4,
        NTS_DatePicker = 5,
        NTS_DropdownList = 6,
        NTS_NumericTextBox = 7,
        NTS_IntegerTextBox = 8,
        NTS_HyperLink = 9,
        NTS_Attachment = 10,
        NTS_DateTimePicker = 11,
        NTS_HtmlArea = 12,
        NTS_Hidden = 13,
    }
    public enum TaskTypeEnum
    {
        StandardTask = 1,
        StepTask = 2,
        SubTask = 3,
        AdhocTask = 4
    }
    public enum ServiceTypeEnum
    {
        StandardService = 1,
        OtherService = 2,
        AdhocService = 4
    }
    public enum NoteTypeEnum
    {
        StandardNote = 1,
        AdhocNote = 4
    }
    public enum StepTaskTemplateTypeEnum
    {
        PredefinedTemplate = 1,
        CustomTemplate = 3
    }

    public enum TaskAssignedToTypeEnum
    {
        [Description("User")]
        User = 1,
        [Description("Team")]
        Team = 2,
        UserHierarchy = 3,
        Runtime = 4
    }

    public enum BusinessRuleLogicTypeEnum
    {
        Standard = 0,
        Custom = 1
    }
    public enum TemplateCreateTypeEnum
    {
        Default = 1,
        New = 2
    }
    public enum SharedWithTypeEnum
    {
        User = 1,
        Team = 2
    }


    public enum InterviewFeedbackEnum
    {
        [Description("Selected, Best Suited for job")]
        Selected = 1,
        [Description("Can be selected, will do the job")]
        CanBeSelected = 2,
        [Description("Rejected")]
        Rejected = 3
    }


    public enum NtsActiveUserTypeEnum
    {
        Requester = 1,
        Owner = 2,
        Assignee = 3,
        SharedWith = 4,
        SharedBy = 5,
        None = 6,
        OwnerOrRequester = 7,
        All = 8,
        PermittedUser = 9
    }


    public enum EmailReceipientTypeEnum
    {
        User = 1,
        Emial = 2
    }

    public enum ProcessDesignComponentExecutionType
    {
        Concurrently = 1,
        WaitForComponentToComplete = 2
    }
    public enum ProcessDesignComponentTypeEnum
    {
        Start = 1,
        Stop = 2,
        Email = 3,
        StepTask = 4,
        ProcessDesign = 5,
        DecisionScript = 6,
        ExecutionScript = 7,
        True = 8,
        False = 9,
        ParentTask = 10,

    }
    public enum WorkAssignmentTypeEnum : int
    {
        AssignToOwner = 0,
        RoundRobin = 1,
        LeastAssigned = 2
    }
    public enum TeamTypeEnum : int
    {
        General = 0,
        AdminTeam = 1,
        SupportTeam = 2,
        SupportRootTeam = 3
    }

    public enum PocecssExecutionTypeEnum
    {
        Manual = 1,
        Scheduled = 2
    }
    public enum GrantStatusEnum : int
    {
        Granted = 0,
        Revoked = 1
    }
    public enum HierarchyTypeEnum : int
    {
        User = 1,
        Position = 2,
        Organization = 3,
        Person = 4,
        Hybrid = 5

    }
    public enum EntityModelTypeEnum : int
    {
        Organization = 1,
        Position = 2,
        Job = 3,
        Grade = 4,
        Person = 5,
        LegalEntity = 6,
        Template = 7,
        Workspace = 8
    }
    public enum UserEntityTypeEnum : int
    {
        User = 1,
        UserRole = 2,
    }

    public enum TableTypeEnum
    {
        [Description("Table")]
        [Display(Name = "Table")]
        Table = 1,
        [Description("View")]
        [Display(Name = "Table")]
        View = 2
    }


    public enum PageTypeEnum
    {
        IndexPage = 1,
        Page = 2,
        Form = 3,
        Note = 4,
        Task = 5,
        Service = 6,
        Custom = 7
    }

    public enum ProcessDesignTypeEnum
    {
        ProcessDesign = 0,
        BusinessLogic = 1
    }
    public enum BusinessLogicExecutionTypeEnum
    {
        PreSubmit = 0,
        PostSubmit = 1,
        Custom = 2
    }
    public enum NodeShapeEnum
    {
        Circle = 0,
        Rectangle = 1,
        Diamond = 2,
        SquareWithHeader = 3,
        Ellipse = 4,
        RoundedRectangle = 5
    }
    public enum ResourceLanguageGroupCodeEnum
    {
        Udf = 0,
        GridHeader = 1
    }
    public enum TagCategoryTypeEnum
    {
        Static = 0,
        Master = 1
    }
    public enum NumberGenerationTypeEnum
    {
        SystemGenerated = 0,
        UserEntry = 1,
        CustomMethod = 2
    }
    public enum AccessLogTypeEnum : int
    {
        Login = 0,
        Logout = 1,
        UnauthorizedAccess = 2,
        Other = 3,
        SessionStart = 4,
        SessionEnd = 5,
    }
    public enum TemplateTypeEnum
    {
        FormIndexPage = 1,
        Page = 2,
        Form = 3,
        Note = 4,
        Task = 5,
        Service = 6,
        Custom = 7,
        ProcessDesign = 8,
        NoteIndexPage = 9,
        TaskIndexPage = 10,
        ServiceIndexPage = 11,
        Dashboard = 12,
        PortalPage = 13,
        ApplicationCustom = 14,
        MenuGroup = 15
    }
    public enum TemplateCategoryTypeEnum
    {
        Standard = 0,
        Other = 1
    }

    public enum TableSelectionTypeEnum
    {
        New = 0,
        Existing = 1,
    }
    public enum UserPermissionTypeEnum
    {
        User = 1,
        UserRole = 2,
    }
    public enum PageContentTypeEnum
    {
        Group = 1,
        Column = 2,
        Cell = 3,
        Component = 4,
        Page = 5
    }
    public enum IndexRenderingTypeEnum
    {
        GridView = 1,
        TileView = 2
    }
    public enum CustomColumnTypeEnum
    {
        Data = 1,
        Link = 2
    }

    public enum CreatePopupTypeEnum
    {
        CenterPopup = 1,
        LeftPopup = 2,
        RightPopup = 3,
        NewPage = 4
    }

    public enum PageRowTypeEnum
    {
        FullWidth = 1,
        Article = 2,
        TwoColumns = 3,
        ThreeColumns = 4
    }
    public enum ComponentTypeEnum
    {
        RichText = 1,
        Image = 2,
        Form = 3,
        Heading = 4,
        Action = 5
    }
    public enum ContentTypeEnum
    {
        Root = 1,
        Portal = 2,
        ContentPage = 3,
        Permission = 4,
        MenuGroup = 5,
    }
    //public enum QuestionTypeEnum
    // {  
    //    SingleChoice=1
    // }
    public enum NotificationActionTypeEnum
    {
        Action = 0,
        Info = 1,
    }
    public enum NotificationActionStatusEnum
    {
        NoActionRequired = 0,
        Pending = 1,
        Completed = 2
    }

    public enum NtsTypeEnum
    {
        Note = 0,
        Task = 1,
        Service = 2
    }
    public enum PrecedenceRelationshipTypeEnum
    {
        FinishToStart = 1,
        StartToStart = 2,
        FinishToFinish = 3,
        StartToFinish = 4
    }
    public enum ScriptExecutionModeEnum
    {
        Prescript = 1,
        PostScript = 2
    }
    public enum HttpVerb
    {
        Get = 1,
        Post = 2
    }
    public enum NtsGroupingLevel
    {
        Root = 0,
        Category = 1,
        Template = 2
    }

    public enum DataOperation : int
    {
        Create = 0,
        Update = 1,
        Correct = 2,
        Delete = 3,
        Read = 4
    }
    //public enum TemplateTypeEnum
    //{
    //    Note = 1,
    //    Task = 2,
    //    Service = 3,
    //    NoteCategoryRoot = 4,
    //    TaskCategoryRoot = 5,
    //    ServiceCategoryRoot = 6,
    //    NoteCategory = 7,
    //    TaskCategory = 8,
    //    ServiceCategory = 9,
    //    NtsTemplate = 10,
    //}
    public enum NtsClassificationEnum : int
    {
        Standard = 0,
        Adhoc = 1,
        Step = 2,
        Other = 3
    }
    public enum ThemeEnum
    {
        Theme1 = 1,
        Theme2 = 2,
        CareerPortal = 3,
        Recruitment = 4,
        SynergyFunding = 5,
        HR = 6,
        SocialMedia = 7,
        Theme3 = 8,
        SynergySolutions = 9,
        DMS = 10,
        Website = 11,
        EGov = 12
    }
    public enum PortalStatusEnum
    {
        Live = 1,
        Inactive = 2,
        Maintenance = 3

    }
    public enum PunchingTypeEnum
    {
        [Description("Sign In")]
        Checkin = 0,
        [Description("Sign Out")]
        Checkout = 1,
        Both = 2
    }

    public enum SigninSignoutTypeEnum
    {
        [Description("Sign In")]
        Checkin = 0,
        [Description("Sign Out")]
        Checkout = 1,

    }
    public enum CommunicationTypeEnum
    {
        Ethernet = 0
    }
    public enum DeviceTypeEnum
    {
        Biometric = 0,
        Mobile = 1,
        RemoteLogin = 2
    }



    public enum ValidationTypeEnum
    {
        [Display(Name = "No Validation")]
        [Description("No Validation")]
        NoValidation = 1,
        [Display(Name = "Validate as an email address")]
        [Description("Validate as an email address")]
        Email = 2,
        [Display(Name = "Validate as a number")]
        [Description("Validate as a number")]
        Number = 3,
        [Display(Name = "Validate as a URL")]
        [Description("Validate as a URL")]
        Url = 4,
        [Display(Name = "...or enter a custom validation")]
        [Description("...or enter a custom validation")]
        Custom = 5
    }
    public enum DeductionTypeEnum
    {
        [Description("Deduct From Annual Leave Balance")]
        DeductFromAnnualLeaveBalance = 0,
        [Description("Deduct From Hourly Rate Of Salary")]
        DeductFromHourlyRateOfSalary = 1,
    }
    public enum EditorCategoryEnum
    {
        Common = 1,
        List = 2,
        Pickers = 3,
        Media = 4,
        People = 5,
        RichContent = 6
    }
    public enum ControlTypeEnum
    {
        TextBox = 1,
        TextArea = 2,
        CheckBox = 3,
        DateTime = 4,
        Email = 5,
        Numeric = 6,
        Decimal = 7,
        Switch = 8,
        CheckBoxList = 9,
        RadioButtonList = 10,
        DropDownList = 11,
        ListView = 12,
        ColorPicker = 13,
        GridLayout = 14,
        RichTextBox = 15
    }
    public enum ToastTypeEnum
    {
        Success = 1,
        Error = 2,
        Warning = 3
    }
    public enum AttachmentTypeEnum : int
    {
        File = 0,
        Note = 1,
        Task = 2,
        Service = 3
    }
    public enum DataColumnTypeEnum
    {
        [Description("Text")]
        Text = 1,
        [Description("Bool")]
        Bool = 2,
        [Description("DateTime")]
        DateTime = 3,
        [Description("Integer")]
        Integer = 4,
        [Description("Double")]
        Double = 5,
        [Description("Long")]
        Long = 6,
        [Description("TextArray")]
        TextArray = 7,
        [Description("Time")]
        Time = 8,
    }
    public enum UdfUITypeEnum
    {
        textfield = 1,
        textarea = 2,
        number = 3,
        password = 4,
        checkbox = 5,
        selectboxes = 6,
        radio = 7,
        select = 8,
        datetime = 9,
        time = 10,
        file = 11,
        hidden = 12,
        signature = 13,
        day = 14,
        currency = 15,
        tags = 16,
        phoneNumber = 17,
        url = 18,
        email = 19,
        datagrid = 20,
        htmlelement = 21,
        content = 22


    }
    public enum ProcessDesignVariableTypeEnum
    {
        Context = 1,
        Service = 2,
        Custom = 3
    }
    public enum PropertyBindingTypeEnum
    {
        Direct = 1,
        Variable = 2
    }

    public enum DataTypeEnum
    {
        String = 1,
        Bool = 2,
        DateTime = 3,
        Long = 4,
        Double = 5,
        Object = 6,
        Int = 7
    }
    public enum DataActionEnum
    {
        Create = 1,
        Edit = 2,
        Delete = 3,
        Read = 4,
        None = 5,
        View = 6
    }
    public enum RunningModeEnum
    {
        Preview = 1,
        Live = 2
    }
    public enum RequestSourceEnum
    {
        Main = 1,
        Create = 2,
        Edit = 3,
        Delete = 4,
        View = 5,
        Post = 6,
        Versioning = 7
    }
    public enum BreMetadataTypeEnum
    {
        InputData = 1,
        MasterData = 2,
        Constant = 3,
        TableMeta = 4
    }

    public enum BusinessRuleTreeNodeTypeEnum
    {
        Root = 1,
        BusinessArea = 2,
        BusinessSection = 3,
        BusinessRuleGroup = 4,
        BusinessRule = 5
    }
    public enum BreInputDataTypeEnum
    {
        Root = 1,
        Property = 2,
        Object = 3
    }

    public enum StatusEnum
    {
        // Null = 0,
        [Description("Active")]
        [Display(Name = "Active")]
        Active = 1,
        [Display(Name = "Inactive")]
        [Description("Inactive")]
        Inactive = 2
    }
    public enum CreateReturnTypeEnum
    {
        ReloadDataInEditMode = 1,
        GotoIndexPage = 2
    }
    public enum FormTypeEnum
    {
        Form = 0,
        Wizard = 1
    }
    public enum ActionButtonPositionEnum
    {
        TopAndBottom = 0,
        TopOnly = 1,
        BottomOnly = 2
    }


    public enum EditReturnTypeEnum
    {
        ReloadDataInEditMode = 1,
        GotoIndexPage = 2
    }

    public enum OrderByEnum : int
    {
        Ascending = 1,
        Descending = 2
    }
    public enum NtsViewTypeEnum : int
    {
        Default = 1,
        Book = 2
    }
    public enum LayoutModeEnum : int
    {
        Main = 0,
        Iframe = 1,
        Popup = 2,
        Tab = 3,
        Card = 4,
        None = 5,
        Div = 6
    }
    public enum CustomTemplateLoadingTypeEnum : int
    {
        Url = 0,
        Javascript = 1
    }
    public enum CustomTemplateTypeEnum : int
    {
        Default = 0,
        Dashboard = 1,
        LOV = 2,
        SynergyWebsite = 3
    }

    public enum BusinessDataTreeNodeTypeEnum
    {
        Root = 1,
        BusinessArea = 2,
        BusinessSection = 3,
        BusinessDataGroup = 4,
        BusinessData = 5
    }
    public enum UserRoleEnum
    {
        User = 1,
        Admin = 2,
        SystemAdmin = 3
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

    public enum NotificationStatusEnum : int
    {
        NotSent = 0,
        Sent = 1,
        Error = 2,
        Cancelled = 3,
        Enqueued = 4
    }
    public enum NotificationTypeEnum : int
    {
        Regular = 0,
        Summary = 1
    }
    public enum FieldDisplayModeEnum : int
    {
        Editable = 0,
        Readonly = 1,
        View = 2
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
        CancelEdit = 26
    }
    public enum AddDeductEnum : int
    {
        Add = 0,
        Deduct = 1
    }
    public enum AssignToTypeEnum : int
    {
        User = 0,
        Query = 1,
        Team = 2,
        Organization = 3,
        ApprovalHierarchy = 4,
        DynamicMethod = 5,
        Candidate = 6,
        HiringManager = 7,
        HeadOfDepartment = 8
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
    public enum UserTypeEnum : int
    {
        CANDIDATE = 0,
        AGENCY = 1,
        HM = 2,
        ORG_UNIT = 3,
        EXTERNAL = 4,
    }

    public enum NtsLockStatusEnum : int
    {
        Released = 0,
        Locked = 1,
    }
    public enum AssignedQueryTypeEnum : int
    {
        Position = 0,
        Person = 1,
        User = 2,
        Team = 3
    }
    public enum DocumentStatusEnum : int
    {
        Draft = 0,
        Published = 1,
    }
    public enum TemplateStatusEnum : int
    {
        Draft = 0,
        Published = 1,
    }
    //public enum NoteUserEnum : int
    //{
    //    Requester = 1,
    //    Owner = 2
    //}
    //public enum TaskUserEnum : int
    //{
    //    Requester = 1,
    //    Owner = 2,
    //    Assignee = 3,
    //    ServiceRequester = 4,
    //    ServiceOwner = 5
    //}
    //public enum ServiceUserEnum : int
    //{
    //    Requester = 1,
    //    Owner = 2,
    //    StepTaskAssignee = 3
    //}
    //public enum NoteActionEnum : int
    //{
    //    Draft = 1,
    //    Submit = 2,
    //    Overdue = 3,
    //    Expire = 4,
    //    Complete = 5,
    //}
    //public enum TaskActionEnum : int
    //{
    //    Draft = 1,
    //    Submit = 2,
    //    Overdue = 3,
    //    Cancel = 4,
    //    Complete = 5,
    //    Reject = 6
    //}
    //public enum ServiceActionEnum : int
    //{
    //    Draft = 1,
    //    Submit = 2,
    //    Overdue = 3,
    //    Cancel = 4,
    //    Complete = 5,
    //    Reject = 6

    //}

    public enum NotificationTemplateTypeEnum : byte
    {
        Webinar = 1,
        Calender = 2,
    }
    public enum InboxTypeEnum : byte
    {
        INBOX = 1,
        RECEIVED = 2,
        SENT = 3,
    }
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

    public enum FieldWidthCodeEnum : int
    {
        Default = 1,
        Large = 2,
        Medium = 3,
        Small = 4,
        XLarge = 5,
        XSmall = 6,
        XLLarge = 7,
        XXLarge = 8,
        XXSmall = 9,
        XXLLarge = 10
    }

    public enum HyperlinkTargetEnum : int
    {
        Popup = 0,
        NewTab = 1
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

    public enum ReadStatusEnum : int
    {
        NotRead = 0,
        Read = 1
    }
    public enum TemplateStageTypeEnum : int
    {
        Stage = 0,
        Step = 1
    }
    public enum NtsStagingEnum : int
    {
        Inprogress = 0,
        Completed = 1
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
        NTS_EmailFolder = 73,
        REC_JobAdvertisement = 74,
        REC_WorkerPoolBatch = 75,
        REC_ManpowerRequirementSummary = 76,
        REC_Job = 77,
        REC_Batch = 78,
        //NtsNote = 79,
        //NtsTask = 80,
        //NtsService = 81,
        SWS_Media = 82,
        Form = 83,
        Page = 84,
        File = 85,
    }

    public enum NotePriorityEnum : int
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
    public enum CommentToEnum : int
    {
        All = 0,
        User = 1
    }

    public enum QualificationTypeEnum
    {
        Educational = 1,
        Certifications = 2,
        Trainings = 3,
    }

    public enum EducationTypeEnum
    {
        FullTime = 1,
        PartTime = 2,
        Online = 3,
    }
    public enum ProficiencyEnum
    {
        Low = 1,
        Medium = 2,
        High = 3,
    }
    public enum LanguageProficiencyEnum
    {
        Read = 1,
        ReadWrite = 2,
        ReadWriteSpeak = 3,
    }


    public enum JobTypeEnum
    {
        Contract = 1,
        Permanent = 2,
    }
    public enum GenderEnum : int
    {
        Male = 0,
        Female = 1,
    }
    public enum MaritalStatusEnum : int
    {
        Single = 0,
        Married = 1,
        Divorced = 2,
        Widow = 3,
        Widower = 4,
    }
    public enum SourceTypeEnum
    {
        CareerPortal = 1,
        Agency = 2,
        Migrated = 3,
        DirectHire = 4,
    }

    public enum ElementValueTypeEnum : int
    {
        Value = 0,
        Percentage = 1,
        Days = 2
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
    public enum TravelClassEnum : int
    {
        Business = 0,
        Economy = 1
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

    public enum PayrollUomEnum : int
    {
        Month = 0,
        //Hour = 0,
        //Day = 1,
        //Week = 2,

        //Other = 4,

    }
    public enum BatchTypeEnum
    {
        ShortlistByHr = 1,
        WorkerAppointment = 2,
    }
    public enum MovePostionEnum : int
    {
        Before = 1,
        After = 2
    }

    public enum BookMoveTypeEnum : int
    {
        [Display(Name = "Same As Level")]
        SameAsLevel = 1,
        Child = 2
    }

    public enum DateTypeEnum : int
    {
        Today = 1,
        [Description("Next Week")]
        NextWeek = 2,
        [Description("Next Month")]
        NextMonth = 3,
        Between = 4
    }
    public enum FilterColumnEnum : int
    {
        [Description("Recevie Date")]
        StartDate = 1,
        [Description("Due Date")]
        DueDate = 2,
    }
    public enum TwoFactorAuthTypeEnum : int
    {
        OnlyOTP = 1,
        OTPAndPassword = 2,
    }

    public enum MedicalCardTypeEnum : int
    {
        Gold = 0,
        Silver = 1,
        Bronze = 2
    }

    public enum PayrollRunTypeEnum : int
    {
        Salary = 0,
        // Adhoc = 1
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

    public enum PayrollExecutionStatusEnum : int
    {
        NotStarted = 0,
        Submitted = 1,
        InProgress = 2,
        Completed = 3,
        Error = 4
    }
    public enum EosTypeEnum : int
    {
        Resignation = 0,
        Termination = 1,
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

    public enum PayrollSchedulerActionEnum : int
    {
        ExecutePayroll = 0,
        LoadDailySalaryEntry = 1
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

    public enum ScheduleExecutionStatusEnum : int
    {
        Unknown = 0,
        Open = 1,
        Running = 2,
        Succeeded = 3,
        Error = 4
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

    public enum ExemptionPeriodEnum : int
    {
        Monthly = 0,
        Yearly = 1
    }

    public enum OTPaymentTypeEnum : int
    {
        Pay = 0,
        TimeOff = 1
    }

    public enum PaymentModeEnum : int
    {
        [Display(Name = "Bank Transfer")]
        [Description("Bank Transfer")]
        BankTransfer = 0,
        Cheque = 1,
        Cash = 2
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
    public enum PayrollPostedSourceEnum : int
    {
        Service = 0,
        Manual = 1,
        // Salary = 2,
        Payroll = 3,
        Accrual = 4
    }
    public enum PayrollProcessStatusEnum : int
    {
        NotProcessed = 0,
        Processed = 1,
        Draft = 2,
    }
    public enum LoggedInAsTypeEnum : int
    {
        LoginCredential = 1,
        LoginAsDifferentUser = 2,
        SwitchProfile = 3
    }
    public enum NtsPriorityEnum : int
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
    public enum ServiceSearchHomeByStatusEnum : int
    {
        [Description("Draft")]
        SERVICE_STATUS_DRAFT = 0,
        [Description("Cancel")]
        SERVICE_STATUS_CANCEL = 1,
        [Description("Complete")]
        SERVICE_STATUS_COMPLETE = 2,
        [Description("Overdue")]
        SERVICE_STATUS_OVERDUE = 3,
        [Description("In Progress")]
        SERVICE_STATUS_INPROGRESS = 4,

    }
    public enum ServiceSearchHomeByPersonEnum : int
    {

        [Description("Requested by me")]
        REQ_BY = 0,
        [Description("Shared with me/Team")]
        SHARE_TO = 1,
    }
    public enum Test : int
    {
        [Description("Zero Desc")]
        Zero = 0,
        [Description("Five Desc")]
        Five = 5,
        Minus = -1
    }
    public enum TaskSearchHomeByStatusEnum : int
    {
        [Description("Draft")]
        TASK_STATUS_DRAFT = 0,
        [Description("Cancel")]
        TASK_STATUS_CANCEL = 1,
        [Description("Completed")]
        TASK_STATUS_COMPLETE = 2,
        [Description("Overdue")]
        TASK_STATUS_OVERDUE = 3,
        [Description("Reject")]
        TASK_STATUS_REJECT = 4,
        [Description("In Progress")]
        TASK_STATUS_INPROGRESS = 5,
        [Description("Planned")]
        TASK_STATUS_PLANNED = 6,
        [Description("Planned Overdue")]
        TASK_STATUS_PLANNED_OVERDUE = 7,

    }
    public enum TaskSearchHomeByPersonEnum : int
    {
        [Description("Assigned to me")]
        ASSIGN_TO = 0,
        [Description("Requested by me")]
        ASSIGN_BY = 1,
        [Description("Shared with me/Team")]
        SHARE_TO = 2,
    }
    public enum NoteSearchHomByStatusEnum : int
    {
        [Description("Draft")]
        NOTE_STATUS_DRAFT = 0,
        //[Description("Canceled")]
        //CANCELED = 1,
        [Description("Complete")]
        NOTE_STATUS_COMPLETE = 2,
        [Description("Active")]
        NOTE_STATUS_INPROGRESS = 3,
        [Description("Expire")]
        NOTE_STATUS_EXPIRE = 4,

        //[Description("Shared with me/Team")]
        //SHARE_TO = 5,

    }
    public enum NoteSearchHomeByPersonEnum : int
    {


        [Description("Shared with me/Team")]
        SHARE_TO = 2,
        [Description("Shared by me/Team")]
        ASSIGN_BY = 1,
        [Description("Owned by me")]
        REQ_BY = 0,
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

    public enum DependantRelationshipTypeEnum : int
    {
        Husband = 0,
        Wife = 1,
        Son = 2,
        Daughter = 3,
        Father = 4,
        Mother = 5,
    }

    public enum PersonTitleEnum : int
    {
        Mr = 0,
        Mrs = 1,
        Ms = 2,
        Miss = 3,
        Dr = 4,
    }

    public enum ReligionEnum : int
    {
        Hindu = 0,
        Christian = 1,
        Islam = 2,
        Other = 3,
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
    public enum FilterOperatorEnum : int
    {
        [Display(Name = "contains")]
        [Description("contains")]
        contains = 0,
        [Display(Name = "does not contain")]
        [Description("does not contain")]
        notContains = 1,
        [Display(Name = "equal")]
        [Description("equal")]
        equals = 2,
        [Display(Name = "does not equal")]
        [Description("does not equal")]
        notEquals = 3,
        [Display(Name = "is set")]
        [Description("is set")]
        set = 4,
        [Display(Name = "is not set")]
        [Description("is not set")]
        notSet = 5,
        [Display(Name = "in Date Range")]
        [Description("in Date Range")]
        inDateRange = 6,
        [Display(Name = "not In Date Range")]
        [Description("not In Date Range")]
        notInDateRange = 7,
        [Display(Name = "after Date")]
        [Description("after Date")]
        afterDate = 8,
        [Display(Name = "before Date")]
        [Description("before Date")]
        beforeDate = 9,
    }
    public enum TimeDimensionRangeTypeEnum : int
    {
        [Display(Name = "custom")]
        [Description("custom")]
        custom = 0,
    }
    public enum TimeDimensionRangeByEnum : int
    {
        [Display(Name = "day")]
        [Description("day")]
        day = 0,
    }
    public enum FolderTypeEnum : int
    {
        Root = 0,
        Workspace = 1,
        Folder = 2,
        File = 3,
        Document = 4
    }
    public enum NtsModifiedStatusEnum
    {
        Created = 0,
        Modified = 1,
        Locked = 2,
        Deleted = 3,
        Archived = 4,
        RestoredDeleted = 5,
        RestoredArchived = 6,
        SyncError = 7,
        None = 8
    }
    public enum DmsPermissionTypeEnum : int
    {
        [Description("Allow")]
        Allow = 0,
        [Description("Deny")]
        Deny = 1
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
        //[Description("All Documents in this Folder and Sub Folders")]
        //AllDocuments = 4,
    }
    public enum SocialMediaTypeEnum : int
    {
        Twitter = 0,
        Instagram = 1,
        Facebook = 2,
        LinkedIn = 3,
        Youtube = 4,
        WhatsApp = 5,
    }
    public enum PostTypeEnum : int
    {
        Post = 0,
        Comment = 1,
        Chat = 2,
        Reply = 3,
    }
    public enum FiscalYearTypeEnum : int
    {
        StartsPreviousYear = 0,
        EndsNextYear = 1,
    }
    public enum DatabaseTypeEnum : int
    {
        Postgresql = 0,
        Sql = 1,
        Neo4j = 2,
        Mongo = 3,
    }
    public enum HierarchyPermissionEnum : int
    {
        All = 0,
        Parent = 1,
        Self = 2,
        LegalEntity = 3,
        Custom = 4
    }

    public enum RosterDutyTypeEnum : int
    {
        Pattern = 0,
        Custom = 1,
        DayOff = 2,
        PublicHoliday = 3
    }
    public enum AccessLogSourceEnum : int
    {
        Biometric = 0,
        Mobile = 1,
        Service = 2,
        Manual = 3,
    }

    public enum IncludeExclude : int
    {
        Include = 0,
        Exclude = 1
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

    public enum EmployeeStatusEnum : int
    {
        Active = 0,
        Terminated = 1,
        Resigned = 2,
        EndOfContract = 3,
        Inactive = 4,
        NoticePeriod = 5,
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

    public enum QuestionTypeEnum : int
    {
        [Description("Single Choice")]
        SingleChoice = 0,
        [Description("Multiple Choice")]
        MultipleChoice = 1,
        [Description("Single Choice Score")]
        SingleChoiceScore = 2,
        [Description("Multiple Choice Score")]
        MultipleChoiceScore = 3,
        [Description("Yes Or No")]
        YesOrNo = 4,
        [Description("True Or False")]
        TrueOrFalse = 5,
        [Description("Descriptive")]
        Descriptive = 6,
        [Description("Single Choice With Comment")]
        SingleChoiceWithComment = 7,
    }
    public enum OptionTypeEnum : int
    {
        [Description("Option")]
        Option = 0,
        [Description("All Of Above")]
        AllOfAbove = 1,
        [Description("None Of Above")]
        NoneOfAbove = 2,

    }

    public enum EDRMDRFileTypeEnum : int
    {
        EDR = 0,
        MDR = 1,
    }

    public enum SocialMediaDatefilters : int
    {
        Last5Mins = 14,
        Last15Mins = 15,
        Last30Mins = 16,
        Last1Hour = 17,
        Last4Hour = 18,
        Last8Hour = 19,
        Last12Hour = 19,
        Custom = 0,
        AllTime = 1,
        Today = 2,
        Yesterday = 3,
        ThisWeek = 4,
        ThisMonth = 5,
        //ThisQuarter = 6,
        ThisYear = 7,
        Last7Days = 8,
        Last30Days = 9,
        LastWeek = 10,
        LastMonth = 11,
        //LastQuarter = 12,
        LastYear = 13,
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
    public enum AssessmentScheduleTypeEnum : int
    {
        Assessment = 0,
        Interview = 1,
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
    public enum DocumentQueryTypeEnum : int
    {
        Folder = 0,
        Document = 1,
        Tag = 3
    }
    public enum AssignedTypeEnum : int
    {
        Team = 0,
        User = 1,
    }
    public enum LockStatusEnum : int
    {
        CheckedIn = 0,
        CheckedOut = 1
    }
    public enum BoolStatus : int
    {
        No = 0,
        Yes = 4,
    }

    public enum ItemTypeEnum
    {
        Service = 1,
        StepTask = 2,
        SubTask = 3,
        AdhocTask = 4,
        StepService = 5,
        RefNote = 6,
        RefTask = 7,
        RefService = 8,
        NoteRef = 9,
        TaskRef = 10,
        ServiceRef = 11,
        Note = 12,
        AdhocService = 13,
        AdhocNote = 14,
        Section = 15,
        Book = 16,
        BookSection = 17
    }
    public enum KanbanTemplateEnum : int
    {
        Custom = 0,
        Weekly = 1,
        Monthly = 2,
        Yearly = 3,
    }
    public enum WorkBoardItemTypeEnum : int
    {
        Text = 1,
        Index = 2,
        WhiteBoard = 3,
        Image = 4,
        Video = 5,
        File = 6,
        Note = 7,
        Task = 8,
        Servivce = 9,
    }

    public enum WorkBoardstatusEnum : int
    {
        Open = 0,
        Closed = 1
    }
    public enum WorkBoardSharingTypeEnum : int
    {
        Link = 0,
        IdKey = 1,
        Email = 2,
    }
    public enum WorkBoardContributionTypeEnum : int
    {
        Contributer = 0,
        Viewer = 1,
    }
    public enum WorkBoardItemShapeEnum : int
    {
        Square = 1,
        Title = 2,
        Octagen = 3,
        Hexagon = 4,
        Gem = 5,
        Triangle = 6,
    }
    public enum WorkBoardItemSizeEnum : int
    {
        Standard = 1,
        Wide = 2,
        Double = 3,
        Resizable = 4,
    }
    public enum BusinessHierarchyItemTypeEnum : int
    {
        ROOT = 0,
        LEVEL1 = 1,
        LEVEL2 = 2,
        LEVEL3 = 3,
        LEVEL4 = 4,
        BRAND = 5,
        MARKET = 6,
        PROVINCE = 7,
        DEPARTMENT = 8,
        CAREER_LEVEL = 9,
        JOB = 10,
        POSITION = 11,
        EMPLOYEE = 12,
    }
}

using System;

namespace ERP.Utility
{
    public class Constant
    {

        public const string ClockServerDisconnect = "Disconnected";
        public const string ClockServerConnect = "Conncected";
        public static long ApplicationCompanyId = 0;
        public static long DefaultCompanyId = 1;
        public static string VacantEmployeeText = "Vacant";
        public static string NewlyRequestedText = "Newly Requested";
        public const long WindowsServiceUserId = 2;
        public const long WcfServiceUserId = 3;
        public const long WebAppUserId = 1;
        public const long SupportUserId = 4;

        public static string KendoVersion = "2018.1.221";


        public static string InitialPopupUrl = "InitialPopupUrl";
        public static string JobTitle = "Job Title";
        public const string RelativeRoot = "../";
        public static string DocumentLocation = "Document/";

        public static string StaffImageFolder = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StaffImageFolder"]);
        public static string StaffResizedImageFolder = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StaffResizedImageFolder"]);
        public static string PhotoDir1Path = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PhotoDir1Path"]);
        public static string PhotoDir1ResizedPath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PhotoDir1ResizedPath"]);

        public static string PhotoDir1Domain = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PhotoDir1Domain"]);
        public static string PhotoDir1UserId = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PhotoDir1UserId"]);
        public static string PhotoDir1Pwd = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PhotoDir1Pwd"]);




        public static string HiqPdfSerialNumber = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["HiqPdfSerialNumber"]);
        public const string NationalityTypeKeyword = "emirati";
        public static int ResizedPhotoHeight = 140;
        public static int ResizedPhotoWidth = 120;

        public const int MaxPdfPageSize = 14000;
        public const double MaxFileSizeAllowed = 2147483648;/*536870912*//*changed .5gb to 2GB*/
        public const int NameStringLength = 500;
        public const int NameStringLength1 = 200;
        public const int LongStringLength = 2000;
        public static DateTime SystemMinDate = DateTime.MinValue;
        public static DateTime SystemMaxDate = DateTime.MaxValue;
        //public static DateTime MinApplicationDate = Convert.ToDateTime("01/01/1900");
        //public static DateTime MaxApplicationDate = Convert.ToDateTime("31/12/4712");
        public static DateTime ApplicationMinDate = new DateTime(1900, 1, 1);
        public static DateTime ApplicationMaxDate = new DateTime(4712, 12, 31);
        public const int ApplicationStartYear = 2019;

        public const string PositionHierarchyName = "DAC Position Hierarchy";
        public const string OrgHierarchyName = "Dubai Airport HR Hierarchy";
        public const string JDDownloadFileName = "JobDescription.docx";
        public const string MSWordContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public const string MSExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string ExcelExtension = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public const string EncryptionKey = "ZDqH28vV7twxB7ML";
        public const string DocExtension = ".docx";
        public const string DomainName = "dca";
        public const string QueryStringEncryptionParam = "enc=";
        public const string QueryStringVersionParam = "appVer=";



        public const string PlaceHolder_NationalityGroup = "Select Nationality Group ..";
        public const string PlaceHolder_Gender = "Select Gender ..";
        public const string PlaceHolder_MaritalStatus = "Select Marital Status ..";
        public const string PlaceHolder_Nationality = "Select Nationality ..";
        public const string PlaceHolder_Religion = "Select Religion ..";
        public const string PlaceHolder_EmployeeType = "Select Employee Type ..";
        public const string PlaceHolder_EmployeeStatus = "Select Employee Status ..";
        public const string PlaceHolder_TerminationReason = "Select Termination Reason ..";
        public const string PlaceHolder_CostCenter = "Select Cost Center ..";
        public const string PlaceHolder_Location = "Select Location ..";
        public const string PlaceHolder_OrganizationType = "Select Organization Type ..";
        public const string PlaceHolder_OrganizationGroup = "Select Organization Group ..";
        public const string PlaceHolder_PaymentMethod = "Select Payment Method ..";
        public static string PlaceHolder_SelectOption = "--Select--";
        public static string PlaceHolder_AllOption = "All";
        public const string PlaceHolder_UpdateMode = "Select Update Mode";
        public const string FaultExceptionMessage = "An error occurred while processing your request. Please try again after sometime or contact Administrator";



        public const string DAName = "DA";
        public const int OrganizationHierarchyNameId = 2;
        public const int PositionHierarchyNameId = 1;
        public const string DXBName = "DXB";
        public const string DWCName = "DWC";
        public const string NotApplicable = "Not Applicable";
        public const string NoParentEmployee = "No Parent Employee";
        public const string NoParentPosition = "No Parent Position";
        public const string NoParentOrganization = "No Parent Organization";
        public const string NoEmployee = "No Employee";
        public const int MaxEmailRetryCount = 5;

        public static string AppVersion
        {
            get { return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion; }
        }
        public static string AppVersionWithParam
        {
            get { return String.Concat(Constant.QueryStringVersionParam, AppVersion); }
        }

        public static string AppName
        {
            get { return AppSettings.AppName; }
        }
        public class Nts
        {
            public static readonly string DefaultCloseButtonName = "Close";
            public static readonly string DefaultCompleteButtonName = "Complete";
            public static readonly string DefaultSubmitButtonName = "Submit";
            public static readonly string DefaultSaveChangesButtonName = "Save Changes";
            public static readonly string DefaultAdhocTaskHeader = "Adhoc Tasks";
            public static readonly string DefaultAdhocTaskAddButtonText = "Create New Task";
            public static readonly string DefaultStepTaskAddButtonText = "Create New Task";
            public static readonly string DefaultAdhocTaskCancelButtonText = "Cancel";
            public static readonly string DefaultStepTaskCancelButtonText = "Cancel";
            public static readonly string CancelSubmitButtonName = "Cancel";
            public static readonly string DefaultReturnButtonName = "Return";
            public static readonly string DefaultRejectButtonName = "Reject";
            public static readonly string DefaultSaveAsDraftButtonName = "Save As Draft";
            public static readonly string DefaultCreateNewVersionName = "Edit";
            public static readonly string DefaultSaveNewVersionName = "Save";
            public static readonly string DefaultCancelEditButtonName = "Cancel Edit";
            public static readonly string DefaultServiceReferenceText = "View Service";
            public static readonly string DefaultBackButtonName = "Back";
            public static readonly string DefaultAssigneeName = "";
            public static readonly string DefaultDelegateButtonName = "Delegate";
            public static readonly string DefaultHeaderSectionName = "Header";
            public static readonly string DefaultSharedSectionName = "Sharing";
            public static readonly string DefaultStepSectionName = "Steps";
            public static readonly string DefaultStatusLabelText = "Status";
            public static readonly string DefaultFieldSectionName = "Details";
            public static readonly string DefaultResubmitButtonName = "ReSubmit";
            public static readonly string DefaultCodeLabelText = "Code";
            public static readonly string DefaultSequenceLabelText = "Sequence No";
            public static readonly int DefaultTemplateMaximumColumn = 2;
            public class HtmlStyle
            {
                public static readonly string FieldControlClass = "form-control";
                public static readonly string LabelControlClass = "control-label";
                public static readonly string Italic = "font-style:italic;";
                public static readonly string UnderLine = "text-decoration:underline;";
            }
        }

        public class Annotation
        {
            public const string DefaultDateTimeFormat = "{0:dd MMM yyyy HH:mm:ss}";
            public const string DefaultDateFormat = "{0:dd MMM yyyy}";
            public const string ToDayAndDateFormat = "{0:ddd, dd MMM yyyy}";
            public const string DateMonthAndYear = "{0:MMM yyyy}";
            public const string DefaultDateFormatOnly = "dd MMM yyyy";
            public const string DefaultDateTimeFormatOnly = "dd MMM yyyy HH:mm:ss";
            public const string DateTimeFormat = "{0:dd MMM yyyy HH:mm}";
            public const string LongDateTimeFormat = "{0:dd MMM yyyy HH:mm:ss}";
            public const string Long_YYYY_MM_DD = "{0:yyyy/MM/dd HH:mm:ss}";
            public const string DefaultTimeFormat = "{0:HH:mm}";
            public const string DayTimeFormat = "{0:d.HH:mm:ss}";
            public const string DayTimeFormatOnly = "d.HH:mm:ss";

            public const string UTCFormat = "{0:yyyy-MM-ddTHH:mm:ss.fffffff}";
            // public const string UTCFormat = "{0:yyyy-MM-ddTHH:mm:ss.fffffffK}";
            public const string YYYY_MM_DD = "{0:yyyy/MM/dd}";
            public const string ToYYYY_MM_DD_Dash = "{0:yyyy-MM-dd}";

            public const string DefaultHijiriDateFormatOnly = "dd/mm/yyyy";

            public const string LongTimeFormat = "{0:HH:mm:ss}";

            //public static DateTime MinimumDate = Convert.ToDateTime("01/01/1900");
            //public static DateTime MaximumDate = Convert.ToDateTime("31/12/4712");
            public const string DateRangeMessage = "Value for {0} must be between {1} and {2}";

            //public const string EffectiveStartDate = "Effective From Date";
            //public const string EffectiveEndDate = "Effective To Date";

            public static class Labels
            {
                public const string EffectiveFromDate = "Effective Start Date";
                public const string EffectiveToDate = "Effective End Date";
                public const string SequenceNo = "Sequence No";
            }

        }

        public class ApplicationVariables
        {

            //public static long ManpowerRootPositionId
            //{
            //    get { return 0; }
            //    // get { return Convert.ToString(System.Web.HttpContext.Current.Application[Constant.ApplicationSettingKey.General.ManpowerRootPositionIdKey]).ToSafeInt(); }
            //}
            //public static int ManpowerDAPositionId
            //{
            //    get { return Convert.ToString(System.Web.HttpContext.Current.Application[Constant.ApplicationSettingKey.General.ManpowerDAPositionIdKey]).ToSafeInt(); }
            //}

            public static int EmployeePreferenceShowPhotoValue
            {
                get { return Convert.ToString(System.Web.HttpContext.Current.Application[Constant.ApplicationSettingKey.General.EmployeePreferenceShowPhotoIdKey]).ToSafeInt(); }
            }
        }
        //public const string WcfNameSpace = "https://hrs.dubaiairports.ae";

        public class ApplicationSettingKey
        {
            public class General
            {
                public static string GRPRootOrganizationIdKey = "GRPRootOrganizationId";
                public static string GRPRootPositionIdKey = "GRPRootPositionId";
                public static string ManpowerRootPositionIdKey = "ManpowerRootPositionId";
                public static string ManpowerDAPositionIdKey = "ManpowerDAPositionId";

                public static string ManpowerRootOrganizationIdKey = "ManpowerRootOrganizationId";
                public static string EmployeePreferenceShowPhotoIdKey = "EmployeeShowPhoto";
                public static string LoginAsFrom = "Login_As_From";
                public static string LoginAsTo = "Login_As_To";

                public static string CEOGradeId = "CEOGradeId";
            }

            public class Integration
            {
                public static string LMSPriorDays = "LMS_Data_PriorDays";
                public static string LMSFTPUserFolder = "LMS_FTP_User_Folder";
                public static string LMSFTPOrgFolder = "LMS_FTP_Org_Folder";
                public static string LMSFTPPassword = "LMS_FTP_Password";
                public static string LMSFTPUserName = "LMS_FTP_UserName";
                public static string LMSFTPPort = "LMS_FTP_Port";
                public static string LMSFTPHost = "LMS_FTP_Host";


                public static string HavasFTPFolder = "Havas_FTP_Folder";
                public static string Havas_Load_Zero_Requision = "Havas_Load_Zero_Requision";
                public static string HavasFTPUserName = "Havas_FTP_Username";
                public static string HavasFTPPassword = "Havas_FTP_Password";
                public static string HavasFTPPort = "Havas_FTP_Port";
                public static string HavasFTPHost = "Havas_FTP_Host";

                public static string HCMCommand = "HCM_Command";
                public static string HCMCommandTimeout = "HCM_Command_Timeout_InMinutes";
                public static string TaleoCommandTimeout = "Taleo_Command_Timeout_InMinutes";
                public static string TaleoApplicantSharedFolder = "Taleo_Applicant_Shared_Folder";
                public static string TaleoRequisitionSharedFolder = "Taleo_Requisition_Shared_Folder";

                public static string TaleoSharedFolder = "TaleoSharedFolder";
                public static string TaleoSharedFolderUserId = "TaleoSharedFolderUserId";
                public static string TaleoSharedFolderPwd = "TaleoSharedFolderPwd";
                public static string TaleoSharedFolderDomain = "TaleoSharedFolderDomain";


                public static string HCMPriorMinutes = "HCM_Data_PriorMinutes";
                public static string SummaryTo = "Summary_To";
                public static string SummaryCC = "Summary_CC";
                public static string TaleoPriorDays = "Taleo_Data_PriorDays";
                public static string ServiceFileArchiveDays = "Service_File_Archive_Days";
                public static string ServiceDataArchiveDays = "Service_Data_Archive_Days";

                public static string GrpWebServiceMode = "Grp_WebService_Mode";
                public static string GrpWebServiceTestUid = "Grp_WebService_Test_Uid";
                public static string GrpWebServiceTestPwd = "Grp_WebService_Test_Pwd";
                public static string GrpWebServiceProdUid = "Grp_WebService_Prod_Uid";
                public static string GrpWebServiceProdPwd = "Grp_WebService_Prod_Pwd";

                public static string UcmWebServiceMode = "UCM_WebService_Mode";

                public static string UcmWebServiceTestUrl = "UCM_WebService_Test_Url";
                public static string UcmWebServiceTestUid = "UCM_WebService_Test_Uid";
                public static string UcmWebServiceTestPwd = "UCM_WebService_Test_Pwd";

                public static string UcmWebServiceProdUrl = "UCM_WebService_Prod_Url";
                public static string UcmWebServiceProdUid = "UCM_WebService_Prod_Uid";
                public static string UcmWebServiceProdPwd = "UCM_WebService_Prod_Pwd";


                public static string BIReportDownloadUrl = "BI_Report_Download_Url";
                public static string BIReportDownloadUid = "BI_Report_Download_Uid";
                public static string BIReportDownloadPwd = "BI_Report_Download_Pwd";

            }

            public class Smtp
            {
                public static string FromEmailId = "SmtpFromEmailId";
                public static string SenderName = "SmtpSenderName";
                public static string Host = "SmtpHost";
                public static string Port = "SmtpPort";
                public static string UserId = "SmtpUserId";
                public static string Password = "SmtpPassword";
            }



        }
        public class Directory
        {
            public const string DataRoot = @"Data\";
            public const string LMS = @"LMS\";
            public const string LMSArchive = @"LMS\Archive\";

            public const string HCM = @"HCM\";
            public const string HCMInbound = @"HCM\Inbound\";
            public const string HCMInboundArchive = @"HCM\Inbound\Archive\";
            public const string HCMInboundUtiltiy = @"HCMUtility\Inbound\";
            public const string HCMOutbound = @"HCM\Outbound\";
            public const string HCMPayrollOutbound = @"HCM\Outbound\Payroll\";
            public const string HCMOutboundArchive = @"HCM\Outbound\Archive\";
            public const string HCMOutboundUtiltiy = @"HCMUtility\Outbound\";
            public const string StaffDBUtiltiy = @"StaffDBUtility\";





            public const string Taleo = @"Taleo\";
            public const string Havas = @"Taleo\Havas";
            public const string HavasArchive = @"Taleo\Havas\Archive";
            public const string TaleoInbound = @"Taleo\Inbound\";
            public const string TaleoRequisition = @"Taleo\Requisition\";
            public const string TaleoRequisitionArchive = @"Taleo\Requisition\Archive\";
            public const string TaleoInboundArchive = @"Taleo\Inbound\Archive\";
            public const string TaleoInboundResult = @"Taleo\Inbound\Result\";
            public const string TaleoOutbound = @"Taleo\Outbound\";
            public const string TaleoOutboundArchive = @"Taleo\Outbound\Archive\";

            public const string TaleoOutboundDW = @"Taleo\Outbound\ToDW\";
            public const string TaleoOutboundDWRecruiting = @"Taleo\Outbound\ToDW\Recruiting\";
            public const string TaleoOutboundDWSmartOrg = @"Taleo\Outbound\ToDW\SmartOrg\";

            public const string TaleoOutboundAttachment = @"Taleo\Outbound\Attachments\";
            public const string TaleoOutboundAttachmentApplicant = @"Taleo\Outbound\Attachments\Applicant\";
            public const string TaleoOutboundAttachmentApplicantArchive = @"Taleo\Outbound\Attachments\Applicant\Archive\";
            public const string TaleoOutboundAttachmentRequisition = @"Taleo\Outbound\Attachments\Requisition\";
            public const string TaleoOutboundAttachmentRequisitionArchive = @"Taleo\Outbound\Attachments\Requisition\Archive\";

            public const string TaleoUtility = @"TaleoUtility\";

            public const string TaleoDataUtility = @"TaleoUtility\DataUtility\";
            public const string TaleoRequisitionUtility = @"TaleoUtility\RequisitiontUtility\";
            public const string TaleoDataUtilityBatchFolder = @"TaleoUtility\DataUtility\bin\Windows\";
            public const string TaleoDataUtilityLrdFolder = @"TaleoUtility\DataUtility\lrd\";

            public const string TaleoCandidateAttachmentUtility = @"TaleoUtility\CandidateAttachmentUtility\";
            public const string TaleoCandidateAttachmentBatchFolder = @"TaleoUtility\CandidateAttachmentUtility\bin\Windows\";
            public const string TaleoCandidateAttachmentLrdFolder = @"TaleoUtility\CandidateAttachmentUtility\lrd\";


            public const string TaleoRequisitionAttachmentUtility = @"TaleoUtility\RequisitionAttachmentUtility\";
            public const string TaleoRequisitionAttachmentBatchFolder = @"TaleoUtility\RequisitionAttachmentUtility\bin\Windows\";
            public const string TaleoRequisitionAttachmentLrdFolder = @"TaleoUtility\RequisitionAttachmentUtility\lrd\";


            public const string TaleoDataWarehousetUtility = @"TaleoUtility\DataWareHouseUtility\";
            public const string TaleoDataWarehouseBatchFolder = @"TaleoUtility\DataWareHouseUtility\bin\Windows\";
            public const string TaleoDataWarehouseLrdFolder = @"TaleoUtility\DataWareHouseUtility\lrd\";

        }
        public class Schema
        {
            public const string General = "dbo";
            public const string Manpower = "mpr";
            public const string GRP = "grp";
            public const string Admin = "adm";
            public const string NTI = "nti";
            public const string Temp = "tmp";
            public const string DataWarehouse = "dwh";
        }
        public class EnvironmentVariable
        {
            public DateTime CurrentDate
            {
                get { return DateTime.Now.ApplicationNow().Date; }
            }
            public DateTime CurrentDateAndTime
            {
                get { return DateTime.Now.ApplicationNow(); }
            }
            public int CurrentYear
            {
                get { return DateTime.Now.ApplicationNow().Year; }
            }
            public int CurrentMonth
            {
                get { return DateTime.Now.ApplicationNow().Month; }
            }
            public int CurrentDay
            {
                get { return DateTime.Now.ApplicationNow().Day; }
            }
        }

        public class SessionVariable
        {
            public const string PersonNo = "PersonNo";
            public const string EmployeeFullName = "EmployeeFullName";
            public const string EmployeeDisplayName = "EmployeeDisplayName";
            public const string EmployeeDisplayNameWithNo = "EmployeeDisplayNameWithNo";
            public const string UserDisplayName = "UserDisplayName";
            public const string UserId = "UserId";
            public const string LoggedInAsByUserId = "LoggedInAsByUserId";
            public const string LoggedInAsByUserName = "LoggedInAsByUserName";
            public const string LoggedInAsType = "LoggedInAsType";
            public const string Language = "Language";
            public const string LanguageName = "LanguageName";
            public const string TextDirection = "TextDirection";
            public const string UserName = "UserName";
            public const string EmailId = "EmailId";
            public const string IsAdmin = "IsAdmin";
            public const string LogoFileId = "LogoFileId";
            public const string UserAuthTypeCode = "UserAuthTypeCode";
            public const string CompanyId = "CompanyId";
            public const string CompanyCode = "CompanyCode";
            public const string PreferredLanguage = "PreferredLanguage";
            
            public const string CultureInfo = "CultureInfo";
            public const string DateFormat = "DateFormat";
            public const string DateTimeFormat = "DateTimeFormat";
            public const string PersonId = "PersonId";
            public const string PermissionCSV = "PermissionCSV";
            public const string PermissionModule = "PermissionModule";
            public const string SessionStartTime = "SessionStartTime";
            public const string PermissionSubModule = "PermissionSubModule";
            //  public const string IsAdmin = "IsAdmin";

            //public const string EmployeeRootPositionId = "EmployeeRootPositionId";
            public const string CompanyRootPositionId = "CompanyRootPositionId";
            public const string EmployeePositionId = "EmployeePositionId";
            public const string EmployeeJobName = "EmployeeJobName";
            public const string EmployeeJobId = "EmployeeJobId";
            public const string EmployeeOrganizationName = "EmployeeOrganizationName";
            public const string UserMobileNo = "UserMobileNo";
            public const string UserIqamahNo = "UserIqamahNo";
            public const string UserEmail = "UserEmail";
            public const string LegalEntityCode = "LegalEntityCode";
            public const string LegalEntityId = "LegalEntityId";
            public const string LegalEntityCount = "LegalEntityCount";


            public const string PositionChartId = "EmployeePositionChartId";
            public const string EmployeeRootOrganizationId = "EmployeeRootOrganizationId";
            public const string CompanyOrganizationId = "EmployeeRootOrganizationId";
            public const string EmployeeOrganizationId = "EmployeeOrganizationId";
            public const string OrgChartId = "EmployeeOrgChartId";

            public const string CCHolderOrganizationMapping = "CCHolderOrganizationMapping";
            public const string UserOrganizationMapping = "UserOrganizationMapping";
            public const string BPOrganizationMapping = "BPOrganizationMapping";
            public const string UserOrganizationMappingWithoutHierarchy = "UserOrganizationMappingWithoutHierarchy";
            public const string BPOrganizationMappingWithoutHierarchy = "BPOrganizationMappingWithoutHierarchy";
            public const string EmployeeGender = "EmployeeGender";
            public const string EmployeeReligion = "EmployeeReligion";
            public const string EmployeeNationalityId = "EmployeeNationalityId";
            public const string LicenseValidity = "LicenseValidity";
            public const string PasswordChangeRequired = "PasswordChangeRequired";
        }
        public class AppSettings
        {
            public static string ApplicationBaseUrl
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ApplicationBaseUrl"]); }
            }
            public static string LicenseApiBaseUrl
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["LSU"]); }
            }
            public static string LicensePrivateKey
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["LPK"]); }
            }
            public static string FileServerWebApiBaseUrl
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["FileServerWebApiBaseUrl"]); }
            }
            public static string UploadPath
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadPath"]); }
            }
            public static string AppName
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AppName"]); }
            }
            public static string TempFilePath
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["TempFilePath"]); }
            }
            public static string FileWebApiBaseUrl
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["FileWebApiBaseUrl"]); }
            }
            public static string TestEmailRecipients
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["TestEmailRecipients"]); }
            }
            public static string TestSmsRecipient
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["TestSmsRecipient"]); }
            }
            public static string PullCutOffDate
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PullCutOffDate"]); }
            }
            public static ApplicationEnvironmentEnum? ApplicationEnvironment
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ApplicationEnvironment"]).ToEnum<ApplicationEnvironmentEnum>(); }
            }
            public static string DataEncryptionKey
            {
                get { return Utility.Helper.Decrypt(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DataEncryptionKey"])); }
            }
            public static string ClusterEnvironment
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ClusterEnvironment"]); }
            }
            public static string SlaveWebApiUrl
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SlaveWebApiBaseUrl"]); }
            }
        }
        public class ConnectionString
        {
            public static string DBCS
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString); }
            }
        }
        public class Smtp
        {
            public static int Port
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"]).ToSafeInt(); }
            }
            public static string Host
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SmtpHost"]); }
            }
            public static string FromId
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SmtpFromId"]); }
            }
            public static string FromFriendlyName
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SmtpFromFriendlyName"]); }
            }
            public static string UserName
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SmtpUserName"]); }
            }
            public static string Password
            {
                get { return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"]); }
            }
        }
    }
}

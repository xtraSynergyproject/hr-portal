using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Synergy.App.DataModel
{
    public class User : DataModelBase
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string DepartmentName { get; set; }
        public string Password { get; set; }
        public string PhotoId { get; set; }
        public string Mobile { get; set; }
        // public StatusEnum UserStatus { get; set; }
        public string ForgotPasswordOTP { get; set; }
        public bool PasswordChanged { get; set; }
        public bool IsSystemAdmin { get; set; }
        public bool IsGuestUser { get; set; }

        public UserRole[] UserRoles { get; set; }
        public UserPermission[] UserPermissions { get; set; }

        public string SignatureId { get; set; }

        public bool EnableRegularEmail { get; set; }
        public bool EnableSummaryEmail { get; set; }

        [ForeignKey("User")]
        public string LineManagerId { get; set; }

        public string ActivationCode { get; set; }

        public string[] LegalEntityIds { get; set; }
        public string FaceDetectionDescriptors { get; set; }

        [ForeignKey("Sponsor")]
        public string SponsorId { get; set; }
        public LOV Sponsor { get; set; }
        public bool EnableTwoFactorAuth { get; set; }
        public TwoFactorAuthTypeEnum? TwoFactorAuthType { get; set; }
        public string TwoFactorAuthOTP { get; set; }
        public DateTime? OTPExpiry { get; set; }
        public string Address { get; set; }
        public bool OverrideCaptchaSettings { get; set; }
        public bool DisableCaptcha { get; set; }
        public bool OverrideTwoFactorAuthentication { get; set; }
        public string NationalId { get; set; }

    }
    [Table("UserLog", Schema = "log")]
    public class UserLog : User
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
        public DateTime LogEndDateTime { get; set; }
        public bool IsDatedLatest { get; set; }
        public bool IsVersionLatest { get; set; }
    }
}

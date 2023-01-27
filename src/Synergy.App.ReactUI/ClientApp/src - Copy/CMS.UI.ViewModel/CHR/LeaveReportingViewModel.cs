using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class LeaveReportingViewModel
    {
        public string RequestTime { get; set; }
        public string ServiceNo { get; set; }
        public string Status { get; set; }
        public string OwnerDisplayName { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string OwnerEmployeeNo { get; set; }
        public string OwnerSponsorhipNo { get; set; }
        public string OwnerJobTitle { get; set; }
        public string OwnerDepartmentName { get; set; }
        public string OwnerLocationtName { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveEndDate { get; set; }
        public string LeaveType { get; set; }
        public string NumberOfTickets { get; set; }
        public string LeaveDuration { get; set; }
        public string LeaveDurationAsPerWorkingDays { get; set; }
        public string LeaveBalance { get; set; }
        public bool AdvanceSalary { get; set; }
        public string Entitlement { get; set; }
        public string EntitlementContract { get; set; }
        public string CreatedDate { get; set; }
        public string PassportNumber { get; set; }
        public string CompanionName1 { get; set; }
        public string CompanionName2 { get; set; }
        public string CompanionName3 { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string WayOfPayment { get; set; }
        public string AuthorizedToUse { get; set; }
        public string JobName { get; set; }
        public string Vacation { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Grade { get; set; }
        public string TemplateName { get; set; }
        public string HireDate { get; set; }
        public string TelephoneNumber { get; set; }
        public string AddressDetail { get; set; }
        public string OtherInformation { get; set; }
        public string Session { get; set; }
        public string Days { get; set; }
        public string Before { get; set; }
        public string After { get; set; }

        public string ReturnReason { get; set; }
        public string ReturnDate { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class UserUploadSpreadsheetViewModel
    {
        public long? Id { get; set; }      
        public string LegalEntity { get; set; }
        public string PersonStatus { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PersonFullName { get; set; }
        public string PersonFullNameInArabic { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string DateOfBirth { get; set; }
        public string IqamahNo { get; set; }
        public string BiometricId { get; set; }
        public string PersonNo { get; set; }
        public string PresentUnitNumber { get; set; }
        public string PresentBuildingNumber { get; set; }
        public string PresentStreetName { get; set; }
        public string PresentCity { get; set; }
        public string PresentPostalCode { get; set; }
        public string PresentAdditionalNumber { get; set; }
        public string PresentNeighborhoodName { get; set; }

        public string HomeUnitNumber { get; set; }
        public string HomeBuildingNumber { get; set; }
        public string HomeStreetName { get; set; }
        public string HomeCity { get; set; }
        public string HomePostalCode { get; set; }
        public string HomeAdditionalNumber { get; set; }
        public string HomeNeighborhoodName { get; set; }

        public string EmergencyContactName1 { get; set; }
        public string EmergencyContactNo1 { get; set; }
        public string Relationship1 { get; set; }
        public string RelationshipType1 { get; set; }
        public string OtherRelation1 { get; set; }

        public string EmergencyContactName2 { get; set; }
        public string EmergencyContactNo2 { get; set; }
        public string Relationship2 { get; set; }
        public string RelationshipType2 { get; set; }
        public string OtherRelation2 { get; set; }
        public string UserId { get; set; }
        public string UserLoginType { get; set; }
        public string UserName { get; set; }
      // 
        public string Email { get; set; }
        public string AlternateEmail { get; set; }       
        public string RemoteWorkingUserId { get; set; }
        public string Mobile { get; set; }
        public string PersonalId { get; set; }
        public string DisableWebAccess { get; set; }
        public string DisableDesktopAccess { get; set; }
        public string DisableMobileAccess { get; set; }
        public string EnableEmailSummary { get; set; }
        public string EnableRegularSummary { get; set; }
        public string IsAdmin { get; set; }
        public string Status { get; set; }
       
        public string AssignmentEffectiveStartDate { get; set; }
       // public string AssignmentEffectiveEndDate { get; set; }
        public string AssignmentStatus { get; set; }
        public string ContractType { get; set; }
        public string ContractEffectiveStartDate { get; set; }
        //public string ContractEffectiveEndDate { get; set; }
        public string Sponsor { get; set; }
        public string DateOfJoin { get; set; }
        public string ContractId { get; set; }
        public string ContractEndDate { get; set; }
        public string ContractRenewable { get; set; }
        public string NoticePeriod { get; set; }
        public string Location { get; set; }
        public string DepartmentName { get; set; }
        public string PositionId { get; set; }
        public string Position { get; set; }
        public string JobId { get; set; }
        public string Job { get; set; }
        public string AssignmentId { get; set; }
        public string AssignmentType { get; set; }
        public string GradeId { get; set; }
        public string AssignmentGrade { get; set; }
        public string ProbationPeriod { get; set; }
        public string LineManager { get; set; }
      //  public string ParentPosition { get; set; }
        public string ParentPositionEffectiveStartDate { get; set; }
       
       // public string SalaryEffectiveEndDate { get; set; }
        public string PayGroup { get; set; }
        public string PayCalender { get; set; }
        public string UseTimeAndAttendenceModule { get; set; }
        public string IsEligibleForOverTime { get; set; }
        public string IsEligibleForEndOfService { get; set; }
        public string IsEligibleForFlightTicketForSelf { get; set; }
        public string IsEligibleForFlightTicketForDependents { get; set; }
        public string FlightTicketFrequency { get; set; }
        public string DisableFlightTicketInPayroll { get; set; }
        public string IsValidateDependentDocumentForBenefit { get; set; }
        public string IsEligibleForsalaryTransferLetter { get; set; }
        public string OverTimePaymentType { get; set; }
        public string PaymentMode { get; set; }
        public string BankBranch { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankIBNNumber { get; set; }
        public string UnpaidLeavesNotInSystem { get; set; }
        public string BasicSalary { get; set; }
        public string HousingAllowance { get; set; }
        public string TransportAllowance { get; set; }
        public string BasicSalaryEffectiveStartDate { get; set; }
        public string BasicSalaryEffectiveEndDate { get; set; }
        public string HousingAllowanceEffectiveStartDate { get; set; }
        public string HousingAllowanceEffectiveEndDate { get; set; }
        public string TransportAllowanceEffectiveStartDate { get; set; }
        public string TransportAllowanceEffectiveEndDate { get; set; }       
        public string SalaryInfoId { get; set; }
        public string ParentPositionRelationshipId { get; set; }
        public string SalaryEffectiveStartDate { get; set; }
        public string PersonRootId { get; set; }
        public string AssignmentRootId { get; set; }
    }


    public class BulkUserUploadSubmit
    {
        public List<UserUploadSpreadsheetViewModel> Created { get; set; }
        public List<UserUploadSpreadsheetViewModel> Updated { get; set; }
        public List<UserUploadSpreadsheetViewModel> Destroyed { get; set; }
    }
}



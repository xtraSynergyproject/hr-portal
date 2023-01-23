//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class HRS_PersonRoot : RootNodeBase
    {

    }
  
    public partial class HRS_Person : NodeDatedBase
    {
        [NotMapped]
        public long PersonId { get; set; }
 
        public string PensionNo { get; set; }
        public PersonTitleEnum? Title { get; set; }
        public GenderEnum? Gender { get; set; }
        public MaritalStatusEnum? MaritalStatus { get; set; }
        public ReligionEnum? Religion { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string Mobile { get; set; }
        public string NationalIdentityCardNo { get; set; }
        public string SponsorshipNo { get; set; }
        public string BiometricId { get; set; }
        public string SponsorName { get; set; }
        public string HealthCardNo { get; set; }
        //public long? PhotoId { get; set; }
        public string OldPersonNo { get; set; }
        public string PersonNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get { return string.Concat(FirstName, " ", MiddleName, MiddleName.IsNullOrEmptyOrWhiteSpace() ? "" : " ", LastName); } }
        [NotMapped]
        public string FullNameWithPersonNo { get { return string.Concat(FullName, PersonNo.IsNullOrEmptyOrWhiteSpace() ? "" : "-", PersonNo); } }
        [NotMapped]
        public string FullNameWithSponsorshipNo { get { return string.Concat(FullName, SponsorshipNo.IsNullOrEmptyOrWhiteSpace() ? "" : "-", SponsorshipNo); } }
        [NotMapped]
        public string DisplayName { get { return string.Concat(FirstName, " ", LastName); } }
        [NotMapped]
        public string DisplayNameWithPersonNo { get { return string.Concat(DisplayName, PersonNo.IsNullOrEmptyOrWhiteSpace() ? "" : "-", PersonNo); } }
        [NotMapped]
        public string DisplayNameWithSponsorshipNo { get { return string.Concat(DisplayName, SponsorshipNo.IsNullOrEmptyOrWhiteSpace() ? "" : "-", SponsorshipNo); } }
        public string FullNameLocal { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string PersonalEmail { get; set; }
        public string BirthTown { get; set; }

        public ApprovalStatusEnum ApprovalStatus { get; set; }
        public override bool IsActive(DateTime? asofDate = null)
        {
            return base.IsActive(asofDate) && ApprovalStatus == ApprovalStatusEnum.Approved;

        }
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
        public RelativeTypeEnum? Relationship1 { get; set; }
        public RelationshipTypeEnum? RelationshipType1 { get; set; }
        public string OtherRelation1 { get; set; }

        public string EmergencyContactName2 { get; set; }
        public string EmergencyContactNo2 { get; set; }
        public RelativeTypeEnum? Relationship2 { get; set; }
        public RelationshipTypeEnum? RelationshipType2 { get; set; }
        public string OtherRelation2 { get; set; }
        public EmployeeStatusEnum? EmployeeStatus { get; set; }

    }
    public class R_PersonRoot : RelationshipDatedBase
    {

    }
    //public class R_PersonRoot_OrganizationRoot : RelationshipBase
    //{

    //}
    public class R_Person_PersonType : RelationshipBase
    {

    }
    public class R_Person_Nationality : RelationshipBase
    {

    }
    public class R_Person_BirthCountry : RelationshipBase
    {

    }
    public class R_PersonRoot_Section : RelationshipBase
    {
        public virtual DateTime EffectiveStartDate { get; set; }
        public virtual DateTime? EffectiveEndDate { get; set; }
    }
    public class R_PersonRoot_SubSection_Section : RelationshipBase
    {
        public virtual DateTime EffectiveStartDate { get; set; }
        public virtual DateTime? EffectiveEndDate { get; set; }
    }
    public class R_PersonRoot_LegalEntity_OrganizationRoot : RelationshipBase
    {

    }
    public class R_Person_PresentCountry : RelationshipBase
    {

    }
    public class R_Person_HomeCountry : RelationshipBase
    {

    }
    public class R_Person_ContactCountry : RelationshipBase
    {

    }
    public class R_Person_EmergencyContact1_Country : RelationshipBase
    {

    }
    public class R_Person_EmergencyContact2_Country : RelationshipBase
    {

    }
    public partial class HRS_Person_Logs : HRS_Person
    {
        public string PersonTypeCode { get; set; }
        public string PersonTypeName { get; set; }
        public string NationalityName { get; set; }
        public string EmployeeStatusName { get; set; }
        public string WorkLocationName { get; set; }
        public string BirthCountryName { get; set; }
        public string PresentConutryName { get; set; }
        public string HomeConutryName { get; set; }
        public string ContactConutryName { get; set; }
        public string EmergencyContactConutryName1 { get; set; }
        public string EmergencyContactConutryName2 { get; set; }
        public string EmergencyContactCountryDialCode1 { get; set; }
        public string EmergencyContactCountryDialCode2 { get; set; }
        public string LegalEntityName { get; set; }
        public long? PhotoId { get; set; }
    }
}

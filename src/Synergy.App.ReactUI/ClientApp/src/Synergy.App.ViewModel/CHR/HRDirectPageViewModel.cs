using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;


namespace Synergy.App.ViewModel
{
    public class HRDirectPageViewModel 
    {
        public string PersonalStatus { get; set; }
        public string Title { get; set; }
        public string MiddleName { get; set; }
        private string _PersonFullName { get; set; }
        public string PersonFullName
        {
            set { _PersonFullName = value; }
            get
            {
                return _PersonFullName.IsNullOrEmpty() ? string.Concat(FirstName, " ", MiddleName, MiddleName.IsNullOrEmptyOrWhiteSpace() ? "" : " "
                    , LastName, "-", PersonNo) : _PersonFullName;
            }
        }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string NationalId { get; set; }
        public string PersonNo { get; set; }
        public string LegalEntity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullNameInArabic { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string BiometricId { get; set; }
        public string UnitNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string StreetName { get; set; }
        public string CityTown { get; set; }
        public string PostalCode { get; set; }
        public string AdditionalNumber { get; set; }
        public string NeighborhoodName { get; set; }
        public string Country { get; set; }
        public string UnitNumber1 { get; set; }
        public string BuildingNumber1 { get; set; }
        public string StreetName1 { get; set; }
        public string CityTown1 { get; set; }
        public string PostalCode1 { get; set; }
        public string AdditionalNumber1 { get; set; }
        public string NeighborhoodName1 { get; set; }
        public string Country1 { get; set; }
        public string PersonalEmail { get; set; }
        public string Country2 { get; set; }
        public string CountryCode { get; set; }
        public string MobileNumber { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string OtherRelation { get; set; }
        public string Country3 { get; set; }
        public string CountryCode1 { get; set; }
        public string MobileNumber1 { get; set; }
        public string Name1 { get; set; }
        public string Relationship1 { get; set; }
        public string OtherRelation1 { get; set; }
        public string CountryCode2 { get; set; }
        public string Country4 { get; set; }
        public string MobileNumber2 { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public DateTime? BirthPlace { get; set; }
        public DateTime? BloodGroup { get; set; }
        public string CurrentTabInfo { get; set; }
        public DataActionEnum DataAction { get; set; }
        public string PhotoId { get; set; }
    }
}

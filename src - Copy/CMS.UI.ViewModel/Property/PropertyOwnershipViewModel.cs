using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class PropertyOwnershipViewModel:ServiceTemplateViewModel
    {
        public string PropertyNo { get; set; }

        public string ContactAccount { get; set; }

        public string RegistryNo { get; set; }

        public string NamantranPurpose { get; set; }
        public string OldOwnerName { get; set; }
        public string OwnerName { get; set; }
        public string MobileNo { get; set; }
        public string OldSurname { get; set; }
        public string Surname { get; set; }
        public string MiddleNameOfOwner { get; set; }
        public string OldMiddleNameOfOwner { get; set; }
        public string FatherOrHusbandName { get; set; }
        public string FatherMiddleName { get; set; }
        public string FatherOrHusbandSurname { get; set; }
        public string OldFatherOrHusbandName { get; set; }
        public string OldFatherMiddleName { get; set; }
        public string OldFatherOrHusbandSurname { get; set; }
        public string ULBNo { get; set; }
        public string WardNo { get; set; }
        public string HouseNo { get; set; }
        public string OldHouseNo { get; set; }
        public string Zone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string OldCity { get; set; }
        public string OldColony { get; set; }
        public string OldPinCode { get; set; }
        public string Localit { get; set; }
        public string OldLocalit { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public string JointOwner1FirstName { get; set; }
        public string JointOwner2FirstName { get; set; }
        public string JointOwner3FirstName { get; set; }
        public string JointOwner4FirstName { get; set; }
        public string JointOwner5FirstName { get; set; }
        public string JointOwner6FirstName { get; set; }
        public string JointOwner1MiddleName { get; set; }
        public string JointOwner2MiddleName { get; set; }
        public string JointOwner3MiddleName { get; set; }
        public string JointOwner4MiddleName { get; set; }
        public string JointOwner5MiddleName { get; set; }
        public string JointOwner6MiddleName { get; set; }
        public string JointOwner1LastName { get; set; }
        public string JointOwner2LastName { get; set; }
        public string JointOwner3LastName { get; set; }
        public string JointOwner4LastName { get; set; }
        public string JointOwner5LastName { get; set; }
        public string JointOwner6LastName { get; set; }
        public bool IsSelfDeclared { get; set; }
        public string PhotoId { get; set; }
        public string RegisteredSaleTransfer { get; set; }
        public string DocumentsSupportingTransfer { get; set; }
        public string PropertyTaxReceipt { get; set; }
        public string WarisanTransferDocuments { get; set; }
        public string OtherDocuments { get; set; }
    }

    public class PropertyAreaDetailsViewModel
    {
        public string UsageType { get; set; }
        public string UsageFactor { get; set; }
        public string FloorNo { get; set; }
        public string TypeOfConstruction { get; set; }
        public string Area { get; set; }
        public double AnnualLettingValue { get; set; }
        public double NetALV { get; set; }
    }
}

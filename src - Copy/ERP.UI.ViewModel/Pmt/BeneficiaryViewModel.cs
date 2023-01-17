using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class BeneficiaryViewModel : NoteViewModel
    {
        [Display(Name = "Name")]
        public string BeneficiaryName { get; set; }
        [Display(Name = "Address")]
        public string BeneficiaryAddress { get; set; }
        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }
        [Display(Name = "VAT No")]
        public string VATNo { get; set; }
        //Bank
        [Display(Name = "Bank")]
        public long? BankId { get; set; }
        [Display(Name = "Bank")]
        public string BankName { get; set; }
        [Display(Name = "Account No")]
        public string AccountNo { get; set; }
        [Display(Name = "IBAN")]
        public string Iban { get; set; }
        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }
        [Display(Name = "Branch")]
        public string Branch { get; set; }
        //Bank1
        [Display(Name = "Bank")]
        public long? BankId1 { get; set; }
        [Display(Name = "Bank")]
        public string BankName1 { get; set; }
        [Display(Name = "Account No")]
        public string AccountNo1 { get; set; }
        [Display(Name = "IBAN")]
        public string Iban1 { get; set; }
        [Display(Name = "Swift Code")]
        public string SwiftCode1 { get; set; }
        [Display(Name = "Branch")]
        public string Branch1 { get; set; }
        //Bank2
        [Display(Name = "Bank")]
        public long? BankId2 { get; set; }
        [Display(Name = "Bank")]
        public string BankName2 { get; set; }
        [Display(Name = "Account No")]
        public string AccountNo2 { get; set; }
        [Display(Name = "IBAN")]
        public string Iban2 { get; set; }
        [Display(Name = "Swift Code")]
        public string SwiftCode2 { get; set; }
        [Display(Name = "Branch")]
        public string Branch2 { get; set; }
        //Bank3
        [Display(Name = "Bank")]
        public long? BankId3 { get; set; }
        [Display(Name = "Bank")]
        public string BankName3 { get; set; }
        [Display(Name = "Account No")]
        public string AccountNo3 { get; set; }
        [Display(Name = "IBAN")]
        public string Iban3 { get; set; }
        [Display(Name = "Swift Code")]
        public string SwiftCode3 { get; set; }
        [Display(Name = "Branch")]
        public string Branch3 { get; set; }
        //Bank4
        [Display(Name = "Bank")]
        public long? BankId4 { get; set; }
        [Display(Name = "Bank")]
        public string BankName4 { get; set; }
        [Display(Name = "Account No")]
        public string AccountNo4 { get; set; }
        [Display(Name = "IBAN")]
        public string Iban4 { get; set; }
        [Display(Name = "Swift Code")]
        public string SwiftCode4 { get; set; }
        [Display(Name = "Branch")]
        public string Branch4 { get; set; }
        //Bank5
        [Display(Name = "Bank")]
        public long? BankId5 { get; set; }
        [Display(Name = "Bank")]
        public string BankName5 { get; set; }
        [Display(Name = "Account No")]
        public string AccountNo5 { get; set; }
        [Display(Name = "IBAN")]
        public string Iban5 { get; set; }
        [Display(Name = "Swift Code")]
        public string SwiftCode5 { get; set; }
        [Display(Name = "Branch")]
        public string Branch5 { get; set; }


        public long? ServiceId { get; set; }
        [Display(Name = "Service No")]
        public string ServiceNo { get; set; }
        [Display(Name = "Service Status")]
        public string ServiceStatus { get; set; }

    }
}

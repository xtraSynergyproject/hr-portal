using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class MedFitCertificateViewModel : FormTemplateViewModel
    {
        public string FirstName { get; set; }
        public string BirthPlace { get; set; }
        public long? Age { get; set; }
        public string FatherName { get; set; }
        public string Village { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
        public string Purpose { get; set; }

        public string MedicalOfficerSignature { get; set; }
        public string MedicalOfficerName { get; set; }
        public string RegistrationNumber { get; set; }
        public string ApplicationId { get; set; }
        public string SignatureId { get; set; }



    }
}

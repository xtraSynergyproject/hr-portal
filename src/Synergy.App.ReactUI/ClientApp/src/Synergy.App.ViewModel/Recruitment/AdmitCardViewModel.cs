using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class AdmitCardViewModel
    {
        public string ApplicationId { get; set; }
        public string RollNo { get; set; }
        public string CandidateName { get; set; }
        public string ExamCenter { get; set; }
        public DateTime ExamDate { get; set; }
        public DateTime? AppointmentLetterDate { get; set; }
        public string CompleteAddress { get; set; }
        public string CityPinCode { get; set; }
        public string StateCountry { get; set; }
        public string BasicSalary { get; set; }
        public string HRA { get; set; }
        public string ExGratiaBonus { get; set; }
        public string ChildEducationBonus { get; set; }
        public string TransportAllowance { get; set; }
        public string UniformMaintenanceAllowance { get; set; }
        public IList<RecCandidatePayElementViewModel> CandidateElementData { get; set; }
    }
}

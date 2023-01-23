using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class CompetenceMatrixViewModel : ServiceTemplateViewModel
    {
       public string RequirementQualification { get; set; }
        public string RequirementTechnical { get; set; }
        public string RequirementSpecialization { get; set; }
        public string RequirementITSkills { get; set; }
        public string RequirementExperience { get; set; }
        public string ActualQualification { get; set; }
        public string ActualTechnical { get; set; }
        public string ActualExperience { get; set; }
        public string ActualSpecialization { get; set; }
        public string ActualITSkills { get; set; }
        public string NatureOfWork { get; set; }
        public string FieldOfExposure { get; set; }
        public string TrainingsUndergone { get; set; }
        public string PositionsWorked { get; set; }
        public string CertificateCourse { get; set; }
        public string DrivingLicense { get; set; }
        public string CountriesWorked { get; set; }
        public string ExtraCurricular { get; set; }
        public string OrganizationWorked { get; set; }
        public string AnyOtherLanguage { get; set; }
        public string TempCode { get; set; }
        public string Source { get; set; }
        public string TaskStatus { get; set; }

    }
}

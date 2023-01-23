using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PersonDependentViewModel
    {
        public long PersonId { get; set; }
        [Display(Name = "Person No")]
        public string PersonNo { get; set; }
        [Display(Name = "Person Name")]
        public string PersonName { get; set; }
        public long DependentId { get; set; }
        [Display(Name = "Relationship Type")]
        public DependantRelationshipTypeEnum RelationshipType { get; set; }
        [Display(Name = "Relative Name")]
        public string DependentName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public ReligionEnum? Religion { get; set; }
        public GenderEnum? Gender { get; set; }
        public string SponsorshipNo { get; set; }
        public MaritalStatusEnum? MaritalStatus { get; set; }
    }
}

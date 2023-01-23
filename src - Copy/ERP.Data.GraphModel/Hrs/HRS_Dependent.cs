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
    public partial class HRS_DependentRoot : RootNodeBase
    {

    }
    public partial class HRS_Dependent : NodeDatedBase
    {
        [NotMapped]
        public long DependentId { get; set; }
        public DependantRelationshipTypeEnum? RelationshipType { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public GenderEnum? Gender { get; set; }
        public string SponsorshipNo { get; set; }
        public DateTime? DateOfBirth { get; set; }        
        public string BirthTown { get; set; }
        public string SponsorId { get; set; }
    }

    public class R_DependentRoot : RelationshipDatedBase
    {

    }
    public class R_DependentRoot_PersonRoot : RelationshipBase
    {

    }    
    public class R_Dependent_BirthCountry : RelationshipBase
    {

    }

    public class R_Dependent_Nationality : RelationshipBase
    {

    }
    public partial class HRS_Dependent_Logs : HRS_Dependent
    {
        public string BirthCountryName { get; set; }
        public long? DocumentId { get; set; }
        public string PersonFullName { get; set; }
        public string NationalityName { get; set; }
        public string PersonNo { get; set; }
    }

}
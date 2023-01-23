using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class NotificationUser
    {
        public long UserId { get; set; }
        public long? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public PersonTitleEnum Title { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
     
        public long? PersonId { get; set; }

        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }

        public long? LocationId { get; set; }
        public long? JobId { get; set; }
        public long? GradeId { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        public long? PositionId { get; set; }
        public long? SupervisorId { get; set; }

        public string LocationName { get; set; }
        public string JobName { get; set; }
        public string GradeName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string PositionName { get; set; }
        public string SupervisorName { get; set; }

        public GenderEnum Gender { get; set; }
        public ReligionEnum Religion { get; set; }
        public MaritalStatusEnum MaritalStatus { get; set; }
        public long NationalityId { get; set; }

 

    }
}

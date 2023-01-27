using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class TeamViewModel : ViewModelBase
    {
        //public long TeamId { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Admin.Team))]
        public string Name { get; set; }
        public string Code { get; set; }
        public string GroupCode { get; set; }
        public string Description { get; set; }
        public WorkAssignmentTypeEnum TeamWorkAssignmentType { get; set; }
        [Display(Name = "Users", ResourceType = typeof(ERP.Translation.Admin.Team))]
        public string Users { get; set; }
        public IList<TeamMemberViewModel> MemberList { get; set; }
        //[Required]
        [Display(Name = "TeamOwner", ResourceType = typeof(ERP.Translation.Admin.Team))]
        //[Display(Name = "Team Ownwer")]
        public long? TeamOwner { get; set; }
        public bool IsTeamOwner { get; set; }

        //[Display(Name = "Select User to Add")]
        [Display(Name = "SelectedUser", ResourceType = typeof(ERP.Translation.Admin.Team))]
        public long SelectedUser { get; set; }

        public long TeamMemberCount { get; set; }

        public TeamTypeEnum? TeamType { get; set; }

        [Display(Name = "Attachment")]
        public long? Attachment { get; set; }
        public FileViewModel SelectedFile { get; set; }
        [Display(Name = "Parent Team")]
        public long? ParentTeamId { get; set; }
        [Display(Name = "Parent Team")]
        public string ParentTeam { get; set; }

        [Display(Name = "Support Category")]
        public long? SupportCategoryId { get; set; }
        [Display(Name = "Support Template Category")]
        public string SupportCategory { get; set; }
        public string Source { get; set; }

        public long? TeamUserId { get; set; }
        public string MemberName { get; set; }
        public string Color { get; set; }
        public long? UserId { get; set; }
        public long? LegalEntityId { get; set; }
    }
}



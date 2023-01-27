using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
    public class WorkBoardInviteViewModel : NoteTemplateViewModel
    {     
        
        public string WorkBoardId { get; set; }
        public string TemplateContent { get; set; }
        public string WorkBoardUniqueId { get; set; }
        public string ShareKey { get; set; }
        public string EmailAddress { get; set; }
        public string OptionalMessage { get; set; }
        public string Link { get; set; }
        public List<WorkBoardInviteDetailsViewModel> Invites { get; set; }
       
        public WorkBoardSharingTypeEnum WorkBoardSharingType { get; set; }
        public WorkBoardContributionTypeEnum WorkBoardContributionType { get; set; }
    }

    public class WorkBoardInviteDetailsViewModel
    {
        public string WorkBoardId { get; set; }
        public string Link { get; set; }
        public string WorkBoardUniqueId { get; set; }
        public string ShareKey { get; set; }
        public WorkBoardSharingTypeEnum WorkBoardSharingType { get; set; }
        public WorkBoardContributionTypeEnum WorkBoardContributionType { get; set; }
    }
}

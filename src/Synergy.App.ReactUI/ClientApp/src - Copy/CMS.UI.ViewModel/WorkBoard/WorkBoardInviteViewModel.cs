using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;

namespace CMS.UI.ViewModel
{
    public class WorkBoardInviteViewModel : NoteTemplateViewModel
    {     
        
        public string WorkBoardId { get; set; }
        public string WorkBoardUniqueId { get; set; }
        public string WorkBoardKey { get; set; }
        public string EmailAddress { get; set; }
        public string OptionalMessage { get; set; }
        public string Link { get; set; }
        public WorkBoardSharingTypeEnum WorkBoardSharingType { get; set; }
        public WorkBoardContributionTypeEnum WorkBoardContributionType { get; set; }
    }
}

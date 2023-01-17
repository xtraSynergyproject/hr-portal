using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
    public class WorkBoardViewModel : NoteTemplateViewModel
    {
        public string WorkboardId { get; set; }
        public string JsonContent { get; set; }
        public string TemplateTypeId { get; set; }
        public string TemplateTypeName { get; set; }
        public string TemplateTypeCode { get; set; }
        public string WorkBoardName { get; set; }
        public string WorkBoardDescription { get; set; }
        public string ColorCode { get; set; }
        public string Message { get; set; }
        public WorkBoardstatusEnum WorkBoardStatus { get; set; }
        public string boards { get; set; }
        public KanbanTemplateEnum kanbanTemplate { get; set; }
        public List<WorkBoardSectionViewModel> WorkBoardSections { get; set; }
        public DateTime? WorkBoardDate { get; set; }
        public string NoteId { get; set; }
        public string IndexId { get; set; }
        public string WorkboardUniqueId { get; set; }
        public string ShareKey { get; set; }
        public string[] WorkBoardTeamIds { get; set; }
        public string WorkBoardTeams { get; set; }
        public WorkBoardOwnerTypeEnum? OwnerType { get; set; }
        public WorkBoardContributionTypeEnum? WorkBoardContributionType { get; set; }
        public WorkBoardTypeEnum WorkBoardType { get; set; }



    }
}

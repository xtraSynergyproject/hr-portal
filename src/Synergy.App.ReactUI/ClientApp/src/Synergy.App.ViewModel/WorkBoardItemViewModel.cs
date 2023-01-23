using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
    public class WorkBoardItemViewModel : NoteTemplateViewModel
    {
        public WorkBoardItemTypeEnum ItemType { get; set; }
        public string id { get; set; }
        public string ItemContent { get; set; }
        public string ItemName { get; set; }
        public string ItemContentIndex { get; set; }
        public string ItemFileId { get; set; }
        public string ItemFileFileId { get; set; }
        public string WorkBoardId { get; set; }
        public string ParentId { get; set; }
        public string ColorCode { get; set; }
        public WorkBoardItemShapeEnum WorkBoardItemShape { get; set; }
        public WorkBoardItemSizeEnum WorkBoardItemSize { get; set; }
        public string ColorName 
        { 
            get {
                switch (ColorCode)
                {
                    case "#f9eb71": 
                        return "yellow";
                    case "#ee85c1":
                        return "pink";
                    case "#a7d96d":
                        return "green";
                    case "#4bc1ec":
                        return "blue";
                    case "#ac8fef":
                        return "purple";
                    case "#a0adbd":
                        return "grey";
                    default:
                        return "yellow";
                }
            } 
        }
        public string WorkBoardSectionId { get; set; }
        public string WorkBoardItemId { get; set; }
        public string NtsNoteId { get; set; }
        public List<WorkBoardItemViewModel> item { get; set; }
    }
}

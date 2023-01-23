using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;

namespace CMS.UI.ViewModel
{
    public class WorkBoardSectionViewModel : NoteTemplateViewModel
    {
        public string id { get; set; }
        public string title { get; set; }      
        public string WorkBoardId { get; set; }
        public string SectionName { get; set; }
        public string HeaderColor { get; set; }
        public string SectionDescription { get; set; }
        public int SectionDigit { get; set; }
        public string WorkBoardSectionId { get; set; }
        public string NtsNoteId { get; set; }
        public string HeaderColorName
        {
            get
            {
                switch (HeaderColor)
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
                        return "blue";
                }
            }
        }

        public List<WorkBoardItemViewModel> item { get; set; }
    }
}

using CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class AssessmentSetAssessmentViewModel
    {
        public string Id { get; set; }
        public string Ids { get; set; }
        public string NoteId { get; set; }
        public string AssessmentId { get; set; }
        public string AssessmentSetId { get; set; }
        public string AssessmentType { get; set; }
        public DataActionEnum DataAction { get; set; }
        public long? SequenceOrder { get; set; }
    }
}

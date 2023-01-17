using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class ApplicationStateCommentViewModel : NoteTemplateViewModel
    {
        public string ApplicationId { get; set; }
        public string ApplicationStateId { get; set; }
        public string Comment { get; set; }
        public string CommentedBy { get; set; }
        public string TemplateCode { get; set; }
        public string ApplicationNo { get; set; }
        public string ApplicationStateCode { get; set; }
    }
}

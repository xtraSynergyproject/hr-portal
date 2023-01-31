using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class PostMessageViewModel : NoteTemplateViewModel
    {
        public string TeamId { get; set; }
        public decimal? SequenceNo { get; set; }
        public string TeamName { get; set; }
        public string SourcePost { get; set; }
        public bool IsUserGuide { get; set; }
        public bool IsPrivate { get; set; }
        public String CloudDocumentUrl { get; set; }
        public string RequestSource { get; set; }
        public NoteReferenceTypeEnum? ReferenceType { get; set; }       
        public string ReferenceTo { get; set; }
        public bool? EnableBroadcast { get; set; }

    }
}

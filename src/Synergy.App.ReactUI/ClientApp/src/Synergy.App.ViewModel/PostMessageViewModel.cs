using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class PostMessageViewModel : NoteTemplateViewModel
    {
        public string Message { get; set; }
        public string MenuName { get; set; }
        public string PostSequence { get; set; }
        public string GoogleDocumentUrl { get; set; }
        public string DocumentId { get; set; }
        public string PostId { get; set; }
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
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhotoId { get; set; }
        public string JobTitle { get; set; }
        public string CreatedByName { get; set; }
        public string MediaNewType { get; set; }
        public string StreamVideoPath { get; set; }
        public string FileId { get; set; }
        public bool? EnableBroadcast { get; set; }
        public long LikesCount { get; set; }
        public long DislikesCount { get; set; }
        public long CommentsCount { get; set; }
        public string Comment { get; set; }

    }
}

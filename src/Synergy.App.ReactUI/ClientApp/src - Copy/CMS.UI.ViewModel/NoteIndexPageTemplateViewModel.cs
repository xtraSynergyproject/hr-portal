using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class NoteIndexPageTemplateViewModel : NoteIndexPageTemplate
    {
        public string RowData { get; set; }
        public IList<NoteIndexPageColumnViewModel> SelectedTableRows { get; set; }
       // public IList<NoteIndexPageColumnViewModel> NoteIndexPageColumns { get; set; }
        public PageViewModel Page { get; set; }
        public string PageId { get; set; }
        public string TableMetadataId { get; set; }

        public long CreatedByMeDraftCount { get; set; }
        public long CreatedByMeCompleteCount { get; set; }
        public long CreatedByMeExpireCount { get; set; }

        public long RequestedByMeDraftCount { get; set; }
        public long RequestedByMeCompleteCount { get; set; }
        public long RequestedByMeExpireCount { get; set; }

        public long SharedWithMeDraftCount { get; set; }
        public long SharedWithMeCompleteCount { get; set; }
        public long SharedWithMeExpireCount { get; set; }

        public long SharedByMeDraftCount { get; set; }
        public long SharedByMeCompleteCount { get; set; }
        public long SharedByMeExpireCount { get; set; }
        public long CreatedOrRequestedByMeDraftCount { get; set; }
        public long CreatedOrRequestedByMeInProgreessOverDueCount { get; set; }
        public long CreatedOrRequestedByMeCompleteCount { get; set; }
        public long CreatedOrRequestedByMeExpiredCancelCloseCount { get; set; }
        public long SharedWithMeInProgressOverDueCount { get; set; }
        public long SharedWithMeExpiredCancelCloseCount { get; set; }
        public long SharedByMeInProgreessOverDueCount { get; set; }
        public long SharedByMeExpiredCancelCloseCount { get; set; }
        public bool IsVersioningButtonVisible
        {
            get
            {
                return EnableEditButton;
            }
        }
        public bool IsViewButtonVisible
        {
            get
            {
                return true;
            }
        }
        public bool IsDeleteButtonVisible
        {
            get
            {
                return EnableDeleteButton;
            }
        }
        public string TemplateName { get; set; }
    }
}

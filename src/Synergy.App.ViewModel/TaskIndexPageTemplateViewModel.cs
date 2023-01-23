﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class TaskIndexPageTemplateViewModel : TaskIndexPageTemplate
    {
        public string RowData { get; set; }
        public IList<TaskIndexPageColumnViewModel> SelectedTableRows { get; set; }
        public IList<ServiceIndexPageColumnViewModel> SelectedRows { get; set; }
        public PageViewModel Page { get; set; }
        public string PageId { get; set; }
        public string TableMetadataId { get; set; }
        public string UdfTableMetadataId { get; set; }

        public long AssignedToMeDraftCount { get; set; }
        public long AssignedToMeCompleteCount { get; set; }
        public long AssignedToMeInProgreessOverDueCount { get; set; }
        public long AssignedToMeRejectCancelCloseCount { get; set; }

        //  public long AssignedToMeInProgreessCount { get; set; }
        //public long AssignedToMeOverdueCount { get; set; }
        //public long AssignedToMeRejectCount { get; set; }

        //public long AssignedToMeCancelCount { get; set; }

        public long CreatedOrRequestedByMeDraftCount { get; set; }
        public long CreatedOrRequestedByMeInProgreessOverDueCount { get; set; }
        public long CreatedOrRequestedByMeCompleteCount { get; set; }
        public long CreatedOrRequestedByMeRejectCancelCloseCount { get; set; }

        //public long CreatedOrRequestedByMeInProgreessCount { get; set; }
        //public long CreatedOrRequestedByMeOverdueCount { get; set; }
        //public long CreatedByMeRejectCount { get; set; }

        //public long CreatedByMeCancelCount { get; set; }

        //public long RequestedByMeDraftCount { get; set; }
        //public long RequestedByMeCompleteCount { get; set; }

        //public long RequestedByMeInProgreessOverDueCount { get; set; }
        //public long RequestedByMeRejectCancelCloseCount { get; set; }


        //public long RequestedByMeOverdueCount { get; set; }
        //public long RequestedByMeRejectCount { get; set; }

        //public long RequestedByMeCancelCount { get; set; }
        //public long RequestedByMeInProgreessCount { get; set; }

        public long SharedWithMeDraftCount { get; set; }
        public long SharedWithMeCompleteCount { get; set; }
        public long SharedWithMeInProgressOverDueCount { get; set; }
        public long SharedWithMeRejectCancelCloseCount { get; set; }

        //public long SharedWithMeOverdueCount { get; set; }
        //public long SharedWithMeRejectCount { get; set; }
        //public long SharedWithMeInProgreessCount { get; set; }
        //public long SharedWithMeCancelCount { get; set; }


        public long SharedByMeDraftCount { get; set; }
        public long SharedByMeCompleteCount { get; set; }

        public long SharedByMeInProgreessOverDueCount { get; set; }
        public long SharedByMeRejectCancelCloseCount { get; set; }
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
        public bool Select { get; set; }
    }
}
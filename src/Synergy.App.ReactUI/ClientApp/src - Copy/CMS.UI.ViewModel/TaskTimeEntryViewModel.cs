using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class TaskTimeEntryViewModel : NtsTaskTimeEntry
    {
        public string NtsServiceId { get; set; }
        public string TimeEntryId { get; set; }
        public string ProjectName { get; set; }
        public string TaskNo { get; set; }
        public string TaskName { get; set; }
        public string WorkComment { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string DurationText
        {
            get
            {
                return ((TimeSpan?)Duration).ToTimeSpanString();
            }
        }
        public string StartDateDisplay 
        {
            get { return StartDate.ToDefaultDateTimeFormat(); }
        }
        public string EndDateDisplay
        {
            get { return EndDate.ToDefaultDateTimeFormat(); }
        }
    }
}

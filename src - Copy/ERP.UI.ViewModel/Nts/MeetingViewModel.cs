using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class MeetingViewModel : ViewModelBase
    {
        public string Title { get; set; }
        public string Key { get; set; }
        public string EncryptKey { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime ScheduledDateTime { get; set; }
        public double Duration { get; set; }
        public string Password { get; set; }
        public string Pin { get; set; }
        public string Url { get; set; }
        public string MeetingName { get; set; }
        public string ParentKey { get; set; }
    } 
}

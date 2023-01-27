using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using static ERP.Utility.Constant;

namespace ERP.UI.ViewModel
{
    public class InterviewerViewModel : ViewModelBase
    {
        public long? ManpowerRequestId { get; set; }
        public string ManpowerRequestNo { get; set; }
        public string JobName { get; set; }
        public string CandidateName { get; set; }
        public long? ApplicationId { get; set; }
        public string ApplicationNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.LongDateTimeFormat)]
        public DateTime? ScheduledStartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.LongDateTimeFormat)]
        public DateTime? ScheduledEndDate { get; set; }

        public string InterviewStatus { get; set; }

        public string Url { get; set; }

    }

}



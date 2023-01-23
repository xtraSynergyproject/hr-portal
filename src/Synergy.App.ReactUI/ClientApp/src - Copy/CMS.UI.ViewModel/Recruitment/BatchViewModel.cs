﻿using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class BatchViewModel : Batch
    {
      
        public string Organization { get; set; }
        public string JobName { get; set; }
        public string BatchStatusName { get; set; }
        public string BatchStatusCode { get; set; }
        public long? NoOfApplication { get; set; }
        public long? NotShortlistByHM { get; set; }
        public long? ShortlistByHM { get; set; }
        public long? ConfirmInterview { get; set; }
        public long? Evaluated { get; set; }
        public string HiringManagerName { get; set; }
        public string HeadOfDepartmentName { get; set; }

        public string TaskId { get; set; }

    }
}

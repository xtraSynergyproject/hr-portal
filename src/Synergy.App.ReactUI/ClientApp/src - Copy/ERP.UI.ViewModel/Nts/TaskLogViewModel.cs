using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class TaskLogViewModel : TaskViewModel
    {
        public long LogTaskId { get; set; }
        public string Event { get; set; }
        public string EventBy { get; set; }
        public DateTime EventOccuredOn { get; set; }       
    }

}



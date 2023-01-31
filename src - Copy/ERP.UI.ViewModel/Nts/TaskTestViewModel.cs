using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class TaskTestViewModel
    {
        public long TaskId { get; set; }
        public string OwnerUserEmail { get; set; }
        public long OwnerUserId { get; set; }
        public string OwnerUserPassword { get; set; }

    }
     
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class AssignmentStatusViewModel : ViewModelBase
    {       
        public long AssignmentStatusId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public bool IsHRActive { get; set; }
        public bool IsPayrollActive { get; set; }
        public Nullable<long> SequenceNo { get; set; }
    }
}

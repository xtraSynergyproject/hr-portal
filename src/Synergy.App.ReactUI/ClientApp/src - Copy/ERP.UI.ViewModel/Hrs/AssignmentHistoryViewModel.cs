using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.ComponentModel;

namespace ERP.UI.ViewModel
{
    public class AssignmentHistoryViewModel  
    {
        public int? EmployeeId { get; set; }
        public GridSelectOption SelectOption { get; set; }
        public bool IsSuccess { get; set; }

    }
}

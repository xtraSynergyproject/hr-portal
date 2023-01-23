using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class EmployeeNewHireRequestViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }

        public bool IsGridVisible { get; set; }

        public int? CandidateId { get; set; }
        public int? EmployeeId { get; set; }

        public int? RequisitionId { get; set; }
        public int? EmployeeTypeId { get; set; }

        public string CurrentTabName { get; set; }
        public int? PositionId { get; set; }
        public int? HierarchyNameId { get; set; }


        public string RequestStatus { get; set; }
        public TransactionMode Mode { get; set; }
        public string Errors { get; set; }

    }
}

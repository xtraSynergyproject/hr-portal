


using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class EmployeeChangeLogViewModel : ViewModelBase
    {
        public EmployeeTransactionEnum TransactionName { get; set; }
        public string ChangedData { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ChangeStatus { get; set; }

        public long? PersonId { get; set; }
    }
}

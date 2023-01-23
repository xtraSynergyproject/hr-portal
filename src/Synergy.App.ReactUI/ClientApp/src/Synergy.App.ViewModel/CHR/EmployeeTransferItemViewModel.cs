using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class EmployeeTransferItemViewModel : NoteViewModel
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string RequestType { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string TransferItemId { get; set; }
        public string IssuedQuantity { get; set; }
        public string ItemQuantity { get; set; }
    }
}

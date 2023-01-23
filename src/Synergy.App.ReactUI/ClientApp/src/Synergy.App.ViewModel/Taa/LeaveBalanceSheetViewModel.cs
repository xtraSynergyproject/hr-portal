using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class LeaveBalanceSheetViewModel : ViewModelBase
    {
        public int Year { get; set; }
        public string UserId { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveTypeCode { get; set; }
        public double OpeningBalance { get; set; }
        public double ClosingBalance { get; set; }
        public string NtsNoteId { get; set; }
    }
}

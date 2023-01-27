

namespace ERP.UI.ViewModel
{
    public class LeaveBalanceSheetViewModel : ViewModelBase    {


        public int Year { get; set; }
        public long UserId { get; set; }
        public long LeaveTypeId { get; set; }
        public string LeaveTypeCode { get; set; }
        public double OpeningBalance { get; set; }
        public double ClosingBalance { get; set; }

    }
}

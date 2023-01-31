

namespace ERP.UI.ViewModel
{
    public class PmtDashboardViewModel
    {
        public long Id { get; set; }
      //  public long Total { get; set; }
        public long OnTime { get; set; }
        public long BehindSchedule { get; set; }
        public long Planned { get; set; }
        public long Completed { get; set; }
        public string Status { get; set; }
        public string ProjectName { get; set; }
        public decimal CompletionPercentage { get; set; }
        public long ToBeCompletedByToday { get; set; }
        public long Draft { get; set; }
        public long EmptyService { get; set; }
        public long OnTimePlanned { get { return OnTime + Planned + EmptyService; } }
        public long Total { get { return OnTime + Planned + EmptyService + BehindSchedule + Completed; } }
        public long? ServiceId { get; set; }
    }
}

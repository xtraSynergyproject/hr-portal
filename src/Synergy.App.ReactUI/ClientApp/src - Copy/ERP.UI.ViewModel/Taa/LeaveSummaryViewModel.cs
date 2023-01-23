

using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class LeaveSummaryViewModel : ViewModelBase    {

       
        public string Month { get; set; }
        public string Type { get; set; }
        [Display(Name = "Number Of Leaves Taken")]
        public long Count { get; set; }

        [Display(Name = "Month")]
        public int MonthVal { get; set; }
        public int Year { get; set; }

        public long UserId { get; set; }
    }
}

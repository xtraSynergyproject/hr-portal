
using ERP.Utility;

namespace ERP.Data.GraphModel
{

    public partial class PAY_Calendar : NodeBase
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsSundayWeekEnd { get; set; }
        public bool IsMondayWeekEnd { get; set; }
        public bool IsTuesdayWeekEnd { get; set; }
        public bool IsWednesdayWeekEnd { get; set; }
        public bool IsThursdayWeekEnd { get; set; }
        public bool IsFridayWeekEnd { get; set; }
        public bool IsSaturdayWeekEnd { get; set; }
    }

}


using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{

    public partial class PAY_CalendarHoliday : NodeBase
    {
        public string HolidayName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
    public class R_CalendarHoliday_Calendar : RelationshipBase
    {

    }
}

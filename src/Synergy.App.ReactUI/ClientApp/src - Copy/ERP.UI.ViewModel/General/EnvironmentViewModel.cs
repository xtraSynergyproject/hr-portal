using System;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class EnvironmentViewModel
    {
        public DateTime CurrentDate
        {
            get { return DateTime.Now.ApplicationNow().Date; }
        }
        public DateTime CurrentDateTime
        {
            get { return DateTime.Now.ApplicationNow(); }
        }
        public int CurrentDay
        {
            get { return DateTime.Now.ApplicationNow().Day; }
        }
        public DayOfWeek CurrentWeekName
        {
            get { return DateTime.Now.ApplicationNow().DayOfWeek; }
        }
        public int CurrentYear
        {
            get { return DateTime.Now.ApplicationNow().Year; }
        }
        public int CurrentMonth
        {
            get { return DateTime.Now.ApplicationNow().Month; }
        }
    }
}

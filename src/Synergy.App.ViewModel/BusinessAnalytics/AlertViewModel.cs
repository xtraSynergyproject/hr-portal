using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class AlertViewModel : NoteTemplateViewModel
    {
        public string queryTableId { get; set; }
        public string queryColumns { get; set; }
        public string[] queryColumnArray { get; set; }
        public string groupbyColumns { get; set; }
        public string query { get; set; }
        public string conditionFunction { get; set; }
        public string conditionType { get; set; }
        public string conditionValue { get; set; }
        public string frequency { get; set; }
        public string limit { get; set; }
        public string evaluateTime { get; set; }
        public string summary { get; set; }
        public string colorCode { get; set; }
        public string cubeJsFilter { get; set; }
        public string columnReferenceId { get; set; }
        public string jsonData { get; set; }
        public string alertid { get; set; }
        public DateTime alert_date { get; set; }
        public string alert_dateDisplay
        {
            get
            {
                var d = Humanizer.DateHumanizeExtensions.Humanize(alert_date);
                return d;
            }
        }
        public string timeDimensionId { get; set; }
        public string timeDimensionDuration { get; set; }
        public string timeDimensionFilter { get; set; }
        public string granularity { get; set; }
        public string chartTypeId { get; set; }
        public string userRole { get; set; }
        public bool isReporting { get; set; }
        public string sourceIds { get; set; }
        public string alert_number { get; set; }
        public bool isRead { get; set; }
        public bool isVisible { get; set; }
        public string groupFilters { get; set; }

    }
    public class BuilderFilterViewModel
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }

    }
}

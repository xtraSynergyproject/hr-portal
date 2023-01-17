using System;
using ERP.Utility;
using Kendo.Mvc.UI;

namespace ERP.UI.ViewModel
{
    public class NtsGanttViewModel : IGanttTask
    {
        public long Id { get; set; }
        public string No { get; set; }
        public string ParentID { get; set; }
        public decimal PercentComplete { get; set; }
        public bool Summary { get; set; }
        public bool Expanded { get; set; }
        public int OrderId { get; set; }

        public string Title { get; set; }
        public string TitleLimited { get { return Title.LimitTo(120); } }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }

        public string NtsId { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public long UserId { get; set; } 

        public int? RecurrId { get; set; }
        public DateTime? DueDate { get; set; }
         
        public ModuleEnum? ModuleName { get; set; }
        public string Assignee { get; set; }

        public string[] Teams { get; set; }
        public string Team { get; set; }
        public string TeamMember
        {
            get
            {
                var str = "";
                if (Team.IsNotNullAndNotEmpty())                
                str = string.Concat(Team, "-",string.Join(",", Teams));
                return str;
            }
        }
    }
}

using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class TeamWorkloadViewModel
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskType { get; set; }
        public string TaskStatus { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateStr { get; set; }
        public DateTime DueDate { get; set; }
        public long? SubTaskId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long? PhotoId { get; set; }
        public bool Checkbox { get; set; }
        public bool IsAssignee { get; set; }
        public ICollection<long> Users { get; set; }
        public ICollection<string> UserNames { get; set; }

        public NtsActionEnum TemplateAction { get; set; }

        public string StatusStyle
        {
            get
            {
                switch (TemplateAction)
                {
                    case NtsActionEnum.Draft:
                        return "Color:#5bc0de";
                    case NtsActionEnum.Submit:
                        return "Color:#f0ad4e";
                    case NtsActionEnum.Complete:
                        return "Color:#5cb85c";
                    case NtsActionEnum.Cancel:
                        return "Color:#999";
                    case NtsActionEnum.Overdue:
                        return "Color:#d9534f";
                    default:
                        return "Color:#999";
                }
            }
        }


    }
}

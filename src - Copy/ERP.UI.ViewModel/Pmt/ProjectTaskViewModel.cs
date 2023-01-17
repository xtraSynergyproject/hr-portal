using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class ProjectTaskViewModel
    {
        public long Id { get; set; }
        public long? SubTaskId { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime Date { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Checkbox { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string StatusCode { get; set; }
        public long? PhotoId { get; set; }
        public ICollection<long> Users { get; set; }
        public ICollection<string> UserNames { get; set; }
        public long TaskCount { get; set; }
        public IList<ProjectTaskViewModel> items { get; set; }

        public long? ParentTaskId { get; set; }
        public bool HasChilderen { get; set; }

        public long? ProjectId { get; set; }

    }
}

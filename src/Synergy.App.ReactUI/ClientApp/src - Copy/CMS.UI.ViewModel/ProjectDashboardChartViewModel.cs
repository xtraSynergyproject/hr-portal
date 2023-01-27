
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CMS.Common;

namespace CMS.UI.ViewModel
{
    public class ProjectDashboardChartViewModel
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public long Value { get; set; }
        public long ValueB { get; set; }
        public long ValueO { get; set; }
        public long ValueC { get; set; }
        public string Type1 { get; set; }
        public long Count1 { get; set; }
        public long Count2 { get; set; }

        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? ProjectStartDate { get; set; }
        public string AssigneeId { get; set; }
        public string TaskCreatedDate { get; set; }

        public int Days { get; set; }
        public int ActualSLA { get; set; }

        public string GroupName { get; set; }
        public long SLAValue { get; set; }
        public string OwnerUserName { get; set; }
        public string StatusName { get; set; }
    }
}

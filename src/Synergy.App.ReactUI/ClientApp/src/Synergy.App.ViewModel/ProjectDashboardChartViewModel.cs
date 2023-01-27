
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
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
        public string TotalNoPropertyGarbageCollected { get; set; }
        public string TotalNoProperty{ get; set; }
        public string TotalNoPropertyGarbageNotCollected{ get; set; }


        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? ProjectStartDate { get; set; }
        public string AssigneeId { get; set; }
        public string TaskCreatedDate { get; set; }

        public int Days { get; set; }
        public int ActualSLA { get; set; }

        public string GroupName { get; set; }
        public long Expected { get; set; }
        public long Actual { get; set; }
        public long SLAValue { get; set; }
        public string OwnerUserName { get; set; }
        public string StatusName { get; set; }
        public List<string> ItemValueLabel { get; set; }
        public List<long> ItemValueSeries { get; set; }
        public List<ProjectDashboardChartViewModel> TimeLogSeries { get; set; }
        public List<int> LineChartValueSeries { get; set; }
        public List<int> LineChartValueSeries1 { get; set; }
        public List<string> ItemStatusColor { get; set; }
        public string StatusColor { get; set; }
        public string TemplateCode { get; set; }
        public string XaxisTitle { get; set; }
        
        public List<StackBarChartViewModel> stackBarValues { get; set; }
        public List<StackBarChartViewModel> StackBarItemValueSeries { get; set; }
        public List<string> StackBarCategories { get; set; }
        public List<BarChartViewModel> BarItemValueSeries { get; set; }
        public List<string> Colors { get; set; }
    }

    public class HeatMapViewModel
    {
        public string name { get; set; }
        public List<HeatMapDataViewModel> data { get; set; }
    }

    public class HeatMapDataViewModel
    {
        public string x { get; set; }
        public int y { get; set; }
    }

    public class StackBarChartViewModel
    {
        public string name { get; set; }
        public List<long> data { get; set; }
    }
    public class BarChartViewModel
    {
        public List<long> data { get; set; }
    }
}

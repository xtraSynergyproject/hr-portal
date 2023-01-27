using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class EGovDashboardViewModel
    {
        public IList<EgovDashboardViewModel> PaymentDueList { get; set; }
        public IList<EgovDashboardViewModel> ServiceList { get; set; }
        public string TemplateCodes { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectDescriptionText
        {
            get
            {
                return ProjectDescription.LimitSize(120);
            }
        }
        public string UserId { get; set; }
        public string Id { get; set; }
        public ProjectPropsalResponseEnum? ResponseType { get; set; }
        public long LikesCount { get; set; }
        public long DislikesCount { get; set; }
        public string ServiceId { get; set; }
        public long CommentsCount { get; set; }
        public string BorderCSS { get; set; }
        public List<string> ItemValueLabel { get; set; }
        public List<long> ItemValueSeries { get; set; }
        public string ProjectCategory { get; set; }
        public string ProjectCategoryName { get; set; }
        public string ProjectStatusCode { get; set; }
        public string ProjectStatus { get; set; }
        public long ProjectsCount { get; set; }
        public string LocationName { get; set; }
        public string StatusColor { get; set; }
        public List<string> ItemStatusColor { get; set; }
        public string ServiceNo { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Comment { get; set; }
        public string DisplayRequestedDate { get; set; }
        public string WardName { get; set; }
        public string WardId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Message { get; set; }
        public string WorkflowStatus { get; set; }
        public StatusEnum TimeLineStatus { get; set; }




    }

    public class EgovDashboardViewModel
    {
        public string ServiceName { get; set; }
        public long DueAmount { get; set; }
        public string Status { get; set; }
        public string DueDate { get; set; }
        public string BillDate { get; set; }
        public int? Count { get; set; }
        public int? InProgressCount { get; set; }
        public int? CompletedCount { get; set; }
        public int? RejectedCount { get; set; }
        public string TemplateCode { get; set; }
    }
}

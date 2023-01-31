using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class EGovProjectViewModel
    {
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public string ServiceNo { get; set; }
        public long SNo { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectCategory { get; set; }
        public string ProjectCategoryName { get; set; }
        public string ProjectWard { get; set; }
        public string ProjectWardName { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Status { get; set; }
        public string ServiceStatus { get; set; }
        public long Like { get; set; }
        public bool IsProposedByCitizen { get; set; }
    }
}

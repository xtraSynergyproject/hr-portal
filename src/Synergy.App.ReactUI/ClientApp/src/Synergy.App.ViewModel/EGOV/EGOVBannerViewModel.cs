using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class EGOVBannerViewModel
    {
        public string BannerImageId { get; set; }
        public string BannerContent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long? SequenceNo { get; set; }
        public string UrlLink { get; set; }
        public string FileId { get; set; }
        public string BannerType { get; set; }
        public string WardId { get; set; }
        public string WardName { get; set; }
        public string Name { get; set; }
        public string FHName { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        

    }
    public class EGOVUserDetailsViewModel
    {
        public string UserId { get; set; }
        public string WardId { get; set; }
        public string WardName { get; set; }
    }
}

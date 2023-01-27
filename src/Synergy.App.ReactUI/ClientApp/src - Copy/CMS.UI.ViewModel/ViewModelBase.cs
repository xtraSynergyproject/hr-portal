using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class ViewModelBase
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public long? SequenceOrder { get; set; }
        public string CompanyId { get; set; }
        public DataActionEnum DataAction { get; set; }
        public StatusEnum Status { get; set; }
        public long VersionNo { get; set; }
    }
}

using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class FormTemplateViewModel : FormTemplate
    {
        public string JsonCopy { get; set; }
        public string Json { get; set; }
        public string DataJson { get; set; }
        public string LocalizedUI { get; set; }
        public string TableMetadataId { get; set; }
        public PageViewModel Page { get; set; }
        public string PageId { get; set; }
        public string RecordId { get; set; }
        public string ParentTemplateId { get; set; }
        public string PortalName { get; set; }
        public Dictionary<string, string> Prms { get; set; }
        public FormIndexPageTemplateViewModel IndexPageTemplateDetails { get; set; }
        public Dictionary<string, string> Udfs { get; set; }
        public Dictionary<string, bool> ReadoOnlyUdfs { get; set; }
        public Dictionary<string, bool> HiddenUdfs { get; set; }
        public string CustomUrl { get; set; }
        public string ReturnUrl { get; set; }
        public LayoutModeEnum? LayoutMode { get; set; }
        public string PopupCallbackMethod { get; set; }
        public string ChartItems { get; set; }
        public List<ColumnMetadataViewModel> ColumnList { get; set; }
        public string TemplateCode { get; set; }
        public bool IsChildTable { get; set; }

        public string UserId { get; set; }
    }
}

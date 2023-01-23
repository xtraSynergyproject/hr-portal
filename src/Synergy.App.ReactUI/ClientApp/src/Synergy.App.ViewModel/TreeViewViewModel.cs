﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.IO;


namespace Synergy.App.ViewModel
{
    public class TreeViewViewModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public dynamic a_attr { get; set; }
        public dynamic state { get; set; }
        public string icon { get; set; }

        public string parent { get; set; }
        public bool children { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string IconCss { get; set; }
        public string IconTitle { get; set; }
        public string ParentId { get; set; }
        public bool hasChildren { get; set; }
        public bool expanded { get; set; }
        public string Type { get; set; }
        public string PortalId { get; set; }
        public long? RootId { get; set; }
        public int ItemLevel { get; set; }
        public bool Checked { get; set; }
        public string Url { get; set; }
        public string UserRoleId { get; set; }
        public string ProjectId { get; set; }
        public string PerformanceId { get; set; }
        public string StageName { get; set; }
        public string PageName { get; set; }
        public string StageId { get; set; }
        public string BatchId { get; set; }
        public string[] StatusCode { get; set; }
        public string StatusCodeStr
        {
            get
            {
                var str = "";
                if (StatusCode != null)
                {
                    str = string.Join(",", StatusCode);
                }
                return str;
            }
        }

        public string BannerId { get; set; }
        public string BannerStyle { get; set; }
        public string Directory { get; set; }
        public TemplateTypeEnum? TemplateType { get; set; }
        public string TemplateTypeText
        {
            get
            {
                return Convert.ToString(TemplateType);
            }
        }
        public DataColumnTypeEnum? FieldDataType { get; set; }
        public string TemplateCode { get; set; }
        public long Count { get; set; }
        public string Namespace { get; set; }
        public bool? HideCheckbox { get; set; }

        public string key { get; set; }
        public string title { get; set; }
        public bool lazy { get; set; }
        public long? SequenceOrder { get; set; }
        public string SequenceOrderText { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MapArea { get; set; }
    }
}

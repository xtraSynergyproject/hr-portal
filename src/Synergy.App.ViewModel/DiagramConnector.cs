using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    [HtmlTargetElement("e-diagram-connector", ParentTag = "e-diagram-connectors")]
    [JsonObject(MemberSerialization.OptIn)]
    [RestrictChildren("e-content-template", new[] { "e-connector-connectorfixeduserhandles", "e-connector-connectorannotations", "e-connector-shape", "e-connector-shape", "e-connector-shape", "e-connector-shape", "e-connector-margin", "e-connector-tooltip", "e-connector-sourcepoint", "e-connector-targetpoint", "e-connector-style", "e-connector-sourcedecorator", "e-connector-targetdecorator" })]
    public class DiagramConnector
    {

        [DefaultValue(null)]
        [HtmlAttributeName("sourceID")]
        [JsonProperty("sourceID")]
        public string SourceID { get; set; }
        [DefaultValue(0)]
        [HtmlAttributeName("sourcePadding")]
        [JsonProperty("sourcePadding")]
        public double SourcePadding { get; set; }

        [DefaultValue("")]
        [HtmlAttributeName("sourcePortID")]
        [JsonProperty("sourcePortID")]
        public string SourcePortID { get; set; }

        [HtmlAttributeName("symbolInfo")]
        [JsonProperty("symbolInfo")]
        public object SymbolInfo { get; set; }

        [DefaultValue(0)]
        [HtmlAttributeName("targetPadding")]
        [JsonProperty("targetPadding")]
        public double TargetPadding { get; set; }

        [DefaultValue("")]
        [HtmlAttributeName("targetPortID")]
        [JsonProperty("targetPortID")]
        public string TargetPortID { get; set; }

        [DefaultValue(true)]
        [HtmlAttributeName("visible")]
        [JsonProperty("visible")]
        public bool Visible { get; set; }
        [DefaultValue(null)]
        [HtmlAttributeName("targetID")]
        [JsonProperty("targetID")]
        public string TargetID { get; set; }
        [DefaultValue(null)]
        [HtmlAttributeName("wrapper")]
        [JsonProperty("wrapper")]
        public object Wrapper { get; set; }
        [DefaultValue(null)]
        [HtmlAttributeName("shape")]
        [JsonProperty("shape")]
        public object Shape { get; set; }
        [HtmlAttributeName("previewSize")]
        [JsonProperty("previewSize")]
        public object PreviewSize { get; set; }
        [HtmlAttributeName("addInfo")]
        [JsonProperty("addInfo")]
        public object AddInfo { get; set; }

        [DefaultValue(10)]
        [HtmlAttributeName("bridgeSpace")]
        [JsonProperty("bridgeSpace")]
        public double BridgeSpace { get; set; }
        [DefaultValue(0)]
        [HtmlAttributeName("connectionPadding")]
        [JsonProperty("connectionPadding")]
        public double ConnectionPadding { get; set; }

        [DefaultValue(null)]
        [HtmlAttributeName("segments")]
        [JsonProperty("segments")]
        public object Segments { get; set; }
        [DefaultValue(0)]
        [HtmlAttributeName("cornerRadius")]
        [JsonProperty("cornerRadius")]
        public double CornerRadius { get; set; }
        [DefaultValue(false)]
        [HtmlAttributeName("excludeFromLayout")]
        [JsonProperty("excludeFromLayout")]
        public bool ExcludeFromLayout { get; set; }

        [DefaultValue(10)]
        [HtmlAttributeName("hitPadding")]
        [JsonProperty("hitPadding")]
        public double HitPadding { get; set; }

        [HtmlAttributeName("dragSize")]
        [JsonProperty("dragSize")]
        public object DragSize { get; set; }
        [DefaultValue(-1)]
        [HtmlAttributeName("zIndex")]
        [JsonProperty("zIndex")]
        public double ZIndex { get; set; }
        protected bool IsChild { get; }
    }
}

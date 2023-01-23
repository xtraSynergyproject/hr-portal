using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using Syncfusion.EJ2.Diagrams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class SwimlaneViewModel
    {
        public List<SwimlaneNodeViewModel> nodes { get; set; }
        public List<SwimlaneConnectorViewModel> connectors { get; set; }
    }

    public class SwimlaneNodeViewModel
    {
        public string id { get; set; }
        public SwimlaneShape shape { get; set; }
        public double offsetX { get; set; }
        public double offsetY { get; set; }
        public double height { get; set; }
        public double width { get; set; }
    }

    public class SwimlaneConnectorViewModel
    {
        public string id { get; set; }
        public string sourceID { get; set; }
        public string targetID { get; set; }
    }

    public class SwimlaneShape
    {
        public string type { get; set; }
        public string orientation { get; set; }
        public SwimlaneHeader header { get; set; }
        public List<SwimlaneLaneViewModel> lanes { get; set; }
        public List<SwimlanePhaseViewModel> phases { get; set; }
        public double phaseSize { get; set; }
    }
    public class SwimlanePhaseViewModel
    {
        public string id { get; set; }
        public double offset { get; set; }
        public SwimlaneHeader header { get; set; }
        public List<SwimlaneChildren> children { get; set; }

    }

    public class SwimlaneLaneViewModel
    {
        public string id { get; set; }
        public SwimlaneHeader header { get; set; }
        public double height { get; set; }
        public List<SwimlaneChildren> children { get; set; }
    }

    public class SwimlaneAnnotation
    {
        public string content { get; set; }
        public SwimlaneStyle style { get; set; }
        public string shape { get; set; }
    }

  
    
    public class SwimlaneHeader
    {
        public SwimlaneAnnotation annotation { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public SwimlaneStyle style { get; set; }
    }

    public class SwimlaneStyle
    {
        public double fontSize { get; set; }
        public string fill { get; set; }
    }
    public class SwimlaneChildren
    {
        public string id { get; set; }
        public List<SwimlaneAnnotation> annotations { get; set; }
        public SwimlaneMargin margin { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public dynamic ports { get; set; }
        public SwimlaneChildrenShape shape { get; set; }
    }

    public class SwimlaneChildrenShape {
        public string type { get; set; }
        public string content { get; set; }
    }

    public class SwimlanePorts
    {
        public string id { get; set; }
        public SwimlaneOffset offset { get; set; }
    }

    public class SwimlaneOffset
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class SwimlaneMargin
    {
        public double top { get; set; }
        public double left { get; set; }
        public double right { get; set; }
        public double bottom { get; set; }
    }


    public class SwimLaneModel
    {
        [DefaultValue(null)]
        [HtmlAttributeName("type")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [DefaultValue(null)]
        [HtmlAttributeName("lanes")]
        [JsonProperty("lanes")]
        public List<Lane> Lanes { get; set; }

        [DefaultValue(null)]
        [HtmlAttributeName("orientation")]
        [JsonProperty("orientation")]
        public string Orientation { get; set; }

        [DefaultValue(null)]
        [HtmlAttributeName("isLane")]
        [JsonProperty("isLane")]
        public bool IsLane { get; set; }

        [DefaultValue(null)]
        [HtmlAttributeName("isPhase")]
        [JsonProperty("isPhase")]
        public bool IsPhase { get; set; }
    }

    public class SwimLane
    {
        [DefaultValue(null)]
        [HtmlAttributeName("orientation")]
        [JsonProperty("orientation")]
        public string Orientation
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("type")]
        [JsonProperty("type")]
        public string Type
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("header")]
        [JsonProperty("header")]
        public Header Header
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("lanes")]
        [JsonProperty("lanes")]
        public List<Lane> Lanes
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("phases")]
        [JsonProperty("phases")]
        public List<Phase> Phases
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("phaseSize")]
        [JsonProperty("phaseSize")]
        public double PhaseSize
        {
            get;
            set;
        }
    }

    public class Header
    {
        [DefaultValue(null)]
        [HtmlAttributeName("annotation")]
        [JsonProperty("annotation")]
        public object Annotation
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("height")]
        [JsonProperty("height")]
        public double Height
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("width")]
        [JsonProperty("width")]
        public double Width
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("orientation")]
        [JsonProperty("orientation")]
        public string Orientation
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("style")]
        [JsonProperty("style")]
        public DiagramTextStyle Style
        {
            get;
            set;
        }
    }

    public class Lane
    {
        [DefaultValue(null)]
        [HtmlAttributeName("id")]
        [JsonProperty("id")]
        public string Id
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("header")]
        [JsonProperty("header")]
        public Header Header
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("orientation")]
        [JsonProperty("orientation")]
        public string Orientation
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("style")]
        [JsonProperty("style")]
        public DiagramTextStyle Style
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("height")]
        [JsonProperty("height")]
        public double Height
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("width")]
        [JsonProperty("width")]
        public double Width
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("children")]
        [JsonProperty("children")]
        public List<DiagramNode> Children
        {
            get;
            set;
        }
    }

    public class Phase
    {
        [DefaultValue(null)]
        [HtmlAttributeName("orientation")]
        [JsonProperty("orientation")]
        public string Orientation
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("id")]
        [JsonProperty("id")]
        public string Id
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("offset")]
        [JsonProperty("offset")]
        public double Offset
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("style")]
        [JsonProperty("style")]
        public DiagramTextStyle Style
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("header")]
        [JsonProperty("header")]
        public Header Header
        {
            get;
            set;
        }
    }

    public class MenuItems
    {
        [DefaultValue(null)]
        [HtmlAttributeName("text")]
        [JsonProperty("text")]
        public string Text
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("id")]
        [JsonProperty("id")]
        public string Id
        {
            get;
            set;
        }
        [DefaultValue(null)]
        [HtmlAttributeName("target")]
        [JsonProperty("target")]
        public string Target
        {
            get;
            set;
        }
    }

}

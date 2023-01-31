using Synergy.App.Common;
using Synergy.App.DataModel;
using Newtonsoft.Json;
//using Syncfusion.EJ2.Diagrams;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synergy.App.ViewModel
{
    public class BusinessRuleNodeViewModel : BusinessRuleNode
    {
        //  public List<DiagramNode> Nodes { get; set; }
        //    public List<DiagramConnector> Connectors { get; set; }
        public string TemplateId { get; set; }
        public string ParentNodeId { get; set; }
        public string SourceId { get; set; }
        public bool Isleft { get; set; }
    }


}

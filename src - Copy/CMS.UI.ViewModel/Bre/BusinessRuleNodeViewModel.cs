using CMS.Common;
using CMS.Data.Model;
using Newtonsoft.Json;
using Syncfusion.EJ2.Diagrams;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.UI.ViewModel
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

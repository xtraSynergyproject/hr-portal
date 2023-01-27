using ERP.Utility;
using Syncfusion.EJ2.Diagrams;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class DiagramViewModel
    {
        public List<Syncfusion.EJ2.Diagrams.DiagramNode> Nodes { get; set; }
        public List<DiagramConnector> Connectors { get; set; }
    }
}

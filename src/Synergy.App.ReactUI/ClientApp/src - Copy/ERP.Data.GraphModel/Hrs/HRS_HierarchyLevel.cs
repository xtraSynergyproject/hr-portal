using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
    public partial class HRS_HierarchyLevel : NodeBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? SequenceNo { get; set; }
    }
    public class R_HierarchyLevel_Hierarchy: RelationshipDatedBase
    {
        
    }
   
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
    public class SAL_PropertyPrice: NodeBase
    {
        public string Area { get; set; }

        public long Price { get; set; }

        public string FloorPlan { get; set; }
       
    }
    public class R_PropertyPrice_UnitType : RelationshipBase
    {

    }
    public class R_PropertyPrice_Project : RelationshipBase
    {

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class PositionChartIndexViewModel 
    {
        public virtual long HierarchyId { get; set; }
        public virtual long HierarchyRootNodeId { get; set; }
        public virtual long AllowedRootNodeId { get; set; }
        public virtual bool CanAddRootNode { get; set; }
        public virtual long AllowedRootNodeLevel { get; set; }
       
        public virtual string AsOnDate { get; set; }
        public OrganizationCategoryEnum OrganizationType { get; set; }
        [Display(Name = "Department Name")]
        public long Organization { get; set; }
        public DateTime AsOfDate { get; set; }


        public int Width { get; set; }
        public int Height { get; set; }
        public string Content { get; set; }

        public string RequestSource { get; set; }

        public Mode ChartMode { get; set; }

    }
}

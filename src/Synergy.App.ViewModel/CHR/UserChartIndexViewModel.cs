using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Synergy.App.ViewModel
{
    public class UserChartIndexViewModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string PermissionType { get; set; }



        public int DirectChildCount { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Content { get; set; }

        public virtual string HierarchyId { get; set; }
        public virtual string HierarchyRootNodeId { get; set; }
        public virtual string AllowedRootNodeId { get; set; }
        public virtual bool CanAddRootNode { get; set; }
        public virtual long AllowedRootNodeLevel { get; set; }

        public virtual string AsOnDate { get; set; }
        public string RequestSource { get; set; }
        public string Permission { get; set; }

    }
}

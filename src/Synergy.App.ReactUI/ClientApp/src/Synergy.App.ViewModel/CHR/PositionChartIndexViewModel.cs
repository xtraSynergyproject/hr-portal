﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Synergy.App.Common;
using System.ComponentModel.DataAnnotations;


namespace Synergy.App.ViewModel
{
    public class PositionChartIndexViewModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }



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
        public OrganizationCategoryEnum OrganizationType { get; set; }
        [Display(Name = "Department Name")]
        public string Organization { get; set; }
        public DateTime AsOfDate { get; set; }
  
        public string RequestSource { get; set; }


    }
}
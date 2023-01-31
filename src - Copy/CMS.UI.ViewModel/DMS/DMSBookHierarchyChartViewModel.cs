using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using CMS.Common;

namespace CMS.UI.ViewModel
{
    public class DMSBookHierarchyChartViewModel:FileExplorerViewModel
    {
        public string Id { get; set; }
        public string DocumentId { get; set; }
        public string PhotoId { get; set; }
        public string NoteId { get; set; }
        public string NodeType { get; set; }
        //public double Count { get; set; }
        public string UserHierarchyNoteId { get; set; }
        public string HierarchyId { get; set; }
        public string HierarchyRootNodeId { get; set; }
        public string UserHierarchyId { get; set; }
        public long SequenceOrder { get; set; }

        public string BookImage { get; set; }

        //public string ParentId { get; set; }
        public string BookId { get; set; }
        public string PageId { get; set; }
       
        [Display(Name = "Department Name")]
        public  string UserName { get; set; }
        [Display(Name = "Department Type")]
        public string OrganizationTypeId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationTypeName { get; set; }

        public string CostCenterCode { get; set; }
        public string CostCenter { get; set; }

        public string CCHolderPositionId { get; set; }

        public string CCHolderTitle { get; set; }

        public string CCHolderName { get; set; }
        public string LocationName { get; set; }

        public long Level { get; set; }
        public string Name { get; set; }

        public bool HasChild { get; set; }
        public long DirectChildCount { get; set; }
        
        //public string CreatedDate { get; set; }

        public string ReportingLine { get; set; }

        public bool? IsExpanded { get; set; }

        public bool? Collapsed { get; set; }

        public string CssClass
        {
            get
            {
                return "org-node-1";
            }
        }

    }
}

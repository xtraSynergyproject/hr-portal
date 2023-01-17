using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Synergy.App.ViewModel
{
    public class OrganizationChartViewModel
    {
        public string Id { get; set; }

        public string NoteId { get; set; }
        public string OrgHierarchyNoteId { get; set; }
        public string HierarchyId { get; set; }

        public string OrgHierarchyId { get; set; }

        public string ParentId { get; set; }
       
        [Display(Name = "Department Name")]
        public  string OrganizationName { get; set; }
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


        public bool HasChild { get; set; }
        public long DirectChildCount { get; set; }


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

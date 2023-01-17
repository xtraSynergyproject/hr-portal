using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
namespace ERP.UI.ViewModel
{
    public class OrganizationSearchViewModel : DatedSearchViewModelBase
    {
        public long OrgId;
        [Display(Name = "Department Name")]
        public string Name { get; set; }
        [Display(Name = "Department Type")]
        public string OrganizationTypeName { get; set; }
        [Display(Name = "Cost Center Name")]
        public string CostCenterName { get; set; }

        public GridSelectOption SelectOption { get; set; }
        public SearchType SearchType { get; set; }
        public long HierarchyId { get; set; }
        public DateTime? AsOnDate { get; set; }

    }
}

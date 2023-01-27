using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
namespace ERP.UI.ViewModel
{
    public class OrganizationSearchParamViewModel
    {
         [Display(Name = "Organization Name")]
        public string OrganizationName { get; set; }

        [Display(Name = "Organization Type")]
        public string OrganizationType { get; set; }

        [Display(Name = "Cost Center")]
        public string CostCenter { get; set; }

        public GridSelectOption SelectOption { get; set; }
        public SearchType SearchType { get; set; }
        public int HierarchyNameId { get; set; }
        public DateTime AsOnDate { get; set; }

    }
}

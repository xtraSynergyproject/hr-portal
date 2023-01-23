using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
namespace ERP.UI.ViewModel
{
    public class OrganizationChartSearchViewModel : DatedSearchViewModelBase
    {
        public long PositionId { get; set; }
        [Display(Name = "Job Name")]
        public string JobTitle { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeFullName { get; set; }

        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }
        [Display(Name = "Employee Number")]
        public string EmployeeNo { get; set; }


        [Display(Name = "Grade")]
        public string GradeName { get; set; }

        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }

        public GridSelectOption SelectOption { get; set; }
        public SearchType SearchType { get; set; }
        public long HierarchyId { get; set; }
        public DateTime AsOnDate { get; set; }

    }
}

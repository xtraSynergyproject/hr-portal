using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
namespace ERP.UI.ViewModel
{
    public class PositionSearchParamViewModel
    {

        [Display(Name = "Employee No")]
        public string EmployeeNo { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeFullName { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Position Name")]
        public string PositionName { get; set; }



        [Display(Name = "Job Description(JD) Content")]
        public string JdContent { get; set; }


        public string Grade { get; set; }

        public string Organization { get; set; }
        public int? PositionId { get; set; }
        public string ReportingToPosition { get; set; }
        public string ReportingToEmployeeNo { get; set; }
        public string ReportingToEmployeeName { get; set; }
 

        public GridSelectOption SelectOption { get; set; }
        public SearchType SearchType { get; set; }
        public int HierarchyNameId { get; set; }
        public DateTime AsOnDate { get; set; }

    }
}

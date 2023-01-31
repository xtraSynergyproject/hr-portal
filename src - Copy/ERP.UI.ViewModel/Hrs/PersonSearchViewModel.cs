using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
namespace ERP.UI.ViewModel
{
    public class PersonSearchViewModel : DatedSearchViewModelBase
    {

        [Display(Name = "Employee No")]
        public string PersonNo { get; set; }

        [Display(Name = "Employee Name")]
        public string FullName { get; set; }

        [Display(Name = "Job Title")]
        public string PositionTitle { get; set; }


        public string Grade { get; set; }
        [Display(Name = "Department Name")]
        public string Organization { get; set; }

        public GridSelectOption SelectOption { get; set; }
        public SearchType SearchType { get; set; }

    }
}

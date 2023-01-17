using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
namespace ERP.UI.ViewModel
{
    public class JobSearchParamViewModel
    {
        [Display(Name = "Job Title")]
        public string JobName { get; set; }

        public GridSelectOption SelectOption { get; set; }
        public SearchType SearchType { get; set; }

    }
}

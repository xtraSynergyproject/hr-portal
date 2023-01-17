using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class DepartmentViewModel:ViewModelBase
    {       
        public string DepartmentName { get; set; } 
        public string DepartmentLevelName { get; set; }
        public string DepartmentLevelCode { get; set; }
        public string DepartmentLevelId { get; set; }
        public string DepartmentTypeName { get; set; }
        public string DepartmentTypeCode { get; set; }
        public string DepartmentTypeId { get; set; }

    }
}

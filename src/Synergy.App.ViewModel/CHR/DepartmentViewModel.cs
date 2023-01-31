using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
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

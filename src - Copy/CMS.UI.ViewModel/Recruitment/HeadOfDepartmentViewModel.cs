using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class HeadOfDepartmentViewModel : HeadOfDepartment
    {
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string OrganizationName { get; set; }
        public string[] OrganizationId { get; set; }
    }
}

using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class RecHeadOfDepartmentViewModel 
    {       
        public string UserId { get; set; }       
        public string Name { get; set; }     
        public string DesignationId { get; set; }
        public string Email { get; set; }
        public string GAECNo { get; set; }      
        public string DepartmentId { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string OrganizationName { get; set; }
        public string[] OrganizationId { get; set; }
    }
}

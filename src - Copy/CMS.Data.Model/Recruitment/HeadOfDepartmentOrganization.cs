using CMS.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.Data.Model
{
    [Table("HeadOfDepartmentOrganization", Schema = "rec")]
    public class HeadOfDepartmentOrganization : DataModelBase
    {
        [ForeignKey("HeadOfDepartment")]
        public string HeadOfDepartmentId { get; set; }
        public HeadOfDepartment HeadOfDepartment { get; set; }

        [ForeignKey("Organization")]
        public string OrganizationId { get; set; }
    }
}

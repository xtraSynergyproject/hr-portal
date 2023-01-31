using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class JobDescriptionViewModel : BaseViewModel
    {

        public int Id { get; set; }

        public string Code { get; set; }

        [Display(Name = "JD Name")]
        public string Name { get; set; }

        public int OrganizationGroupId { get; set; }

        [Display(Name = "Organization Group")]
        public string OrganizationGroupName { get; set; }

        public int Version { get; set; }

        public string Description { get; set; }

        public byte Status { get; set; }

        public byte[] JDFile { get; set; }

    }
}

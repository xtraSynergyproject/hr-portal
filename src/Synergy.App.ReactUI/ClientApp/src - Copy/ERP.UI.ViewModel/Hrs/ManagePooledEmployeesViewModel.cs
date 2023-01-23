using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ManagePooledEmployeesViewModel : BaseViewModel
    {

        public int Id { get; set; }

        public int? PositionId { get; set; }
        public string PositionNameWithTitle { get; set; }
        public int? ManpowerOrganizationRootId { get; set; }

        public int? OrgHierarchyNameId { get; set; }


        public int? HierarchyNameId { get; set; }
        public DateTime AsOnDate { get; set; }

    }
}

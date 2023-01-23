using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class BusinessHierarchyAORViewModel
    {
        public string BusinessHierarchyId { get; set; }
        public string Id { get; set; }
        public string NtsNoteId { get; set; }
        public string ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public string ParentId { get; set; }
        public string ParentName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string BusinessPartnerId { get; set; }
        public string BusinessPartnerName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string ReferenceType { get; set; }
    }
}

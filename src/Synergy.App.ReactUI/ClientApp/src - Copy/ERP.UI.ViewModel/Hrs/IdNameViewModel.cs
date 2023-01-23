using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class IdNameViewModel: IdNameOnlyViewModel
    {
        public long? RootId { get; set; }
        public string Code { get; set; }
        public string GroupName { get; set; }
        public bool? IsSystem { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Color { get; set; }
        public long? FileId { get; set; }
        public long? WorkspaceId { get; set; }
        public long? SequenceNo { get; set; }

        public bool? showAll { get; set; }
        public string Hours { get; set; }
    }
    public class IdNameOnlyViewModel
    {
        public long? Id { get; set; }
        public string Name { get; set; } 
    }
}

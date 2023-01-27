using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class AttachmentViewModel
    {
        public string Id { get;set; }  
        public string DisplayName { get;set; } 
        public string FullName { get;set; } 
        public string ReferenceId { get;set; }
        public string SnapshotMongoId { get; set; }
        public string ContentLength { get; set; }
        public string CreatedDateDisplay { get; set; }
        public string TemplateCode { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class ManualReportViewModel : ViewModelBase
    {
        public string Topic { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public long AttachmentId { get; set; }
        public string Attachment { get; set; }
        public string PublishedDate { get; set; }
        public string VersionNo { get; set; }

    }
}

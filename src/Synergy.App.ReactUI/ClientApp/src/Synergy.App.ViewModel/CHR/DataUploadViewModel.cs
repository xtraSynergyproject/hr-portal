using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;
using Synergy.App.DataModel;

namespace Synergy.App.ViewModel
{
    public class DataUploadViewModel : ServiceTemplate
    {
       
        public string DataUploadId { get; set; }
        public string UploadTypeId { get; set; }

        public HrDataExecutionStatus ExecutionStatus { get; set; }
        public DateTime? ExecutionStartTime { get; set; }
        public DateTime? ExecutionEndTime { get; set; }
        public string ErrorCount { get; set; }
        public string Error { get; set; }
        public string AttachmentFileId { get; set; }

        public string UploadType { get; set; }
        public string ServiceId { get; set; }
        //public string DataAction { get; set; }
    }
}

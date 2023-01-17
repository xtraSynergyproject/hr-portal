using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class PeerReviewViewModel : ViewModelBase
    {
       // public long AdhocTaskServiceTemplateId { get; set; }
        public long? TaskTemplateMasterId { get; set; }
        public long PmsId { get; set; }
        public long ActiveUserId { get; set; }
        public long taskId { get; set; }
    }
}

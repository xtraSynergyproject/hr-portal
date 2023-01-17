using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class SerialNoViewModel:NoteTemplateViewModel
    {
        public string ReferenceHeaderId { get; set; }
        public string ReferenceId { get; set; }
        public string SerialNo { get; set; }
        public string Specification { get; set; }
        public string SerialNosData { get; set; }
        public bool Select { get; set; }
        public List<SerialNoViewModel> SerialNos { get; set; }
    }
}

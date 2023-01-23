using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class LeaveCanceViewModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string UserId { get; set; }
        public string CancelReason { get; set; }
        public string ParentServiceId { get; set; }
    }
}

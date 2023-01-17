using Synergy.App.ViewModel;
using System.Collections.Generic;

namespace Synergy.App.Api.Areas.IMS.Models
{
    public class SubmitSerialNo
    {
        public List<SerialNoViewModel> serialNosData { get; set; }
        public string userId { get; set; }
    }
}

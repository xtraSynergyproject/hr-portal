using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class ScheduleViewModel
    {
        public string Id { get; set; }
        public List<ScheduleInfo> Schedulers { get; set; }
        public string CallbackMethod { get; set; }
    }
}
